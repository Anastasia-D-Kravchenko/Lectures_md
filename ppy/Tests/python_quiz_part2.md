# 🐍 Python Lecture — Quiz Part 2
> Covers: Comprehensions & Generators, Collections, Strings, Exceptions, Classes (OOP)

---

## 📗 Chapter 6 — Comprehensions & Generators

---

**Q1.** What does this list comprehension produce?

```python
[x**2 for x in range(1, 10) if x % 2]
```

- A) `[1, 4, 9, 16, 25, 36, 49, 64, 81]`
- B) `[1, 9, 25, 49, 81]`
- C) `[4, 16, 36, 64]`
- D) `[1, 9, 25, 49, 81, 100]`

<details><summary>✅ Answer</summary>

**B) `[1, 9, 25, 49, 81]`**  
`x % 2` is truthy only for odd numbers. Odd numbers in `range(1, 10)` are 1, 3, 5, 7, 9.

</details>

---

**Q2.** What is the difference between `[x**2 for x in range(10)]` and `(x**2 for x in range(10))`?

<details><summary>✅ Answer</summary>

- Square brackets → **list comprehension**: creates a full list in memory immediately.
- Round parentheses → **generator expression**: creates a lazy generator that yields values on demand, consuming virtually no memory.

</details>

---

**Q3.** What does this produce?

```python
[[int(e) for e in row] for row in [['1','2'],['3','4']]]
```

<details><summary>✅ Answer</summary>

**`[[1, 2], [3, 4]]`**  
Nested list comprehension: outer iterates over rows, inner converts each element to int.

</details>

---

**Q4.** What type does a set comprehension use as delimiters?

- A) `[]`
- B) `()`
- C) `{}`
- D) `<>`

<details><summary>✅ Answer</summary>

**C) `{}`** (curly braces, same as dict comprehension, but without a colon)

</details>

---

**Q5.** What does this produce?

```python
{word for s in ['Warsaw Paris', 'Paris Berlin'] for word in s.split()}
```

<details><summary>✅ Answer</summary>

**`{'Warsaw', 'Paris', 'Berlin'}`**  
Set comprehension — no duplicates. 'Paris' appears twice but is stored once.

</details>

---

**Q6.** What does this dictionary comprehension produce?

```python
{n: 'odd' if n % 2 else 'even' for n in [1, 2, 3, 4]}
```

<details><summary>✅ Answer</summary>

**`{1: 'odd', 2: 'even', 3: 'odd', 4: 'even'}`**

</details>

---

**Q7.** TRUE or FALSE: A generator can be iterated over multiple times, just like a list.

<details><summary>✅ Answer</summary>

**FALSE**  
A generator can only be iterated **once**. Once exhausted, it stays exhausted — there is no "rewind."

</details>

---

**Q8.** What keyword turns a function into a generator function?

<details><summary>✅ Answer</summary>

**`yield`**  
The presence of `yield` inside a function makes the interpreter treat it as a generator function.

</details>

---

**Q9.** What does calling a generator function return?

- A) The first yielded value
- B) A generator object (no code executes yet)
- C) `None`
- D) A list of all values to be yielded

<details><summary>✅ Answer</summary>

**B) A generator object — no code executes yet**  
Code in the generator body only runs when `next()` is called.

</details>

---

**Q10.** What happens when a generator function executes a `return` statement?

<details><summary>✅ Answer</summary>

It raises a `StopIteration` exception, signaling that the generator is exhausted. If `return value` is used, the value is stored as an attribute of the `StopIteration` exception object.

</details>

---

**Q11.** What is the memory size difference in this example?

```python
from sys import getsizeof
lst = [x**3 for x in range(10_000_000)]
gen = (x**3 for x in range(10_000_000))
print(getsizeof(lst), getsizeof(gen))
```

- A) Both are approximately the same size
- B) `lst` is ~89 MB, `gen` is ~104 bytes
- C) `gen` is larger because it stores the computation state
- D) `lst` is smaller because lists are optimized

<details><summary>✅ Answer</summary>

**B) `lst` ~89 MB, `gen` ~104 bytes**  
The generator stores only its state (current iteration), not all values.

</details>

---

**Q12.** How do you "prime" a coroutine (a generator that uses `yield` as an expression to receive values)?

<details><summary>✅ Answer</summary>

