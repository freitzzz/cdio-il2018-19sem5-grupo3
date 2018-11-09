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
    //renderer.setSize(window.innerWidth, window.innerHeight);
    initCamera();
    initControls();
    initCloset();
    initLighting();
    //changeClosetSlots(0);
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
    closet=new Closet([204.5,4.20,100,0,0,0]
                     ,[204.5,4.20,100,0,100,0]
                     ,[4.20,100,100,-100,50,0]
                     ,[4.20,100,100,100,50,0]
                     ,[200,100,0,0,50,-50]);
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
 * Initializes the scene's lighting.
 */
function initLighting(){
    var spotlight = new THREE.SpotLight();
    spotlight.position.set(400, 400, 0.70);
    spotlight.target = group;
    var lightAmbient = new THREE.AmbientLight(0x404040);
    scene.add(spotlight, lightAmbient);
}

/**
 * Updates current closet graphical view
 */
function updateClosetGV(){
    
    for(var i=0;i<closet_faces_ids.length;i++){
        var closet_face=group.getObjectById(closet_faces_ids[i]);
        closet_face.scale.x=getNewScaleValue(closet.getInitialClosetFaces()[i][0],closet.getClosetFaces()[i][0],closet_face.scale.x);
        closet_face.scale.y=getNewScaleValue(closet.getInitialClosetFaces()[i][1],closet.getClosetFaces()[i][1],closet_face.scale.y);
        closet_face.scale.z=getNewScaleValue(closet.getInitialClosetFaces()[i][2],closet.getClosetFaces()[i][2],closet_face.scale.z);
        closet_face.position.x=closet.getClosetFaces()[i][3];
        closet_face.position.y=closet.getClosetFaces()[i][4];
        closet_face.position.z=closet.getClosetFaces()[i][5];
    }

    for(var i=0;i<closet_slots_faces_ids.length;i++){
        var closet_face=group.getObjectById(closet_slots_faces_ids[i]);
        closet_face.scale.x=getNewScaleValue(closet.getInitialClosetSlotFaces()[i][0],closet.getClosetSlotFaces()[i][0],closet_face.scale.x);
        closet_face.scale.y=getNewScaleValue(closet.getInitialClosetSlotFaces()[i][1],closet.getClosetSlotFaces()[i][1],closet_face.scale.y);
        closet_face.scale.z=getNewScaleValue(closet.getInitialClosetSlotFaces()[i][2],closet.getClosetSlotFaces()[i][2],closet_face.scale.z);
        closet_face.position.x=closet.getClosetSlotFaces()[i][3];
        closet_face.position.y=closet.getClosetSlotFaces()[i][4];
        closet_face.position.z=closet.getClosetSlotFaces()[i][5];
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
    var closet_slot_face_id=closet_slots_faces_ids.pop();
    group.remove(group.getObjectById(closet_slot_face_id));
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
 * Changes the current closet slots
 * @param {number} slots Number with the new closet slots
 */
function changeClosetSlots(slots){
    var newSlots=closet.computeNewClosetSlots(slots);
    if(newSlots>0){
        for(var i=0;i<newSlots;i++){
            addSlot();
        }
    }else{
        newSlots=-newSlots;
        if(newSlots==0)removeSlot();
        for(var i=0;i<newSlots;i++){
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
function generateParellepiped(width,height,depth,x,y,z,material,group){
    var parellepipedGeometry=new THREE.CubeGeometry(width,height,depth);
    var parellepiped=new THREE.Mesh(parellepipedGeometry,material);
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
    initCamera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 10, 1000);
    initCamera.position.y = 400;
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
    return (newScaleValue*1)/initialScaleValue;
}

/**
 * Remove when found a better way
 */
function createMaterialWithTexture(){

    //  var texture = new THREE.TextureLoader().load( '../textures/cherry_wood_cabinets.jpg' );
    //  var material = new THREE.MeshBasicMaterial( { map: texture } );
    
    //  return material; 
    return new THREE.MeshNormalMaterial();
}

/**
 * Register the events that can be communicated through the document
 */
function registerEvents(){
    document.addEventListener("changeDimensions",function(changeDimensionsEvent){
        changeClosetDimensions(changeDimensionsEvent.detail.width
                              ,changeDimensionsEvent.detail.height
                              ,changeDimensionsEvent.detail.depth);
    });
    document.addEventListener("changeSlots",function(changeSlotsEvent){
        changeClosetSlots(changeSlotsEvent.detail.slots);
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