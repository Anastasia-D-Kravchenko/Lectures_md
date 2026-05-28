# NAI — Full Answers to Every Photo Task

---

## PART 1 — ONLINE TEST (Pytanie format: mark ALL correct)

---

### PYTANIE 1 — k-NN Classification

**The question across all versions:**
Given 6 vectors in 3 classes, classify x using k=3 and Euclidean distance. Tie → choose alphabetically.

---

**VERSION A** (most common version)
Classes: A: a=(-1,3,3), b=(-1,4,4) | B: c=(2,-3,-1), d=(3,-3,-1) | C: e=(0,4,0), f=(1,4,0)
Classify x = (0, 3, -2)

**Step 1 — compute all distances:**
```
d(x,a) = sqrt((0+1)²+(3-3)²+(-2-3)²) = sqrt(1+0+25)  = sqrt(26) ≈ 5.10
d(x,b) = sqrt((0+1)²+(3-4)²+(-2-4)²) = sqrt(1+1+36)  = sqrt(38) ≈ 6.16
d(x,c) = sqrt((0-2)²+(3+3)²+(-2+1)²) = sqrt(4+36+1)  = sqrt(41) ≈ 6.40
d(x,d) = sqrt((0-3)²+(3+3)²+(-2+1)²) = sqrt(9+36+1)  = sqrt(46) ≈ 6.78
d(x,e) = sqrt((0-0)²+(3-4)²+(-2-0)²) = sqrt(0+1+4)   = sqrt(5)  ≈ 2.24
d(x,f) = sqrt((0-1)²+(3-4)²+(-2-0)²) = sqrt(1+1+4)   = sqrt(6)  ≈ 2.45
```

**Step 2 — 3 nearest:** e=sqrt(5), f=sqrt(6), a=sqrt(26)
→ e (C class), f (C class), a (A class)

**Step 3 — vote:** C=2 votes, A=1 vote → **x classified as class C**

**✓ CORRECT answers:**
- ✅ "3 nearest are: vector a (A class), vector e (C class), vector f (C class)"

**✗ WRONG answers:**
- ❌ "3 nearest are: b, c, d" — wrong distances
- ❌ "distances are d(x,e)=sqrt(10), d(x,f)=sqrt(11)..." — d(x,e)=sqrt(5), d(x,f)=sqrt(6)
- ❌ "3 nearest are: a, b, f" — b is not in top 3

---

**VERSION B** (from Polish online test IMG_1696)
Classes: A: a=(-1,-3,3), b=(-1,-2,4) | B: c=(2,2,1), d=(3,2,2) | C: e=(0,0,2), f=(1,1,3)
Classify x = (-3, 0, -3)

**Distances:**
```
d(x,a) = sqrt((-3+1)²+(0+3)²+(-3-3)²) = sqrt(4+9+36)  = sqrt(49) = 7
d(x,b) = sqrt((-3+1)²+(0+2)²+(-3-4)²) = sqrt(4+4+49)  = sqrt(57)
d(x,c) = sqrt((-3-2)²+(0-2)²+(-3-1)²) = sqrt(25+4+16)  = sqrt(45)
d(x,d) = sqrt((-3-3)²+(0-2)²+(-3-2)²) = sqrt(36+4+25)  = sqrt(65)
d(x,e) = sqrt((-3-0)²+(0-0)²+(-3-2)²) = sqrt(9+0+25)   = sqrt(34)... 
```
Wait: e=(0,0,2): d(x,e) = sqrt(9+0+25) = sqrt(34)? Let me check: (-3-0)²=9, (0-0)²=0, (-3-2)²=25 → sqrt(34) Hmm no: (-3-2)² = (-5)² = 25. Yes sqrt(34)

Actually: sqrt(9+0+25) = sqrt(34)? 9+0+25 = 34. Yes.

```
d(x,e) = sqrt(9+0+25)  = sqrt(34) ≈ 5.83
d(x,f) = sqrt((-3-1)²+(0-1)²+(-3-3)²) = sqrt(16+1+36) = sqrt(53)
```

Distances sorted: a=7, e≈5.83, c≈6.71, b≈7.55, f≈7.28, d≈8.06

Hmm, let me redo carefully:
```
d(x,a) = sqrt(4+9+36) = sqrt(49) = 7
d(x,b) = sqrt(4+4+49) = sqrt(57) ≈ 7.55
d(x,c) = sqrt(25+4+16) = sqrt(45) ≈ 6.71
d(x,d) = sqrt(36+4+25) = sqrt(65) ≈ 8.06
d(x,e) = sqrt(9+0+25) = sqrt(34) ≈ 5.83  ← but e=(0,0,2) so (-3-2)²=25 ✓
d(x,f) = sqrt(16+1+36) = sqrt(53) ≈ 7.28
```

