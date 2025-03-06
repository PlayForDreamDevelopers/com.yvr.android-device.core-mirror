using System;

#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS0672 // Member overrides obsolete member

namespace YVR.AndroidDevice.Core
{
    public abstract class AJCWrapperBase : AJCBase
    {
        private AJCBase m_WrappedClass = null;

        public AJCBase wrappedClass => m_WrappedClass;
        public override IntPtr objPtr => m_WrappedClass.objPtr;
        public override IntPtr classPtr => m_WrappedClass.classPtr;

        protected AJCWrapperBase(AJCBase wrappedClass)
        {
            m_WrappedClass = wrappedClass;
        }

        protected abstract void BeforeCallAJCMethod(string methodName, params object[] args);

        protected abstract void AfterCallAJCMethod(string methodName, params object[] args);

        private T CallWithHooks<T>(Func<T> func, string methodName, params object[] args)
        {
            BeforeCallAJCMethod(methodName, args);
            T result = func();
            AfterCallAJCMethod(methodName, args);
            return result;
        }

        private void CallWithHooks(Action func, string methodName, params object[] args)
        {
            BeforeCallAJCMethod(methodName, args);
            func();
            AfterCallAJCMethod(methodName, args);
        }


        public override T CallStatic<T>(string methodName, params object[] args) =>
            CallWithHooks(() => m_WrappedClass.CallStatic<T>(methodName, args), methodName, args);

        public override T Call<T>(string methodName, params object[] args) =>
            CallWithHooks(() => m_WrappedClass.Call<T>(methodName, args), methodName, args);

        public override void Call(string methodName, params object[] args) => 
            CallWithHooks(() => m_WrappedClass.Call(methodName, args), methodName, args);

        public override void CallStatic(string methodName, params object[] args) =>
            CallWithHooks(() => m_WrappedClass.CallStatic(methodName, args), methodName, args);

        public override void CallJNIStatic(string methodName, params object[] args) =>
            CallWithHooks(() => m_WrappedClass.CallJNIStatic(methodName, args), methodName, args);

        public override T CallJNIStatic<T>(string methodName, params object[] args) =>
            CallWithHooks(() => m_WrappedClass.CallJNIStatic<T>(methodName, args), methodName, args);

        public override T CallJNI<T>(string methodName, params object[] args) =>
            CallWithHooks(() => m_WrappedClass.CallJNI<T>(methodName, args), methodName, args);

        public override void
            CallJNI(string methodName, params object[] args) => 
            CallWithHooks(() => m_WrappedClass.CallJNI(methodName, args), methodName, args);

        public override void
            CallJNIStaticOverload(string methodName, string overloadMethodName, params object[] args) =>
            CallWithHooks(() => m_WrappedClass.CallJNIStaticOverload(methodName, overloadMethodName, args), methodName,
                          args);

        public override T
            CallJNIStaticOverload<T>(string methodName, string overloadMethodName, params object[] args) =>
            CallWithHooks(() => m_WrappedClass.CallJNIStaticOverload<T>(methodName, overloadMethodName, args),
                          methodName, args);

        public override T CallJNIOverload<T>(string methodName, string overloadMethodName, params object[] args) =>
            CallWithHooks(() => m_WrappedClass.CallJNIOverload<T>(methodName, overloadMethodName, args), methodName,
                          args);
    }
}