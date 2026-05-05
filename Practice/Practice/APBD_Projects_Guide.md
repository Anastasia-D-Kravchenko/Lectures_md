# APBD Practice Projects — Structure & Code Guide

> Every folder, every file, every line explained. Read top-to-bottom once, then use it as a lookup.  
> Format: **what it is → why it exists → full working code → what you get when you run it.**

---

# PROJECT 1 — Animal Shelter Registry

**What this project teaches:** OOP fundamentals. Abstract classes, interfaces, inheritance, records, custom exceptions, switch expressions, null safety.

---

## Why the folders are split this way

```
AnimalShelter/
├── Models/          ← the "things" in your domain (Animal, Dog, Cat)
├── Interfaces/      ← contracts other code depends on (IAdoptable)
├── Exceptions/      ← domain-specific errors (AnimalNotFoundException)
├── Services/        ← business logic (ShelterService handles rules)
└── Program.cs       ← entry point — wires everything together
```

**The rule:** Models are dumb data shapes. Services are the brain. Interfaces are promises. Exceptions carry meaning. Nothing in `Models/` knows about `Services/` — data flows up, not down.

---

## `Models/Animal.cs` — Abstract base class

**What it is:** The template every animal must follow. Can't be created directly — only subclasses can.

```csharp
// Models/Animal.cs
namespace AnimalShelter.Models;

public abstract class Animal
{
    // init = can only be set at creation time, never changed after
    // This protects the ID from being accidentally overwritten
    public int     Id        { get; init; }
    public string  Name      { get; set; } = string.Empty;
    public int     AgeYears  { get; set; }

    // private set = anyone can READ IsAdopted, but only THIS class can WRITE it
    // The outside world calls MarkAdopted() — it can't set IsAdopted = true directly
    public bool    IsAdopted { get; private set; }

    // string? = nullable. This animal might not have notes. That's valid.
    public string? Notes     { get; set; }

    // abstract = no body here. Subclasses MUST provide their own version.
    // Dog says "Woof!", Cat says "Meow." — the base class doesn't know which.
    public abstract string Speak();

    // Non-abstract method — all animals share this same logic
    // Only this class can flip IsAdopted because it's private set
    public void MarkAdopted()
    {
        IsAdopted = true;
    }

    // override = replaces the default object.ToString()
    // GetType().Name = "Dog", "Cat", "ExhibitAnimal" — the actual runtime type
    public override string ToString()
        => $"{Name} ({this.GetType().Name}), Age: {AgeYears}";
}
```

**What you get:**
```
// new Dog { Id=1, Name="Rex", AgeYears=3 }.ToString()
Rex (Dog), Age: 3

// animal.Notes?.ToUpper() ?? "No notes"
//   if Notes is null → "No notes"
//   if Notes is "friendly" → "FRIENDLY"
```

> **Why `private set` on IsAdopted?** Without it, any code could write `dog.IsAdopted = true` directly, bypassing any logic in `MarkAdopted()`. The private setter forces callers to go through the method.

---

## `Models/Dog.cs` — Concrete subclass

**What it is:** A real animal that can be created. Inherits `Animal`, adds dog-specific data, and promises it can be adopted.

```csharp
// Models/Dog.cs
namespace AnimalShelter.Models;
using AnimalShelter.Interfaces;

// : Animal        = inherits everything from Animal
// , IAdoptable    = also promises to implement the Adopt() method
public class Dog : Animal, IAdoptable
{
    // init = breed is set once at creation, never changed
    public string Breed       { get; init; } = string.Empty;
    public bool   IsVaccinated { get; set; }

    // override = replaces Animal.Speak() with Dog-specific behaviour
    // Polymorphism: Animal a = new Dog(); a.Speak() → "Woof! I am Rex"
    public override string Speak()
        => $"Woof! I am {Name}";

    // Regular method, not an override — just a Dog-specific helper
    public string Describe()
        => $"{Name} ({Breed}), vaccinated: {IsVaccinated}";

    // Records have 'with' built-in. Classes don't.
    // This manually simulates 'with' — creates a copy with a changed Notes field.
    // The original Dog object is untouched.
    public Dog WithNotes(string notes) => new Dog
    {
        Id           = this.Id,
        Name         = this.Name,
        AgeYears     = this.AgeYears,
        Breed        = this.Breed,
        IsVaccinated = this.IsVaccinated,
        Notes        = notes
    };

    // This is the implementation required by IAdoptable
    // Calls MarkAdopted() from the base class — that's the only way to set IsAdopted
    public void Adopt(string adopterName)
    {
        MarkAdopted();  // flips IsAdopted = true (only Animal can do this)
        Console.WriteLine($"{Name} adopted by {adopterName}!");
    }
}
```

**What you get:**
```csharp
var rex = new Dog { Id=1, Name="Rex", AgeYears=3, Breed="Labrador", IsVaccinated=true };

rex.Speak()     // "Woof! I am Rex"
rex.Describe()  // "Rex (Labrador), vaccinated: True"

// Polymorphism demo:
Animal a = rex;
a.Speak()       // still "Woof! I am Rex" ← override works through base-type reference

// WithNotes creates a NEW object. rex.Notes is still null.
var rex2 = rex.WithNotes("Loves fetch");
rex.Notes    // null
rex2.Notes   // "Loves fetch"
```

---

## `Models/Cat.cs` — Another concrete subclass

**What it is:** Like Dog but for cats. Shows that multiple classes can inherit the same base.

```csharp
// Models/Cat.cs
namespace AnimalShelter.Models;
using AnimalShelter.Interfaces;

public class Cat : Animal, IAdoptable
{
    // Both init — set once at creation, frozen after
    public bool   IsIndoor { get; init; }
    public string Colour   { get; init; } = string.Empty;

    public override string Speak()
        => $"Meow. I am {Name}.";

    public void Adopt(string adopterName)
    {
        MarkAdopted();
        Console.WriteLine($"{Name} the {Colour} cat adopted by {adopterName}!");
    }
}
```

---

## `Models/ExhibitAnimal.cs` — The ISP example

**What it is:** An animal that CANNOT be adopted. It deliberately does NOT implement `IAdoptable`.

```csharp
// Models/ExhibitAnimal.cs
namespace AnimalShelter.Models;

// Only inherits Animal. No IAdoptable.
// If IAdoptable were on Animal itself, this class would be FORCED
// to write a fake Adopt() method — that's an ISP violation.
public class ExhibitAnimal : Animal
{
    public string ExhibitName { get; init; } = string.Empty;

    // Exhibit animals are silent in their enclosure
    public override string Speak() => "...";
}
```

**What you get:**
```csharp
var panda = new ExhibitAnimal { Id=6, Name="Panda", AgeYears=5, ExhibitName="Asia Wing" };

panda.Speak()   // "..."
panda is IAdoptable   // FALSE — the compiler knows this can't be adopted

// In ShelterService.Adopt():
// if (animal is not IAdoptable) throw new InvalidOperationException("Cannot adopt exhibit animals");
```

> **Why no IAdoptable on ExhibitAnimal?** Interface Segregation Principle. If `Adopt()` lived on `Animal`, then `ExhibitAnimal` would need to write `throw new NotImplementedException()` — a red flag that your interface is too fat.

---

## `Models/AdoptionRecord.cs` — Immutable data snapshot

**What it is:** A record that captures the moment an adoption happened. Records compare by value, not reference.

```csharp
// Models/AdoptionRecord.cs
namespace AnimalShelter.Models;

// One line. Compiler generates: constructor, init properties, Equals, ==, GetHashCode, ToString, Deconstruct
public record AdoptionRecord(int Id, Animal Animal, DateOnly AdoptedOn, string AdopterName);
```

**What you get:**
```csharp
var record1 = new AdoptionRecord(1, rex, DateOnly.FromDateTime(DateTime.Now), "Anna");

// Clone with 'with' — one field changed, everything else copied
var record2 = record1 with { AdoptedOn = new DateOnly(2025, 1, 1) };

record1 == record2   // FALSE — different AdoptedOn (value equality, all fields checked)
record1.Animal       // rex (same object reference inside, but record comparison checks Animal == Animal)

// Deconstruct
var (id, animal, date, name) = record1;
// id=1, animal=rex, date=today, name="Anna"

// ToString auto-generated:
Console.WriteLine(record1);
// AdoptionRecord { Id = 1, Animal = Rex (Dog), Age: 3, AdoptedOn = 2025-04-12, AdopterName = Anna }
```

---

## `Interfaces/IAdoptable.cs` — The contract

**What it is:** A promise. Any class that implements this can be adopted. Nothing more.

```csharp
// Interfaces/IAdoptable.cs
namespace AnimalShelter.Interfaces;

public interface IAdoptable
{
    // No fields, no constructor — just a signature
    // Any class implementing this MUST provide this method
    void Adopt(string adopterName);
}
```

> **Why a separate file?** The interface belongs to the consumer (the service that calls `Adopt()`), not to the animals. Keeping it in its own folder makes that clear.

---

## `Exceptions/AnimalNotFoundException.cs` — Domain exception

**What it is:** A specific error type for when an animal ID doesn't exist. Carries the bad ID so callers can log or display it.

```csharp
// Exceptions/AnimalNotFoundException.cs
namespace AnimalShelter.Exceptions;

public class AnimalNotFoundException : Exception
{
    // Store the ID so callers can read it: ex.AnimalId
    public int AnimalId { get; }

    // base(...) = passes the message to Exception's constructor
    // The message is already formatted here — callers just catch and read ex.Message
    public AnimalNotFoundException(int animalId)
        : base($"Animal with ID {animalId} was not found.")
    {
        AnimalId = animalId;
    }
}
```

