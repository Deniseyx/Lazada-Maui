# MAUI Category Selection Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a horizontal category selector to the MAUI Dashboard that filters products, mirroring the Blazor functionality and optimized for small screens.

**Architecture:** Maintain a master list of products (`_allProducts`) and filter the visible `ObservableCollection` based on `_selectedCategory` and search text.

**Tech Stack:** .NET MAUI, XAML, C#.

---

### Task 1: Update Dashboard Logic

**Files:**
- Modify: `Pages/DashboardPage.xaml.cs`

- [ ] **Step 1: Add state variables and initialization**

Update the class fields and constructor to handle the master list and selected category:

```csharp
namespace Lazada_Isagunde.Pages;

public partial class DashboardPage : ContentPage
{
    private readonly FirebaseService _firebaseService;
    private ObservableCollection<Product> _visibleProducts = new();
    private List<Product> _allProducts = new();
    private string _selectedCategory = "General";
    private string _searchText = "";

    public DashboardPage()
    {
        InitializeComponent();
        _firebaseService = App.Services.GetService<FirebaseService>()!;
        
        ProductsCollection.ItemsSource = _visibleProducts;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProducts();
    }
```

- [ ] **Step 2: Update LoadProducts and Add FilterProducts**

Implement the logic to load all approved products into the master list and apply the filter:

```csharp
    private async Task LoadProducts()
    {
        try
        {
            var products = await _firebaseService.GetProductsAsync();
            _allProducts = products.Where(p => p.Status == "Approved").ToList();
            ApplyFilter();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load products: {ex.Message}", "OK");
        }
    }

    private void ApplyFilter()
    {
        var filtered = _allProducts.Where(p => 
            (_selectedCategory == "General" || p.Category == _selectedCategory) &&
            (string.IsNullOrWhiteSpace(_searchText) || p.Title.Contains(_searchText, StringComparison.OrdinalIgnoreCase))
        ).ToList();

        _visibleProducts.Clear();
        foreach (var product in filtered)
        {
            _visibleProducts.Add(product);
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        _searchText = e.NewTextValue ?? "";
        ApplyFilter();
    }

    private void OnCategoryTapped(object sender, EventArgs e)
    {
        if (sender is BindableObject view && view.BindingContext is string category)
        {
            _selectedCategory = category;
            
            // Visual feedback: we'll handle this via data binding or manual update
            // For now, just trigger filter
            ApplyFilter();
            
            // Re-render categories to show selection (handled in XAML via triggers or code)
            UpdateCategoryUI();
        }
    }

    private void UpdateCategoryUI()
    {
        // Find all category items and update their visual state
        if (CategoryList == null) return;
        foreach (var child in CategoryList.Children)
        {
            if (child is VisualElement ve && ve.BindingContext is string cat)
            {
                VisualStateManager.GoToState(ve, cat == _selectedCategory ? "Selected" : "Normal");
            }
        }
    }
```

- [ ] **Step 3: Commit**

```bash
git add Pages/DashboardPage.xaml.cs
git commit -m "feat: add category filtering logic to MAUI Dashboard"
```

---

### Task 2: Add Category Strip UI

**Files:**
- Modify: `Pages/DashboardPage.xaml`

- [ ] **Step 1: Define Category Strip Markup**

Insert the `ScrollView` category ribbon between the search bar and the "Just For You" header. Add the category list and styles:

```xml
                <!-- Search Bar -->
                <Frame BackgroundColor="White" Padding="0" HasShadow="False" CornerRadius="10" HeightRequest="45">
                    <Entry x:Name="SearchEntry" 
                           Placeholder="Search products..." 
                           Margin="10,0" 
                           TextColor="Black"
                           TextChanged="OnSearchTextChanged" />
                </Frame>

                <!-- Category Ribbon -->
                <ScrollView Orientation="Horizontal" HorizontalScrollBarVisibility="Never">
                    <HorizontalStackLayout x:Name="CategoryList" Spacing="15" Padding="5,0">
                        <!-- We'll manually add these in XAML for easy styling or use a BindableLayout -->
                    </HorizontalStackLayout>
                </ScrollView>
```

- [ ] **Step 2: Add Category Items with Visual States**

Populate the `CategoryList` with the specific categories and icons:

```xml
    <ScrollView Orientation="Horizontal" HorizontalScrollBarVisibility="Never">
        <HorizontalStackLayout x:Name="CategoryList" Spacing="15" Padding="5,0">
            <HorizontalStackLayout.Resources>
                <Style TargetType="VerticalStackLayout">
                    <Setter Property="VisualStateManager.VisualStateGroups">
                        <VisualStateGroupList>
                            <VisualStateGroup x:Name="CommonStates">
                                <VisualState x:Name="Normal">
                                    <VisualState.Setters>
                                        <Setter Property="Opacity" Value="0.6" />
                                        <Setter TargetName="Indicator" Property="IsVisible" Value="False" />
                                    </VisualState.Setters>
                                </VisualState>
                                <VisualState x:Name="Selected">
                                    <VisualState.Setters>
                                        <Setter Property="Opacity" Value="1.0" />
                                        <Setter TargetName="Indicator" Property="IsVisible" Value="True" />
                                    </VisualState.Setters>
                                </VisualState>
                            </VisualStateGroup>
                        </VisualStateGroupList>
                    </Setter>
                </Style>
            </HorizontalStackLayout.Resources>

            <!-- Category Template Helper (repeated for each category) -->
            <!-- General -->
            <VerticalStackLayout BindingContext="{x:Static x:String.Empty}" x:Name="CatGeneral">
                <VerticalStackLayout.GestureRecognizers>
                    <TapGestureRecognizer Tapped="OnCategoryTapped" CommandParameter="General" />
                </VerticalStackLayout.GestureRecognizers>
                <Frame HeightRequest="50" WidthRequest="50" CornerRadius="25" Padding="0" HasShadow="False" BackgroundColor="White">
                    <Label Text="🛒" HorizontalOptions="Center" VerticalOptions="Center" FontSize="24" />
                </Frame>
                <Label Text="General" HorizontalOptions="Center" FontSize="11" TextColor="Black" />
                <BoxView x:Name="Indicator" HeightRequest="2" Color="{StaticResource LazadaOrange}" Margin="5,2" IsVisible="True" />
            </VerticalStackLayout>
            
            <!-- Repeat for Electronics (💻), Fashion (✨), Home (🏠), Health (💊), Male (👕), Female (👗), Kids (🧸) -->
            <!-- I will generate the full list in the implementation step -->
        </HorizontalStackLayout>
    </ScrollView>
```

- [ ] **Step 3: Update code-behind to initialize categories**

Ensure `UpdateCategoryUI` is called after load.

- [ ] **Step 4: Verify build**

Run: `dotnet build Lazada_Isagunde.csproj`
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add Pages/DashboardPage.xaml Pages/DashboardPage.xaml.cs
git commit -m "ui: add horizontal category ribbon to MAUI Dashboard"
```
