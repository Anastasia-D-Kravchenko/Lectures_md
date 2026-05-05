# APBD — Code Templates Cheatsheet
> Fill in the `YOUR_*` placeholders. Every block is copy-paste ready.

---

## 🗂️ Project Setup (run once)

```bash
dotnet new webapi -n YOUR_PROJECT --no-openapi
cd YOUR_PROJECT
dotnet add package Microsoft.Data.SqlClient

# Create .gitignore BEFORE first commit — committing bin/ or obj/ costs -50% on the exam
echo "bin/\nobj/\n*.user\n.vs/\nappsettings.Development.json" > .gitignore
git init
git add .gitignore
git commit -m "initial commit with gitignore"

# Remove the template files that come with the webapi template
rm WeatherForecast.cs Controllers/WeatherForecastController.cs
```

---

## 📁 Folder Structures

### Structure A — Pure OOP (Project 1 style)
```
YOUR_PROJECT/
├── Models/           ← data shapes (Animal, Dog, Cat)
├── Interfaces/       ← contracts (IAdoptable)
├── Exceptions/       ← custom exceptions (NotFoundException)
├── Services/         ← business logic (ShelterService)
└── Program.cs
```

### Structure B — LINQ / Analytics (Project 2 style)
```
YOUR_PROJECT/
├── Models/           ← record types (Student, Course, Enrollment)
├── Data/             ← SeedData static class (replaces DB)
├── Queries/          ← static query classes (StudentQueries, EnrollmentQueries)
├── Utilities/        ← generic helpers (GenericRepository<T>, CsvExporter)
└── Program.cs
```

### Structure C — In-Memory Web API (Project 4 style)
```
YOUR_PROJECT/
├── Models/           ← entities (Book, Loan)
├── Dtos/             ← request shapes with validation attributes
├── Data/             ← InMemoryStore (static singleton store)
├── Controllers/      ← HTTP layer only
├── Middleware/       ← ExceptionHandlingMiddleware, RequestTimingMiddleware
└── Program.cs
```

### Structure D — Full 3-Layer with SQL (Project 5 / Exam style)
```
YOUR_PROJECT/
├── Models/           ← domain entities (Patient, Doctor)
├── Dtos/             ← request + response shapes
├── Exceptions/       ← NotFoundException, ConflictException
├── Interfaces/       ← IXxxRepository, IXxxService
├── Repositories/     ← SQL only (SqlXxxRepository)
├── Services/         ← business rules (XxxService)
├── Controllers/      ← HTTP only (XxxController)
├── Middleware/       ← ExceptionHandlingMiddleware
└── Program.cs
```

---

## 📄 Templates by File Type

---

### 1. `Models/YourEntity.cs`

```csharp
namespace YOUR_PROJECT.Models;

public class YOUR_ENTITY
{
    public int      Id          { get; set; }
    public string   Name        { get; set; } = string.Empty;
    public decimal  Price       { get; set; }           // non-nullable value type
    public string?  Notes       { get; set; }           // nullable → string?
    public DateTime CreatedAt   { get; set; }
    public DateTime? ReturnedAt { get; set; }           // nullable DateTime
    public bool     IsAvailable { get; set; } = true;  // default value
}
```

**Abstract base class (OOP projects):**
```csharp
public abstract class YOUR_BASE
{
    public int    Id       { get; init; }               // init = set once at creation, frozen after
    public string Name     { get; set; } = string.Empty;
    public bool   IsActive { get; private set; }       // private set = only THIS class writes it

    public abstract string Describe();                 // subclasses MUST implement — no body here

    public void Activate() => IsActive = true;         // the ONLY way to flip IsActive from outside

    public override string ToString()
        => $"{Name} ({GetType().Name})";               // GetType().Name = actual runtime type, e.g. "Dog"
}
```

**Concrete subclass + interface:**
```csharp
// : YOUR_BASE      = inherits everything from base
// , IYOUR_INTERFACE = also promises to implement the interface method
public class YOUR_ENTITY : YOUR_BASE, IYOUR_INTERFACE
{
    public string YOUR_FIELD { get; init; } = string.Empty;

    public override string Describe()
        => $"{Name}: {YOUR_FIELD}";

    // IYourInterface implementation:
    public void YOUR_METHOD(string param)
    {
        Activate();   // call base class method — only way to set IsActive
        Console.WriteLine($"{Name} — {param}");
    }

    // Records have 'with' built-in. Classes don't.
    // This manually simulates 'with' — creates a copy with a changed field.
    // The original object is untouched.
    public YOUR_ENTITY WithNotes(string notes) => new YOUR_ENTITY
    {
        Id         = this.Id,
        Name       = this.Name,
        YOUR_FIELD = this.YOUR_FIELD,
        Notes      = notes
    };
}
```

**Record (immutable snapshot):**
```csharp
// One line: compiler generates constructor, Equals, ==, GetHashCode, ToString, Deconstruct
public record YOUR_RECORD(int Id, string Name, decimal Value, DateOnly Date);

// Usage:
var r1 = new YOUR_RECORD(1, "Test", 9.99m, DateOnly.FromDateTime(DateTime.Now));
var r2 = r1 with { Value = 0m };        // new copy — r1 is unchanged
var (id, name, val, date) = r1;         // deconstruct into variables
bool same = r1 == r2;                   // value equality: all fields compared, not reference
```

---

### 2. `Dtos/YourDto.cs`

```csharp
namespace YOUR_PROJECT.Dtos;
using System.ComponentModel.DataAnnotations;

// ── Response DTO (what GET returns) ──────────────────────────────
// Never return raw Model objects — DTO controls exactly what JSON fields appear
public class YOUR_ENTITY_ResponseDto
{
    public int     Id     { get; set; }
    public string  Name   { get; set; } = string.Empty;
    public decimal Price  { get; set; }
    public List<YOUR_CHILD_Dto> Children { get; set; } = new();
}

public class YOUR_CHILD_Dto
{
    public int       Id       { get; set; }
    public DateTime  Date     { get; set; }
    public DateTime? NullDate { get; set; }   // nullable — check IsDBNull before reading
    public string    Status   { get; set; } = string.Empty;
    public List<YOUR_ITEM_Dto> Items { get; set; } = new();
}

public class YOUR_ITEM_Dto
{
    public string  Name  { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

// ── Request DTO (what POST receives) ─────────────────────────────
// [ApiController] auto-returns 400 if any [Required] or [Range] fails — no manual check needed
public class YOUR_ENTITY_RequestDto
{
    [Required]
    [MaxLength(200)]
    public string  Name  { get; set; } = string.Empty;

    [Range(0.01, 99999.99)]
    public decimal Price { get; set; }

    [Range(1, 5000)]
    public int     Count { get; set; }

    [Required]
    [StringLength(11, MinimumLength = 11)]   // exact length (e.g. PESEL)
    public string  Code  { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }       // nullable + format check

    public DateTime Date { get; set; }

    public List<YOUR_ITEM_RequestDto> Items { get; set; } = new();
}

public class YOUR_ITEM_RequestDto
{
    [Required]
    public string  Name  { get; set; } = string.Empty;   // lookup field (title, isbn, etc.)
    public decimal Price { get; set; }
}

// ── Record DTO (simple, no validation needed) ─────────────────────
public record LoanRequest(int BookId, string BorrowerId);
```

