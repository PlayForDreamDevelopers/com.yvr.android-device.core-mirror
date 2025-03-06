using System;
using System.Collections.Generic;
using UnityEngine;
using YVR.Utilities;

namespace YVR.AndroidDevice.Core
{
    /// <summary>
    /// AndroidJavaClassPools is a singleton class that manages a dictionary of AndroidJavaClassBase objects.
    /// </summary>
    public class AJCFactory : Singleton<AJCFactory>
    {
        private Dictionary<string, Type> m_Name2MockerTypeMap = null;

        private bool m_MockModeEnable = false;

        public Action<bool> onMockingModeChanged = null;
        public Action<AJCWrapMode> onWrappedModeChanged = null;

        public bool mockModeEnable
        {
            get => m_MockModeEnable;
            set
            {
                if (m_MockModeEnable == value) return;
                m_MockModeEnable = value;
                onMockingModeChanged?.Invoke(value);
            }
        }

        private AJCWrapMode m_WrapMode = AJCWrapMode.None;

        public AJCWrapMode wrapMode
        {
            get => m_WrapMode;
            set
            {
                if (m_WrapMode == value) return;
                m_WrapMode = value;
                onWrappedModeChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Whether the application is in mocking mode or not.
        /// </summary>
        public bool isMocking => Application.isEditor || mockModeEnable;

        protected override void OnInit() { m_Name2MockerTypeMap = new Dictionary<string, Type>(); }

        public void RegisterMockerType(string name, Type type)
        {
            if (typeof(AJCMocker).IsAssignableFrom(type))
                m_Name2MockerTypeMap.SafeAdd(name, type, true);
            else
                throw new ArgumentException("The specified type is not derived from AJCMocker.");
        }

        public void UnRegisterMockerType(string name) { m_Name2MockerTypeMap.Remove(name); }

        public AJCBase GetClass(string className, params object[] args)
        {
            AJCBase ajcInstance = isMocking ? CreateMocker() : new AJCInvoker(className, args);
            WrapAJCInstanceByMode();

            return ajcInstance;

            AJCBase CreateMocker()
            {
                bool typeExist = m_Name2MockerTypeMap.TryGetValue(className, out Type type);
                return typeExist ? (AJCMocker)Activator.CreateInstance(type) : new AJCMocker();
            }

            void WrapAJCInstanceByMode()
            {
                if (wrapMode.HasFlag(AJCWrapMode.Log))
                    ajcInstance = new AJCLogWrapper(ajcInstance);
                if (wrapMode.HasFlag(AJCWrapMode.Timer))
                    ajcInstance = new AJCTimerWrapper(ajcInstance);
            }
        }
    }
}