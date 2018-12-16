////@ts-check
import 'three/examples/js/controls/OrbitControls'
import * as THREE from 'three'
import ThreeCloset from './threejeyass/domain/ThreeCloset';
import Pole from './Pole'
import Drawer from './Drawer'
import Module from './Module'
import SlidingDoor from './SlidingDoor'
import ThreeFace from './threejeyass/domain/ThreeFace';
import Shelf from './Shelf'
import HingedDoor from './HingedDoor'
import FaceOrientationEnum from './api/domain/FaceOrientation';

export default class ProductRenderer {

  canMoveCloset;

  canMoveSlots;

  canMoveComponents;

  /**
   * Instance variable representing the camera through which the scene is rendered.
   * @type{THREE.Camera}
   */
  camera;

  /**
   * @type{THREE.OrbitControls}
   */
  controls;

  /**
   * @type{THREE.Scene}
   */
  scene;

  /**
   * @type{THREE.WebGLRenderer}
   */
  renderer;

  /**
   * @type{THREE.Group}
   */
  group;

  /**
   * @type{THREE.TextureLoader}
   */
  textureLoader;

  /**
   * @type{THREE.MeshPhongMaterial}
   */
  material;

  /**
   * @type{ThreeCloset}
   */
  closet;

  /**
  * Global variable to know when to animate a hinged door 
  */
  hingedDoor;

  /**
   * Global variable to know when to animate a sliding door
   */
  slidingDoor;

  /**
   * Waiting list for doors that are waiting to be rendered (e.g. drawer animation has to end)
   */
  waitingDoors;

  /**
   * Flag to know whether a drawer is closed or not
   */
  openDrawers;

  /**
   * Flag to know whether a hinged door is closed or not
   */
  isHingedDoorClosed;

  /**
   * Instance variable with the current closet faces ids (Mesh IDS from Three.js)
   * @type {number[]}
   */
  closet_faces_ids;

  /**
   * Instance variable with the current closet slots faces ids (Mesh IDS from Three.js)
   * @type {number[]}
   */
  closet_slots_faces_ids;

  /**
   * Global variable with the current closet shelves ids (Mesh IDs from Three.js)
   * @type {number[]}
   */
  closet_shelves_ids;

  closet_poles_ids;

  closet_modules_ids;

  closet_drawers_ids;

  closet_hinged_doors_ids;

  closet_sliding_doors_ids;

  /**
   * Instance variable with the WebGL canvas
   * @type {HTMLCanvasElement}
   */
  canvasWebGL;

  // ------------ Instance variables used to dinamically resize Slots ------------
  /**
   * Instance variables that represent the currently selected slot and face (null if none)
   */
  selected_slot;

  /**
   * 
   */
  selected_face;

  selected_component;

  /**
   * Instance variable that represents the object being hovered (null if none)
   */
  hovered_object;

  /**
   * Instance variable that represents the plane that intersects the closet
   */
  plane;

  /**
   * Instance variable that represents the difference between the intersection's x coordinate
   * and the selected object's x coordinate
   * @type{number}
   */
  offset;

  /**
   * Instance variable with a Vector that holds the mouse coordinates (x, y)
   * @type{THREE.Vector2}
   */
  mouse;

  /**
   * Instance variable with a Vector that represents the intersection between the plane and
   * the clicked object
   * @type{THREE.Vector3}
   */
  intersection;

  /**
   * Instance variable with a Raycaster used for picking (hovering, clicking and identifying) objects
   * @type{THREE.Raycaster}
   */
  raycaster;
  // ------------ End of instance variables used to dinamically resize Slots ------------

  // --------------Beggining of resize control ----------------------

  /**
   * Variables that represent the index of each dimension in the vector
   */
  WIDTH;
  HEIGHT;
  DEPTH;

  /**Vector that saves the value of each resize for each dimensions */
  resizeVec;

  /**Initial values of dimension in three js  */
  initialDimensions;

  /* Initial values of dimension in the website */
  websiteDimensions;

  /**Number of dimensions in question */
  NUMBER_DIMENSIONS;
  // ---------------- End of resize control --------------------------
  /**
   * 
   * @param {HTMLCanvasElement} htmlCanvasElement 
   */
  constructor(htmlCanvasElement) {

    /* Create vector for resizing purposes: */

    this.WIDTH = 0;
    this.HEIGHT = 1;
    this.DEPTH = 2;
    this.resizeVec = [];

    /* Create vector for initial values of height,width and depth */
    this.initialDimensions = [404.5, 300, 100];

    this.NUMBER_DIMENSIONS = 3;

    this.websiteDimensions = [500, 100, 15000];

    this.canMoveCloset = true;
    this.canMoveSlots = true;
    this.canMoveComponents = true;

    this.hingedDoor = null;
    this.slidingDoor = null;
    this.waitingDoors = [];
    this.openDrawers = [];
    this.closet_hinged_doors_ids = [];
    this.closet_sliding_doors_ids = [];
    this.isHingedDoorClosed = false;
    this.closet_faces_ids = [];
    this.closet_slots_faces_ids = [];
    this.closet_poles_ids = [];
    this.closet_shelves_ids = [];
    this.closet_modules_ids = [];
    this.closet_drawers_ids = [];
    this.selected_slot = null;
    this.selected_face = null;
    this.selected_component = null;
    this.hovered_object = null;
    this.plane = null;
    this.mouse = new THREE.Vector2();
    this.intersection = new THREE.Vector3(0, 0, 0);
    this.raycaster = new THREE.Raycaster();
    this.canvasWebGL = htmlCanvasElement;
    this.renderer = new THREE.WebGLRenderer({
      canvas: this.canvasWebGL,
      antialias: true
    });
    this.initCamera();
    this.initControls();
    this.scene = new THREE.Scene();
    this.group = new THREE.Group();
    this.initCloset();
    this.initLighting();

    var geometry = new THREE.SphereBufferGeometry(430, 60, 40);
    geometry.scale(-1, 1, 1);

    var material = new THREE.MeshBasicMaterial({
      map: new THREE.TextureLoader().load("./../../src/assets/background.jpg")
    });


    var mesh = new THREE.Mesh(geometry, material);
    this.renderer.setPixelRatio(window.devicePixelRatio);

    //Creates the intersection plane
    this.plane = new THREE.Plane();
    this.plane.setFromNormalAndCoplanarPoint(new THREE.Vector3(0, 0, 1), new THREE.Vector3(0, 200, 0)).normalize();

    var planeGeometry = new THREE.PlaneGeometry(500, 500);

    var coplanarPoint = this.plane.coplanarPoint();

    var focalPoint = new THREE.Vector3().copy(coplanarPoint).add(this.plane.normal);

    planeGeometry.lookAt(focalPoint);

    planeGeometry.translate(coplanarPoint.x, coplanarPoint.y, coplanarPoint.z);

    var planeMaterial = new THREE.MeshLambertMaterial({
      color: 0xffff00,
      side: THREE.DoubleSide
    });

    var dispPlane = new THREE.Mesh(planeGeometry, planeMaterial);
    dispPlane.visible = false;
    //Finishes creating the intersection plane

    this.scene.add(dispPlane);
    this.scene.add(this.camera);
    this.scene.add(mesh);
    alert("<<<<<<<<<<<")
    this.animate();
    alert(">>>>>>>>>>>")
  }

  activateCanMoveSlots() { this.canMoveSlots = true; }
  deactivateCanMoveSlots() { this.canMoveSlots = false; }

