# APBD C# Practice Projects
**Polish-Japanese Academy of IT — Exam Preparation**

Each project below maps to specific Parts in your `Program.cs` workbook.  
Work through them in order — each builds on the last.

---

## How to use this guide

1. Create the folder structure exactly as shown.
2. Read the class/method specs, then write the code yourself.
3. After each file, go back to the matching `Part XX` in `Program.cs` and re-read the exam questions.
4. The `> EXAM TRAP` callouts are things that appear directly in exams — memorise the answers.

---

---

# PROJECT 1 — Animal Shelter Registry

**Parts covered:** 3, 4, 5, 6, 9, 11, 12, 13  
**Theme:** OOP fundamentals — value vs reference types, properties, null safety, records, inheritance, interfaces, custom exceptions, switch expressions.

---

## Folder structure

```
AnimalShelter/
├── AnimalShelter.csproj
├── Program.cs
├── Models/
│   ├── Animal.cs
│   ├── Dog.cs
│   ├── Cat.cs
│   ├── ExhibitAnimal.cs
│   └── AdoptionRecord.cs
├── Interfaces/
│   └── IAdoptable.cs
├── Exceptions/
│   └── AnimalNotFoundException.cs
└── Services/
    └── ShelterService.cs
```

---

## File-by-file specification

### `Models/Animal.cs`
**What it is:** Abstract base class for all animals.

```
class Animal (abstract)
├── Properties
│   ├── int Id                    { get; init; }
│   ├── string Name               { get; set; }
│   ├── int AgeYears              { get; set; }
│   ├── bool IsAdopted            { get; private set; }   ← only settable inside the class
│   └── string? Notes             { get; set; }           ← nullable string
│
├── Methods
│   ├── abstract string Speak()                           ← no body, subclasses must implement
│   ├── void MarkAdopted()                               ← sets IsAdopted = true
│   └── override string ToString()                       ← returns "Name (Type), Age: X"
```

**Step-by-step:**
1. Declare `abstract class Animal`.
2. Add `Id` with `init` — it can only be set when the object is created (Part 4).
3. Add `IsAdopted` with `private set` — outside code can read it but cannot change it directly.
4. Add `Notes` as `string?` — practice null-safe access: `animal.Notes?.ToUpper() ?? "No notes"` (Part 5).
5. Declare `abstract string Speak()` — no body, no curly braces.
6. Implement `MarkAdopted()` — just sets `IsAdopted = true`.
7. Override `ToString()` — use the type name: `this.GetType().Name`.

> **EXAM TRAP:** Abstract classes can have constructors, fields, and non-abstract methods. Interfaces cannot have fields or constructors. If two unrelated classes need shared state (not just a contract), use an abstract class.

---

### `Models/Dog.cs`
**What it is:** Concrete subclass of `Animal`. Implements `IAdoptable`.

```
class Dog : Animal, IAdoptable
├── Properties
│   ├── string Breed              { get; init; }
│   └── bool IsVaccinated         { get; set; }
│
├── Methods
│   ├── override string Speak()   → returns "Woof! I am {Name}"
│   ├── string Describe()         → returns computed description string
│   └── Dog WithNotes(string n)   → returns 'with' expression copy (Part 6 — records trick with class)
```

**Step-by-step:**
1. Inherit from `Animal`. Add `: IAdoptable` (Part 12).
2. Override `Speak()` — return a string using the `Name` property from the base class.
3. Write `Describe()` — computed property: `$"{Name} ({Breed}), vaccinated: {IsVaccinated}"`.
4. Since `Dog` is not a record, simulate `with` by writing `WithNotes(string n)` that creates `new Dog { Id = Id, Name = Name, ..., Notes = n }` and returns it.

> **EXAM TRAP:** `override` replaces the base implementation. `new` hides it — the base version is still called if accessed through a base-type reference. Never use `new` on a method unless you mean method hiding.

---

### `Models/Cat.cs`
**What it is:** Concrete subclass of `Animal`. Implements `IAdoptable`.

```
class Cat : Animal, IAdoptable
├── Properties
│   ├── bool IsIndoor             { get; init; }
│   └── string Colour             { get; init; }
│
├── Methods
│   └── override string Speak()   → returns "Meow. I am {Name}."
```

**Step-by-step:**
1. Inherit `Animal`, implement `IAdoptable`.
2. Both properties are `init` — they must be set in the object initialiser only.
3. Override `Speak()`.

---

### `Models/ExhibitAnimal.cs`
**What it is:** Animal that cannot be adopted — demonstrates ISP (Part 17).

```
class ExhibitAnimal : Animal
├── Properties
│   └── string ExhibitName        { get; init; }
│
├── Methods
│   └── override string Speak()   → returns "..."   (exhibit animals don't make noise)
```

**Step-by-step:**
1. Inherit `Animal` only — do NOT implement `IAdoptable`.
2. This enforces the principle: interfaces must be small and targeted. `ExhibitAnimal` should not be forced to implement `Adopt()`.

> **EXAM TRAP (ISP):** A class should never be forced to implement methods it does not need. If you put `Adopt()` on `Animal`, then `ExhibitAnimal` would need a fake implementation. Separate the interface instead.

---

### `Models/AdoptionRecord.cs`
**What it is:** Positional record — immutable data snapshot.

```
record AdoptionRecord(int Id, Animal Animal, DateOnly AdoptedOn, string AdopterName)
```

**Step-by-step:**
1. Declare as a positional `record` — one line only.
2. In `Program.cs`, create a record and then clone it with a different date using `with`: `record2 = record1 with { AdoptedOn = DateOnly.FromDateTime(DateTime.Now) }`.
3. Verify that `record1 == record2` is **false** (different date) — records compare by value.

> **EXAM TRAP:** Records use value equality (`==` compares all fields). Classes use reference equality by default. `with` creates a new copy — it does not modify the original.

---

### `Interfaces/IAdoptable.cs`
**What it is:** Segregated interface for adoptable animals only.

```
interface IAdoptable
└── void Adopt(string adopterName)
```

**Step-by-step:**
1. One method only — `void Adopt(string adopterName)`.
2. Implement in `Dog` and `Cat`. Call `MarkAdopted()` from the base class inside the implementation, then print a confirmation line.

---

### `Exceptions/AnimalNotFoundException.cs`
**What it is:** Domain-specific exception (Part 13).

```
class AnimalNotFoundException : Exception
├── Properties
│   └── int AnimalId              { get; }
│
└── Constructor(int animalId)
    └── base($"Animal with ID {animalId} was not found.")
```

**Step-by-step:**
1. Inherit from `Exception`.
2. Pass the message to `base(...)` in the constructor.
3. Store `animalId` in a property for callers to inspect.

