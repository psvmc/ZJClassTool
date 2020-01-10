using System.Windows;

namespace ZJClassTool.Wins
{
    /// <summary>
    /// ToolbarLeft.xaml 的交互逻辑
    /// </summary>
    public partial class ToolbarLeftWin : Window
    {
        public ToolbarLeftWin()
        {
            InitializeComponent();
        }

        private void leftbar_Click(object sender, RoutedEventArgs e)
        {
            var toolbarWin = (ToolbarWin)this.Owner;
            toolbarWin.left_bar_click();
        }
    }
}