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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZJClassTool.Utils;
using System.Collections.ObjectModel;
using WebSocket4Net;

namespace ZJClassTool.Wins
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ToolbarWin : Window
    {

        double pwidth = SystemParameters.PrimaryScreenWidth;
        double pHeight = SystemParameters.PrimaryScreenHeight;
        ToolbarLeftWin leftBar;
        ToolbarRightWin rightBar;
        ToolbarModel pageData = new ToolbarModel();
        public ToolbarWin()
        {
            InitializeComponent();
            this.Topmost = true;
            this.Height = 602;
            this.Left = pwidth - 60;
            this.Top = (pHeight - 602 - 48) / 2;

            pageData.IsRight = true;
            pageData.menuList.Add(new ToolbarMenu()
            {
                Name = "下课",
                Pic = "../Images/ToolBar/toobar_1.png"

            });
            pageData.menuList.Add(new ToolbarMenu()
            {
                Name = "课件",
                Pic = "../Images/ToolBar/toobar_2.png"

            });
            pageData.menuList.Add(new ToolbarMenu()
            {
                Name = "黑板",
                Pic = "../Images/ToolBar/toobar_3.png"

            });
            pageData.menuList.Add(new ToolbarMenu()
            {
                Name = "演板",
                Pic = "../Images/ToolBar/toolbar_13.png"

            });
            pageData.menuList.Add(new ToolbarMenu()
            {
                Name = "训练",
                Pic = "../Images/ToolBar/toobar_4.png"

            });
            pageData.menuList.Add(new ToolbarMenu()
            {
                Name = "抢答",
                Pic = "../Images/ToolBar/toobar_5.png"

            });
            pageData.menuList.Add(new ToolbarMenu()
            {
                Name = "点名",
                Pic = "../Images/ToolBar/toobar_6.png"

            });
            pageData.menuList.Add(new ToolbarMenu()
            {
                Name = "息屏",
                Pic = "../Images/ToolBar/toobar_10.png"

            });
            pageData.menuList.Add(new ToolbarMenu()
            {
                Name = "开始直播",
                Pic = "../Images/ToolBar/toobar_12_1.png"

            });
            DataContext = pageData;
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
        }
        public void left_bar_click()
        {
            pageData.IsRight = false;
            this.Left = 0;
            leftBar.Hide();
            rightBar.Show();
            if (this.Visibility == Visibility.Hidden)
            {
                this.Show();
            }
        }


        public void right_bar_click()
        {
            pageData.IsRight = true;
            rightBar.Hide();
            leftBar.Show();
            this.Left = pwidth - 60;
            if (this.Visibility == Visibility.Hidden)
            {
                this.Show();
            }
        }

        private void toolbar_win_Loaded(object sender, RoutedEventArgs e)
        {
            leftBar = new ToolbarLeftWin();
            leftBar.Topmost = true;
            leftBar.Left = 0;
            leftBar.Top = (pHeight - 50 - 48) / 2;
            leftBar.Owner = this;
            leftBar.Show();


            rightBar = new ToolbarRightWin();
            rightBar.Topmost = true;
            rightBar.Left = pwidth - 30;
            rightBar.Top = (pHeight - 50 - 48) / 2;
            rightBar.Owner = this;
        }

        private void toolbar_item_Click(object sender, RoutedEventArgs e)
        {
            var clickindex = 0;
            var buttons = VTHelper.FindChilds<Button>(toolbar_list, "toolbar_item");
            for (var i = 0; i < buttons.Count; i++)
            {
                if (buttons[i] == sender)
                {
                    clickindex = i;
                    break;
                }
            }
            var item = pageData.menuList[clickindex];
            if (clickindex == 0)
            {
                this.Close();
            }
            else if (clickindex == 2)
            {
                var blockboard = new BlackboardWin();
                blockboard.Topmost = true;
                blockboard.Width = pwidth;
                blockboard.Height = pHeight;
                blockboard.Left = 0;
                blockboard.Top = 0;
                blockboard.ShowDialog();
                blockboard.Owner = this;
            }
            else if (clickindex == 3)
            {
                var blockboard = new ZBlackboardWin();
                blockboard.Topmost = true;
                blockboard.Width = pwidth;
                blockboard.Height = pHeight;
                blockboard.Left = 0;
                blockboard.Top = 0;
                blockboard.ShowDialog();
                blockboard.Owner = this;
            }
            else if (clickindex == 7)
            {
                if (item.Name == "熄屏")
                {

                    item.Name = "恢复";
                }
                else
                {
                    item.Name = "熄屏";
                }
            }
            else if (clickindex == 8)
            {


                var rtmpWin = new ZJRtmpWin();
                rtmpWin.Topmost = true;
                rtmpWin.Width = 300;
                rtmpWin.Height = 200;
                rtmpWin.ShowDialog();
                rtmpWin.Owner = this;
            }
        }

        private void top_button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                this.Visibility = Visibility.Hidden;
                leftBar.Show();
                rightBar.Show();
            }

        }


        // WebSocket
        WebSocket websocket = null;
        //websocket连接
        private void connectWebsocket()
        {
            websocket = new WebSocket("");
            websocket.Opened += websocket_Opened;
            websocket.Closed += websocket_Closed;
            websocket.MessageReceived += websocket_MessageReceived;
            websocket.Open();
        }
        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            //接收服务端发来的消息
            MessageReceivedEventArgs responseMsg = (MessageReceivedEventArgs)e;
            string strMsg = responseMsg.Message;

        }

        private void websocket_Closed(object sender, EventArgs e)
        {

        }

        private void websocket_Opened(object sender, EventArgs e)
        {

        }

        private void sendmsg(string msg)
        {
            if (websocket.State == WebSocketState.Open)
            {
                websocket.Send(msg);
            }

        }


    }


    public class ToolbarModel : ZJNotifyModel
    {
        public ObservableCollection<ToolbarMenu> menuList { get; set; }
        bool _IsRight = true;
        public bool IsRight
        {
            get { return _IsRight; }
            set { _IsRight = value; OnPropertyChanged("IsRight"); }
        }
        public ToolbarModel()
        {
            menuList = new ObservableCollection<ToolbarMenu>();
        }
    }

    public class ToolbarMenu : ZJNotifyModel
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public string Pic { get; set; }
    }
}
