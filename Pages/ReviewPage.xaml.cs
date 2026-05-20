using Lazada_Isagunde.Models;
using Lazada_Isagunde.Services;

namespace Lazada_Isagunde.Pages;

public partial class ReviewPage : ContentPage
{
    private readonly FirebaseService _firebaseService;
    private readonly Order _order;
    private double _currentRating = 0;

    public ReviewPage(Order order)
    {
        InitializeComponent();
        _firebaseService = App.Services.GetService<FirebaseService>()!;
        _order = order;
        
        ProductTitleLabel.Text = order.ItemsSummary;
    }

    private void OnStarClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && double.TryParse(btn.CommandParameter?.ToString(), out double rating))
        {
            _currentRating = rating;
            UpdateStarDisplay();
            RatingTextLabel.Text = GetRatingText(rating);
            SubmitButton.IsEnabled = true;
        }
    }

    private void UpdateStarDisplay()
    {
        var stars = new[] { Star1, Star2, Star3, Star4, Star5 };
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].TextColor = (i < _currentRating) ? Color.FromArgb("#F36F21") : Color.FromArgb("#C8C8C8");
        }
    }

    private string GetRatingText(double rating)
    {
        return rating switch
        {
            1 => "Terrible",
            2 => "Poor",
            3 => "Average",
            4 => "Good",
            5 => "Excellent",
            _ => "Tap a star to rate"
        };
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        if (_currentRating <= 0) return;

        try
        {
            SubmitButton.IsEnabled = false;

            string buyerName = "Customer";
            if (AuthService.IsAdmin)
            {
                buyerName = "Admin User";
            }
            else if (!string.IsNullOrEmpty(AuthService.UserId))
            {
                try
                {
                    var profile = await _firebaseService.GetUserProfileAsync(AuthService.UserId);
                    if (profile != null && !string.IsNullOrEmpty(profile.FullName))
                    {
                        buyerName = profile.FullName;
                    }
                    else
                    {
                        buyerName = AuthService.UserDisplayName ?? AuthService.UserEmail ?? "User_" + AuthService.UserId.Substring(0, 4);
                    }
                }
                catch
                {
                    buyerName = AuthService.UserDisplayName ?? AuthService.UserEmail ?? "Customer";
                }
            }
            
            // Review each item in the order
            foreach (var item in _order.Items)
            {
                var review = new Review
                {
                    ProductId = item.ProductId,
                    BuyerId = AuthService.UserId ?? "Unknown",
                    BuyerName = buyerName,
                    Rating = _currentRating,
                    Comment = ReviewEditor.Text ?? string.Empty,
                    Timestamp = DateTime.UtcNow
                };

                await _firebaseService.SubmitReviewAsync(review);
            }

            // Mark order as reviewed
            await _firebaseService.UpdateOrderReviewStatusAsync(_order.Id, true);

            await DisplayAlert("Success", "Thank you for your review!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to submit review: " + ex.Message, "OK");
            SubmitButton.IsEnabled = true;
        }
    }
}
