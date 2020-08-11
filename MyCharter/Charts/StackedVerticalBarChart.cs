using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
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

            int initialY = GetYAxis().GetMinimumAxisEntry().Position.Y;
            
            // Loop through the x-axis for each data point
            foreach (AxisEntry<TXAxis> x in GetX1Axis().AxisEntries)
            {
                foreach (DataSeries<TXAxis, TYAxis> ds in ChartData)
                {
                    ds.GetDataPointOnX(x.KeyValue);
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
