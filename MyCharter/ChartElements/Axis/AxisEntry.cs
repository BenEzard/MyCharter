namespace MyCharter
{
    public class AxisEntry
    {
        private ImageElement _label;
        public ImageElement Label 
        {
            get => _label;
            set
            {
                _label = value;
                _label.LabelDimensions = null;
            } 
        }

        public object EntryContent { get; set; }

        public object KeyValue { get; set; }

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
            Label = new ImageElement(label);
        }

    }
}
