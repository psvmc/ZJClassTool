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
using ZJClassTool.Utils;

namespace ZJClassTool.Wins
{
    /// <summary>
    /// ZJRtmpWin.xaml 的交互逻辑
    /// </summary>
    public partial class ZJRtmpWin : Window
    {
        RtmpModel rtmpModel = new RtmpModel();
        public ZJRtmpWin()
        {
            InitializeComponent();
            if (ZJRtmpPush.p.StartInfo.Arguments != "")
            {
                rtmpModel.IsStart = true;
            }
            else
            {
                rtmpModel.IsStart = false;
            }
            DataContext = rtmpModel;
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            if (rtmpModel.IsStart)
            {
                ZJRtmpPush.Stop();
                rtmpModel.IsStart = false;
            }
            else
            {
                var audiolist = ZJAudioModel.getAudioDevice();
                foreach (var item in audiolist)
                {
                    Console.WriteLine(item.name);
                }
                ZJRtmpPush.StartPush("Internal Microphone (Cirrus Logic CS8409 (AB 51))", "rtmp://live.xhkjedu.com/tt/01");
                rtmpModel.IsStart = true;
            }

        }
    }

    public class RtmpModel:ZJNotifyModel
    {
        private bool _IsStart;
        public bool IsStart
        {
            get { return _IsStart; }
            set { _IsStart = value; OnPropertyChanged("IsStart"); }
        }

    }
}