> **EXAM TRAP:** `throw ex` resets the stack trace — you lose where the original error happened. `throw` (with no argument) preserves it. Always use plain `throw` in catch blocks when re-throwing.

---

### `Services/ShelterService.cs`
**What it is:** The in-memory data store and business logic.

```
class ShelterService
├── Fields
│   └── private List<Animal> _animals
│   └── private List<AdoptionRecord> _records
│   └── private int _nextId = 1
│
├── Methods
│   ├── void Add(Animal animal)
│   │   └── assigns Id via _nextId++, adds to list
│   │
│   ├── Animal? FindById(int id)
│   │   └── returns FirstOrDefault, result can be null
│   │
│   ├── List<Animal> GetAll()
│   │   └── returns copy of list
│   │
│   ├── List<Animal> GetAvailable()
│   │   └── Where(a => !a.IsAdopted).ToList()
│   │
│   ├── AdoptionRecord Adopt(int animalId, string adopterName)
│   │   ├── calls FindById → throws AnimalNotFoundException if null
│   │   ├── checks animal is IAdoptable → throws InvalidOperationException if not
│   │   ├── calls ((IAdoptable)animal).Adopt(adopterName)
│   │   └── creates and returns AdoptionRecord
│   │
│   └── string Describe(Animal animal)
│       └── switch expression on animal type:
│           Dog d    => $"Dog: {d.Breed}"
│           Cat c    => $"Cat: indoor={c.IsIndoor}"
│           _        => "Unknown animal type"
```

**Step-by-step:**
1. Write `Add()` — set `animal.Id` is `init` so you need to create a new object: use `with` pattern or accept that Id is set outside.  
   Alternative: make `Id` have a private setter and set it here with reflection — or, simpler, accept that Id is passed in from outside and just validate it's unique.
2. Write `FindById()` — returns `Animal?` (nullable). Callers must null-check.
3. In `Adopt()`, use `is IAdoptable adoptable` pattern: `if (animal is not IAdoptable adoptable) throw new InvalidOperationException(...)`.
4. Write the switch expression in `Describe()` — use `animal switch { Dog d => ..., Cat c => ..., _ => ... }` (Part 9).

---

### `Program.cs`

```
static async Task Main()
├── Create ShelterService
├── Add 3 Dogs, 2 Cats, 1 ExhibitAnimal
├── Print all animals with Describe()
├── Adopt one Dog — print AdoptionRecord
├── Try to adopt ExhibitAnimal → catch InvalidOperationException
├── Try FindById(999) → catch AnimalNotFoundException
├── Create AdoptionRecord, clone with 'with', show both records
└── Print GetAvailable() — should be one fewer after adoption
```

---

---

# PROJECT 2 — University Analytics Console App

**Parts covered:** 21, 22, 23, 24, 25, 27, 28, 29, 30  
**Theme:** LINQ (all operators), generics, yield return, StringBuilder, tuples.

---

## Folder structure

```
UniversityAnalytics/
├── UniversityAnalytics.csproj
├── Program.cs
├── Models/
│   ├── Student.cs
│   ├── Course.cs
│   └── Enrollment.cs
├── Data/
│   └── SeedData.cs
├── Queries/
│   ├── StudentQueries.cs
│   ├── EnrollmentQueries.cs
│   └── ReportBuilder.cs
└── Utilities/
    ├── GenericRepository.cs
    └── CsvExporter.cs
```

---

## File-by-file specification

### `Models/Student.cs`

```
record Student(
    int Id,
    string FirstName,
    string LastName,
    string Major,
    double Gpa,
    int EnrollmentYear
)
└── Computed property:
    string FullName => $"{FirstName} {LastName}"
```

**Step-by-step:**
1. Declare as a positional `record`.
2. Add a non-positional computed property `FullName` inside the record body.
3. Practice: create two Students with same values and verify `s1 == s2` is true (value equality).

---

### `Models/Course.cs`

```
record Course(int Id, string Name, int Credits, string Department)
```

### `Models/Enrollment.cs`

```
record Enrollment(int StudentId, int CourseId, double Grade)
```

---

### `Data/SeedData.cs`
**What it is:** Static class with pre-built test data.

```
static class SeedData
├── static List<Student> Students   → 20+ students, varied GPAs (1.5–4.0), 4 majors
├── static List<Course> Courses     → 8 courses, 3 departments
└── static List<Enrollment> Enrollments  → ~40 enrollments, grades 2.0–5.0
```

**Step-by-step:**
1. Create exactly 20 students — vary GPAs, majors (`"Computer Science"`, `"Mathematics"`, `"Physics"`, `"English"`), and enrollment years (2021–2024).
2. Create 8 courses — vary credits (2, 3, 4) and departments.
3. Create ~40 enrollments — every student has 1–3. Some students share courses.

---

### `Queries/StudentQueries.cs`
**What it is:** All student LINQ operations.

```
static class StudentQueries
│
├── List<Student> GetHonourStudents(List<Student> students)
│   └── Where Gpa >= 3.5, OrderByDescending Gpa, ToList
│
├── IEnumerable<string> GetFullNamesByMajor(List<Student> students, string major)
│   └── Where Major == major (case-insensitive), Select FullName
│
├── Dictionary<string, double> GetAverageGpaByMajor(List<Student> students)
│   └── GroupBy Major, ToDictionary(g => g.Key, g => g.Average(s => s.Gpa))
│
├── Dictionary<int, int> GetCountByYear(List<Student> students)
│   └── GroupBy EnrollmentYear, ToDictionary key=year value=count
│
├── List<Student> GetPage(List<Student> students, int page, int pageSize)
│   └── OrderBy Id, Skip((page-1)*pageSize), Take(pageSize), ToList
│
├── bool AllHaveGpa(List<Student> students, double min)
│   └── students.All(s => s.Gpa >= min)
│
├── bool AnyFromYear(List<Student> students, int year)
│   └── students.Any(s => s.EnrollmentYear == year)
│
└── (string Major, double Avg) GetBestMajor(List<Student> students)
    └── GroupBy Major, Select (g.Key, g.Average(Gpa)), MaxBy(x => x.Avg)
        returns a TUPLE — practice deconstruction in Program.cs
```

**Step-by-step:**
1. Write `GetHonourStudents()`. After calling it, add a new student with GPA 4.0 to the original list. Call the method again and show the new student appears — demonstrating deferred execution only happens if you don't call `ToList()`. (Part 21)
2. Write `GetAverageGpaByMajor()`. Use `GroupBy` then `Select` then `ToDictionary`.
3. Write `GetPage()`. Verify page 1 size 5 returns first 5, page 2 returns next 5.
4. Write `GetBestMajor()` returning a tuple. In `Program.cs`: `var (major, avg) = GetBestMajor(...)` — deconstruct it. (Part 30)

