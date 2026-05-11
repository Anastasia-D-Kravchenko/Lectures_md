# 🐍 Python Lecture — Quiz Part 3
> Covers: Magic Methods (operator overloading, `__hash__`, `__bool__`, `__call__`), Inheritance & Polymorphism, MRO, Class Decorators, Context Managers (`with`, `@contextmanager`)

---

## 📔 Chapter 10 — Magic Methods & Operator Overloading

---

**Q1.** When you write `a + b` and `a` is an object of your class, what dunder method is called?

<details><summary>✅ Answer</summary>

**`a.__add__(b)`** (or more precisely `type(a).__add__(a, b)`)

</details>

---

**Q2.** Match each operator to its dunder method:

| Operator | Method |
|---|---|
| `a - b` | ? |
| `a * b` | ? |
| `a / b` | ? |
| `a // b` | ? |
| `a % b` | ? |
| `a ** b` | ? |
| `~a` | ? |

<details><summary>✅ Answer</summary>

| Operator | Method |
|---|---|
| `a - b` | `__sub__` |
| `a * b` | `__mul__` |
| `a / b` | `__truediv__` |
| `a // b` | `__floordiv__` |
| `a % b` | `__mod__` |
| `a ** b` | `__pow__` |
| `~a` | `__invert__` (not `__not__`) |

</details>

---

**Q3.** What is a "reflected" operator method, and when is it called?

<details><summary>✅ Answer</summary>

A reflected method (e.g., `__radd__`) is called when the **left operand** does not support the operation (returns `NotImplemented`), the left and right operands are of **different types**, and the right operand's class defines `__radd__`.

Example: `4 + obj` → tries `int.__add__(4, obj)` → fails → tries `obj.__radd__(4)`.

</details>

---

**Q4.** What happens when you write `4 + obj` where `obj` is of your custom class, but your class only has `__add__` (not `__radd__`)?

- A) `obj.__add__(4)` is called
- B) `int.__add__(4, obj)` is called — works fine
- C) `TypeError: unsupported operand type(s)`
- D) `obj + 4` is evaluated instead

<details><summary>✅ Answer</summary>

**C) `TypeError`**  
`__add__` is called on the left operand. `4 + obj` means `int.__add__(4, obj)`, which fails because `int` doesn't know about your class, and your class has no `__radd__`. Fix: add `__radd__`.

</details>

---

**Q5.** What dunder method does `a += b` call (if defined), and what should it do?

<details><summary>✅ Answer</summary>

**`__iadd__(self, b)`** — should modify `self` in place and return `self`.  
If not defined, Python falls back to `a = a.__add__(b)` (creates new object).

</details>

---

**Q6.** Match each comparison operator to its dunder method:

| Operator | Method |
|---|---|
| `a == b` | ? |
| `a != b` | ? |
| `a < b` | ? |
| `a <= b` | ? |
| `a > b` | ? |
| `a >= b` | ? |

<details><summary>✅ Answer</summary>

| Operator | Method |
|---|---|
| `a == b` | `__eq__` |
| `a != b` | `__ne__` |
| `a < b` | `__lt__` |
| `a <= b` | `__le__` |
| `a > b` | `__gt__` |
| `a >= b` | `__ge__` |

</details>

---

**Q7.** What does `@functools.total_ordering` do, and what minimum methods must you provide?

<details><summary>✅ Answer</summary>

It automatically generates the remaining four comparison methods from just **two** you define:
- `__eq__` (required)
- One of `__lt__`, `__le__`, `__gt__`, or `__ge__`

The decorator fills in the rest.

</details>

---

**Q8.** What is `__hash__` for, and what rule must be respected when you define both `__eq__` and `__hash__`?

<details><summary>✅ Answer</summary>

`__hash__` makes objects usable as dictionary keys or set members. The rule: **if `a == b` (i.e., `__eq__` returns `True`), then `hash(a)` must equal `hash(b)`**.  
Common pattern: `return hash((self.field1, self.field2))`.

</details>

---

**Q9.** TRUE or FALSE: If you define `__eq__` in a class, Python automatically sets `__hash__` to `None`, making objects unhashable.

<details><summary>✅ Answer</summary>

**TRUE** (by default).  
If you want your objects to be hashable *and* have custom equality, you must explicitly define `__hash__` too.

