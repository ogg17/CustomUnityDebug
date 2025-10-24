# Custom Unity Debug
> Custom Unity Debug is an open-source extension for Unity's Debug.Log, offering enhanced logging with customizable tags, colors, and formatting. It allows developers to define, categorize, and filter log messages efficiently. Features include multi-tag logging, global and per-tag enable/disable controls, and seamless integration with Unity's console.

![image](https://github.com/user-attachments/assets/6575f530-011e-4e31-9bdc-e6e12e468336)

## Install
Install via git URL

Requires a version of unity that supports path query parameter for git packages (Unity >= 2019.3.4f1, Unity >= 2020.1a21). 
You can add https://github.com/ogg17/CustomUnityDebug.git?path=Assets/Plugins/CustomUnityDebug to Package Manager

or add "com.masev.customdebug": "https://github.com/ogg17/CustomUnityDebug.git?path=Assets/Plugins/CustomUnityDebug" to Packages/manifest.json.

If you want to set a target version, UniTask uses the *.*.* release tag so you can specify a version like #0.1.0. 
For example https://github.com/ogg17/CustomUnityDebug.git?path=Assets/Plugins/CustomUnityDebug#0.1.0.

## Usage

### Basic
To perform basic logging with the default tag, use the following syntax:

```csharp
    CDebug.Log("Test!");
```
This will log the message "Test!" with the default settings.

For logging warnings, you can specify the `CDebug.WARNING` tag:

```csharp
    CDebug.Log(CDebug.WARNING, "Test!");
```

Similarly, to log errors, use the `CDebug.ERROR` tag:

```csharp
    CDebug.Log(CDebug.ERROR, "Test!");
```
These predefined tags help in distinguishing between regular logs, warnings, and errors. The `WARNING` tag internally calls `Debug.LogWarning`, while the `ERROR` tag calls Debug.LogError for proper logging in Unity.
#### Setting the Default Tag

You can set the default logging tag using the `CDebug.DefaultTag` property. This is useful if you want to globally define a default tag for all logs:

```csharp
CDebug.DefaultTag = CustomTag.TEST;
```

Once set, any log call without a specified tag will default to `CustomTag.TEST`, unless otherwise overridden.

### Custom Tags

Begin by defining an enumeration that represents your custom tags:

#### Defining Custom Tags

```csharp
    public enum CustomTag
    {
        TEST,
        FEATURE_X,
        DATABASE,
        UI,
        NETWORK,
    }
```
This enumeration serves as a list of identifiers for your custom tags.

#### Adding Custom Tags to CDebug

Once you've defined your enumeration, add these custom tags to `CDebug` using the `TryAddTag` method. This method associates each enum value with a `CDebugTag` object, which defines the tag's appearance and formatting.

The `TryAddTag` method has the following signature:
```
    bool CDebug.TryAddTag(Enum tag, CDebugTag debugTag)
```

The `CDebugTag` constructor is defined as:
```
    CDebugTag(string tag, Color32 color, bool bold, bool italic)
```
* `tag`: The display name of the tag.
* `color`: The color to be used for the tag.
* `bold`: A boolean indicating whether the tag text should be bold.
* `italic`: A boolean indicating whether the tag text should be italicized.

Example:
```csharp
    CDebug.TryAddTag(CustomTag.TEST, new CDebugTag("TEST", Color.blue, false, false));
    CDebug.TryAddTag(CustomTag.FEATURE_X, new CDebugTag("FEATURE_X", CDebug.Pink, false, true));
    CDebug.TryAddTag(CustomTag.DATABASE, new CDebugTag("DATABASE", CDebug.WhiteGray, true, false));
    CDebug.TryAddTag(CustomTag.UI, new CDebugTag("UI", CDebug.Orange, true, true));
    CDebug.TryAddTag(CustomTag.NETWORK, new CDebugTag("NETWORK", Color.green, false, false));
```
In this example, each custom tag is assigned a unique name and formatting style.

#### Using Custom Tags in Logging

After setting up your custom tags, you can use them in your logging statements as follows:
```csharp
    CDebug.Log(CustomTag.TEST, "This is a test message.");
    CDebug.Log(CustomTag.FEATURE_X, "Feature X has been initialized.");
    CDebug.Log(CustomTag.DATABASE, "Database connection established.");
    CDebug.Log(CustomTag.UI, "User interface loaded successfully.");
    CDebug.Log(CustomTag.NETWORK, "Network request completed.");
```

### Logging with Multiple Tags

`CDebug` also allows logging with multiple tags in a single log entry. You can pass up to four tags as parameters to the `Log` method:
```csharp
CDebug.Log(CustomTag.UI, CustomTag.NETWORK, "UI received a network response.");
CDebug.Log(CustomTag.DATABASE, CustomTag.FEATURE_X, CustomTag.TEST, "Feature X is processing database test data.");
```

### Enabling and Disabling Logging

You can control the overall logging behavior in your application as well as enable or disable specific tags for finer control.

#### Enabling and Disabling Global Logging

To enable or disable all logging globally, you can use the `CDebug.DebugEnabled` property:

```csharp
CDebug.DebugEnabled = true;  // Enables all debugging logs
CDebug.DebugEnabled = false; // Disables all debugging logs
```

When set to `false`, all logs across the application will be suppressed.

#### Enabling and Disabling Specific Tags

If you only want to disable certain tags, you can control individual tag logging using `CDebug.SetTagEnabled`:

```csharp
CDebug.SetTagEnabled(CustomTag.TEST, true);   // Enables logs for the TEST tag
CDebug.SetTagEnabled(CustomTag.TEST, false);  // Disables logs for the TEST tag
```

This allows you to selectively turn off logging for specific tags while keeping others enabled.
