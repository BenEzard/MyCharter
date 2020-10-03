using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MyCharter.Charts
{
    public class DurationChart : AbstractChart<DateTime, string, Duration>
    {
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
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Return the Minimum and Maximum values for the X Axis.
        /// </summary>
        /// <returns></returns>
        public override (DateTime Min, DateTime Max) GetXAxisBounds()
        {
            DateTime Min = ChartData.SelectMany(ds => ds.DataPoints.Select(dp => dp.DataPointData.StartDateTime)).Min();
            DateTime Max = ChartData.SelectMany(ds => ds.DataPoints.Select(dp => dp.DataPointData.EndDateTime)).Max();

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
