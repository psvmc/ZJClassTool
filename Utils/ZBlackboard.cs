using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ZJClassTool.Utils
{
    enum ZPenType : byte
    {
        Pen = 1,
        Erase = 2
    };
    class ZBBPage
    {
        public Stack<StrokeCollection> polylines { get; set; }
        public Stack<StrokeCollection> undoHistory { get; set; }
        public ZBBPage()
        {
            polylines = new Stack<StrokeCollection>();
            undoHistory = new Stack<StrokeCollection>();
        }
    }

    public class ZBlackboard
    {

        InkCanvas m_canvas;
        Image m_image;
        ZPenType type = ZPenType.Pen;
        int page = 0;
        int erasesize = 64;

        double width;
        double height;

        TextBox m_textbox;

        List<ZBBPage> strokes_page_all = new List<ZBBPage>();

        public ZBlackboard(InkCanvas canvas, Image image, TextBox textbox)
        {
            this.m_canvas = canvas;
            this.m_image = image;
            this.m_textbox = textbox;
            this.width = canvas.Width;
            this.height = canvas.Height;
            strokes_page_all.Add(new ZBBPage());
            if (canvas != null)
            {
                canvas.EditingMode = InkCanvasEditingMode.Ink;
                DrawingAttributes drawingAttributes = new DrawingAttributes();
                //将 InkCanvas 的 DefaultDrawingAttributes 属性的值赋成创建的 DrawingAttributes 类的对象的引用  
                //InkCanvas 通过 DefaultDrawingAttributes 属性来获取墨迹的各种设置，该属性的类型为 DrawingAttributes 型  
                canvas.DefaultDrawingAttributes = drawingAttributes;
                //设置 DrawingAttributes 的 Color 属性设置颜色  
                drawingAttributes.Color = Colors.White;
                drawingAttributes.FitToCurve = true;
                drawingAttributes.IgnorePressure = false;

                canvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
            }
        }

        private void Strokes_StrokesChanged(object sender, System.Windows.Ink.StrokeCollectionChangedEventArgs e)
        {
            if (e.Added.Count > 0)
            {
                var page = strokes_page_all[this.page];
                if (page != null)
                {
                    page.polylines.Push(e.Added);
                }

            }
            else if (e.Removed.Count > 0) {
                var page = strokes_page_all[this.page];
                if (page != null)
                {
                    page.polylines.Pop();
                    page.undoHistory.Push(e.Removed);
                }
            }
            
        }


        // public方法

        // 笔
        public void change_pen() {
            this.type = ZPenType.Pen;
            m_canvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        // 橡皮
        public void change_erase()
        {
            this.type = ZPenType.Erase;
            m_canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        // 撤销
        public void undo()
        {
            var page = strokes_page_all[this.page];
            if (page != null && m_canvas.Strokes.Count > 0)
            {
                m_canvas.Strokes.RemoveAt(m_canvas.Strokes.Count-1);
            }
        }

        // 恢复
        public void redo()
        {
            var page = strokes_page_all[this.page];
            if (page != null && page.undoHistory.Count > 0)
            {
                var line = page.undoHistory.Last();
                m_canvas.Strokes.Add(line);
            }
        }

        // 清空
        public void clear()
        {
            var page = strokes_page_all[this.page];
            if (page != null)
            {
                m_canvas.Strokes.Clear();
            }
        }
    }
}