---

### 3. `Exceptions/` — Custom Exceptions

Define these once. Throw from repository or service. Controller catches and maps to HTTP status.

```csharp
namespace YOUR_PROJECT.Exceptions;

// ── 404 Not Found ─────────────────────────────────────────────────
// Throw when an entity looked up by ID or unique field doesn't exist.
public class NotFoundException : Exception
{
    public int EntityId { get; }

    // Overload 1: looked up by int ID
    public NotFoundException(int id)
        : base($"Entity {id} was not found.")
    {
        EntityId = id;
    }

    // Overload 2: looked up by name, ISBN, PESEL, etc.
    public NotFoundException(string identifier)
        : base($"'{identifier}' was not found.") { }
}

// ── 409 Conflict ──────────────────────────────────────────────────
// Throw when the request is valid but the current state blocks it:
// duplicate unique field, time-slot overlap, already cancelled, etc.
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
```

**Exception → HTTP status mapping (use in every controller):**

| Exception thrown | HTTP | Controller return |
|-----------------|------|-------------------|
| `NotFoundException` | 404 | `NotFound(new { message = ex.Message })` |
| `ConflictException` | 409 | `Conflict(new { message = ex.Message })` |
| `ArgumentException` | 400 | `BadRequest(new { message = ex.Message })` |

---

### 4. `Interfaces/IYourRepository.cs` and `IYourService.cs`

```csharp
namespace YOUR_PROJECT.Interfaces;
using YOUR_PROJECT.Dtos;

// ── Repository interface ──────────────────────────────────────────
// Controller depends on this, not on the concrete SQL class.
// Without this interface: no DI → up to -5 pts; no separation → up to -10 pts.
public interface IYOUR_ENTITY_Repository
{
    Task<IEnumerable<YOUR_ENTITY_ResponseDto>> GetAllAsync();
    Task<IEnumerable<YOUR_ENTITY_ResponseDto>> GetAllAsync(int? filterId, DateTime? filterDate);
    Task<YOUR_ENTITY_ResponseDto?>             GetByIdAsync(int id);
    Task<YOUR_ENTITY_ResponseDto?>             GetWithChildrenAsync(int id);
    Task<YOUR_ENTITY_ResponseDto?>             GetByFieldAsync(string fieldValue);
    Task<int>                                  AddAsync(YOUR_ENTITY_RequestDto dto);
    Task<int>                                  AddWithTransactionAsync(int parentId, YOUR_ENTITY_RequestDto dto);
    Task<bool>                                 UpdateAsync(int id, YOUR_ENTITY_RequestDto dto);
    Task<bool>                                 UpdateStatusAsync(int id, string newStatus);
    Task<bool>                                 DeleteAsync(int id);
}

// ── Service interface (use when you have a service layer) ─────────
public interface IYOUR_ENTITY_Service
{
    Task<IEnumerable<YOUR_ENTITY_ResponseDto>> GetAllAsync();
    Task<IEnumerable<YOUR_ENTITY_ResponseDto>> GetAllAsync(int? filterId, DateTime? filterDate);
    Task<YOUR_ENTITY_ResponseDto>              GetByIdAsync(int id);         // throws NotFoundException
    Task<YOUR_ENTITY_ResponseDto>              GetWithChildrenAsync(int id); // throws NotFoundException
    Task<int>                                  CreateAsync(int parentId, YOUR_ENTITY_RequestDto dto);
    Task                                       UpdateAsync(int id, YOUR_ENTITY_RequestDto dto);
    Task                                       UpdateStatusAsync(int id, string newStatus);
    Task                                       DeleteAsync(int id);
}
```

---

### 5. `Repositories/SqlYourRepository.cs` — Constructor

```csharp
namespace YOUR_PROJECT.Repositories;
using Microsoft.Data.SqlClient;
using YOUR_PROJECT.Dtos;
using YOUR_PROJECT.Exceptions;
using YOUR_PROJECT.Interfaces;

public class SqlYOUR_ENTITY_Repository : IYOUR_ENTITY_Repository
{
    private readonly string _cs;

    // IConfiguration is injected — connection string is NEVER hardcoded
    // Hardcoding it means -5 pts for no DI on the exam
    public SqlYOUR_ENTITY_Repository(IConfiguration config)
        => _cs = config.GetConnectionString("DefaultConnection")!;
}
```

---

#### 4a — GET All (simple flat list)

**Repository:**
```csharp
public async Task<IEnumerable<YOUR_ENTITY_ResponseDto>> GetAllAsync()
{
    var list = new List<YOUR_ENTITY_ResponseDto>();
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "SELECT YOUR_ID, YOUR_FIELD1, YOUR_FIELD2 FROM YOUR_TABLE", conn);
    await using var reader = await cmd.ExecuteReaderAsync();

    // Cache ordinals BEFORE the loop — GetOrdinal() is a string lookup, calling it inside
    // the loop on every row is wasteful and a common exam mistake
    int idOrd  = reader.GetOrdinal("YOUR_ID");
    int f1Ord  = reader.GetOrdinal("YOUR_FIELD1");
    int f2Ord  = reader.GetOrdinal("YOUR_FIELD2");

    while (await reader.ReadAsync())
        list.Add(new YOUR_ENTITY_ResponseDto
        {
            Id     = reader.GetInt32(idOrd),
            Field1 = reader.GetString(f1Ord),
            Field2 = reader.GetString(f2Ord),
        });

    return list;
}
```

**Service:**
```csharp
// No business logic to add — pass straight through to the repo
public Task<IEnumerable<YOUR_ENTITY_ResponseDto>> GetAllAsync()
    => _repo.GetAllAsync();
```

**Controller:**
```csharp
// GET /api/YOUR_ROUTE
[HttpGet]
public async Task<IActionResult> GetAll()
    => Ok(await _service.GetAllAsync());
```

---

#### 4b — GET All with optional query filters

