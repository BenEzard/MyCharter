using MyCharter.ChartElements.Axis;
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
        /// Should the label for this DataSeries be visible?
        /// Note that individual DataPoints may be set to visible/invisible and disregard the DataSeries value.
        /// </summary>
        public AxisLabelFormat DataSeriesLabelFormat { get; set; } = AxisLabelFormat.NONE;

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
        /// Define a new Data Series with the given name and color, setting the specified label format.
        /// </summary>
        /// <param name="name">Name of the Data Series.</param>
        /// <param name="color">Color of the Data Series</param>
        /// <param name="labelFormat">Format to apply the label too</param>
        public DataSeries(string name, Color color, AxisLabelFormat labelFormat)
        {
            Name = name;
            Color = color;
            DataSeriesLabelFormat = labelFormat;
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
                if (object.Equals(dp.xAxisCoord, x))
                {
                    rValue = dp;
                }
            }
            return rValue;
        }
    }
}
