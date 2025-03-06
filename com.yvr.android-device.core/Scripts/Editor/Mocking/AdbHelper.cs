using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace YVR.AndroidDevice.Core.Editor
{
    // Same code as in HybridSpatial SDB,
    // In the future, this class will be moved to a shared library
    internal static class AdbHelper
    {
        private static Process GetAdbProcess(string args)
        {
            var ret = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "adb",
                    Arguments = args,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            return ret;
        }

        private static IEnumerable<string> RunAdbCmd(string args)
        {
            Process process = GetAdbProcess(args);
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        }

        private static IEnumerable<string> GetDevices()
        {
            List<string> devices = RunAdbCmd("devices").ToList();
            int? index = devices.FindIndex(line => line.Contains("List of devices attached"));
            if (index is null or -1) return null;

            devices = devices.GetRange(index.Value + 1, devices.Count - index.Value - 1);

            return devices.Select(line => line.Split('\t')[0].Trim());
        }

        internal static void Push(string localPath, string remotePath) { RunAdbCmd($"push {localPath} {remotePath}"); }

        internal static bool IsDeviceRoot()
        {
            IEnumerable<string> ret = RunAdbCmd("root");
            return ret.Count() == 1 && ret.First().Contains("adbd is already running as root");
        }

        internal static bool IsPathExist(string path)
        {
            IEnumerable<string> ret = RunAdbCmd($"shell ls {path}");
            return !ret.Any() || !ret.First().Contains("No such file or directory");
        }

        internal static void CreateFolder(string path) { RunAdbCmd($"shell mkdir -p {path}"); }

        internal static void RemoveFolder(string path) { RunAdbCmd($"shell rm -r {path}"); }

        internal static bool GetFirstConnectedDevice(out string device)
        {
            device = GetDevices()?.FirstOrDefault();
            return device is not null;
        }
    }
}