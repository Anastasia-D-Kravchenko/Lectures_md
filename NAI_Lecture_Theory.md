# 🧠 NAI — Machine Learning & Optimisation: Full Lecture Theory

> **Course:** NAI 2026 · Lecturer: Marcin Sydow  
> **Coverage:** All 13 Lectures — every definition, formula, algorithm, and intuition

---

## Table of Contents

1. [ML Introduction & Taxonomy](#1-ml-introduction--taxonomy)
2. [k-Nearest Neighbors (k-NN)](#2-k-nearest-neighbors-k-nn)
3. [Perceptron & Delta Rule](#3-perceptron--delta-rule)
4. [Single-Layer Neural Network & Backpropagation](#4-single-layer-neural-network--backpropagation)
5. [Classifier Evaluation](#5-classifier-evaluation)
6. [Naive Bayes Classifier](#6-naive-bayes-classifier)
7. [Decision Trees — ID3](#7-decision-trees--id3)
8. [Regression Trees](#8-regression-trees)
9. [Ensemble Methods: Bagging, Random Forests, Boosting](#9-ensemble-methods)
10. [Unsupervised Learning: k-Means & Hierarchical Clustering](#10-unsupervised-learning)
11. [PCA — Principal Component Analysis](#11-pca)
12. [Knowledge Representation & Overfitting](#12-knowledge-representation--overfitting)
13. [Optimisation & Greedy Algorithms](#13-optimisation--greedy-algorithms)
14. [Dynamic Programming](#14-dynamic-programming)
15. [Local Search: Hill Climbing & Simulated Annealing](#15-local-search)
16. [Formula Cheatsheet](#16-formula-cheatsheet)

---

## 1. ML Introduction & Taxonomy

### What is Machine Learning?

**Machine Learning** is a subfield of AI where algorithms learn patterns from data **without being explicitly programmed** for each rule. It's needed because modern data is too vast and complex for direct human analysis.

**Sources of data:** web logs, online shopping, mobile phones, credit cards, traffic sensors, surveillance cameras, stock markets, scientific measurements.

---

### The 6-Step ML Pipeline

```
  ┌─────────────────────────────────────────────────────────┐
  │                  ML PIPELINE                            │
  │                                                         │
  │  1. Data Acquisition                                    │
  │       ↓                                                 │
  │  2. Data Cleaning & Pre-processing                      │
  │       ↓                                                 │
  │  3. Train / Test Split  (supervised only)               │
  │       ↓                                                 │
  │  4. Learning  ←──────────────────────────┐              │
  │       ↓                                  │              │
  │  5. Evaluation  ─── not good enough? ───┘              │
  │       ↓                                                 │
  │  6. Deployment                                          │
  └─────────────────────────────────────────────────────────┘
```

---

### Two Main Paradigms

```
                      MACHINE LEARNING
                      ┌──────┴──────┐
              SUPERVISED         UNSUPERVISED
          (has labels Y)        (no labels — only X)
           ┌────┴────┐          ┌────────┬──────────┐
    Classification Regression Clustering  PCA  Assoc.Rules
    (output is a  (output is  (k-Means,   (dim. reduction)
     category)    a number)   Hierarchical)
```

| Type | What's given | Goal | Examples |
|------|-------------|------|---------|
| **Supervised** | Features **X** + label **Y** | Learn X → Y mapping | k-NN, Perceptron, Naive Bayes, Trees |
| **Unsupervised** | Features **X** only | Discover structure | k-Means, PCA, Hierarchical Clustering |

> **EXAM TRAP:** The **output type** determines Classification vs Regression — not the input type.  
> - Output is a category name → **Classification**  
> - Output is a number → **Regression**

---

### Decision Table Structure

```
outlook   | temperature | humidity | windy | PLAY?   ← decision attribute
──────────────────────────────────────────────────
sunny     | hot         | high     | false | no      ← case/instance
overcast  | hot         | high     | false | yes
rainy     | mild        | high     | false | yes
...
```

- Each **row** = one **case** (instance / observation)
- Each **column** = one **attribute** (feature / predictor)
- Last column = **decision attribute** (the label to predict)

Attribute types:
- **Nominal** (categorical, unordered): `outlook ∈ {sunny, overcast, rainy}`
- **Numeric** (ordered, continuous): `temperature in °C`

---

## 2. k-Nearest Neighbors (k-NN)

### Core Idea

k-NN is **non-parametric** and **lazy** — it stores all training data and defers all computation to prediction time.

```
    Training phase:        Prediction phase:
    ┌──────────────┐       ┌─────────────────────────────┐
    │ Just store   │       │ 1. Compute distances to all  │
    │ the dataset  │  →    │    training points            │
    └──────────────┘       │ 2. Find k nearest neighbours │
                           │ 3. Majority vote → class     │
                           └─────────────────────────────┘
```

---

### Distance Metrics

**Euclidean Distance** (most common):
```
d(x, y) = √( Σᵢ (xᵢ − yᵢ)² )
```

**Manhattan Distance** (L1 norm):
```
d(x, y) = Σᵢ |xᵢ − yᵢ|
```

---

### Effect of k — The Bias-Variance Trade-off

```
  Decision boundary complexity
  
  k=1 (overfit)          k=5 (good)           k=50 (underfit)
  ┌───────────────┐      ┌───────────────┐      ┌───────────────┐
  │ ●  ○  ●  ○   │      │  ●●    ○○○   │      │               │
  │○ ●○●○ ●○ ●   │  →   │    ●●●       │  →   │    ●●●○○○    │
  │ ●  ○  ●  ○   │      │       ○○○    │      │               │
  │ jagged, noisy│      │  smooth, OK  │      │ too smooth    │
  └───────────────┘      └───────────────┘      └───────────────┘
```

| k | Boundary | Bias | Variance |
|---|----------|------|----------|
| Small (k=1) | Very jagged | Low | High (overfits noise) |
| Medium | Smooth | Medium | Medium ✓ |
| Large | Very smooth | High (underfits) | Low |

**Rule of thumb:** start with `k = √n` where n = training set size.

---

### Feature Scaling is Critical

Without normalisation, features with large ranges dominate distances.

```
Example: patient data
─────────────────────────────────────────────────
Feature     | Range        | Without norm | With [0,1]
───────────────────────────────────────────────────
Age         | 0–100 years  | 100 units    | 1 unit
Income      | €0–€100,000  | 100,000 units| 1 unit
───────────────────────────────────────────────────
Without normalisation: income difference of €1 = age difference of 1 year
→ income completely dominates, age is ignored!
```

> **Key properties:**
> - **No training phase** (lazy): O(1) train time, O(n·d) predict time
> - **Non-parametric**: no assumptions about data distribution
> - **Tie-breaking**: if k neighbours are tied, use the nearest 1 neighbour among the tied classes

---

## 3. Perceptron & Delta Rule

### Biological Inspiration

```
  Biological neuron              Mathematical perceptron
  ──────────────────             ──────────────────────
  Dendrites (inputs)    →        x₁, x₂, ..., xₙ
  Synapses (weights)    →        w₁, w₂, ..., wₙ
  Cell body (soma)      →        Σ wᵢxᵢ − θ  (net value)
  Axon (output)         →        y = f(net)
```

### Net Value and Activation

```
net = w₁x₁ + w₂x₂ + ... + wₙxₙ − θ  =  Wᵀ·X − θ
```

**Step (discrete) activation:**
```
y = 1   if net ≥ 0    (neuron fires)
y = 0   if net < 0    (neuron silent)
```

---

### Geometric Interpretation

```
2D example: w = (w₁, w₂), threshold θ

        x₂
         │        region y=1
         │     /  (same side as W)
    ─────┼────/──────────────────
         │   / ← decision hyperplane:
         │  /    w₁x₁ + w₂x₂ − θ = 0
   y=0   │ /
   ──────┴────────────────── x₁
         │
```

- **W** = normal vector of the decision hyperplane
- **net > 0** when X is on the same side as W → fires
- **net < 0** when X is on the opposite side → silent

---

### Linear Separability — What a Perceptron CAN and CANNOT Do

```
AND  (linearly separable ✓)    OR (linearly separable ✓)    XOR (NOT separable ✗)
┌─────────────────┐             ┌─────────────────┐           ┌─────────────────┐
│  0   0  │  0    │             │  0   0  │  0    │           │  0   0  │  0    │
│   (0,0) │       │             │   (0,0) │       │           │   (0,0) │       │
│  ──────/│───────│             │/────────│───────│           │   ──/───│──/────│
│  0   1  │  0    │             │  0   1  │  1    │           │  0   1  │  1    │
│   (0,1) │       │             │   (0,1) │       │           │   (0,1) │       │
│  1   0  │  0    │             │  1   0  │  1    │           │  1   0  │  1    │
│   (1,0) │       │             │   (1,0) │       │           │   (1,0) │       │
│  1   1  │  1    │             │  1   1  │  1    │           │  1   1  │  0    │
│   (1,1) │       │             │   (1,1) │       │           │   (1,1) │       │
│  ✓ One line separates│        │ ✓ One line separates│       │ ✗ No single line │
└─────────────────┘             └─────────────────┘           └─────────────────┘
```

**XOR requires at minimum 2 layers (a multi-layer network).**

---

### The Delta Rule (Perceptron Learning Rule)

```
After each prediction:

Wₙₑₓₜ  =  Wₒₗ𝒹  +  α · (d − y) · X
θₙₑₓₜ  =  θₒₗ𝒹  −  α · (d − y)

where:
  d = desired output (correct label)
  y = actual output of perceptron
  α = learning rate (small positive number, e.g. 0.1–0.5)
  (d − y) = error:
    0  → correct (no update)
   +1  → should fire but didn't → pull W toward X
   −1  → fired when shouldn't → push W away from X
```

**Training loop:**
```
INITIALISE W, θ randomly
REPEAT:
    FOR EACH (X, d) in training set:
        y = predict(X)
        UPDATE W and θ by delta rule
    E = mean squared error over epoch
UNTIL E ≤ E_max OR max_epochs reached
```

---

## 4. Single-Layer Neural Network & Backpropagation

### Activation Functions Compared

| Function | Formula | Range | Notes |
|----------|---------|-------|-------|
| Step | `y = 1 if net≥0 else 0` | {0, 1} | Discrete, no gradient |
| Sign | `y = signum(net)` | {−1, +1} | Bipolar discrete |
| **Sigmoid (unipolar)** | `y = 1/(1+e^{−net})` | (0, 1) | Differentiable ✓ |
| Sigmoid (bipolar) | `y = 2/(1+e^{−net})−1` | (−1, 1) | Differentiable ✓ |
| Linear | `y = net` | (−∞, +∞) | Regression output |

**Sigmoid shape:**
```
y
1.0 │              ···················
    │          ···
0.5 │      ·/·   ← net=0
    │    ··
0.0 │···
    └──────────────────────────── net
      -5  -3  -1   0   1   3   5
```

**Key sigmoid derivative property** (enables efficient gradient descent):
```
f'(net) = f(net) · (1 − f(net))  =  y · (1 − y)
```
The derivative is expressible entirely in terms of the output `y` — no separate computation needed.

---

### Multi-Class: Local vs Global Representation

```
Local representation (1 neuron per class — preferred):
┌────────────────────────────────────────┐
│  Input layer    Output layer           │
│  x₁ ──┐        z₁ → 0.9 ← "Setosa"   │
│  x₂ ──┼──→     z₂ → 0.1              │
│  x₃ ──┤        z₃ → 0.05             │
│  x₄ ──┘     ↑                         │
│          Winner takes all             │
└────────────────────────────────────────┘
K classes → K neurons. The highest-activating neuron wins.
```

Minimum neurons: **K** (local) vs **⌈log₂K⌉** (global — harder to train).

---

### Weight Update Rules

**Single neuron with sigmoid:**
```
Δw = α · (d − y) · y · (1 − y) · x
Δθ = −α · (d − y) · y · (1 − y)
```

**Matrix form (1-layer network, K neurons, J inputs):**
```
Forward:  z = Γ[W · y]          (Γ applies sigmoid element-wise)
Error:    δzₖ = (dₖ − zₖ) · zₖ · (1 − zₖ)
Update:   W_new = W_old + η · δz · yᵀ    (outer product)
```

---

### Backpropagation (2-Layer Network)

```
Network structure:

  Input x  →  [Hidden V]  →  [Output W]  →  prediction z
  (I units)    (J neurons)    (K neurons)
```

**Forward pass:**
```
y = Γ[V · x]            (hidden layer activations)
z = Γ[W · y]            (output layer activations)
```

**Backward pass:**
```
Step 1 — Output error signals:
  δzₖ = (dₖ − zₖ) · zₖ · (1 − zₖ)

Step 2 — Hidden error signals (propagated backward through W):
  δyⱼ = yⱼ · (1 − yⱼ) · Σₖ (δzₖ · wₖⱼ)

Step 3 — Update output weights:
  W_new = W_old + η · δz · yᵀ

Step 4 — Update hidden weights:
  V_new = V_old + η · δy · xᵀ
```

> **Why "back-propagation"?** The error signals δz from the output layer are propagated **backwards** through W to compute δy for the hidden layer — since the hidden layer has no "desired output" in the training data.

---

## 5. Classifier Evaluation

### The Problem with Accuracy Alone

Consider a dataset with 99% class A and 1% class B. A trivial classifier that always predicts A gets **99% accuracy** while being completely useless — it never detects B.

---

### Confusion Matrix

```
                    PREDICTED
                   Pos    Neg
            ┌──────────────────┐
ACTUAL  Pos │  TP  │  FN      │
        Neg │  FP  │  TN      │
            └──────────────────┘

TP = True Positive   (correctly predicted positive)
TN = True Negative   (correctly predicted negative)
FP = False Positive  (alarm raised incorrectly)
FN = False Negative  (missed a real positive — often the most dangerous!)
```

---

### Key Metrics

```
Accuracy   =  (TP + TN) / (TP + TN + FP + FN)

Precision  =  TP / (TP + FP)    ← "When I said positive, was I right?"
Recall     =  TP / (TP + FN)    ← "Of all real positives, did I catch them?"
F1-Score   =  2·P·R / (P + R)   ← harmonic mean, balances both
```

**Precision vs Recall trade-off:**
```
  Conservative classifier         Liberal classifier
  (only fires when certain)        (fires at any hint)
  ┌────────────────────┐           ┌────────────────────┐
  │ Precision = HIGH   │           │ Precision = LOW    │
  │ Recall = LOW       │           │ Recall = HIGH      │
  │ F1 = moderate      │           │ F1 = moderate      │
  │ "few false alarms" │           │ "catches everything│
  │ "misses some real" │           │  but many FP"      │
  └────────────────────┘           └────────────────────┘
  Use for: spam filtering          Use for: cancer screening
```

> **EXAM TRAP:** F1 is the **harmonic mean** — it is very sensitive to low values. If either P or R is 0, F1 = 0. A classifier with P=0.9, R=0.1 has F1 ≈ 0.18 — terrible, despite high precision.

---

## 6. Naive Bayes Classifier

### Core Theory

Based on **Bayes' Theorem** + the "naive" conditional independence assumption.

**Bayes' Theorem:**
```
P(Y=y | X) = P(X | Y=y) · P(Y=y) / P(X)
             ─────────────────────────────
              posterior ∝ likelihood × prior
```

**The Naive Assumption** — all features are conditionally independent given the class:
```
P(X₁=x₁, ..., Xₙ=xₙ | Y=y)  =  Π P(Xᵢ=xᵢ | Y=y)
                                   i=1..n
```

**Classification rule:**
```
Classify x to:  y* = argmax_y  P(Y=y) · Πᵢ P(Xᵢ=xᵢ | Y=y)
```

---

### Estimating Probabilities from Training Data

```
P(Y=y) = count(y) / total_examples

P(Xᵢ=xᵢ | Y=y) = count(xᵢ AND y) / count(y)
```

---

### The Zero Probability Problem & Laplace Smoothing

```
Without smoothing:
─────────────────
If (feature=value, class=y) never appears in training:
  P(Xᵢ=xᵢ | Y=y) = 0/count(y) = 0
  → entire product = 0, class is eliminated regardless of other evidence ✗

With Laplace (add-one) smoothing:
──────────────────────────────────
P(Xᵢ=xᵢ | Y=y) = (count(xᵢ, y) + 1) / (count(y) + d)

  d = number of distinct values that attribute Xᵢ can take
  → probabilities sum to 1 still ✓
  → no zeros ✓
```

### Why Log-Probabilities?

Multiplying 22 small probabilities (e.g., the mushroom dataset) causes **floating-point underflow** — the product becomes 0 before completing.

```
log P(y|x) ∝ log P(y) + Σᵢ log P(xᵢ|y)
─────────────────────────────────────────
Sums instead of products → no underflow → compare class log-scores → same argmax
```

---

### Classification Process (Visual)

```
New example: [outlook=sunny, temp=cool, humidity=high, windy=true]

For each class y ∈ {YES, NO}:
  score(y) = log P(Y=y)
            + log P(outlook=sunny  | Y=y)
            + log P(temp=cool      | Y=y)
            + log P(humidity=high  | Y=y)
            + log P(windy=true     | Y=y)

Predict class with highest score.
```

---

## 7. Decision Trees — ID3

### Structure of a Decision Tree

```
                    Outlook?
          ┌───────────┼──────────────┐
        sunny      overcast        rainy
          │           │               │
       Humidity?    YES           Windy?
       ┌──┴──┐                   ┌────┴────┐
     high  normal              false     true
       │       │                 │          │
      NO      YES               YES        NO
```

- **Internal nodes** = test one attribute
- **Edges** = possible values of that attribute
- **Leaf nodes** = class label

---

### Information Entropy

**Entropy of set S** (measures impurity / uncertainty):
```
H(S) = − Σ_c  p_c · log₂(p_c)

where p_c = fraction of examples in class c
```

**Special cases:**
```
H = 0    → pure set (all one class) — perfect!
H = 1    → binary case, 50/50 split — maximum confusion
H = log₂K → K classes equally represented — absolute maximum
```

**Visualisation (binary):**
```
H(p)
1.0 │         ●
    │      ●     ●
0.5 │   ●           ●
    │●                 ●
0.0 └────────────────────── p
    0   0.25  0.5  0.75   1
    pure  →   mixed  ←  pure
```

---

### Information Gain — Choosing the Best Split

```
H(S, A) = Σ_v  (|Sᵥ| / |S|) · H(Sᵥ)     (weighted entropy after split)

Gain(S, A) = H(S) − H(S, A)               (how much uncertainty removed?)
```

**The ID3 algorithm always chooses the attribute with the highest Gain.**

---

### ID3 Algorithm

```
FUNCTION ID3(examples, attributes):
    IF all examples same class c:
        RETURN leaf(c)
    IF attributes is empty:
        RETURN leaf(most_common_class)
    ELSE:
        A = attribute with highest Gain(examples, A)
        Create node testing A
        FOR EACH value v of A:
            Sᵥ = examples where A=v
            IF Sᵥ empty: add leaf(most_common_class)
            ELSE: add subtree ID3(Sᵥ, attributes \ {A})
        RETURN node
```

**PlayGolf example — Information Gains:**
```
Attribute    | Gain
─────────────┼──────
Outlook      | 0.246  ← root (highest gain!)
Humidity     | 0.151
Wind         | 0.048
Temperature  | 0.029
```

---

## 8. Regression Trees

### How Regression Trees Differ

Same tree structure as classification trees, but:
- Predict a **continuous number** at each leaf (not a class)
- Leaf prediction = **mean** of training examples in that region
- Split criterion = **minimise RSS** (not information gain)

```
Residual Sum of Squares (RSS):
RSS(Rⱼ) = Σᵢ∈Rⱼ (yᵢ − ȳⱼ)²      where ȳⱼ = mean of Rⱼ

At each step: find predictor j and cutpoint s that minimise:
Total RSS = RSS(R₁) + RSS(R₂)
```

---

### Recursive Binary Splitting

```
Algorithm:
1. For each predictor j, for each split point s:
   R₁(j,s) = {x | xⱼ < s}    R₂(j,s) = {x | xⱼ ≥ s}
   Compute total RSS(R₁) + RSS(R₂)
2. Choose (j*, s*) minimising total RSS
3. Recurse on each region

→ Greedy, top-down (no look-ahead)
→ Stop when regions have < min_examples (e.g. 5)
```

---

### Pruning (Cost-Complexity)

```
C_α(T) = Σ_leaves RSS(leaf) + α · |T|

|T|  = number of leaves
α    = complexity penalty (tuning parameter)
```

```
α → 0:   full tree T₀ (no penalty)  → overfits
α → ∞:   tiny tree (one leaf)       → underfits
Best α: chosen by K-fold cross-validation
```

---

## 9. Ensemble Methods

### Why Ensembles?

Single decision trees have **high variance** — small changes in training data produce very different trees.

> **Core principle:** If n independent random variables each have variance σ², their average has variance σ²/n. **Averaging reduces variance.**

---

### A. Bagging (Bootstrap Aggregation)

```
For b = 1, ..., B:
  1. Sample n points WITH REPLACEMENT from training data  → bootstrap sample Z*_b
  2. Fit a full (unpruned) tree f̂*_b on Z*_b

Final prediction:
  Regression:       f̂_bag(x) = (1/B) · Σ f̂*_b(x)
  Classification:   majority vote across B trees
```

**Out-of-Bag (OOB) Error:**
```
Each bootstrap sample contains ~⅔ of original data.
The remaining ~⅓ (OOB) were NOT used to train that tree.
→ Predict each observation using only its OOB trees
→ Free cross-validation estimate — no separate test set needed!
```

---

### B. Random Forests

**Key modification:** At each split, consider only a random subset of **m** predictors:
```
Classification: m ≈ √p   (p = total predictors)
Regression:     m ≈ p/3
```

**Why this helps:**
```
Without random features          With random features
─────────────────────────        ────────────────────────
Every tree splits on the same    Different trees forced to use
dominant predictor at the root   different features
→ trees are highly CORRELATED    → trees are DECORRELATED
→ averaging correlated trees     → averaging diverse trees
   barely reduces variance          strongly reduces variance
```

**Variable importance:** Sum total RSS/Gini decrease across all splits on each predictor, averaged over all B trees.

---

### C. Boosting

Instead of training trees independently — train **sequentially on residuals**:

```
Algorithm:
1. Set f̂(x) = 0, residuals rᵢ = yᵢ for all i
2. For b = 1, ..., B:
   a. Fit shallow tree f̂_b (depth d) to (X, r)
   b. Update: f̂(x) = f̂(x) + λ · f̂_b(x)
   c. Update residuals: rᵢ = rᵢ − λ · f̂_b(xᵢ)
3. Final model: f̂(x) = Σ_b  λ · f̂_b(x)
```

**Three tuning parameters:**

| Parameter | Effect |
|-----------|--------|
| **B** (# trees) | Unlike bagging, CAN overfit — choose via CV |
| **λ** (learning rate) | Small λ (0.001) → learns slowly, needs large B, often best |
| **d** (tree depth) | d=1 → "stumps" → additive model; d controls interaction depth |

---

### Ensemble Comparison Table

| Method | Trees | Strategy | Interpretable | Accuracy |
|--------|-------|----------|---------------|----------|
| Single Tree | 1 | — | ✓✓✓ | Low |
| Bagging | B | Independent, bootstrapped | ✗ | Medium |
| Random Forest | B | Independent + random features | ✗ | High |
| Boosting | B | Sequential on residuals | ✗ | Highest ✓ |

> **EXAM: "Why use Random Forests over Bagging?"** When a strong predictor exists, all bagging trees split on it first → highly correlated trees → averaging barely helps. RF forces each tree to explore other predictors → decorrelated trees → averaging truly reduces variance.

---

## 10. Unsupervised Learning

### k-Means Clustering

**Goal:** Partition n observations into K clusters minimising total Within-Cluster Sum of Squares (WCSS):

```
Minimise: Σₖ WCV(Cₖ) = Σₖ Σᵢ∈Cₖ Σⱼ (xᵢⱼ − x̄ₖⱼ)²
```

**Algorithm:**
```
1. Randomly assign cluster labels 1..K to each observation
2. REPEAT until assignments don't change:
   a. Compute centroid of each cluster (mean of all points in cluster)
   b. Reassign each point to nearest centroid
```

**Why it converges:** Each step reduces or maintains WCSS → always converges. But may converge to a **local minimum** — run multiple times with different seeds!

---

### k-Means Visualised

```
Initialisation          After iteration 1       Converged
──────────────          ─────────────────       ─────────
  × ×                     × ×                   × ×
 × ×  × ×               × ×  × ×             × ×  × ×
★₁       ★₂            ★₁       ★₂            ★₁       ★₂
  △ △                     △ △                   △ △
 △ △  △ △               △ △  △ △             △ △  △ △

★ = centroid    × = cluster A members    △ = cluster B members
```

---

### Hierarchical Agglomerative Clustering

**No need to specify K in advance.** Builds a complete dendrogram.

**Algorithm:**
```
1. Start: n clusters (one per observation)
2. REPEAT until 1 cluster remains:
   a. Find two clusters with minimum dissimilarity
   b. Merge them
3. Result: dendrogram (tree of all merges)
```

**Reading a dendrogram:**
```
Dissimilarity
     │
 3.0 │────────────────────── final merge
     │           │           │
 2.0 │     ─────────     ─────────
     │     │       │     │   │   │
 1.0 │  ───────   ...   ...  ...  ...
     │  │     │
 0.0 │  A     B    C    D    E    F
     └─────────────────────────────
```

- **Height of fusion** = dissimilarity at merge (Y-axis is meaningful)
- **Horizontal position** = meaningless
- **Cut at height h** → obtain clusters (one per branch below the cut)

---

### Linkage Criteria

| Linkage | Distance(A, B) | Effect |
|---------|----------------|--------|
| **Complete** | max distance over all pairs | Compact, balanced clusters |
| **Single** | min distance over all pairs | Can cause "chaining" |
| **Average** | mean distance over all pairs | Good compromise ✓ |
| **Centroid** | distance between centroids | ⚠️ Can cause inversions — AVOID |

> **EXAM TRAP:** Centroid linkage can produce **inversions** (a later merge at LOWER height than a previous one), making the dendrogram uninterpretable. Avoid it.

---

### When to Standardise Before Clustering

```
STANDARDISE when:                      DON'T standardise when:
────────────────────                   ──────────────────────────
Features in different units            Features in same units and
(height cm, weight kg, age years)      variance differences are meaningful
→ high-variance features dominate
  distance calculation without it
```

---

## 11. PCA — Principal Component Analysis

### Goal: Find Directions of Maximum Variance

PCA finds **linear combinations of variables** that capture the most variance in the data.

```
Data in 2D:                        After PCA:
   x₂                                  PC2
    │       ·                            │·
    │     · · ·                          │ · ·
    │   · · · · ·         →              │  · · ·
    │ · · · · ·                        ──┼──────── PC1
    │   · · ·                         major axis captures
    │     ·                           most variance
    └──────────── x₁
```

---

### First Principal Component (PC1)

```
Z₁ = φ₁₁X₁ + φ₂₁X₂ + ... + φₚ₁Xₚ

Constraint: Σⱼ φⱼ₁² = 1  (unit vector — normalised direction)

Optimisation: maximise (1/n)·Σᵢ (Σⱼ φⱼ₁·xᵢⱼ)²   subject to the constraint

Solution: φ₁ = first eigenvector of the sample covariance matrix XᵀX
```

**Subsequent PCs:**
- PC2: maximises variance **uncorrelated with PC1** → orthogonal to PC1
- PCₘ: orthogonal to all previous PCs
- Maximum: min(n−1, p) principal components

---

### Proportion of Variance Explained (PVE)

```
PVE of m-th PC = Σᵢ zᵢₘ² / (Σⱼ Σᵢ xᵢⱼ²)

All PVEs sum to 1.
```

**Scree plot — choosing how many PCs:**
```
PVE
60% │■
40% │  ■
20% │    ■─■─■─■─■─■
 0% └────────────────── PC number
    1   2   3   4   5
         ↑
       Elbow: adding PC3 gives diminishing returns → keep 2 PCs
```

---

### Key Practical Notes

- **Always centre** variables (subtract mean) before PCA
- **Scale to unit variance** when features have different units — otherwise high-variance features dominate PC1
- **Exception:** if features are in the same units and variance differences are meaningful, skip scaling
- **Biplot:** shows both observations (as points) and variable loadings (as arrows) simultaneously

---

## 12. Knowledge Representation & Overfitting

### Types of Knowledge Representation

| Type | Example | Interpretable? |
|------|---------|---------------|
| **Neural Network** | Millions of floating-point weights | ✗ Black box |
| **Decision Rules** | `IF tear_rate=reduced THEN class=NONE` | ✓✓✓ |
| **Decision Tree** | Hierarchical IF-THEN structure | ✓✓ |

---

### Overfitting vs Underfitting

```
Error
  │
  │   Training error  ──────────────────────────────
  │   Test error  ──────────────── ↗ (overfitting)
  │                      ↘ optimal
  │────────────────────────────────────────────────── Complexity
                              ↑
                         SWEET SPOT
```

| | Training error | Test error | Cause |
|--|---------------|------------|-------|
| **Underfitting** | High | High | Model too simple |
| **Optimal** | Low | Low | Just right ✓ |
| **Overfitting** | Very low | High | Model memorises noise |

Complexity parameters by model:
- Neural networks: # hidden neurons and layers
- Decision trees: # leaves
- Decision rules: # rules

---

### Model Selection: Train / Validation / Test Split

```
┌──────────────────────────────────────────────────┐
│              Dataset                             │
│  ┌──────────────────┬──────────┬──────────┐      │
│  │   Training (~50%)│Validation│  Test    │      │
│  │   Fit model      │(~25%)    │(~25%)    │      │
│  │                  │Choose    │FINAL     │      │
│  │                  │hyperpar. │REPORT    │      │
│  └──────────────────┴──────────┴──────────┘      │
│                                ↑                 │
│              Use only ONCE — sacred!             │
└──────────────────────────────────────────────────┘
```

---

### K-Fold Cross-Validation

```
Data split into K=5 folds:

Fold 1: [TEST ] [train] [train] [train] [train]  → error₁
Fold 2: [train] [TEST ] [train] [train] [train]  → error₂
Fold 3: [train] [train] [TEST ] [train] [train]  → error₃
Fold 4: [train] [train] [train] [TEST ] [train]  → error₄
Fold 5: [train] [train] [train] [train] [TEST ]  → error₅

CV Error = (error₁ + error₂ + ... + error₅) / 5
```

- Most common: **K=10** (10-fold CV)
- Every example tested exactly once
- **Stratification:** maintain class proportions in every fold
- **LOO (Leave-One-Out):** K=n — deterministic but expensive

---

## 13. Optimisation & Greedy Algorithms

### Formal Optimisation Problem

An **optimisation problem** requires:
- **INPUT:** description of the instance
- **OUTPUT — feasibility:** conditions for a valid solution
- **OUTPUT — optimality:** minimise or maximise an objective function

**Examples:**

| Problem | Feasibility | Optimality | Complexity |
|---------|------------|------------|-----------|
| **Knapsack** | Total weight ≤ C | Maximise total value | NP-hard (discrete) |
| **TSP** | Hamiltonian cycle | Minimise total distance | NP-hard |
| **MST** | Spanning tree | Minimise total edge weight | P (polynomial!) |
| **Continuous Knapsack** | Total weight ≤ C (fractions ok) | Maximise total value | P |

---

### Greedy Algorithms: When Do They Work?

A greedy algorithm makes the locally best choice at each step and **never reconsiders**.

**Matroids and the Rado-Edmonds Theorem:**

A **matroid** M = (E, I) satisfies:
1. **Hereditary:** ∅ ∈ I; if A ∈ I and B ⊆ A then B ∈ I
2. **Exchange property:** for A, B ∈ I with |B| = |A|+1, ∃ e ∈ B\A such that A∪{e} ∈ I

**Rado-Edmonds theorem:** Greedy finds the maximum-weight independent set **if and only if** the problem structure is a matroid.

---

### Kruskal's MST Algorithm (Greedy ✓)

```
Graph edges form a matroid (acyclic subsets = independent sets)
→ Greedy works → Kruskal's algorithm is optimal

Algorithm:
1. Sort all edges by weight (ascending)
2. T = ∅
3. FOR each edge e in sorted order:
     IF T ∪ {e} has no cycle:
         T = T ∪ {e}
4. RETURN T  ← Minimum Spanning Tree
```

---

### Continuous vs Discrete Knapsack

```
Continuous Knapsack (greedy ✓):
─────────────────────────────────────────────────
1. Compute density = value / weight for each item
2. Sort by density (descending)
3. Take items greedily; take fraction of last item to fill capacity
→ ALWAYS optimal (matroid structure)

Discrete Knapsack (greedy ✗):
───────────────────────────────────────────────────
Capacity=4, weights=(3,2,2), values=(30,18,17)
Greedy by density: take item 1 (density 10) → value=30, 1 unit left
Optimal: take items 2+3 → value=35
→ Greedy is SUBOPTIMAL → requires Dynamic Programming
```

---

### Huffman Coding

**Goal:** Optimal prefix-free binary code — frequent symbols get shorter codes.

```
Symbol frequencies: a=5, b=9, c=12, d=13, e=16, f=45

Build tree bottom-up (always merge two smallest):

Step 1: merge a(5) + b(9) = ab(14)
Step 2: merge c(12) + d(13) = cd(25)
Step 3: merge ab(14) + e(16) = abe(30)
Step 4: merge cd(25) + abe(30) = cdabe(55)  — wait, skip ahead
...final tree:

              (100)
            /        \
          f(45)     (55)
                  /       \
               (25)       (30)
              /    \      /   \
            c(12) d(13) (14) e(16)
                        / \
                      a(5) b(9)

Codes: f=0 (1 bit), c=100, d=101, a=1100, b=1101, e=111
→ Most frequent symbol f gets shortest code ✓
```

**Complexity:** O(n log n). Always optimal (provably minimises average code length).

---

## 14. Dynamic Programming

### Core Requirements

A problem is suitable for DP when it has:
1. **Optimal substructure:** optimal solution contains optimal sub-solutions
2. **Overlapping sub-problems:** same sub-problem needed multiple times (unlike D&C)

**DP vs Divide & Conquer:**
```
Divide & Conquer:              Dynamic Programming:
  fib(5)                         fib(5)
  ├─ fib(4)                      ├─ fib(4) [computed once, stored]
  │  ├─ fib(3)                   ├─ fib(3) [looked up]
  │  │  ├─ fib(2) ← repeat!      └─ ...
  │  │  └─ fib(1) ← repeat!
  │  └─ fib(2) ← repeat!       Exponential → Linear
  └─ fib(3) ← repeat!
```

---

### Weighted Interval Task Scheduling (WITS)

Sort tasks by finish time. Define:
- **p(j)** = last task compatible with task j (ends before j starts)
- **opt(j)** = optimal value using tasks from {1,...,j}

**Recurrence:**
```
opt(0) = 0
opt(j) = max(  opt(j−1),           ← skip task j
               vⱼ + opt(p(j))  )   ← include task j
```

**Bottom-up fill (O(n)):**
```
M[0] = 0
for j = 1 to n:
    M[j] = max(v_j + M[p(j)], M[j-1])
```

**Reconstruct which tasks to include:**
```
findSolution(j):
    if j == 0: return nothing
    if v_j + M[p(j)] >= M[j-1]:
        return j + findSolution(p(j))    ← include j
    else:
        return findSolution(j-1)          ← skip j
```

---

### 0/1 Knapsack DP

```
Recurrence:
p(0, c) = 0    for all c
p(i, c) = max(  p(i−1, c),              ← skip item i
                p(i−1, c−sᵢ) + vᵢ  )   ← take item i (if c ≥ sᵢ)

Table fill (n=4 items, C=5, values=(10,20,15,20), weights=(1,3,2,4)):

  i↓ c→ │  0   1   2   3   4   5
  ───────┼─────────────────────────
     0   │  0   0   0   0   0   0
  1(s=1) │  0  10  10  10  10  10
  2(s=3) │  0  10  10  20  30  30
  3(s=2) │  0  10  15  20  30  35  ← items 2+3 fill weight 5 → value 35
  4(s=4) │  0  10  15  20  30  35

Answer: p(4,5) = 35
```

> **EXAM TRAP:** O(nC) looks polynomial but C is a NUMBER, not the input size. C=2^1000 needs 2^1000 steps. This is **pseudo-polynomial** — not truly polynomial. Knapsack is still NP-hard.

---

### Key DP Recurrences Summary

```
WITS:         opt(j) = max(vⱼ + opt(p(j)),  opt(j−1))

Knapsack:     p(i,c) = max(p(i−1,c),  p(i−1,c−sᵢ)+vᵢ)

LCS:          c(i,j) = c(i−1,j−1)+1              if xᵢ=yⱼ
                      = max(c(i,j−1), c(i−1,j))  otherwise

Matrix Chain:  m(i,j) = min_{i≤k<j} [m(i,k) + m(k+1,j) + p_{i-1}·pₖ·pⱼ]
```

---

## 15. Local Search

### When Exact Methods Fail

For large NP-hard problems (TSP with 1000 cities, Knapsack with 1000 items), brute force and DP are impractical. **Local search heuristics** find good (not necessarily optimal) solutions quickly.

---

### Neighbourhood Relation

```
N ⊆ S × S  where S = set of all feasible solutions

N(s) = {s' : (s, s') ∈ N}  = neighbours of s

Example (Knapsack): solution = binary vector
Neighbourhood: flip one bit (add or remove one item)
|N(s)| = n  (one option per item)
Diameter = n  (at most n flips to reach any solution from any other)
```

**4 desired properties of a good neighbourhood:**

| Property | Meaning | Ensures |
|----------|---------|---------|
| **Connectedness** | Any solution reachable from any other | Global optimum is reachable |
| **Similarity** | Neighbours differ minimally | Fast, smooth steps |
| **Small neighbourhood** | \|N(s)\| polynomial | Fast step selection |
| **Small diameter** | Short paths in N | Total iterations bounded |

---

### Hill Climbing

```
s = random_initial_solution()
REPEAT:
    best_neighbour = argmax_{s' ∈ N(s)} f(s')
    IF f(best_neighbour) > f(s):
        s = best_neighbour
    ELSE:
        RETURN s    ← local optimum (stuck!)
```

**Problems:**
```
Objective f
    │        local optimum   global optimum
    │        ↓               ↓
    │    ╭───╮              ╭───╮
    │  ╭─╯   ╰─╮          ╭╯   ╰╮
    │╭─╯        ╰─────────╯     ╰─
    └─────────────────────────────── Solution space
    ↑
 start → HC gets stuck at local optimum, never reaches global
```

**Fixes:** random restarts, k-bundle search (top-k solutions maintained)

---

### Simulated Annealing (SA)

Inspired by **annealing in metallurgy** — slow cooling allows atoms to find low-energy states.

**Key idea:** SA sometimes accepts **worse** solutions, with probability that decreases over time.

```
s = random_initial_solution()
T = T_start    (high temperature)
best = s

WHILE T > T_min:
    s_candidate = random element of N(s)
    Δ = f(s_candidate) − f(s)

    IF Δ > 0:                               ← improvement: always accept
        s = s_candidate
    ELSE:                                   ← worsening: accept with probability
        accept with probability exp(Δ/T)

    IF f(s) > f(best): best = s
    T = α · T                               ← cool down (geometric schedule)

RETURN best  (not current s — they may differ!)
```

---

### SA Acceptance Probability

```
P(accept) = exp(Δ/T)    where Δ = f(new) − f(old) < 0  (worsening move)

High T:  exp(Δ/T) ≈ exp(0) ≈ 1    → almost always accept bad moves → EXPLORE
Low T:   exp(Δ/T) ≈ exp(-∞) ≈ 0   → almost never accept bad moves → EXPLOIT
```

**Cooling schedules:**

| Schedule | Formula | Notes |
|----------|---------|-------|
| **Geometric** | T = α·T (α=0.95 typical) | Most common in practice ✓ |
| Linear | T = T − δ | Simple but less smooth |
| Logarithmic | T = T₀/log(1+t) | Theoretically optimal, too slow |

> **EXAM TRAP:** SA returns `best_seen_solution`, NOT `current_solution`. The algorithm wanders — the current position may be worse than the best found during the search.

---

## 16. Formula Cheatsheet

### Distances

| Formula | Name |
|---------|------|
| `√(Σᵢ(xᵢ−yᵢ)²)` | Euclidean distance |
| `Σᵢ\|xᵢ−yᵢ\|` | Manhattan distance |

### Perceptron & Neural Networks

| Formula | Name |
|---------|------|
| `net = WᵀX − θ` | Net activation value |
| `y = 1 if net≥0` | Step activation |
| `σ = 1/(1+e^{−net})` | Sigmoid (unipolar) |
| `σ' = σ(1−σ) = y(1−y)` | Sigmoid derivative |
| `W' = W + α(d−y)X` | Delta rule (step) |
| `W' = W + α(d−y)·y(1−y)·X` | Delta rule (sigmoid) |
| `θ' = θ − α(d−y)` | Threshold update |
| `δzₖ = (dₖ−zₖ)·zₖ·(1−zₖ)` | Output error signal |
| `δyⱼ = yⱼ(1−yⱼ)·Σₖ δzₖ·wₖⱼ` | Hidden error signal (backprop) |
| `W' = W + η·δz·yᵀ` | Matrix weight update |

### Naive Bayes

| Formula | Name |
|---------|------|
| `P(y\|x) ∝ P(y)·Πᵢ P(xᵢ\|y)` | Classification rule |
| `(count(xᵢ,y)+1) / (count(y)+d)` | Laplace smoothing |

### Decision Trees

| Formula | Name |
|---------|------|
| `H(S) = −Σ_c p_c·log₂(p_c)` | Entropy |
| `H(S,A) = Σᵥ (\|Sᵥ\|/\|S\|)·H(Sᵥ)` | Weighted entropy |
| `Gain(S,A) = H(S) − H(S,A)` | Information gain |
| `RSS = Σ(yᵢ−ŷ)²` | Residual sum of squares |
| `C_α(T) = Σ_leaves RSS + α·\|T\|` | Cost-complexity (pruning) |

### Classifier Evaluation

| Formula | Name |
|---------|------|
| `(TP+TN)/(TP+TN+FP+FN)` | Accuracy |
| `TP/(TP+FP)` | Precision |
| `TP/(TP+FN)` | Recall |
| `2·P·R/(P+R)` | F1 score |

### Clustering & PCA

| Formula | Name |
|---------|------|
| `Σₖ Σᵢ∈Cₖ ‖xᵢ−μₖ‖²` | WCSS (k-Means objective) |
| `Z₁ = Σⱼ φⱼ₁·Xⱼ,  Σⱼφⱼ₁²=1` | First principal component |
| `PVE_m = Σᵢzᵢₘ² / Σⱼ Σᵢxᵢⱼ²` | Proportion of variance explained |

### Optimisation & DP

| Formula | Name |
|---------|------|
| `exp(Δ/T)` | SA acceptance probability |
| `opt(j) = max(vⱼ+opt(p(j)), opt(j−1))` | WITS recurrence |
| `p(i,c) = max(p(i−1,c), p(i−1,c−sᵢ)+vᵢ)` | Knapsack DP |
| `c(i,j) = c(i−1,j−1)+1 if xᵢ=yⱼ else max(c(i,j−1),c(i−1,j))` | LCS DP |

---

## Key Exam Traps Summary

| Trap | Correct Answer |
|------|---------------|
| Classification vs Regression | Determined by **output** type (category vs number) |
| k-NN training phase | **There is none** — lazy learner |
| Feature scaling in k-NN | **Critical** — unscaled features dominate |
| XOR and perceptron | **Impossible** with single perceptron — not linearly separable |
| Accuracy on imbalanced data | **Misleading** — use Precision, Recall, F1 |
| Zero probability in Naive Bayes | Use **Laplace smoothing** |
| Random Forests vs Bagging | RF **decorrelates** trees via random feature selection |
| Boosting overfitting | Unlike bagging, boosting **can** overfit as B grows |
| Centroid linkage | Can produce **inversions** in dendrogram — avoid! |
| PCA and scaling | **Always standardise** when features have different units |
| SA returns current vs best | SA returns **best seen**, not current position |
| Knapsack DP complexity | **Pseudo-polynomial** O(nC), not truly polynomial |
| Greedy on Discrete Knapsack | **Fails** — greedy is suboptimal |
| Greedy on MST (Kruskal) | **Optimal** — graph matroids guarantee this |

---

*Compiled from lecture slides by Marcin Sydow (NAI 2026): mlIntro2, perceptron3-en, multiClassNN-en, multiLayerNN-en, naiveBayes-notes-en, trees8, unsupervised10, optTechniques, greedy, dynamic-23-en, localSearch, knowledgeRepresentation-en.*
