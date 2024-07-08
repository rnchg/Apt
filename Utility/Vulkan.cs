using System.Runtime.InteropServices;

namespace General.Apt.App.Utility
{
    public partial class Vulkan
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        public static bool IsSupport()
        {
            var handle = LoadLibrary("vulkan-1.dll");
            if (handle != IntPtr.Zero)
            {
                FreeLibrary(handle);
                return true;
            }
            return false;
        }
    }
}
