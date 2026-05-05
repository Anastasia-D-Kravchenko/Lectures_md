# APBD — C# Coding Exercises (SQL & JSON provided)

> **Your job:** Write the C# code (Repository, Service, Controller) for each endpoint.  
> **Provided for you:** The SQL query and the expected JSON shape.  
> **Domain:** Video rental store — `Customer`, `Rental`, `RentalItem`, `Movie`, `Genre`, `RentalStatus`, `StatusLog`, `Stock`

---

## Schema

```sql
Customer     (Id INT PK, FirstName NVARCHAR(50), LastName NVARCHAR(50), Email NVARCHAR(100), Phone NVARCHAR(20) NULL)
Genre        (Id INT PK, Name NVARCHAR(50))
Movie        (Id INT PK, Title NVARCHAR(100), GenreId INT FK→Genre, DailyRate DECIMAL(6,2))
RentalStatus (Id INT PK, Name NVARCHAR(20))   -- 'pending','active','returned','cancelled'
Rental       (Id INT PK, CustomerId INT FK→Customer, RentalDate DATETIME, ReturnDate DATETIME NULL, StatusId INT FK→RentalStatus)
RentalItem   (Id INT PK, RentalId INT FK→Rental, MovieId INT FK→Movie, Price DECIMAL(6,2))
StatusLog    (Id INT PK, RentalId INT FK→Rental, OldStatus NVARCHAR(20), NewStatus NVARCHAR(20), ChangedAt DATETIME)
Stock        (Id INT PK, MovieId INT FK→Movie, Qty INT)
```

---

## DTOs (already written for you — reference these in every exercise)

```csharp
// ── Response DTOs ──────────────────────────────────────────────────────────

public class CustomerDto
{
    public int     Id        { get; set; }
    public string  FirstName { get; set; } = string.Empty;
    public string  LastName  { get; set; } = string.Empty;
    public string  Email     { get; set; } = string.Empty;
    public string? Phone     { get; set; }               // nullable
}

public class MovieDto
{
    public int     Id        { get; set; }
    public string  Title     { get; set; } = string.Empty;
    public string  GenreName { get; set; } = string.Empty;
    public decimal DailyRate { get; set; }
}

public class RentalDto
{
    public int       Id         { get; set; }
    public int       CustomerId { get; set; }
    public DateTime  RentalDate { get; set; }
    public DateTime? ReturnDate { get; set; }   // nullable
    public string    Status     { get; set; } = string.Empty;
}

public class CustomerWithRentalsDto
{
    public int     Id        { get; set; }
    public string  FirstName { get; set; } = string.Empty;
    public string  LastName  { get; set; } = string.Empty;
    public string  Email     { get; set; } = string.Empty;
    public List<RentalWithMoviesDto> Rentals { get; set; } = new();
}

public class RentalWithMoviesDto
{
    public int       Id         { get; set; }
    public DateTime  RentalDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string    Status     { get; set; } = string.Empty;
    public List<RentalMovieDto> Movies { get; set; } = new();
}

public class RentalMovieDto
{
    public string  Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class MovieSummaryDto
{
    public string  Title        { get; set; } = string.Empty;
    public int     TotalRentals { get; set; }
    public decimal TotalRevenue { get; set; }
}

// ── Request DTOs ───────────────────────────────────────────────────────────

public class CreateCustomerDto
{
    [Required][MaxLength(50)]  public string  FirstName { get; set; } = string.Empty;
    [Required][MaxLength(50)]  public string  LastName  { get; set; } = string.Empty;
    [Required][EmailAddress]   public string  Email     { get; set; } = string.Empty;
    public string? Phone { get; set; }
}

public class UpdateCustomerDto
{
    [Required][MaxLength(50)]  public string  FirstName { get; set; } = string.Empty;
    [Required][MaxLength(50)]  public string  LastName  { get; set; } = string.Empty;
    [Required][EmailAddress]   public string  Email     { get; set; } = string.Empty;
    public string? Phone { get; set; }
}

public class CreateRentalDto
{
    public DateTime           RentalDate { get; set; }
    public List<RentalItemDto> Movies    { get; set; } = new();
}

public class RentalItemDto
{
    [Required] public string  Title { get; set; } = string.Empty;
    [Range(0.01, 9999)] public decimal Price { get; set; }
}

public class UpdateRentalDto
{
    public DateTime           RentalDate { get; set; }
    public List<RentalItemDto> Movies    { get; set; } = new();
}
```

---

---

# EXERCISE 1 — GET all customers (Pattern 4a)

**Endpoint:** `GET /api/customers`  
**Returns:** `200 OK` + flat array. Empty list → `200 OK` + `[]`, never 404.

## Given — SQL

```sql
SELECT Id, FirstName, LastName, Email, Phone
FROM Customer
```

## Given — Expected JSON

```json
[
  {
    "id": 1,
    "firstName": "Anna",
    "lastName": "Kowalska",
    "email": "anna@mail.com",
    "phone": "+48123456789"
  },
  {
    "id": 2,
    "firstName": "Jan",
    "lastName": "Nowak",
    "email": "jan@mail.com",
    "phone": null
  }
]
```

## YOUR TASK — Write the C# code

Write the full **Repository**, **Service**, and **Controller** for this endpoint.  
`Phone` is a nullable column — handle it correctly.

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: declare the method signature




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<IEnumerable<CustomerDto>> GetAllAsync()
{
    // TODO: open connection, build command with the SQL above,
    //       cache ordinals (including nullable Phone),
    //       read rows into list, return list




}

// ── SERVICE ──────────────────────────────────────────────────────────────────
// TODO: pass-through (no business logic needed here)




// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: GET /api/customers
// TODO: action attribute, return Ok(...)
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task<IEnumerable<CustomerDto>> GetAllAsync();

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<IEnumerable<CustomerDto>> GetAllAsync()
{
    var list = new List<CustomerDto>();
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "SELECT Id, FirstName, LastName, Email, Phone FROM Customer", conn);
    await using var reader = await cmd.ExecuteReaderAsync();

    int idOrd    = reader.GetOrdinal("Id");
    int firstOrd = reader.GetOrdinal("FirstName");
    int lastOrd  = reader.GetOrdinal("LastName");
    int emailOrd = reader.GetOrdinal("Email");
    int phoneOrd = reader.GetOrdinal("Phone");   // nullable

    while (await reader.ReadAsync())
        list.Add(new CustomerDto
        {
            Id        = reader.GetInt32(idOrd),
            FirstName = reader.GetString(firstOrd),
            LastName  = reader.GetString(lastOrd),
            Email     = reader.GetString(emailOrd),
            Phone     = reader.IsDBNull(phoneOrd) ? null : reader.GetString(phoneOrd),
        });

    return list;
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public Task<IEnumerable<CustomerDto>> GetAllAsync()
    => _repo.GetAllAsync();

// ── CONTROLLER ───────────────────────────────────────────────────────────────
[HttpGet]
public async Task<IActionResult> GetAll()
    => Ok(await _service.GetAllAsync());
```

**Key points:**
- `GetOrdinal` called **before** the loop — never inside.
- `IsDBNull(phoneOrd)` checked before `GetString` on nullable column.
- Empty list returns `200 OK` + `[]`. No 404 for empty collections.

</details>

---

---

# EXERCISE 2 — GET rentals with optional filters (Pattern 4b)

**Endpoint:** `GET /api/rentals?customerId=3&status=active`  
Both query params are optional. Single SQL handles all cases.

## Given — SQL

```sql
SELECT r.Id, r.CustomerId, r.RentalDate, r.ReturnDate, rs.Name AS Status
FROM Rental r
JOIN RentalStatus rs ON rs.Id = r.StatusId
WHERE (@CustomerId IS NULL OR r.CustomerId = @CustomerId)
  AND (@Status     IS NULL OR rs.Name      = @Status)