Call `next(gen)` once (or `gen.send(None)`) before sending any data. This advances execution to the first `yield` expression where the generator is ready to receive a value.

</details>

---

**Q13.** What does `gen.send(value)` do?

<details><summary>✅ Answer</summary>

It sends `value` to the generator, making it the result of the current `yield` expression. Execution resumes until the next `yield`, whose value is returned to the caller.

</details>

---

**Q14.** What does `itertools.takewhile(lambda x: x < 5, [1,2,3,4,5,6])` return as a list?

<details><summary>✅ Answer</summary>

**`[1, 2, 3, 4]`**  
Takes elements while the predicate is True, stops as soon as it's False (at 5).

</details>

---

**Q15.** What does `itertools.dropwhile(lambda x: x < 4, [1,2,3,4,5,6])` return?

<details><summary>✅ Answer</summary>

**`[4, 5, 6]`**  
Drops elements while predicate is True, then yields all remaining elements.

</details>

---

**Q16.** What is `itertools.islice` used for?

<details><summary>✅ Answer</summary>

It allows selecting a subsequence from an iterable using `start`, `stop`, and `step` parameters — similar to slicing, but works on any iterable (including infinite generators), without requiring indexing.

</details>

---

**Q17.** What does `itertools.groupby` require before grouping, and why?

<details><summary>✅ Answer</summary>

The input iterable **must be sorted** by the key function first. `groupby` only groups **adjacent** elements with the same key — it does not collect non-adjacent matching elements.

</details>

---

## 📘 Chapter 7 — Collections

---

**Q18.** What are the three methods that make an object a full Collection?

<details><summary>✅ Answer</summary>

1. `__contains__` — supports `in` / `not in`
2. `__iter__` — iterable
3. `__len__` — supports `len()`

</details>

---

**Q19.** What is the difference between `list.sort()` and `sorted(list)`?

<details><summary>✅ Answer</summary>

- `list.sort()` — sorts **in place**, returns `None`, only works on lists
- `sorted(iterable)` — returns a **new sorted list**, works on any iterable

</details>

---

**Q20.** What will the following print?

```python
a = {1, 2, 3}
b = {2, 3, 6}
print(a | b)
print(a & b)
print(a - b)
print(a ^ b)
```

<details><summary>✅ Answer</summary>

```
{1, 2, 3, 6}   # union
{2, 3}          # intersection
{1}             # difference (in a but not b)
{1, 6}          # symmetric difference (in one but not both)
```

</details>

---

**Q21.** TRUE or FALSE: Set operators (`|`, `&`, `-`, `^`) require both operands to be sets (or frozensets). The set *methods* (`union()`, `intersection()`, etc.) accept any iterable.

<details><summary>✅ Answer</summary>

**TRUE**

</details>

---

**Q22.** What is a `frozenset` and when would you use it?

<details><summary>✅ Answer</summary>

A `frozenset` is an **immutable** set. It is hashable, so it can be used as a key in a dictionary or as an element inside another set (unlike regular `set`).

</details>

---

**Q23.** What does `d.get('key', 'default')` do differently from `d['key']`?

<details><summary>✅ Answer</summary>

`d.get('key', 'default')` returns `'default'` if the key doesn't exist, **without raising a `KeyError`**. `d['key']` raises `KeyError` if key is missing.

</details>

---

**Q24.** What does `d.setdefault('key', value)` do?

<details><summary>✅ Answer</summary>

If `'key'` exists: returns its current value (does not modify the dict).  
If `'key'` doesn't exist: inserts `'key': value` into the dict and returns `value`.

</details>

---

**Q25.** What is the output?

```python
d = {'Joan': 'Turner', 'Bob': 'Cocker'}
d1 = {'Joan': 'Baez'}
d2 = d | d1
print(d2)
```

<details><summary>✅ Answer</summary>

**`{'Joan': 'Baez', 'Bob': 'Cocker'}`**  
The `|` operator merges dicts; values from the right operand overwrite those from the left when keys clash.

</details>

---

**Q26.** How does iterating over a dictionary differ from iterating over `d.items()`?

<details><summary>✅ Answer</summary>

- `for k in d` — iterates over **keys only**
- `for k, v in d.items()` — iterates over **(key, value) tuples**

</details>

---

**Q27.** What is a `namedtuple` and what advantages does it offer over a regular tuple?

