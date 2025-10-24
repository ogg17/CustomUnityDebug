using System;

namespace Masev.CustomUnityDebug.TextFormatting
{
    /// <summary>
    /// Preset bracket styles that can surround tag content.
    /// </summary>
    public enum Brackets
    {
        Square,
        Round,
        Curly,
        Space,
        None,
    }

    /// <summary>
    /// Helpers that convert bracket presets into their string representations.
    /// </summary>
    public static class BracketStyleExtensions
    {
        /// <summary>
        /// Returns the opening rich text fragment for the supplied bracket style.
        /// </summary>
        /// <param name="style">Bracket preset.</param>
        /// <returns>Opening fragment for the bracket.</returns>
        public static string OpenRichText(this Brackets style) => style switch
        {
            Brackets.Square => "[",
            Brackets.Round => "(",
            Brackets.Curly => "{",
            Brackets.Space => " ",
            Brackets.None => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
        };

        /// <summary>
        /// Returns the closing rich text fragment for the supplied bracket style.
        /// </summary>
        /// <param name="style">Bracket preset.</param>
        /// <returns>Closing fragment for the bracket.</returns>
        public static string CloseRichText(this Brackets style) => style switch
        {
            Brackets.Square => "]",
            Brackets.Round => ")",
            Brackets.Curly => "}",
            Brackets.Space => " ",
            Brackets.None => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
        };
    }
}
