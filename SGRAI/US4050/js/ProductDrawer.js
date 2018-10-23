var camera, controls, scene, renderer;

/**
 * Initial Product Draw function
 */
function main(){
    renderer=new THREE.WebGLRenderer();
    renderer.setSize(window.innerWidth,window.innerHeight);
    document.body.appendChild(renderer.domElement);

    camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 1, 1000);
    camera.position.y = -400;
    camera.position.z = 400;
    camera.rotation.x = .70;

    // controls

    controls = new THREE.OrbitControls( camera, renderer.domElement );

    //controls.addEventListener( 'change', render ); // call this only in static scenes (i.e., if there is no animation loop)

    controls.enableDamping = true; // an animation loop is required when either damping or auto-rotation are enabled
    controls.dampingFactor = 0.25;

    controls.screenSpacePanning = false;

    controls.minDistance = 100;
    controls.maxDistance = 500;

    controls.maxPolarAngle = Math.PI / 2;

    

    scene=new THREE.Scene();
    var cube=new THREE.Mesh(new THREE.CubeGeometry(200,100,100),new THREE.MeshNormalMaterial());

    cube.rotation.z=0.5;

    scene.add(cube);

    update();

}

function update(){
    requestAnimationFrame(update);
    controls.update();
    render();
}

function render(){
    renderer.render(scene,camera);
}