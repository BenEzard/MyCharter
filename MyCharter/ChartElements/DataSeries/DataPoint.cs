namespace MyCharter.ChartElements.DataSeries
{
    /// <summary>
    /// A DataPoint provides the x and y values of plottable information on a Chart.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class DataPoint<T1, T2>
    {
        public T1 AxisCoord1 { get; set; }
        public T2 AxisCoord2 { get; set; }

        public DataPoint(T1 axisCoord1, T2 axisCoord2)
        {
            AxisCoord1 = axisCoord1;
            AxisCoord2 = axisCoord2;
        }
    }
}
