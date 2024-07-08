using General.Apt.Service.Models;
using System.Management;

namespace General.Apt.App.Utility
{
    public static class Searcher
    {
        public static ObservableCollection<ComBoBoxItem<string>> GetProvider()
        {
            var result = new ObservableCollection<ComBoBoxItem<string>>() { new ComBoBoxItem<string> { Text = "CPU", Value = "0:0" } };
            using var managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_VideoController");
            var index = 0;
            foreach (var item in managementObjectSearcher.Get())
            {
                var name = item["Name"];
                var deviceId = index;
                result.Add(new ComBoBoxItem<string>() { Text = name.ToString(), Value = $"1:{deviceId}" });
                index++;
            }
            return result;
        }
    }
}