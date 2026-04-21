# SOLID & Good Practices — Deep Guide with Examples & Under-the-Hood Explanations

---

## 1. Cohesion — How Well a Class Sticks Together

**Cohesion** measures how strongly the methods and fields of a class belong together. High cohesion = the class has one clear purpose. Low cohesion = the class is a grab-bag of unrelated things.

There are several cohesion types ranked from best to worst:

### Functional cohesion (best) — the class does ONE thing

```csharp
// Every method concerns the same topic: VAT calculation
public class VatCalculator
{
    private readonly decimal _standardRate = 0.23m;
    private readonly decimal _reducedRate  = 0.08m;

    public decimal CalculateVat(decimal netPrice, VatRate rate)
        => netPrice * GetRateValue(rate);

    public decimal CalculateGrossPrice(decimal netPrice, VatRate rate)
        => netPrice + CalculateVat(netPrice, rate);

    public decimal CalculateNetPrice(decimal grossPrice, VatRate rate)
        => grossPrice / (1 + GetRateValue(rate));

    private decimal GetRateValue(VatRate rate) => rate switch
    {
        VatRate.Standard => _standardRate,
        VatRate.Reduced  => _reducedRate,
        _ => throw new ArgumentException("Unknown VAT rate")
    };
}
```

### Sequential cohesion — output of one method feeds next

```csharp
// Data flows through the class: Read → Parse → GenerateReport
public class CsvReportProcessor
{
    public string[]          ReadLines(string filePath)     => File.ReadAllLines(filePath);
    public List<SalesRecord> ParseRecords(string[] lines)   => lines.Skip(1).Select(SalesRecord.FromCsv).ToList();
    public Report            GenerateReport(List<SalesRecord> records) => new() { TotalSales = records.Sum(r => r.Amount) };
}
// Should be split: CsvReader, CsvParser, ReportGenerator
```

### Communicational cohesion — different operations on same data

```csharp
// All methods operate on a List<Student> — they can be called independently
public class StudentAnalyzer
{
    public double           CalculateAverageGrade(List<Student> s) => s.Average(x => x.GPA);
    public Student          FindBestStudent(List<Student> s)        => s.OrderByDescending(x => x.GPA).First();
    public List<Student>    FindFailingStudents(List<Student> s)    => s.Where(x => x.GPA < 2.0).ToList();
}
```

### Temporal cohesion (bad) — grouped only because "we run them at startup"

```csharp
public class ApplicationInitializer
{
    public void InitializeAll()
    {
        LoadConfiguration();      // concerns configuration
        OpenDatabaseConnection(); // concerns the database
        InitializeCache();        // concerns the cache
        StartBackgroundJobs();    // concerns the job scheduler
        WarmUpHttpClients();      // concerns HTTP
        SeedTestData();           // concerns test data
    }
    // The only thing these share: "we do this at startup."
}
```

### Coincidental cohesion (worst) — the `AppHelper` anti-pattern

```csharp
// Zero common denominator — just a junk drawer
public class AppHelper
{
    public string  FormatDate(DateTime date)                 { ... }
    public double  ConvertMilesToKm(double miles)            { ... }
    public void    SendEmail(string to, string body)         { ... }
    public bool    IsValidPesel(string pesel)                { ... }
    public byte[]  CompressImage(byte[] image)               { ... }
    public decimal CalculateDiscount(decimal p, int years)   { ... }
}
// Classes named Helper, Utils, Manager, Common, or Helpers.cs
// are almost always coincidental cohesion — a code smell.
```

**Cohesion and the "low cohesion" smell:** when a class has private fields that are only used by a subset of its methods, those fields + methods likely belong in a separate class.

---

## 2. Coupling — How Tightly Classes Are Linked

**Coupling** measures how much one class knows about and depends on another. Low coupling = easy to change, test, and extend. High coupling = changing one thing breaks everything else.

### Types of coupling (worst to best)

**Content coupling (worst)** — one class directly manipulates another's internal state:

```csharp
// BAD
public class Inventory
{
    public List<string> items    = new(); // public field!
    public int          itemCount = 0;
}
public class OrderProcessor
{
    public void ProcessOrder(Inventory inv, string product)
    {
        inv.items.Remove(product); // directly touching internals
        inv.itemCount--;           // what if someone forgets this?
    }
}
```

**Common coupling** — shared global/static state:

```csharp
// BAD
public static class GlobalState
{
    public static bool    IsAuthenticated = false;
    public static decimal TaxRate         = 0.23m;
}
// OrderService reads GlobalState.TaxRate, AuthService writes it
// — who set it and when? Race conditions in multithreaded code.
```

**Control coupling** — passing a flag that tells a class HOW to behave:

```csharp
// BAD — the caller must know ReportGenerator's internals to choose the format
public string GenerateReport(string data, string format)
{
    if (format == "PDF")  return GeneratePdf(data);
    if (format == "HTML") return GenerateHtml(data);
    if (format == "CSV")  return GenerateCsv(data);
    throw new ArgumentException($"Unknown format: {format}");
}
// Fix: use strategy pattern (see OCP below)
```

