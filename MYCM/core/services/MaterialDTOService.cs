using core.domain;
using core.dto;
using core.persistence;
using support.dto;
using System.Collections.Generic;
using System;
namespace core.services
{
    /// <summary>
    /// Service class that helps the transformation of MaterialDTO into Material since some information needs to be accessed on the persistence context
    /// </summary>

    public class MaterialDTOService
    {
        /// <summary>
        /// Transforms a material dto into a material via service
        /// </summary>
        /// <param name="materialDTO">MaterialDTO with the material dto being transformed</param>
        /// <returns>Material with the material transformed from the dto</returns>
        public Material transform(MaterialDTO materialDTO)
        {
            string reference = materialDTO.reference;
            string designation = materialDTO.designation;
            string image = materialDTO.image;
            List<Color> listColor = new List<Color>();
            List<Finish> listFinish = new List<Finish>();
            if (materialDTO.colors != null)
            {
                foreach (ColorDTO color in materialDTO.colors)
                {
                    listColor.Add(color.toEntity());
                }
            }
            if (materialDTO.finishes != null)
            {
                foreach (FinishDTO finish in materialDTO.finishes)
                {
                    listFinish.Add(finish.toEntity());
                }
            }
            return new Material(reference, designation, image, listColor, listFinish);
        }
    }
}