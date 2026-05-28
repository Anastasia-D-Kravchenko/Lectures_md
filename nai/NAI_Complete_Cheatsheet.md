# NAI — Complete Theory Cheatsheet

---

## PART I — MACHINE LEARNING

---

### What is ML / Decision Table / Attribute Types

**Machine Learning** = teaching a computer to find patterns from examples, without programming every rule manually.

**Decision table** = your training data as a table. Each row = one example. Each column = one attribute (feature). Last column = the answer (decision attribute — what you want to predict).

**Attribute types:**
- **Numerical** = a real number. Example: temperature = 23.5°C, age = 21
- **Nominal/Categorical** = a named category. Example: weather = sunny/rainy/cloudy, colour = red/blue

---

### Supervised vs Unsupervised

**Supervised** = every training example comes with the correct answer (label). The model learns to map inputs → outputs.
Examples: spam detection, price prediction, digit recognition.

**Unsupervised** = no labels, no correct answers. The model finds hidden structure on its own.
Examples: customer grouping (clustering), anomaly detection.

> Memory trick: Supervised = teacher gives you the answers. Unsupervised = figure it out yourself.

---

### Classification vs Regression

Both are **supervised**. The difference is what you're predicting:

- **Classification** → output is a **category/label** (nominal). "Is this spam?" → yes/no
- **Regression** → output is a **number** (numerical). "What will the price be?" → €234,000

**The output type determines which it is — not the input type.** Both can have the same inputs.

---

### Overfitting

**What it is:** The model memorises training data including its noise and quirks. It works perfectly on training data but fails on new data — it didn't learn the general pattern, just the specific examples.

> Like studying by memorising exact exam questions instead of understanding the topic. Works on the old exam, fails on a new one.

**How to avoid it:**
1. Simpler model (less complexity)
2. More training data
3. Cross-validation
4. Regularisation

---

### Train/Test Split

**Why:** If you test on the same data you trained on, the score is a lie — the model already saw those examples. The test set simulates new unseen data and gives an honest score.

Rule: train on training set, evaluate only on test set — **never mix**.

---

### K-Fold Cross-Validation

**Problem with one split:** you might get unlucky with which examples end up in the test set.

**K-fold solution:**
1. Split data into K equal chunks (folds)
2. For fold i: train on all other K−1 folds, test on fold i
3. Do this K times (each fold is the test set exactly once)
4. Average the K scores → reliable estimate

Every example is tested exactly once. Common choice: K = 5 or K = 10.

**Stratification** = make sure each fold has the same class proportions as the full dataset. Important when one class is rare — otherwise some folds might have zero examples of it.

---

### Perceptron

The simplest neuron. Takes n inputs, multiplies each by a weight, sums them up, subtracts a threshold, applies an activation.

**Net value:**
```
net = w1·x1 + w2·x2 + ... + wn·xn − θ
```

**Step activation:**
```
y = 1   if net ≥ 0
y = 0   otherwise
```

**Threshold θ:** controls how easy it is to fire. High θ = harder to activate. Low θ = easier.

---

### Delta Rule

When the perceptron makes a mistake → update weights and threshold.

```
Error = d − y        (desired minus actual output)

wi' = wi + α · (d−y) · xi     ← each weight
θ'  = θ  − α · (d−y)          ← threshold (NOTE: minus, not plus)
```

α = learning rate (e.g. 0.1)

**What the error means:**

| Error (d−y) | Situation | Effect |
|---|---|---|
| 0 | Correct | No change |
| +1 | Should fire, didn't | Weights increase toward x; θ decreases (easier to fire) |
| −1 | Fired, shouldn't | Weights decrease away from x; θ increases (harder to fire) |

---

### Geometric Interpretation of Perceptron

The perceptron draws a **hyperplane** (a line in 2D, a flat surface in 3D) in input space:
```
w1·x1 + w2·x2 + ... + wn·xn = θ
```

- **Weight vector w** = the normal vector — perpendicular to the hyperplane, pointing toward the "fire" side
- Points on the **w-side** → y = 1
- Points on the **other side** → y = 0

**Delta rule geometrically:** each mistake rotates and shifts the hyperplane to correctly place the current example on the right side.

---

### Linear Separability + XOR

**Linearly separable** = you can draw a single straight line (hyperplane) that perfectly separates all positives from all negatives.

**A perceptron can ONLY classify linearly separable data.**

**XOR is not linearly separable:**

| x1 | x2 | XOR |
|---|---|---|
| 0 | 0 | 0 |
| 0 | 1 | 1 |
| 1 | 0 | 1 |
| 1 | 1 | 0 |

