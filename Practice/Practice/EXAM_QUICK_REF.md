# EXAM QUICK REFERENCE — Open this on exam day

---

## ⏱ FIRST 5 MINUTES — read before touching the keyboard

```
1. Read BOTH endpoints in the task
2. Draw the DB schema on paper → identify:
   □ Table names  (rename everything in the template)
   □ PK/FK columns
   □ Which column is nullable?  (DateTime? / decimal? / string?)
   □ Which column do you look up by?  (title? isbn? room_number? name?)
   □ What is the exact status name for POST?  ("Scheduled" / "Active" / "Rented" / "Confirmed")
   □ Does the GET response need extra JOIN fields?  (trainer_name / author / doctor_name?)
   □ Does the child DTO have a nullable date?  (return_date / check_out / ended_at?)
3. Count your DTOs: you need exactly 5 (parent, child, line-item, POST body, POST item)
4. THEN open Rider and rename the template
```

---

## ⚡ TERMINAL SETUP (run first, 3 minutes)

```bash
dotnet new webapi -n MyProject --no-openapi
cd MyProject
dotnet add package Microsoft.Data.SqlClient

# ADD .gitignore BEFORE first commit — missing it = -50% of total score
git init
# paste .gitignore content (bin/ obj/ .idea/ *.user)
git add .gitignore
git commit -m "init"

# Remove template garbage
rm WeatherForecast.cs Controllers/WeatherForecastController.cs

# Create folder structure
mkdir Models DTOs Repositories
```

---

## 📁 REQUIRED FOLDER STRUCTURE

```
MyProject/
├── .gitignore                   ← bin/ obj/ .idea/ *.user  — COMMIT FIRST
├── appsettings.json             ← connection string (never hardcode it)
├── Program.cs                   ← AddScoped<IRepo, Repo>() — required for DI marks
├── Controllers/
│   └── XxxController.cs         ← HTTP only: parse → call repo → return status code
├── Models/                      ← one class per DB table (optional but clean)
├── DTOs/
│   ├── XxxDto.cs                ← GET response DTOs (parent + child + line-item)
│   └── AddXxxDto.cs             ← POST request DTOs (body + line-item)
└── Repositories/
    ├── IXxxRepository.cs        ← REQUIRED — no interface = up to -10 pts
    └── XxxRepository.cs         ← implementation: GET (JOIN+Dictionary) + POST (transaction)
```

---

## 🔁 THE PATTERN — every exam is a variation of this

### GET endpoint
```
Controller receives:  GET /api/clients/{id}/bookings
Calls:                await _repo.GetClientWithBookingsAsync(id)
Repo returns:         ClientDto? (null = not found, value = found)
Controller returns:   result is null ? NotFound() : Ok(result)

Inside the repo:
  1. Open connection (await using, await OpenAsync)
  2. Single LEFT JOIN query across all tables
  3. while(await reader.ReadAsync()):
       a. Build parent ClientDto once (if result == null)
       b. IsDBNull(booking_id) → continue  (client has 0 bookings)
       c. TryGetValue bookingId → create BookingDto if new, add to dict + parent list
       d. IsDBNull(class_name) → continue  (booking has 0 classes)
       e. bookingDto.Classes.Add(new ClassInBookingDto { ... })
  4. return result  (null if no rows, ClientDto if rows found)
```

### POST endpoint
```
Controller receives:  POST /api/clients/{id}/bookings  + JSON body
Calls:                await _repo.AddBookingAsync(id, dto)
Repo returns:         int (new ID) or throws KeyNotFoundException
Controller returns:   CreatedAtAction(...) or NotFound(ex.Message)

Inside the repo:
  1. Open connection
  2. BeginTransactionAsync  → await using var tx = (SqlTransaction)await conn.BeginTransactionAsync()
  3. COUNT(1) parent → throw KeyNotFoundException if 0
  4. (Optional) COUNT(1) extra FK → throw KeyNotFoundException if 0
  5. foreach item: SELECT id FROM lookup WHERE name=@Name → throw if null → store (id, price)
  6. SELECT status_id FROM Status WHERE name='Scheduled'
  7. INSERT header → VALUES(...); SELECT SCOPE_IDENTITY() → ExecuteScalarAsync → newId
  8. foreach (id, price): INSERT junction table → ExecuteNonQueryAsync
  9. CommitAsync
  catch: RollbackAsync + throw (re-throw, not throw ex)
```

