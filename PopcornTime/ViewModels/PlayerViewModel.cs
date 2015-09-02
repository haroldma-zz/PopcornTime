using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using PopcornTime.Services.Interfaces;
using PopcornTime.Tools.Mvvm;

namespace PopcornTime.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        private readonly ITorrentStreamService _torrentStreamService;

        public PlayerViewModel(ITorrentStreamService torrentStreamService)
        {
            _torrentStreamService = torrentStreamService;
        }
    }
}