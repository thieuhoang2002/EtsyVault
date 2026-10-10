# 🛡️ EtsyVault

> **Open-source, local-first backup & disaster recovery tool for Etsy sellers.**  
> Built strictly against the **[Official Etsy OpenAPI 3.0 Specification](https://www.etsy.com/openapi/generated/oas/3.0.0.json)**.

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20macOS-brightgreen)]()
[![Buy Me A Coffee](https://img.shields.io/badge/Buy%20Me%20A%20Coffee-thhoang0903-orange?logo=buy-me-a-coffee)](https://buymeacoffee.com/thhoang0903)
[![Ko-fi](https://img.shields.io/badge/Ko--fi-thhoang0903-FF5E5B?logo=ko-fi)](https://ko-fi.com/thhoang0903)
[![PayPal](https://img.shields.io/badge/PayPal-Donate-00457C?logo=paypal)](https://paypal.me/thieuhoang2209)

---

## ⚡ Why EtsyVault?

Etsy account suspensions happen unexpectedly, and sellers frequently lose years of optimized titles, descriptions, variation matrices, and high-resolution product photography.

**EtsyVault** safeguards your business:
- 📦 **1-Click Full Backup:** Archives all listings, tags, materials, inventory, and SKU data.
- 🖼️ **Original Resolution Images:** Downloads original full-size product photos straight to organized folders.
- 🛍️ **Disaster Recovery Ready:** Automatically exports ready-to-import **Shopify CSV** and comprehensive **Excel sheets**.
- 🔒 **100% Private & Local:** Runs completely on your device. Zero external cloud servers, zero telemetry.
- ⏱️ **Safe Throttling:** Strict Leaky-Bucket rate limiting (~2.5 QPS) to strictly prevent Etsy API quota exhaustion.

---

## 🚀 Quick Start

1. Download the latest release for your platform from the [Releases](https://github.com) page.
2. Follow our [2-Minute Free Etsy API Key Setup Guide](docs/how-to-get-etsy-api-key.md).
3. Open `EtsyVault`, enter your Keystring, and click **Start Backup**!

*(Having trouble opening the app on Windows or Mac? See [Bypassing OS Warnings](docs/os-security-warnings.md).)*

---

## 🛠️ Tech Stack & Architecture

- **Runtime:** .NET 10 SDK (Targeting .NET 8 LTS)
- **UI Framework:** AvaloniaUI 12 + FluentAvalonia (Modern Windows 11 Fluent Design)
- **Architecture:** MVVM (`CommunityToolkit.Mvvm`)
- **Excel/CSV Engine:** MiniExcel (High-throughput streaming)
- **Concurreny:** `System.Threading.Channels` for parallel multi-worker image downloads

---

## ☕ Support the Project

EtsyVault is free and open-source. If this tool brought you peace of mind or saved hours of work, consider supporting development:

<a href="https://buymeacoffee.com/thhoang0903"><img src="https://img.buymeacoffee.com/button-api/?text=Buy me a coffee&emoji=☕&slug=thhoang0903&button_colour=FFDD00&font_colour=000000&font_family=Cookie&outline_colour=000000&coffee_colour=ffffff" /></a>

You can also support via **[Ko-fi](https://ko-fi.com/thhoang0903)** or **[PayPal](https://paypal.me/thieuhoang2209)**.

---

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.
