# How to Get Your Free Etsy Developer API Key (Personal Access)

To use **EtsyVault**, you need your personal Etsy API Keystring (Client ID). This guarantees **100% privacy and security** because all communications happen directly between your computer and Etsy's official servers. No middleman, no tracking.

Etsy provides free **Personal Access (5 requests/sec, 5,000 requests/day)** for shop owners. Follow this 2-minute guide to get approved.

---

### Step 1: Access the Etsy Developer Portal
1. Sign in to your Etsy account at [etsy.com](https://www.etsy.com).
2. **Turn off any VPN or Proxy** (use your standard home or office IP address to avoid automated fraud flags).
3. Visit: **[https://www.etsy.com/developers/register](https://www.etsy.com/developers/register)**

---

### Step 2: Fill Out the Registration Form

#### 1. Application Name
- Enter a name related to your internal shop management (e.g., `MyStudio-Archive-Tool` or `CraftStore-Backup`).
- ⚠️ **CRITICAL RULE:** Do **NOT** use the word **"Etsy"** anywhere in the name (Etsy strictly blocks trademark keywords).

#### 2. Describe your application (Under 500 characters)
Copy and paste this pre-approved application template (replace `[Your Shop Name]` with your actual shop name):

```text
I am currently managing [Your Shop Name] on Etsy and developing a private operations tool for my shop. This internal desktop app connects to our local drive via Etsy API to back up our listing catalog, sync stock levels, and organize product assets safely. It is strictly for my own shop's personal operations, runs locally on my machine, and will never share or transfer any data to third parties.
```

#### 3. Application Details Checklist
- **What type of application are you building?**
  - 👉 Select: `[x] Seller Tools` *(Do NOT select Buyer Tools or Mobile)*
- **Who will be the users of this application?**
  - 👉 Select: `[x] Just myself or colleagues` *(Never select public)*
- **Is your application commercial?**
  - 👉 Select: `[x] No`
- **Will your app do any of the following?**
  - 👉 Check: `[x] Upload or edit listings`
  - 👉 Check: `[x] Read sales data`
  - ⚠️ **Leave UNCHECKED:** `Send email` *(Etsy rejects any automated email requests)*

#### 4. URLs
- **Website URL:** Your GitHub profile, portfolio, or store URL.
- **Callback URL / Redirect URI:** `http://localhost:54321/callback`

---

### Step 3: Receive Approval
1. Click **Register Application**.
2. You will immediately receive your **Keystring (Client ID)** and **Shared Secret**.
3. Your initial status will display 🟡 **`Pending Personal Approval`**.
4. Within **12–24 hours**, refresh the page. The badge will turn 🔵 **`Personal Access`** (5 QPS, 5K QPD).
5. Copy your **Keystring** into EtsyVault and start your backup!
