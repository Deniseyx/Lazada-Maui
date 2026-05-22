# Design: MAUI Category Selection and Filtering

Add a mobile-optimized horizontal category selector to the Lazada MAUI application to allow users to filter products on the Dashboard.

## Goals
- Provide a native mobile experience for browsing products by category.
- Match the category set and "General" default logic of the Blazor application.
- Optimize for small mobile screens by saving vertical space.

## Architecture & Logic
- **State Management**: A `string _selectedCategory` field in `DashboardPage.xaml.cs`.
- **Initial State**: Defaults to `"General"`.
- **Filtering Logic**: The `ProductsCollection.ItemsSource` will be updated whenever `_selectedCategory` changes, intersecting with any active search query.
- **UI Component**: A horizontal `ScrollView` containing a `HorizontalStackLayout` of category items.

## Categories
The selector will include:
1. General (Default) - 🛒
2. Electronics - 💻
3. Fashion - ✨
4. Home - 🏠
5. Health - 💊
6. Male Clothes - 👕
7. Female Clothes - 👗
8. Kids - 🧸

## UI Design (Native Category Ribbon)
- **Structure**: Each category item consists of a circular icon container (emoji) and a label.
- **Visual Feedback**: The active category will be highlighted with a themed background or a bottom border.
- **Interactivity**: Tapping an item triggers the `OnCategoryTapped` event, updating the filter.
- **Responsiveness**: Uses a horizontal `ScrollView` to handle any screen width gracefully.

## Implementation Details
1. **DashboardPage.xaml**:
   - Insert the `ScrollView` category ribbon between the search bar and the "Just For You" header.
   - Define styles for `CategoryIconCircle` and `CategoryLabel`.
2. **DashboardPage.xaml.cs**:
   - Add `_selectedCategory` and an `ObservableCollection<Product> _allProducts` to keep a master list for filtering.
   - Implement `LoadProducts` and `FilterProducts` methods.
   - Handle `Tapped` events for category items.

## Testing & Verification
- Verify horizontal scrolling works smoothly.
- Verify tapping a category (e.g., "Fashion") updates the product grid instantly.
- Verify the "General" category shows all approved products.
- Verify the selector looks balanced on both small phones and larger tablets.
