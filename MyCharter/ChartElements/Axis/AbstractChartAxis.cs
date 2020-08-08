using MyCharter.ChartElements;
using MyCharter.ChartElements.Axis;
using MyCharter.Util;
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
        /// Is the Axis visible?
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Should a line be drawn the length (either height or width) of the axis?
        /// </summary>
        public bool AxisLine { get; set; } = true;

        /// <summary>
        /// List of entries on the axis.
        /// SortedList provides sorting that we might not want on Data Series'
        /// List provides no sorting/easy access
        /// </summary>
        public List<AxisEntry> AxisEntries = new List<AxisEntry>();

        /// <summary>
        /// The font that is used to label the axis.
        /// </summary>
        public Font AxisFont { get; set; } = new Font("Arial", 8 * 1.33f, FontStyle.Regular, GraphicsUnit.Point);

        /// <summary>
        /// The amount of padding (in pixels) which is placed above and below axis items.
        /// </summary>
        public int LabelPadding { get; set; } = 5;

        /// <summary>
        /// The amount of padding (in pixels) which is placed between the label and the axis.
        /// </summary>
        public int AxisPadding { get; set; } = 5;

        /// <summary>
        /// Defines where the axis appears in relation to the chart.
        /// </summary>
        public ElementPosition AxisPosition { get; set; }

        private AxisLabelHorizontalPosition _labelHorizontalPosition = AxisLabelHorizontalPosition.LEFT;
        public AxisLabelHorizontalPosition LabelHorizontalPosition
        {
            get => _labelHorizontalPosition;
            set {
                if ((AxisXY == Axis.X) && ((value == AxisLabelHorizontalPosition.LEFT) || (value == AxisLabelHorizontalPosition.CENTER)))
                    _labelHorizontalPosition = value;
                else
                    throw new ArgumentException("X-Axis can only have a Label Horizontal position of LEFT or CENTER");
            }
        }

        /// <summary>
        /// This is the largest label dimensions for the Axis.
        /// Note: Calculated in MeasureLabels()
        /// </summary>
        protected SizeF _maxLabelDimensions = new SizeF(-1, -1);

        /// <summary>
        /// The dimension of the Axis.
        /// </summary>
        protected Size _axisDimensions = new Size(-1, -1);

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
        /// Should the major grid lines be drawn?
        /// </summary>
        public bool MajorGridLine { get; set; } = false;

        /// <summary>
        /// The Pen used to draw the major ticks.
        /// </summary>
        public Pen MinorTickPen = new Pen(Brushes.Black, 1);

        /// <summary>
        /// The length of the minor tick.
        /// </summary>
        public int MinorTickLength = 3;

        public AbstractChart ParentChart = null;

        public int PixelsPerIncrement { get; set; } = 10;

        public AxisWidth AxisWidth = AxisWidth.FIT_TO_LABELS; // Default value

        /// <summary>
        /// The position at where the Axis starts.
        /// </summary>
        public Point AxisCoords { get; set; }

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
        /// Calculate the dimensions of the Axis.
        /// This includes: 
        /// a) the length of the labels, 
        /// b) axis-padding (space between the labels and the tick marks), and again to offset the axis intersection.
        /// c) the length of the tick marks
        /// </summary>
        /// <returns></returns>
        public Size CalculateDimensions(bool includeLabelPadding, bool includeAxisPadding, bool includeTick)
        {
            int width = 0;
            int height = 0;

            if (IsVisible)
            {
                switch (AxisXY)
                {
                    case Axis.X:
                        //width = PixelsPerIncrement; // Offset the x-axis first entry by an increment.
                        if (AxisWidth == AxisWidth.FIT_TO_LABELS)
                        {
                            width += TotalIncrementCount() * (int)_maxLabelDimensions.Width;
                            if (includeLabelPadding)
                                width += TotalIncrementCount() * LabelPadding;
                        }
                        else if (AxisWidth == AxisWidth.FIT_TO_INCREMENT)
                        {
                            width += TotalIncrementCount() * PixelsPerIncrement;
                        }
                        height = (int)_maxLabelDimensions.Height;
                        if (includeAxisPadding)
                            height += AxisPadding;
                        if (includeTick)
                            height += MajorTickLength;
                        break;
                    
                    case Axis.Y:
                        width = (int)_maxLabelDimensions.Width;
                        if (includeAxisPadding)
                            width += AxisPadding;
                        if (includeTick)
                            width += MajorTickLength;
                        height = (AxisEntries.Count * (int)_maxLabelDimensions.Height);
                        if (includeLabelPadding)
                            height += (LabelPadding * AxisEntries.Count) + LabelPadding;
                        break;
                }
            }
            else Console.WriteLine("axis is not visible");

            return new Size(width, height);
        }

        /// <summary>
        /// Returns the size the axis will consume.
        /// This includes: 
        /// a) the length of the labels, 
        /// b) axis-padding (space between the labels and the tick marks).
        /// c) the length of the tick marks
        /// </summary>
        /// <returns></returns>
        public Size GetDimensions()
        {
            if (_axisDimensions.Width == -1)
                _axisDimensions = CalculateDimensions(true, true, true);

            return _axisDimensions;
        }

        /// <summary>
        /// Return the dimensions of the largest label on the Axis.
        /// </summary>
        /// <returns></returns>
        public SizeF GetMaxLabelDimensions()
        {
            // if the label dimensions have not been calculated, then do so.
            if (_maxLabelDimensions.Width == -1)
                MeasureAxisEntryLabels();

            return _maxLabelDimensions;
        }

        /// <summary>
        /// Add an AxisEntry to the Axis.
        /// </summary>
        /// <param name="entry"></param>
        protected void AddEntry(AxisEntry entry)
        {
            AxisEntries.Add(entry);
        }

        /// <summary>
        /// Generate the axis values, between the minimum and maximum values (if applicable).
        /// This method is populated in implementation classes.
        /// </summary>
        internal abstract void GenerateAxisEntries();

        /// <summary>
        /// Draw the major gridlines.
        /// </summary>
        /// <param name="g"></param>
        public void DrawMajorGridLines(Graphics g)
        {
            if (AxisXY == Axis.X)
            {
                foreach (AxisEntry e in AxisEntries)
                {
                    if (e.IsMajorTick)
                    {
                        if (AxisPosition == ElementPosition.BOTTOM)
                            g.DrawLine(MajorGridLinePen, e.Position, new Point(e.Position.X, e.Position.Y - ParentChart.GetAxis(Axis.Y).GetDimensions().Height));
                        else if (AxisPosition == ElementPosition.TOP)
                            g.DrawLine(MajorGridLinePen, 
                                new Point(e.Position.X, e.Position.Y + GetDimensions().Height), 
                                new Point(e.Position.X, e.Position.Y + GetDimensions().Height + ParentChart.GetAxis(Axis.Y).GetDimensions().Height));
                    }
                }
            }
            else if (AxisXY == Axis.Y)
            {
                foreach (AxisEntry e in AxisEntries)
                {
                    if (e.IsMajorTick)
                    {
                        if (AxisPosition == ElementPosition.LEFT)
                            g.DrawLine(MajorGridLinePen, 
                                new Point(e.Position.X + GetDimensions().Width, e.Position.Y), 
                                new Point(e.Position.X + ParentChart.GetAxis(Axis.X).GetDimensions().Width, e.Position.Y));
                        else if (AxisPosition == ElementPosition.RIGHT)
                            g.DrawLine(MajorGridLinePen,
                                new Point(e.Position.X, e.Position.Y),
                                new Point(e.Position.X - ParentChart.GetAxis(Axis.X).GetDimensions().Width, e.Position.Y));
                    }
                }
            }

        }

        /// <summary>
        /// Calculates the dimensions of the axis labels (based on the font being used). 
        /// Also populates _maxLabelDimensions.
        /// </summary>
        public void MeasureAxisEntryLabels()
        {
            if (_maxLabelDimensions.Width > -1)
                return; // If already calculated, don't do it again

            // Create a temporary BMP for 'sketching'
            Bitmap tempBMP = new Bitmap(300, 300);
            Graphics tempGraphics = Graphics.FromImage(tempBMP);
            float maxWidth = 0;
            float maxHeight = 0;

            for (int i = 0; i < AxisEntries.Count; i++)
            {
                ImageText label = (ImageText)AxisEntries[i].Label;
                if (label != null)
                {
                    SizeF stringMeasurement = tempGraphics.MeasureString(label.Text, AxisFont);
                    label.Dimensions = stringMeasurement;

                    // Update the max values.
                    maxWidth = (maxWidth < (int)stringMeasurement.Width) ? (int)stringMeasurement.Width : maxWidth;
                    maxHeight = (maxHeight < (int)stringMeasurement.Height) ? (int)stringMeasurement.Height : maxHeight;
                }
            }

            _maxLabelDimensions = new SizeF(maxWidth, maxHeight);

            tempBMP.Dispose();
        }

        /// <summary>
        /// Output the axis values.
        /// </summary>
        public void DebugOutput_ListScale()
        {
            Console.WriteLine($"ID  " +
                $"{"Key".PadRight(10, ' ')}" +
                $"{"Label".PadRight(15, ' ')}" +
                $"{"Major".PadRight(6, ' ')}" +
                $"{"Dimensions".PadRight(18, ' ')}" +
                $"{"Positions".PadRight(22, ' ')}");
            int counter = 0;
            foreach (object o in AxisEntries)
            {
                if (o is AxisEntry element)
                {
                    var label = (ImageText)element.Label;
                    if (label != null)
                    {
                        var labelText = label.Text;
                        if (labelText.Length > 15)
                            labelText = labelText.Substring(1, 15);
                        var majorLabel = element.IsMajorTick ? "Yes" : "No";

                        Console.WriteLine($"{counter,3} " +
                            $"{element.KeyValue,-10}" +
                            $"{labelText,-15}" +
                            $"{majorLabel.PadRight(6, ' ')}" +
                            $"h={Math.Round(label.Dimensions.Value.Height, 1, MidpointRounding.ToEven)}, w={Math.Round(label.Dimensions.Value.Width, 1, MidpointRounding.ToEven)} ".PadRight(18, ' ') +
                            $"e X={element.Position.X},Y={element.Position.Y} l X={label.Position.X},Y={label.Position.Y}");
                    }
                }
                ++counter;
            }
        }

        /// <summary>
        /// Calculates the intial axis positions; these will need later adjustment.
        /// This will be positions based on the position and size of the label (and NOT be relative to other items).
        /// </summary>
        protected void CalculateInitialAxisValuePositions()
        {
            /// LabelPadding - The amount of padding (in pixels) which is placed above and below axis items.
            /// AxisPadding - The amount of padding (in pixels) which is placed between the label and the axis.
            if (AxisWidth == AxisWidth.FIT_TO_LABELS)
            {
                switch (AxisXY)
                {
                    case Axis.X:
                        int x = PixelsPerIncrement; // change made here
                        // Determine the WIDTH of the label
                        int width = (int)_maxLabelDimensions.Width + LabelPadding;
                        foreach (AxisEntry e in AxisEntries)
                        {
                            e.Position = new Point(x, 0);
                            x += width;
                        }
                        break;
                    case Axis.Y:
                        int y = ((int)_maxLabelDimensions.Height / 2);
                        // Determine the WIDTH of the label
                        int height = (int)_maxLabelDimensions.Height + LabelPadding;
                        foreach (AxisEntry e in AxisEntries)
                        {
                            e.Position = new Point(0, y);
                            y += height;
                        }
                        break;
                }
            }

            else if (AxisWidth == AxisWidth.FIT_TO_INCREMENT)
            {
                switch (AxisXY)
                {
                    case Axis.X:
                        int x = PixelsPerIncrement; // change made here
                        foreach (AxisEntry e in AxisEntries)
                        {
                            e.Position = new Point(x, 0);
                            x += PixelsPerIncrement;
                        }
                        break;
                    case Axis.Y:
                        int y = 0;
                        foreach (AxisEntry e in AxisEntries)
                        {
                            e.Position = new Point(0, y);
                            y += PixelsPerIncrement;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// The initial Axis preparation.
        /// Note that this method a) generates axis values, b) measures axis labels and c) calculates the initial axis value positions.
        /// This initial axis positions is relative to ITSELF, and requires FinaliseAxisLayout() to be called in order to position relative to other items.
        /// </summary>
        public void InitialAxisPreparation()
        {
            GenerateAxisEntries();

            MeasureAxisEntryLabels();

            CalculateInitialAxisValuePositions();
        }

        /// <summary>
        /// Finalise the Axis layout. This:
        /// 1) Calculates the final Axis positions (relative to everything else) 
        /// 2) Calculates the label positions.
        /// </summary>
        public void FinaliseAxisLayout()
        {
            CalculateFinalAxisValuePositions();
            CalculateFinalLabelPosition();
        }

        /// <summary>
        /// Calculate the Label positions relative to the Axis ticks.
        /// </summary>
        private void CalculateFinalLabelPosition()
        {
            if (AxisXY == Axis.X)
            {
                foreach (AxisEntry e in AxisEntries)
                {
                    int x = 0;
                    int y = 0;

                    // The position of the Axis (AxisPosition) changes the y-value.
                    // The horizontal position of the label (LabelHorizontalPosition) changes the x-value.
                    switch (AxisPosition)
                    {
                        case ElementPosition.TOP:
                            y = e.Position.Y;
                            break;
                        case ElementPosition.BOTTOM:
                            y = e.Position.Y + MajorTickLength + 1;
                            break;
                    }

                    switch (LabelHorizontalPosition)
                    {
                        case AxisLabelHorizontalPosition.LEFT:
                            x += e.Position.X;
                            break;
                        case AxisLabelHorizontalPosition.CENTER:
                            x += e.Position.X - ((int)e.Label.Dimensions.Value.Width / 2);
                            break;
                    }
                    e.Label.Position = new Point(x, y);
                } // end foreach
            }
            else if (AxisXY == Axis.Y)
            {
                foreach (AxisEntry e in AxisEntries)
                {
                    switch (AxisPosition)
                    {
                        case ElementPosition.LEFT:
                            // Start with the X-position of the Y-Axis,
                            // then add (the max width - width of this label) to right align the text horizontally
                            int xPos = AxisCoords.X + ((int)GetMaxLabelDimensions().Width - (int)e.Label.Dimensions.Value.Width);
                            // Center the text vertically
                            int yPos = e.Position.Y - (int)_maxLabelDimensions.Height / 2;
                            if (xPos < 0)
                            {
                                xPos = 0;
                            }
                            e.Label.Position = new Point(xPos, yPos);
                            break;
                        case ElementPosition.RIGHT:
                            // The text will be on the RIGHT of the tick
                            e.Label.Position = new Point(e.Position.X + AxisPadding + MajorTickLength,
                                e.Position.Y - (int)_maxLabelDimensions.Height / 2);
                            break;
                    }

                    if (e.Label.Text.Equals("200"))
                        Console.WriteLine($"In CalculateFinalLabelPosition() {e.Label.Position}");

                }
            }
        }

        /// <summary>
        /// Calculate the final positions of Axis entries.
        /// This adds the layout values to the exiswting positions of Axis entries and other Axis dimensions.
        /// This uses CalculateFinalLabelPosition() for the Y-axis (and so should be used AFTER it).
        /// </summary>
        private void CalculateFinalAxisValuePositions()
        {
            GraphicsLayout layout = ParentChart._layout;

            Point refPoint = (AxisXY == Axis.X) ? AxisCoords : ParentChart.GetAxis(Axis.Y).AxisCoords;

            foreach (AxisEntry e in AxisEntries)
            {
                e.Position.X += refPoint.X;
                e.Position.Y += refPoint.Y;
            }
        }

        /// <summary>
        /// Return the Axis coordinates of a given label.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public Point? GetAxisPositionOfLabel(string label)
        {
            Point? rValue = null;
            foreach (AxisEntry e in AxisEntries)
            {
                if (e.Label.Text.Equals(label))
                {
                    rValue = e.Position;
                    break;
                }
            }
            return rValue;
        }

        /// <summary>
        /// Draw the Axis label.
        /// Only draws for major ticks, where there is space free.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bmp"></param>
        private void DrawAxisLabels(Graphics g, Bitmap bmp)
        {           
            foreach (AxisEntry e in AxisEntries)
            {
                if (e.IsMajorTick)
                {
                    if (e.Label.Text.Equals("0"))
                    {
                        Console.WriteLine($"In DrawAxisLabels(): {e.Label.Text} is at {e.Position}; label position is {e.Label.Position}");
                    }

                    // Check to see if space is free for the label.
                    List<int> ignoreColors = new List<int> { ParentChart.ImageBackgroundColor.ToArgb(), Color.Yellow.ToArgb()}; // TODO remove yellow

                    if (ImageMethods.IsSpaceEmpty(g, bmp, 
                            new Rectangle(e.Label.Position.X, e.Label.Position.Y, (int)e.Label.Dimensions.Value.Width, (int)e.Label.Dimensions.Value.Height), 
                            ignoreColors, null /* @"C:\New Folder\zsnip-" + e.Label.Text + ".png"*/)) 
                    {
                        g.DrawString(e.Label.Text, AxisFont, Brushes.Black, e.Label.Position);
                    }
                    else {
                        //Console.WriteLine($"Can't draw {e.Label.Text} due to space restrictions");
                    }
                        
                }
            }
        }

        public abstract void DrawAxisLabels(Graphics g, Point offset);

        /// <summary>
        /// Returns the total number of increments on the scale.
        /// </summary>
        /// <returns></returns>
        public int TotalIncrementCount()
        {
            return AxisEntries.Count + 1;
        }

        /// <summary>
        /// Draw the Axis.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="offset"></param>
        public abstract void DrawAxis(Graphics g, Point offset);

        public abstract void DrawTicks(Graphics g, Point offset);

        public abstract void CalculateLabelPositions(Point offset);

        public abstract string FormatLabelString(object label);

        /// <summary>
        /// Draw the major and minor Ticks on the Axis.
        /// </summary>
        /// <param name="g"></param>
        private void DrawTicks(Graphics g)
        {
            if (AxisXY == Axis.X)
            {
                // Ticks will be VERTICAL
                foreach (var e in AxisEntries)
                {
                    if (e.IsMajorTick)
                    {
                        if (AxisPosition == ElementPosition.BOTTOM)
                            g.DrawLine(MajorTickPen, e.Position, new Point(e.Position.X, e.Position.Y + MajorTickLength));
                        else if (AxisPosition == ElementPosition.TOP)
                            g.DrawLine(MajorTickPen, 
                                new Point(e.Position.X, e.Position.Y + (int)GetMaxLabelDimensions().Height + AxisPadding),
                                new Point(e.Position.X, e.Position.Y + (int)GetMaxLabelDimensions().Height + AxisPadding + MajorTickLength));
                    }
                    else
                    {
                        g.DrawLine(MajorTickPen, e.Position, new Point(e.Position.X, e.Position.Y + MinorTickLength));
                    }

                }
            }
            else if (AxisXY == Axis.Y)
            {
                // Ticks will be HORIZONTAL
                foreach (var e in AxisEntries)
                {
                    if (AxisPosition == ElementPosition.LEFT)
                    {
                        if (e.IsMajorTick)
                        {
                            Point startPosition = new Point(e.Position.X + (int)GetMaxLabelDimensions().Width + AxisPadding, e.Position.Y);
                            g.DrawLine(MajorTickPen, startPosition, new Point(startPosition.X + MajorTickLength, e.Position.Y));
                        }
                        else
                        {
                            Point startPosition = new Point(e.Position.X + (int)GetMaxLabelDimensions().Width + AxisPadding + (MajorTickLength - MinorTickLength), e.Position.Y);
                            g.DrawLine(MinorTickPen, startPosition, new Point(startPosition.X + MinorTickLength, e.Position.Y));
                        }
                    }
                    else if (AxisPosition == ElementPosition.RIGHT)
                    {
                        if (e.IsMajorTick)
                        {
                            Point startPosition = new Point(e.Position.X + AxisPadding, e.Position.Y);
                            g.DrawLine(MajorTickPen, startPosition, new Point(startPosition.X - MajorTickLength, e.Position.Y));
                        }
                        else
                        {
                            Point startPosition = new Point(e.Position.X, e.Position.Y);
                            g.DrawLine(MinorTickPen, startPosition, new Point(startPosition.X + MinorTickLength, e.Position.Y));
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Draw the Axis, including the Axis Line, Ticks and labels.
        /// If IsVisible is set to false, the axis won't be drawn.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bmp"></param>
        public void DrawAxis(Graphics g, Bitmap bmp)
        {
            if (IsVisible)
            {
                DrawTicks(g);
                DrawAxisLabels(g, bmp);
                DrawAxisLine(g);
            }

            if (MajorGridLine)
                DrawMajorGridLines(g);
        }

        /// <summary>
        /// Draw the Axis Line; the border along the X and Y axis.
        /// If AxisLine is false, it will not be drawn.
        /// </summary>
        /// <param name="g"></param>
        private void DrawAxisLine(Graphics g)
        {
            if (AxisLine)
            {
                if (AxisXY == Axis.X)
                {
                    int yOffset = 0;
                    if (AxisPosition == ElementPosition.TOP)
                    {
                        // Offset the y by the full height of the x-axis.
                        yOffset = GetDimensions().Height;
                    }

                    g.DrawLine(new Pen(Color.Black, 1),
                        new Point(AxisCoords.X, AxisCoords.Y + yOffset),
                        new Point(AxisCoords.X + GetDimensions().Width, AxisCoords.Y + yOffset));
                }
                else if (AxisXY == Axis.Y)
                {
                    int xOffset = 0;
                    if (AxisPosition == ElementPosition.LEFT)
                    {
                        // Offset the x by the full width of the yAxis.
                        xOffset = GetDimensions().Width;
                    }
                    if (ParentChart.GetAxis(Axis.X).AxisPosition == ElementPosition.BOTTOM)
                    {
                        g.DrawLine(new Pen(Color.Black, 1),
                            new Point(AxisCoords.X + xOffset, (AxisCoords.Y + ((int)GetMaxLabelDimensions().Height / 2))),
                            new Point(AxisCoords.X + xOffset, AxisCoords.Y + GetDimensions().Height));
                    }
                    else if (ParentChart.GetAxis(Axis.X).AxisPosition == ElementPosition.TOP)
                    {
                        g.DrawLine(new Pen(Color.Black, 1),
                            new Point(AxisCoords.X + xOffset, AxisCoords.Y),// + ((int)GetMaxLabelDimensions().Height / 2))),
                            new Point(AxisCoords.X + xOffset, AxisCoords.Y + GetDimensions().Height));
                    }
                }
            }
        }
    }
}
