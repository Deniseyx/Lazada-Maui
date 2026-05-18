# Design Doc: AuthService Migration (WASM Adaptation)

## Overview
Port the `AuthService` from the MAUI project to the Blazor WASM project, adapting it to use `Blazored.LocalStorage` for persistence and making it a Scoped service suitable for Dependency Injection.

## Architecture
- **AuthService**: A Scoped service that manages user authentication state and interactions with Firebase Auth.
- **Persistence**: `Blazored.LocalStorage` will be used to store user tokens and IDs across page refreshes.
- **Dependency Injection**: Registered in `Program.cs` as `builder.Services.AddScoped<AuthService>()`.

## Implementation Details

### AuthService Properties
- `string? UserToken`
- `string? UserId`
- `bool IsAdmin`
- `bool IsLoggedIn`
- `string? UserEmail`
- `string? UserDisplayName`

### Key Methods
- `InitializeAsync()`: Loads auth state from local storage.
- `RegisterUser(email, password)`: Registers a new user and saves state.
- `LoginUser(email, password)`: Logs in a user, handles admin override, and saves state.
- `Logout()`: Clears state and local storage.
- `GetFreshTokenAsync()`: Retrieves a fresh ID token from Firebase.
- `ResetPassword(email)`: Sends a password reset email.

### Program.cs Changes
- Register `AddBlazoredLocalStorage()`.
- Register `AuthService` as a Scoped service.

## Data Flow
1. User logs in -> `AuthService` calls Firebase Auth -> `AuthService` receives token/UID -> `AuthService` saves to `ILocalStorageService`.
2. Page refresh -> `AuthService` initialized -> `AuthService` reads from `ILocalStorageService`.
3. API call (via `FirebaseService`) -> `FirebaseService` asks `AuthService` for a fresh token -> `AuthService` returns token.

## Testing Strategy
- Verify that `AuthService` can be injected.
- Verify that login saves state to local storage.
- Verify that logout clears local storage.
- Verify build success.
