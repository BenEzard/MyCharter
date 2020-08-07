using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using System;
using System.Drawing;

namespace MyCharter.Charts
{
    public class StackedVerticalBarChart : AbstractChart
    {
        public StackedVerticalBarChart() : base(ChartType.STACKED_VERTICAL_BAR_CHART)
        {
        }

        public override void PlotData(Graphics g)
        {
            var xAxis = GetAxis(Axis.X);
            foreach (DataSeries<DateTime, int> ds in ChartData)
            {
                foreach (DataPoint<DateTime, int> dp in ds.DataPoints)
                {
                    Point p = GetPosition(dp.AxisCoord1.ToString(), dp.AxisCoord2.ToString());
                    g.DrawRectangle(new Pen(ds.Color), new Rectangle(p, new Size(p.X - 10, p.Y - 10)));
                }
            }
        }
    }
}
