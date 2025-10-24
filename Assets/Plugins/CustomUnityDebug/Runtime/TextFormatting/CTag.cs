using System;
using System.Globalization;
using UnityEngine;

namespace Masev.CustomUnityDebug.TextFormatting
{
    /// <summary>
    /// Concrete tag that renders its content using Unity rich text markup and cached formatting.
    /// </summary>
    public class CTag : BaseCTag
    {
        private string formattedStr;
        private bool isDirty = true;

        /// <inheritdoc />
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
        /// Toggles whether this tag contributes to the output when building a message.
        /// </summary>
        public void SetEnabled(bool enabled)
        {
            Enabled = enabled;
        }

        /// <summary>
        /// Defines the raw string that will be wrapped by the configured formatting.
        /// </summary>
        public CTag Str(string str)
        {
            RawStr = str;
            isDirty = true;
            return this;
        }

        /// <summary>
        /// Applies the color used by the rich text `<color>` tag.
        /// </summary>
        public CTag Color(Color32 color)
        {
            RawColor = color;
            isDirty = true;
            return this;
        }

        /// <summary>
        /// Sets the text styling wrapper, for example bold or italic.
        /// </summary>
        public CTag TextType(TextType type)
        {
            RawTextType = type;
            isDirty = true;
            return this;
        }

        /// <summary>
        /// Chooses the bracket characters that surround the raw string.
        /// </summary>
        public CTag Brackets(Brackets brackets)
        {
            RawBrackets = brackets;
            isDirty = true;
            return this;
        }

        /// <summary>
        /// Selects the spacer that is appended after the formatted string.
        /// </summary>
        public CTag Spacer(Spacers spacer)
        {
            RawSpacer = spacer;
            isDirty = true;
            return this;
        }

        /// <summary>
        /// Sets the severity that this tag contributes when determining which Unity log API to call.
        /// </summary>
        public CTag LogType(LogType log)
        {
            RawLogType = log;
            return this;
        }


        /// <summary>
        /// Creates an unconfigured tag that can be set up fluently.
        /// </summary>
        public static CTag New() => new();
        
        /// <summary>
        /// Creates a text tag based on the default text configuration.
        /// </summary>
        /// <param name="str">Text value to embed.</param>
        /// <returns>Configured text tag.</returns>
        public static CTag Text(string str) => CDebugSettings.DefaultText.AsNew().Str(str ?? "NULL");

        /// <summary>
        /// Creates a regular text tag.
        /// </summary>
        public static CTag PlainText(string str) => Text(str).TextType(TextFormatting.TextType.Regular);

        /// <summary>
        /// Creates a bold text tag.
        /// </summary>
        public static CTag BoldText(string str) => Text(str).TextType(TextFormatting.TextType.Bold);

        /// <summary>
        /// Creates an italic text tag.
        /// </summary>
        public static CTag ItalicText(string str) => Text(str).TextType(TextFormatting.TextType.Italic);

        /// <summary>
        /// Creates a tag that combines bold and italic formatting.
        /// </summary>
        public static CTag BoldItalicText(string str) => Text(str).TextType(TextFormatting.TextType.BoldItalic);

        /// <summary>
        /// Creates a tag using the default tag configuration.
        /// </summary>
        /// <param name="str">Text value to embed.</param>
        /// <returns>Configured tag.</returns>
        public static CTag Tag(string str) => CDebugSettings.DefaultTag.AsNew().Str(str ?? "NULL");

        /// <summary>
        /// Wraps the supplied text in square brackets.
        /// </summary>
        public static CTag SquareTag(string str) => Tag(str).Brackets(TextFormatting.Brackets.Square);

        /// <summary>
        /// Wraps the supplied text in curly braces.
        /// </summary>
        public static CTag CurlTag(string str) => Tag(str).Brackets(TextFormatting.Brackets.Curly);

        /// <summary>
        /// Wraps the supplied text in round brackets.
        /// </summary>
        public static CTag RoundTag(string str) => Tag(str).Brackets(TextFormatting.Brackets.Round);

        /// <summary>
        /// Applies the globally configured spacer to the end of the tag.
        /// </summary>
        public static CTag EndTag(string str) => Tag(str).Spacer(CDebugSettings.DefaultSpace.RawSpacer);

        /// <summary>
        /// Creates a label tag that automatically appends the default spacer.
        /// </summary>
        public static CTag Label(string str) => Text(str).Spacer(CDebugSettings.DefaultSpace.RawSpacer);

        /// <summary>
        /// Creates a timestamp tag for the current system time.
        /// </summary>
        /// <param name="format">Custom time format string.</param>
        /// <param name="culture">Culture used when formatting the timestamp.</param>
        /// <returns>Configured timestamp tag.</returns>
        public static CTag Timestamp(string format = "HH:mm:ss.fff", CultureInfo culture = null) =>
            Timestamp(DateTime.Now, format, culture);

        /// <summary>
        /// Creates a timestamp tag for a supplied <see cref="DateTime"/>.
        /// </summary>
        /// <param name="time">Time value to display.</param>
        /// <param name="format">Custom time format string.</param>
        /// <param name="culture">Culture used when formatting the timestamp.</param>
        /// <returns>Configured timestamp tag.</returns>
        public static CTag Timestamp(DateTime time, string format = "HH:mm:ss.fff", CultureInfo culture = null)
        {
            var provider = culture ?? CultureInfo.InvariantCulture;
            var formatted = format == null ? time.ToString(provider) : time.ToString(format, provider);
            return Tag(formatted);
        }

        /// <summary>
        /// Creates a deep copy of an existing tag.
        /// </summary>
        /// <param name="tag">Source tag to clone.</param>
        /// <returns>Cloned tag instance.</returns>
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
