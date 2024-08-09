using General.Apt.Service.Models;
using System.Management;
using System.Runtime.InteropServices;
using Vortice.DXGI;

namespace General.Apt.App.Adapters.Windows
{
    public static class Adapter
    {
        private static readonly string[] _virtualKeywords = ["Virtual", "Microsoft"];
        private static IEnumerable<Cpu> CpuAdapters { get; } = GetCpuAdapters();
        private static IEnumerable<Gpu> GpuAdapters { get; } = GetGpuAdapters();
        private static IEnumerable<DirectML> DirectMLAdapters { get; } = GetDirectMLAdapters();

        public static ObservableCollection<ComBoBoxItem<string>> GetCpuAndGpu()
        {
            var cpus = CpuAdapters.Select((x, i) => new ComBoBoxItem<string>() { Text = x.Name, Value = $"0:{i}" });
            var gpus = GpuAdapters.Select((x, i) => new ComBoBoxItem<string>() { Text = x.Name, Value = $"1:{i}" });
            return new ObservableCollection<ComBoBoxItem<string>>(cpus.Concat(gpus));
        }

        public static ObservableCollection<ComBoBoxItem<string>> GetCpuAndDirectML()
        {
            var cpus = CpuAdapters.Select((x, i) => new ComBoBoxItem<string>() { Text = x.Name, Value = $"0:{i}" });
            var dmls = DirectMLAdapters.Select((x, i) => new ComBoBoxItem<string>() { Text = x.Name, Value = $"1:{i}" });
            return new ObservableCollection<ComBoBoxItem<string>>(cpus.Concat(dmls));
        }

        private static IEnumerable<Cpu> GetCpuAdapters()
        {
            var result = new List<Cpu>();
            var searcher = new ManagementObjectSearcher("Select * From Win32_Processor");
            foreach (var mo in searcher.Get())
            {
                var name = mo["Name"].ToString();
                result.Add(new Cpu() { Name = name });
            }
            return result;
        }

        private static IEnumerable<Gpu> GetGpuAdapters()
        {
            var result = new List<Gpu>();
            var searcher = new ManagementObjectSearcher("Select * From Win32_VideoController");
            foreach (var mo in searcher.Get())
            {
                var name = mo["Name"].ToString();
                if (IsVirtual(name))
                {
                    continue;
                }
                result.Add(new Gpu() { Name = name });
            }
            return result;
        }

        private static IEnumerable<DirectML> GetDirectMLAdapters()
        {
            var result = new List<DirectML>();
            DXGI.CreateDXGIFactory1(out IDXGIFactory1 factory);
            for (var i = 0; factory.EnumAdapters1(i, out var adapter).Success; i++)
            {
                var name = adapter.Description.Description;
                if (IsVirtual(name))
                {
                    continue;
                }
                result.Add(new DirectML() { Name = name });
            }
            return result;
        }

        private static bool IsVirtual(string name)
        {
            return _virtualKeywords.Any(e => name.Contains(e, StringComparison.OrdinalIgnoreCase));
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern nint LoadLibrary(string lpFileName);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(nint hModule);
        public static bool GetVulkanEnable()
        {
            var handle = LoadLibrary("vulkan-1.dll");
            if (handle != nint.Zero)
            {
                FreeLibrary(handle);
                return true;
            }
            return false;
        }
    }
}
