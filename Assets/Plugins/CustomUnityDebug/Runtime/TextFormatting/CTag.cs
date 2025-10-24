using System;
using System.Globalization;
using UnityEngine;

namespace Masev.CustomUnityDebug.TextFormatting
{
    /// <summary>
    /// Debug Tag Class
    /// </summary>
    public class CTag : BaseCTag
    {
        private string formattedStr;
        private bool isDirty = true;

        public override string FormattedStr
        {
            get
            {
                if (isDirty)
                {
                    var sb = StringBuilderPool.Get();
                    sb.Append(RawTextType.OpenRichText())
                        .Append("<color=").Append('#')
                        .Append(ColorUtility.ToHtmlStringRGBA(RawColor))
                        .Append(">")
                        .Append(RawBrackets.OpenRichText())
                        .Append(RawStr)
                        .Append(RawBrackets.CloseRichText())
                        .Append("</color>")
                        .Append(RawTextType.CloseRichText())
                        .Append(RawSpacer.RichText());

                    formattedStr = sb.ToString();
                    StringBuilderPool.Return(sb);
                    isDirty = false;
                }

                return formattedStr;
            }
        }

        /// <summary>
        /// Responsible for displaying this debug tag
        /// </summary>
        public void SetEnabled(bool enabled)
        {
            Enabled = enabled;
        }

        public CTag Str(string str)
        {
            RawStr = str;
            isDirty = true;
            return this;
        }

        public CTag Color(Color32 color)
        {
            RawColor = color;
            isDirty = true;
            return this;
        }

        public CTag TextType(TextTypes type)
        {
            RawTextType = type;
            isDirty = true;
            return this;
        }

        public CTag Brackets(Brackets brackets)
        {
            RawBrackets = brackets;
            isDirty = true;
            return this;
        }

        public CTag Spacer(Spacers spacer)
        {
            RawSpacer = spacer;
            isDirty = true;
            return this;
        }

        public CTag LogType(LogType log)
        {
            RawLogType = log;
            return this;
        }


        public static CTag New() => new();
        
        public static CTag Text(string str) => CDebugSettings.DefaultText.AsNew().Str(str ?? "NULL");
        public static CTag PlainText(string str) => Text(str).TextType(TextTypes.Regular);
        public static CTag BoldText(string str) => Text(str).TextType(TextTypes.Bold);
        public static CTag ItalicText(string str) => Text(str).TextType(TextTypes.Italic);
        public static CTag BoldItalicText(string str) => Text(str).TextType(TextTypes.BoldItalic);

        public static CTag Tag(string str) => CDebugSettings.DefaultTag.AsNew().Str(str ?? "NULL");
        public static CTag SquareTag(string str) => Tag(str).Brackets(TextFormatting.Brackets.Square);
        public static CTag CurlTag(string str) => Tag(str).Brackets(TextFormatting.Brackets.Curly);
        public static CTag RoundTag(string str) => Tag(str).Brackets(TextFormatting.Brackets.Round);
        public static CTag EndTag(string str) => Tag(str).Spacer(CDebugSettings.DefaultSpace.RawSpacer);

        public static CTag Label(string str) => Text(str).Spacer(CDebugSettings.DefaultSpace.RawSpacer);

        public static CTag Timestamp(string format = "HH:mm:ss.fff", CultureInfo culture = null) =>
            Timestamp(DateTime.Now, format, culture);

        public static CTag Timestamp(DateTime time, string format = "HH:mm:ss.fff", CultureInfo culture = null)
        {
            var provider = culture ?? CultureInfo.InvariantCulture;
            var formatted = format == null ? time.ToString(provider) : time.ToString(format, provider);
            return Tag(formatted);
        }

        public static CTag Clone(BaseCTag tag) => new()
        {
            RawStr = tag.RawStr,
            RawColor = tag.RawColor,
            RawTextType = tag.RawTextType,
            RawBrackets = tag.RawBrackets,
            RawSpacer = tag.RawSpacer,
            RawLogType = tag.RawLogType,
            Enabled = tag.Enabled,
        };
    }
}
