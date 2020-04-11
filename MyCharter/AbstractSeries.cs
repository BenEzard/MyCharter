using System.Drawing;

namespace MyCharter
{
    public abstract class AbstractSeries
    {
        /// <summary>
        /// Label of the Series
        /// </summary>
        public string SeriesLabel { get; set; }
        
        /// <summary>
        /// The dimensions of the label, considering the appropriate font.
        /// </summary>
        public SizeF LabelSize { get; set; }
        
    }
}
