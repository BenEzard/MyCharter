using System.Drawing;

namespace MyCharter
{
    public class AxisEntry<TKeyDataType>
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

        public TKeyDataType KeyValue { get; set; }

        public bool IsMajorTick { get; set; } = false;

        /// <summary>
        /// The position of the AxisEntry. It will be the x or y (depending on what the axis is).
        /// </summary>
        public Point Position;

        /// <summary>
        /// Create an AxisEntry with the specified key, content and label.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <param name="label"></param>
        public AxisEntry(TKeyDataType key, object content, string label)
        {
            KeyValue = key;
            EntryContent = content;
            Label = new ImageText(label);
        }

        /// <summary>
        /// Create an AxisEntry with the specified key, content and label.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <param name="label"></param>
        /// <param name="isMajorTick"></param>
        public AxisEntry(TKeyDataType key, object content, string label, bool isMajorTick)
        {
            KeyValue = key;
            EntryContent = content;
            Label = new ImageText(label);
            IsMajorTick = isMajorTick;
        }
    }
}
