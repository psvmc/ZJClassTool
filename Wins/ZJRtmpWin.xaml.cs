namespace ZJClassTool.Wins
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using ZJClassTool.Utils;

    /// <summary>
    /// Defines the <see cref="ZJRtmpWin"/>
    /// </summary>
    public partial class ZJRtmpWin : Window
    {
        /// <summary>
        /// Defines the rtmpModel
        /// </summary>
        internal RtmpModel rtmpModel = new RtmpModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="ZJRtmpWin"/> class.
        /// </summary>
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
            var audiolist = ZJAudioModel.getAudioDevice();
            for (int i = 0; i < audiolist.Count; i++)
            {
                var item = audiolist[i];
                rtmpModel.MYAudioDevices.Add(item);
            }
            DataContext = rtmpModel;

            if (audiolist.Count > 0)
            {
                audio_list_box.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// The close_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/></param>
        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The start_Click
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/></param>
        private void start_Click(object sender, RoutedEventArgs e)
        {
            if (rtmpModel.IsStart)
            {
                ZJRtmpPush.Stop();

                rtmpModel.IsStart = false;
            }
            else
            {
                var selectIndex = audio_list_box.SelectedIndex;
                if (selectIndex >= 0 && selectIndex < rtmpModel.MYAudioDevices.Count)
                {
                    var name = rtmpModel.MYAudioDevices[selectIndex].name;
                    ZJRtmpPush.StartPush(name, "rtmp://live.xhkjedu.com/tt/01");
                    rtmpModel.IsStart = true;
                }
                else
                {
                    MessageBox.Show("没有可用的音频输入设备");
                }
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (rtmpModel.IsStart)
            {
                ZJRtmpPush.Stop();

                rtmpModel.IsStart = false;
            }
        }
    }

    /// <summary>
    /// Defines the <see cref="RtmpModel" />
    /// </summary>
    public class RtmpModel : ZJNotifyModel
    {
        /// <summary>
        /// Defines the _IsStart
        /// </summary>
        private bool _IsStart;

        /// <summary>
        /// Gets or sets the MYAudioDevices
        /// </summary>
        public ObservableCollection<ZJAudioModel> MYAudioDevices { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsStart
        /// </summary>
        public bool IsStart
        {
            get { return _IsStart; }
            set { _IsStart = value; OnPropertyChanged("IsStart"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RtmpModel"/> class.
        /// </summary>
        public RtmpModel()
        {
            _IsStart = false;
            MYAudioDevices = new ObservableCollection<ZJAudioModel>();
        }
    }
}