**Stamp coupling** — passing more data than a method actually needs:

```csharp
// BAD — EmailSender receives the whole Customer but only needs email + name
public void SendWelcomeEmail(Customer customer)
    => Send(customer.Email, $"Welcome, {customer.Name}!");

// GOOD — pass only what is needed
public void SendWelcomeEmail(string email, string name)
    => Send(email, $"Welcome, {name}!");
```

**Data coupling (best)** — clean communication through simple parameters:

```csharp
// GOOD — method takes exactly what it needs, returns exactly what it promises
public class TaxCalculator
{
    public decimal CalculateTax(decimal amount, decimal taxRate) => amount * taxRate;
}
```

### The core solution: program to interfaces, inject dependencies

```csharp
// TIGHT coupling — hard-coded dependency
public class OrderService
{
    private EmailService _emailService = new EmailService(); // baked in
}

// LOOSE coupling — depends on abstraction, receives implementation from outside
public interface INotificationService
{
    void SendNotification(string to, string subject, string body);
}

public class OrderService
{
    private readonly INotificationService _notificationService;

    public OrderService(INotificationService notificationService)
    {
        _notificationService = notificationService; // injected
    }
}

// Decision of WHAT to plug in is made OUTSIDE:
var email = new OrderService(new EmailNotificationService());
var sms   = new OrderService(new SmsNotificationService());
```

---

## 3. DRY — Don't Repeat Yourself (and Its Limits)

**DRY** = every piece of knowledge should have a single, unambiguous representation in the codebase. Duplication means a bug fix must be applied in N places.

### Extracting repeated logic

```csharp
// BEFORE — same discount logic copy-pasted twice
public decimal CalculateOnlinePrice(Product p, Customer c)
{
    decimal discount = 0;
    if (c.Orders.Count > 10) discount = 0.1m;
    if (c.Orders.Count > 50) discount = 0.2m;
    return p.Price * (1 - discount);
}
public decimal CalculateInStorePrice(Product p, Customer c)
{
    decimal discount = 0;
    if (c.Orders.Count > 10) discount = 0.1m;  // copy!
    if (c.Orders.Count > 50) discount = 0.2m;  // copy!
    return p.Price * (1 - discount) + 5.00m;   // + packaging
}

// AFTER — extracted into one method
private decimal GetLoyaltyDiscount(Customer c)
{
    if (c.Orders.Count > 50) return 0.2m;
    if (c.Orders.Count > 10) return 0.1m;
    return 0;
}
public decimal CalculateOnlinePrice(Product p, Customer c)
    => p.Price * (1 - GetLoyaltyDiscount(c));
public decimal CalculateInStorePrice(Product p, Customer c)
    => p.Price * (1 - GetLoyaltyDiscount(c)) + 5.00m;
```

### Named constants instead of magic numbers

```csharp
// BEFORE — "18" scattered through the codebase
if (user.Age >= 18)      { /* ... */ }
if (applicant.Age < 18)  { /* ... */ }

// AFTER — one definition
public static class BusinessRules
{
    public const int     MinimumLegalAge    = 18;
    public const decimal MaxDiscountPercent = 0.3m;
    public const int     MaxLoginAttempts   = 5;
}
if (user.Age >= BusinessRules.MinimumLegalAge)     { /* ... */ }
if (applicant.Age < BusinessRules.MinimumLegalAge) { /* ... */ }
```

### When NOT to DRY — the limits

**Premature DRY is dangerous.** Two things that look identical today may diverge tomorrow.

```csharp
// Age 18 for customer registration (legal requirement)
public bool CanRegister(Customer c) => c.Age >= 18;
// Age 18 for hiring (labour law)
public bool CanBeHired(Employee e)  => e.Age >= 18;
// These look the same but are governed by DIFFERENT business rules.
// Merging them into one method couples two unrelated domains.
```

**Over-DRYing creates its own mess:**

```csharp
// "DRY" that makes things WORSE — one method for two independently-evolving contexts
public void SendEmail(User user, string templateType, object? context = null)
{
    var (subject, body) = templateType switch
    {
        "welcome" => ("Welcome!", $"Hi {user.Name}, welcome!"),
        "order"   => ("Order confirmation", $"Hi {user.Name}, order #{((Order)context!).Id} accepted!"),
        _         => throw new ArgumentException("Unknown type")
    };
    _emailClient.Send(user.Email, subject, body);
    // Now every new template type modifies this method. Growing without bound.
}

// Better: keep them separate when they evolve independently
public void SendWelcomeEmail(User user) { ... }
public void SendOrderConfirmation(User user, Order order) { ... }
```

**The "Rule of Three":** see it once → write it. See it twice → note it. See it three times → then extract/refactor. The third occurrence reveals the true pattern.

