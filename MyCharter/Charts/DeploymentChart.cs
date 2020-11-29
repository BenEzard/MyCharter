using MyCharter.ChartElements.DataSeries;
using System;
using System.Drawing;
using System.IO;


namespace MyCharter.Charts
{
    public class DeploymentChart : AbstractChart<DateTime, string, PlottableShape>
    {
        public int ShapeSize { get; set; } = 10;
        public Color Color1;
        public Color Color2;


        public override void CalculateDataPointLabelDimensions()
        {
            //throw new NotImplementedException();
        }

        public override void PlotData(Graphics g)
        {
            foreach (DataSeries<DateTime, string, PlottableShape> dataSeries in ChartData)
            {
                int y = GetYAxis().GetAxisPosition(dataSeries.Name);
                foreach (DataPoint<DateTime, string, PlottableShape> dataPoint in dataSeries.DataPoints)
                {
                    int x = GetXAxis().GetAxisPosition(dataPoint.xAxisCoord);
                    g.FillEllipse(new SolidBrush(dataPoint.DataPointData.Color), new Rectangle(new Point(x, y - (ShapeSize / 2)), new Size(ShapeSize, ShapeSize)));
                }
            }
        }

        /// <summary>
        /// Load DataPoints from a CSV, excluding any data that is outside of the minimum and maximum requirements. 
        /// Expects a CSV file of type: report_id, version_major, version_no, releaseDate
        /// </summary>
        /// <param name="fileNameAndPath"></param>
        /// <param name="filterMaximumDateTime"></param>
        /// <param name="filterMinimumDateTime"></param>
        /// <returns>The number of lines loaded.</returns>
        public int LoadDataPointsFromCSV(string fileNameAndPath, DateTime? filterMinimumDateTime, DateTime? filterMaximumDateTime)
        {
            if (File.Exists(fileNameAndPath) != true) throw new FileNotFoundException($"The specified data file ({fileNameAndPath}) does not exist.");

            int lineCounter = 0;
            using (var reader = new StreamReader(fileNameAndPath))
            {
                while (reader.EndOfStream == false)
                {
                    ++lineCounter;
                    var line = reader.ReadLine();

                    if (line == ",,,")
                        break;

                    var values = line.Split(',');

                    string reportID = values[0];
                    int majorVersionNumber = Int32.Parse((string)values[1]);
                    int minorVersionNumber = Int32.Parse((string)values[2]);
                    string releaseDateStr = (string)values[3];


                    int firstSlashPos = releaseDateStr.IndexOf("/");
                    int secondSlashPos = releaseDateStr.IndexOf("/", firstSlashPos + 1);
                    int day = Int32.Parse(releaseDateStr.Substring(0, firstSlashPos));
                    int month = Int32.Parse(releaseDateStr.Substring(firstSlashPos + 1, 2));
                    int year = Int32.Parse(releaseDateStr.Substring(secondSlashPos + 1, 4));
                    DateTime releaseDate = new DateTime(year, month, day);

                    Color color = Color2;

                    // if new development
                    if (minorVersionNumber == 0)
                        color = Color1;

                    if ((releaseDate >= filterMinimumDateTime) && (releaseDate <= filterMaximumDateTime))
                    {
                        AddDataPoint(reportID, releaseDate, new PlottableShape(color));
                    }
                    else
                    {
                        //Console.WriteLine($"Discarding data on row {lineCounter} because it doesn't meet filter requirements.");
                    }

                }
            }
            return lineCounter;
        }

        /// <summary>
        /// Add a DataPoint.
        /// If the DataSeries doesn't exist it will be created.
        /// </summary>
        /// <param name="dataSeriesName"></param>
        /// <param name="plottableShape"></param>
        public void AddDataPoint(string dataSeriesName, DateTime date, PlottableShape plottableShape)
        {
            // Check to see if that data series exists
            var dataSeries = (PlottableShapeDataSeries)GetDataSeries(dataSeriesName);
            if (dataSeries != null)
            {
                dataSeries.AddDataPoint(date, plottableShape);
            }
            else
            {
                // TODO Not sure how we should colour the default series.
                dataSeries = new PlottableShapeDataSeries(dataSeriesName, Color.CornflowerBlue);
                AddDataSeries(dataSeries);
                dataSeries.AddDataPoint(date, plottableShape);
            }
        }

        /// <summary>
        /// Return a DataSeries with the specified name.
        /// </summary>
        /// <param name="dataSeriesName"></param>
        /// <returns></returns>
        public DataSeries<DateTime, string, PlottableShape> GetDataSeries(string dataSeriesName)
        {
            PlottableShapeDataSeries rValue = null;
            foreach (PlottableShapeDataSeries ds in ChartData)
            {
                if (ds.Name == dataSeriesName)
                {
                    rValue = ds;
                    break;
                }
                    
            }
            return rValue;
        }
    }
}
