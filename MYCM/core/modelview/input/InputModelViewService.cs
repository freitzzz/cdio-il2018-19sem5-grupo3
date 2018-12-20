using System;
using System.Collections.Generic;
using core.domain;

namespace core.modelview.input
{
    /// <summary>
    /// Class representing the service used for converting instances of Input into ModelView.
    /// </summary>
    public static class InputModelViewService
    {
        /// <summary>
        /// Constant representing the message presented when the provided instance of Input is null.
        /// </summary>
        private const string INPUT_NULL = "Unable to convert the provided input into a view.";

        /// <summary>
        /// Constant representing the message presented when the provided IEnumerable of Input is null.
        /// </summary>
        private const string INPUT_COLLECTION_NULL = "Unable to convert the provided inputs into views.";

        /// <summary>
        /// Converts an instance of Input into an instance of GetInputModelView.
        /// </summary>
        /// <param name="input">Instance of Input being converted.</param>
        /// <returns>An instance of GetInputModelView representing the provided Input.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of Input is null.</exception>
        public static GetInputModelView fromEntity(Input input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(INPUT_NULL);
            }

            GetInputModelView inputModelView = new GetInputModelView();
            inputModelView.name = input.name;
            inputModelView.range = input.range;

            return inputModelView;
        }

        /// <summary>
        /// Converts an IEnumerable of Input into an instance of GetAllInputsModelView.
        /// </summary>
        /// <param name="inputs">IEnumerable of Input being converted.</param>
        /// <returns>An instance of GetAllInputsModelView representing the provided IEnumerable of Input.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided IEnumerable of Input is null.</exception>
        public static GetAllInputsModelView fromCollection(IEnumerable<Input> inputs)
        {
            if (inputs == null)
            {
                throw new ArgumentNullException(INPUT_COLLECTION_NULL);
            }

            GetAllInputsModelView allInputsModelView = new GetAllInputsModelView();

            foreach (Input input in inputs)
            {
                allInputsModelView.Add(fromEntity(input));
            }

            return allInputsModelView;
        }
    }
}