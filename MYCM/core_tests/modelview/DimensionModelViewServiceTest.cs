using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.modelview.dimension;
using core.services;
using Xunit;

namespace core_tests.modelview
{
    public class DimensionModelViewServiceTest
    {
        private bool equalModelViews(GetDimensionModelView oneDimensionModelView, GetDimensionModelView otherDimensionModelView)
        {
            if (oneDimensionModelView == otherDimensionModelView)
            {
                return true;
            }

            if (oneDimensionModelView == null || otherDimensionModelView == null || oneDimensionModelView.GetType() != otherDimensionModelView.GetType())
            {
                return false;
            }

            if (oneDimensionModelView.GetType() == typeof(GetContinuousDimensionIntervalModelView))
            {
                GetContinuousDimensionIntervalModelView oneContinuousMV = (GetContinuousDimensionIntervalModelView)oneDimensionModelView;
                GetContinuousDimensionIntervalModelView otherContinuousMV = (GetContinuousDimensionIntervalModelView)otherDimensionModelView;


                return oneContinuousMV.minValue.Equals(otherContinuousMV.minValue)
                    && oneContinuousMV.maxValue.Equals(otherContinuousMV.maxValue)
                    && oneContinuousMV.increment.Equals(otherContinuousMV.increment)
                    && oneContinuousMV.unit.Equals(otherContinuousMV.unit);
            }

            else if (oneDimensionModelView.GetType() == typeof(GetDiscreteDimensionIntervalModelView))
            {
                GetDiscreteDimensionIntervalModelView oneDiscreteMV = (GetDiscreteDimensionIntervalModelView)oneDimensionModelView;
                GetDiscreteDimensionIntervalModelView otherDiscreteMV = (GetDiscreteDimensionIntervalModelView)otherDimensionModelView;

                var firstNotSecond = oneDiscreteMV.values.Except(otherDiscreteMV.values).ToList();
                var secondNotFirst = otherDiscreteMV.values.Except(oneDiscreteMV.values).ToList();

                bool equalLists = !firstNotSecond.Any() && !secondNotFirst.Any();

                return equalLists && oneDiscreteMV.unit.Equals(otherDiscreteMV.unit);
            }

            else if (oneDimensionModelView.GetType() == typeof(GetSingleValueDimensionModelView))
            {
                GetSingleValueDimensionModelView oneSingleValueMV = (GetSingleValueDimensionModelView)oneDimensionModelView;
                GetSingleValueDimensionModelView otherSingleValueMV = (GetSingleValueDimensionModelView)otherDimensionModelView;

                return oneSingleValueMV.value.Equals(otherSingleValueMV.value) && oneSingleValueMV.unit.Equals(otherSingleValueMV.unit);
            }
            else
            {
                return false;
            }
        }


