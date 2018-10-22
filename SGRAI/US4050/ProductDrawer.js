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
 * Current broswer canvas
 */
var canvas=null;

/**
 * Array which keeps all product matrixes
 */
var matrixes=[];

/**
 * Current u_MvpMatrix
 */
var u_MvpMatrix=null;

/**
 * Current Product WebGL context
 */
var webGL=null;

/**
 * Current product indices
 */
var indices=0;

/**
 * Current Product scales
 */
var scales=[1,1,1];  

/**
 * Current mouse positions relatively to X and Y axes
 */
var mouseX,mouseY;


/**
 * Graphic Representation of a Customized Product main function 
 */
function main(){

  // Retrieve <canvas> element from HTML document
  canvas = document.getElementById('webgl');
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
  
  indices=n;
  drawScene(); //Draws initial scene
  registerEvents();
}

/**
 * Function which executes if a keyboard event is triggered
 * @param {event} keyEvent Keyboard Event
 * @param {WebGL} webGL Current WebGL context
 * @param {number} indices Current productv indices number
 * @param {Matrix4} productView Current product view
 * @param {u_MvpMatrix} u_MvpMatrix Current model view projection product matrix
 */
function onKeyDown(keyEvent){
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
    drawScene();
}

/**
 * Treats mouse button clicked events
 * @param {MouseEvent} mouseEvent MouseEvent with the mouse clicked event
 */
function mouseDown(mouseEvent) {
    if (mouseEvent.buttons == 1) { // Left button
      mouseX = mouseEvent.clientX; // Mouse X coordinate
      mouseY = mouseEvent.clientY; // Mouse Y coordinate
    }
  }

/**
 * Treats mouse moved events
 * @param {MouseEvent} mouseEvent MouseEvent with the mouse moved event
 */
function mouseMove(mouseEvent) {
    if (mouseEvent.buttons == 1) { // Left button
      var deltaX = mouseEvent.clientX - mouseX;
      var deltaY = mouseEvent.clientY - mouseY;
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
      mouseX = mouseEvent.clientX;
      mouseY = mouseEvent.clientY;
    }
    drawScene();
}

/**
 * Treats mouse wheel events
 * @param {MouseEvent} mouseEvent MouseEvent with the mouse wheel event
 */
function mouseWheel(mouseEvent) {
  if (mouseEvent.deltaY < 0) { // Roll up
    camera.distance /= 1.1;
    if (camera.distance < camera.distanceMin) {
      camera.distance = camera.distanceMin;
    }
  } else if (mouseEvent.deltaY > 0) { // Roll down
    camera.distance *= 1.1;
    if (camera.distance > camera.distanceMax) {
      camera.distance = camera.distanceMax;
    }
  } else {
    return;
  }
  drawScene();
}

/**
 * Draws the product scene
 */
function drawScene(){
    var mvpMatrix=matrixes.pop();
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

    mvpMatrix.scale(scales[0],scales[1],scales[2]);
    
    // Pass the model, view and projection matrix to u_MvpMatrix
    webGL.uniformMatrix4fv(u_MvpMatrix, false, mvpMatrix.elements);
  
    // Draw the cube
    webGL.drawElements(webGL.LINE_STRIP, indices, webGL.UNSIGNED_BYTE, 0); //Desenha o cubo com linhas (wireframe)
    matrixes.push(mvpMatrix);
}

/**
 * Registers all events
 */
function registerEvents(){
  document.onkeydown=function(keyDownEvent){onKeyDown(keyDownEvent);}
  canvas.onmousedown=function(mouseEvent){mouseDown(mouseEvent);}
  canvas.onmousemove=function(mouseEvent){mouseMove(mouseEvent);}
  canvas.onwheel=function(mouseEvent){mouseWheel(mouseEvent);}
}