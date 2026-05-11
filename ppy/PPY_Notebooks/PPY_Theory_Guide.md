# PPY – Complete Theory & Study Guide
> All 21 topics from the course, exam patterns highlighted with ⚠️

---

## 1 · Types, Objects & References

Python variables are **labels** (references) pointing to objects in memory.

```python
x = [1, 2, 3]
y = x          # SAME object — both labels point to one list
y.append(4)
print(x)       # [1, 2, 3, 4]  ← x is affected!

x = [9]        # x now points to a NEW list; y unchanged
print(y)       # [1, 2, 3, 4]
print(x is y)  # False
```

### Shallow vs Deep copy
```python
from copy import deepcopy
orig    = [[1, 2], [3, 4]]
shallow = list(orig)          # or orig[:]
deep    = deepcopy(orig)

orig[0][0] = 99
print(shallow[0][0])  # 99  — inner list is SHARED
print(deep[0][0])     # 1   — fully independent

orig[1] = [9, 9]
print(shallow[1])     # [3, 4]  — top-level rebind not reflected
print(deep[1])        # [3, 4]
```

### Tuples with mutable content
```python
tup = ([1], ['a'])
tup[0].extend([2, 3])  # OK — mutating the list itself
print(tup)             # ([1, 2, 3], ['a'])
tup[0] = [9]           # TypeError — can't rebind tuple slot
```

### ⚠️ Exam: bool subtype
```python
type(True).__name__    # 'bool'
isinstance(True, int)  # True — bool IS-A int
type(True) == int      # False — type is exactly bool
bool([0])              # True — non-empty list is truthy
bool('')               # False
bool({})               # False
```

---

## 2 · Operators & Arithmetic

### Floor division & modulo traps
```python
9 / 3      # 3.0   — always float
9 // 3     # 3     — int
-5 // 4    # -2    — floor (toward -∞), NOT truncate
-5 % 4     # 3     — formula: a - (a//b)*b  →  -5 - (-2*4) = 3
17 % -10   # -3
2 ** -8    # 0.00390625  — float
~321       # -322   — ~n = -(n+1)
```

### Chained comparisons
```python
5 < 7 > 2   # True  — equivalent to (5<7) and (7>2)
5 < 7 > 8   # False — (5<7) and (7>8)
```

### Short-circuit: return VALUES not bool
```python
-1 and [1, 2]   # [1, 2]   — truthy and → right side
[] or {1, 2}    # {1, 2}   — falsy or  → right side
[] and {1, 2}   # []       — falsy and → left side (short-circuit)
5 or 99         # 5        — truthy or → left side
0 or 'fallback' # 'fallback'
```

### ⚠️ bool arithmetic (mock test pattern)
```python
bool(22)          # True
bool(0)           # False
bool([0])         # True  ← non-empty list!
True + True + True  # 3
2 + bool(0.5)     # 3   (bool(0.5) == True == 1)
f'{bool(22)} + {2 + bool(0.5)}'  # 'True + 3'
```

---

## 3 · Flow Control

### for/else
```python
# else runs ONLY if loop completed without break
for n in [2, 4, 6]:
    if n > 10: break
else:
    print('no break')   # prints

for n in [2, 4, 6, 15]:
    if n > 10: break
else:
    print('no break')   # does NOT print
```

### Ternary evaluation order
```python
a, b = 5, 0
c = a // b if a > 6 else 3 * a   # condition False → 3*5=15, no ZeroDivisionError
```

### ⚠️ Modifying a list during iteration
```python
nums = [2, 1, 3, 4, 5, 6]
for n in nums:
    if n % 2 != 0:
        nums.remove(n)   # BUG: skips elements
print(nums)   # [2, 4, 6] is WRONG — may skip

# Fix: iterate over a copy
for n in nums[:]:
    if n % 2 != 0:
        nums.remove(n)
```

---

## 4 · Strings

### Key methods
```python
s = 'barbaric'
s.find('bar')           # 0   — first occurrence
s.rfind('bar')          # 3   — last occurrence
s.count('a')            # 2
s.replace('a','X',1)    # 'bXrbaric'  — only 1 replacement

' a\nb c\nd '.split()          # ['a','b','c','d']  any whitespace, no empties
':a::b:'.split(':')            # ['','a','','b','']  empties preserved
'Adam'.center(10,'.')          # '...Adam...'
'img.jpg'.endswith(('.jpg','.png'))  # True — accepts tuple
```

