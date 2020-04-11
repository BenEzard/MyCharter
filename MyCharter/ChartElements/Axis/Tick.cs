using System;

namespace MyCharter
{
    public class Tick : AxisEntry
    {
        public bool IsMajorTick { get; set; } = false;

        /// <summary>
        /// Create a Tick without a label.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="content"></param>
        public Tick(object value, object content) : base(value, content) { }

        public Tick(object value, object content, string label) : base(value, content, label)
        {
        }

        public Tick(object value, object content, string label, bool isMajorTick) : base(value, content, label)
        {
            IsMajorTick = isMajorTick;
        }
    }
}
