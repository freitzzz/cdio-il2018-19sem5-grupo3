using System;
using System.Collections.Generic;
using support.domain.ddd;
namespace core.domain
{
    /**
    <summary>
        Class that represents a Customized Dimensions.
        <br> Customized Dimensions is value object;
    </summary>
    */
    public class CustomizedDimensions : ValueObject
    {
        /**
        <summary>
            Constant that represents the message that ocurrs if the customizedDimensions's height are not valid.
        </summary>
         */
        private const string INVALID_CUSTOMIZED_DIMENSIONS_HEIGHT = "The CustomizedDimensions's height are not valid!";
        /**
        <summary>
            Constant that represents the message that ocurrs if the CustomizedDimensions's width are not valid.
        </summary>
         */
        private const string INVALID_CUSTOMIZED_DIMENSIONS_WIDTH = "The CustomizedDimensions's width are not valid!";
        /**
        <summary>
            Constant that represents the message that ocurrs if the CustomizedDimensions's depth are not valid.
        </summary>
         */
        private const string INVALID_CUSTOMIZED_DIMENSIONS_DEPTH = "The CustomizedDimensions's depth are not valid!";
        /**
        <summary>
            The CustomizedDimensions's height.
        </summary>
         */
        private readonly double height;
        /**
        <summary>
            The CustomizedDimensions's width.
        </summary>
         */
        private readonly double width;
        /**
        <summary>
            The CustomizedDimensions's depth.
        </summary>
         */
        private readonly double depth;
        /**
        <summary>
            Builds a new instance of CustomizedDimensions, receiving its height, width and depth.
        </summary>
        <param name = "height">double with the new CustomizedDimensions's height</param>
        <param name = "width">double with the new CustomizedDimensions's width</param>
        <param name = "depth">double with the new CustomizedDimensions's depth</param>
         */
        public static CustomizedDimensions valueOf(double height, double width, double depth)
        {
            return new CustomizedDimensions(height, width, depth);
        }
        /**
        <summary>
            Builds a new instance of CustomizedDimensions, receiving its height, width and depth.
        </summary>
        <param name = "height">double with the new CustomizedDimensions's height</param>
        <param name = "width">double with the new CustomizedDimensions's width</param>
        <param name = "depth">double with the new CustomizedDimensions's depth</param>
         */
        private CustomizedDimensions(double height, double width, double depth)
        {
            checkCustomizedDimensions(height,width,depth);
            this.height = height;
            this.width = width;
            this.depth = depth;
        }
        /**
        <summary>
            Checks if the CustomizedDimensions's height, width and  depth are valid.
        </summary>
        <param name = "height">double with the new CustomizedDimensions's height</param>
        <param name = "width">double with the new CustomizedDimensions's width</param>
        <param name = "depth">double with the new CustomizedDimensions's depth</param>
        */
        private void checkCustomizedDimensions(double height, double width, double depth)
        {
            if (Double.IsNaN(height)) throw new ArgumentException(INVALID_CUSTOMIZED_DIMENSIONS_HEIGHT);
            if (Double.IsNaN(width)) throw new ArgumentException(INVALID_CUSTOMIZED_DIMENSIONS_WIDTH);
            if (Double.IsNaN(depth)) throw new ArgumentException(INVALID_CUSTOMIZED_DIMENSIONS_DEPTH);
        }
        /**
        <summary>
            Returns a textual with the height, width and depth of the Customized Dimensions.
        </summary>
         */
        public override string ToString()
        {
            return string.Format("Height: {0}, Width {1}, Depth {2}", height, width,depth);
        }
        /**
        <summary>
            Returns the generated hash code of the Customized Dimensions.
        </summary>
         */
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = (hashCode * 23) + this.height.GetHashCode();
            hashCode = (hashCode * 23) + this.width.GetHashCode();
            hashCode = (hashCode * 23) + this.depth.GetHashCode();
            return hashCode.GetHashCode();
        }
        /**
        <summary>
            Checks if a certain Customized Dimensions is the same as a received object.
        </summary>
        <param name = "obj">object to compare to the current Customized Dimensions</param>
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
                CustomizedDimensions custDimensions = (CustomizedDimensions)obj;
                return height.Equals(custDimensions.height) && 
                        width.Equals(custDimensions.width) && 
                        depth.Equals(custDimensions.depth);
            }
        }

        
    }
}