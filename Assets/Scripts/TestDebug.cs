using System.Collections;
using Masev.CustomUnityDebug;
using Masev.CustomUnityDebug.TextFormatting;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestDebug : MonoBehaviour
    {
        public static CTag TEST = CTag.Tag("TEST").Color(Color.cyan);
        public static CTag TEST1 = CTag.Tag("TEST1").Color(CDebug.Pink);
        public static CTag TEST2 = CTag.Tag("TEST2").Color(CDebug.LightGray);
        public static CTag TEST3 = CTag.Tag("TEST3").Color(CDebug.Orange);
        public static CTag DEBUG = CTag.Clone(CDebugSettings.DefaultTag).Str(nameof(TestDebug).ToUpper());


        private void Awake()
        {
#if UNITY_EDITOR
            CDebugSettings.DebugFirstTagAlwaysDefault = true;
            CDebugSettings.DisableAllMessageWithTag = false;
            TEST.SetEnabled(true);
            CDebugSettings.DebugEnabled = true;
            CDebugSettings.DefaultTag = DEBUG;
#else
            CDebugSettings.DebugEnabled = false;
#endif
        }

        private IEnumerator Test()
        {
            CDebug.Log();
            CDebug.Log(TEST, "Test Async!");
            yield return null;
            CDebug.Log(TEST1, "Test Async!");
            CDebug.Log("KEK".AsTag());
            
            Debug.LogAssertion("KEK");
        }

        private void Start()
        {
            CDebug.Log("CORE".AsTag(), CDebug.Space, "One", "Two", "Sree");
            CDebug.Log("Test!", "LOL".AsTag(), CDebug.Warning);
            CDebug.Log(TEST1.AsNew().Spacer(Spacers.Colon), "Test1!");
            CDebug.Log(TEST, TEST1, "Test1!".AsTag());
            CDebug.Log(CDebug.Warning, TEST2, "Test2".AsTag());
            CDebug.Log(CDebug.Error, TEST3, "Test3".AsTag());
            CDebug.Log(this.AsNameTag(), "KEK2".AsTag());
            CDebug.Log("POST".AsTag(), CDebug.None, "Test different variants");

            StartCoroutine(Test());
            StartCoroutine(Test());
        }
    }
}