using MyCharter.ChartElements.DataSeries;
using MyCharter.Util;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;

namespace MyCharter.Charts
{
    public class DurationChart : AbstractChart<DateTime, string, Duration>
    {
        private int _barHeight;
        public int BarHeight
        {
            get => _barHeight;
            set
            {
                if (value > GetYAxis().PixelsPerIncrement)
                    throw new ArgumentException("Bar Height must be less than or equal to PixelsPerIncrement.");

                _barHeight = value;
            }
        }

        /// <summary>
        /// Shape of the bar.
        /// </summary>
        public BarShape BarShape = BarShape.RECTANGLE;

        public DurationChart() : base()
        {
        }

        public override void CalculateDataPointLabelDimensions()
        {
         /*   // Create a temporary BMP for 'sketching'
            Bitmap tempBMP = new Bitmap(300, 300);
            Graphics tempGraphics = Graphics.FromImage(tempBMP);

            var yAxis = GetYAxis();

            foreach (DataSeries<DateTime, string, Duration> dataSeries in ChartData)
            {
                if (dataSeries.DataSeriesLabelFormat != AxisLabelFormat.NONE)
                {
                    foreach (DataPoint<Duration, string> dataPoint in dataSeries.DataPoints)
                    {
                        string label = yAxis.FormatLabelString(dataPoint.yAxisCoord, false);
                        dataPoint.DataPointLabel = new ImageText(label);
                        // Because this is a vertical bar chart, we know that the DataPoint value we want to label will be on the TYAxis value.
                        SizeF stringMeasurement = tempGraphics.MeasureString(label, yAxis.DataPointLabelFont);
                        dataPoint.DataPointLabel.Dimensions = stringMeasurement;
                    }
                }

            tempBMP.Dispose();*/
        }

        public override void PlotData(Graphics g)
        {
            BarHeight = GetYAxis().PixelsPerIncrement - 10;

            foreach (DataSeries<DateTime, string, Duration> dataSeries in ChartData)
            {
                foreach (DataPoint<DateTime, string, Duration> dataPoint in dataSeries.DataPoints)
                {
                    Point startPoint = GetChartPosition(dataPoint.DataPointData.StartDateTime, dataSeries.Name);
                    Point endPoint = GetChartPosition(dataPoint.DataPointData.EndDateTime, dataSeries.Name);

                    Rectangle rec = new Rectangle(startPoint.X, (startPoint.Y - (_barHeight) / 2), (endPoint.X - startPoint.X), _barHeight);
                    if ((BarShape == BarShape.RECTANGLE) || (rec.Width <= 10))
                    {
                        g.FillRectangle(new SolidBrush(dataPoint.DataPointData.Color), rec);
                    }
                    else if ((BarShape == BarShape.ROUNDED_RECTANGLE) || (rec.Width > 10))
                    {
                        using (GraphicsPath path = RoundedRect(rec, 5))
                        {
                            g.FillPath(new SolidBrush(dataPoint.DataPointData.Color), path);
                        }
                    }
                    
                    TimeSpan durationLength = (dataPoint.DataPointData.EndDateTime - dataPoint.DataPointData.StartDateTime);
                    string durationString = durationLength.Hours.ToString().PadLeft(2, '0') + ":" + durationLength.Minutes.ToString().PadLeft(2, '0');
                    if (durationString == "00:00")
                        g.DrawString(durationString,
                            new Font("Arial", 8 * 1.33f, FontStyle.Regular, GraphicsUnit.Point), 
                            Brushes.Red, 
                            new Point(startPoint.X, startPoint.Y - (_barHeight/2)));
                    else
                        g.DrawString(durationString,
                        new Font("Arial", 8 * 1.33f, FontStyle.Regular, GraphicsUnit.Point),
                        Brushes.Black,
                        new Point(startPoint.X, startPoint.Y - (_barHeight / 2)));

                }
            }
        }

