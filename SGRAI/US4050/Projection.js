  // Specify the current projection type, the left, right, bottom and top clipping planes (when applicable), the field of view (when applicable); and the front and back clipping planes
  // Orthographic projection: the zoom effect is achieved by changing the values of the left, right, bottom and top clipping planes
  // Perspective projection: the zoom effect is achieved by changing either the field of view or the distance between the camera and the target
  // The field of view is expressed in degrees
  var projection = {type: 'ortho',
                    size: 10.0,
                    sizeMin: 4.0,
                    sizeMax: 16.0,
                    fovMin: 30.0,
                    fovMax: 60.0,
                    near: 0.1,
                    far: 30.0};