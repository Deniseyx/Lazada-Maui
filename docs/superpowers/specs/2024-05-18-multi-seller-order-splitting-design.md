# Design Spec: Multi-Product Order Splitting

## Problem Statement
Users expect that every distinct product they purchase is treated as a separate order in their history, regardless of whether they come from the same seller or different ones. However, shipping fees should remain fair: if multiple products come from the same seller, the user should only be charged one shipping fee for that seller's batch.

## Proposed Solution: Per-Product Order Splitting
Every unique product in the cart results in an independent order record. Shipping fees are calculated per unique seller.

### Architecture & Data Flow
1.  **Checkout Preparation (`PurchasePage`)**:
    *   Receive all items from the cart.
    *   **Group items by ProductId**: Even if two items are from the same seller, they will be processed as separate orders if they are different products.
    *   **Shipping Fee Calculation**:
        *   Identify unique `SellerId`s in the cart.
        *   `Total Shipping Fee` = (Number of unique `SellerId`s) * ₱80.
    *   Display a single `Grand Total` (Sum of all item prices + Total Shipping Fee).

2.  **Order Placement (`OnPlaceOrderClicked`)**:
    *   Validate shipping details.
    *   **Logic for Shipping Distribution**:
        *   To avoid charging ₱80 on *every* single product order when they share a seller, only the **first** order from each seller group will carry the ₱80 fee. Subsequent orders from the same seller will have a ₱0 shipping fee.
    *   Iterate through each `CartItem` (grouped by product):
        *   Create a new `Order` object for that specific product.
        *   `Items` contains only that single product type.
        *   `ShippingFee` = 80 if this is the first item processed for this seller; otherwise 0.
        *   `TotalPrice` = (Item Price * Quantity) + ShippingFee.
    *   Save each `Order` individually to Firebase via `FirebaseService.PlaceOrderAsync`.
    *   Clear the purchased items from the user's cart.

3.  **Purchase History (`PurchaseHistoryPage`)**:
    *   Load all orders. Every product purchased will appear as a separate entry/card.

### Success Criteria
*   Buying 1 Mouse and 1 Keyboard from "Seller A" results in **two separate orders**.
*   Total shipping fee for the above is **₱80** (since it's one seller).
*   Buying 1 Mouse from "Seller A" and 1 Monitor from "Seller B" results in **two separate orders**.
*   Total shipping fee for the above is **₱160** (two sellers).

### Testing Plan
1.  Add two different products from the same seller to the cart.
2.  Add one product from a different seller.
3.  Verify total shipping fee is ₱160 (2 sellers).
4.  Place order and verify **three separate orders** appear in "My Orders".