<details><summary>✅ Answer</summary>

A `namedtuple` is an immutable sequence (subclass of `tuple`) where each position has a **named field** accessible via dot notation (e.g., `point.x`). Advantages: named access, readable `__repr__`, `_asdict()`, `_replace()`, `_fields`.

</details>

---

**Q28.** What does `defaultdict(int)` do when a missing key is accessed?

<details><summary>✅ Answer</summary>

It automatically creates a new entry with that key and the value `int()` = `0`, instead of raising `KeyError`.

</details>

---

**Q29.** What does `Counter('abracadabra')` produce?

<details><summary>✅ Answer</summary>

**`Counter({'a': 5, 'b': 2, 'r': 2, 'c': 1, 'd': 1})`**

</details>

---

**Q30.** What does `counter.most_common(2)` return for `Counter({'a': 6, 'b': 10, 'c': 1})`?

<details><summary>✅ Answer</summary>

**`[('b', 10), ('a', 6)]`**  
The 2 elements with the highest counts, in descending order.

</details>

---

**Q31.** What is the time complexity of `append` and `appendleft` on a `deque`?

- A) O(n) for both
- B) O(1) for both
- C) O(1) for `append`, O(n) for `appendleft`
- D) O(log n) for both

<details><summary>✅ Answer</summary>

**B) O(1) for both**  
`deque` is a doubly linked list — O(1) operations on both ends.

</details>

---

**Q32.** What does `deque.rotate(2)` do?

<details><summary>✅ Answer</summary>

Rotates elements **2 positions to the right**: the last 2 elements move to the front.  
`deque([1,2,3,4,5]).rotate(2)` → `deque([4,5,1,2,3])`

</details>

---

**Q33.** What does the starred unpacking operator `*` do in this assignment?

```python
a, *b, c = [1, 2, 3, 4, 5]
```

<details><summary>✅ Answer</summary>

`a = 1`, `c = 5`, `b = [2, 3, 4]`  
The starred variable collects all elements not matched by the non-starred variables, as a list.

</details>

---

**Q34.** What does `{**d1, **d2}` produce (where `d1 = {'a': 1}` and `d2 = {'a': 2, 'b': 3}`)?

<details><summary>✅ Answer</summary>

**`{'a': 2, 'b': 3}`**  
Double-star unpacking merges dicts; `d2`'s value for `'a'` overwrites `d1`'s.

</details>

---

## 📙 Chapter 8 — Strings

---

**Q35.** TRUE or FALSE: Python strings are mutable — you can change individual characters.

<details><summary>✅ Answer</summary>

**FALSE**  
Strings are **immutable**. Any operation that "modifies" a string actually creates a new string object.

</details>

---

**Q36.** What is the difference between a raw string `r'\n'` and a normal string `'\n'`?

<details><summary>✅ Answer</summary>

- `'\n'` — newline character (escape sequence interpreted), length 1
- `r'\n'` — two characters: backslash and `n` (backslash is literal), length 2

</details>

---

**Q37.** What does `'barbaric'.find('bar')` and `'barbaric'.rfind('bar')` return?

<details><summary>✅ Answer</summary>

- `find('bar')` → `0` (first occurrence)
- `rfind('bar')` → `3` (last occurrence)

</details>

---

**Q38.** What is the difference between `str.find()` and `str.index()` when the substring is not found?

<details><summary>✅ Answer</summary>

- `find()` returns `-1`
- `index()` raises a `ValueError`

</details>

---

**Q39.** What does `' a\nb c\nd '.split()` return?

<details><summary>✅ Answer</summary>

**`['a', 'b', 'c', 'd']`**  
With no argument, `split()` splits on any whitespace and ignores leading/trailing whitespace, returning no empty strings.

</details>

---

**Q40.** What does `':a::b:'.split(':')` return?

<details><summary>✅ Answer</summary>

**`['', 'a', '', 'b', '']`**  
When a separator is specified, adjacent separators produce empty strings, and leading/trailing separators produce empty strings at the start/end.

</details>

---

**Q41.** What does `' '.join(['To', 'be', 'or', 'not'])` produce?

<details><summary>✅ Answer</summary>

**`'To be or not'`**

</details>

---

**Q42.** What does `'caravaggio.jpg'.endswith(('.jpg', '.png', '.gif'))` return?

