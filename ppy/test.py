# =====================================================================
# PPY Mock Test Solutions & Explanations
# =====================================================================

print("--- Question 1 ---")
# Question: What would be printed out in the console?
# for x, y in zip(['hi', 'there', 'dear', 'friends'], [4, 5, 6]):
#     print(x, y)

print("Output:")
for x, y in zip({'hi', 'there', 'dear', 'friends'}, [4, 5, 6]):
    print(x, y)

print("\nExplanation:")
# The zip() function takes iterables, aggregates them in a tuple, and returns it.
# Crucially, zip() stops when the *shortest* input iterable is exhausted[cite: 16].
# The first list has 4 elements, but the second list has only 3.
# Therefore, it pairs the first three elements and stops, dropping 'friends'.
# Correct Answer: hi 4 \n there 5 \n dear 6

# =====================================================================

print("\n--- Question 2 ---")
# Question: What would be printed out in the console?
# for x, y in zip((i for i in range(3)), [4, 5, 6]):
#     print(x, y)

print("Output:")
for x, y in zip((i for i in range(3)), [4, 5, 6]):
    print(x, y)

print("\nExplanation:")
# Similar to Question 1, but the first iterable is a generator expression.
# (i for i in range(3)) generates the values 0, 1, 2[cite: 16].
# These are zipped with 4, 5, 6. Both iterables have length 3.
# Correct Answer: 0 4 \n 1 5 \n 2 6 (Formatted in the options as 04 15 26)

# =====================================================================

print("\n--- Question 3 ---")
# Question: What would be printed out in the console?
# print(2+2/2)

print("Output:")
print(2+2/2)

print("\nExplanation:")
# Python follows standard mathematical order of operations (PEMDAS/BODMAS).
# Division (2/2) happens before addition (2+1).
# In Python 3, the standard division operator `/` always returns a float[cite: 16].
# Therefore, 2 / 2 evaluates to 1.0, and 2 + 1.0 evaluates to 3.0.
# Correct Answer: 3.0

# =====================================================================

print("\n--- Question 4 ---")
# Question: What would be printed out in the console?
# The image had typos in the call: print(sum_those_numbers( 1, 2-2, =2))
# Assuming standard arguments meant to be passed: (1, 2, 2)

def babys_first_deco(func):
    def wrapper(*args):
        print(args)
        return func(*args)
    return wrapper

@babys_first_deco
def sum_those_numbers(num1, num2, num3):
    return sum((num1, num2, num3))

print("Output:")
try:
    print(sum_those_numbers(1, 2, 2))
except SyntaxError as e:
    print(f"Original code syntax error: {e}")

print("\nExplanation:")
# 1. When `sum_those_numbers(1, 2, 2)` is called, the execution actually goes to `wrapper` because of the decorator[cite: 16].
# 2. `wrapper` receives positional arguments in the `args` tuple: `(1, 2, 2)`[cite: 16].
# 3. It executes `print(args)`, which outputs `(1, 2, 2)` to the console.
# 4. It then calls the original function `func(*args)`, which evaluates `sum((1, 2, 2))` returning 5.
# 5. `wrapper` returns 5, and the outer `print()` displays it.
# Correct Answer: (1, 2, 2) \n 5

# =====================================================================

print("\n--- Question 5 ---")
# Question: What would be printed out in the console?
# The image shows: print(sum_those_numbers (num1: 1, num2: 2, num3: 2))
# Note: 'num1: 1' is invalid syntax for function calls in Python. It should be 'num1=1'.

print("Output (Simulating the call with keyword arguments):")
try:
    # Using proper keyword argument syntax because `num1: 1` raises a SyntaxError before execution
    print(sum_those_numbers(num1=1, num2=2, num3=2))
except TypeError as e:
    print(f"TypeError caught: {e}")

print("\nExplanation:")
# Even if we correct the syntax error (`:` to `=`), the call uses keyword arguments.
# However, the decorator's `wrapper` function is defined as `def wrapper(*args):`[cite: 16].
# `*args` only captures positional arguments. To capture keyword arguments, it would need `**kwargs`.
# Passing keyword arguments to a function that only accepts positional arguments raises a TypeError.
# Correct Answer: TypeError (or 'That will not compile' depending on how strict the test is about the colon syntax)

# =====================================================================

print("\n--- Question 6 ---")
# Question: What would be printed out in the console?
# print(bool(22))

print("Output:")
print(bool(22))

print("\nExplanation:")
# In Python, the bool() built-in function converts a value to a boolean.
# Most objects evaluate to True. For numeric types, any non-zero value evaluates to True[cite: 16].
# Only zero (0, 0.0, etc.) evaluates to False.
# Correct Answer: True

# =====================================================================

print("\n--- Question 7 ---")
# Question: What would be printed out in the console?
# The original code: print('bool(22)' * 2 + bool(0.5))

print("Output:")
try:
    # This simulates the evaluation
    print(f'{bool(22)} + {2 + bool(0.5)}')
except TypeError as e:
    print(f"TypeError caught: {e}")