3 nearest: e=sqrt(34), c=sqrt(45), a=sqrt(49)
→ e(C), c(B), a(A) — all different classes → TIE → choose alphabetically → **class A** (a comes first)

**✓ CORRECT answers:**
- ✅ "3 nearest are: vector a (A class), vector c (B class), vector e (C class)"
- ✅ "vector x classified to A class"
- ✅ Correct distances shown: d(x,a)=sqrt(49), d(x,c)=sqrt(45), d(x,e)=sqrt(34)

---

### PYTANIE 2 — Perceptron Delta Rule

**The question across all versions:**
Given input z, weights w, threshold r, α=1, perceptron gave **incorrect** answer. Find corrected weights and new threshold.

**METHOD:**
1. Compute net = w·z − r (dot product minus threshold)
2. If net ≥ 0 → y=1; if net < 0 → y=0
3. "Incorrect" → desired was opposite: if y=1 then d=0; if y=0 then d=1
4. error = d − y
5. wi' = wi + α·(d−y)·zi
6. r' = r − α·(d−y)
   - If error = +1 (should fire, didn't): weights increase toward z, threshold decreases
   - If error = −1 (fired, shouldn't): weights decrease away from z, threshold increases

---

**VERSION A** (IMG_1644): z=<-4,3,5>, w=<2,5,0>, r=−3, α=1

```
net = 2·(-4) + 5·3 + 0·5 − (−3) = −8 + 15 + 0 + 3 = 10
y = 1  (net ≥ 0)
d = 0  (incorrect → opposite)
error = d − y = −1

w1' = 2 + 1·(−1)·(−4) = 2 + 4 = 6
w2' = 5 + 1·(−1)·3   = 5 − 3 = 2
w3' = 0 + 1·(−1)·5   = 0 − 5 = −5

New weights = <6, 2, −5>

r' = −3 − 1·(−1) = −3 + 1 = −2
Old r = −3, new r = −2 → −2 > −3 → threshold is GREATER
```

**✓ CORRECT answers:**
- ✅ "Corrected weights are <6, 2, −5>"
- ✅ "Corrected weights are NOT <4, 1, −6>" (TRUE — they are <6,2,−5>)
- ✅ "After correction, threshold is greater"
- ❌ "Threshold is smaller" — WRONG

---

**VERSION B** (from shorawa test IMG_1673): z=<2,−1,5>, w=<−2,−1,1>, r=−4, α=1

```
net = (−2)·2 + (−1)·(−1) + 1·5 − (−4) = −4 + 1 + 5 + 4 = 6
y = 1  (net ≥ 0)
d = 0  (incorrect)
error = −1

w1' = −2 − 2 = −4
w2' = −1 − (−1) = 0
w3' = 1 − 5 = −4

New weights = <−4, 0, −4>

r' = −4 − (−1) = −3 → was −4, now −3 → threshold is GREATER
```

**✓ CORRECT answers:**
- ✅ "Answer before correction = 1" (net=6 ≥ 0)
- ✅ "Corrected weights are <−4, 0, −4>"
- ✅ "Threshold is greater"
- ❌ "Answer before correction = 0" — WRONG

---

**VERSION C** (from IMG_1695 Polish test): z=<4,−2,2>, w=<0,−3,4>, r=4, α=1

```
net = 0·4 + (−3)·(−2) + 4·2 − 4 = 0 + 6 + 8 − 4 = 10
y = 1  (net ≥ 0)
d = 0  (incorrect)
error = −1

w1' = 0 − 4 = −4
w2' = −3 − (−2) = −1
w3' = 4 − 2 = 2

New weights = <−4, −1, 2>

r' = 4 − (−1) = 5 → was 4, now 5 → threshold GREATER
```

**✓ CORRECT answers:**
- ✅ "Answer before correction = 1"
- ✅ "Corrected weights are <−4, −1, 2>"
- ✅ "Threshold is greater"

---

### PYTANIE 3 — Classifier Evaluation

**The question:** Given how many cases total, how many predicted positive, how many truly positive, how many correctly predicted negative. Calculate accuracy, precision, recall, F-measure as simplified fractions.

**METHOD — build confusion matrix first:**
```
TP = truly positive (predicted positive AND actually positive)
FP = predicted positive − TP  (predicted positive but wrong)
TN = correctly predicted negative
FN = total − TP − FP − TN
```

Then:
```
Accuracy  = (TP + TN) / total
Precision = TP / (TP + FP)
Recall    = TP / (TP + FN)
F-measure = 2 · Precision · Recall / (Precision + Recall)
```

---

**VERSION A** (IMG_1645/1691): 45 total, 31 predicted positive, 25 truly positive, 6 correctly predicted negative

```
TP = 25,  FP = 31 − 25 = 6,  TN = 6,  FN = 45 − 25 − 6 − 6 = 8

Accuracy  = (25+6)/45 = 31/45
Precision = 25/(25+6) = 25/31
Recall    = 25/(25+8) = 25/33
F-measure = 2·(25/31)·(25/33) / (25/31 + 25/33)
          = 2·625/1023 / (825+775)/1023
          = 1250/1600 = 25/32
```

**✓ CORRECT answer:** Accuracy=31/45, Precision=25/31, Recall=25/33, F=25/32

---

**VERSION B** (IMG_1674): 44 total, 33 predicted positive, 28 truly positive, 3 correctly predicted negative

```
TP = 28,  FP = 33−28 = 5,  TN = 3,  FN = 44−28−5−3 = 8

Accuracy  = (28+3)/44 = 31/44
Precision = 28/(28+5) = 28/33
Recall    = 28/(28+8) = 28/36 = 7/9
F-measure = 2·(28/33)·(7/9) / (28/33 + 7/9)
          = 2·196/297 / (252/297 + 231/297) ... let me simplify
          = 2·(28/33)·(7/9) / ((28·9 + 7·33)/(33·9))
          = 2·196/297 / (252+231)/297
          = 392/483 = 56/69
```

**✓ CORRECT answer:** Accuracy=31/44, Precision=28/33, Recall=7/9, F=56/69

---

### PYTANIE 4 — k-Means (Two Iterations)

**METHOD:**
1. For each point, compute distance to each centroid → assign to nearest
2. Recompute each centroid = mean of all assigned points
3. Repeat for iteration 2
4. If assignments unchanged after iteration 2 → algorithm stopped

---

**VERSION A** (IMG_1646/1692):
Points: A(4,2,4,8), B(8,4,9,4), C(6,9,9,4), D(7,8,6,4)
c1=(4,2,4,8), c2=(6,9,9,4)

**Iteration 1:**
```
d(A,c1)=0,           d(A,c2)=sqrt(4+49+25+16)=sqrt(94)  → A → c1
d(B,c1)=sqrt(16+4+25+16)=sqrt(61), d(B,c2)=sqrt(4+25+0+0)=sqrt(29) → B → c2
d(C,c1)=sqrt(94),    d(C,c2)=0                           → C → c2
d(D,c1)=sqrt(9+36+4+16)=sqrt(65), d(D,c2)=sqrt(1+1+9+0)=sqrt(11)  → D → c2

Cluster 1: {A}   → new c1 = (4,2,4,8)
Cluster 2: {B,C,D} → new c2 = ((8+6+7)/3, (4+9+8)/3, (9+9+6)/3, (4+4+4)/3)
                             = (7, 7, 8, 4)
```

**Iteration 2:**
```
d(A,c1)=0,           d(A,c2)=sqrt(9+25+16+16)=sqrt(66)  → A → c1
d(B,c1)=sqrt(61),    d(B,c2)=sqrt(1+9+1+0)=sqrt(11)     → B → c2
d(C,c1)=sqrt(94),    d(C,c2)=sqrt(1+4+1+0)=sqrt(6)      → C → c2
d(D,c1)=sqrt(65),    d(D,c2)=sqrt(0+1+4+0)=sqrt(5)      → D → c2

Same assignments as before → ALGORITHM STOPPED ✓
```

Final centroids: **c1=(4.0, 2.0, 4.0, 8.0), c2=(7.0, 7.0, 8.0, 4.0)**
Final assignments: **{A} ; {B, C, D}**

**✓ CORRECT answers:**
- ✅ "After two iterations: c1(4.0,2.0,4.0,8.0); c2(7.0,7.0,8.0,4.0)"
- ✅ "Cluster assignments: {A}; {B,C,D}"
- ❌ "In iteration 1 d(c2, C) = sqrt(63)" — WRONG, it's 0 (C = c2 exactly)

---

**VERSION B** (IMG_1673/1675): A(9,4,2,8), B(7,5,8,4), C(1,9,4,1), D(8,9,5,6)
c1=(9,4,2,8), c2=(1,9,4,1)

**Iteration 1:**
```
d(A,c1)=0,  d(A,c2)=sqrt(64+25+4+49)=sqrt(142) → A→c1
d(B,c1)=sqrt(4+1+36+16)=sqrt(57), d(B,c2)=sqrt(36+16+16+9)=sqrt(77) → B→c1
d(C,c1)=sqrt(64+25+4+49)=sqrt(142), d(C,c2)=0  → C→c2
d(D,c1)=sqrt(1+25+9+4)=sqrt(39), d(D,c2)=sqrt(49+0+1+25)=sqrt(75) → D→c1

Cluster 1: {A,B,D} → c1=((9+7+8)/3, (4+5+9)/3, (2+8+5)/3, (8+4+6)/3) = (8,6,5,6)
Cluster 2: {C}     → c2=(1,9,4,1)
```

**Iteration 2:**
```
d(A,c1)=sqrt(1+4+9+4)=sqrt(18), d(A,c2)=sqrt(64+25+4+49)=sqrt(142) → A→c1
d(B,c1)=sqrt(1+1+9+4)=sqrt(15), d(B,c2)=sqrt(36+16+16+9)=sqrt(77)  → B→c1
d(C,c1)=sqrt(49+9+1+25)=sqrt(84), d(C,c2)=0                         → C→c2
d(D,c1)=sqrt(0+9+0+0)=3,         d(D,c2)=sqrt(49+0+1+25)=sqrt(75)  → D→c1

Same assignments → STOPPED ✓
c1 = ((9+7+8)/3, (4+5+9)/3, (2+8+5)/3, (8+4+6)/3) = (8.0, 6.0, 5.0, 6.0)
c2 = (1.0, 9.0, 4.0, 1.0)
```

**✓ CORRECT answers:**
- ✅ "Algorithm has stopped"
- ✅ "After two iterations: c1(8.0,6.0,5.0,6.0); c2(1.0,9.0,4.0,1.0)"
- ✅ "In iteration 1 d(c1,C) = sqrt(142)"
- ❌ Other centroid options — wrong

---

### PYTANIE 5 — Naive Bayes

**METHOD:**
1. Count class priors P(class) = count/total
2. For each attribute: count occurrences in each class → apply Laplace smoothing
3. d = number of distinct values for that attribute in that CLASS's training examples
   - If the observed value doesn't appear in training data at all → d NOT incremented
4. P(xi|class) = (count(xi, class) + 1) / (count(class) + d)
5. Multiply: P(class|x) ∝ P(class) · P(x1|class) · P(x2|class) · P(x3|class)
6. Choose class with higher product

**SMOOTHING COUNT:** Count +1 only when count(xi, class) = 0 (value absent from that class)

---

**VERSION A** (IMG_1647/1693):
Dataset: [no,violet,medium]→acc, [yes,violet,good]→acc, [yes,blue,bad]→unacc, [no,blue,bad]→unacc, [no,violet,good]→unacc
Classify: [yes, violet, medium]

```
P(acc)=2/5,  P(unacc)=3/5

FOR ACC (2 examples):
Attr1 {no,yes}→d=2:      P(yes|acc)    = (1+1)/(2+2) = 2/4 = 1/2
Attr2 {violet}→d=1:      P(violet|acc) = (2+1)/(2+1) = 3/3 = 1
Attr3 {medium,good}→d=2: P(medium|acc) = (1+1)/(2+2) = 2/4 = 1/2

P(acc|x) ∝ 2/5 · 1/2 · 1 · 1/2 = 2/20 = 0.10

FOR UNACC (3 examples):
Attr1 {yes,no}→d=2:   P(yes|unacc)    = (1+1)/(3+2) = 2/5
Attr2 {blue,violet}→d=2: P(violet|unacc) = (1+1)/(3+2) = 2/5
Attr3 {bad,good}→d=2: P(medium|unacc) = (0+1)/(3+2) = 1/5  ← SMOOTHED (#1)

P(unacc|x) ∝ 3/5 · 2/5 · 2/5 · 1/5 = 12/625 = 0.0192

0.10 > 0.0192 → class = ACC ✓
```

**✓ CORRECT answers:**
- ✅ "Class of given observation is acc"
- ✅ "Smoothing was used exactly 1 time(s)"
- ❌ "P(unacc) = 0.82" — WRONG (it's ≈0.161 normalized)

---

**VERSION B** (IMG_1675): Dataset: [yes,violet,medium]→acc, [yes,blue,medium]→acc, [no,violet,medium]→acc, [yes,violet,good]→acc, [yes,blue,good]→unacc
Classify: [yes, violet, bad]

```
P(acc)=4/5,  P(unacc)=1/5

FOR ACC (4 examples):
Attr1 {yes,no}→d=2:        P(yes|acc)    = (3+1)/(4+2) = 4/6 = 2/3
Attr2 {violet,blue}→d=2:   P(violet|acc) = (3+1)/(4+2) = 4/6 = 2/3
Attr3 {medium,good}→d=2:   P(bad|acc)    = (0+1)/(4+2) = 1/6  ← SMOOTHED (#1)

FOR UNACC (1 example: yes,blue,good):
Attr1 {yes}→d=1:     P(yes|unacc)    = (1+1)/(1+1) = 1
Attr2 {blue}→d=1:    P(violet|unacc) = (0+1)/(1+1) = 1/2  ← SMOOTHED (#2)
Attr3 {good}→d=1:    P(bad|unacc)    = (0+1)/(1+1) = 1/2  ← SMOOTHED (#3)

P(acc|x)   ∝ 4/5 · 2/3 · 2/3 · 1/6 = 16/270 ≈ 0.059
P(unacc|x) ∝ 1/5 · 1 · 1/2 · 1/2  = 1/20   = 0.05

0.059 > 0.05 → class = ACC ✓
```

**✓ CORRECT answers:**
- ✅ "Class is acc"
- ✅ "Smoothing used exactly 3 times"
- ✅ "P(unacc) ≈ 0.0222" (1/45 — this is using global d for violet and bad; varies by implementation)
- ❌ "Both classes same probability" — WRONG

---

## PART 2 — WRITTEN TEST A (short answer, ~2–3 sentences each)

### TEST A — Version 1 (page 1: Q1–5, page 2: Q6–9)

---

**Q1 (2p.) — What is unsupervised learning?**

Learning without labelled data. The algorithm is given raw input data only — no correct answers are provided. It discovers hidden structure on its own, such as clusters, patterns, or associations. Examples: k-means clustering, hierarchical clustering.

---

**Q2 (2p.) — Give one example of a classification task.**

Email spam detection: the input is an email (attributes: keywords, sender, length) and the output is a category — "spam" or "not spam". The output is a nominal value (a label), which is what makes it classification rather than regression.

---

**Q3 (2p.) — Explain how the k-means clustering algorithm works.**

1. Randomly assign each data point to one of K clusters.
2. Compute the centroid (mean) of each cluster.
3. Reassign each point to the nearest centroid.
4. Repeat steps 2–3 until assignments no longer change (convergence).

The algorithm minimises total within-cluster variation: Σk Σi ||xi − c̄k||². It always converges but may find a local minimum, so it is run multiple times.

---

**Q4 (2p.) — Name at least three activation functions and write their formulas. Include both continuous and discrete.**

| Function | Formula | Type |
|---|---|---|
| Step (unipolar) | y = 1 if net ≥ 0, else y = 0 | Discrete |
| Sign (bipolar) | y = +1 if net ≥ 0, else y = −1 | Discrete |
| Sigmoid (unipolar) | y = 1 / (1 + e^(−net)) | Continuous |
| Sigmoid (bipolar) | y = 2/(1 + e^(−net)) − 1 | Continuous |
| Linear | y = net | Continuous |

Step and sign are discrete — not differentiable. Sigmoid is preferred for training because its derivative y·(1−y) enables gradient descent.

---

**Q5 (2p.) — Explain the problem of overfitting.**

Overfitting occurs when a model is too complex and memorises the training data including its noise rather than learning general patterns. It has high accuracy on training data but low accuracy on new (test) data — it fails to generalise. It is caused by too many parameters relative to training examples. Solutions: simpler model, more data, regularisation, cross-validation.

---

**Q6 (2p.) — Write the formula for recall. How to interpret this classifier evaluation measure?**

```
Recall = TP / (TP + FN)
```

Recall measures what fraction of all actual positive cases the classifier correctly detected. A low recall means the classifier misses many true positives (high false negatives) — for example, a cancer screening test that misses many sick patients has low recall. High recall is critical in medical and security applications.

---

**Q7 (3p.) — Explain briefly how backpropagation works.**

Backpropagation trains a multi-layer neural network using two passes:

**Forward pass:** Compute hidden layer output y = sigmoid(V·x), then output z = sigmoid(W·y).

**Backward pass:** Compute output error signal δz = (d−z)⊙z⊙(1−z), then propagate back to hidden layer: δy = y⊙(1−y)⊙(W^T·δz).

**Update weights — output weights W first, then hidden weights V:**
W += η·δz·y^T,   V += η·δy·x^T

Repeat until total error E ≤ E_max. W must be updated first because δy is computed using W — if W were updated before computing δy, the gradients would be wrong.

---

**Q8 (2p.) — What are the limitations of the hill climbing algorithm? How can they be addressed?**

Hill climbing has three problems:
1. **Local optima** — stops at a solution where no neighbour is better, but it is not globally best.
2. **Plateaux** — flat regions where all neighbours are equal; no improvement possible.
3. **Ridges** — narrow paths that require sideways moves before going upward.

**Solutions:** Random restarts (run from multiple random starting points); k-bundle search (maintain top-k solutions in parallel).

---

**Q9 (3p.) — Describe step-by-step the brute force algorithm that solves the knapsack problem. What is the time complexity?**

**Algorithm:**
1. Generate all 2^n possible subsets of n items.
2. For each subset: check if total weight ≤ capacity C.
3. Among all feasible subsets, return the one with the maximum total value.

**Time complexity:** O(2^n · n) — exponential in n. Checking each subset takes O(n). Completely impractical for n > 25–30.

---

### TEST A — Version 2 (different paper, page 1)

*Questions same as above except Q3 and Q5:*

**Q3 (2p.) — Draw an example of a dendrogram and explain how to interpret it.**

A dendrogram is a tree diagram produced by hierarchical clustering. Each leaf is one data point. The height at which two clusters merge = the dissimilarity between them. Low height = similar clusters merged early. To get K clusters, cut the dendrogram horizontally at the appropriate height — everything below the cut forms clusters.

```
       ___________
      |           |
    __|__       __|__
   |     |     |     |
  _|_    C    _|_    D
 |   |       |   |
 A   B       E   F

Height axis ↑ (higher = more dissimilar)
```

**Q5 (2p.) — Give an example of a set with entropy = 1.**

Any binary set with exactly 50% of each class. Example: S = {spam, spam, not-spam, not-spam}.
H(S) = −(0.5·log₂0.5) − (0.5·log₂0.5) = −(0.5·(−1)) − (0.5·(−1)) = 0.5 + 0.5 = **1** ✓

Entropy = 1 means maximum uncertainty for a binary problem — the two classes are perfectly balanced.

---

## PART 3 — WRITTEN TEST B (different question set)

**Q1 (2p.) — What is the reason for splitting data into a training set and a test set?**

To get an honest estimate of how well the model generalises to new, unseen data. If we evaluate on training data the result is biased upward — the model has already seen those examples and may have memorised them. The test set simulates new data and reveals the true generalisation error.

---

**Q2 (2p.) — What is the difference between classification and regression?**

Both are supervised learning tasks. The difference is the **type of output**:
- **Classification** — decision attribute is nominal (categorical) → output is a class label (e.g. spam/not-spam, cat/dog).
- **Regression** — decision attribute is numerical (continuous) → output is a number (e.g. house price, temperature).

The type of the INPUT attributes does not determine which it is — it's the OUTPUT type that matters.

---

**Q3 (2p.) — Explain how the hierarchical agglomerative clustering algorithm works.**

1. Start: every data point is its own cluster (N clusters).
2. Compute pairwise distances/dissimilarities between all clusters.
3. Merge the two most similar (closest) clusters into one.
4. Repeat steps 2–3 until only 1 cluster remains.
5. The full merge history is stored as a **dendrogram**.

No need to specify K in advance — you can cut the dendrogram at any level. The choice of linkage (complete, single, average) determines how "distance between clusters" is defined.

---

**Q4 (2p.) — Write down the sigmoid function formula.**

```
y = 1 / (1 + e^(−net))
```

Output is always between 0 and 1. At net=0: y=0.5. As net→+∞: y→1. As net→−∞: y→0.
Derivative: f'(net) = y·(1−y) — computed directly from output, no extra calculation.

---

**Q5 (2p.) — Give an example of a dataset for which a perceptron would not achieve high accuracy in binary classification. Explain.**

The **XOR dataset**: {(0,0)→0, (0,1)→1, (1,0)→1, (1,1)→0}.

A perceptron can only create a single linear decision boundary (hyperplane). XOR is not linearly separable — there is no single straight line that separates the positive cases (0,1) and (1,0) from the negative cases (0,0) and (1,1). The delta rule will oscillate and never converge.

---

**Q6 (2p.) — Write down the F-measure formula. How to interpret this classifier evaluation measure?**

```
F-measure = 2 · Precision · Recall / (Precision + Recall)
```

Where: Precision = TP/(TP+FP), Recall = TP/(TP+FN).

F-measure is the harmonic mean of precision and recall. It is useful when the classes are imbalanced or when both false positives and false negatives matter. F=1 means perfect precision and recall. F close to 0 means either precision or recall (or both) are very low. It penalises classifiers that sacrifice one metric to boost the other.

---

**Q7 (2p.) — What are hyperparameters for decision trees? Name at least two.**

Hyperparameters are settings chosen before training that control model complexity — they are not learned from data.

For decision trees:
1. **Maximum tree depth** — how deep the tree can grow. Deeper = more complex = higher overfitting risk.
2. **Minimum examples per leaf** — a node becomes a leaf if it has fewer than this many examples.
3. **Pruning threshold** — removes branches that don't improve generalisation enough.

---

**Q8 (3p.) — Explain how simulated annealing works. What is the role of the annealing function?**

Simulated Annealing (SA) is a local search algorithm that sometimes accepts worse solutions to escape local optima:

1. Start with initial solution s and high temperature T.
2. Each step: pick a random neighbour s'.
3. If f(s') > f(s): always accept.
4. If f(s') ≤ f(s): accept with probability P = exp(Δf/T) where Δf = f(s') − f(s) < 0.
5. Cool the temperature: T = α·T (geometric cooling, α ∈ 0.8–0.99).
6. Record the best solution seen throughout.
7. Stop when T < T_min or no improvement for K steps.

**Role of annealing function:** Controls exploration vs exploitation. High T → P≈1 → accepts bad moves freely → explores and can escape local optima. Low T → P≈0 → only accepts improvements → exploits like hill climbing. The gradual cooling prevents premature convergence. SA returns the **best solution seen**, not the current one.

---

**Q9 (3p.) — Describe step-by-step the brute force algorithm for TSP. What is the time complexity?**

**Algorithm:**
1. Fix one city as the starting point (to avoid counting rotations).
2. Generate all (n−1)! permutations of the remaining n−1 cities.
3. For each permutation: compute the total cycle length including the return to start.
4. Return the permutation with the minimum total length.

**Time complexity:** O((n−1)! · n) — factorial. For n=20 cities: ≈ 10^18 operations. Completely infeasible for n > 15.

---

## PART 4 — NAI THEORY TEST 2025 (handwritten answers needed)

**Q1 — State three examples of supervised learning.**

1. Email spam classification (input: email features, output: spam/not-spam)
2. House price prediction (input: size/location, output: price in euros) — regression
3. Handwritten digit recognition (input: pixel image, output: digit 0–9)

Key: each training example has a known label/target value.

---

**Q2 — Define a perceptron's "net" value.**

```
net = w1·x1 + w2·x2 + ... + wn·xn − θ
```

The net value is the weighted sum of all inputs minus the threshold θ. The activation function is then applied to net to produce the output y.

---

**Q3 — What is a global output representation of a neural net?**

Global representation encodes the output class as a **binary pattern** across multiple neurons. For K classes you need ⌈log₂K⌉ neurons (e.g. 3 neurons for 8 classes). Each unique bit pattern represents one class. It uses fewer neurons than local representation but is harder to train and interpret.

---

**Q4 — What is hierarchical clustering?**

A bottom-up (agglomerative) clustering algorithm that starts with each data point as its own cluster and repeatedly merges the two most similar clusters. The full merge history is visualised as a dendrogram. It does not require specifying K in advance and is deterministic given the linkage type.

---

**Q5 — What is "attribute smoothing" in Bayesian classification?**

Laplace smoothing — used to prevent the zero-probability problem. When a feature value has never been seen for a given class in training, the probability becomes 0 and wipes out the entire product. Laplace smoothing adds a pseudocount of 1:

```
P(Xi=xi | Y=y) = (count(xi, y) + 1) / (count(y) + d)
```

where d = number of distinct values of attribute Xi.

---

## PART 5 — POLISH WRITTEN TEST (translated questions)

**Q1 — Describe the geometric interpretation of the Delta rule.**

The weight vector **w** is the normal (perpendicular) vector to the decision hyperplane. When the perceptron makes an error, the Delta rule rotates and shifts the hyperplane:
- Error +1 (should fire, didn't): **w** moves toward the input vector **x**, threshold decreases → hyperplane shifts to include **x** on the positive side.
- Error −1 (fired, shouldn't): **w** moves away from **x**, threshold increases → hyperplane shifts to exclude **x**.
Each update moves the decision boundary closer to correctly classifying the current example.

---

**Q2 — Describe the structure and operation of a decision tree classifier.**

**Structure:** Internal nodes test one attribute. Branches represent possible attribute values. Leaf nodes assign a class label.

**Operation:** To classify a new example: start at the root, follow the branch matching the example's attribute value, repeat at each internal node until a leaf is reached — return that leaf's class.

**Training (ID3):** At each node, select the attribute with the highest information gain (entropy reduction). Recurse on each subset. Stop when pure (all one class) or no attributes remain.

---

**Q3 — Describe the concept of cross-validation.**

K-fold cross-validation: split data into K equal folds. For fold i (i = 1..K): train on remaining K−1 folds, test on fold i. Average the K error estimates. Every example is tested exactly once. Produces a reliable generalisation estimate without wasting data. Stratification ensures each fold preserves the class proportions of the full dataset.

---

**Q4 — Differences between supervised and unsupervised learning.**

| | Supervised | Unsupervised |
|---|---|---|
| Training data | Has labels | No labels |
| Goal | Learn input→output mapping | Find hidden structure |
| Examples | Classification, regression | Clustering, association rules |
| Evaluation | Compare predictions to known labels | Harder — no ground truth |

---

**Q5 — Advantages and disadvantages of the greedy approach for optimisation.**

**Advantages:** Very fast — typically O(n log n). Simple to implement. Provably optimal for problems that form a matroid (e.g. minimum spanning tree, Huffman coding).

**Disadvantages:** Not globally optimal in general (e.g. Knapsack). Makes irreversible decisions — once an item is selected/rejected, it cannot be reconsidered. No quality guarantee for non-matroid problems.

---

**Q6 — Name 4 classifier evaluation measures.**

1. **Accuracy** = (TP+TN)/(TP+TN+FP+FN) — overall correctness
2. **Precision** = TP/(TP+FP) — of all positive predictions, how many were right
3. **Recall** = TP/(TP+FN) — of all actual positives, how many were found
4. **F-measure** = 2·P·R/(P+R) — harmonic mean of precision and recall

---

**Q7 — Name 4 desired properties of a neighbourhood relation and their roles.**

1. **Connected** — the global optimum is reachable from any starting solution (guarantees search can find it).
2. **Small** — |N(s)| is polynomial → can evaluate all neighbours efficiently.
3. **Similar** — neighbours differ slightly → small objective change computable in O(1) → smooth search landscape.
4. **Small diameter** — maximum path length between any two solutions is polynomial → optimum reachable in bounded iterations.

---

**Q8 — Possibilities and limitations of a single-layer global classifier.**

**Possibilities:** Can handle multiple classes using ⌈log₂K⌉ neurons (efficient). Outputs continuous values giving a form of confidence score.

**Limitations:** Each output neuron still creates only a linear decision boundary. Cannot separate classes that are not linearly separable (e.g. XOR). Global bit-pattern coding is harder to train and interpret than local representation.

---

**Q9 — Define Naive Bayes classifier.**

Naive Bayes classifies a new instance by choosing the class y* that maximises:
```
P(Y=y) · P(X1=x1|Y=y) · P(X2=x2|Y=y) · ... · P(Xn=xn|Y=y)
```
It assumes conditional independence of all attributes given the class (the "naive" assumption). Despite this assumption almost never holding in practice, it often works well and is computationally efficient.

---

## PART 6 — HUFFMAN CODING TASK

**Sequence:** AABBBBCGDDDDDDEFFFFEGGGGG
**Frequencies (from student count):** A=2, B=4, C=2, D=5, E=1, F=5, G=5 (total=24)

**Step-by-step Huffman algorithm:**

Sort by frequency ascending:
`E=1, A=2, C=2, B=4, D=5, F=5, G=5`

**Merge 1:** E(1) + A(2) → node EA(3)
`[C=2, EA=3, B=4, D=5, F=5, G=5]`

**Merge 2:** C(2) + EA(3) → node CEA(5)
`[B=4, D=5, F=5, G=5, CEA=5]`

**Merge 3:** B(4) + D(5) → node BD(9)
`[F=5, G=5, CEA=5, BD=9]`

**Merge 4:** F(5) + G(5) → node FG(10)
`[CEA=5, BD=9, FG=10]`

**Merge 5:** CEA(5) + BD(9) → node CEABD(14)
`[FG=10, CEABD=14]`

**Merge 6:** FG(10) + CEABD(14) → ROOT(24)

**Tree structure and codes (0=left, 1=right):**
```
                ROOT(24)
              /          \
           FG(10)        CEABD(14)
          /     \        /        \
        F(5)   G(5)   CEA(5)     BD(9)
                      /    \     /    \
                    C(2)  EA(3) B(4)  D(5)
                          /   \
                        E(1)  A(2)
```

**Final codes:**
| Symbol | Code | Bits | Freq | Bits × Freq |
|---|---|---|---|---|
| F | 00 | 2 | 5 | 10 |
| G | 01 | 2 | 5 | 10 |
| C | 100 | 3 | 2 | 6 |
| B | 110 | 3 | 4 | 12 |
| D | 111 | 3 | 5 | 15 |
| E | 1010 | 4 | 1 | 4 |
| A | 1011 | 4 | 2 | 8 |

**Total encoded length: L = 10+10+6+12+15+4+8 = 65 bits**

---

## QUICK COMPUTATION CHEATSHEET

### k-NN distance formula
```
d(x, y) = sqrt( (x1-y1)² + (x2-y2)² + (x3-y3)² )
```

### Perceptron — full update (α=1, incorrect answer)
```
net  = w1·z1 + w2·z2 + w3·z3 − r
y    = 1 if net≥0, else 0
d    = 1−y  (opposite, since incorrect)
err  = d − y
wi'  = wi + err·zi
r'   = r  − err       ← note: minus err, not plus
```

### Confusion matrix from text
```
"Predicted X positive, Y truly positive, Z correctly predicted negative"
TP = Y
FP = X − Y
TN = Z
FN = total − TP − FP − TN
```

### Laplace smoothing
```
P(xi | class) = (count(xi in class) + 1) / (count(class) + d)
d = distinct values of that attribute in that class's training data
```
