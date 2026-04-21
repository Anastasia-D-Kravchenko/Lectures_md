# ADO.NET in C# — Deep Guide with Examples & Under-the-Hood Explanations

---

## 1. What is ADO.NET and Why Does It Exist?

When your C# application needs to talk to a database, something must bridge the gap between C# objects and SQL Server rows. That bridge is **ADO.NET** — Active Data Objects for .NET.

ADO.NET is a set of classes in the `System.Data` and `Microsoft.Data.SqlClient` namespaces that allow you to:
- Open connections to a SQL database
- Send SQL queries
- Read results row by row
- Execute transactions
- Use parameterized queries (the main defense against SQL injection)

It is the **lowest-level** database access technology in the .NET stack. Libraries like Entity Framework Core sit *on top of* ADO.NET and use it internally. Understanding ADO.NET means you understand what every ORM is doing underneath.

### The big picture

```
Your C# code
    ↓
ADO.NET (SqlConnection, SqlCommand, SqlDataReader)
    ↓
Microsoft.Data.SqlClient (network driver)
    ↓
TDS protocol (Tabular Data Stream) over TCP
    ↓
SQL Server
```

Every EF Core query ultimately compiles down to exactly the same ADO.NET code you will write by hand in this guide. The difference is just who writes the boilerplate.

---

## 2. SqlConnection — Opening the Gate to the Database

A `SqlConnection` represents a **single logical connection** to a SQL Server database. Think of it as a phone call to the database — before you can ask anything, you must dial in.

### The connection string

A connection string tells `SqlConnection` where to connect and how to authenticate:

```csharp
// Typical connection string stored in appsettings.json
// (never hardcode it in the class — more on this in section 10)
"Server=localhost;Database=ShopDb;Integrated Security=true;TrustServerCertificate=true"
```

The components:
- `Server` — hostname or IP, e.g. `localhost` or `myserver.database.windows.net`
- `Database` — which database to use on that server
- `Integrated Security=true` — use Windows authentication (no username/password)
- `User Id=...;Password=...` — SQL authentication alternative
- `TrustServerCertificate=true` — accept self-signed certificates in development

### Opening and closing — always use `await using`

```csharp
// ❌ BAD — if an exception occurs between Open() and Close(), the connection leaks
var connection = new SqlConnection(connectionString);
await connection.OpenAsync();
// ... if something throws here ...
await connection.CloseAsync(); // never reached!

// ✅ GOOD — using guarantees Dispose() (= Close()) even on exception
await using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();
// ... rest of the work ...
// Dispose() is called automatically at the end of the block
```

**Why `await using` and not just `using`?**

`await using` calls `DisposeAsync()` instead of `Dispose()`. For `SqlConnection`, `DisposeAsync()` properly releases the connection back to the pool asynchronously without blocking a thread. In ASP.NET Core you should always prefer the async variant.

### Open() vs OpenAsync() — why it matters for scalability

```csharp
// ❌ BAD in ASP.NET Core — blocks the thread while waiting for a connection
connection.Open();

// ✅ GOOD — releases the thread back to the thread pool while waiting
await connection.OpenAsync();
```

**What actually happens during Open():**
1. ADO.NET checks the connection pool for a free connection with a matching connection string
2. If one exists — it is returned immediately (no TCP overhead)
3. If not — a new TCP connection to SQL Server is negotiated (can take 50–500ms)

During this wait, `Open()` blocks the OS thread. In an ASP.NET Core server under load, blocking threads causes **thread pool starvation** — requests pile up waiting for a free thread, latency spikes, and the server grinds to a halt.

`OpenAsync()` returns the thread to the pool during the wait. The same number of requests can be handled with far fewer threads. This is the entire reason async/await exists in server-side .NET.

---

## 3. Connection Pooling — The Hidden Performance Engine

Every call to `new SqlConnection(...)` followed by `OpenAsync()` does **not** necessarily create a new TCP connection to the database. ADO.NET maintains a **connection pool** — a cache of reusable, already-established connections.

### How the pool works under the hood

```
First request:
new SqlConnection(cs) → pool is empty → NEW TCP connection opened
                                                     ↓
                              connection handed to your code
                                                     ↓
await using → connection.Dispose() → NOT closed! Returned to pool

Second request:
new SqlConnection(cs) → pool has a free connection → reused instantly
```

### Key facts about connection pooling

| Fact | Detail |
|---|---|
| Created automatically | No configuration needed — it works out of the box |
| Keyed by connection string | Each unique connection string gets its **own** separate pool |
| Default pool size | Min: 0, Max: **100** connections per pool |
| Close() / Dispose() | Returns the connection to the pool — does **not** close the TCP socket |
| `Pooling=false` | Disables pooling (almost never what you want in production) |

### Configuring pool size in the connection string

```csharp
// Custom pool limits — useful for high-throughput services
"Server=...;Min Pool Size=5;Max Pool Size=200;Connect Timeout=30"
//          ↑ keep 5 warm    ↑ never exceed 200    ↑ timeout if no free connection
```

### What happens when the pool is exhausted?

If all 100 connections are in use and a new request calls `OpenAsync()`, it will **wait** for a connection to be returned to the pool. If no connection is returned within `Connect Timeout` seconds (default: 15s), an `InvalidOperationException` is thrown:

```
Timeout expired. The timeout period elapsed prior to obtaining a connection from the pool.
```

This is a sign your application opens connections but doesn't close them, or that 100 is genuinely not enough and you need to increase `Max Pool Size`.

### Common pooling mistake — not closing connections

```csharp
// ❌ BAD — connection escapes the using block and is never returned to the pool
public SqlConnection GetConnection()
{
    var connection = new SqlConnection(connectionString);
    connection.Open();
    return connection; // caller must call Dispose() — but will they?
}

// ✅ GOOD — always open AND close in the same method, using 'await using'
public async Task<List<Product>> GetProductsAsync()
{
    await using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    // ... do work ...
    return products;
    // Dispose() called automatically — connection returned to pool
}
```

---

## 4. SqlCommand — Sending Instructions to the Database

A `SqlCommand` is the object that holds a SQL query and sends it to the database. It always needs two things:

