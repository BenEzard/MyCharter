using MyCharter.ChartElements.Axis;
using System;
using System.Collections;
using System.Drawing;

namespace MyCharter
{
    public abstract class AbstractChartAxis
    {
        /// <summary>
        /// The X or Y value which denotes which axis this is.
        /// </summary>
        public Axis AxisXY { get; set; }

        /// <summary>
        /// The format (number/time etc) of this axis.
        /// </summary>
        public AxisFormat Format { get; set; }

        /// <summary>
        /// List of entries on the axis.
        /// SortedList provides sorting that we might not want on Data Series'
        /// List provides no sorting/easy access
        /// </summary>
        public SortedList Entries = new SortedList();

        /// <summary>
        /// The font that is used to label the axis.
        /// </summary>
        public Font AxisFont { get; set; } = new Font("Arial", 9 * 1.33f, FontStyle.Regular, GraphicsUnit.Point);

        /// <summary>
        /// The amount of padding (in pixels) which is placed above and below axis items.
        /// </summary>
        public int LabelPadding { get; set; } = 5;

        /// <summary>
        /// The amount of padding (in pixels) which is placed between the label and the axis.
        /// </summary>
        public int AxisPadding { get; set; } = 5;

        public AxisLabelPosition LabelPosition { get; set; }

        /// <summary>
        /// Instantiate an Axis.
        /// </summary>
        /// <param name="format">The AxisFormat of this axis.</param>
        public AbstractChartAxis(AxisFormat format)
        {
            Format = format;
        }

        /// <summary>
        /// Add an AxisEntry to the (sorted) Axis.
        /// </summary>
        /// <param name="entry"></param>
        protected void AddEntry(AxisEntry entry)
        {

            Entries.Add(entry.KeyValue, entry);
        }

        /// <summary>
        /// Generate the axis values, between the minimum and maximum values (if applicable).
        /// This method is populated in implementation classes.
        /// </summary>
        internal abstract void GenerateAxisValues();

        /// <summary>
        /// Calculate the dimensions that the label will take on the image (based on the font being used).
        /// </summary>
        public void CalculateLabelDimensions()
        {
            // Create a temporary BMP for 'sketching'
            Bitmap tempBMP = new Bitmap(400, 400);
            Graphics tempGraphics = Graphics.FromImage(tempBMP);

            foreach (object o in Entries.Values)
            {
                if (o is AxisEntry element)
                {
                    var label = (ImageElement)element.Label;
                    if (label != null)
                        label.LabelDimensions = tempGraphics.MeasureString(label.Label, AxisFont);
                }
            }

            tempBMP.Dispose();
        }

        /// <summary>
        /// Output the axis values.
        /// </summary>
        public void DebugOutput_ListScale()
        {
            foreach (object o in Entries.Values)
            {
                if (o is AxisEntry element)
                {
                    var label = (ImageElement)element.Label;
                    if (label != null)
                    {
                        Console.Write($"Key = {element.KeyValue} ");
                        Console.WriteLine($"; Label = '{label.Label}' with dimensions = '{label.LabelDimensions}' and position = '{label.ChartPosition}'");
                    }
                }
            }
        }

        /// <summary>
        /// Return the maximum label dimensions on the axis.
        /// </summary>
        /// <returns></returns>
        public SizeF GetMaxLabelDimensions()
        {
            SizeF rValue = new SizeF(0, 0);
            foreach (object o in Entries.Values)
            {
                if ((o is AxisEntry element) && (element.Label != null))
                {
                    var label = (ImageElement)element.Label;
                    if (label.LabelDimensions.HasValue)
                    {
                        if (rValue.Width < label.LabelDimensions.Value.Width)
                            rValue.Width = label.LabelDimensions.Value.Width;
                        if (rValue.Height < label.LabelDimensions.Value.Height)
                            rValue.Height = label.LabelDimensions.Value.Height;
                    }
                }
            }
            return rValue;
        }

        /// <summary>
        /// Return the amount of space (in pixels) required for all of the labels on this axis.
        /// </summary>
        /// <returns></returns>        
        internal abstract SizeF GetLabelDimensions();

        /// <summary>
        /// Returns the total number of increments on the scale.
        /// </summary>
        /// <returns></returns>
        public int TotalIncrementCount() {
            return Entries.Count;
        }

        public abstract void DrawAxis(Graphics g, ref Point offset);

    }
}
