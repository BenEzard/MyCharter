using System;
using System.Collections.Generic;
using System.Text;

namespace MyCharter.Util
{
    /// <summary>
    /// Converts a data type to another data type.
    /// </summary>
    public static class CastMethods
    {
        /// <summary>
        /// Converts input to Type of default value or given as typeparam T
        /// </summary>
        /// <typeparam name="T">typeparam is the type in which value will be returned, it could be any type eg. int, string, bool, decimal etc.</typeparam>
        /// <param name="input">Input that need to be converted to specified type</param>
        /// <param name="defaultValue">defaultValue will be returned in case of value is null or any exception occures</param>
        /// <returns>Input is converted in Type of default value or given as typeparam T and returned</returns>
        public static T To<T>(object input, T defaultValue)
        {
            var result = defaultValue;
            try
            {
                if (input == null || input == DBNull.Value) return result;
                if (typeof(T).IsEnum)
                {
                    result = (T)Enum.ToObject(typeof(T), To(input, Convert.ToInt32(defaultValue)));
                }
                else
                {
                    result = (T)Convert.ChangeType(input, typeof(T));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        /// <summary>
        /// Converts input to Type of typeparam T
        /// </summary>
        /// <typeparam name="T">typeparam is the type in which value will be returned, it could be any type eg. int, string, bool, decimal etc.</typeparam>
        /// <param name="input">Input that need to be converted to specified type</param>
        /// <returns>Input is converted in Type of default value or given as typeparam T and returned</returns>
        public static T To<T>(object input)
        {
            return To(input, default(T));
        }

        public static DateTime StringToDateTime(string dateTimeString)
        {
            DateTime rValue = default;
            
            if (dateTimeString.Length == "2020-09-28 05:00:08.000".Length)
            {
                int year = Int32.Parse(dateTimeString.Substring(0, 4));
                int month = Int32.Parse(dateTimeString.Substring(5, 2));
                int day = Int32.Parse(dateTimeString.Substring(8, 2));
                int hour = Int32.Parse(dateTimeString.Substring(11, 2));
                int minute = Int32.Parse(dateTimeString.Substring(14, 2));
                int second = Int32.Parse(dateTimeString.Substring(17, 2));
                rValue = new DateTime(year, month, day, hour, minute, second);
            }

            return rValue;
        }
    }
}