using MyCharter.ChartElements.Axis;
using System.Collections.Generic;
using System.Drawing;

namespace MyCharter.ChartElements.DataSeries
{
    /// <summary>
    /// Define a Data Series.
    /// </summary>
    /// <typeparam name="TXAxisDataType">The data type of the x axis.</typeparam>
    /// <typeparam name="TYAxisDataType">The data type of the y axis.</typeparam>
    /// <typeparam name="TDataPointData"></typeparam>
    public class DataSeries<TXAxisDataType, TYAxisDataType, TDataPointData>
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public List<DataPoint<TXAxisDataType, TYAxisDataType, TDataPointData>> DataPoints { get; set; } = new List<DataPoint<TXAxisDataType, TYAxisDataType, TDataPointData>>();

        /// <summary>
        /// Should the label for this DataSeries be visible?
        /// Note that individual DataPoints may be set to visible/invisible and disregard the DataSeries value.
        /// </summary>
        public AxisLabelFormat DataSeriesLabelFormat { get; set; } = AxisLabelFormat.NONE;

        /// <summary>
        /// The type of display which this DataSeries should have in the legend.
        /// </summary>
        public LegendDisplayType LegendDisplay { get; set; } = LegendDisplayType.SQUARE;
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
        /// <param name="legendDisplay"></param>
        public DataSeries(string name, Color color, AxisLabelFormat labelFormat, LegendDisplayType legendDisplay)
        {
            Name = name;
            Color = color;
            DataSeriesLabelFormat = labelFormat;
            LegendDisplay = legendDisplay;
        }

        /// <summary>
        /// Add a Data Point to the DataSeries.
        /// </summary>
        /// <param name="axisEntry1"></param>
        /// <param name="axisEntry2"></param>
        public void AddDataPoint(TXAxisDataType axisEntry1, TYAxisDataType axisEntry2)
        {
            DataPoints.Add(new DataPoint<TXAxisDataType, TYAxisDataType, TDataPointData>(axisEntry1, axisEntry2));
        }

        public DataPoint<TXAxisDataType, TYAxisDataType, TDataPointData> GetDataPointOnX(TXAxisDataType x)
        {
            DataPoint<TXAxisDataType, TYAxisDataType, TDataPointData> rValue = null;
            foreach (DataPoint<TXAxisDataType, TYAxisDataType, TDataPointData> dp in DataPoints)
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
