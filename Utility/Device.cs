using ComputeSharp;
using General.Apt.Service.Models;
using System.Management;
using System.Runtime.InteropServices;

namespace General.Apt.App.Utility
{
    public static partial class Device
    {
        private const int memorySize = 8 * 1024 * 1024;

        public static ObservableCollection<ComBoBoxItem<string>> Provider { get; } = GetProvider();

        public static ObservableCollection<ComBoBoxItem<string>> Provider2 { get; } = GetProvider2();

        public static ObservableCollection<ComBoBoxItem<string>> GetProvider()
        {
            var result = new ObservableCollection<ComBoBoxItem<string>>() { new ComBoBoxItem<string> { Text = "CPU", Value = "0:0" } };
            var devices = GraphicsDevice.QueryDevices(e => e.DedicatedMemorySize > memorySize).DistinctBy(e => e.Name);
            var index = 0;
            foreach (var item in devices)
            {
                result.Add(new ComBoBoxItem<string>() { Text = item.Name, Value = $"1:{index++}" });
            }
            return result;
        }

        public static ObservableCollection<ComBoBoxItem<string>> GetProvider2()
        {
            var result = new ObservableCollection<ComBoBoxItem<string>>() { new ComBoBoxItem<string> { Text = "CPU", Value = "0:0" } };
            using var managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            var derices = managementObjectSearcher.Get().Cast<ManagementObject>().Where(e => Convert.ToUInt64(e["AdapterRAM"]) > memorySize).DistinctBy(e => e["Name"]);
            var index = 0;
            foreach (var item in derices)
            {
                result.Add(new ComBoBoxItem<string>() { Text = item["Name"].ToString(), Value = $"1:{index++}" });
            }
            return result;
        }

        public static bool VulkanEnable { get; } = GetVulkanEnable();

        [LibraryImport("vulkan-1.dll")]
        private static partial int vkEnumerateInstanceExtensionProperties(IntPtr layerName, ref uint propertyCount, IntPtr properties);
        public static bool GetVulkanEnable()
        {
            try
            {
                uint count = 0;
                IntPtr properties = IntPtr.Zero;
                int result = vkEnumerateInstanceExtensionProperties(IntPtr.Zero, ref count, properties);
                return result == 0;
            }
            catch
            {
                return false;
            }
        }

        //[DllImport("kernel32.dll", SetLastError = true)]
        //private static extern IntPtr LoadLibrary(string lpFileName);
        //[DllImport("kernel32.dll", SetLastError = true)]
        //private static extern bool FreeLibrary(IntPtr hModule);
        //public static bool GetVulkanEnable()
        //{
        //    var handle = LoadLibrary("vulkan-1.dll");
        //    if (handle != IntPtr.Zero)
        //    {
        //        FreeLibrary(handle);
        //        return true;
        //    }
        //    return false;
        //}
    }
}