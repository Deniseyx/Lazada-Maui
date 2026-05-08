param (
    [Parameter(Position=0)]
    [string]$Target = "windows"
)

$ErrorActionPreference = "Stop"

switch ($Target.ToLower()) {
    "android" {
        Write-Host "🚀 Launching on Android..." -ForegroundColor Cyan
        dotnet build -t:Run -f net9.0-android
    }
    "windows" {
        Write-Host "🚀 Launching on Windows..." -ForegroundColor Cyan
        dotnet build -t:Run -f net9.0-windows10.0.19041.0
    }
    "clean" {
        Write-Host "🧹 Cleaning project..." -ForegroundColor Yellow
        dotnet clean
        Get-ChildItem -Path . -Include bin,obj -Recurse | Remove-Item -Recurse -Force
        Write-Host "✅ Clean complete!" -ForegroundColor Green
    }
    "build" {
        Write-Host "🏗️ Building project..." -ForegroundColor Magenta
        dotnet build
    }
    default {
        Write-Host "❓ Unknown target '$Target'. Defaulting to Windows..." -ForegroundColor Yellow
        dotnet build -t:Run -f net9.0-windows10.0.19041.0
    }
}
