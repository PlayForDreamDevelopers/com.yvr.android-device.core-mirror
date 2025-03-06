using System;

namespace YVR.AndroidDevice.Core
{
    /// <summary>
    /// Provides a base class for accessing Java classes.
    /// AJC short for Android Java Class
    /// </summary>
    public abstract class AJCBase
    {
        private AJCBase m_Core = null;

        public AJCBase core
        {
            get
            {
                if (m_Core != null) return m_Core;

                m_Core = this;
                while (m_Core is AJCWrapperBase wrapper) m_Core = wrapper.wrappedClass;

                return m_Core;
            }
        }

        public abstract IntPtr objPtr { get; }
        public abstract IntPtr classPtr { get; }

        [Obsolete("Use CallJNIStatic instead.")]
        public abstract T CallStatic<T>(string methodName, params object[] args);

        [Obsolete("Use CallJNI instead.")]
        public abstract T Call<T>(string methodName, params object[] args);

        [Obsolete("Use CallJNI instead.")]
        public abstract void Call(string methodName, params object[] args);

        [Obsolete("Use CallJNIStatic instead.")]
        public abstract void CallStatic(string methodName, params object[] args);

        public abstract void CallJNIStatic(string methodName, params object[] args);

        public abstract T CallJNIStatic<T>(string methodName, params object[] args);

        public abstract T CallJNI<T>(string methodName, params object[] args);

        public abstract void CallJNI(string methodName, params object[] args);

        public abstract void CallJNIStaticOverload(string methodName, string overloadMethodName, params object[] args);

        public abstract T CallJNIStaticOverload<T>(string methodName, string overloadMethodName, params object[] args);

        public abstract T CallJNIOverload<T>(string methodName, string overloadMethodName, params object[] args);
    }
}