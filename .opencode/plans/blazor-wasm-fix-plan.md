# Blazor WASM Fix Plan — All 54 Issues

## Priority Legend
- 🔴 **HIGH** — must fix (app-breaking)
- 🟡 **MEDIUM** — correctness, security, UX
- 🟢 **LOW** — polish, optimization

---

## H1 🔴 Create `.nojekyll` for GitHub Pages
**File:** `wwwroot/.nojekyll` (new file, empty)
**Why:** Without it, GitHub Pages Jekyll processing hides `_framework/` folder → Blazor .wasm/dll never loads.

---

## H2 🔴 Fix `@@keyframes` in index.html
**File:** `wwwroot/index.html`, line 64
**Change:** `@@keyframes` → `@keyframes`
**Why:** In plain HTML (not Razor), `@@` is literal, making `@keyframes` invalid → loader spinner never spins.

---

## H3 🔴 ProjectCard mutates parent's ImageUrls
**File:** `Components/ProjectCard.razor`, lines 269-276
**Change:**
```csharp
// Before:
ImageUrls.Add(fallbackImageUrl);

// After:
ImageUrls = new List<string>(ImageUrls);
if (!ImageUrls.Contains(fallbackImageUrl))
{
    ImageUrls.Add(fallbackImageUrl);
}
```
**Why:** `OnParametersSet` directly `Add()`s to the parent's `List<string>` reference, permanently modifying shared data.

---

## H4 🔴 BlogPost scroll listener never removed (memory leak)
**File:** `Pages/BlogPost.razor`, lines 75-88
**Change:** Replace inline `<script>` with a JS module pattern that stores listener reference and cleans up. Add disposal via `IJSRuntime`.

**New inline script:**
```html
<script>
(function() {
  var bar = document.getElementById('progress-bar');
  if (!bar) return;
  function update() {
    var scrollTop = window.scrollY;
    var docHeight = document.documentElement.scrollHeight - window.innerHeight;
    var pct = docHeight > 0 ? Math.min(scrollTop / docHeight, 1) : 0;
    bar.style.transform = 'scaleX(' + pct + ')';
  }
  window.addEventListener('scroll', update, { passive: true });
  update();
  window.__blogProgressCleanup = function() {
    window.removeEventListener('scroll', update);
  };
})();
</script>
```

Add to `@code` block:
```csharp
protected override void Dispose(bool disposing)
{
    if (disposing)
    {
        try { JS.InvokeVoidAsync("eval", "if (window.__blogProgressCleanup) window.__blogProgressCleanup()"); } catch { }
    }
}
```
**Why:** Navigating away from blog post leaves orphaned scroll listener that queries removed DOM element.

---

## M1 🟡 Program.cs — parallel fetches + no silent catch
**File:** `Program.cs`, lines 18-40
**Changes:**
1. Replace sequential `await` with `Task.WhenAll()`
2. Replace empty `catch { }` with `catch (Exception ex) { Console.Error.WriteLine($"Pre-fetch failed: {ex.Message}"); }`

**Why:** Sequential fetches delay startup; empty catch hides failures.

---

## M2 🟡 Add `@key` to all `foreach` loops

### Files to fix (8 files):

| File | Line | Current | Fix |
|------|------|---------|-----|
| `Pages/About.razor` | 36 | `@foreach (var category in skills)` | `@key="category.Category"` |
| `Pages/About.razor` | 60 | `@foreach (var cert in certifications)` | `@key="cert.Title"` |
| `Pages/About.razor` | 84 | `@foreach (var t in testimonials)` | `@key="t.Name"` |
| `Pages/Experience.razor` | 28 | `@foreach (var exp in ...)` | `@key="exp.Title"` |
| `Pages/Blog.razor` | 28 | `@foreach (var post in ...)` | `@key="post.Slug"` |
| `Pages/BlogPost.razor` | 44 | `@foreach (var tag in post.Tags)` | `@key="tag"` |
| `Pages/Resume.razor` | 40 | `@foreach (var exp in ...)` | `@key="exp.Title"` |
| `Pages/Resume.razor` | 67 | `@foreach (var category in skills)` | `@key="category.Category"` |
| `Components/ProjectCard.razor` | 32 | `@foreach (var tech in Technologies)` | `@key="tech"` |
| `Components/ProjectCard.razor` | 42 | `@foreach (var tag in Tags)` | `@key="tag"` |
| `Components/CommandPalette.razor` | 23 | Check if any foreach | `@key` as needed |

