using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using YVR.Test.Framework;
using YVR.Utilities;

namespace YVR.AndroidDevice.Core.Tests
{
    [TestFixture]
    public class AJCWrapperTests : IPrebuildSetup, IPostBuildCleanup
    {
        private string m_TestFolder = "Packages/com.yvr.android-device.core/Tests/Runtime/TestData";
        private string testScene => $"{m_TestFolder}/TestCore.unity";

        public void Setup() { TestUtils.AddRequiredScene(testScene); }

        public void Cleanup() { TestUtils.RemoveRequiredScene(testScene); }

        [Test, Order(100)]
        public void GetClass_Default_GetNoneWrapper()
        {
            UnityPlayerMgr.instance.TryGetPropertyValue("ajcBase", out AJCBase ajcBase);
            Assert.That(!(ajcBase is AJCWrapperBase));
        }

        #region LogWrapper

        [Test, Order(200)]
        public void GetClass_SetLogWrapper_GetLogWrapper()
        {
            AJCFactory.instance.wrapMode = AJCWrapMode.Log;
            UnityPlayerMgr.instance.TryGetPropertyValue("ajcBase", out AJCBase ajcBase);
            Assert.That(ajcBase is AJCLogWrapper);
            Assert.That(!(((AJCLogWrapper) ajcBase).wrappedClass is AJCWrapperBase));
        }

        [Test, Order(201)]
        public void GetClass_ReSetNone_GetNoneWrapper()
        {
            AJCFactory.instance.wrapMode = AJCWrapMode.None;
            UnityPlayerMgr.instance.TryGetPropertyValue("ajcBase", out AJCBase ajcBase);
            Assert.That(!(ajcBase is AJCWrapperBase));
        }

        [Test, UnityPlatform(RuntimePlatform.WindowsEditor), Order(202)]
        public void PlayerMgrSendMessage_EditorLogWrapper_ReceiveCallingLog()
        {
            AJCFactory.instance.wrapMode = AJCWrapMode.Log;

            var beforeCallLog = new Regex("Before Calling AJC Method: .*, with args: .*");
            var targetLogLog = new Regex("Received message is for function .* on go .*, msg is .*");
            var afterCallLog = new Regex("After Calling AJC Method: .*, with args: .*");

            string go = "Receiver", function = "OnMessageReceived", msg = "Hello";
            UnityPlayerMgr.instance.SendMessage(go, function, msg);

            LogAssert.Expect(LogType.Log, beforeCallLog);
            LogAssert.Expect(LogType.Log, targetLogLog);
            LogAssert.Expect(LogType.Log, afterCallLog);

            AJCFactory.instance.wrapMode = AJCWrapMode.None;
        }

        [UnityTest, UnityPlatform(RuntimePlatform.Android), Order(203)]
        public IEnumerator PlayerMgrSendMessage_AndroidLogWrapper_ReceiveCallingLog()
        {
            AJCFactory.instance.wrapMode = AJCWrapMode.Log;

            yield return TestUtils.TestScene(testScene, SendMessageAndExceptLog());

            IEnumerator SendMessageAndExceptLog()
            {
                var beforeCallLog = new Regex("Before Calling AJC Method: .*, with args: .*");
                var targetLogLog = new Regex("Received message is for function .* on go .*, msg is .*");
                var afterCallLog = new Regex("After Calling AJC Method: .*, with args: .*");

                string go = "Receiver", function = "OnMessageReceived", msg = "Hello";
                UnityPlayerMgr.instance.SendMessage(go, function, msg);
                yield return null;

                // In Android, the after call log will be printed before the target log,
                // as the actual api calling is in a different thread which controlled by java side.
                LogAssert.Expect(LogType.Log, beforeCallLog);
                LogAssert.Expect(LogType.Log, afterCallLog);
                LogAssert.Expect(LogType.Log, targetLogLog);
            }

            AJCFactory.instance.wrapMode = AJCWrapMode.None;
        }

        [Test, UnityPlatform(RuntimePlatform.Android), Order(204)]
        public void PlayerMgrSendMessage_AndroidMockLogWrapper_ReceiveCallingLog()
        {
            AJCFactory.instance.mockModeEnable = true;
            PlayerMgrSendMessage_EditorLogWrapper_ReceiveCallingLog();
            AJCFactory.instance.mockModeEnable = false;
        }

        #endregion

        #region Timer

        private Regex m_TimerLog = new("AJC Method: .* cost .* ms");

        private void CheckCostTime(string log, string stackTrace, LogType type)
        {
            if (m_TimerLog.IsMatch(log))
            {
                float costTime = float.Parse(Regex.Match(log, @"\d+\.?\d*").Value);
                Assert.That(costTime > 0.0f);
                Application.logMessageReceived -= CheckCostTime;
            }
        }

        [Test, Order(300)]
        public void GetClass_SetTimerWrapper_GetTimerWrapper()
        {
            AJCFactory.instance.wrapMode = AJCWrapMode.Timer;
            UnityPlayerMgr.instance.TryGetPropertyValue("ajcBase", out AJCBase ajcBase);
            Assert.That(ajcBase is AJCTimerWrapper);
        }

