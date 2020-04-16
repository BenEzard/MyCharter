using System;
using System.Drawing;

namespace MyCharter
{
    public abstract class AbstractScaleAxis : AbstractChartAxis
    {
        /// <summary>
        /// The minimum value to be displayed on the axis.
        /// </summary>
        public object MinimumValue { get; set; }

        /// <summary>
        /// The maximum value to be displayed on the axis.
        /// </summary>
        public object MaximumValue { get; set; }

        public int MajorIncrement { get; set; }

        public int MinorIncrement { get; set; }

        public int PixelsPerIncrement { get; set; } = 10;

        /// <summary>
        /// The Pen used to draw the major ticks.
        /// </summary>
        public Pen MinorTickPen = new Pen(Brushes.Black, 1);

        /// <summary>
        /// The length of the minor tick.
        /// </summary>
        public int MinorTickLength = 3;

        /// <summary>
        /// Instantiate an Axis.
        /// </summary>
        /// <param name="format">The AxisFormat of this axis.</param>
        /// <param name="minimumValue">The minimum value displayed on this axis.</param>
        /// <param name="maximumValue">The maximum value displayed on this axis.</param>
        /// <param name="majorIncrement">How often a major increment (tick) occurs.</param>
        /// <param name="minorIncrement">How often a minor increment (tick) occurs.</param>
        /// <param name="pixelsPerIncrement">Number of pixels per increment</param>
        public AbstractScaleAxis(AxisFormat format, object minimumValue, object maximumValue, int majorIncrement, int minorIncrement,
            int pixelsPerIncrement) : base(format)
        {
            Format = format;
            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
            MajorIncrement = majorIncrement;
            MinorIncrement = minorIncrement;
            PixelsPerIncrement = pixelsPerIncrement;

            string errorMessage;
            if (AreAxisValuesValid(out errorMessage) == false)
            {
                throw new ArgumentException(errorMessage);
            }

        }

        public override SizeF GetAxisLabelDimensions()
        {
            int width = 0;
            int height = 0;
            switch (AxisXY)
            {
                case Axis.X:
                    int widthOfLastLabel = (int)Entries[Entries.Count - 1].Label.Dimensions.Value.Width;
                    width = (TotalIncrementCount() * PixelsPerIncrement) + widthOfLastLabel;
                    height = _maxLabelHeight + AxisPadding;
                    break;
                case Axis.Y:
                    width = _maxLabelWidth + AxisPadding;
                    height = (LabelPadding * Entries.Count) + LabelPadding + (Entries.Count * _maxLabelHeight);
                    break;
            }

            return new SizeF(width, height);
        }

        /// <summary>
        /// Check to see if the minimum, maximum and increment values for the Axis are valid.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected abstract bool AreAxisValuesValid(out string errorMessage);

        /// <summary>
        /// Returns the number of major increments on the axis.
        /// </summary>
        /// <returns></returns>
        public int MajorTickCount()
        {
            int rValue = 0;
            foreach (AxisEntry o in Entries)
            {
                if (o.IsMajorTick)
                {
                    ++rValue;
                }
            }
            return rValue;
        }

        /// Returns the number of minor increments on the axis.
        /// </summary>
        /// <returns></returns>
        public int MinorTickCount()
        {
            int rValue = 0;
            foreach (AxisEntry o in Entries)
            {
                if (o.IsMajorTick == false)
                {
                    ++rValue;
                }
            }
            return rValue;
        }

        public override void DrawAxis(Graphics g, Point offset)
        {
            if (AxisXY == Axis.X)
            {
                foreach (var e in Entries)
                {
                    if (e.IsMajorTick)
                    {
                        g.DrawString(e.Label.Text, AxisFont, Brushes.Black, offset.X, offset.Y);
                    }
                    offset.X += PixelsPerIncrement;
                }
            }

        }

        /// <summary>
        /// Draw the scale ticks on the axis.
        /// At the same time, the labels' ChartPosition are set.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="offset"></param>
        public override void DrawTicks(Graphics g, Point offset)
        {
            if (AxisXY == Axis.X)
            {
                foreach (var e in Entries)
                {
                    e.Label.Position = new Point(offset.X, offset.Y - _maxLabelHeight);
                    if (e.IsMajorTick)
                    {
                        g.DrawLine(MajorTickPen, new Point(offset.X, offset.Y), new Point(offset.X, offset.Y + MajorTickLength));
                    } 
                    else
                    {
                        g.DrawLine(MinorTickPen, new Point(offset.X, offset.Y), new Point(offset.X, offset.Y + MinorTickLength));
                    }
                    offset.X += PixelsPerIncrement;
                }
            }
            else if (AxisXY == Axis.Y)
            {
                throw new NotImplementedException("Coding not done for AbstractScaleAxis.DrawTicks() y-axis implementation");
            }

        }

        public override void DrawMajorGridLines(Graphics g)
        {
            if (AxisXY == Axis.X)
            {
                foreach (var e in Entries)
                {
                    Point p = e.Label.Position;
                    if (e.IsMajorTick)
                    {
                        g.DrawLine(MajorGridLinePen, new Point(p.X, p.Y + _maxLabelHeight + MajorTickLength), new Point(p.X, p.Y + 500));
                    }
                }
            }
            else if (AxisXY == Axis.Y)
            {
                throw new NotImplementedException("Coding not done for AbstractScaleAxis.DrawMajorGridLines() y-axis implementation");
            }

        }

        public override void DrawAxisLabels(Graphics g, Point offset)
        {
            foreach (AxisEntry entry in Entries)
            {
                if (entry.IsMajorTick)
                {
                    g.DrawString(entry.Label.Text, AxisFont, Brushes.Black, entry.Label.Position);
                }
            }
        }

    }
}
