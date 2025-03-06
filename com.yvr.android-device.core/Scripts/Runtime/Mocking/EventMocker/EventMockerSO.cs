using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace YVR.AndroidDevice.Core
{
    public abstract class EventMockerSO<T> : ScriptableObject
    {
        protected Action<T> action = null;

        public virtual void Invoke() { }

        public static void RegisterEvent(string triggerSOPath, Action<T> action)
        {
#if UNITY_EDITOR
            List<EventMockerSO<T>> mockingTriggerSOs = AssetDatabase
                                                      .FindAssets("t:ScriptableObject", new[] {triggerSOPath}).ToList()
                                                      .Select(AssetDatabase.GUIDToAssetPath)
                                                      .Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>)
                                                      .Where(so => so != null)
                                                      .Where(so => so is EventMockerSO<T>)
                                                      .Cast<EventMockerSO<T>>()
                                                      .ToList();
            mockingTriggerSOs.ForEach(so => so.action = action);
#endif
        }
    }
}