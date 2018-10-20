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

/**
 * Array which keeps all product matrixes
 */
var matrixes=[];

var u_MvpMatrix=null;

/**
 * Current Product WebGL context
 */
var webGL=null;

var indices=0;

/**
 * Current Product scales
 */
var scales=[1,1,1];  

var mouseX,mouseY;

function degreesToRadians(degrees) {
    return degrees * Math.PI / 180.0;
}

function radiansToDegrees(radians) {
    return radians * 180.0 / Math.PI;
}

function size(fov, distance) {
    return 2.0 * Math.tan(degreesToRadians(fov / 2.0)) * distance;
}

function fov(size, distance) {
    return 2.0 * radiansToDegrees(Math.atan2(size / 2.0, distance));
}

function distance(size, fov) {
    return size / 2.0 / Math.tan(degreesToRadians(fov / 2.0));
}


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

// Specify the camera distance and orientation
  // The horizontal and vertical orientation are expressed in degrees
  var camera = {target: [0.0, 0.0, 0.0],
    distance: 15.0,
    distanceMin: distance(projection.sizeMin, projection.fovMin),
    distanceMax: distance(projection.sizeMax, projection.fovMax),
    orientation: [45.0, 45.0]};




/**
 * Graphic Representation of a Customized Product main function 
 */
function main(){

  // Retrieve <canvas> element from HTML document
  var canvas = document.getElementById('webgl');
  // Get the rendering context for WebGL
  webGL = getWebGLContext(canvas);
  
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
u_MvpMatrix = webGL.getUniformLocation(webGL.program, 'u_MvpMatrix');
  if (!u_MvpMatrix) {
      console.log('Failed to get the storage location of u_MvpMatrix');
      return;
  }

  // Set the eye point and the viewing volume
  var mvpMatrix = new Matrix4();

  matrixes.push(mvpMatrix); //Store the current product matrix
  
  drawScene(webGL,n,u_MvpMatrix,mvpMatrix); //Draws initial scene

  indices=n;

  document.onkeydown=function(keyDownEvent){onKeyDown(keyDownEvent,webGL,n,mvpMatrix,u_MvpMatrix);};
  canvas.onmousedown=function(ev){mouseDown(ev);}
  canvas.onmousemove=function(ev){mouseMove(ev);}

  
}

/**
 * Function which executes if a keyboard event is triggered
 * @param {event} keyEvent Keyboard Event
 * @param {WebGL} webGL Current WebGL context
 * @param {number} indices Current productv indices number
 * @param {Matrix4} productView Current product view
 * @param {u_MvpMatrix} u_MvpMatrix Current model view projection product matrix
 */
function onKeyDown(keyEvent,webGL,indices,productViewMatrix,u_MvpMatrix){
    switch(keyEvent.keyCode){
      case 37: //Left
        scales[0]-=0.2;
        break;
      case 38: //Up
        scales[1]+=0.2;
        break;
      case 39: //Right
        scales[0]+=0.2;
        break;
      case 40: //Down
        scales[1]-=0.2;
        break;
      default:
        break;
    }
    drawScene(webGL,indices,u_MvpMatrix,productViewMatrix);
}

/**
 * Draws the product scene
 * @param {WebGL} webGL Current WebGL context 
 * @param {number} indices Object number of indices
 * @param {u_MvpMatrix} u_MvpMatrix Object u_MvpMatrix
 * @param {Matrix4} mvpMatrix Object Matrix
 */
function drawScene(webGL,indices,u_MvpMatrix,mvpMatrix){
    //Clear color and depth buffer
    webGL.clear(webGL.COLOR_BUFFER_BIT | webGL.DEPTH_BUFFER_BIT);
  
    // Set the clear color and enable the depth test
    webGL.clearColor(0.0, 0.0, 0.0, 1.0); //# 00 00 00 BLACK
    webGL.enable(webGL.DEPTH_TEST)
  

    //Initial Product perspective
    mvpMatrix.setPerspective(30, 1, 1, 100);

    
    // Specify the viewing transformation (positive Z-semi-axis up)
    //   Z
    //   |
    //   O
    //  / \
    // X   Y
    // The viewing transformation is usually defined by calling mvpMatrix.lookAt()
    // Target position
    var targetX = camera.target[0];
    var targetY = camera.target[1];
    var targetZ = camera.target[2];
    // Camera position
    var cameraX = camera.distance * Math.cos(degreesToRadians(camera.orientation[0])) * Math.cos(degreesToRadians(camera.orientation[1])) + camera.target[0];
    var cameraY = camera.distance * Math.sin(degreesToRadians(camera.orientation[0])) * Math.cos(degreesToRadians(camera.orientation[1])) + camera.target[1];
    var cameraZ = camera.distance * Math.sin(degreesToRadians(camera.orientation[1])) + camera.target[2];
    // Up vector
    var upX = Math.cos(degreesToRadians(camera.orientation[0])) * Math.cos(degreesToRadians(camera.orientation[1] + 90.0));
    var upY = Math.sin(degreesToRadians(camera.orientation[0])) * Math.cos(degreesToRadians(camera.orientation[1] + 90.0));
    var upZ = Math.sin(degreesToRadians(camera.orientation[1] + 90.0));
    mvpMatrix.lookAt(cameraX, cameraY, cameraZ, targetX, targetY, targetZ, upX, upY, upZ);






    
  
    //Initial Product view
    //mvpMatrix.lookAt(3, 0, 10, 0, 0, 0, 0, 1, 0);
  
    mvpMatrix.scale(scales[0],scales[1],scales[2]);
    
    // Pass the model, view and projection matrix to u_MvpMatrix
    webGL.uniformMatrix4fv(u_MvpMatrix, false, mvpMatrix.elements);
  
    // Draw the cube
    webGL.drawElements(webGL.LINE_STRIP, indices, webGL.UNSIGNED_BYTE, 0); //Desenha o cubo com linhas (wireframe)

}

function mouseDown(ev) {
    if (ev.buttons == 1) { // Left button
      mouseX = ev.clientX; // Mouse X coordinate
      mouseY = ev.clientY; // Mouse Y coordinate
    }
  }

function mouseMove(ev) {
    if (ev.buttons == 1) { // Left button
      var deltaX = ev.clientX - mouseX;
      var deltaY = ev.clientY - mouseY;
      if (deltaX != 0) {
        camera.orientation[0] -= 0.5 * deltaX; // Horizontal orientation: -0.5 or +0.5 degree/horizontal pixel
        while (camera.orientation[0] < -180.0) {
          camera.orientation[0] += 360.0;
        }
        while (camera.orientation[0] >= 180.0) {
          camera.orientation[0] -= 360.0;
        }
      }
      if (deltaY != 0) {
        camera.orientation[1] += 0.5 * deltaY; // Vertical orientation: -0.5 or +0.5 degree/vertical pixel
        while (camera.orientation[1] < -180.0) {
          camera.orientation[1] += 360.0;
        }
        while (camera.orientation[1] >= 180.0) {
          camera.orientation[1] -= 360.0;
        }
      }
      mouseX = ev.clientX;
      mouseY = ev.clientY;
    }
    matrix=matrixes.pop();
    drawScene(webGL,indices,u_MvpMatrix,matrix);
    matrixes.push(matrix);
}