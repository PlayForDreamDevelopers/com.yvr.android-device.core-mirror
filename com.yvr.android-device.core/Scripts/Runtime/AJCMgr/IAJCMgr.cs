namespace YVR.AndroidDevice.Core
{
    public interface IAJCMgr
    {
        public AJCBase ajcBase { get; set; }
        public object[] constructorArgs { get; set; }
    }
}