        private bool modelViewEqualsDimension(AddDimensionModelView modelView, Dimension dimension)
        {
            if (modelView.GetType() == typeof(AddDiscreteDimensionIntervalModelView) && dimension.GetType() == typeof(DiscreteDimensionInterval))
            {
                DiscreteDimensionInterval discreteDimension = (DiscreteDimensionInterval)dimension;
                AddDiscreteDimensionIntervalModelView discreteModelView = (AddDiscreteDimensionIntervalModelView)modelView;

                List<double> expectedValues = new List<double>();

                foreach (double modelViewValue in discreteModelView.values)
                {
                    double expectedValue = MeasurementUnitService.convertFromUnit(modelViewValue, discreteModelView.unit);
                    expectedValues.Add(expectedValue);
                }

                var firstNotSecond = expectedValues.Except(discreteDimension.values.Select(dv => dv.value)).ToList();
                var secondNotFirst = discreteDimension.values.Select(dv => dv.value).Except(expectedValues).ToList();

                bool equalLists = !firstNotSecond.Any() && !secondNotFirst.Any();

                return equalLists;
            }
            else if (modelView.GetType() == typeof(AddSingleValueDimensionModelView) && dimension.GetType() == typeof(SingleValueDimension))
            {
                SingleValueDimension singleValueDimension = (SingleValueDimension)dimension;
                AddSingleValueDimensionModelView singleValueModelView = (AddSingleValueDimensionModelView)modelView;

                double expectedValue = MeasurementUnitService.convertFromUnit(singleValueModelView.value, singleValueModelView.unit);

                return expectedValue.Equals(singleValueDimension.value);
            }
            else if (modelView.GetType() == typeof(AddContinuousDimensionIntervalModelView) && dimension.GetType() == typeof(ContinuousDimensionInterval))
            {
                ContinuousDimensionInterval continuousDimension = (ContinuousDimensionInterval)dimension;
                AddContinuousDimensionIntervalModelView continuousModelView = (AddContinuousDimensionIntervalModelView)modelView;

                double expectedMinValue = MeasurementUnitService.convertFromUnit(continuousModelView.minValue, continuousModelView.unit);
                double expectedMaxValue = MeasurementUnitService.convertFromUnit(continuousModelView.maxValue, continuousModelView.unit);
                double expectedIncrement = MeasurementUnitService.convertFromUnit(continuousModelView.increment, continuousModelView.unit);

                return expectedMinValue.Equals(continuousDimension.minValue)
                    && expectedMaxValue.Equals(continuousDimension.maxValue)
                    && expectedIncrement.Equals(continuousDimension.increment);
            }
            else
            {
                return false;
            }
        }

        [Fact]
        public void ensureFromEntityThrowsExceptionIfDimensionIsNull()
        {
            Dimension dimension = null;

            Action fromEntity = () => DimensionModelViewService.fromEntity(dimension);

            Assert.Throws<ArgumentNullException>(fromEntity);
        }

        [Fact]
        public void ensureFromEntityConvertsToGetSingleValueDimensionModelViewIfProvidedDimensionIsSingleValue()
        {
            Dimension singleValueDimension = new SingleValueDimension(15);

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(singleValueDimension);

            Type expectedDimensionType = typeof(GetSingleValueDimensionModelView);

            Assert.Equal(expectedDimensionType, getDimensionModelView.GetType());
        }

        [Fact]
        public void ensureFromEntityConvertsSingleValueDimension()
        {
            Dimension singleValueDimension = new SingleValueDimension(15);

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(singleValueDimension);

            GetSingleValueDimensionModelView expected = new GetSingleValueDimensionModelView();
            expected.value = 15;
            expected.unit = MeasurementUnitService.getMinimumUnit();

            GetSingleValueDimensionModelView result = (GetSingleValueDimensionModelView)getDimensionModelView;

            Assert.Equal(expected.value, result.value);
            Assert.Equal(expected.unit, result.unit);
        }

        [Fact]
        public void ensureFromEntityConvertsToGetDiscreteDimensionIntervalModelViewIfProvidedDimensionIsDiscreteInterval()
        {
            Dimension discreteDimensionInterval = new DiscreteDimensionInterval(new List<double>(){
                21, 35, 42
            });

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(discreteDimensionInterval);

            Type expectedDimensionType = typeof(GetDiscreteDimensionIntervalModelView);

            Assert.Equal(expectedDimensionType, getDimensionModelView.GetType());
        }

        [Fact]
        public void ensureFromEntityConvertsDiscreteDimensionInterval()
        {
            List<double> values = new List<double>() { 21, 35, 42 };

            Dimension discreteDimensionInterval = new DiscreteDimensionInterval(values);

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(discreteDimensionInterval);

            GetDiscreteDimensionIntervalModelView result = (GetDiscreteDimensionIntervalModelView)getDimensionModelView;

            GetDiscreteDimensionIntervalModelView expected = new GetDiscreteDimensionIntervalModelView();
            expected.values = values;
            expected.unit = MeasurementUnitService.getMinimumUnit();

            Assert.Equal(expected.values, result.values);
            Assert.Equal(expected.unit, result.unit);
        }

