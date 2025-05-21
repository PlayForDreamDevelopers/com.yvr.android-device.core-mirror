using System;
using System.Collections.Generic;
using UnityEngine;

namespace YVR.AndroidDevice.Core.Utilities
{
    public static class JavaObjectConverter
    {
        public static List<string> JavaListToCSharpList(AndroidJavaObject javaList)
        {
            List<string> packages = new List<string>();
            int size = javaList.Call<int>("size");
            for (int i = 0; i < size; i++)
            {
                string package = javaList.Call<string>("get", i);
                packages.Add(package);
            }

            return packages;
        }

        public static AndroidJavaProxy CreatConsumerProxy<T>(Action<T> callback)
        {
            var consumerProxy = new ConsumerProxy<T>(callback);
            return consumerProxy;
        }
    }
}