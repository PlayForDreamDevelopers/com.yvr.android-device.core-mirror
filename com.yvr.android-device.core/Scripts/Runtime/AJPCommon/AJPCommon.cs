// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using YVR.Utilities;

namespace YVR.AndroidDevice.Core
{
    /// <summary>
    /// Class for common Android Java Proxy.
    /// </summary>
    public class AJPCommon<T> : AndroidJavaProxy
    {
        private Action<T> m_OnCallback = null;
        private string m_ClassName = null;
        private bool m_IsOneTimeCallback = false;

        protected AJPCommon() : base("") { }

        public AJPCommon(Action<T> callback, string className, bool isOneTimeCallback = false) : base(className)
        {
            m_ClassName = className;
            m_IsOneTimeCallback = isOneTimeCallback;
            m_OnCallback = callback;
        }


        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public void onResult(T data)
        {
            this.Debug($"[AD] Android {m_ClassName}");
            m_OnCallback?.Invoke(data);
            if (m_IsOneTimeCallback)
                javaInterface.Dispose();
        }
    }
}