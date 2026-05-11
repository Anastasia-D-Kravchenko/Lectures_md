"""
============================================================
 PPY Python Exam Practice — Complete Coding Workbook
 Werner Lectures, Warsaw 2026
 Based on: Python Mastery Guide · PythonLecture_StudyNotes ·
           PPY1-PPY05 exercises · Python Crash Course 3rd Ed.

 HOW TO USE:
   1. Read every TODO comment carefully.
   2. Write your code where marked — remove the pass / ...
   3. Uncomment the demo calls at the bottom one section at a time.
   4. Every section maps to a real exam/test topic.

 TOPICS COVERED (in order):
   Part 01 — REPL, types, id(), is vs ==, objects & references
   Part 02 — Immutability vs mutability, shallow vs deep copy
   Part 03 — Built-in types overview (int, float, bool, str, None)
   Part 04 — Integers: bases, unlimited precision, caching
   Part 05 — Floats: IEEE 754 trap, decimal fix
   Part 06 — Strings: methods, slicing, f-strings, immutability
   Part 07 — if / elif / else, ternary, chained comparisons
   Part 08 — while loop: break, continue, else clause
   Part 09 — for loop: enumerate, zip, range, iterator protocol
   Part 10 — match statement (Python 3.10+)
   Part 11 — Operators: precedence, //, /, %, **, walrus :=
   Part 12 — Lists: mutation, slicing, sorting, copy
   Part 13 — Tuples: immutability, packing, unpacking, namedtuple
   Part 14 — Dictionaries: access, iteration, comprehension, defaultdict
   Part 15 — Sets: operations, comprehension, frozenset
   Part 16 — Slicing: universal syntax [start:stop:step]
   Part 17 — Functions: scopes (LEGB), default args, *args, **kwargs
   Part 18 — Lambda, filter, map, reduce, sorted with key
   Part 19 — Closures and nonlocal
   Part 20 — Decorators (@functools.wraps, parameterised decorators)
   Part 21 — Comprehensions: list, set, dict, nested
   Part 22 — Generator functions: yield, yield break, send()
   Part 23 — Generator expressions (lazy, memory-efficient)
   Part 24 — itertools: chain, islice, cycle, accumulate, groupby
   Part 25 — OOP: classes, instance vs class attributes, __init__
   Part 26 — Dunder methods: __str__, __repr__, __len__, __eq__, etc.
   Part 27 — Inheritance, super(), polymorphism, isinstance
   Part 28 — Properties (@property, setter, computed)
   Part 29 — Abstract Base Classes (ABC, @abstractmethod)
   Part 30 — Operator overloading (Vector example)
   Part 31 — @dataclass (auto __init__, __repr__, __eq__, frozen)
   Part 32 — Type hints (annotations, Optional, Union, Callable)
   Part 33 — Single dispatch (@functools.singledispatch)
   Part 34 — Exceptions: try/except/else/finally, raise, re-raise
   Part 35 — Custom exceptions
   Part 36 — Context managers (__enter__/__exit__, @contextmanager)
   Part 37 — File I/O: read, write, append, JSON, pathlib
   Part 38 — Modules: import styles, __name__ guard
   Part 39 — collections: Counter, deque, defaultdict, ChainMap
   Part 40 — unittest: TestCase, assertions, setUp/tearDown
   Part 41 — Iterator protocol: __iter__ + __next__ custom class
   Part 42 — Pythonic idioms: swap, any/all, join, walrus, unpacking
   Part 43 — Exam simulations (PPY lab tasks)
============================================================
"""

import functools
import itertools
import copy
import json
import math
import random
import unittest
from abc import ABC, abstractmethod
from collections import namedtuple, defaultdict, Counter, deque, ChainMap
from contextlib import contextmanager
from dataclasses import dataclass, field
from pathlib import Path
from typing import Optional, Union, Callable, TypeVar


# ─────────────────────────────────────────────────────────────
# ENTRY POINT — uncomment sections one at a time as you finish
# ─────────────────────────────────────────────────────────────
def main():
    # part01_repl_and_references()
    # part02_mutability_and_copy()
    # part03_builtin_types_overview()
    # part04_integers()
    # part05_floats()
    # part06_strings()
    # part07_control_flow_if()
    # part08_while_loop()
    # part09_for_loop()
    # part10_match_statement()
    # part11_operators()
    # part12_lists()
    # part13_tuples()
    # part14_dicts()
    # part15_sets()
    # part16_slicing()
    # part17_functions_and_scopes()
    # part18_lambda_map_filter_reduce()
    # part19_closures()
    # part20_decorators()
    # part21_comprehensions()
    # part22_generators_yield()
    # part23_generator_expressions()
    # part24_itertools_demo()
    # part25_oop_basics()
    # part26_dunder_methods()
    # part27_inheritance()
    # part28_properties()
    # part29_abc()
    # part30_operator_overloading()
    # part31_dataclass()
    # part32_type_hints()
    # part33_single_dispatch()
    # part34_exceptions()
    # part35_custom_exceptions()
    # part36_context_managers()
    # part37_file_io()
    # part38_modules()
    # part39_collections_module()
    # part40_unittest_demo()
    # part41_iterator_protocol()
    # part42_pythonic_idioms()
    # part43_exam_simulations()
    print("Done! Uncomment parts in main() to run each section.")


# ══════════════════════════════════════════════════════════════
# PART 01 — REPL, types, id(), is vs ==, objects & references
# ══════════════════════════════════════════════════════════════
def part01_repl_and_references():
    """
    EXAM FACTS:
    - Everything in Python is an object — even integers and functions.
    - Variables are LABELS (references) pointing to objects, not boxes.
    - id(x) returns the memory identity of the object x points to.
    - 'is' tests IDENTITY (same object); '==' tests VALUE.
    - Small integers [-5..256] are cached singletons — 'is' will be True.
    - Assignment NEVER copies — it rebinds the label.
    """

    # TASK 1-A: Assign x=5, print its type and id.
    #           Reassign x=x+1 and print id again.
    #           Q: Does the id change? Why?
    x = 5
    print(type(x), id(x))
    x = x + 1
    print(id(x))  # different id — integers are immutable, new object created

    # TASK 1-B: Small integer caching — when is 'is' True for integers?
    a = 256; b = 256
    print(a is b)   # True  — cached singleton
    c = 257; d = 257
    print(c is d)   # False (usually) — outside cache range     TRUE WAS IN MY CASE

    # TASK 1-C: 'is' vs '==' for lists
    lst1 = [1, 2, 3]
    lst2 = [1, 2, 3]
    lst3 = lst1
    print(lst1 == lst2)  # True  — same values
    print(lst1 is lst2)  # False — different objects
    print(lst1 is lst3)  # True  — same object (rebind, not copy)

    # TASK 1-D: Demonstrate that assignment does NOT copy
    a = [1, 2, 3]
    b = a           # b points to the SAME list
    b.append(4)
    print(a)        # [1, 2, 3, 4] — both changed!

    # TASK 1-E: input() always returns str — demonstrate type conversion
    raw = "42"
    n = int(raw)
    print(type(raw), type(n))  # str, int

    # EXAM: The REPL's '_' variable holds the last evaluated result.
    #       type(x) returns the type object; isinstance(x, T) is safer for inheritance.
    # >> > 10 + 20
    # 30
    # >> > _ * 2
    # 60
    pass


# ══════════════════════════════════════════════════════════════
# PART 02 — IMMUTABILITY vs MUTABILITY, shallow vs deep copy
# ══════════════════════════════════════════════════════════════
def part02_mutability_and_copy():
    """
    IMMUTABLE types: int, float, str, tuple, frozenset, bytes
      → modifying creates a NEW object (different id)
    MUTABLE types: list, dict, set
      → modified IN PLACE (same id)

    SHALLOW COPY: new container, same element references.
    DEEP COPY: completely independent clone of everything.
    """

    # TASK 2-A: Prove a string is immutable (changing it creates a new object)
    s = "hello"
    print(id(s))
    s = s.upper()
    print(id(s))  # different — new string object

    # TASK 2-B: Prove a list is mutable (same id after append)
    lst = [1, 2, 3]
    print(id(lst))
    lst.append(4)
    print(id(lst))  # SAME — modified in place

    # TASK 2-C: Shallow copy — outer list independent, nested lists shared
    original = [[1, 2], [3, 4]]
    shallow = original.copy()          # or original[:]
    shallow.append([5, 6])             # only in shallow
    shallow[0].append(99)              # affects BOTH — shared nested list!
    print(original)  # [[1, 2, 99], [3, 4]] — inner list was shared

    # TASK 2-D: Deep copy — fully independent clone
    deep = copy.deepcopy(original)
    deep[0].append(888)
    print(original)  # unchanged — completely independent

    pass


# ══════════════════════════════════════════════════════════════
# PART 03 — BUILT-IN TYPES OVERVIEW
# ══════════════════════════════════════════════════════════════
def part03_builtin_types_overview():
    """
    EXAM: Know what's mutable and what's immutable.
    Know what's falsy: 0, 0.0, '', [], {}, set(), None, False.
    """

    # TASK 3-A: Print bool() of each falsy value — all should be False
    falsy = [0, 0.0, 0j, '', [], {}, set(), None, False]
    for v in falsy:
        print(f"bool({repr(v):12}) = {bool(v)}")

    # TASK 3-B: Truthy values — non-empty, non-zero
    truthy = [1, -1, 3.14, 'x', [0], {'a': 1}, {0}, True]
    for v in truthy:
        print(f"bool({repr(v):12}) = {bool(v)}")

    # EXAM: bool is a subclass of int. True == 1, False == 0.
    print(True + True)   # 2
    print(False - 0)    # 0
    pass


# ══════════════════════════════════════════════════════════════
# PART 04 — INTEGERS: bases, unlimited precision, caching
# ══════════════════════════════════════════════════════════════
def part04_integers():
    # TASK 4-A: Unlimited precision — Python ints never overflow
    big = 2 ** 200
    print(big)
    print(f"Digits: {len(str(big))}")

    # TASK 4-B: Integer literals in different bases
    print(0b1010)   # binary   → 10
    print(0o17)     # octal    → 15
    print(0xFF)     # hex      → 255
    print(1_000_000) # underscores for readability

    # TASK 4-C: Convert between bases
    n = 255
    print(bin(n))   # '0b11111111'
    print(oct(n))   # '0o377'
    print(hex(n))   # '0xff'
    # Convert back:
    print(int('11111111', 2))  # 255
    print(int('ff', 16))       # 255
    print(int('377', 8))       # 255

    # TASK 4-D: Small integer caching range [-5..256]
    # Explain why: a = 257; b = 257; a is b → False
    # its more than 128 so it further that cash is
    pass