<details><summary>✅ Answer</summary>

**`True`**  
`endswith()` (and `startswith()`) accept a **tuple** of suffixes/prefixes to check against.

</details>

---

**Q43.** What does `'Adam'.center(10, '.')` return?

<details><summary>✅ Answer</summary>

**`'...Adam...'`**  
Centers the string in a field of width 10, padding with `'.'` on both sides.

</details>

---

**Q44.** What does `'AA BB AAA'.replace('AA', 'X', 2)` return?

<details><summary>✅ Answer</summary>

**`'X BB XA'`**  
Replaces only the first 2 occurrences of `'AA'`.

</details>

---

## 📕 Chapter 9 — Exceptions

---

**Q45.** What is the basic structure of exception handling in Python?

<details><summary>✅ Answer</summary>

```python
try:
    # code that may raise
except SomeError:
    # handle it
except AnotherError as e:
    # handle with access to exception object
else:
    # runs if no exception was raised
finally:
    # always runs
```

</details>

---

**Q46.** TRUE or FALSE: Python exceptions have both checked and unchecked types (like Java).

<details><summary>✅ Answer</summary>

**FALSE**  
Python has **no checked exceptions**. All exceptions are unchecked — you can handle them but are not required to.

</details>

---

**Q47.** Given this hierarchy: `IndexError` is a subtype of `LookupError`. What is wrong with this code?

```python
try:
    pass
except LookupError:
    pass
except IndexError:
    pass
```

<details><summary>✅ Answer</summary>

The second `except IndexError` is **unreachable**. Because `IndexError` is a subtype of `LookupError`, all `IndexError` exceptions will be caught by the first clause. The correct order is most-specific first: `except IndexError` before `except LookupError`.

</details>

---

**Q48.** How do you access the exception object and its message?

<details><summary>✅ Answer</summary>

```python
try:
    1 // 0
except ZeroDivisionError as e:
    print(type(e), str(e))
    # or: e.args
```

</details>

---

**Q49.** What does the `finally` block do?

<details><summary>✅ Answer</summary>

It **always executes**, regardless of whether an exception was raised, caught, or not raised at all. Used for cleanup (closing files, releasing resources).

</details>

---

**Q50.** How do you manually raise an exception?

<details><summary>✅ Answer</summary>

```python
raise ValueError("message")
# or re-raise the current exception:
raise
```

</details>

---

**Q51.** What does `assert condition, "message"` do?

<details><summary>✅ Answer</summary>

If `condition` is falsy, it raises `AssertionError` with the given message. Used for debugging and invariant checks. Assertions can be disabled globally with `python -O`.

</details>

---

**Q52.** How do you define a custom exception class?

<details><summary>✅ Answer</summary>

```python
class MyError(Exception):
    pass

class MyErrorWithData(Exception):
    def __init__(self, message, code):
        super().__init__(message)
        self.code = code
```

</details>

---

## 📔 Chapter 10 — Classes

---

**Q53.** In Python, what is the role of `self` in instance methods?

<details><summary>✅ Answer</summary>

`self` is the first parameter of instance methods and receives a reference to the object the method is called on. It is equivalent to `this` in Java/C++, but must be explicitly declared in Python.

</details>

---

**Q54.** TRUE or FALSE: In Python, you must declare instance fields (like `self.x`) before using them in `__init__`.

<details><summary>✅ Answer</summary>

**FALSE**  
Instance attributes are created dynamically — simply assigning `self.x = value` inside `__init__` creates the attribute. No prior declaration is needed.

</details>

---

**Q55.** What is the difference between an instance attribute and a class (static) attribute?

<details><summary>✅ Answer</summary>

