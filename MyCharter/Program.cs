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
            /*Console.WriteLine("==========" + ElementPosition.TOP.ToString() + "-" + ElementPosition.LEFT.ToString());
            DoStackedChartDemo(ElementPosition.TOP, ElementPosition.LEFT);

            Console.WriteLine("==========" + ElementPosition.TOP.ToString() + "-" + ElementPosition.RIGHT.ToString());
            DoStackedChartDemo(ElementPosition.TOP, ElementPosition.RIGHT);*/

            Console.WriteLine("==========" + ElementPosition.BOTTOM.ToString() + "-" + ElementPosition.LEFT.ToString());
            DoStackedChartDemo(ElementPosition.BOTTOM, ElementPosition.LEFT);

            /*Console.WriteLine("=========="+ ElementPosition.BOTTOM.ToString() + "-" + ElementPosition.RIGHT.ToString());
            DoStackedChartDemo(ElementPosition.BOTTOM, ElementPosition.RIGHT);*/
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

            var yAxis = new NumberScaleAxis(0, 40000, 10000, 2500, 10, AxisLabelFormat.NUMBER_THOU_SEP_COMMA);
            yAxis.MajorGridLine = true;
            svChart.SetYAxis(yAxisPositioning, yAxis, AxisWidth.FIT_TO_INCREMENT);

            var volunteersDS = new DataSeries<DateTime, int>("Volunteers", Color.Orange);
            volunteersDS.AddDataPoint(new DateTime(2020, 5, 15), 27000);
            volunteersDS.AddDataPoint(new DateTime(2020, 5, 16), 15000);
            volunteersDS.AddDataPoint(new DateTime(2020, 5, 17), 13200);
            volunteersDS.AddDataPoint(new DateTime(2020, 5, 19), 1000);
            svChart.AddDataSeries(volunteersDS);

            var centrelinkDS = new DataSeries<DateTime, int>("Centrelink", Color.Red);
            centrelinkDS.AddDataPoint(new DateTime(2020, 5, 15), 12000);
            centrelinkDS.AddDataPoint(new DateTime(2020, 5, 16), 500);
            centrelinkDS.AddDataPoint(new DateTime(2020, 5, 18), 5500);
            svChart.AddDataSeries(centrelinkDS);

            svChart.GenerateChart();
        }

    }
}
