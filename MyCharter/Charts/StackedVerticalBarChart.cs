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

        public override void PlotData(Graphics g)
        {
            Point initialPosition = GetYAxis().GetAxisPositionOfLabel("0").Value;
            foreach (DataSeries<TXAxis, TYAxis> ds in ChartData)
            {
                Rectangle dpRectangle;
                foreach (DataPoint<TXAxis, TYAxis> dp in ds.DataPoints)
                {
                    Point dataPointCoord = GetChartPosition(dp.AxisCoord1, dp.AxisCoord2);

                    // Because this chart is VERTICAL, need to get the pixels per increment from the x-axis.
                    int pixelsPerIncrement = GetX1Axis().PixelsPerIncrement;

                    // Offset the x position of the bar to be halfway along the x AxisEntry.
                    Point offsetDataPointCoord = new Point(dataPointCoord.X - (pixelsPerIncrement / 2), dataPointCoord.Y);

                    //dpRectangle = new Rectangle(offsetDataPointCoord, new Size(pixelsPerIncrement - 2, initialPosition.Y - dataPointCoord.Y));

                    dpRectangle = new Rectangle(
                        
                        offsetDataPointCoord, new Size(pixelsPerIncrement - 2, initialPosition.Y - dataPointCoord.Y));
                    g.FillRectangle(new SolidBrush(ds.Color), dpRectangle);
                }

            }
        }
    }
}
