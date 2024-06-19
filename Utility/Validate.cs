using General.Apt.App.Services;
using General.Apt.App.Views.Windows;
using General.Apt.Service.Utility;

namespace General.Apt.App.Utility
{
    public static class Validate
    {
        public static async Task ShowLicense(string message)
        {
            var text = string.Format(Language.GetString("LicenseValidateFail"), message);

            await Message.ShowMessageConfirm(text, () =>
            {
                App.Current.GetRequiredService<WindowsProviderService>().ShowDialog<LicenseWindow>();
            });
        }

        public static async Task ValidateLicense()
        {
            if (!License.Validate(out var requestCode, out var message))
            {
                await ShowLicense(message);
            }
        }
    }
}
