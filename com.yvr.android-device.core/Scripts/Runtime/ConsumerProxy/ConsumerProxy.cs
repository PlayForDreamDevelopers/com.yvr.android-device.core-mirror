using System;
using UnityEngine;

namespace YVR.AndroidDevice.Core.Utilities
{
    public class ConsumerProxy<T> : AndroidJavaProxy
    {
        private Action<T> m_Callback;

        public ConsumerProxy(Action<T> callback) : base("java.util.function.Consumer") { this.m_Callback = callback; }

        // ReSharper disable once InconsistentNaming
        public void accept(T value) { m_Callback?.Invoke(value); }
    }
}