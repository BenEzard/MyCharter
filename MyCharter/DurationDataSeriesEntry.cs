using System;
using System.Drawing;

namespace MyCharter
{
    public class DurationDataSeriesEntry : AxisEntry
    {
        public DurationDataSeriesEntry(DateTime startTime, DateTime endTime, string label) : base ()
        {
            Label = new ImageText(label);
            EntryContent = new Duration(startTime, endTime, Color.Blue);
            KeyValue = label;
        }
    }
}
