using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace core.domain
{
    /// <summary>
    /// Class that represents the algorithm that checks if complement occupies a certain percentage of the parent product
    /// </summary>
    public class WidthPercentageAlgorithm : Algorithm
    {
        /// <summary>
        /// Name of minimum percentage input
        /// </summary>
        private const string MINIMUM_PERCENTAGE_INPUT_NAME = "Minimum Percentage";
        /// <summary>
        /// Name of maximum percentage input
        /// </summary>
        private const string MAXIMUM_PERCENTAGE_INPUT_NAME = "Maximum Percentage";
        /// <summary>
        /// Invalid input message
        /// </summary>
        private const string INVALID_INPUT = "Input is invalid!";
        /// <summary>
        /// Input value invalid message
        /// </summary>
        private const string INPUT_OUTSIDE_RANGE = "Input is not within the established range!";
        /// <summary>
        /// Minimum percentage of occupation
        /// </summary>
        private double minPercentage;
        /// <summary>
        /// Maximum percentage of occupation
        /// </summary>
        private double maxPercentage;
        /// <summary>
        /// Empty constructor
        /// </summary>
        public WidthPercentageAlgorithm()
        {
        }
        /// <summary>
        /// Returns a list of the required inputs to apply the algorithm
        /// </summary>
        /// <returns>list of the required inputs</returns>
        public List<Input> getRequiredInputs()
        {
            List<Input> inputs = new List<Input>();
            Input input1 = new Input(MINIMUM_PERCENTAGE_INPUT_NAME);
            Input input2 = new Input(MAXIMUM_PERCENTAGE_INPUT_NAME);
            inputs.Add(input1);
            inputs.Add(input2);
            return inputs;
        }
        /// <summary>
        /// Sets the input values for the algorithm to be applied
        /// </summary>
        /// <param name="inputs">list of inputs with values</param>
        /// <returns>true if the values were successfully set, throws ArgumentException if any value was not valid</returns>
        public bool setInputValues(List<Input> inputs)
        {
            if (inputs == null || inputs.Count == 0 || inputs.Count != 2)
            {
                throw new ArgumentException(INVALID_INPUT);
            }
            isWithinDataRange(inputs);
            foreach (Input input in inputs)
            {
                switch (input.name)
                {
                    case MINIMUM_PERCENTAGE_INPUT_NAME:
                        minPercentage = Convert.ToDouble(input.value, CultureInfo.InvariantCulture);
                        break;
                    case MAXIMUM_PERCENTAGE_INPUT_NAME:
                        maxPercentage = Convert.ToDouble(input.value, CultureInfo.InvariantCulture);
                        break;
                }
            }
            return true;
        }
        /// <summary>
        /// Checks if input values are within the allowed range
        /// </summary>
        /// <param name="inputs">list of inputs with values to check</param>
        /// <returns>true if values are within allowed range, throws ArgumentException if any value was not within the allowed range, throws FormatException if any input value is not a double</returns>
        public bool isWithinDataRange(List<Input> inputs)
        {
            if (inputs == null || inputs.Count == 0 || inputs.Count != 2)
            {
                throw new ArgumentException(INVALID_INPUT);
            }
            double minPercentage = -1;
            double maxPercentage = -1;
            foreach (Input input in inputs)
            {
                if (String.IsNullOrEmpty(input.name))
                {
                    throw new ArgumentException(INVALID_INPUT);
                }
                switch (input.name)
                {
                    case MINIMUM_PERCENTAGE_INPUT_NAME:
                        minPercentage = Convert.ToDouble(input.value, CultureInfo.InvariantCulture);
                        break;
                    case MAXIMUM_PERCENTAGE_INPUT_NAME:
                        maxPercentage = Convert.ToDouble(input.value, CultureInfo.InvariantCulture);
                        break;
                    default:
                        throw new ArgumentException(INVALID_INPUT);
                }
            }
            return minPercentage >= 0 && minPercentage <= 1 && maxPercentage >= minPercentage && maxPercentage <= 1 ? true : throw new ArgumentOutOfRangeException(INPUT_OUTSIDE_RANGE);
        }
        /// <summary>
        /// Applies the algorithm that restricts a component's width to a certain percentage of the customized father product's width
        /// </summary>
        /// <param name="customProduct">customized product</param>
        /// <param name="product">component product</param>
        /// <returns>component with restricted dimensions, null if the component is not compatible with any of the allowed dimensions</returns>
        public Product apply(CustomizedProduct customProduct, Product product)
        {
            double width = customProduct.customizedDimensions.width;
            double minWidth = width * minPercentage;
            double maxWidth = width * maxPercentage;

            List<Measurement> measurementsToRemove = new List<Measurement>();
            List<Measurement> measurementsToAdd = new List<Measurement>();

            List<Measurement> productMeasurements = product.measurements.Select(m => m.measurement).ToList();

            foreach (Measurement measurement in productMeasurements)
            {
                Dimension dimension = measurement.width;

                if (dimension.GetType() == typeof(SingleValueDimension))
                {
                    SingleValueDimension single = (SingleValueDimension)dimension;
                    if (single.value < minWidth || single.value > maxWidth)
                    {
                        measurementsToRemove.Add(measurement);
                    }
                }
                else if (dimension.GetType() == typeof(DiscreteDimensionInterval))
                {
                    DiscreteDimensionInterval discrete = (DiscreteDimensionInterval)dimension;
                    List<DoubleValue> valToRemove = new List<DoubleValue>();
                    foreach (double value in discrete.values)
                    {
                        if (value < minWidth || value > maxWidth)
                        {
                            valToRemove.Add(value);
                        }
                    }
                    foreach (double val in valToRemove)
                    {
                        discrete.values.Remove(val);
                    }
                    if (discrete.values.Count == 0)
                    {
                        measurementsToRemove.Add(measurement);
                    }
                    else if (discrete.values.Count == 1)
                    {
                        measurement.changeWidthDimension(new SingleValueDimension(discrete.values[0]));
                    }
                }
                else if (dimension.GetType() == typeof(ContinuousDimensionInterval))
                {
                    ContinuousDimensionInterval continuous = (ContinuousDimensionInterval)dimension;
                    if (continuous.minValue > maxWidth || continuous.maxValue < minWidth)
                    {
                        measurementsToRemove.Add(measurement);
                    }
                    else
                    {
                        if (continuous.minValue < minWidth)
                        {
                            continuous.minValue = minWidth;
                        }
                        if (continuous.maxValue > maxWidth)
                        {
                            continuous.maxValue = maxWidth;
                        }
                        if (continuous.maxValue == continuous.minValue)
                        {
                            SingleValueDimension single = new SingleValueDimension(continuous.maxValue);
                            measurement.changeWidthDimension(single);
                        }
                    }
                }

            }

            foreach (Measurement measurement in measurementsToAdd)
            {
                product.addMeasurement(measurement);
            }
            foreach (Measurement measurement in measurementsToRemove)
            {
                bool res = product.removeMeasurement(measurement);
                if (!res)
                {
                    return null;
                }
            }
            return product;
        }
    }
}