  activateCanMoveComponents() { this.canMoveComponents = true; }
  deactivateCanMoveComponents() { this.canMoveComponents = false; }

  activateCanMoveCloset() { this.canMoveCloset = true; }
  deactivateCanMoveCloset() { this.canMoveCloset = false; }

  /**
   * Initiates the closet
   */
  initCloset() {
    var thickness = 4.20;

    
    /* this.closet = new Closet(, //Bottom
      , //Top
      , //Left
      , //Right
      ); //Back */

    this.textureLoader = new THREE.TextureLoader();
    //A MeshPhongMaterial allows for shiny surfaces
    //A soft white light is being as specular light
    //The shininess value is the same as the matte finishing's value
    this.material = new THREE.MeshPhongMaterial({
      specular: 0x404040,
      shininess: 20
    });

    let closet_faces=new Map();
    closet_faces.set(FaceOrientationEnum.BASE,new ThreeFace(null,this.material,FaceOrientationEnum.BASE,404.5, thickness, 100, 0, -210, -195));
    closet_faces.set(FaceOrientationEnum.TOP,new ThreeFace(null,this.material,FaceOrientationEnum.TOP,404.5, thickness, 100, 0, 90, -195));
    closet_faces.set(FaceOrientationEnum.LEFT,new ThreeFace(null,this.material,FaceOrientationEnum.LEFT,thickness, 300, 100, -200, -60, -195));
    closet_faces.set(FaceOrientationEnum.RIGHT,new ThreeFace(null,this.material,FaceOrientationEnum.RIGHT,thickness, 300, 100, 200, -60, -195));
    closet_faces.set(FaceOrientationEnum.BACK,new ThreeFace(null,this.material,FaceOrientationEnum.BACK,404.5, 300, 0, 0, -60, -245.8));

    this.closet=new ThreeCloset(closet_faces,null,1,1);

    this.group.add(this.closet.draw());
    
    this.addSlotNumbered([1]);

    console.log(this.group)

    
    for(let closetFace of this.closet.getClosetFaces().entries()){
      this.closet_faces_ids.push(closetFace["1"].id());
    }

    /* for(let closetFace of faces){
      console.log(">>>>>><<<")
      console.log(closetFace);
      let parellepiped=this.generateParellepiped(closetFace["1"].width(),closetFace["1"].height()
                  ,closetFace["1"].depth(),closetFace["1"].X()
                  ,closetFace["1"].Y(),closetFace["1"].Z(),
                  this.material,this.group);
      console.log(parellepiped);
      this.closet_faces_ids.push(parellepiped);
    } */
    alert("!!!!!!!!!!!!!!!!!!!!!!")
/*     for (var i = 0; i < faces.length; i++) {
      this.closet_faces_ids.push(this.generateParellepiped(faces[i], faces[i][1], faces[i][2], faces[i][3], faces[i][4], faces[i][5], this.material, this.group));
    } */
    alert("!!!")
    this.scene.add(this.group);
    this.group.visible = false;
    this.showCloset();
    console.log(this.group.children[0].children);
    this.renderer.setClearColor(0xFFFFFF, 1);
  }

  /**
   * Shows the closet
   */
  showCloset() {
    this.group.visible = true;
  }

  /**
   * Initializes the scene's lighting.
   */
  initLighting() {
    var spotlight = new THREE.SpotLight(0x404040);
    this.camera.add(spotlight);

    spotlight.target = this.group;
    var lightAmbient = new THREE.AmbientLight(0x404040);
    this.scene.add(lightAmbient);
  }

  /**
   * Updates current closet graphical view
   */
  updateClosetGV() {
    let closetFaces=this.closet.getClosetFaces().entries();
    let closetInitialFaces=this.closet.getInitialClosetFaces().entries();
    for (let closetFace of closetFaces) {
      let closet_face=closetFace["1"].mesh();
      let closet_initial_face=closetInitialFaces.next().value["1"];
      closet_face.scale.x = this.getNewScaleValue(closet_initial_face.width(), closetFace["1"].width(), closet_face.scale.x);
      closet_face.scale.y = this.getNewScaleValue(closet_initial_face.height(), closetFace["1"].height(), closet_face.scale.y);
      closet_face.scale.z = this.getNewScaleValue(closet_initial_face.depth(), closetFace["1"].depth(), closet_face.scale.z);
      closet_face.position.x = closetFace["1"].X();
      closet_face.position.y = closetFace["1"].Y();
      closet_face.position.z = closetFace["1"].Z();
    }

    let closetSlotFaces=this.closet.getClosetSlotFaces().entries();
    for(let asd of closetSlotFaces)console.log(asd);
    let closetInitialSlotFaces=this.closet.getInitialClosetSlotFaces().entries();

    for (let closetSlotFace of closetSlotFaces) {
      console.log("->>>>>>>>>>>>")
      let closet_slot_face=closetSlotFace["1"].mesh();
      let closet_initial_slot_face=closetInitialSlotFaces.next().value["1"];
      closet_slot_face.scale.x = this.getNewScaleValue(closet_initial_slot_face.width(), closetFace["1"].width(), closet_slot_face.scale.x);
      closet_slot_face.scale.y = this.getNewScaleValue(closet_initial_slot_face.height(), closetFace["1"].height(), closet_slot_face.scale.y);
      closet_slot_face.scale.z = this.getNewScaleValue(closet_initial_slot_face.depth(), closetFace["1"].depth(), closet_slot_face.scale.z);
      closet_slot_face.position.x = closetFace["1"].X();
      closet_slot_face.position.y = closetFace["1"].Y();
      closet_slot_face.position.z = closetFace["1"].Z();
    }
  }
  /**
   * Adds a slot to the current closet
   */
  addSlot() {
    ///this.addSlotNumbered([]);
  }

  /**
   * Adds components to the current closet
   */
  addComponent(components) {
    if (components == null || components == undefined) return;
    for (let i = 0; i < components.length; i++) {
      for (let j = 0; j < components[i].length; j++) {
        if (components[i][0].designation == "Shelf") this.generateShelf(components[i][0].slot);
        if (components[i][0].designation == "Pole") this.generatePole(components[i][0].slot);
        if (components[i][0].designation == "Drawer") this.generateDrawer(components[i][0].slot);
        if (components[i][0].designation == "Hinged Door") this.generateHingedDoor(components[i][0].slot);
        if (components[i][0].designation == "Sliding Door") this.generateSlidingDoor();
      }
    }
  }