        [Test, UnityPlatform(RuntimePlatform.WindowsEditor), Order(320)]
        public void PlayerMgrSendMessage_EditorTimerWrapper_ReceiveTimerLog()
        {
            AJCFactory.instance.wrapMode = AJCWrapMode.Timer;

            var targetLogLog = new Regex("Received message is for function .* on go .*, msg is .*");

            string go = "Receiver", function = "OnMessageReceived", msg = "Hello";
            UnityPlayerMgr.instance.SendMessage(go, function, msg);

            Application.logMessageReceived += CheckCostTime;

            LogAssert.Expect(LogType.Log, targetLogLog);
            LogAssert.Expect(LogType.Log, m_TimerLog);

            AJCFactory.instance.wrapMode = AJCWrapMode.None;
        }

        [UnityTest, UnityPlatform(RuntimePlatform.Android), Order(330)]
        public IEnumerator PlayerMgrSendMessage_AndroidTimerWrapper_ReceiveTimerLog()
        {
            AJCFactory.instance.wrapMode = AJCWrapMode.Timer;

            yield return TestUtils.TestScene(testScene, SendMessageAndExceptLog());

            IEnumerator SendMessageAndExceptLog()
            {
                var targetLogLog = new Regex("Received message is for function .* on go .*, msg is .*");

                string go = "Receiver", function = "OnMessageReceived", msg = "Hello";
                Application.logMessageReceived += CheckCostTime;
                UnityPlayerMgr.instance.SendMessage(go, function, msg);

                yield return null;

                LogAssert.Expect(LogType.Log, m_TimerLog);
                LogAssert.Expect(LogType.Log, targetLogLog);
            }

            AJCFactory.instance.wrapMode = AJCWrapMode.None;
        }

        [Test, UnityPlatform(RuntimePlatform.Android), Order(340)]
        public void PlayerMgrSendMessage_AndroidMockTimerWrapper_ReceiveTimerLog()
        {
            AJCFactory.instance.mockModeEnable = true;
            PlayerMgrSendMessage_EditorTimerWrapper_ReceiveTimerLog();
            AJCFactory.instance.mockModeEnable = false;
        }

        #endregion

        #region LogAndTimer

        [Test, Order(400)]
        public void GetClass_SetLogAndTimerWrapper_TimerWrapLogWrapCore()
        {
            AJCFactory.instance.wrapMode = AJCWrapMode.Log | AJCWrapMode.Timer;
            UnityPlayerMgr.instance.TryGetPropertyValue("ajcBase", out AJCBase ajcBase);

            var ajcTimer = ajcBase as AJCTimerWrapper;
            var ajcLogger = ajcTimer.wrappedClass as AJCLogWrapper;
            AJCBase core = ajcLogger.wrappedClass;

            Assert.That(ajcTimer != null);
            Assert.That(ajcLogger != null);
            Assert.That(!(core is AJCWrapperBase) && core != null);
        }

        [Test, UnityPlatform(RuntimePlatform.WindowsEditor), Order(420)]
        public void PlayerMgrSendMessage_EditorTimerLogWrapper_ReceiveCallATimerLog()
        {
            AJCFactory.instance.wrapMode = AJCWrapMode.Timer | AJCWrapMode.Log;

            var targetLogLog = new Regex("Received message is for function .* on go .*, msg is .*");
            var beforeCallLog = new Regex("Before Calling AJC Method: .*, with args: .*");
            var afterCallLog = new Regex("After Calling AJC Method: .*, with args: .*");

            string go = "Receiver", function = "OnMessageReceived", msg = "Hello";
            UnityPlayerMgr.instance.SendMessage(go, function, msg);

            Application.logMessageReceived += CheckCostTime;

            LogAssert.Expect(LogType.Log, beforeCallLog);
            LogAssert.Expect(LogType.Log, targetLogLog);
            LogAssert.Expect(LogType.Log, afterCallLog);
            LogAssert.Expect(LogType.Log, m_TimerLog);

            AJCFactory.instance.wrapMode = AJCWrapMode.None;
        }

        [UnityTest, UnityPlatform(RuntimePlatform.Android), Order(430)]
        public IEnumerator PlayerMgrSendMessage_AndroidTimerLogWrapper_ReceiveCallATimerLog()
        {
            AJCFactory.instance.wrapMode = AJCWrapMode.Timer | AJCWrapMode.Log;

            yield return TestUtils.TestScene(testScene, SendMessageAndExceptLog());

            IEnumerator SendMessageAndExceptLog()
            {
                var targetLogLog = new Regex("Received message is for function .* on go .*, msg is .*");
                var beforeCallLog = new Regex("Before Calling AJC Method: .*, with args: .*");
                var afterCallLog = new Regex("After Calling AJC Method: .*, with args: .*");

                string go = "Receiver", function = "OnMessageReceived", msg = "Hello";
                Application.logMessageReceived += CheckCostTime;
                UnityPlayerMgr.instance.SendMessage(go, function, msg);

                yield return null;

                LogAssert.Expect(LogType.Log, beforeCallLog);
                LogAssert.Expect(LogType.Log, afterCallLog);
                LogAssert.Expect(LogType.Log, m_TimerLog);
                LogAssert.Expect(LogType.Log, targetLogLog);
            }

            AJCFactory.instance.wrapMode = AJCWrapMode.None;
        }

        [Test, UnityPlatform(RuntimePlatform.Android), Order(340)]
        public void PlayerMgrSendMessage_AndroidMockTimerLogWrapper_ReceiveCallATimerLog()
        {
            AJCFactory.instance.mockModeEnable = true;
            PlayerMgrSendMessage_EditorTimerLogWrapper_ReceiveCallATimerLog();
            AJCFactory.instance.mockModeEnable = false;
        }

        #endregion
    }
}