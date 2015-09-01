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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PopcornTime.Extensions;

namespace PopcornTime.Common
{
    public class BetterListView : ListView
    {
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset", typeof (double), typeof (BetterListView),
                new PropertyMetadata(0, VerticalOffsetPropertyChanged));

        public static readonly DependencyProperty BetterSelectedIndexProperty =
            DependencyProperty.RegisterAttached("BetterSelectedIndex", typeof (int), typeof (BetterListView),
                new PropertyMetadata(0, BetterSelectedIndexPropertyChanged));

        private ScrollViewer _scroll;

        public BetterListView()
        {
            Loaded += (s, e) =>
            {
                ScrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
                ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
                SelectionChanged -= OnSelectionChanged;
                SelectionChanged += OnSelectionChanged;
            };

            Unloaded += (s, e) =>
            {
                ScrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
                SelectionChanged -= OnSelectionChanged;
            };
        }

        public ScrollViewer ScrollViewer => _scroll ?? (_scroll = this.GetScrollViewer());

        public double VerticalOffset
        {
            get { return (double) GetValue(VerticalOffsetProperty); }

            set { SetValue(VerticalOffsetProperty, value); }
        }

        public int BetterSelectedIndex
        {
            get { return (int) GetValue(BetterSelectedIndexProperty); }

            set { SetValue(BetterSelectedIndexProperty, value); }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            BetterSelectedIndex = SelectedIndex;
        }

        private static void BetterSelectedIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var list = (BetterListView) d;

            RoutedEventHandler handler = null;
            Action action = () =>
            {
                // ReSharper disable AccessToModifiedClosure
                if (handler != null)
                    list.Loaded -= handler;
                // ReSharper restore AccessToModifiedClosure

                list.SelectedIndex = (int) e.NewValue;
            };
            handler = (s, ee) => action();

            if (list.ScrollViewer == null)
                list.Loaded += handler;
            else
                action();
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            VerticalOffset = ScrollViewer.VerticalOffset;
        }

        private static void VerticalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var list = (BetterListView) d;

            RoutedEventHandler handler = null;
            Action action = () =>
            {
                // ReSharper disable AccessToModifiedClosure
                if (handler != null)
                    list.Loaded -= handler;
                // ReSharper restore AccessToModifiedClosure

                if (list.VerticalOffset != list.ScrollViewer.VerticalOffset)
                    list.ScrollViewer.ChangeView(null, (double) e.NewValue, null, true);
            };
            handler = (s, ee) => action();

            if (list.ScrollViewer == null)
                list.Loaded += handler;
            else
                action();
        }
    }
}