**Repository:**
```csharp
public async Task<IEnumerable<YOUR_ENTITY_ResponseDto>> GetAllAsync(
    int?      filterId   = null,
    DateTime? filterDate = null)
{
    var list = new List<YOUR_ENTITY_ResponseDto>();
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();

    // @FilterId IS NULL = skip that WHERE clause when param is null
    // This pattern lets one query handle both "filter" and "no filter" cases
    await using var cmd = new SqlCommand(@"
        SELECT a.Id, p.Name AS ParentName, a.ScheduledAt, a.Status
        FROM YOUR_TABLE a
        JOIN PARENT_TABLE p ON p.Id = a.ParentId
        WHERE (@FilterId IS NULL OR a.ParentId = @FilterId)
          AND (@FilterDate IS NULL OR CAST(a.ScheduledAt AS DATE) = @FilterDate)",
        conn);

    // NEVER pass plain null to AddWithValue — SQL won't accept it.
    // Cast to object? first, then ?? DBNull.Value
    cmd.Parameters.AddWithValue("@FilterId",   (object?)filterId   ?? DBNull.Value);
    cmd.Parameters.AddWithValue("@FilterDate", (object?)filterDate ?? DBNull.Value);

    await using var reader = await cmd.ExecuteReaderAsync();

    int idOrd   = reader.GetOrdinal("Id");
    int nameOrd = reader.GetOrdinal("ParentName");
    int dateOrd = reader.GetOrdinal("ScheduledAt");

    while (await reader.ReadAsync())
        list.Add(new YOUR_ENTITY_ResponseDto
        {
            Id   = reader.GetInt32(idOrd),
            Name = reader.GetString(nameOrd),
            Date = reader.GetDateTime(dateOrd),
        });

    return list;
}
```

**Service:**
```csharp
public Task<IEnumerable<YOUR_ENTITY_ResponseDto>> GetAllAsync(int? filterId, DateTime? filterDate)
    => _repo.GetAllAsync(filterId, filterDate);
```

**Controller:**
```csharp
// GET /api/YOUR_ROUTE?filterId=5&filterDate=2026-05-01
[HttpGet]
public async Task<IActionResult> GetAll(
    [FromQuery] int?      filterId   = null,
    [FromQuery] DateTime? filterDate = null)
    => Ok(await _service.GetAllAsync(filterId, filterDate));
```

---

#### 4c — GET by ID (single row, 404 if missing)

**Repository:**
```csharp
// Returns null when not found — the service converts null into NotFoundException
public async Task<YOUR_ENTITY_ResponseDto?> GetByIdAsync(int id)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "SELECT YOUR_ID, YOUR_FIELD1, YOUR_NULLABLE_FIELD FROM YOUR_TABLE WHERE YOUR_ID = @Id",
        conn);
    cmd.Parameters.AddWithValue("@Id", id);
    await using var reader = await cmd.ExecuteReaderAsync();

    if (!await reader.ReadAsync()) return null;   // no row → caller handles 404

    int idOrd   = reader.GetOrdinal("YOUR_ID");
    int f1Ord   = reader.GetOrdinal("YOUR_FIELD1");
    int nullOrd = reader.GetOrdinal("YOUR_NULLABLE_FIELD");

    return new YOUR_ENTITY_ResponseDto
    {
        Id            = reader.GetInt32(idOrd),
        Field1        = reader.GetString(f1Ord),
        // Always check IsDBNull before reading a nullable column —
        // calling GetString on a NULL column throws InvalidCastException at runtime
        NullableField = reader.IsDBNull(nullOrd) ? null : reader.GetString(nullOrd),
    };
}
```

**Service:**
```csharp
// Service unwraps null into a typed exception — controller stays clean
public async Task<YOUR_ENTITY_ResponseDto> GetByIdAsync(int id)
{
    var result = await _repo.GetByIdAsync(id);
    return result ?? throw new NotFoundException(id);
}
```

**Controller:**
```csharp
// GET /api/YOUR_ROUTE/{id}
[HttpGet("{id:int}")]
public async Task<IActionResult> GetById(int id)
{
    try
    {
        return Ok(await _service.GetByIdAsync(id));
    }
    catch (NotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}

// ── Without a service layer (repo directly) ───────────────────────
[HttpGet("{id:int}")]
public async Task<IActionResult> GetById(int id)
{
    var result = await _repo.GetByIdAsync(id);
    return result is null ? NotFound() : Ok(result);
}
```

---

#### 4d — GET by ID with LEFT JOIN + Dictionary (nested children + items)

This is the core exam pattern. One query, one reader pass, Dictionary to deduplicate.

**Repository:**
```csharp
public async Task<YOUR_ENTITY_ResponseDto?> GetWithChildrenAsync(int id)
{
    YOUR_ENTITY_ResponseDto? result = null;
    // Dictionary keyed by child ID — ensures each child appears once in the result,
    // even though the JOIN produces one row per (child, item) combination
    var childDict = new Dictionary<int, YOUR_CHILD_Dto>();

    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();

    // Single JOIN query — more efficient than N+1 separate queries.
    // LEFT JOIN means: if parent has no children, we still get one row (with NULLs).
    const string sql = """
        SELECT
            p.YOUR_PARENT_ID, p.YOUR_FIELD1, p.YOUR_FIELD2,
            c.YOUR_CHILD_ID,  c.YOUR_CHILD_DATE, c.YOUR_NULLABLE_DATE,
            s.name            AS status_name,
            i.YOUR_ITEM_ID,   i.YOUR_ITEM_NAME, i.YOUR_ITEM_PRICE
        FROM YOUR_PARENT_TABLE p
        LEFT JOIN YOUR_CHILD_TABLE  c  ON c.YOUR_PARENT_FK = p.YOUR_PARENT_ID
        LEFT JOIN YOUR_STATUS_TABLE s  ON s.status_id = c.status_id
        LEFT JOIN YOUR_ITEM_TABLE   i  ON i.YOUR_CHILD_FK  = c.YOUR_CHILD_ID
        WHERE p.YOUR_PARENT_ID = @Id
        ORDER BY c.YOUR_CHILD_ID, i.YOUR_ITEM_NAME
        """;

    await using var cmd = new SqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@Id", id);
    await using var reader = await cmd.ExecuteReaderAsync();

    // Cache ALL ordinals BEFORE the loop
    int field1Ord       = reader.GetOrdinal("YOUR_FIELD1");
    int field2Ord       = reader.GetOrdinal("YOUR_FIELD2");
    int childIdOrd      = reader.GetOrdinal("YOUR_CHILD_ID");
    int childDateOrd    = reader.GetOrdinal("YOUR_CHILD_DATE");
    int nullableDateOrd = reader.GetOrdinal("YOUR_NULLABLE_DATE");
    int statusOrd       = reader.GetOrdinal("status_name");
    int itemIdOrd       = reader.GetOrdinal("YOUR_ITEM_ID");
    int itemNameOrd     = reader.GetOrdinal("YOUR_ITEM_NAME");
    int itemPriceOrd    = reader.GetOrdinal("YOUR_ITEM_PRICE");

    while (await reader.ReadAsync())
    {
        // Build parent once on the very first row
        if (result is null)
        {
            result = new YOUR_ENTITY_ResponseDto
            {
                Id     = id,
                Field1 = reader.GetString(field1Ord),
                Field2 = reader.GetString(field2Ord),
            };
        }

        // LEFT JOIN → child columns are NULL if parent has no children at all
        if (reader.IsDBNull(childIdOrd)) continue;

        int childId = reader.GetInt32(childIdOrd);
        if (!childDict.TryGetValue(childId, out var childDto))
        {
            // First time we see this child ID — create the DTO and register it
            childDto = new YOUR_CHILD_Dto
            {
                Id       = childId,
                Date     = reader.GetDateTime(childDateOrd),
                NullDate = reader.IsDBNull(nullableDateOrd)
                               ? null
                               : reader.GetDateTime(nullableDateOrd),
                Status   = reader.GetString(statusOrd),
            };
            childDict[childId] = childDto;
            result.Children.Add(childDto);
        }

        // Add item to this child if present (can also be NULL from LEFT JOIN)
        if (!reader.IsDBNull(itemIdOrd))
        {
            childDto.Items.Add(new YOUR_ITEM_Dto
            {
                Name  = reader.GetString(itemNameOrd),
                Price = reader.GetDecimal(itemPriceOrd),
            });
        }
    }

    return result;   // null → caller handles 404
}
```

