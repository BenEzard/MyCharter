using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MyCharter
{
    public class BarChart : AbstractChart
    {
        public BarChart() : base(ChartType.BAR_CHART)
        {
        }

        private GraphicsPath MakeRoundedRect(RectangleF rect, float xradius, float yradius,
            bool roundTopLeft, bool roundTopRight, bool roundBottomLeft, bool roundBottomRight)
        {
            // Make a GraphicsPath to draw the rectangle.
            PointF point1, point2;
            GraphicsPath path = new GraphicsPath();

            // Upper left corner.
            if (roundTopLeft)
            {
                RectangleF corner = new RectangleF(rect.X, rect.Y, 2 * xradius, 2 * yradius);
                path.AddArc(corner, 180, 90);
                point1 = new PointF(rect.X + xradius, rect.Y);
            }
            else point1 = new PointF(rect.X, rect.Y);

            // Top side.
            if (roundTopRight)
                point2 = new PointF(rect.Right - xradius, rect.Y);
            else
                point2 = new PointF(rect.Right, rect.Y);
            path.AddLine(point1, point2);

            // Upper right corner.
            if (roundTopRight)
            {
                RectangleF corner = new RectangleF(rect.Right - 2 * xradius, rect.Y, 2 * xradius, 2 * yradius);
                path.AddArc(corner, 270, 90);
                point1 = new PointF(rect.Right, rect.Y + yradius);
            }
            else point1 = new PointF(rect.Right, rect.Y);

            // Right side.
            if (roundBottomRight)
                point2 = new PointF(rect.Right, rect.Bottom - yradius);
            else
                point2 = new PointF(rect.Right, rect.Bottom);
            path.AddLine(point1, point2);

            // Lower right corner.
            if (roundBottomRight)
            {
                RectangleF corner = new RectangleF(rect.Right - 2 * xradius, rect.Bottom - 2 * yradius, 2 * xradius, 2 * yradius);
                path.AddArc(corner, 0, 90);
                point1 = new PointF(rect.Right - xradius, rect.Bottom);
            }
            else point1 = new PointF(rect.Right, rect.Bottom);

            // Bottom side.
            if (roundBottomLeft)
                point2 = new PointF(rect.X + xradius, rect.Bottom);
            else
                point2 = new PointF(rect.X, rect.Bottom);
            path.AddLine(point1, point2);

            // Lower left corner.
            if (roundBottomLeft)
            {
                RectangleF corner = new RectangleF(rect.X, rect.Bottom - 2 * yradius, 2 * xradius, 2 * yradius);
                path.AddArc(corner, 90, 90);
                point1 = new PointF(rect.X, rect.Bottom - yradius);
            }
            else point1 = new PointF(rect.X, rect.Bottom);

            // Left side.
            if (roundTopLeft)
                point2 = new PointF(rect.X, rect.Y + yradius);
            else
                point2 = new PointF(rect.X, rect.Y);
            path.AddLine(point1, point2);

            // Join with the start point.
            path.CloseFigure();

            return path;
        }

        public override void PlotData(Graphics g)
        {
            // TODO this needs to be elsewhere
            var xAxis = (TimeScaleAxis)GetAxis(Axis.X);
            var yAxis = (DataSeriesAxis)GetAxis(Axis.Y);
            
            if (xAxis is DataSeriesAxis)
            {
                throw new NotImplementedException("BarChart.PlotData() is not implemented for x-axis yet.");
            }

            int height = (int)yAxis.GetMaxLabelDimensions().Height;
            if (yAxis is DataSeriesAxis)
            {
                foreach (AxisEntry entry in yAxis.Entries)
                {
                    if (entry is DurationDataSeriesEntry dsEntry)
                    {
                        Duration d = (Duration)dsEntry.EntryContent;
                        int startX = xAxis.GetAxisEntry(d.StartTime.TimeOfDay);
                        int startY = yAxis.GetAxisEntry(dsEntry.Label.Text);
                        int endX = xAxis.GetAxisEntry(d.EndTime.TimeOfDay);

                        var color = System.Drawing.ColorTranslator.FromHtml("#04B404");
                        if ((endX - startX) < (2 * 7))
                        {
                            g.FillRectangle(new SolidBrush(color), startX, startY, (endX - startX), height);
                        }
                        else
                        {
                            GraphicsPath path = MakeRoundedRect(new RectangleF(startX, startY, (endX - startX), height), 7, 7, true, true, true, true);
                            g.FillPath(new SolidBrush(color), path);
                            g.DrawPath(new Pen(Brushes.Black, 1), path);
                        }

                        if (ShowDataLabels)
                        {
                            TimeSpan length = d.EndTime.TimeOfDay - d.StartTime.TimeOfDay;
                            string dataLabel = length.Hours.ToString().PadLeft(2, '0') + ":" + length.Minutes.ToString().PadLeft(2, '0');
                            g.DrawString(dataLabel, DataLabelFont, Brushes.LightGray, new Point(startX, startY));
                        }


                    }
                }

            }
        }

    }
}
