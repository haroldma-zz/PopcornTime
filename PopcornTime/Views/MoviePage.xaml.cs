using PopcornTime.ViewModels;

namespace PopcornTime.Views
{
    public sealed partial class MoviePage
    {
        public MoviePage()
        {
            InitializeComponent();
            ViewModel = DataContext as MovieViewModel;
        }

        public MovieViewModel ViewModel { get; }
    }
}