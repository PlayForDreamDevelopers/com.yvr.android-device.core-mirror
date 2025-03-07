using System;
using UnityEngine;

namespace YVR.AndroidDevice.Core
{
    /// <summary>
    /// Encapsulates the current Android activity.
    /// </summary>
    public static class AndroidUtils
    {
        private static AndroidJavaObject s_PlayerActivity;

        public static AndroidJavaObject unityPlayerActivity => Application.isEditor
            ? null
            : s_PlayerActivity ??= new AndroidJavaClass("com.unity3d.player.UnityPlayer")
               .GetStatic<AndroidJavaObject>("currentActivity");

        public static void CallJNIMethod(IntPtr typePtr, IntPtr methodPtr, bool isStatic, jvalue[] jniArgs,
                                         params object[] args)
        {
            if (isStatic) AndroidJNI.CallStaticVoidMethod(typePtr, methodPtr, jniArgs);
            else AndroidJNI.CallVoidMethod(typePtr, methodPtr, jniArgs);
            ClearJNIArgArray(args, jniArgs);
        }

        public static T CallJNIMethod<T>(IntPtr typePtr, IntPtr methodPtr, bool isStatic, jvalue[] jniArgs,
                                         params object[] args)
        {
            var ret = default(T);
            if (typeof(T) == typeof(bool))
                ret = (T) (object) (isStatic
                    ? AndroidJNI.CallStaticBooleanMethod(typePtr, methodPtr, jniArgs)
                    : AndroidJNI.CallBooleanMethod(typePtr, methodPtr, jniArgs));
            else if (typeof(T) == typeof(string))
                ret = (T) (object) (isStatic
                    ? AndroidJNI.CallStaticStringMethod(typePtr, methodPtr, jniArgs)
                    : AndroidJNI.CallStringMethod(typePtr, methodPtr, jniArgs));
            else if (typeof(T) == typeof(int))
                ret = (T) (object) (isStatic
                    ? AndroidJNI.CallStaticIntMethod(typePtr, methodPtr, jniArgs)
                    : AndroidJNI.CallIntMethod(typePtr, methodPtr, jniArgs));
            else if (typeof(T) == typeof(long))
                ret = (T) (object) (isStatic
                    ? AndroidJNI.CallStaticLongMethod(typePtr, methodPtr, jniArgs)
                    : AndroidJNI.CallLongMethod(typePtr, methodPtr, jniArgs));
            else if (typeof(T) == typeof(float))
                ret = (T) (object) (isStatic
                    ? AndroidJNI.CallStaticFloatMethod(typePtr, methodPtr, jniArgs)
                    : AndroidJNI.CallFloatMethod(typePtr, methodPtr, jniArgs));
            else if (typeof(T) == typeof(double))
                ret = (T) (object) (isStatic
                    ? AndroidJNI.CallStaticDoubleMethod(typePtr, methodPtr, jniArgs)
                    : AndroidJNI.CallDoubleMethod(typePtr, methodPtr, jniArgs));
            else if (typeof(T) == typeof(char))
                ret = (T) (object) (isStatic
                    ? AndroidJNI.CallStaticCharMethod(typePtr, methodPtr, jniArgs)
                    : AndroidJNI.CallCharMethod(typePtr, methodPtr, jniArgs));
            else if (typeof(T) == typeof(short))
                ret = (T) (object) (isStatic
                    ? AndroidJNI.CallStaticShortMethod(typePtr, methodPtr, jniArgs)
                    : AndroidJNI.CallShortMethod(typePtr, methodPtr, jniArgs));
            else if (typeof(T) == typeof(sbyte))
                ret = (T) (object) (isStatic
                    ? AndroidJNI.CallStaticSByteMethod(typePtr, methodPtr, jniArgs)
                    : AndroidJNI.CallSByteMethod(typePtr, methodPtr, jniArgs));
            else if (typeof(T) == typeof(byte))
                throw new Exception("Byte is not supported, should use sbyte instead.");
            else
            {
                IntPtr retPtr = isStatic
                    ? AndroidJNI.CallStaticObjectMethod(typePtr, methodPtr, jniArgs)
                    : AndroidJNI.CallObjectMethod(typePtr, methodPtr, jniArgs);
                if (retPtr != IntPtr.Zero)
                {
                    if (typeof(T) == typeof(AndroidJavaObject))
                        ret = (T) (object) new AndroidJavaObject(retPtr);
                    else // Bug: if the ret is not array, it will throw exception
                        ret = AndroidJNIHelper.ConvertFromJNIArray<T>(retPtr);

                    AndroidJNI.DeleteLocalRef(retPtr);
                }
            }

            ClearJNIArgArray(args, jniArgs);
            return ret;
        }

        private static void ClearJNIArgArray(object[] args, jvalue[] jniArgs)
        {
            if (jniArgs.Length > 0)
                AndroidJNIHelper.DeleteJNIArgArray(args, jniArgs);
        }
    }
}