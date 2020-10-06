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
            bool doStackedVertical = false;
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
            DurationChart dChart = new DurationChart();
            dChart.Title = "Demo of Duration Chart";
            dChart.SubTitle = "Orientation: x-Axis: " + xAxisPositioning.ToString() + ", y-Axis: " + yAxisPositioning.ToString();
            dChart.OutputFile = @"C:\New Folder\aDemo-duration-chart-" + xAxisPositioning.ToString() + "-" + yAxisPositioning.ToString() + ".png";

            //var etlProcess1 = new DurationDataSeries("ETL Process 1", Color.BlueViolet);
            //etlProcess1.AddDataPoint(new Duration(new DateTime(2020, 9, 20, 0, 0, 0), new DateTime(2020, 9, 20, 1, 15, 0)));
            //etlProcess1.AddDataPoint(new Duration(new DateTime(2020, 9, 21, 17, 0, 0), new DateTime(2020, 9, 21, 17, 40, 0)));
            //dChart.AddDataSeries(etlProcess1);

            //var etlProcess2 = new DurationDataSeries("ETL Process 2", Color.GreenYellow);
            //etlProcess2.AddDataPoint(new Duration(new DateTime(2020, 9, 19, 21, 0, 0), new DateTime(2020, 9, 20, 3, 30, 0)));
            //dChart.AddDataSeries(etlProcess2);

            var gis = new DurationDataSeries("GIS", Color.BlueViolet);
            gis.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 6, 15, 0), new DateTime(2020, 10, 5, 6, 29, 0)));
            gis.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 6, 15, 0), new DateTime(2020, 10, 4, 6, 29, 0)));
            dChart.AddDataSeries(gis);

            var streetConnect = new DurationDataSeries("StreetConnect", Color.BlueViolet);
            streetConnect.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 6, 0, 0), new DateTime(2020, 10, 5, 6, 1, 0)));
            streetConnect.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 6, 0, 0), new DateTime(2020, 10, 4, 6, 5, 0)));
            dChart.AddDataSeries(streetConnect);

            var dashboards = new DurationDataSeries("Dashboards", Color.BlueViolet);
            dashboards.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 5, 30, 0), new DateTime(2020, 10, 5, 5, 33, 0)));
            dashboards.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 5, 30, 0), new DateTime(2020, 10, 4, 5, 42, 0)));
            dChart.AddDataSeries(dashboards);

            var prop007 = new DurationDataSeries("rpt_prop_007", Color.BlueViolet);
            prop007.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 5, 0, 0), new DateTime(2020, 10, 5, 5, 4, 0)));
            prop007.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 5, 1, 0), new DateTime(2020, 10, 4, 5, 3, 0)));
            dChart.AddDataSeries(prop007);

            var paws = new DurationDataSeries("PAWS", Color.BlueViolet);
            paws.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 5, 0, 0), new DateTime(2020, 10, 5, 5, 21, 0)));
            paws.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 5, 12, 0), new DateTime(2020, 10, 4, 6, 3, 0)));
            dChart.AddDataSeries(paws);

            var assetCondition = new DurationDataSeries("AssetCondition", Color.BlueViolet);
            assetCondition.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 5, 8, 0), new DateTime(2020, 10, 4, 8, 30, 0)));
            dChart.AddDataSeries(assetCondition);

            var ahats = new DurationDataSeries("AHATS", Color.BlueViolet);
            ahats.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 5, 0, 0), new DateTime(2020, 10, 5, 5, 8, 0)));
            ahats.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 5, 0, 0), new DateTime(2020, 10, 4, 5, 1, 0)));
            dChart.AddDataSeries(ahats);

            var all004 = new DurationDataSeries("rpt_all_004", Color.BlueViolet);
            all004.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 4, 35, 0), new DateTime(2020, 10, 4, 5, 11, 0)));
            dChart.AddDataSeries(all004);

            var unconsentedBonds = new DurationDataSeries("Unconsented Bonds", Color.BlueViolet);
            unconsentedBonds.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 4, 15, 0), new DateTime(2020, 10, 5, 4, 49, 0)));
            unconsentedBonds.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 4, 15, 0), new DateTime(2020, 10, 4, 5, 36, 0)));
            dChart.AddDataSeries(unconsentedBonds);

            var sacha = new DurationDataSeries("SACHA", Color.BlueViolet);
            sacha.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 4, 0, 0), new DateTime(2020, 10, 5, 4, 28, 0)));
            sacha.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 4, 0, 0), new DateTime(2020, 10, 4, 4, 59, 0)));
            dChart.AddDataSeries(sacha);

            var abs = new DurationDataSeries("ABS", Color.BlueViolet);
            abs.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 4, 0, 0), new DateTime(2020, 10, 5, 5, 20, 0)));
            abs.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 4, 0, 0), new DateTime(2020, 10, 4, 6, 51, 0)));
            dChart.AddDataSeries(abs);

            var tt = new DurationDataSeries("TimeTracker", Color.BlueViolet);
            tt.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 3, 30, 0), new DateTime(2020, 10, 5, 4, 15, 0)));
            tt.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 3, 30, 0), new DateTime(2020, 10, 4, 4, 9, 0)));
            dChart.AddDataSeries(tt);

            var ecis = new DurationDataSeries("ECIS", Color.BlueViolet);
            ecis.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 3, 30, 0), new DateTime(2020, 10, 5, 5, 31, 0)));
            ecis.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 3, 30, 0), new DateTime(2020, 10, 4, 6, 57, 0)));
            dChart.AddDataSeries(ecis);

            var chcr = new DurationDataSeries("CHCR", Color.BlueViolet);
            chcr.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 3, 0, 0), new DateTime(2020, 10, 5, 4, 21, 0)));
            chcr.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 3, 0, 0), new DateTime(2020, 10, 4, 4, 59, 0)));
            dChart.AddDataSeries(chcr);

            var mica = new DurationDataSeries("MICA", Color.BlueViolet);
            mica.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 2, 0, 0), new DateTime(2020, 10, 5, 3, 15, 0)));
            mica.AddDataPoint(new Duration(new DateTime(2020, 10, 4, 2, 0, 0), new DateTime(2020, 10, 4, 4, 34, 0)));
            dChart.AddDataSeries(mica);

            var qMaster = new DurationDataSeries("QMaster", Color.BlueViolet);
            qMaster.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 1, 30, 0), new DateTime(2020, 10, 5, 3, 54, 0)));
            dChart.AddDataSeries(qMaster);

            var vmia = new DurationDataSeries("VMIA", Color.BlueViolet);
            vmia.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 1, 0, 0), new DateTime(2020, 10, 5, 1, 13, 0)));
            dChart.AddDataSeries(vmia);

            var prConnect = new DurationDataSeries("PR Connect", Color.BlueViolet);
            prConnect.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 1, 0, 0), new DateTime(2020, 10, 5, 4, 13, 0)));
            dChart.AddDataSeries(prConnect);

            var masterpiece = new DurationDataSeries("Masterpiece", Color.BlueViolet);
            masterpiece.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 1, 0, 0), new DateTime(2020, 10, 5, 1, 2, 0)));
            dChart.AddDataSeries(masterpiece);

            var debt015 = new DurationDataSeries("rpt_debt_015", Color.BlueViolet);
            debt015.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 1, 0, 0), new DateTime(2020, 10, 5, 1, 23, 0)));
            dChart.AddDataSeries(debt015);

            var h2h = new DurationDataSeries("h2H", Color.BlueViolet);
            h2h.AddDataPoint(new Duration(new DateTime(2020, 10, 5, 0, 15, 0), new DateTime(2020, 10, 5, 0, 24, 0)));
            dChart.AddDataSeries(h2h);

            var xAxis = new DateAndTimeScaleAxis(60, 10, 10, AxisLabelFormat.DATETIME_DDMMYYYY1_HHMM24);
            xAxis.MajorGridLine = true;
            xAxis.LabelHorizontalPosition = AxisLabelHorizontalPosition.CENTER;
            dChart.SetXAxis(xAxisPositioning, xAxis, AxisWidth.FIT_TO_INCREMENT, 90);
            
            var yAxis = new LabelAxis(30, dChart.GetDataSeriesNames());
            dChart.SetY1Axis(yAxisPositioning, yAxis, AxisWidth.FIT_TO_INCREMENT);
            
            dChart.IsLegendVisible = false;
            
            dChart.GenerateChart();

        }

    }
}
