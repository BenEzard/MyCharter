﻿using MyCharter.ChartElements;
using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using MyCharter.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MyCharter
{
    public abstract class AbstractChart
    {
        private ChartType ChartType { get; }

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
        /// The axes to be displayed on the chart
        /// </summary>
        private AbstractChartAxis[] axes = new AbstractChartAxis[2] { null, null };

        /// <summary>
        /// The Pen (colour and width) of a border that will be drawn around the image (inset by MarginWidth).
        /// </summary>
        public Pen BorderPen = new Pen(Color.DarkGray, 1);

        /// <summary>
        /// The background colour of the image.
        /// </summary>
        public Color ImageBackgroundColor = Color.White;

        /// <summary>
        /// TODO should be generic at this point?
        /// </summary>
        public List<DataSeries<DateTime, int>> ChartData {get; set;} = new List<DataSeries<DateTime, int>>();

        public AbstractChart(ChartType chartType)
        {
            ChartType = chartType;
        }

        /// <summary>
        /// Set the axis with a specific type of ChartAxis.
        /// If successful, assigns the Axis type (x,y) and label position to the axis.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="labelPosition"></param>
        /// <param name="axis"></param>
        public void SetAxis(Axis type, ElementPosition labelPosition, AbstractChartAxis axis)
        {
            // Will throw an ArgumentException if it doesn't work.
            ValidateAxis(type, labelPosition, axis);

            int index = (int)type;
            // Apply the requested value
            axis.AxisXY = type;
            axis.AxisPosition = labelPosition;
            axis.ParentChart = this;
            axes[index] = axis;

            axis.InitialAxisPreparation();
        }

        /// <summary>
        /// Set the axis with a specific type of ChartAxis.
        /// If successful, assigns the Axis type (x,y) and label position to the axis.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="labelPosition"></param>
        /// <param name="axis"></param>
        /// <param name="axisWidth"></param>
        public void SetAxis(Axis type, ElementPosition labelPosition, AbstractChartAxis axis, AxisWidth axisWidth)
        {
            // Will throw an ArgumentException if it doesn't work.
            ValidateAxis(type, labelPosition, axis);
         
            int index = (int)type;
            // Apply the requested value
            axis.AxisXY = type;
            axis.AxisPosition = labelPosition;
            axis.ParentChart = this;
            axis.AxisWidth = axisWidth;
            axes[index] = axis;

            axis.InitialAxisPreparation();
        }

        private Point GetLabelOffset(Axis axis)
        {
            int x = 0;
            int y = 0;

            if (BorderPen != null)
            {
                x += (int)BorderPen.Width;
                y += (int)BorderPen.Width;
            }

            var xAxis = GetAxis(Axis.X);
            var yAxis = GetAxis(Axis.Y);

            switch (axis)
            {
                // Get the label position of the x-axis.
                case Axis.X:
                    switch (yAxis.AxisPosition)
                    {
                        case ElementPosition.LEFT:
                            x = GetMargin(ElementPosition.LEFT)
                                + GetPadding(ElementPosition.LEFT)
                                + yAxis.GetDimensions().Width;
                            break;
                        case ElementPosition.RIGHT:
                            x *= -1;
                            x = GetChartDimensions().Width;
                            x -= (GetMargin(ElementPosition.RIGHT)
                                + GetPadding(ElementPosition.RIGHT)
                                + yAxis.GetDimensions().Width);
                            break;
                    }
                    y = GetMargin(ElementPosition.TOP)
                        + GetPadding(ElementPosition.TOP)
                        + GetTitleDimensions().Height
                        + yAxis.GetDimensions().Height
                        + yAxis.MajorTickLength
                        + yAxis.AxisPadding;

                    break;
                case Axis.Y:
                    switch (xAxis.AxisPosition)
                    {
                        case ElementPosition.TOP:
                            x = GetMargin(ElementPosition.TOP)
                                + GetPadding(ElementPosition.TOP);
                            y = GetMargin(ElementPosition.TOP)
                                + GetPadding(ElementPosition.TOP)
                                + GetTitleDimensions().Height
                                + xAxis.GetDimensions().Height;
                            break;
                        case ElementPosition.BOTTOM:
                            x = GetMargin(ElementPosition.TOP)
                                + GetPadding(ElementPosition.TOP);
                            y = GetMargin(ElementPosition.TOP)
                                + GetPadding(ElementPosition.TOP)
                                + GetTitleDimensions().Height
                                + yAxis.GetDimensions().Height;
                            break;
                    }
                    break;
            }

            return new Point(x, y);
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

            var xAxis = GetAxis(Axis.X);
            var yAxis = GetAxis(Axis.Y);

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

            GetAxis(Axis.X).AxisCoords = tempXAxis;
            GetAxis(Axis.Y).AxisCoords = tempYAxis;
        }

        public int GetStartOfDrawingSpace(ElementPosition side)
        {
            int p = 0;
            switch (side)
            {
                case ElementPosition.LEFT:
                    p += GetMargin(side) + GetPadding(side);
                    if (BorderPen != null)
                        p += (int)BorderPen.Width;
                    p += 1; // increment by 1 so that it returns the START of the drawing space (not the end of the non-drawing space).
                    break;
                case ElementPosition.TOP:
                    p += GetMargin(side) + GetPadding(side);
                    if (BorderPen != null)
                        p += (int)BorderPen.Width;
                    p += GetTitleDimensions().Height;
                    p += 1; // increment by 1 so that it returns the START of the drawing space (not the end of the non-drawing space).
                    break;
                case ElementPosition.RIGHT:
                    p += GetMargin(side) + GetPadding(side);
                    if (BorderPen != null)
                        p += (int)BorderPen.Width;
                    p -= 1; // increment by 1 so that it returns the START of the drawing space (not the end of the non-drawing space).
                    break;
                case ElementPosition.BOTTOM:
                    p += GetMargin(side) + GetPadding(side);
                    if (BorderPen != null)
                        p += (int)BorderPen.Width;
                    p -= 1; // increment by 1 so that it returns the START of the drawing space (not the end of the non-drawing space).
                    break;
            }
            return p;
        }

        public void Debug_GetLayout()
        {
            var xAxis = GetAxis(Axis.X);
            var yAxis = GetAxis(Axis.Y);

            Console.WriteLine("Layout\n");
            Console.WriteLine($"Border = {_layout.Border}\n" +
                $"Title = {_layout.Title}\n" +
                $"SubTitle = {_layout.SubTitle}\n" +
                $"y-Axis Labels = { _layout.yAxisLabels}\n" +
                $"y-Axis Ticks = { _layout.yAxisTicks}\n" +
                $"x-Axis Labels = { _layout.xAxisLabels}\n" +
                $"x-Axis Ticks = { _layout.xAxisTicks}");

            Console.WriteLine($"y-Axis (x,y,h,w) = {yAxis.AxisCoords.X}, {yAxis.AxisCoords.Y}, {yAxis.GetDimensions().Height}, {yAxis.GetDimensions().Width}");
            Console.WriteLine($"x-Axis (x,y,h,w) = {xAxis.AxisCoords.X}, {xAxis.AxisCoords.Y}, {xAxis.GetDimensions().Height}, {xAxis.GetDimensions().Width}");

        }

        /// <summary>
        /// Validates the Axis.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="labelPosition"></param>
        /// <param name="axis"></param>
        public void ValidateAxis(Axis type, ElementPosition labelPosition, AbstractChartAxis axis)
        {
            int index = (int)type;

            if (axes[index] != null)
                throw new ArgumentException($"Axis {type} has already been set.");

            if ((axes[0] != null) && (axes[0].Format == AxisFormat.TIME_SCALE) && (axes[1] != null) && (axes[1].Format != AxisFormat.DATA_SERIES))
                throw new ArgumentException($"Axis types are conflicting. {axes[0].AxisXY} is {axes[0].Format} and {axes[1].AxisXY} is {axes[1].Format}.");

            if ((type == Axis.X) && ((labelPosition == ElementPosition.LEFT) || (labelPosition == ElementPosition.RIGHT))
                || (type == Axis.Y) && ((labelPosition == ElementPosition.TOP) || (labelPosition == ElementPosition.BOTTOM)))
                throw new ArgumentException($"Axis label positions are not valid.");
        }

        /// <summary>
        /// Return the specified axis.
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public AbstractChartAxis GetAxis(Axis axis)
        {
            AbstractChartAxis rValue = null;

            for (int i = 0; i < axes.Length; i++)
            {
                if (axes[i].AxisXY == axis)
                {
                    rValue = axes[i];
                    break;
                }
            }

            if (rValue == null)
                throw new ArgumentException($"Can't find axis {axis}. Has it been set?");

            return rValue;
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
            var xAxis = GetAxis(Axis.X);
            var yAxis = GetAxis(Axis.Y);

            CalculateInitialLayout();

            xAxis.FinaliseAxisLayout();
            yAxis.FinaliseAxisLayout();

            SizeF chartDimension = GetChartDimensions();

            Bitmap bmp = new Bitmap((int)chartDimension.Width, (int)chartDimension.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(ImageBackgroundColor);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                DrawBorder(g);

                DrawTitles(g);

                if (xAxis.IsVisible)
                {
                    xAxis.DrawAxis(g, bmp);
                }

                if (yAxis.IsVisible)
                {
                    yAxis.DrawAxis(g, bmp);
                }

                Pen rectPen = new Pen(Brushes.Red, 1);
                rectPen.DashPattern = new float[] { 10, 10 };
                ImageMethods.Debug_DrawRectangle(g, new Rectangle(xAxis.AxisCoords, xAxis.GetDimensions()), rectPen);
                ImageMethods.Debug_DrawRectangle(g, new Rectangle(yAxis.AxisCoords, yAxis.GetDimensions()), rectPen);

                //yAxis.DebugOutput_ListScale();
                PlotData(g);
            }

            bmp.Save(OutputFile, ImageFormat.Png);
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
        /// Calculate the offset of where the ticks should be for a given axis.
        /// For the X-axis will return a point which is at the top-left-most point of the first tick.
        /// For the Y-axis will return a point which is at the left-most point of the first tick.
        /// 
        /// TODO: Add BOTH axis type (where axis scale is displayed on both sides of the chart.
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        private Point CalculateTickOffset(AbstractChartAxis axis)
        {
            Point offset = new Point();
            SizeF titleDimensions = GetTitleDimensions();

            int xPos = 0;
            int yPos = 0;

            var xAxis = GetAxis(Axis.X);
            var yAxis = GetAxis(Axis.Y);

            switch (axis.AxisXY)
            {
                case Axis.X:
                    // Axis on the X-plane can be at the TOP or BOTTOM of the chart.
                    // Return a point which is at the top-left-most point of the first tick.
                    xPos = GetMargin(ElementPosition.LEFT);
                    if (yAxis.AxisPosition == ElementPosition.LEFT)
                    {
                        xPos += (int)yAxis.GetMaxLabelDimensions().Width + yAxis.AxisPadding;
                    }
                    switch (axis.AxisPosition)
                    {
                        case ElementPosition.TOP:
                            yPos = GetMargin(ElementPosition.TOP) + (int)titleDimensions.Height + (int)axis.GetMaxLabelDimensions().Height + axis.AxisPadding;
                            break;
                        case ElementPosition.BOTTOM:
                            yPos = (int)GetChartDimensions().Height - (int)axis.GetMaxLabelDimensions().Height - axis.AxisPadding - axis.MajorTickLength;
                            break;
                    }
                    offset = new Point(xPos, yPos);
                    break;
                
                case Axis.Y:
                    // Axis on the Y-plane can be at the LEFT or RIGHT of the chart.
                    yPos = GetMargin(ElementPosition.TOP) + (int)titleDimensions.Height;
                    if (xAxis.AxisPosition == ElementPosition.TOP)
                    {
                        yPos += (int)xAxis.GetMaxLabelDimensions().Height + xAxis.AxisPadding;
                    }
                    switch (axis.AxisPosition)
                    {
                        case ElementPosition.LEFT:
                            xPos = GetMargin(ElementPosition.LEFT) + (int)axis.GetMaxLabelDimensions().Width + yAxis.AxisPadding;
                            break;
                        case ElementPosition.RIGHT:
                            xPos = (int)GetChartDimensions().Width - GetMargin(ElementPosition.RIGHT) - (int)yAxis.GetMaxLabelDimensions().Width - yAxis.AxisPadding - yAxis.MajorTickLength;
                            break;
                    }
                    offset = new Point(xPos, yPos);
                    break;
            }

            return offset;
        }

        /// <summary>
        /// Draw the titles onto the image, adjusting the offset.
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
                var xAxis = GetAxis(Axis.X);
                var yAxis = GetAxis(Axis.Y);

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

                /*                if (yAxis.AxisPosition == ElementPosition.LEFT)
                                {
                                    chartWidth = _layout.yAxisLabels.X + yAxis.GetDimensions().Width + xAxis.GetDimensions().Width + GetPadding(ElementPosition.RIGHT) + GetMargin(ElementPosition.RIGHT);
                                    if (BorderPen != null)
                                        chartWidth += (int)BorderPen.Width;
                                }
                                else if (yAxis.AxisPosition == ElementPosition.RIGHT)
                                {
                                    chartWidth = _layout.yAxisLabels.X + yAxis.GetDimensions().Width + GetPadding(ElementPosition.RIGHT) + GetMargin(ElementPosition.RIGHT);
                                    if (BorderPen != null)
                                        chartWidth += (int)BorderPen.Width;
                                }

                                if (xAxis.AxisPosition == ElementPosition.TOP)
                                {
                                    chartHeight = _layout.xAxisLabels.X + xAxis.GetDimensions().Height + yAxis.GetDimensions().Height + GetPadding(ElementPosition.BOTTOM) + GetMargin(ElementPosition.BOTTOM);
                                    if (BorderPen != null)
                                        chartHeight += (int)BorderPen.Width;
                                }
                                else if (xAxis.AxisPosition == ElementPosition.BOTTOM)
                                {
                                    chartHeight = _layout.xAxisLabels.X + xAxis.GetDimensions().Height + GetPadding(ElementPosition.BOTTOM) + GetMargin(ElementPosition.BOTTOM);
                                    if (BorderPen != null)
                                        chartHeight += (int)BorderPen.Width;
                                }

                                //                xAxis.CalculateLabelPositions(_layout.xAxisLabels);
                                //                yAxis.CalculateLabelPositions(_layout.yAxisLabels);

                                chartWidth += xAxis.GetDimensions().Width + yAxis.GetDimensions().Width;
                                chartHeight += xAxis.GetDimensions().Height + yAxis.GetDimensions().Height;


                                var lastXElement = xAxis.Entries[xAxis.Entries.Count - 1];
                                var lastYElement = yAxis.Entries[yAxis.Entries.Count - 1];
                                int w = lastXElement.Label.Position.X + (int)lastXElement.Label.Dimensions.Value.Width + GetPadding(ElementPosition.RIGHT) + GetMargin(ElementPosition.RIGHT);
                                int h = lastYElement.Label.Position.Y + (int)lastXElement.Label.Dimensions.Value.Height + GetPadding(ElementPosition.BOTTOM) + GetMargin(ElementPosition.BOTTOM);
                                Console.WriteLine($"new size is {w}, {h}");

                                _chartDimensions = new Size(w, h); // TODO fix*/

            }

            return _chartDimensions;
        }

        public void AddDataSeries(DataSeries<DateTime,int> ds)
        {
            ChartData.Add(ds);
        }

        public abstract void PlotData(Graphics g);

        public Point GetPosition(object xLabel, object yLabel)
        {
            Point rValue = new Point(-1, -1);

            var xAxis = GetAxis(Axis.X);
            var yAxis = GetAxis(Axis.Y);

            var xPoint = xAxis.GetAxisPositionOfLabel(xAxis.FormatLabelString(xLabel));
            var yPoint = yAxis.GetAxisPositionOfLabel(yAxis.FormatLabelString(yLabel));

            if ((xPoint.HasValue) && (yPoint.HasValue))
                rValue = new Point(xPoint.Value.X, yPoint.Value.Y);

            return rValue;
        }

        /// <summary>
        /// A debug method which outputs chart dimensions.
        /// </summary>
        public void DebugOutput_ChartDimensions()
        {
            SizeF xAxisDimensions = GetAxis(Axis.X).GetDimensions();
            SizeF yAxisDimensions = GetAxis(Axis.Y).GetDimensions();
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
            foreach (DataSeries<DateTime, int> ds in ChartData)
            {
                Console.WriteLine($"{ds.Name}  -  {ds.Color}");
                foreach (DataPoint<DateTime, int> dp in ds.DataPoints)
                {
                    Console.WriteLine($"   axis1: {dp.AxisCoord1}, axis2: {dp.AxisCoord2}");
                }
            }
        }
    }
}
