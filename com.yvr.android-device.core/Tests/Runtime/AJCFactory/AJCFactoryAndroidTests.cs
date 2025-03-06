using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace YVR.AndroidDevice.Core.Tests
{
    [TestFixture, UnityPlatform(RuntimePlatform.Android)]
    public class AJCFactoryAndroidTests
    {
        [Test, Order(0)]
        public void IsMocking_AndroidDefault_False() { Assert.That(!AJCFactory.instance.isMocking); }

        [Test]
        public void IsMocking_TurnOnMocking_True()
        {
            AJCFactory.instance.mockModeEnable = true;
            Assert.That(AJCFactory.instance.isMocking);

            // Restore to default value
            AJCFactory.instance.mockModeEnable = false;
        }

        [Test]
        public void IsMocking_TurnOnMocking_RegisteredAJCMgrReceiveCallback()
        {
            var mgr = new DummyAJCMgrForTests();
            mgr.ListenWrapAndMockEvent();

            AJCFactory.instance.mockModeEnable = true;
            Assert.That(AJCFactory.instance.isMocking, "Not succeed to turn on mocking");

            LogAssert.Expect(LogType.Log, "OnGlobalMockModeChanged True");

            AJCFactory.instance.mockModeEnable = false;

            LogAssert.Expect(LogType.Log, "OnGlobalMockModeChanged False");

            mgr.UnListenWrapAndMockEvent();
        }
    }
}