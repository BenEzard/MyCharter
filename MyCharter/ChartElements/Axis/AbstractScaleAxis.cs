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
        /// Add a Tick to the scale axis.
        /// </summary>
        /// <param name="tick"></param>
        public void AddTick(Tick tick)
        {
            AddEntry(tick);
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
            foreach (object o in Entries.Values)
            {
                if (o is Tick element)
                {
                    if (element.IsMajorTick)
                    {
                        ++rValue;
                    }
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
            foreach (object o in Entries.Values)
            {
                if (o is Tick element)
                {
                    if (element.IsMajorTick == false)
                    {
                        ++rValue;
                    }
                }
            }
            return rValue;
        }

        internal override SizeF GetLabelDimensions()
        {
            var maxDimensions = GetMaxLabelDimensions();
            int width = 0;
            int height = 0;

            if (AxisXY == Axis.Y) // If the data series are displayed on the Y axis
            {
                width += (int)maxDimensions.Width + AxisPadding;
                height = ((int)maxDimensions.Height * (Entries.Count + 1)) + (2 * LabelPadding * (Entries.Count + 1));
            }
            else if (AxisXY == Axis.X)
            {
                height = (int)maxDimensions.Height + AxisPadding;
                width += PixelsPerIncrement * (TotalIncrementCount() + 1);
            }
            Console.WriteLine($"Returning height of {height}");
            return new SizeF(width, height);
        }

        public override void DrawAxis(Graphics g, ref Point offset)
        {
            if (AxisXY == Axis.X)
            {
                foreach (var e in Entries.Values)
                {
                    if (e is Tick entry2)
                    {
                        Console.WriteLine("is a tick");
                        if (entry2.IsMajorTick)
                        {
                            g.DrawString(entry2.Label.Label, AxisFont, Brushes.Black, offset.X, offset.Y);
                        }
                        offset.X += PixelsPerIncrement;

                    }
                    else if (e is AxisEntry entry)
                    {
                        Console.WriteLine("need to do something here");
                    }
                }
            }

        }

    }
}