### f-string formatting
```python
x = 42; pi = 3.14159; name = 'Alice'
f'{x=}'        # 'x=42'   — debug format
f'{pi:.2f}'    # '3.14'
f'{name!r}'    # "'Alice'"  — forces repr
f'{name:>10}'  # '     Alice'  — right-align width 10
```

---

## 5 · Collections

### ⚠️ Dict traps
```python
{1:'Ala', 1:'Ma', 1:'Kota'}   # {'Kota'} — last value wins
d = {'Milk':10, 'Apples':3}
set(d)                          # {'Milk','Apples'} — set(dict) = KEYS

d1 = {'a':1,'b':2}
d2 = {'b':99,'c':3}
d1 | d2   # {'a':1,'b':99,'c':3} — right side wins on collision
```

### Starred unpacking
```python
a, *b, c = [1,2,3,4,5]   # a=1, b=[2,3,4], c=5
first, *rest = 'hello'    # first='h', rest=['e','l','l','o']
{**{'a':1}, **{'a':2,'b':3}}  # {'a':2,'b':3}
```

### ⚠️ One-element tuple
```python
t1 = (42,)   # tuple
t2 = (42)    # int — comma is what makes it a tuple!
```

---

## 6 · Slices

```python
a = list(range(10))   # [0,1,2,3,4,5,6,7,8,9]
a[2:5]    # [2,3,4]
a[-3:]    # [7,8,9]
a[::2]    # [0,2,4,6,8]
a[::-1]   # [9,8,7,6,5,4,3,2,1,0]
a[1::2]   # [1,3,5,7,9]
a[1:-1:2] # [1,3,5,7]
```

### ⚠️ Mock test: alphabet slice
```python
alphabet = [chr(i) for i in range(65, 91)]   # A..Z
alphabet[::-2]       # ['Z','X','V','T','R','P','N','L','J','H','F','D','B']
alphabet[::-2][:3]   # ['Z','X','V']
```

---

## 7 · Functions: LEGB, args/kwargs, closures

### LEGB rule
Local → Enclosing → Global → Built-in

### ⚠️ UnboundLocalError trap
```python
n = 1
def fun():
    n = n + 7   # UnboundLocalError: n referenced before assignment
                # Python sees assignment → treats n as local throughout

def fun2():
    global n    # fix: declare global
    n = n + 7

def outer():
    x = 10
    def inner():
        nonlocal x   # fix: declare nonlocal for enclosing scope
        x += 5
    inner()
    return x   # 15
```

### ⚠️ Mutable default argument trap
```python
def funlist(val, lst=[]):   # [] created ONCE at definition
    lst.append(val)
    return lst

funlist(1)   # [1]
funlist(2)   # [1, 2]  ← shared!
funlist(3)   # [1, 2, 3]
funlist('c', ['a','b'])  # ['a','b','c']  — new list passed explicitly
funlist('d')  # [1, 2, 3, 'd']  ← back to shared!

# Fix: use None as default
def funlist(val, lst=None):
    if lst is None: lst = []
    lst.append(val)
    return lst
```

### ⚠️ Default captured at definition time
```python
i = 7
def f(a, b=i):   # b=7 captured NOW
    return a + b
i += 10
f(1)     # 8  (b still 7)
f(1, i)  # 18 (passing current i explicitly)
```

### *args and **kwargs
```python
def show(required, *args, **kwargs):
    print(f'{required=}, {args=}, {kwargs=}')

show(1)              # required=1, args=(), kwargs={}
show(1, 2, 3)        # required=1, args=(2,3), kwargs={}
show(1, x=4, y=5)   # required=1, args=(), kwargs={'x':4,'y':5}
show(1, *[2,3], **{'x':4})  # unpacking
```

### Closures
```python
def outer():
    a = 10
    b = 20
    def innA(arg): return arg + a           # captures: a
    def innB(arg): return arg + b           # captures: b
    def innC(arg): return arg + a + b       # captures: a, b
    return innA, innB, innC

fA, fB, fC = outer()
len(fA.__closure__)   # 1
len(fC.__closure__)   # 2
fA.__closure__[0].cell_contents  # 10
fC(5)   # 35

# Counter closure
def getCounter():
    n = 0
    def count():
        nonlocal n
        n += 1
        return n
    return count

f = getCounter()
f(), f(), f()   # 1, 2, 3
f.__closure__[0].cell_contents  # 3
```

