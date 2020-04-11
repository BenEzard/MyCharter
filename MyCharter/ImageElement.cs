using System.Drawing;

namespace MyCharter
{
    public class ImageElement
    {
        /// <summary>
        /// The label (if exists) of the element.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Dimensions of the label on the image.
        /// </summary>
        public SizeF? LabelDimensions { get; set; } = null;

        /// <summary>
        /// The position that this ImageElement has on the image.
        /// </summary>
        public Point ChartPosition { get; set; }

        /// <summary>
        /// Create a new ImageElement.
        /// </summary>
        /// <param name="label"></param>
        public ImageElement(string label)
        {
            Label = label;
        }
    }
}
