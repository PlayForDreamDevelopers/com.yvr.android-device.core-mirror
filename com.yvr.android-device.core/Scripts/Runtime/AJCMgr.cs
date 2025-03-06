using UnityEngine;

namespace YVR.AndroidDevice.Core
{
    public class AJCMgr<T, TMocker, TElement> : IAJCMgr
        where T : AJCMgr<T, TMocker, TElement>, new()
        where TMocker : AJCMocker
        where TElement : class, IAJCElements, new()
    {
        public AJCBase ajcBase { get; set; }
        public object[] constructorArgs { get; set; }

        private static TElement s_Elements = null;
        public static TElement elements => s_Elements ??= new TElement();

        public AJCMgr() : this(null)
        {
        }

        public AJCMgr(params object[] args)
        {
            constructorArgs = args;
            this.ConfigureAJCFactory(elements.className, typeof(TMocker));
        }

        /// <summary>
        /// Gets the AJC mocker instance for the current AJC element.
        /// </summary>
        /// <returns>The AJC mocker instance.</returns>
        public TMocker GetMocker() => ajcBase.core as TMocker;
    }
}