- **Instance attribute**: belongs to an individual object, stored in the object's `__dict__`. Different objects can have different values.
- **Class attribute**: belongs to the class itself (stored in the class's `__dict__`). Shared by all instances unless shadowed.

</details>

---

**Q56.** What happens when you do `obj.classattr = value` where `classattr` is a class attribute?

<details><summary>✅ Answer</summary>

It creates a **new instance attribute** on `obj` that shadows the class attribute. The class attribute itself is **not modified**. Other instances still see the original class attribute.

</details>

---

**Q57.** What decorator marks a class method, and what is its first parameter?

<details><summary>✅ Answer</summary>

`@classmethod` — first parameter is `cls` (a reference to the class, not an instance).

</details>

---

**Q58.** What decorator marks a static method, and how does it differ from a class method?

<details><summary>✅ Answer</summary>

`@staticmethod` — no hidden first argument at all (no `self` or `cls`). It's like a regular function attached to the class namespace. A class method receives the class as `cls`; a static method receives nothing implicitly.

</details>

---

**Q59.** What is "name mangling" in Python, and when does it apply?

<details><summary>✅ Answer</summary>

When a member name starts with `__` (double underscore) inside a class definition, Python renames it to `_ClassName__membername`. This makes accidental collision with subclass members less likely and prevents direct access from outside using the original name. Example: `__name` in `Person` becomes `_Person__name`.

</details>

---

**Q60.** What is a property in Python and why is it useful?

<details><summary>✅ Answer</summary>

A `@property` allows a method to behave like a public attribute. It provides getter/setter/deleter with validation, while callers use simple attribute syntax (`obj.x = 5`). The advantage is that internal implementation can change without breaking the public API.

</details>

---

**Q61.** What is the output?

```python
class Square:
    def __init__(self, a):
        self._a = float(a)

    @property
    def area(self):
        return self._a ** 2

sq = Square(5)
print(sq.area)
```

<details><summary>✅ Answer</summary>

**`25.0`**  
`sq.area` calls the getter method, returns `5.0 ** 2 = 25.0`.

</details>

---

**Q62.** What is duck typing?

<details><summary>✅ Answer</summary>

If an object has the right methods/attributes, it can be used regardless of its class. The interpreter doesn't check types before calling a method — if it works, it works. Name comes from "if it walks like a duck and quacks like a duck, it's a duck."

</details>

---

**Q63.** What is the difference between `__str__` and `__repr__`?

<details><summary>✅ Answer</summary>

- `__str__`: human-readable string (used by `print()` and `str()`)
- `__repr__`: unambiguous developer-facing string (used in REPL, `repr()`)

If `__str__` is missing, `__repr__` is used as fallback.

</details>

---

**Q64.** What magic method do you define to support the `==` operator?

<details><summary>✅ Answer</summary>

**`__eq__(self, other)`**

</details>

---

**Q65.** What magic method do you implement to make an object callable like a function?

<details><summary>✅ Answer</summary>

**`__call__(self, ...)`**

```python
class Greeter:
    def __call__(self, name):
        return f"Hello, {name}!"

g = Greeter()
print(g("Alice"))  # "Hello, Alice!"
```

</details>

---

**Q66.** In Python inheritance, when does the base class `__init__` get called automatically?

- A) Always, just before the derived class `__init__`
- B) Only if the derived class does NOT define `__init__`
- C) Never — you must always call it explicitly
- D) Only if you use `super().__init__()`

<details><summary>✅ Answer</summary>

**B) Only if the derived class does NOT define `__init__`**  
If the derived class defines `__init__`, the base class constructor is **not called automatically**. You must explicitly call `super().__init__()`.

</details>

---

**Q67.** What is MRO (Method Resolution Order) and what algorithm does Python use?

<details><summary>✅ Answer</summary>

MRO is the order in which Python searches base classes when looking up a method in multiple inheritance. Python uses the **C3 linearization algorithm**, which guarantees a consistent order that respects the "left-to-right, depth-first" principle. You can inspect it with `ClassName.__mro__` or `ClassName.mro()`.

</details>

---

**Q68.** What does `super()` do in the context of inheritance?

<details><summary>✅ Answer</summary>

`super()` returns a proxy object that delegates method calls to the **next class in the MRO**. In a derived class: `super().__init__()` calls the parent class constructor.

</details>

---

## 💻 Code Tasks — Write It Yourself

---

**Task 1.** Write a list comprehension that produces all Pythagorean triplets `(a, b, c)` where `a < b < c <= 20`.

<details><summary>✅ Solution</summary>

```python
triplets = [(a, b, c)
            for a in range(1, 19)
            for b in range(a+1, 20)
            for c in range(b+1, 21)
            if a*a + b*b == c*c]
print(triplets)  # [(3, 4, 5), (5, 12, 13), (6, 8, 10), (8, 15, 17), (9, 12, 15)]
```

</details>

---

**Task 2.** Write a generator function `primes()` that yields prime numbers indefinitely.

