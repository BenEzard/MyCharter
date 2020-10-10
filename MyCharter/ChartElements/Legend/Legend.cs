using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MyCharter.ChartElements.Legend
{
    public class Legend
    {
        /// <summary>
        /// Font that is used for drawing the Legend Title.
        /// </summary>
        public Font LegendTitleFont { get; set; } = new Font("Arial", 6 * 1.33f, FontStyle.Regular, GraphicsUnit.Point);

        /// <summary>
        /// Font that is used for drawing Legend entry text.
        /// </summary>
        public Font LegendEntryFont { get; set; } = new Font("Arial", 6 * 1.33f, FontStyle.Regular, GraphicsUnit.Point);

        /// <summary>
        /// List of entries displayed in the Legend.
        /// </summary>
        public List<LegendEntry> LegendEntries { get; } = new List<LegendEntry>();

        /// <summary>
        /// The size of the Legend icon (in pixels)
        /// </summary>
        public int SizeOfLegendIcon { get; set; } = 10;

        /// <summary>
        /// The gap between the Legend icon and text (in pixels).
        /// </summary>
        public int GapBetweenIconAndText { get; set; } = 5;

        /// <summary>
        /// Contains the maximum WIDTH of a Legend entry and the total HEIGHT of Legend entries.
        /// </summary>
        public SizeF Dimensions = new SizeF();

        /// <summary>
        /// Should the Legend be drawn?
        /// </summary>
        public bool IsLegendVisible { get; set; } = true;

        public Legend() { }

        /// <summary>
        /// Add a new entry to the Legend.
        /// </summary>
        /// <param name="entry"></param>
        public void AddEntry(LegendEntry entry)
        {
            LegendEntries.Add(entry);
        }
    }
}
