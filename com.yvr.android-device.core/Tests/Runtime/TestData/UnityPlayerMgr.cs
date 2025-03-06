namespace YVR.AndroidDevice.Core.Tests
{
    public class UnityPlayerMgr : AJCMgrSingleton<UnityPlayerMgr, UnityPlayerMocker, UnityPlayerElements>
    {
        public void SendMessage(string go, string component, string msg)
        {
            ajcBase.CallJNIStatic(elements.sendMessage, go, component, msg);
        }
    }
}