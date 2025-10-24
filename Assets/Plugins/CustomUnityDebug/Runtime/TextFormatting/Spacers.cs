using System;

namespace Masev.CustomUnityDebug.TextFormatting
{
    /// <summary>
    /// Spacer presets that can be appended after a tag.
    /// </summary>
    public enum Spacers
    {
        Space,
        Dot,
        Comma,
        Colon,
        Ellipsis,
        Dash,
        None,
    }

    /// <summary>
    /// Helpers that convert spacer presets into their string representation.
    /// </summary>
    public static class SpacerStyleExtensions
    {
        /// <summary>
        /// Returns the spacer string associated with the supplied preset.
        /// </summary>
        /// <param name="style">Spacer preset.</param>
        /// <returns>String appended after the tag.</returns>
        public static string RichText(this Spacers style) => style switch
        {
            Spacers.Space => " ",
            Spacers.Dot => ". ",
            Spacers.Comma => ", ",
            Spacers.Colon => ": ",
            Spacers.Ellipsis => "... ",
            Spacers.Dash => "- ",
            Spacers.None => "",
            _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
        };
    }
}
