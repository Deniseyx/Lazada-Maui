using Lazada_Isagunde.Services;
using Lazada_Isagunde.Models;
using System.Collections.ObjectModel;

namespace Lazada_Isagunde.Pages;

public partial class SellerCenterPage : ContentPage
{
    private readonly FirebaseService _firebaseService;
    private readonly ImageService _imageService;
    private string _selectedImagePath;

    public ObservableCollection<Product> MyProducts { get; set; } = new();
    public ObservableCollection<Order> MyOrders { get; set; } = new();

    public SellerCenterPage()
    {
        InitializeComponent();
        _firebaseService = App.Services.GetService<FirebaseService>()!;
        _imageService = App.Services.GetService<ImageService>()!;
        
        ListingsCollection.ItemsSource = MyProducts;
        OrdersCollection.ItemsSource = MyOrders;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadData();
    }

    private async Task LoadData()
    {
        // Load Listings
        var userId = AuthService.UserId;
        var products = await _firebaseService.GetProductsAsync();
        MyProducts.Clear();
        foreach (var p in products.Where(x => x.SellerId == userId)) MyProducts.Add(p);

        // Load Seller Orders
        if (!string.IsNullOrEmpty(userId))
        {
            var orders = await _firebaseService.GetSellerOrdersAsync(userId);
            MyOrders.Clear();
            foreach (var o in orders.OrderByDescending(x => x.OrderDate)) MyOrders.Add(o);
        }
    }

    #region Tab Navigation
    private void OnCreateTabClicked(object sender, EventArgs e) => SwitchTab("Create");
    private void OnOrdersTabClicked(object sender, EventArgs e) => SwitchTab("Orders");
    private void OnListingsTabClicked(object sender, EventArgs e) => SwitchTab("Listings");

    private void SwitchTab(string tab)
    {
        CreateSection.IsVisible = tab == "Create";
        OrdersSection.IsVisible = tab == "Orders";
        ListingsSection.IsVisible = tab == "Listings";

        UpdateTabStyle(CreateTabLabel, CreateTabIndicator, tab == "Create");
        UpdateTabStyle(OrdersTabLabel, OrdersTabIndicator, tab == "Orders");
        UpdateTabStyle(ListingsTabLabel, ListingsTabIndicator, tab == "Listings");
    }

    private void UpdateTabStyle(Label label, BoxView indicator, bool isActive)
    {
        label.TextColor = isActive ? (Color)Application.Current.Resources["LazadaOrange"] : (Color)Application.Current.Resources["Gray500"];
        label.FontAttributes = isActive ? FontAttributes.Bold : FontAttributes.None;
        indicator.Color = isActive ? (Color)Application.Current.Resources["LazadaOrange"] : Colors.Transparent;
    }
    #endregion

    #region Create Product
    private async void OnPickImageClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Please select a product image",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                _selectedImagePath = result.FullPath;
                SelectedImage.Source = ImageSource.FromFile(_selectedImagePath);
                SelectedImage.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnPublishClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ProductNameEntry.Text) || string.IsNullOrEmpty(ProductPriceEntry.Text))
        {
            await DisplayAlert("Error", "Please fill name and price.", "OK");
            return;
        }

        try
        {
            string imageUrl = "https://via.placeholder.com/150"; // Default
            if (!string.IsNullOrEmpty(_selectedImagePath))
            {
                imageUrl = await _imageService.UploadImageAsync(_selectedImagePath);
            }

            var profile = await _firebaseService.GetUserProfileAsync(AuthService.UserId);

            var newProduct = new Product
            {
                Title = ProductNameEntry.Text,
                Description = ProductDescriptionEditor.Text,
                Price = double.Parse(ProductPriceEntry.Text),
                Stock = int.TryParse(ProductStockEntry.Text, out int stock) ? stock : 10,
                ImageUrl = imageUrl,
                Category = GetSelectedCategory(),
                SellerId = AuthService.UserId,
                SellerName = string.IsNullOrEmpty(profile.FullName) ? "Official Store" : profile.FullName
            };

            await _firebaseService.AddProductAsync(newProduct);
            await DisplayAlert("Success", "Added successfully, awaiting admin approval", "OK");
            
            // Reset form
            ProductNameEntry.Text = string.Empty;
            ProductDescriptionEditor.Text = string.Empty;
            ProductPriceEntry.Text = string.Empty;
            ProductStockEntry.Text = string.Empty;
            SelectedImage.IsVisible = false;
            _selectedImagePath = null;

            SwitchTab("Listings");
            await LoadData();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private string GetSelectedCategory()
    {
        if (ElectronicsCb.IsChecked) return "Electronics";
        if (FashionCb.IsChecked) return "Fashion";
        if (HomeCb.IsChecked) return "Home";
        if (HealthCb.IsChecked) return "Health";
        if (ClothesMaleCb.IsChecked) return "Male Clothes";
        if (ClothesFemaleCb.IsChecked) return "Female Clothes";
        if (KidsCb.IsChecked) return "Kids";
        return "General";
    }
    #endregion

    #region Orders
    private async void OnUpdateStatusClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Order order)
        {
            string newStatus = btn.CommandParameter.ToString();
            await _firebaseService.UpdateOrderStatusAsync(order.Id, newStatus);
            await DisplayAlert("Success", $"Order marked as {newStatus}", "OK");
            await LoadData();
        }
    }
    #endregion

    #region Listings
    private async void OnDeleteProductClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Product product)
        {
            bool confirm = await DisplayAlert("Delete", $"Are you sure you want to delete {product.Title}?", "Yes", "No");
            if (confirm)
            {
                // In a real app, you'd have a DeleteProductAsync in FirebaseService
                // For now, we just remove it locally as a placeholder if the service doesn't support it
                MyProducts.Remove(product);
                await DisplayAlert("Deleted", "Product removed.", "OK");
            }
        }
    }
    #endregion
}