        [Fact]
        public void ensureFromEntityConvertsToGetContinuousDimensionIntervalModelViewIfProvidedDimensionIsContinuousInterval()
        {
            double minValue = 20;
            double maxValue = 60;
            double increment = 2;

            Dimension continuousDimensionInterval = new ContinuousDimensionInterval(minValue, maxValue, increment);

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(continuousDimensionInterval);

            Type expectedDimensionType = typeof(GetContinuousDimensionIntervalModelView);

            Assert.Equal(expectedDimensionType, getDimensionModelView.GetType());
        }

        [Fact]
        public void ensureFromEntityConvertsContinuousDimensionInterval()
        {
            double minValue = 20;
            double maxValue = 60;
            double increment = 2;

            Dimension continuousDimensionInterval = new ContinuousDimensionInterval(minValue, maxValue, increment);

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(continuousDimensionInterval);

            GetContinuousDimensionIntervalModelView result = (GetContinuousDimensionIntervalModelView)getDimensionModelView;

            GetContinuousDimensionIntervalModelView expected = new GetContinuousDimensionIntervalModelView();
            expected.minValue = minValue;
            expected.maxValue = maxValue;
            expected.increment = increment;
            expected.unit = MeasurementUnitService.getMinimumUnit();

            Assert.Equal(expected.minValue, result.minValue);
            Assert.Equal(expected.maxValue, result.maxValue);
            Assert.Equal(expected.increment, result.increment);
            Assert.Equal(expected.unit, result.unit);
        }

        [Fact]
        public void ensureFromEntityWithUnitThrowsExceptionIfDimensionIsNull()
        {
            Dimension dimension = null;

            string unit = "dm";

            Action fromEntity = () => DimensionModelViewService.fromEntity(dimension, unit);

            Assert.Throws<ArgumentNullException>(fromEntity);
        }

        [Fact]
        public void ensureFromEntityWithNullUnitUsesMinimumUnit()
        {
            double value = 15;

            Dimension singleValueDimension = new SingleValueDimension(value);

            string unit = null;

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(singleValueDimension, unit);

            GetSingleValueDimensionModelView expected = new GetSingleValueDimensionModelView();
            expected.value = value;
            expected.unit = MeasurementUnitService.getMinimumUnit();

            GetSingleValueDimensionModelView result = (GetSingleValueDimensionModelView)getDimensionModelView;

            Assert.Equal(expected.value, result.value);
            Assert.Equal(expected.unit, result.unit);
        }

        [Fact]
        public void ensureFromEntityWithEmptyUnitUsesMinimumUnit()
        {
            double value = 15;

            Dimension singleValueDimension = new SingleValueDimension(value);

            string unit = "";

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(singleValueDimension, unit);

            GetSingleValueDimensionModelView expected = new GetSingleValueDimensionModelView();
            expected.value = value;
            expected.unit = MeasurementUnitService.getMinimumUnit();

            GetSingleValueDimensionModelView result = (GetSingleValueDimensionModelView)getDimensionModelView;

            Assert.Equal(expected.value, result.value);
            Assert.Equal(expected.unit, result.unit);

        }

        [Fact]
        public void ensureFromEntityWithUnitConvertsToGetSingleValueDimensionModelViewIfProvidedDimensionIsSingleValue()
        {
            Dimension singleValueDimension = new SingleValueDimension(15);

            string unit = "dm";

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(singleValueDimension, unit);

            Type expectedDimensionType = typeof(GetSingleValueDimensionModelView);

            Assert.Equal(expectedDimensionType, getDimensionModelView.GetType());
        }

        [Fact]
        public void ensureFromEntityWithUnitConvertsSingleValueDimension()
        {
            double value = 15;

            Dimension singleValueDimension = new SingleValueDimension(value);

            string unit = "cm";

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(singleValueDimension, unit);

            GetSingleValueDimensionModelView expected = new GetSingleValueDimensionModelView();
            expected.value = MeasurementUnitService.convertToUnit(value, unit);
            expected.unit = unit;

            GetSingleValueDimensionModelView result = (GetSingleValueDimensionModelView)getDimensionModelView;

            Assert.Equal(expected.value, result.value);
            Assert.Equal(expected.unit, result.unit);
        }

