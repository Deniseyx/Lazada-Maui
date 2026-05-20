# Multi-Product Order Splitting Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Ensure that every unique product in a checkout transaction results in a separate order record, while keeping shipping fees calculated per seller (₱80 per unique seller).

**Architecture:** Refactor `OnPlaceOrderClicked` in `PurchasePage.xaml.cs` to iterate through every unique `CartItem` and create a distinct `Order` object. Use a local set to track which sellers have already been assigned a shipping fee in the current transaction.

**Tech Stack:** .NET MAUI, Firebase Realtime Database.

---

### Task 1: Refactor Order Placement Logic in PurchasePage.xaml.cs

**Files:**
- Modify: `Pages/PurchasePage.xaml.cs`

- [ ] **Step 1: Update OnPlaceOrderClicked to split by product**

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

- [ ] **Step 3: Build and verify**
Run: `dotnet build`
Expected: Build SUCCESS.

- [ ] **Step 4: Commit**
```bash
git add Pages/PurchasePage.xaml.cs
git commit -m "feat: split orders per product and handle per-seller shipping fees"
```

---

### Task 2: Verify Shipping Fee Display in PurchasePage.xaml.cs

**Files:**
- Modify: `Pages/PurchasePage.xaml.cs` (Ensure `CalculateTotals` matches placement logic)

- [ ] **Step 1: Verify CalculateTotals logic**
The current `CalculateTotals` already uses `uniqueSellers * 80`. Ensure it remains consistent with the new placement logic.

```csharp
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
```

- [ ] **Step 2: Run build**
Run: `dotnet build`
Expected: Build SUCCESS.

---

### Task 3: Final Verification and Cleanup

- [ ] **Step 1: Final review of Order.cs and FirebaseService.cs**
Ensure `PlaceOrderAsync` is compatible with receiving multiple small orders rapidly. (It currently just posts to Firebase, which is fine).

- [ ] **Step 2: Commit any final tweaks**
```bash
git commit -m "chore: final cleanup of order splitting logic"
```
