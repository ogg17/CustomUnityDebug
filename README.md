# Custom Unity Debug
> Extend Unity's console with composable tags, reusable formatting, and runtime switches instead of hand-crafted `Debug.Log` strings.

<img width="780" height="400" alt="CDebugPreview" src="https://github.com/user-attachments/assets/8be67953-bd0c-4a0b-a8b5-2fa47d6c71b6" />


## Install
Install via Git URL. Unity must support the `path` query parameter (Unity >= 2019.3.4f1, Unity >= 2020.1a21).

Add the package through Package Manager:
```
https://github.com/ogg17/CustomUnityDebug.git?path=Assets/Plugins/CustomUnityDebug
```

Or add it to `Packages/manifest.json`:
```json
"com.masev.customdebug": "https://github.com/ogg17/CustomUnityDebug.git?path=Assets/Plugins/CustomUnityDebug"
```

If you need a specific release, append `#<tag>` (for example `#0.1.0`).

## Quick Start
```csharp
using Masev.CustomUnityDebug;
using Masev.CustomUnityDebug.TextFormatting;
using UnityEngine;

public sealed class Sample : MonoBehaviour
{
    private static readonly CTag Gameplay = CTag.Tag("GAMEPLAY").Color(CDebug.Orange);

    private void Start()
    {
        CDebug.Log("Hello world");                        // Default formatting
        CDebug.Log(CDebug.Warning, "Heads up!");           // Logs as a Unity warning
        CDebug.Log(Gameplay, "Player spawned".AsText());   // Custom tag + text helper
    }
}
```

- `CDebug.Log` takes any number of `BaseCTag` instances. Strings automatically become text tags, so you can mix plain strings with rich tags.
- The highest `LogType` among the supplied tags decides whether the entry is a log, warning, or error.
- Built-in helpers: `CDebug.Debug` (default tag), `CDebug.Ok`, `CDebug.Warning`, `CDebug.Error`, `CDebug.Space` (colon spacer), `CDebug.Text`, and `CDebug.None` (empty tag).

## Tags, Text, and Formatting
`CTag` is the workhorse for defining rich snippets. Create them fluently or clone existing ones at runtime.

```csharp
var system = CTag.Tag("SYSTEM")
    .Color(CDebug.LightGray)
    .Brackets(Brackets.Square);

var label = CTag.Label("Version");     // Adds the default spacer automatically
var value = CTag.Text("1.2.0")
    .Color(CDebug.LightGreen)
    .TextType(TextTypes.Bold);

CDebug.Log(system, label, value);
```

Useful factory methods and extensions:
- `CTag.Tag`, `CTag.Text`, `CTag.Label`, `CTag.Timestamp`, and `CTag.PlainText`
- `AsTag`, `AsEndTag`, `AsText`, `AsBoldText`, `AsLabel`, and `AsTimestamp` extension helpers for strings, enums, Unity objects, and `DateTime`
- `AsNew()` clones a tag so you can tweak color, text, spacer, or brackets without mutating the original

Spacing and bracket presets are handled through the `Spacers` and `Brackets` enums. Combine them to produce anything from `[TAG]` to `Tag: value` or `Loading...` out of the box.

## Runtime Controls
`CDebugSettings` exposes the global switches you typically need in production builds:

- `DebugEnabled` — master toggle for every `CDebug.Log` call.
- `DebugFirstTagAlwaysDefault` — automatically add `DefaultTag` as the first tag
- `DefaultTag`, `DefaultText`, and `DefaultSpace` — customize the look & feel of headers, text, and separators globally.
- `DisableAllMessageWithTag` — when `true`, a disabled tag aborts the entire log call instead of just skipping that tag.

You can also enable/disable individual tags on the fly with `CTag.SetEnabled(false)`. When the tag is disabled and `DisableAllMessageWithTag` is `false`, the tag is skipped but the rest of the message still emits.

## Demo Component
`Assets/Scripts/TestDebug.cs` is a MonoBehaviour that doubles as living documentation:

- Shows basic logging, tag composition, cloning, and formatting helpers
- Demonstrates how runtime switches affect output
- Highlights timestamps, coroutine logging, and spacer/bracket presets
- Provides a context menu item (`Run Demo`) so you can re-run the showcase without restarting Play mode

Drop the component into any scene, press Play, and read through the console output as a guided tour of the API.

## Tips
- Group reusable tags in static fields and clone them with `AsNew()` when you need a temporary variation.
- Use `CTag.Timestamp()` or the `DateTime.AsTimestamp()` extension to add consistent time markers.
- Combine `CDebug.Space` with section headers to visually group related logs in the console.

## Old
> Documentation for old version (0.1.0) you can find in **0.1.0_README.md**
