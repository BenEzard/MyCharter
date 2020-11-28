using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using MyCharter.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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

        private int _contractionMajorIncrements = -1;
        private bool _leftTrimIfNoData = false;
        private bool _rightTrimIfNoData = false;


        public DurationChart() : base()
        {
        }

        public void ConfigureContractionsOnXAxis(int majorIncrementCount, bool leftTrimIfNoData, bool rightTrimIfNoData)
        {
            _contractionMajorIncrements = majorIncrementCount;
            _leftTrimIfNoData = leftTrimIfNoData;
            _rightTrimIfNoData = rightTrimIfNoData;
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

        /// <summary>
        /// Plot the data
        /// </summary>
        /// <param name="g"></param>
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
        /// <param name="filterMaximumDateTime"></param>
        /// <param name="filterMinimumDateTime"></param>
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
                        //Console.WriteLine($"Discarding data on row {lineCounter} because it doesn't meet filter requirements.");
                    }
                    
                }
            }
        }

        public override void GenerateChart()
        {
            base.GenerateChart();

            // test contraction functionality
            if (_contractionMajorIncrements > 0)
            {
                AnalyseAxisRegions(_contractionMajorIncrements, GetXAxis());

                var src = (Bitmap)Image.FromFile(OutputFile);
                Bitmap bmp2 = new Bitmap(src.Width, src.Height);

                int yTopOfChart = GetYAxis().AxisEntries[0].Position.Y;
                int yBottomOfChart = GetYAxis().AxisEntries[GetYAxis().AxisEntries.Count - 1].Position.Y;
                int xStartPosition = 0;
                int xOffset = 0;

                using (Graphics g2 = Graphics.FromImage(src))
                {
                    g2.Clear(Color.White);
                    g2.SmoothingMode = SmoothingMode.AntiAlias;

                    foreach (AxisRegion<DateTime> plotRegion in GetXAxis().AxisPlottedRegion)
                    {
                        int srcXLeft = plotRegion.Start.Position.X;
                        int srcXRight = plotRegion.End.Position.X + ((int)plotRegion.End.Label.Dimensions.Value.Width / 2);
                        int srcWidth = srcXRight - srcXLeft;
                        Console.WriteLine($"For plotRegion {plotRegion} copying {srcXLeft}, {yTopOfChart}, {srcWidth}, {yBottomOfChart}");
                        Rectangle sourceRectangle = new Rectangle(srcXLeft, yTopOfChart, srcWidth, yBottomOfChart);
                        ImageMethods.CopyRegionIntoImage(src, sourceRectangle, ref bmp2, 
                            new Rectangle(xOffset, 0, srcWidth, yBottomOfChart));
                        xOffset += srcWidth;
                        bmp2.Save(@"c:\New Folder\manip.png", ImageFormat.Png);
                        bmp2.Dispose();
                        break;
                    }
                }

                

                    /*var src = (Bitmap)Image.FromFile(OutputFile);
                    Bitmap bmp2 = new Bitmap(src.Width, src.Height);
                    using (Graphics g2 = Graphics.FromImage(src))
                    {
                        g2.Clear(Color.White);
                        g2.SmoothingMode = SmoothingMode.AntiAlias;
                        int xStartPosition = 0;
                        int xOffset = 0;

                        int yTopOfChart = GetYAxis().AxisEntries[0].Position.Y;
                        int yBottomOfChart = GetYAxis().AxisEntries[GetYAxis().AxisEntries.Count - 1].Position.Y;

                        float[] dashValues = { 2, 2 };
                        Pen linePen = new Pen(Brushes.LightGray, 1);
                        linePen.DashPattern = dashValues;

                        for (int i = 0; i < GetXAxis().AxisContractionRegion.Count - 1; i++)
                        {
                            var contractions = GetXAxis().AxisContractionRegion[i];
                            Console.WriteLine($"Copying to {contractions.Start.Position.X}");
                            // Add a little bit on to ensure that it gets the full label
                            xOffset = contractions.Start.Position.X + ((int)contractions.Start.Label.Dimensions.Value.Width / 2);
                            ImageMethods.CopyRegionIntoImage(src, new Rectangle(new Point(xStartPosition, 0), new Size(xOffset, src.Height)),
                                ref bmp2, new Rectangle(new Point(xStartPosition, 0), new Size(xOffset + 50, src.Height)));

                            if (i + 1 <= GetXAxis().AxisContractionRegion.Count - 1) // more
                            {
                                Console.WriteLine($"Top y axis item position is {GetYAxis().AxisEntries[0].Position.Y}");
                                int yLineOffset = yTopOfChart;
                                while (yLineOffset <= yBottomOfChart)
                                {
                                    yLineOffset += 5;
                                    g2.DrawLine(linePen, new Point(xOffset, yLineOffset), new Point(xOffset + 10, yLineOffset));
                                }

                            }
                            else // no more
                            {

                            }
                            //break;
                        }
                        //g2.DrawString("hi there", TitleFont, Brushes.Black, new Point(900, 900));
                    }

                    bmp2.Save(@"c:\New Folder\manip.png", ImageFormat.Png);*/
                }

            }

        private void AnalyseAxisRegions(int consecutiveMajorIncrements, AbstractChartAxis<DateTime> axis)
        {
            if (axis.AxisXY != Axis.X) throw new ArgumentException("AnalyseAxis can only be called on the X-Axis of the Duration Chart.");

            int consecutiveMajIncsNoDataCounter = 0; // A counter of the number of consecutive major increments without any data.

            var minimum = axis.GetMinimumAxisEntry();
            var maximum = axis.GetMaximumAxisEntry();
            
            AxisEntry<DateTime> startOfContraction = null;
            AxisEntry<DateTime> endOfContraction;
            AxisEntry<DateTime> startOfPlottedRange = null;
            AxisEntry<DateTime> endOfPlottedRange;

            List<AxisEntry<DateTime>> majorIncrements = axis.GetMajorAxisEntries();

            for (int i = 0; i < majorIncrements.Count - 1; i++)
            {
                // Is there data between this major increment and the next?
                if (IsDataWithin(majorIncrements[i].KeyValue, majorIncrements[i + 1].KeyValue))
                {
                    consecutiveMajIncsNoDataCounter = 0;

                    // If there's an open contraction range, then close it.
                    if (startOfContraction != null)
                    {
                        endOfContraction = majorIncrements[i];
                        CloseAndAddRegion(startOfContraction, endOfContraction, false);
                        startOfContraction = null;

                    }

                    // If no plottable region is open
                    if (startOfPlottedRange == null)
                    {
                        startOfPlottedRange = majorIncrements[i];
                    }
                }
                else // No data in next segment
                {
                    ++consecutiveMajIncsNoDataCounter;

                    // If left trim is allowed, then start trimming (even if it hasn't hit the consecutive count of blank increments yet)
                    if (_leftTrimIfNoData && i == 0)
                    {
                        startOfContraction = majorIncrements[i];
                    }

                    if (consecutiveMajIncsNoDataCounter == consecutiveMajorIncrements)
                    {
                        // If there's an open plotted range, then close it.
                        if (startOfPlottedRange != null)
                        {
                            endOfPlottedRange = majorIncrements[i];
                            CloseAndAddRegion(startOfPlottedRange, endOfPlottedRange, true);
                            startOfPlottedRange = null;
                        }

                        startOfContraction = majorIncrements[i];
                    }
                }
            }
            // Check to see if there's an open contraction or plottable region
            if (startOfContraction != null)
            {
                endOfContraction = maximum;
                CloseAndAddRegion(startOfContraction, endOfContraction, false);
            }
            if (startOfPlottedRange != null)
            {
                endOfPlottedRange = maximum;
                CloseAndAddRegion(startOfPlottedRange, endOfPlottedRange, true);
            }

            void CloseAndAddRegion(AxisEntry<DateTime> start, AxisEntry< DateTime> end, bool isPlottable) {
                var axisRegion = new AxisRegion<DateTime>(start, end);

                if (isPlottable)
                    axis.AddAxisPlottedRegion(axisRegion);
                else
                    axis.AddAxisContraction(axisRegion);
            }

        }

        /// <summary>
        /// Checks to see if there is data within the specified bounds.
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public bool IsDataWithin(DateTime startDateTime, DateTime endDateTime)
        {
            bool rValue = false;

            foreach (DataSeries<DateTime, string, Duration> dataSeries in ChartData)
            {
                foreach (DataPoint<DateTime, string, Duration> dataPoint in dataSeries.DataPoints)
                {
                    if ((dataPoint.DataPointData.StartDateTime >= startDateTime) && (dataPoint.DataPointData.StartDateTime <= endDateTime) ||
                        (dataPoint.DataPointData.EndDateTime >= startDateTime) && (dataPoint.DataPointData.EndDateTime <= endDateTime))
                    {
                        rValue = true;
                        break;
                    }
                }
                if (rValue)
                    break;
            }
            
            return rValue;
        }
    }

    public enum BarShape
    {
        RECTANGLE,
        ROUNDED_RECTANGLE,
    }
}
