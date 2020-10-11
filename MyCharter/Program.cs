using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using MyCharter.ChartElements.Legend;
using MyCharter.Charts;
using System;
using System.Drawing;

namespace MyCharter
{
    class Program
    {
        static void Main(string[] args)
        {
            bool doStackedVertical = true;
            bool doDuration = true;

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
        }

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

        private static void DoDurationChartDemo(ElementPosition xAxisPositioning, ElementPosition yAxisPositioning)
        {
            DurationChart durationChart = new DurationChart();
            durationChart.Title = "Production ETL Loads";
            durationChart.SubTitle = "28/09 to 03/10 (midday)";
            durationChart.OutputFile = @"C:\New Folder\aDemo-duration-chart-" + xAxisPositioning.ToString() + "-" + yAxisPositioning.ToString() + ".png";
            durationChart.BarShape = BarShape.ROUNDED_RECTANGLE;
            durationChart.LoadDataPointsFromCSV(@"C:\New folder\etllog.csv");

            var xAxis = new DateAndTimeScaleAxis(new DateTime(2020, 9, 28, 0, 00, 0), new DateTime(2020, 10, 5, 9, 20, 0),
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

            durationChart.GenerateChart();

        }

    }
}
