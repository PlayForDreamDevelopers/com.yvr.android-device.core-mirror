#if USE_YVR_UNIRX
using System.Threading;
using UniRx;
using UnityEngine;
using System.IO;

#pragma warning disable CS8321 // Local function is declared but never used

namespace YVR.AndroidDevice.Core
{
    [CreateAssetMenu(fileName = "TextureMockingData", menuName = "YVR/Android-Device/MockingData/Texture")]
    public class MockingTextureSO : MockingDataSO
    {
        public Texture value;

        public byte[] GetTextureData()
        {
#if UNITY_EDITOR
            byte[] ret = LoadDataFromAsset();
#else
            byte[] ret = LoadDataFromTex();
#endif
            return ret;

#if UNITY_EDITOR
            byte[] LoadDataFromAsset()
            {
                string texPath = null;
                var waitHandle = new AutoResetEvent(false);
                Scheduler.MainThread.Schedule(() =>
                {
                    texPath = Path.GetFullPath(UnityEditor.AssetDatabase.GetAssetPath(value));
                    waitHandle.Set();
                });
                waitHandle.WaitOne();
                return File.ReadAllBytes(texPath);
            }
#endif

            byte[] LoadDataFromTex()
            {
                byte[] data = null;
                var waitHandle = new AutoResetEvent(false);

                Scheduler.MainThread.Schedule(() =>
                {
                    var tex2D = (Texture2D) value;
                    data = tex2D.EncodeToPNG();
                    waitHandle.Set();
                });

                waitHandle.WaitOne();
                return data;
            }
        }
    }
}
#endif