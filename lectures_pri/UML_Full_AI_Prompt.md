# AI Prompt – Generate Full UML Analysis from Requirements
### Produces: Class Diagram · Task II Answers · Use Case Diagram (actors + use cases)

Copy everything between `===START===` and `===END===`, paste your requirements where indicated.

---

~~~
===START===

You are a UML expert. I will give you system requirements.
Produce ALL of the following in order: Class Diagram, Task II answers, and a Use Case Diagram.
Follow EVERY rule listed below with no exceptions.

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
## PART A — UML CLASS DIAGRAM RULES
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

### CLASSES
- Identify classes from domain nouns. Name them PascalCase singular nouns.
- If a class cannot be instantiated directly → mark {abstract}.
- Write abstract class names with the {abstract} tag.

### ATTRIBUTES — full syntax:
  visibility name : Type [multiplicity] = default {constraint}
- Visibility: - private (default), + public, # protected.
- Optional attribute → [0..1]
- List attribute → [0..*] or [1..*]
- Fixed array → [n] (e.g. [0..5] = up to 5 entries)
- Derived (computed, not stored) → prefix /  e.g. /age : Integer
- Enumerated values → {constraint}: status : String {"proposal", "planned"}
- CLASS attribute (static, shared by ALL instances) → write UNDERLINED, mark with ** in text:
    **minAttendees : Integer = 10**   means underlined in the real diagram
- Decision: "minimum value", "rule for all instances", "constant" → class attribute.

### METHODS — full syntax:
  visibility name(param: Type) : ReturnType
- Abstract method (no implementation) → {abstract} prefix, written in italics in real diagram
- CLASS method (static) → write UNDERLINED, mark with ** in text: **makeRanking() : List**
  Decision rule:
    → method needs THIS object's data → object method (normal)
    → method queries/ranks/counts ALL objects → class method (**underlined**)
    → method has no meaningful "self" → class method (**underlined**)
- Abstract methods can only exist in abstract classes.
- Concrete classes must implement ALL inherited abstract methods.

### INHERITANCE
- Every generalization set MUST declare BOTH:
    1. {disjoint} or {overlapping}
    2. {complete} or {incomplete}
- {disjoint}: object belongs to EXACTLY ONE subclass at a time
- {overlapping}: object may belong to MULTIPLE subclasses simultaneously
- {complete}: every superclass object must fall into some subclass
- {incomplete}: some superclass objects may belong to no subclass
- «dynamic» tag: object CAN migrate between subclasses over time

### ASSOCIATIONS
- Always add multiplicity at BOTH ends: 1, 0..1, *, 1..*, 3..*, min..max
- Always add association name (verb phrase) + reading direction < or >
- Add role names at each end when they clarify meaning
- If an association has its OWN attributes or methods → ASSOCIATION CLASS
  (connected to the association line by a dashed vertical line)

### AGGREGATION & COMPOSITION
- Aggregation ◇ (open diamond on whole side):
    parts can exist independently, can be shared
- Composition ◆ (filled diamond on whole side):
    parts cannot exist without the whole, cannot be shared
    whole-side multiplicity MUST be 1 (or 0..1), never more

### STEREOTYPES
- «interface»    → pure specification: methods only, no attributes, no implementation
- «persistent»   → instances stored in a database
- «history»      → association class that retains records after relationship ends
- «system»       → non-human actor (automated system)

### TAGGED VALUES  {key = value}
- Annotate elements with metadata: {minValue=100}, {if age under 18}, {author="X"}
- NOT the same as constraints — tagged values describe, constraints restrict

### CONSTRAINTS  {expression}
- Predefined: {complete}, {incomplete}, {disjoint}, {overlapping},
              {ordered}, {bag}, {subset}, {or}, {xor}, {unique}
- Custom: any natural language or OCL in { }

### DERIVED ELEMENTS
- /attributeName : Type   → computed from other data, not stored
- /associationName        → computable from other associations

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
## PART B — TASK II ANSWER RULES
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Answer ALL of the following questions. For each:
- First give the DEFINITION (1 sentence minimum).
- Then give a CONCRETE EXAMPLE taken from the class diagram you drew above.
- For each example, state EXACTLY WHERE on the diagram it appears
  (which class, which attribute/method, which relationship line).

QUESTIONS TO ANSWER:
Q1. Define abstract class and concrete class. What method types (abstract / implemented)
    may each contain? Give examples from your diagram.

Q2. Explain the difference between class attribute and object attribute.
    Give examples from your diagram, naming the exact class and attribute.

Q3. Explain the difference between class method and object method.
    Give examples from your diagram, naming the exact class and method.

