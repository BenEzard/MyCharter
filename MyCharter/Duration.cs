using System;
using System.Drawing;

namespace MyCharter
{
    public class Duration
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Color Color { get; set; }


        public Duration(DateTime startTime, DateTime endTime, Color color)
        {
            StartTime = startTime;
            EndTime = endTime;
            Color = color;
        }

    }
}
