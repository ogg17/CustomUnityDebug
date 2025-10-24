using System;
using System.Collections;
using Masev.CustomUnityDebug;
using Masev.CustomUnityDebug.TextFormatting;
using UnityEngine;
using LogType = Masev.CustomUnityDebug.TextFormatting.LogType;

namespace DefaultNamespace
{
    /// <summary>
    /// Turn this component on to see how Custom Unity Debug behaves in different scenarios.
    /// Each section logs a focused example you can copy into your own project.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Custom Unity Debug/Demo Runner")]
    public sealed class TestDebug : MonoBehaviour
    {
        [Header("Demo Options")]
        [Tooltip("Automatically runs the showcase from Start() so you can just hit Play.")]
        [SerializeField] private bool runOnStart = true;

        [Tooltip("Adds a coroutine sample that shows timestamps across frames.")]
        [SerializeField] private bool includeCoroutineSample = true;

        private static readonly CTag DemoDefaultTag = CTag.Tag("CDEBUG")
            .Color(CDebug.LightGray)
            .TextType(TextTypes.Bold)
            .Brackets(Brackets.Square);

        private static readonly CTag GameplayTag = CTag.Tag("GAMEPLAY").Color(CDebug.Orange);
        private static readonly CTag UiTag = CTag.Tag("UI").Color(CDebug.LightGreen);
        private static readonly CTag NetworkWarningTag = CTag.Tag("NETWORK").Color(CDebug.Pink).LogType(LogType.Warning);
        private static readonly CTag CriticalErrorTag = CTag.Tag("CRITICAL").Color(Color.red)
            .TextType(TextTypes.Bold)
            .LogType(LogType.Error);

        private static readonly CTag ContextLabel = CTag.Label("Context").Color(CDebug.LightGray);
        private static readonly CTag ColonSpacer = CTag.PlainText(string.Empty).Spacer(Spacers.Colon);

        private void Awake()
        {
#if UNITY_EDITOR
            ConfigureDefaults();
#else
            CDebugSettings.DebugEnabled = true;
#endif
        }

        private static void ConfigureDefaults()
        {
            CDebugSettings.DebugEnabled = true;
            CDebugSettings.DebugFirstTagAlwaysDefault = true;
            CDebugSettings.DisableAllMessageWithTag = false;
            CDebugSettings.DefaultTag = DemoDefaultTag.AsNew();
            CDebugSettings.DefaultText = CTag.New()
                .Color(CDebug.LightGray)
                .TextType(TextTypes.Regular)
                .Spacer(Spacers.Space)
                .Brackets(Brackets.None);
            CDebugSettings.DefaultSpace = CTag.New().Spacer(Spacers.Space);
        }

        private void Start()
        {
            if (!runOnStart) return;

            RunDemo();

            if (includeCoroutineSample)
            {
                StartCoroutine(AsyncDemo());
            }
        }

        /// <summary>
        /// Runs the same showcase that executes on Start but can also be triggered from the context menu.
        /// </summary>
        [ContextMenu("Run Demo")]
        public void RunDemo()
        {
            LogSection("Basic logging");
            CDebug.Log("Hello from Custom Unity Debug!");
            CDebug.Log(CDebug.Warning, "Warnings elevate the log type automatically.");
            CDebug.Log(CDebug.Error, "Errors call Debug.LogError under the hood.");

            LogSection("Tags and formatting");
            CDebug.Log(GameplayTag, "Player spawned".AsText());
            CDebug.Log(UiTag, ColonSpacer, "Button pressed".AsBoldText());
            CDebug.Log(NetworkWarningTag, "Packet dropped".AsItalicText());
            CDebug.Log(CriticalErrorTag, "Unhandled exception".AsBoldItalicText());

            LogSection("Combining tags");
            CDebug.Log("SYSTEM".AsTag(), ColonSpacer, "Boot sequence started");
            CDebug.Log("FLOW".AsTag(), CDebug.Space, "opening".AsText(), "closing".AsEndTag());

            LogSection("Cloning and customizing");
            var temporary = GameplayTag.AsNew().Color(CDebug.LightGreen).Str("TEMP");
            CDebug.Log(temporary, "Cloned tag keeps base settings but can be tweaked.");
            NetworkWarningTag.SetEnabled(false);
            CDebug.Log(NetworkWarningTag, "Disabled tags are skipped when DisableAllMessageWithTag is false.");
            NetworkWarningTag.SetEnabled(true);

            LogSection("Runtime switches");
            CDebugSettings.DebugFirstTagAlwaysDefault = false;
            CDebug.Log("Default tag prefix disabled for this line.");
            CDebugSettings.DebugFirstTagAlwaysDefault = true;
            CDebugSettings.DisableAllMessageWithTag = true;
            NetworkWarningTag.SetEnabled(false);
            CDebug.Log(CDebug.Warning, "Next entry is suppressed because the tag is disabled.");
            CDebug.Log(NetworkWarningTag, "This line will not appear because DisableAllMessageWithTag is true.");
            NetworkWarningTag.SetEnabled(true);
            CDebugSettings.DisableAllMessageWithTag = false;
            CDebug.Log(CDebug.Ok, "Restored default behaviour.");

            LogSection("Helpers and timestamps");
            CDebug.Log(ContextLabel, this.AsNameTag(), ColonSpacer, "Component context captured.");
            CDebug.Log("Timestamp".AsTag(), ColonSpacer, DateTime.Now.AsTimestamp());
            CDebug.Log("Custom Format".AsTag(), ColonSpacer, DateTime.UtcNow.AsTimestamp("HH:mm:ss 'UTC'"));

            LogSection("Brackets and spacers");
            var square = CTag.Tag("SQUARE").Brackets(Brackets.Square);
            var round = CTag.Tag("ROUND").Brackets(Brackets.Round);
            var curly = CTag.Tag("CURLY").Brackets(Brackets.Curly);
            CDebug.Log(square, round, curly, "Different bracket presets in one message.");

            var ellipsis = CTag.PlainText("Loading").Spacer(Spacers.Ellipsis);
            CDebug.Log(ellipsis, "done");
        }

        private static void LogSection(string title)
        {
            var header = CTag.BoldText(title).Color(Color.white);
            CDebug.Log(CDebug.Space, header);
        }

        private static IEnumerator AsyncDemo()
        {
            LogSection("Coroutine sequence");
            CDebug.Log("Step 1".AsTag(), ColonSpacer, DateTime.UtcNow.AsTimestamp("HH:mm:ss.fff"), "Starting work");
            yield return new WaitForSeconds(0.2f);
            CDebug.Log("Step 2".AsTag(), ColonSpacer, DateTime.UtcNow.AsTimestamp(), "Continuing");
            yield return null;
            CDebug.Log("Step 3".AsTag(), ColonSpacer, DateTime.UtcNow.AsTimestamp(), CDebug.Ok, "Finished");
        }
    }
}
