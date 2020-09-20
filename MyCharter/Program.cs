using MyCharter.ChartElements.Axis;
using MyCharter.ChartElements.DataSeries;
using MyCharter.Charts;
using MyCharter.Util;
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
            StackedVerticalBarChart<DateTime, int> svChart = new StackedVerticalBarChart<DateTime,int>();
            svChart.Title = "Demo of Stacked Chart";
            svChart.SubTitle = "Orientation: x-Axis: " + xAxisPositioning.ToString() + ", y-Axis: " + yAxisPositioning.ToString();
            svChart.OutputFile = @"C:\New Folder\aDemo-stacked-vertical-chart-"+ xAxisPositioning.ToString()+ "-" + yAxisPositioning.ToString() + ".png";
            
            var xAxis = new DateScaleAxis(new DateTime(2020, 5, 15), new DateTime(2020, 6, 15), 1, 0, 30, AxisLabelFormat.DATE_DDMM1);
            xAxis.MajorGridLine = true;
            xAxis.LabelHorizontalPosition = AxisLabelHorizontalPosition.CENTER;
            svChart.SetX1Axis(xAxisPositioning, xAxis, AxisWidth.FIT_TO_INCREMENT);

            var yAxis = new NumberScaleAxis(minimumValue:0, maximumValue:5000, majorIncrement:1000, 
                minorIncrement:250, pixelsPerIncrement:10, 
                AxisLabelFormat.NUMBER_THOU_SEP_COMMA);
            yAxis.MajorGridLine = true;
            svChart.SetYAxis(yAxisPositioning, yAxis, AxisWidth.FIT_TO_INCREMENT);

            var volunteersDS = new DataSeries<DateTime, int>("Volunteers", Color.Orange, AxisLabelFormat.NUMBER_THOU_SEP_COMMA, LegendDisplayType.SQUARE);
            volunteersDS.AddDataPoint(new DateTime(2020, 5, 15), 2700);
            volunteersDS.AddDataPoint(new DateTime(2020, 5, 16), 1500);
            volunteersDS.AddDataPoint(new DateTime(2020, 5, 17), 1320);
            volunteersDS.AddDataPoint(new DateTime(2020, 5, 19), 100);
            svChart.AddDataSeries(volunteersDS);

            var centrelinkDS = new DataSeries<DateTime, int>("Centrelink", Color.Red, AxisLabelFormat.NUMBER_THOU_SEP_COMMA, LegendDisplayType.SQUARE);
            centrelinkDS.AddDataPoint(new DateTime(2020, 5, 15), 1200);
            centrelinkDS.AddDataPoint(new DateTime(2020, 5, 16), 500);
            centrelinkDS.AddDataPoint(new DateTime(2020, 5, 18), 550);
            svChart.AddDataSeries(centrelinkDS);

            var socialDS = new DataSeries<DateTime, int>("Social", Color.DeepSkyBlue, AxisLabelFormat.NUMBER_THOU_SEP_COMMA, LegendDisplayType.LINE);
            socialDS.AddDataPoint(new DateTime(2020, 5, 16), 1271);
            svChart.AddDataSeries(socialDS);

            svChart.GenerateChart();
        }

        private static void DoDurationChartDemo(ElementPosition xAxisPositioning, ElementPosition yAxisPositioning)
        {
            DurationChart<DateTime, DateTime> svChart = new DurationChart<DateTime, DateTime>();
            svChart.Title = "Demo of Duration Chart";
            svChart.SubTitle = "Orientation: x-Axis: " + xAxisPositioning.ToString() + ", y-Axis: " + yAxisPositioning.ToString();
            svChart.OutputFile = @"C:\New Folder\aDemo-duration-chart-" + xAxisPositioning.ToString() + "-" + yAxisPositioning.ToString() + ".png";

            var xAxis = new DateAndTimeScaleAxis(
                minimumValue: new DateTime(2020,9,20, 0,0,0), 
                maximumValue: new DateTime(2020,9,22, 16,00,00), 
                majorIncrementMinutes: 60, 
                minorIncrementMinutes: 10, 
                pixelsPerIncrement: 10, 
                labelFormat: AxisLabelFormat.DATETIME_DDMM1_HHMM24);
            xAxis.LabelHorizontalPosition = AxisLabelHorizontalPosition.CENTER;
            svChart.SetX1Axis(xAxisPositioning, xAxis, AxisWidth.FIT_TO_INCREMENT, 90);
            Console.WriteLine($"**** {xAxis.GetMaxLabelDimensions()}");
            xAxis.DebugOutput_ListScale();

            var yAxis = new DateScaleAxis(new DateTime(2020,01,01), new DateTime(2020, 01, 05), 5, 1, pixelsPerIncrement: 10,
                AxisLabelFormat.DATE_DDMM1);
            svChart.SetYAxis(yAxisPositioning, yAxis, AxisWidth.FIT_TO_INCREMENT);


            var timeTracker = new DataSeries<DateTime, DateTime>("TimeTracker", Color.BlueViolet, AxisLabelFormat.DATE_DDMM1, LegendDisplayType.SQUARE);
            timeTracker.AddDataPoint(new DateTime(2020, 9, 5, 9, 30, 00), new DateTime(2020, 9, 5, 9, 40, 00));
            svChart.AddDataSeries(timeTracker);

            svChart.GenerateChart();

        }

    }
}
