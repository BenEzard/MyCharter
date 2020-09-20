using System;

namespace MyCharter.ChartElements.Axis
{
    public class DateScaleAxis : AbstractScaleAxis<DateTime>
    {
        public DateScaleAxis(DateTime minimumValue, DateTime maximumValue, int majorIncrementDaysBetween, int minorIncrementDaysBetween, int pixelsPerIncrement,
            AxisLabelFormat labelFormat) :
            base(AxisFormat.DATE_SCALE, minimumValue, maximumValue, majorIncrementDaysBetween, minorIncrementDaysBetween, pixelsPerIncrement)
        {
            LabelFormat = labelFormat;
        }

        /// <summary>
        /// Checks to see if the axis values are valid.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected override bool AreAxisValuesValid(out string errorMessage)
        {
            bool rValue = true;
            errorMessage = null;

            if (MajorIncrement <= 0) errorMessage = $"Major Increment must be > 0 for DATE_SCALE. It is set to {MajorIncrement}";
            if (MinorIncrement < 0) errorMessage = $"Minor Increment must be >= 0 for DATE_SCALE. It is set to {MinorIncrement}";
            if (MajorIncrement < MinorIncrement) errorMessage = $"Major Increment must be > Minor Increment for DATE_SCALE. Major increment is {MajorIncrement}, Minor increment is {MinorIncrement}";

            if (errorMessage != null)
                rValue = false;

            return rValue;
        }

        /// <summary>
        /// Generate the axis values, between the minimum and maximum values, designating which ticks will be major or minor.
        /// </summary>
        internal override void GenerateAxisEntries()
        {
            DateTime maxDate = ((DateTime)MaximumValue).Date;
            DateTime minDate = ((DateTime)MinimumValue).Date;
            DateTime tickDate = minDate;
            AxisEntry<DateTime> tick;
            int tickCounter = 0;

            while (tickDate <= maxDate)
            {
                ++tickCounter;
                tick = new AxisEntry<DateTime>(tickDate, null, FormatLabelString(tickDate, false));

                if (tickCounter % MajorIncrement == 0)
                    tick.IsMajorTick = true;

                AddEntry(tick);
                tickDate = tickDate.AddDays(1);
            }

        }

        /// <summary>
        /// Take the object of the label and format it based on the AxisLabelFormat value.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="isSpecial">Do something special (implemented in the sub-classes) with this label.</param>
        /// <returns></returns>
        public override string FormatLabelString(object label, bool isSpecial)
        {
            string rValue = "";

            if (label is DateTime)
            {
                DateTime date = (DateTime)label;
                string dateValue = "";
                switch (LabelFormat)
                {
                    case AxisLabelFormat.DATE_DDMMYYYY1:
                        rValue = date.Date.ToShortDateString();
                        if (rValue.IndexOf('/') == 1)
                            rValue = '0' + rValue;
                        break;
                    case AxisLabelFormat.DATE_DDMMYYYY2:
                        rValue = date.Date.ToShortDateString();
                        if (rValue.IndexOf('/') == 1)
                            rValue = '0' + rValue;
                        rValue = rValue.Replace('/', '-');
                        break;
                    case AxisLabelFormat.DATE_DDMM1:
                        rValue = date.Date.ToShortDateString();
                        if (rValue.IndexOf('/') == 1)
                            rValue = '0' + rValue;
                        rValue = rValue.Substring(0, 5);
                        break;
                    case AxisLabelFormat.DATE_DDMM2:
                        rValue = date.Date.ToShortDateString();
                        if (rValue.IndexOf('/') == 1)
                            rValue = '0' + rValue;
                        rValue = rValue.Substring(0, 5).Replace('/', '-');
                        break;
                    case AxisLabelFormat.DATE_Wwww:
                        rValue = date.DayOfWeek.ToString();
                        break;
                    case AxisLabelFormat.DATE_WWWWW:
                        rValue = date.DayOfWeek.ToString().ToUpper();
                        break;
                    case AxisLabelFormat.DATE_Www:
                        rValue = date.DayOfWeek.ToString().Substring(0, 3);
                        break;
                    case AxisLabelFormat.DATE_WWW:
                        rValue = date.DayOfWeek.ToString().Substring(0, 3).ToUpper();
                        break;
                    case AxisLabelFormat.DATE_WwwDD:
                        dateValue = date.Date.ToShortDateString();
                        if (dateValue.IndexOf('/') == 1)
                            dateValue = '0' + dateValue;
                        rValue = date.DayOfWeek.ToString().Substring(0, 3) + ' ' + dateValue.Substring(0, 2);
                        break;
                    case AxisLabelFormat.DATE_WWWDD:
                        dateValue = date.Date.ToShortDateString();
                        if (dateValue.IndexOf('/') == 1)
                            dateValue = '0' + dateValue;
                        rValue = date.DayOfWeek.ToString().Substring(0, 3).ToUpper() + ' ' + dateValue.Substring(0, 2);
                        break;
                }
            
            }
            return rValue;
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
                throw new NotImplementedException("This section not implemented.");
                /*double abovePos;
                double belowPos;
                double pixelGapBetween;
                double PixelsPerValue;
                int difference;

                switch (AxisXY)
                {
                    case Axis.X: // 12
                        abovePos = aboveAxisEntry.Position.X; // 15
                        belowPos = belowAxisEntry.Position.X; // 10
                        pixelGapBetween = abovePos - belowPos; // 20px
                        PixelsPerValue = pixelGapBetween / (double)(aboveAxisEntry.KeyValue - belowAxisEntry.KeyValue);
                        difference = keyValue - belowAxisEntry.KeyValue;
                        rValue = (int)(belowPos + (difference * PixelsPerValue));
                        break;
                    case Axis.Y:
                        abovePos = aboveAxisEntry.Position.Y; // 15
                        belowPos = belowAxisEntry.Position.Y; // 10
                        pixelGapBetween = abovePos - belowPos; // 20px
                        PixelsPerValue = pixelGapBetween / (double)(aboveAxisEntry.KeyValue - belowAxisEntry.KeyValue);
                        difference = keyValue - belowAxisEntry.KeyValue;
                        rValue = (int)(belowPos - (difference * PixelsPerValue));
                        break;
                }*/
            }
            else
            {
                rValue = (AxisXY == Axis.X) ? equalAxisEntry.Position.X : equalAxisEntry.Position.Y;
            }

            return rValue;
        }

        public override double GetAxisPixelsPerValue()
        {
            throw new NotImplementedException();
        }
    }
}