**Service:**
```csharp
public async Task<YOUR_ENTITY_ResponseDto> GetWithChildrenAsync(int id)
{
    var result = await _repo.GetWithChildrenAsync(id);
    return result ?? throw new NotFoundException(id);
}
```

**Controller:**
```csharp
// GET /api/YOUR_ROUTE/{id}/YOUR_CHILDREN
[HttpGet("{id:int}/YOUR_CHILDREN")]
public async Task<IActionResult> GetWithChildren(int id)
{
    try
    {
        return Ok(await _service.GetWithChildrenAsync(id));
    }
    catch (NotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}

// ── Without a service layer ───────────────────────────────────────
[HttpGet("{id:int}/YOUR_CHILDREN")]
public async Task<IActionResult> GetWithChildren(int id)
{
    var result = await _repo.GetWithChildrenAsync(id);
    return result is null
        ? NotFound(new { message = $"Entity {id} not found." })
        : Ok(result);
}
```

---

#### 4e — GET by lookup field (PESEL, ISBN, email, etc.)

**Repository:**
```csharp
public async Task<YOUR_ENTITY_ResponseDto?> GetByFieldAsync(string fieldValue)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "SELECT Id, Name, Email FROM YOUR_TABLE WHERE YOUR_UNIQUE_COL = @Val", conn);
    cmd.Parameters.AddWithValue("@Val", fieldValue);
    await using var reader = await cmd.ExecuteReaderAsync();
    if (!await reader.ReadAsync()) return null;

    int idOrd    = reader.GetOrdinal("Id");
    int nameOrd  = reader.GetOrdinal("Name");
    int emailOrd = reader.GetOrdinal("Email");

    return new YOUR_ENTITY_ResponseDto
    {
        Id    = reader.GetInt32(idOrd),
        Name  = reader.GetString(nameOrd),
        Email = reader.IsDBNull(emailOrd) ? null : reader.GetString(emailOrd),
    };
}
```

**Service:**
```csharp
// Used internally for duplicate checks before insert.
// Expose as endpoint only if the business actually needs it.
public async Task<YOUR_ENTITY_ResponseDto> GetByFieldAsync(string fieldValue)
{
    var result = await _repo.GetByFieldAsync(fieldValue);
    return result ?? throw new NotFoundException(fieldValue);
}
```

**Controller:**
```csharp
// GET /api/YOUR_ROUTE/lookup/{value}
[HttpGet("lookup/{value}")]
public async Task<IActionResult> GetByField(string value)
{
    try
    {
        return Ok(await _service.GetByFieldAsync(value));
    }
    catch (NotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}
```

---

#### 4f — INSERT simple (no transaction) → returns new ID

**Repository:**
```csharp
public async Task<int> AddAsync(YOUR_ENTITY_RequestDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        """
        INSERT INTO YOUR_TABLE (YOUR_COL1, YOUR_COL2, YOUR_NULLABLE_COL)
        VALUES (@V1, @V2, @VNull);
        SELECT SCOPE_IDENTITY();
        """, conn);
    cmd.Parameters.AddWithValue("@V1",    dto.Field1);
    cmd.Parameters.AddWithValue("@V2",    dto.Field2);
    cmd.Parameters.AddWithValue("@VNull", (object?)dto.NullableField ?? DBNull.Value);
    // ExecuteScalar returns SCOPE_IDENTITY() — the ID our INSERT just generated.
    // Convert.ToInt32 handles the decimal type that SCOPE_IDENTITY() returns.
    return Convert.ToInt32(await cmd.ExecuteScalarAsync());
}
```

> **WHY `SCOPE_IDENTITY()` not `@@IDENTITY`:** `SCOPE_IDENTITY()` returns the ID generated in the current scope. `@@IDENTITY` returns the last identity generated anywhere — including by a trigger on another table. Always use `SCOPE_IDENTITY()`.

**Service:**
```csharp
public async Task<int> CreateAsync(YOUR_ENTITY_RequestDto dto)
{
    // Business rule: check for duplicate before inserting
    var existing = await _repo.GetByFieldAsync(dto.Name);
    if (existing != null)
        throw new ConflictException($"'{dto.Name}' already exists.");

    return await _repo.AddAsync(dto);
}
```

**Controller:**
```csharp
// POST /api/YOUR_ROUTE
[HttpPost]
public async Task<IActionResult> Create([FromBody] YOUR_ENTITY_RequestDto dto)
{
    try
    {
        int newId = await _service.CreateAsync(dto);
        // 201 Created with Location header pointing to the new resource
        return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
    }
    catch (ConflictException ex)
    {
        return Conflict(new { message = ex.Message });
    }
}

// ── Without a service layer (no duplicate check) ──────────────────
[HttpPost]
public async Task<IActionResult> Create([FromBody] YOUR_ENTITY_RequestDto dto)
{
    int newId = await _repo.AddAsync(dto);
    return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
}
```

---

#### 4g — INSERT with transaction (multi-step, the exam POST pattern)