Q5. Explain the concept of stereotype. Give TWO examples of stereotypes
    defined in UML, explain the meaning of each.

Q6. Explain the concept of overlapping inheritance.
    Give an example from your diagram (or invent one consistent with the requirements).
    Show the diagram notation used.

Q7. Explain the concept of a tagged value.
    Give at least TWO examples from your diagram, naming where each appears.

FORMAT FOR EACH ANSWER:
  ### Q[number] — [Topic]
  **Definition:** ...
  **Example from diagram:**
    - Class/element: [exact name]
    - Where: [attribute/method/association/inheritance group]
    - Why it qualifies: [one sentence explanation]

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
## PART C — UML USE CASE DIAGRAM RULES
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

### ACTORS
- An actor is anyone (person, system) that interacts with the system from outside.
- Identify ALL actors from the requirements.
- Human actors: drawn as stick figures (ASCII: [O] / stick figure)
- System actors: drawn with «system» stereotype
- Generalization between actors IS allowed: e.g. Attendee ──▷ Person

### USE CASES
- A use case = one goal an actor achieves using the system.
- Name use cases as verb phrases: "Print Brochure", "Pay For Ticket"
- Draw each use case as an oval: ( Use Case Name )
- Group all use cases inside a rectangle labelled with the system name: ┌─ System ─┐

### RELATIONSHIPS
- Actor → Use Case: solid line (actor initiates/participates in)
  Write the line as:  ActorName ────── ( Use Case )
- «include»: Use Case A always triggers Use Case B (mandatory sub-step)
  Write as:  ( A ) ─ ─«include»─ ─▷ ( B )
- «extend»: Use Case B optionally extends Use Case A (conditional path)
  Write as:  ( B ) ─ ─«extend»─ ─▷ ( A )
  Add extension point in { }: {if online payment chosen}
- Actor generalization: SubActor ──▷ SuperActor  (inherits all use cases)

### FOR EACH ACTOR — write a plain-text description BEFORE the diagram:

  #### Actor: [ActorName]
  **Type:** Human | System
  **Description:** [what role they play]
  **Use cases they initiate:**
  - ( Use Case 1 ) — [one sentence: what they do and what the system does back]
  - ( Use Case 2 ) — [one sentence]
  **Use cases they participate in (not initiating):**
  - ( Use Case X ) — [one sentence]
  **Relationships to other actors:** [generalization / none]

  (Repeat for every actor)

### ASCII USE CASE DIAGRAM FORMAT

Use this layout:

  ┌────────────────────────── System Name ───────────────────────────┐
  │                                                                   │
  │   ( Use Case 1 )         ( Use Case 2 )       ( Use Case 3 )    │
  │                                                                   │
  │   ( Use Case 4 )         ( Use Case 5 )                         │
  │                                                                   │
  │         ─ ─«include»─ ─▷  arrows between use cases              │
  │         ─ ─«extend»─ ─▷   arrows between use cases              │
  │                                                                   │
  └───────────────────────────────────────────────────────────────────┘

  [O]                          [O]                      [O]
   |  ActorName1                |  ActorName2             |  ActorName3
  / \                          / \                       / \

  ActorName1 ──── ( Use Case 1 )
  ActorName1 ──── ( Use Case 4 )
  ActorName2 ──── ( Use Case 2 )
  ActorName2 ──── ( Use Case 3 )

  «include» / «extend» arrows:
  ( Use Case 1 ) ─ ─«include»─ ─▷ ( Sub Use Case )
  ( Optional UC ) ─ ─«extend»─ ─▷ ( Use Case 2 )   {extension point: condition}

Rules:
- Every use case must be connected to at least one actor.
- Every actor must be connected to at least one use case.
- «include» is for MANDATORY sub-steps (always happens).
- «extend» is for OPTIONAL/CONDITIONAL paths (sometimes happens).
- Do NOT include system internals as use cases (e.g. "save to database" is internal).
- Do NOT add actors that have no use cases.

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
## OUTPUT ORDER — FOLLOW EXACTLY
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

### OUTPUT 1 — CLASS DIAGRAM

#### 1A — ALL CLASSES (list every class first)

For each class output this block:

  CLASS: ClassName {abstract if applicable}
  «stereotype if any»
  ATTRIBUTES:
    - name : Type [mult] = default {constraint}
    - /derivedAttr : Type
    - **classAttr : Type = value**        ← ** = underlined = class/static
  METHODS:
    + methodName(param: Type) : ReturnType          ← object method
    **+ classMethod() : ReturnType**                ← class method (underlined)
    {abstract} + abstractMethod() : ReturnType      ← abstract method

