using System;
using System.Drawing;

namespace MyCharter.ChartElements.Axis
{
    public class NumberScaleAxis : AbstractScaleAxis<int>
    {
        public NumberScaleAxis(int minimumValue, int maximumValue, int majorIncrement, int minorIncrement, int pixelsPerIncrement) :
            base(AxisFormat.NUMBER_SCALE, minimumValue, maximumValue, majorIncrement, minorIncrement, pixelsPerIncrement)
        {

        }

        public NumberScaleAxis(int minimumValue, int maximumValue, int majorIncrement, int minorIncrement, int pixelsPerIncrement, AxisLabelFormat labelFormat) :
    base(AxisFormat.NUMBER_SCALE, minimumValue, maximumValue, majorIncrement, minorIncrement, pixelsPerIncrement)
        {
            LabelFormat = labelFormat;
        }

        public override string FormatLabelString(object label)
        {
            string rValue = null;
            int numValue = (int)label;
            double dValue;

            switch (LabelFormat)
            {
                case AxisLabelFormat.NUMBER_ALL:
                    rValue = label.ToString();
                    break;
                case AxisLabelFormat.NUMBER_THOU_SEP_COMMA:
                    rValue = String.Format("{0:n0}", numValue);
                    break;
                case AxisLabelFormat.NUMBER_THOU_SEP_SPACE:
                    rValue = String.Format("{0:n0}", numValue).Replace(',', ' ');
                    break;
                case AxisLabelFormat.NUMBER_THOU_ABBR:
                    if (numValue >= 1000)
                    {
                        dValue = (double)numValue / (double)1000;
                        rValue = String.Format("{0:n1}", dValue) + "k";
                    }
                    else
                        rValue = numValue.ToString();
                    break;
                case AxisLabelFormat.NUMBER_MIL_ABBR:
                    dValue = (double)numValue / (double)1000000;
                    rValue = String.Format("{0:n1}", dValue) + "m";
                    break;
                default:
                    rValue = label.ToString();
                    break;
            }
            Console.WriteLine($"In NumberScaleAxis.FormatLabelString for {label}, results in {rValue}");

            return rValue;
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
            AxisEntry<int> tick;
            int majorTickCounter = 0;

            while (tickValue >= minValue)
            {
                ++majorTickCounter;
                tick = new AxisEntry<int>(tickValue, null, FormatLabelString(tickValue));

                if (tickValue % MajorIncrement == 0)
                    tick.IsMajorTick = true;

                AddEntry(tick);
                tickValue -= MinorIncrement;
            }
        }

        /// <summary>
        /// Return the position along an Axis for a key value.
        /// For example:
        ///    if the value you're searching for is 12 and 12 is an AxisEntry then it will return that x/y value for that Point.
        ///    if the AxisEntries are 10 and 15, then the value will be 2/5ths between 10 and 15.
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public override int GetAxisPosition(int keyValue)
        {
            int rValue = -1;

            // First, get the AxisEntry for those above/below the specific value.
            AxisEntry<int> belowAxisEntry = null;
            AxisEntry<int> equalAxisEntry = null;
            AxisEntry<int> aboveAxisEntry = null;

            if (AxisXY == Axis.Y)
            {
                // This code will work for DESCENDING scale only
                foreach (AxisEntry<int> e in AxisEntries)
                {
                    if (e.KeyValue > keyValue)
                    {
                        aboveAxisEntry = e;
                    }

                    if ((e.KeyValue < keyValue) && (belowAxisEntry == null))
                    {
                        belowAxisEntry = e;
                    }

                    if (e.KeyValue == keyValue)
                    {
                        equalAxisEntry = e;
                    }
                }

                if (equalAxisEntry != null)
                    Console.WriteLine($"Looking for {keyValue}, below={belowAxisEntry.KeyValue}, equal={equalAxisEntry.KeyValue}, after={aboveAxisEntry.KeyValue}");
                else
                    Console.WriteLine($"Looking for {keyValue}, below={belowAxisEntry.KeyValue} {belowAxisEntry.Position}, after={aboveAxisEntry.KeyValue} {aboveAxisEntry.Position}");
                
                // Second, (if required) calculate how far along it is between ticks
                if (equalAxisEntry == null)
                {
                    double abovePos;
                    double belowPos;
                    double pixelGapBetween;
                    double PixelsPerValue;
                    int difference;

                    switch (AxisXY)
                    {
                        case Axis.X:
                            abovePos = aboveAxisEntry.Position.X;
                            belowPos = belowAxisEntry.Position.X;
                            pixelGapBetween = (abovePos > belowPos) ? abovePos - belowPos : belowPos - abovePos;
                            PixelsPerValue = pixelGapBetween / (double)((aboveAxisEntry.KeyValue > belowAxisEntry.KeyValue) ? aboveAxisEntry.KeyValue - belowAxisEntry.KeyValue : aboveAxisEntry.KeyValue - belowAxisEntry.KeyValue);
                            difference = keyValue - belowAxisEntry.KeyValue;
                            rValue = (int)(belowPos + (difference * PixelsPerValue));
                            break;
                        case Axis.Y:
                            abovePos = aboveAxisEntry.Position.Y;
                            belowPos = belowAxisEntry.Position.Y;
                            pixelGapBetween = (abovePos > belowPos) ? abovePos - belowPos : belowPos - abovePos;
                            PixelsPerValue = pixelGapBetween / (double)((aboveAxisEntry.KeyValue > belowAxisEntry.KeyValue) ? aboveAxisEntry.KeyValue - belowAxisEntry.KeyValue : aboveAxisEntry.KeyValue - belowAxisEntry.KeyValue);
                            difference = keyValue - belowAxisEntry.KeyValue;
                            rValue = (int)(belowPos - (difference * PixelsPerValue));
                            break;
                    }
                }
                else
                {
                    rValue = (AxisXY == Axis.X) ? equalAxisEntry.Position.X : equalAxisEntry.Position.Y;
                }
            }

    

            return rValue;
        }

        public override double GetAxisPixelsPerValue() => PixelsPerIncrement / MinorIncrement;

    }
}