> **EXAM TRAP (deferred execution):** `var query = students.Where(...)` does NOT run the query. The query runs when you call `.ToList()`, `.First()`, `.Count()`, or iterate with `foreach`. Adding items to the source list AFTER writing the query but BEFORE materialising it WILL affect the result.

---

### `Queries/EnrollmentQueries.cs`
**What it is:** Cross-list LINQ joins and group joins.

```
static class EnrollmentQueries
│
├── List<string> GetCourseNamesForStudent(
│       List<Enrollment> enrollments,
│       List<Course> courses,
│       int studentId)
│   └── Where enrollment.StudentId == studentId,
│       Join on CourseId,
│       Select course.Name
│
├── List<(Student Student, double AvgGrade)> GetStudentAverages(
│       List<Student> students,
│       List<Enrollment> enrollments)
│   └── GroupJoin students with enrollments on Id,
│       Select (student, enrollments.Average(Grade) or 0.0 if empty)
│
├── List<Student> GetStudentsWithNoEnrollments(
│       List<Student> students,
│       List<Enrollment> enrollments)
│   └── Where !enrollments.Any(e => e.StudentId == s.Id)
│
└── List<string> GetAllCourseNamesTaken(
        List<Enrollment> enrollments,
        List<Course> courses)
    └── SelectMany or Join, Distinct, OrderBy, returns list of unique course names
```

**Step-by-step:**
1. Write the `Join` in `GetCourseNamesForStudent()`:
   ```
   enrollments
     .Where(e => e.StudentId == studentId)
     .Join(courses, e => e.CourseId, c => c.Id, (e, c) => c.Name)
     .ToList()
   ```
2. Write `GroupJoin` in `GetStudentAverages()`:
   ```
   students.GroupJoin(enrollments,
     s => s.Id,
     e => e.StudentId,
     (s, es) => (s, es.Any() ? es.Average(e => e.Grade) : 0.0))
   ```
3. In `GetStudentsWithNoEnrollments()` use the `!Any()` anti-join pattern.

> **EXAM TRAP:** `Join` = inner join (only students WITH enrollments appear). `GroupJoin` = left join (ALL students appear, with empty collection if no enrollments). Know which you need.

---

### `Queries/ReportBuilder.cs`
**What it is:** Uses `StringBuilder` and `yield return` to build a lazy report.

```
static class ReportBuilder
│
├── IEnumerable<string> GenerateStudentLines(List<Student> students)
│   └── foreach student: yield return formatted line
│       (lazy — only generates lines as consumed)
│
├── string BuildFullReport(List<Student> students, List<Enrollment> enrollments)
│   └── StringBuilder sb
│       sb.AppendLine("=== UNIVERSITY REPORT ===")
│       foreach line in GenerateStudentLines → sb.AppendLine(line)
│       sb.AppendLine($"Total students: {students.Count}")
│       return sb.ToString()
│
└── string SummariseGpa(List<Student> students)
    └── Aggregate to build a summary string:
        students.Aggregate("GPAs: ", (acc, s) => acc + $"{s.Gpa:F1} ")
```

**Step-by-step:**
1. In `GenerateStudentLines()` use `yield return` — add a `Console.WriteLine("generating...")` before the yield to prove it only runs when the caller iterates. (Part 28)
2. Use `StringBuilder` for `BuildFullReport()` — never use `string +=` in a loop (Part 29).
3. In `Program.cs` call `GenerateStudentLines()` but only take the first 5 with `.Take(5)` — observe that only 5 `"generating..."` lines print, not 20. This is lazy evaluation.

> **EXAM TRAP:** `string +=` in a loop creates a new string object every iteration — O(n²) memory. `StringBuilder.Append()` is O(1) amortised. On exams, always use `StringBuilder` when building strings in a loop.

---

### `Utilities/GenericRepository.cs`
**What it is:** Generic in-memory repository demonstrating reification. (Part 27)

```
class GenericRepository<T> where T : class
├── Fields
│   └── private List<T> _items = new()
│
├── Methods
│   ├── void Add(T item)
│   ├── T? FindFirst(Func<T, bool> predicate)       ← accepts a lambda
│   ├── List<T> FindAll(Func<T, bool> predicate)
│   ├── T Max<TKey>(Func<T, TKey> selector)         ← generic method inside generic class
│   │   where TKey : IComparable<TKey>
│   └── int Count() => _items.Count
```

**Step-by-step:**
1. Declare the class with `where T : class` constraint.
2. `FindFirst` accepts `Func<T, bool>` — call it `_items.FirstOrDefault(predicate)`.
3. `Max<TKey>` — a generic method inside a generic class. Constraint `where TKey : IComparable<TKey>` ensures you can compare values.
4. In `Program.cs`: `var repo = new GenericRepository<Student>()`. Add all students. Call `FindAll(s => s.Gpa > 3.0)`.

> **EXAM TRAP (reification):** C# generics are reified — `List<int>` and `List<string>` are genuinely different types at runtime. Java uses type erasure — `List<Integer>` and `List<String>` are both just `List` at runtime. This is why `typeof(List<int>)` works in C# but has no equivalent in Java.

---

### `Utilities/CsvExporter.cs`
**What it is:** Demonstrates `ref`, `out`, tuples, and optional parameters. (Parts 26, 30)

```
static class CsvExporter
│
├── string Export(
│       List<Student> students,
│       string separator = ",",       ← optional parameter with default
│       bool includeHeader = true)    ← optional parameter with default
│   └── StringBuilder, append header if flag, then each student row
│
├── bool TryExport(
│       List<Student> students,
│       out string csv,               ← out parameter
│       out string error)             ← out parameter
│   └── try { csv = Export(students); error = ""; return true; }
│       catch(ex) { csv = ""; error = ex.Message; return false; }
│
└── void CountAndValidate(
        List<Student> students,
        ref int totalProcessed,       ← ref parameter — caller's variable is modified
        ref int invalidCount)
    └── foreach student: totalProcessed++
                         if Gpa < 0 or > 4: invalidCount++
```

**Step-by-step:**
1. Write `Export()` with optional parameters — call it three ways: `Export(students)`, `Export(students, ";")`, `Export(students, ",", false)`.
2. Write `TryExport()` with `out string csv, out string error`. In `Program.cs`: `if (TryExport(students, out var csv, out var err)) ...`.
3. Write `CountAndValidate()` with `ref`. In `Program.cs`: declare `int processed = 0, invalid = 0;` then `CountAndValidate(students, ref processed, ref invalid)`. Print both after.

> **EXAM TRAP:** `out` — the method MUST assign the variable before returning. `ref` — the caller MUST have already assigned the variable before passing it. `in` — passes by reference but read-only (cannot modify).

