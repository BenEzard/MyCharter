using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using MyCharter.Charts;
using System;
using System.Drawing;

namespace MyCharter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==========" + ElementPosition.TOP.ToString() + "-" + ElementPosition.LEFT.ToString());
            DoStackedChartDemo(ElementPosition.TOP, ElementPosition.LEFT);

            Console.WriteLine("==========" + ElementPosition.TOP.ToString() + "-" + ElementPosition.RIGHT.ToString());
            DoStackedChartDemo(ElementPosition.TOP, ElementPosition.RIGHT);

            Console.WriteLine("==========" + ElementPosition.BOTTOM.ToString() + "-" + ElementPosition.LEFT.ToString());
            DoStackedChartDemo(ElementPosition.BOTTOM, ElementPosition.LEFT);

            Console.WriteLine("=========="+ ElementPosition.BOTTOM.ToString() + "-" + ElementPosition.RIGHT.ToString());
            DoStackedChartDemo(ElementPosition.BOTTOM, ElementPosition.RIGHT);

            Console.WriteLine("========== Duration chart");

            DoDurationChartDemo(ElementPosition.BOTTOM, ElementPosition.LEFT);
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
            DurationChart dChart = new DurationChart();
            dChart.Title = "Demo of Duration Chart";
            dChart.SubTitle = "Orientation: x-Axis: " + xAxisPositioning.ToString() + ", y-Axis: " + yAxisPositioning.ToString();
            dChart.OutputFile = @"C:\New Folder\aDemo-duration-chart-" + xAxisPositioning.ToString() + "-" + yAxisPositioning.ToString() + ".png";

            var etlProcess1 = new DurationDataSeries("ETL Process 1", Color.BlueViolet);
            etlProcess1.AddDataPoint(new Duration(new DateTime(2020, 9, 20, 0, 0, 0), new DateTime(2020, 9, 20, 1, 10, 0)));
            etlProcess1.AddDataPoint(new Duration(new DateTime(2020, 9, 21, 17, 0, 0), new DateTime(2020, 9, 21, 17, 40, 0)));
            dChart.AddDataSeries(etlProcess1);

            var etlProcess2 = new DurationDataSeries("ETL Process 2", Color.BlueViolet);
            etlProcess2.AddDataPoint(new Duration(new DateTime(2020, 9, 19, 21, 0, 0), new DateTime(2020, 9, 20, 3, 30, 0)));
            dChart.AddDataSeries(etlProcess2);

            var xAxis = new DateAndTimeScaleAxis(20, AxisLabelFormat.DATETIME_DDMMYYYY1_HHMM24);
            xAxis.LabelHorizontalPosition = AxisLabelHorizontalPosition.CENTER;
            dChart.SetXAxis(xAxisPositioning, xAxis, AxisWidth.FIT_TO_INCREMENT, 90);            

            var yAxis = new LabelAxis(50);
            dChart.SetY1Axis(yAxisPositioning, yAxis, AxisWidth.FIT_TO_INCREMENT);

            dChart.GenerateChart();

        }

    }
}
