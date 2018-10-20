// Specify the camera distance and orientation
  // The horizontal and vertical orientation are expressed in degrees
  var camera = {target: [0.0, 0.0, 0.0],
    distance: 15.0,
    distanceMin: distance(projection.sizeMin, projection.fovMin),
    distanceMax: distance(projection.sizeMax, projection.fovMax),
    orientation: [45.0, 45.0]};