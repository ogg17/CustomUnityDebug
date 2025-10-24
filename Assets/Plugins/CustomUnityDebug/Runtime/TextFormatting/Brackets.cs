using System;

namespace Masev.CustomUnityDebug.TextFormatting
{
    public enum Brackets
    {
        Square,
        Round,
        Curly,
        Space,
        None,
    }

    public static class BracketStyleExtensions
    {
        public static string OpenRichText(this Brackets style) => style switch
        {
            Brackets.Square => "[",
            Brackets.Round => "(",
            Brackets.Curly => "{",
            Brackets.Space => " ",
            Brackets.None => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
        };

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