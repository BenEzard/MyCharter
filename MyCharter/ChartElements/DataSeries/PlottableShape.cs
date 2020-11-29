using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MyCharter.ChartElements.DataSeries
{
    public class PlottableShape
    {
        public Object Shape { get; set; }
        public Color Color { get; set; }

        public PlottableShape(Color color)
        {
            Color = color;
        }
    }
}
