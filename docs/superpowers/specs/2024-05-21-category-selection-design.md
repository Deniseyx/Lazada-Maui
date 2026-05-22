# Design: Category Selection and Filtering

Add a visually appealing category selector to the Lazada Blazor application to allow users to filter products on the Dashboard and Landing pages.

## Goals
- Provide a clear way for users to browse products by category.
- Default to the "General" category upon initial load/login.
- Maintain a clean, professional "Lazada-style" UI.

## Architecture & Logic
- **State Management**: A `selectedCategory` string variable will be added to the code-behind of relevant pages.
- **Initial State**: The variable will be initialized to `"General"`.
- **Filtering Logic**: The `FilteredProducts` computed property will be updated to filter based on both the search query (if any) and the `selectedCategory`.
- **UI Component**: A "Category Strip" consisting of a horizontal row of items, each containing an icon (emoji or Bootstrap icon) and a label.

## Categories
The selector will include the following predefined categories:
1. General (Default)
2. Electronics
3. Fashion
4. Home
5. Health
6. Male Clothes
7. Female Clothes
8. Kids

## UI Design (Approach A: Modern Row)
- **Visuals**: Each category will be represented by an icon above text.
- **Interactivity**: Clicking a category updates the `selectedCategory` state and triggers a re-render.
- **Highlight**: An orange underline (Lazada's primary color) will indicate the currently active category.
- **Responsiveness**: The row will be horizontally scrollable on smaller screens to ensure it doesn't break the layout.

## Implementation Details
1. **Model Update**: Ensure `Product.cs` category names match the UI labels exactly.
2. **Dashboard.razor**:
   - Insert the category strip above the "Just For You" header.
   - Update `OnInitializedAsync` to default the state.
   - Update the filter logic to handle the intersection of search and category.
3. **Landing.razor**:
   - Implement the same strip for guest browsing.
4. **Styling**: Add CSS for `.category-strip`, `.category-item`, and active states in `app.css` or scoped Razor styles.

## Testing & Verification
- Verify that clicking "Electronics" only shows electronic products.
- Verify that the page defaults to "General" after a fresh login.
- Verify that the active category underline moves correctly when a new selection is made.
- Verify that the search bar still works in conjunction with the category filter.