print("\nExplanation:")
# 1. `'bool(22)' * 2` creates a new string repeated twice: `'bool(22)bool(22)'`[cite: 16].
# 2. `bool(0.5)` evaluates to `True` (a boolean, which is a subclass of integer in Python)[cite: 16].
# 3. The code then attempts to add a string and a boolean: `'bool(22)bool(22)' + True`.
# Python does not allow implicit concatenation of strings and integers/booleans. It raises a TypeError.
# The provided options in the image are confusing, but none represent successful execution.
# Based on the options provided in the image, it seems there's a misunderstanding in the question itself.

# =====================================================================

print("\n--- Question 8 ---")
# Question: What would be printed out in the console?

def babys_first_deco2(func):
    gen = (i for i in range(10)) # Generator expression
    def wrapper(*args):
        return next(gen), func(*args)
    return wrapper

@babys_first_deco2
def sum_those_numbers2(num1=0, num2=0, num3=0):
    return sum((num1, num2, num3))

print("Output:")
print(sum_those_numbers2(1, 2))
print(sum_those_numbers2(1, 3))
print(sum_those_numbers2(1, 4))
print(sum_those_numbers2(1, 5))

print("\nExplanation:")
# 1. The decorator creates a single generator object `gen` when the function is decorated[cite: 16].
# 2. This generator produces numbers 0 through 9 lazily.
# 3. Every time the decorated function is called, `wrapper` executes `next(gen)` to get the next number from the sequence[cite: 16].
# 4. It returns a tuple: `(next_number_from_gen, result_of_sum)`.
# First call (1, 2): next(gen) is 0, sum is 3. Returns (0, 3)
# Second call (1, 3): next(gen) is 1, sum is 4. Returns (1, 4)
# Third call (1, 4): next(gen) is 2, sum is 5. Returns (2, 5)
# Fourth call (1, 5): next(gen) is 3, sum is 6. Returns (3, 6)
# Correct Answer: (0, 3) \n (1, 4) \n (2, 5) \n (3, 6)

# =====================================================================

print("\n--- Question 9 ---")
# Question: What would be printed out in the console?
# print({1: 'Ala', 1: 'Ma', 1: 'Kota'})

print("Output:")
print({1: 'Ala', 1: 'Ma', 1: 'Kota'})

print("\nExplanation:")
# This syntax creates a dictionary.
# Dictionary keys must be unique. If you define a dictionary literal with duplicate keys,
# Python evaluates them from left to right, and the later values overwrite the earlier ones[cite: 16].
# So, key 1 is initially 'Ala', then updated to 'Ma', and finally updated to 'Kota'.
# Correct Answer: {1: 'Kota'}

# =====================================================================

print("\n--- Question 10 ---")
# Question: What would be printed out in the console?
cart = {'Milk': 10, 'Apples': 3, 'Apple Juice': 7}

print("Output:")
print(set(cart))

print("\nExplanation:")
# When you pass a dictionary to the `set()` constructor (or iterate over it generally),
# it uses only the dictionary's keys by default[cite: 16].
# So, `set(cart)` creates a set containing 'Milk', 'Apples', and 'Apple Juice'.
# Note: Sets are unordered, so the printed order might not match the insertion order exactly,
# but it will contain those three strings.
# Correct Answer: {'Apple Juice', 'Milk', 'Apples'} (Order may vary)

# =====================================================================

print("\n--- Question 11 ---")
# Question: What would be printed out in the console?
from random import randint
n = 5

print("Output:")
print(len([randint(1,5) for i in range(1, 11) if n]))

print("\nExplanation:")
# Let's break down the list comprehension: `[randint(1,5) for i in range(1, 11) if n]`
# 1. `range(1, 11)` produces 10 elements (from 1 up to 10)[cite: 16].
# 2. `if n` is the filter condition. In Python, an integer is truthy if it is not zero[cite: 16].
# Since n is 5, `if n` evaluates to True for every iteration.
# 3. Because the condition is always True, the loop runs exactly 10 times, generating 10 random integers.
# 4. `len()` returns the number of elements in the resulting list, which is exactly 10.
# Correct Answer: 10

# =====================================================================

print("\n--- Question 12 ---")
# Question: What would be printed out in the console?

print("Output:")
print([chr(i) for i in range(65, 91)][::-2][:3])
print([chr(i) for i in range(65, 91)][::-2][3])

print("\nExplanation:")
# 1. `[chr(i) for i in range(65, 91)]` creates a list of uppercase alphabet characters: ['A', 'B', 'C', ..., 'Z'][cite: 16].
# 2. `[::-2]` slices the list. It starts from the end and steps backward by 2[cite: 16].
#    This produces: ['Z', 'X', 'V', 'T', 'R', 'P', 'N', 'L', 'J', 'H', 'F', 'D', 'B'].
# 3. `[3]` accesses the element at index 3 of this new list[cite: 16].
#    Index 0: 'Z', Index 1: 'X', Index 2: 'V', Index 3: 'T'.
# Output: 'T'
# The options in the image show the intermediate list `['Z', 'X', 'V', ...]`, suggesting the question intended to ask for the result of `[::-2]`.

