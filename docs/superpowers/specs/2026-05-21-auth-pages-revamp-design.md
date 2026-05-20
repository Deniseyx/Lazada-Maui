# Design Spec: Auth Pages Revamp (Login & Register)

**Date:** 2026-05-21
**Status:** Approved
**Topic:** UI/UX Revamp of Login and Register Pages

## 1. Overview
Align the Login and Register pages with the Modern Minimalist design established in the Blazor UI revamp. This involves moving away from the previous purple accents and adopting the white card, orange accent, and soft background aesthetic.

## 2. Visual Design
- **Background:** `var(--lazada-bg)` (#f8f9fa) for the full viewport background.
- **Card:** `modern-card` (white surface, soft shadow, 12px border-radius).
- **Header:** `section-title` (bold, dark text) centered within the card.
- **Accents:** All primary actions (buttons) and links will use `var(--lazada-orange)` (#f57224) and `var(--lazada-orange-deep)` (#e0611d) for hover states.
- **Input Fields:** Use standard `form-control` which should already have some global styling, but ensured with consistent local overrides if necessary.

## 3. Structure
### Login Page
- Centered `modern-card`.
- "Login" title using `section-title`.
- Email and Password inputs.
- "Sign In" button (Orange).
- "Forgot Password?" link (Orange).

### Register Page
- Centered `modern-card`.
- "Create Account" title using `section-title`.
- Full Name, Email, and Password inputs.
- "Sign Up" button (Orange).
- "Login" link (Orange) for existing users.

## 4. CSS Standard
A unified CSS block will be applied to both pages to ensure identical look and feel, replacing all existing `<style>` blocks.

## 5. Implementation Constraints
- Maintain existing logic and bindings (`@bind`, `HandleLogin`, `HandleRegister`).
- No changes to MAUI or backend services.
