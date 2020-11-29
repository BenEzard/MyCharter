using System;
using System.Drawing;

namespace MyCharter.ChartElements.DataSeries
{
    public class PlottableShapeDataSeries : DataSeries<DateTime, string, PlottableShape>
    {
        public PlottableShapeDataSeries(string name, Color color) : base(name, color) { }

        public void AddDataPoint(DateTime dateTime, PlottableShape shape)
        {
            DataPoints.Add(new DataPoint<DateTime, string, PlottableShape>(dateTime, Name, shape));
        }
    }
}