```csharp
// Iteration 1: GetCustomerAsync — write it
// Iteration 2: GetProductAsync — note the similarity, don't refactor yet
// Iteration 3: GetOrderAsync — NOW you see the pattern, refactor:
public async Task<TDto> GetByIdAsync<TEntity, TDto>(int id) where TEntity : class
{
    var entity = await _dbContext.Set<TEntity>().FindAsync(id);
    if (entity == null) throw new NotFoundException($"{typeof(TEntity).Name} {id} not found");
    return _mapper.Map<TDto>(entity);
}
```

---

## 4. S — Single Responsibility Principle (SRP)

> **A class should have only one reason to change.**

"Reason to change" means "stakeholder" — the business analyst changes validation rules, the product owner changes discounts, the DBA changes persistence, the UX team changes email templates. If the same class changes for all four reasons, it violates SRP.

### The violation — one class, four stakeholders

```csharp
public class OrderService
{
    private readonly string _connectionString;
    private readonly string _smtpServer;

    // Actor 1: business analyst → Validation
    public bool ValidateOrder(Order order)
    {
        if (order.Items == null || !order.Items.Any()) return false;
        if (order.Items.Any(i => i.Quantity <= 0))    return false;
        if (order.TotalAmount <= 0)                    return false;
        if (string.IsNullOrWhiteSpace(order.CustomerEmail)) return false;
        return true;
    }

    // Actor 2: product owner → Business logic
    public decimal CalculateDiscount(Order order)
    {
        if (order.TotalAmount > 500) return order.TotalAmount * 0.1m;
        if (order.TotalAmount > 200) return order.TotalAmount * 0.05m;
        return 0;
    }

    // Actor 3: DBA → Data access
    public void SaveOrder(Order order)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        var cmd = new SqlCommand("INSERT INTO Orders ...", conn);
        cmd.ExecuteNonQuery();
    }

    // Actor 4: UX / comms team → Notifications
    public void SendConfirmationEmail(Order order)
    {
        var body = $"Thank you for your order totaling {order.TotalAmount:C}";
        // ... SMTP sending
    }
}
```

### The fix — one class per actor

```csharp
// Actor: business analyst
public interface IOrderValidator { bool Validate(Order order); }
public class OrderValidator : IOrderValidator
{
    public bool Validate(Order order)
    {
        if (order.Items == null || !order.Items.Any()) return false;
        if (order.Items.Any(i => i.Quantity <= 0))    return false;
        if (order.TotalAmount <= 0)                    return false;
        if (string.IsNullOrWhiteSpace(order.CustomerEmail)) return false;
        return true;
    }
}

// Actor: DBA / infrastructure team
public interface IOrderRepository { void Save(Order order); Order? GetById(int id); }
public class SqlOrderRepository : IOrderRepository
{
    private readonly string _connectionString;
    public SqlOrderRepository(string connectionString) => _connectionString = connectionString;
    public void Save(Order order)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        var cmd = new SqlCommand("INSERT INTO Orders (CustomerEmail, TotalAmount, Status) VALUES (@Email, @Amount, @Status)", conn);
        cmd.Parameters.AddWithValue("@Email",  order.CustomerEmail);
        cmd.Parameters.AddWithValue("@Amount", order.TotalAmount);
        cmd.Parameters.AddWithValue("@Status", order.Status);
        cmd.ExecuteNonQuery();
    }
    public Order? GetById(int id) { throw new NotImplementedException(); }
}

// Actor: UX / comms team
public interface INotificationService { void SendOrderConfirmation(Order order); }
public class EmailNotificationService : INotificationService
{
    public void SendOrderConfirmation(Order order)
    {
        var body = $"Thank you for your order totaling {order.TotalAmount:C}";
        Console.WriteLine($"Sending email to {order.CustomerEmail}: {body}");
    }
}

// Actor: product owner (orchestration = the remaining SRP of OrderService)
public class OrderService
{
    private readonly IOrderValidator        _validator;
    private readonly IOrderRepository       _repository;
    private readonly INotificationService   _notificationService;

    public OrderService(IOrderValidator v, IOrderRepository r, INotificationService n)
    {
        _validator           = v;
        _repository          = r;
        _notificationService = n;
    }

    public decimal CalculateDiscount(Order order)
    {
        if (order.TotalAmount > 500) return order.TotalAmount * 0.1m;
        if (order.TotalAmount > 200) return order.TotalAmount * 0.05m;
        return 0;
    }

    public void ProcessOrder(Order order)
    {
        if (!_validator.Validate(order))
            throw new InvalidOperationException("Order is invalid.");

        var discount        = CalculateDiscount(order);
        order.TotalAmount  -= discount;
        order.Status        = "Processed";

        _repository.Save(order);
        _notificationService.SendOrderConfirmation(order);
    }
}
```

**Why this matters mechanically:** each class is now independently testable with zero external dependencies. `OrderValidator` tests need no database mock. `OrderService` tests can inject `MockOrderValidator`, `MockOrderRepository`, and `MockNotificationService` — trivially.

