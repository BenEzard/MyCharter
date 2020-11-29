using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using MyCharter.ChartElements.Legend;
using MyCharter.Charts;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MyCharter
{
    /// <summary>
    /// 
    /// 
    /// 
    /// TODO: LoadFromCSV should be an interface
    /// 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            bool doStackedVertical = true;
            bool doDuration = true;
            bool doDeployment = true;

            if (doStackedVertical)
            {
                Console.WriteLine("==========" + ElementPosition.TOP.ToString() + "-" + ElementPosition.LEFT.ToString());
                DoStackedChartDemo(ElementPosition.TOP, ElementPosition.LEFT);

                Console.WriteLine("==========" + ElementPosition.TOP.ToString() + "-" + ElementPosition.RIGHT.ToString());
                DoStackedChartDemo(ElementPosition.TOP, ElementPosition.RIGHT);

                Console.WriteLine("==========" + ElementPosition.BOTTOM.ToString() + "-" + ElementPosition.LEFT.ToString());
                DoStackedChartDemo(ElementPosition.BOTTOM, ElementPosition.LEFT);

                Console.WriteLine("==========" + ElementPosition.BOTTOM.ToString() + "-" + ElementPosition.RIGHT.ToString());
                DoStackedChartDemo(ElementPosition.BOTTOM, ElementPosition.RIGHT);
            }

            if (doDuration)
            {
                Console.WriteLine("========== Duration chart");
                DoDurationChartDemo(ElementPosition.BOTTOM, ElementPosition.LEFT);
            } 
            
            if (doDeployment)
            {
                Console.WriteLine("\n========== Deployment chart");
                DoDeploymentChartDemo(ElementPosition.BOTTOM, ElementPosition.LEFT);
            }
        }

        /// <summary>
        /// Create a Stacked Chart Demo
        /// </summary>
        /// <param name="xAxisPositioning"></param>
        /// <param name="yAxisPositioning"></param>
        private static void DoStackedChartDemo(ElementPosition xAxisPositioning, ElementPosition yAxisPositioning)
        {
            StackedVerticalBarChart<DateTime, int, Point> svChart = new StackedVerticalBarChart<DateTime,int, Point>();
            svChart.Title = "Demo of Stacked Chart";
            svChart.SubTitle = "Orientation: x-Axis: " + xAxisPositioning.ToString() + ", y-Axis: " + yAxisPositioning.ToString();
            svChart.OutputFile = @"C:\New Folder\aDemo-stacked-vertical-chart-"+ xAxisPositioning.ToString()+ "-" + yAxisPositioning.ToString() + ".png";
            
            var xAxis = new DateScaleAxis(new DateTime(2020, 5, 15), new DateTime(2020, 6, 15), 1, 0, 30, AxisLabelFormat.DATE_DDMM1);
            xAxis.MajorGridLine = true;
            xAxis.LabelHorizontalPosition = AxisLabelHorizontalPosition.CENTER;
            svChart.SetXAxis(xAxisPositioning, xAxis, AxisWidth.FIT_TO_INCREMENT);

            var yAxis = new NumberScaleAxis(minimumValue:0, maximumValue:5000, majorIncrement:1000, 
                minorIncrement:250, pixelsPerIncrement:10, 
                AxisLabelFormat.NUMBER_THOU_SEP_COMMA);
            yAxis.MajorGridLine = true;
            svChart.SetY1Axis(yAxisPositioning, yAxis, AxisWidth.FIT_TO_INCREMENT);

            var volunteersDS = new DataSeries<DateTime, int, Point>("Volunteers", Color.Orange, AxisLabelFormat.NUMBER_THOU_SEP_COMMA, LegendDisplayType.SQUARE);
            volunteersDS.AddDataPoint(new DateTime(2020, 5, 15), 2700);
            volunteersDS.AddDataPoint(new DateTime(2020, 5, 16), 1500);
            volunteersDS.AddDataPoint(new DateTime(2020, 5, 17), 1320);
            volunteersDS.AddDataPoint(new DateTime(2020, 5, 19), 100);
            svChart.AddDataSeries(volunteersDS);

            var centrelinkDS = new DataSeries<DateTime, int, Point>("Centrelink", Color.Red, AxisLabelFormat.NUMBER_THOU_SEP_COMMA, LegendDisplayType.SQUARE);
            centrelinkDS.AddDataPoint(new DateTime(2020, 5, 15), 1200);
            centrelinkDS.AddDataPoint(new DateTime(2020, 5, 16), 500);
            centrelinkDS.AddDataPoint(new DateTime(2020, 5, 18), 550);
            svChart.AddDataSeries(centrelinkDS);

            var socialDS = new DataSeries<DateTime, int, Point>("Social", Color.DeepSkyBlue, AxisLabelFormat.NUMBER_THOU_SEP_COMMA, LegendDisplayType.LINE);
            socialDS.AddDataPoint(new DateTime(2020, 5, 16), 1271);
            svChart.AddDataSeries(socialDS);

            svChart.GenerateChart();
        }


        private static void DoDeploymentChartDemo(ElementPosition xAxisPositioning, ElementPosition yAxisPositioning)
        {
            DateTime minimumDate = new DateTime(2019, 5, 16);
            DateTime maximumDate = new DateTime(2020, 11, 25);

            DeploymentChart deploymentChart = new DeploymentChart();
            deploymentChart.Title = "Deployment Chart";
            deploymentChart.SubTitle = $"{minimumDate.ToShortDateString()} to {maximumDate.ToShortDateString()}";
            deploymentChart.OutputFile = @"C:\New Folder\aDemo-deployment-chart-" + xAxisPositioning.ToString() + "-" + yAxisPositioning.ToString() + ".png";

            deploymentChart.Color1 = Color.RoyalBlue;
            deploymentChart.Color2 = Color.MediumVioletRed;
            deploymentChart.LoadDataPointsFromCSV(@"C:\New folder\deployment_data.csv", minimumDate, maximumDate);

            var xAxis = new DateScaleAxis(minimumDate, maximumDate, 14, 1, 3, AxisLabelFormat.DATE_DDMMYYYY2);
            xAxis.LabelHorizontalPosition = AxisLabelHorizontalPosition.CENTER;
            deploymentChart.SetXAxis(xAxisPositioning, xAxis, AxisWidth.FIT_TO_INCREMENT, 90);

            var yAxis = new LabelAxis(30, deploymentChart.GetDataSeriesNames());
            yAxis.MajorGridLine = true;
            yAxis.AlternatingMajorGridLines = true;
            yAxis.MajorGridLinePen1.DashStyle = DashStyle.Dot;
            yAxis.MajorGridLinePen1.DashPattern = new float[] { 1, 10 };
            yAxis.MajorGridLinePen2.DashStyle = DashStyle.Dash;
            yAxis.MajorGridLinePen2.DashPattern = new float[] { 1, 20 };
            deploymentChart.SetY1Axis(yAxisPositioning, yAxis, AxisWidth.FIT_TO_INCREMENT);

            

            deploymentChart.GenerateChart();
        }

        private static void DoDurationChartDemo(ElementPosition xAxisPositioning, ElementPosition yAxisPositioning)
        {
            DurationChart durationChart = new DurationChart();
            durationChart.Title = "Production ETL Loads";
            durationChart.SubTitle = "28/09 to 03/10 (midday)";
            durationChart.OutputFile = @"C:\New Folder\aDemo-duration-chart-" + xAxisPositioning.ToString() + "-" + yAxisPositioning.ToString() + ".png";
            durationChart.BarShape = BarShape.ROUNDED_RECTANGLE;
            durationChart.LoadDataPointsFromCSV(@"C:\New folder\etllog.csv", new DateTime(2020, 9, 30, 0, 0, 0), new DateTime(2020, 10, 1, 11, 0, 0));

            var xAxis = new DateAndTimeScaleAxis(new DateTime(2020, 9, 30, 0, 0, 0), new DateTime(2020, 10, 1, 11, 0, 0),
                60, 10, 10, AxisLabelFormat.DATETIME_DDMMYYYY1_HHMM24);
            xAxis.MajorGridLine = true;
            xAxis.LabelHorizontalPosition = AxisLabelHorizontalPosition.CENTER;
            durationChart.SetXAxis(xAxisPositioning, xAxis, AxisWidth.FIT_TO_INCREMENT, 90);
            
            var yAxis = new LabelAxis(30, durationChart.GetDataSeriesNames());
            yAxis.MajorGridLine = true;
            yAxis.AlternatingMajorGridLines = true;
            durationChart.SetY1Axis(yAxisPositioning, yAxis, AxisWidth.FIT_TO_INCREMENT);
            
            durationChart.ChartLegend.IsLegendVisible = true;
            durationChart.ChartLegend.Layout = LegendLayout.HORIZONTAL;
            durationChart.ChartLegend.AddEntry(new LegendEntry(LegendDisplayType.SQUARE, Color.CornflowerBlue, "Successful Run"));
            durationChart.ChartLegend.AddEntry(new LegendEntry(LegendDisplayType.SQUARE, Color.Red, "Failed Run"));

            //durationChart.ConfigureContractionsOnXAxis(2, true, true);
            //durationChart.DetectContractions(2, xAxis);

/*            Console.WriteLine($"Checking between 7 and 8 am: {durationChart.HasData(new DateTime(2020, 9, 30, 7, 0, 0), new DateTime(2020, 9, 30, 8, 0, 0))}");
            Console.WriteLine($"Checking between 8 and 9 am: {durationChart.HasData(new DateTime(2020, 9, 30, 8, 0, 0), new DateTime(2020, 9, 30, 9, 0, 0))}");
            Console.WriteLine($"Checking between 9 and 10 am: {durationChart.HasData(new DateTime(2020, 9, 30, 9, 0, 0), new DateTime(2020, 9, 30, 10, 0, 0))}");
            Console.WriteLine($"Checking between 10 and 11 am: {durationChart.HasData(new DateTime(2020, 9, 30, 10, 0, 0), new DateTime(2020, 9, 30, 11, 0, 0))}");
            Console.WriteLine($"Checking between 11 and 12: {durationChart.HasData(new DateTime(2020, 9, 30, 11, 0, 0), new DateTime(2020, 9, 30, 8, 12, 0))}");
            Console.WriteLine($"Checking between 12 and 1 pm: {durationChart.HasData(new DateTime(2020, 9, 30, 12, 0, 0), new DateTime(2020, 9, 30, 13, 0, 0))}");
            Console.WriteLine($"Checking between 1 and 2 pm: {durationChart.HasData(new DateTime(2020, 9, 30, 13, 0, 0), new DateTime(2020, 9, 30, 14, 0, 0))}");*/

            durationChart.GenerateChart();

        }

    }
}
