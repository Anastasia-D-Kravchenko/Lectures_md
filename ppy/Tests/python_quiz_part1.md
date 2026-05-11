# 🐍 Python Lecture — Quiz Part 1
> Covers: Intro, History, Types, Objects & References, Flow Control, Operators, Functions, Lambdas, Decorators, Comprehensions

---

## 📘 Chapter 1 — Intro & History

---

**Q1.** Who created Python and in what year did he start developing it?

- A) Larry Wall, 1987
- B) Guido van Rossum, 1989
- C) James Gosling, 1991
- D) Bjarne Stroustrup, 1985

<details><summary>✅ Answer</summary>

**B) Guido van Rossum, 1989**  
He started in December 1989 while working on the Amoeba project at CWI in Amsterdam.

</details>

---

**Q2.** The name "Python" comes from:

- A) A species of snake used as the logo
- B) Monty Python's Flying Circus (BBC TV series)
- C) The Greek myth of the Python serpent
- D) An acronym for "Practical Yet Typed High-level Object-oriented Notation"

<details><summary>✅ Answer</summary>

**B) Monty Python's Flying Circus (BBC TV series)**

</details>

---

**Q3.** Which of the following are TRUE about Python's type system? (select all that apply)

- A) Python is strongly typed — every object has a definite type
- B) Python is statically typed — type-checking happens before running
- C) Python is dynamically typed — no type-checking before running
- D) Variable names in Python are untyped; objects are typed

<details><summary>✅ Answer</summary>

**A, C, D**  
Python is strongly typed (objects have definite types) and dynamically typed (no compile-time type-checking). Variable *names* are untyped; only the *objects* they point to have types.

</details>

---

**Q4.** TRUE or FALSE: In Python, everything — including integers, floats, classes, and modules — is an object.

<details><summary>✅ Answer</summary>

**TRUE**

</details>

---

**Q5.** What does REPL stand for?

<details><summary>✅ Answer</summary>

**Read, Evaluate, Print, Loop**  
It reads user input, evaluates it, prints the result, and loops back.

</details>

---

**Q6.** What is the special `_` variable in the Python REPL?

- A) A throwaway variable that stores nothing
- B) Refers to the last evaluated and printed value
- C) A soft keyword usable only in `match` statements
- D) A built-in function for ignoring exceptions

<details><summary>✅ Answer</summary>

**B) Refers to the last evaluated and printed value**  
Note: `_` is only available in REPL, not in scripts.

</details>

---

**Q7.** What will this print?

```python
print("k = " + 3)
```

- A) `k = 3`
- B) `k = 3` (after implicit conversion)
- C) `TypeError: can only concatenate str (not "int") to str`
- D) `AttributeError`

<details><summary>✅ Answer</summary>

**C) TypeError**  
Unlike Java, Python does not auto-convert integers to strings for concatenation. You must use `str(3)`.

</details>

---

**Q8.** What are Python's three built-in numeric types?

<details><summary>✅ Answer</summary>

**`int`, `float`, `complex`**  
There is no separate `char` type. `float` in Python corresponds to `double` in C/C++/Java.

</details>

---

**Q9.** Which of the following correctly describes Python `int`?

- A) Fixed 32-bit signed integer
- B) Fixed 64-bit signed integer
- C) Arbitrary precision integer (practically unlimited)
- D) Floating-point with integer ceiling

<details><summary>✅ Answer</summary>

**C) Arbitrary precision integer**  
Python integers can be arbitrarily large.

</details>

---

**Q10.** TRUE or FALSE: In Python there is a built-in `char` type for single characters.

<details><summary>✅ Answer</summary>

**FALSE**  
A single character is simply a one-letter string of type `str`.

</details>

---

**Q11.** Which built-in sequence types are immutable? (select all that apply)

- A) `list`
- B) `tuple`
- C) `range`
- D) `str`

<details><summary>✅ Answer</summary>

**B) `tuple`, C) `range`, D) `str`**  
`list` is mutable. `tuple`, `range`, and `str` are all immutable.

</details>

---

**Q12.** What does this print?

```python
mytuple = (5, 'Hello', [1, 2, 3])
mytuple[2].pop(1)
print(mytuple)
```

- A) `(5, 'Hello', [1, 2, 3])` — tuples are immutable, nothing changes
- B) `(5, 'Hello', [1, 3])` — the list inside the tuple is mutable
- C) `TypeError` — cannot modify elements of a tuple
- D) `IndexError`

<details><summary>✅ Answer</summary>

**B) `(5, 'Hello', [1, 3])`**  
The tuple itself is immutable (you cannot reassign `mytuple[2]`), but the list it *contains* is mutable and can be modified.

