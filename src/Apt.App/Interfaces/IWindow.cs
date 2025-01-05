namespace Apt.App.Interfaces
{
    public interface IWindow
    {
        event RoutedEventHandler Loaded;
        void Show();
    }
}
