using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Masev.CustomUnityDebug
{
    /// <summary>
    /// Main Custom Debug Class
    /// </summary>
    public static class CDebug
    {
        // Short-cuts for CDebugTagType
        public static readonly Enum NONE = CDebugTagType.NONE;
        public static readonly Enum DEBUG = CDebugTagType.DEBUG;
        public static readonly Enum OK = CDebugTagType.OK;
        public static readonly Enum WARNING = CDebugTagType.WARNING;
        public static readonly Enum ERROR = CDebugTagType.ERROR;
        public static readonly Enum UNKNOWN = CDebugTagType.UNKNOWN;


        // Color Presets
        public static readonly Color32 Orange = new(255, 128, 0, 255);
        public static readonly Color32 DarkGreen = new(0, 100, 0, 255);
        public static readonly Color32 WhiteGreen = new(124, 248, 124, 255);
        public static readonly Color32 Pink = new(255, 105, 180, 255);
        public static readonly Color32 WhiteGray = new(200, 200, 200, 255);


        /// <summary>
        /// Enable/Disable all CDebug messages
        /// </summary>
        public static bool DebugEnabled { get; set; } = true;

        /// <summary>
        /// Enable/disable permanent display of the default tag
        /// </summary>
        public static bool AlwaysDefaultTag { get; set; } = true;


        /// <summary>
        /// Global Default Tag for CDebug
        /// </summary>
        public static Enum DefaultTag { get; set; } = CDebugTagType.DEBUG;


        // Dictionary with all Debug Tags
        private static readonly Dictionary<Enum, CDebugTag> TagsMap = new()
        {
            { CDebugTagType.NONE, new CDebugTag("NONE", Color.red, true, true) },
            { CDebugTagType.DEBUG, new CDebugTag("DEBUG", Color.gray, true, false) },
            { CDebugTagType.OK, new CDebugTag("OK", Color.gray, true, false) },
            { CDebugTagType.WARNING, new CDebugTag("WARNING", Color.yellow, true, false) },
            { CDebugTagType.ERROR, new CDebugTag("ERROR", Color.red, true, false) },
            { CDebugTagType.CDEBUG, new CDebugTag("CDEBUG", Orange, true, false) },
            { CDebugTagType.UNKNOWN, new CDebugTag("UNKNOWN", Color.yellow, true, true) },
        };


        // String Builder Pool
        private static readonly Queue<StringBuilder> DisabledStringBuildersQueue = new();

        private static StringBuilder GetStringBuilder()
        {
            StringBuilder builder;
            if (DisabledStringBuildersQueue.Count > 0)
            {
                builder = DisabledStringBuildersQueue.Dequeue();
                return builder;
            }

            builder = new StringBuilder();
            return builder;
        }

        private static void DisableStringBuilder(StringBuilder builder)
        {
            builder.Clear();
            DisabledStringBuildersQueue.Enqueue(builder);
        }


        /// <summary>
        /// Add Own Tag to Custom Debug
        /// </summary>
        /// <param name="tag">Enum</param>
        /// <param name="cDebugTag">New Debug Tag</param>
        /// <returns>Return result of adding</returns>
        public static bool TryAddTag(Enum tag, CDebugTag cDebugTag) => TagsMap.TryAdd(tag, cDebugTag);


        /// <summary>
        /// Log Message with Default Tag
        /// </summary>
        /// <param name="message">Message</param>
        public static void Log(params string[] message) => Log(DefaultTag, message);

        /// <summary>
        /// Log Message with own Tag
        /// </summary>
        /// <param name="tag1">Own Tag</param>
        /// <param name="message">Message</param>
        public static void Log(Enum tag1, params string[] message) =>
            Log(tag1, CDebugTagType.NONE, message);

        /// <summary>
        /// Log Message with own Tags
        /// </summary>
        /// <param name="tag1">Own Tag1</param>
        /// <param name="tag2">Own Tag2</param>
        /// <param name="message">Message</param>
        public static void Log(Enum tag1, Enum tag2, params string[] message) =>
            Log(tag1, tag2, CDebugTagType.NONE, message);

        /// <summary>
        /// Log Message with own Tags
        /// </summary>
        /// <param name="tag1">Own Tag1</param>
        /// <param name="tag2">Own Tag2</param>
        /// <param name="tag3">Own Tag3</param>
        /// <param name="message">Message</param>
        public static void Log(Enum tag1, Enum tag2, Enum tag3, params string[] message) =>
            Log(tag1, tag2, tag3, CDebugTagType.NONE, message);

        /// <summary>
        /// Log Message with own Tags
        /// </summary>
        /// <param name="tag1">Own Tag1</param>
        /// <param name="tag2">Own Tag2</param>
        /// <param name="tag3">Own Tag3</param>
        /// <param name="tag4">Own Tag4</param>
        /// <param name="message">Message</param>
        public static void Log(Enum tag1, Enum tag2, Enum tag3, Enum tag4, params string[] message)
        {
            if (!DebugEnabled) return;
            StringBuilder builder = GetStringBuilder();

            // Add tags
            if (AlwaysDefaultTag && !WriteTag(DefaultTag, builder)) return;
            if (!WriteTag(tag1, builder)) return;
            if (!WriteTag(tag2, builder)) return;
            if (!WriteTag(tag3, builder)) return;
            if (!WriteTag(tag4, builder)) return;

            builder.Append(": ");

            // Add message
            foreach (string m in message)
                builder.Append(m).Append("; ");

            // Choose Log type
            if (Equals(tag1, CDebugTagType.ERROR)
                || Equals(tag2, CDebugTagType.ERROR)
                || Equals(tag3, CDebugTagType.ERROR)
                || Equals(tag4, CDebugTagType.ERROR))
                Debug.LogError(builder.ToString());
            else if (Equals(tag1, CDebugTagType.WARNING)
                     || Equals(tag2, CDebugTagType.WARNING)
                     || Equals(tag3, CDebugTagType.WARNING)
                     || Equals(tag4, CDebugTagType.WARNING))
                Debug.LogWarning(builder.ToString());
            else
                Debug.Log(builder.ToString());

            // Cleaning up String Builder for the following logs
            DisableStringBuilder(builder);
        }


        /// <summary>
        /// Writing a tag to String Builder
        /// </summary>
        /// <param name="tag">Debug Tag</param>
        /// <param name="builder">String Builder</param>
        /// <returns>Return false then Tag is disabled</returns>
        private static bool WriteTag(Enum tag, StringBuilder builder)
        {
            if (TagsMap.TryGetValue(tag, out CDebugTag dt))
            {
                if (!dt.Enabled)
                {
                    DisableStringBuilder(builder);
                    return false;
                }

                if (!Equals(tag, CDebugTagType.NONE))
                    builder.Append(dt);
            }
            else if (TagsMap.TryGetValue(CDebugTagType.UNKNOWN, out CDebugTag udt))
                builder.Append(udt);

            return true;
        }

        /// <summary>
        /// Setting up enabled for a single Debug Tag
        /// </summary>
        /// <param name="tag">Debug Tag</param>
        /// <param name="enabled">Boolean</param>
        public static void SetTagEnabled(Enum tag, bool enabled)
        {
            if (TagsMap.TryGetValue(tag, out CDebugTag dTag))
                dTag.Enabled = enabled;
            else Log(CDebugTagType.CDEBUG, CDebugTagType.ERROR, "Unknown Debug Tag!");
        }
    }
}