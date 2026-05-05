// ============================================================
//  APBD C# EXAM PRACTICE — Complete Coding Workbook
//  Polish-Japanese Academy of IT
//
//  HOW TO USE:
//  1. Read each TODO comment carefully.
//  2. Write your code where marked.
//  3. Uncomment the test calls in Main() one section at a time.
//  4. Every section maps to a real exam topic from the lectures.
//
//  TOPICS COVERED (in order):
//   Part 1  — Git Knowledge Checks (code + comments)
//   Part 2  — .NET Architecture (CLR, JIT, AOT, IDisposable)
//   Part 3  — Value Types vs Reference Types
//   Part 4  — Properties (auto, init, computed, validated)
//   Part 5  — Null Safety (??, ?., ??=)
//   Part 6  — Records & 'with' expressions
//   Part 7  — Delegates, Action, Func, Predicate, Closures
//   Part 8  — async / await / Task
//   Part 9  — switch Expressions & Pattern Matching
//   Part 10 — Collections (List, Dictionary, HashSet, Queue, Stack)
//   Part 11 — Abstract Classes, virtual/override vs new (hiding)
//   Part 12 — Interfaces (multi-implement, default members)
//   Part 13 — Custom Exceptions & rethrow
//   Part 14 — SOLID: SRP
//   Part 15 — SOLID: OCP + Strategy Pattern
//   Part 16 — SOLID: LSP
//   Part 17 — SOLID: ISP
//   Part 18 — SOLID: DIP + IoC / DI Lifetimes
//   Part 19 — Cohesion & Coupling theory (comment tasks)
//   Part 20 — Clean Code / DRY
//   Part 21 — LINQ: deferred execution trap
//   Part 22 — LINQ: Where, Select, OrderBy, ThenBy
//   Part 23 — LINQ: Any, All, Count, First, FirstOrDefault
//   Part 24 — LINQ: GroupBy, Join, GroupJoin, SelectMany
//   Part 25 — LINQ: Skip/Take (paging), Distinct, Aggregate
//   Part 26 — ref / out / named / optional parameters
//   Part 27 — Generics (reification vs type erasure)
//   Part 28 — yield return (lazy sequences)
//   Part 29 — Strings & StringBuilder
//   Part 30 — Tuples & Deconstruction
//   Part 31 — Adapter Pattern
//   Part 32 — Factory Method & Abstract Factory
//   Part 33 — Singleton Pattern
//   Part 34 — Exam Simulation: Git scenario (comment)
//   Part 35 — Exam Simulation: OOP + DIP
//   Part 36 — Exam Simulation: LINQ Analytics
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ─────────────────────────────────────────────────────────────
// ENTRY POINT — uncomment sections one by one as you finish them
// ─────────────────────────────────────────────────────────────
class Program
{
    static async Task Main()
    {
        Part03_ValueVsReference.Demo();
        Part04_Properties.Demo();
        Part05_NullSafety.Demo();
        Part06_Records.Demo();
        // Part07_Delegates.Demo();
        // await Part08_AsyncAwait.Demo();
        Part09_SwitchPatterns.Demo();
        Part10_Collections.Demo();
        // Part11_AbstractInheritance.Demo();
        Part12_Interfaces.Demo();
        Part13_Exceptions.Demo();
        Part15_OCP_Strategy.Demo();
        Part16_LSP.Demo();
        Part18_DIP.Demo();
        Part21_DeferredExecution.Demo();
        Part22_LinqBasics.Demo();
        Part23_LinqQuantifiers.Demo();
        Part24_LinqJoins.Demo();
        Part25_LinqPaging.Demo();
        Part26_RefOut.Demo();
        Part27_Generics.Demo();
        Part28_YieldReturn.Demo();
        Part29_Strings.Demo();
        Part30_Tuples.Demo();
        Part31_Adapter.Demo();
        Part32_Factory.Demo();
        Part33_Singleton.Demo();
        await Part35_ExamSimOOP.Demo();
        Part36_ExamSimLinq.Demo();
        // await Part37_AdoNet_SqlConnection.Demo();
        // await Part38_AdoNet_SqlCommand.Demo();
        // await Part39_AdoNet_Parameterization.Demo();
        // await Part40_AdoNet_SqlDataReader.Demo();
        // await Part41_AdoNet_Transactions.Demo();

        Console.WriteLine("Done! Uncomment sections in Main() to test each part.");
    }
}


// ─────────────────────────────────────────────────────────────
// PART 1 — GIT KNOWLEDGE CHECKS
// ─────────────────────────────────────────────────────────────
// These are comment-only tasks. Answer them in comments below each question.
// Exams often ask you to write the exact git commands.

static class Part01_Git
{
    // TASK 1-A: Write the git commands to:
    //   1. Set your global name and email
    //   2. Create a new local repo
    //   3. Stage all files and make a commit
    //   4. Create and switch to a branch called feature/login
    //
    // git config --global user.name ""
    // git config --global user.email ""
    // git init
    // git add .
    // git commit -m "initial commit"
    // git branch feature/login

    // TASK 1-B: Explain the difference between git merge and git rebase.
    //   - Which produces linear history? rebase
    //   - When should you NEVER rebase? ever rebase branches that have been pushed to a public repository or shared with other developers.
    //
    // merge = combining; rebase = choicing best

    // TASK 1-C: You are on feature/reports. Main received new commits.
    //   Your team lead wants LINEAR history. Write every command:
    //   from "catch up with main" to "feature/reports deleted".
    //
    // git checkout main
    // git rebase main
    // git rebase --ff-only feature/reports
    // git branch -D feature/reports

    // TASK 1-D: What does a conflict look like in a file?
    //   Write the 4 steps to resolve a conflict during a rebase.
    //   IMPORTANT: what must you do AFTER editing the file, beyond 'git add'?
    //
    // status -> vim conflicting file -> add -> commit/rebase/ --abort

    // TASK 1-E: What is --ff-only and why is it used after a rebase?
    // if something wrong dont add it to my beautiful linear history
}


// ─────────────────────────────────────────────────────────────
// PART 2 — .NET ARCHITECTURE (CLR, JIT, AOT, IDisposable)
// ─────────────────────────────────────────────────────────────
static class Part02_DotNetArchitecture
{
    // TASK 2-A: In comments, describe what each component does:
    //   Roslyn, CLR, JIT, AOT, BCL, NuGet
    //   Include the Java equivalent for each.
    //
    // Roslyn = create .dll with lL bytecode
    // CLR    = insure runtime correctness with lL, GC, others
    // JIT    =   (exam: compiles IL at run time)
    // AOT    =   (exam: compiles IL before the app runs)
    // BCL    = Base class library
    // NuGet  = package management

    // TASK 2-B: Implement IDisposable correctly on a class that wraps a resource.
    //   The using statement should automatically call Dispose().
    // using var conn = new SqlConnection(connectionString);

    // public class ManagedFile : IDisposable
    // {
    //     private System.IO.FileStream? _stream;
    //
    //     public ManagedFile(string path)
    //     {
    //         _stream = File.OpenRead(path);
    //     }
    //
    //     public void Dispose()
    //     {
    //         _stream?.Dispose();
    //     }
    // }
    //
    // public static void Demo()
    // {
    //     using var conn = new SqlConnection(connectionString);
    // }
}


// ─────────────────────────────────────────────────────────────
// PART 3 — VALUE TYPES vs REFERENCE TYPES
// ─────────────────────────────────────────────────────────────
static class Part03_ValueVsReference
{
    // TASK 3-A: Create a struct Point with X and Y.
    //   Show that copying a struct creates an independent copy.
    //
    public struct Point { public int x,y; }

    public static void RunTask()
    {
        Point first = new Point { x = 1, y = 2 };
        Point second = first;
        second.x = 100;
        Console.WriteLine($"First X: {first.x}"); // Output: 1
        Console.WriteLine($"Second X: {second.x}"); // Output: 99
    }

    // static void Main()
    // {
    //     RunTask();
    //     RunTask2();
    //     Demo();
    // }
    
    

    // TASK 3-B: Create a class Color with R, G, B.
    //   Show that copying a class variable shares the reference.
    //
    public class Color { public int r,g,b; }

    public static void RunTask2()
    {
        Color color = new Color { r = 255, g = 128, b = 64 };
        Color copy = color;
        Console.WriteLine($"Color R: {color.r}");
        copy.r = 128;
        Console.WriteLine($"Color R: {color.r}");
    }

    // TASK 3-C: Floating-point trap — classic exam question.
    //   Show that 0.1 + 0.2 != 0.3 with double.
    //   Then fix it using decimal.
    //
    public struct Point2 { public int X,Y; }
    public class Color2 { public int R,G,B; }
    public static void Demo()
    {
        var p1 = new Point2 { X = 1, Y = 2 };
        var p2 = p1;
        p2.X = 99;
        Console.WriteLine(p1.X); // expected: 1

        var c1 = new Color2 { R = 255 };
        var c2 = c1;
        c2.R = 128;
        Console.WriteLine(c1.R); // expected: 128
        
        double d = 0.1 + 0.2;
        Console.WriteLine(d == 0.3);   // expected: False
        decimal m = 0.1m + 0.2m;
        Console.WriteLine(m == 0.3m);  // expected: True
    }
}


// ─────────────────────────────────────────────────────────────
// PART 4 — PROPERTIES (all 5 forms)
// ─────────────────────────────────────────────────────────────
static class Part04_Properties
{
    // TASK 4-A: Create a BankAccount class with:
    //   - AccountNumber: init-only (cannot change after object init)
    //   - Balance: private set (only the class can change it)
    //   - Owner: validated — throw ArgumentException if empty/whitespace
    //   - IsRich: computed property (returns true if Balance > 10_000)
    //   - Constructor accepting accountNumber, owner, initialBalance
    //   - Deposit(decimal amount) and Withdraw(decimal amount) with overdraft check
    //   - Override ToString() to show account nicely
    public class BankAccount
    {
       private decimal _balance;
       private string _owner;
       
       public string AccoundNumber { get; init; }

       public string Owner
       {
           get => _owner;
           set
           {
               if(string.IsNullOrWhiteSpace(value)) throw new ArgumentException("empty");
               _owner = value;
           }
       }
       
       public decimal Balance
       {
           private set => _balance = value;
           get => _balance;
       }
       
       public bool IsRich => Balance > 10_000;

       public BankAccount(string balance, string owner, decimal accountNumber)
       {
           Balance = accountNumber;
           Owner = owner;
           AccoundNumber = balance;
       }

       public void Deposit(decimal amount)
        {
            if (amount < 0) throw new ArgumentException("negative");
            if (amount > _balance) throw new ArgumentException("overdraft");
            Balance += amount;
        }
        
        public void Withdraw(decimal amount)
        {
            if (amount < 0) throw new ArgumentException("negative");
            try
            {
                Balance -= amount;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Withdrawal successful u left with " + _balance + "$" );
        }

        public override string ToString()
        {
            return AccoundNumber + " for " + Owner + " " + _balance + "$";
        }
    }

    public static void Demo()
    {
        var acc = new BankAccount("PL001", "Anna", 1000m);
        acc.Deposit(500m);
        acc.Withdraw(200m);
        Console.WriteLine(acc);

        // acc.AccoundNumber = "NEW"; // COMPILER ERROR — init only!
        // Init-only property 'Part04_Properties.BankAccount.AccoundNumber' can only be
        // assigned in an object initializer, or on 'this' or 'base' in an instance constructor
        // or an 'init' accessor
    }

    // static void Main()
    // {
    //     Demo();
    // }
}


// ─────────────────────────────────────────────────────────────
// PART 5 — NULL SAFETY (??, ?., ??=)
// ─────────────────────────────────────────────────────────────
static class Part05_NullSafety
{
    public static void Demo()
    {
        // TASK 5-A: Use ?? to provide a fallback when a string is null
        string maybeNull = null;
        Console.WriteLine(maybeNull); // expected: "default"

        // TASK 5-B: Use ?. to safely access Length without NullReferenceException
        int? len = maybeNull?.Length;
        Console.WriteLine(len); // expected: (nothing — null)

        // TASK 5-C: Use ??= to assign only when null
        string? name = null;
        name ??= "Anonymous";
        Console.WriteLine(name); // expected: Anonymous

        // TASK 5-D: Chain ?. and ?? together
        string? input = null;
        string upper = input?.Trim()?.ToUpper() ?? "EMPTY";
        Console.WriteLine(upper); // expected: EMPTY
    }

    // static void Main()
    // {
    //     Demo();
    // }
}


// ─────────────────────────────────────────────────────────────
// PART 6 — RECORDS & 'with' EXPRESSIONS
// ─────────────────────────────────────────────────────────────
static class Part06_Records
{
    // TASK 6-A: Define a positional record Student(int Id, string Name, string City, string Email)
    public record Student(int Id, string Name, string City, string Email);

    // static void Main()
    // {
    //     Demo();
    // }

    public static void Demo()
    {
        // TASK 6-B: Create two Student records with identical values.
        //   Show they are equal (value equality, not reference equality).
        var s1 = new Student(1, "Anna", "Warsaw", "anna@uni.pl");
        var s3 = new Student(1, "Anna", "Warsaw", "anna@uni.pl");
        Console.WriteLine(s1 == s3); // expected: True

        // TASK 6-C: Use 'with' to create a copy with a different City.
        //   Prove the original is unchanged.
        var s2 = s1 with { City = "Krakow" };
        Console.WriteLine(s2.City); // Krakow
        Console.WriteLine(s1.City); // Warsaw — original untouched

        // TASK 6-D: Deconstruct the record into separate variables
        var (id, name, city, email) = s1;
        Console.WriteLine($"{name} from {city}");

        // TASK 6-E: Print the record — observe the auto-generated ToString()
        Console.WriteLine(s1);
    }
}


// ─────────────────────────────────────────────────────────────
// PART 7 — DELEGATES, ACTION, FUNC, PREDICATE, CLOSURES
// ─────────────────────────────────────────────────────────────
static class Part07_Delegates
{
    public static void Run()
    {
        // TASK 7-A: Create an Action<string> that prints to console with a prefix
        Action<string> printWithPrefux = s => Console.WriteLine($"[prefix] {s}");
        printWithPrefux("hello");

        // TASK 7-B: Create a Func<int, int, string> that formats addition
        Func<int, int, string> addFormat = (a,b) => $"({a} + {b}) = {a+b}";
        Console.WriteLine(addFormat(1, 2));

        // TASK 7-C: Create a Predicate<int> that checks if a number is positive
        Predicate<int> isPredicate = n => n > 0;
        Console.WriteLine(isPredicate(10));

        // TASK 7-D: CLOSURE TRAP — the most classic exam question!
        //   Create a list of 3 lambdas capturing 'i' from a loop.
        //   Run them. What prints? Why? (Answer: 3 3 3)
        //   Fix it by capturing a copy of i.
        var fix_actions = new List<Action>();
        for (int i = 0; i < 3; i++)
        {
            var copy = i;
            fix_actions.Add((() => Console.WriteLine(copy)));
        }
        foreach (var a in fix_actions) a();
    }

    // static void Main()
    // {
    //     Run();
    //     Demo();
    // }
}


// ─────────────────────────────────────────────────────────────
// PART 8 — ASYNC / AWAIT / TASK
// ─────────────────────────────────────────────────────────────
static class Part08_AsyncAwait
{
    // TASK 8-A: Write an async method that simulates fetching data with Task.Delay
    //   Return a string result after 1 second.
    public static async Task<string> FetchDataAsync(string url)
    {
        await Task.Delay(1000); // simulate I/O — does NOT block a thread
        return $"Data from {url}";
    }

    // TASK 8-B: Run two async tasks in PARALLEL using Task.WhenAll
    public static async Task RunParallel()
    {
        var t1 = FetchDataAsync("http://api1.com");
        var t2 = FetchDataAsync("http://api2.com");
        var results = await Task.WhenAll(t1, t2);
        foreach (var r in results) Console.WriteLine(r);
    }

    // static void Main()
    // {
    //     RunParallel();
    // }
}


// ─────────────────────────────────────────────────────────────
// PART 9 — SWITCH EXPRESSIONS & PATTERN MATCHING
// ─────────────────────────────────────────────────────────────
static class Part09_SwitchPatterns
{
    // static void Main()
    // {
    //  Demo();       
    // }
    public static void Demo()
    {
        
        // TASK 9-A: Classic switch expression — map day number (1–7) to name
        int day = 3;
        string datName = day switch { 1 => "Mondey", 2=>"Tuesday", 3=>"Wednesday", 4=>"Thursday", 5=>"Friday", 6=>"Saturday", 7=>"Sunday", _ => "Invalid day"};
        Console.WriteLine(datName);

        // TASK 9-B: Pattern matching on types
        object obj = 3.14;
        string describe = obj switch
        {
            int i => $"int {i}",
            double d => $"double {d}",
            _ => "unknown"
        };
        Console.WriteLine(describe);

        // TASK 9-C: Guards with 'when' — grade classifier
        int score = 82;
        string grade = score switch
        {
            >= 90 => "A",
            >= 75 => "B",
            >= 60 => "C",
            _     => "F"
        };
        Console.WriteLine(grade);

        // EXAM: C# PROHIBITS fall-through between non-empty case blocks.
        //   Java ALLOWS fall-through (you need 'break' to prevent it).
        //   In C#, the compiler ENFORCES break/return/throw — no implicit fall-through.
        // Unhandled exception. System.Runtime.CompilerServices.SwitchExpressionException: Non-exhaustive switch expression failed to match its input.
        //     Unmatched value was 82.
        //     at <PrivateImplementationDetails>.ThrowSwitchExpressionException(Object unmatchedValue)
        // at Part09_SwitchPatterns.Demo() in /Users/anastasiiakravchenko/RiderProjects/Practice/Practice/Program.cs:line 515
        // at Part09_SwitchPatterns.Main() in /Users/anastasiiakravchenko/RiderProjects/Practice/Practice/Program.cs:line 493

    }
}


// ─────────────────────────────────────────────────────────────
// PART 10 — COLLECTIONS
// ─────────────────────────────────────────────────────────────
static class Part10_Collections
{
    public static void Demo()
    {
        // TASK 10-A: List<T> — add, remove, index access, Contains (O(n))
        var list = new List<string> { "alpha", "beta", "gamma" };
        list.Add("delta");
        list.Remove("beta");
        Console.WriteLine(list[0]); // alpha

        // TASK 10-B: Dictionary<K,V> — add, lookup, ContainsKey (O(1))
        var dict = new Dictionary<string, int> { {"hi", 1} };
        dict["alpha"] = 1; dict["beta"] = 2;
        Console.WriteLine(dict.ContainsKey("alpha")); // True
        Console.WriteLine(dict["alpha"]);              // 1

        // TASK 10-C: HashSet<T> — unique items, Contains is O(1)
        var set = new HashSet<int> { 1, 2, 3, 2, 1 };
        Console.WriteLine(set.Count); // 3 (duplicates removed)
        Console.WriteLine(set.Contains(2)); // True

        // TASK 10-D: Queue<T> — FIFO (Enqueue/Dequeue/Peek)
        var queue = new Queue<string>();
        queue.Enqueue("first"); queue.Enqueue("second");
        Console.WriteLine(queue.Dequeue()); // first

        // TASK 10-E: Stack<T> — LIFO (Push/Pop/Peek)
        var stack = new Stack<string>();
        stack.Push("first"); stack.Push("second");
        Console.WriteLine(stack.Pop()); // second

        // EXAM: Dictionary (ContainsKey) and HashSet both offer O(1) Contains.
        //       List.Contains is O(n) — scans every element.
        //       SortedDictionary offers O(log n) access — uses a red-black tree.
    }
}


// ─────────────────────────────────────────────────────────────
// PART 11 — ABSTRACT CLASSES, virtual/override vs new (HIDING)
// ─────────────────────────────────────────────────────────────
static class Part11_AbstractInheritance
{
    // TASK 11-A: Create an abstract class Animal with:
    //   - Property Name (string)
    //   - Abstract method Speak() → string
    //   - Virtual method Describe() that prints "Name: Speak()"
    //   - Override ToString()
    //
    public abstract class Animal
    {
        public string Name { get; set; } = string.Empty;
        public abstract string Speak();
    
