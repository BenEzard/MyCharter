using System;
using System.Drawing;

namespace MyCharter.ChartElements.Axis
{
    public class NumberScaleAxis : AbstractScaleAxis
    {
        public NumberScaleAxis(object minimumValue, object maximumValue, int majorIncrement, int minorIncrement, int pixelsPerIncrement) :
            base(AxisFormat.NUMBER_SCALE, minimumValue, maximumValue, majorIncrement, minorIncrement, pixelsPerIncrement)
        {

        }

        public override string FormatLabelString(object label)
        {
            return label.ToString();
        }

        protected override bool AreAxisValuesValid(out string errorMessage)
        {
            //throw new System.NotImplementedException();
            bool rValue = true;
            errorMessage = null;
            return rValue;
        }

        internal override void GenerateAxisEntries()
        {
            int minValue = (int)MinimumValue;
            int maxValue = (int)MaximumValue;
            int tickValue = maxValue;
            AxisEntry tick;
            int majorTickCounter = 0;

            while (tickValue >= minValue)
            {
                ++majorTickCounter;
                tick = new AxisEntry(tickValue, null, tickValue.ToString());

                if (tickValue % MajorIncrement == 0)
                    tick.IsMajorTick = true;

                AddEntry(tick);
                tickValue -= MinorIncrement;
            }
        }


        /// <summary>
        /// Returns the minimum value from this Axis.
        /// </summary>
        /// <returns></returns>
        public override AxisEntry GetMinimum()
        {
            AxisEntry rValue = null;
            int minimumValue = int.MaxValue;
            foreach (AxisEntry e in Entries)
            {
                if ((int)e.KeyValue <= minimumValue)
                {
                    minimumValue = (int)e.KeyValue;
                    rValue = e;
                }
            }
            return rValue;
        }

        /// <summary>
        /// Returns the maximum value from this Axis.
        /// </summary>
        /// <returns></returns>
        public override AxisEntry GetMaximum()
        {
            AxisEntry rValue = null;
            int maximumValue = int.MinValue;
            foreach (AxisEntry e in Entries)
            {
                if ((int)e.KeyValue >= maximumValue)
                {
                    maximumValue = (int)e.KeyValue;
                    rValue = e;
                }
            }
            return rValue;
        }
    }
}
