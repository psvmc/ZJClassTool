using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZJClassTool.Wins
{
    /// <summary>
    /// PracticeWin.xaml 的交互逻辑
    /// </summary>
    public partial class PracticeWin : Window
    {
        internal double pwidth = SystemParameters.PrimaryScreenWidth;

        internal double pHeight = SystemParameters.PrimaryScreenHeight;

        public PracticeWin()
        {
            InitializeComponent();
            initImage();
            initEvent();
        }

        private void initEvent()
        {
        }

        private void initImage()
        {
            //100%的时候，DPI是96；这条语句的作用时获取放大比例
            float factor = Graphics.FromHwnd(IntPtr.Zero).DpiX / 96;
            Console.WriteLine("factor:" + factor);
            int imageW = Convert.ToInt32(pwidth * factor);
            int imageH = Convert.ToInt32(pHeight * factor);

            Bitmap image = new Bitmap(imageW, imageH, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(image);
            g.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(imageW, imageH), CopyPixelOperation.SourceCopy);

            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                image.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );
            m_image.Source = bitmapSource;
        }

        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}