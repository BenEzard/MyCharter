using MyCharter.ChartElements.Axis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

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
        public List<AxisEntry> Entries = new List<AxisEntry>();

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
        /// This is the maximum label width on the Axis. 
        /// This is calculated in MeasureLabels()
        /// </summary>
        protected int _maxLabelWidth = -1;

        /// <summary>
        /// This is the maximum label height on the Axis. 
        /// This is calculated in MeasureLabels()
        /// </summary>
        protected int _maxLabelHeight = -1;

        /// <summary>
        /// The Pen used to draw the major ticks.
        /// </summary>
        public Pen MajorTickPen = new Pen(Brushes.Black, 1);

        /// <summary>
        /// The length of the major tick.
        /// </summary>
        public int MajorTickLength = 5;

        public Pen MajorGridLinePen = new Pen(Brushes.Gray, 1);


        /// <summary>
        /// Instantiate an Axis.
        /// </summary>
        /// <param name="format">The AxisFormat of this axis.</param>
        public AbstractChartAxis(AxisFormat format)
        {
            Format = format;

            // Configure MajorGridLinePen
            MajorGridLinePen.DashStyle = DashStyle.Dash;
            MajorGridLinePen.DashPattern = new float[] { 3, 3 };
        }

        /// <summary>
        /// Get the total Axis Label dimensions.
        /// </summary>
        /// <returns></returns>
        public abstract SizeF GetAxisLabelDimensions();

        public SizeF GetMaxLabelDimensions()
        {
            return new SizeF(_maxLabelWidth, _maxLabelHeight);
        }

        /// <summary>
        /// Add an AxisEntry to the Axis.
        /// </summary>
        /// <param name="entry"></param>
        protected void AddEntry(AxisEntry entry)
        {
            Entries.Add(entry);
        }

        /// <summary>
        /// Generate the axis values, between the minimum and maximum values (if applicable).
        /// This method is populated in implementation classes.
        /// </summary>
        internal abstract void GenerateAxisValues();

        /*        public Point GetAxisEntry(object key)
                {
                    Point rValue = new Point(-1,-1);
                    foreach (AxisEntry e in Entries)
                    {
                        if (e.KeyValue == key)
                        {
                            rValue = e.Label.ChartPosition;
                        }
                    }
                    return rValue;
                }*/

        public abstract void DrawMajorGridLines(Graphics g);

        /// <summary>
        /// Prepare the Axis. Must be called before it can be displayed.
        /// This method does the following:
        /// - calculate the dimensions the label will take on the image (based on the font being used), updating MaxLabelSize;
        /// </summary>
        public void MeasureLabels()
        {
            // Create a temporary BMP for 'sketching'
            Bitmap tempBMP = new Bitmap(400, 400);
            Graphics tempGraphics = Graphics.FromImage(tempBMP);

            for (int i = 0; i < Entries.Count; i++)
            {
                ImageElement label = (ImageElement)Entries[i].Label;
                if (label != null)
                {
                    SizeF stringMeasurement = tempGraphics.MeasureString(label.Label, AxisFont);
                    label.LabelDimensions = stringMeasurement;

                    // Update the max values.
                    _maxLabelWidth = (_maxLabelWidth < (int)stringMeasurement.Width) ? (int)stringMeasurement.Width : _maxLabelWidth;
                    _maxLabelHeight = (_maxLabelHeight < (int)stringMeasurement.Height) ? (int)stringMeasurement.Height : _maxLabelHeight;
                }
            }

            tempBMP.Dispose();
        }

        /// <summary>
        /// Output the axis values.
        /// </summary>
        public void DebugOutput_ListScale()
        {
            foreach (object o in Entries)
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

        public abstract void DrawAxisLabels(Graphics g, Point offset);

        /// <summary>
        /// Returns the total number of increments on the scale.
        /// </summary>
        /// <returns></returns>
        public int TotalIncrementCount() {
            return Entries.Count;
        }

        public abstract void DrawAxis(Graphics g, Point offset);

        public abstract void DrawTicks(Graphics g, Point offset);
    }
}
