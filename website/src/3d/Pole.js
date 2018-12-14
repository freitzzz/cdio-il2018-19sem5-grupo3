//@ts-check
/**
* Represents a pole by using cylinder geometry
*/
export default class Pole{

	/**
     * Builds a new Pole 
     * 
     * @param {Number} radiusTop Radius of the pole at the top
	 * @param {Number} radiusBottom Radius of the pole at the bottom
	 * @param {Number} height Height of the pole
	 * @param {Number} radialSegments Number of segmented faces around the circumference of the pole 
	 * @param {Number} heightSegments Number of rows of faces along the height of the pole
	 * @param {Boolean} openEnded Indicates whether the ends of the pole are open or capped
	 * @param {Number} thetaStart Starting angle for the pole's first segment
	 * @param {Number} thetaLength Central angle of the pole's circular sector
     */
	constructor(radiusTop,radiusBottom,
				height,radialSegments,
				heightSegments,openEnded,
				thetaStart,thetaLength, slotId){
		this.radiusTop = radiusTop;
		this.radiusBottom = radiusBottom;
		this.height = height;
		this.radialSegments = radialSegments;
		this.heightSegments = heightSegments;
		this.openEnded = openEnded;
		this.thetaStart = thetaStart;
		this.thetaLength = thetaLength;
		this.slotId = slotId;
	}
	
	/**
	* Returns the current top radius of the pole
	*/
	getPoleTopRadius(){return this.radiusTop;}
	
	/**
	* Returns the current bottom radius of the pole
	*/
	getPoleBottomRadius(){return this.radiusBottom;}
	
	/**
	* Returns the current height of the pole
	*/
	getPoleHeight(){return this.height}
	
	/**
	* Changes the Pole's radius
	*/
	changePoleRadius(radiusTop, radiusBottom){
		this.radiusTop = radiusTop;
		this.radiusBottom = radiusBottom;
	}
	
	/**
	* Changes the Pole's height
	*/
	changePoleHeight(height){
		this.height = height;
	}
	
	/**
	* Changes the Pole's number of radial segments
	*/
	changeRadialSegments(radialSegments){
		this.radialSegments = radialSegments;
	}
	
	/**
	* Changes the Pole's number of height segments
	*/
	changeHeightSegments(heightSegments){
		this.heightSegments = heightSegments;
	}
}