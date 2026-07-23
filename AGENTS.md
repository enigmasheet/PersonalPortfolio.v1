# Project Knowledge

## Architecture
- `PersonalPortfolio.v1/` — Blazor WASM source project (`.NET 10`, `net10.0`)
- `portfolio/` — Separate git repo for GitHub Pages deployment artifacts
- CI/CD: GitHub Actions builds `PersonalPortfolio.v1` and pushes output to `enigmasheet/portfolio`

## Deployment Flow
1. Commit to `enigmasheet/PersonalPortfolio.v1` `main`
2. GitHub Actions runs `dotnet publish`
3. Publish output is pushed to `enigmasheet/portfolio` `main`
4. GitHub Pages serves from `enigmasheet/portfolio`

## Known Issues & Resolutions

### Blazor WASM Boot Failure — `loadBootResource` Overreach
- **Root Cause**: `loadBootResource` callback intercepted ALL `type === 'dotnetjs'` requests, not just the main `dotnet.js`. This caused `dotnet.native.<hash>.js` and `dotnet.runtime.<hash>.js` to be resolved to the fingerprinted `dotnet.<hash>.js` instead of their own files, missing `setRuntimeGlobals`.
- **Fix**: Added `name === 'dotnet.js'` guard in `loadBootResource` at `wwwroot/scripts/boot.js:31-36`.
- **File**: `wwwroot/scripts/boot.js` — `loadBootResource` function
- **Regression Guard**: `Scripts/BootJsTests.cs` at `PersonalPortfolio.v1.Tests/Scripts/BootJsTests.cs`:
  - Tests that `name === 'dotnet.js'` guard exists in `loadBootResource`
  - Tests `boot()` is called in both `.then()` and `.catch()`
  - Tests `showError()` adds `visible` class to `#app`
  - Tests error/unhandledrejection event listeners are registered
  - Tests `logLevel: 1` and `configureRuntime` are set

### Blazor WASM Boot Failure — `#app` Not Visible (Resolved)
- **Symptom**: `Blazor.start()` rejects; `#app` rendered content (text selectable) but stays at `opacity: 0` because `boot()` (which adds `visible` class) was only called in `.then()`, not `.catch()`.
- **Fix**: `showError()` now also adds `visible` to `#app`; `boot()` is called in both `.then(boot)` and `.catch(...)` chains.
- **File**: `wwwroot/scripts/boot.js`

### .NET Startup Error — Next Step
- With the fix above, the app is now visible even when `Blazor.start()` rejects. The error message is shown in `#blazor-error-ui` with "Error: ..." prefix.
- **Next**: Check the browser console for the `[boot]` error message when loading `http://localhost:5288` to identify the root cause of `Blazor.start()` rejection.

## Key Files
- `Program.cs` — Entry point, DI registration, MudBlazor setup
- `App.razor` / `App.razor.cs` — Root component, theme binding, `_isDark` tracking
- `wwwroot/scripts/boot.js` — `Blazor.start()` call with `loadBootResource`
- `wwwroot/index.html` — SPA shell with placeholder-based script src
- `Services/JsonDataService.cs` — HTTP-based JSON data loading
- `Services/Program.cs` — `Main` entry point

## Development Commands
```powershell
# Run dev server (http://localhost:5288)
dotnet run --project PersonalPortfolio.v1 --launch-profile http

# Publish (CI handles deploy; test locally with dev server)
dotnet publish PersonalPortfolio.v1 -c Release

# Run all tests
dotnet test PersonalPortfolio.v1.Tests

# Run only boot.js regression tests
dotnet test PersonalPortfolio.v1.Tests --filter "FullyQualifiedName~BootJsTests"
```
