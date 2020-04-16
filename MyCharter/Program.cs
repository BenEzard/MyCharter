using MyCharter.ChartElements.Axis;
using System;
using System.Drawing;

namespace MyCharter
{
    class Program
    {
        static void Main(string[] args)
        {
            BarChart barChart = new BarChart();
            barChart.Title = "This is a demo";
            barChart.SubTitle = "And this is the subtitle";
            barChart.OutputFile = @"C:\New Folder\image2.png";
            var xAxis = new TimeScaleAxis(new DateTime(1900, 1, 1, 18, 0, 0), new DateTime(1900, 1, 2, 7, 0, 0), 60, 10, 15);
            xAxis.MinorTickPen = new Pen(Brushes.LightGray, 1);
            xAxis.LabelHorizontalPosition = AxisLabelHorizontalPosition.CENTER;
            barChart.SetAxis(Axis.X, AxisLabelPosition.TOP, xAxis);
            //xAxis.DebugOutput_ListScale();

            var yAxis = new DataSeriesAxis();
            yAxis.LabelPadding = 10;
            yAxis.AxisPadding = 10;
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 23, 22, 0, 0), new DateTime(2020, 3, 24, 0, 40, 0), "H2H_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 1, 30, 0), new DateTime(2020, 3, 24, 3, 10, 0), "QMaster_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 2, 0, 0), new DateTime(2020, 3, 24, 3, 0, 0), "CorporateDataServices_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 3, 0, 0), new DateTime(2020, 3, 24, 4, 0, 0), "CHCR_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 4, 0, 0), new DateTime(2020, 3, 24, 4, 20, 0), "SACHA_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 4, 20, 0), new DateTime(2020, 3, 24, 5, 0, 0), "UnconsentedBonds_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 5, 0, 0), new DateTime(2020, 3, 24, 5, 20, 0), "AHATS_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 3, 30, 0), new DateTime(2020, 3, 24, 5, 20, 0), "ECIS_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 5, 0, 0), new DateTime(2020, 3, 24, 5, 50, 0), "PAWS_BusinessIntelligence"));
            barChart.SetAxis(Axis.Y, AxisLabelPosition.LEFT, yAxis);
            barChart.GenerateChart();

/*            Console.WriteLine($"Seeking axis entry for 20:00 = { xAxis.GetAxisEntry(new DateTime(1900, 1, 1, 20, 0, 0).TimeOfDay)}");

            Console.WriteLine($"Seeking axis entry for UnconsentedBonds_BusinessIntelligence = { yAxis.GetAxisEntry("UnconsentedBonds_BusinessIntelligence")}");
            Console.WriteLine($"label padding is { yAxis.LabelPadding}");
            Console.WriteLine($"The number of items on the X axis are {xAxis.TotalIncrementCount()}");
            Console.WriteLine($"The number of items on the Y axis are {yAxis.TotalIncrementCount()}");*/

        }
    }
}