        // Regular method in the base class
        public void Eat() => Console.WriteLine("Animal is eating...");
    
        public virtual string Describe() => $"{Name}: {Speak()}";
        public override string ToString() => Describe();
    }

    public class SpecialDog : Dog
    {
        // 'new' hides the parent's Eat() method instead of overriding it.
        // Notice it doesn't need to be virtual in the parent to do this.
        public new void Eat() => Console.WriteLine("SpecialDog is eating fancy kibble!");
    }

    // TASK 11-B: Create Dog, Cat, Fish subclasses.
    //   Fish should OVERRIDE Describe() and also call base.Describe().
    //
    public class Dog : Animal{
        public override string Speak() => "Woof!";
    }

    // TASK 11-C: Create SpecialDog that HIDES Eat() using 'new' (NOT override).
    //   Show the difference:
    //     Animal a = new SpecialDog(); a.Eat() → calls Animal's version (no polymorphism!)
    //     SpecialDog sd = new SpecialDog(); sd.Eat() → calls SpecialDog's version
    //
    public static void RunExample()
    {
        // Case 1: The variable is an Animal
        Animal a = new SpecialDog(); 
        a.Eat(); 
        // Output: "Animal is eating..." 
        // Why? Because 'a' is looking at the Animal "map," and it can't see the hidden method.

        // Case 2: The variable is a SpecialDog
        SpecialDog sd = new SpecialDog();
        sd.Eat(); 
        // Output: "SpecialDog is eating fancy kibble!"
        // Why? Because 'sd' is looking at the SpecialDog "map" which hides the old version.
    }

    // static void Main()
    // {
    //     RunExample();
    // }

}


// ─────────────────────────────────────────────────────────────
// PART 12 — INTERFACES
// ─────────────────────────────────────────────────────────────
static class Part12_Interfaces
{
    // TASK 12-A: Define interfaces ISaveable and IPrintable.
    //   Then create a class Document that implements BOTH.
    //
    interface IRepository { void Save(string data); }
    interface IPrintable  { void Print(); }

    // static void Main()
    // {
    //     Demo();
    // }

    class Document : IRepository, IPrintable, ILoggable
    {
        private string _content = "";
        public void Save(string data) => _content = data;
        public void Print() => Console.WriteLine(_content);
    }

    // TASK 12-B: Demonstrate a default interface method (C# 8+)
    //   Add a default Log() method directly on the interface.
    //
    interface ILoggable
    {
        void Log() => Console.WriteLine("[LOG] " + GetType().Name);
    }

    public static void Demo()
    {
        var doc = new Document();
        IRepository r = doc;
        IPrintable  p = doc;
        r.Save("Hello World");
        p.Print();
        ILoggable l = doc;
        l.Log();
        

        // EXAM: Interfaces CANNOT have backing fields.
        //       Interfaces CANNOT have constructors.
        //       A class CAN implement multiple interfaces.
        //       Default implementations allowed since C# 8.
        //       Convention: interface names start with 'I'.
    }
}


// ─────────────────────────────────────────────────────────────
// PART 13 — CUSTOM EXCEPTIONS & RETHROW
// ─────────────────────────────────────────────────────────────
static class Part13_Exceptions
{
    // TASK 13-A: Create two custom exceptions:
    //   InsufficientFundsException
    //   AccountFrozenException
    //   Both inherit from Exception and accept a message.
    //
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(string message) : base(message) { }
    }

    // TASK 13-B: Write a BankService.Withdraw() that throws these exceptions.
    //   Catch them separately and rethrow with 'throw;' (not 'throw ex;').
    //   Explain WHY 'throw;' preserves the stack trace but 'throw ex;' resets it.
    //
    public static void Demo()
    {
        // try
        // {
        //     BankService.Withdraw(account, 999_999m);
        // }
        // catch (AccountFrozenException ex)
        // {
        //     Console.WriteLine("Account frozen: " + ex.Message);
        //     throw; // preserves original stack trace — ALWAYS use this!
        //            // NEVER: throw ex; -- that resets the stack trace!
        // }
        // catch (InsufficientFundsException ex)
        // {
        //     Console.WriteLine("Not enough money: " + ex.Message);
        // }
    }
}

// ─────────────────────────────────────────────────────────────
// PART 14 — SOLID: SRP (Single Responsibility Principle)
// ─────────────────────────────────────────────────────────────
public class Order { public string CustomerEmail = ""; public decimal Total = 0; }

static class Part14_SRP
{
    // TASK 14-A: Split OrderService_BAD into 4 specialized interfaces and implementations
    
    // 1. Business Analyst (Validation rules)
    public interface IOrderValidator { bool Validate(Order o); }
    public class OrderValidator : IOrderValidator 
    { 
        public bool Validate(Order o) => o != null && o.Total >= 0; 
    }

    // 2. Product Owner (Pricing/Discounts)
    public interface IDiscountCalc { decimal Calculate(Order o); }
    public class DiscountCalculator : IDiscountCalc 
    { 
        public decimal Calculate(Order o) => o.Total * 0.1m; 
    }

    // 3. DBA (Database/Storage)
    public interface IOrderRepository { void Save(Order o); }
    public class SqlOrderRepository : IOrderRepository 
    { 
        public void Save(Order o) => Console.WriteLine("Order saved to SQL"); 
    }

    // 4. UX/Marketing (Notifications)
    public interface INotificationSvc { void Notify(Order o); }
    public class EmailNotificationService : INotificationSvc 
    { 
        public void Notify(Order o) => Console.WriteLine($"Email sent to {o.CustomerEmail}"); 
    }

    // Orchestrator: Responsible ONLY for coordinating the flow (uses constructor injection)
    public class OrderOrchestrator
    {
        private readonly IOrderValidator _validator;
        private readonly IDiscountCalc _calc;
        private readonly IOrderRepository _repo;
        private readonly INotificationSvc _notifier;

        public OrderOrchestrator(
            IOrderValidator validator, IDiscountCalc calc, 
            IOrderRepository repo, INotificationSvc notifier)
        {
            _validator = validator;
            _calc = calc;
            _repo = repo;
            _notifier = notifier;
        }

        public void ProcessOrder(Order o)
        {
            if (_validator.Validate(o))
            {
                var discount = _calc.Calculate(o);
                o.Total -= discount;
                _repo.Save(o);
                _notifier.Notify(o);
            }
        }
    }
}

// ─────────────────────────────────────────────────────────────
// PART 15 — SOLID: OCP + STRATEGY PATTERN
// ─────────────────────────────────────────────────────────────
static class Part15_OCP_Strategy
{
    // TASK 15-A: OCP Discount refactor using Strategy Pattern
    public interface IDiscountRule { decimal Calculate(decimal amount); }
    public class RegularDiscountRule : IDiscountRule { public decimal Calculate(decimal amount) => amount * 0.05m; }
    public class PremiumDiscountRule : IDiscountRule { public decimal Calculate(decimal amount) => amount * 0.10m; }
    public class VipDiscountRule     : IDiscountRule { public decimal Calculate(decimal amount) => amount * 0.20m; }
    public class CorporateDiscountRule : IDiscountRule { public decimal Calculate(decimal amount) => amount * 0.30m; }

    public class DiscountEngine
    {
        private readonly IEnumerable<IDiscountRule> _rules;
        public DiscountEngine(IEnumerable<IDiscountRule> rules) => _rules = rules;

        public decimal GetTotalDiscount(decimal amount)
        {
            // Adding a new rule class doesn't require modifying this loop!
            return _rules.Sum(r => r.Calculate(amount));
        }
    }

    // TASK 15-B: OCP Report formatting refactor
    public interface IReportFormatter { string Format(IEnumerable<string> data); }
    
    public class ConsoleFormatter : IReportFormatter { public string Format(IEnumerable<string> d) => string.Join("\n", d); }
    public class CsvFormatter     : IReportFormatter { public string Format(IEnumerable<string> d) => string.Join(",", d); }
    public class HtmlFormatter    : IReportFormatter { public string Format(IEnumerable<string> d) => $"<ul><li>{string.Join("</li><li>", d)}</li></ul>"; }
    public class JsonFormatter    : IReportFormatter { public string Format(IEnumerable<string> d) => "[\"" + string.Join("\",\"", d) + "\"]"; }

    public class ReportGenerator
    {
        private readonly IReportFormatter _formatter;
        public ReportGenerator(IReportFormatter formatter) => _formatter = formatter;
        public void Generate(IEnumerable<string> data) => Console.WriteLine(_formatter.Format(data));
    }

    public static void Demo()
    {
        var data = new List<string> { "Item1", "Item2", "Item3" };
        
        // Swapping behavior without touching ReportGenerator
        var csvReport = new ReportGenerator(new CsvFormatter());
        csvReport.Generate(data); // Item1,Item2,Item3

        var jsonReport = new ReportGenerator(new JsonFormatter());
        jsonReport.Generate(data); // ["Item1","Item2","Item3"]
    }
}

// ─────────────────────────────────────────────────────────────
// PART 16 — SOLID: LSP (Liskov Substitution Principle)
// ─────────────────────────────────────────────────────────────
static class Part16_LSP
{
    // TASK 16-A: The Classic Violation
    class Rectangle 
    { 
        public virtual double Width { get; set; } 
        public virtual double Height { get; set; } 
        public double Area => Width * Height;
    }

    class Square_BAD : Rectangle
    {
        // By forcing both dimensions to be the same, we break the caller's expectation 
        // that modifying Width leaves Height alone.
        public override double Width 
        { 
            get => base.Width; 
            set { base.Width = value; base.Height = value; } 
        }
        public override double Height 
        { 
            get => base.Height; 
            set { base.Height = value; base.Width = value; } 
        }
    }

    // TASK 16-B: The LSP Fix (Separate implementations, common interface)
    interface IShape { double GetArea(); }
    class RectangleGood : IShape 
    { 
        public double Width { get; set; } 
        public double Height { get; set; } 
        public double GetArea() => Width * Height; 
    }
    class SquareGood : IShape 
    { 
        public double SideLength { get; set; } 
        public double GetArea() => SideLength * SideLength; 
    }

    public static void Demo()
    {
        // LSP VIOLATION: Caller expects a rectangle
        Rectangle rect = new Square_BAD();
        rect.Width = 5;
        rect.Height = 10; 
        // Caller expects 5 * 10 = 50. 
        // Actually gets 100 because setting Height=10 also set Width=10.
        Console.WriteLine($"Violation Area (Expected 50): {rect.Area}"); 

        // CORRECT FIX:
        IShape shape = new SquareGood { SideLength = 10 };
        Console.WriteLine($"Fixed Area: {shape.GetArea()}");
    }
}

// ─────────────────────────────────────────────────────────────
// PART 17 — SOLID: ISP (Interface Segregation Principle)
// ─────────────────────────────────────────────────────────────
public class Student { public int Id { get; set; } }

static class Part17_ISP
{
    // TASK 17-A: Split fat interface into role-based interfaces
    public interface IStudentReader
    {
        Student GetById(int id);
        IEnumerable<Student> GetAll();
    }

    public interface IStudentWriter
    {
        void Add(Student s);
        void Delete(int id);
    }

    public interface IStudentExporter
    {
        byte[] ExportToPdf();
    }

    // Now, read-only classes only implement what they need (No NotImplementedExceptions!)
    public class ReadOnlyStudentRepository : IStudentReader
    {
        public Student GetById(int id) => new Student { Id = id };
        public IEnumerable<Student> GetAll() => new List<Student>();
    }
}

// ─────────────────────────────────────────────────────────────
// PART 18 — SOLID: DIP + IoC / DI Lifetimes
// ─────────────────────────────────────────────────────────────
static class Part18_DIP
{
    // TASK 18-A: Build a 3-layer system with manual wiring
    public record Product(int Id, string Name, decimal Price);

    public interface IProductRepository { IEnumerable<Product> GetAll(); }
    public interface IProductService    { IEnumerable<string> GetFormatted(); }

    public class InMemoryProductRepository : IProductRepository 
    { 
        public IEnumerable<Product> GetAll() => new[] { new Product(1, "Laptop", 1000m), new Product(2, "Smartphone", 500m) };  
    }

    public class ProductService : IProductService 
    { 
        private readonly IProductRepository _repo;
        public ProductService(IProductRepository repo) => _repo = repo;
        
        public IEnumerable<string> GetFormatted() => 
            _repo.GetAll().Select(p => $"{p.Name} - {p.Price:C}");
    }

    public class ProductController 
    { 
        private readonly IProductService _service;
        public ProductController(IProductService service) => _service = service;

        public void ShowAll() 
        {
            foreach(var item in _service.GetFormatted()) Console.WriteLine(item);
        }
    }

    public static void Demo()
    {
        // Composition Root: Manual constructor injection wiring
        var controller = new ProductController(
            new ProductService(
                new InMemoryProductRepository()));
                
        controller.ShowAll();
    }

    /*
    TASK 18-B: DI Lifetimes
      AddTransient  = new instance every time it is requested.
      AddScoped     = one instance per HTTP Request (or scope boundary).
      AddSingleton  = one instance for the entire lifetime of the application (must be thread-safe).
    
    EXAM: NEVER inject a Scoped service into a Singleton. Why?
      Because the Singleton is created once and lives forever. If it holds a reference 
      to a Scoped service (like a Database Context), that Scoped service will never be 
      disposed, creating memory leaks and breaking database connection pooling (Captive Dependency).

    TASK 18-C: Why is Service Locator an ANTI-PATTERN?
      Because it hides dependencies inside the class (e.g., calling Locator.GetService<IDb>() 
      in the middle of a method) instead of explicitly declaring them in the constructor. 
      This makes the code much harder to test and makes the class's requirements invisible 
      from the outside.
    */
}

// ─────────────────────────────────────────────────────────────
// PART 19 — COHESION & COUPLING (theory)
// ─────────────────────────────────────────────────────────────
static class Part19_CohesionCoupling
{
    /*
    TASK 19-A: Rank cohesion types from BEST to WORST (7 types)
      1. Functional (Best) - Elements contribute to a single, well-defined task.
      2. Sequential - Output of one part is the input to the next.
      3. Communicational - Elements operate on the same data.
      4. Procedural - Elements are grouped to ensure a specific sequence of execution.
      5. Temporal - Elements are executed at the same time (e.g., initialization logic).
      6. Logical - Elements do similar things but are otherwise unrelated (e.g., all IO operations).
      7. Coincidental (Worst) - Elements have absolutely no relationship.

    TASK 19-B: 3 warning signs of LOW cohesion
      1. Class name is 'Helper', 'Utils', or 'Manager' without specific context.
      2. The class has a high number of private variables, but methods only use 1 or 2 of them.
      3. Changing one feature of the system forces you to open and modify this class frequently.

    TASK 19-C: Rank coupling types from WORST to BEST (5 types)
      1. Content (Worst) - One module directly alters or relies on the internal workings of another.
      2. Common - Modules share global data/state.
      3. Control - One module passes flags/parameters to dictate the execution path of another.
      4. Stamp - Passing whole objects when only a single property is needed.
      5. Data (Best) - Modules communicate by passing only elementary data parameters.

    TASK 19-D: Code vs Knowledge Duplication
      - Code Duplication: Copy/pasting the exact same algorithm in two places.
      - Knowledge Duplication: Writing code in multiple places that represents the same BUSINESS RULE.
      Knowledge duplication is much worse. If the business rule changes, you have to find and 
      change every instance of it. Code duplication might just be a coincidence that looks similar.
    */
}

// ─────────────────────────────────────────────────────────────
// PART 19 — COHESION & COUPLING (Code Examples)
// ─────────────────────────────────────────────────────────────
static class Part19_CohesionCoupling_Examples
{
    // =========================================================
    // TASK 19-A & 19-B: COHESION EXAMPLES
    // Cohesion is about how related the code inside a class is.
    // =========================================================

    // 🔴 WORST: Coincidental Cohesion (The "Junk Drawer")
    // Warning Sign 1: Name is "Utils" or "Helper".
    // Warning Sign 2: Methods share absolutely no data or purpose.
    public class MiscellanousUtils
    {
        public void CalculateTax() { /* ... */ }
        public void SendEmail() { /* ... */ }
        public void ParseXml() { /* ... */ }
    }

    // 🟡 POOR: Logical Cohesion 
    // Grouped just because they do the "same kind of thing" (IO operations),
    // but they serve totally different parts of the business.
    public class FileProcessor
    {
        public void ReadEmployeeData() { /* ... */ }
        public void WriteSystemErrorLog() { /* ... */ }
        public void ParseCustomerInvoices() { /* ... */ }
    }

    // 🟢 BEST: Functional Cohesion (Single Responsibility)
    // Every method and property works together to perform ONE exact task.
    public class PasswordHasher
    {
        private readonly string _salt;
        public PasswordHasher(string salt) => _salt = salt;

