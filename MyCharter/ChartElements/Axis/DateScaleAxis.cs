using System;

namespace MyCharter.ChartElements.Axis
{
    public class DateScaleAxis : AbstractScaleAxis
    {
        private DateFormat _labelFormat;

        public DateScaleAxis(object minimumValue, object maximumValue, int majorIncrementDaysBetween, int minorIncrementDaysBetween, int pixelsPerIncrement,
            DateFormat labelFormat) :
            base(AxisFormat.DATE_SCALE, minimumValue, maximumValue, majorIncrementDaysBetween, minorIncrementDaysBetween, pixelsPerIncrement)
        {
            _labelFormat = labelFormat;
        }

        protected override bool AreAxisValuesValid(out string errorMessage)
        {
            bool rValue = true;
            errorMessage = null;

            if (MinimumValue is DateTime == false) errorMessage = "Minimum Value must be of type DateTime for DATE_SCALE.";
            if (MaximumValue is DateTime == false) errorMessage = "Maximum Value must be of type DateTime for DATE_SCALE.";
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
            AxisEntry tick;
            int tickCounter = 0;

            while (tickDate <= maxDate)
            {
                ++tickCounter;
                tick = new AxisEntry(tickDate, null, FormatLabel(tickDate));

                if (tickCounter % MajorIncrement == 0)
                    tick.IsMajorTick = true;

                AddEntry(tick);
                tickDate = tickDate.AddDays(1);
            }

        }

        private string FormatLabel(DateTime date)
        {
            string rValue = "";
            string dateValue = "";
            switch (_labelFormat)
            {
                case DateFormat.DDMMYYYY1:
                    rValue = date.Date.ToShortDateString();
                    if (rValue.IndexOf('/') == 1)
                        rValue = '0' + rValue;
                    break;
                case DateFormat.DDMMYYYY2:
                    rValue = date.Date.ToShortDateString();
                    if (rValue.IndexOf('/') == 1)
                        rValue = '0' + rValue;
                    rValue = rValue.Replace('/', '-');
                    break;
                case DateFormat.DDMM1:
                    rValue = date.Date.ToShortDateString();
                    if (rValue.IndexOf('/') == 1)
                        rValue = '0' + rValue;
                    rValue = rValue.Substring(0, 5);
                    break;
                case DateFormat.DDMM2:
                    rValue = date.Date.ToShortDateString();
                    if (rValue.IndexOf('/') == 1)
                        rValue = '0' + rValue;
                    rValue = rValue.Substring(0, 5).Replace('/', '-');
                    break;
                case DateFormat.Wwww:
                    rValue = date.DayOfWeek.ToString();
                    break;
                case DateFormat.WWWWW:
                    rValue = date.DayOfWeek.ToString().ToUpper();
                    break;
                case DateFormat.Www:
                    rValue = date.DayOfWeek.ToString().Substring(0, 3);
                    break;
                case DateFormat.WWW:
                    rValue = date.DayOfWeek.ToString().Substring(0, 3).ToUpper();
                    break;
                case DateFormat.WwwDD:
                    dateValue = date.Date.ToShortDateString();
                    if (dateValue.IndexOf('/') == 1)
                        dateValue = '0' + dateValue;
                    rValue = date.DayOfWeek.ToString().Substring(0, 3) + ' ' + dateValue.Substring(0, 2);
                    break;
                case DateFormat.WWWDD:
                    dateValue = date.Date.ToShortDateString();
                    if (dateValue.IndexOf('/') == 1)
                        dateValue = '0' + dateValue;
                    rValue = date.DayOfWeek.ToString().Substring(0, 3).ToUpper() + ' ' + dateValue.Substring(0, 2);
                    break;
            }
            return rValue;
        }
    }
}