```

## Given — Expected JSON

```json
[
  {
    "id": 7,
    "customerId": 3,
    "rentalDate": "2026-04-01T00:00:00",
    "returnDate": null,
    "status": "active"
  }
]
```

## YOUR TASK — Write the C# code

`ReturnDate` is nullable. Both filter params arrive as nullable from the query string.  
Pay close attention to how you pass nullable C# values to SQL parameters.

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: declare method signature with two nullable filter params




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<IEnumerable<RentalDto>> GetAllAsync(int? customerId, string? status)
{
    // TODO: open connection, use the SQL above,
    //       bind BOTH nullable parameters correctly (DBNull.Value!),
    //       cache ordinals, read rows, handle nullable ReturnDate




}

// ── SERVICE ──────────────────────────────────────────────────────────────────
// TODO: pass-through




// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: GET /api/rentals?customerId=3&status=active
// TODO: [FromQuery] params, return Ok(...)
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task<IEnumerable<RentalDto>> GetAllAsync(int? customerId, string? status);

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<IEnumerable<RentalDto>> GetAllAsync(int? customerId, string? status)
{
    var list = new List<RentalDto>();
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(@"
        SELECT r.Id, r.CustomerId, r.RentalDate, r.ReturnDate, rs.Name AS Status
        FROM Rental r
        JOIN RentalStatus rs ON rs.Id = r.StatusId
        WHERE (@CustomerId IS NULL OR r.CustomerId = @CustomerId)
          AND (@Status     IS NULL OR rs.Name      = @Status)", conn);

    cmd.Parameters.AddWithValue("@CustomerId", (object?)customerId ?? DBNull.Value);
    cmd.Parameters.AddWithValue("@Status",     (object?)status     ?? DBNull.Value);

    await using var reader = await cmd.ExecuteReaderAsync();

    int idOrd         = reader.GetOrdinal("Id");
    int customerIdOrd = reader.GetOrdinal("CustomerId");
    int rentalDateOrd = reader.GetOrdinal("RentalDate");
    int returnDateOrd = reader.GetOrdinal("ReturnDate");  // nullable
    int statusOrd     = reader.GetOrdinal("Status");

    while (await reader.ReadAsync())
        list.Add(new RentalDto
        {
            Id         = reader.GetInt32(idOrd),
            CustomerId = reader.GetInt32(customerIdOrd),
            RentalDate = reader.GetDateTime(rentalDateOrd),
            ReturnDate = reader.IsDBNull(returnDateOrd) ? null : reader.GetDateTime(returnDateOrd),
            Status     = reader.GetString(statusOrd),
        });

    return list;
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public Task<IEnumerable<RentalDto>> GetAllAsync(int? customerId, string? status)
    => _repo.GetAllAsync(customerId, status);

// ── CONTROLLER ───────────────────────────────────────────────────────────────
[HttpGet]
public async Task<IActionResult> GetAll(
    [FromQuery] int?    customerId = null,
    [FromQuery] string? status     = null)
    => Ok(await _service.GetAllAsync(customerId, status));
```

**Key points:**
- `(object?)value ?? DBNull.Value` — mandatory for any nullable C# param going into SQL.
- `IsDBNull(returnDateOrd)` before `GetDateTime` — nullable DB column.

</details>

---

---

# EXERCISE 3 — GET single customer by ID (Pattern 4c)

**Endpoint:** `GET /api/customers/{id}`  
Returns `200` if found, `404` if not.

## Given — SQL

```sql
SELECT Id, FirstName, LastName, Email, Phone
FROM Customer
WHERE Id = @Id
```

## Given — Expected JSON

**200 OK:**
```json
{
  "id": 5,
  "firstName": "Maria",
  "lastName": "Wiśniewska",
  "email": "maria@example.com",
  "phone": null
}
```

**404 Not Found:**
```json
{ "message": "Entity with id 999 was not found." }
```

## YOUR TASK — Write the C# code

The repository returns `null` when not found. The service converts `null` to `NotFoundException`.  
Use the single-row pattern (`if (!await reader.ReadAsync()) return null`) — NOT a while loop.

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: nullable return type




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<CustomerDto?> GetByIdAsync(int id)
{
    // TODO: open connection, parameterize @Id,
    //       single-row read pattern (if !ReadAsync → return null),
    //       cache ordinals AFTER the ReadAsync check,
    //       handle nullable Phone




}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task<CustomerDto> GetByIdAsync(int id)
{
    // TODO: call repo, throw NotFoundException if null




}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: GET /api/customers/{id}
// TODO: [HttpGet("{id:int}")], try/catch NotFoundException → 404
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task<CustomerDto?> GetByIdAsync(int id);

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<CustomerDto?> GetByIdAsync(int id)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "SELECT Id, FirstName, LastName, Email, Phone FROM Customer WHERE Id = @Id", conn);
    cmd.Parameters.AddWithValue("@Id", id);
    await using var reader = await cmd.ExecuteReaderAsync();

    if (!await reader.ReadAsync()) return null;

    int idOrd    = reader.GetOrdinal("Id");
    int firstOrd = reader.GetOrdinal("FirstName");
    int lastOrd  = reader.GetOrdinal("LastName");
    int emailOrd = reader.GetOrdinal("Email");
    int phoneOrd = reader.GetOrdinal("Phone");

    return new CustomerDto
    {
        Id        = reader.GetInt32(idOrd),
        FirstName = reader.GetString(firstOrd),
        LastName  = reader.GetString(lastOrd),
        Email     = reader.GetString(emailOrd),
        Phone     = reader.IsDBNull(phoneOrd) ? null : reader.GetString(phoneOrd),
    };
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task<CustomerDto> GetByIdAsync(int id)
{
    var result = await _repo.GetByIdAsync(id);
    return result ?? throw new NotFoundException(id);
}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
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
```

**Key points:**
- Ordinals cached **after** `ReadAsync()` succeeds — the result set must be positioned before `GetOrdinal`.
- Repository returns `null`, never throws HTTP codes.
- Service owns the `NotFoundException`, not the repository.

</details>

---

---

# EXERCISE 4 — GET customer with all rentals and movies (Pattern 4d)

**Endpoint:** `GET /api/customers/{id}/rentals`  
One SQL query, one reader pass. Dictionary deduplicates parent/child rows from the JOIN.

## Given — SQL

```sql
SELECT
    c.Id          AS CustomerId,
    c.FirstName,
    c.LastName,
    c.Email,
    r.Id          AS RentalId,
    r.RentalDate,
    r.ReturnDate,
    rs.Name       AS Status,
    m.Title       AS MovieTitle,
    ri.Price
