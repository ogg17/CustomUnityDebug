using Masev.CustomUnityDebug.TextFormatting;

namespace Masev.CustomUnityDebug
{
    public static class CDebugSettings
    {
        /// <summary>
        /// Enable/Disable all CDebug messages
        /// </summary>
        public static bool DebugEnabled { get; set; } = true;


        /// <summary>
        /// Enable/Disable first tag is always Default
        /// </summary>
        public static bool DebugFirstTagAlwaysDefault { get; set; } = true;

        public static bool DisableAllMessageWithTag { get; set; } = false;


        /// <summary>
        /// Global Default Tag for CDebug
        /// </summary>
        public static BaseCTag DefaultTag { get; set; } = CTag.New()
            .Str("CDEBUG").Color(CDebug.LightGray).Spacer(Spacers.None)
            .TextType(TextTypes.Regular).Brackets(Brackets.Square);

        public static BaseCTag DefaultText { get; set; } = CTag.New()
            .Str("CDebug").Color(CDebug.LightGray).Spacer(Spacers.None)
            .TextType(TextTypes.Regular).Brackets(Brackets.None);

        public static BaseCTag DefaultSpace { get; set; } = CTag.New()
            .Color(CDebug.LightGray).Spacer(Spacers.Colon);
    }
}