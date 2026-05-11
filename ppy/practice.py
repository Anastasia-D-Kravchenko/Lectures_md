# Task 1. Write a function is_palindrome(s: str) -> bool that returns True if s is a palindrome using slicing.
from itertools import islice

is_palindrome = lambda s: s == s[::-1]
print(is_palindrome("racecar"))

# Task 2. Write a function flatten(lst) that takes a list of lists and returns a flat list using a list comprehension.
def flatten(lst):
    return [a for b  in lst for a in b]
print(flatten([[1,2,3], [4,5,6]]))

# Task 3. Using filter and map, write code that:
all = map(lambda x: x*x, filter(lambda x: x%2==0, range(10)))
print(list(all))

# # Task 4. Write a decorator timer that prints how long (in seconds) a function takes to execute.
# import time
# def timer(func):
#     def wrapper(*args, **kwargs):
#         start = time.time()
#         result = func(*args, **kwargs)
#         end = time.time()
#         print(f"Function {func.__name__} took {end - start} seconds to execute.")
#         return result
#     return wrapper
# @timer
# def slow_function():
#     time.sleep(2)
#     return "Done!"
# slow_function()
#
# class timer:
#     def __init__(self):
#         self.__name__ = "timer"
#     def __enter__(self):
#         self.start = time.time()
#     def __exit__(self, a, b, c):
#         print(f"Function {self.__name__} took {time.time() - self.start} seconds to execute.")
# def slow_function():
#     time.sleep(2)
#     return "Done!"
# with timer():
#     slow_function()

# Task 5. Write a closure make_multiplier(n) that returns a function which multiplies its argument by n.
def make_multiplier(n):
    return lambda x: x*n
double = make_multiplier(2)
triple = make_multiplier(3)
print(double(5))
print(triple(5))

a = [1, 2, 3]
b = a.copy()
b[0] = 99
print(a)
print(b)

# Task 7. Write a generator function fibonacci() that yields Fibonacci numbers indefinitely.
def fibonacci():
    a, b = 0, 1
    while True:
        yield a
        a, b = b, a + b
print(list(islice(fibonacci(), 10)))

# Task 8. Using reduce, compute the product of all elements in [1, 2, 3, 4, 5].
from functools import reduce, singledispatch

print(reduce(lambda x, y: x*y, [1, 2, 3, 4, 5]))

def simple_generator():
    yield "Step 1"
    yield "Step 2"
    return "Finished!"  # This signals the end

gen = simple_generator()

# 1. Normal yields
print(next(gen))  # Output: 'Step 1'
print(next(gen))  # Output: 'Step 2'

# 2. The Return Statement
try:
    print(next(gen))
except StopIteration as e:
    # The return value "Finished!" is stored in the exception's 'value' attribute
    print(f"Generator stopped. Return value was: {e.value}")

gen = (x for x in range(10))
print(next(gen))
print(next(gen))

s = (i for i in range(10))
print(s.__next__())
print(s.__next__())

# Task 3. Using Counter, count the frequency of each word in a string.
from collections import Counter
text = "The quick brown fox jumps over the lazy dog"
counts = Counter()
for i in text.split():
    counts += Counter(i)
print(counts)

# Task 4. Write a class BankAccount with:
# __init__(self, owner, balance=0)
# deposit(amount) — adds to balance
# withdraw(amount) — raises ValueError if insufficient funds
# balance as a read-only property
class BankAccount:
    def __init__(self, owner, balance=0):
        self.owner = owner
        self._balance = balance
    def deposite(self, amount):
        self._balance += amount
    def withdraw(self, amount):
        if amount > self._balance:
            raise ValueError("Insufficient funds")
        self._balance -= amount
    @property
    def balance(self):
        return self._balance
    def __str__(self):
        return f"BankAccount({self.owner}, balance={self._balance})"
    def __repr__(self):
        return str(self)
acc = BankAccount("Alice", 1000)
print(acc.balance)
acc.deposite(500)
print(acc.balance)
acc.withdraw(200)
print(acc.balance)
print(acc)

# Task 5. Write a class Vector2D that supports +, -, * (scalar), and == using magic methods.
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
        return isinstance(other, Vector2D) and self.x == other.x and self.y == other.y
    def __repr__(self):
        return f"Vector2D({self.x}, {self.y})"
vec = Vector2D(1, 2)
print(Vector2D(3, 4) == vec)

# Task 8. Using defaultdict, group a list of words by their first letter.
from collections import defaultdict
words = ["apple", "banana", "cherry", "date", "elderberry"]
my_list = defaultdict(list)
for word in words:
    my_list[word[0]].append(word)
print(my_list)

class MyLibrary:
    def __init__(self, books):
        self._books = books

    def __getitem__(self, index):
        # This allows indexing: library[0]
        print(f"--- Fetching item at index {index} ---")
        return self._books[index]

# 1. Create the object
library = MyLibrary(["Python Basics", "Deep Learning", "Generative AI"])

# 2. Testing Indexing
print(f"Book 1: {library[0]}")

print("\n--- Starting Loop ---")

# 3. Testing Iterability
# Even though we didn't define __iter__, Python will keep
# calling __getitem__ starting at 0 until an IndexError is raised.
for book in library:
    print(f"Read: {book}")

try:
    3/0
except ZeroDivisionError:
    print("Division by zero!")
else:
    print("No division by zero!")
finally:
    print("This will always run")

@singledispatch
def process(data):
    print(f"Processing {data}...")
@process.register(str)
def _(data: str):
    print(f"Processing string: {data}")
@process.register(int)
def _(data: int):
    print(f"Processing integer: {data}")
process("hello")
process(21)
process([1, "a"])