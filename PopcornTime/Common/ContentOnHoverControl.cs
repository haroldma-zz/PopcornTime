using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace PopcornTime.Common
{
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "PointerOver", GroupName = "CommonStates")]
    internal class ContentOnHoverControl : ContentControl
    {
        protected override void OnApplyTemplate()
        {
            VisualStateManager.GoToState(this, "Normal", true);
            base.OnApplyTemplate();
            PointerEntered += OnPointerEntered;
            PointerExited += OnPointerExited;
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            VisualStateManager.GoToState(this, "PointerOver", true);
        }
    }
}