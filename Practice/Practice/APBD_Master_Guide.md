# APBD — Complete Exam Preparation Master Guide
**Polish-Japanese Academy of IT | Based on all course materials, lectures, labs, and practise files**

> **How to use:** Read top-to-bottom once. Then use the section headers to jump to weak spots. Every code block is exam-ready — copy the pattern, change the domain name.

---

## Table of Contents

1. [Git — Commands & Workflow](#1-git)
2. [.NET Architecture — CLR, JIT, IDisposable](#2-net-architecture)
3. [C# Type System — Value vs Reference, Properties, Null, Records](#3-c-type-system)
4. [OOP — Abstract, Interfaces, Exceptions, Patterns](#4-oop)
5. [SOLID Principles — All Five with Code](#5-solid)
6. [LINQ — Every Operator, Deferred Execution, Joins](#6-linq)
7. [Async / Await / Task](#7-async-await)
8. [Generics, Delegates, yield return, StringBuilder](#8-advanced-csharp)
9. [ASP.NET Core — Controllers, Routing, Middleware](#9-aspnet-core)
10. [ADO.NET — SqlConnection, SqlCommand, SqlDataReader, Transactions](#10-adonet)
11. [3-Layer Architecture + DI Registration](#11-3-layer-architecture)
12. [Exam Pattern — The Complete Test Template](#12-exam-pattern)
13. [15-Minute Exam Checklist](#13-exam-checklist)
14. [Flash Cards — 50 Most-Asked Questions](#14-flash-cards)

---

---

## 1. Git

### The four workflows examiners ask about

```bash
# ── WORKFLOW 1: Start a new repo from scratch ──────────────────────
git config --global user.name  "Your Name"
git config --global user.email "you@example.com"
git init
git add .
git commit -m "initial commit"
git remote add origin https://github.com/you/repo.git
git push -u origin main          # -u sets upstream, required once

# ── WORKFLOW 2: Feature branch + rebase (linear history) ──────────
git checkout -b feature/login    # create and switch
# ... write code ...
git add .
git commit -m "add login"

git checkout main
git pull                          # get latest main
git checkout feature/login
git rebase main                   # replay feature commits on top of main
# resolve conflicts if any:
#   edit the file → git add <file> → git rebase --continue
git checkout main
git merge --ff-only feature/login # fast-forward only (no merge commit)
git push
git branch -d feature/login       # clean up local
git push origin --delete feature/login  # clean up remote

# ── WORKFLOW 3: Undo mistakes ──────────────────────────────────────
git restore <file>               # discard unstaged changes to a file
git restore --staged <file>      # unstage a file (keep changes in working dir)
git reset --soft HEAD~1          # undo last commit, keep changes staged
git reset --hard HEAD~1          # undo last commit AND discard changes ⚠️
git revert <hash>                # create a new commit that reverses <hash>
                                 # USE revert on public branches, reset only on local

# ── WORKFLOW 4: Stash unfinished work ─────────────────────────────
git stash                        # save dirty working dir to stash stack
git stash list                   # see all stashes
git stash pop                    # apply newest stash and remove it
git stash apply stash@{1}        # apply specific stash, keep it in list
```

### Merge vs Rebase — the exam question

| | `git merge` | `git rebase` |
|---|---|---|
| History | Non-linear (adds a merge commit) | Linear (replays commits on top) |
| When to use | Public branches, preserve context | Private feature branches |
| NEVER use rebase on... | — | `main` or any shared branch others have pulled |
| Conflict format in file | `<<<<<<< HEAD`, `=======`, `>>>>>>>` | Same format |
| After resolving conflict | `git add` → `git merge --continue` | `git add` → `git rebase --continue` |

> **EXAM TRAP:** After resolving a rebase conflict you must run `git rebase --continue`, NOT `git commit`. Running `git commit` creates an unexpected extra commit.

### `--ff-only` explained

```bash
git merge --ff-only feature/login
```

`--ff-only` means: **only perform this merge if it can be done as a fast-forward** (no new merge commit). It fails if the branches have diverged. This is used after a rebase to guarantee the merge is truly linear. If it fails, your rebase was incomplete.

### `.gitignore` — required before first commit

```gitignore
# ALWAYS add this before committing a .NET project
bin/
obj/
*.user
.vs/
*.suo
appsettings.Development.json
```

> **EXAM TRAP:** Committing `bin/` and `obj/` in the exam costs **-50% of total score**. Add `.gitignore` as the **very first commit**.

---

---

## 2. .NET Architecture

### Component map

| Component | What it does | Java equivalent |
|---|---|---|
| **Roslyn** | C# compiler → converts C# source to IL (Intermediate Language) bytecode | `javac` |
| **CLR** | Common Language Runtime — the virtual machine that runs IL | JVM |
| **JIT** | Just-In-Time compiler — compiles IL → native machine code **at runtime** | JIT in JVM |
| **AOT** | Ahead-Of-Time compiler — compiles IL → native code **before** the app runs | GraalVM native |
| **BCL** | Base Class Library — standard library (string, List, File…) | Java Standard Library |
| **NuGet** | Package manager | Maven / Gradle |

> **EXAM TRAP:** JIT compiles at **runtime** (first call). AOT compiles **before** the app starts. This distinction appears in almost every exam.

### IDisposable — the `using` pattern

```csharp
// ── WRONG: connection is never released if an exception occurs ──
var conn = new SqlConnection(cs);
conn.Open();
// ... if exception here, conn is never closed!

// ── CORRECT: 'using' calls Dispose() even if an exception occurs ──
using var conn = new SqlConnection(cs);   // C# 8+ syntax
await conn.OpenAsync();
// ... Dispose() is guaranteed at end of scope

// ── What Dispose() actually does for SqlConnection ──
// It does NOT close the TCP connection.
// It returns the connection object back to the connection pool.
// The pool reuses it for the next caller. This is why pooling is fast.
```

**Custom IDisposable implementation:**

```csharp
public class FileResource : IDisposable
{
    private FileStream? _stream;
    private bool _disposed = false;

    public FileResource(string path)
    {
        _stream = File.OpenRead(path);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _stream?.Dispose();
            _disposed = true;
        }
    }
}

// Usage:
using var file = new FileResource("data.txt");
// Dispose() called automatically here, even on exception
```

---

---

## 3. C# Type System

### Value types vs Reference types

```csharp
// ── VALUE TYPE (struct) — copied on assignment ──────────────────
struct Point { public int X; public int Y; }

var p1 = new Point { X = 1, Y = 2 };
var p2 = p1;          // COPY — p2 is independent
p2.X = 99;
Console.WriteLine(p1.X);  // 1 — p1 is unchanged

// ── REFERENCE TYPE (class) — shared reference ──────────────────
class Color { public int R, G, B; }

var c1 = new Color { R = 255 };
var c2 = c1;          // SAME OBJECT — both point to same heap memory
c2.R = 0;
Console.WriteLine(c1.R);  // 0 — c1 changed because c1 and c2 are the same object!
```

> **EXAM TRAP (floating point):**
> ```csharp
> double a = 0.1 + 0.2;
> Console.WriteLine(a == 0.3);     // FALSE — floating-point precision error
> Console.WriteLine(a);             // 0.30000000000000004
> // CORRECT comparison:
> Console.WriteLine(Math.Abs(a - 0.3) < 1e-10);  // TRUE
> ```

### Properties — all 5 forms

```csharp
public class BankAccount
{
    // 1. Auto property — compiler generates backing field
    public string Owner { get; set; } = string.Empty;

    // 2. Read-only auto — can only be set in constructor
    public DateTime CreatedAt { get; } = DateTime.Now;

    // 3. init-only — can only be set in object initializer
    public string AccountNumber { get; init; } = string.Empty;

    // 4. Computed property — no backing field, derives from other data
    public string DisplayName => $"{Owner} ({AccountNumber})";

    // 5. Validated property with private backing field
    private decimal _balance;
    public decimal Balance
    {
        get => _balance;
        set
        {
            if (value < 0) throw new ArgumentException("Balance cannot be negative.");
            _balance = value;
        }
    }
}

// init usage:
var acc = new BankAccount { AccountNumber = "PL12345" };
// acc.AccountNumber = "OTHER";  // COMPILE ERROR — init-only after construction
```

### Null safety operators

```csharp
string? name = null;

// ?? — null-coalescing: use right side when left is null
string display = name ?? "Anonymous";        // "Anonymous"

// ?. — null-conditional: short-circuit to null instead of NullReferenceException
int? len = name?.Length;                     // null (not an exception)
string? upper = name?.ToUpper()?.Trim();     // null

// ??= — null-coalescing assignment: assign only if currently null
name ??= "Default";                          // name is now "Default"
name ??= "Other";                            // name stays "Default" (already set)

// Practical chain — common in exam:
string result = user?.Address?.City ?? "Unknown City";
```

### Records and `with` expressions

```csharp
// Positional record — one line
record Student(int Id, string Name, string City, double Gpa);

// Non-positional property inside a record body
record Product(int Id, string Name, decimal Price)
{
    public string DisplayPrice => $"{Price:C}";   // computed property
}

// Value equality — records compare by content, not reference
var s1 = new Student(1, "Anna", "Warsaw", 4.0);
var s2 = new Student(1, "Anna", "Warsaw", 4.0);
Console.WriteLine(s1 == s2);   // TRUE  ← unlike classes!

// 'with' — create a copy with specific fields changed (original is UNCHANGED)
var s3 = s1 with { City = "Kraków" };
Console.WriteLine(s1.City);   // "Warsaw" — s1 is immutable
Console.WriteLine(s3.City);   // "Kraków"

// Deconstruction
var (id, fullName, city, gpa) = s1;
Console.WriteLine(fullName);  // "Anna"
```

> **EXAM TRAP:** Records use **value equality** (`==` compares all fields). Classes use **reference equality** by default (`==` compares memory addresses). `with` creates a **new copy** — it never mutates the original.

---

---

## 4. OOP

### Abstract class vs Interface — decision table

| Need | Use |
|---|---|
| Shared fields/state between related classes | `abstract class` |
| Shared implementation (not just contract) | `abstract class` |
| Multiple inheritance | `interface` (class can implement many) |
| Unrelated classes need the same capability | `interface` |
| A class must have one reason to change | Separate interface per role (ISP) |

```csharp
// ── ABSTRACT CLASS ────────────────────────────────────────────
abstract class Shape
{
    public string Colour { get; set; } = "black";   // field + property allowed
    public abstract double Area();                   // no body — subclasses must implement
    public virtual string Describe()                 // virtual = subclasses CAN override
        => $"{GetType().Name}, colour={Colour}";
}

class Circle : Shape
{
    public double Radius { get; init; }
    public override double Area() => Math.PI * Radius * Radius;   // MUST implement Area
}

// ── INTERFACE ────────────────────────────────────────────────
interface IPrintable { void Print(); }
interface ISaveable  { void Save(string path); }

// A class can implement multiple interfaces
class Report : IPrintable, ISaveable
{
    public void Print() => Console.WriteLine("Printing...");
    public void Save(string path) => File.WriteAllText(path, "data");
}
```

### override vs new (hiding) — exam favourite

```csharp
class Animal
{
    public virtual  void Eat() => Console.WriteLine("Animal eats");
    public          void Sleep() => Console.WriteLine("Animal sleeps");
}

class Dog : Animal
{
    public override void Eat()  => Console.WriteLine("Dog eats");  // POLYMORPHISM
    public new      void Sleep()=> Console.WriteLine("Dog sleeps");// HIDING — NOT polymorphic
}

// The difference shows up when you use a base-type reference:
Animal a = new Dog();
a.Eat();    // "Dog eats"   ← override  — runtime type determines call
a.Sleep();  // "Animal sleeps" ← new — compile-time type determines call ⚠️
```

> **EXAM TRAP:** `override` enables **polymorphism** — the runtime type decides. `new` is **method hiding** — the declared type decides. Using `new` on a method is almost always a design mistake.

### Custom exceptions with rethrow

```csharp
class NotFoundException : Exception
{
    public int EntityId { get; }
    public NotFoundException(int id)
        : base($"Entity {id} not found.") => EntityId = id;
}

class InsufficientFundsException : Exception
{
    public InsufficientFundsException(decimal amount)
        : base($"Insufficient funds. Required: {amount:C}") { }
}

// ── RETHROW RULES ────────────────────────────────────────────
try { SomeOperation(); }
catch (Exception ex)
{
    Log(ex);
    throw;       // ✅ CORRECT — preserves original stack trace
    // throw ex; // ❌ WRONG  — resets stack trace, you lose where it started
}
```

### Switch expression + pattern matching

```csharp
// Switch expression (C# 8+) — no case/break, returns a value
string Classify(object obj) => obj switch
{
    int n when n < 0    => "negative int",
    int n when n == 0   => "zero",
    int n               => $"positive int: {n}",
    string s when s.Length == 0 => "empty string",
    string s            => $"string: {s}",
    null                => "null",
    _                   => "unknown type"          // _ = default
};

// Grade classifier with guards
string Grade(double score) => score switch
{
    >= 90 => "A",
    >= 80 => "B",
    >= 70 => "C",
    >= 60 => "D",
    _     => "F"
};
```

> **EXAM TRAP:** C# **prohibits fall-through** between non-empty case blocks. In Java, `case 1:` falls through to `case 2:` without a `break`. In C# this is a **compile error**.

---

---

## 5. SOLID

### SRP — Single Responsibility Principle

> One class = one reason to change.

```csharp
// ❌ BAD — OrderService does 4 different things
class OrderService_Bad
{
    public void PlaceOrder(Order o) { /* business logic */ }
    public void SendConfirmationEmail(Order o) { /* email logic */ }
    public void SaveToDatabase(Order o) { /* SQL logic */ }
    public void PrintReceipt(Order o) { /* formatting logic */ }
}

// ✅ GOOD — each class has one job
class OrderService      { public void PlaceOrder(Order o) { } }
class EmailNotifier     { public void SendConfirmation(Order o) { } }
class OrderRepository   { public void Save(Order o) { } }
class ReceiptPrinter    { public void Print(Order o) { } }
```

### OCP — Open/Closed Principle + Strategy Pattern

> Open for extension, closed for modification.

```csharp
// ❌ BAD — adding a new discount type requires editing this class
class BadDiscountCalculator
{
    public decimal Calculate(string type, decimal price) => type switch
    {
        "student"  => price * 0.9m,
        "senior"   => price * 0.8m,
        // "employee" would require editing this file ← OCP violation
        _ => price
    };
}

// ✅ GOOD — Strategy Pattern: add new types without touching existing code
interface IDiscountStrategy { decimal Apply(decimal price); }

class StudentDiscount  : IDiscountStrategy { public decimal Apply(decimal p) => p * 0.9m; }
class SeniorDiscount   : IDiscountStrategy { public decimal Apply(decimal p) => p * 0.8m; }
class EmployeeDiscount : IDiscountStrategy { public decimal Apply(decimal p) => p * 0.6m; }
// Adding a new type = adding a new class. Zero changes to existing code.

class PriceCalculator
{
    private readonly IDiscountStrategy _strategy;
    public PriceCalculator(IDiscountStrategy strategy) => _strategy = strategy;
    public decimal Calculate(decimal price) => _strategy.Apply(price);
}
```

### LSP — Liskov Substitution Principle

> A subclass must be usable wherever the base class is expected without breaking the program.

```csharp
// ❌ CLASSIC VIOLATION — Rectangle/Square
class Rectangle
{
    public virtual int Width  { get; set; }
    public virtual int Height { get; set; }
    public int Area() => Width * Height;
}

class Square : Rectangle  // WRONG — Square IS-NOT a proper Rectangle
{
    public override int Width  { set { base.Width = base.Height = value; } get => base.Width; }
    public override int Height { set { base.Width = base.Height = value; } get => base.Height; }
}

// This breaks the caller's assumption:
Rectangle r = new Square();
r.Width = 4; r.Height = 5;
Console.WriteLine(r.Area());  // Expected 20, but Square forces Width==Height → gets 25!

// ✅ FIX — use a shared interface, not inheritance
interface IShape { int Area(); }
class Rectangle2 : IShape { public int W, H; public int Area() => W * H; }
class Square2    : IShape { public int S;    public int Area() => S * S; }
```

### ISP — Interface Segregation Principle

> Many small interfaces are better than one fat interface.

```csharp
// ❌ BAD — fat interface forces unrelated implementations
interface IWorker { void Work(); void Eat(); void Sleep(); }
class Robot : IWorker  // Robots don't eat or sleep!
{
    public void Work() { /* OK */ }
    public void Eat()  { throw new NotImplementedException(); }  // forced fake ← ISP violation
    public void Sleep(){ throw new NotImplementedException(); }
}

// ✅ GOOD — split by role
interface IWorkable  { void Work(); }
interface IFeedable  { void Eat(); }
interface ISleepable { void Sleep(); }

class Human : IWorkable, IFeedable, ISleepable
{
    public void Work()  { }
    public void Eat()   { }
    public void Sleep() { }
}

class Robot2 : IWorkable  // Only implements what it actually does
{
    public void Work() { }
}
```

### DIP — Dependency Inversion Principle + DI Lifetimes

> Depend on abstractions, not concretions.

```csharp
// ❌ BAD — controller creates its own dependency (hardcoded)
class OrdersController_Bad
{
    private readonly SqlOrderRepository _repo = new SqlOrderRepository(); // ← hardcoded
}

// ✅ GOOD — depends on the interface, injected from outside
class OrdersController
{
    private readonly IOrderRepository _repo;  // ← abstraction
    public OrdersController(IOrderRepository repo) => _repo = repo;  // ← DI
}

// ── DI LIFETIMES in Program.cs ────────────────────────────────
// Transient  — new instance every single time it is injected (stateless utilities)
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

// Scoped     — one instance per HTTP request (repositories, services with ADO.NET)
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();
builder.Services.AddScoped<IOrderService,    OrderService>();

// Singleton  — one instance for the entire app lifetime (config, caches)
builder.Services.AddSingleton<IAppSettings, AppSettings>();
```

> **EXAM TRAP:** **NEVER inject a Scoped service into a Singleton.** The Scoped service will never be released for the lifetime of the app. This is called a **captive dependency** and causes memory leaks and stale data.

---

---

## 6. LINQ

### Deferred execution — the most-tested trap

```csharp
var students = new List<Student>
{
    new("Anna", 3.8), new("Bob", 2.5), new("Carol", 3.9)
};

// DEFERRED — query is NOT executed here, just defined
var highGpa = students.Where(s => s.Gpa >= 3.5);

// Adding a student AFTER defining the query
students.Add(new("Dana", 4.0));

// The query runs HERE — Dana IS included because we haven't called ToList() yet
foreach (var s in highGpa)
    Console.WriteLine(s.Name);  // Anna, Carol, Dana

// FIX with ToList() — snapshot at point of materialization
var snapshot = students.Where(s => s.Gpa >= 3.5).ToList();
students.Add(new("Eve", 3.8));
// Eve is NOT in snapshot — snapshot was taken before Eve was added
```

**Deferred operators** (query runs later): `Where`, `Select`, `OrderBy`, `ThenBy`, `GroupBy`, `Join`, `SelectMany`  
**Immediate operators** (run right now): `ToList()`, `ToArray()`, `Count()`, `First()`, `Any()`, `All()`, `Sum()`, `Max()`

### Core operators cheat sheet

```csharp
var students = GetStudents();  // List<Student> — Id, Name, Major, Gpa, Year

// ── Filtering ────────────────────────────────────────────────
var honours = students.Where(s => s.Gpa >= 3.5).ToList();

// ── Projection ───────────────────────────────────────────────
var names = students.Select(s => s.Name).ToList();
var tuples = students.Select(s => (s.Name, s.Gpa)).ToList();  // anonymous tuple

// ── Sorting ──────────────────────────────────────────────────
var sorted = students.OrderByDescending(s => s.Gpa)
                     .ThenBy(s => s.Name)
                     .ToList();

// ── Quantifiers (return bool) ────────────────────────────────
bool anyAbove3 = students.Any(s => s.Gpa > 3.0);
bool allActive = students.All(s => s.IsActive);

// ── Aggregation ──────────────────────────────────────────────
double avg    = students.Average(s => s.Gpa);
double maxGpa = students.Max(s => s.Gpa);
int    count  = students.Count(s => s.Major == "CS");

// ── Paging (Skip + Take) ─────────────────────────────────────
var page2 = students.OrderBy(s => s.Id)
                    .Skip((2 - 1) * 10)   // skip first 10 (page 1)
                    .Take(10)              // take next 10 (page 2)
                    .ToList();

// ── GroupBy ──────────────────────────────────────────────────
var byMajor = students
    .GroupBy(s => s.Major)
    .Select(g => new { Major = g.Key, Count = g.Count(), AvgGpa = g.Average(s => s.Gpa) })
    .ToList();

// ── Join (inner join — only rows that match) ─────────────────
var enrolled = students.Join(
    enrollments,
    s => s.Id,                    // key from students
    e => e.StudentId,             // key from enrollments
    (s, e) => new { s.Name, e.CourseId }   // result shape
).ToList();

// ── GroupJoin (left join — ALL students, even unenrolled ones) ──
var withCourses = students.GroupJoin(
    enrollments,
    s => s.Id,
    e => e.StudentId,
    (s, es) => new { s.Name, Courses = es.ToList() }
).ToList();

// ── SelectMany (flatten collections) ────────────────────────
var allCourseNames = students
    .SelectMany(s => s.Courses)   // each student has a List<Course>
    .Select(c => c.Name)
    .Distinct()
    .ToList();
```

> **EXAM TRAP — Join vs GroupJoin:**
> - `Join` = SQL `INNER JOIN` → students with no enrollments **disappear**
> - `GroupJoin` = SQL `LEFT JOIN` → all students appear, empty list if no enrollments

---

---

## 7. Async / Await

### The golden rules

```csharp
// Rule 1: async propagates — if a method uses await, it must be async
// Rule 2: never block an async method with .Result or .Wait() — causes deadlocks
// Rule 3: I/O methods in ASP.NET Core MUST be async (ADO.NET, HttpClient, File I/O)

// ── Correct async method ────────────────────────────────────
public async Task<string> FetchDataAsync(string url)
{
    using var client = new HttpClient();
    return await client.GetStringAsync(url);  // frees thread while waiting
}

// ── Task.WhenAll — run multiple tasks CONCURRENTLY ───────────
public async Task<(string A, string B)> FetchBothAsync()
{
    var taskA = FetchDataAsync("https://api.example.com/a");  // starts immediately
    var taskB = FetchDataAsync("https://api.example.com/b");  // starts immediately

    await Task.WhenAll(taskA, taskB);         // wait for BOTH to finish
    return (taskA.Result, taskB.Result);      // both are already done here
    // Total time ≈ max(timeA, timeB) instead of timeA + timeB
}

// ── Task.WhenAny — finish as soon as ONE completes ───────────
var firstDone = await Task.WhenAny(taskA, taskB);  // useful for timeouts

// ── Sequential (slow) vs Concurrent (fast) ──────────────────
// SLOW — total = 300ms
await Task.Delay(100);
await Task.Delay(200);

// FAST — total = 200ms (max of both)
await Task.WhenAll(Task.Delay(100), Task.Delay(200));
```

> **EXAM TRAP:** `Task.WhenAll` starts all tasks before awaiting any. Sequential `foreach await` = sum of all delays. `WhenAll` = longest single delay.

---

---

## 8. Advanced C#

### Delegates — Action, Func, Predicate

```csharp
// Action — returns void
Action<string> log = msg => Console.WriteLine($"[LOG] {msg}");
log("Server started");

// Func — returns a value (last type parameter is the return type)
Func<int, int, int> add = (a, b) => a + b;
Console.WriteLine(add(3, 4));  // 7

Func<string, bool> isLong = s => s.Length > 10;

// Predicate — returns bool (shorthand for Func<T, bool>)
Predicate<int> isEven = n => n % 2 == 0;

// Multicast delegate — chain multiple methods
Action<string> multi = msg => Console.Write("A: " + msg);
multi += msg => Console.Write("B: " + msg);   // += adds to chain
multi("hello");   // both fire
multi -= multi;   // -= removes from chain

// ── CLOSURE TRAP — the classic exam question ─────────────────
var actions = new List<Action>();
for (int i = 0; i < 3; i++)
{
    actions.Add(() => Console.WriteLine(i));  // ⚠️ captures reference to i
}
actions.ForEach(a => a());  // Prints: 3, 3, 3 — NOT 0, 1, 2!
// WHY: by the time the actions run, the loop is finished, i = 3

// FIX: capture a local copy inside the loop
for (int i = 0; i < 3; i++)
{
    int copy = i;
    actions.Add(() => Console.WriteLine(copy));  // captures copy
}
actions.ForEach(a => a());  // Now prints: 0, 1, 2
```

### yield return — lazy sequences

```csharp
// yield return produces ONE element at a time, on demand
// The method body is suspended between yields
IEnumerable<int> CountTo(int max)
{
    for (int i = 1; i <= max; i++)
    {
        Console.WriteLine($"generating {i}");
        yield return i;     // pause here, give i to caller
    }
}

// LAZY — only generates as many as the caller consumes:
foreach (var n in CountTo(100).Take(3))
    Console.Write(n + " ");
// Output: "generating 1" 1 "generating 2" 2 "generating 3" 3
// The remaining 97 numbers are NEVER generated
```

### StringBuilder vs string +=

```csharp
// ❌ BAD — O(n²) — creates a new string object every iteration
string result = "";
for (int i = 0; i < 10_000; i++)
    result += i.ToString();    // allocates 10,000 strings!

// ✅ GOOD — O(n) amortised — one buffer, appended in place
var sb = new StringBuilder();
for (int i = 0; i < 10_000; i++)
    sb.Append(i);              // no allocation per iteration

string final = sb.ToString();  // single allocation at the end
```

### Generics and reification

```csharp
// Generic class with type constraint
class Repository<T> where T : class
{
    private readonly List<T> _items = new();
    public void Add(T item) => _items.Add(item);
    public T? Find(Func<T, bool> predicate) => _items.FirstOrDefault(predicate);
}

var repo = new Repository<Student>();
repo.Add(new Student(1, "Anna"));
var anna = repo.Find(s => s.Name == "Anna");

// ── REIFICATION — C# vs Java ──────────────────────────────────
// C#: List<int> and List<string> are DIFFERENT types at runtime
Console.WriteLine(typeof(List<int>)    == typeof(List<string>)); // FALSE
Console.WriteLine(typeof(List<int>).Name);                       // "List`1"

// Java: Type Erasure — List<Integer> and List<String> are SAME type at runtime
// In Java: list.getClass() returns just "ArrayList" for both
```

> **EXAM TRAP:** C# generics are **reified** — real types at runtime. Java uses **type erasure** — `List<Integer>` and `List<String>` are both just `List` at runtime.

---

---

## 9. ASP.NET Core

### Controller anatomy

```csharp
[ApiController]             // ← enables auto-validation + Problem Details errors
[Route("api/products")]     // ← base path for all actions in this class
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    // Constructor injection — DI provides the service
    public ProductsController(IProductService service) => _service = service;

    // GET api/products?minPrice=10&inStock=true
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll(
        [FromQuery] decimal? minPrice,
        [FromQuery] bool? inStock)
    {
        var products = await _service.GetAllAsync(minPrice, inStock);
        return Ok(products);
    }

    // GET api/products/5
    [HttpGet("{id:int}")]    // :int = route constraint — only matches integers
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    // POST api/products
    [HttpPost]
    public async Task<ActionResult<Product>> Create([FromBody] ProductRequest dto)
    {
        var product = await _service.CreateAsync(dto);
        // 201 Created + Location header pointing to GetById
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT api/products/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> Update(int id, [FromBody] ProductRequest dto)
    {
        var product = await _service.UpdateAsync(id, dto);
        return product is null ? NotFound() : Ok(product);
    }

    // DELETE api/products/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
```

### HTTP status code map — memorise this table

| Situation | Code | Helper method |
|---|---|---|
| Data found and returned | 200 | `Ok(data)` |
| POST created a new resource | 201 | `CreatedAtAction(nameof(GetById), new { id }, obj)` |
| PUT/DELETE succeeded, nothing to return | 204 | `NoContent()` |
| Request body is malformed / missing fields | 400 | `BadRequest("reason")` (auto by `[ApiController]`) |
| Not authenticated (no token) | 401 | `Unauthorized()` |
| Authenticated but forbidden | 403 | `Forbid()` |
| Resource with that ID not found | 404 | `NotFound()` |
| Valid request but conflicts with state | 409 | `Conflict("reason")` |
| Server crashed | 500 | `StatusCode(500, ...)` |

> **EXAM TRAP — 400 vs 404:** 400 = **the request itself** is broken (missing field, wrong type). 404 = **the request is fine** but the thing it refers to doesn't exist.  
> **EXAM TRAP — 401 vs 403:** 401 = I don't know who you are. 403 = I know who you are, but you can't do this.  
> **EXAM TRAP — 409 vs 400:** 409 = the data is valid but the **current server state** prevents it (e.g. already booked, already on loan).

### Binding sources

```csharp
// [FromRoute]  — from the URL path segment  /api/items/{id}
public IActionResult Get([FromRoute] int id) { }

// [FromQuery]  — from query string  /api/items?page=2&size=10
public IActionResult GetAll([FromQuery] int? page, [FromQuery] int? size) { }

// [FromBody]   — from the JSON request body (POST/PUT)
// REQUIRES: Content-Type: application/json header
public IActionResult Create([FromBody] ItemRequest dto) { }
```

### `[ApiController]` — what it does automatically

1. **Model validation** — if `[Required]` or `[Range]` annotation fails, returns 400 automatically (no need to write `if (!ModelState.IsValid)`)
2. **Binding source inference** — infers `[FromBody]` for complex types in POST/PUT
3. **Problem Details** — structures 400/404 responses as RFC 7807 JSON

### Middleware pipeline

```csharp
// In Program.cs — ORDER MATTERS
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();  // 1st — wraps everything
app.UseHttpsRedirection();                         // 2nd
app.UseRouting();                                  // 3rd
app.UseAuthentication();                           // 4th — who are you?
app.UseAuthorization();                            // 5th — what can you do?
app.MapControllers();                              // 6th — route to controllers
```

**Custom middleware template:**

```csharp
public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            await _next(context);  // call the next piece of middleware
        }
        finally
        {
            sw.Stop();
            _logger.LogInformation("{Method} {Path} — {Ms}ms",
                context.Request.Method,
                context.Request.Path,
                sw.Elapsed.TotalMilliseconds);
        }
    }
}
```

> **EXAM TRAP — middleware is singleton.** Never inject a **Scoped** service into middleware constructor — it will live forever. Inject scoped services through `InvokeAsync` parameters instead (ASP.NET Core resolves them per-request there).

---

---

## 10. ADO.NET

### The three execute methods

```csharp
// ── ExecuteNonQueryAsync — INSERT, UPDATE, DELETE
// Returns: int (rows affected)
using var cmd = new SqlCommand("DELETE FROM Products WHERE Id = @Id", connection);
cmd.Parameters.AddWithValue("@Id", id);
int rowsAffected = await cmd.ExecuteNonQueryAsync();
bool deleted = rowsAffected > 0;

// ── ExecuteScalarAsync — single value (COUNT, MAX, SCOPE_IDENTITY)
// Returns: object? — must cast
using var cmd2 = new SqlCommand("SELECT COUNT(*) FROM Products", connection);
int count = (int)(await cmd2.ExecuteScalarAsync())!;

// Get new row ID after INSERT:
using var cmd3 = new SqlCommand(
    "INSERT INTO Products (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();",
    connection);
cmd3.Parameters.AddWithValue("@Name", name);
int newId = Convert.ToInt32(await cmd3.ExecuteScalarAsync());

// ── ExecuteReaderAsync — multiple rows, streamed row by row
// Connection must stay open for the entire read
using var cmd4 = new SqlCommand("SELECT Id, Name, Price FROM Products", connection);
using var reader = await cmd4.ExecuteReaderAsync();

while (await reader.ReadAsync())  // advance one row
{
    int    id2   = reader.GetInt32(reader.GetOrdinal("Id"));
    string name2 = reader.GetString(reader.GetOrdinal("Name"));
    // ALWAYS check nullable columns before reading:
    decimal? price = reader.IsDBNull(reader.GetOrdinal("Price"))
                         ? null
                         : reader.GetDecimal(reader.GetOrdinal("Price"));
}
```

### SQL Injection — the single most important security rule

```csharp
// ❌ NEVER DO THIS — SQL injection vulnerability
string query = $"SELECT * FROM Users WHERE Name = '{userName}'";
// If userName = "'; DROP TABLE Users; --"  → table is deleted!

// ✅ ALWAYS use parameters
using var cmd = new SqlCommand(
    "SELECT * FROM Users WHERE Name = @Name", connection);
cmd.Parameters.AddWithValue("@Name", userName);  // value is escaped automatically
```

### Null handling — every nullable column needs this check

```csharp
// When a column can be NULL in SQL, you MUST check IsDBNull before reading
// Calling GetString() on a NULL column throws InvalidCastException

// Pattern for nullable string:
string? email = reader.IsDBNull(reader.GetOrdinal("Email"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Email"));

// Pattern for nullable DateTime:
DateTime? returnDate = reader.IsDBNull(reader.GetOrdinal("return_date"))
                           ? null
                           : reader.GetDateTime(reader.GetOrdinal("return_date"));

// Passing C# null to SQL parameter — NEVER pass plain null:
cmd.Parameters.AddWithValue("@Email", (object?)user.Email ?? DBNull.Value);
```

### GetOrdinal — cache it outside the loop

```csharp
// ❌ SLOW — string lookup every row
while (await reader.ReadAsync())
{
    string name = reader.GetString(reader.GetOrdinal("Name"));  // lookup on every row
}

// ✅ FAST — cache ordinals before the loop
int nameOrd  = reader.GetOrdinal("Name");
int emailOrd = reader.GetOrdinal("Email");
int priceOrd = reader.GetOrdinal("Price");

while (await reader.ReadAsync())
{
    string  name  = reader.GetString(nameOrd);
    string? email = reader.IsDBNull(emailOrd) ? null : reader.GetString(emailOrd);
    decimal price = reader.GetDecimal(priceOrd);
}
```

### Connection management and pooling

```csharp
// ✅ CORRECT — using ensures Dispose() = return to pool
await using var connection = new SqlConnection(_connectionString);
await connection.OpenAsync();
// ... use connection ...
// connection.Dispose() is called automatically — returns connection to pool, NOT close TCP

// Connection string lives in appsettings.json, never hardcoded:
// { "ConnectionStrings": { "DefaultConnection": "Server=...;Database=...;" } }

// Retrieved via IConfiguration injection:
public class MyRepository
{
    private readonly string _cs;
    public MyRepository(IConfiguration config)
        => _cs = config.GetConnectionString("DefaultConnection")!;
}
```

> **EXAM TRAP — connection pooling:**
> - `OpenAsync()` = take connection **from pool** (not open new TCP)
> - `Dispose()` = return connection **to pool** (not close TCP)
> - Default max pool size = **100**
> - Exhausted pool throws `InvalidOperationException: Timeout expired`

### Transactions — the ACID pattern

```csharp
public async Task<int> CreateOrderWithItemsAsync(OrderDto dto)
{
    await using var connection = new SqlConnection(_connectionString);
    await connection.OpenAsync();

    // 1. Begin transaction — ALL commands must use this transaction object
    await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();

    try
    {
        // 2. Every SqlCommand MUST have transaction assigned — CRITICAL
        await using var cmd1 = new SqlCommand(
            "INSERT INTO Orders (CustomerId, Total) VALUES (@Cid, @Total); SELECT SCOPE_IDENTITY();",
            connection,
            transaction);   // ← 3rd constructor argument OR cmd.Transaction = transaction
        cmd1.Parameters.AddWithValue("@Cid",   dto.CustomerId);
        cmd1.Parameters.AddWithValue("@Total", dto.Total);
        int orderId = Convert.ToInt32(await cmd1.ExecuteScalarAsync());

        foreach (var item in dto.Items)
        {
            await using var cmd2 = new SqlCommand(
                "INSERT INTO OrderItems (OrderId, ProductId, Qty) VALUES (@Oid, @Pid, @Qty)",
                connection,
                transaction);
            cmd2.Parameters.AddWithValue("@Oid", orderId);
            cmd2.Parameters.AddWithValue("@Pid", item.ProductId);
            cmd2.Parameters.AddWithValue("@Qty", item.Quantity);
            await cmd2.ExecuteNonQueryAsync();
        }

        // 3. Commit — only if ALL steps succeeded
        await transaction.CommitAsync();
        return orderId;
    }
    catch
    {
        // 4. Rollback — undo EVERYTHING if anything failed
        await transaction.RollbackAsync();
        throw;  // rethrow — let the controller return the right status code
    }
}
```

> **EXAM TRAP:** If you create a `SqlCommand` after `BeginTransactionAsync()` and forget to assign `.Transaction = transaction`, you get:  
> `InvalidOperationException: ExecuteNonQuery requires the command to have a transaction...`

**ACID explained:**
| Letter | Meaning | One sentence |
|---|---|---|
| A | Atomicity | All operations succeed together, or none do |
| C | Consistency | Database moves from one valid state to another |
| I | Isolation | Concurrent transactions don't see each other's uncommitted data |
| D | Durability | Once committed, changes survive crashes |

**`SCOPE_IDENTITY()` vs `@@IDENTITY`:**
- `SCOPE_IDENTITY()` → last identity inserted **in this scope** (your INSERT)
- `@@IDENTITY` → last identity anywhere on this connection (can be from a trigger on another table)
- **Always use `SCOPE_IDENTITY()`**

---

---

## 11. 3-Layer Architecture

### Layer responsibilities — absolute rule

```
Controller   →  parses HTTP, returns status codes, NO SQL
Service      →  business rules, validation, NO SQL, NO HttpContext
Repository   →  executes SQL, maps rows to objects, NO business logic
```

### Complete working example

```csharp
// ── appsettings.json ─────────────────────────────────────────
/*
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyDb;User Id=sa;Password=Pass;TrustServerCertificate=True"
  }
}
*/

// ── Models ────────────────────────────────────────────────────
public class Product
{
    public int     Id       { get; set; }
    public string  Name     { get; set; } = string.Empty;
    public decimal Price    { get; set; }
    public int     Stock    { get; set; }
}

// ── DTOs ──────────────────────────────────────────────────────
public class ProductRequest
{
    [Required]
    [MaxLength(200)]
    public string  Name  { get; set; } = string.Empty;

    [Range(0.01, 99999.99)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int     Stock { get; set; }
}

// ── Repository Interface ──────────────────────────────────────
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?>             GetByIdAsync(int id);
    Task<int>                  AddAsync(Product product);     // returns new id
    Task<bool>                 UpdateAsync(Product product);
    Task<bool>                 DeleteAsync(int id);
}

// ── Repository Implementation ─────────────────────────────────
public class SqlProductRepository : IProductRepository
{
    private readonly string _cs;

    public SqlProductRepository(IConfiguration config)
        => _cs = config.GetConnectionString("DefaultConnection")!;

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var list = new List<Product>();
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();
        await using var cmd = new SqlCommand("SELECT Id, Name, Price, Stock FROM Products", conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        // Cache ordinals outside the loop
        int idOrd = reader.GetOrdinal("Id"), nameOrd = reader.GetOrdinal("Name"),
            priceOrd = reader.GetOrdinal("Price"), stockOrd = reader.GetOrdinal("Stock");

        while (await reader.ReadAsync())
            list.Add(new Product
            {
                Id    = reader.GetInt32(idOrd),
                Name  = reader.GetString(nameOrd),
                Price = reader.GetDecimal(priceOrd),
                Stock = reader.GetInt32(stockOrd),
            });

        return list;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(
            "SELECT Id, Name, Price, Stock FROM Products WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        await using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync()) return null;  // 404

        return new Product
        {
            Id    = reader.GetInt32(reader.GetOrdinal("Id")),
            Name  = reader.GetString(reader.GetOrdinal("Name")),
            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
            Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
        };
    }

    public async Task<int> AddAsync(Product product)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(
            "INSERT INTO Products (Name, Price, Stock) VALUES (@Name, @Price, @Stock); SELECT SCOPE_IDENTITY();",
            conn);
        cmd.Parameters.AddWithValue("@Name",  product.Name);
        cmd.Parameters.AddWithValue("@Price", product.Price);
        cmd.Parameters.AddWithValue("@Stock", product.Stock);
        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(
            "UPDATE Products SET Name=@Name, Price=@Price, Stock=@Stock WHERE Id=@Id", conn);
        cmd.Parameters.AddWithValue("@Id",    product.Id);
        cmd.Parameters.AddWithValue("@Name",  product.Name);
        cmd.Parameters.AddWithValue("@Price", product.Price);
        cmd.Parameters.AddWithValue("@Stock", product.Stock);
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();
        await using var cmd = new SqlCommand("DELETE FROM Products WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        return await cmd.ExecuteNonQueryAsync() > 0;
    }
}

// ── Service Interface ─────────────────────────────────────────
public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product>              GetByIdAsync(int id);   // throws NotFoundException
    Task<int>                  CreateAsync(ProductRequest dto);
    Task<Product>              UpdateAsync(int id, ProductRequest dto);
    Task                       DeleteAsync(int id);
}

// Custom exceptions — service layer uses these; controller catches them
public class NotFoundException  : Exception { public NotFoundException(string m)  : base(m) {} }
public class ConflictException  : Exception { public ConflictException(string m)   : base(m) {} }

// ── Service Implementation ────────────────────────────────────
public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    public ProductService(IProductRepository repo) => _repo = repo;

    public Task<IEnumerable<Product>> GetAllAsync() => _repo.GetAllAsync();

    public async Task<Product> GetByIdAsync(int id)
    {
        var p = await _repo.GetByIdAsync(id);
        return p ?? throw new NotFoundException($"Product {id} not found.");
    }

    public async Task<int> CreateAsync(ProductRequest dto)
    {
        // Business rules go here — e.g., check for duplicate names
        return await _repo.AddAsync(new Product
        {
            Name = dto.Name, Price = dto.Price, Stock = dto.Stock
        });
    }

    public async Task<Product> UpdateAsync(int id, ProductRequest dto)
    {
        var p = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Product {id} not found.");
        p.Name = dto.Name; p.Price = dto.Price; p.Stock = dto.Stock;
        await _repo.UpdateAsync(p);
        return p;
    }

    public async Task DeleteAsync(int id)
    {
        bool deleted = await _repo.DeleteAsync(id);
        if (!deleted) throw new NotFoundException($"Product {id} not found.");
    }
}

// ── Controller ────────────────────────────────────────────────
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    public ProductsController(IProductService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        try   { return Ok(await _service.GetByIdAsync(id)); }
        catch (NotFoundException) { return NotFound(); }
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] ProductRequest dto)
    {
        int id = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> Update(int id, [FromBody] ProductRequest dto)
    {
        try   { return Ok(await _service.UpdateAsync(id, dto)); }
        catch (NotFoundException) { return NotFound(); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try   { await _service.DeleteAsync(id); return NoContent(); }
        catch (NotFoundException) { return NotFound(); }
    }
}

// ── Program.cs ────────────────────────────────────────────────
/*
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<IProductRepository, SqlProductRepository>();  // ← repository
builder.Services.AddScoped<IProductService,    ProductService>();         // ← service
var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
*/
```

---

---

## 12. Exam Pattern

Every APBD exam follows **this exact structure**. Internalize it.

```
Database:  ParentTable ──── ChildTable ──── LineItemTable
                              ↓ FK             ↓ FK (composite PK)
                           StatusTable      SubjectTable

Endpoint 1 — GET /api/{parent}/{id}/{children}
  → 200: parent data + nested children list + nested items per child
  → 404: if parent not found

Endpoint 2 — POST /api/{parent}/{id}/{children}
  → 201: all in ONE transaction
  → 404: if parent or any referenced entity not found
```

### The GET pattern — LEFT JOIN + Dictionary

```csharp
public async Task<ParentDto?> GetParentWithChildrenAsync(int parentId)
{
    ParentDto? result = null;
    var childrenDict = new Dictionary<int, ChildDto>();

    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();

    const string sql = """
        SELECT
            p.parent_id, p.field1, p.field2,
            c.child_id, c.child_date, c.child_nullable_date,
            s.name   AS status_name,
            i.item_id, i.item_name, i.price_at_time
        FROM ParentTable p
        LEFT JOIN ChildTable c    ON c.parent_id = p.parent_id
        LEFT JOIN StatusTable s   ON s.status_id = c.status_id
        LEFT JOIN LineItemTable i ON i.child_id  = c.child_id
        LEFT JOIN SubjectTable sb ON sb.subject_id = i.subject_id
        WHERE p.parent_id = @ParentId
        ORDER BY c.child_id, i.item_name
        """;

    await using var cmd = new SqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@ParentId", parentId);
    await using var reader = await cmd.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        // Build parent object once (first row only)
        if (result is null)
        {
            result = new ParentDto
            {
                Field1 = reader.GetString(reader.GetOrdinal("field1")),
                Field2 = reader.GetString(reader.GetOrdinal("field2")),
            };
        }

        // LEFT JOIN may return NULL child_id if parent has no children
        int childIdOrd = reader.GetOrdinal("child_id");
        if (reader.IsDBNull(childIdOrd)) continue;

        int childId = reader.GetInt32(childIdOrd);
        if (!childrenDict.TryGetValue(childId, out var childDto))
        {
            childDto = new ChildDto
            {
                Id       = childId,
                Date     = reader.GetDateTime(reader.GetOrdinal("child_date")),
                NullDate = reader.IsDBNull(reader.GetOrdinal("child_nullable_date"))
                               ? null
                               : reader.GetDateTime(reader.GetOrdinal("child_nullable_date")),
                Status   = reader.GetString(reader.GetOrdinal("status_name")),
            };
            childrenDict[childId] = childDto;
            result.Children.Add(childDto);
        }

        // Add line item if present (can be NULL from LEFT JOIN)
        int itemIdOrd = reader.GetOrdinal("item_id");
        if (!reader.IsDBNull(itemIdOrd))
        {
            childDto.Items.Add(new ItemDto
            {
                Name  = reader.GetString(reader.GetOrdinal("item_name")),
                Price = reader.GetDecimal(reader.GetOrdinal("price_at_time")),
            });
        }
    }

    return result;  // null = parent not found → controller returns 404
}
```

### The POST pattern — transaction with validation

```csharp
public async Task<int> AddChildWithItemsAsync(int parentId, AddChildDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // Step 1: Verify parent exists
        await using var checkParent = new SqlCommand(
            "SELECT COUNT(1) FROM ParentTable WHERE parent_id = @Id", conn, tx);
        checkParent.Parameters.AddWithValue("@Id", parentId);
        if ((int)(await checkParent.ExecuteScalarAsync())! == 0)
            throw new KeyNotFoundException($"Parent {parentId} not found.");

        // Step 2: Resolve each item's lookup field → ID
        var itemIds = new List<(int Id, decimal Price)>();
        foreach (var item in dto.Items)
        {
            await using var findItem = new SqlCommand(
                "SELECT subject_id FROM SubjectTable WHERE name = @Name", conn, tx);
            findItem.Parameters.AddWithValue("@Name", item.Name);
            var r = await findItem.ExecuteScalarAsync();
            if (r is null) throw new KeyNotFoundException($"Subject '{item.Name}' not found.");
            itemIds.Add(((int)r, item.Price));
        }

        // Step 3: Get default status_id
        await using var getStatus = new SqlCommand(
            "SELECT status_id FROM StatusTable WHERE name = 'Active'", conn, tx);
        int statusId = (int)(await getStatus.ExecuteScalarAsync())!;

        // Step 4: INSERT header record
        await using var insertChild = new SqlCommand("""
            INSERT INTO ChildTable (child_date, return_date, parent_id, status_id)
            VALUES (@Date, NULL, @ParentId, @StatusId);
            SELECT SCOPE_IDENTITY();
            """, conn, tx);
        insertChild.Parameters.AddWithValue("@Date",     dto.Date);
        insertChild.Parameters.AddWithValue("@ParentId", parentId);
        insertChild.Parameters.AddWithValue("@StatusId", statusId);
        int newChildId = Convert.ToInt32(await insertChild.ExecuteScalarAsync());

        // Step 5: INSERT line items
        foreach (var (subjectId, price) in itemIds)
        {
            await using var insertItem = new SqlCommand("""
                INSERT INTO LineItemTable (child_id, subject_id, price_at_time)
                VALUES (@ChildId, @SubjectId, @Price)
                """, conn, tx);
            insertItem.Parameters.AddWithValue("@ChildId",   newChildId);
            insertItem.Parameters.AddWithValue("@SubjectId", subjectId);
            insertItem.Parameters.AddWithValue("@Price",     price);
            await insertItem.ExecuteNonQueryAsync();
        }

        await tx.CommitAsync();
        return newChildId;
    }
    catch
    {
        await tx.RollbackAsync();
        throw;
    }
}
```

### The Controller pattern — catch and translate

```csharp
[HttpGet("{id:int}/children")]
public async Task<IActionResult> Get(int id)
{
    var result = await _repo.GetParentWithChildrenAsync(id);
    return result is null ? NotFound(new { message = $"Parent {id} not found." }) : Ok(result);
}

[HttpPost("{id:int}/children")]
public async Task<IActionResult> Add(int id, [FromBody] AddChildDto dto)
{
    try
    {
        int newId = await _repo.AddChildWithItemsAsync(id, dto);
        return Created($"api/parents/{id}/children", new { id = newId });
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}
```

---

---

## 13. Exam Checklist

### Before touching any code (5 min)

```
□ Read BOTH endpoint descriptions fully before starting
□ Sketch the DB schema on paper — circle nullable columns
□ Identify: what column do you look up items by? (title? isbn? room_number?)
□ Identify: what is the nullable date column? (return_date, check_out, etc.)
□ Identify: what is the default status name for POST? ('Rented'? 'Active'? 'Scheduled'?)
□ Note any extra fields in the GET response (trainer_name, author, building_code?)
```

### Project setup (3 min)

```bash
dotnet new webapi -n MyProject --no-openapi
cd MyProject
dotnet add package Microsoft.Data.SqlClient

# First commit — BEFORE adding any code
echo "bin/\nobj/\n*.user\n.vs/" > .gitignore
git init
git add .gitignore
git commit -m "initial commit with gitignore"

# Remove generated boilerplate
rm WeatherForecast.cs Controllers/WeatherForecastController.cs
```

### Coding order (70 min)

```
□  1. DTOs — all of them first (5 min)
      ResponseDto, ChildDto, ItemInChildDto, AddRequestDto, AddItemDto

□  2. Repository interface — IXxxRepository (2 min)
      Task<ParentDto?> GetXxxAsync(int id)
      Task<int> AddXxxAsync(int id, AddRequestDto dto)

□  3. DI registration in Program.cs (1 min)
      builder.Services.AddScoped<IXxxRepository, SqlXxxRepository>()

□  4. GET repository method (20 min)
      — Single LEFT JOIN across all tables
      — Cache GetOrdinal outside while loop
      — IsDBNull on every nullable column
      — Dictionary to deduplicate parent rows
      — Return null if no rows → controller 404

□  5. POST repository method (25 min)
      — BeginTransactionAsync
      — Check parent exists (ExecuteScalar COUNT → throw KeyNotFoundException)
      — Resolve each item name → id (ExecuteScalar → throw if null)
      — Get default status_id (ExecuteScalar)
      — INSERT header → SELECT SCOPE_IDENTITY() (ExecuteScalar → new id)
      — INSERT each line item (ExecuteNonQuery in loop)
      — CommitAsync / catch → RollbackAsync → throw

□  6. Controller (8 min)
      — Constructor injection of IXxxRepository
      — GET: null check → 404 or Ok(result)
      — POST: try/catch KeyNotFoundException → 404, success → 201

□  7. Push to GitHub (2 min)
      git add .
      git commit -m "complete solution"
      git push
```

### Final verification (7 min)

```
□ Every database method is async Task<...> (non-async = up to -10 pts)
□ Every await has the await keyword
□ Controller never imports Microsoft.Data.SqlClient (SQL is repo's job)
□ Every SqlCommand inside a transaction has transaction assigned (constructor arg OR .Transaction = tx)
□ SCOPE_IDENTITY() used (not @@IDENTITY)
□ IsDBNull checked on every nullable column before GetString/GetDateTime/GetDecimal
□ Null values passed to SQL as: (object?)value ?? DBNull.Value
□ .gitignore is committed, no bin/ or obj/ in repo
□ DI registered: builder.Services.AddScoped<IRepo, SqlRepo>()
□ Code compiles: dotnet build (0 errors)
□ Pushed: git push
```

---

---

## 14. Flash Cards

**Read these the night before the exam. Cover the right column and test yourself.**

| Question | Answer |
|---|---|
| JIT compiles when? | At **runtime** (first method call) |
| AOT compiles when? | **Before** the app runs |
| `Open()` on SqlConnection does what? | Takes a connection from the **pool** (not open new TCP) |
| `Dispose()` on SqlConnection does what? | **Returns** connection to pool (not close TCP) |
| Default pool size? | **100** |
| Pool exhausted → what exception? | `InvalidOperationException: Timeout expired` |
| `ExecuteNonQuery` returns? | `int` — **rows affected** |
| `ExecuteScalar` returns? | `object?` — **first cell of first row** |
| `ExecuteReader` returns? | `SqlDataReader` — **streamed rows** |
| SQL injection fix? | **`cmd.Parameters.AddWithValue("@p", value)`** |
| C# null → SQL NULL? | `(object?)value ?? DBNull.Value` |
| Nullable column reader pattern? | `reader.IsDBNull(ord) ? null : reader.GetXxx(ord)` |
| `SCOPE_IDENTITY()` vs `@@IDENTITY`? | `SCOPE_IDENTITY` = this INSERT scope; `@@IDENTITY` = last on connection incl. triggers |
| Forgot to assign tx to command? | `InvalidOperationException: ExecuteNonQuery requires the command to have a transaction` |
| Transaction pattern? | `BeginTransaction → try → Commit → catch → Rollback → throw` |
| ACID — A? | **Atomicity** — all or nothing |
| ACID — C? | **Consistency** — valid state to valid state |
| ACID — I? | **Isolation** — concurrent txn don't see each other's uncommitted data |
| ACID — D? | **Durability** — committed data survives crashes |
| `override` vs `new`? | `override` = polymorphic (runtime type decides); `new` = hides (compile-time type decides) |
| Records use which equality? | **Value equality** (compare all fields) |
| `with` expression does what? | Creates a **new copy** with specific fields changed (original unchanged) |
| Deferred execution example? | `Where/Select/OrderBy` — query runs on `ToList/First/foreach` |
| Immediate LINQ operators? | `ToList`, `ToArray`, `Count`, `First`, `Any`, `All`, `Sum`, `Max` |
| `Join` vs `GroupJoin`? | `Join` = INNER (missing rows excluded); `GroupJoin` = LEFT (all left-side rows appear) |
| `Task.WhenAll` total time? | ≈ **max** of all tasks (concurrent) |
| Sequential `foreach await` total time? | **Sum** of all tasks |
| Closure trap? | Loop variable captured by reference → all lambdas see the final value |
| Fix for closure trap? | `int copy = i;` inside the loop — captures a per-iteration copy |
| `yield return` is? | **Lazy** — generates one element per iteration on demand |
| `StringBuilder` vs `string +=`? | `StringBuilder` O(1) amortised; `+=` in loop O(n²) |
| C# generics are? | **Reified** — `List<int>` ≠ `List<string>` at runtime |
| Java generics are? | **Type-erased** — `List<Integer>` == `List<String>` at runtime |
| SRP? | One class = **one reason to change** |
| OCP? | Open for **extension**, closed for **modification** |
| LSP? | Subclass must be usable in place of **base class without breaking callers** |
| ISP? | Many **small** interfaces > one fat interface |
| DIP? | Depend on **abstractions**, not concretions |
| Scoped in Singleton? | **Captive dependency** — memory leak, stale data. Never do it. |
| `[ApiController]` does what? | Auto-validates model, infers binding sources, returns 400 on invalid |
| `CreatedAtAction` generates? | 201 status + **Location header** pointing to the new resource |
| `Created(string.Empty, x)` problem? | 201 but **no Location header** — not standard REST |
| 400 vs 404? | 400 = **request is malformed**; 404 = **resource not found** |
| 401 vs 403? | 401 = not authenticated; 403 = authenticated but **not authorized** |
| 409 when? | Request is valid but **current state** prevents it (already booked, already on loan) |
| Middleware order? | Exception handler **first**, then routing, auth, authz, MapControllers **last** |
| Middleware can inject Scoped? | **Not in constructor** — use InvokeAsync parameter injection |
| `.gitignore` must contain? | `bin/`, `obj/` — missing this = **-50% of total score** |
| `git rebase --continue` vs `git commit`? | After resolving conflict in rebase: **`git rebase --continue`**, NOT `git commit` |
| `git reset --soft HEAD~1` vs `--hard`? | `--soft` keeps changes staged; `--hard` **destroys** changes |
| `git revert` vs `git reset`? | `revert` = new commit undoing changes (safe for public branches); `reset` = rewrites history (local only) |
| Layer responsibilities? | Controller = HTTP; Service = business rules; Repository = SQL only |
| `AddScoped` correct for? | Repositories and services in Web APIs |
| `AddSingleton` danger? | If it holds a Scoped dependency → captive dependency |
