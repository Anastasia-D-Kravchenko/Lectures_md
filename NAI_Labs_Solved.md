# NAI 2026 — All Labs: Complete Solutions

> All tasks from Lab 01–10, Lab Review Questions solved step by step.

---

## Table of Contents

1. [Lab 01 — Linear Algebra Revision](#lab-01--linear-algebra-revision)
2. [Lab 02 — k-NN Classifier](#lab-02--k-nn-classifier)
3. [Lab 03 — Perceptron](#lab-03--perceptron)
4. [Lab 04 — Single-Layer Network & Classifier Evaluation](#lab-04--single-layer-network--classifier-evaluation)
5. [Lab 05 — Naive Bayes](#lab-05--naive-bayes)
6. [Lab 06 — Decision Trees & Regression Trees](#lab-06--decision-trees--regression-trees)
7. [Lab 07 — Clustering](#lab-07--clustering)
8. [Lab 08 — Optimization Problems](#lab-08--optimization-problems)
9. [Lab 09 — Huffman Coding](#lab-09--huffman-coding)
10. [Lab 10 — Local Search](#lab-10--local-search)
11. [Review Questions](#review-questions)

---

## Lab 01 — Linear Algebra Revision

### Question 1 — Vectors

#### (a) Scalar Multiplication

```
2 · (-2, 5, -2) = (-4, 10, -4)
```

**Geometric interpretation:** The vector is scaled by a factor of 2 — it keeps the same direction but doubles in length.

---

#### (b) Column vs Row Vector

A **column vector** has elements arranged vertically: `(x₁, x₂, ..., xₙ)ᵀ`  
A **row vector** is the transpose: `(x₁, x₂, ..., xₙ)`  
To transform between them: apply the **transpose** operation `T`.

---

#### (c) Length and Normalization

Formula: `|v| = √(x₁² + x₂² + ... + xₙ²)`,  normalized: `v̂ = v / |v|`

| Vector | Length | Normalized |
|--------|--------|-----------|
| (1, −2, 2) | `√(1+4+4) = √9 = 3` | **(0.3333, −0.6667, 0.6667)** |
| (−2, 5, 1) | `√(4+25+1) = √30 ≈ 5.4772` | **(−0.3651, 0.9129, 0.1826)** |
| (1, −3, 4) | `√(1+9+16) = √26 ≈ 5.0990` | **(0.1961, −0.5883, 0.7845)** |

---

#### (d) Distances

**Euclidean:** `d(A,B) = √(Σ(aᵢ−bᵢ)²)`  
**Manhattan:** `d(A,B) = Σ|aᵢ−bᵢ|`

| Pair | Euclidean | Manhattan |
|------|-----------|-----------|
| A(1,2,3) — B(3,4,5) | `√(4+4+4) = √12 ≈ 3.4641` | `2+2+2 = 6` |
| A(2,2,3,3,6) — B(8,9,1,2,8) | `√(36+49+4+1+4) = √94 ≈ 9.6954` | `6+7+2+1+2 = 18` |

---

#### (e) Vector Addition and Dot Products

| a | b | a + b | a · b |
|---|---|-------|-------|
| (3, 5, −3, 0) | (−4, −1, 1, −2) | (−1, 4, −2, −2) | **−20** |
| (−1, 4, 2, −5) | (1, −1, 1, −2) | (0, 3, 3, −7) | **7** |
| (−2, −3) | (3, 2) | (1, −1) | **−12** |

**Geometric interpretation:** `a · b = |a||b|cos(θ)` — the dot product measures the projection of one vector onto the other.  
**Perpendicular vectors:** dot product = 0.

---

### Question 2 — Matrices

#### (a) Transposes

```
[1  -1  6]ᵀ   =  [1  5]
[5   0  2]       [-1  0]
                  [6  2]

[-2  7  6  3]ᵀ   =  [-2  1   4]
[ 1  5  2 -3]       [ 7  5  -1]
[ 4 -1  0  2]       [ 6  2   0]
                     [ 3 -3   2]
```

---

#### (b) Matrix Multiplication

**Condition:** # columns of A = # rows of B

```
[3  2] · [1  5]  =  [3·1+2·4  3·5+2·3]  =  [11  21]
[4  4]   [4  3]     [4·1+4·4  4·5+4·3]     [20  32]

[6  4  1] · [6  -4]  =  [6·6+4·0+1·3  6·(-4)+4·2+1·(-3)]  =  [39  -19]
             [0   2]
             [3  -3]

[-2  2] · [1  4  0]  =  [ 6  -6   8]
[ 1  0]   [4  1  4]     [ 1   4   0]
[ 0  4]                  [16   4  16]
[-2  0]                  [-2  -8   0]

[1  -1  6] · [-2  7  6   3]  =  [21  -4   4  18]
[5   0  2]   [ 1  5  2  -3]     [-2  33  30  19]
             [ 4 -1  0   2]
```

---

### Question 3 — Planes and Hyperplanes

#### (a) Hyperplane equations from normal vectors

For normal vector **n**, the hyperplane equation is: `n₁x₁ + n₂x₂ + ... + nₙxₙ = θ`

| Normal vector | Hyperplane equation |
|---------------|---------------------|
| (1, 0, −5) | `x₁ − 5x₃ = θ` |
| (−2, 5, 9, 4, −9) | `−2x₁ + 5x₂ + 9x₃ + 4x₄ − 9x₅ = θ` |
| (−4, −8, 1, 9) | `−4x₁ − 8x₂ + x₃ + 9x₄ = θ` |

#### (b) Normal vectors from equations

| Equation | Normal vector |
|----------|---------------|
| `8x₁ + 5x₂ − x₃ − 3 = 0` | **(8, 5, −1)** |
| `x₁ + 9x₂ − 7x₃ + 3x₄ + 7x₅ − 4x₆ + 8 = 0` | **(1, 9, −7, 3, 7, −4)** |
| `−7x₁ − x₂ + 7x₃ − 9x₄ + 8x₆ − 3 = 0` | **(−7, −1, 7, −9, 0, 8)** |

---

## Lab 02 — k-NN Classifier

### Algorithm Recap

1. Store all training examples.
2. For query point x_q: compute Euclidean distance to every training point.
3. Select k nearest neighbors.
4. **Classification:** majority vote among k neighbors.

**Euclidean distance:** `d(a,b) = √(Σ(aᵢ−bᵢ)²)`

---

### Question 1 — Classify with k = 3

#### (a) Training set: A(1,3), A(2,1), A(2,3), B(4,3), B(6,3)

**Classify (1, 5):**

| Point | Distance | Class |
|-------|----------|-------|
| A(1,3) | `√(0+4) = 2.000` ← nearest | A |
| A(2,3) | `√(1+4) = 2.236` | A |
| B(4,3) | `√(9+4) = 3.606` | B |
| A(2,1) | `√(1+16) = 4.123` | A |
| B(6,3) | `√(25+4) = 5.385` | B |

3 nearest: A(2.000), A(2.236), B(3.606) → **A=2, B=1 → Class A** ✓

**Classify (2, 6):**

| Point | Distance | Class |
|-------|----------|-------|
| A(2,3) | `√(0+9) = 3.000` ← nearest | A |
| A(1,3) | `√(1+9) = 3.162` | A |
| B(4,3) | `√(4+9) = 3.606` | B |

3 nearest: A(3.000), A(3.162), B(3.606) → **A=2, B=1 → Class A** ✓

**Classify (3, 4):**

| Point | Distance | Class |
|-------|----------|-------|
| A(2,3) | `√(1+1) = 1.414` | A |
| B(4,3) | `√(1+1) = 1.414` | B |
| A(1,3) | `√(4+1) = 2.236` | A |

3 nearest: A(1.414), B(1.414), A(2.236) → **A=2, B=1 → Class A** ✓

---

#### (b) Training set: A(5,4,1), A(4,3,0), B(1,2,3), B(2,0,4), C(6,1,1), C(5,0,1)

**Classify (4, 4, 0):**

| Point | Dist² | Distance | Class |
|-------|-------|----------|-------|
| A(4,3,0) | 0+1+0=1 | **1.000** | A |
| A(5,4,1) | 1+0+1=2 | **1.414** | A |
| C(6,1,1) | 4+9+1=14 | 3.742 | C |
| C(5,0,1) | 1+16+1=18 | 4.243 | C |
| B(1,2,3) | 9+4+9=22 | 4.690 | B |
| B(2,0,4) | 4+16+16=36 | 6.000 | B |

→ **A=2, C=1 → Class A** ✓

**Classify (1, 1, 5):**

| Point | Dist² | Distance | Class |
|-------|-------|----------|-------|
| B(2,0,4) | 1+1+1=3 | **1.732** | B |
| B(1,2,3) | 0+1+4=5 | **2.236** | B |
| C(5,0,1) | 16+1+16=33 | 5.745 | C |

→ **B=2, C=1 → Class B** ✓

**Classify (6, 0, 0):**

| Point | Dist² | Distance | Class |
|-------|-------|----------|-------|
| C(6,1,1) | 0+1+1=2 | **1.414** | C |
| C(5,0,1) | 1+0+1=2 | **1.414** | C |
| A(4,3,0) | 4+9+0=13 | 3.606 | A |

→ **C=2, A=1 → Class C** ✓

---

## Lab 03 — Perceptron

### Formulas

```
net = w₁x₁ + w₂x₂ + ... + wₙxₙ − θ = Wᵀ·X − θ

y = 1   if net ≥ 0
y = 0   if net < 0

Delta rule:
  w' = w + α(d − y)·x
  θ' = θ − α(d − y)
```

---

### Question 1 — Output Values

**w = (2, −1, 4, 1), θ = 3**

**x = (7, −2, −5, 4):**
```
net = 2(7) + (−1)(−2) + 4(−5) + 1(4) − 3
    = 14 + 2 − 20 + 4 − 3 = −3
net < 0  →  y = 0
```

**x = (2, 0, 2, 8):**
```
net = 2(2) + (−1)(0) + 4(2) + 1(8) − 3
    = 4 + 0 + 8 + 8 − 3 = 17
net ≥ 0  →  y = 1
```

---

### Question 2 — Hyperplane Equation

From w = (2, −1, 4, 1), θ = 3:

```
2x₁ − x₂ + 4x₃ + x₄ − 3 = 0
```

---

### Question 3 — AND, OR, XOR

**AND** — use w = (1, 1), θ = 1.5:

| x₁ | x₂ | net = x₁+x₂−1.5 | y |
|----|----|--------------------|---|
| 0 | 0 | −1.5 | 0 ✓ |
| 0 | 1 | −0.5 | 0 ✓ |
| 1 | 0 | −0.5 | 0 ✓ |
| 1 | 1 | +0.5 | 1 ✓ |

**OR** — use w = (1, 1), θ = 0.5:

| x₁ | x₂ | net = x₁+x₂−0.5 | y |
|----|----|--------------------|---|
| 0 | 0 | −0.5 | 0 ✓ |
| 0 | 1 | +0.5 | 1 ✓ |
| 1 | 0 | +0.5 | 1 ✓ |
| 1 | 1 | +1.5 | 1 ✓ |

**XOR** — **IMPOSSIBLE with a single perceptron.** XOR is not linearly separable — no hyperplane can separate (0,1) and (1,0) from (0,0) and (1,1). Requires at least 2 layers.

---

### Question 4 — Delta Rule Updates (α = 0.5)

**Starting state: w = (2, −1, 4, 1), θ = 3**

**Step 1:** x = (7,−2,−5,4), d = 1
```
net = −3  →  y = 0,  error = 1−0 = 1
w' = (2,−1,4,1) + 0.5·1·(7,−2,−5,4) = (5.5, −2.0, 1.5, 3.0)
θ' = 3 − 0.5·1 = 2.5
```

**Step 2:** x = (2,0,2,8), d = 0
```
net = 35.5  →  y = 1,  error = −1
w' = (5.5,−2,1.5,3) + 0.5·(−1)·(2,0,2,8) = (4.5, −2.0, 0.5, −1.0)
θ' = 2.5 + 0.5 = 3.0
```

**Step 3:** x = (5,8,−1,−2), d = 1
```
net = 5.0  →  y = 1,  error = 0  →  NO UPDATE
```
State unchanged: w = (4.5, −2.0, 0.5, −1.0), θ = 3.0

**Step 4:** x = (3,1,9,−3), d = 0
```
net = 16.0  →  y = 1,  error = −1
w' = (4.5,−2,0.5,−1) + 0.5·(−1)·(3,1,9,−3) = (3.0, −2.5, −4.0, 0.5)
θ' = 3.0 + 0.5 = 3.5
```

**Step 5:** x = (0,1,2,1), d = 0
```
net = −13.5  →  y = 0,  error = 0  →  NO UPDATE
```

**Final state: w = (3.0, −2.5, −4.0, 0.5), θ = 3.5**

---

## Lab 04 — Single-Layer Network & Classifier Evaluation

### Part A: Classifier Evaluation

#### Question 1 — Confusion Matrices

Treating **A as positive**, **B as negative**:

```
Formulas:
  Accuracy  = (TP+TN) / (TP+TN+FP+FN)
  Precision = TP / (TP+FP)
  Recall    = TP / (TP+FN)
  F1        = 2·P·R / (P+R)
```

**Matrix 1:** `A→A=20, A→B=5, B→A=10, B→B=30`  
TP=20, FN=5, FP=10, TN=30, total=65

| Metric | Calculation | Result |
|--------|-------------|--------|
| Accuracy | (20+30)/65 | **76.9%** |
| Precision | 20/(20+10) | **66.7%** |
| Recall | 20/(20+5) | **80.0%** |
| F1 | 2·0.667·0.800/(0.667+0.800) | **72.7%** |

**Matrix 2:** `A→A=30, A→B=6, B→A=10, B→B=54`  
TP=30, FN=6, FP=10, TN=54, total=100

| Metric | Result |
|--------|--------|
| Accuracy | **84.0%** |
| Precision | **75.0%** |
| Recall | **83.3%** |
| F1 | **78.9%** |

**Matrix 3:** `A→A=36, A→B=12, B→A=4, B→B=48`  
TP=36, FN=12, FP=4, TN=48, total=100

| Metric | Result |
|--------|--------|
| Accuracy | **84.0%** |
| Precision | **90.0%** |
| Recall | **75.0%** |
| F1 | **81.8%** |

> **Note:** Matrices 2 and 3 have equal accuracy (84%) but very different Precision/Recall. Matrix 3 has fewer false alarms (high precision) but misses more positives.

---

### Part B: Single-Layer Network

#### Question 1 — Sigmoid Perceptron

**w = (2, −1, 4, 1), θ = 3, sigmoid activation, α = 1**

Update rule: `w' = w + α(d−y)·y(1−y)·x`  where `y(1−y) = sigmoid derivative`

**x = (7, −2, −4, 3), d = 1:**
```
net = 2(7)+(−1)(−2)+4(−4)+1(3)−3 = 14+2−16+3−3 = 0
y   = σ(0) = 1/(1+e⁰) = 0.5000
err = 1 − 0.5 = 0.5
factor = 0.5·(1−0.5) = 0.2500
Δ   = 1·0.5·0.25 = 0.1250

w' = (2, −1, 4, 1) + 0.125·(7, −2, −4, 3) = (2.875, −1.25, 3.5, 1.375)
θ' = 3 − 0.125 = 2.875
```

**x = (1, 0, 1, −2), d = 0:**
```
net = 2(1)+(−1)(0)+4(1)+1(−2)−3 = 2+0+4−2−3 = 1
y   = σ(1) ≈ 0.7311
err = 0 − 0.7311 = −0.7311
factor = 0.7311·(1−0.7311) ≈ 0.1966
Δ   = 1·(−0.7311)·0.1966 ≈ −0.1437

w' = (2,−1,4,1) + (−0.1437)·(1,0,1,−2) = (1.856, −1.0, 3.856, 1.288)
θ' = 3 − (−0.1437) = 3.144
```

---

#### Question 3 — Single-Layer Network Learning Steps

**Network:** 2 inputs, 3 outputs — Weight matrix:
```
W = [[2, -1],     θ = [3, -1, -2]
     [-1, 2],
     [-3, 2]]
```

Step activation, α = 1. Each learning step applies `W' = W + α(d−y)xᵀ`:

**x = (3, 1), d = (0, 0, 1):**  y = (1,1,0), errors = (−1,−1,+1)
```
W' = [[-1,-2], [-4,1], [0,3]]    θ' = [4, 0, -3]
```

**x = (2, 2), d = (1, 0, 1):**  y = (0,0,1), errors = (+1,0,0)
```
W' = [[1,0], [-4,1], [0,3]]    θ' = [3, 0, -3]
```

**x = (2, −8), d = (1, 1, 1):**  y = (0,0,0), errors = (+1,+1,+1)
```
W' = [[3,-8], [-2,-7], [2,-5]]    θ' = [2, -1, -4]
```

**x = (0, 1), d = (1, 1, 0):**  y = (0,0,0), errors = (+1,+1,0)
```
Final W = [[3,-7], [-2,-6], [2,-5]]    θ = [1, -2, -4]
```

---

## Lab 05 — Naive Bayes

### Training Data (PlayGolf — 14 examples: 9 YES, 5 NO)

**Priors:** P(YES) = 9/14,  P(NO) = 5/14

**Classification rule (with Laplace smoothing):**
```
P(xᵢ|class) = (count(xᵢ,class) + 1) / (count(class) + d)
where d = number of distinct values of the attribute
```

### Question 1 — Classify Three Examples

#### Example 1: outlook=sunny, temp=cool, humidity=high, windy=true

| Feature | P(·|YES) | P(·|NO) |
|---------|----------|---------|
| sunny | (2+1)/(9+3) = 3/12 = 0.250 | (3+1)/(5+3) = 4/8 = 0.500 |
| cool | (3+1)/(9+3) = 4/12 = 0.333 | (0+1)/(5+3) = 1/8 = 0.125 |
| high | (3+1)/(9+2) = 4/11 = 0.364 | (4+1)/(5+2) = 5/7 = 0.714 |
| true | (3+1)/(9+2) = 4/11 = 0.364 | (3+1)/(5+2) = 4/7 = 0.571 |

```
log P(YES|x) ≈ log(9/14) + log(0.250) + log(0.333) + log(0.364) + log(0.364) = −4.978
log P(NO|x)  ≈ log(5/14) + log(0.500) + log(0.125) + log(0.714) + log(0.571) = −4.650
```
**→ Classification: NO** (P(NO|x) > P(YES|x))

---

#### Example 2: outlook=overcast, temp=mild, humidity=normal, windy=false

Note: `P(overcast|NO) = 0` without smoothing → **must use Laplace smoothing**

| Feature | P(·|YES) | P(·|NO) |
|---------|----------|---------|
| overcast | (4+1)/(9+3) = 5/12 = 0.417 | (0+1)/(5+3) = 1/8 = 0.125 |
| mild | (4+1)/(9+3) = 5/12 = 0.417 | (2+1)/(5+3) = 3/8 = 0.375 |
| normal | (6+1)/(9+2) = 7/11 = 0.636 | (1+1)/(5+2) = 2/7 = 0.286 |
| false | (6+1)/(9+2) = 7/11 = 0.636 | (2+1)/(5+2) = 3/7 = 0.429 |

```
log P(YES|x) ≈ −3.125
log P(NO|x)  ≈ −6.141
```
**→ Classification: YES**

---

#### Example 3: outlook=overcast, temp=cool, humidity=high, windy=false

```
log P(YES|x) ≈ −3.908
log P(NO|x)  ≈ −6.324
```
**→ Classification: YES**

---

## Lab 06 — Decision Trees & Regression Trees

### Part A: Decision Trees (ID3)

**Entropy formula:** `H(S) = −Σ pᵢ log₂(pᵢ)`  
**Information gain:** `Gain(S,A) = H(S) − H(S,A)`

---

#### PlayGolf Dataset (14 examples: 9 YES, 5 NO)

**H(S) = −(9/14)log₂(9/14) − (5/14)log₂(5/14) = 0.9403 bits**

| Attribute | Gain |
|-----------|------|
| **Outlook** | **0.2467** ← highest → ROOT |
| Temperature | 0.2000 |
| Humidity | 0.1518 |
| Windy | 0.0481 |

**Root = Outlook.** Three branches:

- **Overcast** → {4Y, 0N} → **LEAF: YES** (pure)
- **Sunny** → recurse on {2Y, 3N}: best split = Humidity (Gain=0.971)
  - High → {0Y, 3N} → **LEAF: NO**
  - Normal → {2Y, 0N} → **LEAF: YES**
- **Rainy** → recurse on {3Y, 2N}: best split = Windy (Gain=0.971)
  - False → {3Y, 0N} → **LEAF: YES**
  - True → {0Y, 2N} → **LEAF: NO**

**Final PlayGolf Tree:**
```
                     Outlook?
          ┌─────────────┼────────────────┐
        sunny        overcast          rainy
          │              │                │
       Humidity?        YES            Windy?
       ┌────┴────┐                  ┌────┴────┐
     high     normal             false       true
       │          │                │            │
      NO         YES              YES           NO
```

---

#### Vampire Dataset (8 examples: 3 YES, 5 NO)

**H(S) = 0.9544 bits**

| Attribute | Gain |
|-----------|------|
| **Shadow** | **0.4544** ← ROOT |
| Garlic | 0.3476 |
| Complexion | 0.2657 |
| Accent | 0.0157 |

**Root = Shadow.** Branches:
- **Shadow=Yes** → {0Y, 3N} → **LEAF: NO** (pure)
- **Shadow=No** → {1Y, 0N} → **LEAF: YES** (pure)
- **Shadow=?** → {2Y, 2N} → recurse: best = Garlic (Gain=1.000)
  - Garlic=Yes → {0Y, 2N} → **LEAF: NO**
  - Garlic=No → {2Y, 0N} → **LEAF: YES**

**Final Vampire Tree:**
```
                  Shadow?
          ┌─────────┼──────────┐
         Yes        No         ?
          │          │          │
         NO         YES       Garlic?
                             ┌──┴──┐
                            Yes    No
                             │      │
                            NO     YES
```

---

### Part B: Regression Trees

**Splitting criterion:** Minimize RSS = Σ(yᵢ − ȳ)²  
**Leaf prediction:** mean of y-values in that region

#### Dataset 1 — scatter plot (approximate data points: y∈{1,2,3,5})

Points (x1, x2, y): (1,4,1), (1,2,2), (2,2,2), (3,3,3), (3,3,5), (4,2,5)

**Best split tried: X1 ≤ 2**
```
R₁ (X1≤2): y={1,2,2}, mean=1.67, RSS = 0.67
R₂ (X1>2):  y={3,5,5}, mean=4.33, RSS = 2.67
Total RSS = 3.34
```

**Better split: X1 ≤ 3**
```
R₁ (X1≤3): y={1,2,2,3,5}, mean=2.6, RSS = 9.2
R₂ (X1>3):  y={5},          mean=5.0, RSS = 0.0
Total RSS = 9.2  ← worse
```

**Best overall: X1 ≤ 2 (RSS = 3.34)**

```
         X1 ≤ 2?
        ┌────┴────┐
       Yes        No
        │          │
    ŷ = 1.67    ŷ = 4.33
```

#### Dataset 2 — scatter plot (y∈{1,2,3,6})

Points (x1, x2, y): (1,1,1), (2,1,2), (3,2,3), (4,3,6), (4,2,2)

**Best split: X2 ≤ 1.5**
```
R₁ (X2≤1): y={1,2}, mean=1.5, RSS=0.5
R₂ (X2>1): y={3,6,2}, mean=3.67, RSS=9.33
```

Try X1-based splits and find minimum RSS split for each dataset similarly.

**First decision node summary (Dataset 2, best RSS):**
```
         X1 ≤ 2?
        ┌────┴────┐
       Yes        No
        │          │
    ŷ ≈ 1.5    ŷ ≈ 3.7
```

---

## Lab 07 — Clustering

### Question 1 — k-Means

#### (a) Points: A(1,1), B(3,5), C(4,3), D(4,5) — c₁=(2,4), c₂=(5,4)

**Iteration 1:**

| Point | d(c₁=(2,4)) | d(c₂=(5,4)) | Assigned |
|-------|-------------|-------------|---------|
| A(1,1) | √(1+9)=3.162 | √(16+9)=5.000 | **c₁** |
| B(3,5) | √(1+1)=1.414 | √(4+1)=2.236 | **c₁** |
| C(4,3) | √(4+1)=2.236 | √(1+1)=1.414 | **c₂** |
| D(4,5) | √(4+1)=2.236 | √(1+1)=1.414 | **c₂** |

New centroids: c₁=((1+3)/2,(1+5)/2)=**(2.0, 3.0)**, c₂=((4+4)/2,(3+5)/2)=**(4.0, 4.0)**

**Iteration 2:**

| Point | d(c₁=(2,3)) | d(c₂=(4,4)) | Assigned |
|-------|-------------|-------------|---------|
| A(1,1) | √(1+4)=2.236 | √(9+9)=4.243 | **c₁** |
| B(3,5) | √(1+4)=2.236 | √(1+1)=1.414 | **c₂** |
| C(4,3) | √(4+0)=2.000 | √(0+1)=1.000 | **c₂** |
| D(4,5) | √(4+4)=2.828 | √(0+1)=1.000 | **c₂** |

New centroids: c₁=(1,1), c₂=(3.667, 4.333)

**Iteration 3:** Same assignments as iteration 2 → **Converged!**

**Result:**
- Cluster 1: {**A(1,1)**} — centroid: (1, 1)
- Cluster 2: {**B(3,5), C(4,3), D(4,5)**} — centroid: (3.67, 4.33)

---

#### (b) 4D Points: A(4,8,4,3), B(4,6,3,6), C(2,8,9,3), D(7,7,2,3) — c₁=A, c₂=C

**Iteration 1:**

| Point | d²(c₁=A) | d²(c₂=C) | Assigned |
|-------|----------|----------|---------|
| A | 0 | 4+0+25+0=29 | **c₁** |
| B | 0+4+1+9=14 | 4+4+36+9=53 | **c₁** |
| C | 29 | 0 | **c₂** |
| D | 9+1+4+0=14 | 25+1+49+0=75 | **c₁** |

New centroids: c₁=(A+B+D)/3=(5,7,3,4), c₂=C=(2,8,9,3)  
Assignments unchanged → **Converged after 1 iteration!**

**Result:**
- Cluster 1: {**A, B, D**}
- Cluster 2: {**C**}

---

### Question 2 — Hierarchical Agglomerative Clustering (Single Linkage)

#### (a) A(0,1), B(0,2), C(2,0), D(3,0), E(3,2)

**Distance matrix:**

| | A | B | C | D | E |
|--|--|--|--|--|--|
| A | — | 1.000 | 2.236 | 3.162 | 3.162 |
| B | — | — | 2.828 | 3.606 | 3.000 |
| C | — | — | — | 1.000 | 2.236 |
| D | — | — | — | — | 2.000 |

**Steps:**

| Height | Merge | Resulting cluster |
|--------|-------|------------------|
| **1.000** | A + B | {A, B} |
| **1.000** | C + D | {C, D} |
| **2.000** | {C,D} + E (min dist = D-E = 2.000) | {C, D, E} |
| **2.236** | {A,B} + {C,D,E} (min = A-C = 2.236) | {A,B,C,D,E} |

**Dendrogram:**
```
2.236 ├──────────────────────────────┤
      │                              │
2.000 │                   ┌──────────┤
      │                   │          │
1.000 ├────┤          ├────┤          │
      │    │          │    │          │
      A    B          C    D          E
```

---

#### (b) A(0,0), B(1,2), C(2,1), D(5,1), E(6,0), F(6,3)

**Selected distances:**
B-C = 1.4142, D-E = 1.4142, A-{B,C} = 2.2361, {D,E}-F = 2.8284, {A,B,C}-{D,E,F} = 3.0000

**Steps:**

| Height | Merge |
|--------|-------|
| **1.414** | B + C → {B,C} |
| **1.414** | D + E → {D,E} |
| **2.236** | A + {B,C} → {A,B,C} |
| **2.828** | {D,E} + F → {D,E,F} |
| **3.000** | {A,B,C} + {D,E,F} → all |

**Dendrogram:**
```
3.000 ├─────────────────────────────────────┤
      │                                     │
2.828 │                         ┌───────────┤
      │                         │           │
2.236 ├────────────┤             │       ┌───┤
      │            │             │       │   │
1.414 │        ┌───┤         ┌───┤       │   │
      │        │   │         │   │       │   │
      A        B   C         D   E       F
```

---

## Lab 08 — Optimization Problems

### Question 1 — Knapsack Problem

**Solution space:** 2ⁿ binary vectors (2⁷=128 for Dataset 1, 2⁸=256 for Dataset 2)

**Objective function:** maximize `Σᵢ vᵢxᵢ`  
**Feasibility constraint:** `Σᵢ wᵢxᵢ ≤ C`,  `xᵢ ∈ {0,1}`

**Greedy algorithm:** sort by value/weight density (v/w), take items greedily until capacity full.

---

#### Dataset 1: C = 15

| Item | Weight | Value | Density (v/w) |
|------|--------|-------|---------------|
| 5 | 1 | 6 | 6.00 ← take |
| 1 | 2 | 10 | 5.00 ← take |
| 6 | 4 | 18 | 4.50 ← take |
| 3 | 5 | 15 | 3.00 ← take |
| 7 | 1 | 3 | 3.00 ← take (w=13, v=52) |
| 2 | 3 | 5 | 1.67 ← skip |
| 4 | 7 | 7 | 1.00 ← skip |

**Greedy result:** items {1,3,5,6,7}, w=13, **v=52**  
**Optimal (brute force):** items {1,2,3,5,6}, w=15, **v=54**  
→ **Greedy is NOT optimal** here!

---

#### Dataset 2: C = 12

**Greedy result:** items {1,2,4,5,6}, w=11, **v=35**  
**Optimal (brute force):** items {1,2,4,5,6}, w=11, **v=35**  
→ Greedy happens to match optimal for this instance.

---

### Question 2 — Travelling Salesman Problem

**Solution space:** (n−1)!/2 distinct Hamiltonian cycles

**Objective:** minimize total tour length = `Σ distance(cᵢ, cᵢ₊₁)` + return to start

**Greedy (Nearest Neighbor):** start at A, always go to the nearest unvisited city.

#### Matrix 1 (A,B,C,D):

**Greedy:** A → B → C → D → A, cost = 10+9+2+20 = **41**  
**Optimal:** A → B → D → C → A, cost = 10+10+2+15 = **37**  
→ Greedy is **not optimal**.

#### Matrix 2 (A,B,C,D,E):

**Greedy:** A → B → D → E → C → A, cost = 1+5+8+7+6 = **27**  
**Optimal:** A → B → D → C → E → A, cost = 1+5+9+7+4 = **26**  
→ Greedy is **not optimal**.

---

## Lab 09 — Huffman Coding

### Rules (from lab sheet)

1. When selecting: if tie, choose alphabetically lowest label.
2. When connecting: lower weight goes LEFT; on tie, alphabetically earlier goes LEFT.

### Question 1 — Build Huffman Trees

#### String 1: `AAAABBBBBBCCCDDEEFFG`

Frequencies: A=4, B=6, C=3, D=2, E=2, F=2, G=1  
Total characters: 20

| Symbol | Freq | Code |
|--------|------|------|
| B | 6 | `10` |
| A | 4 | `00` |
| C | 3 | `110` |
| E | 2 | `010` |
| F | 2 | `011` |
| D | 2 | `1111` |
| G | 1 | `1110` |

**Fixed-length code:** 3 bits/char × 20 = **60 bits**  
**Huffman code:** 6·2 + 4·2 + 3·3 + 2·3 + 2·3 + 2·4 + 1·4 = **53 bits** (saving 7 bits)

---

#### String 2: `AABCCCDDEEEFFFFFFFFFFFFGGG`

Frequencies: F=12, C=3, E=3, G=3, D=2, A=2, B=1  
Total characters: 26

| Symbol | Freq | Code |
|--------|------|------|
| F | 12 | `0` |
| C | 3 | `100` |
| E | 3 | `101` |
| G | 3 | `110` |
| D | 2 | `1110` |
| A | 2 | `11111` |
| B | 1 | `11110` |

**Fixed-length code:** 3 × 26 = **78 bits**  
**Huffman code:** 12·1 + 3·3 + 3·3 + 3·3 + 2·4 + 2·5 + 1·5 = **62 bits** (saving 16 bits)

---

#### String 3: `AAABBBBCDDEEFFFFFFFFFFFG`

Frequencies: F=11, B=4, A=3, E=2, D=2, C=1, G=1  
Total characters: 24

| Symbol | Freq | Code |
|--------|------|------|
| F | 11 | `0` |
| B | 4 | `110` |
| A | 3 | `101` |
| E | 2 | `100` |
| D | 2 | `1111` |
| C | 1 | `11100` |
| G | 1 | `11101` |

**Fixed-length code:** 3 × 24 = **72 bits**  
**Huffman code:** 11·1 + 4·3 + 3·3 + 2·3 + 2·4 + 1·5 + 1·5 = **56 bits** (saving 16 bits)

---

## Lab 10 — Local Search

### Question 1 — Knapsack Neighborhood Relation

**Solution representation:** binary vector `x = (x₁, ..., xₙ)`, where xᵢ=1 means item i is taken.

**Proposed neighborhood:** flip exactly one bit (add or remove one item):
```
N(x) = {x' : x' differs from x in exactly one bit position}
```

**Verification of all 4 properties:**

| Property | Status | Argument |
|----------|--------|----------|
| Connected | ✓ | Any solution reachable from any other by ≤ n flips |
| Similar neighbors | ✓ | Neighbors differ by exactly 1 item (±1 weight step) |
| Small: \|N(x)\| = n | ✓ | Exactly one flip per item → O(n) |
| Small diameter = n | ✓ | At most n flips needed → O(n) |

**Graph for n = 3 items (8 nodes):**
```
   000
  /   \
001   010   (each edge = 1 bit flip)
 |╲  ╱|
 | 011 |
 |  |  |
100  |  
 |  101
 |╲ ╱
110  
  \
  111
```

**Hill-Climbing result on `knapsack.txt` (32 items, W_max=75):**  
- Greedy start: weight=74, value=**260**
- After HC: weight=74, value=**260** (already at local optimum)
- Known optimal (from file): weight=75, value=**262**
- HC reaches **99.2% of optimal** — very close but not globally optimal, as expected.

---

### Question 2 — TSP Neighborhood Relation

**Proposed neighborhood: 2-opt swap**

For a tour `(c₁, c₂, ..., cₙ, c₁)`, remove two edges and reconnect:

```
Before: ... cᵢ → cᵢ₊₁ → ... → cⱼ → cⱼ₊₁ ...
After:  ... cᵢ → cⱼ → ... → cᵢ₊₁ → cⱼ₊₁ ...
(segment i+1..j is reversed)
```

**Verification:**

| Property | Status | Argument |
|----------|--------|----------|
| Connected | ✓ | Any tour reachable from any other by 2-opt moves |
| Similar neighbors | ✓ | Neighbors differ by 2 edges only |
| Small: \|N\| = n(n−1)/2 | ✓ | O(n²) — acceptable |
| Diameter | ✓ | O(n²) — polynomial |

---

## Review Questions

### Q1 — Vectors

**x = (−3, −3, −2, −2),  y = (0, 2, −1, −2)**

**(a) Lengths:**
```
|x| = √(9+9+4+4) = √26 ≈ 5.099
|y| = √(0+4+1+4) = √9  = 3.000
```

**(b) Euclidean distance:**
```
d(x,y) = √((−3−0)²+(−3−2)²+(−2+1)²+(−2+2)²) = √(9+25+1+0) = √35 ≈ 5.916
```

**(c) Scalar (dot) product:**
```
x·y = (−3)(0) + (−3)(2) + (−2)(−1) + (−2)(−2) = 0 − 6 + 2 + 4 = 0
```

**(d) Perpendicular?** Yes — dot product = 0.

---

### Q2 — k-NN

Training set: A(2,2,3), A(3,2,4), B(2,−5,3), B(3,−4,2), C(3,2,−2), C(3,1,−1)  
Query: x = (3, −3, 2), k = 3

| Point | Distance² | Distance | Class |
|-------|-----------|----------|-------|
| B(3,−4,2) | 0+1+0=1 | **1.000** | B |
| B(2,−5,3) | 1+4+1=6 | **2.449** | B |
| C(3,1,−1) | 0+16+9=25 | **5.000** | C |
| A(2,2,3) | 1+25+1=27 | 5.196 | A |
| A(3,2,4) | 0+25+4=29 | 5.385 | A |
| C(3,2,−2) | 0+25+16=41 | 6.403 | C |

3 nearest: B(1.000), B(2.449), C(5.000) → **B=2, C=1 → Classification: B**

---

### Q3 — Perceptron Training Step

**w = (2,−1,2), θ = 2, x = (1,2,3), d = 0, α = 1**

```
net = 2(1)+(−1)(2)+2(3)−2 = 2−2+6−2 = 4
y = f(4) = 1   (net ≥ 0)
error = 0 − 1 = −1

w' = (2,−1,2) + 1·(−1)·(1,2,3) = (1, −3, −1)
θ' = 2 − 1·(−1) = 3
```

---

### Q4 — Classifier Evaluation

**100 examples, 70 classified positive (50 correct), 10 false negatives**

Derivation: TP=50, FP=20 (70 classified positive − 50 correct), FN=10, TN=20

**Confusion matrix:**

```
                  Classified as:
                  Positive    Negative
Actual Positive:     50          10
Actual Negative:     20          20
```

| Metric | Calculation | Result |
|--------|-------------|--------|
| Accuracy | (50+20)/100 | **70.0% = 7/10** |
| Precision | 50/(50+20) | **71.4% = 5/7** |
| Recall | 50/(50+10) | **83.3% = 5/6** |
| F-score | 2·(5/7)·(5/6)/((5/7)+(5/6)) | **76.9% = 10/13** |

---

### Q5 — k-Means

**Points:** A(1,−2,−1), B(1,2,−5), C(−2,6,−2), D(1,−4,−2)  
**Initial centroids:** c₁=(1,−2,−1), c₂=(1,2,−5)

**Iteration 1:**

| Point | d(c₁) | d(c₂) | Cluster |
|-------|--------|--------|---------|
| A | √0=0 | √32≈5.66 | **C₁** |
| B | √32≈5.66 | √0=0 | **C₂** |
| C | √74≈8.60 | √34≈5.83 | **C₂** |
| D | √5≈2.24 | √45≈6.71 | **C₁** |

C₁={A,D}, C₂={B,C}  
New centroids: c₁=(1.0, −3.0, −1.5), c₂=(−0.5, 4.0, −3.5)

**Iteration 2:**

Same assignments (C₁={A,D}, C₂={B,C}) → **Converged!**

**Final result:**
- **Cluster 1:** {A(1,−2,−1), D(1,−4,−2)}
- **Cluster 2:** {B(1,2,−5), C(−2,6,−2)}

---

### Q6 — Naive Bayes (Review dataset: 8 examples, 3 YES, 5 NO)

**Classify: (overcast, cool, high, false)**
```
log P(YES|x) ≈ −4.828   >   log P(NO|x) ≈ −5.671
→ Classification: YES
```

**Classify: (rainy, mild, normal, false)**
```
log P(YES|x) ≈ −4.135   >   log P(NO|x) ≈ −6.077
→ Classification: YES
```

---

### Q7 — Huffman Coding

#### `AAAABBBCCCDDDDDDEFFFFFGG`

Frequencies: A=4, B=3, C=3, D=6, E=1, F=5, G=2

| Symbol | Freq | Code |
|--------|------|------|
| D | 6 | `10` |
| F | 5 | `00` |
| A | 4 | `111` |
| B | 3 | `010` |
| C | 3 | `011` |
| G | 2 | `1101` |
| E | 1 | `1100` |

**Fixed:** 3×24 = **72 bits** | **Huffman:** **64 bits** (saving 8)

---

#### `AAABBCCCCDDDDDEEEEFFGGGG`

Frequencies: A=3, B=2, C=4, D=5, E=4, F=2, G=4

| Symbol | Freq | Code |
|--------|------|------|
| D | 5 | `01` |
| G | 4 | `00` |
| C | 4 | `110` |
| E | 4 | `111` |
| A | 3 | `100` |
| B | 2 | `1010` |
| F | 2 | `1011` |

**Fixed:** 3×24 = **72 bits** | **Huffman:** **67 bits** (saving 5)

---

### Q8 — Regression Tree

Dataset (from scatter plot): regions y=0, y=2, y=3, y=4

**Best first split: X2 ≤ 2.5**
```
R₁ (X2 ≤ 2): points with y ∈ {0,2} → mean ≈ 1.20, RSS = 6.00
R₂ (X2 > 2): points with y ∈ {3,4} → mean ≈ 3.40, RSS = 0.00
Total RSS = 6.00 (minimum found)
```

**Decision node:**
```
         X2 ≤ 2?
        ┌────┴────┐
       Yes        No
        │          │
    ŷ ≈ 1.2    ŷ ≈ 3.4
```

---

### Q9 — Knapsack Greedy (C = 22)

Items: i=1(w=3,v=6), 2(w=5,v=9), 3(w=6,v=6), 4(w=3,v=1), 5(w=10,v=11), 6(w=3,v=2), 7(w=5,v=8), 8(w=8,v=7)

| Item | w | v | v/w | Decision |
|------|----|---|-----|---------|
| 1 | 3 | 6 | 2.000 | ✓ Take (w=3) |
| 2 | 5 | 9 | 1.800 | ✓ Take (w=8) |
| 7 | 5 | 8 | 1.600 | ✓ Take (w=13) |
| 5 | 10 | 11 | 1.100 | ✗ Skip (would exceed 22) |
| 3 | 6 | 6 | 1.000 | ✓ Take (w=19) |
| 8 | 8 | 7 | 0.875 | ✗ Skip |
| 6 | 3 | 2 | 0.667 | ✓ Take (w=22) |
| 4 | 3 | 1 | 0.333 | ✗ Skip |

**Greedy result: items {1,2,3,6,7}, weight=22, value=31**

---

## Summary Table — All Final Answers

| Lab | Question | Answer |
|-----|---------|--------|
| L01 | 1a scalar mult | (−4, 10, −4) |
| L01 | 1c lengths | 3, √30, √26 |
| L01 | 1d Euclidean A(1,2,3)B(3,4,5) | 3.464 |
| L01 | 2b matrix products | as computed above |
| L02 | Q1a (1,5) | **A** |
| L02 | Q1a (2,6) | **A** |
| L02 | Q1a (3,4) | **A** |
| L02 | Q1b (4,4,0) | **A** |
| L02 | Q1b (1,1,5) | **B** |
| L02 | Q1b (6,0,0) | **C** |
| L03 | Q1 x=(7,-2,-5,4) | y=0 |
| L03 | Q1 x=(2,0,2,8) | y=1 |
| L03 | Q2 hyperplane | 2x₁−x₂+4x₃+x₄−3=0 |
| L03 | Q3 AND | w=(1,1), θ=1.5 |
| L03 | Q3 XOR | Impossible (not linearly separable) |
| L03 | Q4 final state | w=(3,-2.5,-4,0.5), θ=3.5 |
| L04 | Q1 eval Matrix1 | Acc=76.9%, P=66.7%, R=80.0%, F=72.7% |
| L04 | Q1 eval Matrix2 | Acc=84.0%, P=75.0%, R=83.3%, F=78.9% |
| L04 | Q1 eval Matrix3 | Acc=84.0%, P=90.0%, R=75.0%, F=81.8% |
| L04 | Q1 sigmoid x=(7,-2,-4,3) | y=0.500, w'=(2.875,-1.25,3.5,1.375) |
| L05 | sunny/cool/high/true | **NO** |
| L05 | overcast/mild/normal/false | **YES** |
| L05 | overcast/cool/high/false | **YES** |
| L06 | PlayGolf root | Outlook (gain=0.2467) |
| L06 | Vampire root | Shadow (gain=0.4544) |
| L07 | Q1a k-means final | C1={A}, C2={B,C,D} |
| L07 | Q1b k-means final | C1={A,B,D}, C2={C} |
| L07 | Q2a dendro heights | 1.0, 1.0, 2.0, 2.236 |
| L07 | Q2b dendro heights | 1.414, 1.414, 2.236, 2.828, 3.0 |
| L08 | Q1 Dataset1 optimal | v=54 (greedy gets 52) |
| L08 | Q2 TSP1 optimal | A→B→D→C→A, cost=37 |
| L08 | Q2 TSP2 optimal | A→B→D→C→E→A, cost=26 |
| L09 | String 1 Huffman | 53 bits vs 60 fixed |
| L09 | String 2 Huffman | 62 bits vs 78 fixed |
| L09 | String 3 Huffman | 56 bits vs 72 fixed |
| L10 | Neighborhood | Bit-flip, \|N\|=n, connected, O(n) diameter |
| RQ | Q1 vectors | \|x\|=√26, d=√35, x·y=0, perpendicular |
| RQ | Q2 k-NN | **Class B** |
| RQ | Q3 perceptron | w'=(1,-3,-1), θ'=3 |
| RQ | Q4 evaluation | Acc=70%, P=71.4%, R=83.3%, F=76.9% |
| RQ | Q5 k-means | C1={A,D}, C2={B,C} (converges iter 2) |
| RQ | Q6 NB ex1 | **YES** |
| RQ | Q6 NB ex2 | **YES** |
| RQ | Q7 Huffman1 | 64 bits vs 72 fixed |
| RQ | Q9 greedy knapsack | items {1,2,3,6,7}, v=31 |
