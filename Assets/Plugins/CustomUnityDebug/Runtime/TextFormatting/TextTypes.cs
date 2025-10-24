using System;

namespace Masev.CustomUnityDebug.TextFormatting
{
    public enum TextTypes
    {
        Regular,
        Bold,
        Italic,
        BoldItalic,
    }

    public static class TextStyleExtensions
    {
        public static string OpenRichText(this TextTypes types) => types switch
        {
            TextTypes.Bold => "<b>",
            TextTypes.Italic => "<i>",
            TextTypes.BoldItalic => "<i><b>",
            TextTypes.Regular => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(types), types, null)
        };

        public static string CloseRichText(this TextTypes types) => types switch
        {
            TextTypes.Bold => "</b>",
            TextTypes.Italic => "</i>",
            TextTypes.BoldItalic => "</b></i>",
            TextTypes.Regular => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(types), types, null)
        };
    }
}