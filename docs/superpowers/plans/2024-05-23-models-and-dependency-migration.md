# Models and Dependency Migration Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Migrate remaining models and add necessary NuGet dependencies to the Blazor WASM project.

**Architecture:** Copy models from the MAUI project to the Blazor project, updating namespaces and replacing MAUI-specific types with web-friendly alternatives.

**Tech Stack:** .NET 8, Blazor WASM, Firebase (Authentication, Database, Storage), Blazored.LocalStorage.

---

### Task 1: Migrate Remaining Models

**Files:**
- Create: `Lazada_Isagunde.Blazor/Models/Message.cs`
- Create: `Lazada_Isagunde.Blazor/Models/Order.cs`
- Create: `Lazada_Isagunde.Blazor/Models/Product.cs`
- Create: `Lazada_Isagunde.Blazor/Models/Review.cs`
- Create: `Lazada_Isagunde.Blazor/Models/UserProfile.cs`

- [ ] **Step 1: Create Message.cs**
```csharp
namespace Lazada_Isagunde.Blazor.Models;

public class Message
{
    public string Id { get; set; } = string.Empty;
    public string ConversationId { get; set; } = string.Empty;
    public string SenderId { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public bool IsOutgoing { get; set; }
    public string TimeFormatted => Timestamp.ToString("MMM dd, h:mm tt");
}
```

- [ ] **Step 2: Create Order.cs (with Color fixed)**
```csharp
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
```

- [ ] **Step 3: Create Product.cs**
```csharp
namespace Lazada_Isagunde.Blazor.Models;

public class Product
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
    public string ImageUrl { get; set; } = string.Empty;
    public double Price { get; set; }
    public double Rating { get; set; } = 4.0;
    public int ReviewsCount { get; set; } = 0;
    public string SellerId { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public int Stock { get; set; } = 10;

    // Status: "Pending", "Approved", "Rejected"
    public string Status { get; set; } = "Pending";

    // For UI display
    public string PriceFormatted => $"₱{Price:N0}";
    public string RatingFormatted => $"★ {Rating:F2}";
}
```

- [ ] **Step 4: Create Review.cs**
```csharp
namespace Lazada_Isagunde.Blazor.Models;

public class Review
{
    public string Id { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public string BuyerId { get; set; } = string.Empty;
    public string BuyerName { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string RelativeTime => "Just now";
}
```

- [ ] **Step 5: Create UserProfile.cs**
```csharp
namespace Lazada_Isagunde.Blazor.Models;

public class UserProfile
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
```

### Task 2: Add NuGet Packages

- [ ] **Step 1: Add FirebaseAuthentication.net**
Run: `dotnet add Lazada_Isagunde.Blazor/Lazada_Isagunde.Blazor.csproj package FirebaseAuthentication.net`

- [ ] **Step 2: Add FirebaseDatabase.net**
Run: `dotnet add Lazada_Isagunde.Blazor/Lazada_Isagunde.Blazor.csproj package FirebaseDatabase.net`

- [ ] **Step 3: Add FirebaseStorage.net**
Run: `dotnet add Lazada_Isagunde.Blazor/Lazada_Isagunde.Blazor.csproj package FirebaseStorage.net`

- [ ] **Step 4: Add Blazored.LocalStorage**
Run: `dotnet add Lazada_Isagunde.Blazor/Lazada_Isagunde.Blazor.csproj package Blazored.LocalStorage`

### Task 3: Verification and Commit

- [ ] **Step 1: Verify Blazor project builds**
Run: `dotnet build Lazada_Isagunde.Blazor/Lazada_Isagunde.Blazor.csproj`

- [ ] **Step 2: Commit changes**
```bash
git add Lazada_Isagunde.Blazor/
git commit -m "feat: migrate models and add dependencies to blazor project"
```
