using System;
using System.Collections.Generic;
using YVR.Utilities;

#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS0672 // Member overrides obsolete member

namespace YVR.AndroidDevice.Core
{
    /// <summary>
    /// This class represents a default implementation of the `AndroidJavaClassBase` class.
    /// </summary>
    public class AJCMocker : AJCBase
    {
        private Dictionary<string, Action<object[]>> m_Name2ActionDic = new();
        private Dictionary<string, Func<object[], object>> m_Name2FuncDic = new();
        private Dictionary<string, object> m_Name2ValueDic = new();

        public void MockValue(string name, object value) => m_Name2ValueDic.SafeAdd(name, value, true);

        public void MockAction(string name, Action<object[]> action) => m_Name2ActionDic.SafeAdd(name, action, true);

        public void MockFunc(string name, Func<object[], object> func) => m_Name2FuncDic.SafeAdd(name, func, true);

        public override IntPtr objPtr { get; }
        public override IntPtr classPtr { get; }

        public override T CallStatic<T>(string methodName, params object[] args)
        {
            if (m_Name2ValueDic.TryGetValue(methodName, out object value))
                return (T) value;
            if (m_Name2FuncDic.ContainsKey(methodName))
                return (T) m_Name2FuncDic[methodName].Invoke(args);
            return default;
        }

        public override T Call<T>(string methodName, params object[] args) { return CallStatic<T>(methodName, args); }

        public override void Call(string methodName, params object[] args)
        {
            CallStatic(methodName, args);
        }

        public override void CallStatic(string methodName, params object[] args)
        {
            if (m_Name2ActionDic.ContainsKey(methodName))
            {
                m_Name2ActionDic[methodName].Invoke(args);
            }
        }

        public override void CallJNIStatic(string methodName, params object[] args) { CallStatic(methodName, args); }

        public override T CallJNIStatic<T>(string methodName, params object[] args)
        {
            return CallStatic<T>(methodName, args);
        }

        public override T CallJNI<T>(string methodName, params object[] args)
        {
            return Call<T>(methodName, args);
        }

        public override void CallJNI(string methodName, params object[] args)
        {
            Call(methodName, args);
        }

        public override void CallJNIStaticOverload(string methodName, string overloadMethodName, params object[] args)
        {
            CallStatic(methodName, args);
        }

        public override T CallJNIStaticOverload<T>(string methodName, string overloadMethodName, params object[] args)
        {
            return CallStatic<T>(methodName, args);
        }

        public override T CallJNIOverload<T>(string methodName, string overloadMethodName, params object[] args)
        {
            return Call<T>(methodName, args);
        }
    }
}