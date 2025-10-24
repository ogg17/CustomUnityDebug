using System;
using System.Globalization;

namespace Masev.CustomUnityDebug.TextFormatting
{
    /// <summary>
    /// Convenience extension methods for building <see cref="CTag"/> instances.
    /// </summary>
    public static class CTagExtensions
    {
        /// <summary>
        /// Converts a string into a tag using the default configuration.
        /// </summary>
        public static CTag AsTag(this string str) => CTag.Tag(str);

        /// <summary>
        /// Converts an enum value into a tag using its name.
        /// </summary>
        public static CTag AsTag(this Enum value) => CTag.Tag(value?.ToString());

        /// <summary>
        /// Converts an object reference into a tag using the type name.
        /// </summary>
        public static CTag AsTag(this Object obj) => CTag.Tag(obj?.GetType().Name);
        
        /// <summary>
        /// Builds an end tag (tag plus spacer) from a string.
        /// </summary>
        public static CTag AsEndTag(this string str) => CTag.EndTag(str);

        /// <summary>
        /// Builds an end tag from an enum value.
        /// </summary>
        public static CTag AsEndTag(this Enum value) => CTag.EndTag(value?.ToString());

        /// <summary>
        /// Builds an end tag from an object reference using its type name.
        /// </summary>
        public static CTag AsEndTag(this Object obj) => CTag.EndTag(obj?.GetType().Name);

        /// <summary>
        /// Creates a tag using the Unity object's name instead of its type.
        /// </summary>
        public static CTag AsNameTag(this UnityEngine.Object obj) => CTag.Clone(CDebugSettings.DefaultTag).Str(obj?.name);
        
        /// <summary>
        /// Converts a string into a plain text tag.
        /// </summary>
        public static CTag AsText(this string str) => CTag.Text(str);

        /// <summary>
        /// Converts a string into a bold text tag.
        /// </summary>
        public static CTag AsBoldText(this string str) => CTag.BoldText(str);

        /// <summary>
        /// Converts a string into an italic text tag.
        /// </summary>
        public static CTag AsItalicText(this string str) => CTag.ItalicText(str);

        /// <summary>
        /// Converts a string into a tag that combines bold and italic formatting.
        /// </summary>
        public static CTag AsBoldItalicText(this string str) => CTag.BoldItalicText(str);

        /// <summary>
        /// Converts a string into a label tag that appends the default spacer.
        /// </summary>
        public static CTag AsLabel(this string str) => CTag.Label(str);

        /// <summary>
        /// Creates a timestamp tag for the supplied <see cref="DateTime"/>.
        /// </summary>
        /// <param name="time">Time value to convert.</param>
        /// <param name="format">Optional custom format string.</param>
        /// <param name="culture">Optional culture used for formatting.</param>
        /// <returns>Configured timestamp tag.</returns>
        public static CTag AsTimestamp(this DateTime time, string format = "HH:mm:ss.fff",
            CultureInfo culture = null) => CTag.Timestamp(time, format, culture);

        /// <summary>
        /// Clones a tag or returns a new empty tag when the source is null.
        /// </summary>
        /// <param name="tag">Source tag to clone.</param>
        /// <returns>Cloned or new tag.</returns>
        public static CTag AsNew(this BaseCTag tag)
        {
            if (tag == null) return CTag.New();
            return CTag.Clone(tag);
        }
    }
}