1. A **SQL query string** (`CommandText`)
2. An **open `SqlConnection`** to run it against

```csharp
await using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();

// Two ways to create a SqlCommand:
// Option 1: constructor
await using var command = new SqlCommand("SELECT * FROM Products", connection);

// Option 2: factory method on the connection (preferred — ensures they're linked)
await using var command = connection.CreateCommand();
command.CommandText = "SELECT * FROM Products";
```

### The four execution methods — choosing the right one

This is one of the most commonly tested ADO.NET concepts. Each method is designed for a specific type of query:

#### ExecuteNonQueryAsync() — for INSERT, UPDATE, DELETE

Returns an `int` — the number of **rows affected**. Use this whenever you don't need to read data back.

```csharp
await using var command = new SqlCommand(
    "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)", connection);
command.Parameters.AddWithValue("@Name", "Laptop");
command.Parameters.AddWithValue("@Price", 3499.99m);

int rowsInserted = await command.ExecuteNonQueryAsync();
// rowsInserted == 1 if the insert succeeded
Console.WriteLine($"Rows inserted: {rowsInserted}");
```

#### ExecuteScalarAsync() — for a single value

Returns `object?` — the value in the **first column of the first row**. Use this when your query produces a single aggregate value (COUNT, SUM, MAX, or an identity after INSERT).

```csharp
// Example 1: COUNT
await using var countCommand = new SqlCommand(
    "SELECT COUNT(*) FROM Products WHERE Price > @MinPrice", connection);
countCommand.Parameters.AddWithValue("@MinPrice", 1000m);

object? result = await countCommand.ExecuteScalarAsync();
int count = Convert.ToInt32(result); // safe conversion from object?

// Example 2: get the ID of the newly inserted row
await using var insertCommand = new SqlCommand(
    "INSERT INTO Products (Name, Price) VALUES (@Name, @Price); SELECT SCOPE_IDENTITY();",
    connection);
insertCommand.Parameters.AddWithValue("@Name", "Mouse");
insertCommand.Parameters.AddWithValue("@Price", 129.99m);

object? newId = await insertCommand.ExecuteScalarAsync();
int productId = Convert.ToInt32(newId);
```

**Why `object?` and not `int`?** Because `ExecuteScalarAsync` has no idea what type the first column is — it could be an `int`, a `string`, a `DateTime`, or even `DBNull.Value` (SQL NULL). You must cast it yourself.

#### ExecuteReaderAsync() — for SELECT returning multiple rows

Returns a `SqlDataReader`. Use this when you expect multiple rows and/or multiple columns.

```csharp
await using var command = new SqlCommand("SELECT Id, Name, Price FROM Products", connection);
await using SqlDataReader reader = await command.ExecuteReaderAsync();

var products = new List<Product>();
while (await reader.ReadAsync()) // advance to the next row
{
    products.Add(new Product(
        Id:    reader.GetInt32(0),     // column 0 by ordinal
        Name:  reader.GetString(1),    // column 1
        Price: reader.GetDecimal(2)    // column 2
    ));
}
```

#### ExecuteXmlReaderAsync() — for SELECT … FOR XML

Returns an `XmlReader`. Used only with SQL queries that include `FOR XML` and return XML data. Rare in modern APIs.

### Decision tree: which method to use?

```
Does my query return rows (SELECT)?
├── Yes, many rows with columns  → ExecuteReaderAsync()
├── Yes, one cell only           → ExecuteScalarAsync()
└── No (INSERT / UPDATE / DELETE)
    ├── Do I need rows affected count? → ExecuteNonQueryAsync() (returns int)
    └── Do I need the new row's ID?    → ExecuteScalarAsync() + SCOPE_IDENTITY()
```

---

## 5. SqlDataReader — Reading Results Row by Row

`SqlDataReader` is the fastest way to read query results. It is a **forward-only, read-only, connected** cursor — it streams rows directly from the database as you call `ReadAsync()`.

### What "forward-only" really means

```csharp
// ❌ This will throw — you cannot go back to a previous row
await reader.ReadAsync(); // row 1
await reader.ReadAsync(); // row 2
// you cannot go back to row 1 — forward-only means one direction only
```

### What "connected" really means

The `SqlDataReader` holds the connection open and the SQL Server result set **server-side** until you finish reading. This is different from loading all rows into a `DataTable` in memory at once.

```
SQL Server                     Your Application
  ┌──────────┐     TCP          ┌───────────────────┐
  │ Result   │ ←— keep alive —→ │ SqlDataReader     │
  │ set on   │ ←— ReadAsync() → │ (one row in RAM   │
  │ server   │ ←— next row ——→  │ at a time)        │
  └──────────┘                  └───────────────────┘
```

**Advantage:** You can process millions of rows without loading them all into RAM.
**Constraint:** The connection is busy — don't do anything with it while reading.

### The Read loop — the fundamental pattern

```csharp
await using var reader = await command.ExecuteReaderAsync();

// Before the first ReadAsync(), the reader points BEFORE the first row
// ReadAsync() returns true if it successfully advanced to the next row
// ReadAsync() returns false when there are no more rows
while (await reader.ReadAsync())
{
    // Now we are positioned on a row — safe to read columns
    int id       = reader.GetInt32(0);
    string name  = reader.GetString(1);
    decimal price = reader.GetDecimal(2);
}
// After the loop: reader is positioned AFTER the last row
```

### Accessing columns — by ordinal vs by name

```csharp
// By ordinal (index) — FAST, no dictionary lookup
int id = reader.GetInt32(0);     // column 0
string name = reader.GetString(1); // column 1

// By column name — SLOWER (dictionary lookup each time), but more readable
int id   = reader.GetInt32(reader.GetOrdinal("Id"));
string name = reader.GetString(reader.GetOrdinal("Name"));

// Best of both worlds — resolve ordinal ONCE, use many times
int idOrdinal   = reader.GetOrdinal("Id");
int nameOrdinal = reader.GetOrdinal("Name");

while (await reader.ReadAsync())
{
    int id     = reader.GetInt32(idOrdinal);
    string name = reader.GetString(nameOrdinal);
}
```

### The NULL trap — the most common runtime crash

