using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace ZJClassTool.Utils
{
    internal enum PenType : byte
    {
        Pen = 1,
        Erase = 2
    };

    internal class ZJBBPage
    {
        public List<Polyline> polylines { get; set; }
        public List<Polyline> undoHistory { get; set; }

        public ZJBBPage()
        {
            polylines = new List<Polyline>();
            undoHistory = new List<Polyline>();
        }
    }

    public class ZJBlackboard
    {
        private Canvas m_canvas;
        private Image m_erase_img;
        private PenType type = PenType.Pen;
        private int page = 0;
        private int erasesize = 32;

        private TextBox m_textbox;

        // 只在mouse事件时生效
        private bool sketching = false;

        // 定义下面的变量来解决在部分电脑上touch同时也会触发mouse事件
        private bool istouch = false;

        private bool ismouse = false;

        private List<ZJBBPage> strokes_page_all = new List<ZJBBPage>();

        public ZJBlackboard(Canvas canvas, Image image, TextBox textbox)
        {
            this.m_canvas = canvas;
            this.m_erase_img = image;
            this.m_textbox = textbox;

            strokes_page_all.Add(new ZJBBPage());
            if (canvas != null)
            {
                Canvas.SetZIndex(m_erase_img, int.MaxValue);
                canvas.MouseLeftButtonDown += new MouseButtonEventHandler(m_mousedown);
                canvas.MouseMove += new MouseEventHandler(mousemove);
                canvas.MouseLeftButtonUp += new MouseButtonEventHandler(m_mouseup);

                canvas.TouchDown += new EventHandler<TouchEventArgs>(m_touchdown);
                canvas.TouchMove += new EventHandler<TouchEventArgs>(m_touchmove);
                canvas.TouchUp += new EventHandler<TouchEventArgs>(m_touchup);
            }
        }

        private void setCursor()
        {
            if (this.type == PenType.Pen)
            {
                m_canvas.Cursor = Cursors.Arrow;
            }
            else
            {
                StreamResourceInfo sri = Application.GetResourceStream(new Uri(@"cur\erase.cur", UriKind.Relative));
                m_canvas.Cursor = new Cursor(sri.Stream);
            }
        }

        private void m_mousedown(object sender, MouseEventArgs e)
        {
            if (!istouch)
            {
                this.ismouse = true;
                if (ismouse)
                {
                    if (m_textbox != null)
                    {
                        m_textbox.Text += "mousedown" + (e.GetPosition(this.m_canvas).ToString()) + "\n";
                    }

                    if (!this.sketching)
                    {
                        Polyline curvePolyline = new Polyline();
                        if (this.type == PenType.Pen)
                        {
                            curvePolyline.Stroke = Brushes.White;
                            curvePolyline.StrokeThickness = 2;
                        }
                        else
                        {
                            curvePolyline.Stroke = m_canvas.Background;
                            curvePolyline.StrokeThickness = this.erasesize;
                        }

                        curvePolyline.StrokeLineJoin = PenLineJoin.Round;
                        var Points = new PointCollection();
                        Points.Add(e.GetPosition(this.m_canvas));
                        curvePolyline.Points = Points;
                        strokes_page_all[this.page].polylines.Add(curvePolyline);
                        m_canvas.Children.Add(curvePolyline);
                        this.sketching = true;
                    }
                }
            }
        }

        private void mousemove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(this.m_canvas);
            m_erase_img.SetValue(Canvas.LeftProperty, point.X);
            m_erase_img.SetValue(Canvas.TopProperty, point.Y);
            if (m_erase_img != null)
            {
            }
            if (!istouch)
            {
                if (ismouse)
                {
                    if (m_textbox != null)
                    {
                        m_textbox.Text += "mousemove" + (e.GetPosition(this.m_canvas).ToString()) + "\n";
                    }
                    if (this.sketching)
                    {
                        if (strokes_page_all[this.page].polylines.Count > 0)
                        {
                            Polyline curvePolyline = strokes_page_all[this.page].polylines.Last();

                            curvePolyline.Points.Add(point);
                        }
                    }
                }
            }
        }

        private void m_mouseup(object sender, MouseEventArgs e)
        {
            if (!istouch)
            {
                if (ismouse)
                {
                    if (m_textbox != null)
                    {
                        m_textbox.Text += "mouseup" + (e.GetPosition(this.m_canvas).ToString()) + "\n";
                    }
                    this.sketching = false;
                    ismouse = false;
                }
            }
        }

        private Polyline getLine(int tag)
        {
            var polylines = strokes_page_all[this.page].polylines;
            for (int i = polylines.Count - 1; i >= 0; i--)
            {
                if ((int)polylines[i].Tag == tag)
                {
                    return polylines[i];
                }
            }
            return null;
        }

        private void m_touchdown(object sender, TouchEventArgs e)
        {
            istouch = true;
            if (m_textbox != null)
            {
                m_textbox.Text += "m_touchdown" + e.GetTouchPoint(this.m_canvas).Position.ToString() + "\n";
            }
            Polyline curvePolyline = new Polyline();
            curvePolyline.Tag = e.TouchDevice.Id;
            curvePolyline.Stroke = Brushes.Green;
            curvePolyline.StrokeThickness = 2;
            var Points = new PointCollection();
            Points.Add(e.GetTouchPoint(this.m_canvas).Position);
            curvePolyline.Points = Points;
            strokes_page_all[this.page].polylines.Add(curvePolyline);
            m_canvas.Children.Add(curvePolyline);
        }

        private void m_touchmove(object sender, TouchEventArgs e)
        {
            if (m_textbox != null)
            {
                m_textbox.Text += "m_touchmove" + e.GetTouchPoint(this.m_canvas).Position.ToString() + "\n";
            }
            var deviceid = e.TouchDevice.Id;
            var line = getLine(deviceid);
            if (line != null)
            {
                line.Points.Add(e.GetTouchPoint(this.m_canvas).Position);
            }
        }

        private void m_touchup(object sender, TouchEventArgs e)
        {
            if (m_textbox != null)
            {
                m_textbox.Text += "m_touchup" + e.GetTouchPoint(this.m_canvas).Position.ToString() + "\n";
            }
            istouch = false;
        }

        // public方法

        // 笔
        public void change_pen()
        {
            this.type = PenType.Pen;
            setCursor();
        }

        // 橡皮
        public void change_erase()
        {
            this.type = PenType.Erase;
            setCursor();
        }

        // 撤销
        public void undo()
        {
            var page = strokes_page_all[this.page];
            if (page != null && page.polylines.Count > 0)
            {
                var lastline = page.polylines.Last();
                page.polylines.Remove(lastline);
                page.undoHistory.Add(lastline);
                m_canvas.Children.Remove(lastline);
            }
        }

        // 恢复
        public void redo()
        {
            var page = strokes_page_all[this.page];
            if (page != null && page.undoHistory.Count > 0)
            {
                var lastline = page.undoHistory.Last();
                page.polylines.Add(lastline);
                page.undoHistory.Remove(lastline);
                m_canvas.Children.Add(lastline);
            }
        }

        // 清空
        public void clear()
        {
            var page = strokes_page_all[this.page];
            if (page != null)
            {
                page.polylines.Clear();
                page.undoHistory.Clear();
                m_canvas.Children.Clear();
            }
        }
    }
}