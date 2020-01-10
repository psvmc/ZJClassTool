using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZJClassTool.Views
{
    public class ZJListBox : ListBox
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MyListBoxItem();
        }
    }

    public class MyListBoxItem : ListBoxItem
    {
        protected override void OnSelected(System.Windows.RoutedEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListBoxItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null) { return; }

            ListBoxItem item = (ListBoxItem)dep;
            if (item.IsSelected)
            {
                item.IsSelected = !item.IsSelected;
            }
            base.OnSelected(e);
        }
    }
}