---

---

# PROJECT 3 — Notification Dispatcher

**Parts covered:** 7, 8, 14, 15, 16, 17, 18, 19, 20  
**Theme:** SOLID all five principles, Strategy Pattern, delegates, async/await, DI lifetimes.

---

## Folder structure

```
NotificationDispatcher/
├── NotificationDispatcher.csproj
├── Program.cs
├── Models/
│   ├── Notification.cs
│   ├── EmailNotification.cs
│   ├── SmsNotification.cs
│   └── PushNotification.cs
├── Senders/
│   ├── ISender.cs
│   ├── EmailSender.cs
│   ├── SmsSender.cs
│   └── PushSender.cs
├── Services/
│   ├── INotificationService.cs
│   ├── NotificationService.cs
│   └── NotificationDispatcher.cs
├── Filters/
│   ├── INotificationFilter.cs
│   ├── BusinessHoursFilter.cs
│   └── RateLimitFilter.cs
└── Logging/
    └── IDispatchLogger.cs
    └── ConsoleDispatchLogger.cs
```

---

## File-by-file specification

### `Models/Notification.cs`

```
abstract record Notification(string Subject, string Body)
└── Property (non-positional):
    DateTime CreatedAt { get; } = DateTime.Now
```

> **EXAM TRAP:** Records can be abstract. Positional parameters become `init`-only properties automatically. Non-positional properties can be added inside the record body.

---

### `Models/EmailNotification.cs`

```
record EmailNotification(
    string To,
    string? Cc,
    string Subject,
    string Body
) : Notification(Subject, Body)
```

### `Models/SmsNotification.cs`

```
record SmsNotification(
    string PhoneNumber,
    string Subject,
    string Body
) : Notification(Subject, Body)
```

### `Models/PushNotification.cs`

```
record PushNotification(
    string DeviceToken,
    string AppName,
    string Subject,
    string Body
) : Notification(Subject, Body)
```

---

### `Senders/ISender.cs`

```
interface ISender
└── Task SendAsync(Notification notification)
```

### `Senders/EmailSender.cs`

```
class EmailSender : ISender
└── Task SendAsync(Notification notification)
    └── if (notification is EmailNotification email)
            Console.WriteLine($"[EMAIL] To: {email.To} | Subject: {email.Subject}")
        await Task.Delay(50)   ← simulates network
```

### `Senders/SmsSender.cs`

```
class SmsSender : ISender
└── Task SendAsync(Notification notification)
    └── if (notification is SmsNotification sms)
            Console.WriteLine($"[SMS] To: {sms.PhoneNumber} | {sms.Subject}")
        await Task.Delay(30)
```

### `Senders/PushSender.cs`

```
class PushSender : ISender
└── Task SendAsync(Notification notification)
    └── if (notification is PushNotification push)
            Console.WriteLine($"[PUSH] App: {push.AppName} | {push.Subject}")
        await Task.Delay(20)
```

---

### `Services/NotificationDispatcher.cs`

```
class NotificationDispatcher
├── Fields
│   └── private readonly List<ISender> _senders
│
├── Constructor(List<ISender> senders)
│   └── _senders = senders
│
└── async Task DispatchAsync(Notification notification)
    └── await Task.WhenAll(_senders.Select(s => s.SendAsync(notification)))
```

**Step-by-step:**
1. The dispatcher depends on `List<ISender>` (an abstraction), not on `EmailSender` directly — this is **DIP** (Part 18).
2. Adding a new sender (e.g. `SlackSender`) requires zero changes to `NotificationDispatcher` — this is **OCP** (Part 15).
3. `Task.WhenAll` fires all senders concurrently. Measure the difference vs sequential `foreach await` (Part 8).

> **EXAM TRAP:** `Task.WhenAll` starts all tasks before awaiting any. Total time ≈ slowest single task. Sequential `foreach (var s in senders) await s.SendAsync(n)` = sum of all times.

---

### `Filters/INotificationFilter.cs`

```
interface INotificationFilter
└── bool ShouldSend(Notification notification)
```

**Why this exists:** Demonstrates **ISP** (Part 17) — filters and senders are separate interfaces. A class implementing `ISender` is not forced to also implement filter logic.

### `Filters/BusinessHoursFilter.cs`

```
class BusinessHoursFilter : INotificationFilter
└── bool ShouldSend(Notification notification)
    └── int hour = DateTime.Now.Hour
        return hour >= 8 && hour < 18
```

### `Filters/RateLimitFilter.cs`

```
class RateLimitFilter : INotificationFilter
├── Fields
│   └── private readonly int _maxPerMinute
│   └── private readonly Queue<DateTime> _sentTimes = new()
│
└── bool ShouldSend(Notification notification)
    └── remove entries older than 1 minute from _sentTimes
        if _sentTimes.Count >= _maxPerMinute → return false
        _sentTimes.Enqueue(DateTime.Now)
        return true
```

**Step-by-step:**
1. `Queue<DateTime>` — first in, first out. `Enqueue` adds to the back, `Dequeue` removes from the front. (Part 10)
2. The filter uses `Peek()` to check the oldest time without removing it, then `Dequeue()` only if it is older than 1 minute.

---

### `Logging/ConsoleDispatchLogger.cs`

```
class ConsoleDispatchLogger : IDispatchLogger
└── void Log(Notification notification, bool wasSent)
    └── Console.WriteLine($"[LOG] {notification.CreatedAt:HH:mm:ss} | Sent={wasSent} | {notification.Subject}")
```

**Why this exists:** Separates logging from dispatching — **SRP** (Part 14). The dispatcher does not log. The logger logs. Neither knows about the other's implementation.

> **EXAM TRAP (SRP):** A class should have one reason to change. If logging and dispatching are in the same class, changing the log format requires touching the dispatch logic — two reasons to change, SRP violated.

---

### `Program.cs`

```
static async Task Main()
│
├── // Build manually (without DI container — shows composition root)
├── var senders = new List<ISender> { new EmailSender(), new SmsSender(), new PushSender() }
├── var dispatcher = new NotificationDispatcher(senders)
├── var filter = new BusinessHoursFilter()
├── var logger = new ConsoleDispatchLogger()
│
├── // Create notifications
├── var email = new EmailNotification("anna@uni.pl", null, "Welcome", "Hello!")
├── var sms   = new SmsNotification("+48123456789", "Alert", "Low balance")
├── var push  = new PushNotification("device-abc", "MyApp", "Promo", "50% off")
│
├── // Dispatch with filter check
├── foreach notification in [email, sms, push]:
│   ├── bool send = filter.ShouldSend(notification)
│   ├── logger.Log(notification, send)
│   └── if send → await dispatcher.DispatchAsync(notification)
│
├── // Delegates demo (Part 7)
├── Action<string> log = msg => Console.WriteLine($"[ACTION] {msg}")
├── Func<Notification, bool> isEmail = n => n is EmailNotification
├── Predicate<Notification> isUrgent = n => n.Subject.Contains("Alert")
│
└── // Call each delegate:
    log("Dispatch complete")
    Console.WriteLine(isEmail(email))
    Console.WriteLine(isUrgent(sms))
```

