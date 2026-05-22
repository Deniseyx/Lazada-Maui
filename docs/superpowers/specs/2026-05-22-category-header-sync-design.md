# Design Specification: Category Header Synchronization & Rename Fashion to Jewelry

Synchronize the "Just For You" section header with the active selected category on both Blazor and MAUI platforms, using friendly display names, and rename the "Fashion" category to "Jewelry" throughout the entire application.

## Proposed Changes

### Blazor Pages

#### [MODIFY] [Dashboard.razor](file:///c:/AntigravityProjects/Lazada_Isagunde/Lazada_Isagunde.Blazor/Pages/Dashboard.razor)
- Implement `GetCategoryDisplayName(string category)` to convert category keys to display names:
  - `"Male Clothes"` $\rightarrow$ `"Male"`
  - `"Female Clothes"` $\rightarrow$ `"Female"`
  - Any other category $\rightarrow$ retains itself.
- Update `<div class="category-label">` to use `GetCategoryDisplayName(cat)`.
- Update the `<h2 class="section-title">` to use `GetCategoryDisplayName(selectedCategory)`.
- Rename category `"Fashion"` to `"Jewelry"` in the categories list and icon switch.

#### [MODIFY] [Landing.razor](file:///c:/AntigravityProjects/Lazada_Isagunde/Lazada_Isagunde.Blazor/Pages/Landing.razor)
- Rename category `"Fashion"` to `"Jewelry"` in the categories list and icon switch.
- Update `<div class="category-label">` to use `GetCategoryDisplayName(cat)`.
- Implement `GetCategoryDisplayName(string category)` in the code block.

#### [MODIFY] [SellerCenter.razor](file:///c:/AntigravityProjects/Lazada_Isagunde/Lazada_Isagunde.Blazor/Pages/SellerCenter.razor)
- Rename option value and text `"Fashion"` to `"Jewelry"`.

### MAUI Pages

#### [MODIFY] [DashboardPage.xaml](file:///c:/AntigravityProjects/Lazada_Isagunde/Pages/DashboardPage.xaml)
- Add `x:Name="SectionHeaderLabel"` to the "Just For You" `Label` and set its initial `Text` to `"General"`.
- Rename category CommandParameter and Text `"Fashion"` to `"Jewelry"`.

#### [MODIFY] [DashboardPage.xaml.cs](file:///c:/AntigravityProjects/Lazada_Isagunde/Pages/DashboardPage.xaml.cs)
- Implement `GetCategoryDisplayName(string category)` similarly.
- Update `SectionHeaderLabel.Text` on load and when a category is tapped.

#### [MODIFY] [SellerCenterPage.xaml](file:///c:/AntigravityProjects/Lazada_Isagunde/Pages/SellerCenterPage.xaml)
- Rename the checkbox `FashionCb` to `JewelryCb` and its Label to `"Jewelry"`.

#### [MODIFY] [SellerCenterPage.xaml.cs](file:///c:/AntigravityProjects/Lazada_Isagunde/Pages/SellerCenterPage.xaml.cs)
- Update referencing code to check `JewelryCb` and return `"Jewelry"`.

## Verification Plan

### Automated/Compilation Verification
- Build Blazor app and MAUI app to verify no syntax errors.

### Manual Verification
- Launch both apps and click categories. Verify that the header changes matching the category clicked.
- Try adding a product in Seller Center with the "Jewelry" category and verify it filters under the "Jewelry" category ribbon.
