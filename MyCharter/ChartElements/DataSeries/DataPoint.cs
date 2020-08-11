namespace MyCharter.ChartElements.DataSeries
{
    /// <summary>
    /// A DataPoint provides the x and y values of plottable information on a Chart.
    /// </summary>
    /// <typeparam name="TXAxis"></typeparam>
    /// <typeparam name="TYAxis"></typeparam>
    public class DataPoint<TXAxis, TYAxis>
    {
        public TXAxis AxisCoord1 { get; set; }
        public TYAxis AxisCoord2 { get; set; }

        public DataPoint(TXAxis axisCoord1, TYAxis axisCoord2)
        {
            AxisCoord1 = axisCoord1;
            AxisCoord2 = axisCoord2;
        }
    }
}