This is a critical pitfall. If a column can contain SQL `NULL`, calling `GetString()` / `GetInt32()` / etc. directly will throw an `InvalidCastException`:

```csharp
// Suppose the Description column is NULL for some rows in the database

// ❌ BAD — InvalidCastException for any row where Description is NULL
string description = reader.GetString(2); // CRASH!

// ✅ GOOD — check for NULL before reading
string? description = reader.IsDBNull(2) ? null : reader.GetString(2);

// ✅ ALSO GOOD — use the indexer which returns object (then handle DBNull)
object raw = reader["Description"];
string? description = raw == DBNull.Value ? null : (string)raw;

// ✅ MODERN C# — nullable-aware helper
string? description = await reader.IsDBNullAsync(2) ? null : reader.GetString(2);
```

**Why does this happen?**

SQL `NULL` is represented in ADO.NET as the singleton `DBNull.Value` — a special object, not C# `null`. The typed getter methods (`GetString`, `GetInt32`, etc.) do not understand `DBNull.Value` and throw instead of returning `null`.

### Full SqlDataReader example — reading products

```csharp
public async Task<List<Product>> GetAllProductsAsync()
{
    await using var connection = new SqlConnection(_connectionString);
    await connection.OpenAsync();

    await using var command = new SqlCommand(
        "SELECT Id, Name, Price, Description FROM Products ORDER BY Name",
        connection);

    await using var reader = await command.ExecuteReaderAsync();

    // Pre-resolve ordinals once — avoids repeated dictionary lookups in the loop
    int idOrdinal          = reader.GetOrdinal("Id");
    int nameOrdinal        = reader.GetOrdinal("Name");
    int priceOrdinal       = reader.GetOrdinal("Price");
    int descriptionOrdinal = reader.GetOrdinal("Description");

    var products = new List<Product>();

    while (await reader.ReadAsync())
    {
        products.Add(new Product(
            Id:          reader.GetInt32(idOrdinal),
            Name:        reader.GetString(nameOrdinal),
            Price:       reader.GetDecimal(priceOrdinal),
            Description: reader.IsDBNull(descriptionOrdinal)
                             ? null
                             : reader.GetString(descriptionOrdinal)
        ));
    }

    return products;
}
```

---

## 6. Parameterized Queries — The Only Safe Way to Build SQL

Never concatenate user input directly into a SQL string. This is the gateway to SQL injection — one of the most severe security vulnerabilities in software.

### What SQL injection looks like

Suppose you have this login query:

```csharp
// ❌ CATASTROPHICALLY BAD — SQL injection vulnerability
string sql = $"SELECT * FROM Users WHERE Username = '{username}' AND Password = '{password}'";
```

An attacker enters this as the username: `' OR 1=1 --`

The resulting SQL becomes:
```sql
SELECT * FROM Users WHERE Username = '' OR 1=1 --' AND Password = '...'
```

- `OR 1=1` is always true — every row matches
- `--` comments out the rest, including the password check
- Result: the attacker is logged in as every user simultaneously

With full SQL permissions they can also:
```
'; DROP TABLE Users; --
'; INSERT INTO Admins VALUES ('hacker', 'password'); --
```

### Parameterized queries — how they work

```csharp
// ✅ SAFE — parameterized query
await using var command = new SqlCommand(
    "SELECT * FROM Users WHERE Username = @Username AND Password = @Password",
    connection);

command.Parameters.AddWithValue("@Username", username);
command.Parameters.AddWithValue("@Password", password);
```

**What happens under the hood:**

The SQL query with `@Username` and `@Password` placeholders is sent to SQL Server **as a template**. The values are transmitted **separately**, in a different protocol packet. SQL Server combines them at the parser level — the values are *never* interpolated into the SQL text. No matter what the user types, it is always treated as a literal value, not SQL syntax.

```
Template:  SELECT * FROM Users WHERE Username = @Username AND Password = @Password
Values:    @Username = "' OR 1=1 --"   (just a string, no SQL meaning)
           @Password = "anything"
```

### AddWithValue vs Add — which one to use?

```csharp
// AddWithValue — quick but infers the SQL type from the C# value
command.Parameters.AddWithValue("@Name", "Laptop");
// ADO.NET guesses: this is nvarchar. But guessing can be wrong for edge cases.

// Add — explicit, type-safe, recommended for production code
command.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = "Laptop";
command.Parameters.Add("@Price", SqlDbType.Decimal).Value = 3499.99m;
command.Parameters.Add("@Stock", SqlDbType.Int).Value = 50;
```

**Why does explicit typing matter?**

When SQL Server receives a parameter without explicit type info, it must infer the type. For strings, `AddWithValue` typically sends `nvarchar(4000)` — which may differ from the actual column type (`varchar(200)`), causing implicit conversions that can prevent SQL Server from using indexes efficiently. For high-traffic queries, always use explicit types.

### What CAN and CANNOT be parameterized

Parameters replace **values** only — they cannot replace structural SQL elements:

```csharp
// ✅ Can be parameterized — data values
command.CommandText = "SELECT * FROM Products WHERE Price > @MinPrice";
command.Parameters.AddWithValue("@MinPrice", 100m);

command.CommandText = "INSERT INTO Orders (CustomerId, Total) VALUES (@CustomerId, @Total)";
command.Parameters.AddWithValue("@CustomerId", customerId);
command.Parameters.AddWithValue("@Total", total);

command.CommandText = "WHERE Name LIKE @SearchTerm";
command.Parameters.AddWithValue("@SearchTerm", $"%{searchQuery}%"); // % must be in the value

// ❌ CANNOT be parameterized — structural SQL elements
// Table names:
command.CommandText = "SELECT * FROM @TableName"; // will throw at runtime

// Column names in ORDER BY:
command.CommandText = "SELECT * FROM Products ORDER BY @SortColumn"; // doesn't work

// For these, you must whitelist:
string sortColumn = allowedColumns.Contains(userInput) ? userInput : "Id";
command.CommandText = $"SELECT * FROM Products ORDER BY {sortColumn}";
```

---

## 7. Passing NULL to SQL Parameters — The DBNull.Value Trap

