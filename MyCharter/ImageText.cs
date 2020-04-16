using System.Drawing;

namespace MyCharter
{
    public class ImageText
    {
        /// <summary>
        /// The label (if exists) of the element.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Dimensions of the label on the image.
        /// </summary>
        public SizeF? Dimensions { get; set; } = null;

        /// <summary>
        /// The position that this ImageElement has on the image.
        /// </summary>
        public Point Position { get; set; } = new Point();

        /// <summary>
        /// Create a new ImageElement.
        /// </summary>
        /// <param name="text"></param>
        public ImageText(string text)
        {
            Text = text;
        }
    }
}
