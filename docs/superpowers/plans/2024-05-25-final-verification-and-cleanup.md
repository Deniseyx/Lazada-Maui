# Final Verification and Cleanup Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Implement order splitting and per-seller shipping fees in the Blazor checkout and perform final verification across the workspace.

**Architecture:** Update `Checkout.razor` to match MAUI's `PurchasePage.xaml.cs` logic. Each product becomes a separate order. Shipping is ₱80 per unique seller.

**Tech Stack:** Blazor WebAssembly, .NET MAUI, Firebase, C#.

---

### Task 1: Update Blazor Checkout Logic

**Files:**
- Modify: `Lazada_Isagunde.Blazor/Pages/Checkout.razor`

- [ ] **Step 1: Update total calculations to handle per-seller shipping**

Update the computed properties in `@code` block to count unique sellers.

```razor
    private double subtotal => cartItems?.Sum(i => i.Price * i.Quantity) ?? 0;
    private double shippingFee => (cartItems?.Select(i => i.SellerId).Distinct().Count() ?? 0) * 80;
    private double totalPayment => subtotal + shippingFee;
```

- [ ] **Step 2: Update `PlaceOrder` to implement splitting logic**

Modify `PlaceOrder` to iterate through items and distribute shipping fees.

```razor
    private async Task PlaceOrder()
    {
        if (cartItems == null || !cartItems.Any()) return;

        isProcessing = true;

        string receiverName = useSavedAddress ? (userProfile?.FullName ?? AuthService.UserDisplayName ?? "User") : customName;
        string receiverPhone = useSavedAddress ? (userProfile?.PhoneNumber ?? "") : customPhone;
        string shippingAddress = useSavedAddress ? (userProfile?.ShippingAddress ?? "") : customAddress;

        if (string.IsNullOrEmpty(receiverName) || string.IsNullOrEmpty(shippingAddress))
        {
            isProcessing = false;
            return;
        }

        try
        {
            // Track which sellers have already been charged shipping in this transaction
            var sellersChargedShipping = new HashSet<string>();

            // Every unique product (CartItem) becomes its own order
            foreach (var item in cartItems)
            {
                double itemShippingFee = 0;
                if (!sellersChargedShipping.Contains(item.SellerId))
                {
                    itemShippingFee = 80; // ₱80 per seller
                    sellersChargedShipping.Add(item.SellerId);
                }

                var order = new Order
                {
                    BuyerId = AuthService.UserId!,
                    ReceiverName = receiverName,
                    ReceiverPhone = receiverPhone,
                    ShippingAddress = shippingAddress,
                    Items = new List<CartItem> { item }, // Only this specific product
                    Subtotal = item.Price * item.Quantity,
                    ShippingFee = itemShippingFee,
                    TotalPrice = (item.Price * item.Quantity) + itemShippingFee,
                    Status = "Pending",
                    OrderDate = DateTime.UtcNow,
                    PaymentMethod = paymentMethod,
                    SellerId = item.SellerId,
                    SellerName = item.SellerName
                };

                await FirebaseService.PlaceOrderAsync(order);
            }

            // Clear whole cart after all orders placed
            await FirebaseService.ClearCartAsync(AuthService.UserId!);

            Navigation.NavigateTo("/orders");
        }
        catch (Exception ex)
        {
            // Simple error logging
            Console.WriteLine($"Error placing order: {ex.Message}");
        }
        finally
        {
            isProcessing = false;
        }
    }
```

- [ ] **Step 3: Update UI to show shipping fee details**

Update the shipping fee display line in the HTML.

```razor
                    <div class="d-flex justify-content-between mb-2">
                        <span class="text-muted">Shipping Fee</span>
                        <span>₱@shippingFee.ToString("N0") (@(cartItems?.Select(i => i.SellerId).Distinct().Count() ?? 0) Seller/s)</span>
                    </div>
```

- [ ] **Step 4: Commit changes**

```bash
git add Lazada_Isagunde.Blazor/Pages/Checkout.razor
git commit -m "feat(blazor): implement order splitting and per-seller shipping fee"
```

---

### Task 2: Final Verification and Build

- [ ] **Step 1: Build MAUI project**

Run: `dotnet build Lazada_Isagunde.csproj`
Expected: Build SUCCESS.

- [ ] **Step 2: Build Blazor project**

Run: `dotnet build Lazada_Isagunde.Blazor/Lazada_Isagunde.Blazor.csproj`
Expected: Build SUCCESS.

- [ ] **Step 3: Commit final cleanup**

```bash
git commit --allow-empty -m "chore: final cleanup of order splitting logic"
```
