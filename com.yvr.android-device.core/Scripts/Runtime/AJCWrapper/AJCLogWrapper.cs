using YVR.Utilities;

namespace YVR.AndroidDevice.Core
{
    public class AJCLogWrapper : AJCWrapperBase
    {
        public AJCLogWrapper(AJCBase wrappedClass) : base(wrappedClass) { }

        protected override void BeforeCallAJCMethod(string methodName, params object[] args)
        {
            this.Debug($"Before Calling AJC Method: {methodName}, with args: {args.ToElementsString()}");
        }

        protected override void AfterCallAJCMethod(string methodName, params object[] args)
        {
            this.Debug($"After Calling AJC Method: {methodName}, with args: {args.ToElementsString()}");
        }
    }
}