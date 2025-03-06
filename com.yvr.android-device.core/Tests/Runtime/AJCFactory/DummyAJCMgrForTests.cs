using YVR.Utilities;

namespace YVR.AndroidDevice.Core.Tests
{
    public class DummyAJCMgrForTests
    {
        public AJCMocker GetMocker() { throw new System.NotImplementedException(); }

        public IAJCElements GetElements() { throw new System.NotImplementedException(); }

        public void ListenWrapAndMockEvent()
        {
            AJCFactory.instance.onWrappedModeChanged += OnWrappedModeChanged;
            AJCFactory.instance.onMockingModeChanged += OnMockModeChanged;
        }
        
        public void UnListenWrapAndMockEvent()
        {
            AJCFactory.instance.onWrappedModeChanged -= OnWrappedModeChanged;
            AJCFactory.instance.onMockingModeChanged -= OnMockModeChanged;
        }

        private void OnMockModeChanged(bool isMocking) { this.Debug($"OnGlobalMockModeChanged {isMocking}"); }
        private void OnWrappedModeChanged(AJCWrapMode mode) { this.Debug($"OnWrappedModeChanged {mode}"); }
    }
}