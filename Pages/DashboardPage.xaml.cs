using Lazada_Isagunde.Services;
using System.Collections.ObjectModel;
using Lazada_Isagunde.Models;

namespace Lazada_Isagunde.Pages;

public partial class DashboardPage : ContentPage
{
    private readonly FirebaseService _firebaseService;
    public ObservableCollection<Product> Products { get; set; } = new();

	public DashboardPage()
	{
		InitializeComponent();
        _firebaseService = App.Services.GetService<FirebaseService>()!;
        ProductsCollection.ItemsSource = Products;
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
            var items = await _firebaseService.GetProductsAsync("Approved");
            Products.Clear();
            foreach (var item in items)
            {
                Products.Add(item);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to load products: " + ex.Message, "OK");
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