C# `null` and SQL `NULL` are different concepts. ADO.NET uses `DBNull.Value` as the SQL-side representation of NULL.

### What goes wrong with C# null

```csharp
string? description = null; // user left the description blank

// ❌ BAD — if you pass C# null, ADO.NET ignores the parameter entirely
// SQL Server never sees a value for @Description — query fails
command.Parameters.AddWithValue("@Description", null); // throws ArgumentNullException!

// ❌ ALSO BAD — sends the string "NULL", not a SQL NULL value
command.Parameters.AddWithValue("@Description", "NULL");

// ❌ ALSO BAD — sends an empty string "" — not the same as NULL in the database
command.Parameters.AddWithValue("@Description", string.Empty);
```

### The correct pattern

```csharp
// ✅ CORRECT — coerce C# null to DBNull.Value before passing
command.Parameters.AddWithValue("@Description", (object?)description ?? DBNull.Value);

// Reading it out loud: "if description is null, use DBNull.Value; otherwise use description"
```

### A reusable helper to make this cleaner

```csharp
// Without a helper you write this everywhere:
command.Parameters.AddWithValue("@Description", (object?)description ?? DBNull.Value);
command.Parameters.AddWithValue("@Nickname",    (object?)nickname    ?? DBNull.Value);
command.Parameters.AddWithValue("@PhoneNumber", (object?)phone       ?? DBNull.Value);

// With a helper extension method:
public static class SqlParameterExtensions
{
    public static SqlParameter AddNullable(
        this SqlParameterCollection parameters,
        string paramName,
        object? value)
    {
        return parameters.AddWithValue(paramName, value ?? DBNull.Value);
    }
}

// Usage becomes clean:
command.Parameters.AddNullable("@Description", description);
command.Parameters.AddNullable("@Nickname",    nickname);
command.Parameters.AddNullable("@PhoneNumber", phone);
```

### Nullable value types (int?, decimal?, DateTime?)

```csharp
int? categoryId = null; // user didn't select a category

// ❌ BAD — (object?)null is just null — still throws
command.Parameters.AddWithValue("@CategoryId", (object?)categoryId);

// ✅ CORRECT — must explicitly handle nullable value types too
command.Parameters.AddWithValue("@CategoryId",
    categoryId.HasValue ? (object)categoryId.Value : DBNull.Value);

// Or with the null-coalescing trick:
command.Parameters.AddWithValue("@CategoryId", (object?)categoryId ?? DBNull.Value);
```

---

## 8. ACID Transactions in ADO.NET

A **transaction** groups multiple SQL operations into a single atomic unit: either all succeed together or all fail together. This is essential when partial success would leave the database in an inconsistent state.

### The ACID properties — what they mean in practice

| Property | What it guarantees | Real-world example |
|---|---|---|
| **Atomicity** | All-or-nothing. Either every operation in the transaction commits or none do. | Transfer money: debit account A AND credit account B — both happen or neither does |
| **Consistency** | The database moves from one valid state to another. No constraint violations survive a commit. | A foreign key constraint violation during INSERT rolls the whole transaction back |
| **Isolation** | Concurrent transactions don't see each other's uncommitted changes. | Two users booking the last seat on a flight don't both get a confirmation |
| **Durability** | Once committed, changes survive power outages, crashes, and restarts. | Your online order remains in the database even if the server reboots 1 second after you clicked "Place Order" |

### BeginTransaction — the fundamental pattern

```csharp
await using var connection = new SqlConnection(_connectionString);
await connection.OpenAsync();

// Transaction is created ON the connection — they are linked
using var transaction = connection.BeginTransaction();

try
{
    // IMPORTANT: every SqlCommand inside the transaction MUST be assigned to it
    await using var debitCommand = new SqlCommand(
        "UPDATE Accounts SET Balance = Balance - @Amount WHERE Id = @AccountId",
        connection,
        transaction); // ← the third argument links the command to the transaction

    debitCommand.Parameters.AddWithValue("@Amount", 500m);
    debitCommand.Parameters.AddWithValue("@AccountId", fromAccountId);
    await debitCommand.ExecuteNonQueryAsync();

    await using var creditCommand = new SqlCommand(
        "UPDATE Accounts SET Balance = Balance + @Amount WHERE Id = @AccountId",
        connection,
        transaction); // ← same transaction!

    creditCommand.Parameters.AddWithValue("@Amount", 500m);
    creditCommand.Parameters.AddWithValue("@AccountId", toAccountId);
    await creditCommand.ExecuteNonQueryAsync();

    // If both succeeded — commit: make changes permanent
    await transaction.CommitAsync();
}
catch
{
    // If anything threw — rollback: undo everything
    await transaction.RollbackAsync();
    throw; // re-throw so the caller knows something went wrong
}
```

### Transaction isolation levels — controlling what you can see

When multiple transactions run at the same time, isolation levels determine what changes each can see:

| Level | Dirty Read | Non-Repeatable Read | Phantom Read | Performance |
|---|---|---|---|---|
| **Read Uncommitted** | ✅ allowed | ✅ allowed | ✅ allowed | Fastest |
| **Read Committed** ← SQL Server default | ❌ prevented | ✅ allowed | ✅ allowed | Good |
| **Repeatable Read** | ❌ prevented | ❌ prevented | ✅ allowed | Slower |
| **Serializable** | ❌ prevented | ❌ prevented | ❌ prevented | Slowest |

**SQL Server default is Read Committed** — your transaction can only read rows that other transactions have already committed.

```csharp
// Specifying isolation level explicitly
using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
// IsolationLevel.ReadCommitted is already the default, but it's good to be explicit
```

### The three classic transaction pitfalls

**Pitfall 1: Forgetting to assign the transaction to the command**

```csharp
// ❌ BAD — the command has no Transaction set
// It runs OUTSIDE the transaction — not rolled back if something else fails
await using var command = new SqlCommand("UPDATE ...", connection);
// Missing: , transaction

// ✅ CORRECT
await using var command = new SqlCommand("UPDATE ...", connection, transaction);
```

**Pitfall 2: Keeping a transaction open too long**

