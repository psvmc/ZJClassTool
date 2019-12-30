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

namespace ZJClassTool
{
    /// <summary>
    /// ToolbarLeft.xaml 的交互逻辑
    /// </summary>
    public partial class ToolbarLeft : Window
    {
        public ToolbarLeft()
        {
            InitializeComponent();
        }

        private void leftbar_Click(object sender, RoutedEventArgs e)
        {
            var toolbarWin = (MainWindow)this.Owner;
            toolbarWin.left_bar_click();
        }
    }
}