**Why:** Without `@key`, Blazor uses index-based diffing causing incorrect rendering on list changes.

---

## M3 🟡 NavMenu active route prefix matching
**File:** `Components/NavMenu.razor`, lines 11-17
**Change:** Replace exact `==` with `.StartsWith()`:
```razor
<li><a href="/" class="nav-link @(CurrentRoute == "/" ? "active" : "")">Home</a></li>
<li><a href="/about" class="nav-link @(CurrentRoute != null && CurrentRoute.StartsWith("/about") ? "active" : "")">About</a></li>
<li><a href="/experience" class="nav-link @(CurrentRoute != null && CurrentRoute.StartsWith("/experience") ? "active" : "")">Experience</a></li>
<li><a href="/projects" class="nav-link @(CurrentRoute != null && CurrentRoute.StartsWith("/projects") ? "active" : "")">Projects</a></li>
<li><a href="/blog" class="nav-link @(CurrentRoute != null && CurrentRoute.StartsWith("/blog") ? "active" : "")">Blog</a></li>
<li><a href="/resume" class="nav-link @(CurrentRoute == "/resume" ? "active" : "")">Resume</a></li>
<li><a href="/contact" class="nav-link @(CurrentRoute == "/contact" ? "active" : "")">Contact</a></li>
```
**Why:** `/projects/some-project` should highlight the Projects nav link.

---

## M4 🟡 Add ErrorBoundary in App.razor
**File:** `App.razor`
**Change:** Wrap `<Router>` in `<ErrorBoundary>`:
```razor
<ErrorBoundary>
    <Router ... />
</ErrorBoundary>
```
**Why:** Unhandled exceptions currently show bare error UI with no graceful fallback.

---

## M5 🟡 404.html — move redirect check after Blazor.start()
**File:** `wwwroot/index.html`
**Change:** Move the `redirectPath` block (lines 113-117) inside the `.then()` callback after `Blazor.start()`:
```javascript
Blazor.start().then(function () {
    var redirectPath = sessionStorage.getItem('redirectPath');
    if (redirectPath) {
        sessionStorage.removeItem('redirectPath');
        history.replaceState(null, '', redirectPath);
    }
    showApp();
    ...
}).catch(...)
```
**Why:** Race condition — redirect path is consumed before Blazor initializes, losing deep links on first load.

---

## M6 🟡 Service worker improvements
**File:** `wwwroot/service-worker.js`
**Changes:**
1. Add `/scripts/typing.js` and `/scripts/counters.js` to `PRECACHE_URLS`
2. Add `.catch()` handler to `cacheFirst` for complete fetch failure
3. SW registration `.catch()` (M12 — moved here)

**Why:** Missing precache entries; no fetch error fallback.

---

## M7 🟡 `openLightbox` innerHTML XSS
**File:** `Pages/ProjectDetail.razor`, line 203
**Change:** Replace `innerHTML` with DOM API:
```javascript
window.openLightbox = function(src) {
    var overlay = document.createElement('div');
    overlay.className = 'lightbox-overlay';
    var close = document.createElement('button');
    close.className = 'lightbox-close';
    close.innerHTML = '\u00D7';
    close.setAttribute('aria-label', 'Close');
    close.onclick = function() {
        overlay.classList.remove('open');
        setTimeout(function() { overlay.remove(); }, 300);
    };
    var img = document.createElement('img');
    img.setAttribute('src', src);
    img.setAttribute('alt', '');
    overlay.appendChild(close);
    overlay.appendChild(img);
    document.body.appendChild(overlay);
    requestAnimationFrame(function() { overlay.classList.add('open'); });
    overlay.addEventListener('click', function(e) {
        if (e.target === overlay) {
            overlay.classList.remove('open');
            setTimeout(function() { overlay.remove(); }, 300);
        }
    });
};
```
**Why:** String concatenation in `innerHTML` allows attribute injection XSS if `src` contains special characters.