</details>

---

**Q13.** What does `range(5)` produce when converted to a list?

<details><summary>✅ Answer</summary>

**`[0, 1, 2, 3, 4]`**  
`range(n)` is equivalent to `range(0, n, 1)`.

</details>

---

**Q14.** What is the difference between `is` and `==` in Python?

<details><summary>✅ Answer</summary>

- `==` compares **values** of objects
- `is` compares **identity** (memory address / ID) — checks if two references point to the *same* object

</details>

---

**Q15.** What will the following print?

```python
a = [1, 2, [3, 5]]
b = a
a[2][1] = 4
print(b)
```

- A) `[1, 2, [3, 5]]`
- B) `[1, 2, [3, 4]]`
- C) `TypeError`
- D) `[1, 4, [3, 5]]`

<details><summary>✅ Answer</summary>

**B) `[1, 2, [3, 4]]`**  
`b = a` makes `b` and `a` names for the *same* list object. Modifying via `a` is visible through `b`.

</details>

---

**Q16.** What is the difference between a shallow copy (`list(a)`) and a deep copy (`deepcopy(a)`)?

<details><summary>✅ Answer</summary>

- **Shallow copy**: creates a new list, but the elements are copies of the *references* — mutable objects inside are shared.
- **Deep copy**: recursively creates new objects for all elements, so no sharing of mutable objects.

</details>

---

**Q17.** TRUE or FALSE: Python's `import` works similarly to `#include` in C/C++ — it physically executes the module file at runtime.

<details><summary>✅ Answer</summary>

**TRUE**  
Unlike Java's `import` (which is just syntactic sugar for qualified names), Python's `import` executes the module file.

</details>

---

**Q18.** How do you import only `sqrt` and `factorial` from the `math` module, without the module prefix?

<details><summary>✅ Answer</summary>

```python
from math import sqrt, factorial
```

</details>

---

**Q19.** What are Python's soft keywords (as of Python 3.13)?

<details><summary>✅ Answer</summary>

`_`, `case`, `match`, `type`  
They have special meaning only in certain contexts and can otherwise be used as identifiers.

</details>

---

**Q20.** According to PEP 8, which naming convention should be used for class names?

- A) `lower_case_with_underscores`
- B) `camelCase` (starting lowercase)
- C) `CamelCase` (starting uppercase)
- D) `ALL_CAPITALS`

<details><summary>✅ Answer</summary>

**C) `CamelCase`** (starting with a capital letter)  
Functions and variables use `lower_case_with_underscores`. Constants use `ALL_CAPITALS`.

</details>

---

## 📗 Chapter 2 — Flow Control

---

**Q21.** What is the purpose of the `pass` statement?

<details><summary>✅ Answer</summary>

It is an empty statement that does nothing. Used when syntax requires a statement but no action is needed (e.g., empty class body, empty loop body).

</details>

---

**Q22.** What will this print?

```python
for n in [2, 6, 8, 10]:
    m = 2*n + 1
    print(m)
```

<details><summary>✅ Answer</summary>

```
5
13
17
21
```

</details>

---

**Q23.** TRUE or FALSE: The `else` clause of a `while` loop is executed even when the loop exits via `break`.

<details><summary>✅ Answer</summary>

**FALSE**  
The `else` clause is executed only when the loop is exhausted *without* `break`.

</details>

---

**Q24.** What is the syntax of Python's conditional (ternary) expression?

<details><summary>✅ Answer</summary>

```python
expr_if_true if condition else expr_if_false
```
This is the equivalent of `condition ? expr_if_true : expr_if_false` in C/Java.

</details>

---

**Q25.** What will this print?

```python
a, b = 5, 0
c = a//b if a > 6 else 3*a
print(c)
```

- A) `ZeroDivisionError`
- B) `15`
- C) `inf`
- D) `0`

<details><summary>✅ Answer</summary>

**B) `15`**  
Since `a > 6` is `False`, the expression short-circuits and `a//b` is never evaluated. `3*5 = 15`.

</details>

---

**Q26.** What happens when you modify a list while iterating over it in a `for` loop?

```python
nums = [2, 1, 3, 4, 5, 6]
for n in nums:
    if n % 2 != 0:
        nums.remove(n)
print(nums)
```

- A) `[2, 4, 6]` — all odd numbers removed correctly
- B) `[2, 3, 4, 6]` — some odd numbers missed due to index shifting
- C) `RuntimeError: list changed size during iteration`
- D) `[2, 4, 6, 3]`

<details><summary>✅ Answer</summary>