        [Fact]
        public void ensureFromEntityWithUnitConvertsToGetDiscreteDimensionIntervalModelViewIfProvidedDimensionIsDiscreteInterval()
        {
            Dimension discreteDimensionInterval = new DiscreteDimensionInterval(new List<double>(){
                21, 35, 42
            });

            string unit = "dm";

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(discreteDimensionInterval, unit);

            Type expectedDimensionType = typeof(GetDiscreteDimensionIntervalModelView);

            Assert.Equal(expectedDimensionType, getDimensionModelView.GetType());
        }

        [Fact]
        public void ensureFromEntityWithUnitConvertsDiscreteDimensionInterval()
        {
            List<double> values = new List<double>(){
                21, 35, 42
            };

            Dimension discreteDimensionInterval = new DiscreteDimensionInterval(values);

            string unit = "dm";

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(discreteDimensionInterval, unit);

            GetDiscreteDimensionIntervalModelView result = (GetDiscreteDimensionIntervalModelView)getDimensionModelView;

            GetDiscreteDimensionIntervalModelView expected = new GetDiscreteDimensionIntervalModelView();
            List<double> expectedValues = new List<double>();

            foreach (double dimensionValue in values)
            {
                double convertedValue = MeasurementUnitService.convertToUnit(dimensionValue, unit);
                expectedValues.Add(convertedValue);
            }

            expected.values = expectedValues;
            expected.unit = unit;

            Assert.Equal(expected.values, result.values);
            Assert.Equal(expected.unit, result.unit);
        }

        [Fact]
        public void ensureFromEntityWithUnitConvertsToGetContinuousDimensionIntervalModelViewIfProvidedDimensionIsContinuousInterval()
        {
            Dimension continuousDimensionInterval = new ContinuousDimensionInterval(
                minValue: 20,
                maxValue: 60,
                increment: 2
            );

            string unit = "dm";

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(continuousDimensionInterval, unit);

            Type expectedDimensionType = typeof(GetContinuousDimensionIntervalModelView);

            Assert.Equal(expectedDimensionType, getDimensionModelView.GetType());
        }

        [Fact]
        public void ensureFromEntityWithUnitConvertsContinuousDimensionInterval()
        {
            double minValue = 20;
            double maxValue = 60;
            double increment = 2;

            Dimension continuousDimensionInterval = new ContinuousDimensionInterval(minValue, maxValue, increment);

            string unit = "dm";

            GetDimensionModelView getDimensionModelView = DimensionModelViewService.fromEntity(continuousDimensionInterval, unit);

            GetContinuousDimensionIntervalModelView result = (GetContinuousDimensionIntervalModelView)getDimensionModelView;

            GetContinuousDimensionIntervalModelView expected = new GetContinuousDimensionIntervalModelView();

            expected.minValue = MeasurementUnitService.convertToUnit(minValue, unit);
            expected.maxValue = MeasurementUnitService.convertToUnit(maxValue, unit);
            expected.increment = MeasurementUnitService.convertToUnit(increment, unit);
            expected.unit = unit;

            Assert.Equal(expected.minValue, result.minValue);
            Assert.Equal(expected.maxValue, result.maxValue);
            Assert.Equal(expected.increment, result.increment);
            Assert.Equal(expected.unit, result.unit);
        }


        [Fact]
        public void ensureFromCollectionThrowsExceptionIfEnumerableIsNull()
        {
            IEnumerable<Dimension> dimensions = null;

            Action fromCollection = () => DimensionModelViewService.fromCollection(dimensions);

            Assert.Throws<ArgumentNullException>(fromCollection);
        }