---

## M8 🟡 Color contrast fix
**File:** `wwwroot/css/app.css`, lines 7-8
**Change:** Darken accent for better contrast:
```css
--accent: #5a52e0;    /* was #6c63ff — contrast 4.4:1 vs white, now 5.2:1 */
--accent-hover: #4a42d0;
```
And update dark mode:
```css
--accent: #8b83ff;    /* keep as-is on dark — sufficient contrast on #0a0a0a */
--accent-hover: #a39cff;
```
**Why:** `#6c63ff` on white has 4.4:1 contrast — below WCAG AA 4.5:1 threshold for normal text.

---

## M9 🟡 Form aria-describedby for validation errors
**File:** `Pages/Contact.razor`
**Changes:**
1. Add `id="name-error"`, `id="email-error"`, `id="message-error"` to error spans
2. Add `aria-describedby="name-error"` etc. to corresponding inputs
3. Add `role="alert"` to each error span

**Why:** Screen readers don't announce validation errors without programmatic association.

---

## M10 🟡 Service worker: complete fetch fallback
**File:** `wwwroot/service-worker.js`, `cacheFirst` function
**Change:** Add `.catch()` that returns fallback:
```javascript
function cacheFirst(request) {
  return caches.match(request).then(function (response) {
    return response || fetch(request).then(function (networkResponse) {
      if (!networkResponse.ok) return networkResponse;
      return caches.open(CACHE).then(function (cache) {
        cache.put(request, networkResponse.clone());
        return networkResponse;
      });
    });
  }).catch(function () {
    // Return transparent SVG for images, empty response for others
    if (request.destination === 'image') {
      return new Response('<svg xmlns="http://www.w3.org/2000/svg" width="1" height="1"/>', 
        { headers: { 'Content-Type': 'image/svg+xml' } });
    }
    return new Response('', { status: 503 });
  });
}
```

---

## M11 🟡 Service worker registration error handling
**File:** `wwwroot/index.html`, line 136
**Change:**
```javascript
navigator.serviceWorker.register('service-worker.js')
    .catch(function(err) { console.warn('SW registration failed:', err); });
```

---

## M12 🟡 Already covered by M6/M11 above.

---

## M13 🟡 manifest.json 512px icon
**File:** `wwwroot/manifest.json`, lines 13-18
**Change:** Update 512x512 entry to point to `icon-512.png` (create a 512x512 version of the icon).
```json
{
    "src": "icon-512.png",
    "sizes": "512x512",
    "type": "image/png",
    "purpose": "any maskable"
}
```
**Why:** Reusing 192px icon for 512px slot causes pixelation on high-DPI displays.

---

## M14 🟡 NavMenu resize listener cleanup
**File:** `Components/NavMenu.razor`, script section
**Change:** Store resize handler and clean up:
```javascript
(function() {
    var toggle = document.getElementById('nav-toggle');
    ...
    function handleResize() {
        if (window.innerWidth > 768 && toggle.checked) {
            toggle.checked = false;
            syncAria();
            scrollLock();
        }
    }
    window.addEventListener('resize', handleResize);
    // Store for cleanup
    window.__navCleanup = function() {
        window.removeEventListener('resize', handleResize);
    };
})();
```
Add to `@code` with `IJSRuntime` disposal calling `window.__navCleanup()`.

**Why:** Duplicate resize listeners accumulate on navigation.

---

## M15 🟡 Projects page JSInterop race on dispose
**File:** `Pages/Projects.razor`, lines 265-288
**Change:**
1. In the inline script, check if `window.filterPillsRef` is alive:
```javascript
if (window.filterPillsRef) {
    try {
        window.filterPillsRef.invokeMethodAsync('SetActiveTech', ...);
    } catch(e) { /* component disposed */ }
}
```
2. In `Dispose()`, nullify the global ref:
```csharp
public void Dispose()
{
    JS.InvokeVoidAsync("eval", "window.filterPillsRef = null");
    _ref?.Dispose();
}
```

