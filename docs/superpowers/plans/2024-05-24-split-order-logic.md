# Split Orders per Product Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Refactor order placement logic to split each product into its own order record while maintaining per-seller shipping fees.

**Architecture:** Update `OnPlaceOrderClicked` in `PurchasePage.xaml.cs` to iterate through each `CartItem` and create a separate `Order`. Use a `HashSet` to ensure shipping fees are only applied once per unique seller.

**Tech Stack:** .NET MAUI, C#, FirebaseService

---

### Task 1: Refactor OnPlaceOrderClicked in PurchasePage.xaml.cs

**Files:**
- Modify: `Pages/PurchasePage.xaml.cs`

- [ ] **Step 1: Replace grouping logic with per-item iteration and shipping tracking**

Replace the current `OnPlaceOrderClicked` logic with the new implementation.

```csharp
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
            // Track which sellers have already been charged shipping in this transaction
            var sellersChargedShipping = new HashSet<string>();

            // Every unique product (CartItem) becomes its own order
            foreach (var item in _items)
            {
                double shippingFee = 0;
                if (!sellersChargedShipping.Contains(item.SellerId))
                {
                    shippingFee = 80; // ₱80 per seller
                    sellersChargedShipping.Add(item.SellerId);
                }

                var order = new Order
                {
                    BuyerId = AuthService.UserId,
                    SellerId = item.SellerId,
                    SellerName = item.SellerName,
                    Items = new List<CartItem> { item }, // Only this specific product
                    Subtotal = item.Price * item.Quantity,
                    ShippingFee = shippingFee,
                    TotalPrice = (item.Price * item.Quantity) + shippingFee,
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
```

- [ ] **Step 2: Verify logic via manual inspection**
Ensure `HashSet<string>` correctly tracks `SellerId` to avoid duplicate shipping fees.

- [ ] **Step 3: Build the project**

Run: `dotnet build`
Expected: Build SUCCESS.

- [ ] **Step 4: Commit changes**

```bash
git add Pages/PurchasePage.xaml.cs
git commit -m "feat: split orders per product and handle per-seller shipping fees"
```