        public string Hash(string password) => password + _salt; // (Simplified)
        public bool Verify(string password, string hash) => Hash(password) == hash;
    }


    // =========================================================
    // TASK 19-C: COUPLING EXAMPLES
    // Coupling is about how heavily classes depend on each other.
    // =========================================================

    // 🔴 WORST: 1. Content Coupling
    // One class directly manipulates the internal, hidden state of another.
    public class ShoppingCart { public decimal _internalTotal; /* Should be private! */ }
    public class Checkout 
    { 
        public void Process(ShoppingCart cart) { cart._internalTotal = 0; } // Hijacking internal state!
    }

    // 🟠 BAD: 2. Common (Global) Coupling
    // Classes communicate through a shared global state. If this breaks, everything breaks.
    public static class GlobalState { public static int CurrentUserId; }
    public class ProfileViewer { public void Show() => Console.WriteLine(GlobalState.CurrentUserId); }

    // 🟡 POOR: 3. Control Coupling
    // Passing a "flag" (boolean/enum) to tell the other method what to do inside.
    // Fix: Split this into two separate methods (FormatAsPdf() and FormatAsCsv()).
    public class ReportGenerator
    {
        public void Generate(bool isPdf) 
        {
            if (isPdf) { /* ... */ } else { /* ... */ }
        }
    }

    // 🟢 OKAY: 4. Stamp Coupling
    // Passing a massive object when you only need one tiny piece of data from it.
    public class User { public int Id { get; set; } public string Name { get; set; } /* ... 50 other properties */ }
    public class AccessChecker 
    { 
        // BAD: Why require the whole User object?
        public bool HasAccess_Stamp(User u) => u.Id == 99; 
    }

    // 🌟 BEST: 5. Data Coupling
    // Passing ONLY the exact, simple primitive data required.
    public class AccessCheckerBetter
    {
        // GOOD: Any integer can be checked, no heavy objects attached.
        public bool HasAccess_Data(int userId) => userId == 99;
    }


    // =========================================================
    // TASK 19-D: CODE vs KNOWLEDGE DUPLICATION
    // =========================================================

    // KNOWLEDGE DUPLICATION (The dangerous one!)
    // The BUSINESS RULE is "Free shipping over $50". 
    // Here, that exact knowledge is duplicated in two completely different classes.
    public class CartService
    {
        public decimal GetTotal(decimal subtotal) => subtotal >= 50m ? subtotal : subtotal + 5m;
    }

    public class BannerAdService
    {
        public string GetPromoText(decimal subtotal) => subtotal >= 50m ? "You get free shipping!" : "Spend more!";
    }
    // Why is this bad? If the marketing team changes the free shipping threshold to $100, 
    // you have to find and update this rule in both classes. If you forget one, you have a critical bug.
    // Fix: Move the rule to a central `ShippingPolicy` class.


    // CODE DUPLICATION (Often acceptable)
    // Both methods use a `foreach` loop to filter a list. The CODE looks identical.
    // However, the REASONS they exist are totally different. This is fine! Don't merge them just to save 3 lines of code.
    public class Operations
    {
        public void FilterActiveUsers() { /* foreach user in users if active... */ }
        public void FilterInStockProducts() { /* foreach product in products if instock... */ }
    }
}

// ─────────────────────────────────────────────────────────────
// PART 20 — CLEAN CODE / DRY
// ─────────────────────────────────────────────────────────────

public class OrderItem 
{ 
    public decimal Price; 
    public int Quantity; 
}

public class CompleteOrder 
{ 
    public string CustomerType = ""; 
    public List<OrderItem> Items = new(); 
    public string CustomerEmail = ""; 
}

static class Part20_CleanCode
{
    /*
    TASK 20-A: Rename bad variables
    Rationale: Variable names should reveal intent and be specific to avoid ambiguity.
      - d              -> daysUntilExpiration (Clarifies what the number represents)
      - tmp            -> previousValue (Describes the data's role in the logic)
      - processStuff() -> CalculateMonthlyPayroll() (Uses a clear verb and specific noun)
      - getData()      -> FetchActiveUsers() (Specifies the source action and the entity type)
    */

    // TASK 20-B: Refactor to do ONE thing
    // The main method now acts as an orchestrator, delegating specific tasks to helper methods.
    public static void ProcessOrder_Good(CompleteOrder o)
    {
        ValidateOrder(o);
        decimal total = CalculateTotal(o.Items);
        total = ApplyDiscount(total, o.CustomerType);
        SaveOrder(o);
        NotifyCustomer(o.CustomerEmail, total);
    }

    // Extraction 1: Validation logic
    private static void ValidateOrder(CompleteOrder o)
    {
        if (o == null) throw new ArgumentNullException(nameof(o));
        if (o.Items.Count == 0) throw new InvalidOperationException("No items");
    }

    // Extraction 2: Mathematical calculation using LINQ
    private static decimal CalculateTotal(List<OrderItem> items) => 
        items.Sum(i => i.Price * i.Quantity);

    // Extraction 3: Business rule/logic for tiers
    private static decimal ApplyDiscount(decimal total, string type) => type switch
    {
        "VIP" => total * 0.80m,
        "Premium" => total * 0.90m,
        _ => total
    };

    private static void SaveOrder(CompleteOrder o) { /* DB Save Logic */ }
    
    private static void NotifyCustomer(string email, decimal total) 
    { 
        Console.WriteLine($"Order total: {total:C} sent to {email}"); 
    }

    // TASK 20-C: Command-Query Separation (CQS)
    // Principle: A method should either perform an action (Command) or return data (Query), but not both.
    public class User { public int Id { get; set; } }
    
    // Command: Modifies system state or performs an action. Returns void.
    public static void LogUserAccess(int id) 
    { 
        // Side effect: Logging
        Console.WriteLine($"Logging access for User {id}"); 
    }
    
    // Query: Returns data to the caller. Does NOT produce side effects.
    public static User GetUser(int id) 
    { 
        // Pure retrieval
        return new User { Id = id }; 
    }
}


// ─────────────────────────────────────────────────────────────
// PART 21 — LINQ: DEFERRED EXECUTION TRAP
// ─────────────────────────────────────────────────────────────
static class Part21_DeferredExecution
{
    // static void Main()
    // {
    //     Demo();
    // }
    public static void Demo()
    {
        // TASK 21-A: Classic deferred execution trap — exam favourite!
        var nums = new List<int> { 1, 2, 3 };

        var query = nums.Where(n => n > 1);

        nums.Add(4);
        
        foreach (var n in query) Console.Write(n + " ");

        // TASK 21-B: Fix with ToList() to get a snapshot
        var snapshot = nums.Where(n => n > 1).ToList();
        nums.Add(99);
        Console.WriteLine(string.Join(" ", snapshot)); // 2 3 4 (99 excluded)

        // EXAM: Deferred operators: Where, Select, OrderBy, ThenBy, GroupBy
        //       Terminal operators (trigger execution): ToList, Count, First, Any, All, Average
        //       "Multiple enumeration" = re-executing the query N times → in EF Core that's N SQL queries!
    }
}

// ─────────────────────────────────────────────────────────────
// PART 21 — LINQ: DEFERRED EXECUTION TRAP
// ─────────────────────────────────────────────────────────────
static class Part211_DeferredExecution
{
    public static void Demo()
    {
        // TASK 21-A: Classic deferred execution trap — exam favourite!
        var nums = new List<int> { 1, 2, 3 };

        var query1 = nums.Where(n => n > 1);

        nums.Add(4);
        
        foreach(var n in query1) Console.WriteLine("here" + n);
        Console.WriteLine();

        // TASK 21-B: Fix with ToList() to get a snapshot
        var snapshot = nums.Where(n => n > 1).ToList();
        nums.Add(99);
        Console.WriteLine(string.Join(" ", snapshot)); // 2 3 4 (99 excluded)

        // EXAM: Deferred operators: Where, Select, OrderBy, ThenBy, GroupBy
        //       Terminal operators (trigger execution): ToList, Count, First, Any, All, Average
        //       "Multiple enumeration" = re-executing the query N times → in EF Core that's N SQL queries!
        
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        
        var query = numbers.Where(n => {if(n%2 == 0) Console.WriteLine($"Even: {n}"); return n%2==0; });
        foreach(var n in query) Console.WriteLine(n);
    }
}


// ─────────────────────────────────────────────────────────────
// PART 22 — LINQ BASICS: Where, Select, OrderBy, ThenBy
// ─────────────────────────────────────────────────────────────
static class Part22_LinqBasics
{
    // Shared sample data
    record Student(int Id, string FirstName, string LastName, string City, string Email);

    static List<Student> Students() => new()
    {
        new(1, "Anna",  "Kowalska", "Warsaw", "anna@uni.pl"),
        new(2, "Marek", "Nowak",    "Krakow", "marek@uni.pl"),
        new(3, "Julia", "Wiśniewska","Warsaw","julia@uni.pl"),
        new(4, "Tomasz","Lis",      "Warsaw", "tomasz@uni.pl"),
        new(5, "Lena",  "Adamska",  "Gdansk", "lena@uni.pl"),
    };
    

    public static void Demo()
    {
        var students = Students();

        // TASK 22-A: Filter students from Warsaw, sort by LastName then FirstName,
        //   project to "LastName, FirstName" string
        var warsawStudents = students
            .Where(n => n.City == "Warsaw")
            .OrderBy(n => n.LastName)
            .ThenBy(n => n.FirstName)     // ← never use a second OrderBy()!
            .Select(n=> $"{n.LastName}, {n.FirstName}");
        warsawStudents.ToList().ForEach(Console.WriteLine);

        // TASK 22-B: Project all students to just their Email addresses
        var emails = students.Select(s => s.Email);
        emails.ToList().ForEach(Console.WriteLine);

        // EXAM PITFALL: a second .OrderBy() REPLACES the first!
        //   Wrong: students.OrderBy(s => s.City).OrderBy(s => s.LastName)
        //          → sorts by LastName ONLY (City sort is thrown away!)
        //   Correct: .OrderBy(s => s.City).ThenBy(s => s.LastName)
    }
}


// ─────────────────────────────────────────────────────────────
// PART 23 — LINQ QUANTIFIERS: Any, All, Count, First, Single
// ─────────────────────────────────────────────────────────────
static class Part23_LinqQuantifiers
{
    public static void Demo()
    {
        var numbers = new List<int> { 3, 7, 2, 9, 1 };

        // TASK 23-A: Check if ANY number is greater than 8
        bool anyBig = numbers.Any(n => n > 8);
        if (anyBig) Console.WriteLine("OK, its true");

        // TASK 23-B: Check if ALL numbers are positive
        bool allPositive = numbers.All(n => n > 0);
        Console.WriteLine(allPositive); // True

        // EXAM TRAP: All() on EMPTY collection returns TRUE (vacuously true)!
        Console.WriteLine(new List<int>().All(n => n == 5)); // True — exam classic!

        // TASK 23-C: Count numbers greater than 5
        int count = numbers.Count(n => n > 5);
        Console.WriteLine(count);

        // TASK 23-D: Get first number > 5 safely (use FirstOrDefault)
        int first = numbers.FirstOrDefault(n => n > 10);
        Console.WriteLine(first);
        try 
        {
            // This WILL throw an exception because the condition is impossible
            int first1 = numbers.First(n => n > 10);
            Console.WriteLine(first1);
        }
        catch (InvalidOperationException ex) 
        {
            // This code runs ONLY if the error happens
            Console.WriteLine("Error: The collection contains no matching element.");
        }
        // — if nothing matches, FirstOrDefault returns 0 (default for int), NOT an exception
        // — First() would throw InvalidOperationException if nothing matches
        

        // TASK 23-E: Use Single() — throws if 0 or 2+ matches
        int exactlyOne = numbers.Single(n => n == 9); // ok — exactly one 9
        // int boom = numbers.Single(n => n > 5); // throws — there are multiple

        // EXAM: Any() stops at the first match → prefer over Count() > 0 for existence checks
    }
}


// ─────────────────────────────────────────────────────────────
// PART 24 — LINQ JOINS, GroupBy, GroupJoin, SelectMany
// ─────────────────────────────────────────────────────────────
static class Part24_LinqJoins
{
    record Course(int Id, string Title, string Category, int LecturerId);
    record Enrollment(int StudentId, int CourseId, double? FinalGrade, bool IsActive);
    record Lecturer(int Id, string FirstName, string LastName);

    static List<Course>     Courses()     => new() {
        new(1, "C# Basics",    ".NET",  1),
        new(2, "LINQ Lab",     ".NET",  1),
        new(3, "Azure Cloud",  "Cloud", 2),
    };
    static List<Enrollment> Enrollments() => new() {
        new(1, 1, 4.5,  true),
        new(1, 2, null, true),
        new(2, 3, 5.0,  true),
        new(3, 1, 3.5,  false),
    };
    static List<Lecturer>   Lecturers()   => new() {
        new(1, "Jan",   "Kowalski"),
        new(2, "Maria", "Nowak"),
        new(3, "Adam",  "Wolski"), // no courses assigned
    };

    public static void Demo()
    {
        // TASK 24-A: Two-way Join — enrollments + courses → "StudentId | CourseTitle"
        Enrollments()
            .Join(Courses(), e => e.CourseId, c => c.Id,
                  (e, c) => $"Student {e.StudentId} | {c.Title}")
            .ToList().ForEach(Console.WriteLine);

        // TASK 24-B: GroupBy — count enrollments per course
        var grouped = Enrollments()
            .GroupBy(e => e.CourseId)
            .Select(g => new { CourseId = g.Key, Count = g.Count() });
        grouped.ToList().ForEach(Console.WriteLine);

        // TASK 24-C: GroupJoin (LEFT OUTER JOIN) — all lecturers + course count (incl. 0)
        var lecturerCourses = Lecturers()
            .GroupJoin(Courses(), l => l.Id, c => c.LecturerId,
                       (l, courses) => $"{l.FirstName}: {courses.Count()} courses");
        lecturerCourses.ToList().ForEach(Console.WriteLine);

        // TASK 24-D: Average grade per course — ONLY graded enrollments
        //   EXAM: always filter .Where(e => e.FinalGrade.HasValue) BEFORE Average()!
        var avgGrades = Enrollments()
            .Where(e => e.FinalGrade.HasValue)
            .GroupBy(e => e.CourseId)
            .Select(g => new { CourseId = g.Key, Avg = g.Average(e => e.FinalGrade!.Value) });
        avgGrades.ToList().ForEach(Console.WriteLine);

        // TASK 24-E: SelectMany — flatten all course titles from all lecturers
        //   (= Java's flatMap)
        var allTitles = Lecturers()
            .SelectMany(l => Courses().Where(c => c.LecturerId == l.Id).Select(c => c.Title));
    }
}


// ─────────────────────────────────────────────────────────────
// PART 25 — LINQ PAGING, DISTINCT, AGGREGATE
// ─────────────────────────────────────────────────────────────
static class Part25_LinqPaging
{
    public static void Demo()
    {
        var numbers = Enumerable.Range(1, 20).ToList();

        // TASK 25-A: Implement paging — page 3, page size 4
        //   Formula: Skip((page-1) * pageSize).Take(pageSize)
        //   ALWAYS sort before paging!
        int page = 3, pageSize = 4;
        var paged = numbers
            .OrderBy(n => n)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        Console.WriteLine(string.Join(", ", paged)); // 9, 10, 11, 12

        // TASK 25-B: Distinct cities from mixed list
        var cities = new[] { "Warsaw", "Krakow", "Warsaw", "Gdansk", "Krakow" };
        var uniqueCities = cities.Distinct().OrderBy(c => c);
        Console.WriteLine(string.Join(", ", uniqueCities));

        // TASK 25-C: Aggregate — build a running sum (like a manual Sum)
        int total = numbers.Aggregate(0, (acc, n) => acc + n);
        Console.WriteLine(total); // 210

        // TASK 25-D: ToHashSet() for O(1) lookup — pattern from Challenge 03
        var dotnetCourseIds = new[] { 1, 2 };
        var idSet = dotnetCourseIds.ToHashSet(); // O(1) Contains vs O(n) for list!
        Console.WriteLine(idSet.Contains(2)); // True
    }
}


// ─────────────────────────────────────────────────────────────
// PART 26 — ref / out / named / optional PARAMETERS
// ─────────────────────────────────────────────────────────────
static class Part26_RefOut
{
    // TASK 26-A: ref — caller MUST initialise. Method modifies in place.
    static void AddTen(ref int x) => x += 10;

    // TASK 26-B: out — caller does NOT need to initialise. Method MUST assign.
    static bool TryParseAge(string input, out int age) =>
        int.TryParse(input, out age) && age > 0;

    // TASK 26-C: Named parameters — call in any order
    static void CreateOrder(string customer, int quantity, decimal price) =>
        Console.WriteLine($"{customer}: {quantity} x {price:C}");

    // TASK 26-D: Optional (default) parameters — must come AFTER required ones
    static void Greet(string name, string greeting = "Hello", bool formal = false) =>
        Console.WriteLine($"{(formal ? greeting + "," : greeting)} {name}!");

    public static void Demo()
    {
        int n = 5;
        AddTen(ref n);
        Console.WriteLine(n); // 15

        if (TryParseAge("25", out int age)) Console.WriteLine($"Age: {age}");
        if (TryParseAge("abc", out int bad)) Console.WriteLine(bad);
        else Console.WriteLine("Parse failed");

        CreateOrder(price: 9.99m, customer: "Anna", quantity: 3);

        Greet("Anna");
        Greet("Jan", "Good morning", true);

        // EXAM: ref, out, default params, named params are UNIQUE TO C#.
        //       Java does NOT have any of these. Java always passes by value.
    }
}


// ─────────────────────────────────────────────────────────────
// PART 27 — GENERICS (reification vs type erasure)
// ─────────────────────────────────────────────────────────────
static class Part27_Generics
{
    // TASK 27-A: Write a generic Repository<T> class with constraint: T must be a class
    //   Methods: Add(T item), FindFirst(Func<T,bool>), GetAll()
    //
    public class Repository<T> where T : class
    {
        private readonly List<T> _items = new();
        public void Add(T item) => _items.Add(item);
        public T? FindFirst(Func<T, bool> pred) => _items.FirstOrDefault(pred);
        public IEnumerable<T> GetAll() => _items.AsReadOnly();
    }

    // TASK 27-B: Write a generic static method FindFirst<T> that takes a source and predicate
    public static T? FindFirst<T>(IEnumerable<T> source, Func<T, bool> pred)
        => source.FirstOrDefault(pred);

