name = "Guido"
print("id" in name)
print(f"{3.14159:.2f}") # 3.14
print(f"{1_000_000:,}") # 1,000,000
print("Hello {}!".format("World"))
print("%-10s" % "left")
word = "left"
print(f"{word:<10}")


fruits = ["apple", "banana", "cherry"]
copy1 = fruits[:]
copy2 = fruits.copy() # equivalent

original = [[1, 2], [3, 4]]
shallow = original.copy()
shallow[0][0] = 99  # Changing a nested element
print(original) # Output: [[99, 2], [3, 4]] -> BOTH changed!

import copy
deep = copy.deepcopy(fruits) # deep copy

original = [[1, 2], [3, 4]]
deep_v = copy.deepcopy(original)
deep_v[0][0] = 99
print(original) # Output: [[1, 2], [3, 4]] -> Original is SAFE!


point = (3, 7)
x, y = point # unpacking
a, *rest = (1,2,3,4) # a=1, rest=[2,3,4]
print(rest)
single = (42,)
print(single)
from collections import namedtuple
Person = namedtuple('Person', ['name', 'age'])
p = Person('Alice', 30)
print(p.name, p.age)


person = {"name": "Alice", "age": 30, "city": "Warsaw"}
# Access
person["name"] # 'Alice'
person.get("phone", "N/A") # safe access with default
print(person.get("phone", "N/A"))

person["email"] = "a@b.com"
person.update({"age": 31, "city": "Berlin"})
del person["city"]
removed = person.pop("email", None)
print(person)

for key in person:
    print(key)
for key, val in person.items():
    print(f"{key}: {val}")
person.keys() # dict_keys view
person.values() # dict_values view

square = {n: n**2 for n in range(1,10)}
just_list = {n*2 for n in 10}
print(square)
print(just_list)