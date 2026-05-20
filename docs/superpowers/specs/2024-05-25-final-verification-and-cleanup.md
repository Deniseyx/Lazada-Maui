# Design Spec: Final Verification and Cleanup (Order Splitting)

## Problem Statement
The multi-product order splitting and per-seller shipping fee logic was successfully implemented in the .NET MAUI application. However, the Blazor WebAssembly port, which shares the same Firebase backend, still uses the old monolithic order logic and a flat shipping fee. To ensure data consistency and a unified user experience, the Blazor checkout logic must be brought into parity with the MAUI implementation.

## Proposed Solution

### 1. Blazor Checkout Parity (`Checkout.razor`)
Update the `PlaceOrder` method and total calculation logic in `Lazada_Isagunde.Blazor/Pages/Checkout.razor`:
- **Shipping Calculation**: Calculate total shipping as `(Number of Unique Sellers) * ₱80`.
- **Order Splitting**: Iterate through each `CartItem` and create a separate `Order` record for it.
- **Shipping Distribution**: Only the first item from each unique seller will carry the ₱80 shipping fee. Subsequent items from the same seller will have a ₱0 shipping fee.
- **Firebase Integration**: Call `FirebaseService.PlaceOrderAsync` for each split order.

### 2. Service & Model Review
- **FirebaseService**: Confirm `PlaceOrderAsync` is robust enough for rapid sequential calls.
- **Models**: Ensure `Order` and `CartItem` models remain compatible between platforms.

### 3. Final Verification
- Perform a clean build of both MAUI and Blazor projects.
- Verify no regression in the core order flow.

## Success Criteria
- Buying products from multiple sellers in the Blazor app results in correct shipping fees (₱80 * sellers).
- Every product purchased in Blazor appears as a separate entry in the "My Orders" page.
- Both projects build without errors.