    public static void Demo()
    {
        // EXAM: C# generics use REIFICATION — type info is PRESERVED at runtime.
        //       typeof(T) works. List<int> stores ints efficiently (no boxing).
        //       Java uses TYPE ERASURE — generic type info is LOST after compilation.
        //       Java List<Integer> boxes every int to Object (inefficient).

        Console.WriteLine(new List<string>().GetType().Name); // List`1[String]
    }
}


// ─────────────────────────────────────────────────────────────
// PART 28 — yield return (LAZY SEQUENCES)
// ─────────────────────────────────────────────────────────────
static class Part28_YieldReturn
{
    // TASK 28-A: Write a method returning IEnumerable<int> using yield return.
    //   Generate even numbers from 0 to max lazily.
    //
    public static IEnumerable<int> EvenNumbers(int max)
    {
        for (int i = 0; i <= max; i += 2)
            yield return i; // pauses here, resumes when caller asks for next
    }

    // TASK 28-B: Write a method that uses yield break to stop early.
    //   Return active students, but stop completely if you find an expelled one.
    //
    public static IEnumerable<string> GetActiveStudentNames(IEnumerable<(string Name, bool IsActive, bool IsExpelled)> students)
    {
        foreach (var s in students)
        {
            if (s.IsExpelled) yield break;  // stop iteration entirely
            if (s.IsActive)   yield return s.Name;
        }
    }

    // TASK 28-C: Read file lines lazily (real-world use case)
    //   Only ONE line is in memory at a time — no ReadAllLines() spike.
    public static IEnumerable<string> ReadLines(string path)
    {
        using var reader = new System.IO.StreamReader(path);
        string? line;
        while ((line = reader.ReadLine()) != null)
            yield return line;
    }

    public static void Demo()
    {
        foreach (var n in EvenNumbers(10)) Console.Write(n + " "); // 0 2 4 6 8 10

        // EXAM: yield return turns a method into a state machine (compiler generates it).
        //       Elements are produced ON DEMAND — this is the foundation of LINQ's deferred execution.
    }
}


// ─────────────────────────────────────────────────────────────
// PART 29 — STRINGS & StringBuilder
// ─────────────────────────────────────────────────────────────
static class Part29_Strings
{
    public static void Demo()
    {
        // TASK 29-A: Practice common string methods
        string s = "  Hello, World!  ";
        Console.WriteLine(s.Trim());              // "Hello, World!"
        Console.WriteLine(s.ToUpper());
        Console.WriteLine(s.Replace("World", "C#"));
        Console.WriteLine(s.Split(',')[0].Trim()); // "Hello"
        Console.WriteLine(s.IndexOf("World"));
        Console.WriteLine(s.Substring(9, 5));     // "World"
        Console.WriteLine(string.IsNullOrWhiteSpace("  ")); // True
        Console.WriteLine(string.IsNullOrWhiteSpace(null));  // True

        // TASK 29-B: StringBuilder for loop concatenation — O(n) not O(n²)
        var sb = new StringBuilder();
        for (int i = 1; i <= 1000; i++) sb.Append(i).Append(", ");
        string result = sb.ToString();
        Console.WriteLine(result.Length);

        // TASK 29-C: String interpolation and verbatim strings
        string name = "Anna";
        int age = 22;
        string msg  = $"Hello {name}, you are {age} years old.";
        string path = @"C:\Users\Anna\Documents"; // verbatim — no escape needed

        // TASK 29-D: == on strings compares BY VALUE in C# (unlike Java!)
        string a = "hello";
        string b = "hel" + "lo";
        Console.WriteLine(a == b); // True in C# — value comparison!
        // In Java: a == b might be false (reference comparison); use .equals()
    }
}


// ─────────────────────────────────────────────────────────────
// PART 30 — TUPLES & DECONSTRUCTION
// ─────────────────────────────────────────────────────────────
static class Part30_Tuples
{
    // TASK 30-A: Method returning a named tuple (no class needed)
    static (string Name, int Age) GetPersonInfo(int id) => ("Anna", 22);

    // TASK 30-B: Return a named tuple from an interface method (pattern from codebase)
    interface IDiscountRule
    {
        (decimal amount, string note) Calculate(decimal baseAmount);
    }

    class PercentageDiscount : IDiscountRule
    {
        private readonly decimal _rate;
        private readonly string _label;
        public PercentageDiscount(decimal rate, string label) { _rate = rate; _label = label; }

        public (decimal amount, string note) Calculate(decimal baseAmount) =>
            (baseAmount * _rate, _label);
    }

    public static void Demo()
    {
        // TASK 30-C: Use the named tuple return value
        var info = GetPersonInfo(1);
        Console.WriteLine(info.Name); // Anna
        Console.WriteLine(info.Age);  // 22

        // TASK 30-D: Deconstruct into separate variables
        var (name, age) = GetPersonInfo(1);
        Console.WriteLine($"{name} is {age}");

        // TASK 30-E: Use the discount rule tuple
        IDiscountRule rule = new PercentageDiscount(0.10m, "10% member discount");
        var (amount, note) = rule.Calculate(1000m);
        Console.WriteLine($"Discount: {amount:C} ({note})"); // $100 (10% member discount)
    }
}


// ─────────────────────────────────────────────────────────────
// PART 31 — ADAPTER PATTERN
// ─────────────────────────────────────────────────────────────
static class Part31_Adapter
{
    // Legacy system — you cannot modify this
    static class LegacyEmailGateway
    {
        public static void Send(string to, string subject, string body) =>
            Console.WriteLine($"[LEGACY EMAIL] To: {to} | {subject}");
    }

    // TASK 31-A: Define the injectable interface
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }

    // TASK 31-B: Create the Adapter
    // It maps the interface call to the static legacy method
    public class LegacyEmailAdapter : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            // The adapter "adapts" our interface to the legacy signature
            LegacyEmailGateway.Send(to, subject, body);
        }
    }

    // TASK 31-C: Create the consumer that depends on the abstraction
    public class NotificationService
    {
        private readonly IEmailService _emailService;

        // Constructor Injection: we don't care IF it's legacy or not
        public NotificationService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void Notify(string email, string title, string message)
        {
            _emailService.SendEmail(email, title, message);
        }
    }

    public static void Demo()
    {
        // Wire it up:
        // In a real app, this would happen in your DI Container (e.g., Program.cs)
        IEmailService emailService = new LegacyEmailAdapter();
        var notifier = new NotificationService(emailService);
        
        notifier.Notify("anna@uni.pl", "Welcome", "Hello!");
    }
}


// ─────────────────────────────────────────────────────────────
// PART 32 — FACTORY METHOD & ABSTRACT FACTORY (Completed)
// ─────────────────────────────────────────────────────────────
static class Part32_Factory
{
    // --- TASK 32-A: Factory Method (Provided) ---
    interface ISender { void Send(string message); }
    class EmailSender : ISender { public void Send(string m) => Console.WriteLine("Email: " + m); }
    class SmsSender   : ISender { public void Send(string m) => Console.WriteLine("SMS: "   + m); }

    abstract class NotificationFactory
    {
        public abstract ISender CreateSender(); 
        public void Notify(string msg) => CreateSender().Send(msg); 
    }
    class EmailSenderFactory : NotificationFactory { public override ISender CreateSender() => new EmailSender(); }
    class SmsSenderFactory   : NotificationFactory { public override ISender CreateSender() => new SmsSender(); }

    // --- TASK 32-B: Abstract Factory ---
    
    // Abstract Products
    interface IButton { void Render(); }
    interface ICheckbox { void Render(); }

    // Concrete Products: Windows Family
    class WindowsButton : IButton { public void Render() => Console.WriteLine("Rendering Windows Button"); }
    class WindowsCheckbox : ICheckbox { public void Render() => Console.WriteLine("Rendering Windows Checkbox"); }

    // Concrete Products: Mac Family
    class MacButton : IButton { public void Render() => Console.WriteLine("Rendering Mac Button"); }
    class MacCheckbox : ICheckbox { public void Render() => Console.WriteLine("Rendering Mac Checkbox"); }

    // The Abstract Factory Interface
    interface IUIFactory
    {
        IButton CreateButton();
        ICheckbox CreateCheckbox();
    }

    // Concrete Factories
    class WindowsUIFactory : IUIFactory
    {
        public IButton CreateButton() => new WindowsButton();
        public ICheckbox CreateCheckbox() => new WindowsCheckbox();
    }

    class MacUIFactory : IUIFactory
    {
        public IButton CreateButton() => new MacButton();
        public ICheckbox CreateCheckbox() => new MacCheckbox();
    }

    public static void Demo()
    {
        // 1. Factory Method Usage
        NotificationFactory myFactory = new EmailSenderFactory();
        myFactory.Notify("Factory Method works!");

        // 2. Abstract Factory Usage
        // In a real app, this logic would run once at startup (e.g., if OS is Win, use WindowsUIFactory)
        IUIFactory uiFactory = new MacUIFactory(); 
        
        var btn = uiFactory.CreateButton();
        var chk = uiFactory.CreateCheckbox();

        btn.Render(); // "Rendering Mac Button"
        chk.Render(); // "Rendering Mac Checkbox"
    }
}


// ─────────────────────────────────────────────────────────────
// PART 33 — SINGLETON PATTERN
// ─────────────────────────────────────────────────────────────
static class Part33_Singleton
{
    // TASK 33-A: Classic Singleton with Lazy<T> for thread safety
    //
    public class AppConfig
    {
        private static readonly Lazy<AppConfig> _instance =
            new(() => new AppConfig()); // Lazy = thread-safe lazy init
    //
    private AppConfig() { /* load config */ }
    
    public static AppConfig Instance => _instance.Value;
    public string Environment { get; } = "Production";
    }

    public static void Demo()
    {
        var a = AppConfig.Instance;
        var b = AppConfig.Instance;
        Console.WriteLine(ReferenceEquals(a, b)); // True

        // EXAM: Classic Singleton problems:
        //   1. Hinders unit testing (hidden global state, hard to mock)
        //   2. Violates DIP — classes depend on the concrete static instance
        //   3. Thread safety must be implemented manually
        //
        // MODERN C# PREFERRED APPROACH:
        //   builder.Services.AddSingleton<IAppConfig, AppConfig>();
        //   — handled by DI container, testable, DIP-compliant
    }
}


// ─────────────────────────────────────────────────────────────
// PART 34 — EXAM SIMULATION: GIT SCENARIO (Completed)
// ─────────────────────────────────────────────────────────────
static class Part34_ExamSimGit
{
    /*
     * Step 1: git checkout main               (Switch to local main)
     * Step 2: git pull origin main            (Get the new commit abc123)
     * Step 3: git checkout feature/reports    (Return to your work)
     * Step 4: git rebase main                 (Re-apply your work on top of abc123)
     * Step 5: [Manual Fix in StudentReport.cs]
     * Step 6: git add StudentReport.cs        (Stage the resolution)
     * Step 7: git rebase --continue           (Finish the rebase process)
     * Step 8: git checkout main               (Return to main to bring changes in)
     * Step 9: git merge feature/reports --ff-only (Fast-forward main to your new head)
     * Step 10: git push origin main           (Push linear history to server)
     * Step 11: git branch -d feature/reports  (Delete local branch)
     * Step 12: git push origin --delete feature/reports (Delete remote branch)
     * * BONUS: --ff-only guarantees that the branch can be merged without creating a merge commit. 
     * It is safer because it prevents "diamond" histories and forces you to rebase if main has moved.
     */
}

// ─────────────────────────────────────────────────────────────
// PART 35 — EXAM SIMULATION: OOP + DIP (Completed)
// ─────────────────────────────────────────────────────────────
static class Part35_ExamSimOOP
{
    public abstract record Notification(string Subject, string Body)
    {
        public DateTime CreatedAt { get; init; } = DateTime.Now;
    }

    public record EmailNotification(string To, string Cc, string Subject, string Body) : Notification(Subject, Body);
    public record SmsNotification(string PhoneNumber, string Subject, string Body) : Notification(Subject, Body);
    public record PushNotification(string DeviceToken, string AppName, string Subject, string Body) : Notification(Subject, Body);

    public interface ISender { Task SendAsync(Notification n); }

    public class EmailSender : ISender
    {
        public Task SendAsync(Notification n)
        {
            if (n is EmailNotification e) Console.WriteLine($"[EMAIL] To: {e.To}, Cc: {e.Cc} | {e.Subject}");
            return Task.CompletedTask;
        }
    }

    public class SmsSender : ISender
    {
        public Task SendAsync(Notification n)
        {
            if (n is SmsNotification s) Console.WriteLine($"[SMS] To: {s.PhoneNumber} | {s.Subject}");
            return Task.CompletedTask;
        }
    }

    public class PushSender : ISender
    {
        public Task SendAsync(Notification n)
        {
            if (n is PushNotification p) Console.WriteLine($"[PUSH] App: {p.AppName} | Token: {p.DeviceToken}");
            return Task.CompletedTask;
        }
    }

    public class NotificationDispatcher
    {
        private readonly List<ISender> _senders;
        public NotificationDispatcher(List<ISender> senders) => _senders = senders;

        public async Task DispatchAsync(Notification n) => await Task.WhenAll(_senders.Select(s => s.SendAsync(n)));
    }

    public static async Task Demo()
    {
        var dispatcher = new NotificationDispatcher(new List<ISender>
        {
            new EmailSender(),
            new SmsSender(),
            new PushSender()
        });

        await dispatcher.DispatchAsync(new EmailNotification("anna@uni.pl", "boss@uni.pl", "Welcome", "Hello!"));
        await dispatcher.DispatchAsync(new SmsNotification("+48123456789", "Reminder", "Class at 10am"));
        await dispatcher.DispatchAsync(new PushNotification("device-abc", "UniversityApp", "New grade", "You got 5.0!"));
    }
}

// ─────────────────────────────────────────────────────────────
// PART 36 — EXAM SIMULATION: LINQ ANALYTICS (Completed)
// ─────────────────────────────────────────────────────────────
static class Part36_ExamSimLinq
{
    record Student(int Id, string FirstName, string LastName, string City);
    record Course(int Id, string Title, string Category, int LecturerId, int Ects, DateTime StartDate);
    record Enrollment(int StudentId, int CourseId, double? FinalGrade, bool IsActive);
    record Lecturer(int Id, string FirstName, string LastName, string Department);

    static List<Student> Students() => new() {
        new(1, "Anna",   "Kowalska",   "Warsaw"),
        new(2, "Marek",  "Nowak",      "Krakow"),
        new(3, "Julia",  "Wiśniewska", "Warsaw"),
        new(4, "Tomasz", "Lis",        "Warsaw"),
        new(5, "Lena",   "Adamska",    "Gdansk"),
    };
    static List<Course> Courses() => new() {
        new(1, "C# Basics",       ".NET",  1, 5, new DateTime(2026, 3, 1)),
        new(2, "LINQ Lab",        ".NET",  1, 3, new DateTime(2026, 4, 1)),
        new(3, "Azure Cloud",     "Cloud", 2, 4, new DateTime(2026, 4, 5)),
        new(4, "SQL Foundations", "Data",  2, 4, new DateTime(2026, 2, 1)),
        new(5, "AI Intro",        "Cloud", 3, 5, new DateTime(2026, 4, 10)),
    };
    static List<Enrollment> Enrollments() => new() {
        new(1, 1, 4.5,  true),
        new(1, 2, null, true),
        new(1, 3, null, true),
        new(2, 1, 5.0,  true),
        new(2, 3, 4.0,  true),
        new(3, 2, 3.5,  false),
        new(4, 4, 4.0,  true),
        new(5, 5, null, true),
    };
    static List<Lecturer> Lecturers() => new() {
        new(1, "Jan",   "Kowalski", "IT"),
        new(2, "Maria", "Nowak",    "IT"),
        new(3, "Adam",  "Wolski",   ""),
    };

    public static void Demo()
    {
        var students    = Students();
        var courses     = Courses();
        var enrollments = Enrollments();
        var lecturers   = Lecturers();

        Console.WriteLine("=== QUERY 1 ===");
        courses.OrderByDescending(c => c.Ects)
               .Select(c => $"{c.Title} | {c.Ects} ECTS")
               .ToList().ForEach(Console.WriteLine);

        Console.WriteLine("\n=== QUERY 2 ===");
        students.GroupBy(s => s.City)
            .Select(g => {
                var sIds = g.Select(s => s.Id).ToList();
                var avg = enrollments.Where(e => sIds.Contains(e.StudentId) && e.FinalGrade.HasValue)
                                     .Select(e => e.FinalGrade.Value).DefaultIfEmpty(0).Average();
                return $"{g.Key}: {g.Count()} students, Avg Grade: {(avg > 0 ? avg.ToString("F2") : "No grades")}";
            }).ToList().ForEach(Console.WriteLine);

        Console.WriteLine("\n=== QUERY 3 ===");
        courses.Where(c => c.StartDate.Year == 2026 && c.StartDate.Month == 4)
               .Where(c => !enrollments.Any(e => e.CourseId == c.Id && e.FinalGrade.HasValue))
               .ToList().ForEach(c => Console.WriteLine(c.Title));

        Console.WriteLine("\n=== QUERY 4 ===");
        enrollments.Where(e => e.IsActive)
                   .GroupBy(e => e.CourseId)
                   .OrderByDescending(g => g.Count())
                   .Take(2)
                   .Join(courses, g => g.Key, c => c.Id, (g, c) => $"{c.Title}: {g.Count()} active")
                   .ToList().ForEach(Console.WriteLine);

        Console.WriteLine("\n=== QUERY 5 ===");
        var dotNetIds = courses.Where(c => c.Category == ".NET").Select(c => c.Id).ToHashSet();
        var cloudIds = courses.Where(c => c.Category == "Cloud").Select(c => c.Id).ToHashSet();
        students.Where(s => enrollments.Any(e => e.StudentId == s.Id && dotNetIds.Contains(e.CourseId)) &&
                            enrollments.Any(e => e.StudentId == s.Id && cloudIds.Contains(e.CourseId)))
                .ToList().ForEach(s => Console.WriteLine(s.FirstName));

        Console.WriteLine("\n=== QUERY 6 ===");
        lecturers.Where(l => {
            var lCourses = courses.Where(c => c.LecturerId == l.Id).Select(c => c.Id).ToHashSet();
            var lEnrolls = enrollments.Where(e => lCourses.Contains(e.CourseId));
            return lEnrolls.Any() && lEnrolls.All(e => e.FinalGrade.HasValue);
        }).ToList().ForEach(l => Console.WriteLine(l.FirstName));

        Console.WriteLine("\n=== QUERY 7 ===");
        Console.WriteLine("All have dept: " + lecturers.All(l => !string.IsNullOrEmpty(l.Department)));

        Console.WriteLine("\n=== QUERY 8 ===");
        courses.OrderBy(c => c.Title).Skip(2).Take(2)
               .ToList().ForEach(c => Console.WriteLine(c.Title));
    }
}


