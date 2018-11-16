/**
 * Global Variables for Graphic Control (Camera, Rendering, Scene, etc...)
 */
var camera, controls, scene,  renderer, group, groupB;
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
var drawer = null;
/**
 * Global variable with the current drawer
 */
var drawer1 = null;
/**
 * Global variable with the current drawer
 */
var drawer2 = null;
/**
 * Global variable with the current border
 */
var border = null;

/**
 * Global variable with the current drawer faces ids (Mesh IDS from Three.js)
 */
var drawer_faces_ids = [];
/**
 * Global variable with the current drawer faces ids (Mesh IDS from Three.js)
 */
var drawer_faces_ids1 = [];
/**
 * Global variable with the current drawer faces ids (Mesh IDS from Three.js)
 */
var drawer_faces_ids2 = [];
/**
 * Global variable with the current borders faces ids (Mesh IDS from Three.js)
 */
var borders_faces_ids = [];

/**
 * Global variable with the WebGL canvas
 */
var canvasWebGL;
/**
 * Global variable that represents the plane that intersects the closet
 */
var plane = null;

/**
 * Initial Product Draw function
 */
function main(textureSource) {
    canvasWebGL = document.getElementById("webgl");
    renderer = new THREE.WebGLRenderer({ canvas: canvasWebGL, antialias: true });
    //renderer.setSize(window.innerWidth, window.innerHeight);
    initCamera();
    initControls();
    init3Drawers(textureSource);
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
 * Initiates the closet
 * @param {*} textureSource - Source of the texture being loaded.
 */
function init3Drawers(textureSource) {
    scene = new THREE.Scene();
    group = new THREE.Group();
    groupB = new THREE.Group();
    groupC = new THREE.Group();
    groupD = new THREE.Group();


    drawer = new Drawer([204.5, 4.20, 100, 0, -65, 0] ///baixo (cumprimento,largura,profun, x,y,z)
    , [200, 50, 0, 0, -40, 50] ///frente
    , [4.20, 50, 95, -100, -40, 2.5] ///Esquerdaa
    , [4.20, 50, 95, 100, -40, 2.5] ///Direita
    , [200, 50, 0, 0, -40, -50]);//tras
    var faces = drawer.drawer_faces;

     drawer1 = new Drawer([204.5, 4.20, 100, 0, -5, 0] ///baixo (cumprimento,largura,profun, x,y,z)
    , [200, 50, 0, 0, 20, 50] ///frente
    , [4.20, 50, 100, -100, 20, 0] ///Esquerdaa
    , [4.20, 50, 100, 100, 20, 0] ///Direita
    , [200, 50, 0, 0, 20, -50]);
    var faces1 = drawer1.drawer_faces;

    drawer2 = new Drawer([204.5, 4.20, 100, 0, 75, 0] ///baixo (cumprimento,largura,profun, x,y,z)
    , [200, 50, 0, 0, 90, 50] ///frente
    , [4.20, 50, 100, -100, 90, 0] ///Esquerdaa
    , [4.20, 50, 100, 100, 90, 0] ///Direita
    , [200, 50, 0, 0, 90, -50]);
    var faces2 = drawer2.drawer_faces;

    border = new Module(
         [224.5, 4.20, 100, 0, -70, 0] ///baixo (cumprimento,largura,profun, x,y,z)
        , [224.5, 4.20, 100, 0, 125, 0] ///cima
        , [4.20, 195, 100, -110, 27.5, 0] ///Esquerdaa
        , [4.20, 195, 100, 110, 27.5, 0] ///Direita
        ); ///tras
    var borders = border.module_faces;


    //var src = 'http://127.0.0.1:8000/Renderer/textures/cherry_wood_cabinets.jpg';

    textureLoader = new THREE.TextureLoader();
    var texture = textureLoader.load(textureSource);
    //A MeshPhongMaterial allows for shiny surfaces
    //A soft white light is being as specular light
    //The shininess value is the same as the matte finishing's value
    material = new THREE.MeshPhongMaterial({ /*map: texture, specular: 0x404040, shininess: 20*/ });

    for (var i = 0; i < borders.length; i++) {
        borders_faces_ids.push(generateParellepiped(borders[i][0], borders[i][1], borders[i][2]
            , borders[i][3], borders[i][4], borders[i][5]
            , material, groupB));
    }
    scene.add(groupB);
    for (var i = 0; i < faces.length; i++) {
        drawer_faces_ids.push(generateParellepiped(faces[i][0], faces[i][1], faces[i][2]
            , faces[i][3], faces[i][4], faces[i][5]
            , material, groupB));
    }
    scene.add(group);
     for (var i = 0; i < faces1.length; i++) {
        drawer_faces_ids1.push(generateParellepiped(faces1[i][0], faces1[i][1], faces1[i][2]
            , faces1[i][3], faces1[i][4], faces1[i][5]
            , material, groupC));
    }
    scene.add(groupC);
    for (var i = 0; i < faces2.length; i++) {
        drawer_faces_ids2.push(generateParellepiped(faces2[i][0], faces2[i][1], faces2[i][2]
            , faces2[i][3], faces2[i][4], faces2[i][5]
            , material, groupD));
    }
    scene.add(groupD);
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