# Lazada_Isagunde Blazor WASM Port Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Port the Lazada_Isagunde .NET MAUI app to a standalone Blazor WebAssembly project in a subfolder, sharing the same Firebase backend.

**Architecture:** Twin Architecture (Blazor project lives in a subfolder of the MAUI project). Logic (Models/Services) is copied 1:1 and adapted for WASM. UI is ported from XAML to Razor with Web UX optimizations.

**Tech Stack:** .NET 9.0, Blazor WebAssembly, Firebase (.NET clients), Blazored.LocalStorage, CSS (Vanilla/Bootstrap).

---

### Task 1: Project Scaffolding & Isolation

**Files:**
- Create: `Lazada_Isagunde.Blazor/`
- Modify: `Lazada_Isagunde.csproj`
- Modify: `Lazada_Isagunde.sln`

- [ ] **Step 1: Create the Blazor WASM project**
Run: `dotnet new blazorwasm -o Lazada_Isagunde.Blazor --interactivity WebAssembly --all-interactive`

- [ ] **Step 2: Isolate MAUI from the Blazor folder**
Modify `Lazada_Isagunde.csproj` to exclude the new folder.
```xml
<ItemGroup>
    <DefaultItemExcludes>$(DefaultItemExcludes);Lazada_Isagunde.Blazor\**</DefaultItemExcludes>
</ItemGroup>
```

- [ ] **Step 3: Add Blazor project to Solution**
Run: `dotnet sln Lazada_Isagunde.sln add Lazada_Isagunde.Blazor/Lazada_Isagunde.Blazor.csproj`

- [ ] **Step 4: Verify MAUI still builds**
Run: `dotnet build Lazada_Isagunde.csproj`
Expected: SUCCESS

- [ ] **Step 5: Commit**
```bash
git add .
git commit -m "chore: scaffold blazor project and isolate from maui"
```

### Task 2: Models and Dependency Migration

**Files:**
- Create: `Lazada_Isagunde.Blazor/Models/`
- Modify: `Lazada_Isagunde.Blazor/Lazada_Isagunde.Blazor.csproj`

- [ ] **Step 1: Copy Models 1:1**
Copy all files from `Models/` to `Lazada_Isagunde.Blazor/Models/`.
Update namespaces in the copied files to `Lazada_Isagunde.Blazor.Models`.

- [ ] **Step 2: Add NuGet Packages to Blazor Project**
Run in `Lazada_Isagunde.Blazor/`:
`dotnet add package FirebaseAuthentication.net`
`dotnet add package FirebaseDatabase.net`
`dotnet add package FirebaseStorage.net`
`dotnet add package Blazored.LocalStorage`

- [ ] **Step 3: Verify Blazor project builds with models**
Run: `dotnet build Lazada_Isagunde.Blazor/Lazada_Isagunde.Blazor.csproj`
Expected: SUCCESS

- [ ] **Step 4: Commit**
```bash
git add Lazada_Isagunde.Blazor/
git commit -m "feat: migrate models and add dependencies to blazor project"
```

### Task 3: AuthService Migration (WASM Adaptation)

**Files:**
- Create: `Lazada_Isagunde.Blazor/Services/AuthService.cs`
- Modify: `Lazada_Isagunde.Blazor/Program.cs`

- [ ] **Step 1: Implement AuthService with LocalStorage**
Port `Services/AuthService.cs` to Blazor, replacing MAUI Preferences with `ILocalStorageService`.
```csharp
// Example adaptaton snippet
public class AuthService {
    private readonly ILocalStorageService _localStorage;
    // ... use _localStorage.SetItemAsync("user_token", token)
}
```

- [ ] **Step 2: Register Service in Program.cs**
```csharp
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthService>();
```

- [ ] **Step 3: Commit**
```bash
git add Lazada_Isagunde.Blazor/
git commit -m "feat: implement auth service for blazor wasm"
```

### Task 4: UI Shell & Base Layout

**Files:**
- Modify: `Lazada_Isagunde.Blazor/Layout/MainLayout.razor`
- Create: `Lazada_Isagunde.Blazor/Layout/NavMenu.razor` (Top Header style)

- [ ] **Step 1: Create Shared Header**
Implement a top navigation bar with the Lazada logo, Search bar, and Profile/Cart icons.

- [ ] **Step 2: Apply Responsive Grid**
Update `MainLayout.razor` to use a max-width container for desktop view.

- [ ] **Step 3: Commit**
```bash
git add Lazada_Isagunde.Blazor/Layout/
git commit -m "feat: setup responsive shell and header"
```

### Task 5: Port Core Pages (Landing, Login, Register)

**Files:**
- Create: `Lazada_Isagunde.Blazor/Pages/Landing.razor`
- Create: `Lazada_Isagunde.Blazor/Pages/Login.razor`
- Create: `Lazada_Isagunde.Blazor/Pages/Register.razor`

- [ ] **Step 1: Port Landing Page**
Implement the splash/landing logic with "Get Started" button.

- [ ] **Step 2: Port Login/Register**
Match the MAUI design but use HTML forms and Blazor data binding. Ensure Firebase Auth is triggered.

- [ ] **Step 3: Verify Auth Flow**
Run Blazor app, register a new user, and verify they appear in Firebase Console.

- [ ] **Step 4: Commit**
```bash
git add Lazada_Isagunde.Blazor/Pages/
git commit -m "feat: port landing, login, and registration pages"
```

### Task 6: Port Main Dashboard & Product Detail

**Files:**
- Create: `Lazada_Isagunde.Blazor/Pages/Index.razor` (Dashboard)
- Create: `Lazada_Isagunde.Blazor/Pages/ProductDetail.razor`

- [ ] **Step 1: Port Dashboard**
Fetch products from Firebase and display in a responsive grid.

- [ ] **Step 2: Port Product Detail**
Display images, description, and "Add to Cart" button.

- [ ] **Step 3: Commit**
```bash
git add Lazada_Isagunde.Blazor/Pages/
git commit -m "feat: port dashboard and product detail pages"
```

### Task 7: Port Remaining Pages (Cart, Orders, Profile, Admin)

**Files:**
- Create: `Lazada_Isagunde.Blazor/Pages/Cart.razor`
- Create: `Lazada_Isagunde.Blazor/Pages/Orders.razor`
- Create: `Lazada_Isagunde.Blazor/Pages/Profile.razor`
- Create: `Lazada_Isagunde.Blazor/Pages/Admin/Dashboard.razor`

- [ ] **Step 1: Port Cart & Checkout logic**
Ensure cart items are synced with Firebase for the specific UserID.

- [ ] **Step 2: Port Profile & Order History**

- [ ] **Step 3: Port Admin Dashboard**
Only accessible if `IsAdmin` is true.

- [ ] **Step 4: Commit**
```bash
git add Lazada_Isagunde.Blazor/Pages/
git commit -m "feat: complete porting of all functional pages"
```