**B) `[2, 3, 4, 6]`**  
The number `3` survives because when `1` is removed, indices shift and `3` is skipped. You should iterate over a copy: `for n in nums[:]`.

</details>

---

**Q27.** How does Python's `match` statement differ from `switch` in Java/C++?

- A) `match` supports pattern matching on structured data and ORed patterns with `|`
- B) `match` has fall-through behavior like `switch`
- C) `match` requires a `default` keyword instead of `case _`
- D) `match` only works with integer values

<details><summary>✅ Answer</summary>

**A)**  
Python's `match` supports structured data patterns, ORed patterns (`|`), and uses `_` (or any identifier) as the catch-all. There is **no fall-through**.

</details>

---

**Q28.** What does `enumerate(seasons, 1)` do?

<details><summary>✅ Answer</summary>

Returns an enumerate object that yields `(index, element)` pairs starting the index at `1` instead of `0`.

</details>

---

## 📙 Chapter 3 & 4 — Data Types & Operators

---

**Q29.** What is the result of `9/3` in Python?

- A) `3` (int)
- B) `3.0` (float)
- C) `3` or `3.0` depending on context
- D) `TypeError`

<details><summary>✅ Answer</summary>

**B) `3.0` (float)**  
In Python, `/` always returns a `float`, even with two integer operands. Use `//` for integer division.

</details>

---

**Q30.** What is the result of `-5 // 4` in Python?

- A) `-1` (truncation toward zero, as in C/Java)
- B) `-2` (floor division, truncation toward −∞)
- C) `1`
- D) `0`

<details><summary>✅ Answer</summary>

**B) `-2`**  
`//` is *floor* division — it truncates toward negative infinity, not toward zero.

</details>

---

**Q31.** What is the result of `17 % -10` in Python?

- A) `7`
- B) `-7`
- C) `-3`
- D) `3` (because `(17//-10)*(-10) + result = 17`)

<details><summary>✅ Answer</summary>

**C) `-3`**  
In Python: `17 // -10 = -2`, so `17 % -10 = 17 - (-2)*(-10) = 17 - 20 = -3`.  
Formula: `a % b = a - (a // b) * b`.

</details>

---

**Q32.** TRUE or FALSE: In Python, `^` is the exponentiation operator.

<details><summary>✅ Answer</summary>

**FALSE**  
`^` is bitwise XOR (as in C/Java). Exponentiation is `**`.

</details>

---

**Q33.** What does `2 ** -8` evaluate to?

<details><summary>✅ Answer</summary>

**`0.00390625`** (= 1/256)

</details>

---

**Q34.** What is the result of `5 < 7 > 2` in Python?

- A) `False`
- B) `True`
- C) `TypeError` — chained comparisons not allowed
- D) `2` (result of last comparison)

<details><summary>✅ Answer</summary>

**B) `True`**  
Chained comparisons in Python mean `5 < 7 and 7 > 2`, which is `True and True = True`.

</details>

---

**Q35.** What does `and` return in this expression: `-1 and [1, 2]`?

- A) `True`
- B) `-1`
- C) `[1, 2]`
- D) `False`

<details><summary>✅ Answer</summary>

**C) `[1, 2]`**  
`and` returns the *first falsy* operand, or the *last* operand if all are truthy. Since `-1` is truthy, it returns the second operand `[1, 2]`.

</details>

---

**Q36.** What does `[] or {1, 2}` return?

- A) `False`
- B) `[]`
- C) `{1, 2}`
- D) `True`

<details><summary>✅ Answer</summary>

**C) `{1, 2}`**  
`or` returns the first truthy operand. `[]` is falsy, so it returns `{1, 2}`.

</details>

---

**Q37.** What are "falsy" values in Python? (select all that apply)

- A) `0`
- B) `[]`
- C) `{}`
- D) `None`
- E) `False`
- F) `''`

<details><summary>✅ Answer</summary>

**All of the above: A, B, C, D, E, F**  
Zero numeric values, empty collections/sequences, `None`, and `False` are all falsy.

</details>

---

**Q38.** What does `~321` evaluate to?

- A) `320`
- B) `-321`
- C) `-322`
- D) `322`

<details><summary>✅ Answer</summary>

**C) `-322`**  
Bitwise NOT: `~n = -(n+1)`. So `~321 = -322`. This is equivalent to two's complement bit-flip.

</details>

---

**Q39.** What is the slice `s[::-1]` for string `s = 'abcdef'`?

<details><summary>✅ Answer</summary>

**`'fedcba'`** — a reversed copy of the string.

</details>

---

**Q40.** What are the elements of `ls[1::2]` when `ls = ['a', 'b', 'c', 'd', 'e', 'f']`?

