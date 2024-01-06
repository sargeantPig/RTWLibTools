![workflow](https://github.com/sargeantPig/RTWLibTools/actions/workflows/macos-build-test.yml/badge.svg) ![workflow](https://github.com/sargeantPig/RTWLibTools/actions/workflows/linux-build-test.yml/badge.svg)![workflow](https://github.com/sargeantPig/RTWLibTools/actions/workflows/windows-build-test.yml/badge.svg)
# RTWLibTools
Cross-platform Tools and randomiser for Rome Total War. 

This a re-write of the original RTW Randomiser mod. This version is currently CLI only despite this it is simple to setup and use. In the future there will be a UI made for it. The justification for CLI is it keeps things simple for now and allows for the main randomisation functions to be fully tested and working before adding a UI on top.

## Installation

Installing the Randomiser is a simple process. All the assets required can be found on the [release page](https://github.com/sargeantPig/RTWLibTools/releases). It is recommended to use the latest release unless specifically stated.

The below screenshot shows the randomiser files underlined. Use this as a reference to make sure files are in the correct location.

![image](https://github.com/sargeantPig/RTWLibTools/assets/16241051/4d690f02-e93f-4510-9ced-4b72f82285fc)

Contained in the release assets found on the aforementioned release page should be the following files:

- Application file ( choose the correct one for your OS ) eg. RTW-CLI-Randomiser-windows.exe for windows.
- mod_files.zip ( contains all the standard RTW files for the mod that the randomiser modifies ) 
- settings.zip ( contains two folders, randomiser_config and randomiser_templates )

  1: Place the relevant Application file into the folder
  - windows: C:\Users\YOURNAME\AppData\Local\Feral Interactive\Total War ROME REMASTERED\
  - mac: /Users/YOURNAME/Library/Application Support/Feral Interactive/Total War Rome REMASTERED/
  - linux: ~/.local/share/feral-interactive/Total War ROME REMASTERED/ ( this could be incorrect, will be similar to this though )
    
  2: Extract the mod_files.zip to the same folder. This should merge with the Mod folder already located there.
  
  3: Extract the settings.zip to the same folder.

After these steps have been completed the Randomiser will be ready to use.