**Repository:**
```csharp
public async Task<int> AddWithTransactionAsync(int parentId, YOUR_ENTITY_RequestDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    // ALL SqlCommands inside this method must pass conn AND tx as arguments
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // Step 1: Verify parent exists
        await using var checkParent = new SqlCommand(
            "SELECT COUNT(1) FROM YOUR_PARENT_TABLE WHERE YOUR_PARENT_ID = @Id",
            conn, tx);   // ← conn AND tx always passed together inside a transaction
        checkParent.Parameters.AddWithValue("@Id", parentId);
        if ((int)(await checkParent.ExecuteScalarAsync())! == 0)
            throw new NotFoundException(parentId);

        // Step 2: Resolve each item name → id (lookup)
        // Collect ALL results FIRST — don't INSERT inside this loop
        var resolvedItems = new List<(int LookupId, decimal Price)>();
        foreach (var item in dto.Items)
        {
            await using var find = new SqlCommand(
                "SELECT YOUR_LOOKUP_ID FROM YOUR_LOOKUP_TABLE WHERE name = @Name",
                conn, tx);
            find.Parameters.AddWithValue("@Name", item.Name);
            var found = await find.ExecuteScalarAsync();
            if (found is null)
                throw new NotFoundException(item.Name);   // string overload
            resolvedItems.Add(((int)found, item.Price));
        }

        // Step 3: Get default status id
        await using var getStatus = new SqlCommand(
            "SELECT status_id FROM YOUR_STATUS_TABLE WHERE name = 'YOUR_DEFAULT_STATUS'",
            conn, tx);
        int statusId = (int)(await getStatus.ExecuteScalarAsync())!;

        // Step 4: INSERT header → get new id
        await using var insertHeader = new SqlCommand(
            """
            INSERT INTO YOUR_CHILD_TABLE (YOUR_DATE_COL, YOUR_PARENT_FK, status_id)
            VALUES (@Date, @ParentId, @StatusId);
            SELECT SCOPE_IDENTITY();
            """, conn, tx);
        insertHeader.Parameters.AddWithValue("@Date",     dto.Date);
        insertHeader.Parameters.AddWithValue("@ParentId", parentId);
        insertHeader.Parameters.AddWithValue("@StatusId", statusId);
        int newId = Convert.ToInt32(await insertHeader.ExecuteScalarAsync());

        // Step 5: INSERT line items in loop
        foreach (var (lookupId, price) in resolvedItems)
        {
            await using var insertItem = new SqlCommand(
                """
                INSERT INTO YOUR_ITEM_TABLE (YOUR_CHILD_FK, YOUR_LOOKUP_FK, YOUR_PRICE_COL)
                VALUES (@ChildId, @LookupId, @Price)
                """, conn, tx);
            insertItem.Parameters.AddWithValue("@ChildId",  newId);
            insertItem.Parameters.AddWithValue("@LookupId", lookupId);
            insertItem.Parameters.AddWithValue("@Price",    price);
            await insertItem.ExecuteNonQueryAsync();
        }

        await tx.CommitAsync();
        return newId;
    }
    catch
    {
        // Rollback undoes ALL inserts if ANYTHING failed
        await tx.RollbackAsync();
        throw;   // "throw;" preserves stack trace. "throw ex;" resets it — never write "throw ex;"
    }
}
```

**Service:**
```csharp
public async Task<int> CreateAsync(int parentId, YOUR_ENTITY_RequestDto dto)
{
    // Business rules go here before delegating to the repo
    // NotFoundException and ConflictException thrown inside the repo propagate automatically
    if (dto.Date < DateTime.UtcNow)
        throw new ArgumentException("Date must be in the future.");

    return await _repo.AddWithTransactionAsync(parentId, dto);
}
```

**Controller:**
```csharp
// POST /api/YOUR_ROUTE/{id}/YOUR_CHILDREN
[HttpPost("{id:int}/YOUR_CHILDREN")]
public async Task<IActionResult> AddChild(int id, [FromBody] YOUR_ENTITY_RequestDto dto)
{
    try
    {
        int newId = await _service.CreateAsync(id, dto);
        return Created($"api/YOUR_ROUTE/{id}/YOUR_CHILDREN", new { id = newId });
    }
    catch (NotFoundException ex)  { return NotFound(new { message = ex.Message }); }
    catch (ConflictException ex)  { return Conflict(new { message = ex.Message }); }
    catch (ArgumentException ex)  { return BadRequest(new { message = ex.Message }); }
}
```

---

#### 4h — INSERT with overlap/conflict check inside transaction

Add this block inside the transaction (step 2.5 between Step 2 and Step 3 of 4g):

**Repository snippet:**
```csharp
// e.g. booking a time slot — check no overlap before inserting
await using var checkOverlap = new SqlCommand(@"
    SELECT COUNT(*) FROM YOUR_TABLE
    WHERE YOUR_RESOURCE_ID = @ResourceId
      AND Status != 'cancelled'
      AND @Start < DATEADD(minute, DurationMin, ScheduledAt)
      AND DATEADD(minute, @Dur, @Start) > ScheduledAt",
    conn, tx);
checkOverlap.Parameters.AddWithValue("@ResourceId", dto.ResourceId);
checkOverlap.Parameters.AddWithValue("@Start",      dto.ScheduledAt);
checkOverlap.Parameters.AddWithValue("@Dur",        dto.DurationMin);
if ((int)(await checkOverlap.ExecuteScalarAsync())! > 0)
    throw new ConflictException("Time slot is already booked.");
```

**Service:** *(no change — `ConflictException` propagates automatically)*

**Controller:** *(same as 4g — the `catch (ConflictException)` branch handles it)*

---

#### 4i — UPDATE → 204 on success, 404 if not found

**Repository:**
```csharp
public async Task<bool> UpdateAsync(int id, YOUR_ENTITY_RequestDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "UPDATE YOUR_TABLE SET YOUR_COL1 = @V1, YOUR_COL2 = @V2 WHERE YOUR_ID = @Id",
        conn);
    cmd.Parameters.AddWithValue("@Id", id);
    cmd.Parameters.AddWithValue("@V1", dto.Field1);
    cmd.Parameters.AddWithValue("@V2", dto.Field2);
    // ExecuteNonQueryAsync returns the number of rows affected.
    // 0 rows = ID didn't exist → return false → service throws NotFoundException
    return await cmd.ExecuteNonQueryAsync() > 0;
}
```

**Service:**
```csharp
public async Task UpdateAsync(int id, YOUR_ENTITY_RequestDto dto)
{
    bool updated = await _repo.UpdateAsync(id, dto);
    if (!updated) throw new NotFoundException(id);
    // Returns void — controller returns 204 NoContent
}
```

**Controller:**
```csharp
// PUT /api/YOUR_ROUTE/{id}
[HttpPut("{id:int}")]
public async Task<IActionResult> Update(int id, [FromBody] YOUR_ENTITY_RequestDto dto)
{
    try
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();   // 204 — success, no body needed
    }
    catch (NotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}

// ── Without a service layer ───────────────────────────────────────
[HttpPut("{id:int}")]
public async Task<IActionResult> Update(int id, [FromBody] YOUR_ENTITY_RequestDto dto)
{
    bool ok = await _repo.UpdateAsync(id, dto);
    return ok ? NoContent() : NotFound();
}
```

---

#### 4j — UPDATE status field (cancel, return, close)

**Repository:**
```csharp
public async Task<bool> UpdateStatusAsync(int id, string newStatus)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "UPDATE YOUR_TABLE SET Status = @Status WHERE Id = @Id", conn);
    cmd.Parameters.AddWithValue("@Id",     id);
    cmd.Parameters.AddWithValue("@Status", newStatus);
    return await cmd.ExecuteNonQueryAsync() > 0;
}
```

**Service:**
```csharp
public async Task UpdateStatusAsync(int id, string newStatus)
{
    // Business rule: reject invalid status values before touching the DB
    var valid = new[] { "active", "cancelled", "completed" };
    if (!valid.Contains(newStatus))
        throw new ArgumentException($"Invalid status '{newStatus}'.");

    bool updated = await _repo.UpdateStatusAsync(id, newStatus);
    if (!updated) throw new NotFoundException(id);
}
```

