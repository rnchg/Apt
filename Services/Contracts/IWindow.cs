namespace General.Apt.App.Services.Contracts
{
    public interface IWindow
    {
        event RoutedEventHandler Loaded;
        void Show();
    }
}
