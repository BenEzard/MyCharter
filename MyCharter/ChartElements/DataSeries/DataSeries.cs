using System.Collections.Generic;
using System.Drawing;

namespace MyCharter.ChartElements.DataSeries
{
    /// <summary>
    /// Define a Data Series.
    /// </summary>
    /// <typeparam name="TXAxis">The data type of the x axis.</typeparam>
    /// <typeparam name="TYAxis">The data type of the y axis.</typeparam>
    public class DataSeries<TXAxis, TYAxis>
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public List<DataPoint<TXAxis,TYAxis>> DataPoints { get; set; } = new List<DataPoint<TXAxis, TYAxis>>();

        /// <summary>
        /// Define a new Data Series with the given name and color.
        /// </summary>
        /// <param name="name">Name of the Data Series.</param>
        /// <param name="color">Color of the Data Series</param>
        public DataSeries(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        /// <summary>
        /// Add a Data Point to the DataSeries.
        /// </summary>
        /// <param name="axisEntry1"></param>
        /// <param name="axisEntry2"></param>
        public void AddDataPoint(TXAxis axisEntry1, TYAxis axisEntry2)
        {
            DataPoints.Add(new DataPoint<TXAxis, TYAxis>(axisEntry1, axisEntry2));
        }

        public DataPoint<TXAxis, TYAxis> GetDataPointOnX(TXAxis x)
        {
            DataPoint<TXAxis, TYAxis> rValue = null;
            foreach (DataPoint<TXAxis, TYAxis> dp in DataPoints)
            {
                if (object.Equals(dp.AxisCoord1, x))
                {
                    rValue = dp;
                }
            }
            return rValue;
        }
    }
}
