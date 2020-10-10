using MyCharter.ChartElements.DataSeries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MyCharter.ChartElements.Legend
{
    public class LegendEntry
    {
        /// <summary>
        /// The name of the DataSeries which this legend entry relates to.
        /// (This is supplied so that if a DataSeries is deleted from the chart, the associated LegendEntry can be removed to.
        /// </summary>
        public string DataSeriesName { get; set; }

        /// <summary>
        /// The type of display which this DataSeries should have in the legend.
        /// </summary>
        public LegendDisplayType LegendDisplayShape { get; set; } = LegendDisplayType.SQUARE;

        /// <summary>
        /// The Color of the Legend Icon
        /// </summary>
        public Color IconColor { get; set; }

        private ImageText _entryLabel;
        public ImageText EntryLabel
        {
            get => _entryLabel;
            set
            {
                _entryLabel = value;
                _entryLabel.Dimensions = null;
            }
        }

        /// <summary>
        /// Create a new LegendEntry that is not connected to a DataSeries.
        /// </summary>
        /// <param name="displayType"></param>
        /// <param name="iconColor"></param>
        /// <param name="text"></param>
        public LegendEntry(LegendDisplayType displayType, Color iconColor, string text)
        {
            LegendDisplayShape = displayType;
            IconColor = iconColor;
            EntryLabel = new ImageText(text);
        }

        /// <summary>
        /// Create a new LegendEntry that is connected to a DataSeries.
        /// </summary>
        /// <param name="dataSeriesName"></param>
        /// <param name="displayType"></param>
        /// <param name="iconColor"></param>
        /// <param name="text"></param>
        public LegendEntry(string dataSeriesName, LegendDisplayType displayType, Color iconColor, string text)
        {
            DataSeriesName = dataSeriesName;
            LegendDisplayShape = displayType;
            IconColor = iconColor;
            EntryLabel = new ImageText(text); 
        }

    }
}
