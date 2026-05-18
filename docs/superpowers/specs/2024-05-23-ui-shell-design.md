# Design Specification: UI Shell & Base Layout

**Date:** 2024-05-23
**Topic:** UI Shell & Base Layout for Lazada_Isagunde.Blazor

## 1. Overview
Transition the Blazor application from a sidebar-based layout to a top-header-driven layout, matching the aesthetics of the Lazada e-commerce platform while maintaining consistency with the MAUI project's purple theme.

## 2. Architecture & Components

### 2.1 MainLayout.razor
- Remove the sidebar `div` and the default sidebar CSS classes.
- Use a vertical flex layout.
- Include the `NavMenu` component at the top.
- Wrap the `@Body` in a `main` element with a responsive container.

### 2.2 NavMenu.razor
- Implement a sticky top navigation bar.
- Background color: `#512BD4` (Lazada Purple).
- Layout:
    - **Left:** Branding/Logo (Text-based for now).
    - **Center:** Search Bar (Placeholder input).
    - **Right:** Profile and Cart icons (using Bootstrap Icons or similar).
- UX: Hover states and pointer cursors for interactive elements.

### 2.3 app.css
- Global styles for the purple theme.
- Responsive container class (`.app-container`) with `max-width: 1200px` and `margin: 0 auto`.
- Background color for the page: `#f4f4f4` (Light grey).
- Styles for the header, search bar, and icons.

## 3. Implementation Details

### MainLayout.razor
```razor
@inherits LayoutComponentBase

<div class="page">
    <NavMenu />

    <main>
        <div class="app-container">
            <article class="content px-4">
                @Body
            </article>
        </div>
    </main>
</div>
```

### NavMenu.razor
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
            <div class="header-icon-item">
                <i class="bi bi-person"></i>
                <span>Profile</span>
            </div>
            <div class="header-icon-item">
                <i class="bi bi-cart"></i>
                <span>Cart</span>
            </div>
        </div>
    </div>
</header>
```

### app.css Additions
```css
:root {
    --lazada-purple: #512BD4;
    --lazada-bg: #f4f4f4;
}

body {
    background-color: var(--lazada-bg);
}

.app-container {
    max-width: 1200px;
    margin: 0 auto;
    width: 100%;
}

.navbar-header {
    background-color: var(--lazada-purple);
    color: white;
    padding: 0.5rem 0;
    position: sticky;
    top: 0;
    z-index: 1000;
}

.header-content {
    display: flex;
    align-items: center;
    justify-content: space-between;
}

/* ... more styles ... */
```

## 4. Testing & Verification
- Run `dotnet build` to ensure no compilation errors.
- Visual verification (if possible) of the header and layout.
