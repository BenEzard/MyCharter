using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using MyCharter.Util;
using System;
using System.Drawing;

namespace MyCharter.Charts
{
    public class StackedVerticalBarChart<TXAxisDataType, TYAxisDataType, TDataPointData> : AbstractChart<TXAxisDataType, TYAxisDataType, TDataPointData>
    {
        private int _barWidth;
        public int BarWidth { 
            get => _barWidth;
            set {
                if (value > GetXAxis().PixelsPerIncrement)
                    throw new ArgumentException("Bar Width must be less than or equal to PixelsPerIncrement.");

                _barWidth = value;
            }
        }

        public bool DisplayVerticalBarTotals { get; set; } = true;

        public StackedVerticalBarChart() : base()
        {
        }

        /// <summary>
        /// Calculate the size of the labels attached to the DataPoint.
        /// TODO: Not really sure where this method should sit. How do you know which axis coordinate (if any) the label should be based off?
        /// </summary>
        public override void CalculateDataPointLabelDimensions()
        {
            // Create a temporary BMP for 'sketching'
            Bitmap tempBMP = new Bitmap(300, 300);
            Graphics tempGraphics = Graphics.FromImage(tempBMP);

            var yAxis = GetYAxis();

            foreach (DataSeries<TXAxisDataType, TYAxisDataType, TDataPointData> dataSeries in ChartData)
            {
                if (dataSeries.DataSeriesLabelFormat != AxisLabelFormat.NONE)
                {
                    foreach (DataPoint<TXAxisDataType, TYAxisDataType, TDataPointData> dataPoint in dataSeries.DataPoints)
                    {
                        string label = yAxis.FormatLabelString(dataPoint.yAxisCoord);
                        dataPoint.DataPointLabel = new ImageText(label);
                        // Because this is a vertical bar chart, we know that the DataPoint value we want to label will be on the TYAxis value.
                        SizeF stringMeasurement = tempGraphics.MeasureString(label, yAxis.DataPointLabelFont);
                        dataPoint.DataPointLabel.Dimensions = stringMeasurement;
                    }
                }
            }

            tempBMP.Dispose();
        }

        /// <summary>
        /// Calculate the position of the DataPoint labels.
        /// </summary>
        /// <param name="dataPoint"></param>
        private void CalculateDataLabelPosition(DataPoint<TXAxisDataType, TYAxisDataType, TDataPointData> dataPoint)
        {
            int dataPointLabelHeight = (int)dataPoint.DataPointLabel.Dimensions.Value.Height;
            int dataPointLabelWidth = (int)dataPoint.DataPointLabel.Dimensions.Value.Width;
            Rectangle dataPointRectangle = (Rectangle)dataPoint.GraphicalRepresentation;
            int dpLabelXPos = -1;
            int dpLabelYPos = dataPointRectangle.Y + ((dataPointRectangle.Height - dataPointLabelHeight) / 2);

            if (dataPointLabelWidth >= dataPointRectangle.Width)
            {   // Data Point label is wider than the rectangle
                dpLabelXPos = dataPointRectangle.X - ((dataPointLabelWidth - dataPointRectangle.Width) / 2);
            }
            else
            {   // Rectangle is wider than the Data Point label
                dpLabelXPos = dataPointRectangle.X + ((dataPointRectangle.Width - dataPointLabelWidth) / 2);
            }
            dataPoint.DataPointLabel.Position = new Point(dpLabelXPos, dpLabelYPos);
        }

        public override void PlotData(Graphics g)
        {
            BarWidth = GetXAxis().PixelsPerIncrement - 4;
            
            // TODO: Not sure if below is kosher, pre-supposing a ScaleAxis ?
            int minorIncrement = ((AbstractScaleAxis<TYAxisDataType>)GetYAxis()).MinorIncrement;

            // Loop through the x-axis for each data point
            foreach (AxisEntry<TXAxisDataType> xEntry in GetXAxis().AxisEntries)
            {
                int totalBarValue = 0;
                int initialY = GetYAxis().GetMinimumAxisEntry().Position.Y;
                int pxPerIncrement = GetYAxis().PixelsPerIncrement;
                Font dataPointFont = GetYAxis().DataPointLabelFont;
                Rectangle dataPointRectangle = new Rectangle();
                foreach (DataSeries<TXAxisDataType, TYAxisDataType, TDataPointData> ds in ChartData)
                {
                    // Get the DataPoint on the x-axis for for each DataSeries
                    var dataPoint = ds.GetDataPointOnX(xEntry.KeyValue);
                    if (dataPoint != null)
                    {
                        totalBarValue += (int)(CastMethods.To<double>(dataPoint.yAxisCoord, 0));
                        // The x-value at this point is the axis line
                        int x = GetXAxis().GetAxisPosition(dataPoint.xAxisCoord);
                        int heightBasedOnValue = -1;
                        if (typeof(TYAxisDataType) == typeof(int)) // Will always be an int
                        {
                            heightBasedOnValue = (int)((CastMethods.To<double>(dataPoint.yAxisCoord, 0) / (double)minorIncrement) * pxPerIncrement);
                        }

                        // Get the rectangle measurements
                        dataPointRectangle = new Rectangle(new Point(x - (BarWidth / 2), initialY - heightBasedOnValue),
                            new Size(BarWidth, heightBasedOnValue));
                        dataPoint.GraphicalRepresentation = dataPointRectangle;
                        g.FillRectangle(new SolidBrush(ds.Color), dataPointRectangle);

                        if (ds.DataSeriesLabelFormat != AxisLabelFormat.NONE)
                        {
                            CalculateDataLabelPosition(dataPoint);

                            if (dataPointRectangle.Height >= dataPoint.DataPointLabel.Dimensions.Value.Height)
                            {
                                g.DrawString(dataPoint.DataPointLabel.Text, dataPointFont, Brushes.Black, dataPoint.DataPointLabel.Position);
                            }
                            else
                                dataPoint.IsDataPointLabelVisible = false;
                        }

                        initialY -= heightBasedOnValue;
                    }
                } // end foreach (DataSeries<TXAxis, TYAxis> ds in ChartData)

                // Display the total
                if ((DisplayVerticalBarTotals) && (totalBarValue > 0))
                {
                    string totalLabel = GetYAxis().FormatLabelString(totalBarValue);
                    g.DrawString(totalLabel, dataPointFont, Brushes.Black, new Point(dataPointRectangle.X, initialY-20));
                }

            }
        }
    }
}
