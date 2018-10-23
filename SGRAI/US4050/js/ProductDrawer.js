var camera, controls, scene, renderer;

var base_face_dimensions_axes=[200,0,100,0,0,0];
var top_face_dimensions_axes=[200,0,100,0,100,0];
var left_face_dimensions_axes=[0,100,100,100,50,0];
var right_face_dimensions_axes=[0,100,100,-100,50,0];
var back_face_dimensions_axes=[200,100,0,0,50,-50];

var faces=[base_face_dimensions_axes,top_face_dimensions_axes,left_face_dimensions_axes,right_face_dimensions_axes,back_face_dimensions_axes];
var faces_ids=[];

/**
 * Initial Product Draw function
 */
function main() {
    renderer = new THREE.WebGLRenderer();
    renderer.setSize(window.innerWidth, window.innerHeight);
    document.body.appendChild(renderer.domElement);

    camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 1, 1000);
    camera.position.y = -400;
    camera.position.z = 400;
    camera.rotation.x = .70;

    // controls

    controls = new THREE.OrbitControls(camera, renderer.domElement);

    //controls.addEventListener( 'change', render ); // call this only in static scenes (i.e., if there is no animation loop)

    controls.enableDamping = true; // an animation loop is required when either damping or auto-rotation are enabled
    controls.dampingFactor = 0.25;

    controls.screenSpacePanning = false;

    controls.minDistance = 100;
    controls.maxDistance = 500;

    controls.maxPolarAngle = Math.PI / 2;

    scene=new THREE.Scene();

    var group=new THREE.Group();

    for(var i=0;i<faces.length;i++){
        faces_ids.push(generateCube(faces[i][0],faces[i][1],faces[i][2],faces[i][3],faces[i][4],faces[i][5],new THREE.MeshNormalMaterial(),group));
    }

    scene.add(group);

    animate();

}


function generateCube(width,height,depth,x,y,z,materia1,group){
    var cubeGeometry=new THREE.CubeGeometry(width,height,depth);
    var cube=new THREE.Mesh(cubeGeometry,materia1);
    cube.add(new THREE.AxesHelper(200));
    cube.position.x=x;
    cube.position.y=y;
    cube.position.z=z;
    group.add(cube);
    return cube.id;
}

function animate() {
    requestAnimationFrame(animate);
    controls.update();
    render();
}

function render() {
    renderer.render(scene, camera);
}