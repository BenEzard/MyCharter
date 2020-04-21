using System;
using System.Drawing;

namespace MyCharter
{
    public class TimeScaleAxis : AbstractScaleAxis
    {
        public TimeScaleAxis(object minimumValue, object maximumValue, int majorIncrement, int minorIncrement, int pixelsPerIncrement) :
            base(AxisFormat.TIME_SCALE, minimumValue, maximumValue, majorIncrement, minorIncrement, pixelsPerIncrement)
        {
            
        }

        /// <summary>
        /// Checks to see if the axis spans midnight.
        /// </summary>
        public bool DoesSpanMidnight
        {
            get
            {
                return ((TimeSpan)MaximumValue < (TimeSpan)MinimumValue) ? true : false;
            }
        }

        protected override bool AreAxisValuesValid(out string errorMessage)
        {
            bool rValue = true;
            errorMessage = null;

            if (MinimumValue is DateTime == false) errorMessage = "Minimum Value must be of type DateTime for TIME_SCALE.";
            if (MaximumValue is DateTime == false) errorMessage = "Maximum Value must be of type DateTime for TIME_SCALE.";
            if (MajorIncrement <= 0) errorMessage = $"Major Increment must be > 0 for TIME_SCALE. It is set to {MajorIncrement}";
            if (MinorIncrement <= 0) errorMessage = $"Minor Increment must be > 0 for TIME_SCALE. It is set to {MinorIncrement}";
            if (MajorIncrement <= MinorIncrement) errorMessage = $"Major Increment must be > Minor Increment for TIME_SCALE. Major increment is {MajorIncrement}, Minor increment is {MinorIncrement}";

            if (errorMessage != null)
                rValue = false;

            return rValue;            
        }

        /// <summary>
        /// Generate all of the timescale axis values, using TimeSpan as the key.
        /// </summary>
        internal override void GenerateAxisValues()
        {
            DateTime maxTime = (DateTime)MaximumValue;
            DateTime minTime = (DateTime)MinimumValue;

            // Because this is a TimeScale, check to see if the range spans midnight.
            bool spansMidnight = (maxTime.TimeOfDay < minTime.TimeOfDay) ? true : false;

            // Create an entry for each XItem, which will be the number of minorIncrementsMinutes between minTime and maxTime.
            DateTime current = minTime;
            AxisEntry tick;
            AxisEntry LastTick = null;
            if (spansMidnight)
            {
                while (current.Hour != 0 && current.Minute <= 59) // added =
                {

                    tick = new AxisEntry(key:current.TimeOfDay, content:null, label:current.ToShortTimeString());
                    if ((LastTick == null) || (current.Subtract(minTime).TotalMinutes % MajorIncrement == 0))
                    {
                        tick.IsMajorTick = true;
                    }

                    AddEntry(tick);
                    LastTick = tick;
                    current = current.AddMinutes(MinorIncrement);
                }
            } // end if spansMidnight

            while (current <= maxTime)
            {

                tick = new AxisEntry(current.TimeOfDay, null, current.ToShortTimeString());
                if ((LastTick != null) && (current.Subtract(minTime).TotalMinutes % MajorIncrement == 0))
                {
                    tick.IsMajorTick = true;
                }

                AddEntry(tick);
                current = current.AddMinutes(MinorIncrement);
            }
        }

        /// <summary>
        /// Return the AxisEntry co-ordinates.
        /// This is the top-left of the associated label.
        /// TODO This should be replaced by BinarySearch using IComparer interface
        /// </summary>
        /// <param name="soughtTimeSpan"></param>
        /// <returns></returns>
        public int GetAxisEntry(TimeSpan soughtTimeSpan)
        {
            int rValue = -1;
            Point point = new Point(-1,-1);
            int lastPassedIndex = -1;
            TimeSpan midnight = new TimeSpan(0, 0, 0);

            for (int i = 0; i < Entries.Count; i++)
            {
                AxisEntry ae = Entries[i];
                TimeSpan key = (TimeSpan)ae.KeyValue;

                if (key == soughtTimeSpan)
                {
                    point = ae.Label.Position;
                    Console.WriteLine($"Found {soughtTimeSpan} at {point.X}");
                    break;
                }
                else if ((key > soughtTimeSpan) && (lastPassedIndex == -1))
                {
                    if (i > 0)
                        lastPassedIndex = i - 1;
                    else
                        lastPassedIndex = i;
                    // We can't break it here - we need to keep searching through the entire array in case the timeset spans midnight.
                    // (which means you can't stop it when key > soughtTime)
                    Console.WriteLine($"While looking for {soughtTimeSpan}, went over the limit at index={lastPassedIndex} (which is {(TimeSpan)Entries[lastPassedIndex].KeyValue}, {Entries[lastPassedIndex].Position.X})");
                }
            }

            // If the exact value wasn't passed (and only for the first time), work out where to display the line.
            if ((point.X == -1) && (lastPassedIndex != -1))
            {
                int lowerIndex = lastPassedIndex;
                int upperIndex = -1;

                float val = (float)PixelsPerIncrement / (float)MinorIncrement;

                float lowerDiff = (float)((soughtTimeSpan - (TimeSpan)Entries[lowerIndex].KeyValue).TotalMinutes);
                float pixelMove = lowerDiff * val;
                point = new Point(Entries[lowerIndex].Position.X + (int)pixelMove, Entries[lowerIndex].Position.Y);
/*                TimeSpan upperDiff = new TimeSpan();
                if (Entries.Count > lastPassedIndex+1)
                {
                    upperIndex = lastPassedIndex + 1;
                    upperDiff = ((TimeSpan)Entries[lowerIndex + 1].KeyValue) - soughtTimeSpan;
                }
                Console.WriteLine($"Choosing between {Entries[lowerIndex].KeyValue} {lowerDiff.TotalMinutes} and {Entries[upperIndex].KeyValue} {upperDiff.TotalMinutes}");
                point = Entries[lowerIndex].Position;*/
            }
            
            // If the value can't be mapped to the axis, then we need to determine what is the closest value.
            if (point.X == -1)
            {
                Console.WriteLine($"Can't find the desired location on axis {soughtTimeSpan}");
            }
            else
            {
                switch (AxisXY)
                {
                    case Axis.X:
                        rValue = point.X;
                        break;
                    case Axis.Y:
                        rValue = point.Y;
                        break;
                }
            }
            Console.WriteLine($"Returning with value {rValue}");
            return rValue;
        }

    }
}