// ═══════════════════════════════════════════════════════════════
//  ★  LECTURES 5, 6 & 7 — NEW SECTIONS
//     Topics NOT covered in Parts 1-36 above:
//
//   Part 37 — HTTP & REST fundamentals
//   Part 38 — ASP.NET Core: Controllers, Routing, Status Codes
//   Part 39 — Model Binding & Data Annotations Validation
//   Part 40 — Minimal APIs
//   Part 41 — Middleware pipeline (Use / Run / Map / custom class)
//   Part 42 — Global Exception Handling Middleware
//   Part 43 — Testing: xUnit vs NUnit, AAA, [Theory]/[TestCase]
//   Part 44 — Mocking with Moq
//   Part 45 — Three-Layer Architecture (Controller → Service → Repository)
//   Part 46 — ADO.NET: SqlConnection, connection string, pooling
//   Part 47 — ADO.NET: SqlCommand (ExecuteNonQuery / Scalar / Reader)
//   Part 48 — ADO.NET: SQL Injection & parameterized queries
//   Part 49 — ADO.NET: SqlDataReader & NULL handling
//   Part 50 — ADO.NET: Transactions (ACID, Commit, Rollback, isolation)
//   Part 51 — Exam Simulation: full 3-layer endpoint with transaction
// ═══════════════════════════════════════════════════════════════
 
 
// ─────────────────────────────────────────────────────────────
// PART 37 — HTTP & REST FUNDAMENTALS (Completed)
// ─────────────────────────────────────────────────────────────
static class Part37_HttpRest
{
    // TASK 37-A: HTTP METHODS
    //
    //   Method   | Typical use                    | Idempotent?
    //   ---------|--------------------------------|------------
    //   GET      | Fetch/Retrieve data            | YES
    //   POST     | Create a new resource          | NO
    //   PUT      | Replace/Update entire resource | YES
    //   PATCH    | Partial update of resource     | NO (usually)
    //   DELETE   | Remove a resource              | YES
    
    // TASK 37-B: STATUS CODE RANGES
    //
    //   200 OK           → Success: Request completed and result returned.
    //   201 Created      → Success: Resource created (usually returns Location header).
    //   204 No Content   → Success: Request handled, but nothing to send back (common for DELETE).
    //   400 Bad Request  → Client Error: The server cannot process the request due to bad syntax/validation.
    //   401 Unauthorized → EXAM: It means "unauthenticated" (Server doesn't know who you are).
    //   403 Forbidden    → EXAM: The user IS authenticated but lacks required permissions/roles.
    //   404 Not Found    → Client Error: The requested resource does not exist.
    //   500 Internal Server Error → Server Error: Something crashed on the backend.

    // TASK 37-C: REST RESOURCE DESIGN
    //   Bad:  GET  /api/getOrderById/15
    //   Good: GET  /api/orders/15
    //
    //   Bad:  POST /api/deleteUser?id=5
    //   Good: DELETE /api/users/5
    //
    //   Bad:  GET  /api/createBook
    //   Good: POST /api/books

    // TASK 37-D: STATELESSNESS
    //   HTTP is stateless because the server does not store any client context between requests; 
    //   for session management in REST, this means every request must contain all necessary 
    //   information (like a JWT token) to identify and authorize the user.

    // TASK 37-E: IDEMPOTENCY
    //   PUT is idempotent because calling it multiple times with the same data results in the 
    //   same state, whereas POST is NOT idempotent as it creates a new record every time. 
    //   This matters for retries because if a request fails due to a network glitch, you can 
    //   safely retry a PUT without side effects, but retrying a POST might result in duplicate data.
}
 
// ─────────────────────────────────────────────────────────────
// PART 38 — ASP.NET CORE: CONTROLLERS, ROUTING, STATUS CODES (Completed)
// ─────────────────────────────────────────────────────────────

static class Part38_Controllers
{
    public record BookDto(int Id, string Title, string Author);
    public record CreateBookDto(string Title, string Author);

    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static readonly List<BookDto> _books = new()
        {
            new(1, "Clean Code", "Robert C. Martin"),
            new(2, "The Pragmatic Programmer", "Andrew Hunt"),
        };

        // TASK: GET api/books → return 200 + all books
        [HttpGet]
        public ActionResult<IEnumerable<BookDto>> GetAll()
        {
            return Ok(_books);
        }

        // TASK: GET api/books/{id} → 200 + book OR 404
        [HttpGet("{id:int}")]
        public ActionResult<BookDto> GetById(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            return book != null ? Ok(book) : NotFound();
        }

        // TASK: POST api/books → 201 Created with Location header
        [HttpPost]
        public ActionResult<BookDto> Create(CreateBookDto dto)
        {
            var newId = _books.Max(b => b.Id) + 1;
            var createdBook = new BookDto(newId, dto.Title, dto.Author);
            _books.Add(createdBook);

            // EXAM: This adds the 'Location' header pointing to GetById
            return CreatedAtAction(nameof(GetById), new { id = newId }, createdBook);
        }

        // TASK: PUT api/books/{id} → 200 updated OR 404
        [HttpPut("{id:int}")]
        public ActionResult<BookDto> Update(int id, CreateBookDto dto)
        {
            var index = _books.FindIndex(b => b.Id == id);
            if (index == -1) return NotFound();

            var updatedBook = new BookDto(id, dto.Title, dto.Author);
            _books[index] = updatedBook;
            return Ok(updatedBook);
        }

        // TASK: DELETE api/books/{id} → 204 No Content OR 404
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            _books.Remove(book);
            return NoContent(); // 204
        }
    }

    // TASK 38-B: ROUTING ATTRIBUTES
    /*
     * [ApiController] adds: 
     * 1. Automatic model state validation (returns 400 automatically if DTO is invalid).
     * 2. Attribute routing requirement (actions must have [HttpGet], etc.).
     * 3. Automatic binding (e.g., complex types are assumed to come [FromBody]).
     *
     * [Route("api/[controller]")] for BooksController:
     * Produces: "api/books" (it strips the "Controller" suffix automatically).
     *
     * Differences:
     * [FromRoute]: Data comes from URL segments (e.g., api/books/5 -> 5).
     * [FromQuery]: Data comes from after the '?' in URL (e.g., api/books?genre=it -> "it").
     * [FromBody]: Data comes from the HTTP Request Body (usually JSON).
     */

    // TASK 38-C: PATCH vs PUT
    /*
     * For a partial update (changing only one field): PATCH is semantically correct.
     * PUT is intended for a full replacement of the resource. If you use PUT and 
     * omit the 'author', a strict implementation might set the author to null or empty.
     */
}
 
 
using System.ComponentModel.DataAnnotations;

// ─────────────────────────────────────────────────────────────
// PART 39 — MODEL BINDING & DATA ANNOTATIONS VALIDATION (Completed)
// ─────────────────────────────────────────────────────────────
static class Part39_Validation
{
    // TASK 39-A: Production-ready DTO with Data Annotations
    public class CreateBookDto
    {
        [Required(ErrorMessage = "Title is mandatory")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(120)]
        public string Author { get; set; } = string.Empty;

        [Range(1, 5000, ErrorMessage = "Books must have between 1 and 5000 pages")]
        public int Pages { get; set; }

        [Range(0.01, 9999.99)]
        public decimal Price { get; set; }
    }

    // TASK 39-B: Manual Validation logic
    /*
     * Without the [ApiController] attribute, you would have to manually check the
     * validation state at the beginning of every action like this:
     * * if (!ModelState.IsValid)
     * {
     * return BadRequest(ModelState);
     * }
     */

    // TASK 39-C: MODEL BINDING SOURCES
    /*
     * [FromRoute]  → data comes from: URL segments (e.g., /api/books/{id})
     * [FromQuery]  → data comes from: URL query string (e.g., /api/books?author=Martin)
     * [FromBody]   → data comes from: Request Body (usually the JSON payload)
     * [FromHeader] → data comes from: HTTP Headers (e.g., X-API-Key)
     */
}
 
 
// ─────────────────────────────────────────────────────────────
// PART 40 — MINIMAL APIs (Completed)
// ─────────────────────────────────────────────────────────────

static class Part40_MinimalApis
{
    // TASK 40-A: Minimal API Implementation (Logic for Program.cs)
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();

    var books = new List<BookDto>
    {
        new(1, "Clean Code", "Robert C. Martin"),
        new(2, "Refactoring", "Martin Fowler")
    };

    // GET all
    app.MapGet("/api/books", () => Results.Ok(books));

    // GET by ID
    app.MapGet("/api/books/{id:int}", (int id) =>
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        return book is null ? Results.NotFound() : Results.Ok(book);
    });

    // POST create
    app.MapPost("/api/books", (CreateBookDto dto) =>
    {
        var book = new BookDto(books.Max(b => b.Id) + 1, dto.Title, dto.Author);
        books.Add(book);
        return Results.Created($"/api/books/{book.Id}", book);
    });

    // DELETE
    app.MapDelete("/api/books/{id:int}", (int id) =>
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        if (book is null) return Results.NotFound();
        books.Remove(book);
        return Results.NoContent();
    });

    app.Run();
    */

    // TASK 40-B: CONTROLLERS vs MINIMAL APIs
    //
    //   Criterion                          | Controllers      | Minimal APIs
    //   -----------------------------------|------------------|------------------
    //   Large API with many resources      | Better (Grouping)| Can get messy (Flat)
    //   Quick start / small project        | Slower           | Excellent (Fast)
    //   Attribute-based organization       | Yes ([Route])    | Limited (Fluent API)
    //   Boilerplate code                   | High (Files/Classes)| Very Low
}
 
 
// ─────────────────────────────────────────────────────────────
// PART 41 — MIDDLEWARE PIPELINE (Completed)
// ─────────────────────────────────────────────────────────────
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

static class Part41_Middleware
{
    // TASK 41-A: Middleware Methods
    /*
     * app.Use(async (context, next) => { ... }): 
     * Registers middleware that can process the request and pass it to the next component. 
     * If you forget 'await next()', the pipeline "short-circuits," and subsequent middleware 
     * (and controllers) are never executed.
     *
     * app.Run(async context => { ... }): 
     * Registers "terminal" middleware. It does not have a 'next' parameter, so it always 
     * ends the pipeline and returns a response.
     *
     * app.Map("/health", branch => { ... }): 
     * Branching the pipeline. If the request path starts with "/health", it enters this 
     * separate branch. The rest of the main pipeline is ignored for that request.
     */

    // TASK 41-B: Inline Timing Middleware
    /*
    app.Use(async (context, next) =>
    {
        var watch = Stopwatch.StartNew();
        
        await next(); // Pass to next middleware/controller
        
        watch.Stop();
        Console.WriteLine($"{context.Request.Method} {context.Request.Path} — {watch.Elapsed.TotalMilliseconds}ms");
    });
    */

    // TASK 41-C: Custom Middleware Class
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
            var watch = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                watch.Stop();
                _logger.LogInformation("Request {Path} took {Ms}ms", context.Request.Path, watch.Elapsed.TotalMilliseconds);
            }
        }
    }

    // TASK 41-D: PIPELINE ORDER
    /*
     * 1. app.UseExceptionHandler("/error") (Must be first to catch errors from everything below)
     * 2. app.UseHttpsRedirection()
     * 3. app.UseRouting()
     * 4. app.UseCors()
     * 5. app.UseAuthentication()
     * 6. app.UseAuthorization()
     * 7. app.MapControllers() (The actual logic/endpoint)
     *
     * EXAM: UseAuthentication MUST come before UseAuthorization because 
     * you need to know WHO the user is (Identity) before you can decide 
     * WHAT they are allowed to do (Permissions).
     */

    // TASK 41-E: SCOPED DEPENDENCIES
    /*
     * Middleware classes are singletons (created once at startup). If you inject a 
     * Scoped service (like a DbContext) into the constructor, that service is "captured" 
     * forever, which leads to memory leaks and multi-threading crashes.
     * * Solution: Inject Scoped services directly into the InvokeAsync method parameters. 
     * ASP.NET Core resolves those parameters from the RequestServices container for each request.
     */
}
 
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ─────────────────────────────────────────────────────────────
// PART 42 — GLOBAL EXCEPTION HANDLING MIDDLEWARE (Completed)
// ─────────────────────────────────────────────────────────────
static class Part42_GlobalExceptionHandling
{
    // TASK 42-A: ExceptionHandlingMiddleware
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
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
                _logger.LogError(ex, "An unhandled exception has occurred.");
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { message = "Unexpected server error" });
            }
        }
    }

    // TASK 42-B: Pipeline Position
    /*
     * It needs to be registered HIGH (early) in Program.cs because middleware forms a nested chain 
     * (the Russian Doll model). It can only catch exceptions thrown by middleware or controllers 
     * that are called INSIDE its `await _next(context)` block.
     */

    // TASK 42-C: Advantage over local try/catch
    /*
     * 1. DRY (Don't Repeat Yourself): Removes boilerplate try/catch blocks from every single controller.
     * 2. Consistency: Ensures all unhandled errors return the exact same JSON format to the client.
     */
}

// ─────────────────────────────────────────────────────────────
// PART 43 — TESTING: xUnit vs NUnit, AAA, [Theory]/[TestCase]
// ─────────────────────────────────────────────────────────────
static class Part43_Testing
{
    // TASK 43-A: Calculator
    public class Calculator
    {
        public int Add(int a, int b) => a + b;
        public int Divide(int a, int b)
        {
            if (b == 0) throw new DivideByZeroException("Cannot divide by zero.");
            return a / b;
        }
    }

    // TASK 43-B & 43-C: xUnit Tests
    public class CalculatorTests
    {
        [Fact]
        public void Add_TwoPositiveNumbers_ReturnsSum()
        {
            // Arrange
            var calculator = new Calculator();
            // Act
            var result = calculator.Add(2, 3);
            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void Divide_ByZero_ThrowsDivideByZeroException()
        {
            // Arrange
            var calculator = new Calculator();
            // Act & Assert
            Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
        }

        [Theory]
        [InlineData(2, 3, 5)]
        [InlineData(-2, -3, -5)]
        [InlineData(-2, 3, 1)]
        public void Add_VariousInputs_ReturnsExpectedSum(int a, int b, int expected)
        {
            var calculator = new Calculator();
            Assert.Equal(expected, calculator.Add(a, b));
        }
    }

    // TASK 43-D: NUnit Equivalent
    [TestFixture]
    public class CalculatorTestsNUnit
    {
        [TestCase(2, 3, 5)]
        [TestCase(-2, -3, -5)]
        public void Add_DifferentNumbers_ReturnsExpectedSum(int a, int b, int expected)
        {
            var calculator = new Calculator();
            NUnit.Framework.Assert.AreEqual(expected, calculator.Add(a, b));
        }
    }

    // TASK 43-E: xUnit vs NUnit comparison
    /*
     * Feature               | xUnit         | NUnit
     * ----------------------|---------------|-------------
     * Simple test           | [Fact]        | [Test]
     * Parameterized test    | [Theory]      | [TestCase]
     * Data for param test   | [InlineData]  | [TestCase(1,2)] (Combined with method attribute)
     * Test class marker     | (none needed) | [TestFixture]
     * Setup before each     | Constructor   | [SetUp]
     * Teardown after each   | IDisposable   | [TearDown]
     */

    // TASK 43-F: Test Independence
    /*
     * Tests must not depend on execution order because modern runners run them in parallel 
     * or random order. If Test A changes a static counter variable and Test B expects it 
     * to be 0, Test B will fail randomly depending on which one runs first.
     */
}

// ─────────────────────────────────────────────────────────────
// PART 44 — MOCKING WITH MOQ (Completed)
// ─────────────────────────────────────────────────────────────
static class Part44_Mocking
{
    public interface IStudentRepository
    {
        Task<Student?> GetByIdAsync(int id);
        Task<Student?> GetByEmailAsync(string email);
        Task AddAsync(Student s);
    }
    
    public record Student(int Id, string Email);

    public class StudentService
    {
        private readonly IStudentRepository _repo;
        public StudentService(IStudentRepository repo) => _repo = repo;
        
        public Task<Student?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        
        public async Task<Student> AddAsync(Student student)
        {
            var existing = await _repo.GetByEmailAsync(student.Email);
            if (existing is not null) throw new InvalidOperationException("Email already exists");
            await _repo.AddAsync(student);
            return student;
        }
    }

    public class StudentServiceTests
    {
        // TASK 44-A: Mocking GetById
        [Fact]
        public async Task GetById_ExistingStudent_ReturnsStudent()
        {
            // Arrange
            var mockRepo = new Mock<IStudentRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Student(1, "anna@uni.pl"));
            var service = new StudentService(mockRepo.Object);
            
            // Act
            var result = await service.GetByIdAsync(1);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("anna@uni.pl", result!.Email);
        }

        // TASK 44-B: Mocking Exception Path
        [Fact]
        public async Task AddAsync_DuplicateEmail_ThrowsInvalidOperationException()
        {
            // Arrange
            var mockRepo = new Mock<IStudentRepository>();
            mockRepo.Setup(r => r.GetByEmailAsync("anna@uni.pl")).ReturnsAsync(new Student(1, "anna@uni.pl"));
            var service = new StudentService(mockRepo.Object);
            
            var newStudent = new Student(2, "anna@uni.pl");

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddAsync(newStudent));
        }
    }

    // TASK 44-C: WHY MOCK?
    /*
     * Problem with real DB: Tests become slow, fragile (fail if network is down), and hard to run 
     * in parallel. You are testing the database, not your business logic.
     * * What a mock replaces: It replaces the external dependency (like a repo or API) with a 
     * simulated object. It guarantees deterministic behavior—you tell it exactly what to return.
     */
}

// ─────────────────────────────────────────────────────────────
// PART 45 — THREE-LAYER ARCHITECTURE (Completed)
// ─────────────────────────────────────────────────────────────
static class Part45_ThreeLayers
{
    // TASK 45-A: Call chain
    /*
     * HTTP Request → Controller (API Layer) → Service (Business Layer) → Repository (Data Layer) → Database
     */
    // 

