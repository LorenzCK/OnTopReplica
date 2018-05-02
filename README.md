# OnTopReplica

**A real-time always-on-top “replica” of a window of your choice, for Windows Vista, 7, 8, or 10.**

This simple utility application shows a blank always-on-top window by default.
Users can pick any other window of the system to have an always up-to-date clone of the target window shown always-on-top.
Very useful for monitoring background processes, wrangling with complex multi-window games or tools, watching Youtube videos while working, and so on.

**📢 Features:**

* Clone any of your windows and keep it *always-on-top* while working with other windows,
* Select a subregion of the cloned window, which:
  * Can be stored for future use,
  * Can use relative coordinates from the target window’s borders.
* Auto-resizing (fit the original window, half, quarter and fullscreen mode),
* Position lock on any corner of your screen,
* Adjustable opacity,
* “Click forwarding”: allows to interact with the cloned window,
* “Click-through”: makes the replica ignore any mouse interaction (turns **OnTopReplica** into an overlay if set together with partial opacity),
* “Group switch”-mode automatically switches through a group of windows while you use them.

## Requirements

* Microsoft Windows Vista or greater (the application makes use of native DWM&nbsp;Thumbnails to create replicas),
* Microsoft .NET Framework 4.7.
* Desktop Composition (a.k.a. Windows *Aero*) enabled.

## Installation

Get the [latest version](https://github.com/LorenzCK/OnTopReplica/releases) from the releases section as an MSI&nbsp;installer.

## Contributions

…are very welcome. Fork away! 🍽️

Submitting [issues](https://github.com/LorenzCK/OnTopReplica/issues) and other feedback is also appreciated.

### Roadmap

1. ✅&nbsp;Update to the newest [WindowsFormsAero](https://github.com/LorenzCK/WindowsFormsAero) version.
1. ✅&nbsp;Migrate to .NET 4.7.
1. Improve/add **High DPI** support!
1. “Stored scenarios” that, just like stored regions, automatically clone a window (based on title or window class criteria), select a region, and set other options. Ideally to be used as Taskbar shortlinks.
1. Move to the Windows Store, via Centennial. 🤞

## License

**OnTopReplica** is licensed under the [MS-RL (Microsoft Reciprocal License)](https://github.com/LorenzCK/OnTopReplica/blob/master/LICENSE).
