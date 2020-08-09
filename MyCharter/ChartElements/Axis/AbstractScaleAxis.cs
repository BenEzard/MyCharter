using MyCharter.ChartElements.Axis;
using System;
using System.Drawing;

namespace MyCharter
{
    public abstract class AbstractScaleAxis<TAxisDataType> : AbstractChartAxis<TAxisDataType>
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
            foreach (AxisEntry o in AxisEntries)
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
            foreach (AxisEntry o in AxisEntries)
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
                foreach (var e in AxisEntries)
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
        /// Draw the scale ticks on the axis, coding their position.
        /// At the same time, the labels' ChartPosition are set.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="offset"></param>
        public override void DrawTicks(Graphics g, Point offset)
        {
            if (AxisXY == Axis.X)
            {
                int index = 0;
                foreach (var e in AxisEntries)
                {
                    switch(LabelHorizontalPosition) { 
                        case AxisLabelHorizontalPosition.LEFT:
                            e.Label.Position = new Point(offset.X, offset.Y - (int)_maxLabelDimensions.Height);
                            break;
                        case AxisLabelHorizontalPosition.CENTER:
                            e.Label.Position = new Point((offset.X - (int)(e.Label.Dimensions.Value.Width / 2)), offset.Y - (int)_maxLabelDimensions.Height);
                            break;
                    }

                    e.Position = new Point(offset.X, offset.Y - (int)_maxLabelDimensions.Height);
                    if (e.IsMajorTick)
                    {
                        g.DrawLine(MajorTickPen, new Point(offset.X, offset.Y), new Point(offset.X, offset.Y + MajorTickLength));
                    } 
                    else
                    {
                        g.DrawLine(MinorTickPen, new Point(offset.X, offset.Y), new Point(offset.X, offset.Y + MinorTickLength));
                    }
                    offset.X += PixelsPerIncrement;
                    ++index;
                }
            }
            else if (AxisXY == Axis.Y)
            {
                int halfOfLabelHeight = (int)_maxLabelDimensions.Height % 2 == 0 ? (int)_maxLabelDimensions.Height / 2 : ((int)_maxLabelDimensions.Height - 1) / 2;
                int y = offset.Y + (int)_maxLabelDimensions.Width + AxisPadding;
                for (int index = 0; index < AxisEntries.Count; index++)
                {
                    var e = AxisEntries[index];
                    if (e is AxisEntry entry)
                    {
                        offset.Y += LabelPadding + halfOfLabelHeight;
                        e.Label.Position = new Point(offset.X, offset.Y - halfOfLabelHeight);
                        e.Position = new Point(offset.X + (int)_maxLabelDimensions.Width, offset.Y - halfOfLabelHeight);
                        g.DrawLine(MajorTickPen, new Point(offset.X + (int)_maxLabelDimensions.Width - 3, offset.Y), new Point(offset.X + (int)_maxLabelDimensions.Width, offset.Y));
                        offset.Y += LabelPadding + halfOfLabelHeight;
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        public override void CalculateLabelPositions(Point offset)
        {
//            Console.WriteLine($"starting at {offset}");
            if (AxisXY == Axis.X)
            {
                int index = 0;
                foreach (var e in AxisEntries)
                {
                    switch (LabelHorizontalPosition)
                    {
                        case AxisLabelHorizontalPosition.LEFT:
                            // When the label horizontal position is LEFT, then the start of the label will be on the tick mark.
                            e.Label.Position = new Point(offset.X, offset.Y - (int)_maxLabelDimensions.Height);
//                            Console.WriteLine($"starting at {e.Label.Position}");
                            break;
                        case AxisLabelHorizontalPosition.CENTER:
                            // When the label horizontal position is CENTER, then the tick mark will be at the halfway point of the label.
                            e.Label.Position = new Point((offset.X - (int)(e.Label.Dimensions.Value.Width / 2)), offset.Y - (int)_maxLabelDimensions.Height);
//                            Console.WriteLine($"starting at {e.Label.Position}");
                            break;
                    }

                    e.Position = new Point(offset.X, offset.Y - (int)_maxLabelDimensions.Height);
                    //if (e.IsMajorTick)
                    //{
                    //    g.DrawLine(MajorTickPen, new Point(offset.X, offset.Y), new Point(offset.X, offset.Y + MajorTickLength));
                    //}
                    //else
                    //{
                    //    g.DrawLine(MinorTickPen, new Point(offset.X, offset.Y), new Point(offset.X, offset.Y + MinorTickLength));
                    //}
                    offset.X += PixelsPerIncrement;
                    ++index;
                }
            }
            else if (AxisXY == Axis.Y)
            {
                int halfOfLabelHeight = (int)_maxLabelDimensions.Height % 2 == 0 ? (int)_maxLabelDimensions.Height / 2 : ((int)_maxLabelDimensions.Height - 1) / 2;
                int y = offset.Y + (int)_maxLabelDimensions.Width + AxisPadding;
                for (int index = 0; index < AxisEntries.Count; index++)
                {
                    var e = AxisEntries[index];
                    offset.Y += LabelPadding + halfOfLabelHeight;
                    e.Label.Position = new Point(offset.X, offset.Y - halfOfLabelHeight);
                    e.Position = new Point(offset.X + (int)_maxLabelDimensions.Width, offset.Y - halfOfLabelHeight);
                    //g.DrawLine(MajorTickPen, new Point(offset.X + (int)_maxLabelDimensions.Width - 3, offset.Y), new Point(offset.X + (int)_maxLabelDimensions.Width, offset.Y));
                    offset.Y += LabelPadding + halfOfLabelHeight;
                }
            }

        }

        public override void DrawAxisLabels(Graphics g, Point offset)
        {
            foreach (AxisEntry entry in AxisEntries)
            {
                if (entry.IsMajorTick)
                {
                    g.DrawString(entry.Label.Text, AxisFont, Brushes.Black, entry.Label.Position);
                }
            }
        }

    }
}
