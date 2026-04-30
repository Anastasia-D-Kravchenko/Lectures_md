# Object Oriented Design – Learning Guide
### Lectures 3, 4 & 5 + Tutorial Examples | dr. inż. Tomasz Pieciukiewicz

---

## Table of Contents

1. [Why Object Orientation?](#1-why-object-orientation)
2. [Core OO Concepts](#2-core-oo-concepts)
3. [OO Programming Languages](#3-oo-programming-languages)
4. [UML Class Diagrams – Basics (Lecture 4)](#4-uml-class-diagrams--basics)
5. [Inheritance in Depth](#5-inheritance-in-depth)
6. [Methods: Binding, Overriding & Overloading](#6-methods-binding-overriding--overloading)
7. [Advanced Associations (Lecture 5)](#7-advanced-associations)
8. [Aggregation & Composition](#8-aggregation--composition)
9. [N-Ary Associations](#9-n-ary-associations)
10. [UML Extension Mechanisms](#10-uml-extension-mechanisms)
11. [Quick-Reference Cheat Sheet](#11-quick-reference-cheat-sheet)
12. [Key Terms Glossary](#12-key-terms-glossary)
13. [Worked Examples from Tutorials](#13-worked-examples-from-tutorials)
14. [Statechart Diagrams (Lecture 6)](#14-statechart-diagrams)
15. [Interaction Diagrams (Lecture 7)](#15-interaction-diagrams)
16. [Implementation & Package Diagrams (Lecture 8)](#16-implementation--package-diagrams)
17. [Other OO Models (Lecture 9)](#17-other-oo-models)
18. [Design Patterns (Lecture 11)](#18-design-patterns)
19. [Architectural Patterns (Lecture 12)](#19-architectural-patterns)
20. [Entity Component Systems & Data Oriented Programming (Lecture 13)](#20-entity-component-systems--data-oriented-programming)
21. [Domain Driven Design & SOLID (Lecture 14)](#21-domain-driven-design--solid)
22. [Wrong Assumptions in Software Design](#22-wrong-assumptions-in-software-design)

---

## 1. Why Object Orientation?

There is a fundamental **gap** between how we perceive the real world and how we traditionally model data and processes in IT systems. Object Orientation (OO) is one of the ideologies developed to **close that gap**.

OO has influenced many areas of software engineering:
- Analysis and design methods (Rumbaugh, Booch, Jacobson, Yourdon)
- Programming languages (Smalltalk, C++, Java, Eiffel)
- Databases and persistent storage (ODMG-2.0, ObjectStore)
- Integration technologies (OMG CORBA, OLE/DCOM/ActiveX)
- Visual IDEs and software quality metrics

---

## 2. Core OO Concepts

### 2.1 Object

An **object** is a data structure with associated operations, corresponding to an entity in the problem domain. Key characteristics:

- Has a **name** and well-defined boundaries
- Is self-contained and may be complex
- May be associated with other objects
- Has a **type** (its class)

**Object properties:**
| Property | Description |
|----------|-------------|
| **Identity** | A unique, persistent internal identifier; separates it from all other objects |
| **State** | Attribute values and associations with other objects |
| **Behaviour** | The set of operations that can be performed on the object |

> 💡 **Example:** A bank account object has attributes (Number, Balance, Owner) and behaviours (Withdraw, Deposit, CheckBalance, Close).

### 2.2 Object Identity

- An entity can be identified by the mere fact of its existence, regardless of its properties.
- Two objects may be **identical in value** but still be two separate entities.
- Identifiers are assigned internally by the implementation — the programmer does not manage them explicitly.
- Identifiers can be **transient** (session-only) or **permanent** (persistent).

### 2.3 Class

A **class** is a named description of a group of objects sharing a set of invariants (properties, methods, semantics).

- A class is **not** a set of objects; it **describes** objects.
- The class/subclass relation means a subclass has all invariants of the superclass plus its own.

**Class invariants may include:** name, type, methods, events/exceptions, export list (interface), and constraints.

> 💡 Methods and type information are stored **once in the class**, not repeated in every object instance.

### 2.4 Encapsulation

Encapsulation encloses the structure and implementation of an object into a single block, separating **specification** from **implementation**.

> *"A programmer should know enough about an object to use it efficiently. Everything that may be hidden, should be hidden."* — Parnas, 1972

Two approaches:
- **Orthodox (Smalltalk):** Only methods are visible; all attributes are hidden.
- **Orthogonal (C++):** Any property can be declared public or private.

### 2.5 Inheritance

**Generalization–Specialization** links a more general superclass to more specialized subclasses. The subclass inherits all invariants from the superclass and may add its own.

- Inheritance is **transitive**: a subclass inherits from all its ancestors.
- It is the primary mechanism for implementing generalization–specialization, but not the only one.

### 2.6 Polymorphism

Polymorphism means **"many forms"** of a single entity. Types include:

- **Polymorphic types** (e.g. in ML)
- **Polymorphic methods** – same operation name, different implementation per class
- **Parametric Polymorphism** – parametrized types (generics)

Polymorphism relies on **late (dynamic) binding** to decide at runtime which method implementation to invoke.

### 2.7 Operations vs Methods

| Term | Meaning |
|------|---------|
| **Operation** | The *what* — a function that can be used on an object; a property of the class |
| **Method** | The *how* — a specific implementation of an operation in one class of the hierarchy |

A single `print` operation may have three methods: ASCII, PostScript, Image.

### 2.8 Messages vs Function Calls

| Messages (OO) | Function Calls (procedural) |
|---|---|
| `object.operation(arg1, arg2)` | `function(object, arg1, arg2)` |
| Object is the default parameter | Object is an explicit parameter |
| Use **late binding** | Use **early binding** |

---

## 3. OO Programming Languages

| Language | Key Traits |
|----------|-----------|
| **Smalltalk** | Developed 1976–83 at Xerox PARC; everything is an object; powerful IDE; great for rapid prototyping |
| **C++** | Hybrid (low + high level); supports classes, inheritance, encapsulation; large-scale use; criticized for complexity and error-prone nature |
| **Java** | Mix of C++, Smalltalk and Objective-C; compiled to bytecode; runs on a VM; improved portability and security |
| Others | Eiffel, Ada95, Python, Object Pascal, CLOS, Objective-C, Modula-3, and many more |

---

## 4. UML Class Diagrams – Basics

### 4.1 Class Notation

A class box has up to four fields:

```
ClassName
──────────────────
attributes
──────────────────
methods
──────────────────
custom info (optional)
```

**Full syntax:**
- **Class name:** `«stereotype» visibility Name {tagged_values}`
- **Attributes:** `«stereotype» visibility name : type = initial_value {tagged_values}`
- **Methods:** `«stereotype» visibility name(parameters) : return_type {tagged_values}`

**Visibility symbols:**

| Symbol | Meaning |
|--------|---------|
| `+` | public |
| `-` | private |
| `#` | protected |

**Parameter kinds:** `in`, `out`, `inout`

> 💡 The class name is the **only mandatory** element. By convention it is a singular noun.

### 4.2 Associations

An **association** describes a set of relationships with common semantics between two classes.

```
Person ──────────── works_in ──────────── Company
        *                          1..*
  employee                      employer
```

**Multiplicity examples:**

| UML | Meaning |
|-----|---------|
| `1` | Exactly one |
| `0..1` | Zero or one (optional) |
| `*` or `0..*` | Zero or more |
| `1..*` | One or more |
| `3..5` | Between three and five |

**Names and roles** may be omitted if obvious. A minimal description is either the association name or both role names.

### 4.3 Abstract Classes

- An **abstract class** cannot have instances; it defines common properties for a group.
- Marked with `{abstract}` or by writing the name in *italics*.
- A **concrete class** can have instances and must implement all inherited abstract methods.

**Abstract methods:**
- Specified in an abstract class, implemented in subclasses.
- An abstract class may or may not have abstract methods.
- A concrete class **cannot** have abstract methods.

### 4.4 Interfaces

- Stereotype `«interface»`
- Contains method **specifications only** — no implementation and no attributes.
- All methods are public.
- A class **realizes** an interface by implementing all its methods.
- Other classes **depend on** an interface (shown with a dashed arrow).

> 💡 Unlike interfaces, abstract classes may contain attributes and partial implementations.

### 4.5 Parametrized Classes (Templates)

Used to define generic structures. The type parameter (`T`) is specified at use:

```
Set<Employee>  «bind»<Employee>→  EmployeeSet
```

### 4.6 Class Extent

The **class extent** is the current set of all instances of a class:

| Level | Description |
|-------|-------------|
| I | Direct instances of the class (excluding subclasses) |
| II | All instances (direct + indirect), limited to attributes of this class |
| III | All instances (direct + indirect) with all attributes |

---

## 5. Inheritance in Depth

### 5.1 Constraints on Generalization Sets

| Constraint | Meaning |
|------------|---------|
| `{disjoint}` | Each object belongs to **one and only one** subclass |
| `{overlapping}` | An object may belong to **more than one** subclass simultaneously |
| `{complete}` | Every superclass instance belongs to some subclass; all subclasses identified |
| `{incomplete}` | Some superclass instances may have no matching subclass |
| `«dynamic»` | An object can **migrate** between subclasses without changing identity (not supported in most languages) |

> ⚠️ UML 2.0–2.4.1 default: `{incomplete, disjoint}`. UML 2.5 default: `{incomplete, overlapping}`. The course requires **all constraints** to be stated explicitly.

### 5.2 Multi-Inheritance

- A class can inherit from **two or more** classes, even from different hierarchies.
- **Problem — name conflicts:** if two superclasses define an attribute or method with the same name, the subclass must resolve which version to use (e.g. Eiffel treats this as an error; O2 allows renaming).

### 5.3 Multi-Inheritance vs. Overlapping

| | Multi-Inheritance | Overlapping |
|--|--|--|
| Creates a new class? | Yes | No |
| Scope | Can span multiple inheritance trees | One tree only |
| Typical use | New concept combining two types | Same object, multiple roles |

### 5.4 Multi-Aspect Inheritance (GeneralizationSets)

A superclass can be specialized along **several independent dimensions** (aspects). Each GeneralizationSet is independent. Constraints like `{complete}` can make specific aspects mandatory for all objects.

### 5.5 Ellipsis (`...`)

Used on diagrams to indicate that some identified elements are intentionally not shown (to reduce clutter). Commonly marks missing classes, attributes, or methods.

---

## 6. Methods: Binding, Overriding & Overloading

### 6.1 Binding

**Binding** is the process of replacing a symbolic name with an address or internal entity identifier.

| Type | When | Pros | Cons |
|------|------|------|------|
| **Early (static)** | Compile/link time | Better performance; full static type-checking | Less flexible; no plug-ins |
| **Late (dynamic)** | Runtime | Method overriding; plug-in support; rapid development | Worse performance; hard type-checking |

Late binding enables: polymorphism, dynamic views, query languages, object migration, schema evolution.

### 6.2 Method Overriding

A method in a subclass **overrides** (replaces) the method with the same name inherited from the superclass.

- Requires **late binding**.
- Supports reuse and polymorphism.

```
Employee              Manager
  fire()   →override→   fire()
```

### 6.3 Method Overloading

The same symbol has different semantics depending on the **number or type** of arguments:

```java
move(x, y)       // 2D version
move(x, y, z)    // 3D version
```

> ⚠️ Some authors (e.g. Cardelli) argue overloading is **not** a form of polymorphism. All methods implementing a single operation should share the same signature.

---

## 7. Advanced Associations

### 7.1 Derived Associations

An association marked with `/` is **derived** — it provides redundant information computable from other associations.

```
Department ─has─ Section ─employs─ Employee
                    └──── /works_in ──────┘
```

### 7.2 Association Classes

When an association itself has **attributes or operations**, it is modelled as an **association class** connected to the association line with a dashed line.

If the association class has no methods and no further associations, only its **attributes** need to be shown (name can be omitted).

> 💡 Use case: `File ─── AccessPerm ─── User` where `AccessPerm` holds `permLevel` and `checkOperation()`.

### 7.3 Qualified Associations

A **qualifier** is an attribute (or set of attributes) used to identify objects at one end of the association — like a key in a hashtable. It reduces the effective multiplicity on the target end.

---

## 8. Aggregation & Composition

### 8.1 Aggregation

A specialised association representing a **whole–part** dependency (e.g. an engine and its parts). Context-dependent; often used to work around environment limitations.

```
Whole ◇────────── Part
       *       *
```

### 8.2 Composition

**"Strong" aggregation.** The part's lifecycle is tied to the whole's lifecycle. Part objects **cannot be shared**.

```
Whole ◆────────── Part
       1       *
```

> ⚠️ In composition the multiplicity on the Whole side is **always 1** (or at most `0..1` in rare cases). There is no other valid option.

| | Aggregation | Composition |
|--|--|--|
| Lifecycle coupling | Loose | Tight (part dies with whole) |
| Sharing parts | Allowed | Not allowed |
| Multiplicity on Whole | Any | 1 (or 0..1) |

---

## 9. N-Ary Associations

An **N-ary association** links objects in *n* roles belonging to no more than *n* different classes.

- A binary association is a special case (n = 2).
- For n > 2, the following binary features are **unavailable**: Aggregation, Composition, Qualifiers, One-way navigation.

**Multiplicity rule for n-ary:** The multiplicity at class C specifies how many instances of C are associated with each combination of one instance from each of the remaining n−1 classes.

**Workaround:** Convert an n-ary association to a new **association class** connected to all participating classes with binary associations.

---

## 10. UML Extension Mechanisms

UML provides three mechanisms to extend and customise the model:

### 10.1 Stereotypes

Allow **meta-classification** of model elements. Notation: `«stereotype_name»` or an icon.

- Each element may have **at most one** stereotype shown as an icon.
- Predefined stereotypes exist (e.g. `«interface»`, `«persistent»`, `«include»`, `«extend»`).
- Users may define their own.
- One stereotype can apply to a list of elements; the list ends with `«»`.

### 10.2 Tagged Values

Associate **arbitrary metadata** with any model element as `keyword=value` pairs inside `{ }`.

```
{author="Kowalski", status="tested"}
```

### 10.3 Constraints

Specify **restrictions** on model elements. Can be expressed in:
- Natural language
- Formal language (OCL)
- Mathematical formula
- Code / pseudocode

Notation: enclosed in `{ }` placed after the element or inside an annotation/comment bubble.

**Types of constraints:**

| Type | Description |
|------|-------------|
| **Static** | Checked before a change; the new state must satisfy the constraint; restricts the domain |
| **Dynamic** | Considers both previous and new state; restricts the *change* itself |

**Predefined constraints:**
`{complete}`, `{incomplete}`, `{disjoint}`, `{overlapping}`, `{or}`, `{xor}`, `{ordered}`, `{subset}`, `{bag}`, `{hierarchy}`, `{dag}`

---

## 11. Quick-Reference Cheat Sheet

```
CLASS BOX               ASSOCIATIONS            INHERITANCE
┌─────────────┐         A ──────── B            A
│ «stereo»    │             1    *              △
│ ClassName   │         name, roles, mult.      │
│ {tagged}    │                                 B
├─────────────┤         AGGREGATION        COMPOSITION
│ +attr: Type │         Whole ◇── Part     Whole ◆── Part
│ -priv: int  │               * *               1  *
├─────────────┤
│ +method()   │         INTERFACE          ABSTRACT CLASS
│ #prot(): T  │         «interface»        ClassName
└─────────────┘         methods only       {abstract}
                        no attributes      may have attrs
                                           & impl.
```

| Concept | Symbol / Keyword |
|---------|-----------------|
| Public / Private / Protected | `+` / `-` / `#` |
| Abstract class or method | *Italics* or `{abstract}` |
| Derived attribute/association | `/name` |
| Association class | Dashed line to class box |
| Qualified association | Qualifier box on association end |
| Stereotype | `«name»` |
| Constraint | `{expression}` |
| Ellipsis (elements omitted) | `...` |

---

## 12. Key Terms Glossary

| Term | Definition |
|------|-----------|
| **Object** | A data structure with associated operations representing a real-world entity |
| **Object Identity** | A unique internal identifier that distinguishes an object from all others |
| **Class** | A named description of a group of objects sharing a set of invariants |
| **Encapsulation** | Hiding internal structure; separating interface from implementation |
| **Inheritance** | Mechanism by which a subclass acquires all invariants of its superclass |
| **Polymorphism** | A single operation name having different implementations across classes |
| **Operation** | *What* can be done (abstract specification) |
| **Method** | *How* it is done (concrete implementation) |
| **Early binding** | Name-to-address resolution at compile/link time |
| **Late binding** | Name-to-address resolution at runtime; required for polymorphism |
| **Abstract class** | A class that cannot be instantiated; defines shared structure |
| **Interface** | Method signatures only; no attributes, no implementation |
| **Aggregation** | Whole–part relationship (loose lifecycle coupling) |
| **Composition** | Strong aggregation (part lifecycle tied to whole; no sharing) |
| **Association class** | A class attached to an association to carry its own attributes/methods |
| **Qualified association** | Association with a qualifier attribute acting as a key |
| **N-ary association** | An association linking objects from more than two classes |
| **Stereotype** | Meta-classification label for a UML model element (`«name»`) |
| **Tagged value** | Arbitrary metadata attached to a model element (`{key=value}`) |
| **Constraint** | A restriction on a model element; can be static or dynamic |
| **Derived association** | Redundant association computable from other associations (marked `/`) |
| **Class extent** | The current set of all instances of a class |

---

## 13. Worked Examples from Tutorials

This section walks through the complete class diagrams from Tutorial 7 and the CLD University exercise, explaining **why** each design decision was made.

---

### 13.1 Example A – Event Company (Tutorial 7)

#### Problem statement

An event company organises children's events (schools, kindergartens). The system must track clients (persons or institutions), employees, performers, and orders for items.

---

#### Step 1 – The `Person` abstract class and basic hierarchy

The design starts with a shared `Person` superclass because both **customers** and **employees** share personal data (name, surname, date of birth).

```
┌──────────────────────────┐
│     Person {abstract}    │
├──────────────────────────┤
│  Name                    │
│  Surname                 │
│  dateOfBirth             │
│  /age                    │   ← derived attribute (computed from dateOfBirth)
├──────────────────────────┤
│  calAge() {abstract}     │   ← abstract: each subclass implements differently
└──────────────────────────┘
              △
     ┌────────┴────────┐
     │                 │
┌────────────┐  ┌──────────────────────────┐
│  Customer  │  │       Employee           │
├────────────┤  ├──────────────────────────┤
│ bonusCard  │  │  Salary                  │
│ [0..1]     │  │  hobbies [0..5]          │
├────────────┤  │  bonus = 500 PLN         │
│changeSur.. │  ├──────────────────────────┤
└────────────┘  │  calAge()                │
                │  ViewListOfAllEmployees() │
                └──────────────────────────┘
```

**Key decisions:**
- `Person` is **abstract** because you never create a "bare" Person — only a Customer or Employee.
- `/age` is a **derived attribute** (marked with `/`) because it is always computable from `dateOfBirth`; storing it separately would create redundancy.
- `calAge()` is **abstract** in `Person` and **concrete** in `Employee`, forcing every concrete subclass to provide an implementation.
- `hobbies [0..5]` is a **repeatable attribute** — each employee can have between 0 and 5 hobbies.
- `bonusCardNumber [0..1]` is **optional** — a customer may or may not have a bonus card.

---

#### Step 2 – `{overlapping}` inheritance for WorkingCustomer

Some people are **both** a Customer and an Employee at the same time. This is modelled with `{overlapping}`:

```
              Person {abstract}
                    △
          {overlapping}
     ┌──────────┴──────────┐
     │                     │
  Customer              Employee
     │                     │
     └──────┬──────────────┘
            │
    WorkingCustomer
    ──────────────
    specialBonus
```

> 💡 **Why overlapping?** A `WorkingCustomer` object simultaneously *is* a `Customer` and *is* an `Employee`. With `{disjoint}`, an object could only belong to one subclass — impossible here. `{overlapping}` explicitly permits membership in multiple subclasses.

> ⚠️ `{overlapping}` is **common in the real world** but **not directly supported** by most programming languages. In practice it is implemented using interfaces or composition.

---

#### Step 3 – Employee specialisation with `{disjoint}`

Employees are further split into **Manager** and **Operator**. These are mutually exclusive roles (you can't be both simultaneously), so `{disjoint}` applies:

```
┌────────────────────────────┐
│   Employee {abstract}      │
├────────────────────────────┤
│  Name, Surname, DOB        │
│  salary, workingHours      │
├────────────────────────────┤
│  calAge()                  │
│  calSalary() {abstract}    │   ← different formula per subclass
└────────────────────────────┘
              △
      {disjoint}
     ┌─────────┴──────────┐
     │                    │
┌──────────┐       ┌───────────┐
│  Manager │       │  Operator │
├──────────┤       ├───────────┤
│  Bonus   │       │           │
├──────────┤       ├───────────┤
│calSalary │       │ calSalary │
│(hours    │       │(workingHrs│
│+bonus)   │       │)          │
└──────────┘       └───────────┘
```

**Key decisions:**
- `calSalary()` is **abstract** in `Employee{abstract}` — the formula differs: `Manager` adds a bonus, `Operator` does not.
- `{disjoint}` prevents an employee from being both Manager and Operator.

---

#### Step 4 – Customer age groups with `{dynamic}`

Customers change age category over time. This calls for `{dynamic}` specialisation:

```
        Customer
            △
    {disjoint, dynamic}
   ┌────────┬────────┐
   │        │        │
Child  Teenager  CustomerAdult
age<13  min=13    min=18
```

> 💡 **Why dynamic?** A `Child` customer grows up and eventually becomes a `Teenager`, then a `CustomerAdult` — the **same object** migrates between subclasses without losing its identity (order history, etc. is preserved). `{dynamic}` explicitly marks this as allowed. Note that standard programming languages do **not** support this natively.

---

#### Step 5 – Order and Items

```
┌──────────┐   0..*        1   ┌──────────┐
│  Order   │───────────────────│ Customer │
├──────────┤                   └──────────┘
│/totalPrice│
├──────────┤
│calcTotal()│
└──────────┘
     │  1
     │
     │ 0..*
┌─────────────┐     ┌──────────────────┐
│ ItemInOrder │─────│      Item        │
├─────────────┤     ├──────────────────┤
│  amount     │     │  name, size,     │
└─────────────┘     │  price           │
                    ├──────────────────┤
                    │  addItem()       │
                    │  changePrice()   │
                    └──────────────────┘
```

**Key decisions:**
- `/totalPrice` on `Order` is **derived** — it is computed via `calcTotalPrice(amount * price)` and not stored.
- `ItemInOrder` is an **association class** between `Order` and `Item`, carrying the `amount` attribute which belongs to neither class alone.

---

#### Step 6 – Static and Dynamic Constraints on Employee salary

From the tutorial diagram two constraints govern salary changes:

| Constraint | Type | Expression |
|---|---|---|
| Salary domain limit | **Static** | `salary <= 10 000` — checked on every save |
| Rate of change | **Dynamic** | `can change per year only by 10%` — considers old and new value |

```
┌───────────────────────────────────┐
│           Employee                │
├───────────────────────────────────┤
│  salary {<= 10 000}               │  ← static constraint on the attribute
├───────────────────────────────────┤
│  changeSalary()                   │
└───────────────────────────────────┘
              {can change per year only by 10%}  ← dynamic constraint on the method
```

---

#### Step 7 – Employee–Project with `{subset}` constraint

```
┌──────────┐  0..5   assigned to>   0..*  ┌─────────┐
│ Employee │─────────────────────────────│ Project │
└──────────┘                              └─────────┘
     │                                        │
     │  0..1   is coordinating>   0..*        │
     └────────────────────────────────────────┘

{subset} — the coordinator must be chosen from the employees already assigned to the project
```

> 💡 **Why subset?** The `is coordinating` association is a **subset** of the `assigned to` association. You cannot coordinate a project you are not already assigned to.

---

### 13.2 Example B – University System (Tutorial 7 / CLD Uni)

#### Problem statement

A university system manages students, tutors, courses, rooms and theses.

---

#### Full diagram walkthrough

```
┌─────────────────────────────┐
│      Person {abstract}      │
├─────────────────────────────┤
│  Name, Surname              │
│  Date of Birth, Address     │
└─────────────────────────────┘
              △
     ┌────────┴────────┐
     │                 │
┌──────────┐     ┌──────────────────────────┐
│  Student │     │          Tutor           │
├──────────┤     ├──────────────────────────┤
│ idNumber │     │  scientific_title        │
│ semester │     │  minCourses = 1          │
└──────────┘     │  maxCourses = 3          │
                 └──────────────────────────┘
                              △
                   {complete, disjoint}
                  ┌───────────┴───────────┐
                  │                       │
           ┌──────────┐         ┌──────────────┐
           │ FullTime │         │   Contract   │
           └──────────┘         ├──────────────┤
                │               │  Start date  │
                │               │  End date    │
           ┌────┴────┐          └──────────────┘
           │  Chair  │
           ├─────────┤
           │  Name   │
           │ Address │
           │  Phone  │
           └─────────┘
```

**Key decisions:**
- `{complete, disjoint}` on Tutor: every tutor is either FullTime or Contract — no other type exists and no tutor can be both.
- `FullTime` tutors belong to a `Chair`; `Contract` tutors store contract dates instead.
- `minCourses=1` and `maxCourses=3` are **class-level constraints** on the Tutor class.

---

#### Thesis and its conditional association

```
┌──────────┐   0..1    writes>   ┌──────────────┐
│  Student │──────────────────▶ │    Thesis    │
└──────────┘                    ├──────────────┤
                                │  Title       │
                                │ maxSem. = 4  │
                                └──────────────┘

{has completed three semesters} ← OCL-style note constraint
                                   on the association end
```

> 💡 A student **may** write a thesis (`0..1`), but only after completing 3 semesters. The thesis has a maximum of 4 semesters to be completed (`maxSemesters = 4`). This is a **static constraint** on when the relationship may exist.

---

#### Scientific Advisor association

```
┌──────────┐  0..1  IsScientificAdvisor>  0..*  ┌──────────┐
│  Tutor   │────────────────────────────────────│ Student  │
└──────────┘                                    └──────────┘
```

A tutor may supervise many students; a student has at most one advisor (`0..1`).

---

#### Course, Enrollment (association class), and Schedule

```
┌──────────┐ 0..* attends> 0..* ┌────────────────┐
│  Student │─────────────────────│    Course      │
└──────────┘                     ├────────────────┤
       ╲                         │ topic, date    │
        ╲── Enrollment ──╱       │ time, duration │
           ┌──────────────┐      │ type           │
           │  Enrollment  │      └────────────────┘
           ├──────────────┤             │ 0..*
           │ start date   │             │
           │ end date[0..]│        ┌────┴──────┐  0..*  ┌────────┐
           │ completed    │        │ Schedule  │────────│  Room  │
           └──────────────┘        ├───────────┤        ├────────┤
                                   │ date,time │        │ bldg   │
                                   └───────────┘        │ room # │
                                                        │ seats  │
                                                        │ type   │
                                                        └────────┘
```

**Key decisions:**
- `Enrollment` is an **association class** on the Student–Course link because it carries attributes (`start date`, `end date`, `completed`) that belong to neither Student nor Course alone.
- `end date [0..1]` is **optional** — a student may currently be attending (no end date yet).
- `completed {"yes","no"}` is an **enumeration constraint** on the attribute.
- `Schedule` is a separate class because each lecture/tutorial may happen in a different room at a different time; these aren't fixed properties of the Course.

---

#### Tutor teaching roles

```
┌───────┐  1   IsLecturer>    1..*  ┌────────┐
│ Tutor │──────────────────────────│ Course │
└───────┘  0..* IsTeachAssistant>  └────────┘
```

Every course has exactly **one** lecturer and **at least one** teaching assistant (both are tutors, but in different roles on the same course).

---

### 13.3 Example C – University Student System (Tutorial Lecture 1 PDF)

This diagram extends the university model with richer student modelling.

#### Student class with overloaded `register()`

```
┌────────────────────────────────────────────────────┐
│                    Student                         │
├────────────────────────────────────────────────────┤
│  index {unique}                                    │
│  date of birth                                     │
│  /age                                              │
│  hobbies [0..*]          ← unbounded list          │
│  linkedin [0..1]         ← optional                │
│  minSemesters                                      │
│  status {active, registered, inactive, graduated}  │
│  creditsForDegree                                  │
├────────────────────────────────────────────────────┤
│  changeMinSemester()                               │
│  addHobby()                                        │
│  calAge(dateOfBirth)                               │
│  register()                         ← overloaded ─┐│
│  register(all attributes)                          ││
│  register(all except linkedin)      ← overloaded ─┘│
└────────────────────────────────────────────────────┘
```

> 💡 Three `register()` versions = **method overloading**. Each has a different parameter set. The system picks the right one based on what information is available at registration time.

---

#### Dynamic constraint on `creditsForDegree`

```
{creditsEarned >= creditsRequiredForDegree implies degreeStatus = 'completed'}
```

This is a **static constraint** — whenever credits are updated, the system checks this condition and may automatically update `status` to `graduated`.

---

#### Camera hierarchy demonstrating overloading + multi-inheritance

```
┌──────────────────┐
│  Camera{abstract}│
├──────────────────┤
│  zoom(){abstract}│
└──────────────────┘
         △
   ┌─────┴──────┐
   │            │
┌───────┐  ┌─────────┐
│Picture│  │  Video  │
├───────┤  ├─────────┤
│ par1  │  │  par2   │
├───────┤  ├─────────┤
│zoom   │  │zoom     │
│(par1) │  │(par2)   │
└───────┘  └─────────┘
      △         △
      └────┬────┘
           │
┌──────────────────────┐
│   VideoPicture Camera│   ← multi-inheritance
├──────────────────────┤
│  par1, par2          │
├──────────────────────┤
│  zoom(par1)          │   ← overloaded methods
│  zoom(par2)          │
│  zoom(par1, par2)    │
└──────────────────────┘
```

**Key decisions:**
- `Camera` is **abstract** with an abstract `zoom()`.
- `Picture` and `Video` each specialise zoom with their own parameter type.
- `VideoPicture Camera` uses **multi-inheritance** to combine both, and **overloads** `zoom()` with three signatures (par1 only, par2 only, or both).

---

#### Aggregation vs Composition: Room–Hotel and Student–Group

```
── Aggregation (loose) ──────────────────────────
Student ◇──────── Group
  0..*       0..*
(student can exist without a group; group without students)

── Composition (strong) ─────────────────────────
Room ◆──────── Hotel
 1..*     1
(rooms cannot exist without the hotel that owns them)
```

> 💡 The **diamond** symbol distinguishes the two:
> - `◇` (hollow) = **aggregation**: loose lifecycle, sharing allowed.
> - `◆` (filled) = **composition**: strict lifecycle, no sharing, whole-side multiplicity is always 1.

---

#### Association class: Ingredient–Recipe

```
┌────────────┐  0..*    0..*  ┌──────────┐
│ Ingredient │────────────────│  Recipe  │
├────────────┤                ├──────────┤
│ name       │                │ name     │
│ amount     │                │ desc     │
└────────────┘                └──────────┘
        ╲                    ╱
         ╲── IngrInRecip ──╱
            ┌────────────┐
            │ IngrInRecip│
            ├────────────┤
            │  amount    │   ← how much of THIS ingredient in THIS recipe
            └────────────┘
```

> 💡 The `amount` that ties a specific ingredient to a specific recipe cannot live in either class alone — it only makes sense in the context of the **pair**. This is the classic use-case for an **association class**.

---

### 13.4 Common Design Patterns & Mistakes (from Tutorial Diagrams)

| Situation | Correct approach | Common mistake |
|-----------|-----------------|----------------|
| People sharing attributes but differing in role | Abstract superclass + subclasses | Duplicating attributes in every class |
| Attribute computable from another | Derived attribute `/name` | Storing redundant data |
| Attribute that doesn't always exist | Multiplicity `[0..1]` | Mandatory attribute that breaks some objects |
| Object changes role over lifetime | `{dynamic}` specialisation | Creating a new object and losing history |
| Two roles are mutually exclusive | `{disjoint}` | Leaving it ambiguous |
| All subclasses are known and defined | `{complete}` | Leaving superclass concrete when it shouldn't be |
| Data belongs to the relationship, not either class | Association class | Adding the attribute to one of the classes |
| Part cannot exist without its whole | Composition `◆` | Using plain association or aggregation |
| Coordinator must be subset of assigned | `{subset}` constraint | No constraint, allowing invalid coordinator |

---

*Guide compiled from Lectures 3, 4 & 5 and Tutorial 7 of the Object Oriented Design course.*

---

## 14. Statechart Diagrams

### 14.1 What is a State Machine?

An object can be treated as a **Finite State Machine (FSM)** — a directed graph where:
- **Nodes** = states of the object
- **Edges** = transitions triggered by events

Two types of state machine:

| Type | Scope |
|------|-------|
| **Protocol state machine** | Lifecycle of a single object; uses `{protocol}` tag |
| **Behavioral state machine** | Lifecycle of a group of objects in a specific aspect of system behaviour |

Diagram header syntax: `sm DiagramName` or `sm DiagramName{protocol}`

---

### 14.2 State Notation

A state box has up to four optional sections:

```
┌──────────────────────────────┐
│          State Name          │
├──────────────────────────────┤
│  entry / action1 / action2   │  ← executed on entering
│  do    / activity1           │  ← executed while in state
│  exit  / action1             │  ← executed on leaving
├──────────────────────────────┤
│  event(attrs) / action       │  ← internal event handling
├──────────────────────────────┤
│  [decomposition]             │  ← internal sub-states
└──────────────────────────────┘
```

- **Action** — operation that **cannot** be interrupted; a list of actions is treated as one action.
- **Activity** — operation that **can** be interrupted (usually a reference to another statechart).

---

### 14.3 Events

An event happens at a point in time, is important to the system, and may cause a state transition. Events can be **synchronous or concurrent** and may carry **attributes**.

| Event Type | Syntax | Description |
|------------|--------|-------------|
| **Call event** | `op(a: T)` | Synchronous call for an operation; most common type |
| **Change event** | `when(expression)` | Boolean condition becomes true |
| **Signal** | `signal_name(a: T)` | Asynchronous call, usually from outside the system |
| **Time event** | `after(5 sec.)` | Passage of time |

**Deferred events:** An event can be added to a deferred queue instead of being handled immediately. It will be processed when a state that doesn't defer it becomes active. Notation: `event/deferred`.

---

### 14.4 Transitions

A transition is described by: `event [condition] / action`

| Transition Type | Description |
|-----------------|-------------|
| **External** | Standard transition from one state to another |
| **Internal** | Handled inside the state; no state exit/entry |
| **Self-transition** | Exits and re-enters the same state (entry/exit actions fire) |
| **Completion transition** | Automatic; triggered when all processing in the state finishes |

> 💡 One event may trigger multiple transitions if **mutually exclusive conditions** are specified for each.

**Sample actions:**

| Type | Syntax | Effect |
|------|--------|--------|
| Assignment | `var := expr` | Assign value |
| Call | `opName(args)` | Invoke operation |
| Create | `create ClassName(args)` | Create object |
| Destroy | `destroy()` | Destroy object |
| Send | `signal_name(args)` | Send signal |
| Return | `return value` | Return value |

---

### 14.5 Node Types

| Node | Description |
|------|-------------|
| **Simple state** | No substructure |
| **Sequential composite** | Contains sub-states; only one active at a time |
| **Concurrent composite** | Two or more orthogonal regions active simultaneously |
| **Initial pseudostate** | Filled circle `●`; marks starting state |
| **Final pseudostate** | Circle with dot `⊙`; marks end state |
| **Termination** | Object is destroyed |
| **Decision node** | Forks one transition into several with disjoint conditions |
| **Merge node** | Merges multiple alternatives into one |
| **Fork node** | Splits into concurrent transitions |
| **Join node** | Synchronises concurrent transitions back into one |
| **Junction** | Merges several transitions and forks into new ones |
| **Shallow history (H)** | Restores the last active sub-state |
| **Deep history (H*)** | Restores full nested sub-state history |
| **Entry/exit points** | Named entry or exit on composite state border |
| **Signal receipt** | Receives an incoming signal |
| **Signal sending** | Sends a signal |

---

### 14.6 Composite States

**Sequential composite:** only one sub-state active at a time. Transitions to the composite state enter via initial or named entry point.

**Concurrent composite:** the state is divided into **orthogonal regions** separated by dashed lines, all active simultaneously. Used when:
- Object is an aggregate of independent parts
- Some operations can run concurrently
- Objects act asynchronously

**History state:** When re-entering a composite state, `H` restores the last active sub-state (shallow) or `H*` restores the full sub-state tree (deep).

---

### 14.7 Protocol vs. Behavioral State Machine

| | Protocol | Behavioral |
|--|--|--|
| Focus | Valid sequences of operations for a classifier | Internal behaviour during execution |
| Transition format | `[pre-condition] operation / [post-condition]` | `event [guard] / action` |
| Internal operations | Not specified | Fully specified |
| Use | Declarative API contract | Executable logic (can generate code) |

---

## 15. Interaction Diagrams

Interaction diagrams show how **structural elements cooperate** during execution of a task through **message exchange**. They are used for code generation in CASE tools.

### 15.1 Types

| Type | Header | Focus |
|------|--------|-------|
| **Communication diagram** | `cd Name` | Object links and numbered message flow |
| **Sequence diagram** | `sd Name` | Time-ordered messages on vertical lifelines |
| **Timing diagram** | — | State/value changes of classifiers over time |
| **Interaction overview diagram** | — | Control flow across multiple interaction diagrams |

---

### 15.2 Message Syntax

```
[predecessors] [sequence_expression] : operation_signature
```

- **Predecessors:** e.g. `2.1/` — message only sent after message 2.1 completes
- **Sequence expression:** e.g. `3.1`, `3.1.2` (nested numbering: prefix from calling message)
- **Condition:** `[grade > 4]`
- **Iteration:** `*` (loop) or `* [i = 1..10]` (bounded loop)
- **Returned value:** `result = opName() : Type`

**Nested numbering rule:** if object O receives message `7.3`, all messages O sends in response are numbered `7.3.1`, `7.3.2`, etc.

---

### 15.3 Message Types

| Type | Meaning |
|------|---------|
| **Synchronous** | Sender waits for return of control (filled arrowhead) |
| **Asynchronous** | Sender continues immediately (open arrowhead) |
| **Return** | Return of control to sender (dashed arrow) |
| **Lost** | Target not yet identified |
| **Found** | Sender not yet identified |
| **Balking** | Dropped if target not immediately ready |
| **Timeout** | Dropped after waiting period expires |

---

### 15.4 Communication Diagrams

Objects are shown as boxes connected by **links** (corresponding to class diagram associations). Messages are numbered and placed on the links.

```
:Employee ─── borrow(title) ──→ :Customer
                                    │ 1: is_possible
                                    │ 2: is_in_inventory ──→ :Book
                                    │ 3: is_available ──────→ :BookSpecimen
                                    │ 4: register ──────────→ :BookSpecimen
```

**Multiple objects:** `*` before the class name — the message is sent to **all** instances of that class.

**Active classifiers** (double-bordered box) can initiate threads, send messages at specified times, or perform calculations using other classifiers' data.

---

### 15.5 Sequence Diagrams

Each participant has a vertical **lifeline**. Time progresses downward.

```
:Employee      :Customer     :Book      :BookSpecimen
     │               │          │              │
     │──borrow(title)→│          │              │
     │               │─1:can_borrow?──────────→│
     │               │          │─2:is_available?→│
     │               │          │         │─2.1:register_lease─→│
```

- **Active object** bar (thick rectangle on lifeline) = object is executing
- `«create»` / `«destroy»` marks object creation and destruction
- Time constraints shown as `{b - a < 1 sec.}` between events

---

### 15.6 Combined Fragments (UML 2.0+)

Sequence diagrams use **combined fragment** boxes labelled with an operator:

| Operator | Meaning |
|----------|---------|
| `alt` | if–then–else (mutually exclusive conditions) |
| `opt` | Optional block (executes only if condition is true) |
| `loop` | Repeated sequence |
| `break` | Alternative executed instead of the rest of the diagram when condition met |
| `par` | Concurrent execution |
| `seq` | Weak sequencing (all preceding messages complete before next segment) |
| `strict` | Messages must execute in exact order shown |
| `neg` | Invalid message sequence |
| `critical` | Critical section (no interleaving) |
| `ignore` | Named messages are irrelevant in this context |
| `consider` | Only named messages are relevant |
| `assert` | Any sequence not shown is invalid |

**Gates:** connection points linking messages inside a fragment to messages outside it (used as off-page connectors).

**State invariants:** constraints on a lifeline that must hold at runtime; shown as a rectangle with semicircular ends.

---

### 15.7 Timing Diagrams

Show **state or value changes of a classifier over time**:
- **State lifeline:** Y-axis = list of states; X-axis = time
- **Value lifeline:** shows when a value changes (horizontal lines that cross at each change)

---

### 15.8 Interaction Overview Diagrams

A specialised **activity diagram** where nodes are interaction diagrams (either inline or referenced). Used to show control flow across a group of interaction scenarios.

---

## 16. Implementation & Package Diagrams

### 16.1 Component Diagrams

A **component** is a well-isolated, reusable implementation unit with defined interfaces.

```
┌──────────────────────┐
│  «component»         │
│  Orders              │──○ IOrder        (provided interface)
│                      │──( IInvoice      (required interface)
└──────────────────────┘
```

**Component stereotypes:**

| Stereotype | Contents |
|------------|---------|
| `«subsystem»` | A subsystem |
| `«executable»` | Executable code |
| `«library»` | Software library |
| `«table»` | Database table(s) |

**Specification views:**
- **Black box** (external): only interfaces and operations shown
- **White box** (internal): classes implementing the component are shown in a `«realizations»` section

**Ports and connectors:**
- **Port** — interface-bound element for external communication
- **Complex/composite port** — bound to more than one interface
- **Connector** — connects diagram elements
- **`«delegate»` connector** — routes external port traffic to the internal class handling it

---

### 16.2 Artifacts

An **artifact** is any product of the software lifecycle (document, diagram, executable, script). Components are a type of artifact.

Common artifact stereotypes: `«document»`, `«file»`, `«source»`, `«script»`

**Manifest:** a stereotyped dependency (`«manifest»`) showing how an artifact exposes its contents.

---

### 16.3 Deployment Diagrams

Show the **runtime configuration** of a system: hardware, platforms, software and artifacts deployed on nodes.

```
┌─────────────────────┐          ┌───────────────────┐
│  «server»           │          │  «PC»             │
│  Scheduling Server  │──TCP/IP──│  User's Computer  │
│  ┌─────────────┐    │          └───────────────────┘
│  │«component»  │    │
│  │ Schedules   │    │
│  └─────────────┘    │
└─────────────────────┘
```

Artifacts/components can be connected to nodes using a `«deploy»` dependency.

---

### 16.4 Package Diagrams

A **package** groups model elements (classes, state machines, etc.) and manages their dependencies.

- Each element belongs to exactly one **home package**.
- Packages can be nested.
- This is a **logical** model, not an implementation model.
- Package diagrams become essential in large projects.

Header: `pd DiagramName`

**Visibility in packages:**

| | Rule |
|--|--|
| Within a package | All elements are visible to each other |
| Package A depends on B | All **public** elements of B are visible in A |
| A nested in B | All elements visible to B are visible to A |

**Reference dependencies:**

| Dependency | Effect |
|------------|--------|
| `«import»` | Public import — names added to importing package's namespace |
| `«access»` | Private import — names must be qualified with package name |
| `«merge»` | Package Q adds contents of P to its own elements (like specialisation but with merge rules) |

**Special package stereotypes:**

| Stereotype | Purpose |
|------------|---------|
| `«facade»` | Contains only calls to elements in other packages |
| `«framework»` | Design pattern for a class of problems |
| `«modelLibrary»` | Reusable model elements |
| `«model»` | Abstraction of the system from one viewpoint |
| `«subsystem»` | Logically isolated, consistent part of system with defined interfaces |

---

## 17. Other OO Models

### 17.1 Prototypes

An alternative to class-based OO, closely related to **delegation**.

- Any object can become a **prototype** (a pattern for new objects).
- Two interpretations:
  1. Prototype used as a template **at object creation time only**
  2. Information from prototype available **dynamically at runtime** (changes propagate)
- Links between objects and their prototypes are **transitive**.
- Can model classes, inheritance, and multiple inheritance without formal class syntax.
- Used in **JavaScript** (pre-ES6 classes) — objects inherit directly from other objects via the prototype chain.

**Pros:** Easy, flexible, powerful.
**Cons:** Can be hard to reason about; ability to add attributes/methods dynamically increases risk of errors.

---

### 17.2 Open Classes (Monkey-patching)

Some OO languages (typically interpreted) allow adding **new properties to classes at runtime**:

- Adding new methods to a class = **monkey-patching**.
- Some languages allow patching individual objects (implemented via dynamic class creation).
- **C# Extension Methods** — visible only to packages that introduce and declare use of the extension (more controlled).

**Pros:** Powerful meta-programming; very flexible.
**Cons:** Easy to obfuscate code; errors are hard to catch statically.

---

### 17.3 Traits

Introduced in **Scala** (OO-functional, JVM-based). Similar to interfaces but with additional capabilities:

- Can contain **method implementations** (like Java 8 default methods)
- Can contain **attributes**
- Can be **dynamically added** to objects at creation time
- Partial solution to the multi-inheritance problem
- Order of trait mixing matters — requires careful planning
- Not widely adopted outside Scala

---

### 17.4 Component-Entity Systems (CES / ECS)

An architectural model that **replaces inheritance hierarchies with composition**. Widely used in game programming.

Three elements:

| Element | Role |
|---------|------|
| **Entity** | A bare identity (often just an ID); no data or logic |
| **Component** | Pure data container (e.g. Position, Health, Sprite) |
| **System** | Logic that operates on all entities possessing specific components |

**Key properties:**
- Systems communicate **only via events** — no direct coupling between systems.
- Easy to add, remove, or swap components on entities at runtime.
- Events can be sent over network; replays achieved by recording events with timestamps.

**Advantages:** Eliminates deep/rigid inheritance hierarchies (diamond problem, blob antipattern); highly flexible; code reuse without inheritance.

**Challenges:** Requires careful upfront design of components and systems; not using inheritance means forgoing some OO benefits.

---

## 18. Design Patterns

A **design pattern** is a general, reusable solution to a commonly occurring problem in software design. They deal with **design-time** problems (as opposed to architectural patterns which deal with structure).

> *Origin:* Christopher Alexander (architecture, 1977/79) → Kent Beck & Ward Cunningham (programming, 1987) → "Design Patterns" book (Gang of Four, 1994).

### 18.1 Singleton

**Goal:** Ensure a class has **only one instance** and provide a global access point to it.

```
┌──────────────────────────────┐
│         Singleton            │
├──────────────────────────────┤
│ -instance: Singleton         │
├──────────────────────────────┤
│ -Singleton()                 │
│ +getInstance(): Singleton    │
└──────────────────────────────┘
```

**Use when:** Exactly one instance is required (e.g. thread manager, memory manager, logger).

---

### 18.2 Abstract Factory

**Goal:** Provide an interface for creating **families of related objects** without specifying their concrete classes.

**Participants:**
- `AbstractFactory` — declares creation operations for abstract products
- `ConcreteFactory` — implements creation for a specific product family
- `AbstractProduct` — interface for a product type
- `Product` — concrete product created by a concrete factory
- `Client` — uses only abstract factory and product interfaces

**Use when:** System should be independent of how its products are created; system needs to support multiple product families (e.g. a portable GUI library supporting multiple OS themes).

---

### 18.3 Mediator

**Goal:** Define an object that **encapsulates how a set of objects interact**, promoting loose coupling.

**Participants:**
- `Mediator` — interface for communicating with Colleague objects
- `ConcreteMediator` — coordinates colleagues; knows and maintains all colleagues
- `Colleague` — knows its mediator; communicates through it rather than directly with peers

**Use when:** Many objects communicate in complex ways, creating tight coupling.

---

### 18.4 Observer

**Goal:** Define a **one-to-many dependency** so that when one object changes state, all dependents are notified and updated automatically.

**Participants:**
- `Subject` — knows its observers; provides attach/detach interface
- `ConcreteSubject` — stores state; notifies observers on change
- `Observer` — defines update interface
- `ConcreteObserver` — maintains reference to subject; implements update to stay consistent

**Use when:** Change in one object requires changing others and you don't know how many objects need to change.

---

### 18.5 State

**Goal:** Allow an object to **alter its behaviour when its internal state changes** — the object appears to change its class.

**Participants:**
- `Context` — defines interface for clients; holds current state instance
- `State` — interface for state-specific behaviour
- `ConcreteState` — implements behaviour for one state

**Use when:** Object behaviour depends heavily on its state and must change at runtime.

---

### 18.6 Strategy

**Goal:** Define a **family of algorithms**, encapsulate each one, and make them **interchangeable** independently of the clients that use them.

**Participants:**
- `Strategy` — common interface for all algorithms
- `ConcreteStrategy` — implements one algorithm
- `Context` — configured with a ConcreteStrategy; delegates the algorithm to it

**SOLID connection:** Strategy pattern is the canonical illustration of all five SOLID principles (see Section 21).

**Use when:** Multiple variants of an algorithm exist and clients should be unaware of which is used.

> 📖 **Reading:** Gamma, Helm, Johnson, Vlissides — *Design Patterns: Elements of Reusable Object-Oriented Software* (the "Gang of Four" book).

---

## 19. Architectural Patterns

Architectural patterns deal with **system-level structure** (unlike design patterns which address class/object design).

### 19.1 Application Structure Patterns

| Pattern | Description |
|---------|-------------|
| **Monolithic** | Single-tiered; combines UI and data storage. Most traditional desktop apps. |
| **Layered** | Application split into layers, each using abstractions of the layer below (e.g. UI → Business Logic → Data Access → DB). |
| **Component-based** | Divided into substitutable components, each responsible for a set of functionalities. |
| **Pipeline** | Processing split into stages (components) chained into a pipeline. Common in OS and data processing. |
| **Data-centric** | Multiple apps sharing one database; business logic often in stored procedures. |

---

### 19.2 Messaging Patterns

| Pattern | Description |
|---------|-------------|
| **Event-driven** | Loosely coupled components that raise/consume asynchronous events. Specific consumers listen to specific emitters. |
| **Publish-subscribe** | Messages divided into classes/topics; consumers subscribe to topics. Multiple consumers can receive a single message from a topic. Usually involves a broker. |
| **Message queue** | Similar to pub-sub but messages are consumed once from a queue (point-to-point). |

---

### 19.3 Adaptability Patterns

| Pattern | Description |
|---------|-------------|
| **Plug-in architecture** | Host application provides services and registration/data-exchange protocols; plug-ins extend functionality. |
| **Scripting** | Application embeds an interpreter; scripts interact via APIs. Level of control varies. |
| **Domain Specific Languages (DSL)** | Specialised language for a domain (not necessarily a scripting language — e.g. HTML). |

---

### 19.4 Distribution Patterns

| Pattern | Description |
|---------|-------------|
| **2-tier** | Logic, UI and data across two tiers (usually thick client + database server). |
| **3+ tier** | Typical split: Tier 1 = data, Tier 2 = business logic, Tier 3 = UI. Additional tiers add abstraction levels. |
| **Shared Nothing** | Each node is independent and self-sufficient; no shared memory or disk. Adding nodes improves capacity (horizontal scaling). DB may be sharded. |
| **Broker** | Mediator service managing communication between client and service; helps locate/identify required services. |
| **Peer-to-peer** | Self-organising network; all nodes have the same role. High resilience but very hard to manage. |
| **Service-oriented (SOA)** | Logic split into small, independent (web) services; applications composed by combining services, often via a workflow engine. |
| **REST** | Representational State Transfer; uses web standards; application as a network of information; stateless (connection-wise); no encapsulation. |

---

## 20. Entity Component Systems & Data Oriented Programming

### 20.1 Why ECS? Inheritance Problems

Classic OO inheritance can lead to:

| Problem | Description |
|---------|-------------|
| **Diamond problem** | Ambiguous attribute/method resolution in multi-inheritance |
| **Deep rigid hierarchy** | Hard to maintain and extend; change ripples through many levels |
| **Blob antipattern** | One huge root class accumulates all functionality |

ECS replaces inheritance with **composition**: entities are assembled from components at runtime.

### 20.2 ECS in Detail

- **Component** = pure data (e.g. `Position{x, y}`, `Health{hp}`, `Sprite{img}`). No logic.
- **Entity** = just an identifier linking a set of components. Some practitioners advocate removing entities entirely and using only components + systems.
- **System** = logic that queries all entities with a specific set of components and processes them.

**Communication:** systems interact **only via events** — no direct coupling. This means:
- Easy to add/remove systems at any time
- Events can be sent over a network
- AI can create events processed identically to player input
- Replays: record events + timestamps

### 20.3 Data Oriented Programming (DOP)

Optimises for **CPU cache efficiency** by organising data layout to match access patterns.

Key insight: when a value is read from memory, neighbouring data is pre-fetched into cache. A **cache miss** (data not in cache) is orders of magnitude slower than a cache hit.

**Problem with OOP memory layout:** objects with many fields are stored with all fields together. When a system only needs one field from many objects, most cached data is wasted ("swiss-cheese memory").

**DOP solution:** store data in **arrays of components** (Struct of Arrays) rather than arrays of objects (Array of Structs). All positions are stored together, all health values together, etc.

**Why ECS and DOP work well together:** ECS already separates components by type, naturally producing the memory layout DOP requires.

### 20.4 ECS and DOP — Trade-offs

| Pros | Cons |
|------|------|
| Eliminates deep inheritance problems | Requires careful upfront component design |
| Flexible — components added/removed at runtime | Forfeits OO encapsulation benefits |
| Cache-friendly (with DOP) | Less intuitive for developers trained in OOP |
| Decoupled via events | Can be overkill for small systems |

---

## 21. Domain Driven Design & SOLID

### 21.1 Domain Driven Design (DDD)

**Goal:** Simplify domain complexity through a ubiquitous language, focused system boundaries, and core domain logic.

**Core principles:**
- Align software model with the **business domain**
- Use **iterative/agile** development with domain experts
- Apply **prototyping** and **event storming** to discover the model
- Keep business logic in the **core domain**

---

#### Bounded Contexts

The system is divided into **bounded contexts** — areas with their own language, model and (optionally) independent infrastructure.

- Each context maps to a specific business problem area.
- Relationships between contexts are documented in a **context map**.

**Shared Kernel:** a small shared part of the model used by multiple contexts. Changes require all involved teams. Should be minimised.

---

#### DDD Building Blocks

| Concept | Description |
|---------|-------------|
| **Entity** | Object with a unique identity; identity persists even when attributes change; should carry business logic |
| **Value Object** | No identity; two value objects with same attributes are equal and interchangeable; **immutable** (e.g. Money, Address) |
| **Aggregate Root** | Cluster of entities and value objects; only the root has a global identity; external objects hold references only to the root; internal objects cannot be changed from outside |
| **Factory** | Encapsulates the creation logic for complex aggregates |
| **Repository** | Abstracts persistence; provides collection-like interface for aggregates |
| **Module** | Groups related domain elements; corresponds to packages in implementation |

**CQRS (Command Query Responsibility Segregation):** separates read operations (queries) from write operations (commands). Useful when read and write models have different requirements.

---

### 21.2 SOLID Principles

Five principles of good OO design, each illustrated by the **Strategy** design pattern:

#### S — Single Responsibility Principle
> *"There should never be more than one reason for a class to change."*

A class should do **one thing only**. The Strategy pattern separates the responsibility for a concrete algorithm from the context that uses it.

#### O — Open/Closed Principle
> *"Software entities should be open for extension but closed for modification."*

Change behaviour via **inheritance and composition**, not by modifying existing code. The Strategy pattern lets you add new strategies without touching the context.

#### L — Liskov Substitution Principle
> *"Functions using references to base classes must work correctly with objects of derived classes."*

Subclasses should **behave correctly** when used in place of their base class. Strategies must be substitutable for one another without problems.

#### I — Interface Segregation Principle
> *"Clients should not be forced to depend on interfaces they do not use."*

Keep interfaces **small and focused**. Each Strategy interface should only expose methods the context actually needs.

#### D — Dependency Inversion Principle
> *"High-level modules should not depend on low-level modules. Both should depend on abstractions."*

Use interfaces and abstractions. Both the Context and ConcreteStrategies depend on the abstract `Strategy` interface — neither depends on the other's concrete implementation.

---

## 22. Wrong Assumptions in Software Design

Lecture content: a catalogue of real-world edge cases that invalidate common programmer assumptions. Essential reading before modelling any domain.

### 22.1 Prices and Currencies

- **Don't store prices as floats** — use integer cents or a decimal type to avoid rounding errors.
- Not all currencies subdivide into 100 units (e.g. Mauritania's ouguiya = 5 khoums).
- Some currencies have **no subdivisions** (KRW, JPY).
- Currency symbols are **not unique** globally (`$` is used by USA, Canada, Australia, and others; `¥` is both Yen and Yuan).
- The **position** of the currency symbol varies by locale (before, after, or even in the middle).
- A product may have **many prices**: list price, sale price, tax-exclusive, tax-inclusive, subscription-hidden "free" price, etc.
- Prices can be **negative or zero** (free items, samples, returns).
- Swiss franc must be rounded to **five centimes** (0.05), not two decimal places.

### 22.2 People's Names

- **Order is not universal** — Chinese names have family name first; some Indian names encode village, father's name, given name, and caste in sequence.
- **Multiple family names** — Spanish speakers often have two; Brazilians may have three or four.
- **Patronymics** — Icelandic names include father's name with `-sson`/`-sdóttir` suffix; not a shared family name.
- **Name length** — names are not globally unique; millions of people may share a name.
- **Character sets** — names exist in non-ASCII, non-Latin scripts; some Japanese names use archaic characters not in modern Unicode.
- **Honorifics matter** — in Germany `Herr Professor Doktor Schmidt`; in Japan `-san` or `-bucho` are expected.
- **Reserved words** — a real person has the last name "Null," which breaks systems that treat it as a null value.

### 22.3 Addresses

- **No standard format** — some addresses have no street name (just hamlet); some have both a name and a number; some have multiple numbers (flat + floor + building + street).
- **Building numbers are not always numeric** — `10A/12`, letters, suffixes, dual historic numbering systems (Prague).
- **Zero and negative house numbers** exist.
- **Street names repeat** in large cities (São Paulo has multiple streets named Rua Santa Catarina).
- **Place names are ambiguous** — a hill can appear as "Äntlisberg" on military maps and "Entlisberg" on city maps; both official.
- **Addresses can change** — administrative divisions change, streets get renamed, houseboats can move between countries.
- Addresses can be **very long** — include department names, floor numbers, suite numbers; do not assume < 100 characters.

### 22.4 Time

- The **Year 2038 problem** — Unix timestamps stored as 32-bit signed integers overflow on 19 January 2038.
- **Timestamps have varying precision** — seconds, milliseconds, microseconds, nanoseconds depending on system.
- A precise timestamp is **not guaranteed to be unique** — two machines may record the same moment.

### 22.5 Networks

- Networks are **not** reliable, zero-latency, infinite-bandwidth, secure, homogeneous, or unchanging in topology.
- DHCP addresses change. TCP checksums detect but do not prevent all corruption. Encryption prevents reading but not tampering (without authentication).
- The **Fallacies of Distributed Computing** (Deutsch): reliability, zero latency, infinite bandwidth, secure network, fixed topology, single administrator, zero transport cost, homogeneous network.

> 💡 **Design implication:** model your domain objects (prices, names, addresses, timestamps) to accommodate real-world complexity from the start. Retrofitting is significantly more costly.

---

*Guide compiled from Lectures 3–9, 11–14, Tutorial 7, and supplementary materials of the Object Oriented Design course.*
