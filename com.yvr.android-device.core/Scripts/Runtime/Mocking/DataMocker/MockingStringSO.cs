using UnityEngine;

namespace YVR.AndroidDevice.Core
{
    [CreateAssetMenu(fileName = "StringMockingData", menuName = "YVR/Android-Device/MockingData/String")]
    public class MockingStringSO : MockingDataSO
    {
        [TextArea(1, 100)] public string value;
    }
}