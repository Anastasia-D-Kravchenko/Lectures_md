# NAI — Formula Sheet (Review Questions)

---

## Q1 — Vectors

**Euclidean norm (length):**
```
|x| = sqrt(x1² + x2² + ... + xn²)
```

**Euclidean distance:**
```
d(x, y) = sqrt( (x1-y1)² + (x2-y2)² + ... + (xn-yn)² )
```

**Dot product (scalar product):**
```
x·y = x1·y1 + x2·y2 + ... + xn·yn
```

**Perpendicular check:**
> vectors are perpendicular ↔ dot product = **0**

---

## Q2 — k-NN

Use **Euclidean distance** to all training points → pick k smallest → **majority vote**.

```
d(x, point) = sqrt( sum of (xi - pointi)² )
```

---

## Q3 — Perceptron

**Net value:**
```
net = w1·x1 + w2·x2 + ... + wn·xn − θ
```

**Step activation:**
```
y = 1   if net >= 0
y = 0   otherwise
```

**Delta rule (one update step):**
```
wi' = wi + α·(d − y)·xi       for each weight
θ'  = θ  − α·(d − y)          for threshold
```

Where: `d` = desired output, `y` = actual output, `α` = learning rate

| Error (d−y) | Meaning | Effect |
|---|---|---|
| 0 | Correct | No update |
| +1 | Should fire, didn't | W moves toward X; θ decreases |
| −1 | Fired, shouldn't | W moves away from X; θ increases |

---

## Q4 — Classifier Evaluation

**Confusion matrix:**

|  | Predicted + | Predicted − |
|---|---|---|
| **Actual +** | TP | FN |
| **Actual −** | FP | TN |

**Formulas:**
```
Accuracy  = (TP + TN) / (TP + TN + FP + FN)

Precision = TP / (TP + FP)

Recall    = TP / (TP + FN)

F-score   = 2 · Precision · Recall / (Precision + Recall)
```

---

## Q5 — k-Means

**Assign:** each point → nearest centroid using Euclidean distance.

**Update centroid** (mean of all points in cluster):
```
c_k = (1 / |C_k|) · sum of all x in cluster k
```

Repeat assign → update until nothing changes.

---

## Q6 — Naive Bayes

**Classification rule:**
```
y* = argmax over y of:  P(Y=y) · P(X1=x1|Y=y) · P(X2=x2|Y=y) · ... · P(Xn=xn|Y=y)
```

**Estimate from training data:**
```
P(Y=y)        = count(Y=y) / N

P(Xi=xi|Y=y)  = count(Xi=xi AND Y=y) / count(Y=y)
```

**Laplace smoothing** — use when any count = 0:
```
P(Xi=xi|Y=y) = (count(xi, y) + 1) / (count(y) + d)
```

Where `d` = number of distinct values of attribute Xi

---

## Q7 — Huffman Coding

**Algorithm:**
1. Count frequency of each symbol
2. Sort by frequency ascending
3. Repeatedly merge the **two lowest-frequency** nodes → parent = sum of frequencies
4. Assign `0` / `1` to left/right branches
5. Code for symbol = path from root to that leaf

**Total encoded length in bits:**
```
L = sum of ( freq(symbol) × code_length(symbol) )
```

---

## Q8 — Regression Tree (RSS)

**RSS for one region:**
```
RSS(R) = sum of (yi − mean_y_R)²    for all points i in region R
```

Where `mean_y_R` = mean of y-values in that region.

**Find best split:** try every threshold on every feature, pick the split minimising:
```
RSS(left region) + RSS(right region)
```

---

## Q9 — Greedy Knapsack

**Density (value-to-weight ratio):**
```
density_i = v_i / w_i
```

**Algorithm:**
1. Compute density for each item
2. Sort by density descending (highest first)
3. Add items in order while total weight ≤ C
4. Skip any item that doesn't fit (don't break — keep checking remaining items)

---

## Quick Recap

| Topic | Key formula |
|---|---|
| Norm | `sqrt(sum xi²)` |
| Euclidean distance | `sqrt(sum (xi−yi)²)` |
| Dot product | `sum xi·yi` (= 0 → perpendicular) |
| Perceptron net | `sum wi·xi − θ` |
| Delta rule | `wi' = wi + α(d−y)xi` ; `θ' = θ − α(d−y)` |
| Accuracy | `(TP+TN) / total` |
| Precision | `TP / (TP+FP)` |
| Recall | `TP / (TP+FN)` |
| F-score | `2·P·R / (P+R)` |
| k-Means centroid | mean of all points in cluster |
| Naive Bayes | `P(y) · Π P(xi|y)` |
| Laplace smoothing | `(count+1) / (total+d)` |
| RSS | `sum (yi − mean_y)²` |
| Greedy density | `v_i / w_i` |
| Huffman length | `sum freq × code_length` |
