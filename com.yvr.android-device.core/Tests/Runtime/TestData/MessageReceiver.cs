using UnityEngine;

namespace YVR.AndroidDevice.Core.Tests
{
    public class MessageReceiver : MonoBehaviour
    {
        public void OnMessageReceived(string msg)
        {
            Debug.Log($"Received message is for function OnMessageReceived on go {gameObject.name}, msg is {msg}");
        }
    }
}