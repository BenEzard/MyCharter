using MyCharter.ChartElements.Axis;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MyCharter
{
    public abstract class AbstractChart
    {
        private ChartType ChartType { get; }

        /// <summary>
        /// The margin goes around all four sides of the image.
        /// Measured in pixels.
        /// </summary>
        public int MarginWidth { get; set; } = 10;

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
                _title.Dimensions = null;
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
                _subTitle.Dimensions = null;
            }
       }

        /// <summary>
        /// The font which is used for the Chart Title.
        /// </summary>
        private Font SubTitleFont = new Font("Arial", 12 * 1.33f, FontStyle.Bold, GraphicsUnit.Point);

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
        public void SetAxis(Axis type, AxisLabelPosition labelPosition, AbstractChartAxis axis)
        {
            bool axisAlreadyInUse = false;
            bool conflictingAxisCombination = false;
            bool labelPositionError = false;

            int index = (int)type;

            if (axes[index] != null)
                axisAlreadyInUse = true;

            if ((axes[0] != null) && (axes[0].Format == AxisFormat.TIME_SCALE) && (axes[1] != null) && (axes[1].Format != AxisFormat.DATA_SERIES))
                conflictingAxisCombination = true;

            if ((type == Axis.X) && ((labelPosition == AxisLabelPosition.LEFT) || (labelPosition == AxisLabelPosition.RIGHT))
                || (type == Axis.Y) && ((labelPosition == AxisLabelPosition.TOP) || (labelPosition == AxisLabelPosition.BOTTOM)))
                labelPositionError = true;

            if (axisAlreadyInUse)
                throw new ArgumentException($"Axis {type} has already been set.");
            else if (conflictingAxisCombination)
                throw new ArgumentException($"Axis types are conflicting. {axes[0].AxisXY} is {axes[0].Format} and {axes[1].AxisXY} is {axes[1].Format}.");
            else if (labelPositionError)
                throw new ArgumentException($"Axis label positions are not valid.");
            else
            {
                // Apply the requested value
                axis.AxisXY = type;
                axis.LabelPosition = labelPosition;

                axes[index] = axis;

                if (axis is AbstractScaleAxis)
                    axes[index].GenerateAxisValues();
            }   
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
                else
                    Console.WriteLine();
            }

            if (rValue == null)
                throw new ArgumentException($"Can't find axis {axis}. Has it been set?");

            return rValue;
        }

        public void GenerateChart()
        {
            SizeF chartDimension = GetChartDimensions();
            Bitmap bmp = new Bitmap((int)chartDimension.Width+1, (int)chartDimension.Height+1);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(ImageBackgroundColor);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Draw border
                if (BorderPen != null)
                    g.DrawRectangle(BorderPen, new Rectangle(0, 0, (int)chartDimension.Width, (int)chartDimension.Height));

                // Local variable to keep track of starting position when drawing certain items. Variable reused.
                Point offset = new Point(MarginWidth, MarginWidth);

                // Draw Title
                DrawTitles(g, ref offset);

                var xAxis = GetAxis(Axis.X);
                var yAxis = GetAxis(Axis.Y);

                Point xAxisTicksOffset = offset;
                //Point xAxisTicksOffset = new Point(offset.X - (2 * MarginWidth), offset.Y);
                if (xAxis.LabelPosition == AxisLabelPosition.TOP)
                {
                    // ---- Draw Ticks -------------------------------------------------------------------------------------
                    // if the y-axis labels are on the left, then offset the x-axis.
                    int lhsOfTicks = (int)yAxis.GetAxisLabelDimensions().Width;
                    offset.Y += xAxis.LabelPadding + (int)xAxis.GetMaxLabelDimensions().Height;
                    if (yAxis.LabelPosition == AxisLabelPosition.LEFT)
                    {
                        offset.X += lhsOfTicks;
                    }

                    xAxis.DrawTicks(g, offset);
                    xAxisTicksOffset = offset;

                    // ---- Draw Axis Label --------------------------------------------------------------------------------
                    offset.Y -= (int)xAxis.GetMaxLabelDimensions().Height;
                    xAxis.DrawAxisLabels(g, offset);
                }

                // ---- Draw Y-Axis ------------------------------------------------------------------------------------
                int y = xAxisTicksOffset.Y + xAxis.MajorTickLength + yAxis.LabelPadding;
                yAxis.DrawTicks(g, new Point(offset.X, y));
                yAxis.DrawAxisLabels(g, offset);

                PlotData(g);

                xAxis.DrawMajorGridLines(g);
            }

            bmp.Save(OutputFile, ImageFormat.Png);
        }

        /// <summary>
        /// Draw the titles onto the image, adjusting the offset.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="offset"></param>
        internal void DrawTitles(Graphics g, ref Point offset)
        {
            if (_title != null)
            {
                g.DrawString(Title, TitleFont, Brushes.Black, offset.X, offset.Y);
                offset.Y += (int)_title.Dimensions.Value.Height;
            }
            if (_subTitle != null)
            {
                g.DrawString(SubTitle, SubTitleFont, Brushes.Black, offset.X, offset.Y);
                offset.Y += (int)_subTitle.Dimensions.Value.Height;
            }

        }

        public void CalculateElementSizes()
        {
            CalculateTitleDimensions();

            foreach (AbstractChartAxis axis in axes)
            {
                axis.MeasureLabels();
            }
        }

        /// <summary>
        /// Calculate the dimensions that the Titles will take on the image (based on the font being used).
        /// </summary>
        public void CalculateTitleDimensions()
        {
            // Create a temporary BMP for 'sketching'
            Bitmap tempBMP = new Bitmap(400, 400);
            Graphics tempGraphics = Graphics.FromImage(tempBMP);

            if (String.IsNullOrEmpty(Title) == false)
            {
                _title.Dimensions = tempGraphics.MeasureString(_title.Text, TitleFont);
            }

            if (String.IsNullOrEmpty(SubTitle) == false)
            {
                _subTitle.Dimensions = tempGraphics.MeasureString(_subTitle.Text, TitleFont);
            }

            tempBMP.Dispose();
        }

        /// <summary>
        /// Get the Chart dimensions
        /// TODO Does this belong here, or should it be implemented in a lower level implementation?
        /// </summary>
        /// <returns></returns>
        public SizeF GetChartDimensions()
        {
            CalculateElementSizes();

            int height = 0;

            var xAxis = GetAxis(Axis.X);
            var yAxis = GetAxis(Axis.Y);

            var xAxisLabels = xAxis.GetAxisLabelDimensions();
            var yAxisLabels = yAxis.GetAxisLabelDimensions();

            // ---- Overall Chart Dimensions --------------------------------
            // ---- Calculate width of the chart
            int width = MarginWidth + (int)xAxisLabels.Width + (int)yAxisLabels.Width + MarginWidth;
            int titleHeight = 0;
            
            // Make sure the width is at least big enough for Title and SubTitle
            if (_title != null)
            {
                titleHeight += (int)_title.Dimensions.Value.Height;
                if (width < _title.Dimensions.Value.Width)
                    width = (int)_title.Dimensions.Value.Width;
            }
            if (_subTitle != null)
            {
                titleHeight += (int)_subTitle.Dimensions.Value.Height;
                if (width < _subTitle.Dimensions.Value.Width)
                    width = (int)_subTitle.Dimensions.Value.Width;
            }

            // ---- Calculate height of the chart
            height += MarginWidth 
                + titleHeight
                + (int)xAxisLabels.Height 
                + (int)yAxisLabels.Height 
                + MarginWidth;

            return new SizeF(width, height+100);  // TODO fix
        }

        public abstract void PlotData(Graphics g);

    }
}
