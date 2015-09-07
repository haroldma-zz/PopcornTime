using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace PopcornTime.Services.NavigationService
{
    public interface INavigatable
    {
        bool KeepOnBackstack { get; }
        string PageKey { get; set; }
        void OnNavigatedTo(object parameter, NavigationMode mode, Dictionary<string, object> state);
        void OnSaveState(bool suspending, Dictionary<string, object> state);
        void OnNavigatedFrom();
        string SimplifiedParameter(object parameter);
    }
}