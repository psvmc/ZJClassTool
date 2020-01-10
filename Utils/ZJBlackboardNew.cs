using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace ZJClassTool.Utils
{
    internal enum ZPenType : byte
    {
        Pen = 1,
        Erase = 2
    };

    internal class ZBBPage
    {
        public List<ZBBPageStep> lines { get; set; }
        public List<ZBBPageStep> lines_histoty { get; set; }

        public ZBBPage()
        {
            lines = new List<ZBBPageStep>();
            lines_histoty = new List<ZBBPageStep>();
        }
    }

    internal class ZBBPageStep
    {
        public StrokeCollection lines_curr { get; set; }
        public StrokeCollection lines_add { get; set; }
        public StrokeCollection lines_remove { get; set; }

        public ZBBPageStep()
        {
            lines_curr = new StrokeCollection();
            lines_add = new StrokeCollection();
            lines_remove = new StrokeCollection();
        }
    }

    public class ZJBlackboardNew
    {
        private InkCanvas m_canvas;
        private ZPenType type = ZPenType.Pen;
        private int pagenum = 0;
        private int erasesize = 64;
        private int pensize = 3;
        private int undoOrRedo = 0; //是否在进行撤销恢复操作

        private List<ZBBPage> strokes_page_all = new List<ZBBPage>();

        // 添加这个变量是因为在用橡皮擦时 一次操作会触发多次StrokesChanged回掉 这里是把多次回掉合并在一起
        private ZBBPageStep step = null;

        public ZJBlackboardNew(InkCanvas canvas)
        {
            this.m_canvas = canvas;
            var page = new ZBBPage();
            page.lines.Add(new ZBBPageStep());
            strokes_page_all.Add(page);
            if (canvas != null)
            {
                canvas.EditingMode = InkCanvasEditingMode.Ink;
                DrawingAttributes drawingAttributes = new DrawingAttributes();
                canvas.DefaultDrawingAttributes = drawingAttributes;
                drawingAttributes.Color = Colors.White;
                drawingAttributes.FitToCurve = true;
                drawingAttributes.IgnorePressure = false;
                canvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
                canvas.StrokeCollected += Canvas_StrokeCollected;
                canvas.StrokeErasing += Canvas_StrokeErasing;
                canvas.StrokeErased += Canvas_StrokeErased;
                canvas.MouseUp += Canvas_MouseUp;
            }
        }

        private void Canvas_StrokeErasing(object sender, InkCanvasStrokeErasingEventArgs e)
        {
            undoOrRedo = 0;
        }

        private void Canvas_StrokeErased(object sender, RoutedEventArgs e)
        {
        }

        private void Canvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (step != null)
            {
                var page = strokes_page_all[pagenum];
                if (page != null)
                {
                    step.lines_curr.Add(m_canvas.Strokes);
                    page.lines.Add(step);
                    step = null;
                }
            }
        }

        private void Strokes_StrokesChanged(object sender, StrokeCollectionChangedEventArgs e)
        {
            if (undoOrRedo > 0)
            {
                undoOrRedo -= 1;
                return;
            }

            if (step == null)
            {
                step = new ZBBPageStep();
            }

            // 笔模式
            if (e.Added.Count > 0 && e.Removed.Count == 0)
            {
                step.lines_add.Add(e.Added);
            }
            // 橡皮模式 会多次进入回掉
            else if (e.Removed.Count > 0)
            {
                step.lines_add.Add(e.Added);
                for (int i = 0; i < e.Removed.Count; i++)
                {
                    var removeItem = e.Removed[i];
                    if (step.lines_add.Contains(removeItem))
                    {
                        step.lines_add.Remove(removeItem);
                    }
                    else
                    {
                        step.lines_remove.Add(removeItem);
                    }
                }
            }
        }

        // public方法
        // 笔
        public void change_pen()
        {
            this.type = ZPenType.Pen;
            DrawingAttributes drawingAttributes = new DrawingAttributes();
            m_canvas.DefaultDrawingAttributes = drawingAttributes;
            drawingAttributes.Color = Colors.White;
            drawingAttributes.Width = this.pensize;
            drawingAttributes.Height = this.pensize;
            drawingAttributes.FitToCurve = true;
            drawingAttributes.IgnorePressure = false;
            m_canvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        // 橡皮
        public void change_erase()
        {
            this.type = ZPenType.Erase;

            DrawingAttributes drawingAttributes = new DrawingAttributes();
            m_canvas.DefaultDrawingAttributes = drawingAttributes;
            drawingAttributes.Color = Colors.White;
            drawingAttributes.Width = this.erasesize;
            drawingAttributes.Height = this.erasesize;
            drawingAttributes.FitToCurve = true;
            drawingAttributes.IgnorePressure = false;

            m_canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        // 撤销
        public void undo()
        {
            var page = strokes_page_all[this.pagenum];

            if (page != null && m_canvas.Strokes.Count > 0 && page.lines.Count > 1)
            {
                var last = page.lines.Last();
                page.lines.Remove(last);
                page.lines_histoty.Add(last);
                if (page.lines.Last().lines_curr.Count > 0)
                {
                    undoOrRedo = 2;
                }
                else
                {
                    undoOrRedo = 1;
                }

                m_canvas.Strokes.Clear();
                m_canvas.Strokes.Add(page.lines.Last().lines_curr);
            }
        }

        // 恢复
        public void redo()
        {
            var page = strokes_page_all[this.pagenum];
            if (page != null && page.lines_histoty.Count > 0)
            {
                var line = page.lines_histoty[page.lines_histoty.Count - 1];

                page.lines.Add(line);
                page.lines_histoty.Remove(line);
                if (page.lines.Last().lines_curr.Count > 0)
                {
                    undoOrRedo = 2;
                }
                else
                {
                    undoOrRedo = 1;
                }
                m_canvas.Strokes.Clear();
                m_canvas.Strokes.Add(page.lines.Last().lines_curr);
            }
        }

        // 清空
        public void clear()
        {
            var page = strokes_page_all[this.pagenum];
            if (page != null)
            {
                m_canvas.Strokes.Clear();
                page.lines_histoty.Clear();
                page.lines.Clear();
                page.lines.Add(new ZBBPageStep());
            }
        }
    }
}