**What you get:**
```csharp
try
{
    shelter.FindById(999); // doesn't exist
}
catch (AnimalNotFoundException ex)
{
    Console.WriteLine(ex.Message);   // "Animal with ID 999 was not found."
    Console.WriteLine(ex.AnimalId);  // 999

    // throw;     ← re-throw: preserves original stack trace ✓
    // throw ex;  ← BAD: resets stack trace to this line ✗
}
```

---

## `Services/ShelterService.cs` — Business logic

**What it is:** The brain. Stores all animals, enforces rules (can't adopt exhibit animals), builds records. No UI, no database — just logic.

```csharp
// Services/ShelterService.cs
namespace AnimalShelter.Services;
using AnimalShelter.Exceptions;
using AnimalShelter.Interfaces;
using AnimalShelter.Models;

public class ShelterService
{
    // Private storage — nobody outside can touch these lists directly
    private readonly List<Animal>         _animals = new();
    private readonly List<AdoptionRecord> _records = new();
    private int _nextId = 1;

    public void Add(Animal animal)
    {
        // Id is init-only — set it here before the animal is "locked"
        // We use reflection trick by initialising a new object,
        // but simpler: just accept that Id must be set before calling Add()
        _animals.Add(animal);
        _nextId++;
    }

    // Returns null if not found — caller must check
    public Animal? FindById(int id)
        => _animals.FirstOrDefault(a => a.Id == id);

    public List<Animal> GetAll()
        => _animals.ToList();  // ToList() returns a copy — prevents callers from mutating _animals

    // Only returns animals not yet adopted
    public List<Animal> GetAvailable()
        => _animals.Where(a => !a.IsAdopted).ToList();

    public AdoptionRecord Adopt(int animalId, string adopterName)
    {
        // FindById returns null if not found
        var animal = FindById(animalId)
            ?? throw new AnimalNotFoundException(animalId);  // throw if null

        // Pattern matching: "is animal also an IAdoptable?"
        // If NOT → throw
        if (animal is not IAdoptable adoptable)
            throw new InvalidOperationException(
                $"{animal.Name} is an exhibit animal and cannot be adopted.");

        // Call the interface method — this calls Dog.Adopt() or Cat.Adopt()
        adoptable.Adopt(adopterName);

        var record = new AdoptionRecord(
            _records.Count + 1,
            animal,
            DateOnly.FromDateTime(DateTime.Now),
            adopterName);

        _records.Add(record);
        return record;
    }

    // switch expression on type — cleaner than if/else chain
    // animal switch { Dog d => ..., Cat c => ..., _ => ... }
    public string Describe(Animal animal) => animal switch
    {
        Dog d             => $"Dog: {d.Breed}, vaccinated: {d.IsVaccinated}",
        Cat c             => $"Cat: indoor={c.IsIndoor}, colour={c.Colour}",
        ExhibitAnimal e   => $"Exhibit: {e.ExhibitName}",
        _                 => "Unknown animal type"  // _ = default case
    };
}
```

**What you get:**
```csharp
var shelter = new ShelterService();
var rex = new Dog { Id=1, Name="Rex", AgeYears=3, Breed="Labrador", IsVaccinated=true };
shelter.Add(rex);

shelter.Describe(rex)           // "Dog: Labrador, vaccinated: True"
shelter.GetAvailable().Count    // 1

shelter.Adopt(1, "Anna");       // prints "Rex adopted by Anna!"
shelter.GetAvailable().Count    // 0  (Rex is now IsAdopted=true)

shelter.Adopt(999, "Bob");      // throws AnimalNotFoundException
shelter.Adopt(6, "Eve");        // throws InvalidOperationException (ExhibitAnimal)
```

---

## `Program.cs` — Entry point

```csharp
// Program.cs
using AnimalShelter.Exceptions;
using AnimalShelter.Models;
using AnimalShelter.Services;

var shelter = new ShelterService();

// Seed data
shelter.Add(new Dog         { Id=1, Name="Rex",   AgeYears=3, Breed="Labrador",   IsVaccinated=true  });
shelter.Add(new Dog         { Id=2, Name="Buddy",  AgeYears=5, Breed="Poodle",     IsVaccinated=false });
shelter.Add(new Cat         { Id=3, Name="Luna",   AgeYears=2, Colour="Grey",      IsIndoor=true      });
shelter.Add(new Cat         { Id=4, Name="Shadow", AgeYears=4, Colour="Black",     IsIndoor=false     });
shelter.Add(new ExhibitAnimal { Id=5, Name="Panda", AgeYears=7, ExhibitName="Asia Wing" });

// Print all animals
Console.WriteLine("=== All Animals ===");
foreach (var a in shelter.GetAll())
    Console.WriteLine($"  {a} | {shelter.Describe(a)}");

// Adopt one dog
Console.WriteLine("\n=== Adoption ===");
var record = shelter.Adopt(1, "Anna Kowalska");
Console.WriteLine($"Record: {record}");

// Try to adopt an exhibit animal → expected exception
Console.WriteLine("\n=== Exhibit Animal Attempt ===");
try   { shelter.Adopt(5, "Bob"); }
catch (InvalidOperationException ex) { Console.WriteLine($"Caught: {ex.Message}"); }

// Try a bad ID → expected exception
Console.WriteLine("\n=== Bad ID Attempt ===");
try   { shelter.Adopt(999, "Eve"); }
catch (AnimalNotFoundException ex)   { Console.WriteLine($"Caught: {ex.Message} (ID={ex.AnimalId})"); }

// Records: value equality and 'with'
Console.WriteLine("\n=== Record Demo ===");
var r1 = record;
var r2 = r1 with { AdoptedOn = new DateOnly(2020, 1, 1) };
Console.WriteLine($"r1 == r2: {r1 == r2}");  // False (different date)

Console.WriteLine("\n=== Available After Adoption ===");
foreach (var a in shelter.GetAvailable())
    Console.WriteLine($"  {a}");
```

**Full output:**
```
=== All Animals ===
  Rex (Dog), Age: 3 | Dog: Labrador, vaccinated: True
  Buddy (Dog), Age: 5 | Dog: Poodle, vaccinated: False
  Luna (Cat), Age: 2 | Cat: indoor=True, colour=Grey
  Shadow (Cat), Age: 4 | Cat: indoor=False, colour=Black
  Panda (ExhibitAnimal), Age: 7 | Exhibit: Asia Wing

=== Adoption ===
Rex adopted by Anna Kowalska!
Record: AdoptionRecord { Id = 1, Animal = Rex (Dog), Age: 3, AdoptedOn = 2025-04-12, AdopterName = Anna Kowalska }

=== Exhibit Animal Attempt ===
Caught: Panda is an exhibit animal and cannot be adopted.

=== Bad ID Attempt ===
Caught: Animal with ID 999 was not found. (ID=999)

=== Record Demo ===
r1 == r2: False

=== Available After Adoption ===
  Buddy (Dog), Age: 5
  Luna (Cat), Age: 2
  Shadow (Cat), Age: 4
  Panda (ExhibitAnimal), Age: 7
```

---
---

# PROJECT 2 — University Analytics Console App

**What this project teaches:** Every LINQ operator, generics with constraints, `yield return`, `StringBuilder`, tuples, `ref`/`out` parameters.

---

## Why the folders are split this way

```
UniversityAnalytics/
├── Models/        ← records: Student, Course, Enrollment (data shapes only)
├── Data/          ← SeedData: static test data, no logic
├── Queries/       ← where all LINQ lives, split by concern
│   ├── StudentQueries.cs     ← queries about students only
│   ├── EnrollmentQueries.cs  ← queries that cross students + enrollments + courses
│   └── ReportBuilder.cs      ← builds text output (StringBuilder + yield)
└── Utilities/
    ├── GenericRepository.cs  ← teaches T generics + constraints
    └── CsvExporter.cs        ← teaches ref, out, optional params
```

**The rule:** `Queries/` is split because Student operations don't need enrollment data. Keeping them separate means you can test student logic without touching the join logic.

---

## `Models/Student.cs` — Positional record with a computed property

**What it is:** An immutable data shape. Records can have bodies where you add extra members.

```csharp
// Models/Student.cs
namespace UniversityAnalytics.Models;

// Positional record: compiler generates constructor + init props for everything in ()
// The body {} can add extra computed properties
public record Student(
    int    Id,
    string FirstName,
    string LastName,
    string Major,
    double Gpa,
    int    EnrollmentYear)
{
    // This is NOT a positional parameter — it lives in the body
    // It's computed from FirstName + LastName — no backing field
    public string FullName => $"{FirstName} {LastName}";
}
```

**What you get:**
```csharp
var s1 = new Student(1, "Anna",  "Kowalska", "CS",   3.8, 2022);
var s2 = new Student(1, "Anna",  "Kowalska", "CS",   3.8, 2022);
var s3 = new Student(2, "Marek", "Nowak",    "Math", 2.9, 2023);

s1.FullName    // "Anna Kowalska"
s1 == s2       // TRUE  — same values (record value equality)
s1 == s3       // FALSE — different Id, Name, etc.

// 'with' creates a copy with one field changed
var s4 = s1 with { Gpa = 4.0 };
s1.Gpa         // 3.8  (original unchanged)
s4.Gpa         // 4.0  (new copy)
```

---

## `Models/Course.cs` and `Models/Enrollment.cs`

```csharp
// Models/Course.cs
public record Course(int Id, string Name, int Credits, string Department);

// Models/Enrollment.cs
public record Enrollment(int StudentId, int CourseId, double Grade);
```

---

## `Data/SeedData.cs` — Static test data

**What it is:** Pre-built lists used by every query. No logic — just data.

```csharp
// Data/SeedData.cs
namespace UniversityAnalytics.Data;
using UniversityAnalytics.Models;

public static class SeedData
{
    // static readonly = initialised once, shared across all callers
    public static readonly List<Student> Students = new()
    {
        new(1,  "Anna",   "Kowalska",  "Computer Science", 3.9, 2022),
        new(2,  "Marek",  "Nowak",     "Mathematics",      2.7, 2021),
        new(3,  "Julia",  "Wiśniewska","Computer Science", 3.5, 2023),
        new(4,  "Tomasz", "Lis",       "Physics",          1.8, 2022),
        new(5,  "Olga",   "Jabłońska", "English",          3.2, 2024),
        new(6,  "Paweł",  "Woźniak",   "Mathematics",      4.0, 2021),
        new(7,  "Lena",   "Adamska",   "Computer Science", 3.7, 2023),
        new(8,  "Karol",  "Dąbrowski", "Physics",          2.1, 2022),
    };

    public static readonly List<Course> Courses = new()
    {
        new(1, "Algorithms",       4, "Computer Science"),
        new(2, "Linear Algebra",   3, "Mathematics"),
        new(3, "Quantum Physics",  4, "Physics"),
        new(4, "English Literature",2,"Humanities"),
        new(5, "Data Structures",  3, "Computer Science"),
    };

    public static readonly List<Enrollment> Enrollments = new()
    {
        new(1, 1, 4.5), new(1, 5, 3.8),  // Anna: Algorithms(4.5), DataStructures(3.8)
        new(2, 2, 3.0), new(2, 1, 2.5),  // Marek: LinearAlgebra(3.0), Algorithms(2.5)
        new(3, 1, 4.0), new(3, 5, 4.2),  // Julia
        new(4, 3, 2.0),                  // Tomasz: QuantumPhysics(2.0) — only one
        new(5, 4, 3.5),                  // Olga
        new(6, 2, 4.8), new(6, 1, 4.5), // Paweł
        new(7, 1, 3.9), new(7, 5, 4.1), // Lena
        // Karol has NO enrollments — used to demonstrate anti-join
    };
}
```

---

## `Queries/StudentQueries.cs` — All LINQ on students

**What it is:** Every LINQ operation that only touches the `Student` list.

```csharp
// Queries/StudentQueries.cs
namespace UniversityAnalytics.Queries;
using UniversityAnalytics.Models;

public static class StudentQueries
{
    // Where + OrderByDescending + ToList
    // ToList() materialises — turns the lazy recipe into a real list
    public static List<Student> GetHonourStudents(List<Student> students)
        => students
            .Where(s => s.Gpa >= 3.5)
            .OrderByDescending(s => s.Gpa)
            .ToList();

    // Where (case-insensitive) + Select (project to string)
    // Returns IEnumerable — lazy, not materialised
    public static IEnumerable<string> GetFullNamesByMajor(List<Student> students, string major)
        => students
            .Where(s => s.Major.Equals(major, StringComparison.OrdinalIgnoreCase))
            .Select(s => s.FullName);

    // GroupBy + ToDictionary — group key becomes dict key, aggregate becomes value
    public static Dictionary<string, double> GetAverageGpaByMajor(List<Student> students)
        => students
            .GroupBy(s => s.Major)
            .ToDictionary(
                g => g.Key,                      // key:   "Computer Science"
                g => g.Average(s => s.Gpa));     // value: 3.7 (average of group)

    // GroupBy + ToDictionary — count per year
    public static Dictionary<int, int> GetCountByYear(List<Student> students)
        => students
            .GroupBy(s => s.EnrollmentYear)
            .ToDictionary(g => g.Key, g => g.Count());

    // Skip + Take — paging pattern
    // Page 1 = Skip(0).Take(3)  → items 0,1,2
    // Page 2 = Skip(3).Take(3)  → items 3,4,5
    public static List<Student> GetPage(List<Student> students, int page, int pageSize)
        => students
            .OrderBy(s => s.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

    // All() — returns true only if EVERY student passes the predicate
    // Returns TRUE on an empty collection (vacuous truth — exam trap!)
    public static bool AllHaveGpa(List<Student> students, double min)
        => students.All(s => s.Gpa >= min);

    // Any() — returns true if AT LEAST ONE student matches
    // Stops at the first match — much faster than Count() > 0 for large lists
    public static bool AnyFromYear(List<Student> students, int year)
        => students.Any(s => s.EnrollmentYear == year);

    // Returns a named tuple — two values without a class
    // MaxBy = pick the group with the highest average GPA
    public static (string Major, double Avg) GetBestMajor(List<Student> students)
    {
        var grouped = students
            .GroupBy(s => s.Major)
            .Select(g => (Major: g.Key, Avg: g.Average(s => s.Gpa)));

        return grouped.MaxBy(x => x.Avg)!;
    }
}
```

**What you get:**
```csharp
using UniversityAnalytics.Data;
using UniversityAnalytics.Queries;

var students = SeedData.Students;

// Honour students
StudentQueries.GetHonourStudents(students)
// [Paweł(4.0), Anna(3.9), Lena(3.7), Julia(3.5)]

// By major
StudentQueries.GetFullNamesByMajor(students, "Physics")
// ["Tomasz Lis", "Karol Dąbrowski"]

// Average GPA per major
StudentQueries.GetAverageGpaByMajor(students)
// { "Computer Science": 3.7, "Mathematics": 3.35, "Physics": 1.95, "English": 3.2 }

// Paging
StudentQueries.GetPage(students, page:2, pageSize:3)
// [Tomasz, Olga, Paweł] (items 3, 4, 5)

// All have GPA >= 2.0?
StudentQueries.AllHaveGpa(students, 2.0)  // FALSE (Tomasz has 1.8)
StudentQueries.AllHaveGpa(new List<Student>(), 2.0)  // TRUE (empty = vacuous)

// Best major
var (major, avg) = StudentQueries.GetBestMajor(students);
// major = "Computer Science", avg = 3.7
```

---

## `Queries/EnrollmentQueries.cs` — Cross-list joins

**What it is:** LINQ that joins two or three collections. The key difference between `Join` (inner) and `GroupJoin` (left outer).

```csharp
// Queries/EnrollmentQueries.cs
namespace UniversityAnalytics.Queries;
using UniversityAnalytics.Models;

public static class EnrollmentQueries
{
    // Join = INNER JOIN — only enrollments that match a course appear
    // Steps: filter to this student → join on CourseId → project to course name
    public static List<string> GetCourseNamesForStudent(
        List<Enrollment> enrollments,
        List<Course>     courses,
        int              studentId)
        => enrollments
            .Where(e => e.StudentId == studentId)
            .Join(
                courses,
                e => e.CourseId,   // key from LEFT side (enrollment)
                c => c.Id,         // key from RIGHT side (course)
                (e, c) => c.Name)  // what to return when keys match
            .ToList();

    // GroupJoin = LEFT OUTER JOIN — ALL students appear, even those with no enrollments
    // Returns a list of tuples: (Student, their average grade or 0 if no grades)
    public static List<(Student Student, double AvgGrade)> GetStudentAverages(
        List<Student>    students,
        List<Enrollment> enrollments)
        => students
            .GroupJoin(
                enrollments,
                s => s.Id,          // key from LEFT (student)
                e => e.StudentId,   // key from RIGHT (enrollment)
                (student, es) =>    // es = all enrollments for this student (can be empty!)
                    (student, es.Any() ? es.Average(e => e.Grade) : 0.0))
            .ToList();

    // Anti-join pattern: students NOT in enrollments
    // !Any() = "has no enrollment where StudentId matches"
    public static List<Student> GetStudentsWithNoEnrollments(
        List<Student>    students,
        List<Enrollment> enrollments)
        => students
            .Where(s => !enrollments.Any(e => e.StudentId == s.Id))
            .ToList();

    // SelectMany flattens a nested collection
    // Each enrollment maps to one course name; SelectMany collects all of them
    // Then Distinct removes duplicates, OrderBy sorts
    public static List<string> GetAllCourseNamesTaken(
        List<Enrollment> enrollments,
        List<Course>     courses)
        => enrollments
            .Join(courses, e => e.CourseId, c => c.Id, (e, c) => c.Name)
            .Distinct()
            .OrderBy(name => name)
            .ToList();
}
```

**What you get:**
```csharp
// Course names for student 1 (Anna)
EnrollmentQueries.GetCourseNamesForStudent(SeedData.Enrollments, SeedData.Courses, 1)
// ["Algorithms", "Data Structures"]

// Average grade per student (GroupJoin — Karol has no enrollments → 0.0)
EnrollmentQueries.GetStudentAverages(SeedData.Students, SeedData.Enrollments)
// (Anna, 4.15), (Marek, 2.75), (Julia, 4.1), (Tomasz, 2.0),
// (Olga, 3.5), (Paweł, 4.65), (Lena, 4.0), (Karol, 0.0)  ← Karol: no enrollments

// Students with NO enrollments
EnrollmentQueries.GetStudentsWithNoEnrollments(SeedData.Students, SeedData.Enrollments)
// [Karol Dąbrowski]

// All unique course names taken (at least once)
EnrollmentQueries.GetAllCourseNamesTaken(SeedData.Enrollments, SeedData.Courses)
// ["Algorithms", "Data Structures", "English Literature", "Linear Algebra", "Quantum Physics"]
```

---

## `Queries/ReportBuilder.cs` — StringBuilder + yield return

**What it is:** Demonstrates WHY you use `StringBuilder` in loops and HOW `yield return` makes sequences lazy.

```csharp
// Queries/ReportBuilder.cs
namespace UniversityAnalytics.Queries;
using System.Text;
using UniversityAnalytics.Models;

public static class ReportBuilder
{
    // yield return = lazy generator
    // This method DOES NOT run when called — it runs one step at a time as the caller iterates
    // Console.WriteLine("generating...") proves the laziness — only fires when iterated
    public static IEnumerable<string> GenerateStudentLines(List<Student> students)
    {
        foreach (var s in students)
        {
            Console.WriteLine("generating...");   // only prints when the caller reads this item
            yield return $"{s.Id,3}. {s.FullName,-20} | {s.Major,-20} | GPA: {s.Gpa:F1}";
        }
        // After the last yield, the method ends — no explicit return needed
    }

    // StringBuilder = O(1) append. string += in a loop = O(n²) because each += creates a new string.
    public static string BuildFullReport(List<Student> students, List<Enrollment> enrollments)
    {
        var sb = new StringBuilder();

        sb.AppendLine("=== UNIVERSITY REPORT ===");
        sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
        sb.AppendLine();

        // GenerateStudentLines is lazy — it yields one line at a time
        foreach (var line in GenerateStudentLines(students))
            sb.AppendLine(line);

        sb.AppendLine();
        sb.AppendLine($"Total students:   {students.Count}");
        sb.AppendLine($"Total enrollments: {enrollments.Count}");

        return sb.ToString();  // converts the buffer to a single string at the end
    }

    // Aggregate = fold. Starts with seed "GPAs: ", appends each Gpa.
    public static string SummariseGpa(List<Student> students)
        => students.Aggregate("GPAs: ", (accumulated, s) => accumulated + $"{s.Gpa:F1} ");
}
```

**What you get:**
```csharp
// Lazy demo — only first 3 items generated
var lines = ReportBuilder.GenerateStudentLines(SeedData.Students).Take(3);
foreach (var line in lines)
    Console.WriteLine(line);

// Output:
// generating...
//   1. Anna Kowalska          | Computer Science     | GPA: 3.9
// generating...
//   2. Marek Nowak            | Mathematics          | GPA: 2.7
// generating...
//   3. Julia Wiśniewska       | Computer Science     | GPA: 3.5
// (Only 3 "generating..." — not 8!)

// StringBuilder report
Console.WriteLine(ReportBuilder.BuildFullReport(SeedData.Students, SeedData.Enrollments));
// === UNIVERSITY REPORT ===
// Generated: 2025-04-12 14:30
//
//   1. Anna Kowalska          | Computer Science     | GPA: 3.9
// ... (all 8 students)
//
// Total students:    8
// Total enrollments: 13

// Aggregate
ReportBuilder.SummariseGpa(SeedData.Students)
// "GPAs: 3.9 2.7 3.5 1.8 3.2 4.0 3.7 2.1 "
```

---

## `Utilities/GenericRepository.cs` — Generic class with constraints

**What it is:** A single in-memory store that works for ANY class. Demonstrates C# reification — `GenericRepository<Student>` and `GenericRepository<Course>` are genuinely different types.

```csharp
// Utilities/GenericRepository.cs
namespace UniversityAnalytics.Utilities;

// T is a placeholder for "whatever type the caller chooses"
// where T : class = T must be a reference type (not int, not struct)
//   This ensures T can be null (nullable reference)
public class GenericRepository<T> where T : class
{
    private readonly List<T> _items = new();

    public void Add(T item) => _items.Add(item);

    // Func<T, bool> = a lambda like  s => s.Gpa > 3.5
    // Returns null if nothing matches — caller must check
    public T? FindFirst(Func<T, bool> predicate)
        => _items.FirstOrDefault(predicate);

    public List<T> FindAll(Func<T, bool> predicate)
        => _items.Where(predicate).ToList();

    public int Count() => _items.Count;

    // Generic METHOD inside a generic CLASS
    // TKey is a second type parameter — just for this method
    // where TKey : IComparable<TKey> = TKey must be comparable (int, double, string all qualify)
    public T? Max<TKey>(Func<T, TKey> selector) where TKey : IComparable<TKey>
        => _items.MaxBy(selector);
}
```

**What you get:**
```csharp
// Two completely different types at runtime — C# reification
var studentRepo = new GenericRepository<Student>();
var courseRepo  = new GenericRepository<Course>();

// studentRepo.GetType() ≠ courseRepo.GetType()
// In Java, both would just be GenericRepository at runtime (type erasure)

foreach (var s in SeedData.Students) studentRepo.Add(s);

studentRepo.FindFirst(s => s.Gpa > 3.8)
// Student { Id=1, FirstName=Anna, ... Gpa=3.9 }

studentRepo.FindAll(s => s.Major == "Computer Science")
// [Anna, Julia, Lena]

studentRepo.Max(s => s.Gpa)
// Student { Gpa=4.0, FirstName=Paweł, ... }

studentRepo.Count()  // 8
```

---

## `Utilities/CsvExporter.cs` — ref, out, optional parameters

**What it is:** Three different parameter passing styles that don't exist in Java.

```csharp
// Utilities/CsvExporter.cs
namespace UniversityAnalytics.Utilities;
using System.Text;
using UniversityAnalytics.Models;

public static class CsvExporter
{
    // Optional parameters: separator and includeHeader have defaults
    // Callers can omit them → Export(students)
    // Or override one  → Export(students, ";")
    // Or override both → Export(students, ",", false)
    public static string Export(
        List<Student> students,
        string separator    = ",",     // default value
        bool   includeHeader = true)   // default value
    {
        var sb = new StringBuilder();

        if (includeHeader)
            sb.AppendLine($"Id{separator}FullName{separator}Major{separator}Gpa{separator}Year");

        foreach (var s in students)
            sb.AppendLine($"{s.Id}{separator}{s.FullName}{separator}{s.Major}{separator}{s.Gpa:F1}{separator}{s.EnrollmentYear}");

        return sb.ToString();
    }

    // out parameter = method MUST assign csv and error before returning
    // Caller does NOT initialise them — out handles that
    // Pattern: try → success path. catch → error path. Always assigns both.
    public static bool TryExport(List<Student> students, out string csv, out string error)
    {
        try
        {
            csv   = Export(students);
            error = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            csv   = string.Empty;
            error = ex.Message;
            return false;
        }
    }

    // ref parameter = caller MUST initialise before passing
    // Method modifies the caller's actual variable (not a copy)
    public static void CountAndValidate(
        List<Student> students,
        ref int totalProcessed,   // caller's variable — method writes to it
        ref int invalidCount)
    {
        foreach (var s in students)
        {
            totalProcessed++;  // modifies the caller's actual int
            if (s.Gpa < 0 || s.Gpa > 4.0)
                invalidCount++;
        }
    }
}
```

**What you get:**
```csharp
// Optional parameters — three call styles
CsvExporter.Export(SeedData.Students);                        // comma separator, with header
CsvExporter.Export(SeedData.Students, ";");                   // semicolon, with header
CsvExporter.Export(SeedData.Students, ",", false);            // comma, NO header

// Export output:
// Id,FullName,Major,Gpa,Year
// 1,Anna Kowalska,Computer Science,3.9,2022
// 2,Marek Nowak,Mathematics,2.7,2021
// ...

// out parameters
if (CsvExporter.TryExport(SeedData.Students, out var csv, out var err))
    Console.WriteLine(csv);
else
    Console.WriteLine($"Error: {err}");
// csv is assigned by the method — we didn't have to initialise it

// ref parameters
int processed = 0;   // must initialise before passing with ref
int invalid   = 0;   // must initialise before passing with ref
CsvExporter.CountAndValidate(SeedData.Students, ref processed, ref invalid);
Console.WriteLine($"Processed: {processed}, Invalid: {invalid}");
// Processed: 8, Invalid: 0  (all GPAs are in range)
```

---
---

# PROJECT 3 — Notification Dispatcher

**What this project teaches:** All 5 SOLID principles together, async/await with `Task.WhenAll`, Strategy pattern, delegates, DI lifetimes.

---

## Why the folders are split this way

```
NotificationDispatcher/
├── Models/      ← notification data (what to send)
├── Senders/     ← HOW to send (email, SMS, push) — each is one Strategy
├── Services/    ← orchestration: dispatcher coordinates senders
├── Filters/     ← should we send at all? (business hours, rate limits)
└── Logging/     ← separate from sending (SRP: one class, one job)
```

**The key insight:** `NotificationDispatcher` depends only on `List<ISender>`. It has no idea whether those are email senders or SMS senders. Add `SlackSender` tomorrow — dispatcher doesn't change. That's OCP + DIP together.

---

## `Models/` — Abstract record hierarchy

```csharp
// Models/Notification.cs
namespace NotificationDispatcher.Models;

// abstract record = can't be created directly, subclasses must
// Positional params (Subject, Body) become init properties automatically
public abstract record Notification(string Subject, string Body)
{
    // Non-positional: added in the body, not in the constructor params
    // = DateTime.Now means it's set at object creation time
    public DateTime CreatedAt { get; } = DateTime.Now;
}

// Models/EmailNotification.cs
// : Notification(Subject, Body) = passes Subject+Body to the base record constructor
public record EmailNotification(
    string  To,
    string? Cc,       // nullable — CC is optional
    string  Subject,
    string  Body) : Notification(Subject, Body);

// Models/SmsNotification.cs
public record SmsNotification(
    string PhoneNumber,
    string Subject,
    string Body) : Notification(Subject, Body);

// Models/PushNotification.cs
public record PushNotification(
    string DeviceToken,
    string AppName,
    string Subject,
    string Body) : Notification(Subject, Body);
```

---

## `Senders/ISender.cs` + implementations

```csharp
// Senders/ISender.cs
namespace NotificationDispatcher.Senders;
using NotificationDispatcher.Models;

// The only contract: given a notification, send it asynchronously
public interface ISender
{
    Task SendAsync(Notification notification);
}

// Senders/EmailSender.cs
public class EmailSender : ISender
{
    public async Task SendAsync(Notification notification)
    {
        // 'is' pattern matching: checks the type AND casts in one step
        // 'email' is only in scope inside this if block
        if (notification is EmailNotification email)
            Console.WriteLine($"[EMAIL] To: {email.To} | Subject: {email.Subject}");

        await Task.Delay(50);  // simulates 50ms network call
    }
}

// Senders/SmsSender.cs
public class SmsSender : ISender
{
    public async Task SendAsync(Notification notification)
    {
        if (notification is SmsNotification sms)
            Console.WriteLine($"[SMS] To: {sms.PhoneNumber} | {sms.Subject}");

        await Task.Delay(30);
    }
}

// Senders/PushSender.cs
public class PushSender : ISender
{
    public async Task SendAsync(Notification notification)
    {
        if (notification is PushNotification push)
            Console.WriteLine($"[PUSH] App: {push.AppName} | {push.Subject}");

        await Task.Delay(20);
    }
}
```

---

## `Services/NotificationDispatcher.cs` — OCP + DIP in action

```csharp
// Services/NotificationDispatcher.cs
namespace NotificationDispatcher.Services;
using NotificationDispatcher.Models;
using NotificationDispatcher.Senders;

public class NotificationDispatcher
{
    // Depends on List<ISender> — an abstraction.
    // Does NOT know or care whether EmailSender, SmsSender, or SlackSender is in there.
    // This is DIP: high-level code depends on an interface, not a concrete class.
    private readonly List<ISender> _senders;

    public NotificationDispatcher(List<ISender> senders)
    {
        _senders = senders;
    }

    // Task.WhenAll: starts ALL senders at the same time, waits for ALL to finish
    // Total time ≈ slowest sender (50ms), NOT sum of all (50+30+20=100ms)
    // Compare: sequential foreach → total = 100ms
    public async Task DispatchAsync(Notification notification)
    {
        await Task.WhenAll(_senders.Select(s => s.SendAsync(notification)));
        // .Select creates tasks but doesn't await yet → all start simultaneously
        // Task.WhenAll waits until every task finishes
    }

    // OCP demo: to add SlackSender, you just add it to _senders at the call site.
    // This class never changes.
}
```

**What you get:**
```csharp
var senders    = new List<ISender> { new EmailSender(), new SmsSender(), new PushSender() };
var dispatcher = new NotificationDispatcher(senders);

var email = new EmailNotification("anna@uni.pl", null, "Welcome", "Hello!");
await dispatcher.DispatchAsync(email);

// Output (all three fire concurrently, total ≈50ms not 100ms):
// [EMAIL] To: anna@uni.pl | Subject: Welcome
// (SmsSender and PushSender fire but skip printing — email is not SmsNotification/PushNotification)

// Sequential would look like:
// foreach (var s in senders) await s.SendAsync(email);  // 50ms + 30ms + 20ms = 100ms

// Concurrent with WhenAll:
// var tasks = senders.Select(s => s.SendAsync(email));
// await Task.WhenAll(tasks);  // all fire at once, total ≈50ms
```

---

## `Filters/` — ISP in action

```csharp
// Filters/INotificationFilter.cs
// ISP: Filters and Senders are separate interfaces.
// A class implementing ISender is NOT forced to implement filter logic.
public interface INotificationFilter
{
    bool ShouldSend(Notification notification);
}

// Filters/BusinessHoursFilter.cs
public class BusinessHoursFilter : INotificationFilter
{
    // Returns true only between 8am and 6pm
    public bool ShouldSend(Notification notification)
    {
        int hour = DateTime.Now.Hour;
        return hour >= 8 && hour < 18;
    }
}

// Filters/RateLimitFilter.cs
public class RateLimitFilter : INotificationFilter
{
    private readonly int             _maxPerMinute;
    private readonly Queue<DateTime> _sentTimes = new();  // FIFO queue tracks when notifications were sent

    public RateLimitFilter(int maxPerMinute) => _maxPerMinute = maxPerMinute;

    public bool ShouldSend(Notification notification)
    {
        var oneMinuteAgo = DateTime.Now.AddMinutes(-1);

        // Remove timestamps older than 1 minute from the front of the queue
        // Peek() looks at front without removing. Dequeue() removes from front.
        while (_sentTimes.Count > 0 && _sentTimes.Peek() < oneMinuteAgo)
            _sentTimes.Dequeue();

        if (_sentTimes.Count >= _maxPerMinute)
            return false;  // limit reached

        _sentTimes.Enqueue(DateTime.Now);  // record this send time
        return true;
    }
}
```

---

## `Logging/ConsoleDispatchLogger.cs` — SRP in action

```csharp
// Logging/IDispatchLogger.cs
public interface IDispatchLogger
{
    void Log(Notification notification, bool wasSent);
}

// Logging/ConsoleDispatchLogger.cs
// SRP: This class does ONE thing — logs. It doesn't send, doesn't filter.
// If the log format changes, only this class changes.
// If the dispatch logic changes, only the dispatcher changes.
public class ConsoleDispatchLogger : IDispatchLogger
{
    public void Log(Notification notification, bool wasSent)
    {
        Console.WriteLine(
            $"[LOG] {notification.CreatedAt:HH:mm:ss} | Sent={wasSent} | {notification.Subject}");
    }
}
```

---

## `Program.cs` — Delegates + composition root

```csharp
// Program.cs
using NotificationDispatcher.Filters;
using NotificationDispatcher.Logging;
using NotificationDispatcher.Models;
using NotificationDispatcher.Senders;
using NotificationDispatcher.Services;

// Manual composition root — no DI container, wires everything by hand
var senders    = new List<ISender> { new EmailSender(), new SmsSender(), new PushSender() };
var dispatcher = new NotificationDispatcher(senders);
var filter     = new BusinessHoursFilter();
var limiter    = new RateLimitFilter(maxPerMinute: 5);
var logger     = new ConsoleDispatchLogger();

var notifications = new Notification[]
{
    new EmailNotification("anna@uni.pl", null, "Welcome",   "Hello!"),
    new SmsNotification("+48123456789",        "Alert",     "Low balance"),
    new PushNotification("device-abc", "MyApp","Promo",     "50% off"),
};

// Dispatch each, but only if the filter allows it
foreach (var n in notifications)
{
    bool shouldSend = filter.ShouldSend(n) && limiter.ShouldSend(n);
    logger.Log(n, shouldSend);
    if (shouldSend) await dispatcher.DispatchAsync(n);
}

// --- Delegates demo ---
// Action<T>     = takes T, returns void
Action<string>     log     = msg => Console.WriteLine($"[ACTION] {msg}");

// Func<T, TResult> = takes T, returns TResult
Func<Notification, bool> isEmail = n => n is EmailNotification;

// Predicate<T>  = takes T, returns bool (same as Func<T, bool>)
Predicate<Notification>  isUrgent = n => n.Subject.Contains("Alert");

log("Dispatch complete");                       // [ACTION] Dispatch complete
Console.WriteLine(isEmail(notifications[0]));   // True
Console.WriteLine(isUrgent(notifications[1]));  // True (Subject = "Alert")
```

---
---

# PROJECT 4 — Library API (ASP.NET Core — In-Memory)

**What this project teaches:** Controllers, routing, model binding, validation attributes, HTTP status codes, middleware pipeline order.

---

## Why the folders are split this way

```
LibraryApi/
├── Models/      ← database-like entities (Book, Loan) — what the app stores
├── Dtos/        ← request shapes (BookRequest) — what the API accepts
│                  kept separate because input validation ≠ domain model
├── Data/        ← InMemoryStore replaces a real database
├── Controllers/ ← HTTP layer only: parse request, call data, return status code
└── Middleware/  ← cross-cutting concerns: timing, global error handling
```

**The rule:** Controllers never contain business logic. `BookRequest` has validation annotations, `Book` doesn't — you validate input, not your stored data.

---

## `Models/Book.cs` + `Models/Loan.cs`

```csharp
// Models/Book.cs
namespace LibraryApi.Models;

public class Book
{
    public int    Id          { get; set; }
    public string Title       { get; set; } = string.Empty;
    public string Author      { get; set; } = string.Empty;
    public string ISBN        { get; set; } = string.Empty;
    public int    PageCount   { get; set; }
    public bool   IsAvailable { get; set; } = true;  // default: available when added
}

// Models/Loan.cs
public class Loan
{
    public int       Id          { get; set; }
    public int       BookId      { get; set; }
    public string    BorrowerId  { get; set; } = string.Empty;
    public DateTime  BorrowedAt  { get; set; }
    public DateTime? ReturnedAt  { get; set; }  // null = not yet returned; non-null = returned
}
```

---

## `Dtos/BookRequest.cs` — Validation attributes

**What it is:** The shape of data the API accepts. Attributes here tell ASP.NET Core what's valid before your code even runs.

```csharp
// Dtos/BookRequest.cs
namespace LibraryApi.Dtos;
using System.ComponentModel.DataAnnotations;

public class BookRequest
{
    // [Required] = cannot be null or empty
    // [StringLength] = must be between MinimumLength and max
    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string Title  { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Author { get; set; } = string.Empty;

    [Required]
    public string ISBN   { get; set; } = string.Empty;

    // [Range] = must be between 1 and 5000
    [Range(1, 5000)]
    public int PageCount { get; set; }
}

// Dtos/LoanRequest.cs — simple, no annotations needed (IDs are validated by checking the DB)
public record LoanRequest(int BookId, string BorrowerId);
```

---

## `Data/InMemoryStore.cs` — Replaces a database

```csharp
// Data/InMemoryStore.cs
namespace LibraryApi.Data;
using LibraryApi.Models;

// static = shared across the entire app lifetime (like a singleton)
public static class InMemoryStore
{
    private static int _nextBookId = 4;
    private static int _nextLoanId = 3;

    public static List<Book> Books { get; } = new()
    {
        new Book { Id=1, Title="Clean Code",         Author="Robert Martin", ISBN="978-0132350884", PageCount=431 },
        new Book { Id=2, Title="The Pragmatic Programmer", Author="Hunt & Thomas", ISBN="978-0135957059", PageCount=352, IsAvailable=false },
        new Book { Id=3, Title="Design Patterns",    Author="Gang of Four",   ISBN="978-0201633610", PageCount=395 },
    };

    public static List<Loan> Loans { get; } = new()
    {
        new Loan { Id=1, BookId=2, BorrowerId="student-001", BorrowedAt=DateTime.Now.AddDays(-5) },
        new Loan { Id=2, BookId=2, BorrowerId="student-002", BorrowedAt=DateTime.Now.AddDays(-10), ReturnedAt=DateTime.Now.AddDays(-3) },
    };

    public static int NextBookId() => _nextBookId++;
    public static int NextLoanId() => _nextLoanId++;
}
```

---

## `Controllers/BooksController.cs` — Full REST controller

```csharp
// Controllers/BooksController.cs
namespace LibraryApi.Controllers;
using LibraryApi.Data;
using LibraryApi.Dtos;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;

// [ApiController] = enables automatic model validation (400 if BookRequest fails annotations)
//                   infers [FromBody], [FromRoute], [FromQuery] from parameter types
// [Route("api/books")] = base path for all actions in this controller
[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    // [HttpGet] + no route = GET /api/books
    // [FromQuery] = reads from URL query string: /api/books?author=Martin&available=true
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll(
        [FromQuery] string? author,
        [FromQuery] bool?   available)
    {
        var books = InMemoryStore.Books.AsQueryable();

        if (author    != null) books = books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
        if (available != null) books = books.Where(b => b.IsAvailable == available);

        return Ok(books.ToList());  // 200 + JSON body
    }

    // [HttpGet("{id:int}")] = GET /api/books/5
    // {id:int} = route constraint — only matches integers (not /api/books/abc)
    [HttpGet("{id:int}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
        if (book == null) return NotFound();   // 404 — no body needed
        return Ok(book);                       // 200 + JSON
    }

    // [HttpPost] = POST /api/books
    // [FromBody] = reads JSON body, deserialises to BookRequest
    // [ApiController] auto-validates BookRequest annotations → 400 if invalid
    [HttpPost]
    public ActionResult<Book> Create([FromBody] BookRequest dto)
    {
        // Map DTO → domain model
        var book = new Book
        {
            Id        = InMemoryStore.NextBookId(),
            Title     = dto.Title,
            Author    = dto.Author,
            ISBN      = dto.ISBN,
            PageCount = dto.PageCount,
        };
        InMemoryStore.Books.Add(book);

        // 201 Created
        // nameof(GetById) = "GetById" — the action that can retrieve this new resource
        // new { id = book.Id } = route values for that action → Location: /api/books/4
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // [HttpPut("{id:int}")] = PUT /api/books/5
    // Full replacement: overwrite EVERY field from the DTO
    [HttpPut("{id:int}")]
    public ActionResult<Book> Update(int id, [FromBody] BookRequest dto)
    {
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
        if (book == null) return NotFound();

        // Replace all fields — PUT means full update
        book.Title     = dto.Title;
        book.Author    = dto.Author;
        book.ISBN      = dto.ISBN;
        book.PageCount = dto.PageCount;

        return Ok(book);  // 200 + updated resource
    }

    // [HttpDelete("{id:int}")] = DELETE /api/books/5
    // IActionResult (not ActionResult<T>) because we return 204 with no body
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
        if (book == null) return NotFound();

        InMemoryStore.Books.Remove(book);
        return NoContent();  // 204 — success, no body
    }
}
```

---

## `Controllers/LoansController.cs` — Conflict detection (409)

```csharp
// Controllers/LoansController.cs
[ApiController]
[Route("api/loans")]
public class LoansController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Loan>> GetAll(
        [FromQuery] int?  bookId,
        [FromQuery] bool? active)
    {
        var loans = InMemoryStore.Loans.AsQueryable();
        if (bookId != null) loans = loans.Where(l => l.BookId == bookId);
        if (active != null) loans = loans.Where(l => active.Value ? l.ReturnedAt == null : l.ReturnedAt != null);
        return Ok(loans.ToList());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Loan> GetById(int id)
    {
        var loan = InMemoryStore.Loans.FirstOrDefault(l => l.Id == id);
        return loan == null ? NotFound() : Ok(loan);
    }

    [HttpPost]
    public ActionResult<Loan> Create([FromBody] LoanRequest dto)
    {
        // Check book exists
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == dto.BookId);
        if (book == null) return NotFound(new { error = $"Book {dto.BookId} not found." });

        // 409 Conflict = request is VALID but server state prevents it
        // The book exists — but it's already borrowed. That's a conflict, not a bad request.
        if (!book.IsAvailable)
            return Conflict(new { error = "Book is already on loan." });

        var loan = new Loan
        {
            Id         = InMemoryStore.NextLoanId(),
            BookId     = dto.BookId,
            BorrowerId = dto.BorrowerId,
            BorrowedAt = DateTime.Now,
        };

        InMemoryStore.Loans.Add(loan);
        book.IsAvailable = false;  // mark book as unavailable

        return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Return(int id)
    {
        var loan = InMemoryStore.Loans.FirstOrDefault(l => l.Id == id);
        if (loan == null) return NotFound();

        // Already returned? That's a conflict — same concept as "book already on loan"
        if (loan.ReturnedAt != null)
            return Conflict(new { error = "This loan has already been returned." });

        loan.ReturnedAt = DateTime.Now;  // mark as returned

        // Find the book and make it available again
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == loan.BookId);
        if (book != null) book.IsAvailable = true;

        return NoContent();  // 204 — success, nothing to return
    }
}
```

---

## `Middleware/` — Pipeline components

```csharp
// Middleware/RequestTimingMiddleware.cs
namespace LibraryApi.Middleware;

public class RequestTimingMiddleware
{
    // RequestDelegate = the next component in the pipeline
    private readonly RequestDelegate                    _next;
    private readonly ILogger<RequestTimingMiddleware>   _logger;

    // Constructor: ASP.NET Core injects _next and _logger automatically
    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    // InvokeAsync is called for every HTTP request
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            await _next(context);  // pass request to the next middleware / controller
        }
        finally
        {
            // finally = runs even if the controller threw an exception
            sw.Stop();
            _logger.LogInformation(
                "{Method} {Path} completed in {Ms}ms",
                context.Request.Method,
                context.Request.Path,
                sw.Elapsed.TotalMilliseconds);
        }
    }
}

// Middleware/ExceptionHandlingMiddleware.cs
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate                       _next;
    private readonly ILogger<ExceptionHandlingMiddleware>  _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            // Set status code and write JSON response
            context.Response.StatusCode  = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { message = "An unexpected error occurred." });
        }
    }
}
```

---

## `Program.cs` — Middleware order matters

```csharp
// Program.cs
using LibraryApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

// ORDER IS CRITICAL — each request flows top to bottom
app.UseMiddleware<ExceptionHandlingMiddleware>();   // FIRST: catches any exception from below
app.UseMiddleware<RequestTimingMiddleware>();       // wraps everything after it
app.UseRouting();                                  // matches URL to controller action
app.MapControllers();                              // executes matched controller action

app.Run();
```

**Request flow visualised:**
```
HTTP Request
    ↓
ExceptionHandlingMiddleware.InvokeAsync  ← try block starts
    ↓
RequestTimingMiddleware.InvokeAsync  ← stopwatch starts
    ↓
UseRouting → matches GET /api/books
    ↓
BooksController.GetAll()  ← actual logic runs
    ↑
Response bubbles back up through each middleware
```

---
---

# PROJECT 5 — Clinic Management API (ADO.NET + 3-Layer)

**What this project teaches:** Full 3-layer architecture with a real SQL Server database. Parameterised queries, transactions, `SCOPE_IDENTITY()`, null column handling, DI registration.

---

## Why the folders are split this way

```
ClinicApi/
├── Models/        ← raw domain entities (Patient, Doctor, Appointment)
├── Dtos/          ← shapes for HTTP in/out — input validation + clean output
├── Repositories/  ← SQL only. Knows about tables and queries. Nothing else.
├── Services/      ← business rules. Doesn't know about SQL. Calls repositories.
└── Controllers/   ← HTTP only. Knows about status codes. Calls services.
```

**The 3-layer rule:** A controller never writes SQL. A service never reads `HttpContext`. A repository never makes business decisions. Each layer has exactly one reason to change.

---

## Database setup (run once)

```sql
CREATE DATABASE ClinicDb;
GO
USE ClinicDb;

CREATE TABLE Doctors (
    Id        INT PRIMARY KEY IDENTITY,
    Name      NVARCHAR(100) NOT NULL,
    Specialty NVARCHAR(100) NOT NULL
);

CREATE TABLE Patients (
    Id        INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName  NVARCHAR(100) NOT NULL,
    Pesel     NVARCHAR(11)  UNIQUE NOT NULL,  -- UNIQUE prevents duplicates at DB level
    Email     NVARCHAR(200) NULL              -- NULL allowed
);

CREATE TABLE Appointments (
    Id          INT PRIMARY KEY IDENTITY,
    PatientId   INT          NOT NULL REFERENCES Patients(Id),
    DoctorId    INT          NOT NULL REFERENCES Doctors(Id),
    ScheduledAt DATETIME2    NOT NULL,
    DurationMin INT          NOT NULL DEFAULT 30,
    Status      NVARCHAR(20) NOT NULL DEFAULT 'scheduled'
);

-- Seed data
INSERT INTO Doctors VALUES ('Jan Kowalski', 'Cardiology');
INSERT INTO Doctors VALUES ('Anna Nowak',   'Neurology');
INSERT INTO Patients VALUES ('Marek', 'Zając',    '90010112345', 'marek@email.pl');
INSERT INTO Patients VALUES ('Ewa',   'Lisowska', '85060567890', NULL);
```

---

## `appsettings.json` — Never hardcode credentials

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ClinicDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
  }
}
```

---

## Models + DTOs

```csharp
// Models/Patient.cs
public class Patient
{
    public int     Id        { get; set; }
    public string  FirstName { get; set; } = string.Empty;
    public string  LastName  { get; set; } = string.Empty;
    public string  Pesel     { get; set; } = string.Empty;
    public string? Email     { get; set; }  // nullable — patient may not have email
}