<details><summary>✅ Answer</summary>

**`['b', 'd', 'f']`**  
Start at index 1, step 2: indices 1, 3, 5.

</details>

---

**Q41.** What is the difference between `copy.copy()` and `copy.deepcopy()`?

<details><summary>✅ Answer</summary>

- `copy.copy()` — **shallow copy**: new container, but elements are shared references.
- `copy.deepcopy()` — **deep copy**: recursively creates new objects for all nested mutable elements.

</details>

---

## 📕 Chapter 5 — Functions

---

**Q42.** TRUE or FALSE: In Python, if a function has no `return` statement, it returns `None`.

<details><summary>✅ Answer</summary>

**TRUE**

</details>

---

**Q43.** What is the LEGB rule?

<details><summary>✅ Answer</summary>

The order in which Python searches for variable names:
1. **L**ocal scope
2. **E**nclosing function scope (for nested functions)
3. **G**lobal (module) scope
4. **B**uilt-in scope

</details>

---

**Q44.** What happens in this code and why?

```python
n = 1
def fun():
    n = n + 7
fun()
```

- A) `n` becomes `8` globally
- B) `UnboundLocalError: local variable 'n' referenced before assignment`
- C) `n` stays `1`
- D) `NameError`

<details><summary>✅ Answer</summary>

**B) `UnboundLocalError`**  
Because `n` appears on the left side of an assignment inside the function, Python treats it as a local variable. But it's read before being assigned.  
Fix: add `global n` inside the function.

</details>

---

**Q45.** What keyword is needed inside a nested function to modify a variable from the *enclosing* (not global) scope?

<details><summary>✅ Answer</summary>

**`nonlocal`**

</details>

---

**Q46.** What does `*args` capture in a function definition?

<details><summary>✅ Answer</summary>

All positional arguments beyond the named parameters, packed into a **tuple**.

</details>

---

**Q47.** What does `**kwargs` capture?

<details><summary>✅ Answer</summary>

All keyword arguments not matched by named parameters, packed into a **dict**.

</details>

---

**Q48.** What is a closure in Python?

<details><summary>✅ Answer</summary>

A function defined inside another function that "remembers" references to variables from its enclosing scope — even after the outer function has returned. The remembered variables are stored in the function's `__closure__` attribute.

</details>

---

**Q49.** What is the output?

```python
def getCounter():
    n = 0
    def count():
        nonlocal n
        n += 1
        return n
    return count

f = getCounter()
print(f(), f(), f())
```

<details><summary>✅ Answer</summary>

**`1 2 3`**  
Each call to `f()` increments `n` in the closure.

</details>

---

**Q50.** What is duck typing?

<details><summary>✅ Answer</summary>

The concept that a function works for any type as long as the operations used inside it are valid for that type — no explicit type declarations are needed. Named after "if it walks like a duck and quacks like a duck, it's a duck."

</details>

---

**Q51.** What is the type of a lambda expression?

- A) `lambda`
- B) `function`
- C) `builtin_function_or_method`
- D) `callable`

<details><summary>✅ Answer</summary>

**B) `function`** (`<class 'function'>`)  
Lambdas are functionally identical to named functions except their `__name__` is `'<lambda>'`.

</details>

---

**Q52.** What does this lambda return?

```python
pal = lambda ls: 'palindrome' if ls == ls[::-1] else 'not palindrome'
print(pal('rotor'))
```

<details><summary>✅ Answer</summary>

**`'palindrome'`**  
`'rotor'[::-1]` is `'rotor'`, so the condition is `True`.

</details>

---

**Q53.** Can a lambda have multiple parameters?

<details><summary>✅ Answer</summary>

**Yes.** Example: `lambda x, y: x**2 + y**2`  
The body must be a single expression (no statements like `if/else` blocks, but conditional *expressions* are allowed).

</details>

---

**Q54.** What does `filter(None, [1, 5, 0, '', {}, 2])` return (as a list)?

<details><summary>✅ Answer</summary>

**`[1, 5, 2]`**  
`None` as predicate keeps only truthy values. `0`, `''`, and `{}` are falsy.

</details>

---

**Q55.** What does `map(lambda x: x**x, [1, 2, 3, 4])` return as a list?

<details><summary>✅ Answer</summary>

**`[1, 4, 27, 256]`**

</details>

---

**Q56.** What module does `reduce` come from, and what does it do?

<details><summary>✅ Answer</summary>

From **`functools`**. It applies a binary function cumulatively to items of an iterable, reducing it to a single value.

```python
from functools import reduce
reduce(lambda x, y: x + y, [1, 2, 3, 4])  # → 10
```

