﻿using MyCharter.ChartElements.Axis;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MyCharter
{
    public abstract class AbstractScaleAxis<TAxisDataType> : AbstractChartAxis<TAxisDataType>
    {

        public AxisLabelFormat LabelFormat;

        /// <summary>
        /// The minimum value to be displayed on the axis.
        /// </summary>
        public TAxisDataType MinimumValue { get; set; } = default;

        /// <summary>
        /// The maximum value to be displayed on the axis.
        /// </summary>
        public TAxisDataType MaximumValue { get; set; } = default;

        public int MajorIncrement { get; set; }

        public int MinorIncrement { get; set; }

        //public List<AxisContraction<TAxisDataType>> AxisContractions { get; } = new List<AxisContraction<TAxisDataType>>();

        public AbstractScaleAxis(AxisFormat format, int pixelsPerIncrement) : base(format)
        {
            Format = format;
            PixelsPerIncrement = pixelsPerIncrement;
        }

        /// <summary>
        /// Instantiate an Axis.
        /// </summary>
        /// <param name="format">The AxisFormat of this axis.</param>
        /// <param name="majorIncrement">How often a major increment (tick) occurs.</param>
        /// <param name="minorIncrement">How often a minor increment (tick) occurs.</param>
        /// <param name="pixelsPerIncrement">Number of pixels per increment</param>
        public AbstractScaleAxis(AxisFormat format, int majorIncrement, int minorIncrement,
            int pixelsPerIncrement) : base(format)
        {
            Format = format;
            MajorIncrement = majorIncrement;
            MinorIncrement = minorIncrement;
            PixelsPerIncrement = pixelsPerIncrement;
        }

        /// <summary>
        /// Instantiate an Axis.
        /// </summary>
        /// <param name="format">The AxisFormat of this axis.</param>
        /// <param name="minimumValue">The minimum value displayed on this axis.</param>
        /// <param name="maximumValue">The maximum value displayed on this axis.</param>
        /// <param name="majorIncrement">How often a major increment (tick) occurs.</param>
        /// <param name="minorIncrement">How often a minor increment (tick) occurs.</param>
        /// <param name="pixelsPerIncrement">Number of pixels per increment</param>
        public AbstractScaleAxis(AxisFormat format, TAxisDataType minimumValue, TAxisDataType maximumValue, int majorIncrement, int minorIncrement,
            int pixelsPerIncrement) : base(format)
        {
            Format = format;
            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
            MajorIncrement = majorIncrement;
            MinorIncrement = minorIncrement;
            PixelsPerIncrement = pixelsPerIncrement;

            /*string errorMessage;
            if (AreAxisValuesValid(out errorMessage) == false)
            {
                throw new ArgumentException(errorMessage);
            }*/
        }

        /// <summary>
        /// Check to see if the minimum, maximum and increment values for the Axis are valid.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        //protected abstract bool AreAxisValuesValid(out string errorMessage);

        /// <summary>
        /// Returns the number of major increments on the axis.
        /// </summary>
        /// <returns></returns>
        public int MajorTickCount()
        {
            int rValue = 0;
            foreach (AxisEntry<TAxisDataType> o in AxisEntries)
            {
                if (o.IsMajorTick)
                {
                    ++rValue;
                }
            }
            return rValue;
        }

        /// Returns the number of minor increments on the axis.
        /// </summary>
        /// <returns></returns>
        public int MinorTickCount()
        {
            int rValue = 0;
            foreach (AxisEntry<TAxisDataType> o in AxisEntries)
            {
                if (o.IsMajorTick == false)
                {
                    ++rValue;
                }
            }
            return rValue;
        }

        /// <summary>
        /// Add an AxisContraction.
        /// This should be made up of fully-formed AxisEntries.
        /// </summary>
        /// <param name="contraction"></param>
        /*public void AddAxisContraction(AxisContraction<TAxisDataType> contraction)
        {
            AxisContractions.Add(contraction);
        }*/
    }
}