```csharp
[Fact]
public void ProcessOrder_ValidOrder_SavesAndNotifies()
{
    var mockValidator    = new MockOrderValidator(returnsValid: true);
    var mockRepository   = new MockOrderRepository();
    var mockNotification = new MockNotificationService();
    var service = new OrderService(mockValidator, mockRepository, mockNotification);

    service.ProcessOrder(new Order { TotalAmount = 100, CustomerEmail = "jan@test.com",
        Items = new List<OrderItem> { new() { Quantity = 1 } } });

    Assert.True(mockRepository.WasSaveCalled);
    Assert.True(mockNotification.WasSendCalled);
}
```

---

## 5. O — Open/Closed Principle (OCP)

> **Software entities should be open for extension but closed for modification.**

When adding a new type/case/channel/format, you should add new code (new class) rather than modifying existing code. The core classes remain untouched — they're "closed" to modification but "open" for extension via new implementations.

### The violation — every new case modifies the class

```csharp
// WRONG — every new customer type requires opening this class and editing it
public class DiscountCalculator
{
    public decimal CalculateDiscount(string customerType, decimal orderAmount)
    {
        if      (customerType == "Regular") return orderAmount * 0.05m;
        else if (customerType == "Premium") return orderAmount * 0.10m;
        else if (customerType == "VIP")     return orderAmount * 0.20m;
        // In 3 months: else if ("Corporate") { return 0.15m; } — modifying existing!
        return 0m;
    }
}
```

### The fix — Strategy pattern

```csharp
// Step 1: Define the contract
public interface IDiscountStrategy
{
    decimal CalculateDiscount(decimal orderAmount);
    string GetDescription() => "Standard discount"; // default interface method — C# 8+
}

// Step 2: One class per type — each closed for modification
public class RegularDiscount : IDiscountStrategy
{ public decimal CalculateDiscount(decimal amount) => amount * 0.05m; }

public class PremiumDiscount : IDiscountStrategy
{ public decimal CalculateDiscount(decimal amount) => amount * 0.10m; }

public class VipDiscount : IDiscountStrategy
{ public decimal CalculateDiscount(decimal amount) => amount * 0.20m; }

public class NoDiscount : IDiscountStrategy
{ public decimal CalculateDiscount(decimal amount) => 0m; }

// Step 3: Calculator accepts the strategy — never needs to change
public class DiscountCalculator
{
    private readonly IDiscountStrategy _strategy;
    public DiscountCalculator(IDiscountStrategy strategy) => _strategy = strategy;
    public decimal CalculateDiscount(decimal amount)      => _strategy.CalculateDiscount(amount);
}

// Step 4: New type? Add a new class — ZERO changes to DiscountCalculator
public class CorporateDiscount : IDiscountStrategy
{ public decimal CalculateDiscount(decimal amount) => amount * 0.15m; }

// Usage
var calc    = new DiscountCalculator(new CorporateDiscount());
decimal d   = calc.CalculateDiscount(500m); // 75
```

### Lightweight OCP with Func (no interface needed)

```csharp
public class DiscountCalculator
{
    private readonly Func<decimal, decimal> _discountFn;
    public DiscountCalculator(Func<decimal, decimal> discountFn) => _discountFn = discountFn;
    public decimal CalculateDiscount(decimal amount) => _discountFn(amount);
}

var regular     = new DiscountCalculator(amount => amount * 0.05m);
var vip         = new DiscountCalculator(amount => amount * 0.20m);
var blackFriday = new DiscountCalculator(amount => amount * 0.30m); // new case — no class modified
```

### OCP applied to notification channels

```csharp
public interface INotificationSender { void Send(string message, string recipient); }

public class EmailNotificationSender : INotificationSender
{ public void Send(string msg, string to) => Console.WriteLine($"Email to {to}: {msg}"); }

public class SmsNotificationSender : INotificationSender
{ public void Send(string msg, string to) => Console.WriteLine($"SMS to {to}: {msg}"); }

// Closed for modification — never changes when new channels are added
public class NotificationService
{
    private readonly INotificationSender _sender;
    public NotificationService(INotificationSender sender) => _sender = sender;
    public void Notify(string message, string recipient)
    {
        if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Empty message");
        _sender.Send(message, recipient);
    }
}

// New channel? New class. NotificationService never touched.
public class PushNotificationSender : INotificationSender
{ public void Send(string msg, string to) => Console.WriteLine($"Push to {to}: {msg}"); }
```

**The tell-tale sign you're violating OCP:** every time you add a new case, you open an existing class and add an `else if` or `case` branch.

---

## 6. L — Liskov Substitution Principle (LSP)

> **If S is a subtype of T, objects of type T may be replaced with objects of type S without altering the correctness of the program.**

In simple terms: anywhere you use a base type, you should be able to swap in any subtype and the program still works correctly.

### The classic violation — Square inheriting Rectangle