  /**
   * Generates a cylinder with given properties on a certain position relative to axis x,y and z
   */
  generatePole(slot) {
    var leftFace = this.group.getObjectById(this.closet_faces_ids[2]),
      rightFace = this.group.getObjectById(this.closet_faces_ids[3]);
    var radiusTop = 3,
      radiusBottom = 3,
      radialSegments = 20,
      heightSegments = 20,
      thetaStart = 0,
      thetaLength = Math.PI * 2;
    var openEnded = false;
    var height, x, y, z;

    var pole = new Pole(radiusTop, radiusBottom, height, radialSegments, heightSegments, openEnded, thetaStart, thetaLength);

    //If the closet has no slots, the pole's height needs to be the width of the closet
    //Otherwise the pole needs to go from the closet's left wall to a slot, 
    //from a slot to another slot or from a slot to the closet's right wall
    if (this.closet_slots_faces_ids.length == 0) {
      height = this.getCurrentClosetWidth();
      pole.changePoleHeight(height);
      x = this.calculateComponentPosition(rightFace.position.x, leftFace.position.x);
      y = this.calculateComponentPosition(rightFace.position.y, leftFace.position.y);
      z = this.calculateComponentPosition(rightFace.position.z, leftFace.position.z);

    } else if (slot == 1) { //Pole is added in between the closet's left face and first slot
      let firstSlot = this.group.getObjectById(this.closet_slots_faces_ids[0]);
      height = this.calculateDistance(leftFace.position.x, firstSlot.position.x);
      pole.changePoleHeight(height);
      x = this.calculateComponentPosition(leftFace.position.x, firstSlot.position.x);
      y = this.calculateComponentPosition(leftFace.position.y, firstSlot.position.y);
      z = this.calculateComponentPosition(leftFace.position.z, firstSlot.position.z);

    } else if (slot > 1 && slot <= this.closet_slots_faces_ids.length) { //Pole is added between slots w/ indexes [slot - 1] and [slot]
      let slotToTheLeft = this.group.getObjectById(this.closet_slots_faces_ids[slot - 2]);
      let slotToTheRight = this.group.getObjectById(this.closet_slots_faces_ids[slot - 1]);
      height = this.calculateDistance(slotToTheLeft.position.x, slotToTheRight.position.x);
      pole.changePoleHeight(height);
      x = this.calculateComponentPosition(slotToTheLeft.position.x, slotToTheRight.position.x);
      y = this.calculateComponentPosition(slotToTheLeft.position.y, slotToTheRight.position.y);
      z = this.calculateComponentPosition(slotToTheLeft.position.z, slotToTheRight.position.z);

    } else { //Pole is added between the last slot and the closet's right face
      let lastSlot = this.group.getObjectById(this.closet_slots_faces_ids[slot - 2]);
      height = this.calculateDistance(lastSlot.position.x, rightFace.position.x);
      pole.changePoleHeight(height);
      x = this.calculateComponentPosition(lastSlot.position.x, rightFace.position.x);
      y = this.calculateComponentPosition(lastSlot.position.y, rightFace.position.y);
      z = this.calculateComponentPosition(lastSlot.position.z, rightFace.position.z);
    }
    var cylinderGeometry = new THREE.CylinderGeometry(radiusTop, radiusBottom, pole.getPoleHeight(),
      radialSegments, heightSegments, openEnded, thetaStart, thetaLength);
    var poleMesh = new THREE.Mesh(cylinderGeometry, this.material);
    poleMesh.position.x = x;
    poleMesh.position.y = y;
    poleMesh.position.z = z;
    poleMesh.rotation.z = Math.PI / 2;
    //this.closet.addPole(pole);
    this.group.add(poleMesh);
    this.closet_poles_ids.push(poleMesh.id);
  }

  generateShelf(slot) {
    var leftFace = this.group.getObjectById(this.closet_faces_ids[2]);
    var rightFace = this.group.getObjectById(this.closet_faces_ids[3]);
    var width, x, y, z;

    if (slot == 0) { //If there are no slots, the width of the shelf is the same as the closet's structure
      width = this.getCurrentClosetWidth();
      x = this.calculateComponentPosition(rightFace.position.x, leftFace.position.x);
      y = this.calculateComponentPosition(rightFace.position.y, leftFace.position.y);
      z = this.calculateComponentPosition(rightFace.position.z, leftFace.position.z);
    } else if (slot == 1) { //If the slot is the first one, the shelf is added between the left wall of the closet and the slot
      let firstSlot = this.group.getObjectById(this.closet_slots_faces_ids[0]);
      width = this.calculateDistance(leftFace.position.x, firstSlot.position.x);
      x = this.calculateComponentPosition(leftFace.position.x, firstSlot.position.x);
      y = this.calculateComponentPosition(leftFace.position.y, firstSlot.position.y);
      z = this.calculateComponentPosition(leftFace.position.z, firstSlot.position.z);
    } else if (slot > 1 && slot <= this.closet_slots_faces_ids.length) { //If the chosen slot is not the first nor the last, the shelf is added between two slots
      let slotToTheLeft = this.group.getObjectById(this.closet_slots_faces_ids[slot - 2]);
      let slotToTheRight = this.group.getObjectById(this.closet_slots_faces_ids[slot - 1]);
      width = this.calculateDistance(slotToTheLeft.position.x, slotToTheRight.position.x);
      x = this.calculateComponentPosition(slotToTheLeft.position.x, slotToTheRight.position.x);
      y = this.calculateComponentPosition(slotToTheLeft.position.y, slotToTheRight.position.y);
      z = this.calculateComponentPosition(slotToTheLeft.position.z, slotToTheRight.position.z);
    } else { //If the slot is the last one, the shelf is added between the slot and the right wall of the closet
      let lastSlot = this.group.getObjectById(this.closet_slots_faces_ids[slot - 2]);
      width = this.calculateDistance(lastSlot.position.x, rightFace.position.x);
      x = this.calculateComponentPosition(lastSlot.position.x, rightFace.position.x);
      y = this.calculateComponentPosition(lastSlot.position.y, rightFace.position.y);
      z = this.calculateComponentPosition(lastSlot.position.z, rightFace.position.z);
    }

    //new Shelf([width, height, depth, x, y, z]);
    var meshID = this.generateParellepiped(width, 3, this.closet.getClosetDepth(), x, y, z, this.material, this.group);
    // this.closet.addShelf(shelf);
    this.closet_shelves_ids.push(meshID);
  }