**Controller:**
```csharp
// PATCH /api/YOUR_ROUTE/{id}/status
[HttpPatch("{id:int}/status")]
public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
{
    try
    {
        await _service.UpdateStatusAsync(id, newStatus);
        return NoContent();
    }
    catch (NotFoundException ex)  { return NotFound(new { message = ex.Message }); }
    catch (ArgumentException ex)  { return BadRequest(new { message = ex.Message }); }
}

// ── Without a service layer ───────────────────────────────────────
[HttpPatch("{id:int}/status")]
public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
{
    bool ok = await _repo.UpdateStatusAsync(id, newStatus);
    return ok ? NoContent() : NotFound();
}
```

---

#### 4k — DELETE → 204 on success, 404 if not found

**Repository:**
```csharp
public async Task<bool> DeleteAsync(int id)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "DELETE FROM YOUR_TABLE WHERE YOUR_ID = @Id", conn);
    cmd.Parameters.AddWithValue("@Id", id);
    return await cmd.ExecuteNonQueryAsync() > 0;
}
```

**Service:**
```csharp
public async Task DeleteAsync(int id)
{
    bool deleted = await _repo.DeleteAsync(id);
    if (!deleted) throw new NotFoundException(id);
}
```

**Controller:**
```csharp
// DELETE /api/YOUR_ROUTE/{id}
[HttpDelete("{id:int}")]
public async Task<IActionResult> Delete(int id)
{
    try
    {
        await _service.DeleteAsync(id);
        return NoContent();   // 204 — success, nothing to return
    }
    catch (NotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}

// ── Without a service layer ───────────────────────────────────────
[HttpDelete("{id:int}")]
public async Task<IActionResult> Delete(int id)
{
    bool ok = await _repo.DeleteAsync(id);
    return ok ? NoContent() : NotFound();
}
```

---

### 6. `Services/YourService.cs` — Full Class

All service snippets above assembled into one copy-paste ready file:

```csharp
namespace YOUR_PROJECT.Services;
using YOUR_PROJECT.Dtos;
using YOUR_PROJECT.Exceptions;
using YOUR_PROJECT.Interfaces;

public class YOUR_ENTITY_Service : IYOUR_ENTITY_Service
{
    private readonly IYOUR_ENTITY_Repository _repo;

    // Constructor injection — the DI container provides the repository
    public YOUR_ENTITY_Service(IYOUR_ENTITY_Repository repo) => _repo = repo;

    public Task<IEnumerable<YOUR_ENTITY_ResponseDto>> GetAllAsync()
        => _repo.GetAllAsync();

    public Task<IEnumerable<YOUR_ENTITY_ResponseDto>> GetAllAsync(int? filterId, DateTime? filterDate)
        => _repo.GetAllAsync(filterId, filterDate);

    public async Task<YOUR_ENTITY_ResponseDto> GetByIdAsync(int id)
    {
        var result = await _repo.GetByIdAsync(id);
        return result ?? throw new NotFoundException(id);
    }

    public async Task<YOUR_ENTITY_ResponseDto> GetWithChildrenAsync(int id)
    {
        var result = await _repo.GetWithChildrenAsync(id);
        return result ?? throw new NotFoundException(id);
    }

    public async Task<int> CreateAsync(int parentId, YOUR_ENTITY_RequestDto dto)
    {
        // Add domain-specific validation here before delegating to the repo
        if (dto.Date < DateTime.UtcNow)
            throw new ArgumentException("Date must be in the future.");

        // NotFoundException / ConflictException from repo propagate automatically
        return await _repo.AddWithTransactionAsync(parentId, dto);
    }

    public async Task UpdateAsync(int id, YOUR_ENTITY_RequestDto dto)
    {
        bool updated = await _repo.UpdateAsync(id, dto);
        if (!updated) throw new NotFoundException(id);
    }

    public async Task UpdateStatusAsync(int id, string newStatus)
    {
        var valid = new[] { "active", "cancelled", "completed" };
        if (!valid.Contains(newStatus))
            throw new ArgumentException($"Invalid status '{newStatus}'.");

        bool updated = await _repo.UpdateStatusAsync(id, newStatus);
        if (!updated) throw new NotFoundException(id);
    }

    public async Task DeleteAsync(int id)
    {
        bool deleted = await _repo.DeleteAsync(id);
        if (!deleted) throw new NotFoundException(id);
    }
}
```

---

### 7. `Controllers/YourController.cs` — Full Class

All controller actions assembled — use the service-layer version:

```csharp
namespace YOUR_PROJECT.Controllers;
using Microsoft.AspNetCore.Mvc;
using YOUR_PROJECT.Dtos;
using YOUR_PROJECT.Exceptions;
using YOUR_PROJECT.Interfaces;

[ApiController]
[Route("api/YOUR_ROUTE")]
public class YOUR_ENTITYController : ControllerBase
{
    private readonly IYOUR_ENTITY_Service _service;

    // Constructor injection — NEVER use "new SqlXxxRepository()" here
    // Hardcoding "new" bypasses DI → up to -5 pts on the exam
    public YOUR_ENTITYController(IYOUR_ENTITY_Service service) => _service = service;

    // GET /api/YOUR_ROUTE
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    // GET /api/YOUR_ROUTE?filterId=5&filterDate=2026-05-01
    [HttpGet("filtered")]
    public async Task<IActionResult> GetAllFiltered(
        [FromQuery] int?      filterId   = null,
        [FromQuery] DateTime? filterDate = null)
        => Ok(await _service.GetAllAsync(filterId, filterDate));

    // GET /api/YOUR_ROUTE/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try   { return Ok(await _service.GetByIdAsync(id)); }
        catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    // GET /api/YOUR_ROUTE/{id}/YOUR_CHILDREN
    [HttpGet("{id:int}/YOUR_CHILDREN")]
    public async Task<IActionResult> GetWithChildren(int id)
    {
        try   { return Ok(await _service.GetWithChildrenAsync(id)); }
        catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    // POST /api/YOUR_ROUTE/{id}/YOUR_CHILDREN
    [HttpPost("{id:int}/YOUR_CHILDREN")]
    public async Task<IActionResult> AddChild(int id, [FromBody] YOUR_ENTITY_RequestDto dto)
    {
        try
        {
            int newId = await _service.CreateAsync(id, dto);
            return Created($"api/YOUR_ROUTE/{id}/YOUR_CHILDREN", new { id = newId });
        }
        catch (NotFoundException ex)  { return NotFound(new { message = ex.Message }); }
        catch (ConflictException ex)  { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex)  { return BadRequest(new { message = ex.Message }); }
    }

    // PUT /api/YOUR_ROUTE/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] YOUR_ENTITY_RequestDto dto)
    {
        try   { await _service.UpdateAsync(id, dto); return NoContent(); }
        catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    // PATCH /api/YOUR_ROUTE/{id}/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
    {
        try   { await _service.UpdateStatusAsync(id, newStatus); return NoContent(); }
        catch (NotFoundException ex)  { return NotFound(new { message = ex.Message }); }
        catch (ArgumentException ex)  { return BadRequest(new { message = ex.Message }); }
    }

    // DELETE /api/YOUR_ROUTE/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try   { await _service.DeleteAsync(id); return NoContent(); }
        catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}
```

