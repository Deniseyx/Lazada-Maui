using Lazada_Isagunde.Services;
using Lazada_Isagunde.Models;
using System.Collections.ObjectModel;

namespace Lazada_Isagunde.Pages;

public partial class AdminDashboardPage : ContentPage
{
    private readonly FirebaseService _firebaseService;
    private readonly AuthService _authService;
    public ObservableCollection<Product> PendingProducts { get; set; } = new();

    public AdminDashboardPage()
    {
        InitializeComponent();
        _firebaseService = App.Services.GetService<FirebaseService>()!;
        _authService = App.Services.GetService<AuthService>()!;
        PendingList.ItemsSource = PendingProducts;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPendingProducts();
    }

    private async Task LoadPendingProducts()
    {
        try
        {
            var products = await _firebaseService.GetProductsAsync("Pending");
            PendingProducts.Clear();
            foreach (var p in products)
            {
                PendingProducts.Add(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to load pending products: " + ex.Message, "OK");
        }
    }

    private async void OnRefreshing(object sender, EventArgs e)
    {
        await LoadPendingProducts();
        RefreshView.IsRefreshing = false;
    }

    private async void OnApproveClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Product product)
        {
            bool confirm = await DisplayAlert("Approve", $"Approve {product.Title}?", "Yes", "No");
            if (confirm)
            {
                await _firebaseService.UpdateProductStatusAsync(product.Id, "Approved");
                PendingProducts.Remove(product);
                await DisplayAlert("Success", "Product approved and is now live!", "OK");
            }
        }
    }

    private async void OnRejectClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Product product)
        {
            string reason = await DisplayActionSheet("Reject Reason", "Cancel", null, "Inappropriate Content", "Missing Details", "Incorrect Category", "Other");
            if (reason != "Cancel" && reason != null)
            {
                await _firebaseService.UpdateProductStatusAsync(product.Id, "Rejected");
                PendingProducts.Remove(product);
                await DisplayAlert("Rejected", "Product has been rejected.", "OK");
            }
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        _authService.Logout();
        if (Application.Current != null)
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