---

## 8 · Lambdas, map, filter, reduce

```python
from functools import reduce

list(map(lambda x: x**x, [1,2,3,4]))        # [1,4,27,256]
list(filter(None, [1,5,0,'',{},2]))          # [1,5,2]  — falsy removed
list(filter(lambda x: x>0, [-1,2,-3,4]))     # [2,4]
reduce(lambda x,y: x*y, [1,2,3,4,5])        # 120

pal = lambda s: 'palindrome' if s==s[::-1] else 'not'
pal('rotor')   # 'palindrome'
pal('python')  # 'not'
```

### Common one-liners
```python
# squares of even numbers 1..10
list(map(lambda x: x**2, filter(lambda x: x%2==0, range(1,11))))
# [4,16,36,64,100]

# sorted by last character
sorted(['banana','apple','cherry','date'], key=lambda s: s[-1])
```

---

## 9 · Comprehensions

```python
[x**2 for x in range(1,10) if x%2]   # odd squares: [1,9,25,49,81]

{w for s in ['Warsaw Paris','Paris Berlin']
   for w in s.split()}               # {'Warsaw','Paris','Berlin'}

{n: 'odd' if n%2 else 'even'
   for n in [1,2,3,4]}               # {1:'odd',2:'even',3:'odd',4:'even'}
```

### Generator expressions (lazy)
```python
gen = (x**2 for x in range(5))
print(gen)        # <generator object ...>
print(type(gen))  # <class 'generator'>
print(list(gen))  # [0,1,4,9,16]
print(list(gen))  # []  — exhausted!
```

### ⚠️ Size difference
```python
from sys import getsizeof
lst = [x**3 for x in range(1000)]   # ~8000+ bytes
gen = (x**3 for x in range(1000))   # ~120 bytes — lazy!
```

### Pythagorean triplets
```python
[(a,b,c) for c in range(1,21)
          for b in range(1,c)
          for a in range(1,b)
          if a**2 + b**2 == c**2]
```

---

## 10 · Iterators & Iterables

```python
from collections.abc import Iterable, Iterator

lst = [1,2,3]
isinstance(lst, Iterable)   # True  — has __iter__
isinstance(lst, Iterator)   # False — no __next__

it = iter(lst)
isinstance(it, Iterable)    # True
isinstance(it, Iterator)    # True — has both

hasattr(lst, '__next__')    # False
hasattr(it, '__next__')     # True
```

### Iterable via `__getitem__` (no `__iter__`)
```python
class IterNoIter:
    def __init__(self, data): self.data = data
    def __getitem__(self, index):
        if index >= len(self.data): raise StopIteration
        return self.data[index]

obj = IterNoIter([10,20,30])
isinstance(obj, Iterable)   # False — no __iter__
i = iter(obj)               # Python creates wrapper
hasattr(i, '__next__')      # True
for n in obj: print(n)      # 10 20 30  — works!
```

---

## 11 · Generators & yield

```python
def nums_range(start=0, end=0, step=1):
    while start <= end:
        yield start
        start += step

r = nums_range(0, 5)
type(nums_range)   # <class 'function'>
type(r)            # <class 'generator'>
# A generator object IS both iterable and iterator
# Calling the function returns a generator object; it does NOT run the body
```

### ⚠️ Generator exhaustion
```python
def fibonacci():
    a, b = 0, 1
    while True:
        yield a
        a, b = b, a + b

from itertools import islice
fib = fibonacci()
list(islice(fib, 10))   # [0,1,1,2,3,5,8,13,21,34]
list(islice(fib, 5))    # [55,89,144,233,377]  — continues from where it stopped!
```

---

## 12 · Two-Way Generators (send)

```python
def twoWayGener():
    value = None
    while True:
        x = yield value      # yield sends value OUT, receives x from send()
        value = 2 * x

gen = twoWayGener()
next(gen)        # MUST prime: runs until first yield, returns None
gen.send(1)      # resumes: x=1, value=2, yields 2
gen.send(3)      # resumes: x=3, value=6, yields 6
```

### Priming decorator
```python
import functools

def coroutine(func):
    @functools.wraps(func)
    def primed(*args, **kwargs):
        gen = func(*args, **kwargs)
        next(gen)    # auto-prime
        return gen
    return primed

@coroutine
def longlines(n):
    while True:
        line = yield
        if len(line.split()) > n:
            print('Long:', line)

gen = longlines(3)           # already primed
gen.send('one two')          # silent
gen.send('one two three four five')  # prints
```

