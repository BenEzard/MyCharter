using System;
using System.Drawing;

namespace MyCharter
{
    public class DurationDataSeries : AxisEntry
    {
        public DurationDataSeries(DateTime startTime, DateTime endTime, string label) : base ()
        {
            Label = new ImageElement(label);
            EntryContent = new Duration(startTime, endTime, Color.Blue);
            KeyValue = label;
        }
    }
}
