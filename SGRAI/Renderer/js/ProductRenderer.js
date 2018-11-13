/**
 * Global Variables for Graphic Control (Camera, Rendering, Scene, etc...)
 */
var camera, controls, scene, renderer, group;
/**
 * Global variable for 
 */
var textureLoader;

/**
 * Global variable for the Mesh Material.
 */
var material;

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

/**
 * Global variable with the WebGL canvas
 */
var canvasWebGL;

// ------------ Global variables used to dinamically resize Slots ------------
/**
 * Global variable that represents the currently selected slot (null if none)
 */
var selected_object = null;

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
// ------------ End of global variables used to dinamically resize Slots ------------



/**
 * Initial Product Draw function
 */
function main(textureSource) {
    canvasWebGL=document.getElementById("webgl");
    renderer = new THREE.WebGLRenderer({canvas:canvasWebGL, antialias: true});
    //renderer.setSize(window.innerWidth, window.innerHeight);
    initCamera();
    initControls();
    initCloset(textureSource);
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

    registerEvents();
    animate();
}

/**
 * Initiates the closet
 * @param {*} textureSource - Source of the texture being loaded.
 */
function initCloset(textureSource){
    scene=new THREE.Scene();
    group=new THREE.Group();
    closet=new Closet([204.5,4.20,100,0,0,0]
                     ,[204.5,4.20,100,0,100,0]
                     ,[4.20,100,100,-100,50,0]
                     ,[4.20,100,100,100,50,0]
                     ,[200,100,0,0,50,-50]);
    var faces=closet.closet_faces;


    //var src = 'http://127.0.0.1:8000/Renderer/textures/cherry_wood_cabinets.jpg';

    textureLoader = new THREE.TextureLoader();
    var texture = textureLoader.load( textureSource );
    //A MeshPhongMaterial allows for shiny surfaces
    //A soft white light is being as specular light
    //The shininess value is the same as the matte finishing's value
    material = new THREE.MeshPhongMaterial( { map: texture, specular: 0x404040, shininess: 20 } );

    for(var i=0;i<faces.length;i++){
        closet_faces_ids.push(generateParellepiped(faces[i][0],faces[i][1],faces[i][2]
                                    ,faces[i][3],faces[i][4],faces[i][5]
                                    ,material,group));
    }
    scene.add(group);
    renderer.setClearColor(0xFFFFFF, 1);
}

/**
 * Initializes the scene's lighting.
 */
function initLighting(){
    var spotlight = new THREE.SpotLight(0x404040);
    camera.add(spotlight);

    spotlight.target = group;
    var lightAmbient = new THREE.AmbientLight(0x404040);
    scene.add(lightAmbient);
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
function addSlotNumbered(slotsToAdd){
    for(var i=0;i<slotsToAdd;i++){
        var slotFace=closet.addSlot();
        closet_slots_faces_ids.push(generateParellepiped(slotFace[0],slotFace[1],slotFace[2]
                                    ,slotFace[3],slotFace[4],slotFace[5]
                                    ,material,group));
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
 * Applies the texture to the closet.
 * @param {*} texture - texture being applied.
 */
function applyTexture(texture){
    textureLoader.load(texture, function(tex){
        material.map = tex;
    })
}

/**
 * Changes the closet's material's shininess.
 * @param {*} shininess - new shininess value
 */
function changeShininess(shininess){
    material.shininess = shininess;
}

function changeColor(color){
    material.color.setHex(color);
}

/**
 * Changes the current closet slots
 * @param {number} slots Number with the new closet slots
 */
function changeClosetSlots(slots,slotWidths) {
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
    /* if(slotWidths.length > 0){
        updateSlotWidths(slotWidths);
    } */
    updateClosetGV();
}

function updateSlotWidths(slotWidths){
    for(let i = 0; i < slotWidths.length; i++){
        var closet_face = group.getObjectById(closet_slots_faces_ids[i]);
        closet_face.position.x = slotWidths[i];
    }
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
    setTimeout(function(){
        requestAnimationFrame(animate);
    }, 1000/60);
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
 * @deprecated No need to use this function since the material is now a global variable.
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
        changeClosetDimensions(changeDimensionsEvent.detail.width, changeDimensionsEvent.detail.height, changeDimensionsEvent.detail.depth);
    });

    document.addEventListener("changeSlots", function (changeSlotsEvent) {
        changeClosetSlots(changeSlotsEvent.detail.slots, changeSlotsEvent.detail.slotWidths);
    });
    document.addEventListener("changeMaterial", function(changeMaterialEvent){
        applyTexture(changeMaterialEvent.detail.material);
    });
    document.addEventListener("changeShininess", function(changeShininessEvent){
        changeShininess(changeShininessEvent.detail.shininess);
    });
    document.addEventListener("changeColor", function(changeColorEvent){
        changeColorEvent(changeColorEvent.detail.color);
    });
}

/**
 * Represents the action that occurs when the mouse's left button is pressed (mouse down),
 * which is recognizing the object being clicked on, setting it as the selected one if
 * it is a slot and disabling the rotation control
 */
function onDocumentMouseDown(event) {
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
                selected_object = face;
                if (raycaster.ray.intersectPlane(plane, intersection)) {
                    offset = intersection.x - selected_object.position.x;
                }
            }
        }
    }
}

/**
 * Represents the action that occurs when the mouse is dragged (mouse move), which
 * is interacting with the previously picked object on mouse down (moving it accross
 * the x axis)
 */
function onDocumentMouseMove(event) {
    event.preventDefault();
    //Get mouse position
    var rect = event.target.getBoundingClientRect();
    var x = event.clientX;

    mouse.x = (x - rect.left) / (canvasWebGL.clientWidth / 2.0) - 1.0;

    //Set raycast position
    raycaster.setFromCamera(mouse, camera);

    if (selected_object) {
        if (raycaster.ray.intersectPlane(plane, intersection)) {
            var aux = intersection.x - offset;
            //    if (aux.x <= group.getObjectById(closet_faces_ids[3]).position.x
            //      && aux.x >= group.getObjectById(closet_faces_ids[2].position.x)) {
                var valueCloset = group.getObjectById(closet_faces_ids[2]).position.x;
            if (Math.abs(aux) < Math.abs(valueCloset)) {
                selected_object.position.x = aux;
            }
           
            // }
        }
        return;
    }

    var intersects = raycaster.intersectObjects(scene.children[0].children);
    if (intersects.length > 0) {
        //Updates plane position to look at the camera
        var face = intersects[0].object;
        plane.setFromNormalAndCoplanarPoint(camera.position, face.position);

        if (hovered_object !== face) hovered_object = face;

    } else {
        if (hovered_object !== null) hovered_object = null;
    }
}

/**
 * Represents the action that occurs when the mouse's left button is released, which is
 * setting the selected object as null, since it is no longer being picked, and enabling
 * the rotation control
 */
function onDocumentMouseUp(event) {
    //Enables rotation again
    controls.enabled = true;
    //Sets the selection to null (the slot stops being selected)
    selected_object = null;
}

/**
 * Returns the current closet width
 */
function getCurrentClosetWidth() {
    return closet.getClosetWidth();
}

/**
 * Returns the current closet height
 */
function getCurrentClosetHeight() {
    return closet.getClosetHeight();
}

/**
 * Returns the current closet depth
 */
function getCurrentClosetDepth() {
    return closet.getClosetDepth();
}