# Category Selection Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a visually appealing category selector above the "Just For You" section on the Landing and Dashboard pages, defaulting to "General".

**Architecture:** Use a `selectedCategory` state variable to filter the `FilteredProducts` list locally. Implement a reusable CSS category strip with icons.

**Tech Stack:** Blazor WebAssembly, Bootstrap, Vanilla CSS.

---

### Task 1: Add Category Styles

**Files:**
- Modify: `Lazada_Isagunde.Blazor/wwwroot/css/app.css`

- [ ] **Step 1: Add category strip styles**

Add the following styles to the end of `app.css`:

```css
/* Category Strip */
.category-strip {
    display: flex;
    gap: 15px;
    overflow-x: auto;
    padding: 20px 0;
    margin-bottom: 20px;
    scrollbar-width: none; /* Firefox */
}

.category-strip::-webkit-scrollbar {
    display: none; /* Chrome, Safari, Opera */
}

.category-item {
    display: flex;
    flex-direction: column;
    align-items: center;
    min-width: 100px;
    cursor: pointer;
    padding-bottom: 10px;
    border-bottom: 3px solid transparent;
    transition: all 0.2s;
    text-align: center;
}

.category-item:hover {
    transform: translateY(-2px);
}

.category-item.active {
    border-bottom-color: var(--lazada-orange);
    color: var(--lazada-orange);
}

.category-icon {
    font-size: 28px;
    margin-bottom: 5px;
}

.category-label {
    font-size: 0.9rem;
    font-weight: 500;
}
```

- [ ] **Step 2: Commit**

```bash
git add Lazada_Isagunde.Blazor/wwwroot/css/app.css
git commit -m "style: add category strip styles"
```

---

### Task 2: Implement Category Selection in Dashboard

**Files:**
- Modify: `Lazada_Isagunde.Blazor/Pages/Dashboard.razor`

- [ ] **Step 1: Update state and filtering logic**

Update the `@code` block to include `selectedCategory` and update `FilteredProducts`:

```razor
@code {
    private List<Product>? products;
    private string searchQuery = "";
    private string selectedCategory = "General";

    private IEnumerable<Product> FilteredProducts => 
        (products ?? new())
        .Where(p => (string.IsNullOrWhiteSpace(searchQuery) || (p.Title != null && p.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)))
                    && (selectedCategory == "General" || p.Category == selectedCategory));
    
    // ... rest of the code ...
}
```

- [ ] **Step 2: Add category strip UI**

Insert the category strip markup above the `section-header`:

```razor
<div class="dashboard-container">
    <div class="category-strip">
        @foreach (var cat in new[] { "General", "Electronics", "Fashion", "Home", "Health", "Male Clothes", "Female Clothes", "Kids" })
        {
            <div class="category-item @(selectedCategory == cat ? "active" : "")" @onclick="() => selectedCategory = cat">
                <div class="category-icon">
                    @switch (cat)
                    {
                        case "General": <span>🛒</span> break;
                        case "Electronics": <span>💻</span> break;
                        case "Fashion": <span>✨</span> break;
                        case "Home": <span>🏠</span> break;
                        case "Health": <span>💊</span> break;
                        case "Male Clothes": <span>👕</span> break;
                        case "Female Clothes": <span>👗</span> break;
                        case "Kids": <span>🧸</span> break;
                    }
                </div>
                <div class="category-label">@cat</div>
            </div>
        }
    </div>

    <div class="section-header mb-4">
        <h2 class="section-title">Just For You</h2>
    </div>
    ...
```

- [ ] **Step 3: Verify build**

Run: `dotnet build Lazada_Isagunde.Blazor/Lazada_Isagunde.Blazor.csproj`
Expected: PASS

- [ ] **Step 4: Commit**

```bash
git add Lazada_Isagunde.Blazor/Pages/Dashboard.razor
git commit -m "feat: add category filtering to Dashboard"
```

---

### Task 3: Implement Category Selection in Landing Page

**Files:**
- Modify: `Lazada_Isagunde.Blazor/Pages/Landing.razor`

- [ ] **Step 1: Update state and filtering logic**

Update the `@code` block in `Landing.razor`:

```razor
@code {
    private List<Product>? products;
    private string selectedCategory = "General";

    private IEnumerable<Product> FilteredProducts => 
        (products ?? new())
        .Where(p => selectedCategory == "General" || p.Category == selectedCategory);

    protected override async Task OnInitializedAsync()
    {
        // Add logic to load products if not already there, similar to Dashboard
        products = await FirebaseService.GetProductsAsync("Approved");
    }
    
    // ... existing navigation logic ...
}
```

- [ ] **Step 2: Add category strip and product grid UI**

Update the UI to show the categories and filtered products below the hero section:

```razor
    ... existing hero content ...
</div>

<div class="app-container">
    <div class="category-strip">
        @foreach (var cat in new[] { "General", "Electronics", "Fashion", "Home", "Health", "Male Clothes", "Female Clothes", "Kids" })
        {
            <div class="category-item @(selectedCategory == cat ? "active" : "")" @onclick="() => selectedCategory = cat">
                <div class="category-icon">
                    @switch (cat)
                    {
                        case "General": <span>🛒</span> break;
                        case "Electronics": <span>💻</span> break;
                        case "Fashion": <span>✨</span> break;
                        case "Home": <span>🏠</span> break;
                        case "Health": <span>💊</span> break;
                        case "Male Clothes": <span>👕</span> break;
                        case "Female Clothes": <span>👗</span> break;
                        case "Kids": <span>🧸</span> break;
                    }
                </div>
                <div class="category-label">@cat</div>
            </div>
        }
    </div>

    <h2 class="section-title mb-4">Just For You</h2>
    
    @if (products == null)
    {
        <div class="text-center py-5">
            <div class="spinner-border text-primary" role="status"></div>
        </div>
    }
    else
    {
        <div class="product-grid">
            @foreach (var product in FilteredProducts)
            {
                <div class="product-card" @onclick="() => Navigation.NavigateTo($\"/product/{product.Id}\")">
                    <div class="product-image-wrapper">
                        <img src="@product.DisplayImageUrl" class="product-image" onerror="this.src='https://placehold.co/600x400?text=No+Image'" />
                    </div>
                    <div class="product-info">
                        <h5 class="product-title">@product.Title</h5>
                        <p class="product-price">@product.PriceFormatted</p>
                    </div>
                </div>
            }
        </div>
    }
</div>

<style>
    /* Add landing specific grid styles if not in app.css */
    .product-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(188px, 1fr));
        gap: 12px;
        margin-bottom: 40px;
    }
    ...
</style>
```

- [ ] **Step 3: Verify build**

Run: `dotnet build Lazada_Isagunde.Blazor/Lazada_Isagunde.Blazor.csproj`
Expected: PASS

- [ ] **Step 4: Commit**

```bash
git add Lazada_Isagunde.Blazor/Pages/Landing.razor
git commit -m "feat: add category filtering to Landing page"
```
