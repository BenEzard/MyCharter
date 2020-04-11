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

        public AxisEntry(object key, object content, string label)
        {
            Label = new ImageElement(label);
            KeyValue = key;
            EntryContent = content;
        }

    }
}