FROM Customer c
LEFT JOIN Rental       r  ON r.CustomerId  = c.Id
LEFT JOIN RentalStatus rs ON rs.Id         = r.StatusId
LEFT JOIN RentalItem   ri ON ri.RentalId   = r.Id
LEFT JOIN Movie        m  ON m.Id          = ri.MovieId
WHERE c.Id = @Id
ORDER BY r.Id, m.Title
```

## Given — Expected JSON

**200 OK:**
```json
{
  "id": 3,
  "firstName": "Anna",
  "lastName": "Kowalska",
  "email": "anna@mail.com",
  "rentals": [
    {
      "id": 7,
      "rentalDate": "2026-03-15T00:00:00",
      "returnDate": "2026-03-22T00:00:00",
      "status": "returned",
      "movies": [
        { "title": "Inception",    "price": 9.99 },
        { "title": "Interstellar", "price": 9.99 }
      ]
    },
    {
      "id": 9,
      "rentalDate": "2026-04-01T00:00:00",
      "returnDate": null,
      "status": "active",
      "movies": [
        { "title": "Dune", "price": 12.50 }
      ]
    }
  ]
}
```

**404 Not Found:**
```json
{ "message": "Entity with id 99 was not found." }
```

## YOUR TASK — Write the C# code

This is the hardest GET pattern. You need:
- A `Dictionary<int, RentalWithMoviesDto>` to deduplicate rental rows
- `result is null` check (build parent on first row only)
- `IsDBNull(rentalIdOrd)` guard (customer may have no rentals — LEFT JOIN)
- `TryGetValue` to avoid duplicate rental entries
- `IsDBNull(movieTitleOrd)` guard (rental may have no movies — LEFT JOIN)

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: returns nullable DTO




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<CustomerWithRentalsDto?> GetWithRentalsAsync(int id)
{
    // TODO:
    // 1. Declare result = null and rentalDict = new Dictionary<int, RentalWithMoviesDto>()
    // 2. Open connection, run the SQL above with @Id param
    // 3. Cache ALL ordinals before the loop
    // 4. while loop:
    //    a. Build parent on first row (result is null)
    //    b. Skip if RentalId is NULL (no rentals)
    //    c. TryGetValue on rentalDict — add new RentalWithMoviesDto if missing
    //    d. Skip if MovieTitle is NULL (no movies in rental)
    //    e. Add RentalMovieDto to the rental's Movies list
    // 5. return result (null if customer not found)




}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task<CustomerWithRentalsDto> GetWithRentalsAsync(int id)
{
    // TODO: call repo, throw NotFoundException if null




}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: GET /api/customers/{id}/rentals
// TODO: try/catch NotFoundException → 404
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task<CustomerWithRentalsDto?> GetWithRentalsAsync(int id);

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<CustomerWithRentalsDto?> GetWithRentalsAsync(int id)
{
    CustomerWithRentalsDto? result = null;
    var rentalDict = new Dictionary<int, RentalWithMoviesDto>();

    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(@"
        SELECT
            c.Id          AS CustomerId,
            c.FirstName,
            c.LastName,
            c.Email,
            r.Id          AS RentalId,
            r.RentalDate,
            r.ReturnDate,
            rs.Name       AS Status,
            m.Title       AS MovieTitle,
            ri.Price
        FROM Customer c
        LEFT JOIN Rental       r  ON r.CustomerId  = c.Id
        LEFT JOIN RentalStatus rs ON rs.Id         = r.StatusId
        LEFT JOIN RentalItem   ri ON ri.RentalId   = r.Id
        LEFT JOIN Movie        m  ON m.Id          = ri.MovieId
        WHERE c.Id = @Id
        ORDER BY r.Id, m.Title", conn);
    cmd.Parameters.AddWithValue("@Id", id);
    await using var reader = await cmd.ExecuteReaderAsync();

    // Cache ALL ordinals before the loop
    int customerIdOrd  = reader.GetOrdinal("CustomerId");
    int firstOrd       = reader.GetOrdinal("FirstName");
    int lastOrd        = reader.GetOrdinal("LastName");
    int emailOrd       = reader.GetOrdinal("Email");
    int rentalIdOrd    = reader.GetOrdinal("RentalId");
    int rentalDateOrd  = reader.GetOrdinal("RentalDate");
    int returnDateOrd  = reader.GetOrdinal("ReturnDate");
    int statusOrd      = reader.GetOrdinal("Status");
    int movieTitleOrd  = reader.GetOrdinal("MovieTitle");
    int priceOrd       = reader.GetOrdinal("Price");

    while (await reader.ReadAsync())
    {
        // Build parent only on the very first row
        if (result is null)
        {
            result = new CustomerWithRentalsDto
            {
                Id        = reader.GetInt32(customerIdOrd),
                FirstName = reader.GetString(firstOrd),
                LastName  = reader.GetString(lastOrd),
                Email     = reader.GetString(emailOrd),
            };
        }

        // LEFT JOIN → RentalId is NULL when customer has no rentals
        if (reader.IsDBNull(rentalIdOrd)) continue;

        int rentalId = reader.GetInt32(rentalIdOrd);

        // TryGetValue deduplicates — without this, every movie row creates a new RentalWithMoviesDto
        if (!rentalDict.TryGetValue(rentalId, out var rentalDto))
        {
            rentalDto = new RentalWithMoviesDto
            {
                Id         = rentalId,
                RentalDate = reader.GetDateTime(rentalDateOrd),
                ReturnDate = reader.IsDBNull(returnDateOrd) ? null : reader.GetDateTime(returnDateOrd),
                Status     = reader.GetString(statusOrd),
            };
            rentalDict[rentalId] = rentalDto;
            result.Rentals.Add(rentalDto);
        }

        // LEFT JOIN → MovieTitle is NULL when rental has no items
        if (reader.IsDBNull(movieTitleOrd)) continue;

        rentalDto.Movies.Add(new RentalMovieDto
        {
            Title = reader.GetString(movieTitleOrd),
            Price = reader.GetDecimal(priceOrd),
        });
    }

    return result;  // null means customer ID not found
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task<CustomerWithRentalsDto> GetWithRentalsAsync(int id)
{
    var result = await _repo.GetWithRentalsAsync(id);
    return result ?? throw new NotFoundException(id);
}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
[HttpGet("{id:int}/rentals")]
public async Task<IActionResult> GetWithRentals(int id)
{
    try
    {
        return Ok(await _service.GetWithRentalsAsync(id));
    }
    catch (NotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}
```

**Key points:**
- `result is null` check comes **before** the rental NULL guard — parent always built on row 1.
- `TryGetValue` is mandatory — the JOIN produces one row per movie, which would create duplicate rental objects without it.
- Two separate `IsDBNull` guards: one for rental, one for movie.

</details>

---

---

# EXERCISE 5 — GET movie by title (Pattern 4e)

**Endpoint:** `GET /api/movies/lookup/{title}`

## Given — SQL

```sql
SELECT m.Id, m.Title, g.Name AS GenreName, m.DailyRate
FROM Movie m
JOIN Genre g ON g.Id = m.GenreId
WHERE m.Title = @Title
```

## Given — Expected JSON

**200 OK:**
```json
{
  "id": 4,
  "title": "Inception",
  "genreName": "Sci-Fi",
  "dailyRate": 9.99
}
```

**404 Not Found:**
```json
{ "message": "'Inception' was not found." }
```

## YOUR TASK — Write the C# code

