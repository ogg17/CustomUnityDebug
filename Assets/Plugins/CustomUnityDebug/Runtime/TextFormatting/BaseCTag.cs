using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Masev.CustomUnityDebug.TextFormatting
{
    public abstract class BaseCTag
    {
        public string RawStr { get; protected set; } = string.Empty;
        public Color32 RawColor { get; protected set; } = Color.white;
        public TextTypes RawTextType { get; protected set; } = TextTypes.Regular;
        public Brackets RawBrackets { get; protected set; } = Brackets.None;
        public Spacers RawSpacer { get; protected set; } = Spacers.None;
        public LogType RawLogType { get; protected set; } = LogType.Log;

        public bool Enabled { get; set; } = true;

        public virtual string FormattedStr => String.Empty;


        public static implicit operator BaseCTag(string s) =>
            !string.IsNullOrEmpty(s) ? CDebugSettings.DefaultText.AsNew().Str(s) : CDebugSettings.DefaultText;

        public static implicit operator BaseCTag(Object obj) =>
            CDebugSettings.DefaultTag.AsNew().Str(obj == null ? "NULL" : obj.GetType().Name);

        public bool Equals(BaseCTag other) =>
            RawStr == other.RawStr
            && RawColor.Equals(other.RawColor)
            && RawTextType == other.RawTextType
            && RawBrackets == other.RawBrackets
            && RawSpacer == other.RawSpacer;

        public override bool Equals(object obj) =>
            obj is BaseCTag tag && Equals(tag);

        public override int GetHashCode() => 
            HashCode.Combine(RawStr, RawColor, (int)RawTextType, (int)RawBrackets, (int)RawSpacer);
    }
}