using System;

namespace MyCharter.ChartElements.Axis
{
    public class DateScaleAxis<TXAxis, TYAxis> : AbstractScaleAxis<TXAxis, TYAxis>
    {
        private AxisLabelFormat _labelFormat;

        public DateScaleAxis(object minimumValue, object maximumValue, int majorIncrementDaysBetween, int minorIncrementDaysBetween, int pixelsPerIncrement,
            AxisLabelFormat labelFormat) :
            base(AxisFormat.DATE_SCALE, minimumValue, maximumValue, majorIncrementDaysBetween, minorIncrementDaysBetween, pixelsPerIncrement)
        {
            _labelFormat = labelFormat;
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
                tick = new AxisEntry(tickDate, null, FormatLabelString(tickDate));

                if (tickCounter % MajorIncrement == 0)
                    tick.IsMajorTick = true;

                AddEntry(tick);
                tickDate = tickDate.AddDays(1);
            }

        }

        public override string FormatLabelString(object label)
        {
            string rValue = "";

            if (label is DateTime)
            {
                DateTime date = (DateTime)label;
                string dateValue = "";
                switch (_labelFormat)
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

    }
}
