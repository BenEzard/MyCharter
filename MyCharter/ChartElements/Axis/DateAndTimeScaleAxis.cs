using System;

namespace MyCharter.ChartElements.Axis
{
    public class DateAndTimeScaleAxis : AbstractScaleAxis<DateTime>
    {
        public DateAndTimeScaleAxis(int majorIncrementMinutes, int minorIncrementMinutes, int pixelsPerIncrement, AxisLabelFormat labelFormat) :
    base(AxisFormat.DATE_AND_TIME_SCALE, majorIncrementMinutes, minorIncrementMinutes, pixelsPerIncrement)
        {
            LabelFormat = labelFormat;
        }

        public DateAndTimeScaleAxis(DateTime minimumValue, DateTime maximumValue, int majorIncrementMinutes, int minorIncrementMinutes, int pixelsPerIncrement,
    AxisLabelFormat labelFormat) :
    base(AxisFormat.DATE_AND_TIME_SCALE, minimumValue, maximumValue, majorIncrementMinutes, minorIncrementMinutes, pixelsPerIncrement)
        {
            LabelFormat = labelFormat;
        }

        /// <summary>
        /// Format the label for display. 
        /// For example, the DateTime might be '6/09/2020 11:37:00' and it will be displayed as "6/09 11:37"
        /// If IsSpecial=true the date will be displayed along with the time, if IsSpecial=false only the time will be displayed.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="isSpecial">Do something special (implemented in the sub-classes) with this label.</param>
        /// <returns></returns>
        public override string FormatLabelString(object label, bool isSpecial=false)
        {
            string rValue = "";

            if (label is DateTime)
            {
                DateTime date = (DateTime)label;
                string dateValue = "";

                if (isSpecial) // Display the date and time
                {
                    switch (LabelFormat)
                    {
                        case AxisLabelFormat.DATETIME_DDMMYYYY1_HHMM24:
                            rValue = date.Date.ToShortDateString();
                            if (rValue.IndexOf('/') == 1)
                                rValue = '0' + $"{rValue} {date.Hour:00}:{date.Minute:00}";
                            break;
                        case AxisLabelFormat.DATETIME_DDMMYYYY2_HHMM24:
                            rValue = date.Date.ToShortDateString();
                            if (rValue.IndexOf('/') == 1)
                                rValue = '0' + rValue;
                            rValue = $"{rValue.Replace('/', '-')} {date.Hour:00}:{date.Minute:00}";
                            break;
                        case AxisLabelFormat.DATETIME_DDMM1_HHMM24:
                            rValue = date.Date.ToShortDateString();
                            if (rValue.IndexOf('/') == 1)
                                rValue = '0' + rValue;
                            rValue = $"{rValue.Substring(0, 5)} {date.Hour:00}:{date.Minute:00}";
                            break;
                        case AxisLabelFormat.DATETIME_DDMM2_HHMM24:
                            rValue = date.Date.ToShortDateString();
                            if (rValue.IndexOf('/') == 1)
                                rValue = '0' + rValue;
                            rValue = $"{rValue.Substring(0, 5).Replace('/', '-')} {date.Hour:00}:{date.Minute:00}";
                            break;
                        case AxisLabelFormat.DATETIME_Wwww_HHMM24:
                            rValue = $"{date.DayOfWeek} {date.Hour:00}:{date.Minute:00}";
                            break;
                        case AxisLabelFormat.DATETIME_WWWWW_HHMM24:
                            rValue = $"{date.DayOfWeek.ToString().ToUpper()} {date.Hour:00}:{date.Minute:00}";
                            break;
                        case AxisLabelFormat.DATETIME_Www_HHMM24:
                            rValue = $"{date.DayOfWeek.ToString().Substring(0, 3)} {date.Hour:00}:{date.Minute:00}";
                            break;
                        case AxisLabelFormat.DATETIME_WWW_HHMM24:
                            rValue = $"{date.DayOfWeek.ToString().Substring(0, 3).ToUpper()} {date.Hour:00}:{date.Minute:00}";
                            break;
                        case AxisLabelFormat.DATETIME_WwwDD_HHMM24:
                            dateValue = date.Date.ToShortDateString();
                            if (dateValue.IndexOf('/') == 1)
                                dateValue = '0' + dateValue;
                            rValue = $"{date.DayOfWeek.ToString().Substring(0, 3)} {dateValue.Substring(0, 2)} {date.Hour:00}:{date.Minute:00}";
                            break;
                        case AxisLabelFormat.DATETIME_WWWDD_HHMM24:
                            dateValue = date.Date.ToShortDateString();
                            if (dateValue.IndexOf('/') == 1)
                                dateValue = '0' + dateValue;
                            rValue = $"{date.DayOfWeek.ToString().Substring(0, 3).ToUpper()} {dateValue.Substring(0, 2)} {date.Hour:00}:{date.Minute:00}";
                            break;
                        }
                    }
                    else // IsSpecial == false; display the time only
                    {
                        switch (LabelFormat)
                        {
                            default:
                                rValue = $"{date.Hour:00}:{date.Minute:00}";
                                break;
                        }
                    }

            }

            return rValue;
        }