#### 1B — ALL CONNECTIONS (list every relationship)

  ASSOCIATION:   Class1 "mult" ── label> "mult" Class2
  ASSOC CLASS:   Class1 ── Class2  [via AssociationClassName]
                   AssociationClassName has: attr1, attr2, method()
  INHERITANCE:   SubClass ──▷ SuperClass  {constraint1, constraint2}
  COMPOSITION:   Whole "1" ◆── Part "mult"
  AGGREGATION:   Whole "mult" ◇── Part "mult"
  REALIZATION:   Class ── ─ ─▷ «interface» InterfaceName

#### 1C — ASCII CLASS DIAGRAM

Draw boxes in this format:

  ┌─────────────────────────────────────────┐
  │  «stereotype»                           │
  │  ClassName {abstract}                   │
  ├─────────────────────────────────────────┤
  │  - attribute : Type                     │
  │  - optional[0..1] : Type               │
  │  - list[0..*] : Type                   │
  │  /derived : Type                        │
  │  **classAttr : Type = 10**             │
  ├─────────────────────────────────────────┤
  │  + method() : ReturnType               │
  │  {abstract} + absMethod() : void       │
  │  **+ classMethod() : List**            │
  └─────────────────────────────────────────┘

Relationship lines between boxes:
  ──▷              inheritance (arrow points to parent)
  ◆───             composition (filled diamond on whole)
  ◇───             aggregation (open diamond on whole)
  ─ ─ ─            dashed line (to association class, or realization)
  ── label> ──     named association
  "1..*"           multiplicity label on association line

Layout rules:
- Abstract/parent classes at TOP
- Subclasses BELOW their parent
- Heavily connected classes in the CENTER
- Association classes BELOW their association line, connected by dashed │
- Split into labelled sections if too wide: === SECTION: Events ===

#### 1D — DESIGN DECISIONS (brief numbered list)

  1. Why each class is abstract or concrete
  2. Why each composition was chosen over aggregation (or vice versa)
  3. Why each inheritance is {disjoint} or {overlapping}
  4. Which attributes are class-level and why
  5. Which methods are class-level and why

---

### OUTPUT 2 — TASK II ANSWERS

Answer each question using format:
  ### Q[n] — [Topic]
  **Definition:** ...
  **Example from diagram:**
    - Element: [exact class/attribute/method name]
    - Location: [which class, which compartment, which line]
    - Explanation: [why this is a good example]

---

### OUTPUT 3 — USE CASE DIAGRAM

#### 3A — ACTOR DESCRIPTIONS (one block per actor, BEFORE the diagram)

  #### Actor: [Name]
  **Type:** Human / System
  **Description:** [role in the system]
  **Initiates:**
    - ( Use Case ) — [what they do, what system responds]
  **Participates in:**
    - ( Use Case ) — [their role in this use case]
  **Inherits from:** [actor name or "none"]

#### 3B — ASCII USE CASE DIAGRAM

Draw the full diagram following the format from Part C rules above.
Label every «include» and «extend» arrow.
Add extension points {condition} next to «extend» arrows.

#### 3C — RELATIONSHIP EXPLANATIONS

For every «include» and «extend» arrow, explain in one sentence:
  - «include»: ( A ) ──▷ ( B ):  B always happens when A happens because [reason]
  - «extend»:  ( B ) ──▷ ( A ):  B optionally extends A when [condition]

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
## HERE ARE THE REQUIREMENTS:

[PASTE YOUR REQUIREMENTS TEXT HERE]

===END===
~~~

---

## How to Use This Prompt

| Step | Action |
|------|--------|
| 1 | Copy everything between `===START===` and `===END===` |
| 2 | Replace `[PASTE YOUR REQUIREMENTS TEXT HERE]` with your actual text |
| 3 | Send to any AI (Claude, ChatGPT, Gemini) |
| 4 | You receive all three outputs in one response |

---

## What Each Output Gives You

### Output 1 — Class Diagram
- **1A:** Every class listed with all attributes and methods — easy to check completeness
- **1B:** Every connection listed as text — easy to spot missing associations
- **1C:** Full ASCII diagram — compare box by box with your hand-drawn diagram
- **1D:** Reasoning for every design choice — helps you understand WHY

### Output 2 — Task II Answers
Answers to all exam theory questions, each backed by a **named, located example** from the diagram the AI just drew. Format mirrors exam expectations: definition first, then example with precise location.

### Output 3 — Use Case Diagram
- **3A:** Each actor described in plain English with a bulleted list of which use cases they can trigger — acts as a checklist of "which buttons they can press"
- **3B:** Full ASCII use case diagram with actors, use case ovals, «include»/«extend» arrows
- **3C:** Every «include» and «extend» explained in plain language

---

