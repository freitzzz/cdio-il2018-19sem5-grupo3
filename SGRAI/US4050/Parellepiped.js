//WebGL Graphic Representation of a parellepiped
//Courtesy of { (c) 2012 matsuda }
//Inspired from WebGL Programming Guide Colored Cube example

/**
 * Initializes 3D wireframed module of a parellepiped
 * @param {WebGL Context} webGL Current Product WebGL Context
 */
function initVertexBuffers(webGL) {
    // Create a cube
    //    v6----- v5
    //   /|      /|
    //  v1------v0|
    //  | |     | |
    //  | |v7---|-|v4
    //  |/      |/
    //  v2------v3
  
    var x1=1;
    var x2=1;
  
    var y1=1;   //To be deprecated in the future
    var y2=1;
  
    var z1=1;
    var z2=1;
    
    // Cube
     var vertices = new Float32Array([   // Vertex coordinates
       x1, y1, z1,  -x2, y1, z1,  -x2,-y2, z1,   x1,-y2, z1,  // v0-v1-v2-v3 front
       x1, y1, z1,   x1,-y2, z1,   x1,-y2,-z2,   x1, y1,-z2,  // v0-v3-v4-v5 right
       x1, y1, z1,   x1, y1,-z2,  -x2, y1,-z2,  -x2, y1, z1,  // v0-v5-v6-v1 up
       -x2, y1, z1,  -x2, y1,-z2,  -x2,-y2,-z2,   -x2,-y2, z1,  // v1-v6-v7-v2 left
       -x2,-y2,-z2,   x1,-y2,-z2,    x1,-y2, z1,  -x2,-y2, z1,  // v7-v4-v3-v2 down
       x1,-y2,-z2,  -x2,-y2,-z2,  -x2, y1,-z2,   x1, y1,-z2   // v4-v7-v6-v5 back
    ]);

    //CORES VERTICES
    var colors = new Float32Array([     // Colors
      0.4, 0.4, 1.0,  0.4, 0.4, 1.0,  0.4, 0.4, 1.0,  0.4, 0.4, 1.0,  // v0-v1-v2-v3 front(blue)
      0.4, 1.0, 0.4,  0.4, 1.0, 0.4,  0.4, 1.0, 0.4,  0.4, 1.0, 0.4,  // v0-v3-v4-v5 right(green)
      1.0, 0.4, 0.4,  1.0, 0.4, 0.4,  1.0, 0.4, 0.4,  1.0, 0.4, 0.4,  // v0-v5-v6-v1 up(red)
      1.0, 1.0, 0.4,  1.0, 1.0, 0.4,  1.0, 1.0, 0.4,  1.0, 1.0, 0.4,  // v1-v6-v7-v2 left
      1.0, 1.0, 1.0,  1.0, 1.0, 1.0,  1.0, 1.0, 1.0,  1.0, 1.0, 1.0,  // v7-v4-v3-v2 down
      0.4, 1.0, 1.0,  0.4, 1.0, 1.0,  0.4, 1.0, 1.0,  0.4, 1.0, 1.0   // v4-v7-v6-v5 back
    ]);
    
      //ARESTAS
    var indices = new Uint8Array([       // Indices of the vertices
       0, 1, 2,   2, 3, 0,    // front
       4, 5, 6,   6, 7, 4,    // right
       8, 9,10,   10,11,8,    // up
      12,13,14,  15,16,12,    // left
      16,17,18,  18,19,16,    // down
      20,21,22,  22,23,20     // back
    ]);
  
    // Create a buffer object
    var indexBuffer = webGL.createBuffer();
    if (!indexBuffer) 
      return -1;
  
    // Write the vertex coordinates and color to the buffer object
    if (!initArrayBuffer(webGL, vertices, 3, webGL.FLOAT, 'a_Position'))
      return -1;
  
    if (!initArrayBuffer(webGL, colors, 3, webGL.FLOAT, 'a_Color'))
      return -1;
  
    // Write the indices to the buffer object
    webGL.bindBuffer(webGL.ELEMENT_ARRAY_BUFFER, indexBuffer);
    webGL.bufferData(webGL.ELEMENT_ARRAY_BUFFER, indices, webGL.STATIC_DRAW);
  
    return indices.length;
}

/**
 * Initializes the array buffer of a certain data
 * @param {WebGL Context} webGL Current Parellepiped WebGL context
 * @param {data} data Data being initialized 
 * @param {number} num Number of data axes
 * @param {type} type Type of data being used
 * @param {attribute} attribute Attribute being used for initialization of the data
 */
function initArrayBuffer(webGL, data, num, type, attribute) {
  var buffer = webGL.createBuffer();   // Create a buffer object
  if (!buffer) {
    console.log('Failed to create the buffer object');
    return false;
  }
  // Write date into the buffer object
  webGL.bindBuffer(webGL.ARRAY_BUFFER, buffer);
  webGL.bufferData(webGL.ARRAY_BUFFER, data, webGL.STATIC_DRAW);
  // Assign the buffer object to the attribute variable
  var a_attribute = webGL.getAttribLocation(webGL.program, attribute);
  if (a_attribute < 0) {
    console.log('Failed to get the storage location of ' + attribute);
    return false;
  }
  webGL.vertexAttribPointer(a_attribute, num, type, false, 0, 0);
  // Enable the assignment of the buffer object to the attribute variable
  webGL.enableVertexAttribArray(a_attribute);

  return true;
}