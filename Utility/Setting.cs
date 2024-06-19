using General.Apt.Service.Extensions;
using General.Apt.Service.Models;
using General.Apt.Service.Utility;

namespace General.Apt.App.Utility
{
    public static class Setting
    {
        public static bool GetSetting()
        {
            try
            {
                var config = Properties.Settings.Default.Config.JsonTo<Config>();
                Current.Config = config;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SetSetting()
        {
            try
            {
                Properties.Settings.Default.Config = Current.Config.ToJson();
                Properties.Settings.Default.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
