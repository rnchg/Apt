using General.Apt.Service.Adapters;
using General.Apt.Service.Models;

namespace General.Apt.App.Utility
{
    public static partial class Device
    {
        public static ObservableCollection<ComBoBoxItem<string>> Provider { get; } = Windows.GetAdapters(true);

        public static ObservableCollection<ComBoBoxItem<string>> Provider2 { get; } = Windows.GetAdapters(false);

        public static bool VulkanEnable { get; } = Windows.GetVulkanEnable();
    }
}