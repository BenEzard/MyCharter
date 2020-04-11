using System;

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

        internal override void GenerateAxisValues()
        {
            DateTime maxTime = (DateTime)MaximumValue;
            DateTime minTime = (DateTime)MinimumValue;

            // Because this is a TimeScale, check to see if the range spans midnight.
            bool spansMidnight = (maxTime.TimeOfDay < minTime.TimeOfDay) ? true : false;

            // Create an entry for each XItem, which will be the number of minorIncrementsMinutes between minTime and maxTime.
            DateTime current = minTime;
            Tick tick;
            Tick LastTick = null;
            if (spansMidnight)
            {
                while (current.Hour != 0 && current.Minute < 59)
                {

                    if ((LastTick == null) || (current.Subtract(minTime).TotalMinutes % MajorIncrement == 0))
                    {
                        tick = new Tick(current, null, current.ToShortTimeString());
                        tick.IsMajorTick = true;
                    }
                    else
                        tick = new Tick(current, null);

                    AddTick(tick);
                    LastTick = tick;
                    current = current.AddMinutes(MinorIncrement);
                }
            } // end if spansMidnight

            while (current <= maxTime)
            {
                
                if ((LastTick != null) && (current.Subtract(minTime).TotalMinutes % MajorIncrement == 0))
                {
                    tick = new Tick(current, null, current.ToShortTimeString());
                    tick.IsMajorTick = true;
                }
                else
                    tick = new Tick(current, null);

                AddTick(tick);
                current = current.AddMinutes(MinorIncrement);
            }
        }
    }
}