        [Fact]
        public void ensureFromCollectionThrowsExceptionIfElementIsNull()
        {
            double minValue = 20;
            double maxValue = 60;
            double increment = 2;

            Dimension continuousDimensionInterval = new ContinuousDimensionInterval(minValue, maxValue, increment);

            List<double> values = new List<double>(){
                21, 35, 42
            };

            Dimension discreteDimensionInterval = new DiscreteDimensionInterval(values);


            Dimension nullDimension = null;

            IEnumerable<Dimension> dimensions = new List<Dimension>() { continuousDimensionInterval, discreteDimensionInterval, nullDimension };

            Action fromCollection = () => DimensionModelViewService.fromCollection(dimensions);

            Assert.Throws<ArgumentNullException>(fromCollection);
        }


        [Fact]
        public void ensureFromCollectionWithUnitThrowsExceptionIfEnumerableIsNull()
        {
            IEnumerable<Dimension> dimensions = null;

            string unit = "m";

            Action fromCollection = () => DimensionModelViewService.fromCollection(dimensions, unit);

            Assert.Throws<ArgumentNullException>(fromCollection);
        }

        [Fact]
        public void ensureFromCollectionWithUnitThrowsExceptionIfElementIsNull()
        {
            double minValue = 20;
            double maxValue = 60;
            double increment = 2;

            Dimension continuousDimensionInterval = new ContinuousDimensionInterval(minValue, maxValue, increment);

            List<double> values = new List<double>(){
                21, 35, 42
            };

            Dimension discreteDimensionInterval = new DiscreteDimensionInterval(values);

            Dimension nullDimension = null;

            IEnumerable<Dimension> dimensions = new List<Dimension>() { continuousDimensionInterval, discreteDimensionInterval, nullDimension };

            string unit = "m";

            Action fromCollection = () => DimensionModelViewService.fromCollection(dimensions, unit);

            Assert.Throws<ArgumentNullException>(fromCollection);
        }


        [Fact]
        public void ensureFromCollectionConvertsModelViews()
        {
            double minValue = 20;
            double maxValue = 60;
            double increment = 2;
            Dimension continuousDimensionInterval = new ContinuousDimensionInterval(minValue, maxValue, increment);

            List<double> values = new List<double>(){
                21, 35, 42
            };
            Dimension discreteDimensionInterval = new DiscreteDimensionInterval(values);

            double value = 72;
            Dimension singleValueDimension = new SingleValueDimension(value);

            IEnumerable<Dimension> dimensions = new List<Dimension>() { continuousDimensionInterval, discreteDimensionInterval, singleValueDimension };

            GetAllDimensionsModelView getAllDimensionsModelView = DimensionModelViewService.fromCollection(dimensions);

            string minimumUnit = MeasurementUnitService.getMinimumUnit();

            GetContinuousDimensionIntervalModelView continuousDimensionIntervalModelView = new GetContinuousDimensionIntervalModelView();

            continuousDimensionIntervalModelView.minValue = minValue;
            continuousDimensionIntervalModelView.maxValue = maxValue;
            continuousDimensionIntervalModelView.increment = increment;
            continuousDimensionIntervalModelView.unit = minimumUnit;


            GetDiscreteDimensionIntervalModelView discreteDimensionIntervalModelView = new GetDiscreteDimensionIntervalModelView();

            discreteDimensionIntervalModelView.values = values;
            discreteDimensionIntervalModelView.unit = minimumUnit;


            GetSingleValueDimensionModelView singleValueDimensionModelView = new GetSingleValueDimensionModelView();

            singleValueDimensionModelView.value = value;
            singleValueDimensionModelView.unit = minimumUnit;

            GetAllDimensionsModelView expected = new GetAllDimensionsModelView() { continuousDimensionIntervalModelView, discreteDimensionIntervalModelView, singleValueDimensionModelView };

            for (int i = 0; i < 3; i++)
            {
                Assert.True(equalModelViews(getAllDimensionsModelView[i], expected[i]));
            }
        }