---

## 🔴 CRITICAL RULES — these cost the most marks

| Rule | Penalty if broken |
|---|---|
| No repository interface (`IXxxRepository`) | up to **-10 pts** |
| Not async (`async Task<>` + `await` everywhere) | up to **-10 pts** |
| No dependency injection (using `new Repo()` in controller) | up to **-5 pts** |
| Wrong HTTP status codes | up to **-5 pts** |
| Bad naming / unreadable code | up to **-4 pts** |
| `bin/` or `obj/` committed (missing `.gitignore`) | **-50% total** |
| Code doesn't compile | **0 pts** |
| Not on GitHub | **0 pts** |

---

## 📝 COPY-PASTE SNIPPETS

### Check entity exists
```csharp
await using var cmd = new SqlCommand(
    "SELECT COUNT(1) FROM TableName WHERE id_col = @Id", conn, tx);
cmd.Parameters.AddWithValue("@Id", theId);
if ((int)(await cmd.ExecuteScalarAsync())! == 0)
    throw new KeyNotFoundException($"Entity {theId} not found.");
```

### Look up ID by name
```csharp
await using var cmd = new SqlCommand(
    "SELECT id_col FROM LookupTable WHERE name_col = @Name", conn, tx);
cmd.Parameters.AddWithValue("@Name", item.Name);
var idObj = await cmd.ExecuteScalarAsync();
if (idObj == null)
    throw new KeyNotFoundException($"'{item.Name}' not found.");
int resolvedId = (int)idObj;
```

### INSERT header + get new ID
```csharp
await using var cmd = new SqlCommand(@"
    INSERT INTO HeaderTable (col1, col2, status_id)
    VALUES (@Val1, @Val2, @StatusId);
    SELECT SCOPE_IDENTITY();", conn, tx);
cmd.Parameters.AddWithValue("@Val1", value1);
cmd.Parameters.AddWithValue("@Val2", value2);
cmd.Parameters.AddWithValue("@StatusId", statusId);
int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
```

### INSERT line item
```csharp
await using var cmd = new SqlCommand(@"
    INSERT INTO JunctionTable (header_id, item_id, price)
    VALUES (@HeaderId, @ItemId, @Price)", conn, tx);
cmd.Parameters.AddWithValue("@HeaderId", newId);
cmd.Parameters.AddWithValue("@ItemId", resolvedId);
cmd.Parameters.AddWithValue("@Price", price);
await cmd.ExecuteNonQueryAsync();
```

### NULL parameter (for nullable columns in INSERT)
```csharp
// C# null → SQL NULL — must convert explicitly
cmd.Parameters.AddWithValue("@ReturnDate", (object?)nullableValue ?? DBNull.Value);
```

### Nullable column in reader
```csharp
// DateTime?
ReturnDate = reader.IsDBNull(reader.GetOrdinal("return_date"))
                 ? (DateTime?)null
                 : reader.GetDateTime(reader.GetOrdinal("return_date")),

// string (empty fallback)
TrainerName = reader.IsDBNull(reader.GetOrdinal("trainer_name"))
                  ? string.Empty
                  : reader.GetString(reader.GetOrdinal("trainer_name")),

// decimal?
Price = reader.IsDBNull(reader.GetOrdinal("price"))
            ? (decimal?)null
            : reader.GetDecimal(reader.GetOrdinal("price")),
```

### Full transaction skeleton
```csharp
await using var conn = new SqlConnection(_connectionString);
await conn.OpenAsync();
await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();
try
{
    // ... all SqlCommands use new SqlCommand(sql, conn, tx) ...
    await tx.CommitAsync();
    return newId;
}
catch
{
    await tx.RollbackAsync();
    throw;  // 'throw' not 'throw ex' — preserves stack trace
}
```

### Program.cs registration
```csharp
builder.Services.AddControllers();
builder.Services.AddScoped<IXxxRepository, XxxRepository>();
// For 3-layer (repo + service):
// builder.Services.AddScoped<IXxxRepository, XxxRepository>();
// builder.Services.AddScoped<IXxxService,    XxxService>();
```