  generateDrawer(slot) {
    var leftFace = this.group.getObjectById(this.closet_faces_ids[2]);
    var rightFace = this.group.getObjectById(this.closet_faces_ids[3]);
    var depthDrawer = 3;
    var heightDrawer = 40;
    var depthCloset = this.closet.getClosetDepth();
    var width;
    var x, y, z;
    var spaceDrawerModule = 10;

    //For now this follows the same logic as the pole, it should be changed to whatever dimensions the shelf is allowed to have
    if (this.closet_slots_faces_ids.length == 0) {
      width = this.getCurrentClosetWidth() - 4.20;
      x = this.calculateComponentPosition(rightFace.position.x, leftFace.position.x);
      y = this.calculateComponentPosition(rightFace.position.y, leftFace.position.y);
      z = this.calculateComponentPosition(rightFace.position.z, leftFace.position.z);
    } else if (slot == 1) {
      let firstSlot = this.group.getObjectById(this.closet_slots_faces_ids[0]);
      width = this.calculateDistance(leftFace.position.x, firstSlot.position.x) - 4.20;
      x = this.calculateComponentPosition(leftFace.position.x, firstSlot.position.x);
      y = this.calculateComponentPosition(leftFace.position.y, firstSlot.position.y);
      z = this.calculateComponentPosition(leftFace.position.z, firstSlot.position.z);
    } else if (slot > 1 && slot <= this.closet_slots_faces_ids.length) {
      let slotToTheLeft = this.group.getObjectById(this.closet_slots_faces_ids[slot - 2]);
      let slotToTheRight = this.group.getObjectById(this.closet_slots_faces_ids[slot - 1]);
      width = this.calculateDistance(slotToTheLeft.position.x, slotToTheRight.position.x);
      x = this.calculateComponentPosition(slotToTheLeft.position.x, slotToTheRight.position.x);
      y = this.calculateComponentPosition(slotToTheLeft.position.y, slotToTheRight.position.y);
      z = this.calculateComponentPosition(slotToTheLeft.position.z, slotToTheRight.position.z);
    } else {
      let lastSlot = this.group.getObjectById(this.closet_slots_faces_ids[slot - 2]);
      width = this.calculateDistance(lastSlot.position.x, rightFace.position.x) - 4.20;
      x = this.calculateComponentPosition(lastSlot.position.x, rightFace.position.x);
      y = this.calculateComponentPosition(lastSlot.position.y, rightFace.position.y);
      z = this.calculateComponentPosition(lastSlot.position.z, rightFace.position.z);
    }
    var module = new Module([width, depthDrawer, depthCloset, x, y - (spaceDrawerModule / 4), z], ///Base
      [width, depthDrawer, depthCloset, x, y + heightDrawer + (spaceDrawerModule / 4), z], ///Cima
      [depthDrawer, heightDrawer + (spaceDrawerModule / 4), depthCloset, x - (width / 2), y + (heightDrawer / 2), z], ///Left
      [depthDrawer, heightDrawer + (spaceDrawerModule / 4), depthCloset, x + (width / 2), y + (heightDrawer / 2), z]); ///Rigtht
    var borders = module.module_faces;
    for (var i = 0; i < borders.length; i++) {
      this.closet_modules_ids.push(this.generateParellepiped(borders[i][0],
        borders[i][1], borders[i][2], borders[i][3],
        borders[i][4], borders[i][5], this.material, this.group));
    }
    var drawer = new Drawer([width - spaceDrawerModule, depthDrawer, depthCloset, x, y + (depthDrawer / 2), z], ///Base
      [width - spaceDrawerModule, heightDrawer, depthDrawer, x, y + (heightDrawer / 2), z + (depthCloset / 2) - (depthDrawer / 2)], ///Frent
      [depthDrawer, heightDrawer, depthCloset - (depthDrawer / 2), x - (width / 2) + (spaceDrawerModule / 2), y + (heightDrawer / 2), z], ///Left
      [depthDrawer, heightDrawer, depthCloset - (depthDrawer / 2), x + (width / 2) - (spaceDrawerModule / 2), y + (heightDrawer / 2), z], ///Right
      [width - spaceDrawerModule, heightDrawer, depthDrawer, x, y + (heightDrawer / 2), z - (depthCloset / 2) + (depthDrawer / 2)]); ///Back
    var borders_drawer = drawer.drawer_faces;

    for (var i = 0; i < borders_drawer.length; i++) {
      this.closet_drawers_ids.push(this.generateParellepiped(borders_drawer[i][0],
        borders_drawer[i][1], borders_drawer[i][2], borders_drawer[i][3],
        borders_drawer[i][4], borders_drawer[i][5], this.material, this.group));
    }

    this.closet.addModule(module);
    this.closet.addDrawer(drawer);
  }

  /**
   * Calculates a pole's xyz position
   * @param {Number} leftMostCoordinate xyz coordinate of a closet's wall or a slot that is more to the left
   * @param {Number} rightMostCoordinate xyz coordinate of a closet's wall or a slot that is more to the right
   */
  calculateComponentPosition(leftMostCoordinate, rightMostCoordinate) {
    return (leftMostCoordinate + rightMostCoordinate) / 2;
  }

  calculateDistance(topPosition, bottomPosition) {
    return Math.abs(topPosition - bottomPosition);
  }

  /**
   * Adds a specified number of slots to the current closet
   * @param{array} slotsToAdd - number of slots being added
   */
  addSlotNumbered(slotsToAdd) {
    for (var i = 0; i < slotsToAdd.length; i++) {
      let slot = this.closet.addClosetSlot();
      this.closet_slots_faces_ids.push(slot.id());
      console.log(slot);
    }
    this.updateClosetGV();
  }

  /**
   * Removes a slot from the current closet
   */
  removeSlot() {
    this.closet.removeSlot();
    var closet_slot_face_id = this.closet_slots_faces_ids.pop();
    this.group.remove(this.group.getObjectById(closet_slot_face_id));
    this.updateClosetGV();
  }

  /**
   * Changes the dimensions of the closet
   * @param {number} width Number with the closet width
   * @param {number} height Number with the closet height
   * @param {number} depth Number with the closet depth
   */
  changeClosetDimensions(width, height, depth) {
    this.resizeFactor();

    this.closet.changeClosetWidth(this.resizeVec[this.WIDTH] * width);
    this.closet.changeClosetHeight(this.resizeVec[this.HEIGHT] * height);
    this.closet.changeClosetDepth((this.resizeVec[this.DEPTH] * depth) - 195.8);

    this.updateClosetGV();
  }

  /**
   * Method that populates the vector responsible to resize
   */
  resizeFactor() {
    var i;
    for (i = 0; i < this.NUMBER_DIMENSIONS; i++) {
      this.resizeVec[i] = this.initialDimensions[i] / this.websiteDimensions[i];
    }
  }
  /**
   * Applies the texture to the closet.
   * @param {string} texture - texture being applied.
   */
  applyTexture(texture) {
    this.textureLoader.load(texture, tex => {
      this.material.map = tex
    })
  }

  /**
   * Changes the closet's material's shininess.
   * @param {number} shininess - new shininess value
   */
  changeShininess(shininess) {
    this.material.shininess = shininess;
  }

  /**
   * Changes the closet's material's color.
   * @param {number} color 
   */
  changeColor(color) {
    this.material.color.setHex(color);
  }

  /**
   * Changes the current closet slots
   * @param {number} slots Number with the new closet slots
   */
  changeClosetSlots(slots, slotWidths) {
    var newSlots = this.closet.computeNewClosetSlots(slots);
    if (newSlots > 0) {
      for (var i = 0; i < newSlots; i++) {
        this.addSlot();
      }
    } else {
      newSlots = -newSlots;
      if (newSlots > 0) {
        for (var i = 0; i < newSlots; i++) {
          this.removeSlot();
        }
      }
    }
    /* if(slotWidths.length > 0){
        updateSlotWidths(slotWidths);
    } */
    this.updateClosetGV();
  }

