# PersonalPortfolio.v1

Blazor WebAssembly portfolio for **Abhay Kumar Mandal** — deployed at [abhaymandal.com.np](https://abhaymandal.com.np/).

## Architecture

**Single-page app** — `App.razor` renders `<Home />` directly (no `<Router>`). All sections (Hero, About, Experience, Projects, Blog, Contact, Resume) live on one page with anchor navigation (`#about`, `#projects`, etc.). `BlogPost.razor` and `ProjectDetail.razor` exist but are unused (no `@page` directive).

**Data flow:** `JsonDataService` fetches `wwwroot/data/*.json` at runtime via `HttpClient`. Results are cached in-memory via null-coalescing (`_projects ??= await ...`) so each file is fetched at most once per session. Contact form POSTs to Formspree (`formspree.io/f/xvgkrkwk`). GitHub stats use localStorage cache with fallback data when rate-limited.

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | .NET 10 — Blazor WebAssembly (net10.0, SDK `Microsoft.NET.Sdk.BlazorWebAssembly`) |
| AOT | Disabled (`RunAOTCompilation=false`). Trimming also disabled for deployment reliability. |
| Compression | Brotli + gzip enabled (`CompressionEnabled=true`) |
| WASM size | Optimized via `InvariantGlobalization=true` and `BlazorEnableTimeZoneSupport=false` |
| Hosting | GitHub Pages via CI/CD to `enigmasheet/portfolio` repo |
| CI/CD | `.github/workflows/deploy.yml` — builds with AOT+trimming, gzips static assets, deploys via `JamesIves/github-pages-deploy-action` |
| Contact | Formspree (`POST` to `formspree.io/f/xvgkrkwk`) |
| Icons | Font Awesome 6 (`cdnjs.cloudflare.com`) |
| Fonts | Google Fonts — Inter + JetBrains Mono |

## Project Structure

```
PersonalPortfolio.v1/
├── .github/workflows/deploy.yml   — CI/CD: build, gzip, deploy to enigmasheet/portfolio
├── wwwroot/
│   ├── index.html                  — SPA shell, script references, loader
│   ├── manifest.json               — (removed)
│   ├── css/app.css                 — Global styles (theming, layout, animations, print)
│   ├── scripts/
│   │   ├── boot.js                 — Blazor startup, app reveal (1500ms delay), scroll spy
│   │   ├── animations.js           — IntersectionObserver + MutationObserver, parallax
│   │   ├── theme.js                — Dark/light mode, prefers-color-scheme
│   │   ├── typing.js               — Typewriter effect on hero subtitle
│   │   ├── counters.js             — Animated number counters
│   │   ├── storage.js              — localStorage helpers (get/set/remove)
│   │   └── lightbox.js             — Image lightbox
│   └── data/                       — Static JSON data (projects, experience, skills, blog, etc.)
├── Pages/                          — Blazor components (Home, About, Contact, etc.)
├── Components/                     — Reusable components (Hero, NavMenu, SectionHeader, etc.)
├── Services/
│   ├── IDataService.cs             — Data access interface
│   └── JsonDataService.cs          — HttpClient-backed implementation, per-session in-memory cache
├── Models/                         — Data models (ProjectModel, ExperienceModel, etc.)
├── Layout/                         — MainLayout, NavMenu
└── docs/                           — Compiled WASM output (gitignored, built by CI/CD)
```



## Data Caching

Three layers of caching, from closest to farthest:

| Layer | Type | Scope | Invalidated |
|---|---|---|---|
| In-memory (`JsonDataService`) | Per-session | All JSON data files | On page reload |
| localStorage | Persistent | GitHub API stats | 1-hour TTL checked at read time |

## Key Scripts

| Script | Load | Role |
|---|---|---|
| `theme.js` | Sync in `<head>` | Applies dark/light theme before first paint to prevent flash |
| `animations.js` | Sync in `<body>` | IntersectionObserver for `.fade-in` / `.reveal-stagger`, MutationObserver on `#app` for Blazor DOM churn, scroll parallax |
| `boot.js` | Sync in `<body>` | Blazor startup (`autostart=false`), 1.5s reveal delay, scroll spy, back-to-top |
| `storage.js` | Sync in `<body>` | Safe localStorage accessors |
| `typing.js` | Sync in `<body>` | Typewriter on `[data-typing]` (guarded: no-ops if element absent) |
| `counters.js` | Sync in `<body>` | Animated number counters via IntersectionObserver |
| `lightbox.js` | Sync in `<body>` | Image lightbox |

All scripts are loaded synchronously in `index.html`. Blazor uses `autostart=false` — `boot.js` calls `Blazor.start()` after the scripts are ready.

## Local Development

```bash
# Restore
dotnet restore

# Build (Release for accurate output)
dotnet build -c Release

# Publish to bin/Release/net10.0/publish/wwwroot/
dotnet publish -c Release -p:PublishTrimmed=false -p:RunAOTCompilation=false
```

To preview locally via GitHub Pages:
```bash
Copy-Item -Path "bin/Release/net10.0/publish/wwwroot/*" -Destination "docs/" -Recurse -Force
# Then serve docs/ with any static file server
```

## Deployment

Pushing to `main` triggers the CI/CD workflow (`.github/workflows/deploy.yml`):

1. `actions/checkout@v6`
2. `actions/setup-dotnet@v5` with `wasm-tools` workload
3. `dotnet publish` with AOT + trimming enabled (slower build, faster runtime)
4. `gzip` all text-based static assets (GitHub Pages serves pre-compressed files)
5. `JamesIves/github-pages-deploy-action` pushes to `enigmasheet/portfolio` (separate deployment repo)

The `docs/` directory in this repo is **not tracked** (`.gitignore`) — it exists only for local previews.
