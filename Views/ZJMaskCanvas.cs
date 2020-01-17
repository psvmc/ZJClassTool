using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ZJClassTool.Views
{
    internal class ZJMaskCanvas : Canvas
    {
        private readonly System.Windows.Shapes.Rectangle selectionBorder = new System.Windows.Shapes.Rectangle();
        private Thumb thumb = new Thumb();

        public ZJMaskCanvas()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            selectionBorder.Stroke = Brushes.LightBlue;
            selectionBorder.StrokeThickness = 2;
            SetLeft(selectionBorder, 100);
            SetTop(selectionBorder, 100);
            selectionBorder.Width = 100;
            selectionBorder.Height = 100;
            Children.Add(selectionBorder);

            thumb.Background = Brushes.LightBlue;
            SetLeft(thumb, 300);
            SetTop(thumb, 100);
            thumb.Width = 100;
            thumb.Height = 100;
            Children.Add(thumb);
        }
    }
}