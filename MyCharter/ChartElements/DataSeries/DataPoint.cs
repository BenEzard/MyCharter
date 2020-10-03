using System.Drawing;

namespace MyCharter.ChartElements.DataSeries
{
    /// <summary>
    /// A DataPoint provides the x and y values of plottable information on a Chart.
    /// </summary>
    /// <typeparam name="TXAxisDataType"></typeparam>
    /// <typeparam name="TYAxisDataType"></typeparam>
    public class DataPoint<TXAxisDataType, TYAxisDataType, TDataPointData>
    {
        /// <summary>
        /// The x-axis value of this DataPoint.
        /// </summary>
        public TXAxisDataType xAxisCoord { get; set; }

        /// <summary>
        /// The y-axis value of this DataPoint.
        /// </summary>
        public TYAxisDataType yAxisCoord { get; set; }

        public TDataPointData DataPointData { get; set; }

        /// <summary>
        /// The graphical representation of the DataPoint.
        /// This could be represented as a Rectangle object.
        /// </summary>
        public object GraphicalRepresentation { get; set; }

        private ImageText _dataPointlabel;
        public ImageText DataPointLabel
        {
            get => _dataPointlabel;
            set
            {
                _dataPointlabel = value;
                _dataPointlabel.Dimensions = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDataPointLabelVisible { get; set; } = false;

        /// <summary>
        /// Create a new Data Point with the given x and y coordinate values.
        /// </summary>
        /// <param name="xCoord"></param>
        /// <param name="yCoord"></param>
        public DataPoint(TXAxisDataType xCoord, TYAxisDataType yCoord)
        {
            xAxisCoord = xCoord;
            yAxisCoord = yCoord;
        }

        /// <summary>
        /// Create a new Data Point with the given x and y coordinate values.
        /// </summary>
        /// <param name="xCoord"></param>
        /// <param name="yCoord"></param>
        /// <param name="data"></param>
        public DataPoint(TXAxisDataType xCoord, TYAxisDataType yCoord, TDataPointData data)
        {
            xAxisCoord = xCoord;
            yAxisCoord = yCoord;
            DataPointData = data;
        }
    }
}