---

---

# PROJECT 4 — Library API (ASP.NET Core — In-Memory)

**Parts covered:** 38, 39, 40, 41, 42, 63, 64, 65  
**Theme:** ASP.NET Core controllers, routing, model binding, validation, middleware, conflict detection.

---

## Folder structure

```
LibraryApi/
├── LibraryApi.csproj
├── Program.cs
├── Models/
│   ├── Book.cs
│   └── Loan.cs
├── Dtos/
│   ├── BookRequest.cs
│   └── LoanRequest.cs
├── Data/
│   └── InMemoryStore.cs
├── Controllers/
│   ├── BooksController.cs
│   └── LoansController.cs
└── Middleware/
    ├── RequestTimingMiddleware.cs
    └── ExceptionHandlingMiddleware.cs
```

---

## File-by-file specification

### `Models/Book.cs`

```
class Book
├── int Id                { get; set; }
├── string Title          { get; set; }
├── string Author         { get; set; }
├── string ISBN           { get; set; }
├── int PageCount         { get; set; }
└── bool IsAvailable      { get; set; } = true
```

### `Models/Loan.cs`

```
class Loan
├── int Id                { get; set; }
├── int BookId            { get; set; }
├── string BorrowerId     { get; set; }
├── DateTime BorrowedAt   { get; set; }
└── DateTime? ReturnedAt  { get; set; }   ← nullable — null means not yet returned
```

---

### `Dtos/BookRequest.cs`

```
class BookRequest
├── [Required]
│   [StringLength(200, MinimumLength = 2)]
│   string Title          { get; set; }
│
├── [Required]
│   [MaxLength(120)]
│   string Author         { get; set; }
│
├── [Required]
│   string ISBN           { get; set; }
│
└── [Range(1, 5000)]
    int PageCount          { get; set; }
```

**Step-by-step:**
1. Add `using System.ComponentModel.DataAnnotations;` at the top.
2. With `[ApiController]` on the controller, these annotations are automatically checked. A missing `Title` returns 400 without any code in your action.

> **EXAM TRAP:** Without `[ApiController]`, you must manually write `if (!ModelState.IsValid) return BadRequest(ModelState);` at the top of every action. `[ApiController]` does this for you automatically.

---

### `Dtos/LoanRequest.cs`

```
record LoanRequest(int BookId, string BorrowerId)
```

---

### `Data/InMemoryStore.cs`

```
static class InMemoryStore
│
├── static int _nextBookId = 4
├── static int _nextLoanId = 3
│
├── static List<Book> Books
│   └── 3 seeded books (all IsAvailable = true except book 2 which is on loan)
│
├── static List<Loan> Loans
│   └── 2 active loans (ReturnedAt = null)
│
├── static int NextBookId() → _nextBookId++
└── static int NextLoanId() → _nextLoanId++
```

---

### `Controllers/BooksController.cs`

```
[ApiController]
[Route("api/books")]
class BooksController : ControllerBase
│
├── [HttpGet]
│   ActionResult<IEnumerable<Book>> GetAll(
│       [FromQuery] string? author,
│       [FromQuery] bool? available)
│   └── filter by author (case-insensitive), filter by available, return Ok(list)
│
├── [HttpGet("{id:int}")]
│   ActionResult<Book> GetById(int id)
│   └── FirstOrDefault → 404 if null, else Ok
│
├── [HttpPost]
│   ActionResult<Book> Create([FromBody] BookRequest dto)
│   └── map dto to Book, assign NextBookId(), Add to store
│       return CreatedAtAction(nameof(GetById), new { id = book.Id }, book)
│
├── [HttpPut("{id:int}")]
│   ActionResult<Book> Update(int id, [FromBody] BookRequest dto)
│   └── find book → 404 if missing
│       overwrite all fields (full PUT replacement)
│       return Ok(book)
│
└── [HttpDelete("{id:int}")]
    IActionResult Delete(int id)
    └── find book → 404 if missing
        Remove from store
        return NoContent()   ← 204
```

**Step-by-step:**
1. Write `GetAll()` — chain `.Where()` calls only when the parameter has a value: `if (author != null) books = books.Where(...)`.
2. Write `Create()` — use `CreatedAtAction`. The first argument is the name of the GET-by-id method as a string: `nameof(GetById)`.
3. Write `Update()` — overwrite EVERY field from the DTO. This is a full `PUT` replacement.

> **EXAM TRAP:** `CreatedAtAction(nameof(GetById), new { id = book.Id }, book)` generates the `Location` header in the 201 response. `Created(string.Empty, book)` returns 201 but with no `Location` header — not standard REST.

---

### `Controllers/LoansController.cs`

```
[ApiController]
[Route("api/loans")]
class LoansController : ControllerBase
│
├── [HttpGet]
│   ActionResult<IEnumerable<Loan>> GetAll(
│       [FromQuery] int? bookId,
│       [FromQuery] bool? active)       ← active = ReturnedAt is null
│   └── filter and return
│
├── [HttpGet("{id:int}")]
│   ActionResult<Loan> GetById(int id)
│
├── [HttpPost]
│   ActionResult<Loan> Create([FromBody] LoanRequest dto)
│   ├── find Book → 404 if not found
│   ├── if !book.IsAvailable → 409 Conflict("Book is already on loan")
│   ├── create Loan record, set BorrowedAt = DateTime.Now
│   ├── set book.IsAvailable = false
│   └── return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan)
│
└── [HttpDelete("{id:int}")]
    IActionResult Return(int id)        ← "return" a book
    ├── find Loan → 404 if not found
    ├── if loan.ReturnedAt != null → 409 Conflict("Already returned")
    ├── loan.ReturnedAt = DateTime.Now
    ├── find book by loan.BookId, set IsAvailable = true
    └── return NoContent()
```

**Step-by-step:**
1. In `Create()`, check `book.IsAvailable` BEFORE creating the loan. Return `Conflict(...)` if false.
2. In `Return()`, check `loan.ReturnedAt != null` — if so it's already been returned.
3. When returning, update both the loan (set `ReturnedAt`) AND the book (set `IsAvailable = true`).

> **EXAM TRAP:** 409 Conflict means the request is valid, but the current state of the resource prevents it. 400 Bad Request means the request itself is malformed. A book already on loan is a conflict — the request data is fine.

