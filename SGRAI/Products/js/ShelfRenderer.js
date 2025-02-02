/**
 * Global Variables for Graphic Control (Camera, Rendering, Scene, etc...)
 */
var camera, controls, scene, renderer, group, groupB;
/**
 * Global variable for 
 */
var textureLoader;

/**
 * Global variable for the Mesh Material.
 */
var material;

/**
 * Global variable with the current drawer
 */
var shelf = null;
/**
 * Global variable with the current shelf faces ids (Mesh IDS from Three.js)
 */
var shelf_faces_ids = [];

/**
 * Global variable with the WebGL canvas
 */
var canvasWebGL;

/**
 * Initial Product Draw function
 */
function main(textureSource) {
    canvasWebGL = document.getElementById("webgl");
    renderer = new THREE.WebGLRenderer({ canvas: canvasWebGL, antialias: true });
    //renderer.setSize(window.innerWidth, window.innerHeight);
    initCamera();
    initControls();
    initShelf(textureSource);
    initLighting();

    scene.add(camera);

    animate();
}
function initShelf(textureSource) {
    scene = new THREE.Scene();
    group = new THREE.Group();

    shelf = new Shelf([200, 10, 200, 0, 0, 0]);

    textureLoader = new THREE.TextureLoader();
    var texture = textureLoader.load(textureSource);
    material = new THREE.MeshPhongMaterial({ map: texture, specular: 0x404040, shininess: 20 });

    shelf_faces_ids.push(generateParellepiped(shelf.shelf_base_face_dimensions_axes[0],
        shelf.shelf_base_face_dimensions_axes[1],
        shelf.shelf_base_face_dimensions_axes[2]
        , shelf.shelf_base_face_dimensions_axes[3],
        shelf.shelf_base_face_dimensions_axes[4],
        shelf.shelf_base_face_dimensions_axes[5], material, group));
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
