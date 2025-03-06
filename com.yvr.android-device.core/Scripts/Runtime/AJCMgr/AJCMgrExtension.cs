using System;

namespace YVR.AndroidDevice.Core
{
    public static class AJCMgrExtension
    {
        public static void ConfigureAJCFactory(this IAJCMgr ajcMgr, string className, Type mockerType)
        {
            AJCFactory.instance.onWrappedModeChanged += OnWrappedModeChanged;
            AJCFactory.instance.onMockingModeChanged += OnMockModeChanged;
            OnMockModeChanged(AJCFactory.instance.isMocking);

            void OnMockModeChanged(bool isMocking)
            {
                if (isMocking)
                    AJCFactory.instance.RegisterMockerType(className, mockerType);
                else
                    AJCFactory.instance.UnRegisterMockerType(className);

                ajcMgr.ajcBase = AJCFactory.instance.GetClass(className, ajcMgr.constructorArgs);
            }

            void OnWrappedModeChanged(AJCWrapMode mode)
            {
                ajcMgr.ajcBase = AJCFactory.instance.GetClass(className, ajcMgr.constructorArgs);
            }
        }
    }
}