        [Fact]
        public void ensureFromCollectionWithNullUnitUsesMinimumUnit()
        {
            double minValue = 20;
            double maxValue = 60;
            double increment = 2;
            Dimension continuousDimensionInterval = new ContinuousDimensionInterval(minValue, maxValue, increment);

            List<double> values = new List<double>(){
                21, 35, 42
            };
            Dimension discreteDimensionInterval = new DiscreteDimensionInterval(values);

            double value = 72;
            Dimension singleValueDimension = new SingleValueDimension(value);

            IEnumerable<Dimension> dimensions = new List<Dimension>() { continuousDimensionInterval, discreteDimensionInterval, singleValueDimension };

            GetAllDimensionsModelView getAllDimensionsModelView = DimensionModelViewService.fromCollection(dimensions, null);

            string minimumUnit = MeasurementUnitService.getMinimumUnit();

            GetContinuousDimensionIntervalModelView continuousDimensionIntervalModelView = new GetContinuousDimensionIntervalModelView();

            continuousDimensionIntervalModelView.minValue = minValue;
            continuousDimensionIntervalModelView.maxValue = maxValue;
            continuousDimensionIntervalModelView.increment = increment;
            continuousDimensionIntervalModelView.unit = minimumUnit;


            GetDiscreteDimensionIntervalModelView discreteDimensionIntervalModelView = new GetDiscreteDimensionIntervalModelView();

            discreteDimensionIntervalModelView.values = values;
            discreteDimensionIntervalModelView.unit = minimumUnit;


            GetSingleValueDimensionModelView singleValueDimensionModelView = new GetSingleValueDimensionModelView();

            singleValueDimensionModelView.value = value;
            singleValueDimensionModelView.unit = minimumUnit;

            GetAllDimensionsModelView expected = new GetAllDimensionsModelView() { continuousDimensionIntervalModelView, discreteDimensionIntervalModelView, singleValueDimensionModelView };

            for (int i = 0; i < 3; i++)
            {
                Assert.True(equalModelViews(getAllDimensionsModelView[i], expected[i]));
            }
        }


        [Fact]
        public void ensureFromCollectionWithEmptyUnitUsesMinimumUnit()
        {
            double minValue = 20;
            double maxValue = 60;
            double increment = 2;
            Dimension continuousDimensionInterval = new ContinuousDimensionInterval(minValue, maxValue, increment);

            List<double> values = new List<double>(){
                21, 35, 42
            };
            Dimension discreteDimensionInterval = new DiscreteDimensionInterval(values);

            double value = 72;
            Dimension singleValueDimension = new SingleValueDimension(value);

            IEnumerable<Dimension> dimensions = new List<Dimension>() { continuousDimensionInterval, discreteDimensionInterval, singleValueDimension };

            GetAllDimensionsModelView getAllDimensionsModelView = DimensionModelViewService.fromCollection(dimensions, "");

            string minimumUnit = MeasurementUnitService.getMinimumUnit();

            GetContinuousDimensionIntervalModelView continuousDimensionIntervalModelView = new GetContinuousDimensionIntervalModelView();

            continuousDimensionIntervalModelView.minValue = minValue;
            continuousDimensionIntervalModelView.maxValue = maxValue;
            continuousDimensionIntervalModelView.increment = increment;
            continuousDimensionIntervalModelView.unit = minimumUnit;


            GetDiscreteDimensionIntervalModelView discreteDimensionIntervalModelView = new GetDiscreteDimensionIntervalModelView();

            discreteDimensionIntervalModelView.values = values;
            discreteDimensionIntervalModelView.unit = minimumUnit;


            GetSingleValueDimensionModelView singleValueDimensionModelView = new GetSingleValueDimensionModelView();

            singleValueDimensionModelView.value = value;
            singleValueDimensionModelView.unit = minimumUnit;

            GetAllDimensionsModelView expected = new GetAllDimensionsModelView() { continuousDimensionIntervalModelView, discreteDimensionIntervalModelView, singleValueDimensionModelView };

            for (int i = 0; i < 3; i++)
            {
                Assert.True(equalModelViews(getAllDimensionsModelView[i], expected[i]));
            }
        }

