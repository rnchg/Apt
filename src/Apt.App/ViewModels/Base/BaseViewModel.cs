using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Abstractions.Controls;

namespace Apt.App.ViewModels.Base
{
    public abstract partial class BaseViewModel : ObservableObject, INavigationAware
    {
        public virtual async Task OnNavigatedToAsync()
        {
            using CancellationTokenSource cts = new();
            await DispatchAsync(OnNavigatedTo, cancellationToken: cts.Token);
        }

        public virtual void OnNavigatedTo() { }

        public virtual async Task OnNavigatedFromAsync()
        {
            using CancellationTokenSource cts = new();
            await DispatchAsync(OnNavigatedFrom, cancellationToken: cts.Token);
        }

        public virtual void OnNavigatedFrom() { }

        public static async Task DispatchAsync(Action callback, DispatcherPriority priority = DispatcherPriority.Normal, CancellationToken cancellationToken = default)
        {
            await Application.Current.Dispatcher.InvokeAsync(callback, priority, cancellationToken);
        }
    }
}
