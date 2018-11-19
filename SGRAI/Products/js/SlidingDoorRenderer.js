/**
 * Global Variables for Graphic Control (Camera, Rendering, Scene, etc...)
 */
var camera, controls, scene, renderer, group;

/**
 * Global variable with the WebGL canvas
 */
var canvasWebGL;

/**
 * Global variable for the texture of the objects
 */
var textureLoader;

/**
 * Global variable for the Mesh Material
 */
var material;

/**
 * Global variable with the front door faces ids (Mesh IDs from Three.js)
 */
var front_door_faces_ids = [];

/**
 * Global variable with the front frame borders ids (Mesh IDs from Three.js)
 */
var front_frame_faces_ids = [];

/**
 * Global variable with the back door faces ids (Mesh IDs from Three.js)
 */
var back_door_faces_ids = [];

/**
 * Global variable with the back frame borders ids (Mesh IDs from Three.js)
 */
var back_frame_faces_ids = [];

// ------------ Global variables used to drag doors ------------
/**
 * Global variable with the currently selected door (null if none)
 */
var selected_door = null;

/**
 * Global variable that represents the object being hovered (null if none)
 */
var hovered_object = null;

/**
 * Global variable that represents the plane that intersects the closet
 */
var plane = null;

/**
 * Global variable that represents the difference between the intersection's x coordinate
 * and the selected object's x coordinate
 */
var offset;

/**
 * Global variable with a Vector that holds the mouse coordinates (x, y)
 */
var mouse = new THREE.Vector2();

/**
 * Global variable with a Vector that represents the intersection between the plane and
 * the clicked object
 */
var intersection = new THREE.Vector3(0, 0, 0);

/**
 * Global variable with a Raycaster used for picking (hovering, clicking and identifying) objects
 */
var raycaster = new THREE.Raycaster();
// ------------ End of global variables used to drag doors ------------

/**
 * Initial Product Draw function
 */
function main(textureSource) {
    canvasWebGL = document.getElementById("webgl");
    renderer = new THREE.WebGLRenderer({ canvas: canvasWebGL, antialias: true });

    initCamera();
    initControls();
    initSlidingDoor(textureSource);
    initLighting();

    //Creates the intersection plane
    plane = new THREE.Plane();
    plane.setFromNormalAndCoplanarPoint(new THREE.Vector3(0, 0, 1), new THREE.Vector3(0, 200, 0)).normalize();

    var planeGeometry = new THREE.PlaneGeometry(500, 500);
    var coplanarPoint = plane.coplanarPoint();
    var focalPoint = new THREE.Vector3().copy(coplanarPoint).add(plane.normal);

    planeGeometry.lookAt(focalPoint);
    planeGeometry.translate(coplanarPoint.x, coplanarPoint.y, coplanarPoint.z);

    var planeMaterial = new THREE.MeshLambertMaterial({
        color: 0xffff00,
        side: THREE.DoubleSide
    });

    var dispPlane = new THREE.Mesh(planeGeometry, planeMaterial);
    dispPlane.visible = false;
    //Finishes creating the intersection plane

    scene.add(dispPlane);
    scene.add(camera);

    animate();
}

/**
 * Initiates the sliding door
 * @param {*} textureSource - Source of the texture being loaded.
 */
function initSlidingDoor(textureSource) {
    scene = new THREE.Scene();
    group = new THREE.Group();

    var front_door = new SlidingDoor([150, 187, 5, -30, 27.5, 0]);

    var front_frame = new Module([224.5, 4.20, 5, 0, -70, 0],
        [224.5, 4.20, 5, 0, 125, 0],
        [4.20, 195, 5, -110, 27.5, 0],
        [4.20, 195, 5, 110, 27.5, 0]);

    textureLoader = new THREE.TextureLoader();

    var back_door = new SlidingDoor([150, 187, 5, 30, 27.5, -7]);

    var back_frame = new Module([224.5, 4.20, 5, 0, -70, -7],
        [224.5, 4.20, 5, 0, 125, -7],
        [4.20, 195, 5, -110, 27.5, -7],
        [4.20, 195, 5, 110, 27.5, -7]);

    //A MeshPhongMaterial allows for shiny surfaces
    //A soft white light is being as specular light
    //The shininess value is the same as the matte finishing's value
    material = new THREE.MeshPhongMaterial({ /*map: texture, specular: 0x404040, shininess: 20*/ });

    //Adds front door
    front_door_faces_ids.push(generateParellepiped(
        front_door.sliding_door_axes[0],
        front_door.sliding_door_axes[1],
        front_door.sliding_door_axes[2],
        front_door.sliding_door_axes[3],
        front_door.sliding_door_axes[4],
        front_door.sliding_door_axes[5],
        material, group));

    //Adds front frame
    var borders = front_frame.module_faces;
    for (var i = 0; i < borders.length; i++) {
        front_frame_faces_ids.push(generateParellepiped(borders[i][0],
            borders[i][1], borders[i][2], borders[i][3], borders[i][4],
            borders[i][5], material, group));
    }

    //Adds back door
    back_door_faces_ids.push(generateParellepiped(
        back_door.sliding_door_axes[0],
        back_door.sliding_door_axes[1],
        back_door.sliding_door_axes[2],
        back_door.sliding_door_axes[3],
        back_door.sliding_door_axes[4],
        back_door.sliding_door_axes[5],
        material, group));

    //Adds back frame
    var borders = back_frame.module_faces;
    for (var i = 0; i < borders.length; i++) {
        back_frame_faces_ids.push(generateParellepiped(borders[i][0],
            borders[i][1], borders[i][2], borders[i][3], borders[i][4],
            borders[i][5], material, group));
    }

    scene.add(group);
    renderer.setClearColor(0xFFFFFF, 1);
}

