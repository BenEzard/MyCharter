using MyCharter.ChartElements.Axis;
using System;
using System.Drawing;

namespace MyCharter
{
    public class TimeScaleAxis: AbstractScaleAxis<DateTime>
    {
        public TimeScaleAxis(DateTime minimumValue, DateTime maximumValue, int majorIncrement, int minorIncrement, int pixelsPerIncrement) :
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
                return (((DateTime)MaximumValue).TimeOfDay < ((DateTime)MinimumValue).TimeOfDay) ? true : false;
            }
        }

        protected override bool AreAxisValuesValid(out string errorMessage)
        {
            bool rValue = true;
            errorMessage = null;

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
        internal override void GenerateAxisEntries()
        {
            // TODO fix
         /*   DateTime maxTime = (DateTime)MaximumValue;
            DateTime minTime = (DateTime)MinimumValue;

            // Because this is a TimeScale, check to see if the range spans midnight.
            bool spansMidnight = (maxTime.TimeOfDay < minTime.TimeOfDay) ? true : false;

            // Create an entry for each XItem, which will be the number of minorIncrementsMinutes between minTime and maxTime.
            DateTime current = minTime;
            AxisEntry<TimeSpan> tick;
            AxisEntry<TimeSpan> LastTick = null;
            if (spansMidnight)
            {
                while (current.Hour != 0 && current.Minute <= 59) // added =
                {
                    // below used to be timespan
                    tick = new AxisEntry<TimeSpan>(key:current.TimeOfDay, content:null, label:current.ToShortTimeString());
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

                tick = new AxisEntry<TimeSpan>(current.TimeOfDay, null, current.ToShortTimeString());
                if ((LastTick != null) && (current.Subtract(minTime).TotalMinutes % MajorIncrement == 0))
                {
                    tick.IsMajorTick = true;
                }

                AddEntry(tick);
                current = current.AddMinutes(MinorIncrement);
            }*/
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
            DateTime minValue = (DateTime)MinimumValue;

            // Get where about the soughtTimeSpan would be on the axis.
            // To do this we need to convert soughTimeSpan back into a DateTime.
            DateTime soughtTimeAsDateTime;
            if ((DoesSpanMidnight) && (soughtTimeSpan < minValue.TimeOfDay))
            {
                // Add a day
                var modMinValue = minValue.AddDays(1);
                soughtTimeAsDateTime = new DateTime(modMinValue.Year, modMinValue.Month, modMinValue.Day, soughtTimeSpan.Hours, soughtTimeSpan.Minutes, soughtTimeSpan.Seconds);
            } 
            else
            {
                soughtTimeAsDateTime = new DateTime(minValue.Year, minValue.Month, minValue.Day, soughtTimeSpan.Hours, soughtTimeSpan.Minutes, soughtTimeSpan.Seconds);
            }
            
            var timeDifference = soughtTimeAsDateTime - minValue;
            int increments = (int)timeDifference.TotalMinutes / MinorIncrement;
            int remainder = (int)timeDifference.TotalMinutes % MinorIncrement;

            // Convert a negative number (if required)
            if (increments < 0)
                increments *= -1;
            if (remainder < 0)
                remainder *= -1;

            // Check to see if there is a remainder
            if (remainder > 0)
            {
                float val = (float)PixelsPerIncrement / (float)MinorIncrement;
                point = new Point(AxisEntries[increments].Position.X + (remainder * (int)val), AxisEntries[increments].Position.Y);
            }
            else 
                point = AxisEntries[increments].Position;

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
            return rValue;
        }

        public override string FormatLabelString(object label)
        {
            throw new NotImplementedException();
        }

        public override int GetAxisPosition(DateTime keyValue)
        {
            throw new NotImplementedException();
        }
    }
}
