using General.Apt.Service.Models;
using System.Management;
using Vortice.DXGI;

namespace General.Apt.App.Adapters.Windows
{
    public static class Adapter
    {
        private static readonly string[] _virtualGpuKeys = ["Virtual", "Microsoft"];
        private static IEnumerable<Cpu> CpuAdapters { get; } = GetCpuAdapters();
        private static IEnumerable<Gpu> GpuAdapters { get; } = GetGpuAdapters();
        private static bool IsVirtualGpu(string name) => _virtualGpuKeys.Any(e => name.Contains(e, StringComparison.OrdinalIgnoreCase));

        public static ObservableCollection<ComBoBoxItem<string>> CpuAndGpu { get; } = GetCpuAndGpu();

        private static ObservableCollection<ComBoBoxItem<string>> GetCpuAndGpu()
        {
            var cpus = CpuAdapters.Select((x, i) => new ComBoBoxItem<string>() { Text = x.Name, Value = $"0:{i}" });
            var gpus = GpuAdapters.Select((x, i) => new ComBoBoxItem<string>() { Text = x.Name, Value = $"1:{i}" });
            return new ObservableCollection<ComBoBoxItem<string>>(cpus.Concat(gpus));
        }

        private static IEnumerable<Cpu> GetCpuAdapters()
        {
            var searcher = new ManagementObjectSearcher("Select * From Win32_Processor");
            foreach (var mo in searcher.Get())
            {
                var name = mo["Name"].ToString();
                yield return new Cpu() { Name = name };
            }
        }

        private static IEnumerable<Gpu> GetGpuAdapters()
        {
            DXGI.CreateDXGIFactory1(out IDXGIFactory1 factory);
            for (var i = 0; factory.EnumAdapters1(i, out var adapter).Success; i++)
            {
                var name = adapter.Description.Description;
                if (IsVirtualGpu(name))
                {
                    continue;
                }
                yield return new Gpu() { Name = name };
            }

            //var searcher = new ManagementObjectSearcher("Select * From Win32_VideoController");
            //foreach (var mo in searcher.Get())
            //{
            //    var name = mo["Name"].ToString();
            //    if (IsVirtualGpu(name))
            //    {
            //        continue;
            //    }
            //    yield return new Gpu() { Name = name };
            //}
        }
    }
}
