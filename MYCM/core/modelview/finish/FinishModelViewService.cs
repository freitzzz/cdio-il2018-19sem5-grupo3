using System;
using core.domain;

namespace core.modelview.finish
{
    public static class FinishModelViewService
    {
        /// <summary>
        /// Constant representing the message presented when instance of Finish is null.
        /// </summary>
        private const string ERROR_NULL_FINISH = "The provided finish is invalid.";

        /// <summary>
        /// Converts an instance of Finish into an instance of GetFinishModelView.
        /// </summary>
        /// <param name="finish">Instance of Finish being converted.</param>
        /// <returns>An instance of GetFinishModelView.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of Finish is null.</exception>
        public static GetFinishModelView fromEntity(Finish finish)
        {
            if (finish == null)
            {
                throw new ArgumentNullException(ERROR_NULL_FINISH);
            }

            GetFinishModelView finishModelView = new GetFinishModelView();
            finishModelView.finishId = finish.Id;
            finishModelView.description = finish.description;
            finishModelView.shininess = finish.shininess;

            return finishModelView;
        }
    }
}