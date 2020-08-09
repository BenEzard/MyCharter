using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace MyCharter.Charts
{
    public class StackedVerticalBarChart<TXAxis, TYAxis> : AbstractChart<TXAxis, TYAxis>
    {
        public StackedVerticalBarChart() : base(ChartType.STACKED_VERTICAL_BAR_CHART)
        {
        }

        public override AxisEntry GetMaximumDataSeriesValue(Axis axis)
        {
            // For each value on an axis, we want to combine all of the data series values (and work out the maximum).
            throw new NotImplementedException();
        }

        public override AxisEntry GetMinimumDataSeriesValue(Axis axis)
        {
            throw new NotImplementedException();
        }

        public override void PlotData(Graphics g)
        {
            foreach (DataSeries<TXAxis, TYAxis> ds in ChartData)
            {
                foreach (DataPoint<TXAxis, TYAxis> dp in ds.DataPoints)
                {
                    Point p = GetChartPosition(dp.AxisCoord1, dp.AxisCoord2);
                    Console.WriteLine($"In StackedVerticalBarChart.PlotData; p is {p}");
                    int pixels = GetX1Axis().PixelsPerIncrement;
                    Point zeroPosition = GetYAxis().GetAxisPositionOfLabel("0").Value;
                    Point p2 = new Point(p.X - (pixels / 2), p.Y);
                    //g.DrawRectangle(new Pen(ds.Color), new Rectangle(p, new Size(p.X - 10, p.Y - 10)));
                    g.FillRectangle(Brushes.DarkGray, new Rectangle(p2, new Size(pixels-2, zeroPosition.Y - p.Y)));
                }
            }
        }
    }
}