---

### 8. `Middleware/`

**ExceptionHandlingMiddleware.cs** — catches anything that escapes the controller:
```csharp
namespace YOUR_PROJECT.Middleware;
using YOUR_PROJECT.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate                      _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    { _next = next; _logger = logger; }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            // Only needed if you are NOT catching in the controller.
            // If you catch in the controller AND here, the middleware never fires for these.
            context.Response.StatusCode  = 404;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (ConflictException ex)
        {
            context.Response.StatusCode  = 409;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode  = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(
                new { message = "An unexpected error occurred." });
        }
    }
}
```

> **Pick ONE approach:** either catch in each controller action (explicit, examiner sees the mapping) OR catch in middleware (less repetition). Don't do both. For exams, catching in the controller is safer — it makes the HTTP mapping obvious.

**RequestTimingMiddleware.cs (optional):**
```csharp
public class RequestTimingMiddleware
{
    private readonly RequestDelegate                  _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next,
        ILogger<RequestTimingMiddleware> logger)
    { _next = next; _logger = logger; }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        try   { await _next(context); }
        finally
        {
            sw.Stop();
            _logger.LogInformation("{Method} {Path} — {Ms}ms",
                context.Request.Method, context.Request.Path,
                sw.Elapsed.TotalMilliseconds);
        }
    }
}
```

---

### 9. `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// AddScoped = one instance per HTTP request — correct for repos using SqlConnection
// AddSingleton would share one SqlConnection across all requests — WRONG for ADO.NET
builder.Services.AddScoped<IYOUR_ENTITY_Repository, SqlYOUR_ENTITY_Repository>();
builder.Services.AddScoped<IYOUR_OTHER_Repository,  SqlYOUR_OTHER_Repository>();
// Service layer (recommended for full marks on architecture criterion):
builder.Services.AddScoped<IYOUR_ENTITY_Service, YOUR_ENTITY_Service>();

var app = builder.Build();

// ORDER MATTERS — exception handler must be FIRST so it wraps everything else
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();    // optional
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();
```

---

### 10. `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DB;User Id=YOUR_USER;Password=YOUR_PASS;TrustServerCertificate=True"
  }
}
```

---

## ⚡ ADO.NET Isolated Snippets

### ExecuteScalar — COUNT existence check
```csharp
await using var cmd = new SqlCommand(
    "SELECT COUNT(1) FROM YOUR_TABLE WHERE YOUR_COL = @Val",
    conn, tx);   // omit tx if not inside a transaction
cmd.Parameters.AddWithValue("@Val", yourValue);
int count = (int)(await cmd.ExecuteScalarAsync())!;
if (count == 0) throw new NotFoundException(yourValue.ToString());
```

### ExecuteScalar — lookup id by name (throw if missing)
```csharp
await using var cmd = new SqlCommand(
    "SELECT YOUR_ID FROM YOUR_TABLE WHERE name = @Name",
    conn, tx);
cmd.Parameters.AddWithValue("@Name", yourName);
var result = await cmd.ExecuteScalarAsync();
if (result is null) throw new NotFoundException(yourName);
int foundId = (int)result;
```

### ExecuteScalar — get default status id
```csharp
await using var cmd = new SqlCommand(
    "SELECT status_id FROM YOUR_STATUS_TABLE WHERE name = 'YOUR_DEFAULT'",
    conn, tx);
int statusId = (int)(await cmd.ExecuteScalarAsync())!;
```

### ExecuteScalar — INSERT + get new auto-generated ID
```csharp
await using var cmd = new SqlCommand(
    """
    INSERT INTO YOUR_TABLE (YOUR_COL1, YOUR_COL2)
    VALUES (@V1, @V2);
    SELECT SCOPE_IDENTITY();
    """, conn, tx);   // pass tx if inside a transaction
cmd.Parameters.AddWithValue("@V1", value1);
cmd.Parameters.AddWithValue("@V2", value2);
int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
```

### ExecuteNonQuery — UPDATE
```csharp
await using var cmd = new SqlCommand(
    "UPDATE YOUR_TABLE SET YOUR_COL = @Val WHERE YOUR_ID = @Id", conn);
cmd.Parameters.AddWithValue("@Id",  id);
cmd.Parameters.AddWithValue("@Val", value);
bool success = await cmd.ExecuteNonQueryAsync() > 0;
```

### ExecuteNonQuery — DELETE
```csharp
await using var cmd = new SqlCommand(
    "DELETE FROM YOUR_TABLE WHERE YOUR_ID = @Id", conn);
cmd.Parameters.AddWithValue("@Id", id);
bool deleted = await cmd.ExecuteNonQueryAsync() > 0;
```

### ExecuteNonQuery — INSERT inside loop (line items, with transaction)
```csharp
foreach (var item in items)
{
    await using var cmd = new SqlCommand(
        """
        INSERT INTO YOUR_ITEM_TABLE (YOUR_PARENT_FK, YOUR_ITEM_COL, YOUR_PRICE_COL)
        VALUES (@ParentId, @ItemId, @Price)
        """, conn, tx);   // ← tx REQUIRED inside a transaction
    cmd.Parameters.AddWithValue("@ParentId", parentId);
    cmd.Parameters.AddWithValue("@ItemId",   item.Id);
    cmd.Parameters.AddWithValue("@Price",    item.Price);
    await cmd.ExecuteNonQueryAsync();
}
```

### Nullable column patterns — all types
```csharp
// string?
string? email = reader.IsDBNull(emailOrd) ? null : reader.GetString(emailOrd);

// DateTime?
DateTime? returnDate = reader.IsDBNull(returnOrd) ? null : reader.GetDateTime(returnOrd);

// decimal?
decimal? price = reader.IsDBNull(priceOrd) ? null : reader.GetDecimal(priceOrd);

// int?
int? count = reader.IsDBNull(countOrd) ? null : reader.GetInt32(countOrd);

// Passing C# null → SQL NULL — NEVER pass plain null to AddWithValue
cmd.Parameters.AddWithValue("@Email",  (object?)user.Email  ?? DBNull.Value);
cmd.Parameters.AddWithValue("@Date",   (object?)someDate    ?? DBNull.Value);
cmd.Parameters.AddWithValue("@Count",  (object?)someInt     ?? DBNull.Value);
```

---

## 🔁 Transaction Skeleton

```csharp
await using var conn = new SqlConnection(_cs);
await conn.OpenAsync();
await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

try
{
    // EVERY SqlCommand must pass conn AND tx as 2nd and 3rd arguments:
    // new SqlCommand("...", conn, tx)
    // Forgetting tx means the command runs OUTSIDE the transaction — silent bug

    await tx.CommitAsync();
    return result;
}
catch
{
    // Rollback undoes ALL work if any step fails — database stays clean
    await tx.RollbackAsync();
    throw;   // "throw;" preserves stack trace. "throw ex;" resets it — always use "throw;"
}
```

---

## 🔎 LINQ Quick Reference

```csharp
// Filter
var result = list.Where(x => x.YOUR_FIELD == value).ToList();