```csharp
// ❌ BAD — HTTP call INSIDE the transaction
using var transaction = connection.BeginTransaction();
// ...
var response = await httpClient.GetAsync("https://external-api.com/validate");
// ↑ This could take 2 seconds. During that time:
// - the transaction holds locks on rows
// - other users trying to read or write those rows are BLOCKED
// - this is a deadlock waiting to happen

// ✅ GOOD — do all external work BEFORE opening the transaction
var response = await httpClient.GetAsync("https://external-api.com/validate");
// Only open the transaction when you're ready to do pure database work
using var transaction = connection.BeginTransaction();
```

**Pitfall 3: Using `await using` for SqlTransaction**

```csharp
// ❌ DANGEROUS — await using disposes the transaction at the end of the block
// If Commit() hasn't been called yet, Dispose() triggers a Rollback automatically
// This is usually what you want, but watch the scope carefully

await using var transaction = connection.BeginTransaction();
// ... if an exception occurs anywhere, the implicit Dispose() causes a Rollback
// This is actually correct behavior — but understand why it works!

// ✅ The pattern above (try/catch with explicit Rollback) is clearer and preferred
// because it makes the rollback intent explicit
```

---

## 9. Layered Architecture with ADO.NET — The Repository Pattern

In a well-structured REST API, database code does not belong in controllers. The **Repository pattern** provides a clean abstraction between business logic and data access, and it is a direct application of the Dependency Inversion Principle you know from SOLID.

### Why controllers should not talk directly to the database

```csharp
// ❌ WRONG — SQL in the controller violates SRP and DIP
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly string _connectionString;

    public ProductsController(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Database code in a controller:
        // - not testable (real DB needed for every test)
        // - not reusable (other controllers must copy-paste)
        // - violates SRP (controller handles HTTP AND database)
        // - violates DIP (depends on concrete ADO.NET, not abstraction)
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        // ... 30 lines of ADO.NET code ...
    }
}
```

### The clean layered structure

```
Controllers (Presentation layer)
    → depends on →
IProductRepository (abstraction — defined in the business layer)
    ← implements ←
SqlProductRepository (Data Access layer — ADO.NET lives here)
```

### Step 1: Define the interface (the contract)

```csharp
// This interface belongs to the business/domain layer
// It says WHAT can be done — not HOW
public interface IProductRepository
{
    Task<List<Product>>  GetAllAsync();
    Task<Product?>       GetByIdAsync(int id);
    Task<int>            CreateAsync(Product product);     // returns new ID
    Task                 UpdateAsync(Product product);
    Task                 DeleteAsync(int id);
}
```

### Step 2: Implement with ADO.NET (the concrete class)

```csharp
public class SqlProductRepository : IProductRepository
{
    private readonly string _connectionString;

    // The connection string is injected via IConfiguration — not hardcoded
    public SqlProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");
    }

    public async Task<List<Product>> GetAllAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(
            "SELECT Id, Name, Price, Description FROM Products ORDER BY Name",
            connection);

        await using var reader = await command.ExecuteReaderAsync();

        int idOrd   = reader.GetOrdinal("Id");
        int nameOrd = reader.GetOrdinal("Name");
        int priceOrd = reader.GetOrdinal("Price");
        int descOrd  = reader.GetOrdinal("Description");

        var products = new List<Product>();
        while (await reader.ReadAsync())
        {
            products.Add(new Product(
                Id:          reader.GetInt32(idOrd),
                Name:        reader.GetString(nameOrd),
                Price:       reader.GetDecimal(priceOrd),
                Description: reader.IsDBNull(descOrd) ? null : reader.GetString(descOrd)
            ));
        }
        return products;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(
            "SELECT Id, Name, Price, Description FROM Products WHERE Id = @Id",
            connection);
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null; // no row found

        return new Product(
            Id:          reader.GetInt32(reader.GetOrdinal("Id")),
            Name:        reader.GetString(reader.GetOrdinal("Name")),
            Price:       reader.GetDecimal(reader.GetOrdinal("Price")),
            Description: reader.IsDBNull(reader.GetOrdinal("Description"))
                             ? null
                             : reader.GetString(reader.GetOrdinal("Description"))
        );
    }

    public async Task<int> CreateAsync(Product product)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // SCOPE_IDENTITY() returns the ID auto-generated by SQL Server for the new row
        await using var command = new SqlCommand(
            """
            INSERT INTO Products (Name, Price, Description)
            VALUES (@Name, @Price, @Description);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """,
            connection);

        command.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = product.Name;
        command.Parameters.Add("@Price", SqlDbType.Decimal).Value = product.Price;
        command.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value =
            (object?)product.Description ?? DBNull.Value;

        // ExecuteScalarAsync returns the result of SELECT SCOPE_IDENTITY() — the new ID
        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task UpdateAsync(Product product)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(
            """
            UPDATE Products
            SET Name = @Name, Price = @Price, Description = @Description
            WHERE Id = @Id
            """,
            connection);

        command.Parameters.Add("@Id",    SqlDbType.Int).Value          = product.Id;
        command.Parameters.Add("@Name",  SqlDbType.NVarChar, 200).Value = product.Name;
        command.Parameters.Add("@Price", SqlDbType.Decimal).Value      = product.Price;
        command.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value =
            (object?)product.Description ?? DBNull.Value;

        int rowsAffected = await command.ExecuteNonQueryAsync();

        if (rowsAffected == 0)
            throw new KeyNotFoundException($"Product with ID {product.Id} was not found.");
    }

    public async Task DeleteAsync(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(
            "DELETE FROM Products WHERE Id = @Id", connection);
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        int rowsAffected = await command.ExecuteNonQueryAsync();

        if (rowsAffected == 0)
            throw new KeyNotFoundException($"Product with ID {id} was not found.");
    }
}
```

