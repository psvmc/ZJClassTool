using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Blackboard.xaml 的交互逻辑
    /// </summary>
    public partial class BlackboardWin : Window
    {
        BPageModel pageData = new BPageModel();
        ZJBlackboard myblackboard;
        public BlackboardWin()
        {
            InitializeComponent();

            myblackboard = new ZJBlackboard(this.blackboard_canvas, erase_img, null);
            this.initData();
        }

        void initData()
        {
            pageData.IsRight = true;
            pageData.menuList.Add(new BMenu()
            {
                Name = "铅笔",
                Pic = "../Images/Blackboard/class_2s.png",
                Selected = true
            });
            pageData.menuList.Add(new BMenu()
            {
                Name = "橡皮",
                Pic = "../Images/Blackboard/class_3un.png",
                Selected = false

            });
            pageData.menuList.Add(new BMenu()
            {
                Name = "清空",
                Pic = "../Images/Blackboard/class_6un.png",
                Selected = false

            });
            pageData.menuList.Add(new BMenu()
            {
                Name = "撤销",
                Pic = "../Images/Blackboard/class_4un.png",
                Selected = false

            });
            pageData.menuList.Add(new BMenu()
            {
                Name = "恢复",
                Pic = "../Images/Blackboard/class_5un.png",
                Selected = false

            });

            pageData.menuList.Add(new BMenu()
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
            var buttons = VTHelper.FindChilds<Button>(toolbar_list, "toolbar_item");
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
                    this.Close();
                }
            }
        }
    }

    public class BPageModel : ZJNotifyModel
    {
        public ObservableCollection<BMenu> menuList { get; set; }
        bool _IsRight = true;
        public bool IsRight
        {
            get { return _IsRight; }
            set { _IsRight = value; OnPropertyChanged("IsRight"); }
        }
        public BPageModel()
        {
            menuList = new ObservableCollection<BMenu>();
        }
    }

    public class BMenu : ZJNotifyModel
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        string _Pic;
        public string Pic
        {
            get { return _Pic; }
            set { _Pic = value; OnPropertyChanged("Pic"); }
        }

        bool _Selected;
        public bool Selected
        {
            get { return _Selected; }
            set { _Selected = value; OnPropertyChanged("Selected"); }
        }
    }
}
