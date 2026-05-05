# GIT EXAM CHEAT SHEET — Copy-paste every command

---

## ⚡ STEP 0 — Before you write a single line of code

```bash
# 1. Create the project
dotnet new webapi -n Kolokwium --no-openapi
cd Kolokwium

# 2. Add SQL package
dotnet add package Microsoft.Data.SqlClient

# 3. Init git — DO THIS BEFORE ANYTHING ELSE
git init

# 4. Create .gitignore — PASTE THIS CONTENT:
```

**.gitignore content to paste:**
```
bin/
obj/
*.user
.vs/
.idea/
*.suo
appsettings.Development.json
*.DS_Store
```

```bash
# 5. First commit — .gitignore MUST be in before bin/obj ever exist
git add .gitignore
git commit -m "init: add gitignore"

# 6. Create repo on GitHub (do this in browser), then link it:
git remote add origin https://github.com/YOUR_USERNAME/Kolokwium.git
# ↑ REPLACE YOUR_USERNAME and Kolokwium with your actual GitHub username and repo name

# 7. Set branch name to main and push
git branch -M main
git push -u origin main
```

---

## 🏗️ STEP 1 — Project setup commit (after deleting WeatherForecast files)

```bash
# Delete the template garbage
rm WeatherForecast.cs
rm Controllers/WeatherForecastController.cs

# Commit clean project structure
git add .
git commit -m "setup: clean project, add folder structure"
git push
```

---

## 📁 STEP 2 — After creating all folders and empty files

```bash
git add .
git commit -m "setup: add Models, DTOs, Repositories, Controllers folders"
git push
```

---

## 🗂️ STEP 3 — After adding Models

```bash
git add Models/
git commit -m "feat: add domain models (Client, Booking, ClassType, Status)"
# ↑ RENAME "Client, Booking..." to your actual model names
git push
```

---

## 📤 STEP 4 — After adding DTOs

```bash
git add DTOs/
git commit -m "feat: add DTOs (ClientDto, BookingDto, AddBookingRequestDto)"
# ↑ RENAME to your actual DTO names
git push
```

---

## 🔌 STEP 5 — After adding the interface

```bash
git add Repositories/IClientRepository.cs
git commit -m "feat: add IClientRepository interface"
# ↑ RENAME IClientRepository to your actual interface name
git push
```

---

## 🗄️ STEP 6 — After implementing GET in repository

```bash
git add Repositories/ClientRepository.cs
git commit -m "feat: implement GET - GetClientWithBookingsAsync (JOIN + Dictionary)"
# ↑ RENAME method name to match yours
git push
```

---

## 📮 STEP 7 — After implementing POST in repository

```bash
git add Repositories/ClientRepository.cs
git commit -m "feat: implement POST - AddBookingAsync (transaction + SCOPE_IDENTITY)"
# ↑ RENAME method name to match yours
git push
```

---

## 🎮 STEP 8 — After adding the controller

```bash
git add Controllers/
git commit -m "feat: add ClientsController (GET 200/404, POST 201/404)"
# ↑ RENAME ClientsController to your actual controller name
git push
```

---

## ⚙️ STEP 9 — After updating Program.cs with DI registration

```bash
git add Program.cs
git commit -m "feat: register IClientRepository with AddScoped in Program.cs"
# ↑ RENAME IClientRepository to your actual interface name
git push
```

---

## 🔧 STEP 10 — Bug fixes during development (use these as needed)

```bash
# After fixing any bug:
git add .
git commit -m "fix: correct nullable column handling in reader"
git push

# Other fix commit messages to copy:
git commit -m "fix: add missing await on ExecuteScalarAsync"
git commit -m "fix: add (conn, tx) to SqlCommand inside transaction"
git commit -m "fix: IsDBNull check on return_date column"
git commit -m "fix: correct status name from Scheduled to Active"
git commit -m "fix: use SCOPE_IDENTITY() instead of @@IDENTITY"
git commit -m "fix: add missing AddScoped registration in Program.cs"
git commit -m "fix: correct route from api/client to api/clients"
git commit -m "fix: return 404 when result is null in GET"
git commit -m "fix: return 201 CreatedAtAction instead of 200 Ok on POST"
git commit -m "fix: add RollbackAsync in catch block"
```

---

## 🧪 STEP 11 — After testing in Swagger / Postman

```bash
git add .
git commit -m "test: verify GET and POST endpoints working"
git push
```

---

## 🏁 FINAL — Last commit before time runs out

```bash
# Stage EVERYTHING and push
git add .
git commit -m "solution: complete GET and POST endpoints"
git push

# If push is rejected (someone else pushed / branch issue):
git pull --rebase origin main
git push

# Check everything is on GitHub:
git log --oneline
git status   # should say "nothing to commit, working tree clean"
```

---

## 🚨 EMERGENCY COMMANDS — when things go wrong

```bash
# See what files are staged / unstaged
git status

# See what changed in a file
git diff Repositories/ClientRepository.cs

# Undo changes to one file (back to last commit)
git checkout -- Repositories/ClientRepository.cs

# Unstage a file (keep changes, just don't commit it yet)
git restore --staged Repositories/ClientRepository.cs

# See commit history
git log --oneline

# Push if the first push was rejected
git push -u origin main --force
# ↑ Only use --force if the repo is yours and you know what you're doing

# If you committed bin/ or obj/ by accident (missing .gitignore):
git rm -r --cached bin/ obj/
git add .gitignore
git commit -m "fix: remove bin obj from tracking, add gitignore"
git push
```

---

## 📋 COMMIT MESSAGE TEMPLATES — copy the right one

| What you just did | Commit message |
|---|---|
| Added .gitignore | `init: add gitignore` |
| Deleted WeatherForecast | `setup: clean template files` |
| Added all models | `feat: add domain models` |
| Added all DTOs | `feat: add request and response DTOs` |
| Added interface | `feat: add IXxxRepository interface` |
| Added GET in repo | `feat: implement GET endpoint (LEFT JOIN + reader)` |
| Added POST in repo | `feat: implement POST endpoint (transaction)` |
| Added controller | `feat: add XxxController with GET and POST` |
| Updated Program.cs | `feat: register dependencies in DI` |
| Fixed a bug | `fix: <what you fixed>` |
| Everything done | `solution: complete implementation` |

---

## 📌 FULL SEQUENCE — one block to run at the start of every exam

```bash
# ═══ RUN THIS AT THE START OF EVERY EXAM ═══

dotnet new webapi -n Kolokwium --no-openapi
cd Kolokwium
dotnet add package Microsoft.Data.SqlClient
git init

# Create .gitignore manually in Rider, then:
git add .gitignore
git commit -m "init: add gitignore"

# Go to GitHub in browser:
# New repo → name: Kolokwium → NO readme, NO gitignore → Create
# Then copy the remote URL and run:

git remote add origin https://github.com/YOUR_USERNAME/Kolokwium.git
git branch -M main
git push -u origin main

# Delete template files:
rm WeatherForecast.cs Controllers/WeatherForecastController.cs
mkdir Models DTOs Repositories

git add .
git commit -m "setup: clean project structure"
git push

# ═══ NOW START CODING ═══
```
