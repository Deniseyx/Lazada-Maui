using System;
using System.Collections.Generic;

namespace Lazada_Isagunde.Blazor.Models;

public class Order
{
    public string Id { get; set; } = string.Empty;
    public string BuyerId { get; set; } = string.Empty;
    public string SellerId { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.Now;
    
    // Items in this specific sub-order (per seller)
    public List<CartItem> Items { get; set; } = new();
    
    public double Subtotal { get; set; }
    public double ShippingFee { get; set; } = 80;
    public double TotalPrice { get; set; }
    
    public string Status { get; set; } = "Pending"; // Pending, Shipped, Delivered, Cancelled
    public bool IsReviewed { get; set; } = false;
    public string PaymentMethod { get; set; } = "COD"; // COD, GCash, Card

    // Shipping Details
    public string ReceiverName { get; set; } = string.Empty;
    public string ReceiverPhone { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;

    // UI Helpers
    public string TotalPriceFormatted => $"₱{TotalPrice:N0}";
    public string SubtotalFormatted => $"₱{Subtotal:N0}";
    public string DateFormatted => OrderDate.ToString("MMM dd, yyyy");
    
    public string StatusColor => Status switch
    {
        "Delivered" => "#2ECC71",
        "Pending" => "#FFD700",
        "Shipped" => "#3498DB",
        "Cancelled" => "#FF4500",
        _ => "#808080"
    };

    public string ItemsSummary => Items.Count > 1 
        ? $"{Items[0].Title} + {Items.Count - 1} more" 
        : (Items.Count == 1 ? Items[0].Title : "No items");

    public bool CanUpdateStatus => Status != "Delivered" && Status != "Cancelled";
    public bool IsDelivered => Status == "Delivered";
    public bool ShowReviewButton => Status == "Delivered" && !IsReviewed;
}
