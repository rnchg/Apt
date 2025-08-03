using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace APT.App.Views.Dialogs
{
    public partial class PlusDialog : ContentDialog
    {
        public PlusDialog(ContentPresenter? contentPresenter)
        : base(contentPresenter)
        {
            InitializeComponent();
        }

        protected override void OnButtonClick(ContentDialogButton button)
        {
            base.OnButtonClick(button);
        }
    }
}