    // TASK 45-B: Layer Responsibilities
    /*
     * "Parse HTTP request body and return 404 if not found"  → Controller Layer
     * "Throw exception if email already exists"              → Service Layer (Business Rule)
     * "Execute SELECT * FROM Students WHERE Id = @Id"        → Repository Layer
     * "Return HTTP status code 201 Created"                  → Controller Layer
     * "Map a database row to a C# object"                    → Repository Layer
     */

    // TASK 45-C: Full 3-Layer Implementation
    public record Student(int Id, string FirstName, string LastName, string Email);

    // --- REPOSITORY LAYER ---
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task<Student?> GetByEmailAsync(string email);
        Task AddAsync(Student student);
    }

    public class InMemoryStudentRepository : IStudentRepository
    {
        private static readonly List<Student> _students = new();

        public Task<IEnumerable<Student>> GetAllAsync() => Task.FromResult((IEnumerable<Student>)_students);
        
        public Task<Student?> GetByIdAsync(int id) => 
            Task.FromResult(_students.FirstOrDefault(s => s.Id == id));
            
        public Task<Student?> GetByEmailAsync(string email) => 
            Task.FromResult(_students.FirstOrDefault(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
            
        public Task AddAsync(Student student)
        {
            _students.Add(student);
            return Task.CompletedTask;
        }
    }

    // --- SERVICE LAYER ---
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task<Student> AddAsync(Student student);
    }

    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        
        public StudentService(IStudentRepository repository) => _repository = repository;
        
        public Task<IEnumerable<Student>> GetAllAsync() => _repository.GetAllAsync();
        
        public Task<Student?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        
        public async Task<Student> AddAsync(Student student)
        {
            var existing = await _repository.GetByEmailAsync(student.Email);
            if (existing != null)
            {
                throw new InvalidOperationException("Email already exists");
            }
            
            await _repository.AddAsync(student);
            return student;
        }
    }

    // --- CONTROLLER LAYER ---
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;
        
        public StudentsController(IStudentService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Student>> GetById(int id)
        {
            var student = await _service.GetByIdAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult<Student>> Add(Student student)
        {
            try 
            {
                var created = await _service.AddAsync(student);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // TASK 45-D: DI REGISTRATION (Code for Program.cs)
    /*
     * builder.Services.AddScoped<IStudentRepository, InMemoryStudentRepository>();
     * builder.Services.AddScoped<IStudentService, StudentService>();
     */

    // TASK 45-E: DI LIFETIMES
    /*
     * AddTransient  → new instance every: Time it is injected/requested.
     * AddScoped     → one instance per:   HTTP Request.  ← typical choice for repos/services
     * AddSingleton  → one instance for:   The entire application lifetime.
     *
     * EXAM: Captive Dependency
     * You should NEVER inject a Scoped service into a Singleton because the Singleton is created 
     * only once. It will "capture" the Scoped service, keeping it alive for the life of the app. 
     * If that Scoped service was a DbContext, it would accumulate tracked entities indefinitely, 
     * leading to memory leaks and concurrency crashes.
     */
}


// ─────────────────────────────────────────────────────────────
// PART 46 — ADO.NET: SqlConnection, Connection String, Pooling
// ─────────────────────────────────────────────────────────────

static class Part46_SqlConnection
{
    // TASK 46-A: Anatomy of a connection string — label each segment:
    //   "Server=localhost;Database=MyAppDb;User Id=sa;Password=MyPassword123;TrustServerCertificate=True"
    //
    //   Server               → Specifies the network address or hostname (and optionally the instance name/port) of the SQL Server.
    //   Database             → Specifies the name of the specific database on the server you want to connect to.
    //   User Id / Password   → SQL Server Authentication. (The database engine itself checks the credentials).
    //   Integrated Security=True → Windows Authentication. You can't use it if your app runs on Linux/Docker without Kerberos, if you're outside the Active Directory domain, or typically with Azure SQL.
    //   TrustServerCertificate=True → Used to bypass the SSL certificate validation chain. Mostly used in local development when SQL Server generates a self-signed certificate.

    // TASK 46-B: Complete this async method that opens and tests a SqlConnection.
    public static async Task<bool> TestConnectionAsync(string connectionString)
    {
        try
        {
            // The 'using' statement ensures Dispose() is called, returning the connection to the pool.
            using var connection = new SqlConnection(connectionString);
            
            // Always use Async in ASP.NET Core to avoid thread-blocking.
            await connection.OpenAsync();
            
            Console.WriteLine($"State: {connection.State}"); // Will print "Open"
            return true;
        }
        catch (SqlException ex)
        {
            // Log the exception details in a real app (e.g., using ILogger)
            Console.WriteLine($"Database connection failed: {ex.Message}");
            return false;
        }
    }

    // TASK 46-C: CONNECTION POOLING — answer in comments
    //
    //   When you call connection.Open(), does ADO.NET create a brand-new TCP connection?
    //   No. It first checks the connection pool for an available, unused connection with the exact same connection string. It only creates a new physical TCP connection if the pool is empty (or hasn't reached max size).
    //
    //   When you call connection.Close() or Dispose(), does the TCP connection actually close?
    //   No. The physical TCP connection to the database remains open. ADO.NET simply resets the connection state and returns it to the pool so the next caller can reuse it.
    //
    //   What is the default maximum pool size?
    //   100.
    //
    //   What exception do you get when the pool is exhausted?
    //   System.InvalidOperationException: Timeout expired. The timeout period elapsed prior to obtaining a connection from the pool. This may have occurred because all pooled connections were in use and max pool size was reached.
    //
    //   What connection string parameter disables pooling (and why should you almost never do it)?
    //   Pooling=false. You should almost never do this because establishing a physical TCP connection and authenticating is a very slow and resource-heavy process. Disabling pooling will severely degrade application performance under load.

    // TASK 46-D: WHERE should the connection string be stored in a REST API?
    //   In appsettings.json, inside the "ConnectionStrings" object. (In production, it should be in Environment Variables or Azure Key Vault to keep secrets out of source control).
    //
    //   How do you retrieve it in code?
    //   Using IConfiguration: configuration.GetConnectionString("DefaultConnection");
    //   (This is a shorthand for configuration.GetSection("ConnectionStrings")["DefaultConnection"]).

    // TASK 46-E: CONNECTION STATES — list the three most important ConnectionState enum values
    //   and when each occurs.
    //   Closed → The connection is currently closed to the application. This is the state before Open() is called, or after Close()/Dispose() is called.
    //   Open   → The connection is actively open and ready to execute SQL commands against the database.
    //   Broken → The connection to the database was severed or failed after it was initially opened (e.g., network dropped, server restarted). You must Close() it before you can attempt to Open() it again.
}

// ─────────────────────────────────────────────────────────────
// PART 47 — ADO.NET: SqlCommand (NonQuery / Scalar / Reader)
// ─────────────────────────────────────────────────────────────

static class Part47_SqlCommand
{
    // TASK 47-A: ExecuteNonQueryAsync — use for INSERT, UPDATE, DELETE.
    //   Returns the NUMBER OF AFFECTED ROWS (int).
    public static async Task<int> InsertProductAsync(string connectionString, string name, decimal price)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
    
        using var command = new SqlCommand(
            "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)",
            connection);
    
        // Always use parameters to prevent SQL Injection!
        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Price", price);
    
        // ExecuteNonQueryAsync returns the number of rows affected by the command.
        return await command.ExecuteNonQueryAsync();
    }

    // TASK 47-B: ExecuteScalarAsync — use to get a single value (first cell of first row).
    //   Returns object? — you must cast it.
    //   Common use: COUNT(*), MAX(Id), or a single-column lookup.
    public static async Task<int> GetProductCountAsync(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("SELECT COUNT(*) FROM Products", connection);
        
        // ExecuteScalar returns a single object. We know COUNT(*) returns an int.
        var result = await command.ExecuteScalarAsync();
        return (int)result!; 
    }

    // TASK 47-C: ExecuteReaderAsync — use to read multiple rows.
    //   Returns a SqlDataReader. You advance it row by row with ReadAsync().
    //   IMPORTANT: the connection must remain open for the entire duration of reading.
    public static async Task<List<string>> GetProductNamesAsync(string connectionString)
    {
        var names = new List<string>();
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand("SELECT Name FROM Products", connection);
        
        // The reader requires an active, open connection while reading.
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            // Gets the string from the first column (index 0)
            names.Add(reader.GetString(0));
        }
        
        return names;
    }

    // TASK 47-D: PICK THE RIGHT EXECUTE METHOD — answer in comments:
    //   "INSERT INTO Orders ..."                        → ExecuteNonQueryAsync()
    //   "SELECT COUNT(*) FROM Orders"                   → ExecuteScalarAsync()
    //   "SELECT Id, Name, Price FROM Products"          → ExecuteReaderAsync()
    //   "UPDATE Products SET Price=@p WHERE Id=@id"     → ExecuteNonQueryAsync()
    //   "DELETE FROM Orders WHERE Id=@id"               → ExecuteNonQueryAsync()
    //   "SELECT TOP 1 Id FROM Products ORDER BY Id DESC"→ ExecuteScalarAsync()

    // TASK 47-E: SqlCommand CONSTRUCTOR — two required elements:
    //   new SqlCommand(commandText, connection);
    //   (The SQL query string and the SqlConnection object it should run against)
    //
    //   EXAM: what NuGet package must be installed?
    //   Microsoft.Data.SqlClient (Note: Do not use the legacy System.Data.SqlClient)
}
 
 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

// ─────────────────────────────────────────────────────────────
// PART 48 — SQL INJECTION & PARAMETERIZED QUERIES
// ─────────────────────────────────────────────────────────────

static class Part48_SqlInjection
{
    // TASK 48-A: VULNERABLE code — explain the attack
    //   The code below is DANGEROUS. What would an attacker type as 'username'
    //   to bypass authentication, and why does it work?
    //
    // // DANGEROUS — never do this:
    // string query = "SELECT * FROM Users WHERE Username = '" + username + "'";
    //
    // Attack input: ' OR 1=1 --
    // Resulting query: SELECT * FROM Users WHERE Username = '' OR 1=1 --'
    // Why it works: The single quote (') injected by the attacker closes the string literal early. 
    //               The 'OR 1=1' is a condition that is always true, causing the database to return 
    //               rows regardless of the username. The '--' turns the rest of the query into a comment, 
    //               nullifying any password checks that might have followed.

    // TASK 48-B: FIX IT — rewrite using parameters.
    //   Parameters are ALWAYS treated as data, never as SQL code.
    public static async Task<bool> LoginAsync(string connectionString, string username, string password)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand(
            "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password",
            connection);
            
        // Parameters ensure the input is treated strictly as a literal value, not executable syntax.
        command.Parameters.AddWithValue("@Username", username);
        command.Parameters.AddWithValue("@Password", password);
        
        var count = (int)(await command.ExecuteScalarAsync())!;
        return count > 0;
    }

    // TASK 48-C: WHAT CAN AND CANNOT be parameterized?
    //   Fill in "YES — safe to parameterize" or "NO — cannot parameterize":
    //
    //   WHERE Id = @Id                          → YES — safe to parameterize
    //   VALUES (@Name, @Price) in INSERT        → YES — safe to parameterize
    //   Table name: SELECT * FROM @TableName    → NO — cannot parameterize (SQL engines do not allow parameterizing identifiers like table or column names; use a strict allowlist in C# instead)
    //   Column name in ORDER BY @SortCol        → NO — cannot parameterize (Same reason as above)
    //   LIKE clause: WHERE Name LIKE @Term      → YES — safe to parameterize (But you must append the '%' wildcards in C#, e.g., AddWithValue("@Term", "%" + term + "%"))

    // TASK 48-D: NULL PARAMETERS — how do you pass a C# null as a SQL NULL?
    //   command.Parameters.AddWithValue("@Param", (object?)value ?? DBNull.Value);
    //
    //   Why does passing plain null cause a runtime error?
    //   ADO.NET interprets a plain C# 'null' as meaning "I am not providing this parameter at all." 
    //   To explicitly tell the database to insert a SQL NULL, you must pass the special DBNull.Value object.
}


// ─────────────────────────────────────────────────────────────
// PART 49 — ADO.NET: SqlDataReader & NULL HANDLING
// ─────────────────────────────────────────────────────────────

static class Part49_SqlDataReader
{
    // TASK 49-A: SqlDataReader characteristics — answer in comments:
    //
    //   Is it forward-only or can you go back?
    //   It is strictly forward-only. You cannot move backward to a previous row.
    //
    //   Does it load all rows into memory at once?
    //   No, it streams rows one at a time from the network. This makes it highly memory-efficient for large datasets.
    //
    //   Must the connection stay open while you read?
    //   Yes. Because it is actively streaming data from the database, closing the connection kills the reader.
    //
    //   Where does the reader point BEFORE the first call to ReadAsync()?
    //   It points to a position *before* the first row. You must call ReadAsync() at least once to advance to the first actual row of data.

    // TASK 49-B: NULL HANDLING — the dangerous pattern and the fix.
    //   If a column contains a database NULL and you call reader.GetString(col),
    //   what exception is thrown?
    //   System.Data.SqlTypes.SqlNullValueException (which often surfaces as an InvalidCastException).
    //
    //   Write the SAFE pattern for reading a nullable string column:
    //   string? description = reader.IsDBNull(reader.GetOrdinal("Description"))
    //       ? null
    //       : reader.GetString(reader.GetOrdinal("Description"));
    //
    //   Explain what IsDBNull does:
    //   IsDBNull checks if the database value at the specified column index is equivalent to SQL NULL 
    //   before you attempt to read it, preventing the cast exception from being thrown.

    // TASK 49-C: Complete this reader-based method that maps rows to objects.
    //   Handle nullable columns (Description can be NULL in the database).
    
    public record Product(int Id, string Name, string? Description, decimal Price);
    
    public static async Task<List<Product>> GetProductsAsync(string connectionString)
    {
        var products = new List<Product>();
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        
        using var command = new SqlCommand(
            "SELECT Id, Name, Description, Price FROM Products", connection);
            
        using var reader = await command.ExecuteReaderAsync();
        
        // Cache ordinals outside the loop for better performance, 
        // rather than looking up the string name for every single row.
        int idOrdinal = reader.GetOrdinal("Id");
        int nameOrdinal = reader.GetOrdinal("Name");
        int descOrdinal = reader.GetOrdinal("Description");
        int priceOrdinal = reader.GetOrdinal("Price");

        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32(idOrdinal);
            var name = reader.GetString(nameOrdinal);
            
            // Safe NULL handling for the Description column
            var description = reader.IsDBNull(descOrdinal) 
                ? null 
                : reader.GetString(descOrdinal);
                
            var price = reader.GetDecimal(priceOrdinal);
            
            products.Add(new Product(id, name, description, price));
        }
        
        return products;
    }
}


// ─────────────────────────────────────────────────────────────
// PART 50 — ADO.NET: TRANSACTIONS (ACID, Commit, Rollback, Isolation)
// ─────────────────────────────────────────────────────────────

static class Part50_Transactions
{
    // TASK 50-A: ACID — explain each property in one sentence:
    //   A — Atomicity:   All operations in a transaction succeed together, or they all fail together (all-or-nothing).
    //   C — Consistency: The transaction transitions the database from one valid state to another, enforcing all constraints.
    //   I — Isolation:   Concurrent transactions do not interfere with each other, acting as if they run sequentially.
    //   D — Durability:  Once committed, the changes are permanent and will survive a system crash or power failure.

    // TASK 50-B: Implement a bank transfer using SqlTransaction.
    public static async Task TransferMoneyAsync(string connectionString, int fromId, int toId, decimal amount)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        // Begin transaction
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();

        try
        {
            // Withdrawal command
            await using var withdrawCmd = new SqlCommand(
                "UPDATE Accounts SET Balance = Balance - @Amount WHERE Id = @FromId", connection);
            // CRITICAL: You must explicitly assign the transaction to the command!
            withdrawCmd.Transaction = transaction;
            withdrawCmd.Parameters.AddWithValue("@Amount", amount);
            withdrawCmd.Parameters.AddWithValue("@FromId", fromId);
            await withdrawCmd.ExecuteNonQueryAsync();

            // Deposit command
            await using var depositCmd = new SqlCommand(
                "UPDATE Accounts SET Balance = Balance + @Amount WHERE Id = @ToId", connection);
            depositCmd.Transaction = transaction;
            depositCmd.Parameters.AddWithValue("@Amount", amount);
            depositCmd.Parameters.AddWithValue("@ToId", toId);
            await depositCmd.ExecuteNonQueryAsync();

            // If we made it here, both commands succeeded. Commit the changes!
            await transaction.CommitAsync();
        }
        catch
        {
            // If anything goes wrong, revert any changes made so far.
            await transaction.RollbackAsync();
            throw; // Rethrow so the caller knows the transfer failed
        }
    }
    
    // EXAM: What happens if you create a SqlCommand after BeginTransaction
    //   but forget to assign the transaction to it?
    // ANSWER: You will get an InvalidOperationException at runtime stating: 
    //   "ExecuteNonQuery requires the command to have a transaction when the connection assigned to the command is in a pending local transaction."

    // TASK 50-C: ISOLATION LEVELS — match each level to its behavior:
    //
    //   ReadUncommitted → Lowest level, allows reading uncommitted data (dirty reads possible).
    //   ReadCommitted   → SQL Server default — reads only committed data (dirty reads prevented).
    //   RepeatableRead  → Places locks on all data read. The same read returns the same result within the transaction (prevents non-repeatable reads).
    //   Serializable    → Most restrictive — transactions appear completely sequential (prevents phantom reads by locking ranges).
    //
    // How do you set the isolation level in ADO.NET?
    // ANSWER: await connection.BeginTransactionAsync(IsolationLevel.Serializable);
    //
    // Practical rule: which level is sufficient for most common scenarios?
    // ANSWER: ReadCommitted (the default) is sufficient for the vast majority of applications.

    // TASK 50-D: COMMIT vs ROLLBACK
    //   What does Commit do?  Saves all the operations performed in the transaction permanently to the database.
    //   What does Rollback do? Reverts all operations performed in the transaction, leaving the database unchanged.
    //   If a SqlTransaction is disposed (via 'using') WITHOUT a Commit, what happens? 
    //   ANSWER: ADO.NET automatically rolls it back. However, it is best practice to always call Rollback explicitly in the catch block for clarity and safety.

    // TASK 50-E: WHEN TO USE TRANSACTIONS — mark each scenario YES or NO:
    //   Create an order + add order items + reduce stock    → YES
    //   Read a single product by ID                         → NO (single reads don't need explicit transactions)
    //   Transfer money between accounts                     → YES
    //   Make an HTTP call to a payment gateway mid-transfer → NO (EXAM PITFALL!)
    //   Why? External calls like HTTP can take seconds to complete or time out. A transaction holds database locks for its entire duration. Holding locks while waiting for a slow network call will severely bottleneck your database and cause deadlocks.
}


