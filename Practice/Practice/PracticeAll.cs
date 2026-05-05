// // ============================================================
// //  APBD C# EXAM PRACTICE — Complete Coding Workbook
// //  Polish-Japanese Academy of IT
// //
// //  HOW TO USE:
// //  1. Read each TODO comment carefully.
// //  2. Write your code where marked.
// //  3. Uncomment the test calls in Main() one section at a time.
// //  4. Every section maps to a real exam topic from the lectures.
// //
// //  TOPICS COVERED (in order):
// //   Part 1  — Git Knowledge Checks (code + comments)
// //   Part 2  — .NET Architecture (CLR, JIT, AOT, IDisposable)
// //   Part 3  — Value Types vs Reference Types
// //   Part 4  — Properties (auto, init, computed, validated)
// //   Part 5  — Null Safety (??, ?., ??=)
// //   Part 6  — Records & 'with' expressions
// //   Part 7  — Delegates, Action, Func, Predicate, Closures
// //   Part 8  — async / await / Task
// //   Part 9  — switch Expressions & Pattern Matching
// //   Part 10 — Collections (List, Dictionary, HashSet, Queue, Stack)
// //   Part 11 — Abstract Classes, virtual/override vs new (hiding)
// //   Part 12 — Interfaces (multi-implement, default members)
// //   Part 13 — Custom Exceptions & rethrow
// //   Part 14 — SOLID: SRP
// //   Part 15 — SOLID: OCP + Strategy Pattern
// //   Part 16 — SOLID: LSP
// //   Part 17 — SOLID: ISP
// //   Part 18 — SOLID: DIP + IoC / DI Lifetimes
// //   Part 19 — Cohesion & Coupling theory (comment tasks)
// //   Part 20 — Clean Code / DRY
// //   Part 21 — LINQ: deferred execution trap
// //   Part 22 — LINQ: Where, Select, OrderBy, ThenBy
// //   Part 23 — LINQ: Any, All, Count, First, FirstOrDefault
// //   Part 24 — LINQ: GroupBy, Join, GroupJoin, SelectMany
// //   Part 25 — LINQ: Skip/Take (paging), Distinct, Aggregate
// //   Part 26 — ref / out / named / optional parameters
// //   Part 27 — Generics (reification vs type erasure)
// //   Part 28 — yield return (lazy sequences)
// //   Part 29 — Strings & StringBuilder
// //   Part 30 — Tuples & Deconstruction
// //   Part 31 — Adapter Pattern
// //   Part 32 — Factory Method & Abstract Factory
// //   Part 33 — Singleton Pattern
// //   Part 34 — Exam Simulation: Git scenario (comment)
// //   Part 35 — Exam Simulation: OOP + DIP
// //   Part 36 — Exam Simulation: LINQ Analytics
// // ============================================================
//
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
//
// // ─────────────────────────────────────────────────────────────
// // ENTRY POINT — uncomment sections one by one as you finish them
// // ─────────────────────────────────────────────────────────────
// class Program
// {
//     static async Task Main()
//     {
//         // Part03_ValueVsReference.Demo();
//         // Part04_Properties.Demo();
//         // Part05_NullSafety.Demo();
//         // Part06_Records.Demo();
//         // Part07_Delegates.Demo();
//         // await Part08_AsyncAwait.Demo();
//         // Part09_SwitchPatterns.Demo();
//         // Part10_Collections.Demo();
//         // Part11_AbstractInheritance.Demo();
//         // Part12_Interfaces.Demo();
//         // Part13_Exceptions.Demo();
//         // Part15_OCP_Strategy.Demo();
//         // Part16_LSP.Demo();
//         // Part18_DIP.Demo();
//         // Part21_DeferredExecution.Demo();
//         // Part22_LinqBasics.Demo();
//         // Part23_LinqQuantifiers.Demo();
//         // Part24_LinqJoins.Demo();
//         // Part25_LinqPaging.Demo();
//         // Part26_RefOut.Demo();
//         // Part27_Generics.Demo();
//         // Part28_YieldReturn.Demo();
//         // Part29_Strings.Demo();
//         // Part30_Tuples.Demo();
//         // Part31_Adapter.Demo();
//         // Part32_Factory.Demo();
//         // Part33_Singleton.Demo();
//         // await Part35_ExamSimOOP.Demo();
//         // Part36_ExamSimLinq.Demo();
//
//         Console.WriteLine("Done! Uncomment sections in Main() to test each part.");
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 1 — GIT KNOWLEDGE CHECKS
// // ─────────────────────────────────────────────────────────────
// // These are comment-only tasks. Answer them in comments below each question.
// // Exams often ask you to write the exact git commands.
//
// static class Part01_Git
// {
//     // TASK 1-A: Write the git commands to:
//     //   1. Set your global name and email
//     //   2. Create a new local repo
//     //   3. Stage all files and make a commit
//     //   4. Create and switch to a branch called feature/login
//     //
//     // TODO: Write your answers here as comments:
//     // git config --global user.name "..."
//     // ...
//
//     // TASK 1-B: Explain the difference between git merge and git rebase.
//     //   - Which produces linear history?
//     //   - When should you NEVER rebase?
//     //
//     // TODO: Your explanation here:
//     // ...
//
//     // TASK 1-C: You are on feature/reports. Main received new commits.
//     //   Your team lead wants LINEAR history. Write every command:
//     //   from "catch up with main" to "feature/reports deleted".
//     //
//     // TODO:
//     // ...
//
//     // TASK 1-D: What does a conflict look like in a file?
//     //   Write the 4 steps to resolve a conflict during a rebase.
//     //   IMPORTANT: what must you do AFTER editing the file, beyond 'git add'?
//     //
//     // TODO:
//     // ...
//
//     // TASK 1-E: What is --ff-only and why is it used after a rebase?
//     // TODO:
//     // ...
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 2 — .NET ARCHITECTURE (CLR, JIT, AOT, IDisposable)
// // ─────────────────────────────────────────────────────────────
// static class Part02_DotNetArchitecture
// {
//     // TASK 2-A: In comments, describe what each component does:
//     //   Roslyn, CLR, JIT, AOT, BCL, NuGet
//     //   Include the Java equivalent for each.
//     //
//     // TODO:
//     // Roslyn = ...
//     // CLR    = ...
//     // JIT    = ...   (exam: compiles IL at _______ time)
//     // AOT    = ...   (exam: compiles IL _______ the app runs)
//     // BCL    = ...
//     // NuGet  = ...
//
//     // TASK 2-B: Implement IDisposable correctly on a class that wraps a resource.
//     //   The using statement should automatically call Dispose().
//     //
//     // TODO: uncomment and complete
//
//     // public class ManagedFile : IDisposable
//     // {
//     //     private System.IO.FileStream? _stream;
//     //
//     //     public ManagedFile(string path)
//     //     {
//     //         // TODO: open the file for reading
//     //     }
//     //
//     //     public void Dispose()
//     //     {
//     //         // TODO: dispose the stream safely
//     //     }
//     // }
//     //
//     // public static void Demo()
//     // {
//     //     // TODO: use 'using var' to open a file — Dispose() is called automatically
//     // }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 3 — VALUE TYPES vs REFERENCE TYPES
// // ─────────────────────────────────────────────────────────────
// static class Part03_ValueVsReference
// {
//     // TASK 3-A: Create a struct Point with X and Y.
//     //   Show that copying a struct creates an independent copy.
//     //
//     // TODO: define struct Point here
//     // struct Point { ... }
//
//     // TASK 3-B: Create a class Color with R, G, B.
//     //   Show that copying a class variable shares the reference.
//     //
//     // TODO: define class Color here
//
//     // TASK 3-C: Floating-point trap — classic exam question.
//     //   Show that 0.1 + 0.2 != 0.3 with double.
//     //   Then fix it using decimal.
//     //
//     public static void Demo()
//     {
//         // TODO: struct copy independence
//         // var p1 = new Point { X = 1, Y = 2 };
//         // var p2 = p1;
//         // p2.X = 99;
//         // Console.WriteLine(p1.X); // expected: 1
//
//         // TODO: class reference sharing
//         // var c1 = new Color { R = 255 };
//         // var c2 = c1;
//         // c2.R = 128;
//         // Console.WriteLine(c1.R); // expected: 128
//
//         // TODO: floating point trap
//         // double d = 0.1 + 0.2;
//         // Console.WriteLine(d == 0.3);   // expected: False
//         // decimal m = 0.1m + 0.2m;
//         // Console.WriteLine(m == 0.3m);  // expected: True
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 4 — PROPERTIES (all 5 forms)
// // ─────────────────────────────────────────────────────────────
// static class Part04_Properties
// {
//     // TASK 4-A: Create a BankAccount class with:
//     //   - AccountNumber: init-only (cannot change after object init)
//     //   - Balance: private set (only the class can change it)
//     //   - Owner: validated — throw ArgumentException if empty/whitespace
//     //   - IsRich: computed property (returns true if Balance > 10_000)
//     //   - Constructor accepting accountNumber, owner, initialBalance
//     //   - Deposit(decimal amount) and Withdraw(decimal amount) with overdraft check
//     //   - Override ToString() to show account nicely
//
//     // TODO: write BankAccount here
//
//     public static void Demo()
//     {
//         // TODO: create a BankAccount, deposit, withdraw, print
//         // var acc = new BankAccount("PL001", "Anna", 1000m);
//         // acc.Deposit(500m);
//         // acc.Withdraw(200m);
//         // Console.WriteLine(acc);
//
//         // TODO: prove init-only by trying (and explaining why this line won't compile):
//         // acc.AccountNumber = "NEW"; // COMPILER ERROR — init only!
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 5 — NULL SAFETY (??, ?., ??=)
// // ─────────────────────────────────────────────────────────────
// static class Part05_NullSafety
// {
//     public static void Demo()
//     {
//         // TASK 5-A: Use ?? to provide a fallback when a string is null
//         string? maybeNull = null;
//         // TODO: string result = ...
//         // Console.WriteLine(result); // expected: "default"
//
//         // TASK 5-B: Use ?. to safely access Length without NullReferenceException
//         // TODO: int? len = ...
//         // Console.WriteLine(len); // expected: (nothing — null)
//
//         // TASK 5-C: Use ??= to assign only when null
//         string? name = null;
//         // TODO: name ??= "Anonymous";
//         // Console.WriteLine(name); // expected: Anonymous
//
//         // TASK 5-D: Chain ?. and ?? together
//         string? input = null;
//         // TODO: string upper = input?.Trim()?.ToUpper() ?? "EMPTY";
//         // Console.WriteLine(upper); // expected: EMPTY
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 6 — RECORDS & 'with' EXPRESSIONS
// // ─────────────────────────────────────────────────────────────
// static class Part06_Records
// {
//     // TASK 6-A: Define a positional record Student(int Id, string Name, string City, string Email)
//     // TODO:
//     // public record Student(int Id, string Name, string City, string Email);
//
//     public static void Demo()
//     {
//         // TASK 6-B: Create two Student records with identical values.
//         //   Show they are equal (value equality, not reference equality).
//         // TODO:
//         // var s1 = new Student(1, "Anna", "Warsaw", "anna@uni.pl");
//         // var s3 = new Student(1, "Anna", "Warsaw", "anna@uni.pl");
//         // Console.WriteLine(s1 == s3); // expected: True
//
//         // TASK 6-C: Use 'with' to create a copy with a different City.
//         //   Prove the original is unchanged.
//         // TODO:
//         // var s2 = s1 with { City = "Krakow" };
//         // Console.WriteLine(s2.City); // Krakow
//         // Console.WriteLine(s1.City); // Warsaw — original untouched
//
//         // TASK 6-D: Deconstruct the record into separate variables
//         // TODO:
//         // var (id, name, city, email) = s1;
//         // Console.WriteLine($"{name} from {city}");
//
//         // TASK 6-E: Print the record — observe the auto-generated ToString()
//         // TODO:
//         // Console.WriteLine(s1);
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 7 — DELEGATES, ACTION, FUNC, PREDICATE, CLOSURES
// // ─────────────────────────────────────────────────────────────
// static class Part07_Delegates
// {
//     // TASK 7-A: Create an Action<string> that prints to console with a prefix
//     // TODO:
//     // Action<string> printWithPrefix = msg => Console.WriteLine(">> " + msg);
//
//     // TASK 7-B: Create a Func<int, int, string> that formats addition
//     // TODO:
//     // Func<int, int, string> addFormat = (a, b) => $"{a}+{b}={a+b}";
//
//     // TASK 7-C: Create a Predicate<int> that checks if a number is positive
//     // TODO:
//     // Predicate<int> isPositive = n => n > 0;
//
//     // TASK 7-D: CLOSURE TRAP — the most classic exam question!
//     //   Create a list of 3 lambdas capturing 'i' from a loop.
//     //   Run them. What prints? Why? (Answer: 3 3 3)
//     //   Fix it by capturing a copy of i.
//     public static void Demo()
//     {
//         // Action delegates:
//         // TODO: call printWithPrefix and addFormat from above
//
//         // Closure trap:
//         var broken = new List<Action>();
//         for (int i = 0; i < 3; i++)
//         {
//             // TODO: broken.Add(() => Console.Write(i + " "));
//         }
//         // TODO: foreach (var a in broken) a();
//         // Expected output: 3 3 3  (NOT 0 1 2)
//         // WHY: lambda captures the VARIABLE 'i' by reference, not its value.
//         //      When all lambdas execute, the loop has finished and i == 3.
//
//         Console.WriteLine();
//
//         // FIX: capture a local copy per iteration
//         var fixed_ = new List<Action>();
//         for (int i = 0; i < 3; i++)
//         {
//             // TODO: int copy = i;
//             // TODO: fixed_.Add(() => Console.Write(copy + " "));
//         }
//         // TODO: foreach (var a in fixed_) a();
//         // Expected: 0 1 2
//
//         // TASK 7-E: Build a fluent string pipeline using Func<string,string>
//         //   Steps: Trim → ToUpper → prepend ">>> " → Replace spaces with "_"
//         //   Input: "  hello world  " → Output: >>>_HELLO_WORLD
//         // TODO: implement StringPipeline class or inline Aggregate
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 8 — ASYNC / AWAIT / TASK
// // ─────────────────────────────────────────────────────────────
// static class Part08_AsyncAwait
// {
//     // TASK 8-A: Write an async method that simulates fetching data with Task.Delay
//     //   Return a string result after 1 second.
//     // TODO:
//     // public static async Task<string> FetchDataAsync(string url)
//     // {
//     //     await Task.Delay(1000); // simulate I/O — does NOT block a thread
//     //     return $"Data from {url}";
//     // }
//
//     // TASK 8-B: Run two async tasks in PARALLEL using Task.WhenAll
//     // TODO:
//     // public static async Task RunParallel()
//     // {
//     //     var t1 = FetchDataAsync("http://api1.com");
//     //     var t2 = FetchDataAsync("http://api2.com");
//     //     var results = await Task.WhenAll(t1, t2);
//     //     foreach (var r in results) Console.WriteLine(r);
//     // }
//
//     public static async Task Demo()
//     {
//         // TODO: call FetchDataAsync and print result
//         // var data = await FetchDataAsync("http://example.com");
//         // Console.WriteLine(data);
//
//         // TODO: call RunParallel()
//         // await RunParallel();
//
//         // EXAM NOTE: explain why this causes a deadlock in ASP.NET Core:
//         // var bad = FetchDataAsync("url").Result; // DEADLOCK — blocks thread
//         // The correct way:
//         // var good = await FetchDataAsync("url");
//
//         // EXAM: async void is DANGEROUS — why?
//         // Answer: exceptions from async void are unobservable (cannot be caught
//         //         by the caller). Only acceptable use: event handlers.
//
//         await Task.CompletedTask; // placeholder — remove when you fill in the tasks
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 9 — SWITCH EXPRESSIONS & PATTERN MATCHING
// // ─────────────────────────────────────────────────────────────
// static class Part09_SwitchPatterns
// {
//     public static void Demo()
//     {
//         // TASK 9-A: Classic switch expression — map day number (1–7) to name
//         int day = 3;
//         // TODO:
//         // string dayName = day switch { 1 => "Monday", 2 => "Tuesday", ... _ => "Unknown" };
//         // Console.WriteLine(dayName);
//
//         // TASK 9-B: Pattern matching on types
//         object obj = 3.14;
//         // TODO:
//         // string describe = obj switch
//         // {
//         //     int i    => $"Integer: {i}",
//         //     double d => $"Double: {d}",
//         //     string s => $"String of length {s.Length}",
//         //     null     => "null",
//         //     _        => "Unknown"
//         // };
//         // Console.WriteLine(describe);
//
//         // TASK 9-C: Guards with 'when' — grade classifier
//         int score = 82;
//         // TODO:
//         // string grade = score switch
//         // {
//         //     >= 90 => "A",
//         //     >= 75 => "B",
//         //     >= 60 => "C",
//         //     _     => "F"
//         // };
//         // Console.WriteLine(grade);
//
//         // EXAM: C# PROHIBITS fall-through between non-empty case blocks.
//         //   Java ALLOWS fall-through (you need 'break' to prevent it).
//         //   In C#, the compiler ENFORCES break/return/throw — no implicit fall-through.
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 10 — COLLECTIONS
// // ─────────────────────────────────────────────────────────────
// static class Part10_Collections
// {
//     public static void Demo()
//     {
//         // TASK 10-A: List<T> — add, remove, index access, Contains (O(n))
//         // TODO:
//         // var list = new List<string> { "alpha", "beta", "gamma" };
//         // list.Add("delta");
//         // list.Remove("beta");
//         // Console.WriteLine(list[0]); // alpha
//
//         // TASK 10-B: Dictionary<K,V> — add, lookup, ContainsKey (O(1))
//         // TODO:
//         // var dict = new Dictionary<string, int>();
//         // dict["alpha"] = 1; dict["beta"] = 2;
//         // Console.WriteLine(dict.ContainsKey("alpha")); // True
//         // Console.WriteLine(dict["alpha"]);              // 1
//
//         // TASK 10-C: HashSet<T> — unique items, Contains is O(1)
//         // TODO:
//         // var set = new HashSet<int> { 1, 2, 3, 2, 1 };
//         // Console.WriteLine(set.Count); // 3 (duplicates removed)
//         // Console.WriteLine(set.Contains(2)); // True
//
//         // TASK 10-D: Queue<T> — FIFO (Enqueue/Dequeue/Peek)
//         // TODO:
//         // var queue = new Queue<string>();
//         // queue.Enqueue("first"); queue.Enqueue("second");
//         // Console.WriteLine(queue.Dequeue()); // first
//
//         // TASK 10-E: Stack<T> — LIFO (Push/Pop/Peek)
//         // TODO:
//         // var stack = new Stack<string>();
//         // stack.Push("first"); stack.Push("second");
//         // Console.WriteLine(stack.Pop()); // second
//
//         // EXAM: Dictionary (ContainsKey) and HashSet both offer O(1) Contains.
//         //       List.Contains is O(n) — scans every element.
//         //       SortedDictionary offers O(log n) access — uses a red-black tree.
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 11 — ABSTRACT CLASSES, virtual/override vs new (HIDING)
// // ─────────────────────────────────────────────────────────────
// static class Part11_AbstractInheritance
// {
//     // TASK 11-A: Create an abstract class Animal with:
//     //   - Property Name (string)
//     //   - Abstract method Speak() → string
//     //   - Virtual method Describe() that prints "Name: Speak()"
//     //   - Override ToString()
//     //
//     // TODO: define abstract class Animal
//
//     // TASK 11-B: Create Dog, Cat, Fish subclasses.
//     //   Fish should OVERRIDE Describe() and also call base.Describe().
//     //
//     // TODO: define Dog, Cat, Fish
//
//     // TASK 11-C: Create SpecialDog that HIDES Eat() using 'new' (NOT override).
//     //   Show the difference:
//     //     Animal a = new SpecialDog(); a.Eat() → calls Animal's version (no polymorphism!)
//     //     SpecialDog sd = new SpecialDog(); sd.Eat() → calls SpecialDog's version
//     //
//     // TODO: define base Animal.Eat() as virtual, SpecialDog hides it with 'new'
//
//     public static void Demo()
//     {
//         // TODO: create Animal[] with Dog, Cat, Fish and loop calling Describe()
//         // TODO: demonstrate type-safe pattern matching: if (a is Dog d) ...
//         // TODO: demonstrate 'new' hiding — Animal pointer calls Animal's method
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 12 — INTERFACES
// // ─────────────────────────────────────────────────────────────
// static class Part12_Interfaces
// {
//     // TASK 12-A: Define interfaces ISaveable and IPrintable.
//     //   Then create a class Document that implements BOTH.
//     //
//     // TODO:
//     // interface IRepository { void Save(string data); }
//     // interface IPrintable  { void Print(); }
//     //
//     // class Document : IRepository, IPrintable
//     // {
//     //     private string _content = "";
//     //     public void Save(string data) => _content = data;
//     //     public void Print() => Console.WriteLine(_content);
//     // }
//
//     // TASK 12-B: Demonstrate a default interface method (C# 8+)
//     //   Add a default Log() method directly on the interface.
//     //
//     // TODO:
//     // interface ILoggable
//     // {
//     //     void Log() => Console.WriteLine("[LOG] " + GetType().Name);
//     // }
//
//     public static void Demo()
//     {
//         // TODO: prove a class can be referenced by multiple interface types
//         // var doc = new Document();
//         // IRepository r = doc;
//         // IPrintable  p = doc;
//         // r.Save("Hello World");
//         // p.Print();
//
//         // EXAM: Interfaces CANNOT have backing fields.
//         //       Interfaces CANNOT have constructors.
//         //       A class CAN implement multiple interfaces.
//         //       Default implementations allowed since C# 8.
//         //       Convention: interface names start with 'I'.
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 13 — CUSTOM EXCEPTIONS & RETHROW
// // ─────────────────────────────────────────────────────────────
// static class Part13_Exceptions
// {
//     // TASK 13-A: Create two custom exceptions:
//     //   InsufficientFundsException
//     //   AccountFrozenException
//     //   Both inherit from Exception and accept a message.
//     //
//     // TODO:
//     // public class InsufficientFundsException : Exception
//     // {
//     //     public InsufficientFundsException(string message) : base(message) { }
//     // }
//
//     // TASK 13-B: Write a BankService.Withdraw() that throws these exceptions.
//     //   Catch them separately and rethrow with 'throw;' (not 'throw ex;').
//     //   Explain WHY 'throw;' preserves the stack trace but 'throw ex;' resets it.
//     //
//     public static void Demo()
//     {
//         // TODO:
//         // try
//         // {
//         //     BankService.Withdraw(account, 999_999m);
//         // }
//         // catch (AccountFrozenException ex)
//         // {
//         //     Console.WriteLine("Account frozen: " + ex.Message);
//         //     throw; // preserves original stack trace — ALWAYS use this!
//         //            // NEVER: throw ex; -- that resets the stack trace!
//         // }
//         // catch (InsufficientFundsException ex)
//         // {
//         //     Console.WriteLine("Not enough money: " + ex.Message);
//         // }
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 14 — SOLID: SRP (Single Responsibility Principle)
// // ─────────────────────────────────────────────────────────────
// static class Part14_SRP
// {
//     // EXAM DEFINITION: A class should have ONE reason to change
//     //   = it serves a single actor (stakeholder).
//     //   NOT "one method" — it's about WHO would request a change.
//
//     // TASK 14-A: The class below violates SRP. Identify the 4 actors
//     //   (business analyst, product owner, DBA, UX team) and split it.
//     //
//     // class OrderService_BAD
//     // {
//     //     public bool ValidateOrder(Order o)   { /* actor: ??? */ return true; }
//     //     public decimal CalcDiscount(Order o) { /* actor: ??? */ return 0; }
//     //     public void SaveOrder(Order o)       { /* actor: ??? */ }
//     //     public void SendEmail(Order o)       { /* actor: ??? */ }
//     // }
//
//     // TODO: Create separate interfaces and implementations:
//     //   IOrderValidator    → OrderValidator
//     //   IDiscountCalc      → DiscountCalculator
//     //   IOrderRepository   → SqlOrderRepository
//     //   INotificationSvc   → EmailNotificationService
//     //
//     // Then create an OrderOrchestrator that takes all 4 via constructor injection.
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 15 — SOLID: OCP + STRATEGY PATTERN
// // ─────────────────────────────────────────────────────────────
// static class Part15_OCP_Strategy
// {
//     // EXAM: OCP violation = growing if/else or switch on TYPE
//     //        that must be MODIFIED whenever a new variant is added.
//     //       FIX = Strategy pattern: each variant is a NEW CLASS.
//
//     // TASK 15-A: The method below violates OCP. Refactor it.
//     //
//     // decimal CalcDiscount_BAD(string type, decimal amount)
//     // {
//     //     if (type == "Regular") return amount * 0.05m;
//     //     if (type == "Premium") return amount * 0.10m;
//     //     if (type == "VIP")     return amount * 0.20m;
//     //     // Adding "Corporate" requires OPENING this method — OCP violated!
//     //     return 0m;
//     // }
//
//     // TODO: Create IDiscountRule interface with Calculate(decimal) → decimal
//     // TODO: Implement RegularDiscountRule, PremiumDiscountRule, VipDiscountRule
//     // TODO: Create DiscountEngine that takes IEnumerable<IDiscountRule>
//     //       — adding "CorporateDiscountRule" requires ZERO changes to DiscountEngine
//
//     // TASK 15-B: Apply the same pattern to report formats.
//     //   IReportFormatter with Format(IEnumerable<string>) → string
//     //   ConsoleFormatter, CsvFormatter, HtmlFormatter, JsonFormatter
//     //   ReportGenerator that takes IReportFormatter — never changes.
//
//     public static void Demo()
//     {
//         // TODO: demonstrate adding a new formatter without touching ReportGenerator
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 16 — SOLID: LSP (Liskov Substitution Principle)
// // ─────────────────────────────────────────────────────────────
// static class Part16_LSP
// {
//     // EXAM: Subtype must honour the CONTRACT of its base type.
//     //   3 rules:
//     //   1. Cannot STRENGTHEN preconditions (require MORE from callers)
//     //   2. Cannot WEAKEN postconditions (promise LESS to callers)
//     //   3. Must PRESERVE invariants
//
//     // TASK 16-A: Classic violation — Square inherits Rectangle.
//     //   Show how setting Width=5, Height=10 gives Area=100 (not 50).
//     //
//     // TODO:
//     // class Rectangle { public virtual double Width { get; set; } public virtual double Height { get; set; } }
//     // class Square : Rectangle
//     // {
//     //     // TODO: override Width and Height so both dimensions stay equal
//     //     // This BREAKS the caller's expectation that Area = Width * Height
//     // }
//
//     // TASK 16-B: Fix LSP — use a shared interface instead of inheritance.
//     //
//     // TODO:
//     // interface IShape { double GetArea(); }
//     // class Rectangle : IShape { ... }
//     // class Square    : IShape { ... }
//
//     public static void Demo()
//     {
//         // TODO: demonstrate the LSP violation, then the correct version
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 17 — SOLID: ISP (Interface Segregation Principle)
// // ─────────────────────────────────────────────────────────────
// static class Part17_ISP
// {
//     // EXAM: ISP violation = class must throw NotImplementedException
//     //   because the fat interface has methods it doesn't need.
//
//     // TASK 17-A: Identify the ISP violation, then fix it.
//     //
//     // interface IStudentRepository_BAD
//     // {
//     //     Task<Student?> GetByIdAsync(int id);
//     //     Task AddAsync(Student s);
//     //     Task DeleteAsync(int id);
//     //     Task<byte[]> ExportToPdfAsync(); // read-only repos can't implement this!
//     // }
//
//     // TODO: split into role-based interfaces:
//     //   IStudentReader   — GetByIdAsync, GetAllAsync
//     //   IStudentWriter   — AddAsync, DeleteAsync
//     //   IStudentExporter — ExportToCsvAsync
//
//     // TODO: implement ReadOnlyStudentRepository : IStudentReader
//     //   — no NotImplementedException needed because it only implements what it needs
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 18 — SOLID: DIP + IoC / DI Lifetimes
// // ─────────────────────────────────────────────────────────────
// static class Part18_DIP
// {
//     // EXAM: DIP = High-level modules depend on ABSTRACTIONS (interfaces),
//     //   not on concrete low-level classes.
//     //   The 'inversion': the interface is OWNED by the high-level module.
//
//     // TASK 18-A: Build a 3-layer system with manual wiring.
//     //   Layers: Controller → Service → Repository
//     //   Each depends ONLY on an interface, never on a concrete class.
//     //
//     // TODO:
//     // record Product(int Id, string Name, decimal Price);
//     //
//     // interface IProductRepository { IEnumerable<Product> GetAll(); }
//     // interface IProductService    { IEnumerable<string> GetFormatted(); }
//     //
//     // class InMemoryProductRepository : IProductRepository { ... }
//     // class ProductService : IProductService { /* inject IProductRepository */ }
//     // class ProductController { /* inject IProductService */ public void ShowAll() {...} }
//
//     public static void Demo()
//     {
//         // TODO: composition root — manual wiring (no DI container needed here)
//         // var controller = new ProductController(
//         //     new ProductService(
//         //         new InMemoryProductRepository()));
//         // controller.ShowAll();
//     }
//
//     // TASK 18-B: In comments, explain the 3 DI lifetimes:
//     //   AddTransient  = new instance every ________
//     //   AddScoped     = one instance per ________
//     //   AddSingleton  = one instance for _______  (must be ______-safe)
//     //
//     // EXAM: NEVER inject a Scoped service into a Singleton. Why?
//     // TODO: write your explanation here
//
//     // TASK 18-C: Why is Service Locator an ANTI-PATTERN?
//     //   (Hint: think about hidden dependencies and testability)
//     // TODO: write explanation here
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 19 — COHESION & COUPLING (theory — comment tasks)
// // ─────────────────────────────────────────────────────────────
// static class Part19_CohesionCoupling
// {
//     // TASK 19-A: Rank cohesion types from BEST to WORST (7 types).
//     //   Hint: Functional > Sequential > Communicational > Procedural >
//     //         Temporal > Logical > Coincidental
//     // TODO: write your list here
//
//     // TASK 19-B: What are 3 warning signs of LOW cohesion in a class?
//     // TODO:
//     // 1. Class name is 'Helper', 'Utils', or 'Manager' without specific context
//     // 2. ...
//     // 3. ...
//
//     // TASK 19-C: Rank coupling types from WORST to BEST (5 types).
//     //   Hint: Content (worst) > Common > Control > Stamp > Data (best)
//     // TODO: write your list here
//
//     // TASK 19-D: What is the difference between 'code duplication' and
//     //   'knowledge duplication' in the context of DRY?
//     //   Which is worse? Why?
//     // TODO: write explanation here
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 20 — CLEAN CODE / DRY
// // ─────────────────────────────────────────────────────────────
// static class Part20_CleanCode
// {
//     // TASK 20-A: Rename these bad variable/method names to good ones.
//     //   Bad: d, tmp, processStuff(), getData()
//     //   Good: ???
//     // TODO: explain your renaming rationale in comments
//
//     // TASK 20-B: The method below is too long and violates "do ONE thing".
//     //   Extract at least 3 helper methods from it.
//     //
//     // void ProcessOrder_BAD(Order o)
//     // {
//     //     // Validate
//     //     if (o == null) throw new ArgumentNullException(nameof(o));
//     //     if (o.Items.Count == 0) throw new InvalidOperationException("No items");
//     //
//     //     // Calculate total
//     //     decimal total = 0;
//     //     foreach (var item in o.Items) total += item.Price * item.Quantity;
//     //
//     //     // Apply discount
//     //     if (o.CustomerType == "VIP") total *= 0.80m;
//     //     else if (o.CustomerType == "Premium") total *= 0.90m;
//     //
//     //     // Save to DB
//     //     Database.Save(o);
//     //
//     //     // Send email
//     //     EmailService.Send(o.CustomerEmail, $"Order total: {total:C}");
//     // }
//     //
//     // TODO: refactor into Validate(), CalculateTotal(), ApplyDiscount(), etc.
//
//     // TASK 20-C: Apply Command-Query Separation.
//     //   This method violates CQS:
//     //   User GetAndLogUser(int id) { var u = _db.GetUser(id); _logger.Log(...); return u; }
//     //   Fix: split into GetUser(id) and LogUserAccess(id) — separate concerns.
//     // TODO: write clean version
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 21 — LINQ: DEFERRED EXECUTION TRAP
// // ─────────────────────────────────────────────────────────────
// static class Part21_DeferredExecution
// {
//     public static void Demo()
//     {
//         // TASK 21-A: Classic deferred execution trap — exam favourite!
//         var nums = new List<int> { 1, 2, 3 };
//
//         // TODO: build a query but do NOT materialise it yet
//         // var query = nums.Where(n => n > 1);
//
//         // TODO: add 4 to the source AFTER defining the query
//         // nums.Add(4);
//
//         // TODO: iterate — what will print? (Answer: 2 3 4 — includes 4!)
//         // foreach (var n in query) Console.Write(n + " ");
//
//         // TASK 21-B: Fix with ToList() to get a snapshot
//         // var snapshot = nums.Where(n => n > 1).ToList();
//         // nums.Add(99);
//         // Console.WriteLine(string.Join(" ", snapshot)); // 2 3 4 (99 excluded)
//
//         // EXAM: Deferred operators: Where, Select, OrderBy, ThenBy, GroupBy
//         //       Terminal operators (trigger execution): ToList, Count, First, Any, All, Average
//         //       "Multiple enumeration" = re-executing the query N times → in EF Core that's N SQL queries!
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 22 — LINQ BASICS: Where, Select, OrderBy, ThenBy
// // ─────────────────────────────────────────────────────────────
// static class Part22_LinqBasics
// {
//     // Shared sample data
//     record Student(int Id, string FirstName, string LastName, string City, string Email);
//
//     static List<Student> Students() => new()
//     {
//         new(1, "Anna",  "Kowalska", "Warsaw", "anna@uni.pl"),
//         new(2, "Marek", "Nowak",    "Krakow", "marek@uni.pl"),
//         new(3, "Julia", "Wiśniewska","Warsaw","julia@uni.pl"),
//         new(4, "Tomasz","Lis",      "Warsaw", "tomasz@uni.pl"),
//         new(5, "Lena",  "Adamska",  "Gdansk", "lena@uni.pl"),
//     };
//
//     public static void Demo()
//     {
//         var students = Students();
//
//         // TASK 22-A: Filter students from Warsaw, sort by LastName then FirstName,
//         //   project to "LastName, FirstName" string
//         // TODO:
//         // var warsawStudents = students
//         //     .Where(...)
//         //     .OrderBy(...)
//         //     .ThenBy(...)     // ← never use a second OrderBy()!
//         //     .Select(...);
//         // warsawStudents.ToList().ForEach(Console.WriteLine);
//
//         // TASK 22-B: Project all students to just their Email addresses
//         // TODO:
//         // var emails = students.Select(s => s.Email);
//
//         // EXAM PITFALL: a second .OrderBy() REPLACES the first!
//         //   Wrong: students.OrderBy(s => s.City).OrderBy(s => s.LastName)
//         //          → sorts by LastName ONLY (City sort is thrown away!)
//         //   Correct: .OrderBy(s => s.City).ThenBy(s => s.LastName)
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 23 — LINQ QUANTIFIERS: Any, All, Count, First, Single
// // ─────────────────────────────────────────────────────────────
// static class Part23_LinqQuantifiers
// {
//     public static void Demo()
//     {
//         var numbers = new List<int> { 3, 7, 2, 9, 1 };
//
//         // TASK 23-A: Check if ANY number is greater than 8
//         // TODO:
//         // bool anyBig = numbers.Any(n => n > 8);
//         // Console.WriteLine(anyBig); // True
//
//         // TASK 23-B: Check if ALL numbers are positive
//         // TODO:
//         // bool allPositive = numbers.All(n => n > 0);
//         // Console.WriteLine(allPositive); // True
//
//         // EXAM TRAP: All() on EMPTY collection returns TRUE (vacuously true)!
//         // Console.WriteLine(new List<int>().All(n => n > 0)); // True — exam classic!
//
//         // TASK 23-C: Count numbers greater than 5
//         // TODO:
//         // int count = numbers.Count(n => n > 5);
//
//         // TASK 23-D: Get first number > 5 safely (use FirstOrDefault)
//         // TODO:
//         // int first = numbers.FirstOrDefault(n => n > 5);
//         // — if nothing matches, FirstOrDefault returns 0 (default for int), NOT an exception
//         // — First() would throw InvalidOperationException if nothing matches
//
//         // TASK 23-E: Use Single() — throws if 0 or 2+ matches
//         // TODO:
//         // int exactlyOne = numbers.Single(n => n == 9); // ok — exactly one 9
//         // int boom = numbers.Single(n => n > 5); // throws — there are multiple
//
//         // EXAM: Any() stops at the first match → prefer over Count() > 0 for existence checks
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 24 — LINQ JOINS, GroupBy, GroupJoin, SelectMany
// // ─────────────────────────────────────────────────────────────
// static class Part24_LinqJoins
// {
//     record Course(int Id, string Title, string Category, int LecturerId);
//     record Enrollment(int StudentId, int CourseId, double? FinalGrade, bool IsActive);
//     record Lecturer(int Id, string FirstName, string LastName);
//
//     static List<Course>     Courses()     => new() {
//         new(1, "C# Basics",    ".NET",  1),
//         new(2, "LINQ Lab",     ".NET",  1),
//         new(3, "Azure Cloud",  "Cloud", 2),
//     };
//     static List<Enrollment> Enrollments() => new() {
//         new(1, 1, 4.5,  true),
//         new(1, 2, null, true),
//         new(2, 3, 5.0,  true),
//         new(3, 1, 3.5,  false),
//     };
//     static List<Lecturer>   Lecturers()   => new() {
//         new(1, "Jan",   "Kowalski"),
//         new(2, "Maria", "Nowak"),
//         new(3, "Adam",  "Wolski"), // no courses assigned
//     };
//
//     public static void Demo()
//     {
//         // TASK 24-A: Two-way Join — enrollments + courses → "StudentId | CourseTitle"
//         // TODO:
//         // var result = Enrollments()
//         //     .Join(Courses(), e => e.CourseId, c => c.Id,
//         //           (e, c) => $"Student {e.StudentId} | {c.Title}")
//         //     .ToList().ForEach(Console.WriteLine);
//
//         // TASK 24-B: GroupBy — count enrollments per course
//         // TODO:
//         // var grouped = Enrollments()
//         //     .GroupBy(e => e.CourseId)
//         //     .Select(g => new { CourseId = g.Key, Count = g.Count() });
//
//         // TASK 24-C: GroupJoin (LEFT OUTER JOIN) — all lecturers + course count (incl. 0)
//         // TODO:
//         // var lecturerCourses = Lecturers()
//         //     .GroupJoin(Courses(), l => l.Id, c => c.LecturerId,
//         //                (l, courses) => $"{l.FirstName}: {courses.Count()} courses");
//
//         // TASK 24-D: Average grade per course — ONLY graded enrollments
//         //   EXAM: always filter .Where(e => e.FinalGrade.HasValue) BEFORE Average()!
//         // TODO:
//         // var avgGrades = Enrollments()
//         //     .Where(e => e.FinalGrade.HasValue)
//         //     .GroupBy(e => e.CourseId)
//         //     .Select(g => new { CourseId = g.Key, Avg = g.Average(e => e.FinalGrade!.Value) });
//
//         // TASK 24-E: SelectMany — flatten all course titles from all lecturers
//         //   (= Java's flatMap)
//         // TODO:
//         // var allTitles = Lecturers()
//         //     .SelectMany(l => Courses().Where(c => c.LecturerId == l.Id).Select(c => c.Title));
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 25 — LINQ PAGING, DISTINCT, AGGREGATE
// // ─────────────────────────────────────────────────────────────
// static class Part25_LinqPaging
// {
//     public static void Demo()
//     {
//         var numbers = Enumerable.Range(1, 20).ToList();
//
//         // TASK 25-A: Implement paging — page 3, page size 4
//         //   Formula: Skip((page-1) * pageSize).Take(pageSize)
//         //   ALWAYS sort before paging!
//         int page = 3, pageSize = 4;
//         // TODO:
//         // var paged = numbers
//         //     .OrderBy(n => n)
//         //     .Skip((page - 1) * pageSize)
//         //     .Take(pageSize);
//         // Console.WriteLine(string.Join(", ", paged)); // 9, 10, 11, 12
//
//         // TASK 25-B: Distinct cities from mixed list
//         var cities = new[] { "Warsaw", "Krakow", "Warsaw", "Gdansk", "Krakow" };
//         // TODO:
//         // var uniqueCities = cities.Distinct().OrderBy(c => c);
//         // Console.WriteLine(string.Join(", ", uniqueCities));
//
//         // TASK 25-C: Aggregate — build a running sum (like a manual Sum)
//         // TODO:
//         // int total = numbers.Aggregate(0, (acc, n) => acc + n);
//         // Console.WriteLine(total); // 210
//
//         // TASK 25-D: ToHashSet() for O(1) lookup — pattern from Challenge 03
//         var dotnetCourseIds = new[] { 1, 2 };
//         // TODO:
//         // var idSet = dotnetCourseIds.ToHashSet(); // O(1) Contains vs O(n) for list!
//         // Console.WriteLine(idSet.Contains(2)); // True
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 26 — ref / out / named / optional PARAMETERS
// // ─────────────────────────────────────────────────────────────
// static class Part26_RefOut
// {
//     // TASK 26-A: ref — caller MUST initialise. Method modifies in place.
//     static void AddTen(ref int x) => x += 10;
//
//     // TASK 26-B: out — caller does NOT need to initialise. Method MUST assign.
//     static bool TryParseAge(string input, out int age) =>
//         int.TryParse(input, out age);
//
//     // TASK 26-C: Named parameters — call in any order
//     static void CreateOrder(string customer, int quantity, decimal price) =>
//         Console.WriteLine($"{customer}: {quantity} x {price:C}");
//
//     // TASK 26-D: Optional (default) parameters — must come AFTER required ones
//     static void Greet(string name, string greeting = "Hello", bool formal = false) =>
//         Console.WriteLine($"{(formal ? greeting + "," : greeting)} {name}!");
//
//     public static void Demo()
//     {
//         // TODO: call AddTen with ref
//         int n = 5;
//         // AddTen(ref n);
//         // Console.WriteLine(n); // 15
//
//         // TODO: call TryParseAge with out
//         // if (TryParseAge("25", out int age)) Console.WriteLine($"Age: {age}");
//         // if (TryParseAge("abc", out int bad)) Console.WriteLine(bad);
//         // else Console.WriteLine("Parse failed");
//
//         // TODO: call CreateOrder using named parameters (out of order)
//         // CreateOrder(price: 9.99m, customer: "Anna", quantity: 3);
//
//         // TODO: call Greet with defaults and with all arguments
//         // Greet("Anna");
//         // Greet("Jan", "Good morning", true);
//
//         // EXAM: ref, out, default params, named params are UNIQUE TO C#.
//         //       Java does NOT have any of these. Java always passes by value.
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 27 — GENERICS (reification vs type erasure)
// // ─────────────────────────────────────────────────────────────
// static class Part27_Generics
// {
//     // TASK 27-A: Write a generic Repository<T> class with constraint: T must be a class
//     //   Methods: Add(T item), FindFirst(Func<T,bool>), GetAll()
//     //
//     // TODO:
//     // public class Repository<T> where T : class
//     // {
//     //     private readonly List<T> _items = new();
//     //     public void Add(T item) => _items.Add(item);
//     //     public T? FindFirst(Func<T, bool> pred) => _items.FirstOrDefault(pred);
//     //     public IEnumerable<T> GetAll() => _items.AsReadOnly();
//     // }
//
//     // TASK 27-B: Write a generic static method FindFirst<T> that takes a source and predicate
//     // TODO:
//     // public static T? FindFirst<T>(IEnumerable<T> source, Func<T, bool> pred)
//     //     => source.FirstOrDefault(pred);
//
//     public static void Demo()
//     {
//         // TODO: use Repository<string> and Repository<int>
//         // EXAM: C# generics use REIFICATION — type info is PRESERVED at runtime.
//         //       typeof(T) works. List<int> stores ints efficiently (no boxing).
//         //       Java uses TYPE ERASURE — generic type info is LOST after compilation.
//         //       Java List<Integer> boxes every int to Object (inefficient).
//
//         // TODO: print typeof(List<string>).Name to see reification at work
//         // Console.WriteLine(new List<string>().GetType().Name); // List`1[String]
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 28 — yield return (LAZY SEQUENCES)
// // ─────────────────────────────────────────────────────────────
// static class Part28_YieldReturn
// {
//     // TASK 28-A: Write a method returning IEnumerable<int> using yield return.
//     //   Generate even numbers from 0 to max lazily.
//     //
//     // TODO:
//     // public static IEnumerable<int> EvenNumbers(int max)
//     // {
//     //     for (int i = 0; i <= max; i += 2)
//     //         yield return i; // pauses here, resumes when caller asks for next
//     // }
//
//     // TASK 28-B: Write a method that uses yield break to stop early.
//     //   Return active students, but stop completely if you find an expelled one.
//     //
//     // TODO:
//     // public static IEnumerable<string> GetActiveStudentNames(IEnumerable<(string Name, bool IsActive, bool IsExpelled)> students)
//     // {
//     //     foreach (var s in students)
//     //     {
//     //         if (s.IsExpelled) yield break;  // stop iteration entirely
//     //         if (s.IsActive)   yield return s.Name;
//     //     }
//     // }
//
//     // TASK 28-C: Read file lines lazily (real-world use case)
//     //   Only ONE line is in memory at a time — no ReadAllLines() spike.
//     // TODO:
//     // public static IEnumerable<string> ReadLines(string path)
//     // {
//     //     using var reader = new System.IO.StreamReader(path);
//     //     string? line;
//     //     while ((line = reader.ReadLine()) != null)
//     //         yield return line;
//     // }
//
//     public static void Demo()
//     {
//         // TODO: iterate EvenNumbers — note that nothing executes until foreach
//         // foreach (var n in EvenNumbers(10)) Console.Write(n + " "); // 0 2 4 6 8 10
//
//         // EXAM: yield return turns a method into a state machine (compiler generates it).
//         //       Elements are produced ON DEMAND — this is the foundation of LINQ's deferred execution.
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 29 — STRINGS & StringBuilder
// // ─────────────────────────────────────────────────────────────
// static class Part29_Strings
// {
//     public static void Demo()
//     {
//         // TASK 29-A: Practice common string methods
//         string s = "  Hello, World!  ";
//         // TODO:
//         // Console.WriteLine(s.Trim());              // "Hello, World!"
//         // Console.WriteLine(s.ToUpper());
//         // Console.WriteLine(s.Replace("World", "C#"));
//         // Console.WriteLine(s.Split(',')[0].Trim()); // "Hello"
//         // Console.WriteLine(s.IndexOf("World"));
//         // Console.WriteLine(s.Substring(9, 5));     // "World"
//         // Console.WriteLine(string.IsNullOrWhiteSpace("  ")); // True
//         // Console.WriteLine(string.IsNullOrWhiteSpace(null));  // True
//
//         // TASK 29-B: StringBuilder for loop concatenation — O(n) not O(n²)
//         // TODO:
//         // var sb = new StringBuilder();
//         // for (int i = 1; i <= 1000; i++) sb.Append(i).Append(", ");
//         // string result = sb.ToString();
//         // Console.WriteLine(result.Length);
//
//         // TASK 29-C: String interpolation and verbatim strings
//         string name = "Anna";
//         int age = 22;
//         // TODO:
//         // string msg  = $"Hello {name}, you are {age} years old.";
//         // string path = @"C:\Users\Anna\Documents"; // verbatim — no escape needed
//
//         // TASK 29-D: == on strings compares BY VALUE in C# (unlike Java!)
//         string a = "hello";
//         string b = "hel" + "lo";
//         // TODO:
//         // Console.WriteLine(a == b); // True in C# — value comparison!
//         // In Java: a == b might be false (reference comparison); use .equals()
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 30 — TUPLES & DECONSTRUCTION
// // ─────────────────────────────────────────────────────────────
// static class Part30_Tuples
// {
//     // TASK 30-A: Method returning a named tuple (no class needed)
//     static (string Name, int Age) GetPersonInfo(int id) => ("Anna", 22);
//
//     // TASK 30-B: Return a named tuple from an interface method (pattern from codebase)
//     interface IDiscountRule
//     {
//         (decimal amount, string note) Calculate(decimal baseAmount);
//     }
//
//     class PercentageDiscount : IDiscountRule
//     {
//         private readonly decimal _rate;
//         private readonly string _label;
//         public PercentageDiscount(decimal rate, string label) { _rate = rate; _label = label; }
//
//         public (decimal amount, string note) Calculate(decimal baseAmount) =>
//             (baseAmount * _rate, _label);
//     }
//
//     public static void Demo()
//     {
//         // TASK 30-C: Use the named tuple return value
//         // TODO:
//         // var info = GetPersonInfo(1);
//         // Console.WriteLine(info.Name); // Anna
//         // Console.WriteLine(info.Age);  // 22
//
//         // TASK 30-D: Deconstruct into separate variables
//         // TODO:
//         // var (name, age) = GetPersonInfo(1);
//         // Console.WriteLine($"{name} is {age}");
//
//         // TASK 30-E: Use the discount rule tuple
//         IDiscountRule rule = new PercentageDiscount(0.10m, "10% member discount");
//         // TODO:
//         // var (amount, note) = rule.Calculate(1000m);
//         // Console.WriteLine($"Discount: {amount:C} ({note})"); // $100 (10% member discount)
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 31 — ADAPTER PATTERN
// // ─────────────────────────────────────────────────────────────
// static class Part31_Adapter
// {
//     // TASK 31-A: The legacy system uses static methods (cannot be injected).
//     //   Create an Adapter that wraps it behind injectable interfaces.
//     //
//     // This is exactly the pattern from the SubscriptionRenewal codebase.
//
//     // Legacy system — you cannot modify this
//     static class LegacyEmailGateway
//     {
//         public static void Send(string to, string subject, string body) =>
//             Console.WriteLine($"[LEGACY EMAIL] To: {to} | {subject}");
//     }
//
//     // TODO: Define IEmailService interface with SendEmail(string to, string subject, string body)
//
//     // TODO: Create LegacyEmailAdapter : IEmailService that wraps LegacyEmailGateway
//
//     // TODO: Create a NotificationService that accepts IEmailService via constructor injection
//     //   — it never knows whether it's talking to the legacy gateway or a modern one
//
//     public static void Demo()
//     {
//         // TODO: wire it up and send an email through the adapter
//         // IEmailService emailService = new LegacyEmailAdapter();
//         // var notifier = new NotificationService(emailService);
//         // notifier.Notify("anna@uni.pl", "Welcome", "Hello!");
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 32 — FACTORY METHOD & ABSTRACT FACTORY
// // ─────────────────────────────────────────────────────────────
// static class Part32_Factory
// {
//     // TASK 32-A: Factory Method
//     //   Create abstract class NotificationFactory with abstract CreateSender() → ISender
//     //   Create EmailSenderFactory and SmsSenderFactory as concrete subclasses.
//     //   The client code uses NotificationFactory without knowing the concrete type.
//     //
//     // TODO:
//     // interface ISender { void Send(string message); }
//     // class EmailSender : ISender { public void Send(string m) => Console.WriteLine("Email: " + m); }
//     // class SmsSender   : ISender { public void Send(string m) => Console.WriteLine("SMS: "   + m); }
//     //
//     // abstract class NotificationFactory
//     // {
//     //     public abstract ISender CreateSender(); // Factory Method
//     //     public void Notify(string msg) => CreateSender().Send(msg); // uses factory
//     // }
//     // class EmailSenderFactory : NotificationFactory { public override ISender CreateSender() => new EmailSender(); }
//     // class SmsSenderFactory   : NotificationFactory { public override ISender CreateSender() => new SmsSender(); }
//
//     // TASK 32-B: Abstract Factory
//     //   Creates a FAMILY of related objects.
//     //   IUIFactory with CreateButton() and CreateCheckbox().
//     //   WindowsUIFactory → WindowsButton + WindowsCheckbox
//     //   MacUIFactory     → MacButton + MacCheckbox
//     //
//     // TODO: implement the above
//
//     public static void Demo()
//     {
//         // EXAM: Factory Method = one abstract creation method; subclasses decide the type.
//         //       Abstract Factory = interface with MULTIPLE factory methods; creates a family.
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 33 — SINGLETON PATTERN
// // ─────────────────────────────────────────────────────────────
// static class Part33_Singleton
// {
//     // TASK 33-A: Classic Singleton with Lazy<T> for thread safety
//     //
//     // TODO:
//     // public class AppConfig
//     // {
//     //     private static readonly Lazy<AppConfig> _instance =
//     //         new(() => new AppConfig()); // Lazy = thread-safe lazy init
//     //
//     //     private AppConfig() { /* load config */ }
//     //
//     //     public static AppConfig Instance => _instance.Value;
//     //     public string Environment { get; } = "Production";
//     // }
//
//     public static void Demo()
//     {
//         // TODO: prove that both references point to the same instance
//         // var a = AppConfig.Instance;
//         // var b = AppConfig.Instance;
//         // Console.WriteLine(ReferenceEquals(a, b)); // True
//
//         // EXAM: Classic Singleton problems:
//         //   1. Hinders unit testing (hidden global state, hard to mock)
//         //   2. Violates DIP — classes depend on the concrete static instance
//         //   3. Thread safety must be implemented manually
//         //
//         // MODERN C# PREFERRED APPROACH:
//         //   builder.Services.AddSingleton<IAppConfig, AppConfig>();
//         //   — handled by DI container, testable, DIP-compliant
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 34 — EXAM SIMULATION: GIT SCENARIO (comment-only)
// // ─────────────────────────────────────────────────────────────
// static class Part34_ExamSimGit
// {
//     // SCENARIO: You are on feature/reports.
//     //   Main received commit abc123 while you worked.
//     //   You have one conflict in StudentReport.cs.
//     //   The team lead wants LINEAR history.
//     //   Write every git command from "catch up with main"
//     //   to "feature/reports deleted and pushed".
//     //
//     // TODO: write every git command in order below:
//     //
//     // Step 1: ...
//     // Step 2: ...
//     // ...
//     //
//     // BONUS: What does --ff-only guarantee? Why is it safer than plain git merge?
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 35 — EXAM SIMULATION: OOP + DIP
// // ─────────────────────────────────────────────────────────────
// static class Part35_ExamSimOOP
// {
//     // TASK: Build a complete NotificationSystem from scratch.
//     //
//     // Requirements:
//     //   1. abstract record Notification(string Subject, string Body) with auto-property CreatedAt
//     //   2. Concrete records: EmailNotification(To, Cc?, Subject, Body)
//     //                        SmsNotification(PhoneNumber, Subject, Body)
//     //                        PushNotification(DeviceToken, AppName, Subject, Body)
//     //   3. interface ISender with Task SendAsync(Notification n)
//     //   4. Concrete senders: EmailSender, SmsSender, PushSender
//     //      Each checks the type of notification and prints a descriptive message.
//     //   5. class NotificationDispatcher:
//     //      - takes List<ISender> via constructor (DIP!)
//     //      - DispatchAsync(Notification n) calls ALL senders in parallel via Task.WhenAll
//     //   6. OCP: adding PushSender changes NOTHING in NotificationDispatcher.
//
//     // TODO: implement everything above
//
//     public static async Task Demo()
//     {
//         // TODO: wire up and dispatch different notification types
//         // var dispatcher = new NotificationDispatcher(new List<ISender>
//         // {
//         //     new EmailSender(),
//         //     new SmsSender(),
//         //     new PushSender()
//         // });
//         // await dispatcher.DispatchAsync(new EmailNotification("anna@uni.pl", null, "Welcome", "Hello!"));
//         // await dispatcher.DispatchAsync(new SmsNotification("+48123456789", "Reminder", "Class at 10am"));
//         // await dispatcher.DispatchAsync(new PushNotification("device-abc", "UniversityApp", "New grade", "You got 5.0!"));
//
//         await Task.CompletedTask; // remove when implemented
//     }
// }
//
//
// // ─────────────────────────────────────────────────────────────
// // PART 36 — EXAM SIMULATION: LINQ ANALYTICS
// // ─────────────────────────────────────────────────────────────
// static class Part36_ExamSimLinq
// {
//     // Data model (simplified UniversityData)
//     record Student(int Id, string FirstName, string LastName, string City);
//     record Course(int Id, string Title, string Category, int LecturerId, int Ects, DateTime StartDate);
//     record Enrollment(int StudentId, int CourseId, double? FinalGrade, bool IsActive);
//     record Lecturer(int Id, string FirstName, string LastName, string Department);
//
//     static List<Student> Students() => new() {
//         new(1, "Anna",   "Kowalska",   "Warsaw"),
//         new(2, "Marek",  "Nowak",      "Krakow"),
//         new(3, "Julia",  "Wiśniewska", "Warsaw"),
//         new(4, "Tomasz", "Lis",        "Warsaw"),
//         new(5, "Lena",   "Adamska",    "Gdansk"),
//     };
//     static List<Course> Courses() => new() {
//         new(1, "C# Basics",       ".NET",  1, 5, new DateTime(2026, 3, 1)),
//         new(2, "LINQ Lab",        ".NET",  1, 3, new DateTime(2026, 4, 1)),
//         new(3, "Azure Cloud",     "Cloud", 2, 4, new DateTime(2026, 4, 5)),
//         new(4, "SQL Foundations", "Data",  2, 4, new DateTime(2026, 2, 1)),
//         new(5, "AI Intro",        "Cloud", 3, 5, new DateTime(2026, 4, 10)),
//     };
//     static List<Enrollment> Enrollments() => new() {
//         new(1, 1, 4.5,  true),
//         new(1, 2, null, true),
//         new(1, 3, null, true),
//         new(2, 1, 5.0,  true),
//         new(2, 3, 4.0,  true),
//         new(3, 2, 3.5,  false),
//         new(4, 4, 4.0,  true),
//         new(5, 5, null, true),
//     };
//     static List<Lecturer> Lecturers() => new() {
//         new(1, "Jan",   "Kowalski", "IT"),
//         new(2, "Maria", "Nowak",    "IT"),
//         new(3, "Adam",  "Wolski",   ""),  // missing department
//     };
//
//     public static void Demo()
//     {
//         var students    = Students();
//         var courses     = Courses();
//         var enrollments = Enrollments();
//         var lecturers   = Lecturers();
//
//         Console.WriteLine("=== QUERY 1: All courses sorted by ECTS descending ===");
//         // TODO:
//         // courses.OrderByDescending(c => c.Ects)
//         //        .Select(c => $"{c.Title} | {c.Category} | {c.Ects} ECTS | {c.StartDate:yyyy-MM-dd}")
//         //        .ToList().ForEach(Console.WriteLine);
//
//         Console.WriteLine("\n=== QUERY 2: For each city: student count and average grade ===");
//         // TODO: GroupBy city, then Join or subquery to get enrollments + average grade
//         // Hint: group students by City, then for each group compute the average grade
//         //       from enrollments; if no grades, print "No grades"
//
//         Console.WriteLine("\n=== QUERY 3: Courses starting in April 2026 with NO final grades ===");
//         // TODO: filter courses by StartDate year==2026 month==4,
//         //       then exclude those where ANY enrollment has a grade
//
//         Console.WriteLine("\n=== QUERY 4: Top 2 courses by number of active enrollments ===");
//         // TODO: for each course count active enrollments, take top 2
//
//         Console.WriteLine("\n=== QUERY 5: Students enrolled in BOTH a .NET AND a Cloud course ===");
//         // TODO: build dotnetCourseIds set, cloudCourseIds set, then filter students
//         //       who have at least one enrollment in each set
//
//         Console.WriteLine("\n=== QUERY 6: Lecturers with ALL their enrollments graded ===");
//         // TODO: for each lecturer get their course IDs (ToHashSet()),
//         //       then check All(e => e.FinalGrade.HasValue) for those enrollments
//
//         Console.WriteLine("\n=== QUERY 7: Do all lecturers have a department? (All + IsNullOrEmpty) ===");
//         // TODO: lecturers.All(l => !string.IsNullOrEmpty(l.Department))
//         // EXAM: All() returns TRUE on empty collection!
//
//         Console.WriteLine("\n=== QUERY 8: Paging — page 2, page size 2 of courses sorted by title ===");
//         // TODO: OrderBy(title).Skip(2).Take(2).Select(...)
//     }
// }
