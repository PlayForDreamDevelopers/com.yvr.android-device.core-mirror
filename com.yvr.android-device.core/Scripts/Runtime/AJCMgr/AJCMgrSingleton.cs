using YVR.Utilities;

namespace YVR.AndroidDevice.Core
{
    /// <summary>
    /// Abstract base class for managing Android Java Classes (AJC) as a singleton instance.
    /// </summary>
    /// <typeparam name="T">Derived type of AJCMgrSingleton.</typeparam>
    /// <typeparam name="TMocker">Type of AJC mocker.</typeparam>
    /// <typeparam name="TElement">Type of AJC element.</typeparam>
    public abstract class AJCMgrSingleton<T, TMocker, TElement> : Singleton<T>, IAJCMgr
        where T : AJCMgrSingleton<T, TMocker, TElement>, IAJCMgr, new()
        where TMocker : AJCMocker
        where TElement : class, IAJCElements, new()
    {
        public AJCBase ajcBase { get; set; }
        public object[] constructorArgs { get; set; }

        private static TElement s_Elements = null;

        public static TElement elements => s_Elements ??= new TElement();

        ///<inheritdoc/>
        protected override void OnInit()
        {
            base.OnInit();
            ConfigureAJCFactory();
        }

        protected void ConfigureAJCFactory() { this.ConfigureAJCFactory(elements.className, typeof(TMocker)); }

        /// <summary>
        /// Gets the AJC mocker instance for the current AJC element.
        /// </summary>
        /// <returns>The AJC mocker instance.</returns>
        public TMocker GetMocker() => ajcBase.core as TMocker;
    }
}