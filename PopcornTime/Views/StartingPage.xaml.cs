using PopcornTime.ViewModels;

namespace PopcornTime.Views
{
    public sealed partial class StartingPage
    {
        public StartingPage()
        {
            InitializeComponent();
            ViewModel = DataContext as StartingViewModel;
        }

        public StartingViewModel ViewModel { get; }
    }
}