using System;
using System.Globalization;

namespace Masev.CustomUnityDebug.TextFormatting
{
    public static class CTagExtensions
    {
        public static CTag AsTag(this string str) => CTag.Tag(str);
        public static CTag AsTag(this Enum value) => CTag.Tag(value?.ToString());
        public static CTag AsTag(this Object obj) => CTag.Tag(obj?.GetType().Name);
        
        public static CTag AsEndTag(this string str) => CTag.EndTag(str);
        public static CTag AsEndTag(this Enum value) => CTag.EndTag(value?.ToString());
        public static CTag AsEndTag(this Object obj) => CTag.EndTag(obj?.GetType().Name);
        
        public static CTag AsNameTag(this UnityEngine.Object obj) => CTag.Clone(CDebugSettings.DefaultTag).Str(obj?.name);
        
        public static CTag AsText(this string str) => CTag.Text(str);
        public static CTag AsBoldText(this string str) => CTag.BoldText(str);
        public static CTag AsItalicText(this string str) => CTag.ItalicText(str);
        public static CTag AsBoldItalicText(this string str) => CTag.BoldItalicText(str);
        public static CTag AsLabel(this string str) => CTag.Label(str);

        public static CTag AsTimestamp(this DateTime time, string format = "HH:mm:ss.fff",
            CultureInfo culture = null) => CTag.Timestamp(time, format, culture);

        public static CTag AsNew(this BaseCTag tag)
        {
            if (tag == null) return CTag.New();
            return CTag.Clone(tag);
        }
    }
}