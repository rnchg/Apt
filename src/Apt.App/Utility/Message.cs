using Apt.Service.Consts;
using Apt.Service.Enums;
using Apt.Service.Utility;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Apt.App.Utility
{
    public static class Message
    {
        public static void ShowSnackbar(string title, string message, ControlAppearance appearance, SymbolRegular symbol = SymbolRegular.Fluent24, int timeout = 2)
        {
            App.Current.GetRequiredService<ISnackbarService>().Show(message, title, appearance, new SymbolIcon(symbol), TimeSpan.FromSeconds(timeout));
        }

        public static void ShowSnackbarInfo(string message, SymbolRegular symbol = SymbolRegular.Info24, int timeout = 2) => ShowSnackbar(message, Language.Instance["ShowSnackbarTitleInfo"], ControlAppearance.Info, symbol, timeout);

        public static void ShowSnackbarSuccess(string message, SymbolRegular symbol = SymbolRegular.CheckmarkCircle24, int timeout = 2) => ShowSnackbar(message, Language.Instance["ShowSnackbarTitleSuccess"], ControlAppearance.Success, symbol, timeout);

        public static void ShowSnackbarWarning(string message, SymbolRegular symbol = SymbolRegular.Warning24, int timeout = 2) => ShowSnackbar(message, Language.Instance["ShowSnackbarTitleWarning"], ControlAppearance.Caution, symbol, timeout);

        public static void ShowSnackbarError(string message, SymbolRegular symbol = SymbolRegular.DismissCircle24, int timeout = 2) => ShowSnackbar(message, Language.Instance["ShowSnackbarTitleError"], ControlAppearance.Danger, symbol, timeout);

        public static Task<Wpf.Ui.Controls.MessageBoxResult> ShowMessage(string message, string title)
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox()
            {
                Title = title,
                Content = message,
                CloseButtonText = Language.Instance["ShowMessageButtonClose"]
            };
            return messageBox.ShowDialogAsync();
        }

        public static Task<Wpf.Ui.Controls.MessageBoxResult> ShowMessageInfo(string message) => ShowMessage(message, Language.Instance["ShowMessageTitleInfo"]);

        public static Task<Wpf.Ui.Controls.MessageBoxResult> ShowMessageSuccess(string message) => ShowMessage(message, Language.Instance["ShowMessageTitleSuccess"]);

        public static Task<Wpf.Ui.Controls.MessageBoxResult> ShowMessageWarning(string message) => ShowMessage(message, Language.Instance["ShowMessageTitleWarning"]);

        public static Task<Wpf.Ui.Controls.MessageBoxResult> ShowMessageError(string message) => ShowMessage(message, Language.Instance["ShowMessageTitleError"]);

        public static async Task<Wpf.Ui.Controls.MessageBoxResult> ShowMessageConfirm(string message, Action? ok = null, Action? cancel = null)
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox()
            {
                Content = message,
                Title = Language.Instance["ShowMessageConfirmTitle"],
                IsPrimaryButtonEnabled = true,
                PrimaryButtonText = Language.Instance["ShowMessageConfirmButtonOk"],
                CloseButtonText = Language.Instance["ShowMessageConfirmButtonCancel"],
                PrimaryButtonAppearance = ControlAppearance.Info,
                CloseButtonAppearance = ControlAppearance.Danger
            };
            var result = await messageBox.ShowDialogAsync();
            if (result == Wpf.Ui.Controls.MessageBoxResult.Primary)
            {
                ok?.Invoke();
            }
            else
            {
                cancel?.Invoke();
            }
            return result;
        }

        public static Task AddMessage(MessageType type, string message, Action<Paragraph> action)
        {
            if (type == MessageType.Info)
            {
                return AddMessageInfo(message, action);
            }
            else if (type == MessageType.Success)
            {
                return AddMessageSuccess(message, action);
            }
            else if (type == MessageType.Warning)
            {
                return AddMessageWarning(message, action);
            }
            else if (type == MessageType.Error)
            {
                return AddMessageError(message, action);
            }
            return Task.CompletedTask;
        }

        public static Task AddMessageInfo(string message, Action<Paragraph> action)
        {
            return Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    message = $"[ {DateTime.Now} ]=>[ {message} ]";
                    var run = new Run(message);
                    var color = (Color)ColorConverter.ConvertFromString(AppConst.Color.Info);
                    run.Foreground = new SolidColorBrush(color);
                    var paragraph = new Paragraph(run);
                    action.Invoke(paragraph);
                });
            });
        }

        public static Task AddMessageSuccess(string message, Action<Paragraph> action)
        {
            return Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    message = $"[ {DateTime.Now} ]=>[ {message} ]";
                    var run = new Run(message);
                    var color = (Color)ColorConverter.ConvertFromString(AppConst.Color.Success);
                    run.Foreground = new SolidColorBrush(color);
                    var paragraph = new Paragraph(run);
                    action.Invoke(paragraph);
                });
            });
        }

        public static Task AddMessageWarning(string message, Action<Paragraph> action)
        {
            return Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    message = $"[ {DateTime.Now} ]=>[ {message} ]";
                    var run = new Run(message);
                    var color = (Color)ColorConverter.ConvertFromString(AppConst.Color.Warning);
                    run.Foreground = new SolidColorBrush(color);
                    var paragraph = new Paragraph(run);
                    action.Invoke(paragraph);
                });
            });
        }

        public static Task AddMessageError(string message, Action<Paragraph> action)
        {
            return Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    message = $"[ {DateTime.Now} ]=>[ {message} ]";
                    var run = new Run(message);
                    var color = (Color)ColorConverter.ConvertFromString(AppConst.Color.Error);
                    run.Foreground = new SolidColorBrush(color);
                    var paragraph = new Paragraph(run);
                    action.Invoke(paragraph);
                });
            });
        }

        public static Task AddText(string text, Action<Paragraph> action, string textColor = AppConst.Color.Text)
        {
            return Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var run = new Run(text);
                    var color = (Color)ColorConverter.ConvertFromString(textColor);
                    run.Foreground = new SolidColorBrush(color);
                    var paragraph = new Paragraph(run);
                    action.Invoke(paragraph);
                });
            });
        }

        public static Task AddTextInfo(string text, Action<Paragraph> action, string textColor = AppConst.Color.Info) => AddText(text, action, textColor);

        public static Task AddTextSuccess(string text, Action<Paragraph> action, string textColor = AppConst.Color.Success) => AddText(text, action, textColor);

        public static Task AddTextWarning(string text, Action<Paragraph> action, string textColor = AppConst.Color.Warning) => AddText(text, action, textColor);

        public static Task AddTextError(string text, Action<Paragraph> action, string textColor = AppConst.Color.Error) => AddText(text, action, textColor);
    }
}
