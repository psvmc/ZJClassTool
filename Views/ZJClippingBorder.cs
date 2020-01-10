using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZJClassTool.Views
{
    public class ZJClippingBorder : Border
    {
        protected override void OnRender(DrawingContext dc)
        {
            OnApplyChildClip();
            base.OnRender(dc);
        }

        public override UIElement Child
        {
            get
            {
                return base.Child;
            }
            set
            {
                if (Child != value)
                {
                    if (Child != null)
                    {
                        // Restore original clipping
                        Child.SetValue(ClipProperty, _oldClip);
                    }

                    if (value != null)
                    {
                        _oldClip = value.ReadLocalValue(ClipProperty);
                    }
                    else
                    {
                        // If we dont set it to null we could leak a Geometry object
                        _oldClip = null;
                    }

                    base.Child = value;
                }
            }
        }

        protected virtual void OnApplyChildClip()
        {
            UIElement child = Child;
            if (child != null)
            {
                var top = Math.Max(CornerRadius.TopLeft, CornerRadius.TopRight);
                var bottom = Math.Max(CornerRadius.BottomLeft, CornerRadius.BottomRight);
                var max = Math.Max(top, bottom);
                var size = RenderSize;
                var width = size.Width - (BorderThickness.Left + BorderThickness.Right);
                var height = size.Height - (BorderThickness.Top + BorderThickness.Bottom);
                Geometry result = new RectangleGeometry
            (new Rect(0, 0, width, height), max, max);
                double halfWidth = width / 2;
                double halfHeight = height / 2;

                if (CornerRadius.TopLeft == 0)
                {
                    result = new CombinedGeometry(
                        GeometryCombineMode.Union,
                        result,
                        new RectangleGeometry(new Rect(0, 0, halfWidth, halfHeight))
                    );
                }

                if (CornerRadius.TopRight == 0)
                {
                    result = new CombinedGeometry(GeometryCombineMode.Union, result, new RectangleGeometry
                (new Rect(halfWidth, 0, halfWidth, halfHeight)));
                }

                if (CornerRadius.BottomLeft == 0)
                {
                    result = new CombinedGeometry
                  (GeometryCombineMode.Union, result, new RectangleGeometry
                  (new Rect(0, halfHeight, halfWidth, halfHeight)));
                }
                if (CornerRadius.BottomRight == 0)
                {
                    result = new CombinedGeometry
                  (GeometryCombineMode.Union, result, new RectangleGeometry
                  (new Rect(halfWidth, halfHeight, halfWidth, halfHeight)));
                }
                child.Clip = result;
            }
        }

        public void Update()
        {
            OnApplyChildClip();
        }

        private RectangleGeometry _clipRect = new RectangleGeometry();
        private object _oldClip;
    }
}