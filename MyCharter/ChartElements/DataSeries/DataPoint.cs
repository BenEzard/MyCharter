using System.Drawing;

namespace MyCharter.ChartElements.DataSeries
{
    /// <summary>
    /// A DataPoint provides the x and y values of plottable information on a Chart.
    /// </summary>
    /// <typeparam name="TXAxis"></typeparam>
    /// <typeparam name="TYAxis"></typeparam>
    public class DataPoint<TXAxis, TYAxis>
    {
        /// <summary>
        /// The x-axis value of this DataPoint.
        /// </summary>
        public TXAxis xAxisCoord { get; set; }

        /// <summary>
        /// The y-axis value of this DataPoint.
        /// </summary>
        public TYAxis yAxisCoord { get; set; }

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
        public DataPoint(TXAxis xCoord, TYAxis yCoord)
        {
            xAxisCoord = xCoord;
            yAxisCoord = yCoord;
        }
    }
}
