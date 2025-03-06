using UnityEngine;

namespace YVR.AndroidDevice.Core.Editor
{
    public abstract class EventMockerSOEditor<T> : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var eventMocker = (EventMockerSO<T>) target;
            if (GUILayout.Button("Trigger"))
                eventMocker.Invoke();
        }
    }
}