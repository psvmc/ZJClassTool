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

namespace ZJClassTool
{
    /// <summary>
    /// Blackboard.xaml 的交互逻辑
    /// </summary>
    public partial class Blackboard : Window
    {
        public Blackboard()
        {
            InitializeComponent();
            this.blackboard_canvas.MouseDown += new MouseButtonEventHandler(aBtn_Click);
            this.blackboard_canvas.MouseMove += new MouseEventHandler(aBtn_Click);
            drawpath();
        }

        private void aBtn_Click(object sender, MouseEventArgs e)
        {
            textbox.Text = "fsdfsd";
        }
        private void drawpath()
        {
            Polyline curvePolyline = new Polyline();

            curvePolyline.Stroke = Brushes.Green;
            curvePolyline.StrokeThickness = 2;

            var Points = new PointCollection();
            Points.Add(new Point(0, 0));
            Points.Add(new Point(100, 0));
            Points.Add(new Point(100, 100));
            Points.Add(new Point(200, 200));

            curvePolyline.Points = Points;
            this.blackboard_canvas.Children.Add(curvePolyline);
        }

        private void close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