</details>

---

**Q10.** What does `__bool__` control?

<details><summary>✅ Answer</summary>

It controls the truthiness of an object in boolean contexts (e.g., `if obj:`, `while obj:`, `not obj`). Should return `True` or `False`.

```python
def __bool__(self):
    return self.age >= 18
```

</details>

---

**Q11.** What is the output?

```python
class Person:
    def __init__(self, name, age):
        self.name = name
        self.age = age
    def __bool__(self):
        return self.age >= 18

lst = [Person("A", 13), Person("B", 19), Person("C", 10)]
adults = [p for p in lst if p]
print([p.name for p in adults])
```

<details><summary>✅ Answer</summary>

**`['B']`**  
Only Person B (age 19) passes `__bool__` returning `True`.

</details>

---

**Q12.** What does `__call__` enable, and what is its signature?

<details><summary>✅ Answer</summary>

`__call__` makes an **object callable** like a function. Calling `obj(args)` is equivalent to `obj.__call__(args)`.

```python
class Adder:
    def __init__(self, n):
        self.n = n
    def __call__(self, x):
        return self.n + x

add5 = Adder(5)
print(add5(3))  # 8
```
Useful when you want a function-like object that carries persistent state.

</details>

---

**Q13.** What does `__repr__` return vs. `__str__`, and which one should you prioritize defining?

<details><summary>✅ Answer</summary>

- `__repr__`: unambiguous, developer-facing, ideally a string that recreates the object (e.g., `Person("John", 2001)`). Used in REPL and `repr()`.
- `__str__`: human-readable, used by `print()` and `str()`.

If only one is defined: `__repr__` is used as fallback for both. **Prioritize `__repr__`**.

</details>

---

**Q14.** In an f-string, how do you force `__repr__` to be used instead of `__str__`?

<details><summary>✅ Answer</summary>

Use the `!r` conversion flag: `f'{obj!r}'`  
- `f'{obj}'` → calls `__str__`
- `f'{obj!r}'` → calls `__repr__`
- `f'{obj=}'` → calls `__repr__` by default

</details>

---

**Q15.** What does `__getitem__` enable on a class?

<details><summary>✅ Answer</summary>

It enables **indexing** with `[]`: `obj[key]`. It can also make an object iterable (if it accepts integer indices starting from 0), even without `__iter__`.

</details>

---

**Q16.** TRUE or FALSE: An object without `__iter__` can still be iterated by a `for` loop if it defines `__getitem__`.

<details><summary>✅ Answer</summary>

**TRUE**  
The built-in `iter()` function tries `__getitem__` with indices starting from 0 when `__iter__` is absent, stopping when `IndexError` (or `StopIteration`) is raised.

</details>

---

**Q17.** What does `__len__` enable?

<details><summary>✅ Answer</summary>

It makes `len(obj)` work, and also influences truthiness — if `__bool__` is not defined, Python uses `len(obj) != 0` to determine truthiness.

</details>

---

**Q18.** What is the output?

```python
class Avg:
    def __init__(self):
        self.total = 0
        self.count = 0
    def __call__(self, val):
        self.total += val
        self.count += 1
        return self.total / self.count

avg = Avg()
print(avg(10))
print(avg(20))
print(avg(30))
```

<details><summary>✅ Answer</summary>

```
10.0
15.0
20.0
```
Each call updates the running average: 10/1, 30/2, 60/3.

</details>

---

## 📗 Chapter 10 (continued) — Inheritance, Polymorphism, MRO

---

**Q19.** In Python, when does the base class `__init__` get called automatically?

- A) Always, before the derived `__init__`
- B) Only when the derived class does not define `__init__`
- C) Never
- D) Only when you use the `@inherit` decorator

<details><summary>✅ Answer</summary>

**B)** Only when the derived class does not define `__init__`.  
If the derived class defines `__init__`, you must call `super().__init__()` explicitly.

</details>

---

**Q20.** What does `super()` return and how is it typically used?

<details><summary>✅ Answer</summary>

`super()` returns a proxy object that delegates method calls to the **next class in the MRO**. Most commonly used as:

```python
class Derived(Base):
    def __init__(self, x, y):
        super().__init__(x)   # calls Base.__init__
        self.y = y
```

Unlike Java, `super().__init__()` does not have to be the first line.

