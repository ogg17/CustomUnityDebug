using System;
using System.Text;
using Masev.CustomUnityDebug.TextFormatting;
using UnityEngine;
using LogType = Masev.CustomUnityDebug.TextFormatting.LogType;

namespace Masev.CustomUnityDebug
{
    /// <summary>
    /// Main Custom Debug Class
    /// </summary>
    public static class CDebug
    {
        // Color Presets
        public static readonly Color32 Orange = new(255, 128, 0, 255);
        public static readonly Color32 LightGreen = new(124, 248, 124, 255);
        public static readonly Color32 DarkGreen = new(0, 90, 0, 255);
        public static readonly Color32 Pink = new(255, 105, 180, 255);
        public static readonly Color32 LightGray = new(200, 200, 200, 255);
        
        // Short-cuts for CDebugElements
        public static readonly BaseCTag Debug = CDebugSettings.DefaultTag;
        public static readonly BaseCTag Ok = CTag.Tag("OK").Color(DarkGreen);
        public static readonly BaseCTag Warning = CTag.Tag("WARNING").Color(Color.yellow)
            .TextType(TextTypes.Bold).LogType(LogType.Warning);
        public static readonly BaseCTag Error = CTag.Tag("ERROR").Color(Color.red)
            .TextType(TextTypes.Bold).LogType(LogType.Error);
        public static readonly BaseCTag Space = CTag.PlainText(string.Empty).Spacer(Spacers.Colon);
        public static readonly BaseCTag Text = CDebugSettings.DefaultText;

        // Empty Tag
        public static readonly CTag None = CTag.New();


        /// <summary>
        /// Log Message with own Tags
        /// </summary>
        /// <param name="elements">CDebug text elements</param>
        public static void Log(params BaseCTag[] elements)
        {
            if (!CDebugSettings.DebugEnabled) return;

            using var builder = StringBuilderPool.Rent();
            var disableOnInvalidTag = CDebugSettings.DisableAllMessageWithTag;

            if (CDebugSettings.DebugFirstTagAlwaysDefault)
            {
                if (CDebugSettings.DefaultTag == null || !CDebugSettings.DefaultTag.Enabled)
                {
                    if (disableOnInvalidTag) return;
                }
                else
                {
                    builder.Builder.Append(CDebugSettings.DefaultTag.FormattedStr);
                }
            }

            var logType = LogType.Log;

            if (elements != null)
            {
                foreach (var element in elements)
                {
                    if (element == null || !element.Enabled)
                    {
                        if (disableOnInvalidTag) return;
                        continue;
                    }

                    builder.Builder.Append(element.FormattedStr);

                    if (logType >= element.RawLogType) continue;
                    logType = element.RawLogType;
                }
            }

            var message = builder.Builder.ToString();

            switch (logType)
            {
                case LogType.Log:
                    UnityEngine.Debug.Log(message);
                    break;
                case LogType.Warning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case LogType.Error:
                    UnityEngine.Debug.LogError(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
