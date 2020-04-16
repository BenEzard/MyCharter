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
                while (current.Hour != 0 && current.Minute < 59)
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
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public int GetAxisEntry(TimeSpan timeSpan)
        {
            int rValue = -1;
            Point point = new Point(-1, -1);
            foreach (AxisEntry e in Entries)
            {
                if (e.KeyValue.Equals(timeSpan))
                {
                    point = e.Label.Position;
                }
            }

            switch (AxisXY)
            {
                case Axis.X:
                    rValue = point.X;
                    break;
                case Axis.Y:
                    rValue = point.Y;
                    break;
            }
            return rValue;
        }

    }
}
