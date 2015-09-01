#region License

// Copyright (c) 2015 Harold Martinez-Molina <hanthonym@outlook.com>
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PopcornTime.Extensions;

namespace PopcornTime.Common
{
    public class BetterGridView : GridView
    {
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset", typeof (double), typeof (BetterGridView),
                new PropertyMetadata(0, VerticalOffsetPropertyChanged));

        private ScrollViewer _scroll;

        public BetterGridView()
        {
            Loaded += (s, e) =>
            {
                ScrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
                ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            };
        }

        public ScrollViewer ScrollViewer => _scroll ?? (_scroll = this.GetScrollViewer());

        public double VerticalOffset
        {
            get { return (double) GetValue(VerticalOffsetProperty); }

            set { SetValue(VerticalOffsetProperty, value); }
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            VerticalOffset = ScrollViewer.VerticalOffset;
        }

        private static void VerticalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (BetterGridView) d;

            RoutedEventHandler handler = null;
            Action action = async () =>
            {
                // ReSharper disable AccessToModifiedClosure
                if (handler != null)
                    grid.Loaded -= handler;
                // ReSharper restore AccessToModifiedClosure
                
                if (grid.VerticalOffset.CompareTo(grid.ScrollViewer.VerticalOffset) != 0)
                {
                    await Task.Delay(1);
                    grid.ScrollViewer.ChangeView(null, grid.VerticalOffset, null, true);
                }
            };
            handler = (s, ee) => action();

            if (grid.ScrollViewer == null)
                grid.Loaded += handler;
            else
                action();
        }
    }
}