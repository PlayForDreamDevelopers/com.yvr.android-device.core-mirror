using System;

namespace YVR.AndroidDevice.Core
{
    public class ActionMocker
    {
        private Action<object[]> m_Action = null;
        public ActionMocker(Action<object[]> action) { m_Action = action; }

        public void Invoke(params object[] input) { m_Action?.Invoke(input); }
    }
}