---

## 13 · Decorators

### ⚠️ Decoration timing
```python
def decorator(func):
    print(f'Decorating {func.__name__}')   # runs at IMPORT/definition time
    @functools.wraps(func)
    def wrap(arg):
        val = func(arg)
        print(f'{func.__name__}: {arg} -> {val}')   # runs at CALL time
        return val
    return wrap

@decorator
def funA(x): return x      # "Decorating funA" prints NOW

@decorator
def funB(x): return x*x    # "Decorating funB" prints NOW

print('*** Starting ***')
funA(4)    # "funA: 4 -> 4"
funB(5)    # "funB: 5 -> 25"
```

### functools.wraps
```python
import functools

def deco_without_wraps(func):
    def wrapper(*args, **kwargs): return func(*args, **kwargs)
    return wrapper

def deco_with_wraps(func):
    @functools.wraps(func)
    def wrapper(*args, **kwargs): return func(*args, **kwargs)
    return wrapper

@deco_without_wraps
def f(): 'my docstring'
f.__name__  # 'wrapper'    ← wrong!
f.__doc__   # None         ← lost!

@deco_with_wraps
def g(): 'my docstring'
g.__name__              # 'g'              ← preserved
g.__doc__               # 'my docstring'   ← preserved
hasattr(g,'__wrapped__')  # True — points to original
g.__wrapped__(3, 4)     # call original directly
```

### ⚠️ Stacked decorators (bottom-up application)
```python
@italic    # applied SECOND (outer)
@strong    # applied FIRST (inner)
def itst(s): return s
# equivalent to: itst = italic(strong(itst))

itst('x')   # <i><strong>x</strong></i>

@strong
@italic
def stit(s): return s
stit('x')   # <strong><i>x</i></strong>
```

### Decorator patterns
```python
# Timer
def timer(func):
    @functools.wraps(func)
    def wrapper(*args, **kwargs):
        import time
        t0 = time.perf_counter()
        result = func(*args, **kwargs)
        print(f'{func.__name__}: {time.perf_counter()-t0:.4f}s')
        return result
    return wrapper

# Call counter
def counting(func):
    @functools.wraps(func)
    def wrapper(*args, **kwargs):
        wrapper.counter += 1
        return func(*args, **kwargs)
    wrapper.counter = 0
    return wrapper

# Range check factory
def rangecheck(mn, mx):
    def decorator(func):
        @functools.wraps(func)
        def wrapper(*args, **kwargs):
            for v in list(args) + list(kwargs.values()):
                if isinstance(v, (int,float)) and not (mn <= v <= mx):
                    raise ValueError(f'{v} out of [{mn},{mx}]')
            return func(*args, **kwargs)
        return wrapper
    return decorator

# Singleton
def Singleton(cls):
    instances = {}
    @functools.wraps(cls)
    def get_instance(*args, **kwargs):
        if cls not in instances:
            instances[cls] = cls(*args, **kwargs)
        return instances[cls]
    return get_instance
```

---

## 14 · Classes, Attributes, Properties

### Class vs instance attributes
```python
class Klass:
    a = 'class_a'

x, y = Klass(), Klass()
Klass.a = 'new_a'
x.a   # 'new_a'  — reads class attr
y.a = 'instance_a'   # creates instance attr on y only
x.a   # 'new_a'  — class attr
y.a   # 'instance_a'  — instance attr shadows class
vars(x)  # {}
vars(y)  # {'a':'instance_a'}
```

### `__new__` vs `__init__`
```python
class AClass:
    def __new__(cls, *args, **kwargs):
        print('__new__')        # called FIRST
        return object.__new__(cls)
    def __init__(self, *args, **kwargs):
        print('__init__')       # called SECOND
```

### Properties
```python
from math import sqrt

class Square:
    def __init__(self, a): self._a = float(a)

    @property
    def a(self): return self._a

    @a.setter
    def a(self, value):
        if value < 0: raise ValueError('No negative sides')
        self._a = float(value)

    @property
    def area(self): return self._a ** 2

    @area.setter
    def area(self, value): self._a = sqrt(value)

sq = Square(5)
sq.area = 16    # sets _a = 4.0
sq.a            # 4.0
```