# ══════════════════════════════════════════════════════════════
# PART 05 — FLOATS: IEEE 754 trap, decimal fix
# ══════════════════════════════════════════════════════════════
def part05_floats():
    """
    EXAM CLASSIC: 0.1 + 0.2 != 0.3 (floating point imprecision)
    Fix: use the 'decimal' module for exact arithmetic.
    float corresponds to C double — 64-bit IEEE 754.
    """

    # TASK 5-A: The floating-point trap
    print(0.1 + 0.2)          # 0.30000000000000004
    print(0.1 + 0.2 == 0.3)   # False!
    print(0.1 + 0.2 - 0.3)    # a tiny non-zero number

    # TASK 5-B: Fix with math.isclose
    import math
    print(math.isclose(0.1 + 0.2, 0.3))  # True

    # TASK 5-C: Fix with decimal module for exact precision
    import decimal
    a = decimal.Decimal('0.1')
    b = decimal.Decimal('0.2')
    print(a + b)          # 0.3
    print(a + b == decimal.Decimal('0.3'))  # True

    # TASK 5-D: floor division vs true division
    print(9 / 3)     # 3.0  (always float in Python 3!)
    print(9 // 3)    # 3    (int result)
    print(-7 // 2)   # -4   (rounds TOWARD NEGATIVE INFINITY — differs from Java!)
    print(-7 % 2)    # 1    (sign follows divisor in Python)
    pass


# ══════════════════════════════════════════════════════════════
# PART 06 — STRINGS: methods, slicing, f-strings, immutability
# ══════════════════════════════════════════════════════════════
def part06_strings():
    # TASK 6-A: Practice key string methods
    s = "  Hello, World!  "
    print(s.strip())           # "Hello, World!"
    print(s.upper())
    print(s.lower())
    print(s.replace("World", "Python"))
    print(s.split(","))        # ['  Hello', ' World!  ']
    print(",".join(["a", "b", "c"]))  # 'a,b,c'
    print(s.startswith("  H")) # True
    print(s.find("World"))
    print(s.count("l"))

    # TASK 6-B: Slicing strings
    # TODO:
    # t = "Python"
    # print(t[0])       # P
    # print(t[-1])      # n
    # print(t[1:4])     # yth
    # print(t[::-1])    # nohtyP  (reversed)
    # print(t[::2])     # Pto

    # TASK 6-C: f-strings (Python 3.6+) — preferred format method
    # TODO:
    # name, score = "Anna", 95.5
    # print(f"Student: {name}, Score: {score:.1f}")
    # print(f"{1_000_000:,}")    # 1,000,000
    # print(f"{'left':<10}|")   # left-aligned in 10 chars
    # print(f"{'right':>10}|")  # right-aligned
    # print(f"Pi = {math.pi:.4f}")

    # TASK 6-D: Strings are immutable — methods return NEW strings
    # TODO:
    # original = "hello"
    # upper = original.upper()
    # print(original)  # still "hello" — not modified
    # print(upper)     # "HELLO" — new object

    # TASK 6-E: Check for palindrome using slicing
    # TODO:
    # def is_palindrome(s):
    #     s = s.lower().replace(" ", "")
    #     return s == s[::-1]
    # print(is_palindrome("racecar"))  # True
    # print(is_palindrome("hello"))    # False
    pass


# ══════════════════════════════════════════════════════════════
# PART 07 — if / elif / else, ternary, chained comparisons
# ══════════════════════════════════════════════════════════════
def part07_control_flow_if():
    # TASK 7-A: Grade classifier with if/elif/else
    # TODO:
    # def classify_grade(score):
    #     if score >= 90:
    #         return "A"
    #     elif score >= 75:
    #         return "B"
    #     elif score >= 60:
    #         return "C"
    #     else:
    #         return "F"
    # for s in [95, 80, 65, 40]:
    #     print(f"{s}: {classify_grade(s)}")

    # TASK 7-B: Ternary (conditional) expression
    # TODO:
    # x = 7
    # label = "even" if x % 2 == 0 else "odd"
    # print(f"{x} is {label}")

    # TASK 7-C: Chained comparisons — unique to Python
    # TODO:
    # x = 5
    # print(0 < x < 10)    # True — cleaner than x > 0 and x < 10
    # print(1 < 2 < 3 < 4) # True
    # print(5 < 7 > 3)     # True

    # TASK 7-D: Truthiness-based conditions (Pythonic)
    # TODO:
    # items = []
    # if items:               # Pythonic — checks if non-empty
    #     print("has items")
    # else:
    #     print("empty")
    # name = None
    # if name is None:        # use 'is' for None, not ==
    #     print("no name")
    pass


# ══════════════════════════════════════════════════════════════
# PART 08 — while LOOP: break, continue, else clause
# ══════════════════════════════════════════════════════════════
def part08_while_loop():
    """
    EXAM: The 'else' clause on a while/for loop runs ONLY IF
    the loop ended WITHOUT a break statement.
    This is unique to Python — Java/C++ don't have this.
    """

    # TASK 8-A: Prime checker using while + else (classic exam pattern)
    # TODO:
    # def is_prime(n):
    #     if n < 2: return False
    #     i = 2
    #     while i * i <= n:
    #         if n % i == 0:
    #             break        # found divisor — NOT prime
    #         i += 1
    #     else:
    #         return True      # loop finished without break → prime
    #     return False
    # for n in [2, 7, 9, 13, 25]:
    #     print(f"{n}: {'prime' if is_prime(n) else 'not prime'}")

    # TASK 8-B: Guessing game with while loop
    # TODO:
    # def guessing_game():
    #     secret = random.randint(1, 100)
    #     attempts = []
    #     while True:
    #         guess = int(input("Guess (1-100): "))
    #         attempts.append(guess)
    #         if guess < secret:
    #             print("Too small!")
    #         elif guess > secret:
    #             print("Too large!")
    #         else:
    #             print(f"Correct! Attempts: {len(attempts)}")
    #             print(f"Last 3 guesses: {attempts[-3:]}")
    #             break

    # TASK 8-C: while with continue — skip processing on empty input
    # TODO:
    # inputs = ["hello", "", "world", "", "exit"]
    # i = 0
    # while i < len(inputs):
    #     line = inputs[i]
    #     i += 1
    #     if not line:
    #         continue      # skip blank lines
    #     if line == "exit":
    #         break
    #     print(line.upper())
    pass


# ══════════════════════════════════════════════════════════════
# PART 09 — for LOOP: enumerate, zip, range, iterator protocol
# ══════════════════════════════════════════════════════════════
def part09_for_loop():
    """
    Python 'for' is really a while loop calling iter() then next().
    It stops when StopIteration is raised.
    NEVER modify a list while iterating over it — iterate over a copy.
    """

    # TASK 9-A: enumerate — get index AND value
    # TODO:
    # fruits = ["apple", "banana", "cherry"]
    # for i, fruit in enumerate(fruits, start=1):
    #     print(f"{i}. {fruit}")

    # TASK 9-B: zip — iterate two sequences in parallel
    # TODO:
    # names  = ["Alice", "Bob", "Carol"]
    # scores = [90, 85, 92]
    # for name, score in zip(names, scores):
    #     print(f"{name}: {score}")

    # TASK 9-C: range with step
    # TODO:
    # for i in range(0, 10, 2):   # 0, 2, 4, 6, 8
    #     print(i, end=" ")

    # TASK 9-D: What for actually does — the iterator protocol manually
    # TODO:
    # lst = [10, 20, 30]
    # it = iter(lst)              # calls lst.__iter__()
    # while True:
    #     try:
    #         val = next(it)      # calls it.__next__()
    #         print(val)
    #     except StopIteration:
    #         break

    # TASK 9-E: Modifying list — iterate over a copy!
    # TODO:
    # nums = [1, 2, 3, 4, 5]
    # for n in nums[:]:           # iterate over COPY
    #     if n % 2 == 0:
    #         nums.remove(n)      # safe because we're iterating the copy
    # print(nums)  # [1, 3, 5]

    # TASK 9-F: for/else — runs only if loop ended WITHOUT break
    # TODO:
    # targets = [3, 7, 2, 9]
    # for t in targets:
    #     if t == 5:
    #         print("found 5")
    #         break
    # else:
    #     print("5 not found")   # prints because no break occurred
    pass


# ══════════════════════════════════════════════════════════════
# PART 10 — match STATEMENT (Python 3.10+)
# ══════════════════════════════════════════════════════════════
def part10_match_statement():
    """
    EXAM: match is far more powerful than switch/case in Java.
    - Matches values, sequences, types, guards.
    - No fall-through — only first matching case executes.
    - '|' for OR patterns; '_' is the catch-all.
    """

    # TASK 10-A: Value matching with | (OR)
    # TODO:
    # def http_status(code):
    #     match code:
    #         case 200:
    #             return "OK"
    #         case 404:
    #             return "Not Found"
    #         case 500 | 503:
    #             return "Server Error"
    #         case _:
    #             return "Unknown"
    # for c in [200, 404, 500, 418]:
    #     print(f"{c}: {http_status(c)}")

    # TASK 10-B: Sequence matching — command parser
    # TODO:
    # def parse_command(cmd):
    #     match cmd.split():
    #         case ["quit"]:
    #             return "Quitting"
    #         case ["go", direction]:
    #             return f"Going {direction}"
    #         case ["get", obj, *rest]:
    #             return f"Getting {obj} (extras: {rest})"
    #         case _:
    #             return "Unknown command"
    # for cmd in ["quit", "go north", "get key extra1 extra2", "fly"]:
    #     print(parse_command(cmd))

    # TASK 10-C: Type matching with guards
    # TODO:
    # def describe(value):
    #     match value:
    #         case int() | float() as n if n < 0:
    #             return f"Negative number: {n}"
    #         case str() as s if len(s) > 5:
    #             return f"Long string: {s}"
    #         case _:
    #             return f"Other: {value}"
    # for v in [-3, 3.14, "hi", "Python"]:
    #     print(describe(v))
    pass


# ══════════════════════════════════════════════════════════════
# PART 11 — OPERATORS: precedence, //, /, %, **, walrus :=
# ══════════════════════════════════════════════════════════════
def part11_operators():
    """
    KEY EXAM TRAPS:
    - '/' ALWAYS returns float in Python 3 (even 9/3 = 3.0)
    - '//' rounds toward NEGATIVE INFINITY (not toward zero like Java)
    - '%' sign follows the DIVISOR in Python (not the dividend like Java)
    - '**' is right-associative: 2**3**2 = 2**(3**2) = 512
    - '=' is a STATEMENT (not expression) — use ':=' (walrus) for expressions
    """

    # TASK 11-A: Division traps
    # TODO:
    # print(9 / 3)      # 3.0  — not 3!
    # print(9 // 3)     # 3
    # print(-7 // 2)    # -4   — rounds toward -∞ (Java would give -3)
    # print(-7 % 2)     # 1    — sign follows divisor (Java gives -1)
    # print(7 % -2)     # -1   — sign follows divisor (negative)

    # TASK 11-B: Exponentiation — right-associative
    # TODO:
    # print(2 ** 3 ** 2)  # 512 = 2**(3**2) = 2**9, NOT (2**3)**2=64

    # TASK 11-C: Walrus operator := (Python 3.8+) — assign inside expression
    # TODO:
    # data = [1, 2, 3, 4, 5]
    # # Without walrus:
    # chunk = data[:3]
    # if len(chunk) > 2:
    #     print(chunk)
    # # With walrus:
    # if len(chunk := data[:3]) > 2:
    #     print(chunk)  # chunk is available here!

    # TASK 11-D: Augmented assignment operators
    # TODO:
    # x = 10
    # x += 5;  print(x)   # 15
    # x -= 3;  print(x)   # 12
    # x *= 2;  print(x)   # 24
    # x //= 5; print(x)   # 4
    # x **= 3; print(x)   # 64
    pass


# ══════════════════════════════════════════════════════════════
# PART 12 — LISTS: mutation, slicing, sorting, copy
# ══════════════════════════════════════════════════════════════
def part12_lists():
    # TASK 12-A: All key list methods
    # TODO:
    # fruits = ["apple", "banana", "cherry"]
    # fruits.append("date")          # add to end
    # fruits.insert(1, "avocado")    # insert at index
    # fruits.remove("banana")        # remove first occurrence
    # popped = fruits.pop()          # remove+return last
    # popped2 = fruits.pop(0)        # remove+return by index
    # fruits.sort()                  # in-place sort
    # fruits.sort(reverse=True)      # in-place descending
    # fruits.reverse()               # in-place reverse
    # print(fruits.index("cherry"))  # index of first match
    # print(fruits.count("apple"))   # count occurrences
    # fruits.extend(["fig", "grape"])# add multiple items

    # TASK 12-B: List slicing (see also Part 16)
    # TODO:
    # s = list(range(10))  # [0,1,2,...,9]
    # print(s[2:7])    # [2,3,4,5,6]
    # print(s[::2])    # [0,2,4,6,8]
    # print(s[::-1])   # [9,8,...,0]
    # print(s[-3:])    # [7,8,9]

    # TASK 12-C: sorted() returns new list; .sort() modifies in place
    # TODO:
    # words = ["banana", "apple", "cherry"]
    # new_sorted = sorted(words)          # new list, original unchanged
    # words.sort()                        # modifies words in place
    # sorted_by_len = sorted(words, key=len)  # sort by length

    # TASK 12-D: Lab task — generate 50 random scores, assign grades,
    #             compute average, count passes, find top 5
    # TODO:
    # scores = [random.randint(0, 100) for _ in range(50)]
    # def assign_grade(s):
    #     if s >= 90: return 5
    #     elif s >= 75: return 4
    #     elif s >= 60: return 3
    #     else: return 2
    # grades = [assign_grade(s) for s in scores]
    # avg = sum(scores) / len(scores)
    # passes = sum(1 for s in scores if s >= 60)
    # top5 = sorted(scores, reverse=True)[:5]
    # print(f"Avg: {avg:.1f}, Passes: {passes}, Top5: {top5}")
    pass


# ══════════════════════════════════════════════════════════════
# PART 13 — TUPLES: immutability, packing, unpacking, namedtuple
# ══════════════════════════════════════════════════════════════
def part13_tuples():
    # TASK 13-A: Basic tuple operations
    # TODO:
    # point = (3, 7)
    # x, y = point           # tuple unpacking
    # print(x, y)
    # a, *rest = (1, 2, 3, 4)  # starred unpacking
    # print(a, rest)           # 1  [2, 3, 4]
    # single = (42,)           # trailing comma required for 1-element tuple
    # print(type(single))      # <class 'tuple'>
    # NOT_A_TUPLE = (42)       # just parentheses around an int
    # print(type(NOT_A_TUPLE)) # <class 'int'>

    # TASK 13-B: Tuples are hashable (if elements are) → can be dict keys or set members
    # TODO:
    # my_set = {(1, 2), (3, 4)}
    # my_dict = {(0, 0): "origin", (1, 0): "right"}
    # print(my_dict[(0, 0)])

    # TASK 13-C: namedtuple — self-documenting, tuple-like but with field names
    # TODO:
    # Person = namedtuple('Person', ['name', 'age', 'city'])
    # alice = Person('Alice', 30, 'Warsaw')
    # print(alice.name, alice.age)   # access by name
    # print(alice[0])                # access by index still works
    # name, age, city = alice        # still unpackable

    # TASK 13-D: Swap using tuple packing/unpacking (Pythonic!)
    # TODO:
    # a, b = 5, 9
    # a, b = b, a     # no temp variable needed!
    # print(a, b)     # 9  5
    pass


# ══════════════════════════════════════════════════════════════
# PART 14 — DICTIONARIES: access, iteration, comprehension, defaultdict
# ══════════════════════════════════════════════════════════════
def part14_dicts():
    # TASK 14-A: Key dict operations
    # TODO:
    # person = {"name": "Alice", "age": 30, "city": "Warsaw"}
    # print(person["name"])                # 'Alice'
    # print(person.get("phone", "N/A"))    # safe: 'N/A' (no KeyError)
    # person["email"] = "alice@example.com"
    # person.update({"age": 31, "city": "Berlin"})
    # del person["city"]
    # removed = person.pop("email", None)  # safe pop

    # TASK 14-B: Iterating a dict
    # TODO:
    # for key in person:
    #     print(key)
    # for key, val in person.items():
    #     print(f"{key}: {val}")
    # print(list(person.keys()))
    # print(list(person.values()))

    # TASK 14-C: Dict comprehension
    # TODO:
    # squares = {n: n**2 for n in range(1, 6)}
    # word_len = {w: len(w) for w in ["apple", "banana", "cherry"]}
    # filtered = {k: v for k, v in squares.items() if v > 9}

    # TASK 14-D: defaultdict — auto-creates missing keys
    # TODO:
    # groups = defaultdict(list)
    # for name, dept in [("Alice", "IT"), ("Bob", "HR"), ("Carol", "IT")]:
    #     groups[dept].append(name)
    # print(dict(groups))  # {'IT': ['Alice', 'Carol'], 'HR': ['Bob']}

    # TASK 14-E: Lab task — shopping cart with dict
    # TODO:
    # cart = {}
    # items = [("apple", 1.50), ("banana", 0.80), ("cherry", 3.00)]
    # for name, price in items:
    #     cart[name] = price
    # total = sum(cart.values())
    # most_expensive = max(cart, key=cart.get)
    # avg = total / len(cart)
    # print(f"Total: {total:.2f}, Most expensive: {most_expensive}, Avg: {avg:.2f}")
    pass


# ══════════════════════════════════════════════════════════════
# PART 15 — SETS: operations, comprehension, frozenset
# ══════════════════════════════════════════════════════════════
def part15_sets():
    # TASK 15-A: Set creation and basic operations
    # TODO:
    # a = {1, 2, 3, 4}
    # b = {3, 4, 5, 6}
    # print(a | b)    # union:         {1,2,3,4,5,6}
    # print(a & b)    # intersection:  {3,4}
    # print(a - b)    # difference:    {1,2}
    # print(a ^ b)    # symmetric diff:{1,2,5,6}
    # a.add(7)
    # a.discard(99)   # no error if missing
    # a.remove(1)     # KeyError if missing!

    # TASK 15-B: Set comprehension
    # TODO:
    # evens = {x for x in range(20) if x % 2 == 0}
    # sentences = ['Warsaw Paris Berlin', 'Praha Warsaw Berlin Caen']
    # unique_words = {word for sentence in sentences for word in sentence.split()}

    # TASK 15-C: frozenset — immutable set; can be dict key or set element
    # TODO:
    # fs = frozenset([1, 2, 3])
    # my_dict = {fs: "value"}   # frozenset as dict key
    # outer_set = {frozenset({1, 2}), frozenset({3, 4})}

    # TASK 15-D: Sets for deduplication
    # TODO:
    # names = ["Alice", "Bob", "Alice", "Carol", "Bob"]
    # unique = list(set(names))
    # print(sorted(unique))   # sorted alphabetically
    pass


# ══════════════════════════════════════════════════════════════
# PART 16 — SLICING: universal syntax [start:stop:step]
# ══════════════════════════════════════════════════════════════
def part16_slicing():
    """
    s[start:stop:step]
    - stop is EXCLUSIVE (not included)
    - negative indices count from the end
    - negative step reverses direction
    - works on list, tuple, str, bytes, range
    """

    # TASK 16-A: All slicing patterns
    # TODO:
    # s = list(range(10))  # [0,1,2,3,4,5,6,7,8,9]
    # print(s[2:7])        # [2,3,4,5,6]
    # print(s[::2])        # [0,2,4,6,8]
    # print(s[::-1])       # [9,8,7,6,5,4,3,2,1,0]
    # print(s[2:8:2])      # [2,4,6]
    # print(s[-3:])        # [7,8,9]
    # print(s[:-3])        # [0,1,2,3,4,5,6]
    # print(s[1:-1])       # [1,2,3,4,5,6,7,8]

    # TASK 16-B: Slice a string
    # TODO:
    # t = "Python"
    # print(t[::-1])   # "nohtyP"
    # print(t[1:5])    # "ytho"

    # TASK 16-C: Use slicing as a shallow copy
    # TODO:
    # original = [1, 2, 3]
    # copy = original[:]       # shallow copy
    # copy.append(4)
    # print(original)          # [1,2,3] — unchanged
    pass


# ══════════════════════════════════════════════════════════════
# PART 17 — FUNCTIONS: scopes (LEGB), default args, *args, **kwargs
# ══════════════════════════════════════════════════════════════
def part17_functions_and_scopes():
    """
    LEGB scope resolution order:
      L — Local (inside current function)
      E — Enclosing (any outer function scopes)
      G — Global (module level)
      B — Built-in (len, print, range, ...)

    TRAP: mutable default arguments are shared across calls!
    """

    # TASK 17-A: LEGB demonstration
    # TODO:
    # x = "global"
    # def outer():
    #     x = "enclosing"
    #     def inner():
    #         x = "local"
    #         print(x)    # local
    #     inner()
    #     print(x)        # enclosing
    # outer()
    # print(x)            # global

    # TASK 17-B: global keyword
    # TODO:
    # count = 0
    # def increment():
    #     global count
    #     count += 1
    # increment(); increment()
    # print(count)  # 2

    # TASK 17-C: Default argument TRAP — mutable default is shared!
    # TODO:
    # def bad_append(val, lst=[]):   # BUG: lst is created ONCE
    #     lst.append(val)
    #     return lst
    # print(bad_append(1))   # [1]
    # print(bad_append(2))   # [1, 2]  ← NOT [2]!
    # Fix:
    # def good_append(val, lst=None):
    #     if lst is None:
    #         lst = []
    #     lst.append(val)
    #     return lst

    # TASK 17-D: *args (variable positional args — received as tuple)
    # TODO:
    # def sum_all(*args):
    #     return sum(args)
    # print(sum_all(1, 2, 3, 4, 5))  # 15

    # TASK 17-E: **kwargs (variable keyword args — received as dict)
    # TODO:
    # def describe(**kwargs):
    #     for k, v in kwargs.items():
    #         print(f"  {k}: {v}")
    # describe(name="Alice", age=30, city="Warsaw")

    # TASK 17-F: Combined signature — order: positional, *args, keyword-only, **kwargs
    # TODO:
    # def mixed(a, b, *args, sep=",", **kwargs):
    #     print(a, b, args, sep, kwargs)
    # mixed(1, 2, 3, 4, sep="|", key="value")
    pass


# ══════════════════════════════════════════════════════════════
# PART 18 — LAMBDA, filter, map, reduce, sorted with key
# ══════════════════════════════════════════════════════════════
def part18_lambda_map_filter_reduce():
    """
    EXAM: Prefer list comprehensions over map/filter for clarity.
    Lambda = anonymous function; body must be a SINGLE expression.
    """

    # TASK 18-A: Lambda basics
    # TODO:
    # square = lambda x: x * x
    # print(square(5))   # 25
    # add = lambda x, y: x + y
    # print(add(3, 4))   # 7
    # palindrome = lambda s: s == s[::-1]
    # print(palindrome("racecar"))  # True

    # TASK 18-B: Sorting with lambda key
    # TODO:
    # people = [("Alice", 30), ("Bob", 25), ("Carol", 35)]
    # people.sort(key=lambda p: p[1])      # sort by age
    # print(people)
    # sorted_by_name = sorted(people, key=lambda p: p[0])

    # TASK 18-C: filter, map, reduce
    # TODO:
    # numbers = [1, 2, 3, 4, 5, 6]
    # evens   = list(filter(lambda x: x % 2 == 0, numbers))  # [2,4,6]
    # doubled = list(map(lambda x: x * 2, numbers))          # [2,4,6,8,10,12]
    # total   = functools.reduce(lambda a, b: a + b, numbers)# 21
    # # Prefer comprehensions:
    # evens_comp   = [x for x in numbers if x % 2 == 0]
    # doubled_comp = [x * 2 for x in numbers]

    # TASK 18-D: min/max/sorted with key
    # TODO:
    # words = ["banana", "apple", "cherry", "date"]
    # print(min(words, key=len))     # 'date'
    # print(max(words, key=len))     # 'banana'
    # print(sorted(words, key=len))  # by length
    # print(sorted(words, key=str.lower, reverse=True))  # case-insensitive desc

    # TASK 18-E: filter(None, iterable) — removes all falsy values
    # TODO:
    # mixed = [1, 0, "", "hello", None, [], 42, False]
    # truthy_only = list(filter(None, mixed))
    # print(truthy_only)  # [1, 'hello', 42]
    pass


# ══════════════════════════════════════════════════════════════
# PART 19 — CLOSURES and nonlocal
# ══════════════════════════════════════════════════════════════
def part19_closures():
    """
    CLOSURE: inner function that captures (closes over) a variable
    from its enclosing scope. The variable is shared, not copied.
    'nonlocal' lets the inner function MODIFY the enclosing variable.
    """

    # TASK 19-A: make_counter factory — classic closure pattern
    # TODO:
    # def make_counter(start=0):
    #     count = start                # captured in closure
    #     def increment(by=1):
    #         nonlocal count           # modify enclosing variable
    #         count += by
    #         return count
    #     return increment
    # counter = make_counter(10)
    # print(counter())    # 11
    # print(counter())    # 12
    # print(counter(5))   # 17

    # TASK 19-B: Closure captures VARIABLE by reference — classic trap!
    # TODO:
    # funcs = []
    # for i in range(3):
    #     funcs.append(lambda: i)     # captures 'i' by reference!
    # print([f() for f in funcs])    # [2, 2, 2]  NOT [0, 1, 2]!
    # Fix: use default argument to capture current value:
    # funcs_fixed = []
    # for i in range(3):
    #     funcs_fixed.append(lambda i=i: i)  # i=i captures current value
    # print([f() for f in funcs_fixed])  # [0, 1, 2]

    # TASK 19-C: make_multiplier factory
    # TODO:
    # def make_multiplier(factor):
    #     return lambda x: x * factor
    # double = make_multiplier(2)
    # triple = make_multiplier(3)
    # print(double(5))   # 10
    # print(triple(5))   # 15
    pass


# ══════════════════════════════════════════════════════════════
# PART 20 — DECORATORS
# ══════════════════════════════════════════════════════════════
def part20_decorators():
    """
    A decorator is a function that wraps another function.
    ALWAYS use @functools.wraps(func) to preserve metadata.
    Parameterised decorators add an extra outer layer.
    """

    # TASK 20-A: Simple timer decorator
    # TODO:
    # def timer(func):
    #     @functools.wraps(func)
    #     def wrapper(*args, **kwargs):
    #         import time
    #         start = time.perf_counter()
    #         result = func(*args, **kwargs)
    #         elapsed = time.perf_counter() - start
    #         print(f"{func.__name__} took {elapsed:.4f}s")
    #         return result
    #     return wrapper
    # @timer
    # def slow_sum(n):
    #     return sum(range(n))
    # slow_sum(1_000_000)

    # TASK 20-B: Logger decorator that prints args and result
    # TODO:
    # def logger(func):
    #     @functools.wraps(func)
    #     def wrapper(*args, **kwargs):
    #         print(f"Calling {func.__name__} with {args} {kwargs}")
    #         result = func(*args, **kwargs)
    #         print(f"{func.__name__} returned {result}")
    #         return result
    #     return wrapper
    # @logger
    # def add(a, b):
    #     return a + b
    # add(3, 4)

    # TASK 20-C: Parameterised decorator — @repeat(n)
    # TODO:
    # def repeat(times):
    #     def decorator(func):
    #         @functools.wraps(func)
    #         def wrapper(*args, **kwargs):
    #             for _ in range(times):
    #                 func(*args, **kwargs)
    #         return wrapper
    #     return decorator
    # @repeat(3)
    # def hello():
    #     print("Hello!")
    # hello()   # prints "Hello!" 3 times

    # TASK 20-D: Stacking decorators — applied bottom-up
    # TODO:
    # @timer
    # @logger
    # def multiply(a, b):
    #     return a * b
    # multiply(6, 7)
    # NOTE: @timer is outermost — logger runs first, then timer wraps logger+multiply
    pass


# ══════════════════════════════════════════════════════════════
# PART 21 — COMPREHENSIONS: list, set, dict, nested
# ══════════════════════════════════════════════════════════════
def part21_comprehensions():
    # TASK 21-A: List comprehensions
    # TODO:
    # squares = [x**2 for x in range(10)]
    # evens   = [x for x in range(20) if x % 2 == 0]
    # matrix  = [[i*j for j in range(1, 4)] for i in range(1, 4)]

    # TASK 21-B: Set comprehension — automatic deduplication
    # TODO:
    # unique_lengths = {len(w) for w in ["hello", "world", "python", "hi"]}
    # sentences = ['Warsaw Paris Berlin', 'Praha Warsaw Berlin Caen']
    # unique_words = {word for s in sentences for word in s.split()}

    # TASK 21-C: Dict comprehension
    # TODO:
    # word_len = {w: len(w) for w in ["apple", "banana", "cherry"]}
    # inverted = {v: k for k, v in word_len.items()}  # flip keys and values

    # TASK 21-D: Nested comprehension — flatten 2D matrix
    # TODO:
    # matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]
    # flat = [x for row in matrix for x in row]   # [1,2,3,4,5,6,7,8,9]

    # TASK 21-E: Conditional expression inside comprehension
    # TODO:
    # classified = ["even" if x % 2 == 0 else "odd" for x in range(8)]

    # TASK 21-F: Lab task from PPY03 — group names by length
    # TODO:
    # names = ["Alice", "Bob", "Carol", "Dave", "Eve", "Frank"]
    # by_length = {length: [n for n in names if len(n) == length]
    #              for length in {len(n) for n in names}}
    # print(by_length)
    pass


# ══════════════════════════════════════════════════════════════
# PART 22 — GENERATOR FUNCTIONS: yield, yield break, send()
# ══════════════════════════════════════════════════════════════
def part22_generators_yield():
    """
    EXAM: yield turns a function into a generator.
    Generator produces values LAZILY — one at a time, on demand.
    gen.send(value) sends a value IN to a paused generator (coroutine).
    Must call next(gen) first to advance to the first yield.
    """

    # TASK 22-A: Simple generator — Fibonacci
    # TODO:
    # def fibonacci(n):
    #     curr, prev, count = 0, 1, 0
    #     while count <= n:
    #         yield curr
    #         prev, curr = curr, prev + curr
    #         count += 1
    # for f in fibonacci(10):
    #     print(f, end=" ")

    # TASK 22-B: yield break (using return in a generator)
    # TODO:
    # def active_students(students):
    #     for name, active, expelled in students:
    #         if expelled:
    #             return        # 'return' in generator = yield break
    #         if active:
    #             yield name
    # data = [("Alice", True, False), ("Bob", True, False), ("Carol", False, True), ("Dave", True, False)]
    # print(list(active_students(data)))  # ["Alice", "Bob"] — stops at Carol (expelled)

    # TASK 22-C: Generator — read large file lazily (memory efficient)
    # TODO:
    # def read_lines_lazy(path):
    #     with open(path, encoding="utf-8") as f:
    #         for line in f:
    #             yield line.rstrip()

    # TASK 22-D: PPY05 task — send() coroutine: filter even numbers
    # TODO:
    # def filter_even_send():
    #     while True:
    #         value = yield       # receive value via send()
    #         if value is not None and value % 2 == 0:
    #             yield value     # yield it back
    #         else:
    #             yield None
    # gen = filter_even_send()
    # next(gen)             # prime the generator (advance to first yield)
    # print(gen.send(1))    # None (odd)
    # print(gen.send(2))    # 2   (even)
    # print(gen.send(3))    # None

    # TASK 22-E: PPY05 task — cumulative product with send()
    # TODO:
    # def cumulative_product_send():
    #     total = 1
    #     while True:
    #         value = yield total
    #         if value is not None:
    #             total *= value
    # gen = cumulative_product_send()
    # next(gen)
    # print(gen.send(2))    # 2
    # print(gen.send(3))    # 6
    # print(gen.send(4))    # 24

    # TASK 22-F: PPY04 task — Wizard fight generator
    # TODO:
    # class Attack:
    #     def __init__(self, name, damage):
    #         self.name = name
    #         self.damage = damage
    # class Wizard:
    #     def __init__(self, name, hp):
    #         self.name = name
    #         self.hp = hp
    #         self.attacks = []
    #     def add_attack(self, attack): self.attacks.append(attack)
    #     def is_alive(self): return self.hp > 0
    #     def attack(self, opponent):
    #         chosen = random.choice(self.attacks)
    #         opponent.hp -= chosen.damage
    #         return f"{self.name} uses {chosen.name} on {opponent.name} for {chosen.damage} dmg"
    # def fight(w1, w2):
    #     turn = 0
    #     while w1.is_alive() and w2.is_alive():
    #         attacker, defender = (w1, w2) if turn % 2 == 0 else (w2, w1)
    #         yield attacker.attack(defender)
    #         turn += 1
    #     winner = w1 if w1.is_alive() else w2
    #     yield f"{winner.name} wins!"
    pass


# ══════════════════════════════════════════════════════════════
# PART 23 — GENERATOR EXPRESSIONS (lazy, memory-efficient)
# ══════════════════════════════════════════════════════════════
def part23_generator_expressions():
    """
    Generator expressions use () instead of [].
    They produce values lazily — no memory spike.
    Use instead of list comprehension when you only need to iterate once
    or pass to sum()/min()/max()/any()/all().
    """

    # TASK 23-A: Compare list comprehension vs generator expression
    # TODO:
    # import sys
    # list_comp = [x**2 for x in range(1_000_000)]
    # gen_expr  = (x**2 for x in range(1_000_000))
    # print(sys.getsizeof(list_comp))  # huge (8+ MB)
    # print(sys.getsizeof(gen_expr))   # tiny (~200 bytes)

    # TASK 23-B: Sum with generator (no list needed)
    # TODO:
    # total = sum(x**2 for x in range(1_000_001))
    # print(f"Sum of squares 1..1M = {total:,}")

    # TASK 23-C: any() and all() with generators — short-circuit!
    # TODO:
    # numbers = [2, 4, 6, 8, 11]
    # print(all(x % 2 == 0 for x in numbers))  # False (11 is odd)
    # print(any(x > 10 for x in numbers))       # True  (11 > 10)

    # TASK 23-D: PPY04 task — sum of squares without creating a list
    # TODO:
    # n = 1_000_000
    # result = sum(i*i for i in range(1, n+1))
    # print(result)

    # TASK 23-E: find longest word without creating a list
    # TODO:
    # text = "the quick brown fox jumps over the lazy dog"
    # longest = max(text.split(), key=len)
    # # With generator:
    # longest_gen = max((word for word in text.split()), key=len)
    # print(longest, longest_gen)
    pass


# ══════════════════════════════════════════════════════════════
# PART 24 — itertools DEMO
# ══════════════════════════════════════════════════════════════
def part24_itertools_demo():
    # TASK 24-A: chain — flatten iterables
    # TODO:
    # result = list(itertools.chain([1, 2], [3, 4], [5]))
    # print(result)   # [1,2,3,4,5]

    # TASK 24-B: islice — lazy slicing of any iterable
    # TODO:
    # result = list(itertools.islice(range(100), 5, 15, 2))
    # print(result)   # [5,7,9,11,13]

    # TASK 24-C: cycle and repeat
    # TODO:
    # cycling = list(itertools.islice(itertools.cycle("ABC"), 7))
    # print(cycling)   # ['A','B','C','A','B','C','A']
    # repeated = list(itertools.repeat(0, 5))
    # print(repeated)  # [0,0,0,0,0]

    # TASK 24-D: accumulate — running totals (or any binary function)
    # TODO:
    # running_sum = list(itertools.accumulate([1, 2, 3, 4, 5]))
    # print(running_sum)   # [1,3,6,10,15]
    # import operator
    # running_prod = list(itertools.accumulate([1, 2, 3, 4, 5], operator.mul))
    # print(running_prod)  # [1,2,6,24,120]

    # TASK 24-E: groupby — group consecutive elements by key
    # TODO:
    # data = [("A", 1), ("A", 2), ("B", 3), ("B", 4), ("A", 5)]
    # for key, group in itertools.groupby(data, key=lambda x: x[0]):
    #     print(key, list(group))
    # NOTE: groupby only groups CONSECUTIVE equal-key elements!
    # Sort first if you want all same-key elements together.

    # TASK 24-F: combinations and permutations
    # TODO:
    # print(list(itertools.combinations("ABC", 2)))
    # print(list(itertools.permutations("AB", 2)))
    pass


# ══════════════════════════════════════════════════════════════
# PART 25 — OOP BASICS: classes, instance vs class attributes
# ══════════════════════════════════════════════════════════════
def part25_oop_basics():
    # TASK 25-A: Create a Dog class with class attribute and instance attributes
    # TODO:
    # class Dog:
    #     species = "Canis lupus familiaris"   # class attribute (shared by all)
    #     def __init__(self, name, age):
    #         self.name = name                 # instance attribute
    #         self.age  = age
    #     def bark(self):
    #         return f"{self.name} says Woof!"
    #     @classmethod
    #     def from_birth_year(cls, name, year):   # alternative constructor
    #         return cls(name, 2026 - year)
    #     @staticmethod
    #     def is_adult(age):                       # no self/cls needed
    #         return age >= 2
    # buddy = Dog("Buddy", 3)
    # rex   = Dog.from_birth_year("Rex", 2021)
    # print(buddy.bark())
    # print(Dog.species)           # via class
    # print(buddy.species)         # via instance (looks up class attribute)
    # print(Dog.is_adult(3))       # True

    # TASK 25-B: Demonstrate class attr vs instance attr conflict
    # TODO:
    # buddy.species = "Custom"     # creates INSTANCE attribute, hides class attr
    # print(buddy.species)         # "Custom"   — instance attr wins
    # print(Dog.species)           # still "Canis lupus familiaris"
    pass


# ══════════════════════════════════════════════════════════════
# PART 26 — DUNDER (MAGIC) METHODS
# ══════════════════════════════════════════════════════════════
def part26_dunder_methods():
    # TASK 26-A: Implement a SmartList with full dunder support
    # TODO:
    # class SmartList:
    #     def __init__(self, *items):
    #         self._data = list(items)
    #     def __len__(self):
    #         return len(self._data)
    #     def __getitem__(self, index):
    #         return self._data[index]
    #     def __setitem__(self, index, value):
    #         self._data[index] = value
    #     def __contains__(self, item):
    #         return item in self._data
    #     def __iter__(self):
    #         return iter(self._data)
    #     def __repr__(self):
    #         return f"SmartList({self._data!r})"
    #     def __str__(self):
    #         return f"[{', '.join(str(x) for x in self._data)}]"
    #     def __eq__(self, other):
    #         return isinstance(other, SmartList) and self._data == other._data
    # sl = SmartList(1, 2, 3)
    # print(len(sl))        # __len__
    # print(sl[1])          # __getitem__
    # print(2 in sl)        # __contains__
    # for item in sl:       # __iter__
    #     print(item)
    # print(repr(sl))       # __repr__
    # print(str(sl))        # __str__

    # TASK 26-B: __call__ — make an instance callable
    # TODO:
    # class Multiplier:
    #     def __init__(self, factor):
    #         self.factor = factor
    #     def __call__(self, x):
    #         return x * self.factor
    # double = Multiplier(2)
    # print(double(5))   # 10 — calling an instance!
    # print(callable(double))  # True
    pass


# ══════════════════════════════════════════════════════════════
# PART 27 — INHERITANCE, super(), polymorphism, isinstance
# ══════════════════════════════════════════════════════════════
def part27_inheritance():
    # TASK 27-A: Animal hierarchy with polymorphism
    # TODO:
    # class Animal:
    #     def __init__(self, name, legs=None):
    #         self.name = name
    #         self.legs = legs
    #     def speak(self):
    #         return "(generic sound)"
    #     def __str__(self):
    #         return f"{self.__class__.__name__}({self.name})"
    # class Dog(Animal):
    #     def __init__(self, name):
    #         super().__init__(name, legs=4)
    #     def speak(self):
    #         return f"{self.name} says Woof!"
    # class Cat(Animal):
    #     def __init__(self, name):
    #         super().__init__(name, legs=4)
    #     def speak(self):
    #         return f"{self.name} says Meow!"
    # class Snake(Animal):
    #     def __init__(self, name):
    #         super().__init__(name, legs=0)
    #     def speak(self):
    #         return f"{self.name} says Ssss!"
    # animals = [Dog("Buddy"), Cat("Whiskers"), Snake("Basil"), Animal("Unknown")]
    # for a in animals:
    #     print(a.speak())   # polymorphism — correct method for each type

    # TASK 27-B: isinstance and issubclass
    # TODO:
    # print(isinstance(Dog("Rex"), Dog))     # True
    # print(isinstance(Dog("Rex"), Animal))  # True — checks up hierarchy
    # print(issubclass(Dog, Animal))         # True
    # print(issubclass(Animal, Dog))         # False

    # TASK 27-C: Multiple inheritance (MRO — Method Resolution Order)
    # TODO:
    # class Flyable:
    #     def move(self): return "flying"
    # class Swimmable:
    #     def move(self): return "swimming"
    # class Duck(Flyable, Swimmable):   # MRO: Duck → Flyable → Swimmable
    #     pass
    # d = Duck()
    # print(d.move())         # "flying" — Flyable comes first
    # print(Duck.__mro__)     # shows full MRO
    pass


# ══════════════════════════════════════════════════════════════
# PART 28 — PROPERTIES (@property, setter, computed/read-only)
# ══════════════════════════════════════════════════════════════
def part28_properties():
    # TASK 28-A: Temperature class with validation
    # TODO:
    # class Temperature:
    #     def __init__(self, celsius=0):
    #         self._celsius = celsius          # backing variable (private by convention)
    #     @property
    #     def celsius(self):
    #         return self._celsius
    #     @celsius.setter
    #     def celsius(self, value):
    #         if value < -273.15:
    #             raise ValueError("Temperature below absolute zero!")
    #         self._celsius = value
    #     @property
    #     def fahrenheit(self):                # computed / read-only property
    #         return self._celsius * 9/5 + 32
    # t = Temperature(25)
    # print(t.celsius)     # 25
    # print(t.fahrenheit)  # 77.0
    # t.celsius = 100
    # print(t.fahrenheit)  # 212.0
    # t.celsius = -300     # raises ValueError

    # TASK 28-B: BankAccount with validated Balance
    # TODO:
    # class BankAccount:
    #     def __init__(self, owner, balance=0):
    #         self.owner = owner
    #         self._balance = balance
    #     @property
    #     def balance(self):
    #         return self._balance
    #     def deposit(self, amount):
    #         if amount <= 0: raise ValueError("Deposit must be positive")
    #         self._balance += amount
    #     def withdraw(self, amount):
    #         if amount > self._balance: raise ValueError("Insufficient funds")
    #         self._balance -= amount
    #     def __str__(self):
    #         return f"BankAccount({self.owner}, balance={self._balance:.2f})"
    pass


# ══════════════════════════════════════════════════════════════
# PART 29 — ABSTRACT BASE CLASSES (ABC, @abstractmethod)
# ══════════════════════════════════════════════════════════════
def part29_abc():
    """
    ABC = Abstract Base Class. Cannot be instantiated directly.
    Subclasses MUST implement all @abstractmethod methods.
    Like Java's abstract class / interface.
    """

    # TASK 29-A: Shape hierarchy using ABC
    # TODO:
    # class Shape(ABC):
    #     @abstractmethod
    #     def area(self) -> float:
    #         ...
    #     @abstractmethod
    #     def perimeter(self) -> float:
    #         ...
    #     def describe(self):
    #         print(f"{self.__class__.__name__}: area={self.area():.2f}, "
    #               f"perimeter={self.perimeter():.2f}")
    # class Circle(Shape):
    #     def __init__(self, radius):
    #         self.radius = radius
    #     def area(self):
    #         return math.pi * self.radius ** 2
    #     def perimeter(self):
    #         return 2 * math.pi * self.radius
    # class Rectangle(Shape):
    #     def __init__(self, width, height):
    #         self.width = width
    #         self.height = height
    #     def area(self):
    #         return self.width * self.height
    #     def perimeter(self):
    #         return 2 * (self.width + self.height)
    # shapes = [Circle(5), Rectangle(4, 6)]
    # for s in shapes:
    #     s.describe()
    # # Shape()  ← TypeError: cannot instantiate abstract class

    # TASK 29-B: Abstract class with shared concrete method
    # TODO:
    # class Serializable(ABC):
    #     @abstractmethod
    #     def to_dict(self) -> dict:
    #         ...
    #     def to_json(self) -> str:
    #         return json.dumps(self.to_dict())
    pass


# ══════════════════════════════════════════════════════════════
# PART 30 — OPERATOR OVERLOADING
# ══════════════════════════════════════════════════════════════
def part30_operator_overloading():
    # TASK 30-A: Implement a Vector2D class with full operator support
    # TODO:
    # class Vector2D:
    #     def __init__(self, x, y):
    #         self.x, self.y = x, y
    #     def __add__(self, other):
    #         return Vector2D(self.x + other.x, self.y + other.y)
    #     def __sub__(self, other):
    #         return Vector2D(self.x - other.x, self.y - other.y)
    #     def __mul__(self, scalar):          # Vector * scalar
    #         return Vector2D(self.x * scalar, self.y * scalar)
    #     def __rmul__(self, scalar):         # scalar * Vector
    #         return self.__mul__(scalar)
    #     def __abs__(self):                  # |v| = magnitude
    #         return (self.x**2 + self.y**2) ** 0.5
    #     def __bool__(self):
    #         return bool(self.x or self.y)
    #     def __eq__(self, other):
    #         return isinstance(other, Vector2D) and self.x == other.x and self.y == other.y
    #     def __repr__(self):
    #         return f"Vector2D({self.x}, {self.y})"
    # v1 = Vector2D(1, 2)
    # v2 = Vector2D(3, 4)
    # print(v1 + v2)    # Vector2D(4, 6)
    # print(v1 * 3)     # Vector2D(3, 6)
    # print(3 * v1)     # Vector2D(3, 6)   — uses __rmul__
    # print(abs(v2))    # 5.0
    # print(bool(Vector2D(0, 0)))  # False

    # TASK 30-B: PPY04 task — Cart class with __len__, __iter__, __getitem__
    # TODO:
    # class Product:
    #     def __init__(self, name, price, category):
    #         self.name = name
    #         self.price = price
    #         self.category = category
    # class Cart:
    #     def __init__(self):
    #         self._items = []
    #     def add(self, product): self._items.append(product)
    #     def __len__(self):      return len(self._items)
    #     def __iter__(self):     return iter(self._items)
    #     def __getitem__(self, i): return self._items[i]
    #     def total_price(self):
    #         return sum(p.price for p in self._items)
    #     def apply_discount(self, category, rate):
    #         for p in self._items:
    #             if p.category == category:
    #                 p.price *= (1 - rate)
    pass


# ══════════════════════════════════════════════════════════════
# PART 31 — @dataclass
# ══════════════════════════════════════════════════════════════
def part31_dataclass():
    """
    @dataclass auto-generates: __init__, __repr__, __eq__
    frozen=True makes it immutable (like a named tuple with types).
    field(default_factory=list) for mutable defaults.
    """

    # TASK 31-A: Basic dataclass
    # TODO:
    # @dataclass
    # class Point:
    #     x: float
    #     y: float
    #     z: float = 0.0    # default value
    # p1 = Point(1.0, 2.0)
    # p2 = Point(1.0, 2.0)
    # print(p1)             # Point(x=1.0, y=2.0, z=0.0)  — __repr__ free!
    # print(p1 == p2)       # True  — __eq__ free!

    # TASK 31-B: frozen=True — immutable dataclass
    # TODO:
    # @dataclass(frozen=True)
    # class Color:
    #     r: int
    #     g: int
    #     b: int
    # red = Color(255, 0, 0)
    # # red.r = 128  ← FrozenInstanceError!
    # my_dict = {red: "red"}  # hashable because frozen!

    # TASK 31-C: Mutable default using field(default_factory=...)
    # TODO:
    # @dataclass
    # class Inventory:
    #     name: str
    #     items: list = field(default_factory=list)   # NOT items=[] ← BUG!
    # inv = Inventory("Warehouse")
    # inv.items.append("hammer")
    pass


# ══════════════════════════════════════════════════════════════
# PART 32 — TYPE HINTS
# ══════════════════════════════════════════════════════════════
def part32_type_hints():
    """
    Type hints are OPTIONAL annotations — they do NOT affect runtime.
    They help readers and static tools (mypy, pyright).
    Python remains dynamically typed.
    """

    # TASK 32-A: Annotate functions with type hints
    # TODO:
    # def greet(name: str, times: int = 1) -> str:
    #     return (name + "! ") * times
    # print(greet("Alice"))
    # print(greet("Bob", 3))

    # TASK 32-B: Optional — means the value can be T or None
    # TODO:
    # def find_user(user_id: int) -> Optional[str]:
    #     users = {1: "Alice", 2: "Bob"}
    #     return users.get(user_id)   # returns str or None

    # TASK 32-C: Union (Python 3.10+: use X | Y instead of Union[X, Y])
    # TODO:
    # def process(data: str | int | None) -> list[int]:
    #     if data is None: return []
    #     if isinstance(data, str): return [len(data)]
    #     return [data]

    # TASK 32-D: Callable type hint
    # TODO:
    # def apply(func: Callable[[int], int], value: int) -> int:
    #     return func(value)
    # print(apply(lambda x: x * 2, 5))   # 10

    # TASK 32-E: Generic TypeVar
    # TODO:
    # T = TypeVar("T")
    # def first(lst: list[T]) -> T:
    #     return lst[0]
    # print(first([1, 2, 3]))     # 1
    # print(first(["a", "b"]))    # "a"
    pass


# ══════════════════════════════════════════════════════════════
# PART 33 — SINGLE DISPATCH (@functools.singledispatch)
# ══════════════════════════════════════════════════════════════
def part33_single_dispatch():
    """
    @singledispatch = Python's function 'overloading'.
    Dispatch is based on the TYPE of the first argument.
    Register additional implementations with @fn.register(type).
    """

    # TASK 33-A: process() function with type-based dispatch
    # TODO:
    # @functools.singledispatch
    # def process(data):
    #     raise NotImplementedError(f"No handler for {type(data)}")
    # @process.register(str)
    # def _(data: str):
    #     return data.upper()
    # @process.register(int)
    # def _(data: int):
    #     return data * 2
    # @process.register(list)
    # def _(data: list):
    #     return [process(item) for item in data]
    # print(process("hello"))    # 'HELLO'
    # print(process(21))         # 42
    # print(process([1, "a"]))   # [2, 'A']
    pass


# ══════════════════════════════════════════════════════════════
# PART 34 — EXCEPTIONS: try/except/else/finally, raise, re-raise
# ══════════════════════════════════════════════════════════════
def part34_exceptions():
    """
    Structure: try → except → else → finally
    - 'else' runs ONLY IF no exception was raised
    - 'finally' ALWAYS runs (cleanup)
    - 'raise' without args re-raises the current exception
    """

    # TASK 34-A: Full try/except/else/finally
    # TODO:
    # def safe_divide(a, b):
    #     try:
    #         result = a / b
    #     except ZeroDivisionError:
    #         print("Cannot divide by zero")
    #         return None
    #     except TypeError as e:
    #         print(f"Type error: {e}")
    #         return None
    #     except Exception as e:
    #         print(f"Unexpected: {e}")
    #         raise              # re-raise — never swallow unknown errors!
    #     else:
    #         print(f"Success: {result}")  # only if no exception
    #         return result
    #     finally:
    #         print("Always runs — cleanup here")
    # safe_divide(10, 2)
    # safe_divide(10, 0)
    # safe_divide(10, "x")

    # TASK 34-B: Common exceptions — know all of these
    # ValueError:         int("abc")
    # TypeError:          "2" + 2
    # IndexError:         [1, 2][99]
    # KeyError:           {}["missing"]
    # AttributeError:     None.strip()
    # FileNotFoundError:  open("no_such_file.txt")
    # ZeroDivisionError:  1 / 0
    # StopIteration:      manually exhausted iterator
    # TODO: write try/except blocks that trigger and catch each one
    pass


# ══════════════════════════════════════════════════════════════
# PART 35 — CUSTOM EXCEPTIONS
# ══════════════════════════════════════════════════════════════
def part35_custom_exceptions():
    # TASK 35-A: Custom exception hierarchy
    # TODO:
    # class AppError(Exception):
    #     """Base for all application errors."""
    #     pass
    # class InsufficientFundsError(AppError):
    #     def __init__(self, balance, amount):
    #         self.balance = balance
    #         self.amount  = amount
    #         super().__init__(
    #             f"Insufficient funds: balance={balance}, requested={amount}"
    #         )
    # class AccountFrozenError(AppError):
    #     pass   # pass = use default Exception message
    # def withdraw(balance, amount, frozen=False):
    #     if frozen:
    #         raise AccountFrozenError("Account is frozen")
    #     if amount > balance:
    #         raise InsufficientFundsError(balance, amount)
    #     return balance - amount
    # try:
    #     withdraw(100, 250)
    # except InsufficientFundsError as e:
    #     print(f"Error: {e}")
    #     print(f"Balance was: {e.balance}")
    # try:
    #     withdraw(1000, 50, frozen=True)
    # except AccountFrozenError as e:
    #     print(f"Frozen: {e}")

    # TASK 35-B: PPY flow control lab — custom ZeroArg and NegArg
    # TODO:
    # class ZeroArg(Exception): pass
    # class NegArg(Exception):  pass
    # def safe_log(num, denom):
    #     if denom == 0: raise ZeroArg("denominator is zero")
    #     if num <= 0:   raise NegArg("num must be positive for log")
    #     return math.log10(abs(num)) / denom
    # for n, d in [(100, 2), (0, 2), (10, 0)]:
    #     try:
    #         print(f"f({n},{d}) = {safe_log(n, d)}")
    #     except Exception as e:
    #         print(f"f({n},{d}) -> {type(e).__name__}: {e}")
    pass


# ══════════════════════════════════════════════════════════════
# PART 36 — CONTEXT MANAGERS (__enter__/__exit__, @contextmanager)
# ══════════════════════════════════════════════════════════════
def part36_context_managers():
    """
    Context managers guarantee cleanup even if exceptions occur.
    Two ways: class with __enter__/__exit__, or @contextmanager generator.
    File objects are context managers — always use 'with open(...)'.
    """

    # TASK 36-A: Class-based context manager
    # TODO:
    # class Timer:
    #     def __enter__(self):
    #         import time
    #         self.start = time.perf_counter()
    #         return self
    #     def __exit__(self, exc_type, exc_val, exc_tb):
    #         self.elapsed = time.perf_counter() - self.start
    #         print(f"Elapsed: {self.elapsed:.4f}s")
    #         return False   # False = don't suppress exceptions
    # with Timer() as t:
    #     sum(range(1_000_000))

    # TASK 36-B: @contextmanager — simpler generator-based approach
    # TODO:
    # @contextmanager
    # def managed_db_connection():
    #     print("Opening connection...")
    #     connection = {"status": "open"}
    #     try:
    #         yield connection          # code inside 'with' block runs here
    #     finally:
    #         print("Closing connection...")
    #         connection["status"] = "closed"
    # with managed_db_connection() as conn:
    #     print(f"Using connection: {conn}")

    # TASK 36-C: Multiple context managers in one with statement
    # TODO:
    # # with open("in.txt") as fin, open("out.txt", "w") as fout:
    # #     fout.write(fin.read().upper())
    pass


# ══════════════════════════════════════════════════════════════
# PART 37 — FILE I/O: read, write, append, JSON, pathlib
# ══════════════════════════════════════════════════════════════
def part37_file_io():
    # TASK 37-A: Write a file, read it back, append to it
    # TODO:
    # with open("/tmp/test.txt", "w", encoding="utf-8") as f:
    #     f.write("Hello, World!\n")
    #     f.writelines(["line2\n", "line3\n"])
    # with open("/tmp/test.txt", "r", encoding="utf-8") as f:
    #     content = f.read()
    #     print(content)
    # with open("/tmp/test.txt", "r") as f:
    #     for line in f:              # memory-efficient for large files
    #         print(line.rstrip())
    # with open("/tmp/test.txt", "a") as f:
    #     f.write("appended line\n")

    # TASK 37-B: JSON round-trip
    # TODO:
    # data = {"name": "Alice", "scores": [90, 85, 92], "active": True}
    # with open("/tmp/data.json", "w") as f:
    #     json.dump(data, f, indent=2)
    # with open("/tmp/data.json") as f:
    #     loaded = json.load(f)
    # print(loaded == data)   # True

    # TASK 37-C: pathlib — modern OOP path handling
    # TODO:
    # p = Path("/tmp/mydir/results.txt")
    # p.parent.mkdir(parents=True, exist_ok=True)
    # p.write_text("content here")
    # print(p.read_text())
    # print(list(Path("/tmp").glob("*.txt")))   # find all .txt files
    # print(p.suffix)   # '.txt'
    # print(p.stem)     # 'results'
    # print(p.name)     # 'results.txt'
    pass


# ══════════════════════════════════════════════════════════════
# PART 38 — MODULES: import styles, __name__ guard
# ══════════════════════════════════════════════════════════════
def part38_modules():
    """
    EXAM: __name__ == '__main__' is True ONLY when the file is run directly.
    It is False when imported as a module.
    This guard prevents test code from running on import.
    """

    # TASK 38-A: Import styles (write all 5 variations in comments)
    # TODO:
    # import math                       # math.sqrt(4)
    # import math as m                  # m.sqrt(4)
    # from math import sqrt, pi         # sqrt(4), pi
    # from math import sqrt as root     # root(4)
    # from math import *                # sqrt(4) — not recommended

    # TASK 38-B: __name__ guard — code that only runs when executed directly
    # TODO (comment explaining the pattern):
    # def main():
    #     print("Running main logic")
    # if __name__ == "__main__":
    #     main()
    # When imported: __name__ = 'module_name', so main() does NOT run.
    # When run directly: __name__ = '__main__', so main() DOES run.

    # TASK 38-C: dir() — list available attributes/functions in a module
    # TODO:
    # print([x for x in dir(math) if not x.startswith("_")])
    pass


# ══════════════════════════════════════════════════════════════
# PART 39 — collections MODULE: Counter, deque, defaultdict, ChainMap
# ══════════════════════════════════════════════════════════════
def part39_collections_module():
    # TASK 39-A: Counter — count occurrences efficiently
    # TODO:
    # text = "the quick brown fox jumps over the lazy dog"
    # c = Counter(text.split())
    # print(c.most_common(3))   # 3 most frequent words
    # print(c["the"])           # 2
    # c2 = Counter("abcaab")
    # print(c2)   # Counter({'a': 3, 'b': 2, 'c': 1})

    # TASK 39-B: deque — double-ended queue, O(1) on both ends
    # TODO:
    # d = deque([1, 2, 3], maxlen=5)
    # d.appendleft(0)    # [0,1,2,3]
    # d.append(4)        # [0,1,2,3,4]
    # d.popleft()        # removes 0
    # d.rotate(1)        # rotate right by 1

    # TASK 39-C: defaultdict — no KeyError for missing keys
    # TODO:
    # graph = defaultdict(set)
    # edges = [("A", "B"), ("A", "C"), ("B", "C")]
    # for u, v in edges:
    #     graph[u].add(v)
    #     graph[v].add(u)
    # print(dict(graph))

    # TASK 39-D: ChainMap — overlay multiple dicts
    # TODO:
    # defaults  = {"color": "red", "size": "M", "font": "Arial"}
    # overrides = {"size": "L", "font": "Comic Sans"}
    # config = ChainMap(overrides, defaults)
    # print(config["size"])    # 'L'   — overrides wins
    # print(config["color"])   # 'red' — from defaults (not in overrides)
    pass


# ══════════════════════════════════════════════════════════════
# PART 40 — unittest: TestCase, assertions, setUp/tearDown
# ══════════════════════════════════════════════════════════════
def part40_unittest_demo():
    """
    Run with: python -m pytest practice_all.py -v
    or:       python practice_all.py  (if unittest.main() at end)
    """

    # TASK 40-A: Write tests for a BankAccount class
    # TODO:

    # First implement (or import) BankAccount:
    class BankAccount:
        def __init__(self, owner, balance=0):
            self.owner = owner
            self._balance = balance

        @property
        def balance(self):
            return self._balance

        def deposit(self, amount):
            if amount <= 0:
                raise ValueError("Must be positive")
            self._balance += amount

        def withdraw(self, amount):
            if amount > self._balance:
                raise ValueError("Insufficient funds")
            self._balance -= amount

    class TestBankAccount(unittest.TestCase):
        def setUp(self):
            # TODO: create a shared account for tests
            self.acc = BankAccount("Alice", 1000)

        def test_initial_balance(self):
            # TODO:
            # self.assertEqual(self.acc.balance, 1000)
            pass

        def test_deposit(self):
            # TODO:
            # self.acc.deposit(500)
            # self.assertEqual(self.acc.balance, 1500)
            pass

        def test_withdraw(self):
            # TODO:
            # self.acc.withdraw(200)
            # self.assertEqual(self.acc.balance, 800)
            pass

        def test_withdraw_overdraft_raises(self):
            # TODO:
            # with self.assertRaises(ValueError):
            #     self.acc.withdraw(9999)
            pass

        def test_deposit_negative_raises(self):
            # TODO:
            # with self.assertRaises(ValueError):
            #     self.acc.deposit(-1)
            pass

        def test_is_instance(self):
            # TODO:
            # self.assertIsInstance(self.acc, BankAccount)
            pass

    # TASK 40-B: parametrized-style test (manual loop)
    # TODO:
    # cases = [(2, 3, 5), (0, 0, 0), (-1, 1, 0), (100, -50, 50)]
    # for a, b, expected in cases:
    #     result = a + b
    #     assert result == expected, f"add({a},{b}) = {result}, expected {expected}"
    # print("All parametrized tests passed!")

    suite = unittest.TestLoader().loadTestsFromTestCase(TestBankAccount)
    runner = unittest.TextTestRunner(verbosity=0)
    runner.run(suite)


# ══════════════════════════════════════════════════════════════
# PART 41 — ITERATOR PROTOCOL: __iter__ + __next__ custom class
# ══════════════════════════════════════════════════════════════
def part41_iterator_protocol():
    """
    To make a class iterable: implement __iter__ (returns self or iterator).
    To make it an iterator: implement __next__ (returns next or raises StopIteration).
    """

    # TASK 41-A: PPY04 task — FibonacciSequence class
    # TODO:
    # class FibonacciIterator:
    #     def __init__(self, n):
    #         self.n = n
    #         self.curr = 0
    #         self.prev = 1
    #         self.count = 0
    #     def __iter__(self):
    #         return self
    #     def __next__(self):
    #         if self.count > self.n:
    #             raise StopIteration
    #         result = self.curr
    #         self.prev, self.curr = self.curr, self.prev + self.curr
    #         self.count += 1
    #         return result
    # for f in FibonacciIterator(10):
    #     print(f, end=" ")

    # TASK 41-B: PPY04 task — Tournament class as iterator
    # TODO:
    # class Tournament:
    #     def __init__(self, pairs):
    #         self.pairs = pairs          # list of (wizard1, wizard2) tuples
    #         self._index = 0
    #     def __iter__(self):
    #         return self
    #     def __next__(self):
    #         if self._index >= len(self.pairs):
    #             raise StopIteration
    #         w1, w2 = self.pairs[self._index]
    #         self._index += 1
    #         return list(fight(w1, w2))  # run one fight, return all events

    # TASK 41-C: Manually use iter() and next() — what for-loop does internally
    # TODO:
    # lst = [10, 20, 30]
    # it = iter(lst)
    # print(next(it))   # 10
    # print(next(it))   # 20
    # print(next(it))   # 30
    # next(it)          # StopIteration!
    pass


# ══════════════════════════════════════════════════════════════
# PART 42 — PYTHONIC IDIOMS
# ══════════════════════════════════════════════════════════════
def part42_pythonic_idioms():
    # TASK 42-A: Swap without temp variable (tuple packing/unpacking)
    # TODO:
    # a, b = 5, 9
    # a, b = b, a
    # print(a, b)  # 9  5

    # TASK 42-B: Multiple assignment in one line
    # TODO:
    # x = y = z = 0
    # print(x, y, z)

    # TASK 42-C: String joining — always use join() not +
    # TODO:
    # words = ["Hello", "World", "from", "Python"]
    # print(" ".join(words))     # 'Hello World from Python'
    # print("-".join(words))

    # TASK 42-D: any() and all() — short-circuit evaluation
    # TODO:
    # nums = [2, 4, 6, 8]
    # print(all(x % 2 == 0 for x in nums))  # True — all even
    # print(any(x > 7 for x in nums))       # True — 8 > 7
    # print(all(x > 10 for x in []))        # True — vacuously true on empty!

    # TASK 42-E: Starred unpacking
    # TODO:
    # first, *middle, last = range(10)
    # print(first)   # 0
    # print(middle)  # [1,2,3,4,5,6,7,8]
    # print(last)    # 9
    # a, (b, c) = 1, (2, 3)   # nested unpacking

    # TASK 42-F: Dictionary merge (Python 3.9+)
    # TODO:
    # d1 = {"a": 1, "b": 2}
    # d2 = {"b": 99, "c": 3}
    # merged = d1 | d2   # {"a":1, "b":99, "c":3}  — d2 wins on conflict
    # print(merged)
    # # Old style (works pre-3.9):
    # merged_old = {**d1, **d2}

    # TASK 42-G: EAFP vs LBYL — Python favours EAFP
    # LBYL (Look Before You Leap) — C/Java style:
    # if key in d:
    #     value = d[key]
    # EAFP (Easier to Ask Forgiveness than Permission) — Pythonic:
    # try:
    #     value = d[key]
    # except KeyError:
    #     value = default

    # TASK 42-H: Walrus operator in while loop
    # TODO:
    # import io
    # fake_file = io.StringIO("line1\nline2\nline3")
    # while (line := fake_file.readline()):
    #     print(line.strip())
    pass


# ══════════════════════════════════════════════════════════════
# PART 43 — EXAM SIMULATIONS (PPY lab tasks)
# ══════════════════════════════════════════════════════════════
def part43_exam_simulations():
    """Full lab tasks from PPY01–PPY05 exercises."""

    # ── SIMULATION A (PPY02): Student grade analyser ──────────
    # TASK: generate 50 random scores (0-100), assign grades,
    #       compute average, count passes, find top 5 with slicing.
    # TODO:
    # def grade_analyser():
    #     scores = [random.randint(0, 100) for _ in range(50)]
    #     def grade(s):
    #         return 5 if s >= 90 else 4 if s >= 75 else 3 if s >= 60 else 2
    #     grades = [grade(s) for s in scores]
    #     avg = sum(scores) / len(scores)
    #     passes = sum(1 for s in scores if s >= 60)
    #     top5 = sorted(scores, reverse=True)[:5]
    #     print(f"Average: {avg:.1f}")
    #     print(f"Passed:  {passes}/50")
    #     print(f"Top 5:   {top5}")
    # grade_analyser()

    # ── SIMULATION B (PPY02): User manager with set ────────────
    # TASK: store usernames in a set, prevent duplicates,
    #       search, display sorted, first 5, longest username.
    # TODO:
    # def user_manager():
    #     users = set()
    #     new_users = ["Alice", "Bob", "Alice", "Carol", "Dave", "Eve", "Frank"]
    #     for u in new_users:
    #         if u in users:
    #             print(f"  {u} already exists!")
    #         else:
    #             users.add(u)
    #     sorted_users = sorted(users)
    #     print("All users:", sorted_users)
    #     print("First 5:", sorted_users[:5])
    #     print("Longest:", max(users, key=len))
    # user_manager()

    # ── SIMULATION C (PPY03): Shopping cart with dict ──────────
    # TASK: allow user to add products + prices, calculate total,
    #       find most expensive, compute average.
    # TODO:
    # def shopping_cart():
    #     cart = {}
    #     products = [("apple", 1.50), ("banana", 0.80), ("mango", 3.20), ("0", 0)]
    #     for name, price in products:
    #         if price == 0: break
    #         cart[name] = price
    #     total  = sum(cart.values())
    #     pricey = max(cart, key=cart.get)
    #     avg    = total / len(cart)
    #     print(f"Total: {total:.2f}")
    #     print(f"Most expensive: {pricey} ({cart[pricey]:.2f})")
    #     print(f"Average: {avg:.2f}")
    #     print("Items (reversed):", list(reversed(list(cart.items()))))
    # shopping_cart()

    # ── SIMULATION D (PPY03): Predicate filter function ────────
    # TASK: implement my_filter(lst, predicate) — same as built-in filter.
    #       Test with: even, positive, palindrome predicates.
    # TODO:
    # def my_filter(lst, predicate):
    #     return [x for x in lst if predicate(x)]
    # numbers = list(range(-5, 11))
    # print(my_filter(numbers, lambda x: x % 2 == 0))    # evens
    # print(my_filter(numbers, lambda x: x > 0))          # positives
    # words = ["racecar", "hello", "level", "world", "kayak"]
    # print(my_filter(words, lambda w: w == w[::-1]))      # palindromes

    # ── SIMULATION E (PPY04): FibonacciSequence class ─────────
    # TASK: class FibonacciSequence(n) with __iter__ returning a
    #       separate iterator class. Add reverse iteration.
    # TODO: (build on Part 41 template)

    # ── SIMULATION F (PPY05): Random Walk generator ────────────
    # TASK: random_walk_send(start=0) generator.
    #       Receives step (+1 or -1) via send(), yields current position.
    # TODO:
    # def random_walk_send(start=0):
    #     position = start
    #     while True:
    #         step = yield position
    #         if step is not None:
    #             position += step
    # gen = random_walk_send()
    # next(gen)              # prime
    # print(gen.send(1))     # 1
    # print(gen.send(-1))    # 0
    # print(gen.send(1))     # 1

    # ── SIMULATION G (PPY05): Shapes from user input ──────────
    # TASK: read lines with 1-3 numbers, create tuples:
    #       ('S', (n,), n²)  ('R', (w,h), w*h)  ('C', (a,b,c), a*b*c)
    # TODO:
    # def parse_shapes():
    #     results = []
    #     inputs = ["3", "4 5", "2 3 4", ""]   # simulated input
    #     for line in inputs:
    #         if not line: break
    #         nums = tuple(map(int, line.split()))
    #         if len(nums) == 1:
    #             results.append(('S', nums, nums[0]**2))
    #         elif len(nums) == 2:
    #             results.append(('R', nums, nums[0]*nums[1]))
    #         elif len(nums) == 3:
    #             results.append(('C', nums, nums[0]*nums[1]*nums[2]))
    #     print(results)
    # parse_shapes()

    pass


# ──────────────────────────────────────────────────────────────

# ─────────────────────────────────────────────────────────────
# ENTRY POINT — all sections enabled
# ─────────────────────────────────────────────────────────────
def main():
    part01_repl_and_references()
    print("-"*40)
    part02_mutability_and_copy()
    print("-" * 40)
    part03_builtin_types_overview()
    print("-" * 40)
    part04_integers()
    print("-" * 40)
    part05_floats()
    print("-" * 40)
    part06_strings()
    print("-" * 40)
    part07_control_flow_if()
    print("-" * 40)
    part08_while_loop()
    print("-" * 40)
    part09_for_loop()
    print("-" * 40)
    part10_match_statement()
    print("-" * 40)
    part11_operators()
    print("-" * 40)
    part12_lists()
    print("-" * 40)
    part13_tuples()
    print("-" * 40)
    part14_dicts()
    print("-" * 40)
    part15_sets()
    print("-" * 40)
    part16_slicing()
    print("-" * 40)
    part17_functions_and_scopes()
    print("-" * 40)
    part18_lambda_map_filter_reduce()
    print("-" * 40)
    part19_closures()
    print("-" * 40)
    part20_decorators()
    print("-" * 40)
    part21_comprehensions()
    print("-" * 40)
    part22_generators_yield()
    print("-" * 40)
    part23_generator_expressions()
    print("-" * 40)
    part24_itertools_demo()
    print("-" * 40)
    part25_oop_basics()
    print("-" * 40)
    part26_dunder_methods()
    print("-" * 40)
    part27_inheritance()
    print("-" * 40)
    part28_properties()
    print("-" * 40)
    part29_abc()
    print("-" * 40)
    part30_operator_overloading()
    print("-" * 40)
    part31_dataclass()
    print("-" * 40)
    part32_type_hints()
    print("-" * 40)
    part33_single_dispatch()
    print("-" * 40)
    part34_exceptions()
    print("-" * 40)
    part35_custom_exceptions()
    print("-" * 40)
    part36_context_managers()
    print("-" * 40)
    part37_file_io()
    print("-" * 40)
    part38_modules()
    print("-" * 40)
    part39_collections_module()
    print("-" * 40)
    part40_unittest_demo()
    print("-" * 40)
    part41_iterator_protocol()
    print("-" * 40)
    part42_pythonic_idioms()
    print("-" * 40)
    part43_exam_simulations()
    print("\n" + "="*60)
    print("Execution Complete: All PPY Practice Sections Finished.")
    print("="*60)

if __name__ == "__main__":
    main()