        public override double GetAxisPixelsPerValue()
        {
            throw new NotImplementedException();
        }

        public override int GetAxisPosition(DateTime keyValue)
        {
            int rValue = -1;

            // First, get the AxisEntry for those above/below the specific value.
            AxisEntry<DateTime> belowAxisEntry = null;
            AxisEntry<DateTime> equalAxisEntry = null;
            AxisEntry<DateTime> aboveAxisEntry = null;

            foreach (AxisEntry<DateTime> e in AxisEntries)
            {
                if (e.KeyValue < keyValue)
                {
                    belowAxisEntry = e;
                }

                if (e.KeyValue == keyValue)
                {
                    equalAxisEntry = e;
                }

                if ((e.KeyValue > keyValue) && (aboveAxisEntry == null))
                {
                    aboveAxisEntry = e;
                }
            }

            // Second, (if required) calculate how far along it is between ticks
            if (equalAxisEntry == null)
            {
              //  throw new NotImplementedException("This section not implemented.");

                double abovePos;
                double belowPos;
                double pixelGapBetween;
                double PixelsPerValue;
                int difference=0;

                switch (AxisXY)
                {
                    case Axis.X: // 12
                        abovePos = aboveAxisEntry.Position.X; // 15
                        belowPos = belowAxisEntry.Position.X; // 10
                        pixelGapBetween = abovePos - belowPos; // 20px
                        PixelsPerValue = pixelGapBetween / (double)(aboveAxisEntry.KeyValue - belowAxisEntry.KeyValue).TotalMinutes;
                        difference = (int)(keyValue - belowAxisEntry.KeyValue).TotalMinutes;
                        rValue = (int)(belowPos + (difference * PixelsPerValue));
                        break;
                    case Axis.Y:
                        abovePos = aboveAxisEntry.Position.Y; // 15
                        belowPos = belowAxisEntry.Position.Y; // 10
                        pixelGapBetween = abovePos - belowPos; // 20px
                        PixelsPerValue = pixelGapBetween / (double)(aboveAxisEntry.KeyValue - belowAxisEntry.KeyValue).TotalMinutes;
                        difference = (int)(keyValue - belowAxisEntry.KeyValue).TotalMinutes;
                        rValue = (int)(belowPos - (difference * PixelsPerValue));
                        break;
                }
            }
            else
            {
                rValue = (AxisXY == Axis.X) ? equalAxisEntry.Position.X : equalAxisEntry.Position.Y;
            }

            return rValue;
        }

        //protected override bool AreAxisValuesValid(out string errorMessage)
        //{
        //    //throw new NotImplementedException();
        //    // TODO
        //    errorMessage = "";
        //    return true;
        //}

        internal override void GenerateAxisEntries()
        {
            DateTime tickDateAndTime = MinimumValue;
            DateTime previousDate = MinimumValue.Date.AddDays(-1);
            
            string label;
            AxisEntry<DateTime> tick;
            int tickCounter = 0;

            while (tickDateAndTime <= MaximumValue)
            {
                ++tickCounter;

                // If the date has changed, then display the date; otherwise just the time.
                if (previousDate != tickDateAndTime.Date)
                {
                    label = FormatLabelString(tickDateAndTime, true);
                    previousDate = tickDateAndTime.Date;
                }
                else
                {
                    label = FormatLabelString(tickDateAndTime, false);
                }

                tick = new AxisEntry<DateTime>(tickDateAndTime, null, label);

                if ((tickDateAndTime - MinimumValue).TotalMinutes % MajorIncrement == 0)
                    tick.IsMajorTick = true;
                
                AddEntry(tick);
                tickDateAndTime = tickDateAndTime.AddMinutes(MinorIncrement);
            }

        }
    }
}
