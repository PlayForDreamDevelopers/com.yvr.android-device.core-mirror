using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace YVR.AndroidDevice.Core.Tests
{
    [TestFixture, UnityPlatform(RuntimePlatform.WindowsEditor)]
    public class AJCFactoryEditorTests
    {
        [Test]
        public void IsMocking_Editor_True() { Assert.That(AJCFactory.instance.isMocking); }

        [Test, Order(1)]
        public void GetClass_Editor_AJCMocker()
        {
            string className = "com.yvr.ABC";
            AJCBase ret = AJCFactory.instance.GetClass(className);
            Assert.That(ret.core is AJCMocker);
        }

        [Test, Order(2)]
        public void GetClass_EditorRegisterMocker_TargetType()
        {
            string className = "com.yvr.ABC";
            AJCFactory.instance.RegisterMockerType(className, typeof(DummyAJCMockerForTests));
            AJCBase ret = AJCFactory.instance.GetClass(className);
            Assert.That(ret.core is DummyAJCMockerForTests);
        }

        [Test, Order(3)]
        public void GetClass_EditorUnRegisterMocker_AJCMocker()
        {
            string className = "com.yvr.ABC";
            AJCFactory.instance.UnRegisterMockerType(className);
            AJCBase ret = AJCFactory.instance.GetClass(className);
            Assert.That(ret.core is AJCMocker);
        }
    }
}