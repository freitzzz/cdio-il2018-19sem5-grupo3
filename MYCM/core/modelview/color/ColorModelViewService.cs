using core.domain;

namespace core.modelview.color
{
    public static class ColorModelViewService
    {
        /// <summary>
        /// Constant that represents the message presented when the provided instance of Color is null.
        /// </summary>
        private const string ERROR_NULL_COLOR = "The provided color is invalid";

        /// <summary>
        /// Converts an instance of Color into an instance of GetColorModelView.
        /// </summary>
        /// <param name="color">Instance of Color being converted.</param>
        /// <returns>An instance of GetColorModelView.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of Color is null.</exception>
        public static GetColorModelView fromEntity(Color color)
        {
            if (color == null)
            {
                throw new System.ArgumentNullException(ERROR_NULL_COLOR);
            }

            GetColorModelView colorModelView = new GetColorModelView();
            colorModelView.colorId = color.Id;
            colorModelView.name = color.Name;
            colorModelView.red = color.Red;
            colorModelView.green = color.Green;
            colorModelView.blue = color.Blue;
            colorModelView.alpha = color.Alpha;

            return colorModelView;
        }
    }
}