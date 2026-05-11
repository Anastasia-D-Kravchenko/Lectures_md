# PPY Python Study Guide — Complete Exam Preparation

> **How to use this guide:** Read each section, then immediately do the practice questions at the bottom of each section. Think through every code snippet *before* running it — train your brain to be the compiler.

---

## Table of Contents

1. [Python Fundamentals & Types](#1-python-fundamentals--types)
2. [References, Mutability & Copying](#2-references-mutability--copying)
3. [Lists, Tuples, Sets, Dicts](#3-lists-tuples-sets-dicts)
4. [Slices](#4-slices)
5. [Comprehensions](#5-comprehensions)
6. [Functions: args, kwargs, defaults, closures](#6-functions)
7. [Iterators & Iterables](#7-iterators--iterables)
8. [Generators & yield](#8-generators--yield)
9. [Two-Way Generators (send)](#9-two-way-generators-send)
10. [Decorators](#10-decorators)
11. [OOP: Classes, Methods, Properties](#11-oop-classes-methods-properties)
12. [Magic/Dunder Methods](#12-magicdunder-methods)
13. [Inheritance & super()](#13-inheritance--super)
14. [Context Managers](#14-context-managers)
15. [Exceptions](#15-exceptions)
16. [zip, filter, map, enumerate](#16-zip-filter-map-enumerate)
17. [Collections: defaultdict](#17-collections-defaultdict)
18. [functools: wraps, singledispatch, total_ordering](#18-functools)
19. [JSON & Pickle](#19-json--pickle)
20. [File I/O](#20-file-io)
21. [Regular Expressions](#21-regular-expressions)
22. [Quick Reference: Tricky Outputs](#22-quick-reference-tricky-outputs)

---

## 1. Python Fundamentals & Types

### Core concept
In Python, **everything is an object**. Variables are **names** (labels) that point to objects in memory. The same object can have many names.

```python
x = 10          # 'x' is a name pointing to int object 10
y = x           # 'y' points to the SAME object
print(y is x)   # True — same object, same id()
x = 11          # x now points to a NEW object; y still points to 10
print(y)        # 10
```

### Types overview

| Type | Mutable? | Example |
|------|----------|---------|
| `int`, `float`, `bool`, `str` | ❌ No | `42`, `3.14`, `True`, `"hi"` |
| `tuple` | ❌ No | `(1, 2, 3)` |
| `list` | ✅ Yes | `[1, 2, 3]` |
| `dict` | ✅ Yes | `{"a": 1}` |
| `set` | ✅ Yes | `{1, 2, 3}` |

### bool is a subtype of int
```python
print(bool(22))       # True  (any nonzero number is True)
print(bool(0))        # False
print(bool([]))       # False (empty containers are False)
print(True + 1)       # 2
print(2 + bool(0.5))  # 3  (bool(0.5) == True == 1)

# f-string with bool arithmetic:
print(f'{bool(22)} + {2 + bool(0.5)}')  # True + 3
```

### Arithmetic gotchas
```python
print(2 + 2 / 2)    # 3.0  — division always returns float; PEMDAS: 2/2=1.0, 2+1.0=3.0
print(2 + 2 // 2)   # 3    — floor division returns int; 2//2=1, 2+1=3
print(-8 / 3)        # -2.666...
print(-8 // 3)       # -3  (rounds towards -infinity, NOT zero!)
```

### Strings
```python
s = r'Hello,\nWorld'   # raw string — \n is literal backslash-n
s = 'Hello,\nWorld'    # normal — \n is newline
s = f'{42}'            # f-string — evaluates expression inside {}
s = 'conca' "tenation" # adjacent string literals are concatenated
```

### ❓ Theory Questions
1. What is the difference between `==` and `is`?
2. Why does `-8 // 3` equal `-3` and not `-2`?
3. What does `id()` return? What does it represent?
4. Is `bool` a subclass of `int` in Python? What are the practical consequences?
5. What makes a value "falsy" in Python? List at least 5 falsy values.

### ✅ True/False
- `type(True) == type(1)` → **False** (`bool` vs `int`, though `bool` inherits from `int`)
- `isinstance(True, int)` → **True**
- `0 == False` → **True**
- `0 is False` → **False** (different objects)
- `-8 // 3 == -2` → **False** (it's `-3`)

### 💻 Predict the output
```python
# 1
x = 5
y = x
x += 1
print(y)   # ?

# 2
print(type(-8 // 3).__name__)  # ?

# 3
print(f'{bool(22)} + {2 + bool(0.5)}')  # ?
```
<details><summary>Answers</summary>

1. `5` — integers are immutable; `x += 1` makes x point to a new object
2. `int`
3. `True + 3`

</details>

---

## 2. References, Mutability & Copying

### The key idea
Assignment **never copies** data — it copies the **reference** (the pointer).

```python
a = [1, 2, [3, 5]]
b = a               # b and a point to the SAME list
a[2][1] = 4
print(b)            # [1, 2, [3, 4]] — b sees the change!
```

### Shallow copy vs Deep copy
```python
from copy import deepcopy

a = [1, 2, [3, 5]]

# Shallow copy: new outer list, but inner objects are shared
c = list(a)         # or a.copy() or a[:]
c.append(5)
print(c is a)       # False — different list objects
print(c[2] is a[2]) # True  — inner list is SHARED
a[2][1] = 99
print(c[2])         # [3, 99] — c sees the change in the nested list!

# Deep copy: everything is new, recursively
d = deepcopy(a)
print(d[2] is a[2]) # False
a[2][1] = 0
print(d[2])         # [3, 99] — d is independent
```

### Tuples containing mutable objects
```python
tup = ([1], ['a'])
# tup itself is immutable (can't reassign tup[0] = something)
# but the LIST inside can be mutated!
tup[0].extend([2, 3])
print(tup)          # ([1, 2, 3], ['a'])
print(id(tup[0]))   # same id as before — the list object didn't change, its contents did
```

### ❓ Theory Questions
1. What is a shallow copy? How does it differ from a deep copy?
2. Why can you mutate the contents of a tuple if it contains a list?
3. What does `is` test vs `==`?
4. Name three ways to create a shallow copy of a list.

### ✅ True/False
- After `b = a` for a list, `b is a` → **True**
- After `c = list(a)`, `c is a` → **False**
- After `c = list(a)`, `c[0] is a[0]` (where `a[0]` is a list) → **True**
- You can call `tup[0] = 5` on a tuple → **False**
- You can call `tup[0].append(5)` if `tup[0]` is a list → **True**

### 💻 Predict the output
```python
original_lst = [1, 2, 3]
new_lst = original_lst
new_lst.append(4)
print(len(original_lst))  # ?
```
<details><summary>Answer</summary>4 — both names point to the same list object.</details>

---

## 3. Lists, Tuples, Sets, Dicts

### Lists — key methods
```python
lst = []
lst.append(1)       # adds one item at the end
lst.extend([2, 3])  # adds multiple items (unpacks the iterable)
lst.insert(0, 0)    # insert(index, value) — inserts BEFORE that index
lst.pop()           # removes and returns LAST element
lst.pop(1)          # removes and returns element at index 1
lst.sort()          # sorts in-place (returns None)
sorted(lst)         # returns NEW sorted list (original unchanged)
lst.reverse()       # reverses in-place (returns None)
```

### Tuples
```python
tup = 'John', 'Doe', 1995   # packing (no parentheses needed)
fname, lname, birth = tup   # unpacking
_, x, _, y = ((1,), 2, 3, (4, 5))  # _ as placeholder

# One-element tuple needs trailing comma!
t = (42,)   # tuple
t = (42)    # just int 42 in parentheses, NOT a tuple
```

### Sets
```python
s = {1, 2, 3}       # set literal
s = set()           # empty set (NOT {} — that's a dict!)
s.add(4)
s.remove(1)         # raises KeyError if not present
s.discard(99)       # no error if not present

# Set operations
a | b   # union
a & b   # intersection
a - b   # difference (in a but not in b)
a ^ b   # symmetric difference (in one but not both)
```

### Dicts
```python
d = {}
d[1] = 'one'
d.get(2, 'default')   # returns 'default' if key missing (no error)
del d[1]
1 in d                # True/False key membership

# Last key wins on duplicates!
print({1: 'Ala', 1: 'Ma', 1: 'Kota'})  # {1: 'Kota'}

# set(dict) gives you keys (not values!)
cart = {'Milk': 10, 'Apples': 3, 'Apple Juice': 7}
print(set(cart))   # {'Milk', 'Apples', 'Apple Juice'}  — keys, order varies

# Dict comprehension
d = {i: chr(ord('A') + i) for i in range(5)}  # {0:'A', 1:'B', ...}
```

### ❓ Theory Questions
1. What is the difference between `append` and `extend`?
2. Why does `{}` create a dict, not a set?
3. What happens when you create a dict with duplicate keys?
4. How is a frozenset different from a set?
5. What does `set(dict)` give you?

### ✅ True/False
- `{1: 'Ala', 1: 'Ma', 1: 'Kota'}[1]` equals `'Kota'` → **True** (last one wins)
- `set({'Milk': 10, 'Apples': 3})` gives `{10, 3}` → **False** (gives `{'Milk', 'Apples'}`)
- `[]` is falsy → **True**
- `{0}` is falsy → **False** (non-empty set is truthy, even if `0 in it`)
- Tuples can be used as dict keys → **True** (they're hashable)
- Lists can be used as dict keys → **False** (they're mutable/unhashable)

---

## 4. Slices

### Syntax: `seq[start:stop:step]`
- All are optional; defaults: start=0, stop=end, step=1
- Negative indices count from the end: `-1` = last element
- Negative step reverses direction

```python
a = [1, 2, 3, 'a', 'b', 'Alice', 'Betty']
a[3:5]      # ['a', 'b']          — indices 3 and 4 (stop is exclusive)
a[-2:]      # ['Alice', 'Betty']  — last 2 elements
a[::2]      # [1, 3, 'b', 'Betty']— every other element
a[::-1]     # ['Betty', 'Alice', 'b', 'a', 3, 2, 1] — reversed

# Slice assignment — can change length!
a[3:6] = ['A', 'Ann']  # replaces 3 elements with 2
del a[3:5]             # deletion via slice
a[2:4] = []            # same as del a[2:4]

# chr() and uppercase alphabet trick:
[chr(i) for i in range(65, 91)][::−2][:3]  # ['Z', 'X', 'V']
```

### The alphabet example from the mock test:
```python
# [chr(i) for i in range(65, 91)] builds ['A','B',...,'Z']
# [::-2] reverses and takes every 2nd: ['Z','X','V','T','R','P','N','L','J','H','F','D','B']
# [:3] takes first 3: ['Z', 'X', 'V']
print([chr(i) for i in range(65, 91)][::-2][:3])   # ['Z', 'X', 'V']
```

### ❓ Theory Questions
1. What does `a[::-1]` do?
2. What is the difference between `a[2:4] = []` and `del a[2:4]`?
3. If `a = [1,2,3,4,5]`, what is `a[1:-1:2]`?

### 💻 Predict the output
```python
a = list('ABCDEFG')
print(a[::2])   # ?
print(a[::-1])  # ?
print(a[1:5:2]) # ?
```
<details><summary>Answers</summary>

- `['A', 'C', 'E', 'G']`
- `['G', 'F', 'E', 'D', 'C', 'B', 'A']`
- `['B', 'D']`

</details>

---

## 5. Comprehensions

### List comprehension
```python
[expression for item in iterable if condition]

squares = [x**2 for x in range(10)]
evens   = [x for x in range(20) if x % 2 == 0]
nested  = [x*y for x in range(3) for y in range(3)]
```

### Dict comprehension
```python
{key_expr: val_expr for item in iterable}

d = {i: chr(ord('A') + i) for i in range(5)}
```

### Set comprehension
```python
{expr for item in iterable}
```

### Generator expression (lazy — no brackets)
```python
gen = (x**2 for x in range(10))   # parentheses, not brackets
# NOT computed until iterated
print(type(gen))   # <generator object <genexpr> at ...>

# Used inline:
total = sum(x**2 for x in range(10))
```

### The critical distinction from mock test:
```python
# Generator expression printed directly gives object repr, NOT values
print((i for i in range(1, 10, 2)))
# <generator object <genexpr> at 0x...>

# List comprehension gives values:
print([i for i in range(1, 10, 2)])
# [1, 3, 5, 7, 9]
```

### ❓ Theory Questions
1. What is the difference between a list comprehension and a generator expression?
2. Can you use a generator expression twice? Why or why not?
3. What does `sum(x for x in range(5))` evaluate to without brackets around `x for x...`?

### 💻 Predict the output
```python
# 1 — from mock test
print((i for i in range(1, 10, 2)))   # ?

# 2
x = [i for i in range(5) if i % 2 == 0]
print(x)   # ?

# 3
d = {k: v for k, v in zip('abc', [1,2,3])}
print(d)   # ?
```
<details><summary>Answers</summary>

1. `<generator object <genexpr> at 0x...>`
2. `[0, 2, 4]`
3. `{'a': 1, 'b': 2, 'c': 3}`

</details>

---

## 6. Functions

### Default arguments — evaluated ONCE at definition time!
```python
i = 7
def f(a, b=i):     # b's default is captured as 7 RIGHT NOW
    return a + b

i += 10
print(f(1))        # 8  — NOT 18! Default was captured when def ran.

# DANGEROUS mutable default:
def funlist(val, lst=[]):   # [] created ONCE
    lst.append(val)
    return lst

funlist(1); funlist(2); funlist(3)
print(funlist(4))            # [1, 2, 3, 4]  — the same list every time!
print(funlist('c', ['a','b']))  # ['a', 'b', 'c']  — new list given
print(funlist('d'))          # [1, 2, 3, 4, 'd']  — back to the shared list!
```

### *args and **kwargs
```python
def f(a, *args, **kwargs):
    print(a)       # required positional
    print(args)    # tuple of extra positional args
    print(kwargs)  # dict of keyword args

f(1, 2, 3, x=4, y=5)
# a=1, args=(2,3), kwargs={'x':4,'y':5}

# Unpacking on call:
lst = [2, 3]
d = {'x': 4}
f(1, *lst, **d)
```

### Closures and nonlocal
```python
def outer():
    n = 0                  # this variable lives in outer's scope
    def inner():
        nonlocal n         # tells Python: use outer's n, not a new local
        n += 1
        return n
    return inner

counter = outer()
print(counter())    # 1
print(counter())    # 2
print(counter())    # 3
# counter.__closure__[0].cell_contents  → 3
```

### Functions as first-class objects
```python
def adder(a, b):
    return a + b

ref = adder         # ref IS adder — same object
print(ref is adder) # True
adder = 5           # now 'adder' points to int 5
print(ref(2, 3))    # 5 — ref still works!
print(ref.__name__) # 'adder'
```

### ❓ Theory Questions
1. Why is using a mutable default argument (like `def f(lst=[])`) considered dangerous?
2. What does `nonlocal` do? How is it different from `global`?
3. What does a function's `__closure__` attribute contain?
4. What happens when you reassign the name `adder` after saving a reference in `ref = adder`?

### 💻 Predict the output
```python
# 1
def f(a, b='B', c=None):
    print(f'{a=} {b=} {c=}')
f(1)        # ?
f(1, 2)     # ?
f(c='c', a='a', b='b')  # ?

# 2 — IMPORTANT: nested *args
def total(*args):
    nums1 = args
    def total(*args):
        nums2 = args
        def total(*args):
            nums3 = args
            return sum((*nums1, *nums2, *nums3))
        return total
    return total
print(total(10)(20)(30))   # ?
```
<details><summary>Answers</summary>

1. `a=1 b='B' c=None` / `a=1 b=2 c=None` / `a='a' b='b' c='c'`
2. `60` (10 + 20 + 30)

</details>

---

## 7. Iterators & Iterables

### The protocol
- **Iterable**: has `__iter__()` method → returns an iterator
- **Iterator**: has both `__iter__()` and `__next__()` → produces values one by one

```python
lst = [1, 2, 3]          # list is ITERABLE but not an iterator
it = iter(lst)           # it = lst.__iter__()
print(next(it))          # 1 = it.__next__()
print(next(it))          # 2
print(next(it))          # 3
next(it)                 # StopIteration!
```

### How `for` works under the hood
```python
for x in lst:
    print(x)

# Python does exactly this:
_it = iter(lst)
while True:
    try:
        x = next(_it)
    except StopIteration:
        break
    print(x)
```

### Making an object iterable — implement `__iter__()`
An object is iterable if its class implements `__iter__()`. A generator is both an iterable AND an iterator.

```python
from collections.abc import Iterable

class MyIter:
    def __init__(self, data):
        self.data = data
        self.index = 0
    def __iter__(self):
        return self           # returns itself
    def __next__(self):
        if self.index >= len(self.data):
            raise StopIteration
        val = self.data[self.index]
        self.index += 1
        return val
```

### Object iterable via `__getitem__` (no `__iter__`)
```python
class IterNoIter:
    def __getitem__(self, index):   # Python will create an iterator wrapper
        if ...: raise StopIteration
        return ...

obj = IterNoIter(...)
print(isinstance(obj, Iterable))  # False — no __iter__
i = iter(obj)                     # Python creates iterator using __getitem__
print(hasattr(i, '__next__'))     # True
```

### ❓ Theory Questions
1. What is the difference between an iterable and an iterator?
2. How does Python's `for` loop work at a protocol level?
3. Can an object be iterable without implementing `__iter__`? How?
4. What does `iter()` return when called on a list?
5. What exception signals the end of iteration?

### ✅ True/False
- A list is an iterator → **False** (it's an iterable; `iter(lst)` gives an iterator)
- A generator is an iterator → **True**
- You can call `next()` on a list → **False** (lists have no `__next__`)
- An iterator must implement `__iter__` → **True** (so it can be used in for loops)

---

## 8. Generators & yield

### What is a generator?
A function containing `yield` returns a **generator object** when called. Execution pauses at `yield` and resumes on the next `next()` call.

```python
def fibonacci(n):
    curr, prev, count = 0, 1, 0
    while count <= n:
        yield curr           # pause here, send curr to caller
        prev, curr = curr, prev + curr
        count += 1

gen = fibonacci(5)           # gen is a generator object (nothing runs yet)
print(type(fibonacci))       # <class 'function'>
print(type(gen))             # <class 'generator'>

for f in gen:
    print(f)                 # 0 1 1 2 3 5
```

### Generator vs regular function
```python
def nums_range(start=0, end=0, step=1):
    while start <= end:
        yield start          # this IS a generator (uses yield)
        start += step
```

A function with `yield` is a **generator function**. Calling it returns a **generator object** which:
- Is an **iterable** (has `__iter__`)
- Is an **iterator** (has `__next__`)
- Also implements the **iterator interface**

So `nums_range` returns a generator object which:
- ✅ returns an iterable
- ✅ returns an iterator
- ✅ returns a generator object
- ❌ does NOT return a function (it returns a generator object)

### The decorator with generator — from mock test
```python
def babys_first_deco(func):
    gen = (i for i in range(10))   # generator created once

    def wrapper(*args):
        return next(gen), func(*args)  # returns (next_gen_val, func_result)
    return wrapper

@babys_first_deco
def sum_those_numbers(num1=0, num2=0, num3=0):
    return sum((num1, num2, num3))

# @babys_first_deco means 4 usages — gen advances 4 times
# gen starts at 0, so first call returns 0, second 1, third 2, fourth 3
print(sum_those_numbers(num1=1, num2=2))    # (0, 3)
print(sum_those_numbers(num1=1, num2=3))    # (1, 4)
print(sum_those_numbers(num1=1, num2=4))    # (2, 5)
print(sum_those_numbers(num1=1, num2=5))    # (3, 6)
# Output: (0, 3) (1, 4) (2, 5) (3, 6)
```

### Generators can't be rewound
```python
gen = (x for x in range(5))
list(gen)       # [0, 1, 2, 3, 4]
list(gen)       # []  — exhausted! Must recreate.
```

### ❓ Theory Questions
1. What is a generator function? What does calling it return?
2. When does the body of a generator function start executing?
3. What happens when a generator is exhausted?
4. Can generators be iterated more than once?
5. For `nums_range` (which uses `yield`), which of these is TRUE: returns iterable / returns iterator / returns generator object / returns a function?

### ✅ True/False
- `(x for x in range(5))` creates a list → **False** (generator expression)
- A generator is an iterator → **True**
- Calling a generator function runs its body → **False** (body runs on first `next()`)
- You can use `len()` on a generator → **False** (generators have unknown length)

---

## 9. Two-Way Generators (send)

### The protocol
- `gen.send(value)` sends a value INTO the generator and also advances it
- The generator resumes and the sent value becomes the result of the `yield` expression
- You must **prime** the generator first with `next(gen)` or `gen.send(None)`

```python
def twoWayGener():
    value = None
    while True:
        x = yield value        # 1. sends 'value' out; 2. receives 'x' from caller
        value = 2 * x

gen = twoWayGener()
ret = next(gen)                # PRIMING: runs until first yield, ret = None
print(ret)                     # None

ret = gen.send(1)              # sends 1, x=1, value=2, yields 2
print(ret)                     # 2

ret = gen.send(3)              # sends 3, x=3, value=6, yields 6
print(ret)                     # 6
```

### Coroutine priming decorator
```python
import functools
def coroutine(func):
    @functools.wraps(func)
    def primed(*args, **kwargs):
        p = func(*args, **kwargs)
        next(p)   # auto-prime
        return p
    return primed

@coroutine
def longlines(n):
    while True:
        line = yield
        if len(line.split()) > n:
            print('Long line:', line)

gen = longlines(3)              # already primed by decorator
gen.send("one two three four")  # prints: Long line: one two three four
```

### ❓ Theory Questions
1. What does `gen.send(value)` do?
2. Why do you need to "prime" a generator before sending non-None values?
3. What is the difference between `next(gen)` and `gen.send(None)`?

---

## 10. Decorators

### What is a decorator?
A decorator is a function that takes a function, wraps it, and returns the wrapper. It's applied using `@` syntax.

```python
def my_deco(func):
    def wrapper(*args, **kwargs):
        print("before")
        result = func(*args, **kwargs)
        print("after")
        return result
    return wrapper

@my_deco
def greet(name):
    print(f"Hello {name}")

# @my_deco is exactly equivalent to: greet = my_deco(greet)
```

### ALWAYS use `@functools.wraps(func)` inside the wrapper
Without it, the decorated function loses its name and docstring.

```python
import functools

def deco(func):
    @functools.wraps(func)   # preserves __name__, __doc__, etc.
    def wrapper(*args, **kwargs):
        return func(*args, **kwargs)
    return wrapper

@deco
def multiply(*args):          # WITHOUT @wraps, multiply.__name__ == 'wrapper'
    res = 1
    for arg in args:
        res *= arg
    return res

print(multiply)               # <function multiply at ...>  (NOT wrapper)
```

### When does decoration happen?
At **import time** / **definition time** — when Python reads the `@deco` line, NOT when you call the function.

```python
def decorator(func):
    print('Decorating', func.__name__)  # prints when @decorator is applied
    @functools.wraps(func)
    def wrap(arg):
        val = func(arg)
        print(f'Decorated {func.__name__}: {arg} -> {val}')
        return val
    return wrap

@decorator
def funA(x): return x        # prints "Decorating funA" immediately

@decorator
def funB(x): return x*x      # prints "Decorating funB" immediately

print('*** Starting the program ***')  # this prints AFTER both "Decorating..." lines
```

### Decorator WITH parameters
```python
def rangecheck(mn, mx):            # outer: takes parameters
    def checker(func):             # middle: actual decorator
        @functools.wraps(func)
        def wrap(*args):           # inner: wrapper
            for i in args:
                if i < mn or i > mx:
                    raise ValueError(f'{i} out of range')
            return func(*args)
        return wrap
    return checker                 # returns the decorator

@rangecheck(0, 255)
def color(r, g, b):
    return f'Color({r}, {g}, {b})'
```

### Stacking decorators
```python
@italic         # applied SECOND (outer wrapper)
@strong         # applied FIRST (inner wrapper)
def itst(s):
    return s

# Equivalent to: itst = italic(strong(itst))
# Calling itst("x"): italic wraps strong's result
# strong("x") → "<strong>x</strong>"
# italic("<strong>x</strong>") → "<i><strong>x</strong></i>"
```

### Class as decorator
```python
class Logger:
    def __init__(self, filename):
        self.filename = filename

    def __call__(self, func):          # makes instance callable as a decorator
        @functools.wraps(func)
        def wrapper(*args, **kwargs):
            res = func(*args, **kwargs)
            with open(self.filename, 'a') as f:
                f.write(f'{func.__name__} -> {res}\n')
            return res
        return wrapper

@Logger('log.txt')         # Logger('log.txt') creates instance, then called with func
def f(*args):
    return sum(args)/len(args)
```

### The `__wrapped__` attribute
Using `@functools.wraps` adds `__wrapped__` pointing to the original function:
```python
adder = adder.__wrapped__   # "undecorate" — get original function back
```

### ❓ Theory Questions
1. When does the body of a decorator execute (at decoration time or call time)?
2. What does `@functools.wraps(func)` do and why is it important?
3. If decorators `@A` and `@B` are stacked, which applies first?
4. What is a decorator factory (decorator with parameters)?
5. How can a class act as a decorator?

### ✅ True/False
- `@deco` on a function applies the decorator when the function is **called** → **False** (at definition/import time)
- `@functools.wraps(func)` makes the wrapper preserve `__name__` → **True**
- Decorators can only be applied to functions, not classes → **False**
- `@A @B def f(): ...` means `f = B(A(f))` → **False** (it's `f = A(B(f))`)

### 💻 Predict the output
```python
import functools
from functools import wraps

def deco(func):
    print(f'Decorating {func.__name__}')
    @wraps(func)
    def wrap(arg):
        val = func(arg)
        print(f'Decorated: {arg} -> {val}')
        return val
    return wrap

@deco
def funA(x): return x

@deco
def funB(x): return x * x

print('*** Start ***')
funA(4)
funB(5)
```
<details><summary>Answer</summary>

```
Decorating funA
Decorating funB
*** Start ***
Decorated: 4 -> 4
Decorated: 5 -> 25
```

</details>

---

## 11. OOP: Classes, Methods, Properties

### Class vs Instance attributes
```python
class Klass:
    a = 'class_a'    # class attribute — shared by all instances

x = Klass()
x.a                  # reads class attribute (no instance attr)
x.b = 'instance_b'  # creates instance attribute on x only

Klass.a = 'new_a'   # changes class attr — ALL instances see it
y = Klass()
y.a                  # 'new_a'
```

### `__init__` vs `__new__`
- `__new__` — creates the object (allocates memory)
- `__init__` — initializes it (called after `__new__`)

### Static methods and class methods
```python
class MyClass:
    count = 0

    @staticmethod                    # no implicit first arg
    def utility(x):
        return x * 2

    @classmethod                     # 'cls' = the class itself
    def create(cls, *args):
        return cls(*args)            # can create instances of subclasses too
```

### Properties (the Pythonic getter/setter)
```python
class Square:
    def __init__(self, a):
        self._a = float(a)      # convention: _ means "internal"

    @property
    def a(self):                # getter — accessed as sq.a
        return self._a

    @a.setter
    def a(self, new_a):         # setter — sq.a = 5 calls this
        if new_a < 0: raise ValueError('No negative sides')
        self._a = float(new_a)

    @a.deleter
    def a(self):                # del sq.a calls this
        del self._a

sq = Square(5)
print(sq.a)     # 5.0
sq.a = 7        # calls setter
del sq.a        # calls deleter
```

### Name mangling (double underscore)
```python
class Person:
    def __init__(self, name):
        self.__name = name       # stored as _Person__name

p = Person('Bob')
p.__name                         # AttributeError!
p._Person__name                  # 'Bob' — accessible via mangled name
```

### ❓ Theory Questions
1. What is the difference between a class attribute and an instance attribute?
2. What is name mangling? How do you access a "private" attribute from outside?
3. What is the difference between `@staticmethod` and `@classmethod`?
4. What does `@property` do? What problem does it solve?
5. What are `__new__` and `__init__`, and what order do they run in?

---

## 12. Magic/Dunder Methods

### Essential dunder methods

| Method | When called |
|--------|-------------|
| `__init__` | Object creation |
| `__repr__` | `repr(obj)`, used in console/debugging |
| `__str__` | `str(obj)`, `print(obj)` |
| `__len__` | `len(obj)` |
| `__bool__` | `bool(obj)`, used in `if obj:` |
| `__call__` | `obj(args)` — makes instance callable |
| `__add__` | `obj + other` |
| `__radd__` | `other + obj` (reflected) |
| `__lt__`, `__eq__` | Comparisons |
| `__hash__` | `hash(obj)` — needed for dict keys/set members |
| `__iter__`, `__next__` | Iteration protocol |
| `__getitem__` | `obj[index]` |
| `__enter__`, `__exit__` | Context manager (`with` statement) |

### `__bool__` example
```python
class Person:
    def __init__(self, name, age):
        self.name = name
        self.age = age
    def __bool__(self):
        return self.age >= 18   # True for adults

lst = [Person("A", 13), Person("B", 19), Person("C", 27)]
adults = [p for p in lst if p]   # uses __bool__
```

### `__call__` example
```python
class Average:
    def __init__(self):
        self.sum = 0
        self.count = 0
    def __call__(self, val):       # av(5) calls this
        self.sum += val
        self.count += 1
        return self.sum / self.count

av = Average()
print(av(10))   # 10.0
print(av(20))   # 15.0
```

### Reflected operators (`__radd__`)
```python
# n4 + 5  → calls n4.__add__(5) ✓
# 4 + n4  → 4.__add__(n4) fails, Python tries n4.__radd__(4)

class Modulo7:
    def __add__(self, val):
        return Modulo7(self.num + val)
    def __radd__(self, other):      # called when int + Modulo7
        return Modulo7(self.num + other)
```

### `__hash__` and `__eq__` relationship
If you define `__eq__`, Python **automatically sets `__hash__ = None`** (making the object unhashable). You must also define `__hash__` if you want to use objects in sets/dicts.

```python
class A:
    def __eq__(self, other): return self.name == other.name
    def __hash__(self): return hash(self.name)  # must define both!
```

### ❓ Theory Questions
1. What is the difference between `__str__` and `__repr__`?
2. When is `__radd__` called?
3. Why does defining `__eq__` automatically make a class unhashable?
4. What does `__call__` allow you to do with a class instance?
5. What is `@functools.total_ordering` and why is it useful?

---

## 13. Inheritance & super()

```python
class Base:
    def __init__(self):
        print('init of Base')

class DerivNoInit(Base):
    pass                        # inherits Base.__init__

class DerivWithInit(Base):
    def __init__(self):
        print('init of DerivWithInit')  # does NOT call Base.__init__

class DerivCallingBase(Base):
    def __init__(self):
        super().__init__()      # explicitly calls Base.__init__
        print('init of DerivCallingBase')

DerivNoInit()          # prints: init of Base
DerivWithInit()        # prints: init of DerivWithInit (Base NOT called!)
DerivCallingBase()     # prints: init of Base
                       #         init of DerivCallingBase
```

### Duck typing
Python doesn't check types — it checks if an object has the required method (protocol).
```python
class Chair:
    def legs(self):
        print('Chair; 4 legs')

for animal in [Snake('Basil'), Animal(), Dog('Buddy'), Chair()]:
    animal.legs()    # Chair works too — it has a legs() method!
```

### Polymorphism
```python
class Dog(Animal):
    def legs(self): print(f'Dog {self.name}; 4 legs')

class Snake(Animal):
    def legs(self): print(f'Snake {self.name}; 0 legs')

animals = [Dog('Rex'), Snake('Naja')]
for a in animals:
    a.legs()    # calls the appropriate version — polymorphism
```

---

## 14. Context Managers

### The `with` statement protocol
```python
with expr as var:
    body

# Python calls:
# var = expr.__enter__()
# try: body
# finally: expr.__exit__(exc_type, exc_val, traceback)
```

If `__exit__` returns `True`, exceptions are suppressed.

```python
class TimeIt:
    def __enter__(self):
        import time
        self.start = time.perf_counter()
        return self        # this is what 'as tim' binds to

    def __exit__(self, exc_type, exc_val, trace):
        import time
        self.time = time.perf_counter() - self.start
        # return None (falsy) → don't suppress exceptions

with TimeIt() as tim:
    some_slow_function()
print(f'{tim.time:.2f} sec')
```

### `@contextlib.contextmanager` — generator-based context managers
```python
import contextlib

@contextlib.contextmanager
def WithLocale(loc):
    old = locale.getlocale()
    locale.setlocale(locale.LC_ALL, loc)
    yield                    # code in 'with' block runs here
    locale.setlocale(locale.LC_ALL, old)   # cleanup always runs
```

### ❓ Theory Questions
1. What methods must a class implement to be a context manager?
2. When does `__exit__` get called — only on exception, or always?
3. How do you suppress an exception inside `__exit__`?
4. How does `@contextlib.contextmanager` work?

---

## 15. Exceptions

```python
try:
    result = risky()
except ValueError as e:
    print('Value error:', e)
except (TypeError, KeyError):
    print('Type or Key error')
except Exception as e:
    print('Any exception:', type(e).__name__)
else:
    print('No exception occurred')  # runs only if no exception
finally:
    print('Always runs')            # runs regardless
```

### Re-raising
```python
try:
    do_something()
except Exception as e:
    print('Caught:', e)
    raise       # re-raises the same exception
```

### Custom exceptions
```python
class OutOfRange(ValueError):
    def __init__(self, val, lower, upper):
        super().__init__(f'Got {val}, expected [{lower}, {upper}]')

raise OutOfRange(13, 1, 12)
```

### Using exceptions as control flow (breaking nested loops)
```python
try:
    for a in range(...):
        for b in range(...):
            for c in range(...):
                if condition:
                    raise Exception(a, b, c)   # "break" from all loops
except Exception as ex:
    result = ex.args
else:
    result = (-1, -1, -1)   # only if no exception was raised
```

---

## 16. zip, filter, map, enumerate

### zip
```python
# zip pairs elements from multiple iterables
for x, y in zip({'hi': 'there', 'dear': 'friends'}, [4, 5, 6]):
    print(x, y)
# Output: hi 4
#         dear 5
# Note: dict iteration gives KEYS, not values!
# zip stops at the shortest iterable
```

### zip with generator expression
```python
for x, y in zip((i for i in range(3)), [4, 5, 6]):
    print(x, y)
# 0 4
# 1 5
# 2 6
```

### enumerate
```python
seasons = ["Spring", "Summer", "Autumn", "Winter"]
for ind, s in enumerate(seasons, 1):   # start counting from 1
    print(f'Season {ind}: {s}')
```

### filter
```python
filter(None, lst)         # removes falsy values
filter(func, lst)         # keeps items where func(item) is True
# Returns a filter OBJECT (lazy) — iterate or convert to list
list(filter(lambda x: x > 0, [-1, 2, -3, 4]))   # [2, 4]
```

### The `len()` with `randint` from mock test
```python
from random import randint
n = 5
print(len([randint(a=1, b=5) for _ in range(1, 11) if n]))
# range(1, 11) = 10 items; 'if n' is 'if 5' which is True
# so all 10 items pass the filter → list has 10 elements
# len = 10
```

### 💻 Predict the output
```python
# 1 — from mock test
for x, y in zip({'hi': 'there', 'dear': 'friends'}, [4, 5, 6]):
    print(x, y)

# 2
for x, y in zip((i for i in range(3)), [4, 5, 6]):
    print(x, y)
```
<details><summary>Answers</summary>

1. `hi 4` then `dear 5` (dict gives keys; zip stops after 2 pairs)
2. `0 4` then `1 5` then `2 6`

</details>

---

## 17. Collections: defaultdict

```python
from collections import defaultdict

# defaultdict(factory) — creates missing keys automatically
dd = defaultdict(list)    # missing key → []
dd['x'].append(1)         # no KeyError!

dd = defaultdict(int)     # missing key → 0
dd['count'] += 1          # no KeyError!

dd = defaultdict(set)     # missing key → set()
dd['flights'].add('Paris')
```

---

## 18. functools

### `functools.wraps`
Preserves decorated function's `__name__`, `__doc__`, etc. Also adds `__wrapped__` pointing to the original.

### `functools.singledispatch` — function overloading by type
```python
import functools

@functools.singledispatch
def aFunc(arg):              # default implementation
    raise NotImplementedError(f'No handler for {type(arg)}')

@aFunc.register(int)
def _(v):
    print('int:', v)

@aFunc.register(float)
def _(v1, v2):
    print('float:', v1, v2)

@aFunc.register(list | tuple)  # Python 3.10+
def _(arg):
    print('list/tuple:', arg)

aFunc(5)            # calls int version
aFunc(True)         # bool → int (dispatch by type hierarchy)
aFunc([1, 2, 3])    # calls list version
```

### `functools.singledispatchmethod` — same but for methods
```python
class Alice:
    @singledispatchmethod
    def interact(self, target):
        return "No interaction"

    @interact.register(Cake)
    def _(self, target):
        self.size += 1
```

### `functools.total_ordering`
Provide `__eq__` and one of `__lt__`, `__le__`, `__gt__`, `__ge__` — decorator generates the rest.

---

## 19. JSON & Pickle

### JSON — text-based, human-readable, limited types
```python
import json

# Python → JSON string
json.dumps(obj)
json.dumps(obj, ensure_ascii=False, indent=4)

# Python → JSON file
with open('data.json', 'w', encoding='utf-8') as f:
    json.dump(obj, f)

# JSON string → Python
json.loads(string)

# JSON file → Python
with open('data.json', 'r', encoding='utf-8') as f:
    data = json.load(f)

# Note: JSON converts dict keys to strings!
# {1: 'one'} → {"1": "one"}
# tuples → lists
```

### Custom JSON encoder/decoder
```python
def my_encoder(obj):
    if isinstance(obj, complex):
        return {'cplx': [obj.real, obj.imag]}
    raise TypeError(f'Object of type {type(obj)} not JSON serializable')

def my_decoder(dct):
    if 'cplx' in dct:
        return complex(dct['cplx'][0], dct['cplx'][1])
    return dct

jsn = json.dumps(orig, default=my_encoder)
restored = json.loads(jsn, object_hook=my_decoder)
```

### Pickle — binary, Python-only, preserves object graph
```python
import pickle

# Serialize
data = pickle.dumps(obj)    # to bytes
pickle.dump(obj, file)      # to file

# Deserialize
obj = pickle.loads(data)    # from bytes
obj = pickle.load(file)     # from file

# Pickle preserves references!
# p1 = Person(); p2 = Person(brother=p1); p1.brother = p2
# After pickle/unpickle, p1r is still p2r.brother!
```

---

## 20. File I/O

```python
# Text modes: 'r', 'w', 'a', 'r+', 'w+'
# Binary modes: 'rb', 'wb', 'r+b', 'w+b'

with open('file.txt', 'r') as f:
    content = f.read()         # entire file as string
    f.seek(0)
    lines = f.readlines()      # list of lines
    f.seek(0)
    for line in f:             # iterate line by line (memory-efficient)
        pass

with open('file.txt', 'w') as f:
    f.write('hello\n')
    f.writelines(['a\n', 'b\n'])
```

---

## 21. Regular Expressions

```python
import re

# Functions
re.match(pattern, string)          # match at beginning only
re.search(pattern, string)         # match anywhere
re.fullmatch(pattern, string)      # must match entire string
re.findall(pattern, string)        # list of all matches
re.finditer(pattern, string)       # iterator of match objects
re.sub(pattern, repl, string)      # replace matches

# Common patterns
\d       # digit [0-9]
\w       # word char [a-zA-Z0-9_]
\s       # whitespace
\b       # word boundary
.        # any char except newline
*        # 0 or more
+        # 1 or more
?        # 0 or 1
{n,m}    # n to m times
^        # start of string
$        # end of string
[abc]    # character class
[^abc]   # negated character class
(...)    # capturing group
(?:...)  # non-capturing group
(?P<name>...)  # named group
```

### Groups
```python
m = re.fullmatch(r'([A-Z][a-z]+)\s+([A-Z][a-z]+)', 'John Smith')
m.group(0)       # 'John Smith'  (entire match)
m.group(1)       # 'John'
m.group(2)       # 'Smith'
m.groups()       # ('John', 'Smith')
```

---

## 22. Quick Reference: Tricky Outputs

| Code | Output | Why |
|------|--------|-----|
| `print(2 + 2 / 2)` | `3.0` | PEMDAS: `/` before `+`, `/` → float |
| `print(bool(22))` | `True` | Any nonzero → True |
| `print(f'{bool(22)} + {2 + bool(0.5)}')` | `True + 3` | bool(0.5)=True=1; 2+1=3 |
| `print({1:'Ala', 1:'Ma', 1:'Kota'})` | `{1: 'Kota'}` | Last value wins |
| `print(set({'Milk':10,'Apples':3,'AJ':7}))` | `{'Milk','Apples','AJ'}` | set(dict) = keys |
| `(i for i in range(1,10,2))` | `<generator object...>` | Gen expr isn't a list |
| `[chr(i) for i in range(65,91)][::-2][:3]` | `['Z','X','V']` | Reverse every 2nd, first 3 |
| `zip({'a':1,'b':2},[4,5,6])` | `('a',4), ('b',5)` | Dict → keys; stops at shorter |
| `for x,y in zip((i for i in range(3)),[4,5,6]): print(x,y)` | `0 4 / 1 5 / 2 6` | gen range(3) = 0,1,2 |

### The decorator timing gotcha
```python
import functools
def deco(func):
    print(f'Decorating {func.__name__}')   # RUNS AT IMPORT TIME
    @functools.wraps(func)
    def wrap(arg):
        val = func(arg)
        print(f'{arg} -> {val}')           # RUNS AT CALL TIME
        return val
    return wrap

@deco
def funA(x): return x    # "Decorating funA" prints NOW

print('Start')            # prints AFTER both decorations
funA(4)                   # "4 -> 4" prints NOW
```

### The `functools.wraps` effect on name
```python
from functools import wraps

def deco(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        return func(*args, **kwargs)
    return wrapper

@deco
def multiply(*args):
    ...

print(multiply)   # <function multiply at ...>   ← shows 'multiply', NOT 'wrapper'
```

---

## Write These Snippets from Memory

Practice writing these without looking:

### 1. Simple decorator with `functools.wraps`
```python
import functools
def my_deco(func):
    @functools.wraps(func)
    def wrapper(*args, **kwargs):
        # add behavior here
        return func(*args, **kwargs)
    return wrapper
```

### 2. Decorator with parameters
```python
def repeat(n):
    def decorator(func):
        @functools.wraps(func)
        def wrapper(*args, **kwargs):
            for _ in range(n):
                result = func(*args, **kwargs)
            return result
        return wrapper
    return decorator
```

### 3. Generator with yield
```python
def my_range(start, end, step=1):
    while start <= end:
        yield start
        start += step
```

### 4. Class with property
```python
class Square:
    def __init__(self, a):
        self._a = float(a)
    
    @property
    def a(self):
        return self._a
    
    @a.setter
    def a(self, value):
        if value < 0:
            raise ValueError("No negative sides")
        self._a = float(value)
```

### 5. Context manager class
```python
class Timer:
    def __enter__(self):
        import time
        self.start = time.perf_counter()
        return self
    
    def __exit__(self, *args):
        import time
        self.elapsed = time.perf_counter() - self.start
```

### 6. Custom iterator
```python
class Counter:
    def __init__(self, max_val):
        self.max_val = max_val
        self.current = 0
    
    def __iter__(self):
        return self
    
    def __next__(self):
        if self.current >= self.max_val:
            raise StopIteration
        self.current += 1
        return self.current
```

### 7. singledispatch
```python
from functools import singledispatch

@singledispatch
def process(arg):
    raise NotImplementedError(f"Unsupported type: {type(arg)}")

@process.register(int)
def _(v):
    return v * 2

@process.register(str)
def _(s):
    return s.upper()
```

---

*Good luck on your exam! The best preparation is running every code snippet yourself and explaining the output out loud before seeing it.*