// Models/Doctor.cs
public class Doctor
{
    public int    Id        { get; set; }
    public string Name      { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
}

// Models/Appointment.cs
public class Appointment
{
    public int      Id          { get; set; }
    public int      PatientId   { get; set; }
    public int      DoctorId    { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int      DurationMin { get; set; }
    public string   Status      { get; set; } = "scheduled";
}

// Dtos/PatientRequest.cs
using System.ComponentModel.DataAnnotations;
public class PatientRequest
{
    [Required][MaxLength(100)]         public string  FirstName { get; set; } = string.Empty;
    [Required][MaxLength(100)]         public string  LastName  { get; set; } = string.Empty;
    [Required][StringLength(11, MinimumLength=11)] public string Pesel { get; set; } = string.Empty;
    [EmailAddress]                     public string? Email     { get; set; }
}

// Dtos/AppointmentRequest.cs
public class AppointmentRequest
{
    [Required]       public int      PatientId   { get; set; }
    [Required]       public int      DoctorId    { get; set; }
    [Required]       public DateTime ScheduledAt { get; set; }
    [Range(10, 120)] public int      DurationMin { get; set; } = 30;
}

// Dtos/AppointmentResult.cs — what we return (joins Patient + Doctor name)
public record AppointmentResult(
    int      Id,
    string   PatientFullName,
    string   DoctorName,
    DateTime ScheduledAt,
    int      DurationMin,
    string   Status);
```

---

## `Repositories/SqlPatientRepository.cs` — Full ADO.NET implementation

```csharp
// Repositories/SqlPatientRepository.cs
namespace ClinicApi.Repositories;
using ClinicApi.Models;
using Microsoft.Data.SqlClient;

public class SqlPatientRepository : IPatientRepository
{
    private readonly string _cs;

