using System;
using core.domain;
using core.modelview.customizeddimensions;
using core.services;
using Xunit;

namespace core_tests.modelview
{
    public class CustomizedDimensionsModelViewServiceTest
    {
        [Fact]
        public void ensureFromModelViewThrowsExceptionIfModelViewIsNull()
        {
            AddCustomizedDimensionsModelView addCustomizedDimensionsModelView = null;

            Action fromModelView = () => CustomizedDimensionsModelViewService.fromModelView(addCustomizedDimensionsModelView);

            Assert.Throws<ArgumentException>(fromModelView);
        }

        [Fact]
        public void ensureFromModelViewDoesNotConvertValuesIfNoUnitIsProvided()
        {
            AddCustomizedDimensionsModelView addCustomizedDimensionsModelView = new AddCustomizedDimensionsModelView();

            addCustomizedDimensionsModelView.height = 30;
            addCustomizedDimensionsModelView.width = 50;
            addCustomizedDimensionsModelView.depth = 15;


            CustomizedDimensions customizedDimensions = CustomizedDimensionsModelViewService.fromModelView(addCustomizedDimensionsModelView);

            Assert.Equal(addCustomizedDimensionsModelView.height, customizedDimensions.height);
            Assert.Equal(addCustomizedDimensionsModelView.width, customizedDimensions.width);
            Assert.Equal(addCustomizedDimensionsModelView.depth, customizedDimensions.depth);
        }

        [Fact]
        public void ensureFromModelViewConvertsValuesToProvidedUnit()
        {
            AddCustomizedDimensionsModelView addCustomizedDimensionsModelView = new AddCustomizedDimensionsModelView();

            string unit = "cm";

            addCustomizedDimensionsModelView.height = 30;
            addCustomizedDimensionsModelView.width = 50;
            addCustomizedDimensionsModelView.depth = 15;
            addCustomizedDimensionsModelView.unit = unit;

            //the customized dimensions' values should be in the minimum unit
            CustomizedDimensions customizedDimensions = CustomizedDimensionsModelViewService.fromModelView(addCustomizedDimensionsModelView);

            Assert.Equal(addCustomizedDimensionsModelView.height, MeasurementUnitService.convertToUnit(customizedDimensions.height, unit));
            Assert.Equal(addCustomizedDimensionsModelView.width, MeasurementUnitService.convertToUnit(customizedDimensions.width, unit));
            Assert.Equal(addCustomizedDimensionsModelView.depth, MeasurementUnitService.convertToUnit(customizedDimensions.depth, unit));
        }

        [Fact]
        public void ensureFromEntityThrowsExceptionIfCustomizedDimensionsIsNull()
        {
            CustomizedDimensions customizedDimensions = null;

            Action fromEntity = () => CustomizedDimensionsModelViewService.fromEntity(customizedDimensions);

            Assert.Throws<ArgumentException>(fromEntity);


            //Make sure overload has the same behaviour
            fromEntity = () => CustomizedDimensionsModelViewService.fromEntity(customizedDimensions, "m");

            Assert.Throws<ArgumentException>(fromEntity);
        }


        [Fact]
        public void ensureFromEntityDoesNotConvertValuesIfNoUnitIsProvided()
        {
            double height = 30;
            double width = 50;
            double depth = 15;

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(height, width, depth);

            GetCustomizedDimensionsModelView getCustomizedDimensionsModelView = CustomizedDimensionsModelViewService.fromEntity(customizedDimensions);

            Assert.Equal(getCustomizedDimensionsModelView.height, height);
            Assert.Equal(getCustomizedDimensionsModelView.width, width);
            Assert.Equal(getCustomizedDimensionsModelView.depth, depth);
        }


        [Fact]
        public void ensureFromEntityConvertsValuesToProvidedUnit()
        {
            double height = 30;
            double width = 50;
            double depth = 15;

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(height, width, depth);

            string unit = "m";

            GetCustomizedDimensionsModelView getCustomizedDimensionsModelView = CustomizedDimensionsModelViewService.fromEntity(customizedDimensions, unit);

            Assert.Equal(getCustomizedDimensionsModelView.height, MeasurementUnitService.convertToUnit(height, unit));
            Assert.Equal(getCustomizedDimensionsModelView.width, MeasurementUnitService.convertToUnit(width, unit));
            Assert.Equal(getCustomizedDimensionsModelView.depth, MeasurementUnitService.convertToUnit(depth, unit));
        }

        [Fact]
        public void ensureFromEntityDoesNotConvertValueIfNullUnitIsProvided()
        {
            double height = 30;
            double width = 50;
            double depth = 15;

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(height, width, depth);

            string unit = null;

            GetCustomizedDimensionsModelView getCustomizedDimensionsModelView = CustomizedDimensionsModelViewService.fromEntity(customizedDimensions, unit);

            Assert.Equal(getCustomizedDimensionsModelView.height, height);
            Assert.Equal(getCustomizedDimensionsModelView.width, width);
            Assert.Equal(getCustomizedDimensionsModelView.depth, depth);
        }

        [Fact]
        public void ensureFromEntityDoesNotConvertValueIfEmptyUnitIsProvided()
        {
            double height = 30;
            double width = 50;
            double depth = 15;

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(height, width, depth);

            string unit = "";

            GetCustomizedDimensionsModelView getCustomizedDimensionsModelView = CustomizedDimensionsModelViewService.fromEntity(customizedDimensions, unit);

            Assert.Equal(getCustomizedDimensionsModelView.height, height);
            Assert.Equal(getCustomizedDimensionsModelView.width, width);
            Assert.Equal(getCustomizedDimensionsModelView.depth, depth);
        }





    }
}