# Design Spec: Blazor UI Revamp (Modern Minimalist)

**Date:** 2026-05-20
**Status:** Approved
**Topic:** UI/UX Revamp of the Blazor Web Application

## 1. Overview
The goal is to revamp the entire Blazor application UI to be "simple but nice to look at." The design follows a **Modern Minimalist** approach with a White and Orange color palette.

## 2. Global Foundation
- **Color Palette:**
  - `Primary (Header)`: `#f57224` (Lazada Orange)
  - `Background`: `#f8f9fa` (Soft off-white)
  - `Surface (Cards)`: `#ffffff` (Pure white)
  - `Text`: `#212529` (Standard high-contrast gray)
- **Layout:**
  - Sticky solid orange header.
  - Light gray page background to provide contrast for white cards.
  - Soft, large shadows for elevation (`box-shadow: 0 10px 30px rgba(0,0,0,0.04)`).

## 3. Component Details
### Header
- Solid orange background.
- Clean search bar with an internal search icon.
- Circular white icons for navigation (Cart, Profile, etc.).
- Remove "Settings" from any global navigation if present.

### Profile Page (`Profile.razor`)
- **Banner:** Remove the solid orange box. User info sits on the background with a clean, large avatar.
- **Cards:** White surface cards for "Shipping Information" and "Actions."
- **Sidebar Actions:**
  - **HIDE** the "Settings" link.
  - List items (Orders, Seller Center, etc.) are clean text + icon with soft hover states.
  - **Log Out:** Simplified button or link at the bottom of the list.
- **Form Fields:** Increased vertical spacing and refined typography.

### General Pages
- All main content sections should be wrapped in white "floating" cards.
- Consistent typography and spacing (increased padding and line-height).

## 4. Implementation Constraints
- Do not break backend services (`AuthService`, `FirebaseService`, etc.).
- The `Settings.razor` page remains in the project but is hidden from the UI.
- MAUI project remains untouched.

## 5. Success Criteria
- The UI feels premium and spacious.
- Branding (Orange/White) is consistent.
- No "Settings" links are visible to the user.
- Responsive behavior is maintained.
