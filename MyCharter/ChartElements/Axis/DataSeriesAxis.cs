using MyCharter.ChartElements.DataSeries;
using System;
using System.Collections.Generic;

namespace MyCharter.ChartElements.Axis
{
    public class DataSeriesAxis<TAxisDataType> : AbstractChartAxis<string>
    {
        public DataSeriesAxis() : base(AxisFormat.DATA_SERIES)
        {
        }

        public DataSeriesAxis(List<DataSeries<DateTime, DateTime>> chartData) : base(AxisFormat.DATA_SERIES)
        {
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
            throw new NotImplementedException();
        }

        internal override void GenerateAxisEntries()
        {
            
            throw new NotImplementedException();
        }
    }
}
