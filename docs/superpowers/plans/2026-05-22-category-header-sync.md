# Category Header Synchronization Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Dynamically update the "Just For You" product section header on the Dashboard screen in both the Blazor web app and the MAUI mobile app to show the name of the currently selected category (using friendly display names, e.g., "Male" instead of "Male Clothes").

**Architecture:** Add a mapping method `GetCategoryDisplayName(string category)` to translate database category strings to friendly names. Use this display name dynamically in Blazor markup and update the MAUI Label in the code-behind when category selection state changes.

**Tech Stack:** C#, Blazor, MAUI (.NET 8.0/9.0)

---

### Task 1: Update Blazor Dashboard
**Files:**
- Modify: [Dashboard.razor](file:///c:/AntigravityProjects/Lazada_Isagunde/Lazada_Isagunde.Blazor/Pages/Dashboard.razor)

- [ ] **Step 1: Add GetCategoryDisplayName helper method**
  Add the following method to the `@code` block of `Dashboard.razor`:
  ```csharp
  private string GetCategoryDisplayName(string category) => category switch
  {
      "Male Clothes" => "Male",
      "Female Clothes" => "Female",
      _ => category
  };
  ```

- [ ] **Step 2: Update the category list display name**
  Modify line 24 of `Dashboard.razor` to use the helper:
  ```razor
  <div class="category-label">@GetCategoryDisplayName(cat)</div>
  ```

- [ ] **Step 3: Update the section header text**
  Modify line 30 of `Dashboard.razor` to use the helper:
  ```razor
  <h2 class="section-title">@GetCategoryDisplayName(selectedCategory)</h2>
  ```

- [ ] **Step 4: Verify Blazor compilation**
  Run compilation to make sure there are no syntax errors.

---

### Task 2: Update MAUI Dashboard Layout and Code-behind
**Files:**
- Modify: [DashboardPage.xaml](file:///c:/AntigravityProjects/Lazada_Isagunde/Pages/DashboardPage.xaml)
- Modify: [DashboardPage.xaml.cs](file:///c:/AntigravityProjects/Lazada_Isagunde/Pages/DashboardPage.xaml.cs)

- [ ] **Step 1: Name the Section Header Label in XAML**
  Modify the `Label` at line 173 of `DashboardPage.xaml` to:
  ```xml
  <Label x:Name="SectionHeaderLabel" Text="General" FontSize="20" FontAttributes="Bold" TextColor="Black" />
  ```

- [ ] **Step 2: Add GetCategoryDisplayName helper method to code-behind**
  Add the helper method to `DashboardPage.xaml.cs`:
  ```csharp
  private string GetCategoryDisplayName(string category) => category switch
  {
      "Male Clothes" => "Male",
      "Female Clothes" => "Female",
      _ => category
  };
  ```

- [ ] **Step 3: Update Header Label on Selection and Load**
  In the `OnCategoryTapped` method of `DashboardPage.xaml.cs`, add:
  ```csharp
  SectionHeaderLabel.Text = GetCategoryDisplayName(_selectedCategory);
  ```
  In the `ApplyFilter` method or `OnAppearing`, ensure the header displays the default selected category:
  ```csharp
  SectionHeaderLabel.Text = GetCategoryDisplayName(_selectedCategory);
  ```

- [ ] **Step 4: Verify MAUI compilation**
  Run compilation to ensure there are no build issues.
