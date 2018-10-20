// Vertex shader program
// Generates Vertexes
var VSHADER_SOURCE =
  'attribute vec4 a_Position;\n' +
  'attribute vec4 a_Color;\n' +
  'uniform mat4 u_MvpMatrix;\n' +
  'varying vec4 v_Color;\n' +
  'void main() {\n' +
  '  gl_Position = u_MvpMatrix * a_Position;\n' +
  '  v_Color = a_Color;\n' +
  '}\n';

// Fragment shader program
// Gives colors to pixels
var FSHADER_SOURCE =
  '#ifdef GL_ES\n' +
  'precision mediump float;\n' +
  '#endif\n' +
  'varying vec4 v_Color;\n' +
  'void main() {\n' +
  '  gl_FragColor = v_Color;\n' +
  '}\n';

  var scaleX=1;
  var scaleY=1;
  var scaleZ=1;
/**
 * 
 * Graphic Representation of a Customized Product main function 
 */
function main(){
    // Retrieve <canvas> element from HTML document
    var canvas = document.getElementById('webgl');
    // Get the rendering context for WebGL
    var webGL = getWebGLContext(canvas);
    
    if(!webGL){
        //Send event to user if WebGL is not available on the browser
        console.log('Failed to get the rendering context for WebGL');
        return;
    }

    // Initialize shaders
    if(!initShaders(webGL, VSHADER_SOURCE, FSHADER_SOURCE)){
        //Send event to user if shaders couldn't be initialized
        console.log('Failed to intialize shaders.');
        return;
    }

    //Set the vertex information
    var n = initVertexBuffers(webGL);
    if (n < 0) {
        //Send event to user if the 3D wireframed module couldn't be initialized
        console.log('Failed to set the vertex information');
        return;
    }

    // Set the clear color and enable the depth test
    webGL.clearColor(0.0, 0.0, 0.0, 1.0); //# 00 00 00 BLACK
    webGL.enable(webGL.DEPTH_TEST)

    // Get the storage location of u_MvpMatrix
    var u_MvpMatrix = webGL.getUniformLocation(webGL.program, 'u_MvpMatrix');
    if (!u_MvpMatrix) {
        console.log('Failed to get the storage location of u_MvpMatrix');
        return;
    }

    // Set the eye point and the viewing volume
    var mvpMatrix = new Matrix4();

    //Initial Product perspective
    mvpMatrix.setPerspective(30, 1, 1, 100);

    //Initial Product view
    mvpMatrix.lookAt(3, 0, 10, 0, 0, 0, 0, 1, 0);

    // Pass the model view projection matrix to u_MvpMatrix
    webGL.uniformMatrix4fv(u_MvpMatrix, false, mvpMatrix.elements);

    // Clear color and depth buffer
    webGL.clear(webGL.COLOR_BUFFER_BIT | webGL.DEPTH_BUFFER_BIT);
    
    // Draw the cube
    webGL.drawElements(webGL.LINE_STRIP, n, webGL.UNSIGNED_BYTE, 0); //Desenha o cubo com linhas (wireframe) 

    document.onkeydown=function(keyDownEvent){onKeyDown(keyDownEvent,webGL,n,mvpMatrix,u_MvpMatrix);};
}

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
  
    var y1=1;
    var y2=1;
  
    var z1=1;
    var z2=1;
  
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
      12,13,14,  15,16,13,    // left
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

/**
 * Function which executes if a keyboard event is triggered
 * @param {*} keyEvent Keyboard Event
 * @param {*} webGL Current WebGL context
 * @param {*} productView Current productv view
 * @param {*} u_MvpMatrix Current model view projection product matrix
 */
function onKeyDown(keyEvent,webGL,n,productViewMatrix,u_MvpMatrix){
  switch(keyEvent.keyCode){
    case 37: //Left
      scaleX-=0.2;
      break;
    case 38: //Up
    scaleY+=0.2;
      break;
    case 39: //Right
      scaleX+=0.2;
      break;
    case 40: //Down
      scaleY-=0.2;
      break;
    default:
      break;
  }
  drawScene(webGL,n,u_MvpMatrix,productViewMatrix);
}

function drawScene(webGL,vertexes,u_MvpMatrix,mvpMatrix){
  //Clear color and depth buffer
  webGL.clear(webGL.COLOR_BUFFER_BIT | webGL.DEPTH_BUFFER_BIT);

  // Set the clear color and enable the depth test
  webGL.clearColor(0.0, 0.0, 0.0, 1.0); //# 00 00 00 BLACK
  webGL.enable(webGL.DEPTH_TEST)

  //Initial Product perspective
  mvpMatrix.setPerspective(30, 1, 1, 100);

  //Initial Product view
  mvpMatrix.lookAt(3, 0, 10, 0, 0, 0, 0, 1, 0);

  mvpMatrix.scale(scaleX,scaleY,scaleZ);
  
  // Pass the model, view and projection matrix to u_MvpMatrix
  webGL.uniformMatrix4fv(u_MvpMatrix, false, mvpMatrix.elements);

  // Draw the cube
  webGL.drawElements(webGL.LINE_STRIP, vertexes, webGL.UNSIGNED_BYTE, 0); //Desenha o cubo com linhas (wireframe) 

}