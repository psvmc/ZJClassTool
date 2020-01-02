using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
