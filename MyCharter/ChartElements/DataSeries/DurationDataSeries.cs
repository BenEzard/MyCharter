using System;
using System.Drawing;

namespace MyCharter.ChartElements.DataSeries
{
    public class DurationDataSeries : DataSeries<DateTime, string, Duration>
    {
        public DurationDataSeries(string name, Color color) : base(name, color) { }

        public void AddDataPoint(Duration duration)
        {
            DataPoints.Add(new DataPoint<DateTime, string, Duration>(duration.StartDateTime, Name, duration));
        }
    }
}