```csharp
public class Rectangle
{
    public virtual double Width  { get; set; }
    public virtual double Height { get; set; }
    public double GetArea() => Width * Height;
}

// Mathematically, a square IS a rectangle. But in code:
public class Square : Rectangle
{
    public override double Width
    {
        get => base.Width;
        set { base.Width = value; base.Height = value; } // side effect!
    }
    public override double Height
    {
        get => base.Height;
        set { base.Height = value; base.Width = value; } // side effect!
    }
}

// Client code that expects a Rectangle
public void ResizeAndCheck(Rectangle rect)
{
    rect.Width  = 5;
    rect.Height = 10;
    Debug.Assert(rect.GetArea() == 50, "Area of 5×10 must be 50");
}

ResizeAndCheck(new Rectangle()); // OK — 50
ResizeAndCheck(new Square());    // BOOM — area is 100, not 50!
// Setting Height to 10 also set Width to 10 → 10×10 = 100
```

### The fix — don't inherit, share an interface instead

```csharp
public interface IShape { double GetArea(); }

public class Rectangle : IShape
{
    public double Width  { get; set; }
    public double Height { get; set; }
    public double GetArea() => Width * Height;
}

public class Square : IShape
{
    public double Side { get; set; }
    public double GetArea() => Side * Side; // Square's own contract, no Rectangle side effects
}

public void PrintArea(IShape shape) => Console.WriteLine($"Area: {shape.GetArea()}");
// Substituting Rectangle or Square both work correctly
```

### Three rules a subclass must not break

**1. Don't strengthen preconditions** (don't demand more than the base requires):

```csharp
// Base: accepts any amount > 0
public virtual void ProcessPayment(decimal amount)
{
    if (amount <= 0) throw new ArgumentException("Amount must be > 0");
}

// WRONG — subclass requires minimum 100 (stronger than the base's condition)
public override void ProcessPayment(decimal amount)
{
    if (amount < 100) throw new ArgumentException("Minimum is 100"); // breaks callers!
    base.ProcessPayment(amount);
}
```

**2. Don't weaken postconditions** (don't deliver less than the base promises):

```csharp
// Base promises: after Save(), the file exists on disk at the returned path
public virtual string Save(byte[] data, string fileName)
{
    var path = Path.Combine("storage", fileName);
    File.WriteAllBytes(path, data);
    return path; // guarantee: file exists here
}

// WRONG — saves only to cache, not disk. Returns path but file doesn't exist.
public override string Save(byte[] data, string fileName)
{
    _cache[fileName] = data;
    return Path.Combine("storage", fileName); // file does NOT exist — breaks callers!
}
```

**3. Don't break invariants** (don't violate what the base class always guarantees):

```csharp
// BankAccount invariant: Balance >= 0
public virtual void Withdraw(decimal amount)
{
    if (amount > Balance) throw new InvalidOperationException("Insufficient funds");
    Balance -= amount; // invariant preserved
}

// WRONG — VIP account allows overdraft, Balance goes negative
public override void Withdraw(decimal amount)
{
    Balance -= amount; // invariant BROKEN
}
```

### LSP in practice — spotting violations

```csharp
// Classic symptom: checking type at runtime to special-case subclasses
public void ProcessShape(Shape shape)
{
    if (shape is Circle c) { /* special circle logic */ }
    else if (shape is Triangle t) { /* special triangle logic */ }
    // If Shape were substitutable, you wouldn't need these checks
}

// Throwing NotImplementedException in an override — always an LSP violation
public class Penguin : Bird
{
    public override void Fly() => throw new NotImplementedException("Penguins don't fly!");
}
// Fix: split Bird into IFlyingBird and ISwimmingBird; Penguin implements only ISwimmingBird
```

---

## 7. I — Interface Segregation Principle (ISP)

> **Clients should not be forced to depend on interfaces they do not use.**

A fat interface forces implementing classes to provide empty/throwing methods for things they don't support — a code smell and an LSP violation simultaneously.

### The violation — one fat interface

```csharp
// FAT — every implementor must implement all 10 methods
public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(int id);
    Task<IEnumerable<Student>> GetAllAsync();
    Task AddAsync(Student student);
    Task DeleteAsync(int id);
    Task<byte[]> ExportToPdfAsync();
    Task ArchiveAsync(int studentId);
    Task<decimal> GetAverageGradeAsync(int id);
    // ... more
}

// A read-only class is forced to throw for write operations
public class ReadOnlyStudentRepository : IStudentRepository
{
    public Task<Student?> GetByIdAsync(int id) { /* OK */ return ...; }
    public Task<IEnumerable<Student>> GetAllAsync() { /* OK */ return ...; }
    public Task AddAsync(Student student) => throw new NotImplementedException("Read-only!");
    public Task DeleteAsync(int id)       => throw new NotImplementedException("Read-only!");
    public Task<byte[]> ExportToPdfAsync()=> throw new NotImplementedException("Not my job!");
}
```

### The fix — role-based interfaces