### Name mangling
```python
class Person:
    def __init__(self, name):
        self.__name = name   # stored as _Person__name

p = Person('Bob')
p.__name          # AttributeError
p._Person__name   # 'Bob'  — accessible via mangled name
```

---

## 15 · Magic / Dunder Methods

### `__bool__` and filtering
```python
class Person:
    def __init__(self, name, age): self.name=name; self.age=age
    def __bool__(self): return self.age >= 18

lst = [Person('A',13), Person('B',19), Person('C',27), Person('D',10)]
[p for p in lst if p]   # [Person(B,19), Person(C,27)]
```

### `__call__`
```python
class Average:
    def __init__(self): self.total=0; self.count=0
    def __call__(self, val):
        self.total += val; self.count += 1
        return self.total / self.count

avg = Average()
avg(10)  # 10.0
avg(20)  # 15.0
avg(30)  # 20.0
```

### `__radd__` (reflected operators)
```python
class Modulo7:
    def __init__(self, num): self.num = num % 7
    def __add__(self, val): return Modulo7(self.num + val)
    def __radd__(self, other): return Modulo7(self.num + other)

n4 = Modulo7(4)
n4 + 5   # __add__: Modulo7(2)
4 + n4   # int.__add__ fails → Python tries n4.__radd__(4) → Modulo7(1)
```

### ⚠️ `__hash__` and `__eq__`
```python
# If you define __eq__, Python sets __hash__ = None (unhashable!)
# You MUST also define __hash__ to use objects in sets/dict keys.

class A:
    def __eq__(self, other): return self.x == other.x
    def __hash__(self): return hash(self.x)
```

### Full dunder reference
```python
__str__  __repr__  __format__
__len__  __bool__  __call__
__add__  __sub__  __mul__  __truediv__  __floordiv__  __mod__  __pow__
__matmul__  (@)
__radd__  __rsub__  __rmul__   # reflected: called when left operand fails
__iadd__  __isub__             # augmented assignment: +=, -=
__eq__  __ne__  __lt__  __le__  __gt__  __ge__
__iter__  __next__
__contains__   # in operator
__getitem__  __setitem__  __delitem__
__enter__  __exit__     # context manager
__new__  __init__  __del__
__hash__
__getattr__  __setattr__  __delattr__
```

---

## 16 · Inheritance, Polymorphism, MRO

### super() and __init__
```python
class Base:
    def __init__(self): print('Base init')

class DerivNoInit(Base): pass           # inherits Base.__init__ → prints
class DerivWithInit(Base):
    def __init__(self): print('Derived')  # Base NOT called
class DerivCallsBase(Base):
    def __init__(self):
        super().__init__()              # calls Base → prints 'Base init'
        print('Derived')
```

### ⚠️ MRO (C3 linearisation)
```python
class C(A, B): pass
C.__mro__   # (C, A, B, object)  — left-to-right depth-first, then base

# Cooperative super() — follows MRO chain
class Mixin:
    def method(self):
        print('Mixin'); super().method()

class Base:
    def method(self):
        print('Base')

class MyClass(Mixin, Base): pass
MyClass().method()   # Mixin → Base (super() in Mixin finds Base via MRO)
```

### isinstance checks
```python
isinstance(True, int)    # True  — bool subclasses int
type(True) == int        # False
issubclass(bool, int)    # True
issubclass(int, int)     # True  — a class is a subclass of itself
```

---

## 17 · Exceptions

### try/except/else/finally
```python
def fun(num, denom):
    try:
        z = num // denom
    except ArithmeticError as e:
        print('Exception:', e)
        return None
    else:
        print('Result:', z)    # only if no exception
        return z
    finally:
        print('finally')       # ALWAYS runs, even after return!

fun(27, 4)   # Result: 6 / finally
fun(31, 0)   # Exception: ... / finally
```

### Using exception as multi-loop break
```python
try:
    for a in range(...):
        for b in range(...):
            for c in range(...):
                if condition:
                    raise Exception(a, b, c)
except Exception as ex:
    result = ex.args
```

### Custom exceptions
```python
class OutOfRange(ValueError):
    def __init__(self, val, lower, upper):
        super().__init__(f'{val} not in [{lower}, {upper}]')
        self.val = val
```

---

## 18 · Context Managers

