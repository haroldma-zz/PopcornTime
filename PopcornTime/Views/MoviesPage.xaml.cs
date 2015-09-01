using PopcornTime.ViewModels;

namespace PopcornTime.Views
{
    public sealed partial class MoviesPage
    {
        public MoviesPage()
        {
            InitializeComponent();
            ViewModel = DataContext as MoviesViewModel;
        }

        public MoviesViewModel ViewModel { get; }
    }
}