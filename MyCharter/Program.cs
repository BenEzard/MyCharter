using MyCharter.ChartElements.Axis;
using System;

namespace MyCharter
{
    class Program
    {
        static void Main(string[] args)
        {
            BarChart bc = new BarChart();
            bc.Title = "This is a demo";
            bc.SubTitle = "And this is the subtitle";
            bc.OutputFile = @"C:\New Folder\image2.png";

            bc.SetAxis(Axis.X, AxisLabelPosition.TOP, new TimeScaleAxis(new DateTime(1900, 1, 1, 18, 0, 0), new DateTime(1900, 1, 2, 7, 0, 0), 60, 10, 10));

            var ds = new DataSeriesAxis();
            ds.AddDataSeries(new DurationDataSeries(new DateTime(2020, 3, 23, 22, 0, 0), new DateTime(2020, 3, 24, 0, 40, 0), "H2H_BusinessIntelligence"));
            ds.AddDataSeries(new DurationDataSeries(new DateTime(2020, 3, 24, 1, 30, 0), new DateTime(2020, 3, 24, 3, 10, 0), "QMaster_BusinessIntelligence"));
            ds.AddDataSeries(new DurationDataSeries(new DateTime(2020, 3, 24, 2, 0, 0), new DateTime(2020, 3, 24, 3, 0, 0), "CorporateDataServices_BusinessIntelligence"));
            ds.AddDataSeries(new DurationDataSeries(new DateTime(2020, 3, 24, 3, 0, 0), new DateTime(2020, 3, 24, 4, 0, 0), "CHCR_BusinessIntelligence"));
            ds.AddDataSeries(new DurationDataSeries(new DateTime(2020, 3, 24, 4, 0, 0), new DateTime(2020, 3, 24, 4, 20, 0), "SACHA_BusinessIntelligence"));
            ds.AddDataSeries(new DurationDataSeries(new DateTime(2020, 3, 24, 4, 20, 0), new DateTime(2020, 3, 24, 5, 0, 0), "UnconsentedBonds_BusinessIntelligence"));
            ds.AddDataSeries(new DurationDataSeries(new DateTime(2020, 3, 24, 5, 0, 0), new DateTime(2020, 3, 24, 5, 20, 0), "AHATS_BusinessIntelligence"));
            ds.AddDataSeries(new DurationDataSeries(new DateTime(2020, 3, 24, 3, 30, 0), new DateTime(2020, 3, 24, 5, 20, 0), "ECIS_BusinessIntelligence"));
            ds.AddDataSeries(new DurationDataSeries(new DateTime(2020, 3, 24, 5, 0, 0), new DateTime(2020, 3, 24, 5, 50, 0), "PAWS_BusinessIntelligence"));
            bc.SetAxis(Axis.Y, AxisLabelPosition.LEFT, ds);
            bc.GenerateChart();


            var x = bc.GetAxis(Axis.X);
            var y = (DataSeriesAxis)bc.GetAxis(Axis.Y);
            Console.WriteLine($"The number of items on the X axis are {x.TotalIncrementCount()}");
            Console.WriteLine($"The number of items on the Y axis are {y.TotalIncrementCount()}");

        }
    }
}