The two 1s are at diagonal corners — no single line can separate them from the two 0s. The delta rule will loop forever and never converge.

---

### Activation Functions

| Name | Formula | Type | Output range |
|---|---|---|---|
| **Step (unipolar)** | y = 1 if net ≥ 0, else 0 | Discrete | {0, 1} |
| **Sign (bipolar)** | y = +1 if net ≥ 0, else −1 | Discrete | {−1, +1} |
| **Sigmoid (unipolar)** | y = 1 / (1 + e^(−net)) | Continuous | (0, 1) |
| **Sigmoid (bipolar)** | y = 2 / (1 + e^(−net)) − 1 | Continuous | (−1, 1) |
| **Linear** | y = net | Continuous | (−∞, +∞) |

---

### Why Sigmoid Over Step

**Step is not differentiable** — its gradient is 0 everywhere (undefined at 0). Gradient descent needs a gradient to know which direction to update weights.

**Sigmoid is smooth and differentiable everywhere.** Its derivative:
```
f'(net) = y · (1 − y)
```
Computed directly from the output — no extra calculation needed.

**Behaviour:**
- y ≈ 0.5 (uncertain) → derivative = 0.25 → **largest update** (learn most when unsure)
- y ≈ 0 or 1 (confident) → derivative → 0 → **tiny update** (don't disturb confident predictions)

---

### Steepness Parameter λ

```
y = 1 / (1 + e^(−λ · net))
```

λ controls the slope of the sigmoid:
- Small λ → gentle slope → soft transition
- Large λ → steep slope → sharp transition
- As λ → ∞ → sigmoid becomes exactly the step function

---

### Local vs Global Representation

Both are ways to encode K output classes in a single-layer network.

**Local representation:**
- 1 neuron per class → **K neurons** total
- Each neuron fires only for its class
- Classification = neuron with highest activation (winner-takes-all)
- Only uninterpretable case: all neurons fire equally

**Global representation:**
- Classes encoded as binary patterns → **⌈log₂K⌉ neurons** total
- Example: 8 classes → 3 neurons (000, 001, 010, ..., 111)
- Fewer neurons but harder to train and interpret

> Local = obvious and readable. Global = compact but cryptic.

---

### Backpropagation

Trains a multi-layer network. Two passes per example:

**Forward pass** (compute outputs):
```
y = sigmoid(V · x)     ← hidden layer output
z = sigmoid(W · y)     ← final output
```

**Backward pass** (compute error signals):
```
δz = (d − z) ⊙ z ⊙ (1 − z)         ← output layer error signal
δy = y ⊙ (1 − y) ⊙ (W^T · δz)      ← hidden layer error signal
```

**Update weights — OUTPUT WEIGHTS FIRST:**
```
W += η · δz · y^T    ← output weights (FIRST)
V += η · δy · x^T    ← hidden weights (second)
```

**Why W first?** δy is computed **using W**. If you update W before computing δy, you get wrong gradients. Always compute all error signals with the OLD weights, then update everything.

Accumulate error E over all examples. If E ≤ E_max → stop. Otherwise reset E = 0 and repeat.

---

### Universal Approximation Theorem

**Statement:** A neural network with **one hidden layer** and enough neurons can approximate any continuous function to any desired accuracy.

**What it does NOT say:**
- It doesn't say how many neurons you need
- It doesn't guarantee gradient descent will find that solution
- It's an existence result — the solution exists, finding it is another problem

---

### Naive Bayes

**Classification rule:**
```
y* = argmax over y:  P(Y=y) · P(X1=x1|Y=y) · P(X2=x2|Y=y) · ... · P(Xn=xn|Y=y)
```
Pick the class with the highest product of prior × all feature likelihoods.

**The "naive" assumption:** all attributes are conditionally independent given the class:
```
P(X1, X2, ..., Xn | Y=y) = P(X1|Y=y) · P(X2|Y=y) · ... · P(Xn|Y=y)
```
Almost never true in reality — but it still works surprisingly well.

**Estimate from training data:**
```
P(Y=y)        = count(Y=y) / N
P(Xi=xi|Y=y)  = count(Xi=xi AND Y=y) / count(Y=y)
```

---

### Laplace Smoothing

**Problem:** if a (feature value, class) pair never appeared in training → P = 0 → the entire product = 0, no matter how strong the other evidence. One zero kills everything.

**Fix — add pseudocounts:**
```
P(Xi=xi | Y=y) = (count(xi, y) + 1) / (count(y) + d)
```
where **d = number of distinct values of attribute Xi in that class's training examples**.

**Rule about d:** if the observed value never appeared in the training data at all (not in any class), then d is NOT increased for that value.

**When to use:** whenever count(xi, y) = 0 for any feature in the observation being classified.

---

### k-NN (k-Nearest Neighbours)

**Algorithm:**
1. Store all training examples (no actual training step)
2. For a new point: compute Euclidean distance to every training point
3. Find the k closest (nearest neighbours)
4. **Majority vote** among their classes → that's the prediction

**Lazy learner** = no training phase. All work happens at prediction time. Fast to "train" (just store), slow to predict.

**Euclidean distance:**
```
d(x, y) = sqrt( (x1−y1)² + (x2−y2)² + ... + (xn−yn)² )
```

**Curse of dimensionality:** in very high-dimensional spaces, all points become approximately equidistant. "Nearest neighbour" loses meaning. k-NN degrades badly in high dimensions.

**Effect of k:**
- k too small → overfitting (one noisy neighbour flips the result)
- k too large → underfitting (boundary too smooth, misses local patterns)
- Choose k using cross-validation

---

### Decision Trees

**Structure:** Internal nodes ask "what is the value of attribute X?" Branches = possible values. Leaves = class labels. To classify: follow branches from root to leaf.

**ID3 algorithm — how to BUILD the tree:**
1. If all examples are the same class → make a **leaf** with that class
2. If no attributes left → make a **leaf** with the majority class
3. Otherwise: pick the attribute with the **highest information gain**
4. Split examples by that attribute's values, recurse on each group

---

### Entropy

"How messy/uncertain is this bag of examples?"
```
H(S) = −Σ pc · log₂(pc)
```

- **H = 0** → perfectly pure (all one class). No uncertainty.
- **H = 1** → perfectly 50/50 binary split. Maximum uncertainty.
- **Max = log₂(K)** → K classes perfectly balanced.
- Always ≥ 0. More mixed = higher entropy.

Example: {spam, spam, spam} → H = 0. {spam, not-spam} → H = 1.

---

### Information Gain

"How much does splitting on attribute A reduce the mess?"
```
Gain(S, A) = H(S) − Σv ( |Sv|/|S| ) · H(Sv)
```

Before split: H(S). After split on A: weighted average entropy of each resulting group. Gain = the reduction. **ID3 picks the attribute with the highest gain at every node.**

---

### Decision Tree Hyperparameters

Settings chosen **before** training — not learned from data:
1. **Max depth** — how deep the tree grows. Deeper = more complex = higher overfitting risk.
2. **Min examples per leaf** — a node stops splitting when fewer than this many examples remain.

---

### Clustering — WCV

**Goal:** put similar things together, dissimilar things apart.

**WCV = Within-Cluster Variation** — the quantity clustering algorithms minimise:
```
Σk  Σ(i in cluster k)  ||xi − c̄k||²
```
= sum of squared distances from every point to its cluster centroid.
Small WCV = tight compact clusters = good clustering.

---

### k-Means

**Think:** assign → update → repeat until stable.

1. Randomly assign each point to one of K clusters
2. **Assign:** each point goes to the nearest centroid
3. **Update:** recompute each centroid = mean of all points in that cluster
4. Repeat 2–3 until assignments don't change

**Centroid formula:**
```
c̄k = (1 / |Ck|) · Σ xi   for all xi in cluster k
```

**Why run multiple times?** Random starting assignments can lead to different local minima. Run many times, keep the result with the lowest WCV.

---

### Hierarchical Agglomerative Clustering

**Think:** start with everyone alone, keep merging the two most similar groups.

1. Each point starts as its own cluster (N clusters)
2. Find the two most similar clusters → merge them into one
3. Repeat until only 1 cluster remains
4. Draw the full history as a **dendrogram**

**Dendrogram:** height = dissimilarity at the time of merge. Low merge = similar. Cut at any height to get any number of clusters — no need to decide K in advance.

---

### 4 Linkage Types

How to measure distance between two clusters:

| Linkage | Distance = | Result |
|---|---|---|
| **Complete** | Max pairwise distance between all point pairs | Compact, equal-sized clusters ✓ |
| **Single** | Min pairwise distance between all point pairs | Can cause chaining ✗ |
| **Average** | Mean of all pairwise distances | Good compromise ✓ |
| **Centroid** | Distance between the two centroids | **AVOID** |

**Why avoid centroid?** It causes **inversions** — a later merge happens at a LOWER height than an earlier one. The dendrogram becomes unreadable.

---

### Confusion Matrix

|  | Predicted + | Predicted − |
|---|---|---|
| **Actually +** | TP (correct positive) | FN (missed it) |
| **Actually −** | FP (false alarm) | TN (correct negative) |

**Memory trick:**
- TP / TN = True = correct prediction
- FP = predicted Positive but WRONG (false alarm)
- FN = predicted Negative but WRONG (missed a real positive)

---

### Accuracy, Precision, Recall, F-measure

```
Accuracy  = (TP + TN) / (TP + TN + FP + FN)   ← overall % correct

Precision = TP / (TP + FP)    ← of all I said "positive", how many were right?

Recall    = TP / (TP + FN)    ← of all actual positives, how many did I find?

F-measure = 2 · Precision · Recall / (Precision + Recall)   ← harmonic mean
```

**Interpret:**
- **Low precision** = lots of false alarms (crying wolf)
- **Low recall** = missing many real positives (doctor missing sick patients)
- **F-measure** balances both — if either is low, F is low

---
---

## PART II — DISCRETE OPTIMISATION

---

### Combinatorial Optimisation Problem

Any such problem must specify exactly 3 things:
1. **Input** — what data is given (the problem instance)
2. **Feasibility** — what makes a solution valid (constraints that must be satisfied)
3. **Optimality** — what to maximise or minimise (the objective)

---

### Knapsack Problem

- **Input:** n items, each with weight wᵢ and value vᵢ; knapsack capacity C
- **Feasibility:** total weight of chosen items ≤ C
- **Optimality:** maximise total value
- **Solution space:** 2ⁿ subsets (each item is either in or out)

---

### TSP (Travelling Salesperson Problem)

- **Input:** n cities with distances between every pair
- **Feasibility:** Hamiltonian cycle — visit every city exactly once and return to start
- **Optimality:** minimise total route distance
- **Solution space:** (n−1)!/2 routes

---

### Brute Force — Knapsack

1. Generate all 2ⁿ possible subsets of n items
2. For each subset: check if total weight ≤ C
3. Return the feasible subset with the maximum total value

**Complexity: O(2ⁿ · n)** — completely impractical for n > 25

---

### Brute Force — TSP

1. Fix one city as the starting point
2. Generate all (n−1)! orderings of the remaining cities
3. For each ordering: compute the total route length (including return to start)
4. Return the ordering with the minimum length

**Complexity: O((n−1)! · n)** — completely impractical for n > 15

---

### Greedy Algorithm

**Idea:** always pick the locally best option right now, never look back or reconsider.

**Pros:**
- Very fast — typically O(n log n)
- Simple to implement
- Provably optimal for matroid-structured problems (MST, Huffman)

**Cons:**
- Not globally optimal in general
- Cannot undo past decisions
- No quality guarantee for non-matroid problems

---

### Greedy for Knapsack

**Heuristic:** sort items by density = vᵢ/wᵢ (value per unit weight), take items in order.

**When it FAILS — counterexample:**

Capacity = 4

| Item | Weight | Value | Density |
|---|---|---|---|
| 1 | 3 | 30 | 10.0 |
| 2 | 2 | 18 | 9.0 |
| 3 | 2 | 17 | 8.5 |

Greedy picks item 1 (highest density) → value = 30, weight = 3 (no room for anything else).
Optimal: items 2 + 3 → value = **35**, weight = 4. **Greedy loses.**

---

### Huffman Coding

**Goal:** assign shorter bit codes to more frequent symbols → fewer total bits needed.

**Algorithm:**
1. Sort all symbols by frequency (lowest first)
2. Take the **two lowest** frequency nodes → merge into one parent node (frequency = sum of both)
3. Put the parent back in the queue → repeat until only 1 node (the root) remains
4. Code for each symbol = path from root to that leaf (left = 0, right = 1)

**Prefix-free code:** no codeword is the beginning of another → can decode without separators between symbols.

**Total encoded length:**
```
L = Σ  freq(symbol) × code_length(symbol)
```

---

### Matroid

A structure M = (E, I) — a ground set E and a family I of "independent" subsets — satisfying two axioms:

1. **Hereditary:** any subset of an independent set is also independent. If a set works, removing elements from it still works.
2. **Exchange:** if |B| = |A| + 1 and both A, B are independent, then there exists some element e in B but not in A such that A ∪ {e} is also independent.

**Why it matters:** the greedy algorithm provably finds the optimal solution **only for matroids**.

---

### Rado-Edmonds Theorem

**Greedy is optimal ↔ the problem structure is a matroid.**

- If the problem IS a matroid → greedy always finds the maximum-weight independent set.
- If it is NOT a matroid → there exists a weight assignment that makes greedy fail.

This is why greedy works perfectly for MST and Huffman (both matroids), but fails for Knapsack (not a matroid).

---

### Local Search & Neighbourhood N(s)

**Idea:** start with any solution, keep moving to a better neighbour, repeat.

**N(s)** = the set of all solutions reachable from solution s in one step.
Examples: flip one bit (Knapsack), swap two cities (TSP).

---

### 4 Properties of Good Neighbourhood

| Property | Meaning | Why it matters |
|---|---|---|
| **Connected** | From any solution you can reach any other solution (including the global optimum) | Otherwise the optimum may be unreachable |
| **Small** | \|N(s)\| is polynomial in input size | Can evaluate all neighbours efficiently |
| **Similar** | Neighbours differ only slightly; objective change is small and computable in O(1) | Smooth search landscape, fast evaluation |
| **Small diameter** | Any two solutions are at most polynomial steps apart | Optimum reachable in bounded number of iterations |

**Example — flip-one-bit for Knapsack:**
- Connected: any solution reachable from any other in ≤ n flips ✓
- Small: |N(s)| = n (one flip per item) ✓
- Similar: one flip changes objective by ±vᵢ, computed in O(1) ✓
- Small diameter: any two solutions differ in ≤ n bits ✓

---

### Hill Climbing

Move to the best neighbour → repeat → stop when no neighbour is better.

**3 problems:**
1. **Local optima** — stuck at a point better than all neighbours but not globally best
2. **Plateaux** — flat region, all neighbours equal, no direction to improve
3. **Ridges** — narrow high-value path that requires sideways moves before going up

**Solutions:** random restarts (run from many different starting points, keep best result); k-bundle (maintain top-k solutions running in parallel).

---

### Simulated Annealing

**Core idea:** like hill climbing but sometimes accept a WORSE solution to escape local optima — with decreasing probability over time.

**Acceptance rule:**
```
Always accept if new solution is better (Δf > 0).
Accept worse move with probability:  P = exp(Δf / T)
   where  Δf = f(new) − f(current) < 0   (negative = worse)
          T  = current temperature (positive)
```

- High T → P ≈ 1 → accepts almost anything → **explores freely**
- Low T → P ≈ 0 → only accepts improvements → **behaves like hill climbing**

**Geometric cooling:**
```
T = α · T    each step,   α ∈ (0.8, 0.99)
```

**Returns:** the **best solution seen during the entire run**, NOT the current solution. The current solution may have gotten worse due to accepted bad moves — the best-seen record is tracked separately throughout.

**Stopping conditions:** T < T_min; no accepted move in last K steps; max iterations reached.

---

## QUICK FORMULA REFERENCE

| Topic | Formula |
|---|---|
| Euclidean distance | `sqrt( Σ (xi−yi)² )` |
| Perceptron net | `Σ wi·xi − θ` |
| Delta rule (weight) | `wi' = wi + α(d−y)·xi` |
| Delta rule (threshold) | `θ' = θ − α(d−y)` |
| Sigmoid | `y = 1 / (1 + e^(−net))` |
| Sigmoid derivative | `f'(net) = y · (1−y)` |
| Local representation | K neurons for K classes |
| Global representation | ⌈log₂K⌉ neurons for K classes |
| Backprop δz | `(d−z) ⊙ z ⊙ (1−z)` |
| Backprop δy | `y ⊙ (1−y) ⊙ (W^T · δz)` |
| Entropy | `H(S) = −Σ pc · log₂(pc)` |
| Information gain | `H(S) − Σv (|Sv|/|S|)·H(Sv)` |
| WCV | `Σk Σi ||xi − c̄k||²` |
| Accuracy | `(TP+TN) / total` |
| Precision | `TP / (TP+FP)` |
| Recall | `TP / (TP+FN)` |
| F-measure | `2·P·R / (P+R)` |
| Naive Bayes | `P(y) · Π P(xi|y)` |
| Laplace smoothing | `(count+1) / (total+d)` |
| Knapsack solution space | `2ⁿ subsets` |
| TSP solution space | `(n−1)!/2 routes` |
| Greedy density | `density = vᵢ / wᵢ` |
| Huffman total length | `L = Σ freq × code_length` |
| SA acceptance | `P = exp(Δf / T)` |
| Geometric cooling | `T = α · T,  α ∈ (0.8, 0.99)` |