### Step 3: The controller depends only on the interface

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    // Depends on the ABSTRACTION — has no idea if it's SQL, MongoDB, or an in-memory fake
    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll()
        => Ok(await _repository.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(Product product)
    {
        int newId = await _repository.CreateAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        if (id != product.Id) return BadRequest("ID mismatch");
        await _repository.UpdateAsync(product);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
```

### Step 4: Wire everything together in Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

// Register the repository — when someone asks for IProductRepository,
// create a SqlProductRepository and inject IConfiguration into it
builder.Services.AddScoped<IProductRepository, SqlProductRepository>();
builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
```

### Why Scoped and not Singleton?

`AddScoped` means one instance per HTTP request. Since `SqlProductRepository` opens connections on each call (from the pool), it has no state between requests — but using `Scoped` prevents it from being accidentally shared between concurrent requests (which would be unsafe if you ever introduce per-instance state).

### Testing becomes trivial — the key benefit of the Repository pattern

```csharp
// In-memory fake — no database required for tests
public class FakeProductRepository : IProductRepository
{
    private readonly List<Product> _products = new()
    {
        new Product(1, "Test Laptop",  3499.99m, "A great laptop"),
        new Product(2, "Test Mouse",   129.99m,  null)
    };

    public Task<List<Product>>  GetAllAsync()         => Task.FromResult(_products.ToList());
    public Task<Product?>       GetByIdAsync(int id)  => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
    public Task<int>            CreateAsync(Product p) { _products.Add(p); return Task.FromResult(p.Id); }
    public Task                 UpdateAsync(Product p) { /* update in list */ return Task.CompletedTask; }
    public Task                 DeleteAsync(int id)    { /* remove from list */ return Task.CompletedTask; }
}

// Unit test — fast, isolated, no database
[Fact]
public async Task GetAll_ReturnsAllProducts()
{
    var repository = new FakeProductRepository();
    var controller = new ProductsController(repository);

    var result = await controller.GetAll();

    var ok = Assert.IsType<OkObjectResult>(result.Result);
    var products = Assert.IsType<List<Product>>(ok.Value);
    Assert.Equal(2, products.Count);
}
```

---

## 10. Connection String Storage — Where It Lives and Why

The connection string is one of the most sensitive pieces of configuration in your application. It contains credentials to your database.

### The only acceptable location: appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ShopDb;Integrated Security=true;TrustServerCertificate=true"
  }
}
```

### Reading it in code via IConfiguration

```csharp
public class SqlProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public SqlProductRepository(IConfiguration configuration)
    {
        // GetConnectionString("X") is shorthand for
        // configuration["ConnectionStrings:X"]
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found in configuration.");
    }
}
```

### What NOT to do — and why

```csharp
// ❌ NEVER — hardcoded in the class
public class SqlProductRepository
{
    private readonly string _connectionString =
        "Server=localhost;Database=ShopDb;Password=SuperSecret123;";
    // Problems:
    // 1. Password is now visible in source code and in git history — forever
    // 2. Changing the connection requires a code change, recompile, and redeploy
    // 3. Different environments (dev / staging / prod) need different strings — impossible
}

// ❌ NEVER — hardcoded as a constant
public static class AppConstants
{
    public const string ConnectionString =
        "Server=prod-db.mycompany.com;Password=...";
}
```

### Production-grade: environment variables and Azure Key Vault

In production, you should never have real credentials in `appsettings.json` either — that file is often committed to version control. The proper approach:

```csharp
// .NET configuration reads from multiple sources in order — later sources override earlier
// 1. appsettings.json         (base config — committed to git, no secrets)
// 2. appsettings.Production.json (environment-specific — committed to git, no secrets)
// 3. Environment variables    (set on the server or in Docker — not in git)
// 4. Azure Key Vault / AWS Secrets Manager (for true production secrets)

// In the shell / Docker:
// ConnectionStrings__DefaultConnection=Server=prod-db;Password=real-password;
// (double underscore __ is the section separator in env vars)
```

For the purpose of learning and development, `appsettings.json` is correct. For production: secrets go in environment variables or a secrets manager.

---

## 11. Full Working Example — Repository, Transaction, Proper Error Handling

Here is a realistic scenario: transferring money between two accounts. It demonstrates transactions, error handling, proper NULL handling, and the repository pattern working together.

```csharp
// Domain model
public record Account(int Id, string OwnerName, decimal Balance);

// Interface
public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(int id);
    Task TransferAsync(int fromAccountId, int toAccountId, decimal amount);
}

// Implementation
public class SqlAccountRepository : IAccountRepository
{
    private readonly string _connectionString;

    public SqlAccountRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string missing.");
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(
            "SELECT Id, OwnerName, Balance FROM Accounts WHERE Id = @Id",
            connection);
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync()) return null;

        return new Account(
            Id:        reader.GetInt32(reader.GetOrdinal("Id")),
            OwnerName: reader.GetString(reader.GetOrdinal("OwnerName")),
            Balance:   reader.GetDecimal(reader.GetOrdinal("Balance"))
        );
    }

    public async Task TransferAsync(int fromAccountId, int toAccountId, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Transfer amount must be positive.", nameof(amount));

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // All database work inside a single transaction — atomic money movement
        using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

        try
        {
            // Step 1: verify sender has sufficient balance
            await using var checkCommand = new SqlCommand(
                "SELECT Balance FROM Accounts WHERE Id = @Id",
                connection, transaction);
            checkCommand.Parameters.Add("@Id", SqlDbType.Int).Value = fromAccountId;

            object? balanceResult = await checkCommand.ExecuteScalarAsync();
            if (balanceResult is null || balanceResult == DBNull.Value)
                throw new KeyNotFoundException($"Source account {fromAccountId} not found.");

            decimal currentBalance = Convert.ToDecimal(balanceResult);
            if (currentBalance < amount)
                throw new InvalidOperationException(
                    $"Insufficient funds. Balance: {currentBalance:C}, Requested: {amount:C}");

            // Step 2: debit the source account
            await using var debitCommand = new SqlCommand(
                "UPDATE Accounts SET Balance = Balance - @Amount WHERE Id = @AccountId",
                connection, transaction);
            debitCommand.Parameters.Add("@Amount",    SqlDbType.Decimal).Value = amount;
            debitCommand.Parameters.Add("@AccountId", SqlDbType.Int).Value     = fromAccountId;
            await debitCommand.ExecuteNonQueryAsync();

            // Step 3: credit the destination account
            await using var creditCommand = new SqlCommand(
                "UPDATE Accounts SET Balance = Balance + @Amount WHERE Id = @AccountId",
                connection, transaction);
            creditCommand.Parameters.Add("@Amount",    SqlDbType.Decimal).Value = amount;
            creditCommand.Parameters.Add("@AccountId", SqlDbType.Int).Value     = toAccountId;

            int rowsAffected = await creditCommand.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
                throw new KeyNotFoundException($"Destination account {toAccountId} not found.");

            // All three steps succeeded — commit!
            await transaction.CommitAsync();
        }
        catch
        {
            // Anything failed — undo everything
            await transaction.RollbackAsync();
            throw; // re-throw: caller gets the original exception
        }
    }
}
```

---

## 12. Common Pitfalls — A Diagnostic Reference

```
PITFALL                                  ROOT CAUSE         SOLUTION
─────────────────────────────────────────────────────────────────────────────
InvalidCastException on GetString()      NULL in column     Check IsDBNull() before calling GetString()
                                                            or use reader["Col"] as string