# =====================================================================

print("\n--- Question 13 ---")
# Question: What would be printed out in the console?

print("Output:")
print((i for i in range(1, 10, 2)))

print("\nExplanation:")
# The expression `(i for i in range(1, 10, 2))` uses parentheses instead of square brackets `[]`.
# In Python, this syntax creates a *generator expression*, not a tuple[cite: 16].
# To create a tuple from a comprehension, you must use `tuple(...)`.
# Printing a generator object directly outputs its memory address representation.
# Correct Answer: <generator object <genexpr> at ...>

# =====================================================================

print("\n--- Question 14 ---")
# Question: What would be printed out in the console?
from functools import wraps

def deco(function):
    @wraps(function)
    def wrapper(*args, **kwargs):
        return function(*args, **kwargs)
    return wrapper

@deco
def multiply(*args):
    res = 1
    for arg in args:
        res *= arg
    return res

print("Output:")
print(multiply)

print("\nExplanation:")
# Without `@wraps`, decorating a function replaces the original function object with the `wrapper` function object.
# Printing it would show `<function deco.<locals>.wrapper at ...>`.
# The `@wraps` decorator copies metadata (like `__name__` and `__doc__`) from the original function to the wrapper function[cite: 16].
# Because `@wraps(function)` is used, the wrapper retains the name 'multiply'.
# Correct Answer: <function multiply at ...>

# =====================================================================

print("\n--- Question 15 ---")
# Question: What would be printed out in the console?

print("Output:")
hi_message = ('hello', 'cruel', ['world'])
hi_message[2].append('!')
print(hi_message)

print("\nExplanation:")
# `hi_message` is a tuple. Tuples are immutable; you cannot reassign its elements (e.g., `hi_message[0] = 'hi'` is invalid)[cite: 16].
# However, the third element (`hi_message[2]`) is a list, which is a mutable object.
# While you cannot change the tuple to point to a *different* list, you can mutate the list object itself[cite: 16].
# `append('!')` modifies the list in-place.
# Correct Answer: ('hello', 'cruel', ['world', '!'])

# =====================================================================

print("\n--- Question 16 ---")
# Question: What would be printed out in the console?

def total1(args):
    nums1 = args
    def total2(args):
        nums2 = args
        def total3(args):
            nums3 = args
            return sum((nums1, nums2, nums3))
        return total3
    return total2

print("Output:")
print(total1(10)(20)(30))

print("\nExplanation:")
# This demonstrates closures (currying).
# 1. `total1(10)` returns the `total2` function object, maintaining state where `nums1` = 10[cite: 16].
# 2. Calling that returned function with 20: `total2(20)` returns the `total3` function object, with `nums1`=10 and `nums2`=20.
# 3. Calling that returned function with 30: `total3(30)` sets `nums3`=30 and executes `sum((10, 20, 30))`[cite: 16].
# Correct Answer: 60

# =====================================================================

print("\n--- Question 17 ---")
# Question: What would be the amount of elements in the original list?

print("Output:")
original_lst = [1, 2, 3]
new_lst = original_lst
new_lst.append(4)
print(len(original_lst))

print("\nExplanation:")
# In Python, assignment `new_lst = original_lst` does NOT create a copy of the list.
# It creates a new reference pointing to the exact same list object in memory[cite: 16].
# When you mutate the list via `new_lst.append(4)`, you are modifying that shared object.
# Therefore, `original_lst` also reflects the change and contains 4 elements.
# The options provided in the image (0, 1, 3) are incorrect. The actual answer is 4.

# =====================================================================

print("\n--- Question 18 ---")
# Question: How is the object iterable?
# Explanation:
# For an object to be considered iterable in Python, its class must implement the `__iter__()` method[cite: 16].
# This method must return an iterator object.
# While lists and tuples are built-in iterables, custom classes can become iterable by fulfilling this requirement. They do not need to contain internal tuples or be lists.
# Correct Answer: It's class has to implement the __iter__() method.

# =====================================================================

print("\n--- Question 19 ---")
# Question: Select only true statements

def nums_range(start=0, end=10, step=1):
    while start <= end:
        yield start
        start += step

print("Output/Explanation:")
print("type(nums_range):", type(nums_range))
print("type(nums_range()):", type(nums_range()))

# Explanation:
# Because the function contains the `yield` keyword, it is a generator function.
# - "nums_range returns a generator object" is TRUE. Calling the function returns a generator object, it doesn't execute the loop immediately[cite: 16].
# - "nums_range returns an iterator" is TRUE. Every generator object is an iterator[cite: 16].
# - "nums_range returns an iterable" is TRUE. Every iterator is an iterable[cite: 16].
# - "nums_range returns a function" is FALSE. The definition creates a function, but executing it returns a generator object.
# - "nums_range implements iter interface" is partially tricky. The *generator object* returned by the function implements the iterator protocol. The function itself does not.