---

### `Middleware/RequestTimingMiddleware.cs`

```
class RequestTimingMiddleware
├── Fields
│   ├── private readonly RequestDelegate _next
│   └── private readonly ILogger<RequestTimingMiddleware> _logger
│
├── Constructor(RequestDelegate next, ILogger<...> logger)
│   └── assigns both fields
│
└── async Task InvokeAsync(HttpContext context)
    ├── var sw = Stopwatch.StartNew()
    ├── try { await _next(context) }
    └── finally
          sw.Stop()
          _logger.LogInformation(
              "{Method} {Path} completed in {Ms}ms",
              context.Request.Method,
              context.Request.Path,
              sw.Elapsed.TotalMilliseconds)
```

**Step-by-step:**
1. Use `finally` — not `after _next`. `finally` runs even if the downstream controller throws.
2. Register in `Program.cs` with `app.UseMiddleware<RequestTimingMiddleware>()` — BEFORE `app.MapControllers()`.

> **EXAM TRAP:** Middleware is singleton (one instance for the app lifetime). Never inject a `Scoped` service (like `DbContext`) into the constructor — it will be kept alive forever. Inject scoped services into `InvokeAsync` parameters instead — ASP.NET Core resolves them per-request there.

---

### `Middleware/ExceptionHandlingMiddleware.cs`

```
class ExceptionHandlingMiddleware
├── Fields
│   ├── private readonly RequestDelegate _next
│   └── private readonly ILogger<ExceptionHandlingMiddleware> _logger
│
└── async Task InvokeAsync(HttpContext context)
    └── try { await _next(context) }
        catch (Exception ex)
            _logger.LogError(ex, "Unhandled exception")
            context.Response.StatusCode = 500
            context.Response.ContentType = "application/json"
            await context.Response.WriteAsJsonAsync(new { message = "An unexpected error occurred." })
```

**Step-by-step:**
1. Register this as the FIRST middleware in `Program.cs` — it must wrap everything else to catch any exception.
2. Set `ContentType` before writing the body — otherwise the response header is already flushed.

---

### `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

// Order matters:
app.UseMiddleware<ExceptionHandlingMiddleware>();   // ← FIRST
app.UseMiddleware<RequestTimingMiddleware>();
app.UseRouting();
app.MapControllers();

app.Run();
```

> **EXAM TRAP (pipeline order):** `UseExceptionHandler` / custom exception middleware must come first. `UseAuthentication` must come before `UseAuthorization` — you must know who the user is before deciding what they can do.

---

---

# PROJECT 5 — Clinic Management API (ADO.NET + 3-Layer)

**Parts covered:** 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 63, 64  
**Theme:** Full three-layer architecture with real SQL Server, parameterised queries, transactions, DI registration.

---

## Folder structure

```
ClinicApi/
├── ClinicApi.csproj
├── appsettings.json
├── Program.cs
├── Models/
│   ├── Patient.cs
│   ├── Doctor.cs
│   └── Appointment.cs
├── Dtos/
│   ├── PatientRequest.cs
│   ├── AppointmentRequest.cs
│   └── AppointmentResult.cs
├── Repositories/
│   ├── IPatientRepository.cs
│   ├── IAppointmentRepository.cs
│   ├── SqlPatientRepository.cs
│   └── SqlAppointmentRepository.cs
├── Services/
│   ├── IPatientService.cs
│   ├── IAppointmentService.cs
│   ├── PatientService.cs
│   └── AppointmentService.cs
└── Controllers/
    ├── PatientsController.cs
    └── AppointmentsController.cs
```

---

## Database setup

Before writing any code, create the database:

```sql
CREATE DATABASE ClinicDb;
GO
USE ClinicDb;

CREATE TABLE Doctors (
    Id       INT PRIMARY KEY IDENTITY,
    Name     NVARCHAR(100) NOT NULL,
    Specialty NVARCHAR(100) NOT NULL
);

CREATE TABLE Patients (
    Id        INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName  NVARCHAR(100) NOT NULL,
    Pesel     NVARCHAR(11) UNIQUE NOT NULL,
    Email     NVARCHAR(200) NULL
);

CREATE TABLE Appointments (
    Id          INT PRIMARY KEY IDENTITY,
    PatientId   INT NOT NULL REFERENCES Patients(Id),
    DoctorId    INT NOT NULL REFERENCES Doctors(Id),
    ScheduledAt DATETIME2 NOT NULL,
    DurationMin INT NOT NULL DEFAULT 30,
    Status      NVARCHAR(20) NOT NULL DEFAULT 'scheduled'
);

INSERT INTO Doctors VALUES ('Jan Kowalski', 'Cardiology');
INSERT INTO Doctors VALUES ('Anna Nowak', 'Neurology');
INSERT INTO Patients VALUES ('Marek', 'Zając', '90010112345', 'marek@email.pl');
INSERT INTO Patients VALUES ('Ewa', 'Lisowska', '85060567890', NULL);
```

---

## File-by-file specification

### `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ClinicDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
  }
}
```

> **EXAM TRAP:** Never hardcode connection strings in controller or repository code. Inject `IConfiguration` and call `config.GetConnectionString("DefaultConnection")`. In production, use environment variables or Azure Key Vault.

---

### `Models/Patient.cs`

```
class Patient
├── int Id              { get; set; }
├── string FirstName    { get; set; }
├── string LastName     { get; set; }
├── string Pesel        { get; set; }
└── string? Email       { get; set; }   ← nullable
```

### `Models/Doctor.cs`

```
class Doctor
├── int Id              { get; set; }
├── string Name         { get; set; }
└── string Specialty    { get; set; }
```

### `Models/Appointment.cs`

```
class Appointment
├── int Id              { get; set; }
├── int PatientId       { get; set; }
├── int DoctorId        { get; set; }
├── DateTime ScheduledAt { get; set; }
├── int DurationMin     { get; set; }
└── string Status       { get; set; }
```

---

### `Dtos/PatientRequest.cs`

```
class PatientRequest
├── [Required] [MaxLength(100)] string FirstName
├── [Required] [MaxLength(100)] string LastName
├── [Required] [StringLength(11, MinimumLength = 11)] string Pesel
└── [EmailAddress] string? Email
```

### `Dtos/AppointmentRequest.cs`

```
class AppointmentRequest
├── [Required] int PatientId
├── [Required] int DoctorId
├── [Required] DateTime ScheduledAt
└── [Range(10, 120)] int DurationMin
```

### `Dtos/AppointmentResult.cs`

```
record AppointmentResult(
    int Id,
    string PatientFullName,
    string DoctorName,
    DateTime ScheduledAt,
    int DurationMin,
    string Status
)
```

---

### `Repositories/IPatientRepository.cs`

```
interface IPatientRepository
├── Task<IEnumerable<Patient>> GetAllAsync()
├── Task<Patient?> GetByIdAsync(int id)
├── Task<Patient?> GetByPeselAsync(string pesel)
├── Task<int> AddAsync(Patient patient)               ← returns new Id
└── Task<bool> DeleteAsync(int id)                    ← returns true if found and deleted
```

### `Repositories/SqlPatientRepository.cs`

```
class SqlPatientRepository : IPatientRepository
├── Fields
│   └── private readonly string _connectionString
│
├── Constructor(IConfiguration config)
│   └── _connectionString = config.GetConnectionString("DefaultConnection")!
│
├── async Task<IEnumerable<Patient>> GetAllAsync()
│   └── SELECT Id, FirstName, LastName, Pesel, Email FROM Patients
│       ExecuteReaderAsync → while ReadAsync → map each row to Patient
│       Handle Email: reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal)
│
├── async Task<Patient?> GetByIdAsync(int id)
│   └── SELECT ... WHERE Id = @Id
│       ExecuteReaderAsync → if ReadAsync() returns true → map one row, else return null
│
├── async Task<Patient?> GetByPeselAsync(string pesel)
│   └── SELECT ... WHERE Pesel = @Pesel
│       Same pattern as GetByIdAsync
│
├── async Task<int> AddAsync(Patient patient)
│   └── INSERT INTO Patients(FirstName,LastName,Pesel,Email) VALUES(@fn,@ln,@pesel,@email)
│       ; SELECT SCOPE_IDENTITY()
│       ExecuteScalarAsync → cast to int → return new Id
│       Pass null email as: command.Parameters.AddWithValue("@email", (object?)patient.Email ?? DBNull.Value)
│
└── async Task<bool> DeleteAsync(int id)
    └── DELETE FROM Patients WHERE Id = @Id
        ExecuteNonQueryAsync → return rowsAffected > 0