"Timeout: no free connection from pool"  Connection leak    Always use 'await using' for SqlConnection

Thread pool exhaustion under load        Using Open()       Use OpenAsync() — releases thread during wait
                                         instead of         
                                         OpenAsync()        

SQL Injection                            String concat      ALWAYS use @parameters, never concatenate
                                         in query

Parameter ignored by SQL Server          Passing C# null    Use (object?)value ?? DBNull.Value

Command outside transaction              Missing 3rd arg    new SqlCommand(sql, conn, transaction)

Deadlock on long-running transaction     HTTP call inside   Do all external I/O BEFORE BeginTransaction()
                                         transaction        

Hardcoded connection string              No separation      Store in appsettings.json ConnectionStrings section
                                         of config          Inject via IConfiguration

Blocking threads in ASP.NET Core         Using sync methods Use ExecuteNonQueryAsync(), ExecuteScalarAsync(),
                                                            ReadAsync() consistently

IndexOutOfRangeException on GetInt32(N)  Wrong ordinal      Use reader.GetOrdinal("ColumnName") instead of
                                                            hardcoded numbers — safer when SELECT * changes
```

---

## 13. Summary and Synthesis

ADO.NET is the foundation of all .NET database access — even if you eventually use Entity Framework Core, understanding what happens underneath makes you a more effective developer.

### The key classes and what they do

| Class | Role |
|---|---|
| `SqlConnection` | Represents a logical connection; manages pooled TCP connections |
| `SqlCommand` | Holds a SQL query; executes it via one of four async methods |
| `SqlDataReader` | Forward-only row cursor; streams results without loading all into RAM |
| `SqlTransaction` | Groups commands into an atomic ACID unit |
| `SqlParameter` | Type-safe value binding; the defense against SQL injection |

### The five principles of good ADO.NET code

**1. Always async** — use `OpenAsync()`, `ExecuteNonQueryAsync()`, `ReadAsync()` in ASP.NET Core. Synchronous equivalents block threads and destroy scalability.

**2. Always `await using`** — `SqlConnection`, `SqlCommand`, and `SqlDataReader` all implement `IAsyncDisposable`. Without it, connections leak out of the pool and the application grinds to a halt.

**3. Always parameterize** — `@Parameter` syntax, not string concatenation. No exceptions. One SQL injection attack can destroy an entire database.

**4. Always handle DBNull** — `IsDBNull()` before every typed getter on a nullable column. An `InvalidCastException` in production from a NULL value is embarrassing and avoidable.

**5. Put ADO.NET in repositories** — the controller talks to `IProductRepository`, not to `SqlConnection`. This gives you testability, loose coupling, and clean separation of concerns — exactly what SOLID taught you.

### How it connects to what you learned before

| Concept from previous modules | How it shows up in ADO.NET |
|---|---|
| SRP | Repository handles database — controller handles HTTP |
| DIP | Controller depends on `IProductRepository`, not on `SqlProductRepository` |
| DI / Scoped lifetime | `AddScoped<IProductRepository, SqlProductRepository>()` in Program.cs |
| Loose coupling | Swapping `SqlProductRepository` for `FakeProductRepository` in tests requires zero controller changes |
| ACID / transactions | Atomicity, Consistency, Isolation, Durability — `BeginTransaction()` / `CommitAsync()` / `RollbackAsync()` |

---

---

# 📝 Quiz — Practice Questions with Answer Key

*Test your understanding. Each question mirrors the depth of what was covered above.*

---

**Q1.** Which method of the `SqlCommand` class should be used to execute an INSERT query when we want to know the number of inserted rows?

- A) `ExecuteReaderAsync()`
- B) `ExecuteScalarAsync()`
- C) `ExecuteNonQueryAsync()`
- D) `ExecuteXmlReaderAsync()`

---

**Q2.** Which of the following statements about connection pooling in ADO.NET are **true**? *(Select all that apply)*

- A) The pool is created automatically — it does not require explicit configuration
- B) Calling `connection.Close()` or `Dispose()` actually closes the TCP connection
- C) The default maximum number of connections in the pool is 100
- D) Each unique connection string has its own separate connection pool
- E) Connection pooling requires manual activation via the `Pooling=true` parameter

---

**Q3.** What does the `ExecuteScalarAsync()` method of the `SqlCommand` class return?

- A) An object of type `SqlDataReader`
- B) A value of type `int` representing the number of modified rows
- C) A value of type `object?` — the first cell of the first row of the result
- D) A `List` collection containing all query results

---

**Q4.** An attacker enters the following value into a login field: `' OR 1=1 --`. What type of attack does this represent?

- A) Cross-Site Scripting (XSS)
- B) SQL Injection
- C) Cross-Site Request Forgery (CSRF)
- D) Buffer Overflow

---

**Q5.** Which of the following SQL query elements can be safely replaced with parameters (`@ParameterName`) in ADO.NET? *(Select all that apply)*

- A) A value in a WHERE clause (e.g., `WHERE Id = @Id`)
- B) A table name (e.g., `SELECT * FROM @TableName`)
- C) A value inserted in an INSERT statement (e.g., `VALUES (@Name, @Price)`)
- D) A column name in ORDER BY (e.g., `ORDER BY @SortColumn`)
- E) A value in a LIKE clause (e.g., `WHERE Name LIKE @SearchTerm`)

