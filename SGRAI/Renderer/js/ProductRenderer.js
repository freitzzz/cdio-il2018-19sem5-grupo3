/**
 * Global Variables for Graphic Control (Camera, Rendering, Scene, etc...)
 */
var camera, controls, scene, renderer, group;
/**
 * Global variable with the current closet
 */
var closet = null;

/**
 * Global variable with the current closet faces ids (Mesh IDS from Three.js)
 */
var closet_faces_ids = [];

/**
 * Global variable with the current closet slots faces ids (Mesh IDS from Three.js)
 */
var closet_slots_faces_ids = [];


//new


var selection = null, hovered = null, plane = null, int0;

/**
 * Global variable with a vector that holds the mouse coordinates
 */
var mouse = new THREE.Vector2();

var offset = new THREE.Vector3();

var intersection = new THREE.Vector3(0, 0, 0);

var raycaster = new THREE.Raycaster();


//end new


/**
 * Global variable with the WebGL canvas
 */
var canvasWebGL;

/**
 * Initial Product Draw function
 */
function main() {
    canvasWebGL = document.getElementById("webgl");
    renderer = new THREE.WebGLRenderer({ canvas: canvasWebGL });

    //renderer.setSize(window.innerWidth, window.innerHeight);
    //renderer.domElement.addEventListener('mousedown', onDocumentMouseDown, false); old
    //renderer.domElement.addEventListener('mousemove', raycast, false); old

    initCamera();
    initControls();
    initCloset();

    // Adds lights
    //scene.add(new THREE.AmbientLight(0x444444));
    //var dirLight = new THREE.DirectionalLight(0xffffff);
    //dirLight.position.set(200, 200, 1000).normalize();
    //camera.add(dirLight);
    //camera.add(dirLight.target);


    plane = new THREE.Plane();
    plane.setFromNormalAndCoplanarPoint(new THREE.Vector3(0, 0, 1), new THREE.Vector3(0, 200, 0)).normalize();

    var planeGeometry = new THREE.PlaneGeometry(500, 500);

    var coplanarPoint = plane.coplanarPoint();

    var focalPoint = new THREE.Vector3().copy(coplanarPoint).add(plane.normal);

    planeGeometry.lookAt(focalPoint);

    planeGeometry.translate(coplanarPoint.x, coplanarPoint.y, coplanarPoint.z);

    var planeMaterial = new THREE.MeshLambertMaterial({ color: 0xffff00, side: THREE.DoubleSide });

    var dispPlane = new THREE.Mesh(planeGeometry, planeMaterial);
    dispPlane.visible = false;

    scene.add(dispPlane);
    scene.add(camera);

    //changeClosetSlots(0);
    registerEvents();
    animate();
}

// /**
//  * Raycast function used for picking
//  */
// function raycast(e) {
//     if (e.buttons != 2) return;

//     var x = e.clientX;
//     var y = e.clientY;
//     var rect = e.target.getBoundingClientRect();
//     mouse.x = (x - rect.left) / (canvasWebGL.clientWidth / 2.0) - 1.0;
//     mouse.y = (canvasWebGL.clientHeight - (y - rect.top)) / (canvasWebGL.clientHeight / 2.0) - 1.0;

//     //2. set the picking ray from the camera position and mouse coordinates
//     raycaster.setFromCamera(mouse, camera);

//     //3. compute intersections
//     var intersects = raycaster.intersectObjects(scene.children[0].children);
//     if (intersects != null) {
//         var face = intersects[0].object;

//         for (var i = 0; i < closet_slots_faces_ids.length; i++) {
//             var closet_face = group.getObjectById(closet_slots_faces_ids[i]);
//             if (JSON.stringify(closet_face) == JSON.stringify(face)) {

//                 if (mouse.x > 0 && face.position.x < group.getObjectById(closet_faces_ids[3]).position.x - 15) {
//                     face.translateX(4);
//                 }

//                 if (mouse.x < 0 && face.position.x > group.getObjectById(closet_faces_ids[2]).position.x + 15) {
//                     face.translateX(-4);
//                 }
//             }
//         }
//     }
// }

/**
 * Initiates the closet
 */