```csharp
// Split by role
public interface IStudentReader
{
    Task<Student?> GetByIdAsync(int id);
    Task<IEnumerable<Student>> GetAllAsync();
    Task<IEnumerable<Student>> GetByNameAsync(string name);
    Task<bool> ExistsAsync(int id);
}

public interface IStudentWriter
{
    Task AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(int id);
}

public interface IStudentReportProvider
{
    Task<decimal> GetAverageGradeAsync(int studentId);
    Task<IEnumerable<Student>> GetTopStudentsAsync(int count);
    Task<int> CountAsync();
}

public interface IStudentExporter
{
    Task<byte[]> ExportToCsvAsync();
    Task<byte[]> ExportToPdfAsync();
}

// Read-only repository — implements ONLY IStudentReader, zero NotImplementedException
public class StudentReader : IStudentReader
{
    private readonly AppDbContext _context;
    public StudentReader(AppDbContext context) => _context = context;

    public async Task<Student?> GetByIdAsync(int id)         => await _context.Students.FindAsync(id);
    public async Task<IEnumerable<Student>> GetAllAsync()    => await _context.Students.ToListAsync();
    public async Task<IEnumerable<Student>> GetByNameAsync(string name)
        => await _context.Students.Where(s => s.Name.Contains(name)).ToListAsync();
    public async Task<bool> ExistsAsync(int id)              => await _context.Students.AnyAsync(s => s.Id == id);
    // No write methods at all
}

// Full CRUD repository — implements both
public class StudentRepository : IStudentReader, IStudentWriter
{
    /* implements all read AND write methods */
}

// Each service depends only on what it uses
public class StudentListService
{
    private readonly IStudentReader _reader; // narrow dependency
    public StudentListService(IStudentReader reader) => _reader = reader;
    public async Task<IEnumerable<Student>> GetStudentsAsync() => await _reader.GetAllAsync();
}

public class StudentEnrollmentService
{
    private readonly IStudentReader _reader;
    private readonly IStudentWriter _writer;
    public StudentEnrollmentService(IStudentReader reader, IStudentWriter writer)
    { _reader = reader; _writer = writer; }

    public async Task EnrollAsync(Student student)
    {
        if (await _reader.ExistsAsync(student.Id))
            throw new InvalidOperationException("Student already exists");
        await _writer.AddAsync(student);
    }
}
```

### One class, many interfaces — this is fine

```csharp
// A single class can implement multiple interfaces
public class NotificationHub : IEmailSender, ISmsSender, IPushNotificationSender,
                                ISlackSender, INotificationAnalytics
{
    // implements all methods
}

// DI registration — each interface maps to the same instance
builder.Services.AddScoped<IEmailSender,              NotificationHub>();
builder.Services.AddScoped<ISmsSender,                NotificationHub>();
builder.Services.AddScoped<IPushNotificationSender,   NotificationHub>();
```

**The signal that ISP is violated:** you see `throw new NotImplementedException()` or `{ }` (empty body) in interface implementations.

---

## 8. D — Dependency Inversion Principle (DIP)

> 1. High-level modules should not depend on low-level modules. Both should depend on abstractions.
> 2. Abstractions should not depend on details. Details should depend on abstractions.

**The dependency is "inverted"** — instead of high-level code depending on concrete low-level code, both depend on an interface that the high-level code defines.

### Before DIP — tight coupling between layers

```csharp
// High-level module depends DIRECTLY on the low-level implementation
public class OrderService
{
    private readonly SqlOrderRepository _repository = new SqlOrderRepository("Server=...");
    // Now OrderService can ONLY use SQL Server. Cannot test without a database.
}

// Dependency direction: OrderService → SqlOrderRepository
// Changing the database forces changing OrderService
```

### After DIP — both depend on an abstraction

```csharp
// Step 1: Abstraction (interface) in the HIGH-LEVEL layer (or a shared "Contracts" layer)
public interface IOrderRepository
{
    void Save(Order order);
    Order? GetById(int id);
    // NOTE: no SqlDataReader, no SQL-specific types — interface reveals nothing about implementation
}

// Step 2: Low-level detail IMPLEMENTS the abstraction
public class SqlOrderRepository : IOrderRepository
{
    private readonly string _connectionString;
    public SqlOrderRepository(string cs) => _connectionString = cs;
    public void Save(Order order)
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        using var cmd = new SqlCommand("INSERT INTO Orders ...", conn);
        cmd.ExecuteNonQuery();
    }
    public Order? GetById(int id) { /* ... */ return null; }
}

// Step 3: High-level module depends ONLY on the interface
public class OrderService
{
    private readonly IOrderRepository _repository;
    public OrderService(IOrderRepository repository) => _repository = repository;
    public void PlaceOrder(Order order)
    {
        if (order.Items.Count == 0) throw new InvalidOperationException("Order must have products");
        order.TotalPrice = order.Items.Sum(i => i.Price * i.Quantity);
        _repository.Save(order); // doesn't know or care if it's SQL, Mongo, or in-memory
    }
}

// Step 4: Caller (composition root) decides which implementation to inject
var repo    = new SqlOrderRepository("Server=localhost;Database=Shop");
var service = new OrderService(repo);
service.PlaceOrder(new Order { ... });
```

