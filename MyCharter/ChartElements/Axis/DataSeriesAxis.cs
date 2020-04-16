using System;
using System.Drawing;

namespace MyCharter
{
    public class DataSeriesAxis : AbstractChartAxis
    {
        public DataSeriesAxis() : base(AxisFormat.DATA_SERIES)
        {
        }

        internal override void GenerateAxisValues()
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
                    int halfOfLabelHeight = _maxLabelHeight % 2 == 0 ? _maxLabelHeight / 2 : (_maxLabelHeight - 1) / 2;
                    int x = offset.X - _maxLabelWidth - AxisPadding;
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


        public override SizeF GetAxisLabelDimensions()
        {
            int width = 0;
            int height = 0;
            switch (AxisXY)
            {
                case Axis.X:
                    throw new NotImplementedException("DataSeriesAxis.GetAxisLabelDimensions not implemented for X axis.");
                case Axis.Y:
                    width = _maxLabelWidth + AxisPadding;
                    Console.WriteLine($"(LabelPadding * Entries.Count) + LabelPadding + (Entries.Count * _maxLabelHeight)");
                    Console.WriteLine($"({LabelPadding} * {(Entries.Count + 1)}) + {LabelPadding} + ({(Entries.Count + 1)} * {_maxLabelHeight})");
                    Console.WriteLine($"({LabelPadding * (Entries.Count + 1)}) + {LabelPadding} + ({Entries.Count + 1 * _maxLabelHeight})");
                    height = (LabelPadding * (Entries.Count + 1)) + LabelPadding + ((Entries.Count + 1) * _maxLabelHeight);
                    Console.WriteLine($"height = {height}");
                    break;
            }

            return new SizeF(width, height);
        }

        public override void DrawMajorGridLines(Graphics g)
        {
            throw new NotImplementedException();
        }
    }
}