</details>

---

**Q21.** What is the difference between `isinstance(obj, SomeClass)` and `type(obj) == SomeClass`?

<details><summary>✅ Answer</summary>

- `isinstance(obj, SomeClass)` → returns `True` if `obj` is an instance of `SomeClass` **or any subclass** of it.
- `type(obj) == SomeClass` → returns `True` only for the **exact** class, not subclasses.

Prefer `isinstance` for polymorphic checks.

</details>

---

**Q22.** What does `issubclass(Dog, Animal)` check?

<details><summary>✅ Answer</summary>

Returns `True` if `Dog` is `Animal` or a **subclass** of `Animal`. Note: `issubclass(Animal, Animal)` also returns `True`.

</details>

---

**Q23.** What is polymorphism in Python's context, and how does it differ from Java?

<details><summary>✅ Answer</summary>

In Python, polymorphism is based on **duck typing**: if an object has the right method, it can be used regardless of its class or inheritance hierarchy. No type declarations are needed. In Java, polymorphism requires a class/interface hierarchy (static type system). Python's approach is sometimes called **structural subtyping**.

</details>

---

**Q24.** TRUE or FALSE: In Python, a class can inherit from multiple base classes simultaneously.

<details><summary>✅ Answer</summary>

**TRUE** — Python supports multiple inheritance:
```python
class C(A, B):
    pass
```

</details>

---

**Q25.** What is the "Deadly Diamond of Death" problem in multiple inheritance?

<details><summary>✅ Answer</summary>

When class `D` inherits from both `B` and `C`, which both inherit from `A`, and `A`'s method is overridden differently in `B` and `C` — it's ambiguous which version `D` should use. Python solves this with the MRO (C3 linearization).

</details>

---

**Q26.** What is MRO (Method Resolution Order) and what algorithm does Python use?

<details><summary>✅ Answer</summary>

MRO is the search order for methods/attributes through a class hierarchy. Python uses **C3 linearization**, which guarantees:
1. A subclass always appears before its base classes.
2. Order of base classes in the definition is preserved.

Inspect via `ClassName.__mro__` or `ClassName.mro()`.

</details>

---

**Q27.** What is the MRO of `C` in this hierarchy?

```python
class X: pass
class Y: pass
class A(X, Y): pass
class B(X, Y): pass
class C(A, B): pass
```

<details><summary>✅ Answer</summary>

**`C → A → B → X → Y → object`**

</details>

---

**Q28.** What happens when MRO is inconsistent and Python cannot resolve it?

```python
class X: pass
class Y: pass
class A(X, Y): pass
class B(Y, X): pass
class C(A, B): pass
```

<details><summary>✅ Answer</summary>

Python raises a **`TypeError: Cannot create a consistent method resolution order (MRO)`**.  
Here `A` requires `X` before `Y`, but `B` requires `Y` before `X` — contradiction.

</details>

---

**Q29.** What does a class decorator (like `Singleton`) do differently from a function decorator?

<details><summary>✅ Answer</summary>

A class decorator takes a **class** as input and returns a **callable** that produces instances. It can intercept object creation. Example: a Singleton decorator ensures only one instance of a class ever exists.

</details>

---

**Q30.** How can a class itself be used as a **decorator factory** (creating configurable decorators)?

<details><summary>✅ Answer</summary>

Define `__init__` to accept configuration, and `__call__` to accept and wrap a function:

```python
class Logger:
    def __init__(self, filename):
        self.filename = filename  # configuration

    def __call__(self, func):     # decorating
        @functools.wraps(func)
        def wrapper(*args, **kwargs):
            result = func(*args, **kwargs)
            with open(self.filename, 'a') as f:
                f.write(f"{func.__name__} -> {result}\n")
            return result
        return wrapper

@Logger('log.txt')
def add(a, b):
    return a + b
```

</details>

---

## 📘 Chapter 11 — Context Managers & `with`

---

**Q31.** What two methods must an object implement to be a context manager?

<details><summary>✅ Answer</summary>

**`__enter__(self)`** and **`__exit__(self, exc_type, exc_value, traceback)`**

</details>

---

**Q32.** What does the `with` statement guarantee?

<details><summary>✅ Answer</summary>

