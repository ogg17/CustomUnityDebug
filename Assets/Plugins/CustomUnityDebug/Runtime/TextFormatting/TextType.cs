using System;

namespace Masev.CustomUnityDebug.TextFormatting
{
    /// <summary>
    /// Text styling presets used when formatting a tag.
    /// </summary>
    public enum TextType
    {
        Regular,
        Bold,
        Italic,
        BoldItalic,
    }

    /// <summary>
    /// Helpers that translate <see cref="TextType"/> presets into Unity rich text tags.
    /// </summary>
    public static class TextStyleExtensions
    {
        /// <summary>
        /// Returns the opening rich text tag for the supplied style.
        /// </summary>
        /// <param name="type">Text styling preset.</param>
        /// <returns>Opening rich text markup.</returns>
        public static string OpenRichText(this TextType type) => type switch
        {
            TextType.Bold => "<b>",
            TextType.Italic => "<i>",
            TextType.BoldItalic => "<i><b>",
            TextType.Regular => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        /// <summary>
        /// Returns the closing rich text tag for the supplied style.
        /// </summary>
        /// <param name="type">Text styling preset.</param>
        /// <returns>Closing rich text markup.</returns>
        public static string CloseRichText(this TextType type) => type switch
        {
            TextType.Bold => "</b>",
            TextType.Italic => "</i>",
            TextType.BoldItalic => "</b></i>",
            TextType.Regular => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
