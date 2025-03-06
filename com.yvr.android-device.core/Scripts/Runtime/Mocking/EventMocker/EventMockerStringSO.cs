using UnityEngine;

namespace YVR.AndroidDevice.Core
{
    [CreateAssetMenu(fileName = "AJPCommonString", menuName = "YVR/Android-Device/AJPCommon/String Trigger")]
    public class EventMockerStringSO : EventMockerSO<string>
    {
        [TextArea(1, 100)] public string data = null;

        public override void Invoke() { action.Invoke(data); }
    }
}