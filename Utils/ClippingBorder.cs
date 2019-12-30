using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ZJClassTool.Utils
{
    public class ClippingBorder : Border
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
                if (this.Child != value)
                {
                    if (this.Child != null)
                    {
                        // Restore original clipping
                        this.Child.SetValue(UIElement.ClipProperty, _oldClip);
                    }

                    if (value != null)
                    {
                        _oldClip = value.ReadLocalValue(UIElement.ClipProperty);
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
            UIElement child = this.Child;
            if (child != null)
            {
                var top = Math.Max(this.CornerRadius.TopLeft, this.CornerRadius.TopRight);
                var bottom = Math.Max(this.CornerRadius.BottomLeft, this.CornerRadius.BottomRight);
                var max = Math.Max(top,bottom);
                var size = this.RenderSize;
                var width = size.Width - (this.BorderThickness.Left + this.BorderThickness.Right);
                var height = size.Height - (this.BorderThickness.Top + this.BorderThickness.Bottom);
                Geometry result = new RectangleGeometry
            (new Rect(0, 0, width, height), max, max);
                double halfWidth = width / 2;
                double halfHeight = height / 2;

                if (this.CornerRadius.TopLeft == 0)
                {
                    result = new CombinedGeometry(
                        GeometryCombineMode.Union,
                        result,
                        new RectangleGeometry(new Rect(0, 0, halfWidth, halfHeight))
                    );
                }

                if (this.CornerRadius.TopRight == 0)
                {
                    result = new CombinedGeometry(GeometryCombineMode.Union, result, new RectangleGeometry
                (new Rect(halfWidth, 0, halfWidth, halfHeight)));
                }

                if (this.CornerRadius.BottomLeft == 0)
                {
                    result = new CombinedGeometry
                  (GeometryCombineMode.Union, result, new RectangleGeometry
                  (new Rect(0, halfHeight, halfWidth, halfHeight)));
                }
                if (this.CornerRadius.BottomRight == 0)
                {
                    result = new CombinedGeometry
                  (GeometryCombineMode.Union, result, new RectangleGeometry
                  (new Rect(halfWidth, halfHeight, halfWidth, halfHeight)));
                }


                //_clipRect.RadiusX = _clipRect.RadiusY = Math.Max(0.0, this.CornerRadius.TopLeft - (this.BorderThickness.Left * 0.5));

                //Rect rect = new Rect(this.RenderSize);
                //rect.Height -= (this.BorderThickness.Top + this.BorderThickness.Bottom);
                //rect.Width -= (this.BorderThickness.Left + this.BorderThickness.Right);
                //_clipRect.Rect = rect;
                child.Clip = result;
            }
        }

        public void Update() { OnApplyChildClip(); }

        private RectangleGeometry _clipRect = new RectangleGeometry();
        private object _oldClip;
    }
}
