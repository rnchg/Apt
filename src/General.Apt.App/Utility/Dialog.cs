using System.Windows.Navigation;

namespace General.Apt.App.Utility
{
    public static class Dialog
    {
        public static void ShowWindow(string title, string uri, int width, int height, Window owner = null)
        {
            var window = new NavigationWindow();
            window.Title = title;
            window.Width = width;
            window.Height = height;
            window.Owner = owner;
            if (window.Owner == null)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            window.ResizeMode = ResizeMode.NoResize;
            window.Source = new Uri(uri, UriKind.Relative);
            window.ShowsNavigationUI = false;
            window.Show();
        }

        public static void ShowDialogWindow(string title, string uri, int width, int height, Window owner = null)
        {
            var window = new NavigationWindow();
            window.Title = title;
            window.Width = width;
            window.Height = height;
            window.Owner = owner;
            if (window.Owner == null)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            window.ResizeMode = ResizeMode.NoResize;
            window.Source = new Uri(uri, UriKind.Relative);
            window.ShowsNavigationUI = false;
            var result = window.ShowDialog();
            if (result.HasValue && result.Value)
            {
                window.Close();
            }
        }

        public static MessageBoxResult ShowDialog(string content)
        {
            return MessageBox.Show(content);
        }
        public static MessageBoxResult ShowDialog(string content, string caption)
        {
            return MessageBox.Show(content, caption);
        }
        public static MessageBoxResult ShowDialog(string content, string caption, MessageBoxButton button)
        {
            return MessageBox.Show(content, caption, button);
        }
        public static MessageBoxResult ShowDialog(string content, string caption, MessageBoxButton button, MessageBoxImage image)
        {
            return MessageBox.Show(content, caption, button, image);
        }
        public static MessageBoxResult ShowDialog(string content, string caption, MessageBoxImage image, Action ok)
        {
            var result = ShowDialog(content, caption, MessageBoxButton.OKCancel, image);
            if (result == MessageBoxResult.OK)
            {
                ok?.Invoke();
            }
            return result;
        }
        public static MessageBoxResult ShowDialog(string content, string caption, MessageBoxImage image, Action ok, Action cancel)
        {
            var result = ShowDialog(content, caption, image, ok);
            if (result == MessageBoxResult.Cancel)
            {
                cancel?.Invoke();
            }
            return result;
        }

        public static MessageBoxResult ShowInformationDialog(string content)
        {
            return ShowDialog(content, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static MessageBoxResult ShowInformationDialog(string content, string caption)
        {
            return ShowDialog(content, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static MessageBoxResult ShowInformationDialog(string content, string caption, MessageBoxButton button)
        {
            return ShowDialog(content, caption, button, MessageBoxImage.Information);
        }
        public static MessageBoxResult ShowInformationDialog(string content, string caption, Action ok)
        {
            return ShowDialog(content, caption, MessageBoxImage.Information, ok);
        }
        public static MessageBoxResult ShowInformationDialog(string content, string caption, Action ok, Action cancel)
        {
            return ShowDialog(content, caption, MessageBoxImage.Information, ok, cancel);
        }

        public static MessageBoxResult ShowWarningDialog(string content)
        {
            return ShowDialog(content, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public static MessageBoxResult ShowWarningDialog(string content, string caption)
        {
            return ShowDialog(content, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public static MessageBoxResult ShowWarningDialog(string content, string caption, MessageBoxButton button)
        {
            return ShowDialog(content, caption, button, MessageBoxImage.Warning);
        }
        public static MessageBoxResult ShowWarningDialog(string content, string caption, Action ok)
        {
            return ShowDialog(content, caption, MessageBoxImage.Warning, ok);
        }
        public static MessageBoxResult ShowWarningDialog(string content, string caption, Action ok, Action cancel)
        {
            return ShowDialog(content, caption, MessageBoxImage.Warning, ok, cancel);
        }

        public static MessageBoxResult ShowErrorDialog(string content)
        {
            return ShowDialog(content, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static MessageBoxResult ShowErrorDialog(string content, string caption)
        {
            return ShowDialog(content, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static MessageBoxResult ShowErrorDialog(string content, string caption, MessageBoxButton button)
        {
            return ShowDialog(content, caption, button, MessageBoxImage.Error);
        }
        public static MessageBoxResult ShowErrorDialog(string content, string caption, Action ok)
        {
            return ShowDialog(content, caption, MessageBoxImage.Error, ok);
        }
        public static MessageBoxResult ShowErrorDialog(string content, string caption, Action ok, Action cancel)
        {
            return ShowDialog(content, caption, MessageBoxImage.Error, ok, cancel);
        }
    }
}
