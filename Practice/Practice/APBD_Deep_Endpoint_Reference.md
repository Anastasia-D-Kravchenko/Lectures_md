# APBD — Deep Endpoint Reference
> Every possible JSON endpoint pattern your exam might ask for.  
> All three layers (Repository → Service → Controller) for each case.  
> `YOUR_*` placeholders are fill-in-the-blank. Everything is copy-paste ready.

---

## How to read this file

Each section covers **one kind of HTTP request**. Inside each section you get:

| Block | What it does |
|-------|-------------|
| **Repository** | Raw ADO.NET SQL — returns `null` or data, never throws HTTP codes |
| **Service** | Business rules — converts `null` → `NotFoundException`, enforces constraints |
| **Controller** | HTTP mapping only — calls service, catches exceptions, returns status codes |

**Pick the pattern that matches your endpoint, swap the names, done.**

---

## 📁 Folder Structure (exam-safe default)

```
YOUR_PROJECT/
├── Controllers/          ← HTTP layer only
│   └── EntitiesController.cs
├── Dtos/
│   ├── EntityResponseDto.cs
│   └── EntityRequestDto.cs
├── Exceptions/
│   ├── NotFoundException.cs
│   └── ConflictException.cs
├── Interfaces/
│   ├── IEntityRepository.cs
│   └── IEntityService.cs
├── Repositories/
│   └── SqlEntityRepository.cs
├── Services/
│   └── EntityService.cs
├── Middleware/
│   └── ExceptionHandlingMiddleware.cs
└── Program.cs
```

---

## ⚠️ Universal Rules (exam point loss if violated)

```
✗ Never hardcode connection string  → up to -5 pts
✗ Never use "new SqlRepo()" in controller → up to -5 pts
✗ Never use @@IDENTITY  → use SCOPE_IDENTITY()
✗ Never "throw ex;"    → always "throw;"  (preserves stack trace)
✗ Never pass plain null to AddWithValue → (object?)val ?? DBNull.Value
✗ Never call GetOrdinal() inside while loop → cache before loop
✗ Never call GetString/GetInt32 on a NULL column → always check IsDBNull first
✗ Never commit bin/ or obj/ to git → .gitignore must be first commit (-50%)
✗ Never put SqlConnection import in Controller
```

---

## 🔑 Exceptions (define once, use everywhere)

```csharp
// Exceptions/NotFoundException.cs
namespace YOUR_PROJECT.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(int id)
        : base($"Entity with id {id} was not found.") { }

    public NotFoundException(string identifier)
        : base($"'{identifier}' was not found.") { }
}
```

```csharp
// Exceptions/ConflictException.cs
namespace YOUR_PROJECT.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
```

**Exception → HTTP mapping:**

| Exception | HTTP | Controller return |
|-----------|------|-------------------|
| `NotFoundException` | 404 | `NotFound(new { message = ex.Message })` |
| `ConflictException` | 409 | `Conflict(new { message = ex.Message })` |
| `ArgumentException` | 400 | `BadRequest(new { message = ex.Message })` |

---

## 🔌 Repository Constructor (always the same)

```csharp
// Repositories/SqlEntityRepository.cs
namespace YOUR_PROJECT.Repositories;
using Microsoft.Data.SqlClient;
using YOUR_PROJECT.Dtos;
using YOUR_PROJECT.Exceptions;
using YOUR_PROJECT.Interfaces;

public class SqlEntityRepository : IEntityRepository
{
    private readonly string _cs;

    public SqlEntityRepository(IConfiguration config)
        => _cs = config.GetConnectionString("DefaultConnection")!;
}
```

---

## GET Patterns

---

### 4a — GET all, flat list, no filters

**When asked:** "Return all X", "List all orders", "Get all customers"  
**URL:** `GET /api/entities`  
**Returns:** `200 OK` + JSON array (empty array if none — never 404 for empty list)

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task<IEnumerable<EntityResponseDto>> GetAllAsync();
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task<IEnumerable<EntityResponseDto>> GetAllAsync()
{
    var list = new List<EntityResponseDto>();
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "SELECT YOUR_COL1, YOUR_COL2, YOUR_COL3 FROM YOUR_TABLE", conn);
    await using var reader = await cmd.ExecuteReaderAsync();

    // Cache ordinals BEFORE the loop — string lookup is expensive inside a loop
    int col1Ord = reader.GetOrdinal("YOUR_COL1");
    int col2Ord = reader.GetOrdinal("YOUR_COL2");
    int col3Ord = reader.GetOrdinal("YOUR_COL3");

    while (await reader.ReadAsync())
        list.Add(new EntityResponseDto
        {
            YOUR_FIELD1 = reader.GetInt32(col1Ord),      // replace GetInt32 with the correct type getter
            YOUR_FIELD2 = reader.GetString(col2Ord),     // e.g. GetString, GetDecimal, GetDateTime
            YOUR_FIELD3 = reader.GetDecimal(col3Ord),
        });

    return list;
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public Task<IEnumerable<EntityResponseDto>> GetAllAsync()
    => _repo.GetAllAsync();  // pass-through — no business logic needed
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
[HttpGet]
public async Task<IActionResult> GetAll()
    => Ok(await _service.GetAllAsync());
```

> **Exam note:** Empty list → `200 OK` + `[]`. Do **not** return `404` for an empty collection.

---

### 4b — GET all with optional query-string filters

**When asked:** "Filter by date", "Filter by status", "Show orders for customer X"  
**URL:** `GET /api/entities?filterId=5&filterDate=2026-05-01`  
**Returns:** `200 OK` + filtered array

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task<IEnumerable<EntityResponseDto>> GetAllAsync(int? filterId, DateTime? filterDate);
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task<IEnumerable<EntityResponseDto>> GetAllAsync(
    int?      filterId   = null,
    DateTime? filterDate = null)
{
    var list = new List<EntityResponseDto>();
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();

    // @FilterId IS NULL means "skip this clause when not provided"
    // Single query handles both filtered and unfiltered cases
    await using var cmd = new SqlCommand(@"
        SELECT e.YOUR_COL1, p.YOUR_COL2 AS YOUR_ALIAS1, e.YOUR_DATE_COL, e.YOUR_STATUS_COL
        FROM YOUR_TABLE e
        JOIN PARENT_TABLE p ON p.Id = e.YOUR_PARENT_FK
        WHERE (@FilterId   IS NULL OR e.YOUR_PARENT_FK = @FilterId)
          AND (@FilterDate IS NULL OR CAST(e.YOUR_DATE_COL AS DATE) = @FilterDate)",
        conn);

    // Plain null is not valid for AddWithValue — always cast first
    cmd.Parameters.AddWithValue("@FilterId",   (object?)filterId   ?? DBNull.Value);
    cmd.Parameters.AddWithValue("@FilterDate", (object?)filterDate ?? DBNull.Value);

    await using var reader = await cmd.ExecuteReaderAsync();

    int col1Ord   = reader.GetOrdinal("YOUR_COL1");
    int aliasOrd  = reader.GetOrdinal("YOUR_ALIAS1");
    int dateOrd   = reader.GetOrdinal("YOUR_DATE_COL");
    int statusOrd = reader.GetOrdinal("YOUR_STATUS_COL");

    while (await reader.ReadAsync())
        list.Add(new EntityResponseDto
        {
            YOUR_FIELD1       = reader.GetInt32(col1Ord),      // e.g. Id — adjust getter type
            YOUR_ALIAS1_FIELD = reader.GetString(aliasOrd),
            YOUR_DATE_FIELD   = reader.GetDateTime(dateOrd),
            YOUR_STATUS_FIELD = reader.GetString(statusOrd),
        });

    return list;
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public Task<IEnumerable<EntityResponseDto>> GetAllAsync(int? filterId, DateTime? filterDate)
    => _repo.GetAllAsync(filterId, filterDate);
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
// GET /api/entities?filterId=5&filterDate=2026-05-01
[HttpGet]
public async Task<IActionResult> GetAll(
    [FromQuery] int?      filterId   = null,
    [FromQuery] DateTime? filterDate = null)
    => Ok(await _service.GetAllAsync(filterId, filterDate));
```

> **Variant:** Filter by string (status, category, name prefix):  
> `[FromQuery] string? status = null` → `AND (@Status IS NULL OR e.Status = @Status)`

---

### 4c — GET single by int ID → 404 if missing

