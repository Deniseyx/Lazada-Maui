using Lazada_Isagunde.Models;
using Lazada_Isagunde.Services;
using System.Collections.ObjectModel;

namespace Lazada_Isagunde.Pages;

public partial class CartPage : ContentPage
{
    private readonly FirebaseService _firebaseService;
    public ObservableCollection<CartItem> Items { get; set; } = new();

    public CartPage()
    {
        InitializeComponent();
        _firebaseService = App.Services.GetService<FirebaseService>()!;
        CartCollection.ItemsSource = Items;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCart();
    }

    private async Task LoadCart()
    {
        if (string.IsNullOrEmpty(AuthService.UserId)) return;

        var items = await _firebaseService.GetCartItemsAsync(AuthService.UserId);
        Items.Clear();
        foreach (var item in items) Items.Add(item);

        UpdateTotal();
        EmptyCartState.IsVisible = Items.Count == 0;
        CartCollection.IsVisible = Items.Count > 0;
    }

    private void UpdateTotal()
    {
        double total = Items.Sum(i => i.Price * i.Quantity);
        TotalPriceLabel.Text = $"₱{total:N0}";
    }

    private async void OnIncreaseQuantity(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is CartItem item)
        {
            var product = await _firebaseService.GetProductByIdAsync(item.ProductId);
            if (product != null && item.Quantity >= product.Stock)
            {
                await DisplayAlert("Limit Reached", $"Only {product.Stock} items available in stock.", "OK");
                return;
            }

            item.Quantity++;
            await _firebaseService.AddToCartAsync(AuthService.UserId, item);
            UpdateTotal();
            // CollectionView might need a nudge to refresh if not using INotifyPropertyChanged on CartItem
            CartCollection.ItemsSource = null;
            CartCollection.ItemsSource = Items;
        }
    }

    private async void OnDecreaseQuantity(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is CartItem item && item.Quantity > 1)
        {
            item.Quantity--;
            await _firebaseService.AddToCartAsync(AuthService.UserId, item);
            UpdateTotal();
            CartCollection.ItemsSource = null;
            CartCollection.ItemsSource = Items;
        }
    }

    private async void OnRemoveItem(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is CartItem item)
        {
            await _firebaseService.RemoveFromCartAsync(AuthService.UserId, item.ProductId);
            Items.Remove(item);
            UpdateTotal();
            EmptyCartState.IsVisible = Items.Count == 0;
        }
    }

    private async void OnGoShoppingClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

    private async void OnCheckoutClicked(object sender, EventArgs e)
    {
        if (Items.Count == 0) return;

        // For this version, we checkout ALL items in the cart
        await Navigation.PushAsync(new PurchasePage(Items.ToList()));
    }
}
