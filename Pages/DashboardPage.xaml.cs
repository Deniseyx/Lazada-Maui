using Lazada_Isagunde.Services;
using System.Collections.ObjectModel;
using Lazada_Isagunde.Models;
using Microsoft.Maui.Controls;
using System.Linq;

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

    private async Task LoadProducts()
    {
        try
        {
            var products = await _firebaseService.GetProductsAsync();
            
            // Run database migration from Fashion to Jewelry
            bool migrated = false;
            foreach (var p in products)
            {
                if (string.Equals(p.Category, "Fashion", StringComparison.OrdinalIgnoreCase))
                {
                    p.Category = "Jewelry";
                    await _firebaseService.UpdateProductAsync(p);
                    migrated = true;
                }
            }
            if (migrated)
            {
                // Reload after migrating
                products = await _firebaseService.GetProductsAsync();
            }

            _allProducts = products.Where(p => p.Status == "Approved").ToList();
            ApplyFilter();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load products: {ex.Message}", "OK");
        }
    }

    private string GetCategoryDisplayName(string category) => category switch
    {
        "Male Clothes" => "Male",
        "Female Clothes" => "Female",
        _ => category
    };

    private void ApplyFilter()
    {
        if (SectionHeaderLabel != null)
        {
            SectionHeaderLabel.Text = GetCategoryDisplayName(_selectedCategory);
        }

        var filtered = _allProducts.Where(p => 
            (string.Equals(_selectedCategory, "General", StringComparison.OrdinalIgnoreCase) || 
             string.Equals(p.Category, _selectedCategory, StringComparison.OrdinalIgnoreCase)) &&
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
        string category = "";
        
        if (sender is View view && view.GestureRecognizers.Count > 0)
        {
            var tgr = view.GestureRecognizers[0] as TapGestureRecognizer;
            category = tgr?.CommandParameter as string ?? "";
        }

        if (string.IsNullOrEmpty(category)) return;

        _selectedCategory = category;
        ApplyFilter();
        UpdateCategoryUI();
    }

    private void UpdateCategoryUI()
    {
        if (CategoryStrip == null) return;

        foreach (var child in CategoryStrip.Children)
        {
            if (child is VerticalStackLayout vsl && vsl.GestureRecognizers.Count > 0)
            {
                var tgr = vsl.GestureRecognizers[0] as TapGestureRecognizer;
                string cat = tgr?.CommandParameter as string ?? "";
                
                bool isSelected = cat == _selectedCategory;
                VisualStateManager.GoToState(vsl, isSelected ? "Selected" : "Normal");
                
                // Toggle the indicator BoxView visibility
                var indicator = vsl.Children.OfType<BoxView>().FirstOrDefault();
                if (indicator != null)
                {
                    indicator.IsVisible = isSelected;
                }
            }
        }
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Profile");
    }

    private async void OnProductTapped(object sender, EventArgs e)
    {
        if (e is TappedEventArgs tapped && tapped.Parameter is Product product)
        {
            await Shell.Current.Navigation.PushAsync(new ProductDetailPage(product));
        }
    }

    private async void OnCartClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushAsync(new CartPage());
    }
}
