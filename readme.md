# AlienFX for Unity

A powerful and elegant C# library for AlienFX's SDK.

## Description

This library is an AlienFX SDK wrapper that can be used in C# and Unity. It supports all the features and functions available in Alienware SDK 5.2, and can be used in This makes it easy for application developers to control the lights on AlienFX devices and It is.

## Requirement
Unity 2018.4.13f1 or higher.

## Releases Note
See [here.](https://github.com/aiczk/AlienFX_for_Unity/releases)

## Demo

"Hello World" with Alienware AlienFX SDK.
This sample is very simple, just set the colors and exit immediately.
```C#
LightFx lfx = new LightFx();

// Initialize LightFx
lfx.Initialize();

// The version information of the SDK can be obtained via the StringBuilder.
// Note: The Capacity of the StringBuilder is 255 (byte.MaxValue).
var version = new StringBuilder(255);  
lfx.GetVersion(version);
Debug.Log($"SDK Version: {version}");

// Set the brightness. (The default brightness is Min(0x00000000))
LfxColorEncode color = LfxColorEncode.Green.Brightness(LfxBrightness.Half);

// The Light function has three types of overrides.
// AlienFX.LfxColorEncode
lfx.Light(LfxLocationMask.All, LfxColorEncode.Green);
// AlienFX.LfxColor
lfx.Light(LfxLocationMask.All, new LfxColor(0, 255, 0, 255));
// UnityEngine.Color
lfx.Light(LfxLocationMask.All, new Color(0, 1, 0, 1));

// Causes the physical color change
lfx.Update();

// Cleanup and detach from the system
lfx.Release();
```

## Implementation schedule
Extensions to the features available using the editor extension.


##  License
[MIT](https://github.com/aiczk/AlienFX_for_Unity/blob/master/license.txt)