# Design: Split Orders per Product with Per-Seller Shipping Fees

## Goal
Refactor the order placement logic in `PurchasePage.xaml.cs` so that every `CartItem` in the checkout process results in a separate `Order` record in Firebase. Shipping fees (₱80) must still be applied once per unique seller across the entire transaction.

## Current State
- `OnPlaceOrderClicked` groups `CartItem`s by `SellerId`.
- It creates one `Order` per seller, containing all items from that seller.
- Each such order has a flat ₱80 shipping fee.

## Proposed Change
- Remove the `GroupBy(i => i.SellerId)` logic.
- Iterate through `_items` (the list of `CartItem`s) directly.
- Use a `HashSet<string> sellersChargedShipping` to track which sellers have already had a shipping fee applied in the current loop.
- For each `CartItem`:
    - Check if its `SellerId` is in the `HashSet`.
    - If not, set `shippingFee = 80` and add the `SellerId` to the `HashSet`.
    - If it is, set `shippingFee = 0`.
    - Create a new `Order` object for this single item.
    - Call `_firebaseService.PlaceOrderAsync(order)`.

## Data Model Alignment
- `Order.Items` will now contain exactly one `CartItem`.
- `Order.Subtotal` will be `item.Price * item.Quantity`.
- `Order.TotalPrice` will be `Subtotal + shippingFee`.

## Verification Plan
- **Manual Code Review:** Ensure the `HashSet` logic correctly identifies the first occurrence of each seller.
- **Build:** Run `dotnet build` to ensure no compilation errors.
- **Logic Verification:**
    - If items = [{Seller: A, Product: 1}, {Seller: A, Product: 2}], then:
        - Order 1 (Product 1): Shipping = 80
        - Order 2 (Product 2): Shipping = 0
        - Total Shipping = 80 (Correct).
    - If items = [{Seller: A, Product: 1}, {Seller: B, Product: 3}], then:
        - Order 1 (Product 1): Shipping = 80
        - Order 2 (Product 3): Shipping = 80
        - Total Shipping = 160 (Correct).

## Files to Modify
- `Pages/PurchasePage.xaml.cs`