        [Fact]
        public void ensureFromModelViewThrowsExceptionIfModelViewIsNull()
        {
            Action fromModelView = () => DimensionModelViewService.fromModelView(null);

            Assert.Throws<ArgumentNullException>(fromModelView);
        }


        [Fact]
        public void ensureFromModelViewCreatesSingleValueDimension()
        {
            AddSingleValueDimensionModelView singleValueDimensionModelView = new AddSingleValueDimensionModelView();
            singleValueDimensionModelView.value = 124;

            Dimension dimension = DimensionModelViewService.fromModelView(singleValueDimensionModelView);

            Type singleValueDimensionType = typeof(SingleValueDimension);

            Assert.Equal(singleValueDimensionType, dimension.GetType());
        }


        [Fact]
        public void ensureFromModelViewCreatesSingleValueDimensionWithExpectedData()
        {
            double value = 25;
            string unit = "dm";

            AddSingleValueDimensionModelView singleValueDimensionModelView = new AddSingleValueDimensionModelView();
            singleValueDimensionModelView.value = value;
            singleValueDimensionModelView.unit = unit;

            SingleValueDimension singleValueDimension = (SingleValueDimension)DimensionModelViewService.fromModelView(singleValueDimensionModelView);

            double expectedValue = MeasurementUnitService.convertFromUnit(value, unit);

            Assert.Equal(expectedValue, singleValueDimension.value);
        }


        [Fact]
        public void ensureFromModelViewCreatesContinuousDimensionInterval()
        {
            AddContinuousDimensionIntervalModelView continuousDimensionIntervalModelView = new AddContinuousDimensionIntervalModelView();
            continuousDimensionIntervalModelView.minValue = 25;
            continuousDimensionIntervalModelView.maxValue = 45;
            continuousDimensionIntervalModelView.increment = 5;

            Dimension dimension = DimensionModelViewService.fromModelView(continuousDimensionIntervalModelView);

            Type continuousDimensionIntervalType = typeof(ContinuousDimensionInterval);

            Assert.Equal(continuousDimensionIntervalType, dimension.GetType());
        }


        [Fact]
        public void ensureFromModelViewCreatesContinuousDimensionIntervalWithExpectedData()
        {
            double minValue = 20;
            double maxValue = 120;
            double increment = 5;

            string unit = "m";

            AddContinuousDimensionIntervalModelView continuousDimensionIntervalModelView = new AddContinuousDimensionIntervalModelView()
            {
                minValue = minValue,
                maxValue = maxValue,
                increment = increment,
                unit = unit
            };


            ContinuousDimensionInterval continuousDimensionInterval = (ContinuousDimensionInterval)DimensionModelViewService.fromModelView(continuousDimensionIntervalModelView);

            double expectedMinValue = MeasurementUnitService.convertFromUnit(minValue, unit);
            double expectedMaxvalue = MeasurementUnitService.convertFromUnit(maxValue, unit);
            double expectedIncrement = MeasurementUnitService.convertFromUnit(increment, unit);


            Assert.Equal(expectedMinValue, continuousDimensionInterval.minValue);
            Assert.Equal(expectedMaxvalue, continuousDimensionInterval.maxValue);
            Assert.Equal(expectedIncrement, continuousDimensionInterval.increment);
        }


        [Fact]
        public void ensureFromModelViewCreatesDiscreteDimensionInterval()
        {
            AddDiscreteDimensionIntervalModelView discreteDimensionIntervalModelView = new AddDiscreteDimensionIntervalModelView();
            discreteDimensionIntervalModelView.values = new List<double>() { 12, 31, 45, 67 };

            Dimension dimension = DimensionModelViewService.fromModelView(discreteDimensionIntervalModelView);

            Type discreteDimensionIntervalType = typeof(DiscreteDimensionInterval);

            Assert.Equal(discreteDimensionIntervalType, dimension.GetType());
        }


