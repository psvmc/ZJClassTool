using System.Windows;

namespace ZJClassTool.Wins
{
    /// <summary>
    /// ToolbarRight.xaml 的交互逻辑
    /// </summary>
    public partial class ToolbarRightWin : Window
    {
        public ToolbarRightWin()
        {
            InitializeComponent();
        }

        private void bar_Click(object sender, RoutedEventArgs e)
        {
            var toolbarWin = (ToolbarWin)this.Owner;
            toolbarWin.right_bar_click();
        }
    }
}