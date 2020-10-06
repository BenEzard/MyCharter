using MyCharter.ChartElements.DataSeries;
using System;
using System.Drawing;
using System.Linq;

namespace MyCharter.Charts
{
    public class DurationChart : AbstractChart<DateTime, string, Duration>
    {
        private int _barHeight;
        public int BarHeight
        {
            get => _barHeight;
            set
            {
                if (value > GetYAxis().PixelsPerIncrement)
                    throw new ArgumentException("Bar Height must be less than or equal to PixelsPerIncrement.");

                _barHeight = value;
            }
        }

        public DurationChart() : base()
        {
        }

        public override void CalculateDataPointLabelDimensions()
        {
            // Create a temporary BMP for 'sketching'
            Bitmap tempBMP = new Bitmap(300, 300);
            Graphics tempGraphics = Graphics.FromImage(tempBMP);

            var yAxis = GetYAxis();

           /* foreach (DataSeries<DateTime, string, Duration> dataSeries in ChartData)
            {
                if (dataSeries.DataSeriesLabelFormat != AxisLabelFormat.NONE)
                {
                    foreach (DataPoint<Duration, string> dataPoint in dataSeries.DataPoints)
                    {
                        string label = yAxis.FormatLabelString(dataPoint.yAxisCoord, false);
                        dataPoint.DataPointLabel = new ImageText(label);
                        // Because this is a vertical bar chart, we know that the DataPoint value we want to label will be on the TYAxis value.
                        SizeF stringMeasurement = tempGraphics.MeasureString(label, yAxis.DataPointLabelFont);
                        dataPoint.DataPointLabel.Dimensions = stringMeasurement;
                    }
                }
            }*/

            tempBMP.Dispose();
        }

        public override void PlotData(Graphics g)
        {
            BarHeight = GetYAxis().PixelsPerIncrement - 10;

            foreach (DataSeries<DateTime, string, Duration> dataSeries in ChartData)
            {
                foreach (DataPoint<DateTime, string, Duration> dataPoint in dataSeries.DataPoints)
                {
                    Point startPoint = GetChartPosition(dataPoint.DataPointData.StartDateTime, dataSeries.Name);
                    Point endPoint = GetChartPosition(dataPoint.DataPointData.EndDateTime, dataSeries.Name);
                    g.FillRectangle(new SolidBrush(dataSeries.Color), new Rectangle(startPoint.X, (startPoint.Y - (_barHeight)/2), (endPoint.X - startPoint.X), _barHeight));
                    TimeSpan durationLength = (dataPoint.DataPointData.EndDateTime - dataPoint.DataPointData.StartDateTime);
                    g.DrawString(durationLength.Hours.ToString().PadLeft(2, '0')+":"+durationLength.Minutes.ToString().PadLeft(2, '0'),
                        new Font("Arial", 8 * 1.33f, FontStyle.Regular, GraphicsUnit.Point), 
                        Brushes.Black, 
                        new Point(startPoint.X, startPoint.Y - (_barHeight/2)));
                }
            }
        }

        /// <summary>
        /// Return the Minimum and Maximum values for the X Axis.
        /// </summary>
        /// <returns></returns>
        public override (DateTime Min, DateTime Max) GetXAxisBounds()
        {
            DateTime Min = ChartData.SelectMany(ds => ds.DataPoints.Select(dp => dp.DataPointData.StartDateTime)).Min();
            DateTime Max = ChartData.SelectMany(ds => ds.DataPoints.Select(dp => dp.DataPointData.EndDateTime)).Max();
            // temp fix - make sure the max is greater than the chart
            Max = Max.AddMinutes(1);
            return (Min, Max);
        }

        /// <summary>
        /// Get the Minimum and the Maximum values for the Y Axis.
        /// </summary>
        /// <returns></returns>
        public override (string Min, string Max) GetYAxisBounds()
        {
            string Min = ChartData.Select(ds => ds.Name).First();
            string Max = ChartData.Select(ds => ds.Name).Last();

            return (Min, Max);
        }

    }
}
