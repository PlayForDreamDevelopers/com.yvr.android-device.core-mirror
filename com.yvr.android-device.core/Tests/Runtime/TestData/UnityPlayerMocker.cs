using UnityEngine;

namespace YVR.AndroidDevice.Core.Tests
{
    public class UnityPlayerMocker : AJCMocker
    {
        [UnityEngine.Scripting.Preserve]
        public UnityPlayerMocker()
        {
            UnityPlayerElements elements = UnityPlayerMgr.elements;
            MockAction(elements.sendMessage, SendMessage);
        }

        private void SendMessage(object[] args)
        {
            string go = args[0] as string;
            string function = args[1] as string;
            string msg = args[2] as string;
            Debug.Log($"Received message is for function {function} on go {go}, msg is {msg}");
        }
    }
}