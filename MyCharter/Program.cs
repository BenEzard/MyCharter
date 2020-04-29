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
            barChart.Title = "PRODUCTION load (composite of 24/04 to 26/04)";
//            barChart.SubTitle = "And this is the subtitle";
            barChart.OutputFile = @"C:\New Folder\bar-chart.png";
            var xAxis = new TimeScaleAxis(new DateTime(1900, 1, 1, 19, 0, 0), new DateTime(1900, 1, 2, 7, 0, 0), 60, 10, 15);
            xAxis.MinorTickPen = new Pen(Brushes.LightGray, 1);
            xAxis.LabelHorizontalPosition = AxisLabelHorizontalPosition.CENTER;
            barChart.SetAxis(Axis.X, AxisLabelPosition.TOP, xAxis);
            //xAxis.DebugOutput_ListScale();

            var yAxis = new DataSeriesAxis();
            yAxis.LabelPadding = 10;
            yAxis.AxisPadding = 10;
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 19, 0, 0), new DateTime(2020, 3, 24, 19, 18, 0), "Global_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 0, 15, 0), new DateTime(2020, 3, 24, 6, 47, 0), "H2H_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 1, 30, 0), new DateTime(2020, 3, 24, 2, 50, 0), "QMaster_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 2, 0, 0), new DateTime(2020, 3, 24, 2, 38, 0), "MICA_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 3, 0, 0), new DateTime(2020, 3, 24, 3, 45, 0), "CHCR_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 3, 30, 0), new DateTime(2020, 3, 24, 4, 45, 0), "ECIS_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 3, 30, 0), new DateTime(2020, 3, 24, 3, 40, 0), "TurnaroundTimes_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 4, 0, 0), new DateTime(2020, 3, 24, 4, 40, 0), "SACHA_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 4, 0, 0), new DateTime(2020, 3, 24, 6, 5, 0), "ABS_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 4, 15, 0), new DateTime(2020, 3, 24, 5, 15, 0), "UnconsentedBonds_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 4, 30, 0), new DateTime(2020, 3, 24, 5, 6, 0), "etl_rpt_all_004_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 5, 0, 0), new DateTime(2020, 3, 24, 5, 40, 0), "PAWS_BusinessIntelligence"));



            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 5, 0, 0), new DateTime(2020, 3, 24, 5, 12, 0), "AHATS_BusinessIntelligence"));
            yAxis.AddDataSeries(new DurationDataSeriesEntry(new DateTime(2020, 3, 24, 6, 0, 0), new DateTime(2020, 3, 24, 6, 3, 0), "StreetConnect_BusinessIntelligence"));
            barChart.SetAxis(Axis.Y, AxisLabelPosition.LEFT, yAxis);
            barChart.GenerateChart();
        }
    }
}