  /**
   * 
   * @param {number[]} slotWidths 
   */
  updateSlotWidths(slotWidths) {
    for (let i = 0; i < slotWidths.length; i++) {
      var closet_face = this.group.getObjectById(this.closet_slots_faces_ids[i]);
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
  generateParellepiped(width, height, depth, x, y, z, material, group) {
    var parellepipedGeometry = new THREE.CubeGeometry(width, height, depth);
    var parellepiped = new THREE.Mesh(parellepipedGeometry, material);
    parellepiped.position.x = x;
    parellepiped.position.y = y;
    parellepiped.position.z = z;
    this.group.add(parellepiped);
    return parellepiped.id;
  }

  /**
   * Animates the scene
   */
  animate() {
    /*         var instance = this;
            setTimeout(function () {
        
            }, 1000 / 60)
            //TODO: re-enable frame cap  */

    requestAnimationFrame(() => this.animate());
    this.render();
  }

  /**
   * Renders the scene
   */
  render() {
    this.renderer.render(this.scene, this.camera);
  }

  /**
   * Initializes the graphic representation controls
   */
  initControls() {
    this.controls = new THREE.OrbitControls(this.camera, this.renderer.domElement);

    this.controls.target = new THREE.Vector3(0, 0, 0);

    this.controls.enableDamping = false; // an animation loop is required when either damping or auto-rotation are enabled
    this.controls.dampingFactor = 0.25;

    this.controls.screenSpacePanning = false;
    this.controls.minDistance = 100;
    this.controls.maxDistance = 500;

    this.controls.maxPolarAngle = Math.PI / 2;
  }

  /**
   * Initializes the graphic representation camera
   */
  initCamera() {
    this.camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 10, 1000);
    this.camera.position.y = 400;
    this.camera.position.z = 400;
    this.camera.rotation.x = .70;
  }

  /**
   * Computes the new scale value based on the initial scale value, new scale value and the current scale value
   * @param {number} initialScaleValue Number with the initial scale value
   * @param {number} newScaleValue Number with the new scale value
   * @param {number} currentScaleValue Number with the current scale value
   */
  getNewScaleValue(initialScaleValue, newScaleValue, currentScaleValue) {
    if (initialScaleValue == 0) return 0;
    return (newScaleValue * 1) / initialScaleValue;
  }

  /**
   * Represents the action that occurs when any keyboard key is pressed (key down),
   * which is blocking its action (disabling it).
   * @param {*} event 
   */
  onKeyDown(event) {
    event.preventDefault();
    alert("entro no three");
    // switch (event.keyCode) {
    //     case 37:
    //         alert('left');
    //         break;
    //     case 38:
    //         alert('up');
    //         break;
    //     case 39:
    //         alert('right');
    //         break;
    //     case 40:
    //         alert('down');
    //         break;
    // }
  }

  /**
 * Represents the action that occurs when the mouse's left button is pressed (mouse down),
 * which is recognizing the object being clicked on, setting it as the selected one if
 * it is a slot and disabling the rotation control
 */
  onMouseDown(event) {
    event.preventDefault();
    var context = this;
    this.raycaster.setFromCamera(this.mouse, this.camera);

    //Finds all intersected objects (closet faces)
    var intersects = this.raycaster.intersectObjects(this.scene.children[0].children);

    //Checks if any closet face was intersected
    if (intersects.length > 0) {

      //Gets the closest (clicked) object
      var face = intersects[0].object;

      //Checks if the selected closet face is a slot 
      for (var i = 0; i < this.closet_slots_faces_ids.length; i++) {
        var closet_face = this.group.getObjectById(this.closet_slots_faces_ids[i]);
        if (closet_face == face) {
          //Disables rotation while moving the slot
          this.controls.enabled = false;
          //Sets the selection to the current slot
          this.selected_slot = face;
          if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
            this.offset = this.intersection.x - this.selected_slot.position.x;
          }
        }
      }

      //Checks if the selected object is a shelf
      for (let j = 0; j < this.closet_shelves_ids.length; j++) {
        let shelf = this.group.getObjectById(this.closet_shelves_ids[j]);
        if (shelf == face) {
          this.controls.enabled = false;
          this.selected_component = face;
          if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
            this.offset = this.intersection.y - this.selected_component.position.y;
          }
        }
      }

      //Checks if the selected object is a pole
      for (let j = 0; j < this.closet_poles_ids.length; j++) {
        let pole = this.group.getObjectById(this.closet_poles_ids[j]);
        if (pole == face) {
          this.controls.enabled = false;
          this.selected_component = face;
          if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
            this.offset = this.intersection.y - this.selected_component.position.y;
          }
        }
      }

