using MyCharter.ChartElements;
using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using MyCharter.Charts;
using MyCharter.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace MyCharter
{
    public abstract class AbstractChart<TXAxisDataType, TYAxisDataType, TDataPointData>
    {
        public GraphicsLayout _layout;

        private Size _chartDimensions = new Size(-1, -1);

        #region ChartMargin
        /// <summary>
        /// The margin goes around all four sides of the image.
        /// Left, Top, Right, Bottom.
        /// Measured in pixels.
        /// </summary>
        private int[] _marginWidth { get; set; } = new int[4] { 10, 10, 10, 10 };

        /// <summary>
        /// Set the margins which surround the Chart.
        /// Can pass in a single value (to apply to all sides), or 2 values (left and right, top and bottom) or 4 sides (left, top, right, bottom)
        /// </summary>
        /// <param name="margins"></param>
        public void SetMargin(int[] margins)
        {
            switch (margins.Length)
            {
                case 4: // Four values entered, apply to left, top, right, bottom.
                    _marginWidth[0] = margins[0];
                    _marginWidth[1] = margins[1];
                    _marginWidth[2] = margins[2];
                    _marginWidth[3] = margins[3];
                    break;
                case 2: // Two values entered, apply first-value to left & right, and second value to top & bottom.
                    _marginWidth[0] = margins[1];
                    _marginWidth[1] = margins[0];
                    _marginWidth[2] = margins[1];
                    _marginWidth[3] = margins[0];
                    break;
                case 1: // One value entered, apply to all margins.
                    _marginWidth[0] = margins[0];
                    _marginWidth[1] = margins[0];
                    _marginWidth[2] = margins[0];
                    _marginWidth[3] = margins[0];
                    break;

            }
        }

        /// <summary>
        /// Return the margin for the particular position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public int GetMargin(ElementPosition position)
        {
            return _marginWidth[(int)position];
        }
        #endregion

        #region ChartPadding
        /// <summary>
        /// The padding that goes around all four sides, between the border and other content (Title, Axis labels)
        /// Left, Top, Right, Bottom.
        /// Measured in pixels.
        /// </summary>
        private int[] _paddingWidth { get; set; } = new int[4] { 10, 10, 10, 10 };

        /// <summary>
        /// Set the padding which surround the Chart.
        /// Can pass in a single value (to apply to all sides), or 2 values (left and right, top and bottom) or 4 sides (left, top, right, bottom)
        /// </summary>
        /// <param name="padding"></param>
        public void SetPadding(int[] padding)
        {
            switch (padding.Length)
            {
                case 4: // Four values entered, apply to left, top, right, bottom.
                    _paddingWidth[0] = padding[0];
                    _paddingWidth[1] = padding[1];
                    _paddingWidth[2] = padding[2];
                    _paddingWidth[3] = padding[3];
                    break;
                case 2: // Two values entered, apply first-value to left & right, and second value to top & bottom.
                    _paddingWidth[0] = padding[1];
                    _paddingWidth[1] = padding[0];
                    _paddingWidth[2] = padding[1];
                    _paddingWidth[3] = padding[0];
                    break;
                case 1: // One value entered, apply to all margins.
                    _paddingWidth[0] = padding[0];
                    _paddingWidth[1] = padding[0];
                    _paddingWidth[2] = padding[0];
                    _paddingWidth[3] = padding[0];
                    break;
            }
        }

        /// <summary>
        /// Return the padding for the particular position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public int GetPadding(ElementPosition position)
        {
            return _paddingWidth[(int)position];
        }
        #endregion

        #region Titles
        /// <summary>
        /// The ImageElement representation of the ChartTitle.
        /// </summary>
        private ImageText _title = null;

        /// <summary>
        /// The title that is displayed at the top of the chart.
        /// </summary>
        public string Title {
            get
            {
                if (_title == null)
                    return null;
                else
                    return _title.Text;
            }
            set
            {
                _title = new ImageText(value);
                CalculateTitleDimensions();
            }
        }

        /// <summary>
        /// The font which is used for the Chart Title.
        /// </summary>
        private Font TitleFont = new Font("Arial", 14 * 1.33f, FontStyle.Bold, GraphicsUnit.Point);

        /// <summary>
        /// The ImageElement representation of the ChartSubTitle.
        /// </summary>
        private ImageText _subTitle = null;

        /// <summary>
        /// The sub title that is displayed at the top of the chart.
        /// </summary>
        public string SubTitle
        {
            get
            {
                if (_subTitle == null)
                    return null;
                else
                    return _subTitle.Text;
            }
            set
            {
                _subTitle = new ImageText(value);
                CalculateTitleDimensions();
            }
        }

        /// <summary>
        /// The font which is used for the Chart Title.
        /// </summary>
        private Font SubTitleFont = new Font("Arial", 12 * 1.33f, FontStyle.Bold, GraphicsUnit.Point);
        #endregion

        /// <summary>
        /// The font used for Data Labels.
        /// </summary>
        protected Font DataLabelFont = new Font("Arial", 6 * 1.33f, FontStyle.Regular, GraphicsUnit.Point);

        public bool ShowDataLabels { get; set; } = true;

        /// <summary>
        /// The filepath (path + filename + file extension) of the image to output.
        /// </summary>
        public string OutputFile = null;

        /// <summary>
        /// The 1st x-axis.
        /// </summary>
        private AbstractChartAxis<TXAxisDataType> _xAxis1 = null;

        /// <summary>
        /// The y-axis.
        /// </summary>
        private AbstractChartAxis<TYAxisDataType> _yAxis = null;

        /// <summary>
        /// The Pen (colour and width) of a border that will be drawn around the image (inset by MarginWidth).
        /// </summary>
        public Pen BorderPen = new Pen(Color.DarkGray, 1);

        /// <summary>
        /// The background colour of the image.
        /// </summary>
        public Color ImageBackgroundColor = Color.White;

        /// <summary>
        /// Should the Legend be drawn?
        /// </summary>
        public bool IsLegendVisible { get; set; } = true;

        /// <summary>
        /// TODO should be generic at this point?
        /// </summary>
        public List<DataSeries<TXAxisDataType, TYAxisDataType, TDataPointData>> ChartData {get; set;} = new List<DataSeries<TXAxisDataType, TYAxisDataType, TDataPointData>>();

        public DataSeriesLabelOption LabelOption { get; set; } = DataSeriesLabelOption.LABEL_ON_DATA_SERIES;

        public AbstractChart()
        {
        }

        /// <summary>
        /// Set the X-axis with a specific type of ChartAxis.
        /// If successful, assigns the Axis type (x,y) and label position to the axis.
        /// </summary>
        /// <param name="labelPosition"></param>
        /// <param name="axis"></param>
        /// <param name="axisWidth"></param>
        /// <param name="axisLabelAngle"></param>
        public void SetXAxis(ElementPosition labelPosition, AbstractScaleAxis<TXAxisDataType> axis, AxisWidth axisWidth, int axisLabelAngle = 0)
        {
            if ((axisLabelAngle != 0) && (axisLabelAngle != 90)) throw new ArgumentException("AxisLabelAngle can only be 0 or 90.");

            // Will throw an ArgumentException if it doesn't work.
            ValidateAxis(Axis.X, labelPosition);

            // Apply the requested value
            axis.AxisXY = Axis.X;
            axis.AxisPosition = labelPosition;
            axis.AxisWidth = axisWidth;
            axis.LabelAngle = axisLabelAngle;
            _xAxis1 = axis;

            // If BOTH the Minimum or Maximum haven't been set, then auto-calculate values.
            if (EqualityComparer<TXAxisDataType>.Default.Equals(axis.MinimumValue, default)
                && EqualityComparer<TXAxisDataType>.Default.Equals(axis.MaximumValue, default))
            {
                (TXAxisDataType Min, TXAxisDataType Max) = GetXAxisBounds();
                axis.MinimumValue = Min;
                axis.MaximumValue = Max;
            }
            Console.WriteLine($"For X axis min is {axis.MinimumValue} and max is {axis.MaximumValue}");

            axis.InitialAxisPreparation();
        }

        /// <summary>
        /// Set the X-axis with a specific type of ChartAxis.
        /// If successful, assigns the Axis type (x,y) and label position to the axis.
        /// </summary>
        /// <param name="labelPosition"></param>
        /// <param name="axis"></param>
        /// <param name="axisWidth"></param>
        public void SetY1Axis(ElementPosition labelPosition, AbstractScaleAxis<TYAxisDataType> axis, AxisWidth axisWidth)
        {
            // Will throw an ArgumentException if it doesn't work.
            ValidateAxis(Axis.Y, labelPosition);

            // Apply the requested value
            axis.AxisXY = Axis.Y;
            axis.AxisPosition = labelPosition;
            axis.AxisWidth = axisWidth;
            _yAxis = axis;
            
            // If BOTH the Minimum or Maximum haven't been set, then auto-calculate values.
            if (EqualityComparer<TYAxisDataType>.Default.Equals(axis.MinimumValue, default)
                && EqualityComparer<TYAxisDataType>.Default.Equals(axis.MaximumValue, default))
            {
                (TYAxisDataType Min, TYAxisDataType Max) = GetYAxisBounds();
                axis.MinimumValue = Min;
                axis.MaximumValue = Max;
            }

            axis.InitialAxisPreparation();
        }

        /// <summary>
        /// Calculate the initial layout of positions.
        /// </summary>
        public void CalculateInitialLayout()
        {
            int x = 0;
            int y = 0;

            // Margins are the outer-most part of the image on all four sides.
            x += GetMargin(ElementPosition.LEFT);
            y += GetMargin(ElementPosition.TOP);

            // If a border exists, then add that
            if (BorderPen != null)
            {
                _layout.Border = new Point(x, y);
                x += (int)BorderPen.Width;
                y += (int)BorderPen.Width;
            }

            // Padding
            x += GetPadding(ElementPosition.LEFT);
            y += GetPadding(ElementPosition.TOP);

            if (_title != null)
            {
                _layout.Title = new Point(x, y);
                _title.Position = new Point(x, y);
                y += (int)_title.Dimensions.Value.Height;
            }

            if (_subTitle != null)
            {
                _layout.SubTitle = new Point(x, y);
                _subTitle.Position = new Point(x, y);
                y += (int)_subTitle.Dimensions.Value.Height;
                y += 10; // Add some additional space under the heading.
            }

            var xAxis = _xAxis1;
            var yAxis = _yAxis;

            Point tempXAxis = new Point(x, y);
            Point tempYAxis = new Point(x, y);

            if (xAxis.AxisPosition == ElementPosition.TOP)
            {
                // If the x-axis is on the top, then we want the y-axis to be moved down by the height of the x-axis
                tempYAxis.Y += xAxis.GetDimensions().Height;

                if (yAxis.AxisPosition == ElementPosition.LEFT)
                {
                    // If the y-axis is on the left, then we want to move the x-axis right the width of the y-axis
                    tempXAxis.X += xAxis.GetDimensions().Height;
                }
                else if (yAxis.AxisPosition == ElementPosition.RIGHT)
                {
                    // If the Y-Axis is on the right, we want to offset the X-Axis a bit to make room for the left-most label on the x-axis
                   // tempXAxis.X += (int)xAxis.GetMaxLabelDimensions().Width;
                    tempYAxis.X = tempXAxis.X + xAxis.GetDimensions().Width;
                }
                        
            }
            else if (xAxis.AxisPosition == ElementPosition.BOTTOM)
            {
                tempXAxis.Y += yAxis.GetDimensions().Height;
                if (yAxis.AxisPosition == ElementPosition.LEFT)
                {
                    tempXAxis.X += yAxis.GetDimensions().Width;
                }
                else if (yAxis.AxisPosition == ElementPosition.RIGHT)
                {
                    tempYAxis.X = tempXAxis.X + xAxis.GetDimensions().Width;
                }

            }

            _xAxis1.AxisCoords = tempXAxis;
            _yAxis.AxisCoords = tempYAxis;
        }

        public void Debug_GetLayout()
        {
            Console.WriteLine("Layout\n");
            Console.WriteLine($"Border = {_layout.Border}\n" +
                $"Title = {_layout.Title}\n" +
                $"SubTitle = {_layout.SubTitle}\n" +
                $"y-Axis Labels = { _layout.yAxisLabels}\n" +
                $"y-Axis Ticks = { _layout.yAxisTicks}\n" +
                $"x-Axis Labels = { _layout.xAxisLabels}\n" +
                $"x-Axis Ticks = { _layout.xAxisTicks}");

            Console.WriteLine($"y-Axis (x,y,h,w) = {_yAxis.AxisCoords.X}, {_yAxis.AxisCoords.Y}, {_yAxis.GetDimensions().Height}, {_yAxis.GetDimensions().Width}");
            Console.WriteLine($"x-Axis (x,y,h,w) = {_xAxis1.AxisCoords.X}, {_xAxis1.AxisCoords.Y}, {_xAxis1.GetDimensions().Height}, {_xAxis1.GetDimensions().Width}");

        }

        /// <summary>
        /// Validates the Axis.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="labelPosition"></param>
        public void ValidateAxis(Axis type, ElementPosition labelPosition)
        {
            // TODO redo
           /* int index = (int)type;

            if (axes[index] != null)
                throw new ArgumentException($"Axis {type} has already been set.");

            if ((axes[0] != null) && (axes[0].Format == AxisFormat.TIME_SCALE) && (axes[1] != null) && (axes[1].Format != AxisFormat.DATA_SERIES))
                throw new ArgumentException($"Axis types are conflicting. {axes[0].AxisXY} is {axes[0].Format} and {axes[1].AxisXY} is {axes[1].Format}.");

            if ((type == Axis.X) && ((labelPosition == ElementPosition.LEFT) || (labelPosition == ElementPosition.RIGHT))
                || (type == Axis.Y) && ((labelPosition == ElementPosition.TOP) || (labelPosition == ElementPosition.BOTTOM)))
                throw new ArgumentException($"Axis label positions are not valid.");*/
        }

        /// <summary>
        /// Return a Rectangle which shows the coordinates of the border.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetBorderCoordinates()
        {
            SizeF chartDimension = GetChartDimensions();
            int width = (int)chartDimension.Width - (GetMargin(ElementPosition.LEFT) + GetMargin(ElementPosition.RIGHT));
            int height = (int)chartDimension.Height - (GetMargin(ElementPosition.TOP) + GetMargin(ElementPosition.BOTTOM));
            return new Rectangle(GetMargin(ElementPosition.LEFT), GetMargin(ElementPosition.TOP), width, height);
        }

        /// <summary>
        /// Draw a border around the Chart.
        /// </summary>
        /// <param name="g"></param>
        public void DrawBorder(Graphics g)
        {
            if (BorderPen != null)
            {
                g.DrawRectangle(BorderPen, GetBorderCoordinates());
            }
        }

        /// <summary>
        /// Draw the Chart.
        /// </summary>
        public void GenerateChart()
        {
            var xAxis = _xAxis1;
            var yAxis = _yAxis;

            CalculateInitialLayout();

            xAxis.FinaliseAxisLayout(xAxis.AxisCoords, yAxis.AxisCoords);
            yAxis.FinaliseAxisLayout(xAxis.AxisCoords, yAxis.AxisCoords);
            CalculateDataPointLabelDimensions();

            SizeF chartDimension = GetChartDimensions();

            Bitmap bmp = new Bitmap((int)chartDimension.Width, (int)chartDimension.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(ImageBackgroundColor);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                DrawBorder(g);

                DrawTitles(g);

                xAxis.DrawAxis(g, bmp, xAxis.AxisPosition, xAxis.GetDimensions().Width, yAxis.GetDimensions().Height);
                yAxis.DrawAxis(g, bmp, xAxis.AxisPosition, xAxis.GetDimensions().Width, yAxis.GetDimensions().Height);

                /*Pen rectPen = new Pen(Brushes.Red, 1);
                rectPen.DashPattern = new float[] { 10, 10 };
                ImageMethods.Debug_DrawRectangle(g, new Rectangle(xAxis.AxisCoords, xAxis.GetDimensions()), rectPen);
                ImageMethods.Debug_DrawRectangle(g, new Rectangle(yAxis.AxisCoords, yAxis.GetDimensions()), rectPen);*/

                PlotData(g);

                if (IsLegendVisible)
                {
                    Bitmap legendBMP = DrawLegend();

                    int xOffset = bmp.Width;
                    xOffset -= GetMargin(ElementPosition.RIGHT) + GetPadding(ElementPosition.RIGHT);
                    if (BorderPen != null)
                        xOffset -= (int)BorderPen.Width;
                    xOffset -= legendBMP.Width;

                    int yOffset = GetMargin(ElementPosition.TOP) + GetPadding(ElementPosition.TOP);
                    if (BorderPen != null)
                        yOffset += (int)BorderPen.Width;

                    ImageMethods.CopyRegionIntoImage(legendBMP, new Rectangle(0, 0, legendBMP.Width, legendBMP.Height),
                        ref bmp, new Rectangle(xOffset,
                        yOffset, legendBMP.Width, legendBMP.Height));
                }

            }

            bmp.Save(OutputFile, ImageFormat.Png);
        }

        public Bitmap DrawLegend()
        {
            int sizeOfLegendIcon = 10;
            int lineWidth = 3;
            int gapBetweenIconAndText = 5;

            int xOffset = 0;
            int yOffset = 0;
            int maxTextWidth = 0;
            Font legendFont = GetYAxis().AxisFont;
            Bitmap bmp = new Bitmap(500, 500);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(ImageBackgroundColor);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                foreach (DataSeries<TXAxisDataType, TYAxisDataType, TDataPointData> dataSeries in ChartData)
                {
                    SizeF stringMeasurement = g.MeasureString(dataSeries.Name, legendFont);
                    if (stringMeasurement.Width > maxTextWidth)
                        maxTextWidth = (int)stringMeasurement.Width;

                    int textHeight = (int)stringMeasurement.Height;

                    if (yOffset == 0)
                        yOffset = (textHeight - sizeOfLegendIcon) / 2;

                    switch (dataSeries.LegendDisplay)
                    {
                        case LegendDisplayType.SQUARE:
                            g.FillRectangle(new SolidBrush(dataSeries.Color), new Rectangle(xOffset, yOffset, sizeOfLegendIcon, sizeOfLegendIcon));
                            break;
                        case LegendDisplayType.LINE:
                            g.DrawLine(new Pen(dataSeries.Color, lineWidth), 
                                new Point(0, yOffset + (sizeOfLegendIcon - lineWidth) /2), 
                                new Point(sizeOfLegendIcon, yOffset + (sizeOfLegendIcon - lineWidth) / 2));
                            break;
                    }
                    
                    xOffset += sizeOfLegendIcon + gapBetweenIconAndText;
                    yOffset -= (textHeight - sizeOfLegendIcon) / 2;
                    g.DrawString(dataSeries.Name, legendFont, Brushes.Black, new Point(xOffset, yOffset));
                    yOffset += textHeight + gapBetweenIconAndText;
                    xOffset = 0;
                }
                yOffset -= gapBetweenIconAndText;
                maxTextWidth += sizeOfLegendIcon + gapBetweenIconAndText;

                bmp = ImageMethods.CropImage(bmp, new Rectangle(new Point(0, 0), new Size(maxTextWidth, yOffset)));
                bmp.Save(@"c:\new folder\legend.png", ImageFormat.Png);
                //bmp.Dispose();
            }

            return bmp;
        }

        /// <summary>
        /// Get the Title dimensions for both Title and SubTitle, adding padding.TOP below the subtitle.
        /// </summary>
        /// <returns></returns>
        private Size GetTitleDimensions()
        {
            int width = 0;
            int height = 0;

            if ((_title != null) && (_title.Dimensions.HasValue))
            {
                width += (int)_title.Dimensions.Value.Width;
                height += (int)_title.Dimensions.Value.Height;
            }

            if ((_subTitle != null) && (_subTitle.Dimensions.HasValue))
            {
                width += (int)_subTitle.Dimensions.Value.Width;
                height += (int)_subTitle.Dimensions.Value.Height;
            }
            height += GetPadding(ElementPosition.TOP);

            return new Size(width, height);
        }

        /// <summary>
        /// Draw the titles onto the image.
        /// </summary>
        /// <param name="g"></param>
        internal void DrawTitles(Graphics g)
        {
            if (_title != null)
            {
                g.DrawString(Title, TitleFont, Brushes.Black, _layout.Title.X, _layout.Title.Y);
            }
            if (_subTitle != null)
            {
                g.DrawString(SubTitle, SubTitleFont, Brushes.Black, _layout.SubTitle.X, _layout.SubTitle.Y);
            }

        }

           /// <summary>
        /// Calculate the dimensions that the Titles will take on the image (based on the font being used).
        /// </summary>
        public void CalculateTitleDimensions()
        {
            // Create a temporary BMP for 'sketching'
            Bitmap tempBMP = new Bitmap(600, 600);
            Graphics tempGraphics = Graphics.FromImage(tempBMP);

            if ((String.IsNullOrEmpty(Title) == false) && (_title.Dimensions == null))
            {
                _title.Dimensions = tempGraphics.MeasureString(_title.Text, TitleFont);
            }

            if ((String.IsNullOrEmpty(SubTitle) == false) && (_subTitle.Dimensions == null))
            {
                _subTitle.Dimensions = tempGraphics.MeasureString(_subTitle.Text, TitleFont);
            }

            tempBMP.Dispose();
        }

        public abstract void CalculateDataPointLabelDimensions();

        /// <summary>
        /// Get the Chart dimensions.
        /// </summary>
        /// <returns></returns>
        public Size GetChartDimensions()
        {
            // Only calculate the dimensions once.
            if (_chartDimensions.Width == -1)
            {
                int chartWidth = 0;
                int chartHeight = 0;
                var xAxis = _xAxis1;
                var yAxis = _yAxis;

                // Margins - appear at the outer-most of the image
                chartWidth += GetMargin(ElementPosition.LEFT) + GetMargin(ElementPosition.RIGHT);
                chartHeight += GetMargin(ElementPosition.TOP) + GetMargin(ElementPosition.BOTTOM);

                // Border
                if (BorderPen != null)
                {
                    chartWidth += (int)BorderPen.Width * 2;
                    chartHeight += (int)BorderPen.Width * 2;
                }

                // Padding
                chartWidth += GetPadding(ElementPosition.LEFT) + GetPadding(ElementPosition.RIGHT);
                chartHeight += GetPadding(ElementPosition.TOP) + GetPadding(ElementPosition.BOTTOM);

                // Title(s)
                Size titleDimensions = GetTitleDimensions();
                chartHeight += titleDimensions.Height;

                // Axis
                chartWidth += yAxis.GetDimensions().Width + xAxis.GetDimensions().Width;
                chartHeight += yAxis.GetDimensions().Height + xAxis.GetDimensions().Height;

                // Make sure the width is at least big enough for Title and SubTitle
                if (chartWidth < titleDimensions.Width)
                    chartWidth = (int)titleDimensions.Width;

                _chartDimensions = new Size(chartWidth, chartHeight); // TODO fix
            }

            return _chartDimensions;
        }

        public void AddDataSeries(DataSeries<TXAxisDataType, TYAxisDataType, TDataPointData> ds)
        {
            ChartData.Add(ds);
        }

        public abstract void PlotData(Graphics g);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        /// <returns></returns>
        public Point GetChartPosition(TXAxisDataType xValue, TYAxisDataType yValue)
        {
            return new Point(_xAxis1.GetAxisPosition(xValue), _yAxis.GetAxisPosition(yValue));
        }

        /// <summary>
        /// A debug method which outputs chart dimensions.
        /// </summary>
        public void DebugOutput_ChartDimensions()
        {
            SizeF xAxisDimensions = _xAxis1.GetDimensions();
            SizeF yAxisDimensions = _yAxis.GetDimensions();
            SizeF titleDimensions = GetTitleDimensions();

            Console.WriteLine($"Margin (L;T;R;B): {GetMargin(ElementPosition.LEFT)}; {GetMargin(ElementPosition.TOP)}; " +
                $"{GetMargin(ElementPosition.RIGHT)}; {GetMargin(ElementPosition.BOTTOM)}");
            Console.WriteLine($"Border (p;w): {_layout.Border}; {(int)BorderPen.Width}");
            Console.WriteLine($"Padding (L;T;R;B): {GetPadding(ElementPosition.LEFT)}; {GetPadding(ElementPosition.TOP)}; " +
                $"{GetPadding(ElementPosition.RIGHT)}; {GetPadding(ElementPosition.BOTTOM)}");
            Console.WriteLine($"Titles (p1;p2;w;h): {_title.Position}; {_subTitle.Position}; {(int)titleDimensions.Width}; {(int)titleDimensions.Height}");
            Console.WriteLine($"x-Axis (lp;tp;w;h): {_layout.xAxisLabels}; {_layout.xAxisTicks}; {(int)xAxisDimensions.Width}; {(int)xAxisDimensions.Height}");
            Console.WriteLine($"y-Axis (lp;tp;w;h): {_layout.yAxisLabels}; {_layout.yAxisTicks}; {(int)yAxisDimensions.Width}; {(int)yAxisDimensions.Height}");
        }

        public void DebugOutput_DataSeries()
        {
            foreach (DataSeries<TXAxisDataType, TYAxisDataType, TDataPointData> ds in ChartData)
            {
                Console.WriteLine($"{ds.Name}  -  {ds.Color}");
                foreach (DataPoint<TXAxisDataType, TYAxisDataType, TDataPointData> dp in ds.DataPoints)
                {
                    Console.WriteLine($"   axis1: {dp.xAxisCoord}, axis2: {dp.yAxisCoord}");
                }
            }
        }

        public AbstractChartAxis<TXAxisDataType> GetXAxis()
        {
            return _xAxis1;
        }

        public AbstractChartAxis<TYAxisDataType> GetYAxis()
        {
            return _yAxis;
        }

        /// <summary>
        /// Get the Minimum and the Maximum values for the X Axis.
        /// </summary>
        /// <returns></returns>
        public virtual (TXAxisDataType Min, TXAxisDataType Max) GetXAxisBounds()
        {
            TXAxisDataType Min = default;
            TXAxisDataType Max = default;

            if (ChartData.Count > 0)
            {
                //IEnumerable<TXAxisDataType> xValues = ChartData.SelectMany(ds => ds.DataPoints.Select(ds => ds.xAxisCoord));
                IEnumerable<TXAxisDataType> xValues = ChartData.SelectMany(ds => ds.DataPoints.Select(dp => dp.xAxisCoord));
                Min = xValues.Min();
                Max = xValues.Max();
            }
            else
                throw new DataException("No data available; cannot calculate the minimum and maximum values for X Axis.");

            return (Min, Max);
        }

        /// <summary>
        /// Get the Minimum and the Maximum values for the Y Axis.
        /// </summary>
        /// <returns></returns>
        public virtual (TYAxisDataType Min, TYAxisDataType Max) GetYAxisBounds()
        {
            TYAxisDataType Min = default;
            TYAxisDataType Max = default;

            if (ChartData.Count > 0)
            {
                IEnumerable<TYAxisDataType> yValues = ChartData.SelectMany(ds => ds.DataPoints.Select(ds => ds.yAxisCoord));
                Min = yValues.Min();
                Max = yValues.Max();
            }
            else
                throw new DataException("No data available; cannot calculate the minimum and maximum values for Y Axis.");


            return (Min, Max);
        }

        /// <summary>
        /// Return all of the Data Series names.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetDataSeriesNames()
        {
            return ChartData.Select(ds => ds.Name);
        }
    }
}
