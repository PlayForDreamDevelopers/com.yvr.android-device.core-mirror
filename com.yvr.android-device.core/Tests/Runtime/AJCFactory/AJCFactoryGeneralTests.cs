using NUnit.Framework;

namespace YVR.AndroidDevice.Core.Tests
{
    [TestFixture]
    public class AJCFactoryGeneralTests
    {
        [Test]
        public void RegisterMockerType_ValidMockerType_NullException()
        {
            string className = "com.unity3d.player.UnityPlayer";
            Assert.That(() => AJCFactory.instance.RegisterMockerType(className, typeof(UnityPlayerMocker)),
                        Throws.Nothing);
        }

        [Test]
        public void RegisterMockerType_InValidMockerType_ArgumentException()
        {
            // Using a wrong Type for mocker Type
            string className = "com.unity3d.player.UnityPlayer";
            Assert.That(() => AJCFactory.instance.RegisterMockerType(className, typeof(AJCFactoryAndroidTests)),
                        Throws.ArgumentException);
        }
    }
}