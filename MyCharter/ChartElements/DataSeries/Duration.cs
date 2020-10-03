using System;
using System.Drawing;

namespace MyCharter
{
    public class Duration
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public Color Color { get; set; }

        public Duration(DateTime startDateTime, DateTime endDateTime)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            Color = Color.Blue;
        }

        public Duration(DateTime startDateTime, DateTime endDateTime, Color color)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            Color = color;
        }

    }
}
