using Lazada_Isagunde.Models;
using Lazada_Isagunde.Services;
using System.Collections.ObjectModel;

namespace Lazada_Isagunde.Pages;

public partial class PurchaseHistoryPage : ContentPage
{
    private readonly FirebaseService _firebaseService;
    public ObservableCollection<Order> Orders { get; set; } = new();

    public PurchaseHistoryPage()
    {
        InitializeComponent();
        _firebaseService = App.Services.GetService<FirebaseService>()!;
        OrdersCollection.ItemsSource = Orders;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadOrders();
    }

    private async Task LoadOrders()
    {
        if (string.IsNullOrEmpty(AuthService.UserId)) return;

        try
        {
            var items = await _firebaseService.GetUserOrdersAsync(AuthService.UserId);
            Orders.Clear();
            foreach (var item in items.OrderByDescending(o => o.OrderDate))
            {
                Orders.Add(item);
            }

            EmptyHistoryState.IsVisible = Orders.Count == 0;
            OrdersCollection.IsVisible = Orders.Count > 0;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to load orders: " + ex.Message, "OK");
        }
    }

    private async void OnStartShoppingClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