        [Fact]
        public void ensureFromModelViewCreatesDiscreteDimensionIntervalWithExpectedData()
        {
            List<double> values = new List<double>() { 12, 31, 45, 67 };

            string unit = "cm";

            AddDiscreteDimensionIntervalModelView discreteDimensionIntervalModelView = new AddDiscreteDimensionIntervalModelView();
            discreteDimensionIntervalModelView.values = values;
            discreteDimensionIntervalModelView.unit = unit;

            DiscreteDimensionInterval discreteDimensionInterval = (DiscreteDimensionInterval)DimensionModelViewService.fromModelView(discreteDimensionIntervalModelView);

            for (int i = 0; i < values.Count; i++)
            {
                double expectedValue = MeasurementUnitService.convertFromUnit(values[i], unit);

                Assert.Equal(expectedValue, discreteDimensionInterval.values[i].value);
            }

        }


        [Fact]
        public void ensureFromModelViewsThrowsExceptionIfEnumerableIsNull()
        {
            IEnumerable<AddDimensionModelView> dimensionModelViews = null;

            Action fromModelViews = () => DimensionModelViewService.fromModelViews(dimensionModelViews);

            Assert.Throws<ArgumentNullException>(fromModelViews);
        }


        [Fact]
        public void ensureFromModelViewsThrowsExceptionIfElementIsNull()
        {
            string unit = "m";

            double minValue = 20;
            double maxValue = 120;
            double increment = 5;

            AddContinuousDimensionIntervalModelView continuousDimensionIntervalModelView = new AddContinuousDimensionIntervalModelView()
            {
                minValue = minValue,
                maxValue = maxValue,
                increment = increment,
                unit = unit
            };


            List<double> values = new List<double>() { 12, 31, 45, 67 };

            AddDiscreteDimensionIntervalModelView discreteDimensionIntervalModelView = new AddDiscreteDimensionIntervalModelView();
            discreteDimensionIntervalModelView.values = values;
            discreteDimensionIntervalModelView.unit = unit;


            AddDimensionModelView nullModelView = null;

            IEnumerable<AddDimensionModelView> modelViews = new List<AddDimensionModelView>() { continuousDimensionIntervalModelView, discreteDimensionIntervalModelView, nullModelView };

            Action fromModelViews = () => DimensionModelViewService.fromModelViews(modelViews);

            Assert.Throws<ArgumentNullException>(fromModelViews);
        }


        [Fact]
        public void ensureFromModelViewsCreatesEnumerableOfDimensionWithExpectedData()
        {
            string unit = "m";

            double minValue = 20;
            double maxValue = 120;
            double increment = 5;

            AddContinuousDimensionIntervalModelView continuousDimensionIntervalModelView = new AddContinuousDimensionIntervalModelView()
            {
                minValue = minValue,
                maxValue = maxValue,
                increment = increment,
                unit = unit
            };


            List<double> values = new List<double>() { 12, 31, 45, 67 };

            AddDiscreteDimensionIntervalModelView discreteDimensionIntervalModelView = new AddDiscreteDimensionIntervalModelView();
            discreteDimensionIntervalModelView.values = values;
            discreteDimensionIntervalModelView.unit = unit;


            IEnumerable<AddDimensionModelView> modelViews = new List<AddDimensionModelView>() { continuousDimensionIntervalModelView, discreteDimensionIntervalModelView };

            IEnumerable<Dimension> dimensions = DimensionModelViewService.fromModelViews(modelViews);

            Assert.Equal(modelViews.Count(), dimensions.Count());

            var modelViewEnumerator = modelViews.GetEnumerator();
            var dimensionEnumerator = dimensions.GetEnumerator();

            for (int i = 0; i < modelViews.Count(); i++)
            {
                modelViewEnumerator.MoveNext();
                dimensionEnumerator.MoveNext();

                Assert.True(modelViewEqualsDimension(modelViewEnumerator.Current, dimensionEnumerator.Current));
            }
        }
    }
}