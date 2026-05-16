# Design Doc: Porting Lazada_Isagunde to Blazor WASM

**Status:** Draft  
**Date:** 2026-05-16  
**Topic:** Blazor WASM Port (Twin Architecture)

## 1. Overview
Port the existing .NET MAUI application `Lazada_Isagunde` to a standalone Blazor WebAssembly project located within the same repository. This enables a web-based version of the Lazada clone while maintaining synchronization with the same Firebase backend.

## 2. Architecture: Twin Architecture
The project will follow the structure outlined in the provided `BLAZOR_PORT_INSTRUCTIONS.txt`:
- **Location:** `Lazada_Isagunde.Blazor/` subdirectory in the root.
- **Isolation:** Update the MAUI `.csproj` to exclude the Blazor folder from its build process.
- **Solution:** Add the Blazor project to the existing `Lazada_Isagunde.sln`.

## 3. Core Logic Migration (Copy Strategy)
As requested, logic will be copied 1:1 and adapted:
- **Models:** `Models/` folder copied to `Lazada_Isagunde.Blazor/Models/`. Namespaces updated.
- **Services:** `Services/` folder copied to `Lazada_Isagunde.Blazor/Services/`.
    - `AuthService.cs`: Refactor to use `Blazored.LocalStorage` for token storage instead of MAUI-specific APIs.
    - `ImageService.cs`: Refactor to use direct `HttpClient` calls for Cloudinary/Firebase uploads (WASM-friendly).
    - `FirebaseService.cs`: Update to ensure compatibility with WASM (Firebase .NET client usually works well, but requires testing).

## 4. UI Implementation
- **Layout:** "Responsive Grid" approach.
- **Branding:** Match the purple/white theme of the MAUI app.
- **Navigation:** 
    - Full parity with MAUI navigation structure (Tab-based flow mapped to Web-friendly headers/menus).
    - Implement a shared header with profile-driven navigation, mirroring the mobile flow but adapted for desktop widths.
- **Pages:** Port **ALL** MAUI XAML pages to Blazor `.razor` components, ensuring Web UX optimizations (hover states, pointer cursors, desktop spacing).
    - `LandingPage.xaml` -> `Pages/Landing.razor`
    - `LoginPage.xaml` -> `Pages/Login.razor`
    - `RegisterPage.xaml` -> `Pages/Register.razor`
    - `AdminDashboardPage.xaml` -> `Pages/Admin/Dashboard.razor`
    - `CartPage.xaml` -> `Pages/Cart.razor`
    - `ContactUsPage.xaml` -> `Pages/Contact.razor`
    - `DashboardPage.xaml` -> `Pages/Index.razor`
    - `ProductDetailPage.xaml` -> `Pages/ProductDetail.razor`
    - `ProfilePage.xaml` -> `Pages/Profile.razor`
    - `PurchaseHistoryPage.xaml` -> `Pages/Orders.razor`
    - `SellerCenterPage.xaml` -> `Pages/Seller/Center.razor`
    - `SettingsPage.xaml` -> `Pages/Settings.razor`
    - etc. (All pages listed in the `Pages/` directory).

## 5. Success Criteria & Data Synchronization
- **Shared Backend:** Both the .NET MAUI and Blazor WASM applications **MUST** use the exact same Firebase Project (Realtime Database, Authentication, and Storage).
- **Real-time Sync:** Any change made in the Blazor app (e.g., adding to cart, placing an order) must be instantly visible in the MAUI app, and vice-versa.
- **Unified Auth:** A user registered on Mobile can log in on Web with the same credentials.
- **Product Consistency:** Both apps fetch from the same `products` node in Firebase.

## 6. Testing Strategy
- Manual verification of Auth flows.
- CRUD operation verification for Products/Cart/Orders.
- Verification that the MAUI project still builds and runs correctly after the Blazor folder is added.
