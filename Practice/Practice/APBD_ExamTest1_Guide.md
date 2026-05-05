# APBD Example Test 1 — Complete Solution Guide + 3 Practice Tests
**Polish-Japanese Academy of IT — Kolokwium Preparation**

---

## What this file covers

1. **The exact exam** — database schema, both endpoints, scoring rules
2. **100% solution** — every file, every class, every method with full code
3. **Why each choice earns/loses points** — examiner logic explained
4. **3 similar practice tests** — same structure, different domain, to build muscle memory

---

---

# PART 1 — EXAM WALKTHROUGH: Example Test 1

## The exam task (summary)

**Domain:** Movie rental shop  
**Total points:** 30 (12 + 12 + 6 for architecture/naming)  
**Time:** ~90 minutes

### Database schema

```
Customer          Movie
──────────        ──────────────────
customer_id PK    movie_id PK
first_name        title
last_name         release_date
                  price_per_day

Rental                    Rental_Item
──────────────────────    ────────────────────────
rental_id PK              rental_id PK FK→Rental
rental_date               movie_id  PK FK→Movie
return_date (nullable)    price_at_rental
customer_id FK→Customer
status_id   FK→Status

Status
──────────
status_id PK
name
```

### Endpoint 1 — GET customer rentals (12 pts)
```
GET api/customers/{id}/rentals
→ 200 with customer + nested rentals + nested movies
→ 404 if customer not found
```

### Endpoint 2 — POST new rental (12 pts)
```
POST api/customers/{id}/rentals
Body: { rentalDate, movies: [ { title, rentalPrice } ] }
→ 201 on success (all inside ONE transaction)
→ 404 if customer or any movie not found
→ All operations in a single transaction
```

### Scoring breakdown (memorise this)

| Mistake | Penalty |
|---|---|
| Bad naming / unreadable code | up to -4 |
| Wrong HTTP status codes | up to -5 |
| No separate repository layer | up to **-10** |
| No dependency injection | up to -5 |
| Not async where required | up to **-10** |
| Missing `.gitignore` or committing `bin/obj` | **-50%** |
| Code doesn't compile | **0 points** |
| Not on GitHub | **0 points** |

> The two biggest traps are **no repository layer (-10)** and **not async (-10)**. These alone can fail you.

---

## Solution folder structure

```
Kolokwium/
├── .gitignore                          ← REQUIRED — must include bin/ obj/
├── Kolokwium.sln
└── Kolokwium/
    ├── Kolokwium.csproj
    ├── appsettings.json
    ├── Program.cs
    ├── Controllers/
    │   └── CustomersController.cs
    ├── Models/
    │   ├── Customer.cs
    │   ├── Rental.cs
    │   ├── RentalItem.cs
    │   └── Movie.cs
    ├── DTOs/
    │   ├── CustomerRentalsDto.cs
    │   ├── RentalDto.cs
    │   ├── MovieInRentalDto.cs
    │   ├── AddRentalRequestDto.cs
    │   └── AddRentalMovieDto.cs
    └── Repositories/
        ├── ICustomerRepository.cs
        └── CustomerRepository.cs
```

---

## Every file — full content

### `.gitignore`

```gitignore
bin/
obj/
*.user
.vs/
*.suo
appsettings.Development.json
```

> **WHY:** Committing `bin/` and `obj/` costs you **-50%** of your total score. Always add `.gitignore` first, before your first commit.

---

### `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=RentalDb;User Id=sa;Password=YourPassword123;TrustServerCertificate=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

> **WHY:** Connection string lives in `appsettings.json`, not hardcoded in the repository class. The repository reads it via `IConfiguration` injection.

---

### `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

// Register controllers
builder.Services.AddControllers();

// Register repository with DI — Scoped = one per HTTP request
// This is REQUIRED for full marks on "dependency injection" criterion
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
```

> **WHY `AddScoped`:** Repositories use `SqlConnection` — one per request is correct.  
> **WHY NOT `new CustomerRepository()`:** Hardcoding `new` in the controller means no DI → up to -5 pts.

---

### `Models/Customer.cs`

```csharp
namespace Kolokwium.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
```

### `Models/Rental.cs`

```csharp
namespace Kolokwium.Models;

public class Rental
{
    public int RentalId { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime? ReturnDate { get; set; }   // nullable — not yet returned
    public int CustomerId { get; set; }
    public int StatusId { get; set; }
}
```

### `Models/RentalItem.cs`

```csharp
namespace Kolokwium.Models;

public class RentalItem
{
    public int RentalId { get; set; }
    public int MovieId { get; set; }
    public decimal PriceAtRental { get; set; }
}
```

### `Models/Movie.cs`

```csharp
namespace Kolokwium.Models;

public class Movie
{
    public int MovieId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public decimal PricePerDay { get; set; }
}
```

---

### `DTOs/CustomerRentalsDto.cs`

```csharp
namespace Kolokwium.DTOs;

// This is what GET returns at the top level
public class CustomerRentalsDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<RentalDto> Rentals { get; set; } = new();
}
```

> **WHY a separate DTO:** Never return raw model objects from a controller. The DTO controls exactly what JSON fields appear and their names. The exam JSON sample shows `firstName` (camelCase) — that is handled automatically by `System.Text.Json`.

### `DTOs/RentalDto.cs`

```csharp
namespace Kolokwium.DTOs;

public class RentalDto
{
    public int Id { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime? ReturnDate { get; set; }   // null means not yet returned
    public string Status { get; set; } = string.Empty;
    public List<MovieInRentalDto> Movies { get; set; } = new();
}
```

### `DTOs/MovieInRentalDto.cs`

```csharp
namespace Kolokwium.DTOs;

public class MovieInRentalDto
{
    public string Title { get; set; } = string.Empty;
    public decimal PriceAtRental { get; set; }
}
```

### `DTOs/AddRentalRequestDto.cs`

```csharp
namespace Kolokwium.DTOs;

// This is the POST request body
public class AddRentalRequestDto
{
    public DateTime RentalDate { get; set; }
    public List<AddRentalMovieDto> Movies { get; set; } = new();
}
```

### `DTOs/AddRentalMovieDto.cs`

```csharp
namespace Kolokwium.DTOs;

public class AddRentalMovieDto
{
    public string Title { get; set; } = string.Empty;
    public decimal RentalPrice { get; set; }
}
```

---

### `Repositories/ICustomerRepository.cs`

```csharp
namespace Kolokwium.Repositories;

using Kolokwium.DTOs;

public interface ICustomerRepository
{
    // Returns null if customer not found
    Task<CustomerRentalsDto?> GetCustomerWithRentalsAsync(int customerId);

