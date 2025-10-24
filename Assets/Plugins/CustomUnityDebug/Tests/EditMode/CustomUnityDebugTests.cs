using System.Text;
using Masev.CustomUnityDebug.TextFormatting;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif

namespace Masev.CustomUnityDebug.Tests
{
    [TestFixture]
    public class CustomUnityDebugTests
    {
        private bool originalDebugEnabled;
        private bool originalDebugFirstTagAlwaysDefault;
        private bool originalDisableAllMessageWithTag;
        private BaseCTag originalDefaultTag;
        private BaseCTag originalDefaultText;
        private BaseCTag originalDefaultSpace;

        [SetUp]
        public void SetUp()
        {
            originalDebugEnabled = CDebugSettings.DebugEnabled;
            originalDebugFirstTagAlwaysDefault = CDebugSettings.DebugFirstTagAlwaysDefault;
            originalDisableAllMessageWithTag = CDebugSettings.DisableAllMessageWithTag;
            originalDefaultTag = CTag.Clone(CDebugSettings.DefaultTag);
            originalDefaultText = CTag.Clone(CDebugSettings.DefaultText);
            originalDefaultSpace = CTag.Clone(CDebugSettings.DefaultSpace);

            CDebugSettings.DebugEnabled = true;
            CDebugSettings.DebugFirstTagAlwaysDefault = true;
            CDebugSettings.DisableAllMessageWithTag = false;
            CDebugSettings.DefaultTag = CTag.Clone(originalDefaultTag);
            CDebugSettings.DefaultText = CTag.Clone(originalDefaultText);
            CDebugSettings.DefaultSpace = CTag.Clone(originalDefaultSpace);

#if UNITY_EDITOR
            Debug.ClearDeveloperConsole();
#endif
            LogAssert.ignoreFailingMessages = false;
        }

        [TearDown]
        public void TearDown()
        {
            CDebugSettings.DebugEnabled = originalDebugEnabled;
            CDebugSettings.DebugFirstTagAlwaysDefault = originalDebugFirstTagAlwaysDefault;
            CDebugSettings.DisableAllMessageWithTag = originalDisableAllMessageWithTag;
            CDebugSettings.DefaultTag = CTag.Clone(originalDefaultTag);
            CDebugSettings.DefaultText = CTag.Clone(originalDefaultText);
            CDebugSettings.DefaultSpace = CTag.Clone(originalDefaultSpace);
        }

        [Test]
        public void CTagFormattedStrCombinesRichTextElements()
        {
            var tag = CTag.New()
                .Str("DATA")
                .Color(new Color32(10, 20, 30, 255))
                .TextType(TextType.BoldItalic)
                .Brackets(Brackets.Round)
                .Spacer(Spacers.Colon);

            var formatted = tag.FormattedStr;

            Assert.AreEqual("<i><b><color=#0A141EFF>(DATA)</color></b></i>: ", formatted);
        }

        [Test]
        public void AsNewProducesIndependentCopy()
        {
            var originalFormatted = CDebugSettings.DefaultTag.FormattedStr;
            var clone = CDebugSettings.DefaultTag.AsNew()
                .Str("NEW")
                .Color(Color.blue);

            Assert.AreEqual(originalFormatted, CDebugSettings.DefaultTag.FormattedStr);
            Assert.AreNotEqual(originalFormatted, clone.FormattedStr);
        }

        [Test]
        public void StringBuilderPoolDoesNotReuseBuildersAboveMaxCapacity()
        {
            var largeBuilder = StringBuilderPool.Get(StringBuilderPool.MaxRetainedCapacity + 256);
            var originalCapacity = largeBuilder.Capacity;

            StringBuilderPool.Return(largeBuilder);

            var nextBuilder = StringBuilderPool.Get();
            try
            {
                Assert.LessOrEqual(nextBuilder.Capacity, StringBuilderPool.MaxRetainedCapacity);
                Assert.Greater(originalCapacity, StringBuilderPool.MaxRetainedCapacity);
            }
            finally
            {
                StringBuilderPool.Return(nextBuilder);
            }
        }

        [Test]
        public void LogSkipsDisabledTagsWhenDisableAllMessageWithTagIsFalse()
        {
            CDebugSettings.DisableAllMessageWithTag = false;
            var disabledTag = CTag.Tag("SKIP");
            disabledTag.SetEnabled(false);

            var expectedBuilder = new StringBuilder();
            expectedBuilder.Append(CDebugSettings.DefaultTag.FormattedStr);
            expectedBuilder.Append(CDebugSettings.DefaultText.AsNew().Str("Hello").FormattedStr);

            LogAssert.Expect(UnityEngine.LogType.Log, expectedBuilder.ToString());

            CDebug.Log(disabledTag, "Hello");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogAbortsWhenDisableAllMessageWithTagIsTrue()
        {
            CDebugSettings.DisableAllMessageWithTag = true;
            var disabledTag = CTag.Tag("SKIP");
            disabledTag.SetEnabled(false);

            CDebug.Log(disabledTag, "Hello");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogProducesNoOutputWhenDebuggingDisabled()
        {
            CDebugSettings.DebugEnabled = false;

            CDebug.Log("Silence please");

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogUsesHighestLogTypeAmongElements()
        {
            var expectedBuilder = new StringBuilder();
            expectedBuilder.Append(CDebugSettings.DefaultTag.FormattedStr);
            expectedBuilder.Append(CDebug.Warning.FormattedStr);
            expectedBuilder.Append(CDebug.Error.FormattedStr);
            expectedBuilder.Append(CDebugSettings.DefaultText.AsNew().Str("Oops").FormattedStr);

            LogAssert.Expect(UnityEngine.LogType.Error, expectedBuilder.ToString());

            CDebug.Log(CDebug.Warning, CDebug.Error, "Oops");

            LogAssert.NoUnexpectedReceived();
        }
    }
}
