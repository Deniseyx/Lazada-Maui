using Lazada_Isagunde.Models;
using Lazada_Isagunde.Services;

namespace Lazada_Isagunde.Pages;

public partial class ProductDetailPage : ContentPage
{
    private readonly Product _product;
    private readonly FirebaseService _firebaseService;

    public ProductDetailPage(Product product)
    {
        InitializeComponent();
        _product = product;
        _firebaseService = App.Services.GetService<FirebaseService>()!;

        BindingContext = _product;
        LoadProductDetails();
        CheckOwnership();
        LoadReviews();
    }

    private void LoadProductDetails()
    {
        ProductImage.Source = _product.DisplayImageUrl;
        PriceLabel.Text = _product.PriceFormatted;
        TitleLabel.Text = _product.Title;
        DescriptionLabel.Text = _product.Description;
        SellerNameLabel.Text = string.IsNullOrEmpty(_product.SellerName) ? "Official Store" : _product.SellerName;
        RatingLabel.Text = _product.Rating.ToString("F1");
        StockLabel.Text = $"{_product.Stock} items available";

        if (_product.Stock <= 0)
        {
            BuyNowButton.IsEnabled = false;
            AddToCartButton.IsEnabled = false;
            BuyNowButton.Text = "Out of Stock";
            BuyNowButton.BackgroundColor = Colors.Gray;
        }
    }

    private async void LoadReviews()
    {
        try
        {
            var reviews = await _firebaseService.GetReviewsForProductAsync(_product.Id);
            ReviewsCollection.ItemsSource = reviews;
            NoReviewsState.IsVisible = reviews.Count == 0;
            
            if (reviews.Count > 0)
            {
                double avg = reviews.Average(r => r.Rating);
                RatingLabel.Text = avg.ToString("F1");
            }
        }
        catch (Exception)
        {
            // Silent error for reviews
        }
    }

    private void CheckOwnership()
    {
        if (_product.SellerId == AuthService.UserId)
        {
            BuyNowButton.IsVisible = false;
            AddToCartButton.IsVisible = false;
            OwnerMessageLabel.IsVisible = true;
        }
    }

    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(AuthService.UserId))
        {
            await DisplayAlert("Login Required", "Please login to add items to cart.", "OK");
            return;
        }

        var cartItem = new CartItem
        {
            ProductId = _product.Id,
            Title = _product.Title,
            ImageUrl = _product.ImageUrl,
            Price = _product.Price,
            Quantity = 1,
            SellerId = _product.SellerId,
            SellerName = string.IsNullOrEmpty(_product.SellerName) ? "Official Store" : _product.SellerName
        };

        await _firebaseService.AddToCartAsync(AuthService.UserId, cartItem);
        await DisplayAlert("Success", "Added to cart!", "OK");
    }

    private async void OnBuyNowClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(AuthService.UserId))
        {
            await DisplayAlert("Login Required", "Please login to purchase items.", "OK");
            return;
        }

        // Direct buy logic: navigate to PurchasePage with this single item
        var cartItem = new CartItem
        {
            ProductId = _product.Id,
            Title = _product.Title,
            ImageUrl = _product.ImageUrl,
            Price = _product.Price,
            Quantity = 1,
            SellerId = _product.SellerId,
            SellerName = string.IsNullOrEmpty(_product.SellerName) ? "Official Store" : _product.SellerName
        };

        await Navigation.PushAsync(new PurchasePage(new List<CartItem> { cartItem }));
    }
}
