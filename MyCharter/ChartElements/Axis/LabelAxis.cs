using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCharter.ChartElements.Axis
{
    public class LabelAxis : AbstractScaleAxis<string>
    {
        public List<string> DataSeriesNames = null;

        public LabelAxis(int pixelsPerIncrement, List<string> dataSeriesNames) : base(AxisFormat.DATA_SERIES, pixelsPerIncrement)
        {
            DataSeriesNames = dataSeriesNames;
        }

        public override string FormatLabelString(object label, bool isSpecial = false)
        {
            return label.ToString();
        }

        public override double GetAxisPixelsPerValue()
        {
            throw new NotImplementedException();
        }

        public override int GetAxisPosition(string keyValue)
        {
            int rValue;

            AxisEntry<string> xValues = AxisEntries.Where(s => s.Label.Text == keyValue).Single();

            rValue = (AxisXY == Axis.X) ? xValues.Position.X : xValues.Position.Y;

            return rValue;
        }

        internal override void GenerateAxisEntries()
        {
            foreach (string name in DataSeriesNames)
            {
                AddEntry(new AxisEntry<string>(name, null, FormatLabelString(name, false), true));
            }
        }

        public void RemoveSeries(List<string> seriesList, bool removeThoseInList)
        {
            var index = 0;
            while (index < DataSeriesNames.Count())
            {
                if (((removeThoseInList == false) && (seriesList.Contains(DataSeriesNames[index]) == false)) ||
                    ((removeThoseInList) && (seriesList.Contains(DataSeriesNames[index]))))
                {
                    DataSeriesNames.Remove(DataSeriesNames[index]);
                }
                else
                {
                    index++;
                }
            }
        }

        public void RemoveSeries(string seriesNameStartsWith, bool removeMatching)
        {
            var index = 0;
            while (index < DataSeriesNames.Count())
            {
                if (((removeMatching) && (DataSeriesNames[index].StartsWith(seriesNameStartsWith)))
                    || ((removeMatching == false) && (DataSeriesNames[index].StartsWith(seriesNameStartsWith) == false)))
                {
                    DataSeriesNames.Remove(DataSeriesNames[index]);
                }
                else
                {
                    index++;
                }
            }
            
            // Call this method again to get it to recalculate based on new values.
            //CalculateInitialAxisValuePositions();
        }
    }
}
