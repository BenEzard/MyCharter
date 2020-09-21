using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MyCharter.Charts
{
    public class DurationChart<TXAxis, TYAxis> : AbstractChart<TXAxis, TYAxis>
    {
        public DurationChart() : base(ChartType.DURATION_CHART)
        {
        }

        public override void CalculateDataPointLabelDimensions()
        {
            // Create a temporary BMP for 'sketching'
            Bitmap tempBMP = new Bitmap(300, 300);
            Graphics tempGraphics = Graphics.FromImage(tempBMP);

            var yAxis = GetYAxis();

            foreach (DataSeries<TXAxis, TYAxis> dataSeries in ChartData)
            {
                if (dataSeries.DataSeriesLabelFormat != AxisLabelFormat.NONE)
                {
                    foreach (DataPoint<TXAxis, TYAxis> dataPoint in dataSeries.DataPoints)
                    {
                        string label = yAxis.FormatLabelString(dataPoint.yAxisCoord, false);
                        dataPoint.DataPointLabel = new ImageText(label);
                        // Because this is a vertical bar chart, we know that the DataPoint value we want to label will be on the TYAxis value.
                        SizeF stringMeasurement = tempGraphics.MeasureString(label, yAxis.DataPointLabelFont);
                        dataPoint.DataPointLabel.Dimensions = stringMeasurement;
                    }
                }
            }

            tempBMP.Dispose();
        }

        public override void PlotData(Graphics g)
        {
            //throw new NotImplementedException();
        }

    }
}