```

**Step-by-step:**
1. Always `using var connection = new SqlConnection(_connectionString)` and `await connection.OpenAsync()` before running any command.
2. In `GetAllAsync()`, cache ordinals OUTSIDE the `while` loop: `int idOrd = reader.GetOrdinal("Id")` — faster than string lookup per row.
3. In `AddAsync()`, combine INSERT + `SELECT SCOPE_IDENTITY()` in one command string. Use `ExecuteScalarAsync` — it returns the identity of the inserted row.
4. Null email: `command.Parameters.AddWithValue("@email", (object?)patient.Email ?? DBNull.Value)` — never pass plain C# `null` to ADO.NET.

> **EXAM TRAP:** `SCOPE_IDENTITY()` returns the last identity value inserted in the CURRENT scope and connection. Use it (not `@@IDENTITY`) because `@@IDENTITY` can be affected by triggers that insert into other tables.

---

### `Repositories/IAppointmentRepository.cs`

```
interface IAppointmentRepository
├── Task<IEnumerable<AppointmentResult>> GetAllAsync(int? doctorId, DateTime? date)
├── Task<AppointmentResult?> GetByIdAsync(int id)
├── Task<int> AddWithTransactionAsync(Appointment appointment)   ← uses a transaction
└── Task<bool> CancelAsync(int id)
```

### `Repositories/SqlAppointmentRepository.cs`

```
class SqlAppointmentRepository : IAppointmentRepository
│
├── async Task<IEnumerable<AppointmentResult>> GetAllAsync(int? doctorId, DateTime? date)
│   └── SELECT a.Id, p.FirstName+' '+p.LastName AS PatientFullName,
│              d.Name AS DoctorName, a.ScheduledAt, a.DurationMin, a.Status
│       FROM Appointments a
│       JOIN Patients p ON a.PatientId = p.Id
│       JOIN Doctors d ON a.DoctorId = d.Id
│       WHERE (@DoctorId IS NULL OR a.DoctorId = @DoctorId)
│         AND (@Date IS NULL OR CAST(a.ScheduledAt AS DATE) = @Date)
│       Map to AppointmentResult record
│
├── async Task<int> AddWithTransactionAsync(Appointment appointment)
│   └── open connection, begin transaction
│       Step 1: check patient exists → SELECT COUNT(*) WHERE Id = @PatientId
│               if 0 → throw NotFoundException
│       Step 2: check doctor exists → same pattern
│       Step 3: check no overlap →
│               SELECT COUNT(*) FROM Appointments
│               WHERE DoctorId = @DoctorId AND Status != 'cancelled'
│               AND @Start < DATEADD(minute, DurationMin, ScheduledAt)
│               AND DATEADD(minute, @Dur, @Start) > ScheduledAt
│       Step 4: INSERT INTO Appointments(...) VALUES(...); SELECT SCOPE_IDENTITY()
│       CommitAsync on success, RollbackAsync in catch, rethrow
│
└── async Task<bool> CancelAsync(int id)
    └── UPDATE Appointments SET Status = 'cancelled' WHERE Id = @Id
        return rowsAffected > 0
```

**Step-by-step:**
1. In `GetAllAsync()`, pass nullable parameters to SQL using: `command.Parameters.AddWithValue("@DoctorId", (object?)doctorId ?? DBNull.Value)`.
2. In `AddWithTransactionAsync()`, ALL commands must use the same `connection` AND `transaction` object. Pass `transaction` as the third argument to `SqlCommand`, or set `cmd.Transaction = transaction`.
3. The overlap check SQL uses `DATEADD(minute, DurationMin, ScheduledAt)` to compute the end time in SQL — avoids loading all rows into C# just to check overlap.

> **EXAM TRAP:** If you forget to assign `cmd.Transaction = transaction` to even one command inside a transaction block, you will get `InvalidOperationException: ExecuteNonQuery requires the command to have a transaction...` at runtime.

---

### `Services/PatientService.cs`

```
class PatientService : IPatientService
├── Fields
│   └── private readonly IPatientRepository _repo
│
├── Constructor(IPatientRepository repo) → _repo = repo
│
├── Task<IEnumerable<Patient>> GetAllAsync() → _repo.GetAllAsync()
│
├── async Task<Patient> GetByIdAsync(int id)
│   └── var p = await _repo.GetByIdAsync(id)
│       return p ?? throw new NotFoundException($"Patient {id} not found")
│
└── async Task<int> AddAsync(PatientRequest dto)
    ├── var existing = await _repo.GetByPeselAsync(dto.Pesel)
    ├── if existing != null → throw InvalidOperationException("PESEL already registered")
    ├── var patient = new Patient { FirstName = dto.FirstName, ... }
    └── return await _repo.AddAsync(patient)
