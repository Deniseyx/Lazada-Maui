# UI Shell & Base Layout Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Redesign the Blazor application shell to use a top-header-driven layout with Lazada-style purple branding and a responsive centered container.

**Architecture:** Replace the default sidebar layout with a sticky top `NavMenu` component. Wrap the main content in a responsive container with a maximum width of 1200px.

**Tech Stack:** Blazor WebAssembly, Bootstrap Icons, Vanilla CSS.

---

### Task 1: Global Styles & Theme

**Files:**
- Modify: `Lazada_Isagunde.Blazor/wwwroot/css/app.css`
- Modify: `Lazada_Isagunde.Blazor/Layout/MainLayout.razor.css` (Clear)
- Modify: `Lazada_Isagunde.Blazor/Layout/NavMenu.razor.css` (Clear)

- [ ] **Step 1: Update app.css with Lazada theme and responsive grid**

```css
:root {
    --lazada-purple: #512BD4;
    --lazada-bg: #f4f4f4;
    --container-max-width: 1200px;
}

body {
    background-color: var(--lazada-bg);
    margin: 0;
    font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif;
}

.app-container {
    max-width: var(--container-max-width);
    margin: 0 auto;
    width: 100%;
    padding: 0 15px;
}

/* Header Styles */
.navbar-header {
    background-color: var(--lazada-purple);
    color: white;
    padding: 10px 0;
    position: sticky;
    top: 0;
    z-index: 1000;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.header-content {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 20px;
}

.navbar-brand {
    color: white;
    font-weight: bold;
    font-size: 1.5rem;
    text-decoration: none;
    white-space: nowrap;
}

.navbar-brand:hover {
    color: #e0e0e0;
}

/* Search Bar */
.header-center {
    flex-grow: 1;
    max-width: 700px;
}

.search-container {
    display: flex;
    width: 100%;
}

.search-input {
    flex-grow: 1;
    padding: 8px 15px;
    border: none;
    border-radius: 4px 0 0 4px;
    outline: none;
}

.search-button {
    background-color: #f57224; /* Lazada Orange for the button to pop */
    color: white;
    border: none;
    padding: 8px 20px;
    border-radius: 0 4px 4px 0;
    cursor: pointer;
}

.search-button:hover {
    background-color: #d0611e;
}

/* Icons */
.header-right {
    display: flex;
    gap: 20px;
    align-items: center;
}

.header-icon-item {
    display: flex;
    flex-direction: column;
    align-items: center;
    cursor: pointer;
    font-size: 0.8rem;
    color: white;
    text-decoration: none;
}

.header-icon-item i {
    font-size: 1.2rem;
}

.header-icon-item:hover {
    color: #e0e0e0;
}

/* Content Area */
main {
    padding-top: 20px;
}

.content {
    background-color: white;
    padding: 20px;
    border-radius: 4px;
    min-height: 80vh;
}
```

- [ ] **Step 2: Clear scoped CSS files to prevent conflicts**

Modify `Lazada_Isagunde.Blazor/Layout/MainLayout.razor.css` to be empty.
Modify `Lazada_Isagunde.Blazor/Layout/NavMenu.razor.css` to be empty.

- [ ] **Step 3: Commit styles**

```bash
git add Lazada_Isagunde.Blazor/wwwroot/css/app.css Lazada_Isagunde.Blazor/Layout/MainLayout.razor.css Lazada_Isagunde.Blazor/Layout/NavMenu.razor.css
git commit -m "style: apply Lazada purple theme and responsive grid"
```

### Task 2: Implement NavMenu (Top Header)

**Files:**
- Modify: `Lazada_Isagunde.Blazor/Layout/NavMenu.razor`

- [ ] **Step 1: Replace NavMenu.razor with top-header implementation**

```razor
<header class="navbar-header">
    <div class="app-container header-content">
        <div class="header-left">
            <a class="navbar-brand" href="">Lazada Isagunde</a>
        </div>
        
        <div class="header-center">
            <div class="search-container">
                <input type="text" class="search-input" placeholder="Search in Lazada..." />
                <button class="search-button">
                    <i class="bi bi-search"></i>
                </button>
            </div>
        </div>

        <div class="header-right">
            <a href="profile" class="header-icon-item">
                <i class="bi bi-person"></i>
                <span>Profile</span>
            </a>
            <a href="cart" class="header-icon-item">
                <i class="bi bi-cart"></i>
                <span>Cart</span>
            </a>
        </div>
    </div>
</header>
```

- [ ] **Step 2: Commit NavMenu**

```bash
git add Lazada_Isagunde.Blazor/Layout/NavMenu.razor
git commit -m "feat: implement top-navigation header"
```

### Task 3: Update MainLayout Shell

**Files:**
- Modify: `Lazada_Isagunde.Blazor/Layout/MainLayout.razor`

- [ ] **Step 1: Update MainLayout.razor to use the new shell structure**

```razor
@inherits LayoutComponentBase

<div class="page">
    <NavMenu />

    <main>
        <div class="app-container">
            <article class="content">
                @Body
            </article>
        </div>
    </main>
</div>

<div id="blazor-error-ui">
    An unhandled error has occurred.
    <a href="" class="reload">Reload</a>
    <a class="dismiss">🗙</a>
</div>
```

- [ ] **Step 2: Commit MainLayout**

```bash
git add Lazada_Isagunde.Blazor/Layout/MainLayout.razor
git commit -m "feat: update main layout to top-header shell"
```

### Task 4: Verification

- [ ] **Step 1: Build the project**

Run: `dotnet build Lazada_Isagunde.Blazor/Lazada_Isagunde.Blazor.csproj`
Expected: SUCCESS

- [ ] **Step 2: Final Commit**
(Already committed in steps)
