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
        private ImageElement _title = null;

        /// <summary>
        /// The title that is displayed at the top of the chart.
        /// </summary>
        public string Title {
            get
            {
                if (_title == null)
                    return null;
                else 
                    return _title.Label;
            }
            set
            {
                _title = new ImageElement(value);
                _title.LabelDimensions = null;
            }
        }

        /// <summary>
        /// The font which is used for the Chart Title.
        /// </summary>
        private Font TitleFont = new Font("Arial", 14 * 1.33f, FontStyle.Bold, GraphicsUnit.Point);

        /// <summary>
        /// The ImageElement representation of the ChartSubTitle.
        /// </summary>
        private ImageElement _subTitle = null;

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
                    return _subTitle.Label;
            }
            set
            {
                _subTitle = new ImageElement(value);
                _subTitle.LabelDimensions = null;
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
                Console.WriteLine($"{type} set to index {index} {axis} : {axes[index].AxisXY}");

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
            CalculateElementSizes();
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
                Point offset = new Point(0, 0);

                // Draw Title
                DrawTitles(g, ref offset);

                var xAxis = GetAxis(Axis.X);
                var yAxis = GetAxis(Axis.Y);

                yAxis.DrawAxis(g, ref offset);

                // If the yAxis labels are on the left, then we want to leave a gap for them.
                if (yAxis.LabelPosition == AxisLabelPosition.LEFT)
                {
                    offset.X += (int)yAxis.GetMaxLabelDimensions().Width;
                }

                xAxis.DrawAxis(g, ref offset);
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
                offset.Y += (int)_title.LabelDimensions.Value.Height;
            }
            if (_subTitle != null)
            {
                g.DrawString(SubTitle, SubTitleFont, Brushes.Black, offset.X, offset.Y);
                offset.Y += (int)_subTitle.LabelDimensions.Value.Height;
            }

        }

        public void CalculateElementSizes()
        {
            CalculateTitleDimensions();

            foreach (AbstractChartAxis axis in axes)
            {
                axis.CalculateLabelDimensions();
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
                _title.LabelDimensions = tempGraphics.MeasureString(_title.Label, TitleFont);
            }

            if (String.IsNullOrEmpty(SubTitle) == false)
            {
                _subTitle.LabelDimensions = tempGraphics.MeasureString(_subTitle.Label, TitleFont);
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
            int height = 0;

            var xAxis = GetAxis(Axis.X);
            var yAxis = GetAxis(Axis.Y);

            var xAxisLabels = xAxis.GetLabelDimensions();
            var yAxisLabels = yAxis.GetLabelDimensions();

            // ---- Overall Chart Dimensions --------------------------------
            // ---- Calculate width of the chart
            int width = MarginWidth + (int)xAxisLabels.Width + (int)yAxisLabels.Width + MarginWidth;
            
            // Make sure the width is at least big enough for Title and SubTitle
            if (_title != null)
            {
                if (width < _title.LabelDimensions.Value.Width)
                    width = (int)_title.LabelDimensions.Value.Width;
            }
            if (_subTitle != null)
            {
                if (width < _subTitle.LabelDimensions.Value.Width)
                    width = (int)_subTitle.LabelDimensions.Value.Width;
            }

            // ---- Calculate height of the chart
            height += MarginWidth + (int)xAxisLabels.Height + (int)yAxisLabels.Height + MarginWidth;

            return new SizeF(width+20, height+20); // TODO : the calculations aren't quite right
        }

    }
}
