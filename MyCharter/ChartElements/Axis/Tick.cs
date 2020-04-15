using System;

namespace MyCharter
{
    public class Tick : AxisEntry
    {
        public bool IsMajorTick { get; set; } = false;

        public Tick(object key, object content, string label) : base(key, content, label)
        {
        }
    }
}