### Class-based
```python
import time

class TimeIt:
    def __enter__(self):
        self.start = time.perf_counter()
        return self          # value bound to 'as' variable

    def __exit__(self, exc_type, exc_val, trace):
        self.time = time.perf_counter() - self.start
        return False         # False/None → don't suppress exceptions
                             # True → suppress exceptions

with TimeIt() as tim:
    sum(range(1_000_000))
print(f'{tim.time:.4f}s')
```

### @contextmanager decorator
```python
import contextlib

@contextlib.contextmanager
def atBeginning(lst):
    lst.reverse()            # setup (before yield)
    yield                    # control passes to with-block
    lst.reverse()            # teardown (after yield)

lst = [3,4,5]
with atBeginning(lst):
    lst.append(2)
    lst.append(1)
    print('Inside:', lst)   # [5,4,3,2,1]  — reversed then appended
print('After:', lst)        # [1,2,3,4,5]  — reversed back
```

---

## 19 · Collections module

```python
from collections import Counter, defaultdict, deque, namedtuple

# Counter
c = Counter('abracadabra')
c.most_common(2)   # [('a',5),('b',2)]
c1 + c2            # sum counts
c1 - c2            # subtract (drops zeros/negatives)

# defaultdict
by_letter = defaultdict(list)
for w in words:
    by_letter[w[0]].append(w)   # no KeyError on missing key

# deque
dq = deque([1,2,3,4,5])
dq.rotate(2)    # [4,5,1,2,3]  — 2 from right → front
dq.rotate(-1)   # [5,1,2,3,4]  — 1 from left → right

dq2 = deque([1,2,3,4,5], maxlen=3)
print(dq2)      # deque([3,4,5],maxlen=3)
dq2.append(6)   # deque([4,5,6]) — drops from left

# namedtuple
Point = namedtuple('Point', ['x','y'])
p = Point(3, 4)
p.x, p.y   # 3, 4
```

---

## 20 · functools: singledispatch, total_ordering

### singledispatch
```python
from functools import singledispatch

@singledispatch
def process(arg): return f'unknown: {type(arg).__name__}'

@process.register(int)
def _(v): return f'int: {v*2}'

@process.register(str)
def _(s): return f'str: {s.upper()}'

@process.register(list | tuple)   # Python 3.10+
def _(seq): return f'seq: {list(seq)}'

process(5)       # 'int: 10'
process('hi')    # 'str: HI'
process([1,2])   # 'seq: [1,2]'
process(True)    # 'int: 2'  ← bool dispatches to int!
process(3.14)    # 'unknown: float'
```

### total_ordering
```python
from functools import total_ordering

@total_ordering
class Student:
    def __eq__(self, other): return self.grade == other.grade
    def __lt__(self, other): return self.grade < other.grade
    # total_ordering generates: __le__, __gt__, __ge__ automatically
```

---

## 21 · Final Exam Patterns (Cheat Sheet)

| Pattern | Key fact |
|---------|----------|
| `y = x` on list | Same object — mutations shared |
| `shallow = lst[:]` | Top-level copy; nested objects shared |
| `-5 // 4` | `-2` (floor, not truncate) |
| `bool([0])` | `True` (non-empty list) |
| `{1:'a',1:'b'}` | `{1:'b'}` last key wins |
| `set(dict)` | Keys, not values |
| `(42)` | `int`, not tuple — need `(42,)` |
| Mutable default `def f(lst=[])` | Shared across calls — use `None` |
| `b=i` at definition | Captures current `i` value, not later changes |
| `@A @B def f()` | Applied bottom-up: `A(B(f))` |
| `@decorator` | `print` inside decorator body runs at IMPORT time |
| `functools.wraps` | Preserves `__name__`, `__doc__`, adds `__wrapped__` |
| `__eq__` without `__hash__` | `__hash__` set to `None` → unhashable |
| `isinstance(True,int)` | `True` — bool is subclass of int |
| `issubclass(int,int)` | `True` — class is subclass of itself |
| `finally` | Runs even after `return` |
| Generator exhaustion | Second `list(gen)` returns `[]` |
| `send()` first call | Must be `next(gen)` or `gen.send(None)` |
| `super()` in MRO | Follows MRO chain, not direct parent |
| `__exit__` returns `True` | Suppresses the exception |
| `deque(maxlen=n)` | Fixed-size; appending drops from opposite end |
| `singledispatch(True)` | Dispatches to `int` handler (bool subclasses int) |