function initCloset() {
    scene = new THREE.Scene();

    group = new THREE.Group();
    closet = new Closet([204.5, 4.20, 100, 0, 0, 0]
        , [204.5, 4.20, 100, 0, 100, 0]
        , [4.20, 100, 100, -100, 50, 0]
        , [4.20, 100, 100, 100, 50, 0]
        , [200, 100, 0, 0, 50, -50]);
    var faces = closet.closet_faces;

    for (var i = 0; i < faces.length; i++) {
        closet_faces_ids.push(generateParellepiped(faces[i][0], faces[i][1], faces[i][2]
            , faces[i][3], faces[i][4], faces[i][5]
            , createMaterialWithTexture(), group));
    }
    scene.add(group);
    renderer.setClearColor(0xFFFFFF, 1);
}

/**
 * Updates current closet graphical view
 */
function updateClosetGV() {

    for (var i = 0; i < closet_faces_ids.length; i++) {
        var closet_face = group.getObjectById(closet_faces_ids[i]);
        closet_face.scale.x = getNewScaleValue(closet.getInitialClosetFaces()[i][0], closet.getClosetFaces()[i][0], closet_face.scale.x);
        closet_face.scale.y = getNewScaleValue(closet.getInitialClosetFaces()[i][1], closet.getClosetFaces()[i][1], closet_face.scale.y);
        closet_face.scale.z = getNewScaleValue(closet.getInitialClosetFaces()[i][2], closet.getClosetFaces()[i][2], closet_face.scale.z);
        closet_face.position.x = closet.getClosetFaces()[i][3];
        closet_face.position.y = closet.getClosetFaces()[i][4];
        closet_face.position.z = closet.getClosetFaces()[i][5];
    }

    for (var i = 0; i < closet_slots_faces_ids.length; i++) {
        var closet_face = group.getObjectById(closet_slots_faces_ids[i]);
        closet_face.scale.x = getNewScaleValue(closet.getInitialClosetSlotFaces()[i][0], closet.getClosetSlotFaces()[i][0], closet_face.scale.x);
        closet_face.scale.y = getNewScaleValue(closet.getInitialClosetSlotFaces()[i][1], closet.getClosetSlotFaces()[i][1], closet_face.scale.y);
        closet_face.scale.z = getNewScaleValue(closet.getInitialClosetSlotFaces()[i][2], closet.getClosetSlotFaces()[i][2], closet_face.scale.z);
        closet_face.position.x = closet.getClosetSlotFaces()[i][3];
        closet_face.position.y = closet.getClosetSlotFaces()[i][4];
        closet_face.position.z = closet.getClosetSlotFaces()[i][5];
    }
}


/**
 * Adds a slot to the current closet
 */
function addSlot() {
    addSlotNumbered(1);
}

/**
 * Adds a specified number of slots to the current closet
 */
function addSlotNumbered(slotsToAdd) {
    for (var i = 0; i < slotsToAdd; i++) {
        var slotFace = closet.addSlot();
        closet_slots_faces_ids.push(generateParellepiped(slotFace[0], slotFace[1], slotFace[2]
            , slotFace[3], slotFace[4], slotFace[5]
            , createMaterialWithTexture(), group));
    }
    updateClosetGV();
}

/**
 * Removes a slot from the current closet
 */
function removeSlot() {
    closet.removeSlot();
    var closet_slot_face_id = closet_slots_faces_ids.pop();
    group.remove(group.getObjectById(closet_slot_face_id));
    updateClosetGV();
}

/**
 * Changes the dimensions of the closet
 * @param {number} width Number with the closet width
 * @param {number} height Number with the closet height
 * @param {number} depth Number with the closet depth
 */
function changeClosetDimensions(width, height, depth) {
    closet.changeClosetWidth(width);
    closet.changeClosetHeight(height);
    closet.changeClosetDepth(depth);
    updateClosetGV();
}

/**
 * Changes the current closet slots
 * @param {number} slots Number with the new closet slots
 */
