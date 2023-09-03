**This is a prototype.**

# Overview
This is a simple clean video player written in C#. This was made to make watching & following tutorials easier.
The player will float above any app,
currently I try to keep it to as little ui as possible and handle all interactions using hotkeys.
![image](https://github.com/2latemc/VideoPlayer/assets/89020720/00cfe80b-71da-41f8-83fc-6c24a6d7699d)


# Installation
Installer will be added later.

Download the [lastest release](https://github.com/2latemc/VideoPlayer/releases) from the release page. 
1. Open *C:\Program Files* and make a new folder called Video Player.
2. Copy the downloaded files into the new folder
3. Right click the exe, create a shortcut, move the shortcut to **C:\Users\<user>\AppData\Roaming\Microsoft\Windows\Start Menu**
4. Right click a video file -> Open With -> Choose another app -> Scroll down and select "More Apps" -> Browse the programs exe and select open
5. Right click a video file again, go to open with, Choose another app, and select the Video Player. Make sure to tick "Always use this App to open"


# Usage

#### Enter / Exit fullscreen - F

#### Enable / Disable always on Top - T
#### Skip forwards / backwards - Arrow Keys
#### Pause / Unpause - Space / Left Mouse
#### Exit - Alt + F4

Hover over the left part of the video to show the volume control.

# Roadmap
1. Implement timeline
2. ~~Fix volume controls~~
3. Implement volume controls hotkeys
4. Make UI prettier
5. Create Installer
6. Support more media types
7. Proper error handling
8. Visual feedback when skipping & changing volume & Topmost