**When asked:** "Get customer by ID", "Return order details", "Fetch a single record"  
**URL:** `GET /api/entities/{id}`  
**Returns:** `200 OK` + object, or `404` if not found

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task<EntityResponseDto?> GetByIdAsync(int id);
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task<EntityResponseDto?> GetByIdAsync(int id)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "SELECT YOUR_COL1, YOUR_COL2, YOUR_NULLABLE_COL, YOUR_COL3 FROM YOUR_TABLE WHERE Id = @Id", conn);
    cmd.Parameters.AddWithValue("@Id", id);
    await using var reader = await cmd.ExecuteReaderAsync();

    if (!await reader.ReadAsync()) return null;  // not found — caller handles 404

    int col1Ord        = reader.GetOrdinal("YOUR_COL1");
    int col2Ord        = reader.GetOrdinal("YOUR_COL2");
    int nullableColOrd = reader.GetOrdinal("YOUR_NULLABLE_COL");  // nullable column
    int col3Ord        = reader.GetOrdinal("YOUR_COL3");

    return new EntityResponseDto
    {
        YOUR_FIELD1 = reader.GetInt32(col1Ord),
        YOUR_FIELD2 = reader.GetString(col2Ord),
        // IsDBNull MUST be checked before reading any nullable column
        YOUR_NULLABLE_FIELD = reader.IsDBNull(nullableColOrd) ? null : reader.GetString(nullableColOrd),
        YOUR_FIELD3 = reader.GetDecimal(col3Ord),
    };
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public async Task<EntityResponseDto> GetByIdAsync(int id)
{
    var result = await _repo.GetByIdAsync(id);
    return result ?? throw new NotFoundException(id);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
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

// ── Without a service layer (direct repo) ────────────────────────
[HttpGet("{id:int}")]
public async Task<IActionResult> GetById(int id)
{
    var result = await _repo.GetByIdAsync(id);
    return result is null ? NotFound() : Ok(result);
}
```

---

### 4d — GET by ID with nested children + items (LEFT JOIN + Dictionary)

**When asked:** "Get customer with all their rentals and movies", "Order with all line items", "Patient with all visits and prescriptions"  
**URL:** `GET /api/entities/{id}/children`  
**Returns:** `200 OK` + nested object, or `404`

> **This is the core exam pattern.** One query, one reader pass, Dictionary to deduplicate parent/child rows.

```csharp
// ── DTOs ─────────────────────────────────────────────────────────
public class ParentResponseDto
{
    public int    Id       { get; set; }
    public string YOUR_COL1  { get; set; } = string.Empty;
    public string YOUR_COL2  { get; set; } = string.Empty;
    public List<ChildDto> Children { get; set; } = new();
}

public class ChildDto
{
    public int       Id         { get; set; }
    public DateTime  YOUR_DATE_FIELD     { get; set; }
    public DateTime? YOUR_NULLABLE_FIELD { get; set; }   // nullable — check IsDBNull
    public string    YOUR_STATUS_FIELD   { get; set; } = string.Empty;
    public List<ItemDto> Items           { get; set; } = new();
}

public class ItemDto
{
    public string  YOUR_ITEM_COL1 { get; set; } = string.Empty;
    public decimal YOUR_ITEM_COL2 { get; set; }
}
```

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task<ParentResponseDto?> GetWithChildrenAsync(int id);
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task<ParentResponseDto?> GetWithChildrenAsync(int id)
{
    ParentResponseDto? result = null;
    // Dictionary keyed by child ID — prevents duplicate child entries
    // JOIN produces one row per (child × item) — without this, child appears N times
    var childDict = new Dictionary<int, ChildDto>();

    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();

    // LEFT JOIN: parent row appears even when no children exist (columns will be NULL)
    // ORDER BY ensures rows for the same child ID are contiguous (not required, but safe)
    const string sql = """
        SELECT
            p.YOUR_PK      AS ParentId,
            p.YOUR_COL1    AS ParentName,
            p.YOUR_COL2    AS ParentEmail,
            c.YOUR_PK      AS ChildId,
            c.YOUR_DATE_COL AS ChildDate,
            c.YOUR_NULLABLE_DATE AS ChildReturn,
            s.YOUR_STATUS_NAME_COL AS StatusName,
            i.YOUR_PK      AS ItemId,
            i.YOUR_ITEM_COL1 AS ItemTitle,
            i.YOUR_ITEM_COL2 AS ItemPrice
        FROM   Parent p
        LEFT JOIN YOUR_CHILD_TABLE  c ON c.YOUR_PARENT_FK = p.YOUR_PK
        LEFT JOIN YOUR_STATUS_TABLE s ON s.YOUR_STATUS_PK = c.YOUR_STATUS_FK
        LEFT JOIN YOUR_ITEM_TABLE   i ON i.YOUR_CHILD_FK  = c.YOUR_PK
        WHERE p.Id = @Id
        ORDER BY c.Id, i.YOUR_ITEM_COL1
        """;

    await using var cmd = new SqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@Id", id);
    await using var reader = await cmd.ExecuteReaderAsync();

    // Cache ALL ordinals before the loop
    int parentIdOrd   = reader.GetOrdinal("ParentId");     // maps to p.YOUR_PK
    int parentNameOrd = reader.GetOrdinal("ParentName");   // maps to p.YOUR_COL1
    int parentEmailOrd= reader.GetOrdinal("ParentEmail");  // maps to p.YOUR_COL2
    int childIdOrd    = reader.GetOrdinal("ChildId");      // maps to c.YOUR_PK
    int childDateOrd  = reader.GetOrdinal("ChildDate");    // maps to c.YOUR_DATE_COL
    int childRetOrd   = reader.GetOrdinal("ChildReturn");  // maps to c.YOUR_NULLABLE_DATE
    int statusOrd     = reader.GetOrdinal("StatusName");   // maps to s.YOUR_STATUS_NAME_COL
    int itemIdOrd     = reader.GetOrdinal("ItemId");       // maps to i.YOUR_PK
    int itemTitleOrd  = reader.GetOrdinal("ItemTitle");    // maps to i.YOUR_ITEM_COL1
    int itemPriceOrd  = reader.GetOrdinal("ItemPrice");    // maps to i.YOUR_ITEM_COL2

    while (await reader.ReadAsync())
    {
        // Build parent only on the very first row
        if (result is null)
        {
            result = new ParentResponseDto
            {
                Id    = reader.GetInt32(parentIdOrd),
                YOUR_COL1 = reader.GetString(parentNameOrd),
                YOUR_COL2 = reader.GetString(parentEmailOrd),
            };
        }

        // LEFT JOIN → child columns are NULL when parent has no children at all
        if (reader.IsDBNull(childIdOrd)) continue;

        int childId = reader.GetInt32(childIdOrd);

        // TryGetValue avoids duplicate child entries from the JOIN expansion
        if (!childDict.TryGetValue(childId, out var childDto))
        {
            childDto = new ChildDto
            {
                Id                  = childId,
                YOUR_DATE_FIELD     = reader.GetDateTime(childDateOrd),
                YOUR_NULLABLE_FIELD = reader.IsDBNull(childRetOrd)
                             ? null
                             : reader.GetDateTime(childRetOrd),
                YOUR_STATUS_FIELD = reader.GetString(statusOrd),
            };
            childDict[childId] = childDto;
            result.Children.Add(childDto);
        }

        // Add item to child if present (also nullable via LEFT JOIN)
        if (!reader.IsDBNull(itemIdOrd))
        {
            childDto.Items.Add(new ItemDto
            {
                YOUR_ITEM_COL1 = reader.GetString(itemTitleOrd),
                YOUR_ITEM_COL2 = reader.GetDecimal(itemPriceOrd),
            });
        }
    }

    return result;  // null → caller throws NotFoundException
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public async Task<ParentResponseDto> GetWithChildrenAsync(int id)
{
    var result = await _repo.GetWithChildrenAsync(id);
    return result ?? throw new NotFoundException(id);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
// GET /api/parents/{id}/rentals     (or /orders, /visits, /children — whatever fits)
[HttpGet("{id:int}/children")]
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
[HttpGet("{id:int}/children")]
public async Task<IActionResult> GetWithChildren(int id)
{
    var result = await _repo.GetWithChildrenAsync(id);
    return result is null
        ? NotFound(new { message = $"Entity {id} not found." })
        : Ok(result);
}
```

> **Exam variants of this pattern:**
> - Customer → Rentals → Movies  
> - Doctor → Appointments → Prescriptions  
> - Patient → Visits → Procedures  
> - Order → OrderLines → Products  
> - Student → Enrollments → Courses  

---

### 4e — GET by lookup field (PESEL, ISBN, email, code)

**When asked:** "Find patient by PESEL", "Search book by ISBN", "Get user by email"  
**URL:** `GET /api/entities/lookup/{value}`  
**Returns:** `200 OK` + object, or `404`

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task<EntityResponseDto?> GetByFieldAsync(string fieldValue);
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task<EntityResponseDto?> GetByFieldAsync(string fieldValue)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "SELECT YOUR_COL1, YOUR_COL2, YOUR_NULLABLE_COL FROM YOUR_TABLE WHERE YOUR_UNIQUE_COL = @Val", conn);
    cmd.Parameters.AddWithValue("@Val", fieldValue);
    await using var reader = await cmd.ExecuteReaderAsync();

    if (!await reader.ReadAsync()) return null;

    int col1Ord        = reader.GetOrdinal("YOUR_COL1");
    int col2Ord        = reader.GetOrdinal("YOUR_COL2");
    int nullableColOrd = reader.GetOrdinal("YOUR_NULLABLE_COL");

    return new EntityResponseDto
    {
        YOUR_FIELD1         = reader.GetInt32(col1Ord),
        YOUR_FIELD2         = reader.GetString(col2Ord),
        YOUR_NULLABLE_FIELD = reader.IsDBNull(nullableColOrd) ? null : reader.GetString(nullableColOrd),
    };
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
// Also used internally by CreateAsync to check for duplicates before INSERT
public async Task<EntityResponseDto> GetByFieldAsync(string fieldValue)
{
    var result = await _repo.GetByFieldAsync(fieldValue);
    return result ?? throw new NotFoundException(fieldValue);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
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

### 4f — GET with aggregation / summary / analytics

**When asked:** "Average grade per course", "Total spend per customer", "Count of orders by status"  
**URL:** `GET /api/entities/summary`  
**Returns:** `200 OK` + array of summary objects

```csharp
// ── DTO ──────────────────────────────────────────────────────────
public class SummaryDto
{
    public string  YOUR_GROUP_KEY { get; set; } = string.Empty;
    public int     YOUR_COUNT_COL { get; set; }
    public decimal YOUR_AVG_COL   { get; set; }
    public decimal YOUR_TOTAL_COL { get; set; }
}
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task<IEnumerable<SummaryDto>> GetSummaryAsync()
{
    var list = new List<SummaryDto>();
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(@"
        SELECT
            c.YOUR_GROUP_COL AS YOUR_GROUP_KEY,
            COUNT(*)         AS YOUR_COUNT_COL,
            AVG(e.YOUR_VALUE_COL)  AS YOUR_AVG_COL,
            SUM(e.YOUR_VALUE_COL)  AS YOUR_TOTAL_COL
        FROM YOUR_TABLE e
        JOIN YOUR_GROUP_TABLE c ON c.Id = e.YOUR_GROUP_FK
        GROUP BY c.YOUR_GROUP_COL
        ORDER BY c.YOUR_GROUP_COL", conn);
    await using var reader = await cmd.ExecuteReaderAsync();

    int grpOrd = reader.GetOrdinal("YOUR_GROUP_KEY");
    int cntOrd = reader.GetOrdinal("YOUR_COUNT_COL");
    int avgOrd = reader.GetOrdinal("YOUR_AVG_COL");
    int totOrd = reader.GetOrdinal("YOUR_TOTAL_COL");

    while (await reader.ReadAsync())
        list.Add(new SummaryDto
        {
            YOUR_GROUP_KEY = reader.GetString(grpOrd),
            YOUR_COUNT_COL = reader.GetInt32(cntOrd),
            YOUR_AVG_COL   = reader.GetDecimal(avgOrd),
            YOUR_TOTAL_COL = reader.GetDecimal(totOrd),
        });

    return list;
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public Task<IEnumerable<SummaryDto>> GetSummaryAsync()
    => _repo.GetSummaryAsync();
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
[HttpGet("summary")]
public async Task<IActionResult> GetSummary()
    => Ok(await _service.GetSummaryAsync());
```

---

## POST Patterns

---

### 4g — POST simple INSERT → returns 201 with new ID

**When asked:** "Create a new X", "Add an entity", "Register Y"  
**URL:** `POST /api/entities`  
**Body:** JSON object  
**Returns:** `201 Created` + `{ id: <newId> }`

```csharp
// ── REQUEST DTO ──────────────────────────────────────────────────
using System.ComponentModel.DataAnnotations;

public class EntityRequestDto
{
    [Required]
    [MaxLength(200)]
    public string  YOUR_FIELD1 { get; set; } = string.Empty;  // e.g. Name, Title, Code

    [Range(0.01, 99999.99)]
    public decimal YOUR_FIELD2 { get; set; }                  // e.g. Price, Amount

    [EmailAddress]
    public string? YOUR_NULLABLE_FIELD { get; set; }          // nullable field, e.g. Email

    public DateTime YOUR_FIELD3 { get; set; }                 // e.g. Date, ScheduledAt
}
```

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task<int> AddAsync(EntityRequestDto dto);
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task<int> AddAsync(EntityRequestDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand("""
        INSERT INTO YOUR_TABLE (YOUR_COL1, YOUR_COL2, YOUR_NULLABLE_COL, YOUR_COL3)
        VALUES (@Val1, @Val2, @NullableVal, @Val3);
        SELECT SCOPE_IDENTITY();
        """, conn);

    cmd.Parameters.AddWithValue("@Val1", dto.YOUR_FIELD1);
    cmd.Parameters.AddWithValue("@Val2", dto.YOUR_FIELD2);
    // Nullable field: must use DBNull.Value, not null
    cmd.Parameters.AddWithValue("@NullableVal", (object?)dto.YOUR_NULLABLE_FIELD ?? DBNull.Value);
    cmd.Parameters.AddWithValue("@Val3", dto.YOUR_FIELD3);

    // ExecuteScalar returns the value of SCOPE_IDENTITY() — the new auto-generated ID
    // Convert.ToInt32 handles the decimal type that SCOPE_IDENTITY() returns
    return Convert.ToInt32(await cmd.ExecuteScalarAsync());
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public async Task<int> CreateAsync(EntityRequestDto dto)
{
    // Optional duplicate check before inserting
    var existing = await _repo.GetByFieldAsync(dto.YOUR_UNIQUE_FIELD);
    if (existing is not null)
        throw new ConflictException($"'{dto.YOUR_UNIQUE_FIELD}' already exists.");

    return await _repo.AddAsync(dto);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
// POST /api/entities
[HttpPost]
public async Task<IActionResult> Create([FromBody] EntityRequestDto dto)
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

// ── Without a service layer ───────────────────────────────────────
[HttpPost]
public async Task<IActionResult> Create([FromBody] EntityRequestDto dto)
{
    int newId = await _repo.AddAsync(dto);
    return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
}
```

> **WHY `SCOPE_IDENTITY()` not `@@IDENTITY`:**  
> `@@IDENTITY` returns the last identity generated anywhere in the session — including by triggers on other tables. `SCOPE_IDENTITY()` is scoped to your current command. Always use `SCOPE_IDENTITY()`.

---

### 4h — POST transactional INSERT (multi-step, the exam POST pattern)

**When asked:** "Create a rental with movies", "Add an order with line items", "Book an appointment with services"  
**URL:** `POST /api/parents/{id}/children`  
**Body:** JSON with a list of items  
**Returns:** `201 Created`, `404` if parent or lookup missing, `409` if conflict

```csharp
// ── REQUEST DTOs ─────────────────────────────────────────────────
public class ParentChildRequestDto
{
    public DateTime             YOUR_DATE_FIELD { get; set; }   // e.g. Date, ScheduledAt
    public List<ItemRequestDto> Items           { get; set; } = new();
}

public class ItemRequestDto
{
    [Required]
    public string  YOUR_LOOKUP_FIELD { get; set; } = string.Empty;  // lookup key e.g. Title, ISBN, Code
    public decimal YOUR_VALUE_FIELD  { get; set; }                  // e.g. Price, Amount
}
```

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task<int> AddWithTransactionAsync(int parentId, ParentChildRequestDto dto);
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task<int> AddWithTransactionAsync(int parentId, ParentChildRequestDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    // ALL commands inside this method MUST pass both conn AND tx
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // ── Step 1: Verify parent exists ─────────────────────────
        await using var checkParent = new SqlCommand(
            "SELECT COUNT(1) FROM PARENT_TABLE WHERE Id = @Id",
            conn, tx);
        checkParent.Parameters.AddWithValue("@Id", parentId);
        if ((int)(await checkParent.ExecuteScalarAsync())! == 0)
            throw new NotFoundException(parentId);

        // ── Step 2: Resolve each item lookup field → database ID ──
        // Collect ALL resolved IDs first — do NOT INSERT inside this loop
        var resolvedItems = new List<(int LookupId, decimal Value)>();
        foreach (var item in dto.Items)
        {
            await using var findItem = new SqlCommand(
                "SELECT Id FROM YOUR_LOOKUP_TABLE WHERE YOUR_LOOKUP_COL = @LookupVal",
                conn, tx);
            findItem.Parameters.AddWithValue("@LookupVal", item.YOUR_LOOKUP_FIELD);
            var found = await findItem.ExecuteScalarAsync();
            if (found is null)
                throw new NotFoundException(item.YOUR_LOOKUP_FIELD);  // string overload
            resolvedItems.Add(((int)found, item.YOUR_VALUE_FIELD));
        }

        // ── Step 3: Resolve default status ID ────────────────────
        await using var getStatus = new SqlCommand(
            "SELECT Id FROM YOUR_STATUS_TABLE WHERE YOUR_STATUS_NAME_COL = 'YOUR_DEFAULT_STATUS'",
            conn, tx);
        int statusId = (int)(await getStatus.ExecuteScalarAsync())!;

        // ── Step 4: INSERT the header row, get new ID ────────────
        await using var insertHeader = new SqlCommand("""
            INSERT INTO YOUR_CHILD_TABLE (YOUR_DATE_COL, YOUR_PARENT_FK, YOUR_STATUS_FK)
            VALUES (@DateVal, @ParentId, @StatusId);
            SELECT SCOPE_IDENTITY();
            """, conn, tx);
        insertHeader.Parameters.AddWithValue("@DateVal",  dto.YOUR_DATE_FIELD);
        insertHeader.Parameters.AddWithValue("@ParentId", parentId);
        insertHeader.Parameters.AddWithValue("@StatusId", statusId);
        int newId = Convert.ToInt32(await insertHeader.ExecuteScalarAsync());

        // ── Step 5: INSERT line items in a loop ──────────────────
        foreach (var (lookupId, value) in resolvedItems)
        {
            await using var insertItem = new SqlCommand("""
                INSERT INTO YOUR_ITEM_TABLE (YOUR_CHILD_FK, YOUR_LOOKUP_FK, YOUR_VALUE_COL)
                VALUES (@ChildId, @LookupId, @Value)
                """, conn, tx);
            insertItem.Parameters.AddWithValue("@ChildId",  newId);
            insertItem.Parameters.AddWithValue("@LookupId", lookupId);
            insertItem.Parameters.AddWithValue("@Value",    value);
            await insertItem.ExecuteNonQueryAsync();
        }

        await tx.CommitAsync();
        return newId;
    }
    catch
    {
        await tx.RollbackAsync();
        throw;  // "throw;" preserves stack trace — never write "throw ex;"
    }
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public async Task<int> CreateAsync(int parentId, ParentChildRequestDto dto)
{
    // Business rules before hitting the DB
    if (!dto.Items.Any())
        throw new ArgumentException("At least one item is required.");

    if (dto.YOUR_DATE_FIELD < DateTime.UtcNow)   // remove if date doesn't need to be in the future
        throw new ArgumentException("Date must be in the future.");

    // NotFoundException / ConflictException from repo propagate automatically
    return await _repo.AddWithTransactionAsync(parentId, dto);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
// POST /api/parents/{id}/rentals
[HttpPost("{id:int}/children")]
public async Task<IActionResult> AddChild(int id, [FromBody] ParentChildRequestDto dto)
{
    try
    {
        int newId = await _service.CreateAsync(id, dto);
        return Created($"api/parents/{id}/children", new { id = newId });
    }
    catch (NotFoundException ex)  { return NotFound(new { message = ex.Message }); }
    catch (ConflictException ex)  { return Conflict(new { message = ex.Message }); }
    catch (ArgumentException ex)  { return BadRequest(new { message = ex.Message }); }
}
```

---

### 4i — POST with overlap / conflict check inside transaction

**When asked:** "Book a time slot", "Reserve a resource", "Schedule avoiding double-booking"

Add this block **inside the transaction, between Step 2 and Step 3** of pattern 4h:

```csharp
// ── REPOSITORY snippet ───────────────────────────────────────────
// Insert inside the transaction in pattern 4h, between Step 2 and Step 3:
// Check no existing booking overlaps with [Start, Start+Duration)
await using var checkOverlap = new SqlCommand(@"
    SELECT COUNT(*)
    FROM YOUR_BOOKING_TABLE
    WHERE YOUR_RESOURCE_FK = @ResourceId
      AND YOUR_STATUS_COL != 'YOUR_CANCELLED_STATUS'
      AND @Start < DATEADD(minute, YOUR_DURATION_COL, YOUR_START_COL)
      AND DATEADD(minute, @Dur, @Start) > YOUR_START_COL",
    conn, tx);
checkOverlap.Parameters.AddWithValue("@ResourceId", dto.YOUR_RESOURCE_FIELD);
checkOverlap.Parameters.AddWithValue("@Start",      dto.YOUR_START_FIELD);
checkOverlap.Parameters.AddWithValue("@Dur",        dto.YOUR_DURATION_FIELD);

if ((int)(await checkOverlap.ExecuteScalarAsync())! > 0)
    throw new ConflictException("Time slot is already booked.");
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
// No change needed — ConflictException from the repo propagates automatically.
// Optionally add date/time validation here before calling the repo:
public async Task<int> CreateAsync(int parentId, ParentChildRequestDto dto)
{
    if (dto.YOUR_START_FIELD < DateTime.UtcNow)
        throw new ArgumentException("Start time must be in the future.");

    return await _repo.AddWithTransactionAsync(parentId, dto);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
[HttpPost("{id:int}/children")]
public async Task<IActionResult> AddChild(int id, [FromBody] ParentChildRequestDto dto)
{
    try
    {
        int newId = await _service.CreateAsync(id, dto);
        return Created($"api/YOUR_ROUTE/{id}/children", new { id = newId });
    }
    catch (NotFoundException ex)  { return NotFound(new { message = ex.Message }); }
    catch (ConflictException ex)  { return Conflict(new { message = ex.Message }); }
    catch (ArgumentException ex)  { return BadRequest(new { message = ex.Message }); }
}
```

---

### 4j — POST checking existence of a referenced entity (FK validation)

**When asked:** "Assign doctor to appointment", "Assign instructor to course" — where you need to validate a FK before insert.

```csharp
// ── REPOSITORY snippet ───────────────────────────────────────────
// Insert inside transaction, before INSERT:
await using var checkRef = new SqlCommand(
    "SELECT COUNT(1) FROM YOUR_REFERENCED_TABLE WHERE Id = @RefId",
    conn, tx);
checkRef.Parameters.AddWithValue("@RefId", dto.YOUR_REFERENCED_ID_FIELD);
if ((int)(await checkRef.ExecuteScalarAsync())! == 0)
    throw new NotFoundException(dto.YOUR_REFERENCED_ID_FIELD);
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
// No extra logic needed — NotFoundException from repo propagates automatically.
public async Task<int> CreateAsync(EntityRequestDto dto)
{
    return await _repo.AddAsync(dto);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
[HttpPost]
public async Task<IActionResult> Create([FromBody] EntityRequestDto dto)
{
    try
    {
        int newId = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
    }
    catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    catch (ConflictException ex) { return Conflict(new { message = ex.Message }); }
}
```

---

## PUT / PATCH / DELETE Patterns

---

### 4k — PUT full update → 204 on success, 404 if not found

**When asked:** "Update entity", "Replace all fields of X", "Modify a record"  
**URL:** `PUT /api/entities/{id}`  
**Returns:** `204 No Content`, or `404`

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task<bool> UpdateAsync(int id, EntityRequestDto dto);
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task<bool> UpdateAsync(int id, EntityRequestDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "UPDATE YOUR_TABLE SET YOUR_COL1 = @Val1, YOUR_COL2 = @Val2 WHERE Id = @Id", conn);
    cmd.Parameters.AddWithValue("@Id",   id);
    cmd.Parameters.AddWithValue("@Val1", dto.YOUR_FIELD1);
    cmd.Parameters.AddWithValue("@Val2", dto.YOUR_FIELD2);
    // ExecuteNonQueryAsync returns rows affected — 0 means ID did not exist
    return await cmd.ExecuteNonQueryAsync() > 0;
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public async Task UpdateAsync(int id, EntityRequestDto dto)
{
    bool updated = await _repo.UpdateAsync(id, dto);
    if (!updated) throw new NotFoundException(id);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
[HttpPut("{id:int}")]
public async Task<IActionResult> Update(int id, [FromBody] EntityRequestDto dto)
{
    try
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();  // 204 — success, no body
    }
    catch (NotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}

// ── Without a service layer ───────────────────────────────────────
[HttpPut("{id:int}")]
public async Task<IActionResult> Update(int id, [FromBody] EntityRequestDto dto)
{
    bool ok = await _repo.UpdateAsync(id, dto);
    return ok ? NoContent() : NotFound();
}
```

---

### 4l — PATCH status-only update (cancel, complete, return, activate)

**When asked:** "Cancel a booking", "Mark order as shipped", "Return a rental", "Close a ticket"  
**URL:** `PATCH /api/entities/{id}/status`  
**Body:** `"cancelled"` (plain string)  
**Returns:** `204 No Content`, `404`, or `400` for invalid status

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task<bool> UpdateStatusAsync(int id, string newStatus);
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
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

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public async Task UpdateStatusAsync(int id, string newStatus)
{
    // Validate allowed values before touching the DB
    var valid = new[] { "active", "cancelled", "completed", "pending" };
    if (!valid.Contains(newStatus))
        throw new ArgumentException($"Invalid status '{newStatus}'.");

    bool updated = await _repo.UpdateStatusAsync(id, newStatus);
    if (!updated) throw new NotFoundException(id);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
// PATCH /api/entities/{id}/status
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

> **Variant — PATCH with a DTO instead of plain string:**
> ```csharp
> public class StatusUpdateDto { public string Status { get; set; } = string.Empty; }
> // Controller: [FromBody] StatusUpdateDto dto  →  _service.UpdateStatusAsync(id, dto.Status)
> ```

---

### 4m — PATCH set a nullable date field (return date, end date, resolved at)

**When asked:** "Mark as returned", "Set completion date", "Record return of a rental"  
**URL:** `PATCH /api/entities/{id}/return`  
**Returns:** `204`, or `404`

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task<bool> SetReturnDateAsync(int id, DateTime date);
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task<bool> SetReturnDateAsync(int id, DateTime returnDate)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "UPDATE YOUR_TABLE SET YOUR_DATE_COL = @Date WHERE Id = @Id", conn);
    cmd.Parameters.AddWithValue("@Id",   id);
    cmd.Parameters.AddWithValue("@Date", returnDate);
    return await cmd.ExecuteNonQueryAsync() > 0;
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public async Task SetReturnDateAsync(int id, DateTime date)
{
    bool updated = await _repo.SetReturnDateAsync(id, date);
    if (!updated) throw new NotFoundException(id);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
// PATCH /api/entities/{id}/return
[HttpPatch("{id:int}/return")]
public async Task<IActionResult> Return(int id)
{
    try
    {
        await _service.SetReturnDateAsync(id, DateTime.UtcNow);
        return NoContent();
    }
    catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
}
```

---

### 4n — DELETE → 204 on success, 404 if not found

**When asked:** "Remove an entity", "Delete a record", "Unregister X"  
**URL:** `DELETE /api/entities/{id}`  
**Returns:** `204 No Content`, or `404`

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task<bool> DeleteAsync(int id);
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task<bool> DeleteAsync(int id)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "DELETE FROM YOUR_TABLE WHERE Id = @Id", conn);
    cmd.Parameters.AddWithValue("@Id", id);
    return await cmd.ExecuteNonQueryAsync() > 0;
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public async Task DeleteAsync(int id)
{
    bool deleted = await _repo.DeleteAsync(id);
    if (!deleted) throw new NotFoundException(id);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
[HttpDelete("{id:int}")]
public async Task<IActionResult> Delete(int id)
{
    try
    {
        await _service.DeleteAsync(id);
        return NoContent();
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

### 4o — PUT transactional update (replace header + delete old items + insert new items)

**When asked:** "Replace an order's line items", "Update a rental and its movies", "Reassign all services on a booking"  
**URL:** `PUT /api/parents/{id}/children`  
**Body:** new state for the whole aggregate  
**Returns:** `204 No Content`, `404` if entity not found, `404` if a lookup item is missing

> The key insight: SQL `UPDATE` on a multi-row child relationship is almost always easier as **DELETE all children + re-INSERT** inside one transaction. This avoids diffing old vs new rows.

```csharp
// ── REQUEST DTO ──────────────────────────────────────────────────
public class UpdateWithItemsDto
{
    public DateTime             YOUR_DATE_FIELD { get; set; }   // e.g. Date, ScheduledAt
    public List<ItemRequestDto> Items           { get; set; } = new();
}
```

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task UpdateWithItemsAsync(int id, UpdateWithItemsDto dto);
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task UpdateWithItemsAsync(int id, UpdateWithItemsDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // ── Step 1: Verify header row exists ─────────────────────
        await using var checkExists = new SqlCommand(
            "SELECT COUNT(1) FROM PARENT_TABLE WHERE Id = @Id",
            conn, tx);
        checkExists.Parameters.AddWithValue("@Id", id);
        if ((int)(await checkExists.ExecuteScalarAsync())! == 0)
            throw new NotFoundException(id);

        // ── Step 2: Update the header fields ─────────────────────
        await using var updateHeader = new SqlCommand(
            "UPDATE YOUR_PARENT_TABLE SET YOUR_DATE_COL = @DateVal WHERE Id = @Id",
            conn, tx);
        updateHeader.Parameters.AddWithValue("@Id",      id);
        updateHeader.Parameters.AddWithValue("@DateVal", dto.YOUR_DATE_FIELD);
        await updateHeader.ExecuteNonQueryAsync();

        // ── Step 3: Resolve all item lookup fields → IDs before deleting ─
        // Do this BEFORE deleting — if any lookup is missing, roll back cleanly
        var resolvedItems = new List<(int LookupId, decimal Value)>();
        foreach (var item in dto.Items)
        {
            await using var findItem = new SqlCommand(
                "SELECT Id FROM YOUR_LOOKUP_TABLE WHERE YOUR_LOOKUP_COL = @LookupVal",
                conn, tx);
            findItem.Parameters.AddWithValue("@LookupVal", item.YOUR_LOOKUP_FIELD);
            var found = await findItem.ExecuteScalarAsync();
            if (found is null)
                throw new NotFoundException(item.YOUR_LOOKUP_FIELD);
            resolvedItems.Add(((int)found, item.YOUR_VALUE_FIELD));
        }

        // ── Step 4: Delete ALL existing child rows for this parent ─
        // Simpler and safer than trying to diff old vs new
        await using var deleteItems = new SqlCommand(
            "DELETE FROM YOUR_ITEM_TABLE WHERE YOUR_PARENT_FK = @Id",
            conn, tx);
        deleteItems.Parameters.AddWithValue("@Id", id);
        await deleteItems.ExecuteNonQueryAsync();

        // ── Step 5: Re-INSERT the new child rows ──────────────────
        foreach (var (lookupId, value) in resolvedItems)
        {
            await using var insertItem = new SqlCommand("""
                INSERT INTO YOUR_ITEM_TABLE (YOUR_PARENT_FK, YOUR_LOOKUP_FK, YOUR_VALUE_COL)
                VALUES (@ParentId, @LookupId, @Value)
                """, conn, tx);
            insertItem.Parameters.AddWithValue("@ParentId", id);
            insertItem.Parameters.AddWithValue("@LookupId", lookupId);
            insertItem.Parameters.AddWithValue("@Value",    value);
            await insertItem.ExecuteNonQueryAsync();
        }

        await tx.CommitAsync();
    }
    catch
    {
        await tx.RollbackAsync();
        throw;
    }
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public async Task UpdateWithItemsAsync(int id, UpdateWithItemsDto dto)
{
    if (!dto.Items.Any())
        throw new ArgumentException("At least one item is required.");

    await _repo.UpdateWithItemsAsync(id, dto);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
// PUT /api/parents/{id}
[HttpPut("{id:int}")]
public async Task<IActionResult> Update(int id, [FromBody] UpdateWithItemsDto dto)
{
    try
    {
        await _service.UpdateWithItemsAsync(id, dto);
        return NoContent();
    }
    catch (NotFoundException ex)  { return NotFound(new { message = ex.Message }); }
    catch (ArgumentException ex)  { return BadRequest(new { message = ex.Message }); }
}
```

> **Real-world exam variants:**
> - Update an order → delete old `OrderLine` rows → insert new `OrderLine` rows  
> - Update a prescription → delete old `PrescriptionDrug` rows → insert new ones  
> - Update a rental → delete `Rental_Item` rows → insert new `Rental_Item` rows  

---

### 4p — PATCH transactional status update with side-effects across tables

**When asked:** "Cancel an order and restore stock", "Complete a booking and charge the customer", "Approve a request and notify via a log table"  
**URL:** `PATCH /api/entities/{id}/status`  
**Returns:** `204 No Content`, `404`, `409` if transition invalid

> The difference from simple 4l: status change must also write to other tables atomically (stock, log, audit trail, linked records).

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task UpdateStatusWithSideEffectsAsync(int id, string newStatus);
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task UpdateStatusWithSideEffectsAsync(int id, string newStatus)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // ── Step 1: Read current state ────────────────────────────
        await using var readCurrent = new SqlCommand(
            "SELECT YOUR_STATUS_COL FROM YOUR_TABLE WHERE Id = @Id",
            conn, tx);
        readCurrent.Parameters.AddWithValue("@Id", id);
        var currentStatusObj = await readCurrent.ExecuteScalarAsync();
        if (currentStatusObj is null)
            throw new NotFoundException(id);
        string currentStatus = (string)currentStatusObj;  // YOUR_STATUS_COL value

        // ── Step 2: Guard invalid state transitions ───────────────
        // e.g. cannot cancel something already completed
        if (currentStatus == "completed" && newStatus == "cancelled")
            throw new ConflictException(
                "Cannot cancel a completed entity.");

        if (currentStatus == newStatus)
            throw new ConflictException(
                $"Entity is already '{newStatus}'.");

        // ── Step 3: Update primary table status ───────────────────
        await using var updateStatus = new SqlCommand(
            "UPDATE YOUR_TABLE SET Status = @Status, YOUR_UPDATED_AT_COL = @Now WHERE Id = @Id",
            conn, tx);
        updateStatus.Parameters.AddWithValue("@Id",     id);
        updateStatus.Parameters.AddWithValue("@Status", newStatus);
        updateStatus.Parameters.AddWithValue("@Now",    DateTime.UtcNow);
        await updateStatus.ExecuteNonQueryAsync();

        // ── Step 4: Side-effect A — write audit/history log ───────
        await using var insertLog = new SqlCommand("""
            INSERT INTO STATUS_LOG (EntityId, OldStatus, NewStatus, YOUR_TIMESTAMP_COL)
            VALUES (@EntityId, @OldVal, @NewVal, @Now)
            """, conn, tx);
        insertLog.Parameters.AddWithValue("@EntityId", id);
        insertLog.Parameters.AddWithValue("@OldVal",      currentStatus);
        insertLog.Parameters.AddWithValue("@NewVal",      newStatus);
        insertLog.Parameters.AddWithValue("@Now",      DateTime.UtcNow);
        await insertLog.ExecuteNonQueryAsync();

        // ── Step 4b: Side-effect B — restore stock on cancellation ─
        // (adapt or remove this block depending on domain)
        if (newStatus == "cancelled")
        {
            await using var getItems = new SqlCommand(
                "SELECT YOUR_PRODUCT_FK, YOUR_QTY_COL FROM YOUR_LINE_TABLE WHERE YOUR_PARENT_FK = @Id",
                conn, tx);
            getItems.Parameters.AddWithValue("@Id", id);
            await using var itemReader = await getItems.ExecuteReaderAsync();

            var toRestore = new List<(int ProductId, int Qty)>();
            int prodOrd = itemReader.GetOrdinal("YOUR_PRODUCT_FK");
            int qtyOrd  = itemReader.GetOrdinal("YOUR_QTY_COL");
            while (await itemReader.ReadAsync())
                toRestore.Add((itemReader.GetInt32(prodOrd), itemReader.GetInt32(qtyOrd)));
            await itemReader.CloseAsync();  // MUST close reader before running more commands on same conn

            foreach (var (productId, qty) in toRestore)
            {
                await using var restoreStock = new SqlCommand(
                    "UPDATE YOUR_STOCK_TABLE SET YOUR_STOCK_COL = YOUR_STOCK_COL + @Qty WHERE Id = @ProdId",
                    conn, tx);
                restoreStock.Parameters.AddWithValue("@Qty",    qty);
                restoreStock.Parameters.AddWithValue("@ProdId", productId);
                await restoreStock.ExecuteNonQueryAsync();
            }
        }

        await tx.CommitAsync();
    }
    catch
    {
        await tx.RollbackAsync();
        throw;
    }
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public async Task UpdateStatusWithSideEffectsAsync(int id, string newStatus)
{
    var valid = new[] { "pending", "active", "cancelled", "completed" };
    if (!valid.Contains(newStatus))
        throw new ArgumentException($"Invalid status '{newStatus}'.");

    await _repo.UpdateStatusWithSideEffectsAsync(id, newStatus);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
[HttpPatch("{id:int}/status")]
public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
{
    try
    {
        await _service.UpdateStatusWithSideEffectsAsync(id, newStatus);
        return NoContent();
    }
    catch (NotFoundException ex)  { return NotFound(new { message = ex.Message }); }
    catch (ConflictException ex)  { return Conflict(new { message = ex.Message }); }
    catch (ArgumentException ex)  { return BadRequest(new { message = ex.Message }); }
}
```

> **Important:** When you need to read rows with a `SqlDataReader` and then run more commands **on the same connection inside the same transaction**, you MUST `CloseAsync()` the reader first. Two open readers on one connection throws `InvalidOperationException`.

---

### 4q — DELETE transactional (cascade children manually, then delete parent)

**When asked:** "Delete a customer and all their data", "Remove an order including its lines", "Delete a patient and all visits"  
**URL:** `DELETE /api/entities/{id}`  
**Returns:** `204 No Content`, `404` if not found, `409` if deletion is blocked by business rules

> SQL `ON DELETE CASCADE` handles this at the DB level — but if the schema does **not** have cascade, or if you need **business-rule checks** before deleting, you must do it manually in a transaction.

```csharp
// ── INTERFACE ────────────────────────────────────────────────────
Task DeleteWithCascadeAsync(int id);
```

```csharp
// ── REPOSITORY ───────────────────────────────────────────────────
public async Task DeleteWithCascadeAsync(int id)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // ── Step 1: Verify the parent exists ─────────────────────
        await using var checkExists = new SqlCommand(
            "SELECT COUNT(1) FROM PARENT_TABLE WHERE Id = @Id",
            conn, tx);
        checkExists.Parameters.AddWithValue("@Id", id);
        if ((int)(await checkExists.ExecuteScalarAsync())! == 0)
            throw new NotFoundException(id);

        // ── Step 2: Business-rule guard (optional) ────────────────
        // e.g. cannot delete an order that has been shipped
        await using var checkStatus = new SqlCommand(
            "SELECT YOUR_STATUS_COL FROM PARENT_TABLE WHERE Id = @Id",
            conn, tx);
        checkStatus.Parameters.AddWithValue("@Id", id);
        var status = (string)(await checkStatus.ExecuteScalarAsync())!;  // YOUR_STATUS_COL
        if (status == "completed")  // replace with YOUR blocking status value
            throw new ConflictException(
                "Cannot delete a completed entity.");

        // ── Step 3: Delete grandchildren first (deepest FK level) ─
        // FK order: grandchild → child → parent (reverse of INSERT order)
        await using var deleteGrandChildren = new SqlCommand(
            """
            DELETE gc
            FROM YOUR_GRANDCHILD_TABLE gc
            JOIN YOUR_CHILD_TABLE c ON c.YOUR_PK = gc.YOUR_CHILD_FK
            WHERE c.YOUR_PARENT_FK = @Id
            """, conn, tx);
        deleteGrandChildren.Parameters.AddWithValue("@Id", id);
        await deleteGrandChildren.ExecuteNonQueryAsync();

        // ── Step 4: Delete children ───────────────────────────────
        await using var deleteChildren = new SqlCommand(
            "DELETE FROM CHILD_TABLE WHERE ParentId = @Id",
            conn, tx);
        deleteChildren.Parameters.AddWithValue("@Id", id);
        await deleteChildren.ExecuteNonQueryAsync();

        // ── Step 5: Delete the parent ─────────────────────────────
        await using var deleteParent = new SqlCommand(
            "DELETE FROM PARENT_TABLE WHERE Id = @Id",
            conn, tx);
        deleteParent.Parameters.AddWithValue("@Id", id);
        await deleteParent.ExecuteNonQueryAsync();

        await tx.CommitAsync();
    }
    catch
    {
        await tx.RollbackAsync();
        throw;
    }
}
```

```csharp
// ── SERVICE ──────────────────────────────────────────────────────
public async Task DeleteWithCascadeAsync(int id)
{
    // NotFoundException and ConflictException from repo propagate automatically
    await _repo.DeleteWithCascadeAsync(id);
}
```

```csharp
// ── CONTROLLER ───────────────────────────────────────────────────
[HttpDelete("{id:int}")]
public async Task<IActionResult> Delete(int id)
{
    try
    {
        await _service.DeleteWithCascadeAsync(id);
        return NoContent();
    }
    catch (NotFoundException ex)  { return NotFound(new { message = ex.Message }); }
    catch (ConflictException ex)  { return Conflict(new { message = ex.Message }); }
}
```

> **FK deletion order rule:** always delete in reverse of INSERT order.  
> If INSERT goes `Parent → Child → Grandchild`, DELETE goes `Grandchild → Child → Parent`.  
> Violating this order throws a FK constraint violation at runtime.

> **Shortcut when no business rules needed and schema has CASCADE:**  
> A single `DELETE FROM PARENT_TABLE WHERE Id = @Id` is enough — the DB handles children.  
> Use the transactional pattern above only when you need checks or the schema lacks CASCADE.

---

## Full Interface Reference

```csharp
// Interfaces/IEntityRepository.cs
namespace YOUR_PROJECT.Interfaces;
using YOUR_PROJECT.Dtos;

public interface IEntityRepository
{
    // GET all
    Task<IEnumerable<EntityResponseDto>> GetAllAsync();
    Task<IEnumerable<EntityResponseDto>> GetAllAsync(int? filterId, DateTime? filterDate);

    // GET single
    Task<EntityResponseDto?>  GetByIdAsync(int id);
    Task<ParentResponseDto?>  GetWithChildrenAsync(int id);
    Task<EntityResponseDto?>  GetByFieldAsync(string fieldValue);

    // GET analytics
    Task<IEnumerable<SummaryDto>> GetSummaryAsync();

    // POST
    Task<int> AddAsync(EntityRequestDto dto);
    Task<int> AddWithTransactionAsync(int parentId, ParentChildRequestDto dto);

    // PUT / PATCH — simple (single table)
    Task<bool> UpdateAsync(int id, EntityRequestDto dto);
    Task<bool> UpdateStatusAsync(int id, string newStatus);
    Task<bool> SetReturnDateAsync(int id, DateTime date);

    // PUT / PATCH — transactional (multi-table)
    Task UpdateWithItemsAsync(int id, UpdateWithItemsDto dto);          // 4o: replace header + re-insert items
    Task UpdateStatusWithSideEffectsAsync(int id, string newStatus);    // 4p: status + audit log + stock

    // DELETE — simple
    Task<bool> DeleteAsync(int id);

    // DELETE — transactional cascade
    Task DeleteWithCascadeAsync(int id);                                 // 4q: delete grandchildren → children → parent
}
```

```csharp
// Interfaces/IEntityService.cs
public interface IEntityService
{
    Task<IEnumerable<EntityResponseDto>> GetAllAsync();
    Task<IEnumerable<EntityResponseDto>> GetAllAsync(int? filterId, DateTime? filterDate);
    Task<EntityResponseDto>  GetByIdAsync(int id);          // throws NotFoundException
    Task<ParentResponseDto>  GetWithChildrenAsync(int id);  // throws NotFoundException
    Task<EntityResponseDto>  GetByFieldAsync(string value); // throws NotFoundException
    Task<IEnumerable<SummaryDto>> GetSummaryAsync();
    Task<int>  CreateAsync(EntityRequestDto dto);
    Task<int>  CreateAsync(int parentId, ParentChildRequestDto dto);
    Task       UpdateAsync(int id, EntityRequestDto dto);
    Task       UpdateStatusAsync(int id, string newStatus);
    Task       SetReturnDateAsync(int id, DateTime date);
    Task       DeleteAsync(int id);
}
```

---

## Full Controller (all actions assembled)

```csharp
namespace YOUR_PROJECT.Controllers;
using Microsoft.AspNetCore.Mvc;
using YOUR_PROJECT.Dtos;
using YOUR_PROJECT.Exceptions;
using YOUR_PROJECT.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class EntitiesController : ControllerBase
{
    private readonly IEntityService _service;

    // NEVER use "new SqlEntityRepository()" here — always inject via interface
    public EntitiesController(IEntityService service) => _service = service;

    // GET /api/entities
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    // GET /api/entities?filterId=5&filterDate=2026-05-01
    [HttpGet("filtered")]
    public async Task<IActionResult> GetAllFiltered(
        [FromQuery] int?      filterId   = null,
        [FromQuery] DateTime? filterDate = null)
        => Ok(await _service.GetAllAsync(filterId, filterDate));

    // GET /api/entities/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try   { return Ok(await _service.GetByIdAsync(id)); }
        catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    // GET /api/entities/{id}/children
    [HttpGet("{id:int}/children")]
    public async Task<IActionResult> GetWithChildren(int id)
    {
        try   { return Ok(await _service.GetWithChildrenAsync(id)); }
        catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    // GET /api/entities/lookup/{value}
    [HttpGet("lookup/{value}")]
    public async Task<IActionResult> GetByField(string value)
    {
        try   { return Ok(await _service.GetByFieldAsync(value)); }
        catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    // GET /api/entities/summary
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
        => Ok(await _service.GetSummaryAsync());

    // POST /api/entities
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EntityRequestDto dto)
    {
        try
        {
            int newId = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
        }
        catch (ConflictException ex) { return Conflict(new { message = ex.Message }); }
    }

    // POST /api/entities/{id}/children
    [HttpPost("{id:int}/children")]
    public async Task<IActionResult> AddChild(int id, [FromBody] ParentChildRequestDto dto)
    {
        try
        {
            int newId = await _service.CreateAsync(id, dto);
            return Created($"api/entities/{id}/children", new { id = newId });
        }
        catch (NotFoundException ex)  { return NotFound(new { message = ex.Message }); }
        catch (ConflictException ex)  { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex)  { return BadRequest(new { message = ex.Message }); }
    }

    // PUT /api/entities/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EntityRequestDto dto)
    {
        try   { await _service.UpdateAsync(id, dto); return NoContent(); }
        catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    // PATCH /api/entities/{id}/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
    {
        try   { await _service.UpdateStatusAsync(id, newStatus); return NoContent(); }
        catch (NotFoundException ex)  { return NotFound(new { message = ex.Message }); }
        catch (ArgumentException ex)  { return BadRequest(new { message = ex.Message }); }
    }

    // PATCH /api/entities/{id}/return
    [HttpPatch("{id:int}/return")]
    public async Task<IActionResult> Return(int id)
    {
        try   { await _service.SetReturnDateAsync(id, DateTime.UtcNow); return NoContent(); }
        catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    // DELETE /api/entities/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try   { await _service.DeleteAsync(id); return NoContent(); }
        catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}
```

---

## DTO Validation Quick Reference

```csharp
using System.ComponentModel.DataAnnotations;

public class EntityRequestDto
{
    [Required]
    [MaxLength(200)]
    public string YOUR_STRING_FIELD { get; set; } = string.Empty;     // e.g. Name, Title

    [Range(0.01, 99999.99)]
    public decimal YOUR_DECIMAL_FIELD { get; set; }                    // e.g. Price, Amount

    [Range(1, 5000)]
    public int YOUR_INT_FIELD { get; set; }                            // e.g. Count, Quantity

    [Required]
    [StringLength(11, MinimumLength = 11)]  // exact length, e.g. PESEL
    public string YOUR_FIXED_LENGTH_FIELD { get; set; } = string.Empty;

    [EmailAddress]
    public string? YOUR_NULLABLE_EMAIL_FIELD { get; set; }             // nullable e.g. Email

    public DateTime YOUR_DATE_FIELD { get; set; }                      // e.g. Date, ScheduledAt

    public List<ItemRequestDto> Items { get; set; } = new();
}
```

> `[ApiController]` auto-returns `400 Bad Request` when any validation attribute fails — no manual `ModelState` check needed.

---

## ADO.NET Snippets — Isolated

### ExecuteScalar — COUNT existence check
```csharp
await using var cmd = new SqlCommand(
    "SELECT COUNT(1) FROM YOUR_TABLE WHERE Col = @Val",
    conn, tx);  // omit tx if not inside a transaction
cmd.Parameters.AddWithValue("@Val", yourValue);
int count = (int)(await cmd.ExecuteScalarAsync())!;
if (count == 0) throw new NotFoundException(yourValue.ToString());
```

### ExecuteScalar — lookup ID by name → throw if missing
```csharp
await using var cmd = new SqlCommand(
    "SELECT Id FROM YOUR_TABLE WHERE YOUR_LOOKUP_COL = @LookupVal",
    conn, tx);
cmd.Parameters.AddWithValue("@LookupVal", yourLookupValue);
var result = await cmd.ExecuteScalarAsync();
if (result is null) throw new NotFoundException(yourLookupValue);
int foundId = (int)result;
```

### ExecuteScalar — get default status ID
```csharp
await using var cmd = new SqlCommand(
    "SELECT Id FROM YOUR_STATUS_TABLE WHERE YOUR_STATUS_NAME_COL = 'YOUR_DEFAULT_STATUS'",
    conn, tx);
int statusId = (int)(await cmd.ExecuteScalarAsync())!;
```

### ExecuteScalar — INSERT + get new auto-generated ID
```csharp
await using var cmd = new SqlCommand("""
    INSERT INTO YOUR_TABLE (YOUR_COL1, YOUR_COL2)
    VALUES (@Val1, @Val2);
    SELECT SCOPE_IDENTITY();
    """, conn, tx);
cmd.Parameters.AddWithValue("@Val1", yourValue1);
cmd.Parameters.AddWithValue("@Val2", yourValue2);
int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
```

### ExecuteNonQuery — UPDATE
```csharp
await using var cmd = new SqlCommand(
    "UPDATE YOUR_TABLE SET YOUR_COL = @Val WHERE Id = @Id", conn);
cmd.Parameters.AddWithValue("@Id",  id);
cmd.Parameters.AddWithValue("@Val", yourValue);
bool success = await cmd.ExecuteNonQueryAsync() > 0;
```

### ExecuteNonQuery — DELETE
```csharp
await using var cmd = new SqlCommand(
    "DELETE FROM YOUR_TABLE WHERE Id = @Id", conn);
cmd.Parameters.AddWithValue("@Id", id);
bool deleted = await cmd.ExecuteNonQueryAsync() > 0;
```

### ExecuteNonQuery — INSERT inside loop (line items)
```csharp
foreach (var item in items)
{
    await using var cmd = new SqlCommand("""
        INSERT INTO YOUR_ITEM_TABLE (YOUR_PARENT_FK, YOUR_LOOKUP_FK, YOUR_VALUE_COL)
        VALUES (@ParentId, @LookupId, @Value)
        """, conn, tx);  // tx REQUIRED inside a transaction
    cmd.Parameters.AddWithValue("@ParentId", parentId);
    cmd.Parameters.AddWithValue("@LookupId", item.YOUR_LOOKUP_FIELD);
    cmd.Parameters.AddWithValue("@Value",    item.YOUR_VALUE_FIELD);
    await cmd.ExecuteNonQueryAsync();
}
```

### Nullable column — all types
```csharp
// string?
string? yourStringVal = reader.IsDBNull(yourStrOrd) ? null : reader.GetString(yourStrOrd);

// DateTime?
DateTime? yourDateVal = reader.IsDBNull(yourDateOrd) ? null : reader.GetDateTime(yourDateOrd);

// decimal?
decimal? yourDecimalVal = reader.IsDBNull(yourDecOrd) ? null : reader.GetDecimal(yourDecOrd);

// int?
int? yourIntVal = reader.IsDBNull(yourIntOrd) ? null : reader.GetInt32(yourIntOrd);

// C# null → SQL NULL (ALWAYS use this pattern for nullable params)
cmd.Parameters.AddWithValue("@YourNullableStr",  (object?)dto.YOUR_NULLABLE_STR   ?? DBNull.Value);
cmd.Parameters.AddWithValue("@YourNullableDate", (object?)dto.YOUR_NULLABLE_DATE  ?? DBNull.Value);
cmd.Parameters.AddWithValue("@YourNullableInt",  (object?)dto.YOUR_NULLABLE_INT   ?? DBNull.Value);
```

---

## Transaction Skeleton

```csharp
await using var conn = new SqlConnection(_cs);
await conn.OpenAsync();
await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

try
{
    // EVERY SqlCommand inside this block must pass BOTH conn AND tx:
    //   new SqlCommand("...", conn, tx)
    // Forgetting tx runs the command OUTSIDE the transaction — silent bug, no rollback

    await tx.CommitAsync();
    return result;
}
catch
{
    // Rollback undoes all inserts/updates if ANYTHING failed
    await tx.RollbackAsync();
    throw;  // "throw;" preserves stack trace. "throw ex;" resets it — NEVER write "throw ex;"
}
```

---

## Middleware

```csharp
// Middleware/ExceptionHandlingMiddleware.cs
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

> **Pick ONE approach:** either catch in each controller action OR catch in middleware. Not both. For exams, catching in the controller is safer — the examiner can see the mapping explicitly.

---

## Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// AddScoped = one instance per HTTP request — correct for ADO.NET (SqlConnection is not thread-safe)
// AddSingleton would share one SqlConnection across all requests — WRONG
builder.Services.AddScoped<IEntityRepository, SqlEntityRepository>();
builder.Services.AddScoped<IEntityService,    EntityService>();

// Add more pairs for each entity:
// builder.Services.AddScoped<IOtherRepository, SqlOtherRepository>();
// builder.Services.AddScoped<IOtherService,    OtherService>();

var app = builder.Build();

// ORDER MATTERS — ExceptionHandlingMiddleware must be FIRST so it wraps everything
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();
```

---

## appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DB;User Id=YOUR_USER;Password=YOUR_PASS;TrustServerCertificate=True"
  }
}
```

---

## HTTP Status Code Reference

| Return | Status | When |
|--------|--------|------|
| `Ok(data)` | 200 | GET succeeded |
| `CreatedAtAction(nameof(GetById), new { id }, data)` | 201 | POST — with Location header |
| `Created($"api/route/{id}", new {{ id }})` | 201 | POST — simpler, no Location header |
| `NoContent()` | 204 | DELETE / PUT / PATCH succeeded, no body |
| `BadRequest(new { message })` | 400 | Invalid input (also auto via `[ApiController]`) |
| `NotFound(new { message })` | 404 | Resource not found |
| `Conflict(new { message })` | 409 | Valid request, current state blocks it |

---

## All Possible Request Shapes — Quick Map

| Request | URL | Method | Controller action |
|---------|-----|--------|-------------------|
| All records | `/api/entities` | GET | `GetAll()` |
| Filtered list | `/api/entities?filterId=5` | GET | `GetAll([FromQuery] ...)` |
| Single by ID | `/api/entities/{id}` | GET | `GetById(int id)` |
| Nested children | `/api/entities/{id}/children` | GET | `GetWithChildren(int id)` |
| By lookup field | `/api/entities/lookup/{value}` | GET | `GetByField(string value)` |
| Analytics/summary | `/api/entities/summary` | GET | `GetSummary()` |
| Simple create | `/api/entities` | POST | `Create([FromBody] dto)` |
| Create with items | `/api/entities/{id}/children` | POST | `AddChild(int id, [FromBody] dto)` |
| Full replace, 1 table | `/api/entities/{id}` | PUT | `Update(int id, [FromBody] dto)` — **4k** |
| Full replace + re-insert items | `/api/parents/{id}` | PUT | `Update(int id, [FromBody] dto)` — **4o** |
| Status update, simple | `/api/entities/{id}/status` | PATCH | `UpdateStatus(...)` — **4l** |
| Status + cross-table side-effects | `/api/entities/{id}/status` | PATCH | `UpdateStatus(...)` — **4p** |
| Set return/end date | `/api/entities/{id}/return` | PATCH | `Return(int id)` — **4m** |
| Remove, 1 table | `/api/entities/{id}` | DELETE | `Delete(int id)` — **4n** |
| Remove + cascade children manually | `/api/entities/{id}` | DELETE | `Delete(int id)` — **4q** |

---

## ✅ Pre-Submit Checklist

```
□ Every repo method is async Task<...>
□ Every await is present (missing await → -10 pts)
□ GetOrdinal() cached OUTSIDE the while loop
□ IsDBNull() checked on EVERY nullable column before reading it
□ C# null → SQL NULL: (object?)value ?? DBNull.Value
□ Every SqlCommand inside a transaction: new SqlCommand("...", conn, tx)
□ SCOPE_IDENTITY() used (NOT @@IDENTITY)
□ Rethrow written as "throw;" (NOT "throw ex;")
□ Controller does NOT import Microsoft.Data.SqlClient
□ NotFoundException / ConflictException defined in Exceptions/ folder
□ Exceptions thrown from repo/service, caught in controller with correct HTTP codes
□ DI registered: builder.Services.AddScoped<IRepo, SqlRepo>()
□ DI registered: builder.Services.AddScoped<IService, Service>()
□ Middleware registered: ExceptionHandlingMiddleware FIRST in pipeline
□ .gitignore committed before anything else (missing = -50%)
□ dotnet build → 0 errors
□ git add . && git commit -m "complete" && git push
```
