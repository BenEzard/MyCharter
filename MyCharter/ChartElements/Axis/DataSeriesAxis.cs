using MyCharter.ChartElements.Axis;
using System;
using System.Drawing;

namespace MyCharter
{
    public class DataSeriesAxis : AbstractChartAxis
    {
        public DataSeriesAxis() : base(AxisFormat.DATA_SERIES)
        {
        }

        internal override void GenerateAxisEntries()
        {
            if (Entries.Count == 0) throw new ArgumentException("There are no values on the Data Series Axis.");
        }

        public void AddDataSeries(DurationDataSeriesEntry dataSeries)
        {
            AddEntry(dataSeries);
        }

        public override void DrawAxis(Graphics g, Point offset)
        {
            switch (AxisXY)
            {
                case Axis.X:
                    throw new ArgumentException("Not implemented for DataSeriesAxis.DrawAxis x-axis.");
                case Axis.Y:
                    foreach (var e in Entries)
                    {
                        if (e is AxisEntry entry)
                        {
                            offset.Y += LabelPadding;
                            g.DrawString(entry.Label.Text, AxisFont, Brushes.Black, offset);
                            offset.Y += (int)entry.Label.Dimensions.Value.Height + LabelPadding;
                        }
                    }
                    break;
            }
        }

        public override void DrawTicks(Graphics g, Point offset)
        {
            switch (AxisXY)
            {
                case Axis.X:
                    throw new NotImplementedException("DataSeriesAxis.DrawTicks not implemented for X axis.");
                case Axis.Y:
                    int halfOfLabelHeight = (int)_maxLabelDimensions.Height % 2 == 0 ? (int)_maxLabelDimensions.Height / 2 : ((int)_maxLabelDimensions.Height - 1) / 2;
                    int x = offset.X - (int)_maxLabelDimensions.Width - AxisPadding;
                    for (int index = 0; index < Entries.Count; index++)
                    {
                        var e = Entries[index];
                        if (e is AxisEntry entry)
                        {
                            offset.Y += LabelPadding + halfOfLabelHeight;
                            e.Label.Position = new Point(x, offset.Y - halfOfLabelHeight);
                            e.Position = new Point(x, offset.Y - halfOfLabelHeight);
                            g.DrawLine(MajorTickPen, new Point(offset.X - 3, offset.Y), new Point(offset.X, offset.Y));
                            offset.Y += LabelPadding + halfOfLabelHeight;
                        }
                    }
                    break;
            }
        }

    /*    public override void CalculateLabelPositions(Graphics g, Point offset)
        {
            switch (AxisXY)
            {
                case Axis.X:
                    throw new NotImplementedException("DataSeriesAxis.DrawTicks not implemented for X axis.");
                case Axis.Y:
                    int halfOfLabelHeight = (int)_maxLabelDimensions.Height % 2 == 0 ? (int)_maxLabelDimensions.Height / 2 : ((int)_maxLabelDimensions.Height - 1) / 2;
                    int x = offset.X - (int)_maxLabelDimensions.Width - AxisPadding;
                    for (int index = 0; index < Entries.Count; index++)
                    {
                        var e = Entries[index];
                        offset.Y += LabelPadding + halfOfLabelHeight;
                        e.Label.Position = new Point(x, offset.Y - halfOfLabelHeight);
                        e.Position = new Point(x, offset.Y - halfOfLabelHeight);
                        //g.DrawLine(MajorTickPen, new Point(offset.X - 3, offset.Y), new Point(offset.X, offset.Y));
                        offset.Y += LabelPadding + halfOfLabelHeight;
                    }
                    break;
            }
        }*/

        public override void DrawAxisLabels(Graphics g, Point offset)
        {
            foreach (AxisEntry entry in Entries)
            {
                g.DrawString(entry.Label.Text, AxisFont, Brushes.Black, entry.Label.Position);
            }
        }

        /// <summary>
        /// Return the AxisEntry co-ordinates.
        /// This is the top-left of the associated label.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetAxisEntry(string key)
        {
            int rValue = -1;
            Point point = new Point(-1, -1);
            foreach (AxisEntry e in Entries)
            {
                DurationDataSeriesEntry dds = (DurationDataSeriesEntry)e;
                if (dds.KeyValue.Equals(key))
                {
                    point = dds.Position;
                    break;
                }
            }

            rValue = (AxisXY == Axis.X) ? point.X : point.Y;

            return rValue;
        }

        public override void DrawMajorGridLines(Graphics g)
        {
            throw new NotImplementedException();
        }

        public override void CalculateLabelPositions(Point offset)
        {
            throw new NotImplementedException();
        }
    }
}
