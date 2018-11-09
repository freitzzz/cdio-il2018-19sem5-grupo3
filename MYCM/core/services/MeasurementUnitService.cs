using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace core.services
{
    /// <summary>
    /// Service responsible for measurement unit conversion.
    /// </summary>
    public sealed class MeasurementUnitService
    {
        /// <summary>
        /// Constant representing error when the given unit has not matched any measurement unit.
        /// </summary>
        private const string ERROR_UNIT_NOT_FOUND = "Unable to convert to the specified unit";

        /// <summary>
        /// Converts the dimension's measurement value in milimetres to the equivalent in the given units.
        /// </summary>
        /// <param name="value">Dimension value.</param>
        /// <param name="newUnit">Dimension's measurement unit.</param>
        /// <returns>The dimension's converted measurement unit value.</returns>
        public static double convertToUnit(double value, string newUnit)
        {
            double conversionValue;

            IDictionary<string, double> unitMap = loadUnitMap();

            if (newUnit == null)
            {
                return value * unitMap.Values.Min();
            }
            if (!unitMap.TryGetValue(newUnit, out conversionValue))
            {
                throw new ArgumentException(ERROR_UNIT_NOT_FOUND);   //throw exception if value does not exist
            }

            return value / conversionValue;
        }

        /// <summary>
        /// Converts the dimension's measurement value in the given unit to the equivalent in milimetres.
        /// </summary>
        /// <param name="value">Dimension value.</param>
        /// <param name="oldUnit">Dimension's measurement unit.</param>
        /// <returns></returns>
        public static double convertFromUnit(double value, string oldUnit)
        {
            double conversionValue;

            IDictionary<string, double> unitMap = loadUnitMap();

            if (oldUnit == null)
            {
                return value * unitMap.Values.Min();
            }

            if (!unitMap.TryGetValue(oldUnit, out conversionValue))
            {
                throw new ArgumentException(ERROR_UNIT_NOT_FOUND);   //throw exception if value does not exist
            }

            return value * conversionValue;
        }

        /// <summary>
        /// Retrieves the string representation of the minimum measurement unit.
        /// </summary>
        /// <returns>String representation of the minimum measurement unit.</returns>
        public static string getMinimumUnit()
        {
            IDictionary<string, double> unitMap = loadUnitMap();

            string result = "";

            double min = double.MaxValue;

            foreach (string key in unitMap.Keys)
            {
                if (unitMap[key] < min)
                {
                    min = unitMap[key];
                    result = key;
                }
            }

            return result;
        }


        /// <summary>
        /// Loads the measurements from the Json convertion file.
        /// </summary>
        /// <returns>Dictionary in which the keys are the measurement units and the values are the milimetre-to-unit conversion rate.</returns>
        private static IDictionary<string, double> loadUnitMap()
        {
            Dictionary<string, double> unitDictionary = null;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(@"../core/measurement_units.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                unitDictionary = (Dictionary<string, double>)serializer.Deserialize(file, typeof(Dictionary<string, double>));
            }

            return unitDictionary;
        }


    }
}