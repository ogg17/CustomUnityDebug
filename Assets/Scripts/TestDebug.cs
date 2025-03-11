using Masev.CustomUnityDebug;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestDebug : MonoBehaviour
    {
        private void Awake()
        {
            CDebug.TryAddTag(CustomTag.TEST, new CDebugTag("TEST", Color.blue, false, false));
            CDebug.TryAddTag(CustomTag.TEST1, new CDebugTag("TEST1", CDebug.Pink, false, true));
            CDebug.TryAddTag(CustomTag.TEST2, new CDebugTag("TEST2", CDebug.WhiteGray, true, false));
            CDebug.TryAddTag(CustomTag.TEST3, new CDebugTag("TEST3", CDebug.Orange, true, true));
            
            CDebug.DefaultTag = CustomTag.TEST;
            CDebug.AlwaysDefaultTag = false;
            CDebug.SetTagEnabled(CustomTag.TEST, true);
            CDebug.DebugEnabled = true;
        }

        private void Start()
        {
            CDebug.Log("Test!");
            CDebug.Log(CDebug.DEBUG, CustomTag.TEST1, "Test1!");
            CDebug.Log(CDebug.WARNING, CustomTag.TEST2,"Test2");
            CDebug.Log(CDebug.ERROR, CustomTag.TEST3,"Test3");
        }
    }

    public enum CustomTag
    {
        TEST,
        TEST1,
        TEST2,
        TEST3,
        TEST4,
    }
}