</details>

---

**Q57.** What does `sorted` return, and how does it differ from the `list.sort()` method?

<details><summary>✅ Answer</summary>

- `sorted(iterable)` returns a **new sorted list** and works on any iterable.
- `list.sort()` sorts the list **in-place** and returns `None`.

Both accept `key=` and `reverse=` keyword arguments.

</details>

---

**Q58.** What is a decorator in Python?

<details><summary>✅ Answer</summary>

A decorator is a callable that takes another callable as its argument and returns a callable. Used with the `@decorator` syntax, it is equivalent to:

```python
func = decorator(func)
```

Common uses: logging, counting invocations, caching (memoization), input validation.

</details>

---

**Q59.** What does `functools.wraps(func)` do inside a decorator?

<details><summary>✅ Answer</summary>

It copies the original function's metadata (`__name__`, `__doc__`, `__module__`, etc.) to the wrapper function, and adds a `__wrapped__` attribute pointing to the original function.

</details>

---

**Q60.** What is the output?

```python
def counting(func):
    import functools
    @functools.wraps(func)
    def modified(*args, **kwargs):
        modified.counter += 1
        return func(*args, **kwargs)
    modified.counter = 0
    return modified

@counting
def adder(a, b):
    return a + b

adder(1, 2)
adder(3, 4)
print(adder.counter)
```

<details><summary>✅ Answer</summary>

**`2`**

</details>

---

## 💻 Code Tasks — Write It Yourself

---

**Task 1.** Write a function `is_palindrome(s: str) -> bool` that returns `True` if `s` is a palindrome using slicing.

<details><summary>✅ Solution</summary>

```python
def is_palindrome(s):
    return s == s[::-1]

print(is_palindrome("rotor"))  # True
print(is_palindrome("python")) # False
```

</details>

---

**Task 2.** Write a function `flatten(lst)` that takes a list of lists and returns a flat list using a list comprehension.

<details><summary>✅ Solution</summary>

```python
def flatten(lst):
    return [item for sublist in lst for item in sublist]

print(flatten([[1, 2], [3, 4], [5]]))  # [1, 2, 3, 4, 5]
```

</details>

---

**Task 3.** Using `filter` and `map`, write code that:
1. Filters only even numbers from `[1..10]`
2. Squares them

<details><summary>✅ Solution</summary>

```python
result = list(map(lambda x: x**2, filter(lambda x: x % 2 == 0, range(1, 11))))
print(result)  # [4, 16, 36, 64, 100]
```

</details>

---

**Task 4.** Write a decorator `timer` that prints how long (in seconds) a function takes to execute.

<details><summary>✅ Solution</summary>

```python
import functools
import time

def timer(func):
    @functools.wraps(func)
    def wrapper(*args, **kwargs):
        start = time.time()
        result = func(*args, **kwargs)
        end = time.time()
        print(f"{func.__name__} took {end - start:.4f}s")
        return result
    return wrapper

@timer
def slow():
    time.sleep(0.1)

slow()
```

</details>

---

**Task 5.** Write a closure `make_multiplier(n)` that returns a function which multiplies its argument by `n`.

<details><summary>✅ Solution</summary>

```python
def make_multiplier(n):
    return lambda x: x * n

triple = make_multiplier(3)
print(triple(7))  # 21
```

</details>

---

**Task 6.** What does this print? (PREDICT before running)

```python
a = [1, 2, 3]
b = a[:]
b[0] = 99
print(a)
print(b)
```

<details><summary>✅ Answer</summary>

```
[1, 2, 3]
[99, 2, 3]
```
`a[:]` creates a shallow copy, so `a` and `b` are different list objects. Modifying `b[0]` (an immutable int) doesn't affect `a`.

</details>

---

**Task 7.** Write a generator function `fibonacci()` that yields Fibonacci numbers indefinitely.

<details><summary>✅ Solution</summary>

```python
def fibonacci():
    a, b = 0, 1
    while True:
        yield a
        a, b = b, a + b

fib = fibonacci()
print([next(fib) for _ in range(10)])  # [0, 1, 1, 2, 3, 5, 8, 13, 21, 34]
```

</details>

---

**Task 8.** Using `reduce`, compute the product of all elements in `[1, 2, 3, 4, 5]`.

<details><summary>✅ Solution</summary>

```python
from functools import reduce
result = reduce(lambda x, y: x * y, [1, 2, 3, 4, 5])
print(result)  # 120
```

</details>

---

*End of Part 1. Ask for Part 2 to cover: Comprehensions & Generators, Collections, Strings, Exceptions, Classes, Context Managers.*
