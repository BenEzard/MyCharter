using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using MyCharter.Util;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace MyCharter.Charts
{
    public class StackedVerticalBarChart<TXAxis, TYAxis> : AbstractChart<TXAxis, TYAxis>
    {
        private int _barWidth;
        public int BarWidth { 
            get => _barWidth;
            set {
                if (value > GetX1Axis().PixelsPerIncrement)
                    throw new ArgumentException("Bar Width must be less than or equal to PixelsPerIncrement.");

                _barWidth = value;
            }
        }

        public StackedVerticalBarChart() : base(ChartType.STACKED_VERTICAL_BAR_CHART)
        {
        }

        public override void PlotData(Graphics g)
        {
            BarWidth = GetX1Axis().PixelsPerIncrement - 4;
            
            // TODO: Not sure if below is kosher, pre-supposing a ScaleAxis ?
            int minorIncrement = ((AbstractScaleAxis<TYAxis>)GetYAxis()).MinorIncrement;

            // Loop through the x-axis for each data point
            foreach (AxisEntry<TXAxis> xEntry in GetX1Axis().AxisEntries)
            {
                int initialY = GetYAxis().GetMinimumAxisEntry().Position.Y;
                int pxPerIncrement = GetYAxis().PixelsPerIncrement;
                foreach (DataSeries<TXAxis, TYAxis> ds in ChartData)
                {
                    // Get the DataPoint on the x-axis for for each DataSeries
                    var dataPoint = ds.GetDataPointOnX(xEntry.KeyValue);
                    if (dataPoint != null)
                    {
                        Console.WriteLine($"Looking for {xEntry.Label.Text} on {ds.Name} and found {dataPoint.AxisCoord1}, {dataPoint.AxisCoord2} {ds.Color.ToString().ToUpper()}");
                        
                        // The x-value at this point is the axis line
                        int x = GetX1Axis().GetAxisPosition(dataPoint.AxisCoord1);
                        //Point p = GetChartPosition(dataPoint.AxisCoord1, dataPoint.AxisCoord2);
                        //int x = p.X;
//                        Console.WriteLine(p);
                        int heightBasedOnValue = -1;
                        if (typeof(TYAxis) == typeof(int)) // Will always be an int
                        {
                            heightBasedOnValue = (int)((CastMethods.To<double>(dataPoint.AxisCoord2, 0) / (double)minorIncrement) * pxPerIncrement);
                            Console.WriteLine($"x = {x}, height = {heightBasedOnValue}, width = {BarWidth}, px = {pxPerIncrement}");
                        }

                        // Get the rectangle measurements
                        Rectangle dataPointRectangle = new Rectangle(new Point(x - (BarWidth / 2), initialY - heightBasedOnValue),
                            new Size(BarWidth, heightBasedOnValue));
                        dataPoint.GraphicalRepresentation = dataPointRectangle;
                        g.FillRectangle(new SolidBrush(ds.Color), dataPointRectangle);

                        Console.WriteLine($"Rectangle dimensions are top = {dataPointRectangle}");
                        initialY -= heightBasedOnValue;
                    }
                }
            }


            /*foreach (DataSeries<TXAxis, TYAxis> ds in ChartData)
            {
                Rectangle dpRectangle;
                if (firstDataSeries) {
                    foreach (DataPoint<TXAxis, TYAxis> dp in ds.DataPoints)
                    {
                        Point dataPointCoord = GetChartPosition(dp.AxisCoord1, dp.AxisCoord2);

                        // Offset the x position of the bar to be halfway along the x AxisEntry.
                        Point offsetDataPointCoord = new Point(dataPointCoord.X - (BarWidth / 2), dataPointCoord.Y);

                        Console.WriteLine($"initial position = {initialY}, offsetdatapoint = {offsetDataPointCoord}");
                        dpRectangle = new Rectangle(offsetDataPointCoord, new Size(BarWidth, initialY - dataPointCoord.Y));
                        lastPlotted = offsetDataPointCoord;
                        g.FillRectangle(new SolidBrush(ds.Color), dpRectangle);
                    }
                }

                if (firstDataSeries == false)
                {
                    foreach (DataPoint<TXAxis, TYAxis> dp in ds.DataPoints)
                    {
                        g.FillRectangle(new SolidBrush(ds.Color),
                        new Rectangle(new Point(lastPlotted.Value.X, lastPlotted.Value.Y - (int)GetYAxis().GetAxisPixelsPerValue() * (int)dp.AxisCoord2),
                        new Size(BarWidth, 30)));
                    }
                }

            firstDataSeries = false;
            } */
        }
    }
}
