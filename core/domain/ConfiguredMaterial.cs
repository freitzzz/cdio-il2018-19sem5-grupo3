using System;
using System.Collections.Generic;
using support.domain.ddd;
namespace core.domain
{
    /**
    <summary>
        Class that represents a Configured Material.
        <br>Configured Material is value object;
    </summary>
    */
    public class ConfiguredMaterial : ValueObject
    {
        /**
        <summary>
            Constant that represents the message that ocurrs if the ConfiguredMaterial's color are not valid.
        </summary>
         */
        private const string INVALID_CONFIGURED_MATERIAL_COLOR = "The ConfiguredMaterial's color are not valid!";

        /**
        <summary>
            Constant that represents the message that ocurrs if the ConfiguredMaterial's finish are not valid.
        </summary>
         */
        private const string INVALID_CONFIGURED_MATERIAL_FINISH = "The ConfiguredMaterial's finish are not valid!";
        /**
        <summary>
            The ConfiguredMaterial's color.
        </summary>
         */
        private readonly Color color;

        /**
         <summary>
             The ConfiguredMaterial's finishe.
         </summary>
          */
        private readonly Finish finish;
        /**
        <summary>
            Builds a new instance of ConfiguredMaterial, receiving its color and finish.
        </summary>
        <param name = "color">string with the new ConfiguredMaterial's color</param>
        <param name = "finish">string with the new ConfiguredMaterial's finish</param>
         */
        public static ConfiguredMaterial valueOf(Color color, Finish finish)
        {
            return new ConfiguredMaterial(color, finish);
        }
        /**
        <summary>
            Builds a new instance of ConfiguredMaterial, receiving its color.
        </summary>
        <param name = "color">string with the new ConfiguredMaterial's color</param>
         */
        public static ConfiguredMaterial valueOf(Color color)
        {
            return new ConfiguredMaterial(color);
        }
        /**
        <summary>
            Builds a new instance of ConfiguredMaterial, receiving its finish.
        </summary>
        <param name = "finish">string with the new ConfiguredMaterial's finish</param>
         */
        public static ConfiguredMaterial valueOf(Finish finish)
        {
            return new ConfiguredMaterial(finish);
        }
        /**
        <summary>
            Builds a new instance of ConfiguredMaterial, receiving its color and finish.
        </summary>
        <param name = "color">The new ConfiguredMaterial's color</param>
        <param name = "finish">The new ConfiguredMaterial's finishe</param>
         */
        private ConfiguredMaterial(Color color, Finish finish)
        {
            checkConfiguredMaterialColor(color);
            checkConfiguredMaterialFinish(finish);
            this.color = color;
            this.finish = finish;
        }
        /**
       <summary>
           Builds a new instance of ConfiguredMaterial, receiving its color.
       </summary>
       <param name = "color">The new ConfiguredMaterial's color</param>
        */
        private ConfiguredMaterial(Color color)
        {
            checkConfiguredMaterialColor(color);
            this.color = color;
            this.finish = null;
        }
        /**
        <summary>
            Builds a new instance of ConfiguredMaterial, receiving its finish.
        </summary>
        <param name = "finish">The new ConfiguredMaterial's finishe</param>
         */
        private ConfiguredMaterial(Finish finish)
        {
            checkConfiguredMaterialFinish(finish);
            this.finish = finish;
            this.color =  null;
        }
        /**
        <summary>
            Checks if the ConfiguredMaterial's color is valid.
        </summary>
        <param name = "color">The ConfiguredMaterial's color</param>
        */
        private void checkConfiguredMaterialColor(Color color)
        {
            if (String.IsNullOrEmpty(color.ToString())) throw new ArgumentException(INVALID_CONFIGURED_MATERIAL_COLOR);
        }
        /**
       <summary>
           Checks if the ConfiguredMaterial's finish is valid.
       </summary>
       <param name = "finish">The ConfiguredMaterial's finish</param>
       */
        private void checkConfiguredMaterialFinish(Finish finish)
        {
            if (String.IsNullOrEmpty(finish.ToString())) throw new ArgumentException(INVALID_CONFIGURED_MATERIAL_FINISH);
        }
        /**
        <summary>
            Returns a textual with the color and finish of the Configured Material.
        </summary>
         */
        public override string ToString()
        {
            return string.Format("Color: {0}, Finish {1}", color, finish);
        }

        /**
        <summary>
            Returns the generated hash code of the Configured Material.
        </summary>
         */
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = (hashCode * 23) + this.color.GetHashCode();
            hashCode = (hashCode * 23) + this.finish.GetHashCode();

            return hashCode.GetHashCode();
        }

        /**
        <summary>
            Checks if a certain Configured Material is the same as a received object.
        </summary>
        <param name = "obj">object to compare to the current Configured Material</param>
         */
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ConfiguredMaterial configMaterial = (ConfiguredMaterial)obj;
                return finish.Equals(configMaterial.finish) && color.Equals(configMaterial.color);
            }
        }

    }
}