---

## L1 🟢 Empty catch blocks — add logging

### Files (7 files):
- `Program.cs` line 39
- `About.razor` line 334
- `Projects.razor` line 393
- `Experience.razor` line 190
- `Resume.razor` lines 308, 316
- `Blog.razor` line 81
- `BlogPost.razor` line 281

**Fix:** Replace each `catch { }` or `catch { ... = new(); }` with:
```csharp
catch (Exception ex)
{
    Console.Error.WriteLine($"Error loading data: {ex.Message}");
    // existing fallback logic
}
```

---

## L2 🟢 Remove dead Index.razor
**File:** `Pages/Index.razor`
**Action:** Delete file (has no `@page`, never routed, `<Home />` is rendered by `Home.razor@page "/"`)

---

## L3 🟢 Remove dead CSS classes in theme.js
**File:** `wwwroot/scripts/theme.js`, lines 5-6
**Change:** Remove:
```javascript
document.body.classList.toggle('dark-preview', ...);
document.body.classList.toggle('white-preview', ...);
```

---

## L4 🟢 transition: background → background-color
**File:** `wwwroot/css/app.css`, line 52
**Change:** `transition: background var(--transition), color var(--transition);` → `transition: background-color var(--transition), color var(--transition);`
**Why:** `background` shorthand animates all sub-properties; `background-color` is more performant.

---

## L5 🟢 Subset Font Awesome (optional)
**File:** `wwwroot/index.html`, line 12
**Change:** Replace full CDN link with a subset containing only used icons. This requires creating a custom subset via Fontello or similar.
**Why:** Full Font Awesome 4.7.0 (~600 icons) loaded but only ~12 used. Saves ~50KB CSS + font files.

---

## L6 🟢 Google Fonts display=swap
**File:** `wwwroot/index.html`, line 11
**Change:** Already fixed by H2 batch (added `&display=swap`).

---

## L7 🟢 Remove empty csproj folder references
**File:** `PersonalPortfolio.v1.csproj`, lines 18-19
**Change:** Remove:
```xml
<Folder Include="Api\" />
<Folder Include="Shared\" />
```

---

## L8 🟢 Add CSP meta tag
**File:** `wwwroot/index.html`, add after line 21:
```html
<meta http-equiv="Content-Security-Policy" content="
    default-src 'self';
    script-src 'self' 'sha256-...' https://plausible.io https://cdnjs.cloudflare.com;
    style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdnjs.cloudflare.com;
    font-src 'self' https://fonts.gstatic.com https://cdnjs.cloudflare.com;
    connect-src 'self' https://plausible.io;
    img-src 'self' data: https://opengraph.githubassets.com;
    frame-src 'none';
    object-src 'none';
">
```
**Note:** Inline script hashes need to be computed after final HTML is built.

---

## L9 🟢 typing.js innerHTML flash
**File:** `wwwroot/scripts/typing.js`, line 23
**Change:** Replace `innerHTML` with:
```javascript
// Before: el.innerHTML = text.slice(0, i) + cursor;
// After:
if (!el.__textSpan) {
    el.__textSpan = document.createElement('span');
    el.__cursorSpan = document.createElement('span');
    el.__cursorSpan.className = 'typing-cursor';
    el.__cursorSpan.textContent = '|';
    el.textContent = '';
    el.appendChild(el.__textSpan);
    el.appendChild(el.__cursorSpan);
}
el.__textSpan.textContent = text.slice(0, i);
```
**Why:** `innerHTML` destroys/creates DOM nodes on each keystroke causing unnecessary layout thrash.

---

## Execution Order
1. H1, H2, H3, H4 (create new files + edit 3 files)
2. M1–M6 (edit 6 files)
3. M7–M9, M14–M15 (edit 4 files)
4. L1–L9 (edit 7 files + delete 1 file)
5. Build & test: `dotnet build`
6. Verify no regression