    // Returns new rental_id on success; throws on validation failure
    Task<int> AddRentalAsync(int customerId, AddRentalRequestDto dto);
}
```

> **WHY an interface:** The controller depends on `ICustomerRepository` (abstraction), not `CustomerRepository` (implementation). This is **DIP** and enables DI. Without this interface you lose up to -5 pts for no DI and up to -10 pts for no separation.

---

### `Repositories/CustomerRepository.cs`

This is the most important file. Read every comment.

```csharp
namespace Kolokwium.Repositories;

using Kolokwium.DTOs;
using Microsoft.Data.SqlClient;

public class CustomerRepository : ICustomerRepository
{
    private readonly string _connectionString;

    // IConfiguration is injected — connection string is never hardcoded
    public CustomerRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    // ─────────────────────────────────────────────────────────────
    // GET: return customer + all their rentals + movies per rental
    // ─────────────────────────────────────────────────────────────
    public async Task<CustomerRentalsDto?> GetCustomerWithRentalsAsync(int customerId)
    {
        // We build the result in memory using a dictionary keyed by rental_id
        // so we can append movies to the right rental as we read rows.
        CustomerRentalsDto? result = null;
        var rentalsDict = new Dictionary<int, RentalDto>();

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // Single query using JOINs — more efficient than N+1 separate queries.
        // LEFT JOIN on Rental so customers with zero rentals still appear.
        // LEFT JOIN on Rental_Item so rentals with no items still appear.
        const string sql = """
            SELECT
                c.customer_id,
                c.first_name,
                c.last_name,
                r.rental_id,
                r.rental_date,
                r.return_date,
                s.name        AS status_name,
                ri.movie_id,
                m.title,
                ri.price_at_rental
            FROM Customer c
            LEFT JOIN Rental r      ON r.customer_id = c.customer_id
            LEFT JOIN Status s      ON s.status_id   = r.status_id
            LEFT JOIN Rental_Item ri ON ri.rental_id  = r.rental_id
            LEFT JOIN Movie m       ON m.movie_id     = ri.movie_id
            WHERE c.customer_id = @CustomerId
            ORDER BY r.rental_id, m.title
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CustomerId", customerId);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            // First row — build the customer wrapper
            if (result is null)
            {
                result = new CustomerRentalsDto
                {
                    FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                    LastName  = reader.GetString(reader.GetOrdinal("last_name")),
                };
            }

            // rental_id may be NULL if customer has no rentals (LEFT JOIN)
            int rentalIdOrdinal = reader.GetOrdinal("rental_id");
            if (reader.IsDBNull(rentalIdOrdinal))
                continue;   // customer exists but has no rentals

            int rentalId = reader.GetInt32(rentalIdOrdinal);

            // Add rental to dict if we haven't seen it yet
            if (!rentalsDict.TryGetValue(rentalId, out var rentalDto))
            {
                rentalDto = new RentalDto
                {
                    Id         = rentalId,
                    RentalDate = reader.GetDateTime(reader.GetOrdinal("rental_date")),
                    ReturnDate = reader.IsDBNull(reader.GetOrdinal("return_date"))
                                    ? null
                                    : reader.GetDateTime(reader.GetOrdinal("return_date")),
                    Status = reader.GetString(reader.GetOrdinal("status_name")),
                };
                rentalsDict[rentalId] = rentalDto;
                result.Rentals.Add(rentalDto);
            }

            // Add movie to the rental if present (movie columns can be NULL)
            int movieIdOrdinal = reader.GetOrdinal("movie_id");
            if (!reader.IsDBNull(movieIdOrdinal))
            {
                rentalDto.Movies.Add(new MovieInRentalDto
                {
                    Title         = reader.GetString(reader.GetOrdinal("title")),
                    PriceAtRental = reader.GetDecimal(reader.GetOrdinal("price_at_rental")),
                });
            }
        }

        // result is null only if no row was returned — customer does not exist
        return result;
    }

    // ─────────────────────────────────────────────────────────────
    // POST: add a rental + its items, all in ONE transaction
    // ─────────────────────────────────────────────────────────────
    public async Task<int> AddRentalAsync(int customerId, AddRentalRequestDto dto)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // Begin transaction — ALL inserts must be atomic
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();

        try
        {
            // ── Step 1: Verify customer exists ──────────────────
            const string checkCustomer = "SELECT COUNT(1) FROM Customer WHERE customer_id = @Id";
            await using var checkCmd = new SqlCommand(checkCustomer, connection, transaction);
            checkCmd.Parameters.AddWithValue("@Id", customerId);
            int customerCount = (int)(await checkCmd.ExecuteScalarAsync())!;
            if (customerCount == 0)
                throw new KeyNotFoundException($"Customer {customerId} not found.");

            // ── Step 2: Resolve movie titles → movie_ids ────────
            // We look up each movie by title and collect their IDs.
            var movieIds = new List<(int Id, decimal Price)>();

            foreach (var movieDto in dto.Movies)
            {
                const string findMovie = "SELECT movie_id FROM Movie WHERE title = @Title";
                await using var movieCmd = new SqlCommand(findMovie, connection, transaction);
                movieCmd.Parameters.AddWithValue("@Title", movieDto.Title);

                var movieResult = await movieCmd.ExecuteScalarAsync();
                if (movieResult is null)
                    throw new KeyNotFoundException($"Movie '{movieDto.Title}' not found.");

                movieIds.Add(((int)movieResult, movieDto.RentalPrice));
            }

            // ── Step 3: Get default status_id for 'Rented' ─────
            const string getStatus = "SELECT status_id FROM Status WHERE name = 'Rented'";
            await using var statusCmd = new SqlCommand(getStatus, connection, transaction);
            int statusId = (int)(await statusCmd.ExecuteScalarAsync())!;

            // ── Step 4: Insert into Rental ──────────────────────
            const string insertRental = """
                INSERT INTO Rental (rental_date, return_date, customer_id, status_id)
                VALUES (@RentalDate, NULL, @CustomerId, @StatusId);
                SELECT SCOPE_IDENTITY();
                """;
            await using var rentalCmd = new SqlCommand(insertRental, connection, transaction);
            rentalCmd.Parameters.AddWithValue("@RentalDate", dto.RentalDate);
            rentalCmd.Parameters.AddWithValue("@CustomerId", customerId);
            rentalCmd.Parameters.AddWithValue("@StatusId", statusId);

            int newRentalId = Convert.ToInt32(await rentalCmd.ExecuteScalarAsync());

            // ── Step 5: Insert one Rental_Item per movie ────────
            foreach (var (movieId, price) in movieIds)
            {
                const string insertItem = """
                    INSERT INTO Rental_Item (rental_id, movie_id, price_at_rental)
                    VALUES (@RentalId, @MovieId, @Price)
                    """;
                await using var itemCmd = new SqlCommand(insertItem, connection, transaction);
                itemCmd.Parameters.AddWithValue("@RentalId", newRentalId);
                itemCmd.Parameters.AddWithValue("@MovieId", movieId);
                itemCmd.Parameters.AddWithValue("@Price", price);
                await itemCmd.ExecuteNonQueryAsync();
            }

            // ── Step 6: Commit — only if ALL steps succeeded ────
            await transaction.CommitAsync();
            return newRentalId;
        }
        catch
        {
            // Rollback undoes ALL inserts if anything failed
            await transaction.RollbackAsync();
            throw;   // rethrow so controller can return the right status code
        }
    }
}
```

---

### `Controllers/CustomersController.cs`

```csharp
namespace Kolokwium.Controllers;

using Kolokwium.DTOs;
using Kolokwium.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _repository;

    // Constructor injection — DI container provides the repository
    public CustomersController(ICustomerRepository repository)
    {
        _repository = repository;
    }

    // ── GET api/customers/{id}/rentals ──────────────────────────
    [HttpGet("{id:int}/rentals")]
    public async Task<IActionResult> GetCustomerRentals(int id)
    {
        var result = await _repository.GetCustomerWithRentalsAsync(id);

        // null means no customer row was returned → 404
        if (result is null)
            return NotFound(new { message = $"Customer with ID {id} was not found." });

        return Ok(result);
    }

    // ── POST api/customers/{id}/rentals ─────────────────────────
    [HttpPost("{id:int}/rentals")]
    public async Task<IActionResult> AddRental(int id, [FromBody] AddRentalRequestDto dto)
    {
        try
        {
            int newRentalId = await _repository.AddRentalAsync(id, dto);

            // 201 Created — standard response for a successful POST
            return Created(
                $"api/customers/{id}/rentals",
                new { rentalId = newRentalId });
        }
        catch (KeyNotFoundException ex)
        {
            // Customer or movie not found → 404
            return NotFound(new { message = ex.Message });
        }
    }
}
```

---

## Line-by-line explanation: why each decision earns marks

### Repository pattern (worth up to 10 pts)

The exam explicitly deducts **up to -10 pts** for not separating database communication into a distinct layer.

```
Controller  →  ICustomerRepository  →  CustomerRepository  →  SQL Server
(HTTP layer)    (interface/contract)    (ADO.NET implementation)
```

- The controller never touches `SqlConnection` or `SqlCommand` directly.
- If you swap SQL Server for PostgreSQL tomorrow, you only change `CustomerRepository`. The controller never changes.

### Async everywhere (worth up to 10 pts)

Every method that does I/O (database, network, disk) must be `async`:

```csharp
// WRONG — blocks the thread, loses up to -10 pts
public CustomerRentalsDto? GetCustomerWithRentals(int id) { ... }

// CORRECT
public async Task<CustomerRentalsDto?> GetCustomerWithRentalsAsync(int id) { ... }
```

Async I/O frees the thread to serve other requests while waiting for SQL Server. On a loaded server this is the difference between 100 concurrent users and 10.

### The LEFT JOIN strategy (GET endpoint)

The naive approach is three separate queries:
1. `SELECT * FROM Customer WHERE ...`
2. `SELECT * FROM Rental WHERE customer_id = @id`
3. For each rental: `SELECT * FROM Rental_Item WHERE rental_id = @id`

This is the **N+1 problem** — if a customer has 10 rentals, you make 12 queries. The single JOIN query makes 1.

The trick is using a `Dictionary<int, RentalDto>` to deduplicate:
- Each row from the JOIN has one (rental_id, movie). 
- The dictionary ensures each rental appears once.
- Each new movie row just calls `.Add()` on the right rental's movies list.

### Transaction in POST (12 pts endpoint)

The transaction wraps ALL operations:
```
BeginTransaction
  ├── Check customer exists
  ├── For each movie: look up movie_id
  ├── Get status_id for 'Rented'
  ├── INSERT INTO Rental
  └── For each movie: INSERT INTO Rental_Item
CommitAsync   ← only if everything above succeeded
```

If movie lookup throws `KeyNotFoundException`, the catch block calls `RollbackAsync()` — the `Rental` insert (which already happened) is undone. The database stays clean.

### `SCOPE_IDENTITY()` vs `@@IDENTITY`

```sql
INSERT INTO Rental (...) VALUES (...);
SELECT SCOPE_IDENTITY();
```

`SCOPE_IDENTITY()` returns the identity generated in the current scope (your INSERT). `@@IDENTITY` returns the last identity generated anywhere — including by a trigger that fires on another table. Always use `SCOPE_IDENTITY()`.

### Status codes

| Situation | Code | Helper |
|---|---|---|
| Customer found, data returned | 200 | `Ok(result)` |
| Customer or movie not found | 404 | `NotFound(new { message })` |
| Rental created successfully | 201 | `Created(uri, body)` |
| Request body is malformed | 400 | auto by `[ApiController]` |

---

---

# PART 2 — PRACTICE TEST A: Library Book Loans

**Same structure as the real exam. Different domain.**  
Time yourself: 90 minutes.

## Database schema

```sql
CREATE TABLE Member (
    member_id   INT NOT NULL IDENTITY(1,1),
    first_name  NVARCHAR(100) NOT NULL,
    last_name   NVARCHAR(100) NOT NULL,
    email       NVARCHAR(200) NOT NULL,
    CONSTRAINT Member_pk PRIMARY KEY (member_id)
);

CREATE TABLE Book (
    book_id         INT NOT NULL IDENTITY(1,1),
    title           NVARCHAR(300) NOT NULL,
    author          NVARCHAR(200) NOT NULL,
    isbn            NVARCHAR(20)  NOT NULL,
    fee_per_day     DECIMAL(10,2) NOT NULL,
    CONSTRAINT Book_pk PRIMARY KEY (book_id)
);

CREATE TABLE Loan_Status (
    status_id   INT NOT NULL IDENTITY(1,1),
    name        NVARCHAR(100) NOT NULL,
    CONSTRAINT Loan_Status_pk PRIMARY KEY (status_id)
);

CREATE TABLE Loan (
    loan_id     INT NOT NULL IDENTITY(1,1),
    loan_date   DATETIME NOT NULL,
    due_date    DATETIME NOT NULL,
    return_date DATETIME NULL,
    member_id   INT NOT NULL REFERENCES Member(member_id),
    status_id   INT NOT NULL REFERENCES Loan_Status(status_id),
    CONSTRAINT Loan_pk PRIMARY KEY (loan_id)
);

CREATE TABLE Loan_Book (
    loan_id         INT NOT NULL REFERENCES Loan(loan_id),
    book_id         INT NOT NULL REFERENCES Book(book_id),
    fee_at_loan     DECIMAL(10,2) NOT NULL,
    CONSTRAINT Loan_Book_pk PRIMARY KEY (loan_id, book_id)
);

INSERT INTO Loan_Status (name) VALUES ('Active'), ('Returned'), ('Overdue');
INSERT INTO Member (first_name, last_name, email) VALUES
    ('Maria', 'Kowalska', 'maria@lib.pl'),
    ('Piotr', 'Nowak',    'piotr@lib.pl');
INSERT INTO Book (title, author, isbn, fee_per_day) VALUES
    ('Clean Code',    'Robert C. Martin', '978-0132350884', 1.50),
    ('The Pragmatic Programmer', 'David Thomas', '978-0135957059', 1.75),
    ('Refactoring',   'Martin Fowler',    '978-0134757599', 2.00);
INSERT INTO Loan (loan_date, due_date, return_date, member_id, status_id) VALUES
    ('2026-04-01', '2026-04-15', '2026-04-14', 1, 2),
    ('2026-04-20', '2026-05-04', NULL,          2, 1);
INSERT INTO Loan_Book (loan_id, book_id, fee_at_loan) VALUES
    (1, 1, 1.50), (1, 2, 1.75), (2, 3, 2.00);
```

## Endpoints

### GET `api/members/{id}/loans`

Returns member info + all their loans + books per loan.

**Expected response (200):**
```json
{
    "firstName": "Maria",
    "lastName": "Kowalska",
    "email": "maria@lib.pl",
    "loans": [
        {
            "id": 1,
            "loanDate": "2026-04-01T00:00:00",
            "dueDate": "2026-04-15T00:00:00",
            "returnDate": "2026-04-14T00:00:00",
            "status": "Returned",
            "books": [
                { "title": "Clean Code", "author": "Robert C. Martin", "feeAtLoan": 1.50 },
                { "title": "The Pragmatic Programmer", "author": "David Thomas", "feeAtLoan": 1.75 }
            ]
        }
    ]
}
```
**If member not found:** 404

### POST `api/members/{id}/loans`

Add a new loan. Check member exists. Check each book exists (by ISBN). All in a transaction.

**Request body:**
```json
{
    "loanDate": "2026-05-05",
    "dueDate": "2026-05-19",
    "books": [
        { "isbn": "978-0132350884", "feeAtLoan": 1.50 },
        { "isbn": "978-0134757599", "feeAtLoan": 2.00 }
    ]
}
```
**On success:** 201 Created  
**If member or book not found:** 404

---

## Solution folder structure

```
LibraryApi/
├── .gitignore
├── LibraryApi.sln
└── LibraryApi/
    ├── LibraryApi.csproj
    ├── appsettings.json
    ├── Program.cs
    ├── Controllers/
    │   └── MembersController.cs
    ├── DTOs/
    │   ├── MemberLoansDto.cs
    │   ├── LoanDto.cs
    │   ├── BookInLoanDto.cs
    │   ├── AddLoanRequestDto.cs
    │   └── AddLoanBookDto.cs
    └── Repositories/
        ├── IMemberRepository.cs
        └── MemberRepository.cs
```

## Full solution

### `Program.cs`
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
```

### `DTOs/MemberLoansDto.cs`
```csharp
namespace LibraryApi.DTOs;
public class MemberLoansDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName  { get; set; } = string.Empty;
    public string Email     { get; set; } = string.Empty;
    public List<LoanDto> Loans { get; set; } = new();
}
```

### `DTOs/LoanDto.cs`
```csharp
namespace LibraryApi.DTOs;
public class LoanDto
{
    public int       Id         { get; set; }
    public DateTime  LoanDate   { get; set; }
    public DateTime  DueDate    { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string    Status     { get; set; } = string.Empty;
    public List<BookInLoanDto> Books { get; set; } = new();
}
```

### `DTOs/BookInLoanDto.cs`
```csharp
namespace LibraryApi.DTOs;
public class BookInLoanDto
{
    public string  Title     { get; set; } = string.Empty;
    public string  Author    { get; set; } = string.Empty;
    public decimal FeeAtLoan { get; set; }
}
```

### `DTOs/AddLoanRequestDto.cs`
```csharp
namespace LibraryApi.DTOs;
public class AddLoanRequestDto
{
    public DateTime LoanDate { get; set; }
    public DateTime DueDate  { get; set; }
    public List<AddLoanBookDto> Books { get; set; } = new();
}
```

### `DTOs/AddLoanBookDto.cs`
```csharp
namespace LibraryApi.DTOs;
public class AddLoanBookDto
{
    public string  Isbn      { get; set; } = string.Empty;
    public decimal FeeAtLoan { get; set; }
}
```

### `Repositories/IMemberRepository.cs`
```csharp
namespace LibraryApi.Repositories;
using LibraryApi.DTOs;

public interface IMemberRepository
{
    Task<MemberLoansDto?> GetMemberWithLoansAsync(int memberId);
    Task<int> AddLoanAsync(int memberId, AddLoanRequestDto dto);
}
```

### `Repositories/MemberRepository.cs`
```csharp
namespace LibraryApi.Repositories;
using LibraryApi.DTOs;
using Microsoft.Data.SqlClient;

public class MemberRepository : IMemberRepository
{
    private readonly string _connectionString;

    public MemberRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    public async Task<MemberLoansDto?> GetMemberWithLoansAsync(int memberId)
    {
        MemberLoansDto? result = null;
        var loansDict = new Dictionary<int, LoanDto>();

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        const string sql = """
            SELECT
                m.first_name, m.last_name, m.email,
                l.loan_id, l.loan_date, l.due_date, l.return_date,
                ls.name     AS status_name,
                b.title, b.author, lb.fee_at_loan
            FROM Member m
            LEFT JOIN Loan l         ON l.member_id = m.member_id
            LEFT JOIN Loan_Status ls ON ls.status_id = l.status_id
            LEFT JOIN Loan_Book lb   ON lb.loan_id   = l.loan_id
            LEFT JOIN Book b         ON b.book_id    = lb.book_id
            WHERE m.member_id = @MemberId
            ORDER BY l.loan_id, b.title
            """;

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MemberId", memberId);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            if (result is null)
            {
                result = new MemberLoansDto
                {
                    FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                    LastName  = reader.GetString(reader.GetOrdinal("last_name")),
                    Email     = reader.GetString(reader.GetOrdinal("email")),
                };
            }

            int loanIdOrd = reader.GetOrdinal("loan_id");
            if (reader.IsDBNull(loanIdOrd)) continue;

            int loanId = reader.GetInt32(loanIdOrd);
            if (!loansDict.TryGetValue(loanId, out var loanDto))
            {
                loanDto = new LoanDto
                {
                    Id         = loanId,
                    LoanDate   = reader.GetDateTime(reader.GetOrdinal("loan_date")),
                    DueDate    = reader.GetDateTime(reader.GetOrdinal("due_date")),
                    ReturnDate = reader.IsDBNull(reader.GetOrdinal("return_date"))
                                    ? null
                                    : reader.GetDateTime(reader.GetOrdinal("return_date")),
                    Status = reader.GetString(reader.GetOrdinal("status_name")),
                };
                loansDict[loanId] = loanDto;
                result.Loans.Add(loanDto);
            }

            int titleOrd = reader.GetOrdinal("title");
            if (!reader.IsDBNull(titleOrd))
            {
                loanDto.Books.Add(new BookInLoanDto
                {
                    Title     = reader.GetString(titleOrd),
                    Author    = reader.GetString(reader.GetOrdinal("author")),
                    FeeAtLoan = reader.GetDecimal(reader.GetOrdinal("fee_at_loan")),
                });
            }
        }

        return result;
    }

    public async Task<int> AddLoanAsync(int memberId, AddLoanRequestDto dto)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

        try
        {
            // Verify member
            await using var memberCmd = new SqlCommand(
                "SELECT COUNT(1) FROM Member WHERE member_id = @Id", conn, tx);
            memberCmd.Parameters.AddWithValue("@Id", memberId);
            if ((int)(await memberCmd.ExecuteScalarAsync())! == 0)
                throw new KeyNotFoundException($"Member {memberId} not found.");

            // Resolve ISBNs to book_ids
            var bookIds = new List<(int Id, decimal Fee)>();
            foreach (var b in dto.Books)
            {
                await using var bookCmd = new SqlCommand(
                    "SELECT book_id FROM Book WHERE isbn = @Isbn", conn, tx);
                bookCmd.Parameters.AddWithValue("@Isbn", b.Isbn);
                var r = await bookCmd.ExecuteScalarAsync();
                if (r is null) throw new KeyNotFoundException($"Book ISBN '{b.Isbn}' not found.");
                bookIds.Add(((int)r, b.FeeAtLoan));
            }

            // Get 'Active' status
            await using var statusCmd = new SqlCommand(
                "SELECT status_id FROM Loan_Status WHERE name = 'Active'", conn, tx);
            int statusId = (int)(await statusCmd.ExecuteScalarAsync())!;

            // Insert Loan
            await using var loanCmd = new SqlCommand("""
                INSERT INTO Loan (loan_date, due_date, return_date, member_id, status_id)
                VALUES (@LoanDate, @DueDate, NULL, @MemberId, @StatusId);
                SELECT SCOPE_IDENTITY();
                """, conn, tx);
            loanCmd.Parameters.AddWithValue("@LoanDate",  dto.LoanDate);
            loanCmd.Parameters.AddWithValue("@DueDate",   dto.DueDate);
            loanCmd.Parameters.AddWithValue("@MemberId",  memberId);
            loanCmd.Parameters.AddWithValue("@StatusId",  statusId);
            int newLoanId = Convert.ToInt32(await loanCmd.ExecuteScalarAsync());

            // Insert Loan_Book rows
            foreach (var (bookId, fee) in bookIds)
            {
                await using var itemCmd = new SqlCommand("""
                    INSERT INTO Loan_Book (loan_id, book_id, fee_at_loan)
                    VALUES (@LoanId, @BookId, @Fee)
                    """, conn, tx);
                itemCmd.Parameters.AddWithValue("@LoanId", newLoanId);
                itemCmd.Parameters.AddWithValue("@BookId", bookId);
                itemCmd.Parameters.AddWithValue("@Fee",    fee);
                await itemCmd.ExecuteNonQueryAsync();
            }

            await tx.CommitAsync();
            return newLoanId;
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}
```

### `Controllers/MembersController.cs`
```csharp
namespace LibraryApi.Controllers;
using LibraryApi.DTOs;
using LibraryApi.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly IMemberRepository _repo;

    public MembersController(IMemberRepository repo) => _repo = repo;

    [HttpGet("{id:int}/loans")]
    public async Task<IActionResult> GetMemberLoans(int id)
    {
        var result = await _repo.GetMemberWithLoansAsync(id);
        return result is null
            ? NotFound(new { message = $"Member {id} not found." })
            : Ok(result);
    }

    [HttpPost("{id:int}/loans")]
    public async Task<IActionResult> AddLoan(int id, [FromBody] AddLoanRequestDto dto)
    {
        try
        {
            int loanId = await _repo.AddLoanAsync(id, dto);
            return Created($"api/members/{id}/loans", new { loanId });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
```

---

---

# PART 3 — PRACTICE TEST B: Hotel Room Reservations

## Database schema

```sql
CREATE TABLE Guest (
    guest_id    INT NOT NULL IDENTITY(1,1),
    first_name  NVARCHAR(100) NOT NULL,
    last_name   NVARCHAR(100) NOT NULL,
    passport_no NVARCHAR(50)  NOT NULL,
    CONSTRAINT Guest_pk PRIMARY KEY (guest_id)
);

CREATE TABLE Room (
    room_id     INT NOT NULL IDENTITY(1,1),
    room_number NVARCHAR(10)  NOT NULL,
    type        NVARCHAR(50)  NOT NULL,
    price_per_night DECIMAL(10,2) NOT NULL,
    CONSTRAINT Room_pk PRIMARY KEY (room_id)
);

CREATE TABLE Reservation_Status (
    status_id   INT NOT NULL IDENTITY(1,1),
    name        NVARCHAR(100) NOT NULL,
    CONSTRAINT Reservation_Status_pk PRIMARY KEY (status_id)
);

CREATE TABLE Reservation (
    reservation_id  INT NOT NULL IDENTITY(1,1),
    check_in        DATETIME NOT NULL,
    check_out       DATETIME NULL,
    guest_id        INT NOT NULL REFERENCES Guest(guest_id),
    status_id       INT NOT NULL REFERENCES Reservation_Status(status_id),
    CONSTRAINT Reservation_pk PRIMARY KEY (reservation_id)
);

CREATE TABLE Reservation_Room (
    reservation_id  INT NOT NULL REFERENCES Reservation(reservation_id),
    room_id         INT NOT NULL REFERENCES Room(room_id),
    price_at_booking DECIMAL(10,2) NOT NULL,
    CONSTRAINT Reservation_Room_pk PRIMARY KEY (reservation_id, room_id)
);

INSERT INTO Reservation_Status (name) VALUES ('Confirmed'), ('Checked Out'), ('Cancelled');
INSERT INTO Guest (first_name, last_name, passport_no) VALUES
    ('Anna',  'Wiśniewska', 'ABC123456'),
    ('Tomasz','Górski',     'XYZ789012');
INSERT INTO Room (room_number, type, price_per_night) VALUES
    ('101', 'Single', 120.00),
    ('202', 'Double', 180.00),
    ('303', 'Suite',  350.00);
INSERT INTO Reservation (check_in, check_out, guest_id, status_id) VALUES
    ('2026-03-10', '2026-03-14', 1, 2),
    ('2026-04-25', NULL,         2, 1);
INSERT INTO Reservation_Room (reservation_id, room_id, price_at_booking) VALUES
    (1, 1, 120.00), (1, 2, 180.00), (2, 3, 350.00);
```

## Endpoints

### GET `api/guests/{id}/reservations`

**200 response:**
```json
{
    "firstName": "Anna",
    "lastName": "Wiśniewska",
    "passportNo": "ABC123456",
    "reservations": [
        {
            "id": 1,
            "checkIn": "2026-03-10T00:00:00",
            "checkOut": "2026-03-14T00:00:00",
            "status": "Checked Out",
            "rooms": [
                { "roomNumber": "101", "type": "Single", "priceAtBooking": 120.00 },
                { "roomNumber": "202", "type": "Double", "priceAtBooking": 180.00 }
            ]
        }
    ]
}
```

**404** if guest not found.

### POST `api/guests/{id}/reservations`

**Request body:**
```json
{
    "checkIn": "2026-06-01",
    "rooms": [
        { "roomNumber": "101", "priceAtBooking": 120.00 },
        { "roomNumber": "303", "priceAtBooking": 350.00 }
    ]
}
```

Check guest exists. Check each room exists by `room_number`. Insert in one transaction. Return 201. Return 404 if guest or room not found.

---

## Solution folder structure

```
HotelApi/
├── .gitignore
├── HotelApi.sln
└── HotelApi/
    ├── HotelApi.csproj
    ├── appsettings.json
    ├── Program.cs
    ├── Controllers/
    │   └── GuestsController.cs
    ├── DTOs/
    │   ├── GuestReservationsDto.cs
    │   ├── ReservationDto.cs
    │   ├── RoomInReservationDto.cs
    │   ├── AddReservationRequestDto.cs
    │   └── AddReservationRoomDto.cs
    └── Repositories/
        ├── IGuestRepository.cs
        └── GuestRepository.cs
```

## Key differences from the original exam (note these carefully)

| | Exam (Movie Rental) | This Test (Hotel) |
|---|---|---|
| Lookup movie by | `title` | `room_number` |
| Nullable end date | `return_date` | `check_out` |
| Line-item table | `Rental_Item` | `Reservation_Room` |
| Price field name | `price_at_rental` | `price_at_booking` |
| Status seed value | `'Rented'` | `'Confirmed'` |

The **code pattern is identical**. Only the SQL table names and column names change.

### `Controllers/GuestsController.cs`
```csharp
namespace HotelApi.Controllers;
using HotelApi.DTOs;
using HotelApi.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/guests")]
public class GuestsController : ControllerBase
{
    private readonly IGuestRepository _repo;
    public GuestsController(IGuestRepository repo) => _repo = repo;

    [HttpGet("{id:int}/reservations")]
    public async Task<IActionResult> GetGuestReservations(int id)
    {
        var result = await _repo.GetGuestWithReservationsAsync(id);
        return result is null
            ? NotFound(new { message = $"Guest {id} not found." })
            : Ok(result);
    }

    [HttpPost("{id:int}/reservations")]
    public async Task<IActionResult> AddReservation(int id, [FromBody] AddReservationRequestDto dto)
    {
        try
        {
            int reservationId = await _repo.AddReservationAsync(id, dto);
            return Created($"api/guests/{id}/reservations", new { reservationId });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
```

### `Repositories/GuestRepository.cs` — GET method SQL

```csharp
const string sql = """
    SELECT
        g.first_name, g.last_name, g.passport_no,
        r.reservation_id, r.check_in, r.check_out,
        rs.name           AS status_name,
        rm.room_number, rm.type,
        rr.price_at_booking
    FROM Guest g
    LEFT JOIN Reservation r         ON r.guest_id      = g.guest_id
    LEFT JOIN Reservation_Status rs ON rs.status_id    = r.status_id
    LEFT JOIN Reservation_Room rr   ON rr.reservation_id = r.reservation_id
    LEFT JOIN Room rm               ON rm.room_id       = rr.room_id
    WHERE g.guest_id = @GuestId
    ORDER BY r.reservation_id, rm.room_number
    """;
```

### `Repositories/GuestRepository.cs` — POST transaction (lookup by room_number)

```csharp
// Look up room by room_number instead of title
await using var roomCmd = new SqlCommand(
    "SELECT room_id FROM Room WHERE room_number = @RoomNumber", conn, tx);
roomCmd.Parameters.AddWithValue("@RoomNumber", r.RoomNumber);
var result = await roomCmd.ExecuteScalarAsync();
if (result is null) throw new KeyNotFoundException($"Room '{r.RoomNumber}' not found.");
```

---

---

# PART 4 — PRACTICE TEST C: Gym Class Bookings

## Database schema

```sql
CREATE TABLE Client (
    client_id   INT NOT NULL IDENTITY(1,1),
    first_name  NVARCHAR(100) NOT NULL,
    last_name   NVARCHAR(100) NOT NULL,
    phone       NVARCHAR(20)  NOT NULL,
    CONSTRAINT Client_pk PRIMARY KEY (client_id)
);

CREATE TABLE Trainer (
    trainer_id  INT NOT NULL IDENTITY(1,1),
    full_name   NVARCHAR(200) NOT NULL,
    specialty   NVARCHAR(100) NOT NULL,
    CONSTRAINT Trainer_pk PRIMARY KEY (trainer_id)
);

CREATE TABLE Class_Type (
    type_id     INT NOT NULL IDENTITY(1,1),
    name        NVARCHAR(100) NOT NULL,
    duration_min INT NOT NULL,
    price       DECIMAL(10,2) NOT NULL,
    CONSTRAINT Class_Type_pk PRIMARY KEY (type_id)
);

CREATE TABLE Booking_Status (
    status_id   INT NOT NULL IDENTITY(1,1),
    name        NVARCHAR(100) NOT NULL,
    CONSTRAINT Booking_Status_pk PRIMARY KEY (status_id)
);

CREATE TABLE Booking (
    booking_id      INT NOT NULL IDENTITY(1,1),
    booking_date    DATETIME NOT NULL,
    client_id       INT NOT NULL REFERENCES Client(client_id),
    trainer_id      INT NOT NULL REFERENCES Trainer(trainer_id),
    status_id       INT NOT NULL REFERENCES Booking_Status(status_id),
    CONSTRAINT Booking_pk PRIMARY KEY (booking_id)
);

CREATE TABLE Booking_Class (
    booking_id      INT NOT NULL REFERENCES Booking(booking_id),
    type_id         INT NOT NULL REFERENCES Class_Type(type_id),
    price_at_booking DECIMAL(10,2) NOT NULL,
    CONSTRAINT Booking_Class_pk PRIMARY KEY (booking_id, type_id)
);

INSERT INTO Booking_Status (name) VALUES ('Scheduled'), ('Completed'), ('Cancelled');
INSERT INTO Trainer (full_name, specialty) VALUES ('Katarzyna Wróbel', 'Yoga'), ('Marcin Lewandowski', 'CrossFit');
INSERT INTO Client (first_name, last_name, phone) VALUES ('Zofia', 'Adamska', '+48111222333'), ('Rafał', 'Bąk', '+48444555666');
INSERT INTO Class_Type (name, duration_min, price) VALUES ('Yoga Flow', 60, 45.00), ('CrossFit WOD', 45, 50.00), ('Pilates', 55, 40.00);
INSERT INTO Booking (booking_date, client_id, trainer_id, status_id) VALUES
    ('2026-04-10', 1, 1, 2),
    ('2026-04-28', 2, 2, 1);
INSERT INTO Booking_Class (booking_id, type_id, price_at_booking) VALUES (1, 1, 45.00), (2, 2, 50.00);
```

## Endpoints

### GET `api/clients/{id}/bookings`

**200 response:**
```json
{
    "firstName": "Zofia",
    "lastName": "Adamska",
    "phone": "+48111222333",
    "bookings": [
        {
            "id": 1,
            "bookingDate": "2026-04-10T00:00:00",
            "trainerName": "Katarzyna Wróbel",
            "status": "Completed",
            "classes": [
                { "className": "Yoga Flow", "durationMin": 60, "priceAtBooking": 45.00 }
            ]
        }
    ]
}
```

**404** if client not found.

### POST `api/clients/{id}/bookings`

**Request body:**
```json
{
    "bookingDate": "2026-05-10",
    "trainerId": 1,
    "classes": [
        { "className": "Yoga Flow", "priceAtBooking": 45.00 },
        { "className": "Pilates",   "priceAtBooking": 40.00 }
    ]
}
```

Validate: client exists, trainer exists, each class name exists. One transaction. Return 201 or 404.

## What is different here

| | Previous tests | This test |
|---|---|---|
| POST lookup by | `title` or `room_number` | `className` (maps to `Class_Type.name`) |
| Extra FK in header | none | `trainer_id` must also be validated |
| Response extra field | — | `trainerName` in each booking DTO |

### Extra validation for trainer

```csharp
// After checking client, also check trainer
await using var trainerCmd = new SqlCommand(
    "SELECT COUNT(1) FROM Trainer WHERE trainer_id = @TrainerId", conn, tx);
trainerCmd.Parameters.AddWithValue("@TrainerId", dto.TrainerId);
if ((int)(await trainerCmd.ExecuteScalarAsync())! == 0)
    throw new KeyNotFoundException($"Trainer {dto.TrainerId} not found.");
```

### Extra JOIN for trainer name in GET

```sql
SELECT
    c.first_name, c.last_name, c.phone,
    b.booking_id, b.booking_date,
    t.full_name   AS trainer_name,      -- ← extra join
    bs.name       AS status_name,
    ct.name       AS class_name, ct.duration_min,
    bc.price_at_booking
FROM Client c
LEFT JOIN Booking b         ON b.client_id  = c.client_id
LEFT JOIN Trainer t         ON t.trainer_id = b.trainer_id    -- ← extra join
LEFT JOIN Booking_Status bs ON bs.status_id = b.status_id
LEFT JOIN Booking_Class bc  ON bc.booking_id = b.booking_id
LEFT JOIN Class_Type ct     ON ct.type_id    = bc.type_id
WHERE c.client_id = @ClientId
ORDER BY b.booking_id, ct.name
```

### `DTOs/BookingDto.cs`
```csharp
namespace GymApi.DTOs;
public class BookingDto
{
    public int      Id          { get; set; }
    public DateTime BookingDate { get; set; }
    public string   TrainerName { get; set; } = string.Empty;   // ← extra field
    public string   Status      { get; set; } = string.Empty;
    public List<ClassInBookingDto> Classes { get; set; } = new();
}
```

### `DTOs/ClassInBookingDto.cs`
```csharp
namespace GymApi.DTOs;
public class ClassInBookingDto
{
    public string  ClassName       { get; set; } = string.Empty;
    public int     DurationMin     { get; set; }
    public decimal PriceAtBooking  { get; set; }
}
```

---

---

# PART 5 — Exam Day Checklist

Use this 15-minute pre-exam checklist every time.

## Before writing a single line of code (5 min)

```
□ Read BOTH endpoints fully before starting
□ Draw the DB schema on paper — identify PKs, FKs, nullable columns
□ Note: which column do you look up entities by? (title? isbn? room_number?)
□ Note: which column is nullable? (return_date, check_out, etc.)
□ Note: what is the status name to use on POST? ('Rented', 'Active', 'Confirmed'...)
□ Note: does the GET response need extra JOINs? (trainer_name, author, etc.)
```

## Project setup (3 min)

```
□ dotnet new webapi -n MyProject --no-openapi
□ cd MyProject && dotnet add package Microsoft.Data.SqlClient
□ Create .gitignore IMMEDIATELY — before first commit
□ git init && git add .gitignore && git commit -m "init"
□ Delete WeatherForecast files (WeatherForecast.cs, Controllers/WeatherForecastController.cs)
□ Set up appsettings.json with connection string
□ Register controller services in Program.cs
```

## Coding order (70 min)

```
□ 1. Create DTOs (all of them first — 5 min)
□ 2. Create interface (IXxxRepository — 2 min)  
□ 3. Register in Program.cs (AddScoped — 1 min)
□ 4. Implement GET in repository (25 min)
□    - Single JOIN query
□    - Dictionary to deduplicate parent rows
□    - IsDBNull check on nullable columns
□    - Return null if no rows → controller returns 404
□ 5. Implement POST in repository (25 min)
□    - BeginTransactionAsync
□    - Verify all entities exist (throw KeyNotFoundException)
□    - Get status_id for the default status name
□    - INSERT header row → SCOPE_IDENTITY()
□    - INSERT line items in loop
□    - CommitAsync / RollbackAsync in catch
□ 6. Write controller (10 min)
□    - Constructor injection of IXxxRepository
□    - GET: check null → 404 or 200
□    - POST: try/catch KeyNotFoundException → 404, success → 201
□ 7. Final commit and push (2 min)
```

## Final checks (5 min)

```
□ Every method touching DB is async Task<...>
□ Every await has the await keyword
□ Controller never uses new SqlConnection (that is the repo's job)
□ Transaction: BeginTransactionAsync, all cmds have ,transaction, CommitAsync, RollbackAsync
□ SCOPE_IDENTITY() used (not @@IDENTITY)
□ IsDBNull checked on every nullable column before calling GetString/GetDateTime/GetDecimal
□ null parameters passed as: (object?)value ?? DBNull.Value
□ .gitignore committed, no bin/ or obj/ in repo
□ Push to GitHub: git add . && git commit -m "solution" && git push
```

---

## The pattern that never changes

Every APBD test has the same shape. Once you internalize this, the only work is filling in the names:

```
GET /api/{entity}/{id}/{children}

Repository.GetXxxWithChildrenAsync(id):
    1. Single LEFT JOIN query across all tables
    2. Dictionary<int, ParentDto> to deduplicate
    3. while(ReadAsync): build parent once, add children each row
    4. Return null if no rows returned

Controller.GetXxx(id):
    var result = await _repo.GetXxxAsync(id);
    return result is null ? NotFound() : Ok(result);

─────────────────────────────────────────────────────

POST /api/{entity}/{id}/{children}

Repository.AddXxxAsync(id, dto):
    1. Open connection, BeginTransactionAsync
    2. Check parent entity exists (ExecuteScalar COUNT → throw KeyNotFound)
    3. For each child: look up id by name/isbn/number (ExecuteScalar → throw KeyNotFound)
    4. Get default status_id (ExecuteScalar)
    5. INSERT header → SELECT SCOPE_IDENTITY() (ExecuteScalar → newId)
    6. For each child: INSERT line item (ExecuteNonQuery)
    7. CommitAsync / catch → RollbackAsync + throw

Controller.AddXxx(id, dto):
    try { int id = await _repo.AddXxxAsync(id, dto); return Created(...); }
    catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
```

Memorise this pattern. Every test is a variation of it.
