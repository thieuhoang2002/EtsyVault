# How to Get Your Free Etsy Developer API Key (Personal Access)

To use **EtsyVault**, you need your personal Etsy API Keystring (Client ID). This guarantees **100% privacy and security** because all communications happen directly between your computer and Etsy's official servers. No middleman, no tracking.

Because you already own an active Etsy shop, obtaining **Personal Access (5 requests/sec, 5,000 requests/day)** is free, fast, and approved virtually **100% of the time within 12–24 hours**.

Follow this 2-minute step-by-step guide to get approved.

---

### Step 1: Access the Etsy Developer Portal
1. Make sure you are signed in to the Etsy account that **owns your active shop**.
2. **Turn off any VPN or Proxy** (use your normal home or office internet connection so Etsy recognizes your trusted seller IP).
3. Visit: **[https://www.etsy.com/developers/register](https://www.etsy.com/developers/register)**

---

### Step 2: Fill Out the Application Form

#### 1. Application Name
- Enter a name related to your internal shop operations (e.g., `MyStudio-Archive-Tool` or `Workshop-Catalog-Sync`).
- ⚠️ **CRITICAL RULE:** Do **NOT** use the word **"Etsy"** anywhere in the name (Etsy strictly blocks applications containing their trademark).

#### 2. Describe your application (Under 500 characters)
Copy and paste this pre-approved application template (replace `[Your Shop Name]` with your actual Etsy shop name):

```text
I am the owner of [Your Shop Name] on Etsy and developing a private operational backup tool for our store. This internal desktop app runs locally on my machine and connects via the Etsy API to archive our listing catalog, download original photography assets, and keep local inventory backups in case of emergency. It is strictly for my own shop's private management, runs entirely locally, and will never share or transfer any data to third parties.
```

#### 3. Application Details Checklist
- **What type of application are you building?**
  - 👉 Select: `[x] Seller Tools` *(Do NOT select Buyer Tools or Mobile)*
- **Who will be the users of this application?**
  - 👉 Select: `[x] Just myself or colleagues` *(Crucial: Never select public)*
- **Is your application commercial?**
  - 👉 Select: `[x] No`
- **Will your app do any of the following?**
  - 👉 Check: `[x] Upload or edit listings` *(Required for reading catalog details)*
  - 👉 Check: `[x] Read sales data` *(Optional, but signals internal operations)*
  - ⚠️ **Leave UNCHECKED:** `Send email` *(Never check this — Etsy heavily restricts automated emailing)*

#### 4. URLs
- **Website URL:** Your Etsy shop URL (e.g., `https://www.etsy.com/shop/YourShopName`) or your personal portfolio/website.
- **Callback URL / Redirect URI:** `http://localhost:54321/callback`

---

### Step 3: Receive Approval
1. Click **Register Application**.
2. You will immediately receive your **Keystring (Client ID)** and **Shared Secret**.
3. Your initial status will display 🟡 **`Pending Personal Approval`**.
4. Within **12–24 hours**, refresh the developer portal page. Because you are an established shop owner, your application is typically approved automatically with the badge 🔵 **`Personal Access`** (5 QPS, 5K QPD).
5. Copy your **Keystring** into EtsyVault and start your backup!