// Project
var result = list.Select(x => new { x.Id, x.Name }).ToList();

// Sort (multi-level)
var result = list.OrderByDescending(x => x.Value).ThenBy(x => x.Name).ToList();

// Quantifiers
bool any  = list.Any(x => x.Field == value);
bool all  = list.All(x => x.Field >= 0);   // NOTE: true on empty list (vacuous truth)

// Aggregates
double avg   = list.Average(x => x.Number);
double max   = list.Max(x => x.Number);
int    count = list.Count(x => x.Active);
string fold  = list.Aggregate("Seed: ", (acc, x) => acc + $"{x.Value} ");

// Paging
var page = list.OrderBy(x => x.Id)
               .Skip((pageNum - 1) * pageSize)
               .Take(pageSize)
               .ToList();

// GroupBy → anonymous type
var grouped = list
    .GroupBy(x => x.Category)
    .Select(g => new { Key = g.Key, Count = g.Count(), Avg = g.Average(x => x.Value) })
    .ToList();

// GroupBy → Dictionary
var dict = list
    .GroupBy(x => x.Category)
    .ToDictionary(g => g.Key, g => g.Average(x => x.Value));

// MaxBy / MinBy — returns the element itself, not just the max value
var best = list.MaxBy(x => x.Score);

// INNER join — only rows that match in BOTH lists
var joined = listA.Join(listB,
    a => a.Id, b => b.AId,
    (a, b) => new { a.Name, b.Data }).ToList();

// LEFT join / GroupJoin — all rows from listA, even with no match in listB
var leftJoin = listA.GroupJoin(listB,
    a => a.Id, b => b.AId,
    (a, bs) => new { a.Name, Items = bs.ToList() }).ToList();

// Anti-join — items in listA with NO match in listB
var noMatch = listA
    .Where(a => !listB.Any(b => b.AId == a.Id))
    .ToList();

// Flatten nested collections (SelectMany)
var flat = listA
    .SelectMany(a => a.Items)
    .Select(i => i.Name)
    .Distinct()
    .OrderBy(n => n)
    .ToList();
```

---

## 🧩 OOP Patterns

### Switch expression (type + guard)
```csharp
// Pattern matching on type — cleaner than if/else chain
// Guards (when) narrow within a type match
string Classify(object obj) => obj switch
{
    YOUR_TYPE_A a when a.Value > 0 => $"positive A: {a.Value}",
    YOUR_TYPE_A a                  => "non-positive A",
    YOUR_TYPE_B b                  => $"B: {b.Name}",
    null                           => "null",
    _                              => "unknown"   // _ = default case
};
```

### Async concurrent — Task.WhenAll
```csharp
// Both tasks START at the same time — total time ≈ slowest task, not the sum
var taskA = _repo.GetSomethingAsync();
var taskB = _repo.GetSomethingElseAsync();
await Task.WhenAll(taskA, taskB);
var a = taskA.Result;
var b = taskB.Result;

// Fire-and-forget over a collection
await Task.WhenAll(items.Select(i => ProcessAsync(i)));
```

### Delegates
```csharp
Action<string>        log     = msg => Console.WriteLine($"[LOG] {msg}");
Func<YOUR_TYPE, bool> isValid = x => x.Value > 0;   // Func: returns a value
Predicate<YOUR_TYPE>  isOk    = x => x.IsActive;     // Predicate: always returns bool
```

### yield return (lazy generator)
```csharp
// Nothing runs until the caller iterates — memory-efficient for large sequences
public IEnumerable<string> GenerateLines(List<YOUR_TYPE> items)
{
    foreach (var item in items)
        yield return $"{item.Id}. {item.Name}";
}
// Usage: var first3 = GenerateLines(items).Take(3).ToList();
```

### ref / out parameters
```csharp
// out: method MUST assign before returning; caller does NOT initialise first
public bool TryGet(int id, out YOUR_TYPE result, out string error)
{
    try   { result = GetById(id); error = string.Empty; return true; }
    catch { result = default!;    error = "Not found";  return false; }
}
// Caller:
if (TryGet(1, out var item, out var err)) Console.WriteLine(item.Name);

// ref: caller MUST initialise before passing; method modifies the caller's variable
public void Count(List<YOUR_TYPE> items, ref int total, ref int invalid)
{
    foreach (var i in items) { total++; if (i.Value < 0) invalid++; }
}
// Caller:
int total = 0, invalid = 0;
Count(list, ref total, ref invalid);
```

### Generic class with constraint
```csharp
// where T : class = T must be a reference type (not int, bool, etc.)
public class GenericRepository<T> where T : class
{
    private readonly List<T> _items = new();
    public void Add(T item) => _items.Add(item);
    public T? FindFirst(Func<T, bool> predicate) => _items.FirstOrDefault(predicate);
    public List<T> FindAll(Func<T, bool> predicate) => _items.Where(predicate).ToList();
    // Secondary type constraint: TKey must support comparison (IComparable<TKey>)
    public T? Max<TKey>(Func<T, TKey> selector) where TKey : IComparable<TKey>
        => _items.MaxBy(selector);
}
```

---

## 📊 HTTP Status Code Reference

| Return | Status | When |
|--------|--------|------|
| `Ok(data)` | 200 | GET succeeded |
| `CreatedAtAction(nameof(GetById), new { id }, data)` | 201 | POST — with Location header |
| `Created($"api/route/{id}", new {{ id }})` | 201 | POST — simpler, no Location header |
| `NoContent()` | 204 | DELETE / PUT succeeded, no body |
| `BadRequest(new { message })` | 400 | Invalid input (also auto via `[ApiController]`) |
| `NotFound(new { message })` | 404 | Resource not found |
| `Conflict(new { message })` | 409 | Valid request, state blocks it |

---

## ✅ Pre-Submit Checklist

```
□ Every repo method is async Task<...>
□ Every await keyword is present (not-async loses up to -10 pts)
□ GetOrdinal cached OUTSIDE the while loop
□ IsDBNull checked on EVERY nullable column before GetString/GetDateTime/GetDecimal/GetInt32
□ C# null → SQL NULL:  (object?)value ?? DBNull.Value
□ Every SqlCommand inside transaction:  new SqlCommand("...", conn, tx)
□ SCOPE_IDENTITY() used (NOT @@IDENTITY)
□ Rethrow written as  throw;  (NOT  throw ex;)
□ Controller does NOT import Microsoft.Data.SqlClient
□ NotFoundException / ConflictException defined in Exceptions/ folder
□ Exceptions thrown from repo/service, caught in controller with correct status codes
□ DI registered: builder.Services.AddScoped<IRepo, SqlRepo>()
□ DI registered: builder.Services.AddScoped<IService, Service>()   ← if using service layer
□ Middleware registered: ExceptionHandlingMiddleware FIRST
□ .gitignore committed, no bin/ or obj/ in repo (missing costs -50%)
□ dotnet build → 0 errors
□ git add . && git commit -m "complete" && git push
```
