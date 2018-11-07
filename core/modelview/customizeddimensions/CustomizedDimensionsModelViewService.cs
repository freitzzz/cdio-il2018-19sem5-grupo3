using core.domain;
using core.services;

namespace core.modelview.customizeddimensions
{
    /// <summary>
    /// Class representing a service used for converting CustomizedDimensions into ModelViews and vice-versa.
    /// </summary>
    public static class CustomizedDimensionsModelViewService
    {

        //TODO: handle unit conversion

        /// <summary>
        /// Converts an instance of AddCustomizedDimensionsModelView into an instance of CustomizedDimensions.
        /// </summary>
        /// <param name="modelView">Instance of AddCustomizedDimensionsModelView.</param>
        /// <returns>The created instance of CustomizedDimensions</returns>
        public static CustomizedDimensions fromModelView(AddCustomizedDimensionsModelView modelView)
        {
            double height = MeasurementUnitService.convertFromUnit(modelView.height, modelView.unit);
            double width = MeasurementUnitService.convertFromUnit(modelView.width, modelView.unit);
            double depth = MeasurementUnitService.convertFromUnit(modelView.depth, modelView.unit);

            return CustomizedDimensions.valueOf(height, width, depth);
        }

        /// <summary>
        /// Converts an instance of CustomizedDimensions into an instance of GetCustomizedModelView.
        /// </summary>
        /// <param name="customizedDimensions">Instance of CustomizedDimensions.</param>
        /// <returns>The created instance of GetCustomizedDimensionsModelView.</returns>
        public static GetCustomizedDimensionsModelView fromEntity(CustomizedDimensions customizedDimensions)
        {
            GetCustomizedDimensionsModelView modelView = new GetCustomizedDimensionsModelView();
            modelView.customizedDimensionsId = customizedDimensions.Id;
            modelView.height = customizedDimensions.height;
            modelView.width = customizedDimensions.width;
            modelView.depth = customizedDimensions.depth;

            return modelView;
        }
    }
}