using System;
using Masev.CustomUnityDebug.TextFormatting;
using UnityEngine;
using LogType = Masev.CustomUnityDebug.TextFormatting.LogType;

namespace Masev.CustomUnityDebug
{
    /// <summary>
    /// Entry point for composing Unity console messages that reuse the tagging helpers from this package.
    /// </summary>
    public static class CDebug
    {
        public static readonly Color32 Orange = new(255, 128, 0, 255);
        public static readonly Color32 LightGreen = new(124, 248, 124, 255);
        public static readonly Color32 DarkGreen = new(0, 90, 0, 255);
        public static readonly Color32 Pink = new(255, 105, 180, 255);
        public static readonly Color32 LightGray = new(200, 200, 200, 255);

        /// <summary>
        /// Default tag.
        /// </summary>
        public static readonly BaseCTag Debug = CDebugSettings.DefaultTag;

        /// <summary>
        /// Convenience tag that marks a message as successful.
        /// </summary>
        public static readonly BaseCTag Ok = CTag.Tag("OK").Color(DarkGreen);

        /// <summary>
        /// Convenience tag that flags a message as a warning.
        /// </summary>
        public static readonly BaseCTag Warning = CTag.Tag("WARNING").Color(Color.yellow)
            .TextType(TextType.Bold).LogType(LogType.Warning);

        /// <summary>
        /// Convenience tag that flags a message as an error.
        /// </summary>
        public static readonly BaseCTag Error = CTag.Tag("ERROR").Color(Color.red)
            .TextType(TextType.Bold).LogType(LogType.Error);

        /// <summary>
        /// Default spacer.
        /// </summary>
        public static readonly BaseCTag Space = CDebugSettings.DefaultSpace;

        /// <summary>
        /// Default text.
        /// </summary>
        public static readonly BaseCTag Text = CDebugSettings.DefaultText;

        /// <summary>
        /// Blank tag instance that can be configured fluently without affecting shared defaults.
        /// </summary>
        public static readonly CTag None = CTag.New();

        /// <summary>
        /// Logs a message built from the supplied <see cref="BaseCTag"/> elements, honoring <see cref="CDebugSettings"/> options and taking the most severe <see cref="LogType"/>.
        /// </summary>
        /// <param name="elements">Sequence of text fragments and formatting tags to compose the message.</param>
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
