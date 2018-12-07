using core.domain;
using core.modelview.color;
using core.modelview.finish;

namespace core.modelview.customizedmaterial
{
    public static class CustomizedMaterialModelViewService
    {
        /// <summary>
        /// Constant that represents the message presented when the provided 
        /// </summary>
        private const string ERROR_NULL_CUSTOMIZED_MATERIAL = "The provided customized material is invalid";

        /// <summary>
        /// Converts an instance of CustomizedMaterial into an instance of GetCustomizedMaterialModelView.
        /// </summary>
        /// <param name="customizedMaterial">Instance of CustomizedMaterial being converted.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of CustomizedMaterial is null.</exception>
        public static GetCustomizedMaterialModelView fromEntity(CustomizedMaterial customizedMaterial)
        {
            if (customizedMaterial == null)
            {
                throw new System.ArgumentNullException(nameof(customizedMaterial));
            }

            GetCustomizedMaterialModelView customizedMaterialModelView = new GetCustomizedMaterialModelView();
            customizedMaterialModelView.customizedMaterialId = customizedMaterial.Id;
            customizedMaterialModelView.materialId = customizedMaterial.material.Id;
            customizedMaterialModelView.finish = FinishModelViewService.fromEntity(customizedMaterial.finish);
            customizedMaterialModelView.color = ColorModelViewService.fromEntity(customizedMaterial.color);

            return customizedMaterialModelView;
        }
    }
}