using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZJClassTool.Utils;

namespace ZJClassTool.Wins
{
    /// <summary>
    /// Blackboard.xaml 的交互逻辑
    /// </summary>
    public partial class ZBlackboardWin : Window
    {
        private ZBPageModel pageData = new ZBPageModel();
        private ZJBlackboardNew myblackboard;

        public ZBlackboardWin()
        {
            InitializeComponent();

            myblackboard = new ZJBlackboardNew(this.blackboard_canvas);
            this.initData();
        }

        private void initData()
        {
            pageData.pagenum = 1;
            pageData.currpage = 1;
            pageData.menuList.Add(new ZBMenu()
            {
                Name = "铅笔",
                Pic = "../Images/Blackboard/class_2s.png",
                Selected = true
            });
            pageData.menuList.Add(new ZBMenu()
            {
                Name = "橡皮",
                Pic = "../Images/Blackboard/class_3un.png",
                Selected = false
            });
            pageData.menuList.Add(new ZBMenu()
            {
                Name = "清空",
                Pic = "../Images/Blackboard/class_6un.png",
                Selected = false
            });
            pageData.menuList.Add(new ZBMenu()
            {
                Name = "撤销",
                Pic = "../Images/Blackboard/class_4un.png",
                Selected = false
            });
            pageData.menuList.Add(new ZBMenu()
            {
                Name = "恢复",
                Pic = "../Images/Blackboard/class_5un.png",
                Selected = false
            });

            pageData.menuList.Add(new ZBMenu()
            {
                Name = "PPT",
                Pic = "../Images/Blackboard/class_7un.png",
                Selected = false
            });

            DataContext = pageData;
        }

        private void menu_item_Click(object sender, RoutedEventArgs e)
        {
            var clickindex = 0;
            var buttons = ZJVTHelper.FindChilds<Button>(toolbar_list, "toolbar_item");
            for (var i = 0; i < buttons.Count; i++)
            {
                if (buttons[i] == sender)
                {
                    clickindex = i;
                    break;
                }
            }
            if (clickindex < pageData.menuList.Count)
            {
                var menudata = pageData.menuList[clickindex];
                if (clickindex == 0)
                {
                    pageData.menuList[0].Selected = true;
                    pageData.menuList[1].Selected = false;
                    pageData.menuList[0].Pic = "../Images/Blackboard/class_2s.png";
                    pageData.menuList[1].Pic = "../Images/Blackboard/class_3un.png";
                    myblackboard.change_pen();
                }
                else if (clickindex == 1)
                {
                    pageData.menuList[0].Selected = false;
                    pageData.menuList[1].Selected = true;
                    pageData.menuList[0].Pic = "../Images/Blackboard/class_2un.png";
                    pageData.menuList[1].Pic = "../Images/Blackboard/class_3s.png";
                    myblackboard.change_erase();
                }
                else if (clickindex == 2)
                {
                    myblackboard.clear();
                }
                else if (clickindex == 3)
                {
                    myblackboard.undo();
                }
                else if (clickindex == 4)
                {
                    myblackboard.redo();
                }
                else if (clickindex == 5)
                {
                    this.Hide();
                }
            }
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string docpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                String imagepath = Path.Combine(docpath, "classtools", "images");

                if (!Directory.Exists(imagepath))
                {
                    Directory.CreateDirectory(imagepath);
                }
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                string timestr = Convert.ToInt64(ts.TotalMilliseconds).ToString();
                string filepath = Path.Combine(imagepath, timestr + ".jpg");
                int width = (int)Math.Round(blackboard_canvas.ActualWidth);
                int height = (int)Math.Round(blackboard_canvas.ActualHeight);
                RenderTargetBitmap bmpCopied = new RenderTargetBitmap(
                        width,
                        height,
                        96,
                        96,
                        PixelFormats.Default
                    );
                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(blackboard_canvas);
                    dc.DrawRectangle(
                        vb,
                        null,
                        new Rect(new System.Windows.Point(), new System.Windows.Size(width, height))
                        );
                }
                bmpCopied.Render(dv);
                using (FileStream file = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.QualityLevel = 80;
                    encoder.Frames.Add(BitmapFrame.Create(bmpCopied));
                    encoder.Save(file);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            pageData.pagenum += 1;
            pageData.currpage = pageData.pagenum;
            myblackboard.changepage(pageData.currpage - 1);
        }

        private void last_button_Click(object sender, RoutedEventArgs e)
        {
            if (pageData.currpage > 1)
            {
                pageData.currpage -= 1;
                myblackboard.changepage(pageData.currpage - 1);
            }
        }

        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            if (pageData.currpage < pageData.pagenum)
            {
                pageData.currpage += 1;
                myblackboard.changepage(pageData.currpage - 1);
            }
        }
    }

    public class ZBPageModel : ZJNotifyModel
    {
        public ObservableCollection<ZBMenu> menuList { get; set; }
        private int _pagenum = 1;

        public int pagenum
        {
            get { return _pagenum; }
            set { _pagenum = value; OnPropertyChanged("pagenum"); }
        }

        private int _currpage = 1;// 从1开始

        public int currpage
        {
            get { return _currpage; }
            set { _currpage = value; OnPropertyChanged("currpage"); }
        }

        public ZBPageModel()
        {
            menuList = new ObservableCollection<ZBMenu>();
        }
    }

    public class ZBMenu : ZJNotifyModel
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        private string _Pic;

        public string Pic
        {
            get { return _Pic; }
            set { _Pic = value; OnPropertyChanged("Pic"); }
        }

        private bool _Selected;

        public bool Selected
        {
            get { return _Selected; }
            set { _Selected = value; OnPropertyChanged("Selected"); }
        }
    }
}