Note: the 404 message uses the **string overload** of `NotFoundException` — it wraps the value in single quotes.

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: lookup by string, returns nullable DTO




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<MovieDto?> GetByTitleAsync(string title)
{
    // TODO: single-row pattern, no nullable columns this time




}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task<MovieDto> GetByTitleAsync(string title)
{
    // TODO: throw NotFoundException(title) — string overload




}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: GET /api/movies/lookup/{title}
// TODO: [HttpGet("lookup/{title}")], try/catch → 404
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task<MovieDto?> GetByTitleAsync(string title);

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<MovieDto?> GetByTitleAsync(string title)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(@"
        SELECT m.Id, m.Title, g.Name AS GenreName, m.DailyRate
        FROM Movie m
        JOIN Genre g ON g.Id = m.GenreId
        WHERE m.Title = @Title", conn);
    cmd.Parameters.AddWithValue("@Title", title);
    await using var reader = await cmd.ExecuteReaderAsync();

    if (!await reader.ReadAsync()) return null;

    int idOrd        = reader.GetOrdinal("Id");
    int titleOrd     = reader.GetOrdinal("Title");
    int genreNameOrd = reader.GetOrdinal("GenreName");
    int dailyRateOrd = reader.GetOrdinal("DailyRate");

    return new MovieDto
    {
        Id        = reader.GetInt32(idOrd),
        Title     = reader.GetString(titleOrd),
        GenreName = reader.GetString(genreNameOrd),
        DailyRate = reader.GetDecimal(dailyRateOrd),
    };
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task<MovieDto> GetByTitleAsync(string title)
{
    var result = await _repo.GetByTitleAsync(title);
    return result ?? throw new NotFoundException(title);  // string overload → "'Inception' was not found."
}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
[HttpGet("lookup/{title}")]
public async Task<IActionResult> GetByTitle(string title)
{
    try
    {
        return Ok(await _service.GetByTitleAsync(title));
    }
    catch (NotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}
```

</details>

---

---

# EXERCISE 6 — GET movie rental summary (Pattern 4f)

**Endpoint:** `GET /api/movies/summary`  
Aggregation query — no nullable columns in the result (use `ISNULL` in SQL to coerce NULLs from LEFT JOIN).

## Given — SQL

```sql
SELECT
    m.Title,
    COUNT(ri.Id)              AS TotalRentals,
    ISNULL(SUM(ri.Price), 0)  AS TotalRevenue
FROM Movie m
LEFT JOIN RentalItem ri ON ri.MovieId = m.Id
GROUP BY m.Id, m.Title
ORDER BY TotalRevenue DESC
```

## Given — Expected JSON

```json
[
  { "title": "Inception",    "totalRentals": 14, "totalRevenue": 139.86 },
  { "title": "Dune",         "totalRentals": 7,  "totalRevenue": 87.50  },
  { "title": "The Notebook", "totalRentals": 0,  "totalRevenue": 0.00   }
]
```

## YOUR TASK — Write the C# code

`TotalRevenue` uses `ISNULL(..., 0)` in SQL so it is never NULL in the result set — safe to call `GetDecimal` directly.

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: returns IEnumerable<MovieSummaryDto>




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<IEnumerable<MovieSummaryDto>> GetSummaryAsync()
{
    // TODO: no filters, no nullable reads needed (ISNULL handles it in SQL)




}

// ── SERVICE ──────────────────────────────────────────────────────────────────
// TODO: pass-through




// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: GET /api/movies/summary
// Note: this route must be registered BEFORE {id:int} routes to avoid conflicts
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task<IEnumerable<MovieSummaryDto>> GetSummaryAsync();

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<IEnumerable<MovieSummaryDto>> GetSummaryAsync()
{
    var list = new List<MovieSummaryDto>();
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(@"
        SELECT
            m.Title,
            COUNT(ri.Id)             AS TotalRentals,
            ISNULL(SUM(ri.Price), 0) AS TotalRevenue
        FROM Movie m
        LEFT JOIN RentalItem ri ON ri.MovieId = m.Id
        GROUP BY m.Id, m.Title
        ORDER BY TotalRevenue DESC", conn);
    await using var reader = await cmd.ExecuteReaderAsync();

    int titleOrd   = reader.GetOrdinal("Title");
    int countOrd   = reader.GetOrdinal("TotalRentals");
    int revenueOrd = reader.GetOrdinal("TotalRevenue");

    while (await reader.ReadAsync())
        list.Add(new MovieSummaryDto
        {
            Title        = reader.GetString(titleOrd),
            TotalRentals = reader.GetInt32(countOrd),
            TotalRevenue = reader.GetDecimal(revenueOrd),
        });

    return list;
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public Task<IEnumerable<MovieSummaryDto>> GetSummaryAsync()
    => _repo.GetSummaryAsync();

// ── CONTROLLER ───────────────────────────────────────────────────────────────
[HttpGet("summary")]
public async Task<IActionResult> GetSummary()
    => Ok(await _service.GetSummaryAsync());
```

**Key point:** `ISNULL(SUM(ri.Price), 0)` in SQL means the column is never NULL in the result set, so `GetDecimal` is safe without an `IsDBNull` guard. If the SQL used `SUM(ri.Price)` without `ISNULL`, you'd need `reader.IsDBNull(revenueOrd) ? 0m : reader.GetDecimal(revenueOrd)` in C#.

</details>

---

---

# EXERCISE 7 — POST create customer (Pattern 4g)

**Endpoint:** `POST /api/customers`

## Given — SQL

```sql
INSERT INTO Customer (FirstName, LastName, Email, Phone)
VALUES (@FirstName, @LastName, @Email, @Phone);
SELECT SCOPE_IDENTITY();
```

## Given — Expected JSON

**Request body:**
```json
{
  "firstName": "Piotr",
  "lastName":  "Zając",
  "email":     "piotr@mail.com",
  "phone":     null
}
```

**201 Created response:**
```json
{ "id": 11 }
```

**409 Conflict (duplicate email):**
```json
{ "message": "'piotr@mail.com' already exists." }
```

## YOUR TASK — Write the C# code

The service must check for duplicate email before inserting — use a separate repo method `ExistsByEmailAsync(string email)` that you also write.  
`Phone` is nullable — bind it correctly.

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: two method signatures — ExistsByEmailAsync and AddAsync




// ── REPOSITORY — ExistsByEmailAsync ──────────────────────────────────────────
public async Task<bool> ExistsByEmailAsync(string email)
{
    // TODO: SELECT COUNT(1) FROM Customer WHERE Email = @Email
    //       return count > 0




}

// ── REPOSITORY — AddAsync ────────────────────────────────────────────────────
public async Task<int> AddAsync(CreateCustomerDto dto)
{
    // TODO: INSERT with SCOPE_IDENTITY(), bind nullable Phone,
    //       return Convert.ToInt32(ExecuteScalarAsync())




}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task<int> CreateAsync(CreateCustomerDto dto)
{
    // TODO: duplicate check → ConflictException, then call repo




}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: POST /api/customers
// Returns: 201 Created, 409 Conflict
// TODO: CreatedAtAction pointing to GetById, catch ConflictException
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task<bool> ExistsByEmailAsync(string email);
Task<int>  AddAsync(CreateCustomerDto dto);

// ── REPOSITORY — ExistsByEmailAsync ──────────────────────────────────────────
public async Task<bool> ExistsByEmailAsync(string email)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "SELECT COUNT(1) FROM Customer WHERE Email = @Email", conn);
    cmd.Parameters.AddWithValue("@Email", email);
    int count = (int)(await cmd.ExecuteScalarAsync())!;
    return count > 0;
}