## Troubleshooting / Follow-up Prompts

| Problem | Follow-up prompt to send |
|---------|--------------------------|
| AI missed a class | *"You missed [X] from requirement [N]. Add it and redraw section 1A and 1C."* |
| Diagram is misaligned | *"Redraw only section 1C with cleaner alignment. Keep boxes the same width."* |
| Use case missing an actor | *"Actor [X] should also connect to use case [Y]. Update sections 3A and 3B."* |
| Want to check your own diagram | Compare your diagram against 1C class by class and 3B actor by actor |
| Want only the ASCII class diagram | *"Give me only section 1C."* |
| Want only use cases for one actor | *"Give me only the 3A block for actor [Name]."* |
| Disagree with a design choice | *"Why did you choose {disjoint} not {overlapping} for [X] → [Y,Z]? Justify."* |
| Want to verify Task II answers | *"For Q2, show me the exact line in the diagram where the class attribute appears."* |

---

## Reference: UML Notations Used in Output

```
CLASS DIAGRAM SYMBOLS
─────────────────────────────────────────────────────────────
┌──────────────┐   Class box
│  ClassName   │   Abstract: {abstract} tag or italic name
├──────────────┤   Stereotype: «name» above class name
│  - attr:Type │   Private attribute
│  /derived    │   Derived attribute (computed)
│  **static**  │   Class (static) attribute — underlined in real diagram
├──────────────┤
│  + method()  │   Object method
│  **+static** │   Class method — underlined in real diagram
│  {abstract}  │   Abstract method — italic in real diagram
└──────────────┘

──▷              Inheritance (hollow arrow → parent)
◆────            Composition (filled diamond on whole)
◇────            Aggregation (open diamond on whole)
─ ─ ─▷           Realization (dashed hollow arrow → interface)
────             Association (plain line)
"1..*"           Multiplicity on association end
── label> ──     Named association with direction
{disjoint,complete}   Generalization set constraint

USE CASE DIAGRAM SYMBOLS
─────────────────────────────────────────────────────────────
[O]
 |   Stick figure = human actor
/ \

«system»         Non-human actor label

( Use Case )     Use case oval

────             Actor participates in use case

─ ─«include»─▷  Mandatory inclusion (always happens)
─ ─«extend»─▷   Optional extension (conditional)
{condition}      Extension point on «extend»

──▷              Actor generalization (sub-actor inherits all UCs)

┌── System ──┐   System boundary rectangle
│            │   All use cases go inside
└────────────┘   Actors go outside
```

---

## Example: What the Actor Description Block Should Look Like

For the Splattercon system the AI should produce something like:

```
#### Actor: Attendee
**Type:** Human
**Description:** A person who registers for and attends a convention.
**Initiates:**
  - ( Fill Registration Form ) — Attendee starts the registration process online;
    system stores their personal details.
  - ( Pay For Ticket Online ) — Attendee optionally pays the ticket price online;
    system records the payment.
  - ( View Convention Brochure ) — Attendee requests the event list;
    system prints/displays the brochure.
**Participates in:**
  - ( Get Identifier ) — Attendee provides details at the registration desk;
    system issues a numbered badge.
  - ( Take Part in Event ) — Attendee joins a LARP, panel, or lecture;
    system records their attendance.
**Inherits from:** Person

#### Actor: Guest
**Type:** Human
**Description:** An invited speaker or performer at the convention.
**Initiates:**
  - ( Deliver Lecture ) — Guest presents a lecture; system records their participation.
  - ( Speak on Panel ) — Guest speaks at a panel; system records their role.
**Participates in:**
  - ( Print Identifier ) — Staff prints a guest badge; system provides guest data.
**Inherits from:** Person

#### Actor: Organizer
**Type:** Human
**Description:** Convention staff member who manages events and registration.
**Initiates:**
  - ( Register Event Proposal ) — Organizer accepts a proposal;
    system changes event status to "planned" and assigns date/time/location.
  - ( Print Convention Brochure ) — Organizer triggers brochure generation;
    system outputs the full event list.
  - ( Make Ranking of Conventions ) — Organizer requests rankings;
    system sorts conventions by attendee count.
  - ( Print Identifier ) — Organizer prints a badge for an attendee or guest;
    system retrieves their identifier.
**Participates in:** none
**Inherits from:** none

#### Actor: Vendor
**Type:** Human
**Description:** A business operating a booth in the convention's business area.
**Initiates:**
  - ( Register Booth ) — Vendor registers a booth with area and merchandise type;
    system calculates the fee.
  - ( Pay Booth Fee ) — Vendor pays the calculated fee; system records payment.
**Participates in:** none
**Inherits from:** none
```