/**
 * Initializes the scene's lighting.
 */
function initLighting() {
    var spotlight = new THREE.SpotLight(0x404040);
    camera.add(spotlight);

    spotlight.target = group;
    var lightAmbient = new THREE.AmbientLight(0x404040);
    scene.add(lightAmbient);
}


/**
 * Generates a parellepiped with a certain dimensions (width, height, depth) and on a certain position relatively to axes (x,y,z)
 * @param {number} width Number with the parellepiped width
 * @param {number} height Number with the parellepiped height
 * @param {number} depth Number with the parellepiped depth
 * @param {number} x Number with the parellepiped position relatively to the X axe
 * @param {number} y Number with the parellepiped position relatively to the Y axe
 * @param {number} z Number with the parellepiped position relatively to the Z axe
 * @param {THREE.Material} material THREE.Material with the parellepiped material
 * @param {THREE.Group} group THREE.Group with the group where the parellepied will be putted
 */
function generateParellepiped(width, height, depth, x, y, z, material, group) {
    var parellepipedGeometry = new THREE.CubeGeometry(width, height, depth);
    var parellepiped = new THREE.Mesh(parellepipedGeometry, material);
    parellepiped.position.x = x;
    parellepiped.position.y = y;
    parellepiped.position.z = z;
    group.add(parellepiped);
    return parellepiped.id;
}

/**
 * Animates the scene
 */
function animate() {
    //animate the scene at 60 frames per second
    setTimeout(function () {
        requestAnimationFrame(animate);
    }, 1000 / 60);
    controls.update();
    render();
}

/**
 * Renders the scene
 */
function render() {
    renderer.render(scene, camera);
}

/**
 * Initializes the graphic representation controls
 */
function initControls() {
    controls = new THREE.OrbitControls(camera, renderer.domElement);

    controls.target = new THREE.Vector3(0, 0, 0);

    controls.enableDamping = false; // an animation loop is required when either damping or auto-rotation are enabled
    controls.dampingFactor = 0.25;

    controls.screenSpacePanning = false;
    controls.minDistance = 100;
    controls.maxDistance = 500;

    controls.maxPolarAngle = Math.PI / 2;

    canvasWebGL.addEventListener('mousedown', onDocumentMouseDown, false);
    canvasWebGL.addEventListener('mousemove', onDocumentMouseMove, false);
    canvasWebGL.addEventListener('mouseup', onDocumentMouseUp, false);
}

/**
 * Initializes the graphic representation camera
 */
function initCamera() {
    camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 10, 1000);
    camera.position.y = 400;
    camera.position.z = 400;
    camera.rotation.x = .70;
}

/**
 * Represents the action that occurs when the mouse's left button is pressed (mouse down),
 * which is recognizing the object being clicked on, setting it as the selected one if
 * it is a slot and disabling the rotation control
 */
function onDocumentMouseDown(event) {
    event.preventDefault();
    raycaster.setFromCamera(mouse, camera);

    //Finds all intersected objects
    var intersects = raycaster.intersectObjects(scene.children[0].children);

    //Checks if any object was intersected
    if (intersects.length > 0) {
        //Gets the closest (clicked) object
        var clicked_object = intersects[0].object;

        //Checks if the intersected object is a door
        for (var i = 0; i < front_door_faces_ids.length; i++) {

            if (clicked_object == group.getObjectById(front_door_faces_ids[i]) || clicked_object == group.getObjectById(back_door_faces_ids[i])) {
                //Disables rotation while moving the door
                controls.enabled = false;
                //Sets the selection to the current door
                selected_door = clicked_object;
                if (raycaster.ray.intersectPlane(plane, intersection)) {
                    offset = intersection.x - selected_door.position.x;
                }
            }
        }
    }
}

/**
 * Represents the action that occurs when the mouse's left button is released, which is
 * setting the selected object as null, since it is no longer being picked, and enabling
 * the rotation control
 */
function onDocumentMouseUp(event) {
    //Sets the selected door to null (the door stops being selected)
    selected_door = null;
    //Enables rotation again
    controls.enabled = true;
}

/**
 * Represents the action that occurs when the mouse is dragged (mouse move), which
 * is interacting with the previously picked object on mouse down (moving it accross
 * the x axis)
 */
function onDocumentMouseMove(event) {
    event.preventDefault();

    var rect = event.target.getBoundingClientRect();
    var x = event.clientX;
    mouse.x = (x - rect.left) / (canvasWebGL.clientWidth / 2.0) - 1.0; //Get mouse x position

    raycaster.setFromCamera(mouse, camera); //Set raycast position

    //If the selected object is a door
    if (selected_door && raycaster.ray.intersectPlane(plane, intersection)) {

        var newPosition = intersection.x - offset; //Subtracts the offset to the x coordinate of the intersection point
        var limit = group.getObjectById(front_frame_faces_ids[2]).position.x + 80;

        //"+ 80" explanation: since the doors are initially placed in the x positions 30 and -30 (and not 0),
        //we need to subtract that value from the borders of the right frames x position, which is -110.
        //|-110 + 30| = 80. Don't know if the value should be stored in a global variable, since
        //it can't be computed dinamically because the selected door x position keeps changing 
        //when dragged

        if (Math.abs(newPosition) < Math.abs(limit)) selected_door.position.x = newPosition;
    }

    var intersects = raycaster.intersectObjects(scene.children[0].children);
    if (intersects.length > 0) {
        //Updates plane position to look at the camera
        var object = intersects[0].object;
        plane.setFromNormalAndCoplanarPoint(camera.position, object.position);

        if (hovered_object !== object) hovered_object = object;
    } else if (hovered_object !== null) hovered_object = null;
}