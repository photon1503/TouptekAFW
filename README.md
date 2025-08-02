# ASCOM FilterWheel Driver for Multiple Touptek AFW Devices

**A  ASCOM-compliant driver for managing multiple Touptek filter wheels simultaneously**

## Features

 **Multi-device support** - Control up to two Touptek AFW filter wheels concurrently on a single PC  
**USB ID-based identification** - Uses unique USB identifiers instead of generic "FILTERWHEEL" device names  
**ASCOM Certified** - Fully passes the ASCOM Conformance Test ([View Report](ascom-conform.txt))  
  

## Why This Driver?

The standard Touptek filter wheel drivers have limitations when multiple devices are connected. This driver solves:
- The naming conflict when multiple wheels identify as "FILTERWHEEL"
- The inability to control more than one wheel simultaneously without manual intervention


## Installation
1. Download the latest release
2. Run the installer
3. Select your devices by USB ID in your imaging software's ASCOM chooser

*Requires ASCOM Platform 6.5 or later*
  ![Setup screen](image.png)

  ![Usage in NINA](image-1.png)