        /// <summary>
        /// Calculate the GraphicsPath for a rounded rectangle with the specified bounds and radius.
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Return the Minimum and Maximum values for the X Axis.
        /// </summary>
        /// <returns></returns>
        public override (DateTime Min, DateTime Max) GetXAxisBounds()
        {
            DateTime Min = ChartData.SelectMany(ds => ds.DataPoints.Select(dp => dp.DataPointData.StartDateTime)).Min();
            DateTime Max = ChartData.SelectMany(ds => ds.DataPoints.Select(dp => dp.DataPointData.EndDateTime)).Max();
            // temp fix - make sure the max is greater than the chart
            Max = Max.AddMinutes(1);
            return (Min, Max);
        }

        /// <summary>
        /// Get the Minimum and the Maximum values for the Y Axis.
        /// </summary>
        /// <returns></returns>
        public override (string Min, string Max) GetYAxisBounds()
        {
            string Min = ChartData.Select(ds => ds.Name).First();
            string Max = ChartData.Select(ds => ds.Name).Last();

            return (Min, Max);
        }

        /// <summary>
        /// Add a DataPoint.
        /// If the DataSeries doesn't exist it will be created.
        /// </summary>
        /// <param name="dataSeriesName"></param>
        /// <param name="duration"></param>
        public void AddDataPoint(string dataSeriesName, Duration duration)
        {
            // Check to see if that data series exists
            var dataSeries = (DurationDataSeries)GetDataSeries(dataSeriesName);
            if (dataSeries != null)
            {
                dataSeries.AddDataPoint(duration);
            } else
            {
                // TODO Not sure how we should colour the default series.
                dataSeries = new DurationDataSeries(dataSeriesName, Color.CornflowerBlue);
                AddDataSeries(dataSeries);
                dataSeries.AddDataPoint(duration);
            }
        }

        /// <summary>
        /// Return a DataSeries with the specified name.
        /// </summary>
        /// <param name="dataSeriesName"></param>
        /// <returns></returns>
        public DataSeries<DateTime, string, Duration> GetDataSeries(string dataSeriesName)
        {
            DurationDataSeries rValue = null;
            foreach (DurationDataSeries ds in ChartData)
            {
                if (ds.Name == dataSeriesName)
                    rValue = ds;
            }
            return rValue;
        }

        /// <summary>
        /// Load DataPoints from a CSV. 
        /// </summary>
        /// <param name="fileNameAndPath"></param>
        public void LoadDataPointsFromCSV(string fileNameAndPath)
        {
            using (var reader = new StreamReader(fileNameAndPath))
            {
                while (reader.EndOfStream == false)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    DateTime startDT = CastMethods.StringToDateTime(values[1]);
                    DateTime endDT = CastMethods.StringToDateTime(values[3]);
                    Color durationColor = Color.CornflowerBlue;
                    if (values[4].Length > 0)
                        durationColor = Color.Red;
                    AddDataPoint(values[0], new Duration(startDT, endDT, durationColor));
                }
            }
        }

        /// <summary>
        /// Load DataPoints from a CSV, excluding any data that is outside of the minimum and maximum requirements. 
        /// </summary>
        /// <param name="fileNameAndPath"></param>
        public void LoadDataPointsFromCSV(string fileNameAndPath, DateTime filterMinimumDateTime, DateTime filterMaximumDateTime)
        {
            using (var reader = new StreamReader(fileNameAndPath))
            {
                int lineCounter = 0;
                while (reader.EndOfStream == false)
                {
                    ++lineCounter;
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    DateTime startDT = CastMethods.StringToDateTime(values[1]);
                    DateTime endDT = CastMethods.StringToDateTime(values[3]);
                    Color durationColor = Color.CornflowerBlue;
                    if (values[4].Length > 0)
                        durationColor = Color.Red;

                    if ((startDT >= filterMinimumDateTime) && (endDT <= filterMaximumDateTime))
                    {
                        AddDataPoint(values[0], new Duration(startDT, endDT, durationColor));
                    } 
                    else
                    {
                        Console.WriteLine($"Discarding data on row {lineCounter} because it doesn't meet filter requirements.");
                    }
                    
                }
            }
        }

    }

    public enum BarShape
    {
        RECTANGLE,
        ROUNDED_RECTANGLE,
    }
}
