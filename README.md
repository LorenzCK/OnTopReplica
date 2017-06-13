A real-time, always on top, "replica" of a window of your choice, based on DWM Thumbnails.\
Completely written in C# for .NET 4.0/4.5, makes use of the [Windows Forms Aero](https://github.com/LorenzCK/WindowsFormsAero) library.

Requirements
------------

You will need the .NET 4.0/4.5 framework and Windows Vista/7/8 with Aero enabled (also known as *Desktop Composition*, which is always enabled on Windows 8).

Installation and User Guide
---------------------------

Get the [latest version (v.3.5.1)](https://github.com/paulodeleo/OnTopReplica/releases/tag/v3.5.1) from the releases section, both as installer and as standalone portable executable.

Features of current version
---------------------------

-   Clone any of your windows and keep it *always on top* while working with other windows,
-   Select a subregion of the cloned window:
    -   Store the selected subregions for future use,
    -   Now with *relative* subregions from the window's borders.
-   Auto-resizing (fit the original window, half, quarter and fullscreen mode),
-   Position lock on the screen's corners,
-   Adjustable opacity,
-   "Click forwarding" allows to interact with the cloned window,
-   "Click-through" allows to click through the cloned thumbnail (especially useful with partial opacity),
-   "Group switch" mode automatically switches through a group of windows while you use them,
-   Non invasive installation doesn't require administrator elevation.
