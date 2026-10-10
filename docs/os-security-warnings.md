# How to Open EtsyVault on Windows & macOS (SmartScreen & Gatekeeper)

EtsyVault is a 100% free, open-source project. Because code signing certificates cost hundreds of dollars per year from Apple and Microsoft, newly released community builds may trigger OS security warnings. 

Rest assured: **EtsyVault contains zero trackers, zero telemetry, and its full source code is publicly auditable on GitHub.**

---

## 🪟 Windows (Microsoft Defender SmartScreen)

When running `EtsyVault.exe` for the first time, Windows SmartScreen may show a blue popup stating:  
> *"Windows protected your PC: Microsoft Defender SmartScreen prevented an unrecognized app from starting."*

### How to open:
1. Click the text link: **"More info"** *(Thông tin khác)*.
2. Click the button: **"Run anyway"** *(Vẫn chạy)*.
3. The app will launch normally and will not prompt you again.

---

## 🍎 macOS (Apple Gatekeeper)

On macOS, downloading third-party unsigned binaries may show:  
> *"EtsyVault cannot be opened because it is from an unidentified developer"* or *"App is damaged and can’t be opened."*

### How to open (Option 1 - Graphical):
1. Locate `EtsyVault.app` in Finder.
2. **Right-click (or Control-click)** on the app icon and select **Open**.
3. In the security popup, click **Open** once more.

### How to open (Option 2 - Terminal):
If macOS flags the quarantine attribute, open Terminal and run:
```bash
xattr -cr /Applications/EtsyVault.app
```
Then double-click to launch.