`__exit__` is **always** called after the `with` block — even if an exception is raised, `return` is used, or `sys.exit()` is called. This guarantees proper cleanup (like closing files, releasing locks).

</details>

---

**Q33.** What does `with manager as value:` do?

<details><summary>✅ Answer</summary>

`value` receives whatever `__enter__` **returns**. If `__enter__` returns `self`, you can access the manager's state as `value`.

</details>

---

**Q34.** What are the three parameters of `__exit__` and what do they contain?

<details><summary>✅ Answer</summary>

```python
def __exit__(self, exc_type, exc_value, traceback):
```

- `exc_type` — type of the exception (or `None` if no exception)
- `exc_value` — exception instance (or `None`)
- `traceback` — traceback object (or `None`)

</details>

---

**Q35.** What should `__exit__` return to suppress an exception vs. re-raise it?

<details><summary>✅ Answer</summary>

- Return **`True`** → exception is **suppressed**, program continues normally.
- Return **`False`** (or `None`) → exception is **re-raised** after `__exit__` finishes.

</details>

---

**Q36.** TRUE or FALSE: You can use two context managers in a single `with` statement.

<details><summary>✅ Answer</summary>

**TRUE**:
```python
with open('in.txt') as fin, open('out.txt', 'w') as fout:
    fout.write(fin.read())
```

</details>

---

**Q37.** What is `@contextlib.contextmanager` and what structure must the decorated function have?

<details><summary>✅ Answer</summary>

It's a decorator from `contextlib` that turns a **generator function** with exactly **one `yield`** into a context manager:

```python
@contextlib.contextmanager
def managed():
    # setup (≈ __enter__)
    yield value_for_as  # execution pauses here; with-block runs
    # teardown (≈ __exit__)
```

</details>

---

**Q38.** What does this context manager do?

```python
@contextlib.contextmanager
def atBeginning(lst):
    lst.reverse()
    yield
    lst.reverse()
```

<details><summary>✅ Answer</summary>

Before the `with` block: reverses the list in place.  
After the `with` block: reverses it back.  
Temporarily makes `list.append()` add elements at the beginning instead of the end.

</details>

---

**Q39.** How do you handle exceptions inside a `@contextmanager` generator?

<details><summary>✅ Answer</summary>

Wrap `yield` in `try/except/finally`:

```python
@contextlib.contextmanager
def managed():
    setup()
    try:
        yield
    except SomeError as e:
        handle(e)
        raise  # or suppress by not re-raising
    finally:
        cleanup()  # always runs
```

</details>

---

**Q40.** TRUE or FALSE: The `open()` built-in returns a context manager, so files are automatically closed after the `with` block.

<details><summary>✅ Answer</summary>

**TRUE**  
```python
with open('file.txt') as f:
    data = f.read()
# f is closed here automatically
```

</details>

---

**Q41.** Name four standard library objects/classes that support the context manager protocol.

<details><summary>✅ Answer</summary>

Any four from: `open()` (files), `threading.Lock`, `sqlite3.connect()`, `socket.socket`, `tempfile.TemporaryFile`, `zipfile.ZipFile`, `tarfile.open`, `subprocess.Popen`, `decimal.localcontext()`, `os.scandir()`.

</details>

---

**Q42.** What does `__enter__` return in this timer context manager?

```python
class TimeIt:
    def __enter__(self):
        self.start = time.perf_counter()
        return self

    def __exit__(self, *args):
        self.time = time.perf_counter() - self.start

with TimeIt() as tim:
    do_work()

print(tim.time)
```

<details><summary>✅ Answer</summary>

`__enter__` returns **`self`** (the `TimeIt` object itself), so `tim` is the same object as the context manager. After the `with` block, `tim.time` contains the elapsed seconds.

</details>

---

**Q43.** What is the difference between a `try/finally` block and a `with` statement?

<details><summary>✅ Answer</summary>

Both guarantee cleanup code runs. `with` is more concise, reusable (define once, use anywhere), and encapsulates the setup/teardown logic in the context manager class. `try/finally` is more explicit but you must repeat the cleanup code every time.

</details>

---

## 💻 Code Tasks — Write It Yourself

---

**Task 1.** Write a class `Fraction` that supports `+`, `-`, `*`, `/`, `==`, and `__repr__`. Use GCD to simplify.

<details><summary>✅ Solution</summary>

