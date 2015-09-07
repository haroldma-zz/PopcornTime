using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Navigation;
using PopcornTime.Common;
using PopcornTime.Services.NavigationService;

namespace PopcornTime.Tools.Mvvm
{
    public abstract class ViewModelBase : ObservableObject, INavigatable
    {
        public bool IsInDesignMode => DesignMode.DesignModeEnabled;
        public virtual bool KeepOnBackstack { get; } = true;
        public string PageKey { get; set; }

        public virtual void OnNavigatedTo(object parameter, NavigationMode mode, Dictionary<string, object> state)
        {
        }

        public virtual void OnSaveState(bool suspending, Dictionary<string, object> state)
        {
        }

        public virtual void OnNavigatedFrom()
        {
        }

        public virtual string SimplifiedParameter(object parameter)
        {
            return parameter?.ToString();
        }
    }
}