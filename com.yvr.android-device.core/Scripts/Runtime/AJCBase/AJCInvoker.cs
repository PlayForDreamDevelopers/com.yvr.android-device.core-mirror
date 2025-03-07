using System;
using System.Collections.Concurrent;
using UnityEngine;
using YVR.Utilities;

#pragma warning disable CS0672 // Member overrides obsolete member

namespace YVR.AndroidDevice.Core
{
    /// <summary>
    /// AndroidJavaClassLogWrapper is a class that serves as a wrapper for AndroidJavaClass, providing logging capabilities.
    /// </summary>
    public class AJCInvoker : AJCBase
    {
        private readonly AndroidJavaObject m_JavaObject;
        private readonly IntPtr m_ClassPtr;
        private readonly IntPtr m_ObjPtr;

        public override IntPtr objPtr => m_ObjPtr;
        public override IntPtr classPtr => m_ClassPtr;

        public AndroidJavaObject javaObject => m_JavaObject;
        private ConcurrentDictionary<string, IntPtr> m_Method2PtrDic = new();

        /// <summary>
        /// Constructs an instance of AndroidJavaClassLogWrapper with the given class name.
        /// </summary>
        /// <param name="className">The name of the Java class to be wrapped.</param>
        /// <param name="args">Constructing parameters. Warning! For java objects, to keep multiple arguments of the same type, look at the implementation of this method.</param>
        public AJCInvoker(string className, params object[] args)
        {
            if (args is {Length: > 0})
            {
                var first = args[0];
                this.Info($"Create AJCInvoker with {first?.GetType().FullName}");
                switch (first)
                {
                    case AndroidJavaClass androidJavaClass:
                        m_JavaObject = new AndroidJavaObject(className, androidJavaClass);
                        break;
                    case AndroidJavaProxy androidJavaProxy:
                        m_JavaObject = new AndroidJavaObject(className, androidJavaProxy);
                        break;
                    case AndroidJavaObject androidJavaObject:
                        m_JavaObject = new AndroidJavaObject(className, androidJavaObject);
                        break;
                    case AndroidJavaRunnable runnable:
                        m_JavaObject = new AndroidJavaObject(className, runnable);
                        break;
                    default:
                        m_JavaObject = new AndroidJavaObject(className, args);
                        break;
                }
            }
            else
            {
                m_JavaObject = new AndroidJavaObject(className);
            }

            m_ClassPtr = m_JavaObject.GetRawClass();
            m_ObjPtr = m_JavaObject.GetRawObject();
        }


        public AJCInvoker(AndroidJavaObject javaObject)
        {
            m_JavaObject = javaObject;
            m_ClassPtr = m_JavaObject.GetRawClass();
            m_ObjPtr = m_JavaObject.GetRawObject();
        }

        public override T CallStatic<T>(string methodName, params object[] args)
        {
            return m_JavaObject.CallStatic<T>(methodName, args);
        }

        public override T Call<T>(string methodName, params object[] args)
        {
            return m_JavaObject.Call<T>(methodName, args);
        }

        public override void Call(string methodName, params object[] args) { m_JavaObject.Call(methodName, args); }

        public override void CallStatic(string methodName, params object[] args)
        {
            m_JavaObject.CallStatic(methodName, args);
        }

        public override void CallJNIStatic(string methodName, params object[] args)
        {
            GetMethodPtrAndArgs(methodName, args, true, out IntPtr methodPtr, out jvalue[] jniArgs);
            AndroidUtils.CallJNIMethod(m_ClassPtr, methodPtr, true, jniArgs, args);
        }

        public override T CallJNIStatic<T>(string methodName, params object[] args)
        {
            GetMethodPtrAndArgs<T>(methodName, args, true, out IntPtr methodPtr, out jvalue[] jniArgs);
            return AndroidUtils.CallJNIMethod<T>(m_ClassPtr, methodPtr, true, jniArgs, args);
        }

        public override T CallJNI<T>(string methodName, params object[] args)
        {
            GetMethodPtrAndArgs<T>(methodName, args, false, out IntPtr methodPtr, out jvalue[] jniArgs);
            return AndroidUtils.CallJNIMethod<T>(m_ObjPtr, methodPtr, false, jniArgs, args);
        }

        public override void CallJNI(string methodName, params object[] args)
        {
            GetMethodPtrAndArgs(methodName, args, false, out IntPtr methodPtr, out jvalue[] jniArgs);
            AndroidUtils.CallJNIMethod(m_ObjPtr, methodPtr, false, jniArgs, args);
        }

        public override void CallJNIStaticOverload(string methodName, string overloadMethodName, params object[] args)
        {
            GetMethodPtrAndArgs(methodName, args, true, out IntPtr methodPtr, out jvalue[] jniArgs, overloadMethodName);
            AndroidUtils.CallJNIMethod(m_ClassPtr, methodPtr, true, jniArgs, args);
        }

        public override T CallJNIStaticOverload<T>(string methodName, string overloadMethodName, params object[] args)
        {
            GetMethodPtrAndArgs<T>(methodName, args, true, out IntPtr methodPtr, out jvalue[] jniArgs,
                                   overloadMethodName);
            return AndroidUtils.CallJNIMethod<T>(m_ClassPtr, methodPtr, true, jniArgs, args);
        }

        public override T CallJNIOverload<T>(string methodName, string overloadMethodName, params object[] args)
        {
            GetMethodPtrAndArgs<T>(methodName, args, false, out IntPtr methodPtr, out jvalue[] jniArgs,
                                   overloadMethodName);
            return AndroidUtils.CallJNIMethod<T>(m_ObjPtr, methodPtr, false, jniArgs, args);
        }

        private void GetMethodPtrAndArgs(string methodName, object[] args, bool isStatic, out IntPtr methodPtr,
                                         out jvalue[] jniArgs, string overLoadMethodName = "")
        {
            string caredName = string.IsNullOrEmpty(overLoadMethodName) ? methodName : overLoadMethodName;
            if (!m_Method2PtrDic.ContainsKey(caredName))
                m_Method2PtrDic[caredName] = AndroidJNIHelper.GetMethodID(m_ClassPtr, methodName, args, isStatic);

            methodPtr = m_Method2PtrDic[caredName];

            jniArgs = args.Length == 0 ? Array.Empty<jvalue>() : AndroidJNIHelper.CreateJNIArgArray(args);
        }

        private void GetMethodPtrAndArgs<T>(string methodName, object[] args, bool isStatic, out IntPtr methodPtr,
                                            out jvalue[] jniArgs, string overLoadMethodName = "")
        {
            string caredName = string.IsNullOrEmpty(overLoadMethodName) ? methodName : overLoadMethodName;
            if (!m_Method2PtrDic.ContainsKey(caredName))
                m_Method2PtrDic[caredName] = AndroidJNIHelper.GetMethodID<T>(m_ClassPtr, methodName, args, isStatic);

            methodPtr = m_Method2PtrDic[caredName];

            jniArgs = args.Length == 0 ? Array.Empty<jvalue>() : AndroidJNIHelper.CreateJNIArgArray(args);
        }
    }
}