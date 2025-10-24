using Masev.CustomUnityDebug.TextFormatting;

namespace Masev.CustomUnityDebug
{
    /// <summary>
    /// Global configuration surface for the Custom Unity Debug system.
    /// </summary>
    public static class CDebugSettings
    {
        /// <summary>
        /// Toggles all CDebug output without changing any individual tag configuration.
        /// </summary>
        public static bool DebugEnabled { get; set; } = true;

        /// <summary>
        /// Automatically prefixes messages with <see cref="DefaultTag"/>.
        /// </summary>
        public static bool DebugFirstTagAlwaysDefault { get; set; } = true;

        /// <summary>
        /// When true the entire message will not be shown if at least one tag is disabled.
        /// </summary>
        public static bool DisableAllMessageWithTag { get; set; } = false;

        /// <summary>
        /// Shared default tag that is used as the default prefix when <see cref="DebugFirstTagAlwaysDefault"/> is enabled.
        /// </summary>
        public static BaseCTag DefaultTag { get; set; } = CTag.New()
            .Str("CDEBUG").Color(CDebug.LightGray).Spacer(Spacers.None)
            .TextType(TextType.Regular).Brackets(Brackets.Square);

        /// <summary>
        /// The default text is applied whenever a simple string is converted to a tag.
        /// </summary>
        public static BaseCTag DefaultText { get; set; } = CTag.New()
            .Str("CDebug").Color(CDebug.LightGray).Spacer(Spacers.None)
            .TextType(TextType.Regular).Brackets(Brackets.None);

        /// <summary>
        /// Spacer preset used by helper methods when inserting separators between tags.
        /// </summary>
        public static BaseCTag DefaultSpace { get; set; } = CTag.New()
            .Color(CDebug.LightGray).Spacer(Spacers.Colon);
    }
}
