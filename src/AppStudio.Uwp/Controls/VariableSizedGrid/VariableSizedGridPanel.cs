﻿using System;
using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public class VariableSizedGridPanel : Panel
    {
        private List<Rect> _cells;

        internal bool IsReady { get; set; } = false;

        #region Orientation
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        private static void OrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGridPanel;
            control.InvalidateMeasure();
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(VariableSizedGridPanel), new PropertyMetadata(Orientation.Horizontal, OrientationChanged));
        #endregion

        #region MaximumRowsOrColumns
        public int MaximumRowsOrColumns
        {
            get { return (int)GetValue(MaximumRowsOrColumnsProperty); }
            set { SetValue(MaximumRowsOrColumnsProperty, value); }
        }

        private static void MaximumRowsOrColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGridPanel;
            control.InvalidateMeasure();
        }

        public static readonly DependencyProperty MaximumRowsOrColumnsProperty = DependencyProperty.Register("MaximumRowsOrColumns", typeof(int), typeof(VariableSizedGridPanel), new PropertyMetadata(0, MaximumRowsOrColumnsChanged));
        #endregion

        #region AspectRatio
        public double AspectRatio
        {
            get { return (double)GetValue(AspectRatioProperty); }
            set { SetValue(AspectRatioProperty, value); }
        }

        private static void AspectRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGridPanel;
            control.InvalidateMeasure();
        }

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(VariableSizedGridPanel), new PropertyMetadata(1.0, AspectRatioChanged));
        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.IsReady && base.Children.Count > 0)
            {
                _cells = new List<Rect>();

                double sizeWidth = availableSize.Width;
                double sizeHeight = availableSize.Height;

                if (double.IsInfinity(sizeWidth))
                {
                    sizeWidth = Window.Current.Bounds.Width;
                }
                if (double.IsInfinity(sizeHeight))
                {
                    sizeHeight = Window.Current.Bounds.Height;
                }

                double cw = sizeWidth / this.MaximumRowsOrColumns;
                double ch = cw * this.AspectRatio;
                if (Orientation == Orientation.Vertical)
                {
                    ch = sizeHeight / this.MaximumRowsOrColumns;
                    cw = ch / this.AspectRatio;
                }

                cw = Math.Round(cw);
                ch = Math.Round(ch);

                int n = 0;
                foreach (FrameworkElement item in base.Children)
                {
                    int colSpan = 1;
                    int rowSpan = 1;
                    PrepareItem(n, item, ref colSpan, ref rowSpan);
                    double w = cw * colSpan;
                    double h = ch * rowSpan;
                    GetNextPosition(_cells, new Size(cw, ch), new Size(w, h));
                    item.Measure(new Size(w, h));
                    n++;
                }

                return MeasureSize(_cells);
            }

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.IsReady && base.Children.Count > 0)
            {
                int n = 0;
                foreach (var item in base.Children)
                {
                    var rect = _cells[n++];
                    item.Arrange(rect);
                }
                return MeasureSize(_cells);
            }
            return base.ArrangeOverride(finalSize);
        }

        private Rect GetNextPosition(List<Rect> cells, Size cellSize, Size itemSize)
        {
            if (Orientation == Orientation.Horizontal)
            {
                for (int y = 0; ; y++)
                {
                    for (int x = 0; x < this.MaximumRowsOrColumns; x++)
                    {
                        var rect = new Rect(new Point(x * cellSize.Width, y * cellSize.Height), itemSize);
                        if (RectFitInCells(rect, cells))
                        {
                            cells.Add(rect);
                            return rect;
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; ; x++)
                {
                    for (int y = 0; y < this.MaximumRowsOrColumns; y++)
                    {
                        var rect = new Rect(new Point(x * cellSize.Width, y * cellSize.Height), itemSize);
                        if (RectFitInCells(rect, cells))
                        {
                            cells.Add(rect);
                            return rect;
                        }
                    }
                }
            }
        }

        private static bool RectFitInCells(Rect rect, List<Rect> cells)
        {
            return !cells.Any(r => !(r.Left >= rect.Right || r.Right <= rect.Left || r.Top >= rect.Bottom || r.Bottom <= rect.Top));
        }

        protected virtual void PrepareItem(int index, UIElement element, ref int colSpan, ref int rowSpan)
        {
            colSpan = index % 3 == 0 ? 2 : 1;
            rowSpan = index % 3 == 0 ? 2 : 1;
        }

        private static Size MeasureSize(List<Rect> cells)
        {
            double mx = cells.Max(r => r.Right);
            double my = cells.Max(r => r.Bottom);
            return new Size(mx, my);
        }
    }
}
