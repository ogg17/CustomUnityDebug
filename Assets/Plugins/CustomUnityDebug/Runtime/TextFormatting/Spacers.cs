using System;

namespace Masev.CustomUnityDebug.TextFormatting
{
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

    public static class SpacerStyleExtensions
    {
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