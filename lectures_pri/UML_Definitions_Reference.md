# UML & Object Oriented Design — Complete Definitions Reference
> For use with Task II exam questions.  
> Each entry: **one-line definition** → expanded explanation → examples.

---

## Table of Contents

1. [Core OO Concepts](#1-core-oo-concepts)
2. [UML Class Diagram Elements](#2-uml-class-diagram-elements)
3. [Associations & Relationships](#3-associations--relationships)
4. [Inheritance & Generalization](#4-inheritance--generalization)
5. [UML Extension Mechanisms](#5-uml-extension-mechanisms)
6. [Use Case Diagrams](#6-use-case-diagrams)
7. [Statechart Diagrams](#7-statechart-diagrams)
8. [Interaction Diagrams](#8-interaction-diagrams)
9. [Activity Diagrams](#9-activity-diagrams)
10. [Package Diagrams](#10-package-diagrams)
11. [Implementation Diagrams](#11-implementation-diagrams)
12. [Other OO Models](#12-other-oo-models)
13. [Design Patterns](#13-design-patterns)
14. [Architectural Patterns](#14-architectural-patterns)
15. [SOLID Principles](#15-solid-principles)
16. [Domain Driven Design](#16-domain-driven-design)
17. [Wrong Assumptions in Software Design](#17-wrong-assumptions-in-software-design)
18. [Complete Notation Reference](#18-complete-notation-reference)
19. [Exam Quick-Reference](#19-exam-quick-reference)

---

## 1. Core OO Concepts

### Object
**One-line:** A data structure with associated operations representing a real-world entity.

An object has three key properties:
- **Identity** — a unique, persistent internal identifier that separates it from all other objects, regardless of whether attribute values are identical to another's.
- **State** — the current set of attribute values and associations with other objects.
- **Behaviour** — the set of operations that can be performed on it.

> Two objects may have identical attribute values yet remain two separate entities because their identity is distinct.

---

### Class
**One-line:** A named description of a group of objects sharing a set of invariants (attributes, methods, constraints).

A class is **not** a set of objects — it *describes* objects. The class stores method code and type information once; each instance stores only its own attribute values. Class invariants include: name, type, methods, constraints, and export list.

---

### Object Identity
**One-line:** A unique internal identifier assigned to an object at creation that persists for the object's entire lifetime.

Identity is independent of attribute values. Two customers named "Jan Kowalski" with the same address are still two separate objects. Identifiers can be **transient** (session-only) or **persistent** (survive program restarts).

---

### Encapsulation
**One-line:** Hiding internal structure and implementation behind a well-defined interface; separating specification from implementation.

> *"Everything that can be hidden, should be hidden."* — Parnas, 1972

Two approaches:
- **Orthodox (Smalltalk):** only methods are visible; all attributes are hidden.
- **Orthogonal (C++):** any property can be individually declared public or private.

---

### Polymorphism
**One-line:** A single operation name having different implementations across classes in a hierarchy.

A `print()` operation can have three methods: ASCII, PostScript, Image. All share the same name but behave differently. Polymorphism relies on **late (dynamic) binding** to decide at runtime which implementation to invoke.

Types:
- Polymorphic types (e.g. in ML)
- Polymorphic methods: same name, different implementation per class
- Parametric polymorphism: parametrised types (generics)

---

### Operation vs Method
**One-line:** An operation is the **what** (abstract specification); a method is the **how** (one concrete implementation in one class).

One `print` operation may be realised by three methods: ASCII, PostScript, image. The operation is defined in the class; the method is the specific code that executes.

---

### Early Binding (Static Binding)
**One-line:** Name-to-address resolution at **compile/link time**.

- ✅ Better performance, full static type-checking
- ❌ Less flexible — no plug-in support, no polymorphism

---

### Late Binding (Dynamic Binding)
**One-line:** Name-to-address resolution at **runtime**.

The actual method called is determined when the program runs, based on the runtime type of the object. Required for polymorphism, method overriding, dynamic views, query languages, and object migration.

---

### Method Overriding
**One-line:** A subclass provides its own implementation of a method that already exists (with the same name and signature) in the superclass.

Requires late binding. When the method is called on a subclass instance, the subclass version executes.

> **Example:** `Employee.fire()` is overridden by `Manager.fire()` to add approval steps.

---

### Method Overloading
**One-line:** The same method name is defined multiple times in one class, each with a different number or type of parameters.

The compiler/runtime selects which version to call based on the argument list.

> **Example:** `move(x, y)` for 2D and `move(x, y, z)` for 3D.

> ⚠️ Some authors (e.g. Cardelli) argue overloading is **not** a form of polymorphism.

---

### Class Extent
**One-line:** The current set of all instances of a given class.

| Level | Includes |
|-------|---------|
| I | Direct instances only (excluding subclasses) |
| II | All instances (direct + indirect), limited to this class's attributes |
| III | All instances (direct + indirect) with all their attributes |

---

## 2. UML Class Diagram Elements

### UML Class Diagram
**One-line:** A structural diagram showing classes, their attributes and methods, and the static relationships between them.

Represents the **static structure** of a system — the blueprint showing what classes exist and how they connect. Does **not** show runtime behaviour or the order of operations.

---

### Class Box
**One-line:** A rectangle divided into compartments showing the class name, attributes, methods, and optionally custom information.

```
┌───────────────────────────────┐
│  «stereotype»  ClassName      │  ← compartment 1: name [MANDATORY]
├───────────────────────────────┤
│  - attribute : Type           │  ← compartment 2: attributes
├───────────────────────────────┤
│  + method() : ReturnType      │  ← compartment 3: methods
├───────────────────────────────┤
│  custom info                  │  ← compartment 4: optional
└───────────────────────────────┘
```

The class name is the only mandatory element. By convention it is a **singular noun in PascalCase**. Abstract class names are written in *italics* or tagged `{abstract}`.

---

### Attribute
**One-line:** A named property of a class that holds data for each instance.

**Full syntax:** `«stereotype» visibility name : type [multiplicity] = default {constraint}`

| Visibility | Symbol |
|------------|--------|
| public | `+` |
| private | `-` |
| protected | `#` |
| package | `~` |

```
- dateOfBirth : Date
- hobbies[0..5] : String       ← list, up to 5 entries
- contact[0..1] : String       ← optional
/age : Integer                  ← derived (computed, not stored)
```

---

### Method (Operation)
**One-line:** A named operation that a class can perform, specified with parameters and return type.

**Full syntax:** `«stereotype» visibility name(param: Type) : ReturnType {constraint}`

```
+ calculateBonus(month: Integer) : Float
{abstract} + calcAge() : Integer          ← abstract method
```

---

### Abstract Class
**One-line:** A class that **cannot be instantiated**; it defines shared properties for a group of related classes.

Marked with `{abstract}` tag or *italic* name. May contain:
- ✅ Abstract methods (declared but not implemented — subclasses must implement)
- ✅ Implemented (concrete) methods (with actual code)
- ✅ Attributes

> You never create objects directly from an abstract class. `Person{abstract}` — you only create `Attendee` or `Guest` objects, never a bare `Person`.

---

### Concrete Class
**One-line:** A class that **can be instantiated** — actual objects are created from it.

Must implement **all** abstract methods inherited from any abstract ancestor. Cannot contain abstract methods.

> **Examples:** `Attendee`, `Guest` (subclasses of `Person{abstract}`)

---

### Abstract Method
**One-line:** A method declared in an abstract class with **no implementation**; every concrete subclass must provide its own implementation.

Written in *italics* or with `{abstract}` tag. Signals that the operation exists but each subclass computes it differently.

> **Example:** `{abstract} + calcBonus() : Float` in `Pirate{abstract}` — `Manager` and `Enforcer` each provide their own `calcBonus()`.

---

### Object Method (Instance Method)
**One-line:** A method that operates on **one specific instance**, using and modifying that instance's own attribute values.

Written as **normal text** in UML (not underlined). Called on an object: `myAttendee.calAge()`. Uses `this` (the calling object's data).

**Decision rule:** if the method needs THIS object's data → object method.

> **Example:** `calAge()` on `Attendee` — reads `this.dateOfBirth` to compute this object's age.

---

### Class Method (Static Method)
**One-line:** A method that operates on the **class as a whole**, not on any individual instance; it cannot access instance attributes.

Written **underlined** in UML. Called on the class: `Convention.makeRanking()`. Has no `this` reference. Used for: rankings, counts, factory methods, global queries.

**Decision rule:** if the method queries or aggregates **all** objects of the class → class method.

> **Example:** `makeRankingOfConventions()` on `Convention` — sorts ALL conventions; there is no single "this" convention.

---

### Object Attribute (Instance Attribute)
**One-line:** An attribute for which each instance of the class holds its own **independent copy** with its own value.

The **default** for all attributes in UML (not underlined). Two `Convention` objects each have their own separate `name`, `startDate`, `ticketPrice`, etc.

> **Example:** `- name : String` on `Convention` — each convention has its own name.

---

### Class Attribute (Static Attribute)
**One-line:** An attribute for which there is only **one copy shared by all instances** of the class.

Written **underlined** in UML. Useful for constants, counters, system-wide rules. Changing it changes it for all instances simultaneously.

**Decision rule:** "minimum value / rule for all / constant" → class attribute.

> **Example:** `minAttendees : Integer = 10` on `LARP_Game` — the rule applies to every LARP game; storing it per-instance would waste memory and risk inconsistency.

---

### Derived Attribute
**One-line:** An attribute whose value is always **computed** from other stored data and is therefore **not stored** independently.

Marked with `/` prefix. Storing it would create redundancy.

> **Examples:**
> - `/age` on `Attendee` — computable from `dateOfBirth` and today's date
> - `/sumOfSalaries` on `PirateCrew` — computable from all `Pirate` salaries
> - `/totalProfit` on `Investment` — computable from `profit[0..5]` array

---

## 3. Associations & Relationships

### Association
**One-line:** A structural relationship between two classes indicating that instances of one class are connected to instances of the other.

Drawn as a line between classes. Always has **multiplicity at both ends**, optionally a **name** (verb phrase) with reading direction (`<` or `>`), and **role names**.

> **Example:** `Convention "1..*" ── <attend ── "0..*" Attendee`

---

### Multiplicity
**One-line:** A specification of how many instances on one end of an association can be linked to one instance on the other end.

| Notation | Meaning |
|----------|---------|
| `1` | Exactly one |
| `0..1` | Zero or one (optional) |
| `*` or `0..*` | Zero or more |
| `1..*` | One or more |
| `3..5` | Between three and five |
| `10..100` | Between ten and one hundred |
| `3..*` | At least three |

---

### Role Name
**One-line:** A label on one end of an association that describes the role played by the class at that end.

> **Example:** In `Employee ── works_in ── Company`, role names are `employee` and `employer`.

---

### Association Class
**One-line:** A class attached to an association line (by a dashed line) that carries attributes or methods belonging to the **relationship itself**, not either class.

Used when data describes the pair, not either individual object.

**Classic test:** "Can this attribute belong to Class A alone? To Class B alone?" If no to both — it belongs to the association.

> **Example:** `Attendance` between `Convention` and `Attendee` carries `numberedIdentifier`, which is unique *within* a convention — it belongs to the pair, not to either class alone.

> **Example:** `Enrollment` between `Student` and `Course` carries `startDate`, `endDate`, `completed`.

---

### Qualified Association
**One-line:** An association with a qualifier attribute that acts as a key, identifying a specific subset of objects at one end.

Drawn as a small rectangle on the association end near the qualifying class. Reduces effective multiplicity on the target end.

---

### N-Ary Association
**One-line:** An association linking objects from **more than two classes** simultaneously.

Drawn as a diamond with lines going to each class. For n > 2, aggregation, composition, qualifiers, and one-way navigation are **not** available.

> Best practice: convert to an association class connected to all participants by binary associations.

---

### Derived Association
**One-line:** A redundant association (marked with `/` prefix) that is always computable from other associations in the model.

> **Example:** `Department ─has─ Section ─employs─ Employee` and `/works_in` between `Department` and `Employee` — derivable from the other two.

---

### Aggregation (Weak Whole-Part)
**One-line:** A specialised association representing a whole-part relationship where parts can exist independently and can be shared.

Drawn with **open diamond (◇)** on the **whole** side.

- ✅ Parts **can** exist without the whole
- ✅ Parts **can** belong to multiple wholes simultaneously
- Multiplicity on whole side: any value

> **Example:** `Student ◇── Group` — a student can exist without a group; a student can belong to multiple groups.

---

### Composition (Strong Whole-Part)
**One-line:** A specialised aggregation where parts **cannot exist without the whole** and **cannot be shared** between wholes.

Drawn with **filled diamond (◆)** on the **whole** side.

- ❌ Parts **cannot** exist without their whole (destroyed with it)
- ❌ Parts **cannot** be shared between wholes
- Multiplicity on whole side: **always 1** (or `0..1` at most — never more)

> **Example:** `Room ◆── Hotel` — a room cannot exist without its hotel; destroying the hotel destroys its rooms.

---

### Aggregation vs Composition — Key Difference

| | Aggregation ◇ | Composition ◆ |
|--|--|--|
| Lifecycle | Loose — part survives whole | Strict — part dies with whole |
| Sharing | Allowed | Not allowed |
| Whole-side multiplicity | Any | Must be 1 (or 0..1) |
| Diamond | Open (hollow) | Filled |

---

## 4. Inheritance & Generalization

### Inheritance (Generalization-Specialization)
**One-line:** A relationship where a subclass acquires all invariants of its superclass and may add its own.

Drawn as a solid line with a **hollow triangle (▷)** pointing **to** the parent class. Inheritance is **transitive**: a subclass inherits from all its ancestors.

```
SubClass ──▷ SuperClass
```

---

### Generalization Set
**One-line:** A named group of inheritance relationships from one superclass to multiple subclasses, described by a pair of constraints.

Every generalization set **must** declare **both**:
1. `{disjoint}` OR `{overlapping}`
2. `{complete}` OR `{incomplete}`

---

### `{disjoint}`
**One-line:** Each object of the superclass belongs to **exactly one** subclass at a time.

> **Example:** `Event → {LARP_Game, Panel, Lecture}` is `{disjoint}` — an event cannot simultaneously be a LARP_Game AND a Panel.

---

### `{overlapping}`
**One-line:** An object of the superclass **may belong to multiple subclasses** simultaneously.

> **Example:** `Person → {Attendee, Guest}` is `{overlapping}` — the same person can be an invited Guest at one convention AND a paying Attendee at another.

---

### `{complete}`
**One-line:** **Every** object of the superclass must belong to at least one of the listed subclasses — all cases are identified.

> **Example:** `Pirate → {Manager, Enforcer}` is `{complete}` — the requirements say all pirates are either managers or enforcers; no other types exist.

---

### `{incomplete}`
**One-line:** Some objects of the superclass may **not** belong to any listed subclass — additional subclasses may exist in the future.

> **Example:** `Vehicle → {Car, Truck}` might be `{incomplete}` because Motorcycle, Bus, etc. could be added later.

---

### «dynamic»
**One-line:** An object **can migrate** between subclasses over its lifetime without losing its identity.

Marked as `«dynamic»` on the generalization set. Standard programming languages do **not** support this natively — in practice implemented with interfaces or state patterns.

> **Example:** `Customer → {Child, Teenager, Adult}` is `«dynamic»` — the same customer object ages through categories, retaining their order history.

---

### Overlapping Inheritance
**One-line:** A generalization set where an object can simultaneously be classified under two or more subclasses; marked with `{overlapping}`.

The constraint `{complete, overlapping}` is placed next to the fork of inheritance arrows.

> ⚠️ Common in the real world but **not directly supported** by most programming languages.

> **Example from Splattercon:** `Person {abstract} → {complete, overlapping} → Attendee, Guest`  
> A person who spoke at Splattercon 2024 (Guest) may also register as a paying Attendee at Splattercon 2025. The same `Person` object is simultaneously `Guest` and `Attendee`.

---

### Multi-Inheritance
**One-line:** A class that inherits from **two or more different parent classes**, potentially from different hierarchies.

Creates a new concrete class combining features of multiple parents.

**Differs from overlapping:** overlapping is about one object playing multiple roles within ONE hierarchy; multi-inheritance creates a new class FROM multiple classes.

> ⚠️ Problem: name conflicts if both parents define an attribute/method with the same name.

---

### Multi-Aspect Inheritance
**One-line:** A superclass specialized along **two or more independent dimensions**, each forming its own separate generalization set.

> **Example:** `Vehicle` specialized by fuel type (`{Electric, Petrol}`) AND by size (`{Small, Large}`) — two separate generalization sets. An `ElectricSmall` object inherits from both `ElectricVehicle` and `SmallVehicle`.

---

## 5. UML Extension Mechanisms

### Stereotype
**One-line:** A UML extension mechanism that **meta-classifies** a model element, assigning it to a special named category.

Written as `«stereotype_name»` (guillemets). Can be applied to classes, associations, actors, use cases, and other UML elements.

Rules:
- Each element may have **at most one** stereotype shown as an icon
- Predefined stereotypes exist in UML; users may define custom ones

---

#### «interface»
**Applied to:** class  
**Meaning:** contains **method specifications only** — no attributes, no implementation. All methods are implicitly public. Other classes **realize** it (implement all methods).

```
ConcreteClass ── - - ▷ «interface» InterfaceName
```

> **Key difference from abstract class:** an abstract class MAY have attributes and partial implementation; an interface has neither.

---

#### «persistent»
**Applied to:** class  
**Meaning:** instances are stored **permanently** (in a database or file system) and survive program restarts.

> Contrast with `«transient»` — exists only in memory during one session.

---

#### «history»
**Applied to:** association class  
**Meaning:** records of this association are **retained even after the relationship ends**. Used for audit trails.

> **Example:** `Safekeeping «history»` between `Pirate` and `Map` — even after a Pirate stops keeping a Map, the Safekeeping record is preserved for 2 years.

---

#### «system»
**Applied to:** actor (in use case diagrams)  
**Meaning:** the actor is a **non-human automated external system**, not a person.

---

#### «include» (use case)
**Applied to:** dependency between use cases  
**Meaning:** the base use case **always** triggers the included use case (mandatory sub-step). Arrow points FROM base TO included.

---

#### «extend» (use case)
**Applied to:** dependency between use cases  
**Meaning:** the extending use case **optionally** extends the base use case when a condition is met. Arrow points FROM extending TO base.

---

### Tagged Value
**One-line:** A key-value pair `{key = value}` that attaches **arbitrary metadata** to any UML model element.

Tagged values do **not** restrict the model — they **annotate** it.

**Format:** `{key = value}` or `{natural language note}`

| Example | Where | Meaning |
|---------|-------|---------|
| `{author = "Kowalski"}` | class | documentation metadata |
| `{= 100}` | `Wealth.value` | minimum value note |
| `{if age under 18}` | `Attendee.contact` | contextual note |
| `{yes, no}` | `Map.isMapTrue` | enumeration note |
| `{may change in future}` | `Wealth.minValue` | design note |

> **Difference from constraint:** a tagged value **describes**; a constraint **restricts**.

---

### Constraint
**One-line:** A condition or restriction that **must always be true** for the model to be valid; expressed in `{ }`.

Can be expressed in natural language, OCL (Object Constraint Language), mathematical formulas, or pseudocode.

| Type | Description | Example |
|------|-------------|---------|
| **Static** | Checked before a change; restricts the domain of values | `{salary <= 10000}` |
| **Dynamic** | Considers both previous AND new state; restricts the change | `{can change per year only by 10%}` |

**Predefined UML constraints:**

| Constraint | Meaning |
|------------|---------|
| `{complete}` | All subclasses identified; every superclass object in some subclass |
| `{incomplete}` | More subclasses may exist |
| `{disjoint}` | Object in exactly one subclass |
| `{overlapping}` | Object may be in multiple subclasses |
| `{ordered}` | Objects at this association end are ordered |
| `{bag}` | Duplicate objects at this end are allowed |
| `{subset}` | This association is a subset of another |
| `{or}` | At least one of two associations must be active |
| `{xor}` | Exactly one of two associations must be active |
| `{hierarchy}` | Instances form a tree (no cycles) |
| `{dag}` | Directed acyclic graph (no cycles but multiple parents allowed) |
| `{unique}` | Values must be distinct |

---

### Comment (Note)
**One-line:** A free-text annotation attached to a diagram element for human readers, with **no semantic effect** on the model.

Drawn as a rectangle with a **folded corner**, connected to the element by a dashed line. Does not restrict anything.

| | Constraint | Comment |
|--|--|--|
| Notation | `{ }` | dog-ear box + dashed line |
| Effect | Semantically **binding** — enforces a rule | Purely **informational** — no enforcement |

---

### `{subset}` Constraint
**One-line:** Specifies that one association is a subset of another — every object linked by the first must also be linked by the second.

Drawn as a dashed arrow between two associations labelled `{subset}`.

> **Example:** `"is coordinating" {subset} "is assigned to"` — a project coordinator must already be assigned to that project.

> **Example:** `"isTrue map" {subset} "leads to"` — the one true map must be one of the 3–5 maps leading to the treasure.

---

### Constraint vs Tagged Value vs Comment

| | Constraint | Tagged Value | Comment |
|--|--|--|--|
| Notation | `{rule}` | `{key=value}` | dog-ear box |
| Purpose | Restricts | Annotates | Explains |
| Enforced? | ✅ Yes | ❌ No | ❌ No |
| Example | `{disjoint}` | `{author="X"}` | *"This is temporary"* |

---

## 6. Use Case Diagrams

### Use Case
**One-line:** A description of a system function that delivers a result of value to an actor; represents **one goal** an actor achieves using the system.

Named as a verb phrase (e.g. "Print Brochure", "Pay For Ticket"). Drawn as an oval. All use cases go **inside** the system boundary rectangle.

---

### Actor
**One-line:** A person, organisation, or external system that interacts with the system from **outside** its boundary.

- Human actors: stick figures
- System actors: rectangle with `«system»` stereotype
- Actors are **outside** the system boundary
- Generalization between actors is allowed: `SubActor ──▷ SuperActor`

---

### System Boundary
**One-line:** A rectangle that encloses all use cases belonging to the system and separates them from external actors.

Everything **inside** = system functionality. Everything **outside** = external world (actors).

---

### «include» Relationship
**One-line:** The base use case **always** triggers the included use case as a mandatory sub-step; the included use case is not optional.

```
( Base UC ) ── «include» ──▷ ( Included UC )
```

> **Example:** `Register ── «include» ──▷ Verify Identity` (always verifies identity)

---

### «extend» Relationship
**One-line:** The extending use case **optionally** adds behaviour to the base use case when a specific condition is met.

```
( Optional UC ) ── «extend» ──▷ ( Base UC )   {extension point: condition}
```

> **Example:** `( Pay Online ) ── «extend» ──▷ ( Register )` `{if online payment chosen}`

---

### «include» vs «extend»

| | «include» | «extend» |
|--|--|--|
| Happens | Always (mandatory) | Optionally (conditional) |
| Arrow direction | Base → Included | Optional → Base |
| Extension point | No | Yes `{condition}` |

---

## 7. Statechart Diagrams

### State Machine
**One-line:** A model of an object's lifecycle as a directed graph where nodes are states and edges are transitions triggered by events.

| Type | Description |
|------|-------------|
| **Protocol** | Lifecycle of a single object; uses `{protocol}` tag |
| **Behavioral** | Describes system behaviour across objects |

---

### State
**One-line:** A condition of an object during which it waits for an event or performs an ongoing activity.

```
┌──────────────────────────────┐
│          State Name          │
├──────────────────────────────┤
│  entry / action              │  ← executed on entering
│  do    / activity            │  ← executed while in state (interruptible)
│  exit  / action              │  ← executed on leaving
├──────────────────────────────┤
│  event(attrs) / action       │  ← internal event handling
└──────────────────────────────┘
```

---

### Action
**One-line:** An operation executed during a transition that **cannot be interrupted**.

---

### Activity
**One-line:** An operation that **can be interrupted** (usually a reference to another statechart); executed while the object remains in a state.

---

### Transition
**One-line:** A change from one state to another, described by: `event [condition] / action`

| Type | Description |
|------|-------------|
| **External** | Standard transition between different states |
| **Internal** | Handled inside a state without exit/entry; state does not change |
| **Self-transition** | Exits and re-enters the same state (entry/exit actions fire) |
| **Completion** | Automatic; fires when all current-state processing finishes |

---

### Event Types

| Type | Syntax | Description |
|------|--------|-------------|
| Call event | `op(a: T)` | Synchronous operation call; most common |
| Change event | `when(expression)` | Boolean condition becomes true |
| Signal | `signalName(a: T)` | Asynchronous, usually from outside |
| Time event | `after(5 sec.)` | Passage of time |

---

### Composite State — Sequential
**One-line:** A state containing nested sub-states where **only one** sub-state is active at a time.

---

### Composite State — Concurrent
**One-line:** A state divided into **orthogonal regions** separated by dashed lines; **all** regions are active simultaneously.

---

### History State
**One-line:** A pseudostate that, when a composite state is re-entered, **restores the last active** sub-state.

| Symbol | Meaning |
|--------|---------|
| `H` | Shallow history — restores only the directly nested sub-state |
| `H*` | Deep history — restores the full nested sub-state tree |

---

### Deferred Event
**One-line:** An event that is **queued** rather than handled immediately; processed when the object reaches a state that does not defer it.

---

## 8. Interaction Diagrams

### Communication Diagram
**One-line:** An interaction diagram showing objects as boxes connected by links, with **numbered messages** on the links.

Objects correspond to class diagram instances. Messages are numbered (1, 2, 3...) and nested calls use dotted notation (2.1, 2.2...).  
Header: `cd DiagramName`

---

### Sequence Diagram
**One-line:** An interaction diagram showing **time-ordered message exchanges** between objects on vertical lifelines; time progresses downward.

Each object has a vertical lifeline. Activation bars show when an object is processing. Supports `«create»` and `«destroy»` markers.  
Header: `sd DiagramName`

---

### Lifeline
**One-line:** A vertical dashed line representing one participant's existence over the duration of the interaction.

---

### Combined Fragment

**One-line:** A rectangular box in a sequence diagram that encloses a sequence of messages and labels them with an interaction operator.

| Operator | Meaning |
|----------|---------|
| `alt` | if–then–else (mutually exclusive conditions) |
| `opt` | Optional block (executes only if condition true) |
| `loop` | Repeated sequence |
| `break` | Alternative executed instead of the rest when condition met |
| `par` | Concurrent execution |
| `seq` | Weak sequencing |
| `strict` | Messages must execute in exact order shown |
| `neg` | Invalid message sequence |
| `critical` | Critical section (no interleaving) |
| `ignore` | Named messages are irrelevant in this context |
| `consider` | Only named messages are relevant |
| `assert` | Any sequence not shown is invalid |

---

### Message Types

| Type | Symbol | Meaning |
|------|--------|---------|
| Synchronous | Filled arrowhead `──▶` | Sender waits for return |
| Asynchronous | Open arrowhead `──>` | Sender continues immediately |
| Return | Dashed `- - ->` | Return of control to sender |
| Lost | — | Target not yet identified |
| Found | — | Sender not yet identified |
| Balking | — | Dropped if target not immediately ready |
| Timeout | — | Dropped after waiting period expires |

---

## 9. Activity Diagrams

### Activity Diagram
**One-line:** A behavioural diagram that models the **flow of actions and decisions** within a process or use case; similar to a flowchart but richer.

Used for: modelling business processes, use case flows, algorithms.

| Element | Description |
|---------|-------------|
| Initial node `●` | Marks where the flow starts |
| Activity/action | Rounded rectangle; one step |
| Decision node `◇` | One input, multiple outputs with `[guards]` |
| Merge node `◇` | Multiple inputs, one output |
| Fork node `═` | One input splits into concurrent flows |
| Join node `═` | Multiple concurrent flows merge into one |
| Final node `⊙` | Marks where the flow ends |
| Swimlanes | Columns/rows assigning actions to responsible actors |

---

### Interaction Overview Diagram
**One-line:** A specialised activity diagram where nodes are **interaction fragments** (sequence or communication diagrams) rather than simple actions.

Used for: showing control flow across multiple related interaction scenarios.

---

## 10. Package Diagrams

### Package
**One-line:** A grouping mechanism for model elements that manages **visibility and dependency** between logically related parts of a model.

- Each element belongs to exactly **one** home package
- Packages can be **nested**
- This is a **logical** model, not an implementation model (not the same as folders)
- Header: `pd DiagramName`

---

### Package Visibility Rules

| Situation | Visibility |
|-----------|-----------|
| Within a package | All elements visible to each other |
| Package A depends on B | All **public** elements of B visible in A |
| Package A nested in B | All elements visible to B also visible to A |

---

### Package Dependency Types

| Dependency | Effect |
|------------|--------|
| `«import»` | **Public** import — names added to importing package's namespace |
| `«access»` | **Private** import — names must be qualified with source package name |
| `«merge»` | Package Q incorporates contents of P; similar to specialisation |

---

### Package Stereotypes

| Stereotype | Purpose |
|------------|---------|
| `«facade»` | Contains only calls to elements in other packages; simplified entry point |
| `«framework»` | A design pattern for a class of problems; reusable skeleton |
| `«modelLibrary»` | Contains reusable model elements shared across projects |
| `«model»` | An abstraction of the system from one specific viewpoint |
| `«subsystem»` | A logically isolated part of the system with well-defined interfaces |

---

## 11. Implementation Diagrams

### Component
**One-line:** A well-isolated, reusable implementation unit with defined interfaces.

```
┌──────────────────────┐
│  «component»         │──○  IOrder     (provided interface)
│  Orders              │──(  IInvoice   (required interface)
└──────────────────────┘
```

| View | Description |
|------|-------------|
| Black box | Only interfaces and operations shown |
| White box | Internal realizing classes shown in `«realizations»` section |

**Stereotypes:** `«subsystem»`, `«executable»`, `«library»`, `«table»`

---

### Artifact
**One-line:** Any product of the software lifecycle — document, diagram, executable, source file, script.

**Common stereotypes:** `«document»`, `«file»`, `«source»`, `«script»`

---

### Deployment Diagram
**One-line:** Shows the **runtime configuration** of a system: hardware nodes, platforms, and software artifacts deployed on those nodes.

---

## 12. Other OO Models

### Prototype-Based OO
**One-line:** An OO approach where any object can serve as a **prototype** for new objects, without formal class definitions.

Two interpretations:
1. Prototype used as a template **at creation time** only (copy-and-modify)
2. Information from the prototype available **dynamically at runtime** — changes propagate to derived objects

Used in: JavaScript (pre-ES6 classes) via the prototype chain.

- ✅ Easy, flexible, powerful meta-programming
- ❌ Hard to reason about; dynamic additions increase error risk

---

### Open Classes (Monkey-Patching)
**One-line:** The ability to add new methods or properties to an existing class **at runtime**, after the class has been defined.

Available in: Ruby, Python, JavaScript. C# Extension Methods provide a more controlled version.

- ✅ Very flexible meta-programming
- ❌ Easy to obfuscate code; errors are hard to catch statically

---

### Traits
**One-line:** Reusable units of behaviour (introduced in Scala) that can contain **method implementations AND attributes**, and can be mixed into classes or dynamically added to objects.

Similar to interfaces but richer:
- ✅ Can contain method implementations (like Java 8+ default interface methods)
- ✅ Can contain attributes
- ✅ Can be dynamically added to objects at creation time
- Provides a partial solution to the multi-inheritance problem

> Order of trait mixing matters — requires careful planning.

---

### Entity Component System (ECS)
**One-line:** An architectural model that **replaces inheritance hierarchies with composition**; entities are assembled from data components processed by separate systems.

| Element | Role |
|---------|------|
| **Entity** | A bare identity (often just an integer ID); no data, no logic |
| **Component** | Pure data container (e.g. `Position{x,y}`, `Health{hp}`); no logic inside |
| **System** | Logic that queries ALL entities with specific components and processes them |

Systems communicate **only via events** — no direct coupling. This means:
- Easy to add/remove/swap systems at any time
- Events can be sent over a network
- Replays: record events + timestamps

✅ Eliminates diamond problem, deep rigid hierarchies, blob antipattern.  
❌ Requires careful upfront design; forfeits some OO encapsulation.

---

### Data Oriented Programming (DOP)
**One-line:** A programming approach that organises data layout in memory to **match CPU cache access patterns**, prioritising performance over OO structure.

**Problem with naive OOP:** objects with many fields store all fields together. When a system needs only one field from many objects, most cached data is wasted ("swiss-cheese memory").

**DOP solution:** store data in arrays of components (**Struct of Arrays**) rather than arrays of objects (Array of Structs). All positions together, all health values together, etc.

> ECS and DOP work well together: ECS already separates components by type, naturally producing the DOP-friendly memory layout.

---

## 13. Design Patterns

### Design Pattern
**One-line:** A named, reusable solution to a commonly occurring design problem in a given context; a description of communicating objects and classes.

Origin: Christopher Alexander (architecture, 1977) → Gang of Four book (1994).

| Category | Examples |
|----------|---------|
| **Creational** | Singleton, Factory Method, Abstract Factory, Builder |
| **Structural** | Facade, Decorator, Adapter, Composite |
| **Behavioural** | Strategy, Observer, State, Mediator, Command |

---

### Singleton Pattern
**One-line:** Ensures a class has **only one instance** and provides a global access point to that instance.

```
Class Singleton:
  - instance : Singleton      ← class attribute (the single instance)
  - Singleton()               ← private constructor (prevents direct instantiation)
  + getInstance() : Singleton ← class method (returns the single instance)
```

> Use when: exactly one shared resource needed (thread manager, logger, config).

---

### Abstract Factory Pattern
**One-line:** Provides an interface for creating **families of related objects** without specifying their concrete classes.

| Participant | Role |
|-------------|------|
| `AbstractFactory` | Declares creation operations for abstract product types |
| `ConcreteFactory` | Implements creation for one specific product family |
| `AbstractProduct` | Interface for one product type |
| `ConcreteProduct` | The actual product created by a ConcreteFactory |
| `Client` | Uses only AbstractFactory and AbstractProduct interfaces |

> **Example:** a GUI library creating buttons for Windows, Mac, or Linux — client code never refers to platform-specific classes.

---

### Mediator Pattern
**One-line:** Defines an object that **encapsulates how a set of objects interact**, promoting loose coupling by preventing direct object references.

| Participant | Role |
|-------------|------|
| `Mediator` | Interface for communicating with Colleague objects |
| `ConcreteMediator` | Coordinates colleagues; knows and maintains all colleagues |
| `Colleague` | Knows its mediator; communicates through it |

> **Analogy:** air traffic controller (mediator) coordinates planes (colleagues) so they don't communicate with each other directly.

---

### Observer Pattern
**One-line:** Defines a **one-to-many dependency** so that when one object (Subject) changes state, all dependents (Observers) are notified automatically.

| Participant | Role |
|-------------|------|
| `Subject` | Maintains list of observers; provides attach/detach |
| `ConcreteSubject` | Stores state of interest; notifies on change |
| `Observer` | Defines update interface |
| `ConcreteObserver` | Implements update() to stay consistent with subject |

> **Example:** a spreadsheet cell (Subject) notifies a chart and a formula cell (Observers) whenever its value changes.

---

### State Pattern
**One-line:** Allows an object to **alter its behaviour when its internal state changes**; the object appears to change its class.

| Participant | Role |
|-------------|------|
| `Context` | Holds reference to current State; delegates state-specific requests |
| `State` | Interface for all state-specific behaviours |
| `ConcreteState` | Implements behaviour for one specific state |

> Closely related to statechart diagrams — each ConcreteState corresponds to one node in the statechart.

---

### Strategy Pattern
**One-line:** Defines a **family of algorithms**, encapsulates each one, and makes them **interchangeable** independently of the clients that use them.

| Participant | Role |
|-------------|------|
| `Strategy` | Common interface for all algorithm variants |
| `ConcreteStrategy` | Implements one specific algorithm |
| `Context` | Configured with a ConcreteStrategy; delegates the algorithm call |

**SOLID illustration:**

| Principle | How Strategy illustrates it |
|-----------|----------------------------|
| S — Single Responsibility | Each ConcreteStrategy has one job |
| O — Open/Closed | Add new strategies without modifying Context |
| L — Liskov Substitution | All strategies substitutable through the interface |
| I — Interface Segregation | Strategy interface exposes only what Context needs |
| D — Dependency Inversion | Context depends on Strategy abstraction, not ConcreteStrategy |

---

### Facade Pattern
**One-line:** Provides a **simplified interface** to a complex subsystem, hiding internal complexity from clients.

---

### Decorator Pattern
**One-line:** Attaches **additional responsibilities to an object dynamically** as an alternative to subclassing for extending functionality.

---

### Factory Method Pattern
**One-line:** Defines an interface for creating an object but lets **subclasses decide** which concrete class to instantiate.

---

## 14. Architectural Patterns

### Architectural Pattern
**One-line:** A general, reusable solution to a **system-level structural problem**; concerns the overall organisation of a system, not individual classes.

> Distinction: architectural patterns deal with coarse-grained subsystems; design patterns deal with class/object relationships.

---

### Application Structure Patterns

| Pattern | Description |
|---------|-------------|
| **Monolithic** | Single-tiered; combines UI, business logic, and data storage in one deployable unit |
| **Layered** | Horizontal layers; each uses abstractions of the layer below (UI → Logic → Data → DB) |
| **Component-based** | Substitutable, independently deployable components with defined responsibilities |
| **Pipeline** | Sequential stages transforming data; stages can run in parallel (Unix pipes, ETL, compilers) |
| **Data-centric** | Multiple apps share one central database; logic may live in stored procedures |

---

### Messaging Patterns

| Pattern | Description |
|---------|-------------|
| **Event-driven** | Loosely coupled components communicate by raising/consuming asynchronous events |
| **Publish-subscribe** | Messages organised into topics; consumers subscribe; a broker usually involved; broadcast |
| **Message queue** | Messages consumed ONCE by exactly one consumer; point-to-point delivery |

---

### Adaptability Patterns

| Pattern | Description |
|---------|-------------|
| **Plug-in** | Host provides services and registration protocols; plug-ins extend functionality without modifying host |
| **Scripting** | Embedded interpreter; scripts interact via APIs; level of control varies |
| **DSL** | Specialised language for one domain (SQL, HTML, CSS, regex) |

---

### Distribution Patterns

| Pattern | Description |
|---------|-------------|
| **2-tier** | Thick client + database server |
| **3-tier / N-tier** | Tier 1: data, Tier 2: business logic, Tier 3: UI; each tier independently scalable |
| **Shared Nothing** | Each node independent; no shared memory or disk; adding nodes increases capacity |
| **Broker** | Mediator service routes requests between clients and services |
| **Peer-to-peer** | All nodes have equal roles; high resilience; very hard to manage/secure |
| **SOA** | Logic split into small independent web services; composed via workflow engine |
| **REST** | Application as network of information resources; stateless; uses web standards |

---

## 15. SOLID Principles

| Principle | One-line |
|-----------|---------|
| **S** — Single Responsibility | A class should have only ONE reason to change; it should do one thing |
| **O** — Open/Closed | Open for extension, closed for modification; add behaviour via inheritance/composition |
| **L** — Liskov Substitution | Objects of a derived class must be substitutable for the base class without breaking correctness |
| **I** — Interface Segregation | Clients should not be forced to depend on interfaces they do not use; keep interfaces small |
| **D** — Dependency Inversion | High-level modules should not depend on low-level modules; both should depend on abstractions |

---

## 16. Domain Driven Design

| Concept | One-line |
|---------|---------|
| **Entity** | Object with unique identity that persists even when attributes change; should carry business logic |
| **Value Object** | No identity; two identical value objects are equal and interchangeable; **immutable** (e.g. Money, Address) |
| **Aggregate Root** | Cluster of entities and value objects; only the root has global identity and is accessible from outside |
| **Repository** | Abstracts persistence; provides collection-like interface for retrieving and storing aggregates |
| **Bounded Context** | Area of the system with its own language, model, and (optionally) independent infrastructure |
| **CQRS** | Command Query Responsibility Segregation — separates read (queries) from write (commands) operations |

---

## 17. Wrong Assumptions in Software Design

### Prices & Currencies
- Do **not** store prices as floats — use integer cents or a decimal type
- Not all currencies divide into 100 units (Mauritania's ouguiya = 5 khoums)
- Some currencies have **no subdivisions** (Japanese yen, Korean won)
- `$` is used by USA, Canada, Australia and others; `¥` is both Yen and Yuan
- A product may have **many prices**: list, sale, tax-exclusive, tax-inclusive, etc.
- Prices can be **negative** (returns) or **zero** (free items)
- Swiss franc rounds to **five centimes** (0.05), not two decimal places

### People's Names
- Name order is **not universal** — Chinese names have family name first
- **Multiple family names**: Spanish speakers commonly have two; Brazilians may have three or four
- **Patronymics**: Icelandic last names (-sson/-sdóttir) are not shared family names
- A real person has the last name **"Null"** — systems must handle it
- Honorifics matter: German `Herr Professor Doktor`; Japanese `-san`, `-bucho`

### Addresses
- **No universal format**: some addresses have no street name (just a hamlet)
- Building numbers are **not always numeric**: `10A/12`, letters, dual historic systems
- Zero and **negative house numbers** exist
- Street names **repeat** within a city
- Addresses can **change**: streets renamed, borders redrawn, houseboats can move between countries

### Time
- **Year 2038 problem**: 32-bit signed Unix timestamps overflow on 19 January 2038
- Timestamps have **varying precision**: seconds, milliseconds, microseconds, nanoseconds
- A precise timestamp is **not guaranteed to be unique** across machines

### Networks — Fallacies of Distributed Computing (Deutsch)
The following assumptions are **wrong**:
1. The network is reliable
2. Latency is zero
3. Bandwidth is infinite
4. The network is secure
5. Topology does not change
6. There is one administrator
7. Transport cost is zero
8. The network is homogeneous

> **Design implication:** model domain objects (prices, names, addresses, timestamps) to accommodate real-world complexity from the **start**. Retrofitting is significantly more costly.

---

## 18. Complete Notation Reference

### UML Diagram Types

| Category | Diagram | Header | Purpose |
|----------|---------|--------|---------|
| **Structural** | Class diagram | `cd` | Classes, attributes, methods, relationships |
| | Object diagram | — | Snapshot of objects at one moment |
| | Component diagram | — | Components and their interfaces |
| | Deployment diagram | — | Hardware nodes and deployed artifacts |
| | Package diagram | `pd` | Logical grouping and dependencies |
| **Behavioural** | Use case diagram | — | Actors and system functionalities |
| | Activity diagram | — | Flow of actions and decisions |
| | Statechart diagram | `sm` | States and transitions of one class/object |
| | Interaction overview | — | Control flow across multiple interactions |
| **Interaction** | Sequence diagram | `sd` | Time-ordered messages on lifelines |
| | Communication diagram | `cd` | Numbered messages on object links |
| | Timing diagram | — | State/value changes over time |

---

### Class Diagram — Symbol Reference

| Symbol | Meaning |
|--------|---------|
| `──▷` | Inheritance (hollow arrow pointing TO parent) |
| `── - - ▷` | Realization (dashed line, hollow arrow TO interface) |
| `────` | Plain association |
| `◇────` | Aggregation (open diamond on WHOLE side) |
| `◆────` | Composition (filled diamond on WHOLE side) |
| `─ ─ ─` | Dashed line (to association class, or dependency) |
| `"1"` | Multiplicity: exactly one |
| `"0..1"` | Multiplicity: zero or one |
| `"*"` | Multiplicity: zero or more |
| `"1..*"` | Multiplicity: one or more |
| `"3..5"` | Multiplicity: between three and five |
| `/name` | Derived attribute or derived association |
| <u>name</u> | Underlined = class (static) attribute or method |
| `{abstract}` | Abstract class or method |
| `«name»` | Stereotype |
| `{key=value}` | Tagged value |
| `{rule}` | Constraint |
| `...` | Ellipsis: some elements intentionally not shown |

---

### Use Case Diagram — Symbol Reference

| Symbol | Meaning |
|--------|---------|
| Stick figure | Human actor |
| `«system»` box | Non-human system actor |
| `( UC Name )` | Use case oval |
| Rectangle | System boundary (all UCs inside; all actors outside) |
| `────` | Actor-to-use-case connection (participation) |
| `──▷` | Actor generalization (sub-actor inherits all UCs) |
| `─ ─ «include» ─▷` | Base UC always triggers included UC |
| `─ ─ «extend» ─▷` | Optional UC extends base UC when condition met |
| `{condition}` | Extension point on `«extend»` arrow |

---

### Statechart — Symbol Reference

| Symbol | Meaning |
|--------|---------|
| `●` | Initial pseudostate |
| `⊙` | Final pseudostate |
| Rounded rectangle | State box |
| `──event[cond]/action──▷` | Transition label |
| `H` | Shallow history pseudostate |
| `H*` | Deep history pseudostate |
| Double horizontal bar `═` | Fork/join bar (concurrent regions) |
| `◇` | Decision/merge node |

---

### Sequence Diagram — Symbol Reference

| Symbol | Meaning |
|--------|---------|
| `:ClassName` | Lifeline header |
| `│` (dashed) | Vertical lifeline |
| Thick rectangle on lifeline | Activation bar (object is executing) |
| `──▶` | Synchronous message (filled arrowhead) |
| `──>` | Asynchronous message (open arrowhead) |
| `- - ->` | Return message (dashed) |
| `«create»` | Object creation |
| `«destroy»` | Object destruction |
| `┌─ alt ─┐` | Combined fragment |

---

## 19. Exam Quick-Reference

The 25 most-tested definitions across Task II exam sheets.

| # | Term | One-line definition |
|---|------|---------------------|
| 1 | **Abstract class** | Cannot be instantiated; defines shared structure; may have abstract AND implemented methods; marked `{abstract}` |
| 2 | **Concrete class** | Can be instantiated; implements ALL inherited abstract methods; CANNOT have abstract methods |
| 3 | **Abstract method** | Declared but not implemented; must be implemented by every concrete subclass; written italic/`{abstract}` |
| 4 | **Object attribute** | One copy per instance; normal text in UML |
| 4 | **Class attribute** | One copy for ALL instances; **underlined** in UML |
| 5 | **Object method** | Operates on one object using its own data; normal text in UML |
| 5 | **Class method** | Operates on all instances / class level; **underlined** in UML |
| 6 | **Stereotype** | `«name»`; meta-classifies a UML element; extends UML vocabulary |
| 7 | **Tagged value** | `{key=value}`; attaches arbitrary metadata; does NOT restrict |
| 8 | **Constraint** | `{rule}`; restricts model; semantically binding; enforced |
| 9 | **Comment / Note** | Dog-ear box with dashed line; purely informational; no enforcement |
| 10 | **`{overlapping}`** | Object belongs to multiple subclasses at once |
| 10 | **`{disjoint}`** | Object belongs to exactly one subclass at a time |
| 11 | **`{complete}`** | Every superclass object falls into some subclass |
| 11 | **`{incomplete}`** | Some superclass objects may be in no subclass |
| 12 | **`«dynamic»`** | Object can change subclass during its lifetime (same identity) |
| 13 | **Aggregation ◇** | Parts exist independently; sharing allowed; whole multiplicity: any |
| 13 | **Composition ◆** | Parts die with whole; no sharing; whole multiplicity must be 1 |
| 14 | **Association class** | Class attached to an association line (dashed); carries attributes/methods of the relationship itself |
| 15 | **Derived attribute** | `/prefix`; computed from other data; NOT stored independently |
| 16 | **«include»** | UC always triggers another UC (mandatory); base → included |
| 16 | **«extend»** | UC optionally extends another when condition met; optional → base |
| 17 | **Multi-inheritance** | Class inherits from two+ different parent classes |
| 17 | **Overlapping inheritance** | Same object has multiple types within ONE hierarchy |
| 18 | **`{subset}`** | One association is a subset of another; drawn as dashed arrow between associations |
| 19 | **«interface»** | Methods only, no attributes, no implementation; other classes REALIZE it |
| 20 | **Encapsulation** | Hiding internal structure; separating interface from implementation |
| 21 | **Late binding** | Method call resolved at RUNTIME; required for polymorphism |
| 21 | **Early binding** | Method call resolved at COMPILE TIME; better performance |
| 22 | **Method overriding** | Subclass replaces a superclass method with its own version |
| 22 | **Method overloading** | Same method name, different parameter signature in one class |
| 23 | **Strategy pattern** | Family of interchangeable algorithms behind a common interface |
| 23 | **Observer pattern** | Subject notifies all dependent Observers on state change |
| 23 | **Singleton pattern** | Exactly one instance; class attribute holds it; private constructor |
| 24 | **N-ary association** | Links objects from MORE THAN two classes simultaneously |
| 25 | **Qualified association** | Association with a qualifier attribute acting as a lookup key |

---

*Compiled from OOD Lectures 3–14, Tutorial 7, and supplementary materials.*