    // IConfiguration injected by DI — reads the connection string from appsettings.json
    public SqlPatientRepository(IConfiguration config)
        => _cs = config.GetConnectionString("DefaultConnection")!;

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        var result = new List<Patient>();

        // 'using var' = Dispose() called automatically when scope ends
        // OpenAsync() = non-blocking, returns thread to pool while waiting
        using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();

        using var cmd = new SqlCommand(
            "SELECT Id, FirstName, LastName, Pesel, Email FROM Patients", conn);

        using var reader = await cmd.ExecuteReaderAsync();

        // Cache ordinals ONCE outside the loop — faster than string lookup per row
        int idOrd = reader.GetOrdinal("Id");
        int fnOrd = reader.GetOrdinal("FirstName");
        int lnOrd = reader.GetOrdinal("LastName");
        int peOrd = reader.GetOrdinal("Pesel");
        int emOrd = reader.GetOrdinal("Email");

        while (await reader.ReadAsync())
        {
            result.Add(new Patient
            {
                Id        = reader.GetInt32(idOrd),
                FirstName = reader.GetString(fnOrd),
                LastName  = reader.GetString(lnOrd),
                Pesel     = reader.GetString(peOrd),
                // IsDBNull MUST be checked before GetString on a nullable column
                // Skip this check → InvalidCastException at runtime if value is NULL in DB
                Email     = reader.IsDBNull(emOrd) ? null : reader.GetString(emOrd),
            });
        }
        return result;
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();

