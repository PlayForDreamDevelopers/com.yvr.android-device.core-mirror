using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using YVR.Test.Framework;

namespace YVR.AndroidDevice.Core.Tests
{
    [TestFixture]
    public class AJCBaseTests : IPrebuildSetup, IPostBuildCleanup
    {
        private string m_TestFolder = "Packages/com.yvr.android-device.core/Tests/Runtime/TestData";
        private string testScene => $"{m_TestFolder}/TestCore.unity";

        public void Setup() { TestUtils.AddRequiredScene(testScene); }

        public void Cleanup() { TestUtils.RemoveRequiredScene(testScene); }

        [Test, UnityPlatform(RuntimePlatform.WindowsEditor)]
        public void PlayerMgrSendMessage_Editor_ReceiveLog()
        {
            string go = "Receiver", function = "OnMessageReceiver", msg = "Hello";
            string targetLog = $"Received message is for function {function} on go {go}, msg is {msg}";
            UnityPlayerMgr.instance.SendMessage(go, function, msg);
            LogAssert.Expect(LogType.Log, targetLog);
        }

        [Test, UnityPlatform(RuntimePlatform.Android), Order(100)]
        public void PlayerMgrSendMessage_Mock_ReceiveLog()
        {
            AJCFactory.instance.mockModeEnable = true;

            // Mocking behavior should be equal to which in editor
            PlayerMgrSendMessage_Editor_ReceiveLog();

            AJCFactory.instance.mockModeEnable = false;
        }

        [UnityTest, UnityPlatform(RuntimePlatform.Android), Order(101)]
        public IEnumerator PlayerMgrSendMessage_Android_ReceiveLog()
        {
            yield return TestUtils.TestScene(testScene, SendMessageAndExceptLog());

            IEnumerator SendMessageAndExceptLog()
            {
                string go = "Receiver", function = "OnMessageReceived", msg = "Hello";
                string targetLog = $"Received message is for function {function} on go {go}, msg is {msg}";
                UnityPlayerMgr.instance.SendMessage(go, function, msg);
                yield return null;
                LogAssert.Expect(LogType.Log, targetLog);
            }
        }
    }
}