using General.Apt.Service.Models;
using System.Management;

namespace General.Apt.App.Utility
{
    public static class Searcher
    {
        public static ObservableCollection<ComBoBoxItem<string>> GetProvider()
        {
            var result = new ObservableCollection<ComBoBoxItem<string>>();
            var managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_VideoController");
            var count = 0;
            foreach (var mo in managementObjectSearcher.Get())
            {
                result.Add(new ComBoBoxItem<string>() { Text = mo["Name"].ToString(), Value = $"1:{count}" });
                count++;
            }
            managementObjectSearcher.Dispose();
            result.Insert(0, new ComBoBoxItem<string> { Text = "CPU", Value = "0:0" });
            return result;
        }
    }
}
