using Lazada_Isagunde.Models;
using Lazada_Isagunde.Services;

namespace Lazada_Isagunde.Pages;

public partial class PurchasePage : ContentPage
{
    private readonly List<CartItem> _items;
    private readonly FirebaseService _firebaseService;
    private UserProfile _userProfile;

    public PurchasePage(List<CartItem> items)
    {
        InitializeComponent();
        _items = items;
        _firebaseService = App.Services.GetService<FirebaseService>()!;
        
        ItemsSummaryCollection.ItemsSource = _items;
        CalculateTotals();
        LoadUserProfile();

        UseSavedAddressCb.CheckedChanged += (s, e) => {
            CustomAddressSection.IsVisible = !e.Value;
            SavedAddressDisplay.IsVisible = e.Value;
        };
    }

    private void CalculateTotals()
    {
        double subtotal = _items.Sum(i => i.Price * i.Quantity);
        
        // Shipping fee logic: ₱80 per unique Seller
        var uniqueSellers = _items.Select(i => i.SellerId).Distinct().Count();
        double shippingFee = uniqueSellers * 80;
        
        double total = subtotal + shippingFee;

        SubtotalLabel.Text = $"₱{subtotal:N0}";
        ShippingFeeLabel.Text = $"₱{shippingFee:N0} ({uniqueSellers} Seller/s)";
        TotalPaymentLabel.Text = $"₱{total:N0}";
    }

    private async void LoadUserProfile()
    {
        if (string.IsNullOrEmpty(AuthService.UserId)) return;
        
        _userProfile = await _firebaseService.GetUserProfileAsync(AuthService.UserId);
        
        SavedNameLabel.Text = string.IsNullOrEmpty(_userProfile.FullName) ? "Name not set" : _userProfile.FullName;
        SavedPhoneLabel.Text = string.IsNullOrEmpty(_userProfile.PhoneNumber) ? "Phone not set" : _userProfile.PhoneNumber;
        SavedAddressLabel.Text = string.IsNullOrEmpty(_userProfile.ShippingAddress) ? "Address not set" : _userProfile.ShippingAddress;

        if (string.IsNullOrEmpty(_userProfile.FullName))
        {
            UseSavedAddressCb.IsChecked = false;
        }
    }

    private async void OnPlaceOrderClicked(object sender, EventArgs e)
    {
        string name = UseSavedAddressCb.IsChecked ? _userProfile.FullName : CustomNameEntry.Text;
        string phone = UseSavedAddressCb.IsChecked ? _userProfile.PhoneNumber : CustomPhoneEntry.Text;
        string address = UseSavedAddressCb.IsChecked ? _userProfile.ShippingAddress : CustomAddressEditor.Text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
        {
            await DisplayAlert("Error", "Please provide shipping details.", "OK");
            return;
        }

        try
        {
            // Split items by seller to create separate orders
            var itemsBySeller = _items.GroupBy(i => i.SellerId);

            foreach (var group in itemsBySeller)
            {
                var sellerId = group.Key;
                var sellerItems = group.ToList();
                double sellerSubtotal = sellerItems.Sum(i => i.Price * i.Quantity);

                var order = new Order
                {
                    BuyerId = AuthService.UserId,
                    SellerId = sellerId,
                    SellerName = sellerItems[0].SellerName,
                    Items = sellerItems,
                    Subtotal = sellerSubtotal,
                    ShippingFee = 80, // ₱80 per seller
                    TotalPrice = sellerSubtotal + 80,
                    OrderDate = DateTime.Now,
                    Status = "Pending",
                    PaymentMethod = GetSelectedPaymentMethod(),
                    ReceiverName = name,
                    ReceiverPhone = phone,
                    ShippingAddress = address
                };

                await _firebaseService.PlaceOrderAsync(order);
            }

            // Clear items from cart that were just bought
            foreach (var item in _items)
            {
                await _firebaseService.RemoveFromCartAsync(AuthService.UserId, item.ProductId);
            }

            await DisplayAlert("Success", "Order placed successfully!", "OK");
            await Navigation.PushAsync(new PurchaseHistoryPage());
            Navigation.RemovePage(this);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to place order: " + ex.Message, "OK");
        }
    }

    private string GetSelectedPaymentMethod()
    {
        if (GcashRb.IsChecked) return "GCash";
        if (CardRb.IsChecked) return "Card";
        return "COD";
    }
}