**Dependency direction after DIP:**

```
OrderService  →  IOrderRepository  ←  SqlOrderRepository
     ↑                  ↑                     ↑
 high-level         abstraction            low-level
```

Both point at the abstraction — the arrow is *inverted* compared to the naive approach.

### Swapping implementations without touching OrderService

```csharp
// MongoDB implementation — zero changes to OrderService
public class MongoOrderRepository : IOrderRepository
{
    public void Save(Order order)
    {
        var col = _database.GetCollection<Order>("orders");
        col.InsertOne(order);
    }
    public Order? GetById(int id) => _database.GetCollection<Order>("orders")
                                              .Find(o => o.Id == id).FirstOrDefault();
}

// In-memory implementation for tests — no database at all
public class FakeOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();
    public void Save(Order order)   => _orders.Add(order);
    public Order? GetById(int id)   => _orders.FirstOrDefault(o => o.Id == id);
}

// OrderService code never changes
var prod  = new OrderService(new SqlOrderRepository("..."));   // production
var mongo = new OrderService(new MongoOrderRepository());       // different DB
var test  = new OrderService(new FakeOrderRepository());        // unit tests
```

---

## 9. IoC, DI, and the ASP.NET Core Container

These three concepts are nested:

```
IoC (Inversion of Control)         — architectural principle (broadest)
  └─ DIP (Dependency Inversion Principle) — SOLID design principle
        └─ DI (Dependency Injection)      — technique for implementing IoC
```

**Inversion of Control** means the framework calls your code rather than your code calling the framework. The DI container is the runtime mechanism that implements this.

### Three DI injection patterns

```csharp
// 1. Constructor injection — RECOMMENDED for required dependencies
public class OrderService
{
    private readonly IOrderRepository _repo;
    private readonly IEmailService    _email;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IOrderRepository repo, IEmailService email, ILogger<OrderService> logger)
    {
        _repo   = repo   ?? throw new ArgumentNullException(nameof(repo));
        _email  = email  ?? throw new ArgumentNullException(nameof(email));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
}

// 2. Property injection — for OPTIONAL dependencies
public class OrderService
{
    public ILogger<OrderService>? Logger { get; set; } // null = no logging
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;

    public void PlaceOrder(Order order)
    {
        Logger?.LogInformation("Placing order...");
        _repo.Save(order);
    }
}

// 3. Method injection — dependency used by ONLY ONE method
public class OrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;

    public void PlaceOrder(Order order, IDiscountCalculator discountCalc)
    {
        var discount = discountCalc.Calculate(order);
        order.ApplyDiscount(discount);
        _repo.Save(order);
    }
}
```

### Registering services in ASP.NET Core (Program.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);

// "When someone asks for IOrderRepository, give them SqlOrderRepository"
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();
builder.Services.AddScoped<IEmailService,    SmtpEmailService>();
// "When someone asks for OrderService, create it — container auto-resolves dependencies"
builder.Services.AddScoped<OrderService>();

var app = builder.Build();
// ASP.NET Core DI injects into controller constructors automatically
app.MapControllers();
app.Run();
```

### Three service lifetimes

| Lifetime | How long the instance lives | Use for |
|---|---|---|
| **Transient** `AddTransient` | New instance every injection | Stateless services, lightweight operations |
| **Scoped** `AddScoped` | One instance per HTTP request | Most services (repositories, business logic) |
| **Singleton** `AddSingleton` | One instance for the app lifetime | Caches, config, thread-safe shared state |

```csharp
builder.Services.AddTransient<IEmailService,   SmtpEmailService>();     // new per injection
builder.Services.AddScoped<IOrderRepository,   SqlOrderRepository>();    // new per request
builder.Services.AddSingleton<ICacheService,   InMemoryCacheService>();  // one for all time
```

**Captive dependency trap:** never inject a Scoped service into a Singleton. The Singleton lives forever and holds a Scoped instance that should have been disposed after the first request.

```csharp
// BAD — Singleton captures a Scoped service
builder.Services.AddSingleton<IReportGenerator, ReportGenerator>();
builder.Services.AddScoped<IOrderRepository,    SqlOrderRepository>();

public class ReportGenerator : IReportGenerator
{
    private readonly IOrderRepository _repo; // Scoped held by Singleton — WRONG
    public ReportGenerator(IOrderRepository repo) => _repo = repo;
}
```

### Service Locator anti-pattern — don't do this

```csharp
// BAD — hides actual dependencies, makes the class untestable
public class BadService
{
    private readonly IServiceProvider _provider;
    public BadService(IServiceProvider provider) => _provider = provider;
    public void DoWork()
    {
        var repo = _provider.GetRequiredService<IOrderRepository>(); // hidden dependency!
        repo.Save(new Order());
    }
}

