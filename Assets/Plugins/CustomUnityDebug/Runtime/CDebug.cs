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
        // Short-cuts for CDebugElements
        public static readonly BaseCTag Debug = CDebugSettings.DefaultTag;
        public static readonly BaseCTag Ok = CTag.Tag("OK").Color(Color.darkGreen);

        public static readonly BaseCTag Warning =
            CTag.Tag("WARNING").Color(Color.yellow).TextType(TextTypes.Bold).LogType(LogType.Warning);

        public static readonly BaseCTag Error = CTag.Tag("ERROR").Color(Color.red).TextType(TextTypes.Bold)
            .LogType(LogType.Error);

        public static readonly BaseCTag Space = CTag.PlainText(string.Empty).Spacer(Spacers.Colon);

        public static readonly BaseCTag Text = CDebugSettings.DefaultText;

        // Empty Tag
        public static readonly CTag None = CTag.New();

        // Color Presets
        public static readonly Color32 Orange = new(255, 128, 0, 255);
        public static readonly Color32 WhiteGreen = new(124, 248, 124, 255);
        public static readonly Color32 Pink = new(255, 105, 180, 255);
        public static readonly Color32 WhiteGray = new(200, 200, 200, 255);


        /// <summary>
        /// Log Message with own Tags
        /// </summary>
        /// <param name="elements">CDebug text elements</param>
        public static void Log(params BaseCTag[] elements)
        {
            if (!CDebugSettings.DebugEnabled) return;

            using var builder = StringBuilderPool.Rent();

            var length = elements?.Length ?? 0;

            if (CDebugSettings.DebugFirstTagAlwaysDefault)
            {
                if (!TryWriteTag(CDebugSettings.DefaultTag, builder.Builder)
                    && CDebugSettings.DisableAllMessageWithTag) return;
            }

            if (length >= 1)
            {
                foreach (var element in elements)
                    if (!TryWriteTag(element, builder.Builder)
                        && CDebugSettings.DisableAllMessageWithTag) return;
            }

            // Choose a Log type
            var logType = LogType.Log;
            if (length >= 1)
            {
                foreach (var element in elements)
                {
                    if (logType >= element.RawLogType) continue;
                    logType = element.RawLogType;
                    if (logType == LogType.Error) break;
                }
            }

            switch (logType)
            {
                case LogType.Log:
                    UnityEngine.Debug.Log(builder.Builder.ToString());
                    break;
                case LogType.Warning:
                    UnityEngine.Debug.LogWarning(builder.Builder.ToString());
                    break;
                case LogType.Error:
                    UnityEngine.Debug.LogError(builder.Builder.ToString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static bool TryWriteTag(BaseCTag element, StringBuilder builder)
        {
            if (element == null || !element.Enabled)
                return false;

            if (!element.Equals(None))
            {
                builder.Append(element.FormattedStr);
            }

            return true;
        }
    }
}