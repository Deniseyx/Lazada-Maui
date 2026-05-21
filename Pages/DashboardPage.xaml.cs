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
            ApplyFilter();
            UpdateCategoryUI();
        }
        else if (sender is View v && v.GestureRecognizers.FirstOrDefault() is TapGestureRecognizer tgr && tgr.CommandParameter is string cat)
        {
            // Alternative way if BindingContext isn't set yet
            _selectedCategory = cat;
            ApplyFilter();
            UpdateCategoryUI();
        }
    }

    private void UpdateCategoryUI()
    {
        // CategoryList is added in Task 2 XAML updates
        // This method will be expanded once the UI element exists
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