        // @Id = parameterised — never concatenate user input into SQL
        using var cmd = new SqlCommand(
            "SELECT Id, FirstName, LastName, Pesel, Email FROM Patients WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;  // no row → return null

        return new Patient
        {
            Id        = reader.GetInt32(0),
            FirstName = reader.GetString(1),
            LastName  = reader.GetString(2),
            Pesel     = reader.GetString(3),
            Email     = reader.IsDBNull(4) ? null : reader.GetString(4),
        };
    }

    public async Task<Patient?> GetByPeselAsync(string pesel)
    {
        using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();
        using var cmd = new SqlCommand(
            "SELECT Id, FirstName, LastName, Pesel, Email FROM Patients WHERE Pesel = @Pesel", conn);
        cmd.Parameters.AddWithValue("@Pesel", pesel);
        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;
        return new Patient
        {
            Id=reader.GetInt32(0), FirstName=reader.GetString(1),
            LastName=reader.GetString(2), Pesel=reader.GetString(3),
            Email=reader.IsDBNull(4) ? null : reader.GetString(4),
        };
    }

    public async Task<int> AddAsync(Patient patient)
    {
        using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();

        // Two SQL statements in one command string:
        // 1. INSERT the row
        // 2. SELECT SCOPE_IDENTITY() = the Id the database auto-generated
        using var cmd = new SqlCommand(@"
            INSERT INTO Patients (FirstName, LastName, Pesel, Email)
            VALUES (@FirstName, @LastName, @Pesel, @Email);
            SELECT SCOPE_IDENTITY();", conn);

        cmd.Parameters.AddWithValue("@FirstName", patient.FirstName);
        cmd.Parameters.AddWithValue("@LastName",  patient.LastName);
        cmd.Parameters.AddWithValue("@Pesel",     patient.Pesel);
        // C# null ≠ SQL NULL — must convert explicitly
        cmd.Parameters.AddWithValue("@Email", (object?)patient.Email ?? DBNull.Value);

        // ExecuteScalarAsync returns first column of first row = the new Id
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();
        using var cmd = new SqlCommand("DELETE FROM Patients WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        // ExecuteNonQueryAsync returns rows affected
        // 0 rows = ID didn't exist → return false → controller returns 404
        int rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }
}
```

---

## `Repositories/SqlAppointmentRepository.cs` — Transaction pattern

```csharp
// Repositories/SqlAppointmentRepository.cs
public class SqlAppointmentRepository : IAppointmentRepository
{
    private readonly string _cs;
    public SqlAppointmentRepository(IConfiguration config)
        => _cs = config.GetConnectionString("DefaultConnection")!;

    public async Task<IEnumerable<AppointmentResult>> GetAllAsync(int? doctorId, DateTime? date)
    {
        var result = new List<AppointmentResult>();
        using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();

        // JOIN across 3 tables in one query — no round trips
        // @DoctorId IS NULL = if the parameter is null, skip that filter
        using var cmd = new SqlCommand(@"
            SELECT a.Id,
                   p.FirstName + ' ' + p.LastName AS PatientFullName,
                   d.Name AS DoctorName,
                   a.ScheduledAt, a.DurationMin, a.Status
            FROM Appointments a
            JOIN Patients p ON a.PatientId = p.Id
            JOIN Doctors  d ON a.DoctorId  = d.Id
            WHERE (@DoctorId IS NULL OR a.DoctorId = @DoctorId)
              AND (@Date IS NULL OR CAST(a.ScheduledAt AS DATE) = @Date)", conn);

        // Nullable int/DateTime → must pass DBNull.Value if null
        cmd.Parameters.AddWithValue("@DoctorId", (object?)doctorId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Date",     (object?)date     ?? DBNull.Value);

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new AppointmentResult(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetDateTime(3),
                reader.GetInt32(4),
                reader.GetString(5)));
        }
        return result;
    }

    // Full transaction: validates patient + doctor exist, checks no time overlap, then inserts
    public async Task<int> AddWithTransactionAsync(Appointment appt)
    {
        using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();
        using var tx = conn.BeginTransaction();  // start transaction

        try
        {
            // Step 1: patient exists?
            using var checkPatient = new SqlCommand(
                "SELECT COUNT(*) FROM Patients WHERE Id = @Id", conn, tx);  // ← tx is 3rd argument!
            checkPatient.Parameters.AddWithValue("@Id", appt.PatientId);
            if ((int)(await checkPatient.ExecuteScalarAsync())! == 0)
                throw new KeyNotFoundException($"Patient {appt.PatientId} not found.");

            // Step 2: doctor exists?
            using var checkDoctor = new SqlCommand(
                "SELECT COUNT(*) FROM Doctors WHERE Id = @Id", conn, tx);
            checkDoctor.Parameters.AddWithValue("@Id", appt.DoctorId);
            if ((int)(await checkDoctor.ExecuteScalarAsync())! == 0)
                throw new KeyNotFoundException($"Doctor {appt.DoctorId} not found.");

            // Step 3: no time overlap for this doctor
            // Overlap = the existing appointment's window overlaps the new appointment's window
            // Computed in SQL — avoids loading all appointments into C# just to check
            using var checkOverlap = new SqlCommand(@"
                SELECT COUNT(*) FROM Appointments
                WHERE DoctorId = @DoctorId
                  AND Status != 'cancelled'
                  AND @Start < DATEADD(minute, DurationMin, ScheduledAt)
                  AND DATEADD(minute, @Dur, @Start) > ScheduledAt", conn, tx);
            checkOverlap.Parameters.AddWithValue("@DoctorId", appt.DoctorId);
            checkOverlap.Parameters.AddWithValue("@Start",    appt.ScheduledAt);
            checkOverlap.Parameters.AddWithValue("@Dur",      appt.DurationMin);
            if ((int)(await checkOverlap.ExecuteScalarAsync())! > 0)
                throw new InvalidOperationException("Doctor already has an appointment at this time.");

            // Step 4: insert — all previous checks passed
            using var insert = new SqlCommand(@"
                INSERT INTO Appointments (PatientId, DoctorId, ScheduledAt, DurationMin, Status)
                VALUES (@PatientId, @DoctorId, @ScheduledAt, @DurationMin, 'scheduled');
                SELECT SCOPE_IDENTITY();", conn, tx);
            insert.Parameters.AddWithValue("@PatientId",   appt.PatientId);
            insert.Parameters.AddWithValue("@DoctorId",    appt.DoctorId);
            insert.Parameters.AddWithValue("@ScheduledAt", appt.ScheduledAt);
            insert.Parameters.AddWithValue("@DurationMin", appt.DurationMin);

            var newId = Convert.ToInt32(await insert.ExecuteScalarAsync());

            // All 4 steps succeeded → make them permanent
            await tx.CommitAsync();
            return newId;
        }
        catch
        {
            // ANY failure → undo everything — the DB is unchanged
            await tx.RollbackAsync();
            throw;  // re-throw preserving original stack trace (throw; not throw ex;)
        }
    }

    public async Task<bool> CancelAsync(int id)
    {
        using var conn = new SqlConnection(_cs);
        await conn.OpenAsync();
        using var cmd = new SqlCommand(
            "UPDATE Appointments SET Status = 'cancelled' WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        return await cmd.ExecuteNonQueryAsync() > 0;
    }
}
```

---

## `Services/PatientService.cs` — Business rules live here

```csharp
// Services/PatientService.cs
namespace ClinicApi.Services;
using ClinicApi.Dtos;
using ClinicApi.Models;
using ClinicApi.Repositories;

public class PatientService : IPatientService
{
    // Depends on the interface, not the concrete class (DIP)
    private readonly IPatientRepository _repo;

    public PatientService(IPatientRepository repo) => _repo = repo;

    public Task<IEnumerable<Patient>> GetAllAsync() => _repo.GetAllAsync();

    public async Task<Patient> GetByIdAsync(int id)
    {
        // Business rule: throw a typed exception if not found
        // Controller catches this and maps it to 404
        var p = await _repo.GetByIdAsync(id);
        return p ?? throw new KeyNotFoundException($"Patient {id} not found.");
    }

    public async Task<int> AddAsync(PatientRequest dto)
    {
        // Business rule: PESEL must be unique
        // The repository just runs SQL — it doesn't know about this rule
        var existing = await _repo.GetByPeselAsync(dto.Pesel);
        if (existing != null)
            throw new InvalidOperationException("A patient with this PESEL is already registered.");

        var patient = new Patient
        {
            FirstName = dto.FirstName,
            LastName  = dto.LastName,
            Pesel     = dto.Pesel,
            Email     = dto.Email,
        };
        return await _repo.AddAsync(patient);
    }
}
```

---

## `Controllers/PatientsController.cs` — HTTP layer only

```csharp
// Controllers/PatientsController.cs
namespace ClinicApi.Controllers;
using ClinicApi.Dtos;
using ClinicApi.Models;
using ClinicApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _service;

    // DI injects IPatientService → resolved to PatientService (registered in Program.cs)
    public PatientsController(IPatientService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Patient>> GetById(int id)
    {
        try   { return Ok(await _service.GetByIdAsync(id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add([FromBody] PatientRequest dto)
    {
        try
        {
            int id = await _service.AddAsync(dto);
            // 201 Created + Location: /api/patients/3
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (InvalidOperationException ex)
        {
            // 409 Conflict — PESEL already registered
            return Conflict(new { error = ex.Message });
        }
    }
}
```

---

## `Controllers/AppointmentsController.cs`

```csharp
[ApiController]
[Route("api/appointments")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _service;
    public AppointmentsController(IAppointmentService service) => _service = service;

    // [FromQuery] = reads from URL: /api/appointments?doctorId=1&date=2025-04-15
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentResult>>> GetAll(
        [FromQuery] int?      doctorId,
        [FromQuery] DateTime? date)
        => Ok(await _service.GetAllAsync(doctorId, date));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] AppointmentRequest dto)
    {
        try
        {
            int id = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id }, id);
        }
        catch (KeyNotFoundException ex)
            { return NotFound(new { error = ex.Message }); }    // patient or doctor not found
        catch (InvalidOperationException ex)
            { return Conflict(new { error = ex.Message }); }    // time slot taken
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancel(int id)
    {
        bool ok = await _service.CancelAsync(id);
        return ok ? NoContent() : NotFound();   // 204 if cancelled, 404 if not found
    }
}
```

---

## `Program.cs` — DI registration & middleware

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// 3-layer DI registration:
// Each HTTP request gets its OWN set of repository + service instances (Scoped)
// They share the same connection string but each has its own SqlConnection lifecycle
builder.Services.AddScoped<IPatientRepository,     SqlPatientRepository>();
builder.Services.AddScoped<IAppointmentRepository, SqlAppointmentRepository>();
builder.Services.AddScoped<IPatientService,        PatientService>();
builder.Services.AddScoped<IAppointmentService,    AppointmentService>();

var app = builder.Build();

// Exception middleware FIRST — catches anything thrown below it
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.MapControllers();

app.Run();
```

**What happens for one request:**
```
POST /api/patients
    ↓
ExceptionHandlingMiddleware  (outer safety net)
    ↓
PatientsController.Add()
    ↓
PatientService.AddAsync()     ← business rule: PESEL unique?
    ↓
SqlPatientRepository.GetByPeselAsync()  ← SELECT ... WHERE Pesel = @Pesel
SqlPatientRepository.AddAsync()         ← INSERT ... ; SELECT SCOPE_IDENTITY()
    ↑
returns new Id = 3
    ↑
PatientService returns 3
    ↑
PatientsController: return CreatedAtAction(...)  → 201 + Location: /api/patients/3
```

---

## Quick-reference: every HTTP status code used

| Code | Method | When |
|------|--------|------|
| 200 OK | `Ok(data)` | GET succeeded with results |
| 201 Created | `CreatedAtAction(...)` | POST succeeded, new resource created |
| 204 No Content | `NoContent()` | DELETE / return succeeded, nothing to show |
| 400 Bad Request | auto (ApiController) | Validation attribute failed |
| 404 Not Found | `NotFound()` | Resource with that ID doesn't exist |
| 409 Conflict | `Conflict(...)` | Valid request, but state prevents it (book on loan, PESEL taken, slot booked) |
| 500 Internal | middleware | Unhandled exception |

## Quick-reference: every exam answer

| Question | Answer |
|----------|--------|
| `abstract class` vs `interface` | Abstract: fields, constructors, non-abstract methods. Interface: contract only, no state. |
| `override` vs `new` | `override` → polymorphism works through base-type reference. `new` → hides, base version still called via base reference. |
| Record equality | Records compare by value (all fields). Classes compare by reference by default. |
| Deferred execution | `Where`, `Select`, `OrderBy` are lazy. Run when you call `ToList`, `First`, `foreach`. |
| `Join` vs `GroupJoin` | `Join` = inner join (only matching rows). `GroupJoin` = left outer join (all left-side rows appear). |
| `Task.WhenAll` vs sequential | `WhenAll` ≈ slowest task. Sequential = sum of all tasks. |
| `ref` vs `out` | `ref`: caller must assign first. `out`: method must assign before return. |
| Reification | C# keeps generic type at runtime. Java erases it. |
| `yield return` | Lazy — one element generated at a time. Nothing runs until iterated. |
| `StringBuilder` vs `+=` | `+=` in loop = O(n²). `StringBuilder.Append` = O(1) amortised. |
| Scoped in Singleton | Captive dependency — Scoped never released. Memory leak. |
| `SCOPE_IDENTITY()` | ID of last INSERT in this scope. Safer than `@@IDENTITY` (unaffected by triggers). |
| `cmd.Transaction = tx` | Every SqlCommand inside a transaction MUST have this. Without it → `InvalidOperationException`. |
| Middleware order | Exception handler first. Auth before authz. `MapControllers` last. |
| `CreatedAtAction` vs `Created` | `CreatedAtAction` = 201 + Location header. `Created("")` = 201 but no Location. |
| 400 vs 409 | 400 = request malformed. 409 = request valid, server state prevents it. |