// ─────────────────────────────────────────────────────────────
// PART 51 — EXAM SIMULATION: Full 3-Layer Endpoint + Transaction
// ─────────────────────────────────────────────────────────────

namespace ExamSimulation
{
    // STEP 1: Define DTOs and models
    public record PlaceOrderDto(int CustomerId, int BookId, int Quantity);
    public record OrderResult(int OrderId, decimal TotalPrice);

    // STEP 2: Repository interface + SQL implementation
    public interface IOrderRepository
    {
        Task<bool> CustomerExistsAsync(int customerId);
        Task<int> GetBookStockAsync(int bookId);
        Task<decimal> GetBookPriceAsync(int bookId);
        Task<int> PlaceOrderAsync(int customerId, int bookId, int quantity, decimal totalPrice);
    }

    public class SqlOrderRepository : IOrderRepository
    {
        private readonly string _connectionString;
        public SqlOrderRepository(IConfiguration config)
            => _connectionString = config.GetConnectionString("DefaultConnection")
               ?? throw new InvalidOperationException("Missing connection string");

        public async Task<bool> CustomerExistsAsync(int customerId)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new SqlCommand("SELECT COUNT(*) FROM Customers WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", customerId);
            
            return (int)(await command.ExecuteScalarAsync())! > 0;
        }

        public async Task<int> GetBookStockAsync(int bookId)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new SqlCommand("SELECT Stock FROM Books WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", bookId);
            
            var result = await command.ExecuteScalarAsync();
            return result == null ? 0 : (int)result;
        }

        public async Task<decimal> GetBookPriceAsync(int bookId)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new SqlCommand("SELECT Price FROM Books WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", bookId);
            
            var result = await command.ExecuteScalarAsync();
            return result == null ? 0m : (decimal)result;
        }

        public async Task<int> PlaceOrderAsync(int customerId, int bookId, int quantity, decimal totalPrice)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
            
            try
            {
                // a) INSERT Order and get OrderId using SCOPE_IDENTITY()
                await using var insertCommand = new SqlCommand(
                    "INSERT INTO Orders (CustomerId, TotalPrice) VALUES (@Cid, @Total); SELECT CAST(SCOPE_IDENTITY() AS INT);", 
                    connection, 
                    transaction); // DO NOT FORGET THE TRANSACTION!
                    
                insertCommand.Parameters.AddWithValue("@Cid", customerId);
                insertCommand.Parameters.AddWithValue("@Total", totalPrice);
                
                int orderId = (int)(await insertCommand.ExecuteScalarAsync())!;

                // b) UPDATE Book Stock
                await using var updateCommand = new SqlCommand(
                    "UPDATE Books SET Stock = Stock - @Quantity WHERE Id = @BookId", 
                    connection, 
                    transaction);
                    
                updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                updateCommand.Parameters.AddWithValue("@BookId", bookId);
                
                await updateCommand.ExecuteNonQueryAsync();

                // Commit if both succeeded
                await transaction.CommitAsync();
                
                return orderId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    // Custom Exceptions for Business Logic
    public class NotFoundException : Exception { public NotFoundException(string message) : base(message) {} }
    public class BadRequestException : Exception { public BadRequestException(string message) : base(message) {} }

    // STEP 3: Service
    public interface IOrderService
    {
        Task<OrderResult> PlaceOrderAsync(PlaceOrderDto dto);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        public OrderService(IOrderRepository repo) => _repo = repo;

        public async Task<OrderResult> PlaceOrderAsync(PlaceOrderDto dto)
        {
            // Business Rule 1: Customer must exist
            bool customerExists = await _repo.CustomerExistsAsync(dto.CustomerId);
            if (!customerExists)
                throw new NotFoundException($"Customer with ID {dto.CustomerId} not found.");

            // Business Rule 2: Book must have enough stock
            int stock = await _repo.GetBookStockAsync(dto.BookId);
            if (stock < dto.Quantity)
                throw new BadRequestException($"Insufficient stock for Book ID {dto.BookId}. Requested: {dto.Quantity}, Available: {stock}.");

            // Calculate total price
            decimal price = await _repo.GetBookPriceAsync(dto.BookId);
            decimal totalPrice = price * dto.Quantity;

            // Apply transactional operation
            int orderId = await _repo.PlaceOrderAsync(dto.CustomerId, dto.BookId, dto.Quantity, totalPrice);

            return new OrderResult(orderId, totalPrice);
        }
    }

    // STEP 4: Controller
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrdersController(IOrderService service) => _service = service;

        [HttpPost]
        public async Task<ActionResult<OrderResult>> PlaceOrder(PlaceOrderDto dto)
        {
            try
            {
                OrderResult result = await _service.PlaceOrderAsync(dto);
                // Return 201 Created (ideally pointing to a GetOrderById endpoint, but using string.Empty for simplicity here)
                return Created(string.Empty, result); 
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Fallback for unexpected exceptions
                return StatusCode(500, new { error = "An unexpected error occurred processing your order.", details = ex.Message });
            }
        }
    }

    // STEP 5: DI Registration in Program.cs
    public static class DependencyInjectionSetup
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Register layers using Scoped lifetime (standard for EF Core and ADO.NET Repositories in Web APIs)
            services.AddScoped<IOrderRepository, SqlOrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}
 
 
// ═══════════════════════════════════════════════════════════════
//  ★  PDF SUPPLEMENT — topics found in the study guides
//     that were NOT yet covered above
//
//   Part 52 — events (event keyword, += vs =, Clicked?.Invoke)
//   Part 53 — sealed keyword (class & method)
//   Part 54 — Pattern Matching Deep Dive (property, list, relational)
//   Part 55 — record struct vs record class
//   Part 56 — Git: remote commands (push -u, fetch, stash, blame, diff)
//   Part 57 — Git: --force-with-lease, pull.rebase, git log flags
//   Part 58 — ADO.NET: SqlDataReader typed methods & GetOrdinal
//   Part 59 — ADO.NET: SqlCommand — 3 ways to assign transaction
//   Part 60 — ADO.NET: SCOPE_IDENTITY() + MapToObject pattern
//   Part 61 — ADO.NET: Full e-commerce order transaction (multi-table)
//   Part 62 — Exam Simulation: OOP + Records + Notification Dispatcher
// ═══════════════════════════════════════════════════════════════
 
 
// ─────────────────────────────────────────────────────────────
// PART 52 — EVENTS (event keyword, += / -=, Clicked?.Invoke)
// ─────────────────────────────────────────────────────────────
// From the APBD Study Guide §0.8 — a missing topic from Part 7.
// The 'event' keyword restricts a delegate field so that
// OUTSIDE the class only += and -= are allowed.
// INSIDE the class you can invoke and reassign freely.
 
// static class Part52_Events
// {
//     // TASK 52-A: Implement a Button class with a Click event.
//     //   Rules enforced by 'event':
//     //     - From outside: only += and -= are allowed
//     //     - Assignment (=) from outside → COMPILER ERROR
//     //     - Invocation from outside → COMPILER ERROR
//     //
//     // public class Button
//     // {
//     //     // Public FIELD delegate — dangerous: anyone can invoke/reassign/clear
//     //     public Action? OnClickRaw; // no 'event' keyword
//     //
//     //     // EVENT — from outside only += and -= are permitted
//     //     public event Action? Clicked;
//     //
//     //     public void Click()
//     //     {
//     //         // TODO: Clicked?.Invoke();  // ?.Invoke() is null-safe invocation
//     //     }
//     // }
//     //
//     // public static void Demo()
//     // {
//     //     var btn = new Button();
//     //     btn.Clicked += () => Console.WriteLine("Handler 1 fired");
//     //     btn.Clicked += () => Console.WriteLine("Handler 2 fired");
//     //     // btn.Clicked = null;   // COMPILER ERROR — cannot assign from outside
//     //     // btn.Clicked();        // COMPILER ERROR — cannot invoke from outside
//     //     btn.Click();            // fires both handlers
//     // }
//
//     // TASK 52-B: What EXACTLY does 'event' prevent from outside the class?
//     //   TODO: (1) Assignment with = (2) Direct invocation ()
//     //   Only += and -= are allowed from outside.
//
//     // TASK 52-C: When is async void the ONLY acceptable use?
//     //   TODO: ... event handlers (Button_Click, etc.) because event handlers
//     //   are required to return void, and async Task handlers can't be assigned
//     //   to Action?-based events directly.
// }
 
 
// ─────────────────────────────────────────────────────────────
// PART 53 — SEALED KEYWORD (class & method)
// ─────────────────────────────────────────────────────────────
// From the APBD/CSharp Mastery Guide §2.1 table — sealed was listed
// but not practiced in Parts 11-12 above.
 
// static class Part53_Sealed
// {
//     // TASK 53-A: sealed CLASS — prevents further inheritance
//     //   Use case: immutable data types, security-sensitive classes,
//     //   performance optimization (JIT can devirtualize calls).
//     //
//     // public sealed class ImmutablePoint
//     // {
//     //     public int X { get; }
//     //     public int Y { get; }
//     //     public ImmutablePoint(int x, int y) { X = x; Y = y; }
//     // }
//     // // public class SpecialPoint : ImmutablePoint { } // COMPILER ERROR
//
//     // TASK 53-B: sealed OVERRIDE — prevent further overriding in subclass chain
//     //
//     // public abstract class Shape
//     // {
//     //     public abstract double Area();
//     //     public virtual string Describe() => $"Area = {Area():F2}";
//     // }
//     //
//     // public class Circle : Shape
//     // {
//     //     public double Radius { get; }
//     //     public Circle(double r) => Radius = r;
//     //     public override double Area() => Math.PI * Radius * Radius;
//     //     public sealed override string Describe() => $"Circle r={Radius} → {base.Describe()}";
//     //     // 'sealed' here: subclasses of Circle cannot override Describe() again
//     // }
//     //
//     // public class SpecialCircle : Circle
//     // {
//     //     public SpecialCircle(double r) : base(r) { }
//     //     // public override string Describe() => "..."; // COMPILER ERROR — sealed!
//     // }
//
//     // TASK 53-C: EXAM comparison table — complete in comments:
//     //   Keyword         | Applied to | Effect
//     //   ----------------|------------|------------------------------------------
//     //   abstract        | class/method | TODO: ...
//     //   virtual         | method     | TODO: ...
//     //   override        | method     | TODO: ...
//     //   new (hiding)    | method     | TODO: ...
//     //   sealed (class)  | class      | TODO: ...
//     //   sealed (method) | method     | TODO: ...
// }
 
 
// ─────────────────────────────────────────────────────────────
// PART 54 — PATTERN MATCHING DEEP DIVE (C# 8–11)
// ─────────────────────────────────────────────────────────────
// From CSharp Mastery Guide §9.6 — Part 9 above covers basics;
// this covers property patterns, relational patterns, and list patterns.
 
// static class Part54_PatternMatchingDeep
// {
//     // TASK 54-A: PROPERTY PATTERN — match on properties inside the type check
//     //
//     // Assume these types from Part 11:
//     // class Laptop   : Equipment { public int RamSizeGB; public string OsVersion; }
//     // class Camera   : Equipment { public int Megapixels; }
//     // class Projector: Equipment { public int Lumens; }
//     //
//     // public static string Categorise(object equipment) => equipment switch
//     // {
//     //     // TODO: Laptop with RamSizeGB >= 32 → "Workstation laptop"
//     //     // TODO: Laptop with RamSizeGB >= 16 → "Professional laptop"
//     //     // TODO: Laptop (any)               → "Standard laptop"
//     //     // TODO: Camera with Megapixels >= 50 → "Professional camera"
//     //     // TODO: Camera (any)               → "Standard camera"
//     //     // TODO: Projector with Lumens >= 5000 → "Conference projector"
//     //     // TODO: _ (wildcard)               → "Other equipment"
//     // };
//
//     // TASK 54-B: TUPLE PATTERN — match on combinations of values
//     //
//     // public static string RentalStatus(bool isActive, bool isOverdue) =>
//     //     (isActive, isOverdue) switch
//     //     {
//     //         // TODO: (false, _)    → "Returned"
//     //         // TODO: (true, true)  → "OVERDUE"
//     //         // TODO: (true, false) → "Active"
//     //     };
//
//     // TASK 54-C: RELATIONAL PATTERN + when guard
//     //
//     // public static string GradeLabel(int score) => score switch
//     // {
//     //     // TODO: >= 90          → "A"
//     //     // TODO: >= 75          → "B"
//     //     // TODO: >= 60 and < 75 → "C"  (uses 'and' pattern combinator)
//     //     // TODO: _              → "F"
//     // };
//
//     // TASK 54-D: LIST PATTERN (C# 11) — match on array contents
//     //
//     // int[] arr = { 1, 2, 3 };
//     // // TODO: match if arr starts with 1, 2 (use [1, 2, ..] pattern)
//     // // TODO: match if arr has exactly one element (use [var only] pattern)
//     // // TODO: match if arr is empty (use [] pattern)
//     //
//     // EXAM: What does '..' mean inside a list pattern?
//     // TODO: ... (matches zero or more elements — the "slice pattern")
//
//     // TASK 54-E: 'is' expression with declaration
//     //   Write code that checks if an object is a Laptop with >= 16GB RAM
//     //   and, if so, prints its name using the declared variable.
//     //
//     // object obj = new Laptop("Dell", 16, "Win11");
//     // // TODO: if (obj is Laptop { RamSizeGB: >= 16 } laptop)
//     // //           Console.WriteLine($"{laptop.Name} is high-spec");
// }
 
 
// ─────────────────────────────────────────────────────────────
// PART 55 — RECORD STRUCT vs RECORD CLASS
// ─────────────────────────────────────────────────────────────
// From APBD Study Guide §0.10 exam note — 'record struct' was
// explicitly called out but NOT practiced in Part 6 above.
 
// static class Part55_RecordStruct
// {
//     // TASK 55-A: Understand the difference — fill in this table in comments:
//     //
//     //   Feature                   | record class (default) | record struct
//     //   --------------------------|------------------------|--------------------
//     //   Stored in                 | TODO: ...              | TODO: ...
//     //   Immutable by default?     | TODO: ...              | TODO: ...
//     //   Value equality            | TODO: ...              | TODO: ...
//     //   Can inherit?              | TODO: ...              | TODO: ...
//     //   'with' expression works?  | TODO: ...              | TODO: ...
//     //   Suitable for keys in Dict | TODO: ...              | TODO: ...
//
//     // TASK 55-B: Define a record struct Point2D with X and Y.
//     //   Show that:
//     //     1. A copy of a record struct is INDEPENDENT (value type)
//     //     2. You CAN mutate a record struct field (unlike positional record class)
//     //     3. Value equality still works
//     //
//     // TODO: public record struct Point2D(double X, double Y);
//     //
//     // var p1 = new Point2D(1.0, 2.0);
//     // var p2 = p1;          // independent copy — value type!
//     // p2 = p2 with { X = 99 };
//     // Console.WriteLine(p1.X); // TODO: what does this print and why?
//     // Console.WriteLine(p1 == new Point2D(1.0, 2.0)); // value equality
//
//     // TASK 55-C: EXAM TRAP — 'record struct' is NOT immutable
//     //   Show that you can do this (unlike a positional record class):
//     //
//     // public record struct MutablePoint(int X, int Y);
//     // var mp = new MutablePoint(1, 2);
//     // mp.X = 99; // This compiles! record struct fields are mutable.
//     // // But for a positional record CLASS: s.City = "x"; // COMPILER ERROR
//     //
//     // TODO: demonstrate the difference
// }
 
 
// ─────────────────────────────────────────────────────────────
// PART 56 — GIT: REMOTE COMMANDS, STASH, BLAME, DIFF
// ─────────────────────────────────────────────────────────────
// From APBD §1.7 and Git supplement §A — these commands were NOT
// covered in Part 1 above.
 
// static class Part56_GitRemoteAndAdvanced
// {
//     // TASK 56-A: REMOTE WORKFLOW — write all commands in comments
//     //
//     //   Clone an existing repo from GitHub:
//     //   TODO: git clone https://github.com/user/repo.git
//     //
//     //   Add a remote named 'origin' to a local repo that has none:
//     //   TODO: git remote add origin https://...
//     //
//     //   Push for the first time and set upstream tracking:
//     //   TODO: git push -u origin main
//     //   (What does -u / --set-upstream do? TODO: ...)
//     //
//     //   Subsequent pushes once tracking is set:
//     //   TODO: git push
//     //
//     //   Fetch (download) without merging:
//     //   TODO: git fetch origin
//     //   (How is this different from git pull? TODO: ...)
//     //
//     //   Pull = fetch + merge (or rebase if pull.rebase=true):
//     //   TODO: git pull
//
//     // TASK 56-B: STASH — temporarily shelve uncommitted work
//     //
//     //   Save current changes without committing:
//     //   TODO: git stash
//     //
//     //   Restore the most recent stash:
//     //   TODO: git stash pop
//     //
//     //   List all stashes:
//     //   TODO: git stash list
//     //
//     //   Scenario: you're mid-feature, urgent hotfix needed.
//     //   Write the full sequence of commands:
//     //   TODO: git stash → git switch main → fix → commit → git switch feature → git stash pop
//
//     // TASK 56-C: DIFF AND BLAME — inspecting changes
//     //
//     //   See unstaged changes:
//     //   TODO: git diff
//     //
//     //   See staged changes (what will be committed):
//     //   TODO: git diff --staged
//     //
//     //   See differences between two branches:
//     //   TODO: git diff main..feature/login
//     //
//     //   See full diff of a specific commit:
//     //   TODO: git show a1b2c3d
//     //
//     //   See who wrote which lines (lines 10-20) of a file:
//     //   TODO: git blame -L 10,20 Program.cs
//     //   (What information does each line of 'blame' output show? TODO: ...)
// }
 
 
// ─────────────────────────────────────────────────────────────
// PART 57 — GIT: --force-with-lease, pull.rebase, log flags
// ─────────────────────────────────────────────────────────────
// From the Git supplement §A decision table and workflow diagrams.
 
