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

        public void AddDataSeries(DurationDataSeries dataSeries)
        {
            AddEntry(dataSeries);
        }

        internal override SizeF GetLabelDimensions()
        {
            var maxDimensions = GetMaxLabelDimensions();
            int width = 0;
            int height = 0; 

            if (AxisXY == Axis.Y) // If the data series are displayed on the Y axis
            {
                width += (int)maxDimensions.Width + AxisPadding;
                height = ((int)maxDimensions.Height * (Entries.Count + 1)) + (2 * LabelPadding * (Entries.Count + 1));
            }
            else if (AxisXY == Axis.X)
            {
                height = (int)maxDimensions.Height + AxisPadding;
                throw new NotImplementedException("Code not yet written");
            }
            return new SizeF(width, height);
        }

        public override void DrawAxis(Graphics g, ref Point offset)
        {
            switch (AxisXY)
            {
                case Axis.X:
                    Console.WriteLine("here");
                    break;
                case Axis.Y:
                    foreach (var e in Entries.Values)
                    {
                        if (e is AxisEntry entry)
                        {
                            offset.Y += LabelPadding;
                            g.DrawString(entry.Label.Label, AxisFont, Brushes.Black, offset);
                            offset.Y += (int)entry.Label.LabelDimensions.Value.Height + LabelPadding;
                        }
                    }
                    break;
            }
/*            if (AxisXY == Axis.X)
            {
                foreach (var e in Entries.Values)
                {
                    if (e is Tick entry2)
                    {
                        Console.WriteLine("--is a tick");
                        if (entry2.IsMajorTick)
                        {
                            g.DrawString(entry2.Label.Label, AxisFont, Brushes.Black, offset.X, offset.Y);
                        }
                        offset.X += PixelsPerIncrement;

                    }
                    else if (e is AxisEntry entry)
                    {
                        Console.WriteLine("--need to do something here");
                    }
                }
            }*/

        }
    }
}
