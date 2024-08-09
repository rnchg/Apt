using General.Apt.App.Adapters.Windows;
using General.Apt.Service.Models;

namespace General.Apt.App.Utility
{
    public static partial class Device
    {
        public static ObservableCollection<ComBoBoxItem<string>> CpuAndDirectML { get; } = Adapter.GetCpuAndDirectML();

        public static ObservableCollection<ComBoBoxItem<string>> CpuAndGpu { get; } = Adapter.GetCpuAndGpu();

        public static bool VulkanEnable { get; } = Adapter.GetVulkanEnable();
    }
}