```python
from math import gcd

class Fraction:
    def __init__(self, num, den):
        if den == 0:
            raise ValueError("Denominator cannot be zero")
        g = gcd(abs(num), abs(den))
        sign = -1 if den < 0 else 1
        self.num = sign * num // g
        self.den = sign * den // g

    def __add__(self, other):
        return Fraction(self.num * other.den + other.num * self.den,
                        self.den * other.den)

    def __sub__(self, other):
        return Fraction(self.num * other.den - other.num * self.den,
                        self.den * other.den)

    def __mul__(self, other):
        return Fraction(self.num * other.num, self.den * other.den)

    def __truediv__(self, other):
        return Fraction(self.num * other.den, self.den * other.num)

    def __eq__(self, other):
        return self.num == other.num and self.den == other.den

    def __repr__(self):
        return f"Fraction({self.num}, {self.den})"

    def __str__(self):
        return f"{self.num}/{self.den}"

a = Fraction(1, 2)
b = Fraction(1, 3)
print(a + b)   # 5/6
print(a * b)   # 1/6
print(a == Fraction(2, 4))  # True
```

</details>

---

**Task 2.** Write a context manager class `SuppressErrors(*exception_types)` that silently catches specified exception types.

<details><summary>✅ Solution</summary>

```python
class SuppressErrors:
    def __init__(self, *exceptions):
        self.exceptions = exceptions

    def __enter__(self):
        return self

    def __exit__(self, exc_type, exc_value, tb):
        if exc_type and issubclass(exc_type, self.exceptions):
            return True   # suppress
        return False      # re-raise

with SuppressErrors(ZeroDivisionError, ValueError):
    x = 1 / 0            # suppressed

with SuppressErrors(ValueError):
    raise TypeError("not suppressed")  # this will propagate
```

</details>

---

**Task 3.** Implement a `Stack` class with `push`, `pop`, `peek`, `__len__`, `__bool__`, and `__repr__`.

<details><summary>✅ Solution</summary>

```python
class Stack:
    def __init__(self):
        self._data = []

    def push(self, item):
        self._data.append(item)

    def pop(self):
        if not self._data:
            raise IndexError("pop from empty stack")
        return self._data.pop()

    def peek(self):
        if not self._data:
            raise IndexError("peek at empty stack")
        return self._data[-1]

    def __len__(self):
        return len(self._data)

    def __bool__(self):
        return len(self._data) > 0

    def __repr__(self):
        return f"Stack({self._data})"

s = Stack()
s.push(1); s.push(2); s.push(3)
print(s)        # Stack([1, 2, 3])
print(len(s))   # 3
print(bool(s))  # True
print(s.pop())  # 3
```

</details>

---

**Task 4.** PREDICT: What is the output and why?

```python
class A:
    def greet(self):
        return "Hello from A"

class B(A):
    def greet(self):
        return "Hello from B"

class C(A):
    def greet(self):
        return "Hello from C"

class D(B, C):
    pass

d = D()
print(d.greet())
print(D.__mro__)
```

<details><summary>✅ Answer</summary>

```
Hello from B
(<class '__main__.D'>, <class '__main__.B'>, <class '__main__.C'>, <class '__main__.A'>, <class 'object'>)
```

MRO: `D → B → C → A → object`. `greet` is found first in `B`.

</details>

---

**Task 5.** Write a `@contextmanager` that temporarily redirects `print` output to a list.

<details><summary>✅ Solution</summary>

```python
import contextlib
import io

@contextlib.contextmanager
def capture_output():
    import sys
    old_stdout = sys.stdout
    sys.stdout = buffer = io.StringIO()
    try:
        yield buffer
    finally:
        sys.stdout = old_stdout

with capture_output() as buf:
    print("hello")
    print("world")

lines = buf.getvalue().strip().split('\n')
print(lines)  # ['hello', 'world']
```

</details>

---

**Task 6.** Write a class `InfiniteCounter` that is iterable (using `__iter__` and `__next__`) and yields integers from `start` indefinitely.

<details><summary>✅ Solution</summary>

```python
class InfiniteCounter:
    def __init__(self, start=0):
        self.current = start

    def __iter__(self):
        return self

    def __next__(self):
        val = self.current
        self.current += 1
        return val

from itertools import islice
counter = InfiniteCounter(5)
print(list(islice(counter, 5)))  # [5, 6, 7, 8, 9]
```