```

**Step-by-step:**
1. The service layer enforces the business rule: PESEL must be unique. The repository just runs SQL — it does not know about business rules.
2. The controller catches `NotFoundException` → 404, and `InvalidOperationException` → 409.

> **EXAM TRAP (layer responsibilities):** Controller = parse HTTP, return status codes. Service = business rules (e.g. "PESEL must be unique"). Repository = execute SQL. Never put a `SELECT` query in a controller. Never return `ActionResult` from a service.

---

### `Controllers/PatientsController.cs`

```
[ApiController]
[Route("api/patients")]
class PatientsController : ControllerBase
│
├── Constructor(IPatientService service) → _service = service
│
├── [HttpGet]
│   async Task<ActionResult<IEnumerable<Patient>>> GetAll()
│   └── return Ok(await _service.GetAllAsync())
│
├── [HttpGet("{id:int}")]
│   async Task<ActionResult<Patient>> GetById(int id)
│   └── try { return Ok(await _service.GetByIdAsync(id)) }
│       catch (NotFoundException) { return NotFound() }
│
└── [HttpPost]
    async Task<ActionResult<int>> Add([FromBody] PatientRequest dto)
    └── try
          int id = await _service.AddAsync(dto)
          return CreatedAtAction(nameof(GetById), new { id }, id)
        catch (InvalidOperationException ex)
          return Conflict(new { error = ex.Message })
```

---

### `Controllers/AppointmentsController.cs`

```
[ApiController]
[Route("api/appointments")]
class AppointmentsController : ControllerBase
│
├── [HttpGet]
│   async Task<ActionResult<IEnumerable<AppointmentResult>>> GetAll(
│       [FromQuery] int? doctorId,
│       [FromQuery] DateTime? date)
│   └── return Ok(await _service.GetAllAsync(doctorId, date))
│
├── [HttpGet("{id:int}")]
│   async Task<ActionResult<AppointmentResult>> GetById(int id)
│
├── [HttpPost]
│   async Task<ActionResult<int>> Create([FromBody] AppointmentRequest dto)
│   └── try { ... return CreatedAtAction(...) }
│       catch (NotFoundException ex)   → NotFound(new { error = ex.Message })
│       catch (ConflictException ex)   → Conflict(new { error = ex.Message })
│
└── [HttpDelete("{id:int}")]
    async Task<IActionResult> Cancel(int id)
    └── bool ok = await _service.CancelAsync(id)
        return ok ? NoContent() : NotFound()
```

---

### `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// 3-layer DI registration — Scoped because each HTTP request gets its own set
builder.Services.AddScoped<IPatientRepository, SqlPatientRepository>();
builder.Services.AddScoped<IAppointmentRepository, SqlAppointmentRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.MapControllers();

app.Run();
```

> **EXAM TRAP (DI lifetimes):**  
> `Transient` = new instance every time injected.  
> `Scoped` = one instance per HTTP request — correct for repositories and services.  
> `Singleton` = one instance for the whole app — never inject Scoped into Singleton (captive dependency — the Scoped service is never released).

---

---

## Quick-reference: exam answers to memorise

| Question | Answer |
|---|---|
| `abstract` vs `interface` | Abstract: can have fields, constructors, non-abstract methods. Interface: contract only, no state. |
| `override` vs `new` | `override` replaces through polymorphism. `new` hides — base version still called via base-type reference. |
| Record equality | Records compare by value. Classes compare by reference. |
| `with` expression | Creates a new copy of a record with specified fields changed. Does not mutate. |
| Deferred execution | LINQ query runs when materialised (`ToList`, `First`, `foreach`). Not when declared. |
| `Join` vs `GroupJoin` | `Join` = inner join. `GroupJoin` = left join (all left-side items appear). |
| `Task.WhenAll` vs sequential | `WhenAll` = concurrent, total time ≈ slowest. Sequential = sum of all times. |
| `ref` vs `out` | `ref`: caller must assign before passing. `out`: method must assign before returning. |
| Reification | C# generics are real types at runtime (`List<int>` ≠ `List<string>`). Java erases them. |
| `yield return` | Lazy — generates one element at a time. Caller controls how many are consumed. |
| `StringBuilder` vs `+=` | `+=` in a loop = O(n²). `StringBuilder.Append` = O(1) amortised. |
| SRP | One class = one reason to change. Logger and dispatcher are separate. |
| OCP | Open for extension (new sender), closed for modification (dispatcher never changes). |
| LSP | Subclass can replace base class without breaking caller's assumptions. |
| ISP | Many small interfaces > one large. `IAdoptable` separate from `IAnimal`. |
| DIP | Depend on abstractions (`ISender`), not concretions (`EmailSender`). |
| `[ApiController]` | Auto-validates model, infers binding sources, returns 400 on invalid. |
| `CreatedAtAction` | Returns 201 + `Location` header. `Created(string.Empty, x)` returns 201 but no `Location`. |
| 400 vs 404 | 400 = request is malformed. 404 = request is valid, resource doesn't exist. |
| 401 vs 403 | 401 = not authenticated (who are you?). 403 = not authorised (you can't do this). |
| 409 | Request is valid, but current server state prevents it (e.g. already booked). |
| Middleware order | Exception handler first, then routing, then auth/authz, then MapControllers last. |
| Scoped in Singleton | Captive dependency — Scoped service never released. Causes memory leaks. |
| `using var conn` | Calls `Dispose()` on exit, which returns the connection to the pool (not close TCP). |
| Connection pooling | `Open()` = take from pool. `Close()` = return to pool. Default max size = 100. |
| `ExecuteNonQuery` | INSERT / UPDATE / DELETE. Returns rows affected. |
| `ExecuteScalar` | Returns first column of first row. Use for `COUNT(*)`, `SCOPE_IDENTITY()`. |
| `ExecuteReader` | Returns `SqlDataReader`. Forward-only, streaming. Connection must stay open. |
| Null column in reader | `reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal)` — never skip the check. |
| SQL injection fix | Always use `Parameters.AddWithValue("@param", value)`. Never concatenate user input. |
| C# null → SQL NULL | `command.Parameters.AddWithValue("@p", (object?)value ?? DBNull.Value)` |
| Transaction pattern | `BeginTransaction` → try → `CommitAsync` → catch → `RollbackAsync` → `throw` |
| `cmd.Transaction = tx` | EVERY `SqlCommand` inside a transaction MUST have this assigned or you get `InvalidOperationException`. |
| `SCOPE_IDENTITY()` | Returns the identity of the last INSERT in the current scope. Safer than `@@IDENTITY`. |
| Layer responsibilities | Controller = HTTP. Service = business rules. Repository = SQL only. |
| `AddScoped` | One instance per HTTP request. Correct choice for repositories and services. |
