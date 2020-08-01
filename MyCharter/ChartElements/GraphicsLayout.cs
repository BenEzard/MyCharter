using System.Drawing;

namespace MyCharter.ChartElements
{
    public struct GraphicsLayout
    {
        public Point Border { get; set; } 
        public Point Title { get; set; }
        public Point SubTitle { get; set; }
        public Point yAxisLabels { get; set; }
        public Point yAxisTicks { get; set; }
        public Point xAxisLabels { get; set; }
        public Point xAxisTicks { get; set; }
        public Point xAxis { get; set; }
        public Point yAxis { get; set; }

    }
}