<details><summary>✅ Solution</summary>

```python
def primes():
    def is_prime(n):
        if n < 2: return False
        for i in range(2, int(n**0.5) + 1):
            if n % i == 0: return False
        return True
    n = 2
    while True:
        if is_prime(n):
            yield n
        n += 1

from itertools import islice
print(list(islice(primes(), 10)))  # [2, 3, 5, 7, 11, 13, 17, 19, 23, 29]
```

</details>

---

**Task 3.** Using `Counter`, count the frequency of each word in a string.

<details><summary>✅ Solution</summary>

```python
from collections import Counter

text = "to be or not to be that is the question"
counts = Counter(text.split())
print(counts.most_common(3))  # [('to', 2), ('be', 2), ...]
```

</details>

---

**Task 4.** Write a class `BankAccount` with:
- `__init__(self, owner, balance=0)`
- `deposit(amount)` — adds to balance
- `withdraw(amount)` — raises `ValueError` if insufficient funds
- `balance` as a read-only property

<details><summary>✅ Solution</summary>

```python
class BankAccount:
    def __init__(self, owner, balance=0):
        self._owner = owner
        self._balance = float(balance)

    @property
    def balance(self):
        return self._balance

    def deposit(self, amount):
        if amount <= 0:
            raise ValueError("Deposit must be positive")
        self._balance += amount

    def withdraw(self, amount):
        if amount > self._balance:
            raise ValueError("Insufficient funds")
        self._balance -= amount

    def __repr__(self):
        return f"BankAccount({self._owner!r}, balance={self._balance:.2f})"

acc = BankAccount("Alice", 100)
acc.deposit(50)
acc.withdraw(30)
print(acc)  # BankAccount('Alice', balance=120.00)
```

</details>

---

**Task 5.** Write a class `Vector2D` that supports `+`, `-`, `*` (scalar), and `==` using magic methods.

<details><summary>✅ Solution</summary>

```python
class Vector2D:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def __add__(self, other):
        return Vector2D(self.x + other.x, self.y + other.y)

    def __sub__(self, other):
        return Vector2D(self.x - other.x, self.y - other.y)

    def __mul__(self, scalar):
        return Vector2D(self.x * scalar, self.y * scalar)

    def __eq__(self, other):
        return self.x == other.x and self.y == other.y

    def __repr__(self):
        return f"Vector2D({self.x}, {self.y})"

v1 = Vector2D(1, 2)
v2 = Vector2D(3, 4)
print(v1 + v2)   # Vector2D(4, 6)
print(v1 * 3)    # Vector2D(3, 6)
print(v1 == Vector2D(1, 2))  # True
```

</details>

---

**Task 6.** PREDICT: What does this print?

```python
class A:
    x = 10

a = A()
a.x = 99
print(A.x)
print(a.x)
```

<details><summary>✅ Answer</summary>

```
10
99
```
`a.x = 99` creates an **instance attribute** that shadows the class attribute. `A.x` remains `10`.

</details>

---

**Task 7.** Write a context manager class `Timer` that measures execution time of a `with` block.

<details><summary>✅ Solution</summary>

```python
import time

class Timer:
    def __enter__(self):
        self.start = time.time()
        return self

    def __exit__(self, *args):
        self.elapsed = time.time() - self.start
        print(f"Elapsed: {self.elapsed:.4f}s")
        return False  # don't suppress exceptions

with Timer():
    total = sum(range(10_000_000))
```

</details>

---

**Task 8.** Using `defaultdict`, group a list of words by their first letter.

<details><summary>✅ Solution</summary>

```python
from collections import defaultdict

words = ['apple', 'banana', 'avocado', 'blueberry', 'cherry', 'apricot']
grouped = defaultdict(list)
for word in words:
    grouped[word[0]].append(word)

for letter, group in sorted(grouped.items()):
    print(f"{letter}: {group}")
# a: ['apple', 'avocado', 'apricot']
# b: ['banana', 'blueberry']
# c: ['cherry']
```

</details>

---

*End of Part 2. If you want Part 3 — ask! It will cover: context managers (`with`, `@contextmanager`), magic methods in depth (operator overloading, `__len__`, `__iter__`, `__getitem__`, `__enter__`/`__exit__`), inheritance & MRO, polymorphism, class decorators (`@dataclass`), and `singledispatch`.*
