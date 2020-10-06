using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCharter.ChartElements.Axis
{
    public class LabelAxis : AbstractScaleAxis<string>
    {
        public IEnumerable<string> DataSeriesNames = null;

        public LabelAxis(int pixelsPerIncrement, IEnumerable<string> dataSeriesNames) : base(AxisFormat.DATA_SERIES, pixelsPerIncrement)
        {
            DataSeriesNames = dataSeriesNames;
        }

        public override string FormatLabelString(object label, bool isSpecial = false)
        {
            return label.ToString();
        }

        public override double GetAxisPixelsPerValue()
        {
            throw new NotImplementedException();
        }

        public override int GetAxisPosition(string keyValue)
        {
            int rValue;

            AxisEntry<string> xValues = AxisEntries.Where(s => s.Label.Text == keyValue).Single();

            rValue = (AxisXY == Axis.X) ? xValues.Position.X : xValues.Position.Y;

            return rValue;
        }

        internal override void GenerateAxisEntries()
        {
            foreach (string name in DataSeriesNames)
            {
                AddEntry(new AxisEntry<string>(name, null, FormatLabelString(name, false), true));
            }
        }
    }
}
