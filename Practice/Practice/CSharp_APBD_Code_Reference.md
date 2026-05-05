# C# & APBD Complete Code Reference

> **Source:** APBD_CSharp_Complete_Study_Guide_Enhanced.pdf + CSharp_Mastery_Guide_Enhanced.pdf  
> **Purpose:** Every code pattern from your study materials, with output shown after each example so you know exactly what you get.

---

## Table of Contents

1. [Git & GitHub](#1-git--github)
2. [C# Fundamentals](#2-c-fundamentals)
3. [Object-Oriented C#](#3-object-oriented-c)
4. [SOLID Principles & IoC](#4-solid-principles--ioc)
5. [LINQ](#5-linq)
6. [Design Patterns](#6-design-patterns)
7. [ADO.NET — SQL Server](#7-adonet--sql-server)
8. [Common Pitfalls Cheat Sheet](#8-common-pitfalls-cheat-sheet)

---

## 1. Git & GitHub

### 1.1 First-Time Setup

```bash
git config --global user.name "Jan Kowalski"
git config --global user.email "s12345@pjwstk.edu.pl"
git config --global init.defaultBranch main
git config --global pull.rebase true   # cleaner history
git config --list                      # verify
```

### 1.2 The Three-Zone Lifecycle

```bash
# Working Directory → (git add) → Staging Area → (git commit) → Repository

git status                          # what changed?
git add Program.cs                  # stage one file
git add .                           # stage everything
git commit -m "Add Student model"   # permanent snapshot
git commit -am "Fix null check"     # stage tracked files + commit in one step
```

### 1.3 Inspect History

```bash
git log --oneline                           # compact: hash + message
git log --oneline --graph --all --decorate  # visual branch graph ← memorise this!
git show a1b2c3d                            # what changed in one commit
git diff                                    # unstaged changes
git diff --staged                           # staged changes (about to commit)
git blame -L 10,20 Program.cs              # who wrote lines 10-20
```

### 1.4 Branches

```bash
git switch -c feature/payment-system   # create + switch (modern syntax)
git branch -a                          # list all (* = current)
git switch main                        # return to main
git branch -d feature/payment-system   # delete merged branch
git branch -D feature/payment-system   # force-delete even if not merged
```

### 1.5 Merge vs Rebase

| Aspect | `git merge` | `git rebase` |
|--------|-------------|--------------|
| History | Non-linear — creates a merge commit | Linear — as if your branch always sat on top of main |
| When to use | Shared/public branches | Local, not-yet-pushed work |
| SHA rewrite? | No | Yes — commits get new SHAs |
| Conflicts | Resolve once at merge point | Resolve per replayed commit |

**Merge workflow:**
```bash
git switch main
git merge feature/login              # fast-forward if possible
git merge --no-ff feature/login      # force merge commit always
```

**Rebase workflow (linear history):**
```bash
git switch feature/rental-service
git rebase main                      # replay your commits on top of main
# If conflict: edit file → git add file.cs → git rebase --continue
git switch main
git merge --ff-only feature/rental-service   # safe fast-forward only
git branch -d feature/rental-service
git push
```

**After rebase — git log looks like this (straight line, no diamond):**
```
* b9e4c1f (HEAD→main) Add password hashing
* c3a7d2e Add login endpoint
* f2b8c4d Fix logout race condition
* 1a9d3e5 Add refresh token support
* 9c2a1b3 Initial auth setup
```

**After merge — git log looks like this (diamond shape):**
```
*   a3f9e1c (HEAD→main) Merge branch feature/login
|\
| * d4c2b8a Add password hashing
| * 7e1a3f2 Add login endpoint
* | f2b8c4d Fix logout race condition
* | 1a9d3e5 Add refresh token support
|/
* 9c2a1b3 Initial auth setup
```

> ⚠️ **NEVER rebase commits already pushed that others may have fetched.** If forced, use `git push --force-with-lease` (never `--force`).

### 1.6 Resolving Conflicts

```bash
# Conflict markers in your file look like this:
<<<<<<< HEAD
public decimal DailyRate() => 25m;   # your version
=======
public decimal DailyRate() => 30m;   # incoming version
>>>>>>> feature/pricing-update

# Steps:
# 1. Edit file to correct final state — remove ALL markers
git add ConflictedFile.cs
git commit              # finish merge
# OR
git rebase --continue   # finish rebase

# Bail out at any time:
git merge --abort
git rebase --abort
```

> ⚠️ After resolving: code must **compile and run** — removing markers is not enough.

### 1.7 Remote: push, pull, fetch, clone

```bash
git clone https://github.com/username/repo.git
git push -u origin main    # first push (sets upstream)
git push                   # subsequent pushes
git fetch origin           # download only, no merge
git pull                   # fetch + merge/rebase (depends on config)
git stash                  # temporarily save uncommitted work
git stash pop              # restore it
git stash list             # see all stashes
```

### 1.8 Fast-Forward vs 3-Way Merge

| Type | When | Merge commit? | Graph |
|------|------|---------------|-------|
| Fast-forward | main has NO new commits since branch was created | No | Straight line |
| 3-Way merge | Both branches diverged | Yes — two parents | Diamond shape |
| `--no-ff` | FF possible but you force a merge commit | Yes always | Diamond |
| `--ff-only` | Only merges if FF possible, errors if diverged | Never | Always straight |

---

## 2. C# Fundamentals

### 2.1 .NET Platform: CLR, JIT, AOT

```csharp
// using statement — auto-calls Dispose() when block exits, even on exception
using var conn = new SqlConnection(connectionString);
conn.Open();
// conn.Dispose() called here automatically

// Implementing IDisposable for unmanaged resources
public class FileProcessor : IDisposable
{
    private FileStream _stream;
    public FileProcessor(string path) => _stream = File.OpenRead(path);
    public void Dispose() => _stream?.Dispose();
}
```

> **Exam:** JIT compiles IL to machine code at **execution time**. AOT compiles IL **before** the application runs.

| Component | Role |
|-----------|------|
| Roslyn | C# compiler → produces .dll with IL bytecode |
| CLR | Runtime: executes IL, manages GC, type safety |
| JIT | Converts IL → native machine code at first call |
| AOT | Compiles ahead of time → faster startup, no JIT |
| BCL | Base Class Library — String, List, File, DateTime |

### 2.2 Value Types vs Reference Types

```csharp
// VALUE TYPE — copy is independent
int a = 10;
int b = a;   // independent copy
b = 20;
Console.WriteLine(a);   // 10 — unchanged

// REFERENCE TYPE — both point to same object on heap
int[] arr1 = { 1, 2, 3 };
int[] arr2 = arr1;    // SAME object
arr2[0] = 999;
Console.WriteLine(arr1[0]);  // 999 — both changed!

// STRUCT — value type, copied independently
public struct Point { public int X; public int Y; }
Point p1 = new Point { X = 1, Y = 2 };
Point p2 = p1;   // independent copy
p2.X = 99;
Console.WriteLine(p1.X);  // 1 — unchanged

// Floating-point trap (classic exam question!)
double d = 0.1 + 0.2;
Console.WriteLine(d == 0.3);   // FALSE — floating-point imprecision!
Console.WriteLine(d);          // 0.30000000000000004
// Fix: use decimal for money
decimal m = 0.1m + 0.2m;
Console.WriteLine(m == 0.3m);  // TRUE
```

**Output:**
```
10
999
1
False
0.30000000000000004
True
```

| Property | Value Type | Reference Type |
|----------|------------|----------------|
| Examples | `int, double, bool, char, struct, enum` | `class, string, array, delegate, interface` |
| Stored in | Stack (usually) | Heap |
| Assignment copies | The actual DATA | The REFERENCE (pointer) |
| Default value | `0 / false / '\0'` | `null` |
| Can be null? | No (unless `int?`) | Yes |

### 2.3 Properties — All Forms

```csharp
public class Student
{
    // Form 1: Auto-property — compiler generates backing field
    public string Name { get; set; }

    // Form 2: Read-only after construction (only settable in constructor)
    public int Id { get; }

    // Form 3: init-only (C# 9+) — settable in object initializer, never after
    public string Email { get; init; }

    // Form 4: Full property with validation
    private int _age;
    public int Age
    {
        get => _age;
        set
        {
            if (value < 0) throw new ArgumentException("Age cannot be negative");
            _age = value;
        }
    }

    // Form 5: Computed — no backing field, calculated each access
    public bool IsAdult => Age >= 18;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";

    public Student(int id) { Id = id; }
}

// Object initializer syntax:
var s = new Student(1)
{
    Name     = "Anna",
    Email    = "anna@pjwstk.edu.pl",  // init-only — can set here
    FirstName = "Anna",
    LastName  = "Kowalska"
};
// s.Email = "x";  // COMPILER ERROR — init-only property!
```

| | `set` | `init` (C# 9+) |
|--|-------|----------------|
| In constructor | ✓ | ✓ |
| In object initializer `{ }` | ✓ | ✓ |
| After construction | ✓ Any time | ✗ Compile error |

### 2.4 Constructors & Null Safety

```csharp
public class Course
{
    public string Title   { get; }
    public string Code    { get; }
    public int    Credits { get; }

    // Primary constructor
    public Course(string title, string code, int credits)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be blank");
        Title   = title;
        Code    = code;
        Credits = credits;
    }

    // Overloaded: default 3 credits — chains to main constructor
    public Course(string title, string code) : this(title, code, 3) { }
}

// Null safety operators
string? maybeNull = null;
string result = maybeNull ?? "default";      // ?? null-coalescing: use right if left is null
int? len = maybeNull?.Length;               // ?. null-conditional: returns null instead of throwing
string? name = null;
name ??= "Anonymous";                        // ??= null-coalescing assignment
string upper = maybeNull?.Trim()?.ToUpper() ?? "EMPTY";   // chaining
```

**Output:**
```
default
(null)
Anonymous
EMPTY
```

### 2.5 Strings & StringBuilder

```csharp
string s = " Hello, World! ";
Console.WriteLine(s.Trim());              // "Hello, World!"
Console.WriteLine(s.ToUpper());           // " HELLO, WORLD! "
Console.WriteLine(s.Contains("World"));  // True
Console.WriteLine(s.Replace("World", "C#"));  // " Hello, C#! "
Console.WriteLine(s.Split(',')[0].Trim()); // "Hello"
Console.WriteLine(s.IndexOf("World"));   // 9
Console.WriteLine(s.Substring(9, 5));    // "World"

// String interpolation (preferred over string.Format)
string name = "Anna"; int age = 22;
string msg = $"Hello {name}, you are {age} years old.";
Console.WriteLine(msg);   // Hello Anna, you are 22 years old.

// Verbatim string — no escape processing
string path = @"C:\Users\Anna\Documents";

// StringBuilder for loops — O(n) instead of O(n²)
var sb = new StringBuilder();
for (int i = 1; i <= 5; i++) sb.Append(i).Append(" ");
Console.WriteLine(sb.ToString());  // 1 2 3 4 5

// Null/empty check
Console.WriteLine(string.IsNullOrWhiteSpace(" "));   // True
Console.WriteLine(string.IsNullOrWhiteSpace(null));  // True
Console.WriteLine(string.IsNullOrWhiteSpace("hi"));  // False

// In C#, == on strings compares BY VALUE (unlike Java!)
string a = "hello";
string b = "hel" + "lo";
Console.WriteLine(a == b);  // True
```

### 2.6 Collections

| Collection | Access | Add | Contains | Best for |
|-----------|--------|-----|----------|----------|
| `List<T>` | O(1) by index | O(1) amortised | O(n) | Ordered, indexed data |
| `Dictionary<K,V>` | O(1) by key | O(1) | O(1) | Key→value lookup |
| `HashSet<T>` | N/A | O(1) | O(1) | Unique items, fast membership |
| `Queue<T>` | Peek O(1) | Enqueue O(1) | O(n) | FIFO |
| `Stack<T>` | Peek O(1) | Push O(1) | O(n) | LIFO |
| `SortedDictionary<K,V>` | O(log n) | O(log n) | O(log n) | Key→value sorted |

```csharp
// List<T>
var names = new List<string> { "Anna", "Marek", "Julia" };
names.Add("Tomasz");
names.Remove("Marek");
Console.WriteLine(names.Count);            // 3
Console.WriteLine(names.Contains("Anna")); // True
names.Sort();
Console.WriteLine(string.Join(", ", names)); // Anna, Julia, Tomasz

// Dictionary<K,V>
var scores = new Dictionary<string, int> { ["Anna"] = 95, ["Marek"] = 87 };
scores["Julia"] = 91;
scores["Anna"] = 99;   // overwrites
Console.WriteLine(scores["Anna"]);            // 99
Console.WriteLine(scores.ContainsKey("Eve")); // False
scores.TryGetValue("Eve", out int eveScore);  // eveScore = 0 (default)

// HashSet<T>
var cities = new HashSet<string> { "Warsaw", "Krakow", "Warsaw" };
Console.WriteLine(cities.Count);  // 2 — duplicate removed
```

**Output:**
```
3
True
Anna, Julia, Tomasz
99
False
2
```

### 2.7 Generics: C# vs Java

```csharp
// C# — type preserved at runtime (REIFICATION)
var list = new List<string>();
Console.WriteLine(list.GetType().Name);  // List`1[String] — type info kept!

// Generic class with constraints
public class Repository<T> where T : class
{
    private readonly List<T> _items = new();
    public void Add(T item) => _items.Add(item);
    public T? FindFirst(Func<T, bool> pred) => _items.FirstOrDefault(pred);
    public IEnumerable<T> GetAll() => _items.AsReadOnly();
}

// Generic method
public static T? FindFirst<T>(IEnumerable<T> source, Func<T, bool> pred)
    => source.FirstOrDefault(pred);

// Constraints
public class Service<T> where T : class, IEntity, new()
// T must be: reference type, implement IEntity, have parameterless constructor
```

| Feature | C# | Java |
|---------|----|----|
| Type info at runtime | ✓ Preserved (reification) | ✗ Erased |
| Value type generics | `List<int>` — no boxing | `List<Integer>` — boxes |
| Constraints | `where T : class, IFoo, new()` | `? super / ? extends` |

> **Exam:** C# uses **reification** — generic type info is preserved at runtime. Java uses **type erasure**.

### 2.8 Delegates, Action, Func, Predicate, Closures

```csharp
// Built-in generic delegates
Action<string>      print  = s => Console.WriteLine(s);
Func<int, int>      square = n => n * n;
Predicate<int>      isPos  = n => n > 0;

print("Hello");             // Hello
Console.WriteLine(square(5)); // 25
Console.WriteLine(isPos(-1)); // False

// Func — last type parameter is ALWAYS the return type
Func<int, int, string> add = (a, b) => $"{a}+{b}={a+b}";
Console.WriteLine(add(3, 4));  // 3+4=7
```

**Output:**
```
Hello
25
False
3+4=7
```

**CLOSURE TRAP — classic exam question:**
```csharp
// BAD — lambda captures variable by REFERENCE, not value
var actions = new List<Action>();
for (int i = 0; i < 3; i++)
    actions.Add(() => Console.Write(i + " "));  // captures i by reference
foreach (var a in actions) a();
// Output: 3 3 3   (NOT 0 1 2! When lambdas run, i == 3)

// FIX — capture a local copy inside the loop
var fixed_actions = new List<Action>();
for (int i = 0; i < 3; i++) {
    int copy = i;  // new variable per iteration
    fixed_actions.Add(() => Console.Write(copy + " "));
}
foreach (var a in fixed_actions) a();
// Output: 0 1 2
```

**Events — restricting delegate access:**
```csharp
public class Button
{
    public event Action? Clicked;   // from outside: only += and -= allowed
    public void Click()
    {
        Clicked?.Invoke();          // invoke only from inside the class
    }
}
var btn = new Button();
btn.Clicked += () => Console.WriteLine("Button clicked!");
// btn.Clicked = null;    // COMPILER ERROR
// btn.Clicked();         // COMPILER ERROR
btn.Click();              // Button clicked!
```

### 2.9 async/await and Task

```csharp
// Basic async method
public async Task<string> FetchDataAsync(string url)
{
    using var client = new HttpClient();
    // Thread RELEASED here while waiting for HTTP response
    string data = await client.GetStringAsync(url);
    return data.ToUpper();  // runs on resumed thread
}

// Run multiple tasks in PARALLEL
var task1 = FetchStudentsAsync();
var task2 = FetchCoursesAsync();
await Task.WhenAll(task1, task2);  // both run concurrently
var students = await task1;
var courses  = await task2;

// WRONG — deadlock risk in ASP.NET Core!
var bad = FetchDataAsync(url).Result;    // blocks thread

// CORRECT
var good = await FetchDataAsync(url);
```

> ⚠️ `.Result` blocks the current thread, but the async continuation needs that thread → **deadlock**. Always use `await`.

> ⚠️ `async void` is dangerous — exceptions are lost. Only acceptable for event handlers. Use `async Task` everywhere else.

### 2.10 Records (C# 9+)

```csharp
// Positional record — compiler auto-generates constructor, Equals, GetHashCode, Deconstruct, ToString
public record Student(int Id, string Name, string City, string Email);

var s1 = new Student(1, "Anna Kowalska", "Warsaw", "anna@uni.local");
Console.WriteLine(s1);
// Student { Id = 1, Name = Anna Kowalska, City = Warsaw, Email = anna@uni.local }

// 'with' expression — creates a COPY with one field changed; original unchanged
var s2 = s1 with { City = "Krakow" };
Console.WriteLine(s2.City);  // Krakow
Console.WriteLine(s1.City);  // Warsaw — original untouched

// Value equality — records compare BY VALUE not by reference
var s3 = new Student(1, "Anna Kowalska", "Warsaw", "anna@uni.local");
Console.WriteLine(s1 == s3);  // True — same values
Console.WriteLine(s1 == s2);  // False — different City

// Deconstruction
var (id, name, city, email) = s1;
Console.WriteLine($"{name} from {city}");  // Anna Kowalska from Warsaw
```

**Output:**
```
Student { Id = 1, Name = Anna Kowalska, City = Warsaw, Email = anna@uni.local }
Krakow
Warsaw
True
False
Anna Kowalska from Warsaw
```

> **Exam:** Records are **reference types** by default. Use `record struct` for value type. `with` creates a copy — original never mutated.

### 2.11 switch Expressions (C# 8+)

```csharp
// Modern switch expression
int day = 3;
string dayName = day switch
{
    1 => "Monday",
    2 => "Tuesday",
    3 => "Wednesday",
    _ => "Other"  // _ is the default/wildcard
};

// Pattern matching on types
object obj = 3.14;
string describe = obj switch
{
    int i    => $"Integer: {i}",
    double d => $"Double: {d}",
    string s => $"String: {s}",
    null     => "null",
    _        => "Unknown"
};
Console.WriteLine(describe);  // Double: 3.14

// Guards with 'when'
int score = 75;
string grade = score switch
{
    >= 90 => "A",
    >= 75 => "B",
    >= 60 => "C",
    _     => "F"
};
Console.WriteLine(grade);  // B
```

**Output:**
```
Double: 3.14
B
```

> **Exam:** C# **prohibits fall-through** between non-empty case blocks. Java allows it (only `break` prevents it).

### 2.12 ref, out, Named & Optional Parameters

```csharp
// ref — pass by reference; caller MUST initialise before calling
void AddTen(ref int x) => x += 10;
int n = 5;
AddTen(ref n);
Console.WriteLine(n);  // 15 — modified in place

// out — caller does NOT need to initialise; method MUST assign before returning
bool TryParseAge(string input, out int age)
{
    return int.TryParse(input, out age);
}
if (TryParseAge("25", out int parsedAge))
    Console.WriteLine($"Age: {parsedAge}");  // Age: 25

// Named parameters — call in any order
void CreateOrder(string customer, int quantity, decimal price)
    => Console.WriteLine($"{customer}: {quantity} x {price:C}");
CreateOrder(price: 9.99m, customer: "Anna", quantity: 3);
// Anna: 3 x $9.99

// Optional (default) parameters
void Greet(string name, string greeting = "Hello", bool formal = false)
    => Console.WriteLine($"{(formal ? greeting + "," : greeting)} {name}!");
Greet("Anna");                        // Hello Anna!
Greet("Anna", "Good morning", true);  // Good morning, Anna!
```

**Output:**
```
15
Age: 25
Anna: 3 x $9.99
Hello Anna!
Good morning, Anna!
```

### 2.13 yield return — Lazy Sequences

```csharp
// yield return — generates elements lazily (nothing runs until iterated)
public IEnumerable<int> GetEvenNumbers(int max)
{
    for (int i = 0; i <= max; i += 2)
        yield return i;  // pauses here, resumes on next iteration
}
foreach (var n in GetEvenNumbers(10))
    Console.Write(n + " ");  // 0 2 4 6 8 10

// Real-world: read a huge CSV line-by-line — only one line in memory at a time
public IEnumerable<string> ReadLines(string path)
{
    using var reader = new StreamReader(path);
    string? line;
    while ((line = reader.ReadLine()) != null)
        yield return line;
}
```

---

## 3. Object-Oriented C#

### 3.1 Abstract Classes & Virtual/Override

```csharp
// virtual + override = polymorphism (correct method chosen by actual type, not variable type)
public abstract class Equipment
{
    public string Name        { get; set; }
    public bool   IsAvailable { get; set; } = true;

    // abstract = subclass MUST implement
    public abstract decimal DailyRate();

    // virtual = subclass CAN override
    public virtual string Describe() => $"{Name} — {DailyRate():C}/day";
}

public class Laptop : Equipment
{
    public string Processor { get; set; }
    public int    RamGb     { get; set; }
    public Laptop(string name, string proc, int ram) { Name = name; Processor = proc; RamGb = ram; }
    public override decimal DailyRate()  => 25m;
    public override string  Describe()   => base.Describe() + $" | RAM: {RamGb}GB";
}

// 'new' keyword HIDES the method — NOT polymorphism!
public class SpecialLaptop : Laptop
{
    public SpecialLaptop(string n) : base(n, "i9", 32) {}
    public new decimal DailyRate() => 30m;  // hides, does NOT override
}

Equipment e = new SpecialLaptop { Name = "Pro" };
Console.WriteLine(e.DailyRate());  // $25.00 — calls Laptop's version, NOT SpecialLaptop's!
```

**Output:**
```
$25.00
```

| Keyword | Effect | Polymorphism? |
|---------|--------|---------------|
| `virtual` | Method can be overridden | Yes, if overridden |
| `abstract` | No body; subclass must override | Yes, always |
| `override` | Replaces virtual/abstract properly | Yes — polymorphic dispatch |
| `new` (hide) | Hides base method; base pointer calls base version | **NO** |
| `sealed` | Prevents further overriding | N/A |

> **Exam:** `Animal a = new Dog(); a.Eat()` where Dog uses `new` (hiding) → calls **Animal's** Eat(). Only `override` enables polymorphism.

### 3.2 Interfaces

```csharp
// Convention: interfaces start with 'I'
public interface IRentalService
{
    bool Rent(int userId, int equipmentId);
    bool Return(int rentalId);
    IEnumerable<Rental> GetActiveRentals(int userId);
}

public class RentalService : IRentalService
{
    private readonly List<Rental>    _rentals    = new();
    private readonly List<Equipment> _equipment  = new();

    public bool Rent(int userId, int equipmentId)
    {
        var item = _equipment.FirstOrDefault(e => e.Id == equipmentId);
        if (item == null || !item.IsAvailable) return false;
        item.IsAvailable = false;
        _rentals.Add(new Rental(userId, equipmentId, DateTime.Now, 7));
        return true;
    }
    public bool Return(int rentalId) { return true; }
    public IEnumerable<Rental> GetActiveRentals(int userId)
        => _rentals.Where(r => r.UserId == userId && r.IsActive);
}

// A class can implement MULTIPLE interfaces:
public class NotificationHub : IEmailSender, ISmsSender, IPushSender { }
```

> **Exam:** A class can implement multiple interfaces ✓. Interfaces cannot have backing fields ✗. Interfaces cannot have constructors ✗. Default implementations allowed since C# 8 ✓.

### 3.3 Inheritance: C# vs Java

| Feature | C# | Java |
|---------|----|----|
| Extend class | `class Child : Parent` | `class Child extends Parent` |
| Implement interface | `class Child : IFoo` | `class Child implements IFoo` |
| Both | `class Child : Parent, IFoo` | `class Child extends P implements IFoo` |
| Virtual methods | Must be marked `virtual` explicitly | All non-static/non-final are virtual |
| Override | Must use `override` keyword | `@Override` annotation (optional) |
| Method hiding | `new` keyword — no polymorphism | Not applicable |
| Prevent inheritance | `sealed` | `final` |

### 3.4 Custom Exceptions & Rethrow

```csharp
// Custom domain exception
public class RentalLimitExceededException : Exception
{
    public int UserId { get; }
    public RentalLimitExceededException(int userId)
        : base($"User {userId} has reached the maximum rental limit")
        => UserId = userId;
}

// Usage
try
{
    service.Rent(userId, equipmentId);
}
catch (RentalLimitExceededException ex)
{
    Console.WriteLine($"Cannot rent: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());  // log FULL stack trace — not just Message
    throw;         // re-throw preserving original stack trace
    // NEVER: throw ex;  — that resets the stack trace to this line!
}
finally
{
    // runs always — good for cleanup
}
```

---

## 4. SOLID Principles & IoC

### 4.1 Cohesion Types (best to worst)

| Type | Description | Violation sign |
|------|-------------|----------------|
| Functional (best) | Class does ONE thing | Class name contains "and" |
| Sequential | Output of one method feeds next | Should split into pipeline stages |
| Communicational | Different ops on same data | OK, check if split makes sense |
| Procedural | Grouped because they run in sequence | Consider split by responsibility |
| Temporal | Grouped because "we run at startup" | `ApplicationInitializer` anti-pattern |
| Logical | Methods do same kind of thing via flag | Control coupling + fat method |
| Coincidental (worst) | No common denominator — grab-bag | "Helper", "Utils", "Manager" classes |

### 4.2 Coupling Types (worst to best)

| Type | Description | Fix |
|------|-------------|-----|
| Content (worst) | Directly manipulates another class's fields | Use methods/properties |
| Common | Shared global/static mutable state | Inject via constructor |
| Control | Pass a flag telling HOW to behave | Strategy pattern |
| Stamp | Pass more data than needed | Pass only required params |
| Data (best) | Communicate through simple parameters only | Keep this — it's the goal |

### 4.3 SRP — Single Responsibility Principle

```csharp
// VIOLATION — OrderService has 4 reasons to change (4 actors)
public class OrderService_BAD
{
    public bool    ValidateOrder(Order o)    { } // actor: business analyst
    public decimal CalculateDiscount(Order o){ } // actor: product owner
    public void    SaveOrder(Order o)        { } // actor: DBA
    public void    SendEmail(Order o)        { } // actor: UX/comms team
}

// FIX — split by actor
public interface IOrderValidator     { bool Validate(Order o); }
public interface IOrderRepository    { void Save(Order o); }
public interface INotificationService { void SendConfirmation(Order o); }

public class OrderService  // now only handles orchestration
{
    private readonly IOrderValidator      _validator;
    private readonly IOrderRepository     _repo;
    private readonly INotificationService _notify;

    public OrderService(IOrderValidator v, IOrderRepository r, INotificationService n)
    { _validator = v; _repo = r; _notify = n; }

    public void ProcessOrder(Order order)
    {
        if (!_validator.Validate(order))
            throw new InvalidOperationException("Invalid");
        _repo.Save(order);
        _notify.SendConfirmation(order);
    }
}
```

> **Exam:** SRP = "one reason to change" = serves a single stakeholder/actor. NOT "one method per class" or "max 100 lines".

### 4.4 OCP — Open/Closed Principle

```csharp
// VIOLATION — every new customer type requires modifying this method
public decimal CalculateDiscount_BAD(string type, decimal amount)
{
    if (type == "Regular") return amount * 0.05m;
    if (type == "Premium") return amount * 0.10m;
    if (type == "VIP")     return amount * 0.20m;
    // Adding 'Corporate' means OPENING this method again
    return 0m;
}

// FIX — Strategy pattern
public interface IDiscountStrategy { decimal CalculateDiscount(decimal amount); }
public class RegularDiscount : IDiscountStrategy
    { public decimal CalculateDiscount(decimal a) => a * 0.05m; }
public class PremiumDiscount : IDiscountStrategy
    { public decimal CalculateDiscount(decimal a) => a * 0.10m; }
public class VipDiscount     : IDiscountStrategy
    { public decimal CalculateDiscount(decimal a) => a * 0.20m; }

// DiscountCalculator NEVER changes when new types are added
public class DiscountCalculator
{
    private readonly IDiscountStrategy _strategy;
    public DiscountCalculator(IDiscountStrategy s) => _strategy = s;
    public decimal CalculateDiscount(decimal amount) => _strategy.CalculateDiscount(amount);
}

// New type: just add a new class — NOTHING modified
public class CorporateDiscount : IDiscountStrategy
    { public decimal CalculateDiscount(decimal a) => a * 0.15m; }
```

> **Exam:** Main OCP violation smell = a growing `if/else` or `switch` on object type that requires modification whenever a new variant is added.

### 4.5 LSP — Liskov Substitution Principle

```csharp
// CLASSIC VIOLATION: Square inherits Rectangle — breaks Area invariant
public class Rectangle
{
    public virtual double Width  { get; set; }
    public virtual double Height { get; set; }
    public double GetArea() => Width * Height;
}
public class Square : Rectangle
{
    public override double Width  { set { base.Width  = value; base.Height = value; } }
    public override double Height { set { base.Height = value; base.Width  = value; } }
}

void ResizeAndCheck(Rectangle r)
{
    r.Width = 5; r.Height = 10;   // caller expects Area = 50
    Console.WriteLine(r.GetArea()); // Rectangle: 50, Square: 100 ← violation!
}

// FIX — don't inherit; use a shared interface instead
public interface IShape { double GetArea(); }
public class Rectangle : IShape { ... }  // own contract
public class Square    : IShape { ... }  // own contract, no side effects
```

| LSP Rule | Meaning |
|----------|---------|
| Cannot strengthen preconditions | Subclass cannot require MORE from callers than base |
| Cannot weaken postconditions | Subclass cannot promise LESS than base |
| Must preserve invariants | Subclass cannot break class-level invariants |

### 4.6 ISP — Interface Segregation Principle

```csharp
// VIOLATION — fat interface forces read-only repos to implement ExportToPdf
public interface IStudentRepository_BAD
{
    Task<Student?> GetByIdAsync(int id);
    Task AddAsync(Student s);
    Task DeleteAsync(int id);
    Task<byte[]> ExportToPdfAsync();  // read-only repos must throw NotImplementedException!
}

// FIX — role-based, segregated interfaces
public interface IStudentReader
{
    Task<Student?> GetByIdAsync(int id);
    Task<IEnumerable<Student>> GetAllAsync();
}
public interface IStudentWriter
{
    Task AddAsync(Student s);
    Task DeleteAsync(int id);
}
public interface IStudentExporter
{
    Task<byte[]> ExportToCsvAsync();
}

// Read-only implementation: ONLY implements IStudentReader — no NotImplementedException!
public class ReadOnlyStudentRepository : IStudentReader
{
    public Task<Student?> GetByIdAsync(int id)             => /* real logic */;
    public Task<IEnumerable<Student>> GetAllAsync()        => /* real logic */;
}
```

> **Exam:** ISP violation sign = classes must throw `NotImplementedException` or leave methods empty.

### 4.7 DIP — Dependency Inversion Principle

```csharp
// Step 1: Define abstraction in the HIGH-LEVEL layer
public interface IOrderRepository
{
    void Save(Order order);
    Order? GetById(int id);
}

// Step 2: Low-level detail IMPLEMENTS abstraction
public class SqlOrderRepository : IOrderRepository
{
    public void Save(Order order)    { /* SQL INSERT */ }
    public Order? GetById(int id)    { /* SQL SELECT */ return null; }
}

// Step 3: High-level depends ONLY on interface
public class OrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;
    public void PlaceOrder(Order order) => _repo.Save(order);
}

// Swap implementations without touching OrderService:
var prod = new OrderService(new SqlOrderRepository());
var test = new OrderService(new FakeOrderRepository());  // no DB needed!
```

> **Exam:** The "inversion" = the LOWER-LEVEL module implements an interface defined by the HIGHER-LEVEL module. Dependency arrow is reversed.

### 4.8 IoC Container & DI Lifetimes

```csharp
// ASP.NET Core Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IEmailService, SmtpEmailService>();
// Transient: new instance EVERY time it is requested

builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();
// Scoped: one instance PER HTTP REQUEST

builder.Services.AddSingleton<ICacheService, InMemoryCacheService>();
// Singleton: ONE instance for entire app lifetime — MUST be thread-safe

builder.Services.AddScoped<OrderService>();  // auto-resolves constructor deps
```

| Lifetime | Created when | Lives until | Use for |
|---------|--------------|-------------|---------|
| `AddTransient` | Every injection | End of injection scope | Stateless, lightweight — EmailSender |
| `AddScoped` | Every HTTP request | End of HTTP request | Most services, repositories |
| `AddSingleton` | First request / startup | App shutdown | Thread-safe caches, config |

> ⚠️ **NEVER inject Scoped into Singleton** — the Singleton captures the first request's scoped instance and reuses it forever.

### 4.9 Service Locator Anti-Pattern

```csharp
// BAD — Service Locator HIDES actual dependencies
public class BadService
{
    private readonly IServiceProvider _provider;
    public BadService(IServiceProvider provider) => _provider = provider;
    public void DoWork()
    {
        var repo = _provider.GetRequiredService<IOrderRepository>(); // hidden dep!
        repo.Save(new Order());
    }
}

// GOOD — explicit constructor injection
public class GoodService
{
    private readonly IOrderRepository _repo;
    public GoodService(IOrderRepository repo) => _repo = repo;
    public void DoWork() => _repo.Save(new Order());
}
```

---

## 5. LINQ

### 5.1 Mental Model & Deferred Execution

```csharp
// DEFERRED EXECUTION TRAP — classic exam question!
var nums = new List<int> { 1, 2, 3 };
var query = nums.Where(n => n > 1);  // recipe built — NOTHING runs yet
nums.Add(4);                          // modify source AFTER defining query
foreach (var n in query) Console.Write(n + " ");
// Output: 2 3 4   (NOT 2 3 — includes 4 because query runs NOW)

// FIX: materialise with ToList() to get a snapshot
var snapshot = nums.Where(n => n > 1).ToList();  // runs NOW
nums.Add(99);
Console.WriteLine(string.Join(" ", snapshot));  // 2 3 4 (99 excluded)
```

**Output:**
```
2 3 4
2 3 4
```

> **Exam:** Deferred operators: `Where`, `Select`, `OrderBy`. Terminal operators (trigger execution): `ToList`, `Count`, `First`, `Any`, `All`, `Average`.

### 5.2 LINQ Operator Quick Reference

| Operator | SQL equiv | Deferred? | Key notes |
|----------|-----------|-----------|-----------|
| `.Where(pred)` | WHERE | ✓ | Filter; always before `.Select()` |
| `.Select(proj)` | SELECT | ✓ | Project/transform each element |
| `.SelectMany(col)` | flatMap/JOIN | ✓ | Flatten nested collections — Java's `flatMap()` |
| `.OrderBy(key)` | ORDER BY ASC | ✓ | Returns `IOrderedEnumerable` |
| `.ThenBy(key)` | secondary ORDER BY | ✓ | **NEVER use second `.OrderBy()`!** |
| `.GroupBy(key)` | GROUP BY | ✓ | Returns `IGrouping<K,T>` |
| `.Join(...)` | INNER JOIN | ✓ | Match keys from two collections |
| `.GroupJoin(...)` | LEFT OUTER JOIN | ✓ | Keep all from left side |
| `.Any(pred)` | EXISTS | ✗ Terminal | Stops at first match — prefer over `Count()>0` |
| `.All(pred)` | ALL | ✗ Terminal | Returns **true on empty collection!** |
| `.FirstOrDefault(pred)` | SELECT TOP 1 | ✗ Terminal | Safe — returns null if nothing matches |
| `.Single(pred)` | (expect exactly 1) | ✗ Terminal | Throws if 0 or 2+ matches |
| `.Distinct()` | SELECT DISTINCT | ✓ | Remove duplicates |
| `.Skip(n).Take(n)` | OFFSET/FETCH | ✓ | Paging pattern |
| `.Average/.Sum/.Max/.Min` | AVG/SUM/MAX/MIN | ✗ Terminal | Use `.HasValue` for nullable |
| `.ToList()` | (materialise) | ✗ Terminal | Triggers deferred execution, snapshot |
| `.ToHashSet()` | (materialise) | ✗ Terminal | O(1) Contains — use for inner joins |

### 5.3 Where (Filter)

```csharp
// Filter Warsaw students
var warsawStudents = students
    .Where(s => s.City == "Warsaw")
    .ToList();
// Anna, Tomasz, Lena

// Multiple conditions (AND)
var activeWithGrade = enrollments
    .Where(e => e.IsActive && e.FinalGrade.HasValue && e.FinalGrade >= 4.0);
```

### 5.4 Select (Projection)

```csharp
// Extract a single field
var emails = students.Select(s => s.Email);

// Project to formatted string
var previews = students
    .Select(s => $"{s.IndexNumber} | {s.FirstName} {s.LastName} | {s.City}");

// Project to anonymous type
var partials = students
    .Select(s => new { s.IndexNumber, FullName = $"{s.FirstName} {s.LastName}" });

// Select with index
var indexed = students
    .Select((s, i) => $"{i+1}. {s.FirstName} {s.LastName}");
```

### 5.5 OrderBy / ThenBy (Sorting)

```csharp
// Single sort
var sorted = students.OrderBy(s => s.LastName);

// Multi-level — ALWAYS use ThenBy, NEVER a second OrderBy!
var multiSort = students
    .OrderBy(s => s.City)
    .ThenBy(s => s.LastName)    // secondary — CORRECT
    .ThenBy(s => s.FirstName);  // tertiary

// WRONG — second OrderBy REPLACES the first!
var wrong = students
    .OrderBy(s => s.City)      // ignored!
    .OrderBy(s => s.LastName); // only this applies

// Descending
var topGrades = enrollments
    .Where(e => e.FinalGrade.HasValue)
    .OrderByDescending(e => e.FinalGrade);
```

### 5.6 First / FirstOrDefault / Single / SingleOrDefault

```csharp
// SAFE pattern — use FirstOrDefault with null check
var student = students.FirstOrDefault(s => s.Id == 999);
Console.WriteLine(student?.FirstName ?? "Not found");  // Not found

// Single when you expect EXACTLY one result
var course = courses.SingleOrDefault(c => c.Title == "LINQ Foundations");
// throws if two courses had the same title
```

| Method | 0 matches | 1 match | 2+ matches |
|--------|-----------|---------|-----------|
| `First()` | Throws | Returns it | Returns first |
| `FirstOrDefault()` | Returns null/default | Returns it | Returns first |
| `Single()` | Throws | Returns it | Throws |
| `SingleOrDefault()` | Returns null/default | Returns it | Throws |

### 5.7 Any / All

```csharp
bool hasInactive = enrollments.Any(e => !e.IsActive);   // True
bool allHaveDept = lecturers.All(l => !string.IsNullOrEmpty(l.Department)); // True

// Performance: Any() vs Count()
// Any() stops at FIRST match — O(1) best case
// Count() > 0 scans ENTIRE collection — always O(n)
if (students.Any(s => s.City == "Warsaw"))   // CORRECT

// Vacuous truth: All() on an EMPTY collection returns true!
Console.WriteLine(new List<int>().All(n => n > 0));  // True
```

> **Exam:** `All()` on **empty collection returns TRUE** (vacuously true). Always prefer `Any()` over `Count() > 0`.

### 5.8 Aggregation

```csharp
int total  = students.Count();
int warsaw = students.Count(s => s.City == "Warsaw");

// Average with null handling
double avg = enrollments
    .Where(e => e.FinalGrade.HasValue)
    .Average(e => e.FinalGrade!.Value);

double max = enrollments
    .Where(e => e.FinalGrade.HasValue)
    .Max(e => e.FinalGrade!.Value);

int totalEcts = courses.Sum(c => c.Ects);
```

### 5.9 Distinct / Skip / Take (Paging)

```csharp
// Distinct cities — sorted
var cities = students
    .Select(s => s.City)
    .Distinct()
    .OrderBy(c => c)
    .ToList();
// Gdansk, Krakow, Lublin, Poznan, Warsaw, Wroclaw

// Paging — page 2, page size 2
int page = 2, pageSize = 2;
var paged = courses
    .OrderBy(c => c.Title)       // ALWAYS sort before paging
    .Skip((page - 1) * pageSize) // skip first page
    .Take(pageSize)              // take current page
    .Select(c => $"{c.Title} | {c.Category}");
```

### 5.10 Join (Inner Join)

```csharp
// Two-way join: enrollments + students
var joined = enrollments
    .Join(
        students,
        e => e.StudentId,  // key from enrollments
        s => s.Id,         // key from students
        (e, s) => new { s.FirstName, s.LastName, e.CourseId, e.FinalGrade }
    );

// Three-way join: enrollment + student + course
var report = enrollments
    .Join(students,  e => e.StudentId, s => s.Id, (e, s) => new { e, s })
    .Join(courses,  es => es.e.CourseId, c => c.Id,
        (es, c) => new {
            Student = $"{es.s.FirstName} {es.s.LastName}",
            Course  = c.Title,
            Grade   = es.e.FinalGrade
        })
    .Where(x => x.Grade.HasValue)
    .OrderByDescending(x => x.Grade);
```

### 5.11 SelectMany (flatMap)

```csharp
// SelectMany flattens nested collections — equivalent to Java's flatMap()
var studentCourses = students
    .SelectMany(
        s => enrollments.Where(e => e.StudentId == s.Id),
        (s, e) => new { StudentName = $"{s.FirstName} {s.LastName}", e.CourseId }
    );

// If Student had List<string> Hobbies:
var allHobbies = students.SelectMany(s => s.Hobbies);
```

> **Exam:** `SelectMany` = LINQ equivalent of `flatMap()` in Java Stream API.

### 5.12 GroupBy & GroupJoin

```csharp
// GroupBy — students per city
var byCity = students
    .GroupBy(s => s.City)
    .Select(g => new
    {
        City  = g.Key,
        Count = g.Count(),
        Names = string.Join(", ", g.Select(s => $"{s.FirstName} {s.LastName}"))
    });
// Warsaw: 3 — Anna Kowalska, Tomasz Lis, Lena Adamska

// GroupJoin — LEFT OUTER JOIN equivalent
// All lecturers + their course counts (including those with 0 courses)
var lecturersWithCounts = lecturers
    .GroupJoin(
        courses,
        l => l.Id,
        c => c.LecturerId,
        (l, cs) => new { Name = $"{l.FirstName} {l.LastName}", Count = cs.Count() }
    )
    .OrderByDescending(x => x.Count);
```

### 5.13 Complete LINQ Lab Solutions (Tasks 1–16)

```csharp
// Task01: Students from Warsaw
public IEnumerable<string> Task01_StudentsFromWarsaw()
    => students
        .Where(s => s.City == "Warsaw")
        .Select(s => $"{s.IndexNumber} | {s.FirstName} {s.LastName} | {s.City}");
// s30101 | Anna Kowalska | Warsaw
// s30104 | Tomasz Lis | Warsaw
// s30107 | Lena Adamska | Warsaw

// Task02: Email addresses
public IEnumerable<string> Task02_StudentEmailAddresses()
    => students.Select(s => s.Email);

// Task03: Alphabetically by last then first name
public IEnumerable<string> Task03_StudentsSortedAlphabetically()
    => students
        .OrderBy(s => s.LastName)
        .ThenBy(s => s.FirstName)
        .Select(s => $"{s.IndexNumber} | {s.FirstName} {s.LastName}");

// Task04: First Analytics course (safe with null check)
public IEnumerable<string> Task04_FirstAnalyticsCourse()
{
    var course = courses.FirstOrDefault(c => c.Category == "Analytics");
    yield return course is null
        ? "No Analytics course found"
        : $"{course.Title} | Start: {course.StartDate:yyyy-MM-dd}";
}

// Task05: Any inactive enrollment?
public IEnumerable<string> Task05_IsThereAnyInactiveEnrollment()
{
    bool hasInactive = enrollments.Any(e => !e.IsActive);
    yield return $"Has inactive enrollment: {hasInactive}";
}

// Task06: All lecturers have department?
public IEnumerable<string> Task06_DoAllLecturersHaveDepartment()
{
    bool allHave = lecturers.All(l => !string.IsNullOrEmpty(l.Department));
    yield return $"All lecturers have department: {allHave}";
}

// Task07: Count active enrollments
public IEnumerable<string> Task07_CountActiveEnrollments()
{
    int count = enrollments.Count(e => e.IsActive);
    yield return $"Active enrollments: {count}";
}

// Task08: Distinct cities sorted
public IEnumerable<string> Task08_DistinctStudentCities()
    => students.Select(s => s.City).Distinct().OrderBy(c => c);
// Gdansk, Krakow, Lublin, Poznan, Warsaw, Wroclaw

// Task09: Three newest enrollments
public IEnumerable<string> Task09_ThreeNewestEnrollments()
    => enrollments
        .OrderByDescending(e => e.EnrollmentDate)
        .Take(3)
        .Select(e => $"{e.EnrollmentDate:yyyy-MM-dd} | Student {e.StudentId} | Course {e.CourseId}");

// Task10: Second page of courses (pageSize=2)
public IEnumerable<string> Task10_SecondPageOfCourses()
    => courses
        .OrderBy(c => c.Title)
        .Skip(2)      // (page-1)*pageSize = (2-1)*2 = 2
        .Take(2)
        .Select(c => $"{c.Title} | {c.Category}");

// Task11: Join students with enrollments
public IEnumerable<string> Task11_JoinStudentsWithEnrollments()
    => enrollments
        .Join(students, e => e.StudentId, s => s.Id,
            (e, s) => $"{s.FirstName} {s.LastName} | {e.EnrollmentDate:yyyy-MM-dd}");

// Task12: All student-course pairs (3-way join)
public IEnumerable<string> Task12_StudentCoursePairs()
    => enrollments
        .Join(students, e => e.StudentId, s => s.Id, (e, s) => new { e, s })
        .Join(courses, es => es.e.CourseId, c => c.Id,
            (es, c) => $"{es.s.FirstName} {es.s.LastName} | {c.Title}");

// Task13: Group enrollments by course (with course title)
public IEnumerable<string> Task13_GroupEnrollmentsByCourse()
    => enrollments
        .GroupBy(e => e.CourseId)
        .Join(courses, g => g.Key, c => c.Id,
            (g, c) => $"{c.Title}: {g.Count()} enrollments")
        .OrderByDescending(s => s);

// Task14: Average grade per course (only graded)
public IEnumerable<string> Task14_AverageGradePerCourse()
    => enrollments
        .Where(e => e.FinalGrade.HasValue)
        .GroupBy(e => e.CourseId)
        .Join(courses, g => g.Key, c => c.Id,
            (g, c) => $"{c.Title}: {g.Average(e => e.FinalGrade!.Value):F2}");

// Task15: Lecturers and their course counts (GroupJoin = left outer join)
public IEnumerable<string> Task15_LecturersAndCourseCounts()
    => lecturers
        .GroupJoin(courses, l => l.Id, c => c.LecturerId,
            (l, cs) => $"{l.FirstName} {l.LastName}: {cs.Count()} courses");

// Task16: Highest grade per student
public IEnumerable<string> Task16_HighestGradePerStudent()
    => enrollments
        .Where(e => e.FinalGrade.HasValue)
        .GroupBy(e => e.StudentId)
        .Join(students, g => g.Key, s => s.Id,
            (g, s) => $"{s.FirstName} {s.LastName}: {g.Max(e => e.FinalGrade!.Value):F1}");
```

### 5.14 Challenge Solutions

```csharp
// Challenge01: Students with more than one active enrollment
public IEnumerable<string> Challenge01_StudentsWithMoreThanOneActiveCourse()
    => students
        .Where(s => enrollments.Count(e => e.StudentId == s.Id && e.IsActive) > 1)
        .Select(s => {
            int count = enrollments.Count(e => e.StudentId == s.Id && e.IsActive);
            return $"{s.FirstName} {s.LastName}: {count} active courses";
        });
// Anna Kowalska: 2 active courses, Marek Nowak: 2 active courses

// Challenge02: April 2026 courses without final grades
public IEnumerable<string> Challenge02_AprilCoursesWithoutFinalGrades()
    => courses
        .Where(c => c.StartDate.Year == 2026 && c.StartDate.Month == 4)
        .Where(c => !enrollments
            .Where(e => e.CourseId == c.Id)
            .Any(e => e.FinalGrade.HasValue))
        .Select(c => c.Title);

// Challenge03: Lecturers and average grade across all their courses
public IEnumerable<string> Challenge03_LecturersAndAverageGradeAcrossTheirCourses()
{
    return lecturers.Select(l =>
    {
        var courseIds = courses.Where(c => c.LecturerId == l.Id).Select(c => c.Id).ToHashSet(); // O(1) lookup!
        var grades    = enrollments
            .Where(e => courseIds.Contains(e.CourseId) && e.FinalGrade.HasValue)
            .Select(e => e.FinalGrade!.Value).ToList();
        string avg = grades.Count > 0 ? grades.Average().ToString("F2") : "N/A";
        return $"{l.FirstName} {l.LastName}: {avg}";
    });
}

// Challenge04: Cities and active enrollment counts
public IEnumerable<string> Challenge04_CitiesAndActiveEnrollmentCounts()
    => students
        .GroupJoin(
            enrollments.Where(e => e.IsActive),
            s => s.Id, e => e.StudentId,
            (s, es) => new { s.City, Count = es.Count() })
        .GroupBy(x => x.City)
        .Select(g => new { City = g.Key, Total = g.Sum(x => x.Count) })
        .OrderByDescending(x => x.Total)
        .Select(x => $"{x.City}: {x.Total} active enrollments");
// Warsaw: 6 active enrollments, Krakow: 2 active enrollments ...
```

---

## 6. Design Patterns

### 6.1 Strategy Pattern (OCP in action)

```csharp
// Strategy interface
public interface IPaymentFeeStrategy
{
    bool AppliesTo(string method);
    (decimal fee, string note) Calculate(decimal amount);
}

// Concrete strategies — one per payment method
public class CardFeeStrategy : IPaymentFeeStrategy
{
    public bool AppliesTo(string m) => m == "CARD";
    public (decimal fee, string note) Calculate(decimal amount) => (amount * 0.02m, "card fee; ");
}
public class BankTransferStrategy : IPaymentFeeStrategy
{
    public bool AppliesTo(string m) => m == "BANK_TRANSFER";
    public (decimal fee, string note) Calculate(decimal amount) => (amount * 0.01m, "bank fee; ");
}

// Context — never changes when a new payment type is added
var strategies = new List<IPaymentFeeStrategy>
{
    new CardFeeStrategy(),
    new BankTransferStrategy(),
};
var strategy = strategies.FirstOrDefault(s => s.AppliesTo(paymentMethod))
    ?? throw new ArgumentException("Unsupported payment method");
var (fee, note) = strategy.Calculate(amount);
```

### 6.2 Adapter Pattern

```csharp
// Existing legacy system — static methods, cannot be injected
public static class LegacyBillingGateway
{
    public static void SaveInvoice(RenewalInvoice invoice) { /* ... */ }
    public static void SendEmail(string email, string subject, string body) { /* ... */ }
}

// Adapter — wraps legacy static API behind injectable interfaces
public class LegacyBillingAdapter : IInvoicePublisher, INotificationService
{
    public void Save(RenewalInvoice invoice)
        => LegacyBillingGateway.SaveInvoice(invoice);
    public void SendEmail(string email, string subject, string body)
        => LegacyBillingGateway.SendEmail(email, subject, body);
}

// High-level service uses it without knowing about legacy
var service = new SubscriptionRenewalService(
    new CustomerRepository(),
    new LegacyBillingAdapter(),   // satisfies both IInvoicePublisher and INotificationService
    new LegacyBillingAdapter());
```

### 6.3 Factory Method vs Abstract Factory

| Pattern | Creates | How | When to use |
|---------|---------|-----|-------------|
| Factory Method | Single type | Abstract method in base class; subclasses override | Class shouldn't know concrete type |
| Abstract Factory | A FAMILY of related objects | Interface with multiple factory methods | Ensure product families are compatible |

### 6.4 Singleton — Classic vs DI Container

```csharp
// AVOID: Classic Singleton — hinders unit testing, violates DIP
public class LegacySingleton
{
    private static LegacySingleton? _instance;
    private LegacySingleton() { }
    public static LegacySingleton Instance => _instance ??= new LegacySingleton();
}

// PREFER: AddSingleton via DI container — testable, injected via interface
builder.Services.AddSingleton<ICacheService, InMemoryCacheService>();
```

> **Exam:** Classic Singleton hinders unit testing. In ASP.NET Core prefer `AddSingleton` via DI container.

---

## 7. ADO.NET — SQL Server

### 7.1 Connection String & Setup

```csharp
// appsettings.json — NEVER hardcode credentials in code!
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyAppDb;User Id=sa;Password=...;TrustServerCertificate=True"
  }
}

// Program.cs — retrieve safely
string cs = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Missing connection string!");
```

### 7.2 SqlConnection — The using Pattern (Guaranteed Cleanup)

```csharp
// WRONG — connection leaks if exception thrown before Close()
var conn = new SqlConnection(cs);
conn.Open();
int x = 10 / 0;  // Exception! conn.Close() never reached → pool exhausts!
conn.Close();

// CORRECT — using declaration (C# 8+)
// Compiler generates try/finally: Dispose() is ALWAYS called
public async Task DoWorkAsync()
{
    using var conn = new SqlConnection(cs);
    await conn.OpenAsync();  // await = non-blocking!
    // ... execute queries ...
}  // ← conn.Dispose() guaranteed even on exception

// Repository with IConfiguration injection
public class ProductRepository
{
    private readonly string _cs;
    public ProductRepository(IConfiguration config)
        => _cs = config.GetConnectionString("DefaultConnection")!;

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            using var conn = new SqlConnection(_cs);
            await conn.OpenAsync();
            Console.WriteLine($"State: {conn.State}");     // State: Open
            Console.WriteLine($"Server: {conn.ServerVersion}"); // Server: 16.00.4085
            return true;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"DB error: {ex.Message}");
            return false;
        }
    }
}
```

**Connection Pooling:**

| Event | What ADO.NET does |
|-------|------------------|
| `conn.OpenAsync()` | Checks pool for free connection — lends instantly |
| `conn.Dispose()` | Connection NOT closed — returned to pool for reuse |
| Pool full (default: 100) | Next `Open()` blocks 15s, then throws `InvalidOperationException: Timeout` |

### 7.3 SqlCommand — ExecuteReader, ExecuteScalar, ExecuteNonQuery

| Method | Returns | Use for |
|--------|---------|---------|
| `ExecuteReaderAsync()` | `SqlDataReader` | SELECT → multiple rows |
| `ExecuteScalarAsync()` | `object?` (first col, first row) | SELECT COUNT, single value |
| `ExecuteNonQueryAsync()` | `int` (rows affected) | INSERT, UPDATE, DELETE |

**1. ExecuteReader — SELECT Multiple Rows:**
```csharp
using var conn = new SqlConnection(_cs);
await conn.OpenAsync();
using var cmd = new SqlCommand("SELECT Id, FirstName, LastName FROM Students", conn);
using var reader = await cmd.ExecuteReaderAsync();
while (await reader.ReadAsync())  // true = row read; false = no more rows
{
    int    id        = reader.GetInt32(0);   // column index 0
    string firstName = reader.GetString(1);  // column index 1
    string lastName  = reader.GetString(2);  // column index 2
    Console.WriteLine($"Student: {id} - {firstName} {lastName}");
}
```

**Output:**
```
Student: 1 - Anna Kowalska
Student: 2 - Jan Nowak
Student: 3 - Maria Wisniewska
```

**2. ExecuteScalar — Single Value:**
```csharp
using var conn = new SqlConnection(_cs);
await conn.OpenAsync();

// Get count
using var cmd = new SqlCommand("SELECT COUNT(*) FROM Students", conn);
int count = (int)(await cmd.ExecuteScalarAsync())!;
Console.WriteLine($"Total students: {count}");  // Total students: 3

// Get new row ID after INSERT
using var ins = new SqlCommand(
    "INSERT INTO Students(FirstName,LastName) VALUES(@fn,@ln); SELECT SCOPE_IDENTITY();",
    conn);
ins.Parameters.AddWithValue("@fn", "Piotr");
ins.Parameters.AddWithValue("@ln", "Zielinski");
int newId = Convert.ToInt32(await ins.ExecuteScalarAsync());
Console.WriteLine($"New student ID: {newId}");  // New student ID: 4
```

**3. ExecuteNonQuery — INSERT / UPDATE / DELETE:**
```csharp
using var conn = new SqlConnection(_cs);
await conn.OpenAsync();

// INSERT
using var ins = new SqlCommand("INSERT INTO Products(Name,Price) VALUES(@n,@p)", conn);
ins.Parameters.AddWithValue("@n", "Laptop");
ins.Parameters.AddWithValue("@p", 2499.99m);
Console.WriteLine($"Inserted: {await ins.ExecuteNonQueryAsync()} row(s)"); // Inserted: 1 row(s)

// UPDATE
using var upd = new SqlCommand("UPDATE Products SET Price=@p WHERE Id=@id", conn);
upd.Parameters.AddWithValue("@p", 2299.99m);
upd.Parameters.AddWithValue("@id", 1);
Console.WriteLine($"Updated: {await upd.ExecuteNonQueryAsync()} row(s)");  // Updated: 1 row(s)

// DELETE — use return value for 404 detection in REST API
using var del = new SqlCommand("DELETE FROM Products WHERE Id=@id", conn);
del.Parameters.AddWithValue("@id", 99);
int deleted = await del.ExecuteNonQueryAsync();
if (deleted == 0) return NotFound();  // HTTP 404
```

### 7.4 SQL Injection & Parameterization

```csharp
// NEVER — SQL Injection vulnerability
string sql = "SELECT COUNT(1) FROM Users WHERE Username = '" + username +
             "' AND Password = '" + password + "'";
// Attacker enters: ' OR 1=1 --
// DB executes: WHERE Username = '' OR 1=1 --' ...  → login bypassed!

// SAFE — parameterized query (@param is a placeholder, never concatenated)
public async Task<bool> LoginUser(string username, string password)
{
    using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    const string sql = "SELECT COUNT(1) FROM Users WHERE Username=@username AND Password=@password";
    using var cmd = new SqlCommand(sql, conn);

    // Method 1: AddWithValue (quick)
    cmd.Parameters.AddWithValue("@username", username);
    cmd.Parameters.AddWithValue("@password", password);

    // Method 2: Explicit SqlParameter (use for nvarchar, decimal — avoids type mismatch)
    cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 100) { Value = username });

    int count = (int)(await cmd.ExecuteScalarAsync())!;
    return count > 0;
}

// NULL pitfall — C# null is NOT SQL NULL
cmd.Parameters.AddWithValue("@middle", null);  // throws NullReferenceException!

// Fix:
string? middle = null;
cmd.Parameters.AddWithValue("@middle", (object?)middle ?? DBNull.Value);
// SQL receives: proper NULL
```

### 7.5 SqlDataReader — Reading & Mapping

```csharp
// Typed Methods Reference:
// reader.GetInt32(i)     → int     (most common for IDs)
// reader.GetString(i)    → string  (most common for text)
// reader.GetDecimal(i)   → decimal (NEVER GetDouble for money!)
// reader.GetDateTime(i)  → DateTime
// reader.GetBoolean(i)   → bool
// reader.GetGuid(i)      → Guid
// reader.GetOrdinal("Name") → int (get index from column name — safer than hardcoded)

// Handling NULL columns — ALWAYS check IsDBNull first!
while (await reader.ReadAsync())
{
    string name    = reader.GetString(1);  // safe — column is NOT NULL
    // reader.GetString(4)  // throws InvalidCastException if NULL in DB!
    string? middle = reader.IsDBNull(4) ? null : reader.GetString(4);  // correct
    DateTime? grad = reader.IsDBNull(5) ? null : reader.GetDateTime(5);
    Console.WriteLine($"{name} {middle ?? "(no middle name)"}");
}
// Output:
// Anna (no middle name)
// Jan Piotr
// Maria (no middle name)

// Clean Repository with Object Mapping
public record Student(int Id, string FirstName, string LastName, DateTime Dob, string? Email);

public class StudentRepository
{
    public async Task<List<Student>> GetAllAsync()
    {
        var list = new List<Student>();
        using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();
        using var cmd = new SqlCommand(
            "SELECT Id,FirstName,LastName,DateOfBirth,Email FROM Students", conn);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            list.Add(MapToStudent(reader));  // extracted mapping method
        return list;
    }

    private static Student MapToStudent(SqlDataReader r) => new(
        r.GetInt32(0), r.GetString(1), r.GetString(2),
        r.GetDateTime(3),
        r.IsDBNull(4) ? null : r.GetString(4));
}
```

### 7.6 Database Transactions

```csharp
// ACID: Atomicity, Consistency, Isolation, Durability

// Bank transfer — atomic debit + credit
public async Task ExecuteTransferAsync(int fromId, int toId, decimal amount)
{
    using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    using var tx = conn.BeginTransaction();  // ← start transaction
    try
    {
        // Step 1: Debit sender — tx is the 3rd argument (CRITICAL!)
        using var debit = new SqlCommand(
            "UPDATE Accounts SET Balance=Balance-@amt WHERE Id=@id",
            conn, tx);   // ← pass tx here!
        debit.Parameters.AddWithValue("@amt", amount);
        debit.Parameters.AddWithValue("@id", fromId);
        if (await debit.ExecuteNonQueryAsync() == 0)
            throw new InvalidOperationException($"Account {fromId} not found.");

        // Step 2: Credit receiver — SAME tx!
        using var credit = new SqlCommand(
            "UPDATE Accounts SET Balance=Balance+@amt WHERE Id=@id",
            conn, tx);   // ← same tx!
        credit.Parameters.AddWithValue("@amt", amount);
        credit.Parameters.AddWithValue("@id", toId);
        if (await credit.ExecuteNonQueryAsync() == 0)
            throw new InvalidOperationException($"Account {toId} not found.");

        await tx.CommitAsync();   // ← both succeeded → make permanent
        Console.WriteLine($"Transfer of {amount:C} completed.");
    }
    catch (Exception ex)
    {
        await tx.RollbackAsync();  // ← undo EVERYTHING
        Console.WriteLine($"Rolled back: {ex.Message}");
        throw;  // re-throw preserving stack trace (throw; NOT throw ex;)
    }
}
```

**Output:**
```
Transfer of $500.00 completed.
--- if account not found ---
Rolled back: Account 99 not found. All changes undone.
```

> ⚠️ Most common transaction bug: **forgetting `tx` as the 3rd argument to `SqlCommand`**. Without it, the command runs outside the transaction — rollback has no effect.

**Three ways to assign transaction to a command:**
```csharp
var tx = conn.BeginTransaction();
// Method 1: Constructor (preferred)
using var cmd1 = new SqlCommand("UPDATE ...", conn, tx);
// Method 2: Property
using var cmd2 = new SqlCommand("UPDATE ...", conn);
cmd2.Transaction = tx;
// Method 3: CreateCommand
using var cmd3 = conn.CreateCommand();
cmd3.CommandText = "UPDATE ...";
cmd3.Transaction = tx;
```

**Transaction Isolation Levels:**
```csharp
using var tx = conn.BeginTransaction(IsolationLevel.Serializable);
```

| Level | Dirty Reads | Non-repeatable | Phantom | Use for |
|-------|-------------|----------------|---------|---------|
| `ReadUncommitted` | Possible | Possible | Possible | Reporting (speed) |
| `ReadCommitted` (default) | Prevented | Possible | Possible | Most OLTP |
| `RepeatableRead` | Prevented | Prevented | Possible | Same data read twice |
| `Serializable` | Prevented | Prevented | Prevented | Financial — highest |

---

## 8. Common Pitfalls Cheat Sheet

### LINQ Pitfalls

| Pitfall | Symptom | Fix |
|---------|---------|-----|
| Second `.OrderBy()` replaces first | Sorted only by last key | Use `.ThenBy()` for secondary sorts |
| `.First()` on empty | `InvalidOperationException` | Use `.FirstOrDefault()` and null-check |
| Deferred execution + mutation | Query returns unexpected items | Call `.ToList()` to materialise |
| Multiple enumeration | DB query runs N times | Store in `List<T>` first |
| `.HasValue` not checked | `NullReferenceException` on nullable | `.Where(e => e.FinalGrade.HasValue)` first |

### async/await Pitfalls

| Pitfall | Symptom | Fix |
|---------|---------|-----|
| `async void` (non-event-handler) | Exceptions silently lost | Use `async Task` everywhere except event handlers |
| `.Result` or `.Wait()` | Deadlock in ASP.NET Core | Always `await` instead |

### OOP Pitfalls

| Pitfall | Symptom | Fix |
|---------|---------|-----|
| `new` method hiding | Polymorphism doesn't work | Use `virtual` + `override` |
| Closure captures variable | All lambdas print same value | `int copy = i;` inside loop |
| `== ` on class objects | Compares addresses not content | Override `Equals`; use records; or `==` IS value for strings in C# |

### ADO.NET Pitfalls

| Pitfall | Symptom | Fix |
|---------|---------|-----|
| Missing `using` on `SqlConnection` | Pool exhaustion → `Timeout` | Always `using var conn = ...` |
| Forgetting `tx` on `SqlCommand` | Command outside transaction — rollback useless | Pass `tx` as 3rd constructor argument |
| Reading NULL without `IsDBNull()` | `InvalidCastException` at runtime | `if (!reader.IsDBNull(i))` before `GetXxx(i)` |
| String concatenation in SQL | SQL Injection vulnerability | Always use `@param` parameters |
| `AddWithValue(null)` | `NullReferenceException` | `(object?)value ?? DBNull.Value` |
| Synchronous `Open()` in ASP.NET Core | Thread pool starvation | Always `await conn.OpenAsync()` |

### SOLID Pitfalls

| Pitfall | Symptom | Fix |
|---------|---------|-----|
| Service Locator | Hidden deps, untestable | Constructor-inject what you need |
| Scoped into Singleton | First request's data leaked forever | Match lifetimes; never inject Scoped into Singleton |

### Git Pitfalls

| Pitfall | Symptom | Fix |
|---------|---------|-----|
| Rebasing pushed commits | Teammates' histories diverge | Only rebase local branches; `--force-with-lease` if forced |
| `git merge` twice / forget `-d` | Duplicate commits; stale branch | `git branch -d feature/x` after every merge |

---

## Quick Exam Reminders

- **JIT** compiles IL at execution time. **AOT** compiles before execution.
- **Value types** store data (copy is independent). **Reference types** store a pointer (copy shares object).
- `init` = settable only during initialization. `set` = settable any time.
- **Closure** captures the VARIABLE, not its value. Fix: `int copy = i;` inside loop.
- `async void` = dangerous (exceptions lost). Use `async Task` except event handlers.
- `.Result` = deadlock risk. Always `await`.
- **Deferred execution**: `Where`, `Select`, `OrderBy` are lazy. `ToList`, `Count`, `First` trigger execution.
- `All()` on **empty collection = true** (vacuously true).
- Second `.OrderBy()` **replaces** the first — use `.ThenBy()`.
- `new` keyword **hides** (no polymorphism). `override` enables polymorphism.
- **ISP** violation = `NotImplementedException` in interface implementations.
- **DIP** = dependency arrow reversed — lower-level implements interface defined by higher-level.
- Singleton into Scoped = **never**. Scoped into Singleton = **never**.
- SQL parameterization: `@param` placeholders — never string concatenation.
- Transaction `tx` **must** be passed as 3rd argument to `SqlCommand` — or rollback won't work.
- `reader.IsDBNull(i)` **must** be called before `GetXxx(i)` on nullable columns.
- `git rebase` rewrites SHAs — **never** rebase already-pushed commits.
- `git merge --ff-only` = safety net: fails if not fast-forwardable.
