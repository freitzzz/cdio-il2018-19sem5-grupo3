/**
 * Converts degrees in radians
 * @param {number} degrees Number with the float point degrees number being converted to radians
 */
function degreesToRadians(degrees) {
    return degrees * Math.PI / 180.0;
}

/**
 * Converts radians in degrees
 * @param {number} radians Number with the float point radians number being converted to degrees
 */
function radiansToDegrees(radians) {
    return radians * 180.0 / Math.PI;
}

/**
 * Computes the size which an object is from the camera/eye using the fov and distance
 * @param {number} fov Number with the float point number of the fov
 * @param {number} distance Number with the float point number of the distance
 */
function size(fov, distance) {
    return 2.0 * Math.tan(degreesToRadians(fov / 2.0)) * distance;
}

/**
 * Computes the fov which an object is from the camera/eye using the size and distance
 * @param {number} size Number with the float point number of the size
 * @param {number} distance Number with the float point number of the distance
 */
function fov(size, distance) {
    return 2.0 * radiansToDegrees(Math.atan2(size / 2.0, distance));
}

/**
 * Computes the distance which an object is from the camera/eye using the size and fov
 * @param {number} size Number with the float point number of the size 
 * @param {number} fov Number with the float point number of the fov
 */
function distance(size, fov) {
    return size / 2.0 / Math.tan(degreesToRadians(fov / 2.0));
}