using Apt.Service.Extensions;
using Apt.Service.Models;
using System.Management;
using Vortice.DXGI;

namespace Apt.App.Adapters.Windows
{
    public static class Adapter
    {
        private static readonly string[] _virtualGpuKeys = ["Virtual", "Microsoft"];
        private static bool IsVirtualGpu(string? name) => _virtualGpuKeys.Any(e => name?.Contains(e, StringComparison.OrdinalIgnoreCase) ?? false);

        private static IEnumerable<Cpu> CpuAdapters { get; } = GetCpuAdapters();
        private static IEnumerable<Gpu> GpuAdapters { get; } = GetGpuAdapters();

        public static ObservableCollection<ComBoBoxItem<string>> CpuAndGpu { get; } = GetCpuAndGpu();

        private static ObservableCollection<ComBoBoxItem<string>> GetCpuAndGpu()
        {
            var cpus = CpuAdapters.Select((x, i) => new ComBoBoxItem<string>() { Text = x.Name, Value = new Provider() { Type = nameof(Cpu), Value = i }.ToJson() });
            var gpus = GpuAdapters.Select((x, i) => new ComBoBoxItem<string>() { Text = x.Name, Value = new Provider() { Type = nameof(Gpu), Value = i }.ToJson() });
            return new ObservableCollection<ComBoBoxItem<string>>(cpus.Concat(gpus));
        }

        private static IEnumerable<Cpu> GetCpuAdapters()
        {
            var searcher = new ManagementObjectSearcher("Select * From Win32_Processor");
            if (searcher is not null)
            {
                foreach (var mo in searcher.Get())
                {
                    var name = mo["Name"]?.ToString();
                    yield return new Cpu() { Name = name };
                }
            }
        }

        private static IEnumerable<Gpu> GetGpuAdapters()
        {
            DXGI.CreateDXGIFactory1(out IDXGIFactory1? factory);
            if (factory is not null)
            {
                for (uint i = 0; factory.EnumAdapters1(i, out var adapter).Success; i++)
                {
                    var name = adapter.Description.Description;
                    if (IsVirtualGpu(name))
                    {
                        continue;
                    }
                    yield return new Gpu() { Name = name };
                }
            }

            //var searcher = new ManagementObjectSearcher("Select * From Win32_VideoController");
            //if (searcher is not null)
            //{
            //    foreach (var mo in searcher.Get())
            //    {
            //        var name = mo["Name"]?.ToString();
            //        if (IsVirtualGpu(name))
            //        {
            //            continue;
            //        }
            //        yield return new Gpu() { Name = name };
            //    }
            //}
        }
    }
}
