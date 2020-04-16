using System.Drawing;

namespace MyCharter
{
    public class AxisEntry
    {
        private ImageText _label;
        public ImageText Label 
        {
            get => _label;
            set
            {
                _label = value;
                _label.Dimensions = null;
            } 
        }

        public object EntryContent { get; set; }

        public object KeyValue { get; set; }

        public bool IsMajorTick { get; set; } = false;

        /// <summary>
        /// The position of the AxisEntry. It will be the x or y (depending on what the axis is).
        /// </summary>
        public Point Position;

        public AxisEntry()
        {
        }

        public AxisEntry(object key, object content)
        {
            KeyValue = key;
            EntryContent = content;
        }

        /// <summary>
        /// Create an AxisEntry with the specified key, content and label.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <param name="label"></param>
        public AxisEntry(object key, object content, string label)
        {
            KeyValue = key;
            EntryContent = content;
            Label = new ImageText(label);
        }

    }
}