// static class Part57_GitAdvancedWorkflow
// {
//     // TASK 57-A: --force-with-lease vs --force
//     //
//     //   What does git push --force do?
//     //   TODO: ...
//     //   Why is it dangerous?
//     //   TODO: ... (overwrites remote even if teammates pushed since you last fetched)
//     //
//     //   What does git push --force-with-lease do differently?
//     //   TODO: ... (only overwrites if the remote tip matches your local tracking ref —
//     //             safe: fails if someone else pushed)
//     //
//     //   When MUST you use one of these? (After what operation?)
//     //   TODO: ... (after rebasing already-pushed commits — their SHAs changed)
//     //   Which one should you always prefer?
//     //   TODO: --force-with-lease
 
//     // TASK 57-B: pull.rebase configuration
//     //
//     //   Set git pull to always rebase instead of merge:
//     //   TODO: git config --global pull.rebase true
//     //   Why is this the recommended setting?
//     //   TODO: ... (keeps history linear — avoids noisy "Merge branch main" commits)
//
//     // TASK 57-C: git log flags — memorise for the exam
//     //   Explain what each flag does in:
//     //   git log --oneline --graph --all --decorate
//     //
//     //   --oneline:  TODO: ...
//     //   --graph:    TODO: ...
//     //   --all:      TODO: ...
//     //   --decorate: TODO: ...
//     //
//     //   EXAM: The above command is the one most likely asked to reproduce exactly.
//
//     // TASK 57-D: Decision table — WHEN to use merge vs rebase
//     //   Fill in the right choice and reason:
//     //
//     //   Situation                                    | Use        | Why
//     //   --------------------------------------------|------------|-------------------
//     //   Local feature branch, NOT yet pushed        | TODO: ...  | TODO: ...
//     //   Branch that teammates are working on too    | TODO: ...  | TODO: ...
//     //   Keeping your feature branch up-to-date      | TODO: ...  | TODO: ...
//     //   Hotfix merged into main urgently            | TODO: ...  | TODO: ...
//     //   git pull (fetch + integrate)                | TODO: ... (set pull.rebase true)
// }
 
 

// ─────────────────────────────────────────────────────────────
// PART 58 — ADO.NET: SqlDataReader TYPED METHODS & GetOrdinal
// ─────────────────────────────────────────────────────────────

static class Part58_ReaderTypedMethods
{
    // TASK 58-A: TYPED METHOD REFERENCE TABLE — fill in each C# type:
    //
    //   C# method              | SQL Server type         | Notes
    //   -----------------------|-------------------------|------------------------------
    //   reader.GetInt32(i)     | int                     | Most common for IDs
    //   reader.GetInt64(i)     | bigint                  | Used for very large IDs, file sizes, or high-precision timestamps.
    //   reader.GetString(i)    | varchar, nvarchar       | Most common for text
    //   reader.GetDecimal(i)   | decimal, money          | EXAM: Never use GetDouble for money because floating-point math causes precision loss/rounding errors. Decimal is fixed-point.
    //   reader.GetDateTime(i)  | datetime, datetime2     | Returns System.DateTime in C#.
    //   reader.GetBoolean(i)   | bit                     | Maps to C# 'bool' (0 = false, 1 = true).
    //   reader.GetGuid(i)      | uniqueidentifier        | Returns System.Guid in C#.
    //   reader.GetOrdinal("X") | —                       | Returns 'int' representing the 0-based column index for the given column name.

    // TASK 58-B: GetOrdinal — safer column access by NAME instead of index
    //   Why is column index fragile? What if the SELECT changes column order?
    //   If someone modifies the SQL query (e.g., changes "SELECT Id, Name" to "SELECT Name, Id" or adds a new column in a "SELECT *"), hardcoded indices will read the wrong data or cause InvalidCastExceptions.
    //
    //   Rewrite this unsafe code to use GetOrdinal:
    //   SAFE: int id = reader.GetInt32(reader.GetOrdinal("Id"));
    //
    //   PERFORMANCE TIP: cache GetOrdinal results BEFORE the while loop
    //   int idIndex = reader.GetOrdinal("Id");
    //   int nameIndex = reader.GetOrdinal("Name");
    //   while (await reader.ReadAsync()) {
    //       int id = reader.GetInt32(idIndex);
    //   }

    // TASK 58-C: Complete this mapping method using GetOrdinal and IsDBNull
    
    public record Student(int Id, string FirstName, string LastName, DateTime DateOfBirth, string? Email);
    
    private static Student MapToStudent(SqlDataReader r)
    {
        return new Student(
            r.GetInt32(r.GetOrdinal("Id")),
            r.GetString(r.GetOrdinal("FirstName")),
            r.GetString(r.GetOrdinal("LastName")),
            r.GetDateTime(r.GetOrdinal("DateOfBirth")),
            r.IsDBNull(r.GetOrdinal("Email")) ? null : r.GetString(r.GetOrdinal("Email"))
        );
    }
    
    // TASK: Why is extracting MapToStudent() into a private method a good practice?
    // DRY (Don't Repeat Yourself). You can reuse this exact same mapping logic for GetAllStudents(), GetStudentById(), etc. If the database schema changes, you only update the mapping in one place, making it easier to test and maintain.
}


// ─────────────────────────────────────────────────────────────
// PART 59 — ADO.NET: THREE WAYS TO ASSIGN TRANSACTION
// ─────────────────────────────────────────────────────────────

static class Part59_TransactionAssignment
{
    // TASK 59-A: The MOST COMMON BUG in ADO.NET transactions
    //   What runtime exception do you get if you create a SqlCommand
    //   after BeginTransaction() but forget to assign the transaction?
    //   SqlException: "ExecuteNonQuery requires the command to have a transaction when the connection assigned to the command is in a pending local transaction."

    // TASK 59-B: All THREE ways to assign a transaction — implement each
    public static void TransactionExamples(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var tx = connection.BeginTransaction();
        
        // Method 1: Pass as 3rd argument to constructor (PREFERRED — most readable)
        using var cmd1 = new SqlCommand("UPDATE Table1 SET Col = 1", connection, tx);
        
        // Method 2: Set the .Transaction property after construction
        using var cmd2 = new SqlCommand("UPDATE Table1 SET Col = 1", connection);
        cmd2.Transaction = tx;
        
        // Method 3: connection.CreateCommand() then set both properties
        using var cmd3 = connection.CreateCommand();
        cmd3.CommandText = "UPDATE Table1 SET Col = 1";
        cmd3.Transaction = tx;
    }
    
    // EXAM: which method is preferred and why?
    // Method 1. By passing it in the constructor, the compiler/IntelliSense reminds you to include it, and the transaction is explicitly bound the moment the object is created. You cannot forget to set it on the next line.

    // TASK 59-C: ISOLATION LEVELS — fill in the blank for each level's guarantees:
    //
    //   Level             | Dirty Reads | Non-repeatable | Phantoms | Use when
    //   ------------------|-------------|----------------|----------|----------
    //   ReadUncommitted   | possible    | possible       | possible | Reading massive logs/analytics where exact precision isn't critical, to avoid blocking writers.
    //   ReadCommitted     | prevented   | possible       | possible | General standard use (SQL Server DEFAULT).
    //   RepeatableRead    | prevented   | prevented      | possible | Financial reports/aggregates where data cannot change mid-read.
    //   Serializable      | prevented   | prevented      | prevented| Strict inventory/booking systems to prevent concurrent inserts from ruining availability logic.
    //
    //   How to set isolation level in code:
    //   using var tx = connection.BeginTransaction(IsolationLevel.Serializable);
}


// ─────────────────────────────────────────────────────────────
// PART 60 — ADO.NET: SCOPE_IDENTITY() + MapToObject PATTERN
// ─────────────────────────────────────────────────────────────

static class Part60_ScopeIdentityAndMapping
{
    // TASK 60-A: SCOPE_IDENTITY() — get the auto-generated ID of a new row
    //   Why can't you just use ExecuteNonQueryAsync for INSERT when you need the ID?
    //   ExecuteNonQueryAsync only returns an integer representing the number of rows affected (e.g., 1 row inserted). It does not return data generated by the database.
    //
    public static async Task<int> InsertStudentAsync(string connectionString, string firstName, string lastName)
    {
        using var conn = new SqlConnection(connectionString);
        await conn.OpenAsync();
        using var cmd = new SqlCommand(
            "INSERT INTO Students(FirstName,LastName) VALUES(@fn,@ln); SELECT SCOPE_IDENTITY();",
            conn);
            
        cmd.Parameters.AddWithValue("@fn", firstName);
        cmd.Parameters.AddWithValue("@ln", lastName);
        
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result); // Safe conversion
    }
    
    // EXAM: Why use Convert.ToInt32() instead of (int) cast for SCOPE_IDENTITY?
    // SCOPE_IDENTITY() returns a numeric/decimal type in SQL Server, which ADO.NET unboxes as a C# decimal. Casting a boxed decimal directly to (int) throws an InvalidCastException. Convert.ToInt32 handles the type conversion safely.

    // TASK 60-B: MapToObject repository pattern — complete GetAllAsync
    public record Student(int Id, string FirstName, string LastName, DateTime DateOfBirth, string? Email);
    
    public static async Task<List<Student>> GetAllStudentsAsync(string connectionString)
    {
        var students = new List<Student>();
        using var conn = new SqlConnection(connectionString);
        await conn.OpenAsync();
        using var cmd = new SqlCommand(
            "SELECT Id, FirstName, LastName, DateOfBirth, Email FROM Students", conn);
            
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            students.Add(MapToStudent(reader));
        }
        return students;
    }
    
    private static Student MapToStudent(SqlDataReader r) =>
        new(
            r.GetInt32(r.GetOrdinal("Id")),
            r.GetString(r.GetOrdinal("FirstName")),
            r.GetString(r.GetOrdinal("LastName")),
            r.GetDateTime(r.GetOrdinal("DateOfBirth")),
            r.IsDBNull(r.GetOrdinal("Email")) ? null : r.GetString(r.GetOrdinal("Email"))
        );

    // TASK 60-C: DELETE returns 0 when row not found — use for 404 detection
    public static async Task<bool> DeleteStudentAsync(string connectionString, int id)
    {
        using var conn = new SqlConnection(connectionString);
        await conn.OpenAsync();
        using var cmd = new SqlCommand("DELETE FROM Students WHERE Id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        
        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;  // false = not found (return 404 in controller)
    }
}


// ─────────────────────────────────────────────────────────────
// PART 61 — ADO.NET: FULL MULTI-TABLE TRANSACTION
// ─────────────────────────────────────────────────────────────

static class Part61_MultiTableTransaction
{
    public static async Task<int> PlaceOrderAsync(
        string connectionString,
        int customerId,
        List<(int ProductId, int Qty)> items)
    {
        using var conn = new SqlConnection(connectionString);
        await conn.OpenAsync();
        using var tx = (SqlTransaction)await conn.BeginTransactionAsync(); 
        
        try
        {
            // Step 1: Insert order header — get the new OrderId
            using var orderCmd = new SqlCommand(
                "INSERT INTO Orders(CustomerId,OrderDate,Status) VALUES(@cid,GETDATE(),'Pending');" +
                " SELECT SCOPE_IDENTITY();",
                conn, tx);  // tx as 3rd arg — critical!
                
            orderCmd.Parameters.AddWithValue("@cid", customerId);
            int orderId = Convert.ToInt32(await orderCmd.ExecuteScalarAsync());
    
            // Step 2: For each item — insert order line + reduce stock
            foreach (var (productId, qty) in items)
            {
                // Insert OrderLine
                using var lineCmd = new SqlCommand(
                    "INSERT INTO OrderLines(OrderId, ProductId, Quantity) VALUES(@oid, @pid, @qty)", 
                    conn, tx);
                lineCmd.Parameters.AddWithValue("@oid", orderId);
                lineCmd.Parameters.AddWithValue("@pid", productId);
                lineCmd.Parameters.AddWithValue("@qty", qty);
                await lineCmd.ExecuteNonQueryAsync();
                
                // Update Product Stock
                using var updateCmd = new SqlCommand(
                    "UPDATE Products SET Stock = Stock - @qty WHERE Id = @pid", 
                    conn, tx);
                updateCmd.Parameters.AddWithValue("@pid", productId);
                updateCmd.Parameters.AddWithValue("@qty", qty);
                
                int updated = await updateCmd.ExecuteNonQueryAsync();
                if (updated == 0)
                {
                    // Triggers the catch block to rollback if product doesn't exist
                    throw new Exception($"Product {productId} not found.");
                }
            }
    
            await tx.CommitAsync();
            return orderId;
        }
        catch
        {
            await tx.RollbackAsync();
            throw;  // rethrow so caller (controller) knows it failed
        }
    }

    // TASK 61-B: EXAM QUESTIONS on this pattern
    //
    //   Q: Why do all SqlCommands in this method share the same 'conn' and 'tx'?
    //   Transactions are strictly bound to a single physical connection. For the operations to be considered part of the same atomic unit of work, they must execute on that exact connection instance and explicitly reference the transaction object.
    //
    //   Q: If INSERT into OrderLines succeeds but UPDATE Products fails, what happens to the OrderLines rows that were already inserted?
    //   The catch block executes tx.RollbackAsync(). This undoes ALL operations from the start of the transaction, meaning the OrderLines inserts (and the Orders insert) are completely reverted as if they never happened.
    //
    //   Q: Why is it a bad idea to make HTTP calls or file I/O inside this try block?
    //   Database locks are held for the entire duration of the transaction. External network/file calls can take seconds or hang entirely. Holding database locks while waiting for a network response will block other queries, destroy database performance, and cause deadlocks/timeouts.
    //
    //   Q: What does GETDATE() do in the SQL? Do you need to pass it as a parameter?
    //   GETDATE() is a built-in SQL Server function that returns the current database server timestamp. You do not need to pass it as a parameter; SQL Server evaluates and injects it internally when the query runs.
}
 
 
// ─────────────────────────────────────────────────────────────
// PART 62 — EXAM SIMULATION: OOP + Records + NotificationDispatcher
// ─────────────────────────────────────────────────────────────
// From APBD §8 Simulation B — demonstrates abstract records, OCP,
// Task.WhenAll, and the Dispatcher pattern together.
 
// static class Part62_ExamSimNotifications
// {
//     // SCENARIO: Build a NotificationSystem.
//     //
//     // Requirements:
//     //   - abstract record Notification(string Subject, string Body)
//     //     with auto-property DateTime CreatedAt = DateTime.Now
//     //   - Subclasses (positional records):
//     //       EmailNotification(string To, string? Cc, string Subject, string Body)
//     //       SmsNotification(string PhoneNumber, string Subject, string Body)
//     //       PushNotification(string DeviceToken, string AppName, string Subject, string Body)
//     //   - Interface ISender with: Task SendAsync(Notification n)
//     //   - Implementations: EmailSender, SmsSender, PushSender
//     //       (each uses 'is' type check to print appropriate info)
//     //   - Class NotificationDispatcher(List<ISender> senders)
//     //       with: Task DispatchAsync(Notification n)
//     //       → dispatches to ALL senders in PARALLEL using Task.WhenAll
//     //   - OCP goal: adding a new Notification type or a new Sender
//     //               NEVER requires changing NotificationDispatcher
//
//     // STEP 1: abstract base record
//     // public abstract record Notification(string Subject, string Body)
//     // {
//     //     // TODO: public DateTime CreatedAt { get; } = DateTime.Now;
//     // }
//
//     // STEP 2: concrete subrecords
//     // TODO: public record EmailNotification(string To, string? Cc,
//     //                                       string Subject, string Body)
//     //           : Notification(Subject, Body);
//     // TODO: public record SmsNotification(string PhoneNumber,
//     //                                     string Subject, string Body)
//     //           : Notification(Subject, Body);
//     // TODO: public record PushNotification(string DeviceToken, string AppName,
//     //                                      string Subject, string Body)
//     //           : Notification(Subject, Body);
//
//     // STEP 3: ISender + implementations
//     // public interface ISender
//     // {
//     //     Task SendAsync(Notification n);
//     // }
//     //
//     // public class EmailSender : ISender
//     // {
//     //     public Task SendAsync(Notification n)
//     //     {
//     //         // TODO: if (n is EmailNotification e)
//     //         //           Console.WriteLine($"Email to {e.To}: {n.Subject}");
//     //         return Task.CompletedTask;
//     //     }
//     // }
//     //
//     // public class SmsSender : ISender { /* TODO: similar pattern for SmsNotification */ }
//     // public class PushSender : ISender { /* TODO: similar pattern for PushNotification */ }
//
//     // STEP 4: Dispatcher — OCP-compliant, never changes
//     // public class NotificationDispatcher
//     // {
//     //     private readonly List<ISender> _senders;
//     //     public NotificationDispatcher(List<ISender> senders) => _senders = senders;
//     //
//     //     public async Task DispatchAsync(Notification n)
//     //         // TODO: await Task.WhenAll(_senders.Select(s => s.SendAsync(n)));
//     //         // EXAM: Task.WhenAll fires all senders CONCURRENTLY (not one by one)
//     // }
//
//     // STEP 5: Composition root + test calls
//     // public static async Task Demo()
//     // {
//     //     var dispatcher = new NotificationDispatcher(new List<ISender>
//     //     {
//     //         new EmailSender(), new SmsSender(), new PushSender()
//     //     });
//     //     await dispatcher.DispatchAsync(
//     //         new EmailNotification("anna@uni.pl", null, "Test", "Hello"));
//     //     await dispatcher.DispatchAsync(
//     //         new SmsNotification("+48123456789", "Alert", "Low balance"));
//     //     await dispatcher.DispatchAsync(
//     //         new PushNotification("device-abc", "MyApp", "Promo", "50% off today!"));
//     // }
//
//     // TASK 62-B: EXAM QUESTIONS on this design:
//     //
//     //   Q: Which SOLID principle does NotificationDispatcher demonstrate?
//     //   TODO: ... (OCP — open for extension via new senders/notifications, closed for modification)
//     //
//     //   Q: What pattern is ISender + List<ISender> + loop/WhenAll?
//     //   TODO: ... (Strategy pattern — algorithms swappable at runtime)
//     //
//     //   Q: Why Task.WhenAll instead of foreach + await s.SendAsync()?
//     //   TODO: ... (WhenAll fires all concurrently; sequential foreach would wait for each
//     //             sender to finish before starting the next)
//     //
//     //   Q: If PushSender is added to the list, what code in NotificationDispatcher changes?
//     //   TODO: ... (NOTHING — that is the point of OCP)
// }