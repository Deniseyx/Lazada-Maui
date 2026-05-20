# Blazor UI Revamp Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Revamp the Blazor UI to a Modern Minimalist design with a White and Orange palette, while hiding the Settings feature.

**Architecture:** Update global CSS for consistent styling (colors, shadows, layout) and surgically modify Razor components to implement the new UI structure without touching backend logic.

**Tech Stack:** Blazor (C#), HTML/CSS, Bootstrap Icons.

---

### Task 1: Global CSS Overhaul

**Files:**
- Modify: `Lazada_Isagunde.Blazor/wwwroot/css/app.css`

- [ ] **Step 1: Update CSS variables and global styles**
Replace the `:root` and body styles with the new palette and soft backgrounds.

```css
:root {
    --lazada-orange: #f57224;
    --lazada-orange-deep: #e0611d;
    --lazada-bg: #f8f9fa;
    --surface: #ffffff;
    --text-main: #212529;
    --text-muted: #6c757d;
    --card-shadow: 0 10px 30px rgba(0,0,0,0.04);
    --container-max-width: 1200px;
}

body {
    background-color: var(--lazada-bg);
    color: var(--text-main);
    margin: 0;
    font-family: 'Inter', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif;
}

.app-container {
    max-width: var(--container-max-width);
    margin: 0 auto;
    width: 100%;
    padding: 0 20px;
}
```

- [ ] **Step 2: Add modern card and typography styles**
Append the following to `app.css`.

```css
.modern-card {
    background-color: var(--surface);
    border-radius: 12px;
    box-shadow: var(--card-shadow);
    padding: 30px;
    border: none;
    margin-bottom: 24px;
}

.section-title {
    font-weight: 700;
    font-size: 1.25rem;
    margin-bottom: 1.5rem;
    color: var(--text-main);
}

.form-label {
    font-weight: 500;
    font-size: 0.9rem;
    color: var(--text-muted);
    margin-bottom: 8px;
}

.form-control {
    border-radius: 8px;
    border: 1px solid #e9ecef;
    padding: 12px 16px;
    background-color: #fff;
    transition: border-color 0.2s, box-shadow 0.2s;
}

.form-control:focus {
    border-color: var(--lazada-orange);
    box-shadow: 0 0 0 3px rgba(245, 114, 36, 0.1);
}
```

- [ ] **Step 3: Commit CSS changes**

```bash
git add Lazada_Isagunde.Blazor/wwwroot/css/app.css
git commit -m "style: update global CSS variables and base styles for modern minimalist look"
```

---

### Task 2: Header Refinement

**Files:**
- Modify: `Lazada_Isagunde.Blazor/Layout/NavMenu.razor`
- Modify: `Lazada_Isagunde.Blazor/wwwroot/css/app.css`

- [ ] **Step 1: Clean up Header CSS**
Update `.navbar-header` and search styles in `app.css`.

```css
.navbar-header {
    background-color: var(--lazada-orange);
    height: 72px;
    display: flex;
    align-items: center;
    position: sticky;
    top: 0;
    z-index: 1000;
    border-bottom: 1px solid var(--lazada-orange-deep);
}

.search-container {
    display: flex;
    width: 100%;
    background: white;
    border-radius: 8px;
    overflow: hidden;
    padding: 2px;
}

.search-input {
    flex-grow: 1;
    padding: 10px 15px;
    border: none;
    outline: none;
    font-size: 0.95rem;
}

.search-button {
    background: transparent;
    color: var(--lazada-orange);
    border: none;
    padding: 0 15px;
    cursor: pointer;
}
```

- [ ] **Step 2: Update NavMenu.razor structure**
Refine the search bar and icons.

```razor
<div class="header-center">
    <div class="search-container">
        <input type="text" class="search-input" placeholder="Search products..." @bind="searchQuery" @onkeyup="HandleSearch" />
        <button class="search-button" @onclick="ExecuteSearch">
            <i class="bi bi-search"></i>
        </button>
    </div>
</div>
```

- [ ] **Step 3: Commit Header changes**

```bash
git add Lazada_Isagunde.Blazor/Layout/NavMenu.razor Lazada_Isagunde.Blazor/wwwroot/css/app.css
git commit -m "ui: refine header layout and search bar styling"
```

---

### Task 3: Profile Page Revamp

**Files:**
- Modify: `Lazada_Isagunde.Blazor/Pages/Profile.razor`

- [ ] **Step 1: Remove old header block and apply new layout**
Replace the top section and wrap content in `modern-card`.

```razor
<div class="profile-container">
    <div class="d-flex align-items-center mb-5 mt-2">
        <div class="profile-avatar-new">
            <i class="bi bi-person-circle"></i>
        </div>
        <div class="ms-4">
            <h2 class="fw-bold mb-1">@(profile?.FullName ?? AuthService.UserDisplayName ?? "User")</h2>
            <p class="text-muted mb-0">@AuthService.UserEmail</p>
        </div>
    </div>

    <div class="row">
        <div class="col-md-8">
            <div class="modern-card">
                <h4 class="section-title">Shipping Information</h4>
                <!-- ... form fields ... -->
            </div>
        </div>
        <!-- ... sidebar ... -->
    </div>
</div>
```

- [ ] **Step 2: Update Sidebar and HIDE Settings**
Clean up the list group and remove the settings link.

```razor
<div class="col-md-4">
    <div class="modern-card p-0 overflow-hidden">
        <div class="list-group list-group-flush">
            <a href="/orders" class="list-group-item list-group-item-action border-0 py-3 px-4">
                <i class="bi bi-box me-3 text-muted"></i> My Orders
            </a>
            <a href="/seller-center" class="list-group-item list-group-item-action border-0 py-3 px-4">
                <i class="bi bi-shop me-3 text-muted"></i> Seller Center
            </a>
            <a href="/contact" class="list-group-item list-group-item-action border-0 py-3 px-4">
                <i class="bi bi-headset me-3 text-muted"></i> Contact Us
            </a>
            @if (AuthService.IsAdmin)
            {
                <a href="/admin" class="list-group-item list-group-item-action border-0 py-3 px-4 text-primary">
                    <i class="bi bi-shield-check me-3"></i> Admin Dashboard
                </a>
            }
        </div>
    </div>

    <button class="btn btn-link text-danger text-decoration-none w-100 mt-2" @onclick="Logout">
        <i class="bi bi-box-arrow-right me-2"></i> Log Out
    </button>
</div>
```

- [ ] **Step 3: Update Profile Page styles**
Update the `<style>` block at the bottom.

```css
<style>
    .profile-avatar-new {
        font-size: 64px;
        color: var(--lazada-orange);
        line-height: 1;
    }
    .list-group-item:hover {
        background-color: #fff9f6 !important;
        color: var(--lazada-orange) !important;
    }
    .list-group-item:hover i {
        color: var(--lazada-orange) !important;
    }
    .btn-primary {
        background-color: var(--lazada-orange);
        border: none;
        border-radius: 8px;
        padding: 12px 30px;
        font-weight: 600;
    }
    .btn-primary:hover {
        background-color: var(--lazada-orange-deep);
    }
</style>
```

- [ ] **Step 4: Commit Profile changes**

```bash
git add Lazada_Isagunde.Blazor/Pages/Profile.razor
git commit -m "ui: revamp profile page with modern minimalist card design and hide settings"
```

---

### Task 4: Global Cleanup & Verification

**Files:**
- Modify: `Lazada_Isagunde.Blazor/Layout/MainLayout.razor`
- Search: All `.razor` files for `/settings`

- [ ] **Step 1: Simplify MainLayout**
Ensure `MainLayout` doesn't add extra padding that conflicts with `modern-card`.

- [ ] **Step 2: Search and Remove Settings links**
Run `grep` to find any other links to `/settings` and remove them.

- [ ] **Step 3: Final Verification**
Build and run the app to ensure no compilation errors and check the UI manually.

```bash
dotnet build Lazada_Isagunde.Blazor
```

- [ ] **Step 4: Commit Cleanup**

```bash
git commit -m "cleanup: remove remaining references to settings page"
```
