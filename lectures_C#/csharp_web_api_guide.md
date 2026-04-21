# Building Modern Web APIs in .NET: HTTP, REST, and ASP.NET Core

> This guide charts the path from networking communication fundamentals to building a complete REST API in ASP.NET Core. We start with the types of applications in .NET, then work through HTTP and REST, and finally assemble everything into a working Web API application with validation, DTOs, configuration, middleware, and documentation.

---

## Table of Contents

1. [Types of Applications in .NET](#1-types-of-applications-in-net)
2. [HTTP: The Foundation of Web Application Communication](#2-http-the-foundation-of-web-application-communication)
3. [REST API: Resources, Endpoints, and Consistent Design Rules](#3-rest-api-resources-endpoints-and-consistent-design-rules)
4. [ASP.NET Core: Application Architecture and the Middleware Pipeline](#4-aspnet-core-application-architecture-and-the-middleware-pipeline)
5. [Your First ASP.NET Core Web API Project](#5-your-first-aspnet-core-web-api-project)
6. [Controllers, Routing, and HTTP Method Handling](#6-controllers-routing-and-http-method-handling)
7. [Minimal APIs as an Alternative to Controllers](#7-minimal-apis-as-an-alternative-to-controllers)
8. [Model Binding and Data Validation](#8-model-binding-and-data-validation)
9. [Returning Responses: IActionResult, ActionResult\<T\>, and Status Codes](#9-returning-responses)
10. [DTOs and Data Mapping](#10-dtos-and-data-mapping)
11. [Application Configuration: appsettings, Environment Variables, and IOptions](#11-application-configuration)
12. [Middleware in Practice: Logging, Exception Handling, and CORS](#12-middleware-in-practice)
13. [Swagger and OpenAPI: Documentation and API Testing](#13-swagger-and-openapi)
14. [Building a Complete REST CRUD API Step by Step](#14-building-a-complete-rest-crud-api-step-by-step)
15. [Most Common Mistakes and Pitfalls](#15-most-common-mistakes-and-pitfalls)
16. [Key Takeaways](#16-key-takeaways)

---

## 1. Types of Applications in .NET

.NET allows you to build several different types of applications. The platform itself is shared, but the execution model, user interface, and way the application communicates with its environment can be entirely different. This matters because Web API is not the "only correct" form of application — it is simply one variant that works very well when you want to expose functionality to other systems, frontends, or mobile applications.

| Application Type | Purpose | Typical Characteristics |
|---|---|---|
| Console application | CLI tools, prototypes, batch jobs, simple automation | No GUI, simple startup, convenient for learning fundamentals |
| Desktop application | Programs running locally on the user's computer | GUI, works with local resources, usually more tightly coupled to the platform |
| Web application | Systems accessible through a browser | Centralized deployment, easy updates, communication over HTTP |
| Web API | Exposing data and operations to other clients | No traditional UI, emphasis on endpoints, JSON, HTTP status codes |
| Mobile application | Apps for Android and iOS | Mobile interface, device integration, often consumes a backend API |
| Microservice / service | Narrowly specialized backend component | Independent deployment, limited responsibility, usually network communication |

In practice these application types very often collaborate with each other. For example, a mobile app and a web frontend consume the same Web API. In turn, the Web API may call other services, persist data in a database, and publish events.

> **Important.** A Web API is not the same thing as "any web application." A traditional web application generates HTML for the end user, whereas a Web API typically returns data — most often JSON — for other programs.

---

## 2. HTTP: The Foundation of Web Application Communication

### What Is HTTP

HTTP is a communication protocol used on the World Wide Web. In the simplest terms, a client sends a request and a server sends back a response. The client can be a browser, a mobile app, an SPA frontend, Postman, or another backend service. The server can be our ASP.NET Core application.

Every HTTP request contains several basic elements: a method, a resource address, headers, and optionally a message body. The response contains a status code, headers, and optionally a body. In an API that body is most often a representation of a resource — for example a user object, a list of orders, or error information.

### Request and Response

```
Request:
GET /api/books/42 HTTP/1.1
Host: localhost:5001
Accept: application/json

Response:
HTTP/1.1 200 OK
Content-Type: application/json

{
  "id": 42,
  "title": "Clean Code",
  "author": "Robert C. Martin"
}
```

Two things are apparent from this simple example. First, the client does not "call a C# method on the server" — it sends a standard HTTP request. Second, the server should respond not only with data but also with the correct status code and headers.

### The Most Important HTTP Methods

| Method | Typical Use | Notes |
|---|---|---|
| GET | Retrieving data | Should not change system state |
| POST | Creating a resource or performing an operation | Usually not idempotent |
| PUT | Full resource update | Should be idempotent |
| PATCH | Partial update | Applies only to selected fields |
| DELETE | Removing a resource | Usually idempotent from the perspective of the final effect |

> **Best practice.** The choice of HTTP method carries semantic meaning. It is not a cosmetic detail. Clients, testing tools, caches, proxies, and developers reading the API assume specific behavior based on the method used.

### Status Codes

Status codes inform the client how the request concluded. This is one of the most important communication mechanisms in an API. Even if you return valid JSON, using the wrong status code sends the client a misleading signal about the result of the operation.

| Range | Meaning | Examples |
|---|---|---|
| 2xx | Success | 200 OK, 201 Created, 204 No Content |
| 3xx | Redirections | 301 Moved Permanently, 302 Found |
| 4xx | Client-side error | 400 Bad Request, 401 Unauthorized, 403 Forbidden, 404 Not Found |
| 5xx | Server-side error | 500 Internal Server Error |

> **Note.** `401 Unauthorized` does not mean "lacks permissions" — it most often means the client has not provided valid authentication. If the user is logged in but does not have the right to perform the operation, `403 Forbidden` is usually more appropriate.

### Headers and Data Format

Headers carry metadata about the request and response. In practice you will frequently encounter `Content-Type`, `Accept`, `Authorization`, and cache-related headers. Modern APIs most commonly work with JSON, so you will very often see `application/json`.

```json
{
  "id": 1,
  "name": "Alice",
  "email": "alice@example.com"
}
```

### Statelessness and Idempotency

In the context of HTTP and REST, statelessness is very important. The server should not assume it "remembers the user's previous screen" or "knows what the client did a moment ago" unless that information is contained in the current request. Every request should carry everything needed for its own processing.

The second important concept is idempotency. If you send the same idempotent request multiple times, the final effect should be the same as after a single invocation. For this reason `GET`, `PUT`, and usually `DELETE` are treated differently from `POST`.

---

## 3. REST API: Resources, Endpoints, and Consistent Design Rules

### What Is REST

REST is an architectural style for building networked services. In practice this means a few simple principles: we operate on resources, use standard HTTP methods, transfer data representations, and maintain consistent endpoint semantics. REST does not mean "arbitrary JSON over HTTP." For an API to be truly readable it must have a sensible resource structure and predictable behavior.

### Resources and Endpoints

In REST we think in terms of resources, not UI actions. The URL should describe the resource, and the HTTP method should describe the operation.

| Goal | Endpoint | Method |
|---|---|---|
| Retrieve list of books | `/api/books` | GET |
| Retrieve a single book | `/api/books/42` | GET |
| Add a book | `/api/books` | POST |
| Full update | `/api/books/42` | PUT |
| Delete a book | `/api/books/42` | DELETE |

> **Best practice.** URLs should typically use nouns describing resources, not verbs describing actions. Prefer `/api/orders/15` over `/api/getOrderById/15`.

### JSON as a Resource Representation

REST does not mandate JSON, but in practice JSON is the most common data-exchange format. The client receives a representation of the resource, not a reference to an object in server memory. The API does not directly expose backend classes — it exposes a safe, controlled representation of them.

### Key Principles of a Good REST API

- Design endpoints around resources and the relationships between them.
- Choose HTTP methods according to their meaning.
- Return appropriate status codes instead of always responding with `200 OK`.
- Maintain consistency in naming, formats, and error handling.
- Do not mix business logic with ad-hoc response formatting.
- Treat the client as an independent system that needs a stable contract.

> **Important.** REST is a set of rules intended to make building predictable APIs easier. If one endpoint returns an object, another returns a list, a third always responds with `200` even on error, and a fourth expects parameters in an unusual place, the client ends up guessing instead of relying on a clear contract.

---

## 4. ASP.NET Core: Application Architecture and the Middleware Pipeline

### What Is ASP.NET Core

ASP.NET Core is a modern framework for building web applications and APIs in .NET. It is cross-platform, lightweight, fast, and designed to work well in both small projects and production systems. In practice it gives us ready-made hosting, built-in dependency injection, configuration, logging, and a request-processing pipeline.

### The Request Pipeline Model

Every request passes through the middleware pipeline — a chain of successive components. Each middleware can do something before passing the request along, after the next component has executed, or terminate processing at its own stage entirely.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

In the modern startup model, application configuration is concentrated primarily in `Program.cs`. That is where we configure services, build the application, and set up the pipeline.

> **Note.** Middleware order matters. It is not a list of random calls. Changing the order can break authorization, CORS, routing, or exception handling.

### The Most Important Architectural Elements

- **Host** — starts the application and manages its lifecycle.
- **Kestrel** — acts as the HTTP server.
- **Middleware pipeline** — processes requests and responses.
- **Dependency Injection** — allows registering and injecting dependencies.
- **Configuration** — merges settings from files, environment variables, and other sources.
- **Logging** — enables observing application behavior.

---

## 5. Your First ASP.NET Core Web API Project

### Creating the Project

A first API can be created from a ready-made template. Such a project already has the basic structure, configuration, and example files needed to get started.

```bash
dotnet new webapi -n BookStoreApi
cd BookStoreApi
dotnet run
```

After launch the application typically starts locally at a specified HTTP and HTTPS address. Depending on the template you may also get a preconfigured Swagger setup and a sample endpoint.

### Typical Files and Folders

- `Program.cs` — responsible for configuring services and the pipeline.
- `appsettings.json` — stores application settings.
- `Controllers/` — contains controller classes if you use that model.
- `Properties/launchSettings.json` — describes launch profiles for the development environment.

> **Best practice.** At the start of a project it is worth maintaining a clear structure. Even if the application is small, it is a good idea to separate input and output contracts, business logic, and HTTP entry points right away.

---

## 6. Controllers, Routing, and HTTP Method Handling

### Why Controllers Exist

A controller is a class responsible for handling HTTP requests for a given group of endpoints. This approach works well when the API already has several resources, more routing logic, and a larger number of actions.

```csharp
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<BookDto>> GetAll()
    {
        var books = new List<BookDto>
        {
            new BookDto { Id = 1, Title = "Clean Code", Author = "Robert C. Martin" },
            new BookDto { Id = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt" }
        };

        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public ActionResult<BookDto> GetById(int id)
    {
        if (id == 1)
        {
            return Ok(new BookDto { Id = 1, Title = "Clean Code", Author = "Robert C. Martin" });
        }

        return NotFound();
    }

    [HttpPost]
    public ActionResult<BookDto> Create(CreateBookDto dto)
    {
        var created = new BookDto
        {
            Id = 101,
            Title = dto.Title,
            Author = dto.Author
        };

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
```

### Most Commonly Used Attributes

- `[ApiController]` — enables convenient API behaviors, including improved model binding and automatic validation-error responses.
- `[Route]` — sets the base route for a controller or action.
- `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]` — bind an action to an HTTP method.
- `[FromRoute]`, `[FromQuery]`, `[FromBody]` — explicitly indicate the source of input data.

### Routing

Routing maps a URL to the appropriate action. In practice we often use the `[controller]` token, path parameters, and type constraints such as `{id:int}`. Well-designed routing helps consumers understand the API without reading the implementation.

> **Note.** If you use `PUT`, you are typically communicating a full resource update. If you are updating only a single field, `PATCH` may be more semantically appropriate. This difference affects client expectations.

---

## 7. Minimal APIs as an Alternative to Controllers

### What Minimal APIs Are

Minimal APIs let you define endpoints without creating full controller classes. Instead of an elaborate attribute-based model and separate classes, a single mapping in `Program.cs` or in dedicated extension modules is often sufficient.

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var books = new List<BookDto>
{
    new BookDto { Id = 1, Title = "Clean Code", Author = "Robert C. Martin" },
    new BookDto { Id = 2, Title = "Refactoring", Author = "Martin Fowler" }
};

app.MapGet("/api/books", () => Results.Ok(books));

app.MapGet("/api/books/{id:int}", (int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);
    return book is null ? Results.NotFound() : Results.Ok(book);
});

app.MapPost("/api/books", (CreateBookDto dto) =>
{
    var book = new BookDto
    {
        Id = books.Max(b => b.Id) + 1,
        Title = dto.Title,
        Author = dto.Author
    };

    books.Add(book);
    return Results.Created($"/api/books/{book.Id}", book);
});

app.Run();
```

### When to Choose Controllers vs. Minimal APIs

| Criterion | Controllers | Minimal APIs |
|---|---|---|
| Large API with many resources | Very good | Possible, but less readable at scale |
| Quick start and small project | Good | Very convenient |
| Extensive filtering and action organization | Usually more convenient | Requires more organizational discipline |
| Learning HTTP and API basics | Good | Good |

In practice both approaches are valid. The choice depends on the size of the project, the team, and the nature of the application.

---

## 8. Model Binding and Data Validation

### Model Binding

Model binding is the mechanism that maps data from an HTTP request onto method parameters and C# objects. The framework does it for us according to defined rules.

Data can come from:
- the route — e.g., `/api/books/42`
- the query string — e.g., `/api/books?page=2&size=20`
- the JSON request body — e.g., in `POST` and `PUT`
- headers

### Validation with Data Annotations

```csharp
using System.ComponentModel.DataAnnotations;

public class CreateBookDto
{
    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    public string Author { get; set; } = string.Empty;

    [Range(1, 5000)]
    public int Pages { get; set; }

    [Range(typeof(decimal), "0.01", "9999.99")]
    public decimal Price { get; set; }
}
```

If you use `[ApiController]`, validation errors are handled automatically and the client will receive a `400 Bad Request` response with information about the model problems.

### FluentValidation

For simple models, Data Annotations are often sufficient. When there are more rules or you want better separation of validation from the model, FluentValidation is a convenient solution.

```csharp
using FluentValidation;

public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(200);

        RuleFor(x => x.Author)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}
```

> **Important.** Validation should target input data at the system boundary. It is better to reject a bad request immediately than to let invalid state propagate deeper into the business logic and the database.

---

## 9. Returning Responses

### IActionResult, ActionResult\<T\>, and Status Codes

In controllers you very often do not return a raw object — instead you return a type representing the HTTP response.

- `IActionResult` — convenient when the action can return many different response types.
- `ActionResult<T>` — convenient when the success type is known but you still want the ability to return errors like `NotFound()` or `BadRequest()`.

```csharp
[HttpGet("{id:int}")]
public ActionResult<BookDto> GetById(int id)
{
    var book = _bookService.GetById(id);

    if (book is null)
    {
        return NotFound();
    }

    return Ok(book);
}

[HttpDelete("{id:int}")]
public IActionResult Delete(int id)
{
    var deleted = _bookService.Delete(id);

    if (!deleted)
    {
        return NotFound();
    }

    return NoContent();
}
```

### Most Commonly Used Responses

- `Ok(...)` — for successfully returning data.
- `CreatedAtAction(...)` — after creating a resource.
- `NoContent()` — when the operation succeeded but there is no body to return.
- `BadRequest(...)` — for invalid input data.
- `NotFound()` — when the resource does not exist.
- `Problem(...)` — for communicating a structured error.

> **Best practice.** After creating a new resource it is good to return `201 Created` and provide the path to the newly created item. This is a clearer contract than a plain `200 OK`.

---

## 10. DTOs and Data Mapping

### Why DTOs Exist

A DTO (Data Transfer Object) is an object used to carry data between layers or between systems. In a Web API, DTOs help separate the internal application model from the contract exposed externally.

If you directly expose a domain entity or an ORM entity, problems appear quickly:
- you reveal fields the client should not see
- you tie the API contract to the internal code structure
- you make it harder to evolve the domain model
- the goals of the persistence layer and the HTTP layer become mixed

### Request DTOs and Response DTOs

```csharp
public class CreateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
}

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
}
```

In practice input and output DTOs are often separated. The data needed to create an object does not have to be identical to the data you want to show the client after saving.

### Manual Mapping

```csharp
public static BookDto MapToDto(Book book)
{
    return new BookDto
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author
    };
}
```

### AutoMapper

With a larger number of models you can use AutoMapper — a library that automates mapping between types.

```csharp
using AutoMapper;

public class BooksProfile : Profile
{
    public BooksProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateBookDto, Book>();
    }
}
```

> **Note.** DTOs are not "art for art's sake." In a normal API, DTOs help you maintain control over the contract and avoid accidentally exposing the wrong data.

---

## 11. Application Configuration

### appsettings, Environment Variables, and IOptions

ASP.NET Core can pull configuration from multiple sources. You typically start with `appsettings.json` and `appsettings.{Environment}.json` files, but environment variables, command-line arguments, and other configuration providers are available as well.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=BookStoreDb;Trusted_Connection=True;"
  },
  "MailSettings": {
    "Host": "smtp.example.com",
    "Port": 587,
    "Sender": "noreply@example.com"
  }
}
```

### Strongly Typed Configuration with IOptions

```csharp
public class MailSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Sender { get; set; } = string.Empty;
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MailSettings>(
    builder.Configuration.GetSection("MailSettings"));

public class NotificationService
{
    private readonly MailSettings _settings;

    public NotificationService(IOptions<MailSettings> options)
    {
        _settings = options.Value;
    }
}
```

`IOptions<T>` lets you read configuration in a typed, readable way. This is better than constantly fetching strings by key scattered throughout the codebase.

> **Best practice.** Secrets, passwords, and connection strings for environments other than local should not be committed to the repository. The mere fact that something "works from appsettings.json" does not mean it is a good production solution.

---

## 12. Middleware in Practice

### Exception Handling

A well-prepared API should not leak raw exceptions and stack traces to the client. Exceptions need to be logged, and the client should receive a controlled response.

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            message = "An unexpected server error occurred."
        });
    });
});

app.MapControllers();
app.Run();
```

### Logging

Logging is used to observe system behavior, diagnose errors, and analyze request flow. In practice you want to log the most important events without flooding the logs with random noise. Good logging helps you later reconstruct what actually happened in the system.

### CORS

CORS (Cross-Origin Resource Sharing) controls which frontends can make requests to the API from a browser. This is particularly important when the frontend runs at a different address than the backend.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("https://frontend.example.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("FrontendPolicy");
app.MapControllers();
app.Run();
```

> **Note.** CORS is not an authorization mechanism. It is a set of browser rules governing cross-origin requests. The API still needs separate authentication and authorization mechanisms.

---

## 13. Swagger and OpenAPI

### Documentation and API Testing

Swagger and OpenAPI help describe an API in a way that is both machine-readable and human-friendly. This makes it easier to document endpoints, test them without building a separate client, and quickly understand the application's contract.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
```

Swagger UI displays a list of endpoints, parameters, model schemas, and response codes, and allows you to execute requests directly from the browser.

> **Best practice.** Swagger is extremely helpful during development, but its public exposure in a production environment should be a deliberate decision — not the result of a default configuration left uncontrolled.

---

## 14. Building a Complete REST CRUD API Step by Step

### How to Assemble All the Elements

Building a complete CRUD API usually follows a similar pattern:

1. Define the resource, e.g., `Book`, `Student`, or `Order`.
2. Prepare DTO contracts for creation, update, and read operations.
3. Design endpoints following REST conventions and HTTP methods.
4. Add input validation.
5. In the controller or Minimal API, invoke the business logic.
6. Map the model to a response DTO.
7. Return the appropriate HTTP status code.
8. Add configuration, error handling, logging, and documentation.

### Example Flow for a Book Resource

```csharp
[HttpPut("{id:int}")]
public ActionResult<BookDto> Update(int id, UpdateBookDto dto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var book = _bookService.GetById(id);
    if (book is null)
    {
        return NotFound();
    }

    book.Title = dto.Title;
    book.Author = dto.Author;
    book.Price = dto.Price;

    _bookService.Update(book);

    var result = new BookDto
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        Price = book.Price
    };

    return Ok(result);
}
```

This example clearly illustrates the full flow: HTTP input → model binding → validation → application logic → mapping → response. In a larger project some of this logic would move into services, repositories, or an application layer, but the underlying mechanism stays the same.

> **Important.** A "working API" is not just an endpoint that returns JSON. A good API has consistent routing, sensible validation, controlled responses, a secure DTO contract, organized configuration, and predictable behavior in error scenarios.

---

## 15. Most Common Mistakes and Pitfalls

- Always returning `200 OK`, even when an error occurred or the resource does not exist.
- Exposing domain or ORM entities directly to the client.
- Failing to distinguish between input-data validation and business-logic errors.
- Unreadable endpoints with verbs in the URL and inconsistent naming.
- Confusing `PUT` with a partial update, which semantically fits `PATCH` better.
- Lack of control over middleware order.
- Storing secrets and production settings directly in the repository.
- Skipping API documentation, forcing the client to guess the contract.

---

## 16. Key Takeaways

- A Web API is a specialized type of web application that exposes data and operations to other clients over HTTP.
- To design APIs well you need to understand requests, responses, HTTP methods, status codes, headers, and statelessness.
- REST organizes API design around resources, endpoints, and consistent operation semantics.
- ASP.NET Core provides ready-made hosting, a middleware pipeline, DI, configuration, and convenient tools for building APIs.
- Controllers and Minimal APIs solve a similar problem but suit slightly different contexts.
- Model binding and validation protect the system boundary from invalid data.
- Correct HTTP responses require not only data but also proper status codes and a sensible error contract.
- DTOs help separate the application's internal model from what the client sees.
- Configuration, middleware, and Swagger are part of a mature API — not an afterthought left until the end.
- A complete CRUD API is the combination of good HTTP design, correct implementation, and well-organized application architecture.
