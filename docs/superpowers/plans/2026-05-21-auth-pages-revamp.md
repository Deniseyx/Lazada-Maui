# Auth Pages Revamp Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Revamp Login and Register pages to match Modern Minimalist design with orange accents and white cards.

**Architecture:** Update HTML structure to use `modern-card` and `section-title` classes, and replace local `<style>` blocks with standardized CSS.

**Tech Stack:** Blazor (Razor components), CSS.

---

### Task 1: Update Login.razor

**Files:**
- Modify: `Lazada_Isagunde.Blazor/Pages/Login.razor`

- [ ] **Step 1: Replace HTML structure**
Update the structure to use `modern-card` and `section-title`.

```razor
<div class="auth-container d-flex align-items-center justify-content-center">
    <div class="modern-card auth-card">
        <h2 class="section-title text-center mb-4">Login</h2>
        
        @if (!string.IsNullOrEmpty(errorMessage))
        {
            <div class="alert alert-danger">@errorMessage</div>
        }

        <div class="form-group mb-3">
            <input type="email" class="form-control" placeholder="Email" @bind="email" />
        </div>
        
        <div class="form-group mb-4">
            <input type="password" class="form-control" placeholder="Password" @bind="password" />
        </div>

        <button class="btn btn-primary w-100 mb-4" @onclick="HandleLogin" disabled="@isProcessing">
            @if (isProcessing)
            {
                <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                <span> Signing in...</span>
            }
            else
            {
                <span>Sign In</span>
            }
        </button>

        <div class="text-center">
            <a href="/forgot-password" class="auth-link">Forgot Password?</a>
        </div>
    </div>
</div>
```

- [ ] **Step 2: Replace CSS styles**
Replace the `<style>` block with the standardized version.

```css
<style>
    .auth-container {
        min-height: calc(100vh - 150px);
        background-color: var(--lazada-bg);
    }
    .auth-card {
        width: 100%;
        max-width: 420px;
        padding: 40px !important;
    }
    .auth-link {
        color: var(--lazada-orange);
        text-decoration: none;
        font-weight: 600;
        transition: color 0.2s;
    }
    .auth-link:hover {
        color: var(--lazada-orange-deep);
        text-decoration: underline;
    }
    .btn-primary {
        background-color: var(--lazada-orange);
        border: none;
        border-radius: 8px;
        height: 52px;
        font-weight: 700;
        font-size: 1.1rem;
    }
    .btn-primary:hover {
        background-color: var(--lazada-orange-deep);
    }
</style>
```

- [ ] **Step 3: Commit**
`git add Lazada_Isagunde.Blazor/Pages/Login.razor && git commit -m "ui: revamp login page for modern design consistency"`

---

### Task 2: Update Register.razor

**Files:**
- Modify: `Lazada_Isagunde.Blazor/Pages/Register.razor`

- [ ] **Step 1: Replace HTML structure**
Mirror the Login design.

```razor
<div class="auth-container d-flex align-items-center justify-content-center">
    <div class="modern-card auth-card">
        <h2 class="section-title text-center mb-4">Create Account</h2>
        
        @if (!string.IsNullOrEmpty(errorMessage))
        {
            <div class="alert alert-danger">@errorMessage</div>
        }

        <div class="form-group mb-3">
            <input type="text" class="form-control" placeholder="Full Name" @bind="fullName" />
        </div>

        <div class="form-group mb-3">
            <input type="email" class="form-control" placeholder="Email" @bind="email" />
        </div>
        
        <div class="form-group mb-4">
            <input type="password" class="form-control" placeholder="Password" @bind="password" />
        </div>

        <button class="btn btn-primary w-100 mb-4" @onclick="HandleRegister" disabled="@isProcessing">
            @if (isProcessing)
            {
                <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                <span> Signing up...</span>
            }
            else
            {
                <span>Sign Up</span>
            }
        </button>

        <div class="text-center">
            <p class="text-muted">Already have an account? <a href="/login" class="auth-link">Login</a></p>
        </div>
    </div>
</div>
```

- [ ] **Step 2: Replace CSS styles**
Apply the same standardized style block.

- [ ] **Step 3: Commit**
`git add Lazada_Isagunde.Blazor/Pages/Register.razor && git commit -m "ui: revamp register page for modern design consistency"`

---

### Task 3: Verification

- [ ] **Step 1: Build the project**
Run: `dotnet build`
Expected: Build SUCCESS.

- [ ] **Step 2: Verify styles and logic**
Ensure no purple references remain and all bindings are correct.