### Controller skeleton
```csharp
[ApiController]
[Route("api/entities")]
public class EntitiesController : ControllerBase
{
    private readonly IXxxRepository _repo;
    public EntitiesController(IXxxRepository repo) => _repo = repo;

    [HttpGet("{id:int}/children")]
    public async Task<IActionResult> GetChildren(int id)
    {
        var result = await _repo.GetWithChildrenAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{id:int}/children")]
    public async Task<IActionResult> AddChild(int id, [FromBody] AddChildDto dto)
    {
        try
        {
            int newId = await _repo.AddChildAsync(id, dto);
            return CreatedAtAction(nameof(GetChildren), new { id = newId }, new { id = newId });
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }
}
```

---

## 🧠 EXAM THEORY ANSWERS — memorise these

| Question | Answer |
|---|---|
| Why `AddScoped` not `AddSingleton`? | `SqlConnection` is not thread-safe. Scoped = one per request = safe. Singleton = shared across all requests = thread-safe issues. |
| Scoped in Singleton? | Captive dependency — Scoped never released → memory leak. Never inject Scoped into Singleton. |
| `SCOPE_IDENTITY()` vs `@@IDENTITY`? | `SCOPE_IDENTITY` = ID from last INSERT in current scope. `@@IDENTITY` = affected by triggers on other tables. Always use `SCOPE_IDENTITY`. |
| Why `throw` not `throw ex`? | `throw` re-throws with original stack trace. `throw ex` resets it → harder to debug. |
| Why LEFT JOIN not INNER JOIN? | INNER JOIN: parent with 0 children → 0 rows → null → 404 (wrong). LEFT JOIN: parent with 0 children → 1 row with NULLs → we skip it → return parent with empty list (correct). |
| `IsDBNull` — why check it? | Calling `GetString()`/`GetDateTime()` on a NULL DB value throws `InvalidCastException`. Always `IsDBNull` first on nullable columns. |
| `ExecuteScalarAsync` vs `ExecuteNonQueryAsync`? | Scalar = for queries returning ONE value (COUNT, SCOPE_IDENTITY, SELECT single col). NonQuery = for INSERT/UPDATE/DELETE (returns rows affected). ReaderAsync = for SELECT returning rows. |
| `CreatedAtAction` vs `Created`? | `CreatedAtAction` = 201 + `Location` header pointing to the new resource. `Created("")` = 201 but no Location. Exam expects `CreatedAtAction`. |
| 400 vs 409? | 400 = request is malformed (validation failed). 409 = request is valid but server state prevents it (book already on loan, PESEL already registered). |
| `cmd.Transaction = tx` same as `new SqlCommand(sql, conn, tx)`? | Yes. Constructor form is cleaner. Both ensure the command runs inside the transaction. Forgetting this → `InvalidOperationException`. |
| Why `Convert.ToInt32` on `ExecuteScalarAsync` result? | `SCOPE_IDENTITY()` returns `decimal` (SQL type). Direct cast `(int)` fails. `Convert.ToInt32` handles the type conversion safely. |
| Middleware order? | Exception handler FIRST (wraps everything). Then HTTPS redirect. Then routing. Then auth. Then MapControllers LAST. |

---

## ✅ FINAL CHECKLIST — run through before committing

```
□ Every repo method is async Task<...>
□ Every DB call has await
□ IXxxRepository interface exists and is registered with AddScoped in Program.cs
□ Controller uses constructor injection (_repo = repo) — no 'new XxxRepository()' anywhere
□ Transaction: BeginTransactionAsync, all cmds have (conn, tx), CommitAsync, RollbackAsync in catch
□ IsDBNull checked before GetString/GetDateTime/GetDecimal on every nullable column
□ Null SQL params use (object?)val ?? DBNull.Value
□ SCOPE_IDENTITY() used — not @@IDENTITY
□ Convert.ToInt32 wraps ExecuteScalarAsync result for the new ID
□ Controller returns 201 CreatedAtAction (not 200 Ok) on successful POST
□ Controller returns 404 NotFound when repo returns null or throws KeyNotFoundException
□ .gitignore has bin/ and obj/ — those folders are NOT in the repo
□ Connection string is in appsettings.json (not hardcoded in repository)
□ git add . && git commit -m "solution" && git push
```
