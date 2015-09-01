using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace PopcornTime.Common
{
    public abstract class IncrementalLoadingBase<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        #region State 

        private bool _busy;

        #endregion

        #region Private methods 

        private async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            try
            {
                var items = await LoadMoreItemsOverrideAsync(c, count);

                if (items == null)
                    return new LoadMoreItemsResult();

                foreach (var item in items)
                    Add(item);

                return new LoadMoreItemsResult {Count = (uint) items.Count};
            }
            finally
            {
                _busy = false;
            }
        }

        #endregion

        #region ISupportIncrementalLoading 

        public bool HasMoreItems => HasMoreItemsOverride();

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (_busy)
            {
                throw new InvalidOperationException("Only one operation in flight at a time");
            }

            _busy = true;

            return AsyncInfo.Run(c => LoadMoreItemsAsync(c, count));
        }

        #endregion

        #region Overridable methods 

        protected abstract Task<IList<T>> LoadMoreItemsOverrideAsync(CancellationToken c, uint count);
        protected abstract bool HasMoreItemsOverride();

        #endregion
    }
}