function changeClosetSlots(slots) {
    var newSlots = closet.computeNewClosetSlots(slots);
    if (newSlots > 0) {
        for (var i = 0; i < newSlots; i++) {
            addSlot();
        }
    } else {
        newSlots = -newSlots;
        if (newSlots == 0) removeSlot();
        for (var i = 0; i < newSlots; i++) {
            removeSlot();
        }
    }
    updateClosetGV();
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
    requestAnimationFrame(animate);
    controls.update()
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
    // controls

    controls = new THREE.OrbitControls(camera, renderer.domElement);

    controls.target = new THREE.Vector3(0, 0, 0); // new

    //controls.addEventListener( 'change', render ); // call this only in static scenes (i.e., if there is no animation loop)

    controls.enableDamping = false; // an animation loop is required when either damping or auto-rotation are enabled
    controls.dampingFactor = 0.25;

    controls.screenSpacePanning = false;
    controls.minDistance = 100;
    controls.maxDistance = 500;

    controls.maxPolarAngle = Math.PI / 2;
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
 * Computes the new scale value based on the initial scale value, new scale value and the current scale value
 * @param {number} initialScaleValue Number with the initial scale value
 * @param {number} newScaleValue Number with the new scale value
 * @param {number} currentScaleValue Number with the current scale value
 */
function getNewScaleValue(initialScaleValue, newScaleValue, currentScaleValue) {
    if (initialScaleValue == 0) return 0;
    return (newScaleValue * 1) / initialScaleValue;
}

/**
 * Remove when found a better way
 */
function createMaterialWithTexture() {

    //var texture = new THREE.TextureLoader().load( '../Renderer/textures/cherry_wood_cabinets.jpg' );
    //var material = new THREE.MeshBasicMaterial( { map: texture } );


    //return material; 
    return new THREE.MeshNormalMaterial();
}

/**
 * Register the events that can be communicated through the document
 */
function registerEvents() {
    document.addEventListener("changeDimensions", function (changeDimensionsEvent) {
        changeClosetDimensions(changeDimensionsEvent.detail.width
            , changeDimensionsEvent.detail.height
            , changeDimensionsEvent.detail.depth);
    });

    document.addEventListener("changeSlots", function (changeSlotsEvent) {
        changeClosetSlots(changeSlotsEvent.detail.slots);
    });

    document.addEventListener('mousedown', onDocumentMouseDown, false); //new

    document.addEventListener('mousemove', onDocumentMouseMove, false); //new

    document.addEventListener('mouseup', onDocumentMouseUp, false); //new
}

function onDocumentMouseDown(event) { //new
    event.preventDefault();
    raycaster.setFromCamera(mouse, camera);

    //Finds all intersected objects (closet faces)
    var intersects = raycaster.intersectObjects(scene.children[0].children);

    //Checks if any closet face was intersected
    if (intersects.length > 0) {
        //Gets the closest (clicked) object
        var face = intersects[0].object;

        //Checks if the selected closet face is a slot 
        for (var i = 0; i < closet_slots_faces_ids.length; i++) {
            var closet_face = group.getObjectById(closet_slots_faces_ids[i]);

            if (JSON.stringify(closet_face) == JSON.stringify(face)) {

                //Disables rotation while moving the slot
                controls.enabled = false;

                //Sets the selection to the current slot
                selection = face;
                if (raycaster.ray.intersectPlane(plane, intersection)) {
                    //   offset.copy(intersection).sub(selection.position);
                    offset.x = intersection.x - selection.position.x;
                }
                // var aux = raycaster.ray.intersectPlane(plane, intersection);
                //  if (aux) int0 = aux;
            }
        }
    }
}

function onDocumentMouseMove(event) { //new
    event.preventDefault();
    //Get mouse position
    var rect = event.target.getBoundingClientRect();
    var x = event.clientX;

    mouse.x = (x - rect.left) / (canvasWebGL.clientWidth / 2.0) - 1.0;

    //Set raycast position
    raycaster.setFromCamera(mouse, camera);

    if (selection) {
        if (raycaster.ray.intersectPlane(plane, intersection)) {
            var aux = intersection.x - offset.x;
            if (aux) {
                selection.position.x = aux;
            }
            //selection.position.copy(intersection.sub(offset));
        }
        // var int = raycaster.ray.intersectPlane(plane, intersection);
        // if (int) {
        //     offset.x = int.x - int0.x;
        //     selection.position.x += offset.x;
        //     int0 = int;
        // }
        return;
    }

    var intersects = raycaster.intersectObjects(scene.children[0].children);
    if (intersects.length > 0) {
        //Updates plane position to look at the camera
        var face = intersects[0].object;
        plane.setFromNormalAndCoplanarPoint(camera.position, face.position);

        if (hovered !== face) hovered = face;

    } else {
        if (hovered !== null) hovered = null;
    }
}

function onDocumentMouseUp(event) {
    //Enables rotation again
    controls.enabled = true;
    //Sets the selection to null (the slot stops being selected)
    selection = null;
}

/**
 * Returns the current closet width
 */
function getCurrentClosetWidth() { return closet.getClosetWidth(); }

/**
 * Returns the current closet height
 */
function getCurrentClosetHeight() { return closet.getClosetHeight(); }

/**
 * Returns the current closet depth
 */
function getCurrentClosetDepth() { return closet.getClosetDepth(); }