      //Checks if the selected closet face is a face
      if (this.group.getObjectById(this.closet_faces_ids[3]) == face ||
        this.group.getObjectById(this.closet_faces_ids[2]) == face) {
        //Disables rotation while moving the face
        this.controls.enabled = false;
        //Sets the selection to the current face
        this.selected_face = face;
        if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
          this.offset = this.intersection.x - this.selected_face.position.x;
        }
      }

      if (this.canMoveComponents) {
        var flagOpen = false;
        var flagClose = false;
        var j = 0;

        //Checks if the selected object is a sliding door

        while (!flagOpen && !flagClose && j < this.closet_sliding_doors_ids.length) {
          this.slidingDoor = this.group.getObjectById(this.closet_sliding_doors_ids[j]);
          if (this.slidingDoor == face) {
            this.controls.enabled = false;
            if (this.slidingDoor.position.x < 0) {
              flagClose = true; //"Closing" ==> slide door to the right
            } else {
              flagOpen = true; //"Opening" ==> slide door to the left
            }
          }
          j++;
        }

        if (flagOpen) {
          requestAnimationFrame(function () {
            context.slideDoorToLeft();
          });
        } else if (flagClose) {
          requestAnimationFrame(function () {
            context.slideDoorToRight();
          });
        }

        flagOpen = false;
        flagClose = false;
        j = 0;

        while (!flagOpen && !flagClose && j < this.closet_drawers_ids.length) {
          //Always get the front face of any drawer at index 5*j+1
          var drawer_front_face = this.group.getObjectById(this.closet_drawers_ids[5 * j + 1]);
          //Check if the selected object is a drawer's front face
          if (drawer_front_face == face) {
            this.controls.enabled = false;
            var drawer_base_face = this.group.getObjectById(this.closet_drawers_ids[5 * j]);
            var drawer_left_face = this.group.getObjectById(this.closet_drawers_ids[5 * j + 2]);
            var drawer_right_face = this.group.getObjectById(this.closet_drawers_ids[5 * j + 3]);
            var drawer_back_face = this.group.getObjectById(this.closet_drawers_ids[5 * j + 4]);
            if (drawer_front_face.position.z >= -50) {
              flagClose = true;
            } else {
              flagOpen = true;
            }
          }
          j++;
        }

        if (flagOpen) {
          requestAnimationFrame(function () {
            context.openDrawer(drawer_front_face, drawer_back_face, drawer_base_face, drawer_left_face, drawer_right_face);
          });
        } else if (flagClose) {
          requestAnimationFrame(function () {
            context.closeDrawer(drawer_front_face, drawer_back_face, drawer_base_face, drawer_left_face, drawer_right_face);
          });
        }

        j = 0;
        flagOpen = false;
        flagClose = false;
        while (!flagOpen && !flagClose && j < this.closet_hinged_doors_ids.length) {
          this.hingedDoor = this.group.getObjectById(this.closet_hinged_doors_ids[j]);
          var closet_face = this.group.getObjectById(this.closet_faces_ids[0]);
          if (this.hingedDoor == face) {
            this.controls.enabled = false;
            if (this.hingedDoor.rotation.y < 0) {
              flagClose = true;
            } else {
              flagOpen = true;
            }
          }
          j++;
        }

        if (flagOpen) {
          requestAnimationFrame(function () {
            context.openHingedDoor();
          });
        } else if (flagClose) {
          requestAnimationFrame(function () {
            context.closeHingedDoor();
          });
        }
      }
    }
  }


  slideDoorToLeft() {
    if (this.doesClosetHaveOpenDrawers()) {
      this.waitingDoors.push(this.slideDoorToLeftAnimation);
      this.closeAllOpenDrawers();
    } else {
      this.slideDoorToLeftAnimation();
    }
  }

  slideDoorToLeftAnimation() {
    let closet_left = this.group.getObjectById(this.closet_faces_ids[2]);
    let distanceFromDoorToLeftFace = Math.abs(this.slidingDoor.position.x - closet_left.position.x);
    let position = (Math.abs(closet_left.position.x - closet_left.geometry.parameters.width) / 2) - 2;
    if (position < distanceFromDoorToLeftFace) {
      this.slidingDoor.translateX(-1);
      var context = this;
      requestAnimationFrame(function () {
        context.slideDoorToLeft();
      });
      this.render();
      this.controls.update();
    }
  }

  slideDoorToRight() {
    if (this.doesClosetHaveOpenDrawers()) {
      this.waitingDoors.push(this.slideDoorToRightAnimation);
      this.closeAllOpenDrawers();
    } else {
      this.slideDoorToRightAnimation();
    }
  }

  slideDoorToRightAnimation() {
    let closet_right = this.group.getObjectById(this.closet_faces_ids[3]);
    let distanceFromDoorToRightFace = Math.abs(this.slidingDoor.position.x - closet_right.position.x);
    let position = (Math.abs(closet_right.position.x + closet_right.geometry.parameters.width) / 2) - 2;
    if (position < distanceFromDoorToRightFace) {
      var context = this;
      this.slidingDoor.translateX(1);
      requestAnimationFrame(function () {
        context.slideDoorToRight();
      });
      this.render();
      this.controls.update();
    }
  }

  openHingedDoor() {
    if (this.hingedDoor.rotation.y > (-Math.PI / 2)) {
      var rotationX = (this.hingedDoor.geometry.parameters.width / 2);
      this.hingedDoor.translateX(-rotationX);
      this.hingedDoor.rotation.y -= Math.PI / 100;
      this.hingedDoor.translateX(rotationX);
      var context = this;
      requestAnimationFrame(function () {
        context.openHingedDoor();
      });
      this.render();
      this.controls.update();
    }
  }

  closeHingedDoor() {
    var hingedDoorSlot = this.getHingedDoorSlot(this.hingedDoor);
    if (this.doesSlotHaveOpenDrawers(hingedDoorSlot)) {
      this.waitingDoors.push(this.closeHingedDoorAnimation);
      this.closeSlotOpenDrawers(hingedDoorSlot);
    } else {
      this.closeHingedDoorAnimation();
    }
  }

  checkAddDrawerTriggers(slot) {
    this.generateDrawer(slot);
    if (this.doesSlotHaveHingedDoor(slot)) {
      if (!this.isHingedDoorClosed) {
        this.hingedDoor = this.group.getObjectById(this.closet_hinged_doors_ids[slot - 1]);
        requestAnimationFrame(this.openHingedDoor);
      }
    }
    if (this.doesClosetHaveSlidingDoors()) {
      var front_door = this.group.getObjectById(this.closet_sliding_doors_ids[0]);
      var back_door = this.group.getObjectById(this.closet_sliding_doors_ids[1]);
      //Front face of the last added drawer is always at index length - 4
      var addedDrawer = this.group.getObjectById(this.closet_drawers_ids[this.closet_drawers_ids.length - 4]);
      if (addedDrawer.position.x < 0) {
        var context = this;
        if (front_door.position.x < 0) {
          this.slidingDoor = front_door;
          context.slideDoorToRight();
        }
        if (back_door.position.x < 0) {
          this.slidingDoor = back_door;
          context.slideDoorToLeft();
        }
      } else {
        if (front_door.position.x > 0) {
          this.slidingDoor = front_door;
          context.slideDoorToLeft();
        }
        if (back_door.position.x > 0) {
          this.slidingDoor = back_door;
          context.slideDoorToRight();
        }
      }

    }
  }

  checkAddSlidingDoorTriggers() {
    if (this.doesClosetHaveHingedDoors()) {
      alert("There are closet slots that have hinged doors!");
    } else {
      if (this.doesClosetHaveOpenDrawers()) {
        if (this.openDrawers.length > 0) {
          this.waitingDoors.push(function () {
            this.generateSlidingDoor();
          });
          this.closeAllOpenDrawers();
        } else {
          this.generateSlidingDoor();
        }
      } else {
        this.generateSlidingDoor();
      }
    }
  }

  checkAddHingedDoorTriggers(slot) {
    if (this.doesSlotHaveHingedDoor(slot)) {
      alert("This slot already has a door!");
    } else if (this.doesClosetHaveSlidingDoors()) {
      alert("The closet already has sliding doors!");
    } else {
      if (this.doesSlotHaveOpenDrawers(slot)) {
        if (this.openDrawers.length > 0) {
          this.waitingDoors.push(function () {
            this.addHingedDoor(slot);
          });
          this.closeSlotOpenDrawers(slot);
        } else {
          this.addHingedDoor(slot);
        }
      } else {
        this.addHingedDoor(slot);
      }
    }
  }

  addHingedDoor(slot) {
    if (this.openDrawers.length == 0) {
      this.generateHingedDoor(slot);
    } else {
      this.generateHingedDoor(slot);
    }
  }

  generateHingedDoor(slot) {
    var leftFace = this.group.getObjectById(this.closet_faces_ids[2]);
    var rightFace = this.group.getObjectById(this.closet_faces_ids[3]);
    var depth = 3;
    var height = this.closet.getClosetHeight();
    var depth_closet = this.closet.getClosetDepth();
    var width;
    var x, y, z;

    //For now this follows the same logic as the pole, it should be changed to whatever dimensions the shelf is allowed to have
    if (this.closet_slots_faces_ids.length == 0) {
      width = this.closet.getClosetWidth();
      x = this.calculateComponentPosition(rightFace.position.x, leftFace.position.x);
      y = this.calculateComponentPosition(rightFace.position.y, leftFace.position.y);
      z = this.calculateComponentPosition(rightFace.position.z, leftFace.position.z);
    } else if (slot == 1) {
      let firstSlot = this.group.getObjectById(this.closet_slots_faces_ids[0]);
      width = this.calculateDistance(leftFace.position.x, firstSlot.position.x);
      x = this.calculateComponentPosition(leftFace.position.x, firstSlot.position.x);
      y = this.calculateComponentPosition(leftFace.position.y, firstSlot.position.y);
      z = this.calculateComponentPosition(leftFace.position.z, firstSlot.position.z);
    } else if (slot > 1 && slot <= this.closet_slots_faces_ids.length) {
      let slotToTheLeft = this.group.getObjectById(this.closet_slots_faces_ids[slot - 2]);
      let slotToTheRight = this.group.getObjectById(this.closet_slots_faces_ids[slot - 1]);
      width = this.calculateDistance(slotToTheLeft.position.x, slotToTheRight.position.x);
      x = this.calculateComponentPosition(slotToTheLeft.position.x, slotToTheRight.position.x);
      y = this.calculateComponentPosition(slotToTheLeft.position.y, slotToTheRight.position.y);
      z = this.calculateComponentPosition(slotToTheLeft.position.z, slotToTheRight.position.z);
    } else {
      let lastSlot = this.group.getObjectById(this.closet_slots_faces_ids[slot - 2]);
      width = this.calculateDistance(lastSlot.position.x, rightFace.position.x);
      x = this.calculateComponentPosition(lastSlot.position.x, rightFace.position.x);
      y = this.calculateComponentPosition(lastSlot.position.y, rightFace.position.y);
      z = this.calculateComponentPosition(lastSlot.position.z, rightFace.position.z);
    }

    var meshID = this.generateParellepiped(width, height, depth, x, y, z + (depth_closet / 2), this.material, this.group);
    var hingedDoor = new HingedDoor([width, height, depth, x, y, z + (depth_closet / 2)], slot, meshID);
    this.closet.addHingedDoor(hingedDoor);
    this.closet_hinged_doors_ids.push(meshID);
  }

  generateSlidingDoor() {
    var leftFace = this.group.getObjectById(this.closet_faces_ids[2]);
    var rightFace = this.group.getObjectById(this.closet_faces_ids[3]);
    var topFace = this.group.getObjectById(this.closet_faces_ids[1]);
    var bottomFace = this.group.getObjectById(this.closet_faces_ids[0]);
    var height = this.closet.getClosetHeight();
    var width = this.closet.getClosetWidth();
    var z = this.group.getObjectById(this.closet_faces_ids[4]).position.z + this.closet.getClosetDepth();

    var thickness = 4.20;

    var front_door = new SlidingDoor([width / 2, (height - thickness), 5, leftFace.position.x / 2, leftFace.position.y, z + 7]);

    var front_frame = new Module([width, thickness, 5, bottomFace.position.x, bottomFace.position.y, z + 7],
      [width, thickness, 5, topFace.position.x, topFace.position.y, z + 7],
      [thickness, height, 5, leftFace.position.x, leftFace.position.y, z + 7],
      [thickness, height, 5, rightFace.position.x, rightFace.position.y, z + 7]);

    var back_door = new SlidingDoor([width / 2, (height - thickness), 5, rightFace.position.x / 2, rightFace.position.y, z + 2]);

    var back_frame = new Module([width, thickness, 5, bottomFace.position.x, bottomFace.position.y, z + 2],
      [width, thickness, 5, topFace.position.x, topFace.position.y, z + 2],
      [thickness, height, 5, leftFace.position.x, leftFace.position.y, z + 2],
      [thickness, height, 5, rightFace.position.x, rightFace.position.y, z + 2]);

    //Adds front frame
    var borders = front_frame.module_faces;
    for (var i = 0; i < borders.length; i++) {
      this.generateParellepiped(borders[i][0],
        borders[i][1], borders[i][2], borders[i][3],
        borders[i][4], borders[i][5], this.material, this.group);
    }

    //Adds front door
    var front_door_mesh_id = this.generateParellepiped(
      front_door.sliding_door_axes[0],
      front_door.sliding_door_axes[1],
      front_door.sliding_door_axes[2],
      front_door.sliding_door_axes[3],
      front_door.sliding_door_axes[4],
      front_door.sliding_door_axes[5],
      this.material, this.group);

    //Adds back door
    var back_door_mesh_id = this.generateParellepiped(
      back_door.sliding_door_axes[0],
      back_door.sliding_door_axes[1],
      back_door.sliding_door_axes[2],
      back_door.sliding_door_axes[3],
      back_door.sliding_door_axes[4],
      back_door.sliding_door_axes[5],
      this.material, this.group);

    this.closet.addSlidingDoor(front_door);
    this.closet.addSlidingDoor(back_door);

    this.closet_sliding_doors_ids.push(front_door_mesh_id);
    this.closet_sliding_doors_ids.push(back_door_mesh_id);

    //Adds back frame
    var borders = back_frame.module_faces;
    for (var i = 0; i < borders.length; i++) {
      this.generateParellepiped(borders[i][0],
        borders[i][1], borders[i][2], borders[i][3],
        borders[i][4], borders[i][5], this.material, this.group);
    }
  }


  closeSlotOpenDrawers(slot) {
    var i = 0;
    var index = 0;
    var closet_front = Math.abs(this.group.getObjectById(this.closet_faces_ids[4]).position.z);
    while (i < this.closet_drawers_ids.length) {
      if (this.closet.drawers[index].slotId == slot) {
        console.log(this.closet.drawers[index]);
        var drawer_front_face = this.group.getObjectById(this.closet_drawers_ids[5 * index + 1]);
        if (drawer_front_face.position.z > closet_front) {
          var drawer_base_face = this.group.getObjectById(this.closet_drawers_ids[5 * index]);
          var drawer_left_face = this.group.getObjectById(this.closet_drawers_ids[5 * index + 2]);
          var drawer_right_face = this.group.getObjectById(this.closet_drawers_ids[5 * index + 3]);
          var drawer_back_face = this.group.getObjectById(this.closet_drawers_ids[5 * index + 4]);
          this.closeDrawer(drawer_front_face, drawer_back_face, drawer_base_face, drawer_left_face, drawer_right_face);
        }
      }
      i += 5;
      index++;
    }
  }

  closeAllOpenDrawers() {
    var i = 0;
    var index = 0;
    var closet_front = Math.abs(this.group.getObjectById(this.closet_faces_ids[4]).position.z);
    while (i < this.closet_drawers_ids.length) {
      var drawer_front_face = this.group.getObjectById(this.closet_drawers_ids[5 * index + 1]);
      if (drawer_front_face.position.z > closet_front) {
        var drawer_base_face = this.group.getObjectById(this.closet_drawers_ids[5 * index]);
        var drawer_left_face = this.group.getObjectById(this.closet_drawers_ids[5 * index + 2]);
        var drawer_right_face = this.group.getObjectById(this.closet_drawers_ids[5 * index + 3]);
        var drawer_back_face = this.group.getObjectById(this.closet_drawers_ids[5 * index + 4]);
        this.closeDrawer(drawer_front_face, drawer_back_face, drawer_base_face, drawer_left_face, drawer_right_face);
      }
      i += 5;
      index++;
    }
  }


  doesSlotHaveHingedDoor(slot) {
    for (let i = 0; i < this.closet_hinged_doors_ids.length; i++) {
      if (this.closet.hingedDoors[i].slotId == slot) {
        return true;
      }
    }
    return false;
  }

  doesSlotHaveOpenDrawers(slot) {
    var closet_front = Math.abs(this.group.getObjectById(this.closet_faces_ids[4]).position.z);
    var index = 0;
    for (let i = 0; i < this.closet_drawers_ids.length; i += 6) {
      if (this.closet.drawers[index].slotId == slot) {
        return this.group.getObjectById(this.closet_drawers_ids[5 * index + 1]).position.z
          >= closet_front;
      }
      index++;
    }
    return false;
  }

  doesClosetHaveOpenDrawers() {
    var closet_front = Math.abs(this.group.getObjectById(this.closet_faces_ids[4]).position.z);
    var index = 0;
    for (let i = 0; i < this.closet_drawers_ids.length; i += 6) {
      if (i > 0) index = i - 5;
      return this.group.getObjectById(this.closet_drawers_ids[5 * index + 1]).position.z
        >= closet_front;
    }
    return false;
  }

  doesClosetHaveHingedDoors() {
    return this.closet.hingedDoors.length != 0;
  }

  doesClosetHaveSlidingDoors() {
    return this.closet.slidingDoors.length != 0;
  }


  closeHingedDoorAnimation() {
    if (this.hingedDoor.rotation.y < 0) {
      var rotationX = this.hingedDoor.geometry.parameters.width / 2;
      this.hingedDoor.translateX(-rotationX);
      this.hingedDoor.rotation.y += Math.PI / 100;
      this.hingedDoor.translateX(rotationX);
      var context = this;
      requestAnimationFrame(function () {
        context.closeHingedDoor();
      });
      this.render();
      this.controls.update();
    } else {
      this.isHingedDoorClosed = true;
    }
  }

  getHingedDoorSlot(hingedDoorMesh) {
    for (let i = 0; i < this.closet.hingedDoors.length; i++) {
      if (hingedDoorMesh.id == this.closet.hingedDoors[i].meshId) {
        return this.closet.hingedDoors[i].slotId;
      }
    }
    return;
  }

  openDrawer(drawer_front_face, drawer_back_face, drawer_base_face, drawer_left_face, drawer_right_face) {
    if (drawer_front_face.position.z <= -50) {
      drawer_front_face.translateZ(1);
      drawer_back_face.translateZ(1);
      drawer_base_face.translateZ(1);
      drawer_left_face.translateZ(1);
      drawer_right_face.translateZ(1);
      var context = this;
      requestAnimationFrame(function () {
        context.openDrawer(drawer_front_face, drawer_back_face, drawer_base_face, drawer_left_face, drawer_right_face);
      });
      this.render();
      this.controls.update();
    } else {
      this.openDrawers.push(true);
    }
  }

  closeDrawer(drawer_front_face, drawer_back_face, drawer_base_face, drawer_left_face, drawer_right_face) {
    var closet_front = this.group.getObjectById(this.closet_faces_ids[4]).position.z;
    if (drawer_back_face.position.z > closet_front + 3) {
      drawer_front_face.translateZ(-1);
      drawer_back_face.translateZ(-1);
      drawer_base_face.translateZ(-1);
      drawer_left_face.translateZ(-1);
      drawer_right_face.translateZ(-1);
      var context = this;
      requestAnimationFrame(function () {
        context.closeDrawer(drawer_front_face, drawer_back_face, drawer_base_face, drawer_left_face, drawer_right_face);
      });
      this.render();
      this.controls.update();
    } else {
      this.openDrawers.pop();
      if (this.openDrawers.length == 0) {
        while (this.waitingDoors.length != 0) {
          this.waitingDoors.pop()();
        }
      }
    }
  }


  /**
   * Represents the action that occurs when the mouse's left button is released, which is
   * setting the selected object as null, since it is no longer being picked, and enabling
   * the rotation control
   */
  onMouseUp(event) {
    //Enables rotation again
    this.controls.enabled = true;
    //Sets the selected slot to null (the slot stops being selected)
    this.selected_slot = null;
    //Sets the selected face to null (the face stops being selected)
    this.selected_face = null;
    //Sets the selected closet component to null (the component stops being selected)
    this.selected_component = null;
  }

  /**
   * Represents the action that occurs when the mouse is dragged (mouse move), which
   * is interacting with the previously picked object on mouse down (moving it accross
   * the x axis)
   */
  onMouseMove(event) {
    event.preventDefault();

    var rect = event.target.getBoundingClientRect();
    var x = event.clientX;
    var y = event.clientY;
    this.mouse.x = (x - rect.left) / (this.canvasWebGL.clientWidth / 2.0) - 1.0; //Get mouse x position
    this.mouse.y = -((y - rect.bottom) / (this.canvasWebGL.clientHeight / 2.0) + 1.0); //Get mouse y position
    this.raycaster.setFromCamera(this.mouse, this.camera); //Set raycast position

    //If the selected object is a slot
    if (this.selected_slot && this.canMoveSlots) {
      this.moveSlot();
      return;
    }

    //If the selected object is a closet face
    if (this.selected_face && this.canMoveCloset) {
      this.moveFace();
      return;
    }

    //If the selected object is a closet pole or shelf
    if (this.selected_component && this.canMoveComponents) {
      this.moveComponent();
      return;
    }

    var intersects = this.raycaster.intersectObjects(this.scene.children[0].children);
    if (intersects.length > 0) {
      //Updates plane position to look at the camera
      var face = intersects[0].object;
      this.plane.setFromNormalAndCoplanarPoint(this.camera.position, face.position);

      if (this.hovered_object !== face) this.hovered_object = face;
    } else {
      if (this.hovered_object !== null) this.hovered_object = null;
    }
  }

  /**
   * Moves the slot across the defined plan that intersects the closet, without overlapping the closet's faces
   */
  moveSlot() {
    if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
      var newPosition = this.intersection.x - this.offset; //Subtracts the offset to the x coordinate of the intersection point
      var valueCloset = this.group.getObjectById(this.closet_faces_ids[2]).position.x;
      if (Math.abs(newPosition) < Math.abs(valueCloset)) { //Doesn't allow the slot to overlap the faces of the closet
        this.selected_slot.position.x = newPosition;
      }
    }
  }

  /**
   * Moves the face across the defined plan that intersects the closet, without overlapping the closet's slots
   */
  moveFace() {
    if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
      if (this.selected_face == this.group.getObjectById(this.closet_faces_ids[3])) { //If the selected face is the right one

        var rightFacePosition = this.intersection.x - this.offset + this.selected_face.position.x; //Position of the right closet face

        if (this.closet_slots_faces_ids.length == 0) { //If there are no slots
          this.selected_face.position.x = rightFacePosition;
          this.closet.changeClosetWidth(rightFacePosition);
          this.updateClosetGV();

        } else {
          var rightSlotPosition = this.group.getObjectById(this.closet_slots_faces_ids[this.closet_slots_faces_ids.length - 1]).position.x; //Position of the last (more to the right) slot 

          if (rightFacePosition - rightSlotPosition > rightSlotPosition) { //Checks if right face doesn't intersect the slot
            this.selected_face.position.x = rightFacePosition;
            this.closet.changeClosetWidth(rightFacePosition);
            this.updateClosetGV();
          }
        }
      } else if (this.selected_face == this.group.getObjectById(this.closet_faces_ids[2])) { //If the selected face is the left one

        var leftFacePosition = -this.intersection.x - this.offset - this.selected_face.position.x; //Position of the left closet face

        if (this.closet_slots_faces_ids.length == 0) { //If there are no slots
          this.selected_face.position.x = leftFacePosition;
          this.closet.changeClosetWidth(leftFacePosition);
          this.updateClosetGV();
        } else {
          var leftSlotPosition = -this.group.getObjectById(this.closet_slots_faces_ids[0]).position.x; //Position of the first (more to the left) slot

          if (leftFacePosition - leftSlotPosition > leftSlotPosition) { //Checks if left face doesn't intersect the slot
            this.selected_face.position.x = leftFacePosition;
            this.closet.changeClosetWidth(leftFacePosition);
            this.updateClosetGV();
          }
        }
      }
    }
  }

  /**
   * Moves a component across the y axis without overlapping the slots planes or the closets planes
   */
  moveComponent() {
    if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
      var computedPosition = this.intersection.y - this.offset; //The component's new computed position on the yy axis

      if (computedPosition < this.group.getObjectById(this.closet_faces_ids[1]).position.y &&
        computedPosition >= this.group.getObjectById(this.closet_faces_ids[0]).position.y) {
        this.selected_component.position.y = computedPosition; //Sets the new position as long as the component stays within the closet boundaries
      }
    }

    var intersects = this.raycaster.intersectObjects(this.scene.children[0].children);
    if (intersects.length > 0) {
      //Updates plane position to look at the camera
      var object = intersects[0].object;
      this.plane.setFromNormalAndCoplanarPoint(this.camera.position, object.position);

      if (this.hovered_object !== object) this.hovered_object = object;
    } else if (this.hovered_object !== null) this.hovered_object = null;
  }

  /**
   * Returns the current closet width
   */
  getCurrentClosetWidth() {
    return this.closet.getClosetWidth();
  }

  /**
   * Returns the current closet height
   */
  getCurrentClosetHeight() {
    return this.closet.getClosetHeight();
  }

  /**
   * Returns the current closet depth
   */
  getCurrentClosetDepth() {
    return this.closet.getClosetDepth();
  }
}