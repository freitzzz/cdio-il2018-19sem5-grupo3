using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace core.domain {
    /// <summary>
    /// Class that represents the algorithm that checks if complement occupies a certain percentage of the parent product
    /// </summary>
    public class WidthPercentageAlgorithm : Algorithm {
        /// <summary>
        /// Constant representing the WidthPercentageAlgorithm's name.
        /// </summary>
        private const string WIDTH_PERCENTAGE_ALGORITHM_NAME = "Width Percentage Algorithm";

        /// <summary>
        /// Constant representing the WidthPercentageAlgorithm's description.
        /// </summary>
        private const string WIDTH_PERCENTAGE_ALGORITHM_DESCRIPTION = "Restrains the component to occupy a certain percentage of the father product's width.";

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
        /// Constant representing the message describing the available value range.
        /// </summary>
        private const string VALUE_RANGE = "From 0 to 1";

        /// <summary>
        /// Input value invalid message
        /// </summary>
        private const string INPUT_OUTSIDE_RANGE = "Input is not within the established range!";

        /// <summary>
        /// Max Percentage invalid message
        /// </summary>
        private const string MAX_PERCENTAGE_INVALID = "Maximum Percentage value is not within the established range!";

        /// <summary>
        /// Min Percentage invalid message
        /// </summary>
        private const string MIN_PERCENTAGE_INVALID = "Minimum Percentage value is not within the established range!";

        /// <summary>
        /// Constructor used for injecting an instance of ILazyLoader.
        /// </summary>
        /// <param name="lazyLoader">Instance of ILazyLoader being injected.</param>
        private WidthPercentageAlgorithm(ILazyLoader lazyLoader) : base(lazyLoader) { }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public WidthPercentageAlgorithm() {
            Input input1 = Input.valueOf(MINIMUM_PERCENTAGE_INPUT_NAME, VALUE_RANGE);
            InputValue minPercentage = new InputValue(input1);
            Input input2 = Input.valueOf(MAXIMUM_PERCENTAGE_INPUT_NAME, VALUE_RANGE);
            InputValue maxPercentage = new InputValue(input2);

            List<InputValue> inputValues = new List<InputValue>() { minPercentage, maxPercentage };

            checkName(WIDTH_PERCENTAGE_ALGORITHM_NAME);
            checkDescription(WIDTH_PERCENTAGE_ALGORITHM_DESCRIPTION);
            checkInputs(inputValues.Select(iv => iv.input));

            this.name = WIDTH_PERCENTAGE_ALGORITHM_NAME;
            this.description = WIDTH_PERCENTAGE_ALGORITHM_DESCRIPTION;
            this.restrictionAlgorithm = RestrictionAlgorithm.WIDTH_PERCENTAGE_ALGORITHM;
            this.inputValues = new List<InputValue>(inputValues);
        }

        /// <summary>
        /// Retrieves the minimum percentage value.
        /// </summary>
        /// <returns>Minimum percentage value.</returns>
        private double getMinPercentage() {
            string minPercentageValue = this.inputValues
                    .Where(iv => iv.input.name.Equals(MINIMUM_PERCENTAGE_INPUT_NAME))
                    .Select(iv => iv.value).SingleOrDefault();

            double valueAsDouble;

            bool parsed = double.TryParse(minPercentageValue, NumberStyles.Float, CultureInfo.InvariantCulture, out valueAsDouble);
            return parsed ? valueAsDouble : -1;
        }

        /// <summary>
        /// Retrieves the maximum percentage value.
        /// </summary>
        /// <returns>Maximum percentage value.</returns>
        private double getMaxPercentage() {
            string maxPercentageValue = this.inputValues
                    .Where(iv => iv.input.name.Equals(MAXIMUM_PERCENTAGE_INPUT_NAME))
                    .Select(iv => iv.value).SingleOrDefault();

            double valueAsDouble;

            bool parsed = double.TryParse(maxPercentageValue, NumberStyles.Float, CultureInfo.InvariantCulture, out valueAsDouble);
            return parsed ? valueAsDouble : -1;
        }

        protected override void checkValue(Input input, string value) {
            if (input == null) throw new ArgumentException(INVALID_INPUT);

            Input algorithmInput = this.inputValues.Where(iv => iv.input.Equals(input)).Select(iv => iv.input).SingleOrDefault();

            if (algorithmInput == null) throw new ArgumentException(INVALID_INPUT);

            double valueAsDouble;
            bool parsed = double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out valueAsDouble);

            if (!parsed) throw new ArgumentException(INPUT_OUTSIDE_RANGE);


            //check if value is within the range
            if (valueAsDouble <= 0 || valueAsDouble > 1) throw new ArgumentException(INPUT_OUTSIDE_RANGE);

            switch (input.name) {
                case MINIMUM_PERCENTAGE_INPUT_NAME: {
                        double max = getMaxPercentage();

                        //only allow to set a minimum percentage under the max percentage
                        if (max > 0 && valueAsDouble > max) throw new ArgumentException(MIN_PERCENTAGE_INVALID);

                    }
                    break;
                case MAXIMUM_PERCENTAGE_INPUT_NAME: {
                        double min = getMinPercentage();

                        //only allow to set a maximum percentage above the min percentage
                        if (min > 0 && valueAsDouble < min) throw new ArgumentException(MAX_PERCENTAGE_INVALID);
                    }
                    break;
            }
        }

        /// <summary>
        /// Applies the algorithm that restricts a component's width to a certain percentage of the customized father product's width
        /// </summary>
        /// <param name="customProduct">customized product</param>
        /// <param name="product">component product</param>
        /// <returns>component with restricted dimensions, null if the component is not compatible with any of the allowed dimensions</returns>
        public override Product apply(CustomizedProduct customProduct, Product originalProduct) {
            ready();
            List<Material> materials = new List<Material>();
            foreach (ProductMaterial productMaterial in originalProduct.productMaterials) {
                materials.Add(productMaterial.material);
            }
            List<Measurement> measurements = new List<Measurement>();
            foreach (ProductMeasurement productMeasurement in originalProduct.productMeasurements) {
                measurements.Add(productMeasurement.measurement);
            }

            Product copyProduct = null;

            if (originalProduct.components.Any()) {
                List<Product> components = new List<Product>();
                foreach (Component component in originalProduct.components) {
                    components.Add(component.complementaryProduct);
                }
                copyProduct = new Product(originalProduct.reference, originalProduct.designation, originalProduct.modelFilename, originalProduct.productCategory, materials, measurements, components, originalProduct.slotWidths);
            } else {
                copyProduct = new Product(originalProduct.reference, originalProduct.designation, originalProduct.modelFilename, originalProduct.productCategory, materials, measurements, originalProduct.slotWidths);
            }
            double width = customProduct.customizedDimensions.width;
            double min = getMinPercentage();
            if (min < 0) {
                throw new ArgumentException(MIN_PERCENTAGE_INVALID);
            }
            double max = getMaxPercentage();
            if (max < 0) {
                throw new ArgumentException(MAX_PERCENTAGE_INVALID);
            }
            double minWidth = width * getMinPercentage();
            double maxWidth = width * getMaxPercentage();

            List<Measurement> measurementsToRemove = new List<Measurement>();

            List<Measurement> productMeasurements = copyProduct.productMeasurements.Select(m => m.measurement).ToList();

            foreach (Measurement measurement in productMeasurements) {
                Dimension dimension = measurement.width;

                if (dimension.GetType() == typeof(SingleValueDimension)) {
                    SingleValueDimension single = (SingleValueDimension)dimension;
                    if (single.value < minWidth || single.value > maxWidth) {
                        measurementsToRemove.Add(measurement);
                    }
                } else if (dimension.GetType() == typeof(DiscreteDimensionInterval)) {
                    DiscreteDimensionInterval discrete = (DiscreteDimensionInterval)dimension;
                    List<DoubleValue> valToRemove = new List<DoubleValue>();
                    foreach (double value in discrete.values) {
                        if (value < minWidth || value > maxWidth) {
                            valToRemove.Add(value);
                        }
                    }
                    foreach (double val in valToRemove) {
                        discrete.values.Remove(val);
                    }
                    if (discrete.values.Count == 0) {
                        measurementsToRemove.Add(measurement);
                    } else if (discrete.values.Count == 1) {
                        measurement.changeWidthDimension(new SingleValueDimension(discrete.values[0]));
                    }
                } else if (dimension.GetType() == typeof(ContinuousDimensionInterval)) {
                    ContinuousDimensionInterval continuous = (ContinuousDimensionInterval)dimension;
                    if (continuous.minValue > maxWidth || continuous.maxValue < minWidth) {
                        measurementsToRemove.Add(measurement);
                    } else {
                        double currentMin = continuous.minValue, currentMax = continuous.maxValue;
                        if (continuous.minValue < minWidth) {
                            currentMin = minWidth;
                        }
                        if (continuous.maxValue > maxWidth) {
                            currentMax = maxWidth;
                        }
                        if (currentMin == currentMax) {
                            SingleValueDimension single = new SingleValueDimension(currentMax);
                            measurement.changeWidthDimension(single);
                        } else {
                            double newIncrement = continuous.increment;
                            if (currentMax - currentMin < continuous.increment) {
                                newIncrement = currentMax - currentMin;
                            }
                            ContinuousDimensionInterval newContinuous = new ContinuousDimensionInterval(currentMin, currentMax, newIncrement);

                            measurement.changeWidthDimension(newContinuous);
                        }
                    }
                }
            }
            foreach (Measurement measurement in measurementsToRemove) {

                try {
                    copyProduct.removeMeasurement(measurement);
                } catch (InvalidOperationException) {
                    return null;
                } catch (ArgumentException) {
                    return null;
                }
            }
            return copyProduct;
        }
    }
}