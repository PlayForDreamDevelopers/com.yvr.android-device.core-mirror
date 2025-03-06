using System.Diagnostics;
using YVR.Utilities;

namespace YVR.AndroidDevice.Core
{
    public class AJCTimerWrapper : AJCWrapperBase
    {
        private Stopwatch m_Stopwatch = new();

        public AJCTimerWrapper(AJCBase wrappedClass) : base(wrappedClass) { }

        protected override void BeforeCallAJCMethod(string methodName, params object[] args) { m_Stopwatch.Restart(); }

        protected override void AfterCallAJCMethod(string methodName, params object[] args)
        {
            m_Stopwatch.Stop();
            this.Debug($"AJC Method: {methodName} cost {m_Stopwatch.ElapsedTicks / 10000.0d} ms");
        }
    }
}