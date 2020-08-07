using System;
using System.Collections.Generic;
using System.Drawing;

namespace MyCharter.ChartElements.DataSeries
{
    public class DataSeries<T1, T2>
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public List<DataPoint<T1,T2>> DataPoints { get; set; } = new List<DataPoint<T1, T2>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        public DataSeries(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        /// <summary>
        /// Add a Data Point to the DataSeries.
        /// </summary>
        /// <param name="axisEntry1"></param>
        /// <param name="axisEntry2"></param>
        public void AddDataPoint(T1 axisEntry1, T2 axisEntry2)
        {
            DataPoints.Add(new DataPoint<T1, T2>(axisEntry1, axisEntry2));
        }
    }
}