---

**Q6.** Why should `OpenAsync()` be used instead of `Open()` when opening a `SqlConnection` in ASP.NET Core?

- A) `OpenAsync()` is safer against SQL Injection
- B) `Open()` is not supported in ASP.NET Core
- C) `OpenAsync()` does not block the thread while waiting for the connection, which prevents thread pool exhaustion
- D) `OpenAsync()` automatically encrypts the database connection

---

**Q7.** What happens if we call `reader.GetString()` on a column that contains a NULL value in the database?

- A) The method will return an empty string `""`
- B) The method will return `null`
- C) An `InvalidCastException` will be thrown
- D) The method will return the value `DBNull.Value`

---

**Q8.** Which of the following statements about `SqlDataReader` are **true**? *(Select all that apply)*

- A) It is forward-only — you can only move forward, row by row
- B) It requires an open connection to the database for the entire duration of reading
- C) It loads all results into memory at once when `ExecuteReader()` is called
- D) It allows modification of the retrieved data directly in the database
- E) Before the first call to `Read()`, it does not point to any row

---

**Q9.** What is the purpose of using the `using` statement when working with `SqlConnection`?

- A) To automatically open the database connection
- B) To guarantee that the connection is closed even in the event of an exception
- C) To enable connection pooling
- D) To enable the use of parameterized queries

---

**Q10.** What does the acronym ACID stand for in the context of database transactions?

- A) Authentication, Consistency, Integrity, Durability
- B) Atomicity, Consistency, Isolation, Durability
- C) Atomicity, Concurrency, Isolation, Distribution
- D) Access, Control, Identity, Data

---

**Q11.** Which of the following elements are **required** to correctly create and execute a `SqlCommand`? *(Select all that apply)*

- A) The SQL query text (CommandText)
- B) An open `SqlConnection` object
- C) A `SqlTransaction` object
- D) A `SqlDataReader` object
- E) The `Microsoft.Data.SqlClient` NuGet package

---

**Q12.** What is the default transaction isolation level in SQL Server?

- A) Read Uncommitted
- B) Read Committed
- C) Repeatable Read
- D) Serializable

---

**Q13.** Which of the following mistakes are typical pitfalls when working with transactions in ADO.NET? *(Select all that apply)*

- A) Forgetting to assign the transaction to the `SqlCommand` — the operation executes outside the transaction
- B) Keeping a transaction open for too long, which blocks data for other users
- C) Using `await using` for `SqlTransaction`
- D) Performing HTTP calls or long-running operations inside a transaction
- E) Using `BeginTransaction()` on an open connection

---

**Q14.** How should a `null` value from C# be passed as a SQL query parameter in ADO.NET?

- A) `command.Parameters.AddWithValue("@Param", null);`
- B) `command.Parameters.AddWithValue("@Param", (object?)value ?? DBNull.Value);`
- C) `command.Parameters.AddWithValue("@Param", "NULL");`
- D) `command.Parameters.AddWithValue("@Param", string.Empty);`

---

**Q15.** Where in the layered architecture of a REST API should the connection string be stored?

- A) Directly in the repository class code as a constant
- B) In the `appsettings.json` file under the `ConnectionStrings` section
- C) In the controller constructor as a parameter
- D) In a comment in `Program.cs` for easy access

---

## ✅ Answer Key

| Q | Answer | Explanation |
|---|---|---|
| 1 | **C** | `ExecuteNonQueryAsync()` returns `int` — the number of rows affected. That's the only method that tells you "how many rows did this change?" |
| 2 | **A, C, D** | B is false — `Close()`/`Dispose()` returns the connection to the pool, not closes the TCP socket. E is false — pooling is enabled by default; `Pooling=false` disables it, not `Pooling=true` enables it |
| 3 | **C** | `ExecuteScalarAsync()` returns `object?` — the value in the first column of the first row. Must be cast manually (e.g., `Convert.ToInt32(result)`) |
| 4 | **B** | `' OR 1=1 --` is the textbook SQL Injection pattern. The `'` closes the string literal, `OR 1=1` bypasses the condition, and `--` comments out the rest |
| 5 | **A, C, E** | Table names (B) and column names in ORDER BY (D) cannot be parameterized — they are structural SQL elements. Only values can be parameters. For table/column names, use a whitelist |
| 6 | **C** | `OpenAsync()` yields the thread while waiting for a pooled connection, preventing thread pool starvation. `Open()` blocks the thread — catastrophic under high load |
| 7 | **C** | `InvalidCastException` — the typed getter methods cannot handle `DBNull.Value`. Always call `reader.IsDBNull(ordinal)` first on nullable columns |
| 8 | **A, B, E** | C is false — it streams rows, never loads all at once. D is false — it is read-only. A, B, E correctly describe the forward-only, connected, pre-row-positioned cursor |
| 9 | **B** | The `using` statement calls `Dispose()` at the end of the block regardless of exceptions — which returns the connection to the pool. It does NOT open automatically (you still call `OpenAsync()`) |
| 10 | **B** | Atomicity, Consistency, Isolation, Durability — the four guarantees of a database transaction |
| 11 | **A, B, E** | A `SqlCommand` only needs: the SQL text, an open connection, and the NuGet package. `SqlTransaction` (C) is optional (only if you're in a transaction). `SqlDataReader` (D) is a result, not an input |
| 12 | **B** | Read Committed — SQL Server's default. A transaction can only read rows that other transactions have already committed (dirty reads are prevented) |
| 13 | **A, B, C, D** | E is wrong — `BeginTransaction()` on an open connection IS the correct way to start a transaction. A, B, C, D are all real pitfalls. Note: C (`await using` for `SqlTransaction`) auto-rolls back on dispose — understand the scope |
| 14 | **B** | `(object?)value ?? DBNull.Value` is the correct pattern. `null` throws, `"NULL"` is a string, `string.Empty` is an empty string — none of them represent SQL NULL |
| 15 | **B** | `appsettings.json` under `ConnectionStrings`. Never hardcode (A), never in the controller (C), never in a comment (D). Read via `IConfiguration.GetConnectionString("Name")` |