// GOOD — explicit, testable
public class GoodService
{
    private readonly IOrderRepository _repo;
    public GoodService(IOrderRepository repo) => _repo = repo;
    public void DoWork() => _repo.Save(new Order());
}
```

### Full layered example with DIP + DI

```csharp
// Interfaces (abstractions — defined in the business layer)
public interface IProductRepository { Task<List<Product>> GetAllAsync(); Task<Product?> GetByIdAsync(int id); }
public interface IProductService    { Task<List<ProductDto>> GetAllProductsAsync(); Task<ProductDto?> GetProductByIdAsync(int id); }

public record Product(int Id, string Name, decimal Price);
public record ProductDto(int Id, string Name, string FormattedPrice);

// Low-level detail implements the abstraction
public class SqlProductRepository : IProductRepository
{
    private readonly string _connectionString;
    public SqlProductRepository(IConfiguration cfg)
        => _connectionString = cfg.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException();

    public async Task<List<Product>> GetAllAsync()
        => new List<Product> { new(1, "Laptop", 3499.99m), new(2, "Mouse", 129.99m) };

    public async Task<Product?> GetByIdAsync(int id)
        => (await GetAllAsync()).FirstOrDefault(p => p.Id == id);
}

// High-level module — depends only on abstractions
public class ProductService : IProductService
{
    private readonly IProductRepository     _repository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, ILogger<ProductService> logger)
    { _repository = repository; _logger = logger; }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        _logger.LogInformation("Fetching all products");
        return (await _repository.GetAllAsync())
            .Select(p => new ProductDto(p.Id, p.Name, $"{p.Price:C}"))
            .ToList();
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var p = await _repository.GetByIdAsync(id);
        return p is null ? null : new ProductDto(p.Id, p.Name, $"{p.Price:C}");
    }
}

// Controller — depends on service interface
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService) => _productService = productService;

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
        => Ok(await _productService.GetAllProductsAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var p = await _productService.GetProductByIdAsync(id);
        return p is null ? NotFound() : Ok(p);
    }
}

// Composition root — registers everything
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IProductRepository, SqlProductRepository>();
builder.Services.AddScoped<IProductService,    ProductService>();
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
app.Run();
```

### Testing with a fake — DIP makes it trivial

```csharp
public class FakeProductRepository : IProductRepository
{
    private readonly List<Product> _products = new() { new(1, "Test Product", 99.99m) };
    public Task<List<Product>>    GetAllAsync()         => Task.FromResult(_products);
    public Task<Product?> GetByIdAsync(int id)          => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
}

[Fact]
public async Task GetAllProducts_ReturnsFormattedProducts()
{
    var service = new ProductService(new FakeProductRepository(), NullLogger<ProductService>.Instance);
    var result  = await service.GetAllProductsAsync();

    Assert.Single(result);
    Assert.Equal("Test Product", result[0].Name);
    Assert.Equal("$99.99",       result[0].FormattedPrice);
}
```

---

## 10. Complete Summary and Pitfalls

### The 5 SOLID principles in one sentence each

| Principle | One sentence | Key tool |
|---|---|---|
| **SRP** | One class, one reason to change, one stakeholder | Split classes by actor |
| **OCP** | Add new code, don't modify old code | Strategy pattern / interface + new class |
| **LSP** | Subtypes must honour the base type's contract | Prefer composition over inheritance |
| **ISP** | Don't force clients to depend on what they don't use | Multiple small interfaces |
| **DIP** | Depend on abstractions; the abstraction belongs to the high-level module | Interface + constructor injection |

### SOLID works together

SRP says "one responsibility per class." OCP says "extend via new classes." ISP says "keep interfaces small." DIP says "depend on the abstractions." LSP says "honour the contract." These four form a self-reinforcing system:

- SRP → small classes with clear purpose
- ISP → small interfaces that only describe one role
- DIP → classes depend on those small interfaces
- OCP → new behaviour = new class implementing an existing interface
- LSP → new classes genuinely fulfil the interface contract

### Common pitfalls

```
PITFALL                             SOLUTION
────────────────────────────────────────────────────────────────────
Helper / Utils / Manager class      Split into focused classes by domain
  (coincidental cohesion)

new SomeService() inside a class    Constructor injection + interface
  (tight coupling)

if/else or switch per new case      Strategy pattern (OCP)

throw NotImplementedException       ISP — split the fat interface
  in interface implementations

Runtime type checks (is/as)         LSP — redesign the hierarchy
  on subtypes

Injecting IServiceProvider          Direct constructor injection of what you need
  (Service Locator anti-pattern)

Injecting Scoped into Singleton     Match lifetime to shortest dependency
  (captive dependency)

Abstracting everything              YAGNI — only abstract what has 2+ implementations
  (overengineering)                 or needs to be mockable in tests

Magic numbers scattered in code     Named constants in a BusinessRules class
  (violates DRY)

Merging two coincidentally identical Rule of Three: wait for the third occurrence,
  methods too soon                  then extract
```
