/**
 * Global Variables for Graphic Control (Camera, Rendering, Scene, etc...)
 */
var camera, controls, scene, renderer,group;

/**
 * Global variable with the current closet
 */
var closet=null;

/**
 * Global variable with the current closet faces ids (Mesh IDS from Three.js)
 */
var closet_faces_ids=[];

/**
 * Global variable with the current closet slots faces ids (Mesh IDS from Three.js)
 */
var closet_slots_faces_ids=[];


/**
 * Initial Product Draw function
 */
function main() {
    var canvasWebGL=document.getElementById("webgl");
    renderer = new THREE.WebGLRenderer({canvas:canvasWebGL});
    renderer.setSize(window.innerWidth, window.innerHeight);
    initCamera();
    initControls();
    initCloset();
    scene.add(group);
    registerEvents();
    animate();
}

/**
 * Initiates the closet
 */
function initCloset(){
    scene=new THREE.Scene();
    group=new THREE.Group();
    closet=new Closet();
    var faces=closet.closet_faces;
    
    for(var i=0;i<faces.length;i++){
        closet_faces_ids.push(generateParellepiped(faces[i][0],faces[i][1],faces[i][2]
                                    ,faces[i][3],faces[i][4],faces[i][5]
                                    ,createMaterialWithTexture(),group));
    }
    scene.add(group);
    renderer.setClearColor(0xFFFFFF,1);
}

/**
 * Updates current closet graphical view
 */
function updateClosetGV(){
    
    for(var i=0;i<closet_faces_ids.length;i++){
        var closet_face=group.getObjectById(closet_faces_ids[i]);
        closet_face.scale.x=getNewScaleValue(closet.initial_closet_faces[i][0],closet.closet_faces[i][0],closet_face.scale.x);
        closet_face.scale.y=getNewScaleValue(closet.initial_closet_faces[i][1],closet.closet_faces[i][1],closet_face.scale.y);
        closet_face.scale.z=getNewScaleValue(closet.initial_closet_faces[i][2],closet.closet_faces[i][2],closet_face.scale.z);
        closet_face.position.x=closet.closet_faces[i][3];
        closet_face.position.y=closet.closet_faces[i][4];
        closet_face.position.z=closet.closet_faces[i][5];
    }

    for(var i=0;i<closet_slots_faces_ids.length;i++){
        var closet_face=group.getObjectById(closet_slots_faces_ids[i]);
        closet_face.scale.x=getNewScaleValue(closet.initial_closet_slots_faces[i][0],closet.closet_slots_faces[i][0],closet_face.scale.x);
        closet_face.scale.y=getNewScaleValue(closet.initial_closet_slots_faces[i][1],closet.closet_slots_faces[i][1],closet_face.scale.y);
        closet_face.scale.z=getNewScaleValue(closet.initial_closet_slots_faces[i][2],closet.closet_slots_faces[i][2],closet_face.scale.z);
        closet_face.position.x=closet.closet_slots_faces[i][3];
        closet_face.position.y=closet.closet_slots_faces[i][4];
        closet_face.position.z=closet.closet_slots_faces[i][5];
    }
}



/**
 * Adds a slot to the current closet
 */
function addSlot(){
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
                                    ,createMaterialWithTexture(),group));
    }
    updateClosetGV();
}

/**
 * Removes a slot from the current closet
 */
function removeSlot(){
    closet.removeSlot();
    closet_slots_faces_ids.pop();
    updateClosetGV();
}

/**
 * Changes the dimensions of the closet
 * @param {number} width Number with the closet width
 * @param {number} height Number with the closet height
 * @param {number} depth Number with the closet depth
 */
function changeClosetDimensions(width,height,depth){
    closet.changeClosetWidth(width);
    closet.changeClosetHeight(height);
    closet.changeClosetDepth(depth);
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
function generateParellepiped(width,height,depth,x,y,z,material,group){
    var parellepipedGeometry=new THREE.CubeGeometry(width,height,depth);
    var parellepiped=new THREE.Mesh(parellepipedGeometry,material);
    //cube.add(new THREE.AxesHelper(200)); Displays the parellepiped axes
    parellepiped.position.x=x;
    parellepiped.position.y=y;
    parellepiped.position.z=z;
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
    renderer.render(scene, initCamera);
}

/**
 * Initializes the graphic representation controls
 */
function initControls(){
    // controls

    controls = new THREE.OrbitControls(initCamera, renderer.domElement);

    //controls.addEventListener( 'change', render ); // call this only in static scenes (i.e., if there is no animation loop)

    controls.enableDamping = true; // an animation loop is required when either damping or auto-rotation are enabled
    controls.dampingFactor = 0.25;

    controls.screenSpacePanning = false;

    controls.minDistance = 100;
    controls.maxDistance = 500;

    controls.maxPolarAngle = Math.PI / 2;
}

/**
 * Initializes the graphic representation camera
 */
function initCamera(){
    initCamera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 1, 1000);
    initCamera.position.y = -400;
    initCamera.position.z = 400;
    initCamera.rotation.x = .70;
}

/**
 * Computes the new scale value based on the initial scale value, new scale value and the current scale value
 * @param {number} initialScaleValue Number with the initial scale value
 * @param {number} newScaleValue Number with the new scale value
 * @param {number} currentScaleValue Number with the current scale value
 */
function getNewScaleValue(initialScaleValue,newScaleValue,currentScaleValue){
    if(initialScaleValue==0)return 0;
    return (newScaleValue*currentScaleValue)/initialScaleValue;
}

/**
 * Remove when found a better way
 */
function createMaterialWithTexture(){

    var texture = new THREE.TextureLoader().load( '../textures/cherry_wood_cabinets.jpg' );
    var material = new THREE.MeshBasicMaterial( { map: texture } );
    
    return material;
}

/**
 * Register the events that can be communicated through the document
 */
function registerEvents(){
    document.addEventListener("changeDimensions",function(changeDimensionsEvent){
        changeClosetDimensions(changeClosetDimensions.detail.width
                              ,changeClosetDimensions.detail.height
                              ,changeClosetDimensions.detail.depth);
    });
    document.addEventListener("addSlot",function(addSlotEvent){
        addSlotNumbered(addSlotEvent.detail.slots);
    });
    document.addEventListener("removeSlot",function(removeSlotEvent){
        addSlotNumbered(removeSlotEvent.detail.slots);
    });
}

/**
 * Returns the current closet width
 */
function getCurrentClosetWidth(){return closet.getClosetWidth();}

/**
 * Returns the current closet height
 */
function getCurrentClosetHeight(){return closet.getClosetHeight();}

/**
 * Returns the current closet depth
 */
function getCurrentClosetDepth(){return closet.getClosetDepth();}