</details>

---

**Task 7.** Write a Singleton decorator (as a function) that ensures only one instance of a class is ever created.

<details><summary>✅ Solution</summary>

```python
import functools

def Singleton(cls):
    instances = {}
    @functools.wraps(cls)
    def get_instance(*args, **kwargs):
        if cls not in instances:
            instances[cls] = cls(*args, **kwargs)
        return instances[cls]
    return get_instance

@Singleton
class Config:
    def __init__(self):
        self.debug = False

c1 = Config()
c2 = Config()
print(c1 is c2)  # True
```

</details>

---

**Task 8.** PREDICT: What does this print, and explain the MRO?

```python
class Base:
    def method(self):
        print("Base.method")

class Mixin:
    def method(self):
        print("Mixin.method")
        super().method()

class MyClass(Mixin, Base):
    pass

MyClass().method()
```

<details><summary>✅ Answer</summary>

```
Mixin.method
Base.method
```

MRO: `MyClass → Mixin → Base → object`.  
`MyClass().method()` calls `Mixin.method`, which calls `super().method()` — in the MRO, next after `Mixin` is `Base`, so `Base.method` is called.  
This is the classic **cooperative multiple inheritance / mixin** pattern.

</details>

---

## 🧠 Mixed Rapid-Fire Questions

---

**Q44.** What does `__new__` do, and how does it differ from `__init__`?

<details><summary>✅ Answer</summary>

- `__new__(cls, ...)` — **creates** and returns the new object. First argument is the class.
- `__init__(self, ...)` — **initializes** the already-created object. First argument is `self`.

`__new__` runs first; the object it returns is passed to `__init__` as `self`.

</details>

---

**Q45.** What does `functools.singledispatch` do?

<details><summary>✅ Answer</summary>

It allows a function to have **multiple implementations selected based on the type of the first argument** (single dispatch). Register versions with `@func.register(SomeType)`. If no registered type matches, the default implementation is called.

```python
@functools.singledispatch
def process(arg):
    raise NotImplementedError

@process.register(int)
def _(n):
    print(f"int: {n}")

@process.register(str)
def _(s):
    print(f"str: {s}")
```

</details>

---

**Q46.** What is the difference between "protected" (`_name`) and "private" (`__name`) members in Python?

<details><summary>✅ Answer</summary>

- `_name` — **convention only**: "please don't use this from outside". Interpreter does nothing special.
- `__name` — **name mangling**: Python renames it to `_ClassName__name` inside the class definition. Accessing `obj.__name` from outside raises `AttributeError`, but `obj._ClassName__name` still works. Prevents accidental collision in subclasses.

</details>

---

**Q47.** TRUE or FALSE: Python prevents you from accessing name-mangled members (`__name`) from outside the class entirely.

<details><summary>✅ Answer</summary>

**FALSE**  
You *can* still access them via the mangled name: `obj._ClassName__name`. Python makes it inconvenient, not impossible.

</details>

---

**Q48.** What is a mixin class?

<details><summary>✅ Answer</summary>

A mixin is a small class that provides a specific **reusable behavior** via multiple inheritance, without being intended for standalone use. It typically doesn't have its own `__init__` and doesn't store state. Mixins allow adding functionality to classes without deep inheritance trees.

</details>

---

**Q49.** What is the `__dict__` attribute of an object?

<details><summary>✅ Answer</summary>

A **dictionary** containing the instance's own attributes (fields) and their values. Class attributes are stored in the class's `__dict__`, not in the instance's. Use `vars(obj)` as an alias for `obj.__dict__`.

</details>

---

**Q50.** What does `callable(obj)` return?

<details><summary>✅ Answer</summary>

`True` if `obj` is callable (functions, classes, objects with `__call__`), `False` otherwise.

</details>

---

*🎉 Quiz complete! Parts 1–3 together cover the entire Python lecture from intro through context managers.*

*Topic summary by part:*
- *Part 1: Intro, types, flow control, operators, functions, lambdas, decorators*
- *Part 2: Comprehensions, generators, collections, strings, exceptions, classes basics*
- *Part 3: Magic methods, operator overloading, inheritance, MRO, class decorators, context managers*