// ── REPOSITORY — AddAsync ────────────────────────────────────────────────────
public async Task<int> AddAsync(CreateCustomerDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand("""
        INSERT INTO Customer (FirstName, LastName, Email, Phone)
        VALUES (@FirstName, @LastName, @Email, @Phone);
        SELECT SCOPE_IDENTITY();
        """, conn);
    cmd.Parameters.AddWithValue("@FirstName", dto.FirstName);
    cmd.Parameters.AddWithValue("@LastName",  dto.LastName);
    cmd.Parameters.AddWithValue("@Email",     dto.Email);
    cmd.Parameters.AddWithValue("@Phone",     (object?)dto.Phone ?? DBNull.Value);

    return Convert.ToInt32(await cmd.ExecuteScalarAsync());
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task<int> CreateAsync(CreateCustomerDto dto)
{
    bool exists = await _repo.ExistsByEmailAsync(dto.Email);
    if (exists)
        throw new ConflictException($"'{dto.Email}' already exists.");

    return await _repo.AddAsync(dto);
}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
{
    try
    {
        int newId = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
    }
    catch (ConflictException ex)
    {
        return Conflict(new { message = ex.Message });
    }
}
```

**Key points:**
- `SCOPE_IDENTITY()` not `@@IDENTITY`.
- `Convert.ToInt32` — `ExecuteScalarAsync` returns `object?`, `SCOPE_IDENTITY()` returns decimal in SQL Server.
- `(object?)dto.Phone ?? DBNull.Value` — the only correct way to pass a nullable C# value as SQL NULL.

</details>

---

---

# EXERCISE 8 — POST transactional rental creation (Pattern 4h)

**Endpoint:** `POST /api/customers/{customerId}/rentals`  
Multi-step transaction: verify customer → resolve movie IDs → get status ID → insert Rental → insert RentalItems.

## Given — SQL (5 statements, each runs separately)

```sql
-- Step 1: verify customer
SELECT COUNT(1) FROM Customer WHERE Id = @CustomerId

-- Step 2: resolve movie by title (runs per movie in a loop)
SELECT Id FROM Movie WHERE Title = @Title

-- Step 3: get default status ID
SELECT Id FROM RentalStatus WHERE Name = 'active'

-- Step 4: insert Rental header, return new ID
INSERT INTO Rental (CustomerId, RentalDate, ReturnDate, StatusId)
VALUES (@CustomerId, @RentalDate, NULL, @StatusId);
SELECT SCOPE_IDENTITY();

-- Step 5: insert RentalItem (runs per movie in a loop)
INSERT INTO RentalItem (RentalId, MovieId, Price)
VALUES (@RentalId, @MovieId, @Price)
```

## Given — Expected JSON

**Request body:**
```json
{
  "rentalDate": "2026-05-05T00:00:00",
  "movies": [
    { "title": "Inception",    "price": 9.99 },
    { "title": "Interstellar", "price": 9.99 }
  ]
}
```

**201 Created:**
```json
{ "id": 15 }
```

**404 — customer not found:**
```json
{ "message": "Entity with id 3 was not found." }
```

**404 — movie not found:**
```json
{ "message": "'Interstellar' was not found." }
```

**400 — empty movies list (service validation):**
```json
{ "message": "At least one movie is required." }
```

## YOUR TASK — Write the C# code

This is the core exam pattern. Every `SqlCommand` inside the transaction **must** pass `conn, tx`.  
Resolve ALL movie IDs before inserting anything (collect into a list first).

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: returns Task<int>




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<int> AddRentalAsync(int customerId, CreateRentalDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // TODO: Step 1 — verify customer exists (NotFoundException if not)

        // TODO: Step 2 — resolve movie IDs by title into a List<(int MovieId, decimal Price)>
        //                 throw NotFoundException(item.Title) if any movie not found

        // TODO: Step 3 — get status ID for 'active'

        // TODO: Step 4 — INSERT Rental, get new ID with SCOPE_IDENTITY()

        // TODO: Step 5 — INSERT RentalItem for each resolved movie

        await tx.CommitAsync();
        // TODO: return newId
    }
    catch
    {
        await tx.RollbackAsync();
        throw;  // always "throw;" never "throw ex;"
    }
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task<int> CreateAsync(int customerId, CreateRentalDto dto)
{
    // TODO: validate dto.Movies is not empty (ArgumentException → 400)
    //       validate dto.RentalDate is not in the past (ArgumentException → 400)




}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: POST /api/customers/{customerId}/rentals
// Returns: 201, 404, 409, 400
// TODO: Created($"api/customers/{customerId}/rentals", ...) — NOT CreatedAtAction
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task<int> AddRentalAsync(int customerId, CreateRentalDto dto);

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<int> AddRentalAsync(int customerId, CreateRentalDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // Step 1: verify customer exists
        await using var checkCustomer = new SqlCommand(
            "SELECT COUNT(1) FROM Customer WHERE Id = @CustomerId", conn, tx);
        checkCustomer.Parameters.AddWithValue("@CustomerId", customerId);
        if ((int)(await checkCustomer.ExecuteScalarAsync())! == 0)
            throw new NotFoundException(customerId);

        // Step 2: resolve all movie IDs before any INSERT
        var resolvedMovies = new List<(int MovieId, decimal Price)>();
        foreach (var item in dto.Movies)
        {
            await using var findMovie = new SqlCommand(
                "SELECT Id FROM Movie WHERE Title = @Title", conn, tx);
            findMovie.Parameters.AddWithValue("@Title", item.Title);
            var found = await findMovie.ExecuteScalarAsync();
            if (found is null)
                throw new NotFoundException(item.Title);  // string overload
            resolvedMovies.Add(((int)found, item.Price));
        }

        // Step 3: get status ID for 'active'
        await using var getStatus = new SqlCommand(
            "SELECT Id FROM RentalStatus WHERE Name = 'active'", conn, tx);
        int statusId = (int)(await getStatus.ExecuteScalarAsync())!;

        // Step 4: insert Rental header
        await using var insertRental = new SqlCommand("""
            INSERT INTO Rental (CustomerId, RentalDate, ReturnDate, StatusId)
            VALUES (@CustomerId, @RentalDate, NULL, @StatusId);
            SELECT SCOPE_IDENTITY();
            """, conn, tx);
        insertRental.Parameters.AddWithValue("@CustomerId", customerId);
        insertRental.Parameters.AddWithValue("@RentalDate", dto.RentalDate);
        insertRental.Parameters.AddWithValue("@StatusId",   statusId);
        int newId = Convert.ToInt32(await insertRental.ExecuteScalarAsync());

        // Step 5: insert each RentalItem
        foreach (var (movieId, price) in resolvedMovies)
        {
            await using var insertItem = new SqlCommand("""
                INSERT INTO RentalItem (RentalId, MovieId, Price)
                VALUES (@RentalId, @MovieId, @Price)
                """, conn, tx);
            insertItem.Parameters.AddWithValue("@RentalId", newId);
            insertItem.Parameters.AddWithValue("@MovieId",  movieId);
            insertItem.Parameters.AddWithValue("@Price",    price);
            await insertItem.ExecuteNonQueryAsync();
        }

        await tx.CommitAsync();
        return newId;
    }
    catch
    {
        await tx.RollbackAsync();
        throw;
    }
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task<int> CreateAsync(int customerId, CreateRentalDto dto)
{
    if (!dto.Movies.Any())
        throw new ArgumentException("At least one movie is required.");

    if (dto.RentalDate.Date < DateTime.UtcNow.Date)
        throw new ArgumentException("Rental date cannot be in the past.");

    return await _repo.AddRentalAsync(customerId, dto);
}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
[HttpPost("{customerId:int}/rentals")]
public async Task<IActionResult> AddRental(int customerId, [FromBody] CreateRentalDto dto)
{
    try
    {
        int newId = await _service.CreateAsync(customerId, dto);
        return Created($"api/customers/{customerId}/rentals", new { id = newId });
    }
    catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    catch (ConflictException ex) { return Conflict(new { message = ex.Message }); }
    catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
}
```

**Key points:**
- Every `new SqlCommand(...)` inside the try block passes **both** `conn` AND `tx`. Missing `tx` is a silent transaction escape bug.
- Resolve ALL movie IDs **before** any INSERT — if a movie is invalid, rollback is clean (no partial header row committed).
- `throw;` not `throw ex;` in the catch — preserves the original stack trace.

</details>

---

---

# EXERCISE 9 — PUT full update customer (Pattern 4k)

**Endpoint:** `PUT /api/customers/{id}`

## Given — SQL

```sql
UPDATE Customer
SET FirstName = @FirstName,
    LastName  = @LastName,
    Email     = @Email,
    Phone     = @Phone
WHERE Id = @Id
```

## Given — Expected JSON

**Request body:**
```json
{
  "firstName": "Anna",
  "lastName":  "Kowalska-Nowak",
  "email":     "anna.new@mail.com",
  "phone":     null
}
```

**204 No Content** (success — no body)  
**404 Not Found:** `{ "message": "Entity with id 5 was not found." }`

## YOUR TASK — Write the C# code

Repository returns `bool` (true = updated, false = ID not found).  
Service converts `false` into `NotFoundException`.

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: returns Task<bool>




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<bool> UpdateAsync(int id, UpdateCustomerDto dto)
{
    // TODO: bind @Id, @FirstName, @LastName, @Email, @Phone (nullable!),
    //       return ExecuteNonQueryAsync() > 0




}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task UpdateAsync(int id, UpdateCustomerDto dto)
{
    // TODO: call repo, throw NotFoundException if false




}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: PUT /api/customers/{id}
// Returns: 204, 404
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task<bool> UpdateAsync(int id, UpdateCustomerDto dto);

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<bool> UpdateAsync(int id, UpdateCustomerDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(@"
        UPDATE Customer
        SET FirstName = @FirstName,
            LastName  = @LastName,
            Email     = @Email,
            Phone     = @Phone
        WHERE Id = @Id", conn);
    cmd.Parameters.AddWithValue("@Id",        id);
    cmd.Parameters.AddWithValue("@FirstName", dto.FirstName);
    cmd.Parameters.AddWithValue("@LastName",  dto.LastName);
    cmd.Parameters.AddWithValue("@Email",     dto.Email);
    cmd.Parameters.AddWithValue("@Phone",     (object?)dto.Phone ?? DBNull.Value);

    return await cmd.ExecuteNonQueryAsync() > 0;
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task UpdateAsync(int id, UpdateCustomerDto dto)
{
    bool updated = await _repo.UpdateAsync(id, dto);
    if (!updated) throw new NotFoundException(id);
}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
[HttpPut("{id:int}")]
public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
{
    try
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }
    catch (NotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}
```

</details>

---

---

# EXERCISE 10 — PATCH status update (Pattern 4l)

**Endpoint:** `PATCH /api/rentals/{id}/status`  
Body: plain JSON string (e.g. `"cancelled"`).

## Given — SQL

```sql
UPDATE Rental
SET StatusId = (SELECT Id FROM RentalStatus WHERE Name = @Status)
WHERE Id = @Id
```

## Given — Expected JSON

**Request body:** `"cancelled"`  
**204 No Content** (success)  
**404:** `{ "message": "Entity with id 7 was not found." }`  
**400:** `{ "message": "Invalid status 'archived'." }`

## YOUR TASK — Write the C# code

Service validates that `newStatus` is one of: `pending`, `active`, `returned`, `cancelled`.

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: Task<bool> UpdateStatusAsync(int id, string newStatus)




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<bool> UpdateStatusAsync(int id, string newStatus)
{
    // TODO: subquery in UPDATE to resolve status name → ID
    //       return ExecuteNonQueryAsync() > 0




}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task UpdateStatusAsync(int id, string newStatus)
{
    // TODO: validate against allowed values → ArgumentException
    //       call repo → NotFoundException if false




}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: PATCH /api/rentals/{id}/status
// Body: [FromBody] string newStatus
// Returns: 204, 404, 400
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task<bool> UpdateStatusAsync(int id, string newStatus);

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<bool> UpdateStatusAsync(int id, string newStatus)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(@"
        UPDATE Rental
        SET StatusId = (SELECT Id FROM RentalStatus WHERE Name = @Status)
        WHERE Id = @Id", conn);
    cmd.Parameters.AddWithValue("@Id",     id);
    cmd.Parameters.AddWithValue("@Status", newStatus);
    return await cmd.ExecuteNonQueryAsync() > 0;
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task UpdateStatusAsync(int id, string newStatus)
{
    var valid = new[] { "pending", "active", "returned", "cancelled" };
    if (!valid.Contains(newStatus))
        throw new ArgumentException($"Invalid status '{newStatus}'.");

    bool updated = await _repo.UpdateStatusAsync(id, newStatus);
    if (!updated) throw new NotFoundException(id);
}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
[HttpPatch("{id:int}/status")]
public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
{
    try
    {
        await _service.UpdateStatusAsync(id, newStatus);
        return NoContent();
    }
    catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
}
```

</details>

---

---

# EXERCISE 11 — PATCH set return date (Pattern 4m)

**Endpoint:** `PATCH /api/rentals/{id}/return`  
No request body. Server sets `ReturnDate = DateTime.UtcNow` and status to `'returned'`.

## Given — SQL

```sql
UPDATE Rental
SET ReturnDate = @ReturnDate,
    StatusId   = (SELECT Id FROM RentalStatus WHERE Name = 'returned')
WHERE Id = @Id
```

## Given — Expected JSON

**204 No Content** (success — no body)  
**404:** `{ "message": "Entity with id 7 was not found." }`

## YOUR TASK — Write the C# code

Controller passes `DateTime.UtcNow` to the service. Repository accepts the `DateTime` as a parameter.

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: Task<bool> SetReturnDateAsync(int id, DateTime returnDate)




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<bool> SetReturnDateAsync(int id, DateTime returnDate)
{
    // TODO: bind @Id and @ReturnDate, return rows affected > 0




}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task SetReturnDateAsync(int id, DateTime returnDate)
{
    // TODO: NotFoundException if repo returns false




}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: PATCH /api/rentals/{id}/return
// No [FromBody] — date comes from server
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task<bool> SetReturnDateAsync(int id, DateTime returnDate);

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<bool> SetReturnDateAsync(int id, DateTime returnDate)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(@"
        UPDATE Rental
        SET ReturnDate = @ReturnDate,
            StatusId   = (SELECT Id FROM RentalStatus WHERE Name = 'returned')
        WHERE Id = @Id", conn);
    cmd.Parameters.AddWithValue("@Id",         id);
    cmd.Parameters.AddWithValue("@ReturnDate", returnDate);
    return await cmd.ExecuteNonQueryAsync() > 0;
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task SetReturnDateAsync(int id, DateTime returnDate)
{
    bool updated = await _repo.SetReturnDateAsync(id, returnDate);
    if (!updated) throw new NotFoundException(id);
}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
[HttpPatch("{id:int}/return")]
public async Task<IActionResult> Return(int id)
{
    try
    {
        await _service.SetReturnDateAsync(id, DateTime.UtcNow);
        return NoContent();
    }
    catch (NotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}
```

</details>

---

---

# EXERCISE 12 — DELETE simple (Pattern 4n)

**Endpoint:** `DELETE /api/customers/{id}`

## Given — SQL

```sql
DELETE FROM Customer WHERE Id = @Id
```

## Given — Expected JSON

**204 No Content** (success — no body)  
**404:** `{ "message": "Entity with id 5 was not found." }`

## YOUR TASK — Write the C# code

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: Task<bool> DeleteAsync(int id)




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<bool> DeleteAsync(int id)
{
    // TODO: ExecuteNonQueryAsync, return rows > 0




}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task DeleteAsync(int id)
{
    // TODO: NotFoundException if false




}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: DELETE /api/customers/{id}
// Returns: 204, 404
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task<bool> DeleteAsync(int id);

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task<bool> DeleteAsync(int id)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var cmd = new SqlCommand(
        "DELETE FROM Customer WHERE Id = @Id", conn);
    cmd.Parameters.AddWithValue("@Id", id);
    return await cmd.ExecuteNonQueryAsync() > 0;
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task DeleteAsync(int id)
{
    bool deleted = await _repo.DeleteAsync(id);
    if (!deleted) throw new NotFoundException(id);
}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
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
```

</details>

---

---

# EXERCISE 13 — PUT transactional update (replace items) (Pattern 4o)

**Endpoint:** `PUT /api/rentals/{id}`  
Replace the `RentalDate` and completely replace all `RentalItem` rows in one transaction.

## Given — SQL (4 statements)

```sql
-- Step 1: verify rental exists
SELECT COUNT(1) FROM Rental WHERE Id = @Id

-- Step 2: update header
UPDATE Rental SET RentalDate = @RentalDate WHERE Id = @Id

-- Step 3: delete old items
DELETE FROM RentalItem WHERE RentalId = @Id

-- Step 4: insert new item (loop)
INSERT INTO RentalItem (RentalId, MovieId, Price)
VALUES (@RentalId, @MovieId, @Price)

-- (resolve movie ID before Step 3 — in a loop)
SELECT Id FROM Movie WHERE Title = @Title
```

## Given — Expected JSON

**Request body:**
```json
{
  "rentalDate": "2026-05-10T00:00:00",
  "movies": [
    { "title": "Dune",            "price": 12.50 },
    { "title": "The Dark Knight", "price": 9.99  }
  ]
}
```

**204 No Content** (success)  
**404 — rental not found:** `{ "message": "Entity with id 7 was not found." }`  
**404 — movie not found:** `{ "message": "'Dune' was not found." }`  
**400 — empty list:** `{ "message": "At least one movie is required." }`

## YOUR TASK — Write the C# code

Resolve movie IDs **before** deleting old items (so rollback is clean if a title is invalid).

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: Task UpdateRentalAsync(int id, UpdateRentalDto dto)




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task UpdateRentalAsync(int id, UpdateRentalDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // TODO: Step 1 — check rental exists, NotFoundException if not

        // TODO: Resolve movie IDs FIRST (before any delete)
        //       List<(int MovieId, decimal Price)> resolvedMovies

        // TODO: Step 2 — update RentalDate

        // TODO: Step 3 — delete old RentalItem rows

        // TODO: Step 4 — insert new RentalItem rows in a loop

        await tx.CommitAsync();
    }
    catch
    {
        await tx.RollbackAsync();
        throw;
    }
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task UpdateRentalAsync(int id, UpdateRentalDto dto)
{
    // TODO: validate movies not empty, delegate to repo




}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: PUT /api/rentals/{id}
// Returns: 204, 404, 400
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task UpdateRentalAsync(int id, UpdateRentalDto dto);

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task UpdateRentalAsync(int id, UpdateRentalDto dto)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // Step 1: verify rental exists
        await using var checkExists = new SqlCommand(
            "SELECT COUNT(1) FROM Rental WHERE Id = @Id", conn, tx);
        checkExists.Parameters.AddWithValue("@Id", id);
        if ((int)(await checkExists.ExecuteScalarAsync())! == 0)
            throw new NotFoundException(id);

        // Resolve movie IDs BEFORE deleting — clean rollback if any title is invalid
        var resolvedMovies = new List<(int MovieId, decimal Price)>();
        foreach (var item in dto.Movies)
        {
            await using var findMovie = new SqlCommand(
                "SELECT Id FROM Movie WHERE Title = @Title", conn, tx);
            findMovie.Parameters.AddWithValue("@Title", item.Title);
            var found = await findMovie.ExecuteScalarAsync();
            if (found is null)
                throw new NotFoundException(item.Title);
            resolvedMovies.Add(((int)found, item.Price));
        }

        // Step 2: update header
        await using var updateHeader = new SqlCommand(
            "UPDATE Rental SET RentalDate = @RentalDate WHERE Id = @Id", conn, tx);
        updateHeader.Parameters.AddWithValue("@Id",         id);
        updateHeader.Parameters.AddWithValue("@RentalDate", dto.RentalDate);
        await updateHeader.ExecuteNonQueryAsync();

        // Step 3: delete all old items
        await using var deleteItems = new SqlCommand(
            "DELETE FROM RentalItem WHERE RentalId = @Id", conn, tx);
        deleteItems.Parameters.AddWithValue("@Id", id);
        await deleteItems.ExecuteNonQueryAsync();

        // Step 4: insert new items
        foreach (var (movieId, price) in resolvedMovies)
        {
            await using var insertItem = new SqlCommand("""
                INSERT INTO RentalItem (RentalId, MovieId, Price)
                VALUES (@RentalId, @MovieId, @Price)
                """, conn, tx);
            insertItem.Parameters.AddWithValue("@RentalId", id);
            insertItem.Parameters.AddWithValue("@MovieId",  movieId);
            insertItem.Parameters.AddWithValue("@Price",    price);
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

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task UpdateRentalAsync(int id, UpdateRentalDto dto)
{
    if (!dto.Movies.Any())
        throw new ArgumentException("At least one movie is required.");

    await _repo.UpdateRentalAsync(id, dto);
}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
[HttpPut("{id:int}")]
public async Task<IActionResult> Update(int id, [FromBody] UpdateRentalDto dto)
{
    try
    {
        await _service.UpdateRentalAsync(id, dto);
        return NoContent();
    }
    catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
}
```

</details>

---

---

# EXERCISE 14 — PATCH transactional status with side-effects (Pattern 4p)

**Endpoint:** `PATCH /api/rentals/{id}/status`  
Body: `"cancelled"`.  
When cancelling: update status → log to `StatusLog` → restore `Stock.Qty` for each movie.

## Given — SQL (5 statements)

```sql
-- Step 1: read current status (existence check too)
SELECT rs.Name
FROM Rental r
JOIN RentalStatus rs ON rs.Id = r.StatusId
WHERE r.Id = @Id

-- Step 2: update status
UPDATE Rental
SET StatusId = (SELECT Id FROM RentalStatus WHERE Name = @NewStatus)
WHERE Id = @Id

-- Step 3: insert into StatusLog
INSERT INTO StatusLog (RentalId, OldStatus, NewStatus, ChangedAt)
VALUES (@RentalId, @OldStatus, @NewStatus, @ChangedAt)

-- Step 4a: read movie IDs for stock restore
SELECT MovieId FROM RentalItem WHERE RentalId = @RentalId

-- Step 4b: restore stock (per movie, in a loop)
UPDATE Stock SET Qty = Qty + 1 WHERE MovieId = @MovieId
```

## Given — Expected JSON

**Request body:** `"cancelled"`  
**204 No Content** (success)  
**409 — already cancelled:** `{ "message": "Entity is already 'cancelled'." }`  
**409 — returned → cannot cancel:** `{ "message": "Cannot cancel a returned rental." }`  
**400 — invalid status:** `{ "message": "Invalid status 'archived'." }`  
**404:** `{ "message": "Entity with id 7 was not found." }`

## YOUR TASK — Write the C# code

Critical trap: after reading movie IDs with a `SqlDataReader`, you **must call `CloseAsync()`** before running more commands on the same connection inside the same transaction.

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: Task UpdateStatusWithSideEffectsAsync(int id, string newStatus)




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task UpdateStatusWithSideEffectsAsync(int id, string newStatus)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // TODO: Step 1 — read current status, throw NotFoundException if null

        // TODO: Step 2 (C# only) — guard invalid transitions:
        //       same status → ConflictException
        //       returned + trying to cancel → ConflictException

        // TODO: Step 3 — update Rental.StatusId

        // TODO: Step 4 — insert StatusLog row

        // TODO: Step 5 (only if newStatus == "cancelled"):
        //       read MovieIds from RentalItem into a List<int>
        //       CloseAsync() the reader (REQUIRED before next command!)
        //       loop and UPDATE Stock

        await tx.CommitAsync();
    }
    catch
    {
        await tx.RollbackAsync();
        throw;
    }
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task UpdateStatusWithSideEffectsAsync(int id, string newStatus)
{
    // TODO: validate allowed statuses → ArgumentException
    //       delegate to repo (NotFoundException and ConflictException propagate automatically)




}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: PATCH /api/rentals/{id}/status
// Returns: 204, 404, 409, 400
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task UpdateStatusWithSideEffectsAsync(int id, string newStatus);

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task UpdateStatusWithSideEffectsAsync(int id, string newStatus)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // Step 1: read current status
        await using var readCurrent = new SqlCommand(@"
            SELECT rs.Name
            FROM Rental r
            JOIN RentalStatus rs ON rs.Id = r.StatusId
            WHERE r.Id = @Id", conn, tx);
        readCurrent.Parameters.AddWithValue("@Id", id);
        var currentStatusObj = await readCurrent.ExecuteScalarAsync();
        if (currentStatusObj is null)
            throw new NotFoundException(id);
        string currentStatus = (string)currentStatusObj;

        // Step 2: guard invalid transitions
        if (currentStatus == newStatus)
            throw new ConflictException($"Entity is already '{newStatus}'.");
        if (currentStatus == "returned" && newStatus == "cancelled")
            throw new ConflictException("Cannot cancel a returned rental.");

        // Step 3: update status
        await using var updateStatus = new SqlCommand(@"
            UPDATE Rental
            SET StatusId = (SELECT Id FROM RentalStatus WHERE Name = @NewStatus)
            WHERE Id = @Id", conn, tx);
        updateStatus.Parameters.AddWithValue("@Id",        id);
        updateStatus.Parameters.AddWithValue("@NewStatus", newStatus);
        await updateStatus.ExecuteNonQueryAsync();

        // Step 4: insert StatusLog
        await using var insertLog = new SqlCommand("""
            INSERT INTO StatusLog (RentalId, OldStatus, NewStatus, ChangedAt)
            VALUES (@RentalId, @OldStatus, @NewStatus, @ChangedAt)
            """, conn, tx);
        insertLog.Parameters.AddWithValue("@RentalId",  id);
        insertLog.Parameters.AddWithValue("@OldStatus", currentStatus);
        insertLog.Parameters.AddWithValue("@NewStatus", newStatus);
        insertLog.Parameters.AddWithValue("@ChangedAt", DateTime.UtcNow);
        await insertLog.ExecuteNonQueryAsync();

        // Step 5: restore stock if cancelling
        if (newStatus == "cancelled")
        {
            await using var getMovies = new SqlCommand(
                "SELECT MovieId FROM RentalItem WHERE RentalId = @RentalId", conn, tx);
            getMovies.Parameters.AddWithValue("@RentalId", id);
            await using var movieReader = await getMovies.ExecuteReaderAsync();

            var movieIds = new List<int>();
            int movieIdOrd = movieReader.GetOrdinal("MovieId");
            while (await movieReader.ReadAsync())
                movieIds.Add(movieReader.GetInt32(movieIdOrd));

            await movieReader.CloseAsync();  // MUST close before running next command on same conn

            foreach (var movieId in movieIds)
            {
                await using var restoreStock = new SqlCommand(
                    "UPDATE Stock SET Qty = Qty + 1 WHERE MovieId = @MovieId", conn, tx);
                restoreStock.Parameters.AddWithValue("@MovieId", movieId);
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

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task UpdateStatusWithSideEffectsAsync(int id, string newStatus)
{
    var valid = new[] { "pending", "active", "returned", "cancelled" };
    if (!valid.Contains(newStatus))
        throw new ArgumentException($"Invalid status '{newStatus}'.");

    await _repo.UpdateStatusWithSideEffectsAsync(id, newStatus);
}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
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

**Critical trap:** `await movieReader.CloseAsync()` before the stock UPDATE loop. Two open readers on one connection → `InvalidOperationException`. Collecting into a `List<int>` first and then closing is the safe pattern.

</details>

---

---

# EXERCISE 15 — DELETE transactional cascade (Pattern 4q)

**Endpoint:** `DELETE /api/customers/{id}`  
Business rule: cannot delete a customer who has an `active` rental.  
Cascade order: `StatusLog` → `RentalItem` → `Rental` → `Customer`.

## Given — SQL (6 statements)

```sql
-- Step 1: check customer exists
SELECT COUNT(1) FROM Customer WHERE Id = @Id

-- Step 2: guard — check for active rentals
SELECT COUNT(1)
FROM Rental r
JOIN RentalStatus rs ON rs.Id = r.StatusId
WHERE r.CustomerId = @Id AND rs.Name = 'active'

-- Step 3: delete StatusLog (grandchildren)
DELETE sl
FROM StatusLog sl
JOIN Rental r ON r.Id = sl.RentalId
WHERE r.CustomerId = @Id

-- Step 4: delete RentalItem (children of Rental)
DELETE ri
FROM RentalItem ri
JOIN Rental r ON r.Id = ri.RentalId
WHERE r.CustomerId = @Id

-- Step 5: delete Rental (children of Customer)
DELETE FROM Rental WHERE CustomerId = @Id

-- Step 6: delete Customer (parent)
DELETE FROM Customer WHERE Id = @Id
```

## Given — Expected JSON

**204 No Content** (success)  
**404:** `{ "message": "Entity with id 3 was not found." }`  
**409:** `{ "message": "Cannot delete customer with active rentals." }`

## YOUR TASK — Write the C# code

All 6 commands run inside the same transaction. FK deletion order is mandatory: deepest level first.

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
// TODO: Task DeleteWithCascadeAsync(int id)




// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task DeleteWithCascadeAsync(int id)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // TODO: Step 1 — check customer exists (NotFoundException)

        // TODO: Step 2 — check for active rentals (ConflictException)

        // TODO: Step 3 — delete StatusLog rows joined through Rental

        // TODO: Step 4 — delete RentalItem rows joined through Rental

        // TODO: Step 5 — delete Rental rows

        // TODO: Step 6 — delete Customer

        await tx.CommitAsync();
    }
    catch
    {
        await tx.RollbackAsync();
        throw;
    }
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task DeleteWithCascadeAsync(int id)
{
    // TODO: delegate — exceptions propagate automatically




}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
// Route: DELETE /api/customers/{id}
// Returns: 204, 404, 409
```

<details>
<summary>▶ ANSWER</summary>

```csharp
// ── INTERFACE ────────────────────────────────────────────────────────────────
Task DeleteWithCascadeAsync(int id);

// ── REPOSITORY ───────────────────────────────────────────────────────────────
public async Task DeleteWithCascadeAsync(int id)
{
    await using var conn = new SqlConnection(_cs);
    await conn.OpenAsync();
    await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

    try
    {
        // Step 1: verify customer exists
        await using var checkExists = new SqlCommand(
            "SELECT COUNT(1) FROM Customer WHERE Id = @Id", conn, tx);
        checkExists.Parameters.AddWithValue("@Id", id);
        if ((int)(await checkExists.ExecuteScalarAsync())! == 0)
            throw new NotFoundException(id);

        // Step 2: guard — no active rentals
        await using var checkActive = new SqlCommand(@"
            SELECT COUNT(1)
            FROM Rental r
            JOIN RentalStatus rs ON rs.Id = r.StatusId
            WHERE r.CustomerId = @Id AND rs.Name = 'active'", conn, tx);
        checkActive.Parameters.AddWithValue("@Id", id);
        if ((int)(await checkActive.ExecuteScalarAsync())! > 0)
            throw new ConflictException("Cannot delete customer with active rentals.");

        // Step 3: delete StatusLog (grandchildren — deepest FK)
        await using var deleteLog = new SqlCommand("""
            DELETE sl
            FROM StatusLog sl
            JOIN Rental r ON r.Id = sl.RentalId
            WHERE r.CustomerId = @Id
            """, conn, tx);
        deleteLog.Parameters.AddWithValue("@Id", id);
        await deleteLog.ExecuteNonQueryAsync();

        // Step 4: delete RentalItem (children of Rental)
        await using var deleteItems = new SqlCommand("""
            DELETE ri
            FROM RentalItem ri
            JOIN Rental r ON r.Id = ri.RentalId
            WHERE r.CustomerId = @Id
            """, conn, tx);
        deleteItems.Parameters.AddWithValue("@Id", id);
        await deleteItems.ExecuteNonQueryAsync();

        // Step 5: delete Rental
        await using var deleteRentals = new SqlCommand(
            "DELETE FROM Rental WHERE CustomerId = @Id", conn, tx);
        deleteRentals.Parameters.AddWithValue("@Id", id);
        await deleteRentals.ExecuteNonQueryAsync();

        // Step 6: delete Customer
        await using var deleteCustomer = new SqlCommand(
            "DELETE FROM Customer WHERE Id = @Id", conn, tx);
        deleteCustomer.Parameters.AddWithValue("@Id", id);
        await deleteCustomer.ExecuteNonQueryAsync();

        await tx.CommitAsync();
    }
    catch
    {
        await tx.RollbackAsync();
        throw;
    }
}

// ── SERVICE ──────────────────────────────────────────────────────────────────
public async Task DeleteWithCascadeAsync(int id)
{
    await _repo.DeleteWithCascadeAsync(id);
}

// ── CONTROLLER ───────────────────────────────────────────────────────────────
[HttpDelete("{id:int}")]
public async Task<IActionResult> Delete(int id)
{
    try
    {
        await _service.DeleteWithCascadeAsync(id);
        return NoContent();
    }
    catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
    catch (ConflictException ex) { return Conflict(new { message = ex.Message }); }
}
```

**FK deletion order:** StatusLog → RentalItem → Rental → Customer. Reversing this order throws a FK constraint violation at runtime. The rule: delete in the exact reverse of INSERT order.

</details>

---

---

## Quick-Check Rules (lose points if violated)

```
□ GetOrdinal() called BEFORE the while loop, AFTER ReadAsync() opens the result set
□ IsDBNull() checked on EVERY nullable column before GetString/GetInt32/GetDecimal/GetDateTime
□ Nullable C# param → SQL: (object?)val ?? DBNull.Value
□ SCOPE_IDENTITY() not @@IDENTITY
□ Convert.ToInt32(await cmd.ExecuteScalarAsync()) for new insert ID
□ ExecuteNonQueryAsync() > 0 to detect "row not found" on UPDATE/DELETE
□ Every SqlCommand inside a transaction: new SqlCommand("...", conn, tx)  ← never forget tx
□ throw; not throw ex;  (preserves stack trace)
□ CloseAsync() the reader before another command on the same connection
□ Repository returns null/false — never throws HTTP codes
□ Service converts null → NotFoundException, false → NotFoundException
□ Controller catches exceptions → maps to correct HTTP codes
□ [ApiController] auto-validates DTOs → 400 without manual ModelState check
□ Empty list → 200 + [] (never 404)
□ Cascade delete order: deepest FK first → walk up to parent
```
