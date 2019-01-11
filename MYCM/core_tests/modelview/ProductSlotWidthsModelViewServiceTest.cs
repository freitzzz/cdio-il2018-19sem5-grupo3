using System;
using core.domain;
using core.modelview.productslotwidths;
using core.services;
using Xunit;

namespace core_tests.modelview
{
    public class ProductSlotWidthsModelViewServiceTest
    {
        [Fact]
        public void ensureFromModelViewThrowsExceptionIfModelViewIsNull()
        {
            Action fromModelView = () => ProductSlotWidthsModelViewService.fromModelView(null);

            Assert.Throws<ArgumentNullException>(fromModelView);
        }

        [Fact]
        public void ensureFromModelViewCreatesInstance()
        {
            double minWidth = 25;
            double maxWidth = 50;
            double recommendedWidth = 35;

            string unit = "cm";

            AddProductSlotWidthsModelView productSlotWidthsModelView = new AddProductSlotWidthsModelView();
            productSlotWidthsModelView.minWidth = minWidth;
            productSlotWidthsModelView.maxWidth = maxWidth;
            productSlotWidthsModelView.recommendedWidth = recommendedWidth;
            productSlotWidthsModelView.unit = unit;

            ProductSlotWidths slotWidths = ProductSlotWidthsModelViewService.fromModelView(productSlotWidthsModelView);

            double expectedMinWidth = MeasurementUnitService.convertFromUnit(minWidth, unit);
            double expectedMaxWidth = MeasurementUnitService.convertFromUnit(maxWidth, unit);
            double expectedRecommendedWidth = MeasurementUnitService.convertFromUnit(recommendedWidth, unit);

            Assert.Equal(expectedMinWidth, slotWidths.minWidth);
            Assert.Equal(expectedMaxWidth, slotWidths.maxWidth);
            Assert.Equal(expectedRecommendedWidth, slotWidths.recommendedWidth);
        }

        [Fact]
        public void ensureFromEntityThrowsExceptionIfProductSlotWidthsIsNull()
        {
            Action fromEntity = () => ProductSlotWidthsModelViewService.fromEntity(null);

            Assert.Throws<ArgumentNullException>(fromEntity);
        }

        [Fact]
        public void ensureFromEntityCreatesModelViewWithExpectedData()
        {
            double minWidth = 25;
            double maxWidth = 50;
            double recommendedWidth = 35;

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(minWidth, maxWidth, recommendedWidth);

            GetProductSlotWidthsModelView result = ProductSlotWidthsModelViewService.fromEntity(slotWidths);

            GetProductSlotWidthsModelView expected = new GetProductSlotWidthsModelView();
            expected.minWidth = minWidth;
            expected.maxWidth = maxWidth;
            expected.recommendedWidth = recommendedWidth;
            expected.unit = MeasurementUnitService.getMinimumUnit();

            Assert.Equal(expected.minWidth, result.minWidth);
            Assert.Equal(expected.maxWidth, result.maxWidth);
            Assert.Equal(expected.recommendedWidth, result.recommendedWidth);
            Assert.Equal(expected.unit, result.unit);
        }

        [Fact]
        public void ensureFromEntityWithNullUnitUsesMinimumUnit()
        {
            double minWidth = 25;
            double maxWidth = 50;
            double recommendedWidth = 35;

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(minWidth, maxWidth, recommendedWidth);

            GetProductSlotWidthsModelView result = ProductSlotWidthsModelViewService.fromEntity(slotWidths, null);

            string expectedUnit = MeasurementUnitService.getMinimumUnit();

            Assert.Equal(expectedUnit, result.unit);
        }

        [Fact]
        public void ensureFromEntityWithEmptyUnitUsesMinimumUnit()
        {
            double minWidth = 25;
            double maxWidth = 50;
            double recommendedWidth = 35;

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(minWidth, maxWidth, recommendedWidth);

            GetProductSlotWidthsModelView result = ProductSlotWidthsModelViewService.fromEntity(slotWidths, "");

            string expectedUnit = MeasurementUnitService.getMinimumUnit();

            Assert.Equal(expectedUnit, result.unit);
        }

        [Fact]
        public void ensureFromEntityWithUnitConvertsValues()
        {
            double minWidth = 25;
            double maxWidth = 50;
            double recommendedWidth = 35;

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(minWidth, maxWidth, recommendedWidth);

            string unit = "dm";

            GetProductSlotWidthsModelView result = ProductSlotWidthsModelViewService.fromEntity(slotWidths, unit);

            GetProductSlotWidthsModelView expected = new GetProductSlotWidthsModelView();
            expected.minWidth = MeasurementUnitService.convertToUnit(minWidth, unit);
            expected.maxWidth = MeasurementUnitService.convertToUnit(maxWidth, unit);
            expected.recommendedWidth = MeasurementUnitService.convertToUnit(recommendedWidth, unit);
            expected.unit = unit;

            Assert.Equal(expected.minWidth, result.minWidth);
            Assert.Equal(expected.maxWidth, result.maxWidth);
            Assert.Equal(expected.recommendedWidth, result.recommendedWidth);
            Assert.Equal(expected.unit, result.unit);
        }
    }
}