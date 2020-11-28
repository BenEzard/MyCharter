namespace MyCharter.ChartElements.Axis
{
    public class AxisRegion<TDataType>
    {
        public AxisEntry<TDataType> Start { get; set; } = null;
        public AxisEntry<TDataType> End { get; set; } = null;

        public AxisRegion(AxisEntry<TDataType> start, AxisEntry<TDataType> end)
        {
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return $"{Start.Label.Text} --> {End.Label.Text}";
        }
    }
}
