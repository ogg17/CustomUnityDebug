using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Masev.CustomUnityDebug.TextFormatting
{
    /// <summary>
    /// Base abstraction for string fragments that can contribute formatting metadata to a CDebug message.
    /// </summary>
    public abstract class BaseCTag
    {
        public string RawStr { get; protected set; } = string.Empty;
        public Color32 RawColor { get; protected set; } = Color.white;
        public TextType RawTextType { get; protected set; } = TextType.Regular;
        public Brackets RawBrackets { get; protected set; } = Brackets.None;
        public Spacers RawSpacer { get; protected set; } = Spacers.None;
        public LogType RawLogType { get; protected set; } = LogType.Log;

        /// <summary>
        /// Indicates whether the tag should be included when building a log entry.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets the fully formatted string representation of this tag. Implementations usually cache the generated value for reuse.
        /// </summary>
        public virtual string FormattedStr => String.Empty;


        /// <summary>
        /// Implicitly converts a string into a text tag using <see cref="CDebugSettings.DefaultText"/>.
        /// </summary>
        /// <param name="s">Source string value.</param>
        /// <returns>A text tag wrapping the supplied string.</returns>
        public static implicit operator BaseCTag(string s) =>
            !string.IsNullOrEmpty(s) ? CDebugSettings.DefaultText.AsNew().Str(s) : CDebugSettings.DefaultText;

        /// <summary>
        /// Implicitly converts a Unity object into a tag containing the object's type name.
        /// </summary>
        /// <param name="obj">Unity object instance.</param>
        /// <returns>Tag describing the object's type or NULL if the object is missing.</returns>
        public static implicit operator BaseCTag(Object obj) =>
            CDebugSettings.DefaultTag.AsNew().Str(obj == null ? "NULL" : obj.GetType().Name);


        public bool Equals(BaseCTag other)
        {
            if (ReferenceEquals(null, other)) return false;

            return RawStr == other.RawStr
                   && RawColor.Equals(other.RawColor)
                   && RawTextType == other.RawTextType
                   && RawBrackets == other.RawBrackets
                   && RawSpacer == other.RawSpacer
                   && RawLogType == other.RawLogType;
        }
        
        public override bool Equals(object obj) =>
            ReferenceEquals(this, obj) || obj is BaseCTag tag && Equals(tag);
        
        public override int GetHashCode() => 
            HashCode.Combine(RawStr, RawColor, (int)RawTextType, (int)RawBrackets, (int)RawSpacer, (int)RawLogType);
    }
}
