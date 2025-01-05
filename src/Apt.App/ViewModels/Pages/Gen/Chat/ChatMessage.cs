using CommunityToolkit.Mvvm.ComponentModel;

namespace Apt.App.ViewModels.Pages.Gen.Chat
{
    public partial class ChatMessage : ObservableObject
    {
        [ObservableProperty]
        public string _type = null!;

        [ObservableProperty]
        public string _text = null!;

        [ObservableProperty]
        public string _image = null!;

        [ObservableProperty]
        public string _video = null!;

        [ObservableProperty]
        public DateTime _time = DateTime.Now;

        [ObservableProperty]
        public bool _isOwner;
    }
}
