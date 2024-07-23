using General.Apt.Service.Adapters.Windows;
using General.Apt.Service.Models;

namespace General.Apt.App.Utility
{
    public static partial class Device
    {
        public static ObservableCollection<ComBoBoxItem<string>> Provider { get; } = Adapter.GetAdapters(false);

        public static ObservableCollection<ComBoBoxItem<string>> Provider2 { get; } = Adapter.GetAdapters(true);

        public static bool VulkanEnable { get; } = Adapter.GetVulkanEnable();
    }
}