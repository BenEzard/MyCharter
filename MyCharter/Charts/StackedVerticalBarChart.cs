using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using System;
using System.Drawing;

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
            var xAxis = GetAxis(Axis.X); // date scale axis
            foreach (DataSeries<TXAxis, TYAxis> ds in ChartData)
            {
                foreach (DataPoint<TXAxis, TYAxis> dp in ds.DataPoints)
                {
                    Point p = GetPosition(dp.AxisCoord1.ToString(), dp.AxisCoord2.ToString());
                    Console.WriteLine($"In StackedVerticalBarChart.PlotData; p is {p}");
                    g.DrawRectangle(new Pen(ds.Color), new Rectangle(p, new Size(p.X - 10, p.Y - 10)));
                }
            }
        }
    }
}
