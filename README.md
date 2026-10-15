<div align="center">

# 🛡️ EtsyVault
### The #1 Free, Local-First Backup & Disaster Recovery Tool for Etsy Sellers

[![GitHub Release](https://img.shields.io/github/v/release/thieuhoang2002/EtsyVault?color=blue&logo=github)](https://github.com/thieuhoang2002/EtsyVault/releases/latest)
[![GitHub Downloads](https://img.shields.io/github/downloads/thieuhoang2002/EtsyVault/total?color=success&logo=github)](https://github.com/thieuhoang2002/EtsyVault/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20macOS-brightgreen?logo=apple&logoColor=white)]()
[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)]()
[![Avalonia UI](https://img.shields.io/badge/UI-Avalonia%2012-purple?logo=avalonia)]()
[![Web Suite](https://img.shields.io/badge/Web%20Operations-Etsy%20Command%20Center-FF6B00?logo=googlechrome&logoColor=white)](https://shopops.thhoang.io.vn)

<p align="center">
  <b>Safeguard your entire Etsy shop in 1-click. Archive listings, tags, variation matrices, inventory, and original full-resolution photos directly to your PC. Ready-to-import Shopify CSV export included.</b>
</p>

[⬇️ Download Free Releases](#-quick-download) •
[✨ Key Features](#-features) •
[🔄 How It Connects](#-how-does-etsyvault-connect-to-your-shop) •
[❓ FAQ](#-frequently-asked-questions-faq) •
[☕ Support](#-support-the-project)

</div>

---

## ⚡ Why EtsyVault?

Etsy account suspensions and sudden shop closures happen unexpectedly without prior notice. Sellers frequently lose years of optimized titles, descriptions, variation matrices, listing tags, and irreplaceable high-resolution product photography.

**EtsyVault gives you 100% ownership and peace of mind:**
- 🛡️ **Zero Cloud Exposure (100% Private):** Runs completely on your device. Your shop credentials, listing details, and customer information never touch any third-party cloud server or external database.
- 📦 **1-Click Full Shop Backup:** Archives active, draft, and expired listings with all tags, materials, inventory matrices, and SKU data.
- 🖼️ **Original Full-Resolution Photos:** Bypasses compressed thumbnails to fetch and save original full-size product photos straight to organized folders on your PC.
- 🛍️ **Disaster Recovery Ready (Shopify CSV Export):** Automatically compiles and exports a ready-to-import **Shopify CSV** and comprehensive **Excel sheets** for instant multi-channel selling or store restoration.
- ⏱️ **Safe Throttling (Anti-Ban Guarantee):** Engineered with strict Leaky-Bucket rate limiting (~2.5 QPS) adhering strictly to the **[Official Etsy OpenAPI 3.0 Specification](https://www.etsy.com/openapi/generated/oas/3.0.0.json)** to prevent API quota exhaustion.

---

## 🚀 Quick Download

No compiling or command-line setup required. Download the pre-built desktop application for your operating system:

| Platform | Architecture | Direct Download Link |
| :--- | :--- | :--- |
| **🪟 Windows** | 64-bit (x64) | [**⬇️ Download EtsyVault-Windows-x64.zip**](https://github.com/thieuhoang2002/EtsyVault/releases/latest/download/EtsyVault-Windows-x64.zip) |
| **🍏 macOS** | Apple Silicon (M1/M2/M3/M4) | [**⬇️ Download EtsyVault-macOS-AppleSilicon-arm64.tar.gz**](https://github.com/thieuhoang2002/EtsyVault/releases/latest/download/EtsyVault-macOS-AppleSilicon-arm64.tar.gz) |
| **🍏 macOS** | Intel Processor (x64) | [**⬇️ Download EtsyVault-macOS-Intel-x64.tar.gz**](https://github.com/thieuhoang2002/EtsyVault/releases/latest/download/EtsyVault-macOS-Intel-x64.tar.gz) |

> 📌 *Note for Windows / Mac SmartScreen warnings: Because this is an open-source indie application, please check [Bypassing OS Security Warnings](docs/os-security-warnings.md) for quick 5-second launch instructions.*

---

## 📊 Feature Comparison: Why Choose EtsyVault?

| Capability | EtsyVault (Free & Open Source) | Etsy Default CSV Export | Expensive SaaS Tools ($30–$90/mo) |
| :--- | :---: | :---: | :---: |
| **Price** | **100% Free Forever** | Free | Monthly recurring fee |
| **Download Original Photos** | ✅ **Yes (Full Resolution)** | ❌ No (Text only) | ⚠️ Cloud-stored only |
| **Shopify CSV Migration** | ✅ **Yes (Ready to import)** | ❌ No (Requires manual format) | ✅ Yes |
| **Privacy & Security** | ✅ **100% Local (On your PC)** | ✅ On Etsy | ❌ Stored on 3rd-party clouds |
| **Tags & Variations Matrix** | ✅ **Complete breakdown** | ⚠️ Partial | ✅ Yes |
| **Official OpenAPI v3 Compliance**| ✅ **Strict RFC 7636 PKCE** | N/A | ⚠️ Often scrape illegally |

---

## 📸 Screenshots

| 1. Ready & Demo Mode | 2. API Credentials & Advanced Settings |
| :---: | :---: |
| ![Demo Mode](screenshots/01-demo-mode.png) | ![Credentials Setup](screenshots/02-credentials-setup.png) |
| **3. Realtime Concurrency Logs & Progress** | **4. Backup Complete & Disaster Recovery** |
| ![Backup Progress](screenshots/03-backup-progress.png) | ![Backup Success](screenshots/04-backup-success.png) |

---

## 🔄 How Does EtsyVault Connect to Your Shop?

You might wonder: *“How does the app know which shop to download just from a Keystring?”*

Here is the simple, 100% private 4-step connection process:

1. **Keep Your Normal Web Browser Logged In:** Keep your shop account signed in on your standard browser (Chrome, Safari, Edge).
2. **Enter your Keystring & Click Start:** The app opens your browser to Etsy’s official authorization consent page (`https://www.etsy.com/oauth/connect`).
3. **One-Click Authorization:** Because your browser already knows who you are, you simply click **"Grant Access"**. Etsy securely issues a temporary access token directly to the local loopback listener running on your machine via RFC 7636 OAuth 2.0 PKCE.
4. **Auto-Discovery of Your Shop (`/users/me/shops`):** The app calls Etsy’s official endpoint to ask: *“Which shop belongs to this authenticated seller?”*. Etsy returns your exact `shop_id`, and EtsyVault immediately begins archiving your listings, inventory, and original high-resolution photos!

> 🔒 **Zero Configuration & Zero Risk:** You never need to look up your internal Shop ID manually. No passwords or secret keys are ever shared with any third party.

---

## 🛠️ Step-by-Step Setup Guide

1. Download the latest release for your platform from the [Quick Download](#-quick-download) table or the [Releases](https://github.com/thieuhoang2002/EtsyVault/releases) page.
2. Follow our quick [2-Minute Free Etsy API Key Setup Guide](docs/how-to-get-etsy-api-key.md) to generate your free personal Keystring.
3. Open `EtsyVault`, paste your Keystring, choose your destination folder, and click **🚀 Start Full Backup**!

---

## 🏗️ Technical Architecture & Under the Hood

- **Runtime:** .NET 10 SDK (High-performance cross-platform runtime)
- **UI Framework:** AvaloniaUI 12 + FluentAvalonia (Modern Windows 11 Fluent Design & macOS native styling)
- **Architecture:** Clean MVVM (`CommunityToolkit.Mvvm`)
- **Excel & CSV Engine:** `MiniExcel` (Stream-based memory-efficient processing for shops with 10,000+ listings)
- **Concurrency Pipeline:** `System.Threading.Channels` for parallel multi-worker high-speed image downloads without UI freezing
- **Security Standard:** RFC 7636 OAuth 2.0 PKCE authentication with local loopback listener

👉 *For architectural diagrams, sequence flows, and concurrency pipeline analysis, see [System Architecture & Engineering Deep-Dive](docs/architecture.md).*

---

## ❓ Frequently Asked Questions (FAQ)

<details>
<summary><b>1. Can I use EtsyVault to migrate my shop to Shopify?</b></summary>
<br>
Yes! EtsyVault automatically formats and generates an import-ready <code>Shopify_Import.csv</code> alongside your raw Excel file. You can import this file directly into Shopify's Products admin without manually reformatting columns, pricing, titles, tags, or descriptions.
</details>

<details>
<summary><b>2. Will EtsyVault download my full-size product photos or just thumbnails?</b></summary>
<br>
EtsyVault resolves the highest resolution image URL available on Etsy's CDN (including uncompressed original uploads), saving them into neatly organized folders matching each listing ID and SKU.
</details>

<details>
<summary><b>3. Can using EtsyVault get my Etsy shop banned or rate-limited?</b></summary>
<br>
No. EtsyVault is built strictly according to Etsy's official developer policies. It enforces a strict Leaky-Bucket rate limiter capped at ~2.5 Queries Per Second (well below Etsy's official limit of 10 QPS), ensuring zero risk of API throttling or suspension.
</details>

<details>
<summary><b>4. Does EtsyVault cost anything? Are there hidden fees?</b></summary>
<br>
EtsyVault is 100% free, libre, and open-source software licensed under MIT. There are no subscriptions, limits on listings count, or premium paywalls.
</details>

<details>
<summary><b>5. Is my Etsy data safe?</b></summary>
<br>
Absolutely. EtsyVault is a <b>local-first desktop software</b>. It does not possess any backend server, telemetry tracking, or analytics. All authentication tokens and shop data reside purely in your computer's RAM and local disk directory.
</details>

---

## 🌐 Looking to Scale Your Etsy Operations & Sales?

While **EtsyVault** specializes in local asset backup and disaster recovery, check out our companion web platform:

👉 **[Command Center for Etsy](https://shopops.thhoang.io.vn)** — *All-in-One Operations, Finance & Automation Suite*
- 🎯 **Competitor Radar & Sales Delta:** Track real-time listing sales velocities and discover untapped niches.
- 🛡️ **Trademark Shield:** Automated keyword risk scanning to protect listings against IP strikes.
- 📈 **Net P&L Calculator:** Precise profit breakdown accounting for Etsy transaction fees, VAT, and ad spend.
- 🔐 **Zero-Knowledge E2EE Vault:** Bank-grade client-side encryption for secure operations.

---

## ☕ Support the Project

EtsyVault is developed and maintained as an independent open-source tool. If this tool brought you peace of mind or saved hours of work, consider supporting development:

<p align="left">
  <a href="https://buymeacoffee.com/thhoang0903" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" style="height: 50px !important;width: 181px !important;" ></a>
</p>

You can also support via **[Ko-fi](https://ko-fi.com/thhoang0903)** or **[PayPal](https://paypal.me/thieuhoang2209)**.

---

## 📄 License

Distributed under the **MIT License**. See [`LICENSE`](LICENSE) for more information.
