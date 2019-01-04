//@ts-check
import 'three/examples/js/controls/OrbitControls'
import * as THREE from 'three'
import Closet from './Closet'
import Pole from './Pole'
import Drawer from './Drawer'
import Module from './Module'
import SlidingDoor from './SlidingDoor'
import Shelf from './Shelf'
import HingedDoor from './HingedDoor'
import store from "./../store";
import {
  SET_RESIZE_VECTOR_GLOBAL, SET_CUSTOMIZED_PRODUCT_COMPONENTS, REMOVE_CUSTOMIZED_PRODUCT_COMPONENT, SET_COMPONENT_TO_REMOVE, SET_DOORS_FLAG
} from "./../store/mutation-types.js";

export default class ProductRenderer {

  /* Flags used to interact with the graphical representation on certain steps of the wizard */

  /**
   * Flag used to control if the user can or can not resize the closet.
   */
  canMoveCloset;


  /**
   * Flag used to control if the user can or can not move the closet's slots.
   */
  canMoveSlots;


  /**
   * Flag used to control if the user can or can not move the closet's components.
   */
  canMoveComponents;

  /**
   * Instance variable representing the camera through which the scene is rendered.
   * @type {THREE.Camera}
   */
  camera;

  /**
   * @type {THREE.OrbitControls}
   */
  controls;

  /**
   * @type {THREE.Scene}
   */
  scene;

  /**
   * @type {THREE.WebGLRenderer}
   */
  renderer;

  /**
   * @type {THREE.Group}
   */
  group;

  /**
   * @type {THREE.MeshPhongMaterial}
   */
  material;

  /**
   * @type {Closet}
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

  /**
 * Global variable with the current closet poles ids (Mesh IDs from Three.js)
 * @type {number[]}
 */
  closet_poles_ids;

  /**
 * Global variable with the current closet modules ids (Mesh IDs from Three.js)
 * @type {number[]}
 */
  closet_modules_ids;

  /**
 * Global variable with the current closet drawers ids (Mesh IDs from Three.js)
 * @type {number[]}
 */
  closet_drawers_ids;

  /**
 * Global variable with the current closet hinged doors ids (Mesh IDs from Three.js)
 * @type {number[]}
 */
  closet_hinged_doors_ids;

  /**
 * Global variable with the current closet sliding doors ids (Mesh IDs from Three.js)
 * @type {number[]}
 */
  closet_sliding_doors_ids;

  /**
   * Instance variable with the WebGL canvas
   * @type {HTMLCanvasElement}
   */
  canvasWebGL;

  /**
   * Instance variables that represent the currently selected slot (null if none)
   */
  selected_slot;

  /**
   * Instance variables that represent the currently selected face (null if none)
   */
  selected_face;

  /**
   * Instance variables that represent the currently selected component (null if none)
   */
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
   * @type {THREE.Raycaster}
   */
  raycaster;

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

    this.websiteDimensions = [];

    this.canMoveCloset = false;
    this.canMoveSlots = false;
    this.canMoveComponents = false;

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

    //enable shadows
    this.renderer.shadowMap.enabled = true;
    //Available types: BasicShadowMap, PCFShadowMap, PCFSoftShadowMap
    this.renderer.shadowMap.type = THREE.BasicShadowMap;

    this.scene = new THREE.Scene();
    this.group = new THREE.Group();

    this.initCloset();
    this.initCamera();
    this.initControls();
    this.initLighting();
    this.initPanorama();

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

    this.animate();
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

    this.closet = new Closet([404.5, thickness, 100, 0, -210, -195], //Bottom
      [404.5, thickness, 100, 0, 90, -195], //Top
      [thickness, 300, 100, -200, -60, -195], //Left
      [thickness, 300, 100, 200, -60, -195], //Right
      [404.5, 300, 0, 0, -60, -245.8], 0); //Back

    var faces = this.closet.closet_faces;
    
    //A MeshPhongMaterial allows for shiny surfaces
    //A soft white light is being as specular light
    //The shininess value is the same as the matte finishing's value
    this.material = new THREE.MeshPhongMaterial();
    this.material.specular = new THREE.Color(0x404040);
    this.material.shininess = 20;
    this.material.map;

    for (var i = 0; i < faces.length; i++) {
      this.closet_faces_ids.push(this.generateParellepiped(faces[i][0], faces[i][1], faces[i][2], faces[i][3], faces[i][4], faces[i][5], this.material, this.group, ""));
    }
    this.scene.add(this.group);
    this.group.visible = false;
    this.renderer.setClearColor(0xFFFFFF, 1);
  }

  /**
   * Shows the closet
   */
  showCloset() {
    this.group.visible = true;
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
  * Initializes the panorama background.
  */
  initPanorama() {
    var sphereGeometry = new THREE.SphereBufferGeometry(430, 60, 40);
    sphereGeometry.scale(-1, 1, 1);

    var roomMaterial = new THREE.MeshBasicMaterial({
      map: new THREE.TextureLoader().load("./../../src/assets/background.jpg")
    });

    var room = new THREE.Mesh(sphereGeometry, roomMaterial);
    this.scene.add(room);
  }

  /**
   * Initializes the scene's lighting.
   */
  initLighting() {
    //soft white light coming from the sky and soft brown reflecting from the ground
    var hemisphereLight = new THREE.HemisphereLight(0x404040, 0xffe5a3, 0.5);
    this.scene.add(hemisphereLight);

    //light bulb positioned on the right of the camera's initial position
    var lightBulbRight = new THREE.PointLight(0x404040, 0.2);
    lightBulbRight.position.set(280, 175, 280);
    lightBulbRight.castShadow = true;
    lightBulbRight.shadow.mapSize.set(512, 512);
    this.scene.add(lightBulbRight);

/*     var lightBulbRightHelper = new THREE.PointLightHelper(lightBulbRight, 10);
    lightBulbRightHelper.visible = true;
    this.scene.add(lightBulbRightHelper); */

    //light bulb positioned on the left of the camera's initial position
    var lightBulbLeft = new THREE.PointLight(0x404040, 0.2);
    lightBulbLeft.position.set(-280, 175, 280);
    lightBulbLeft.castShadow = true;
    lightBulbLeft.shadow.mapSize.set(512, 512);
    this.scene.add(lightBulbLeft);

/*     var lightBulbLeftHelper = new THREE.PointLightHelper(lightBulbLeft, 10);
    lightBulbLeftHelper.visible = true;
    this.scene.add(lightBulbLeftHelper); */

    //sunlight coming out of the window in the middle pointing at the closet
    var sunLightCenter = new THREE.DirectionalLight(0xffffff, 1);
    sunLightCenter.position.set(-450, 100, -600);
    sunLightCenter.target = this.group;
    sunLightCenter.castShadow = true;
    sunLightCenter.shadow.mapSize.set(512, 512);
    sunLightCenter.shadow.camera.near = 0.5;
    sunLightCenter.shadow.camera.far = 500;
    this.scene.add(sunLightCenter);

/*     var sunLightCenterHelper = new THREE.DirectionalLightHelper(sunLightCenter, 5);
    this.scene.add(sunLightCenterHelper); */
  }

  /**
   * Updates current closet graphical view
   */
  updateClosetGV() {
    for (var i = 0; i < this.closet_faces_ids.length; i++) {
      let closet_face = this.group.getObjectById(this.closet_faces_ids[i]);
      closet_face.scale.x = this.getNewScaleValue(this.closet.getInitialClosetFaces()[i][0], this.closet.getClosetFaces()[i][0], closet_face.scale.x);
      closet_face.scale.y = this.getNewScaleValue(this.closet.getInitialClosetFaces()[i][1], this.closet.getClosetFaces()[i][1], closet_face.scale.y);
      closet_face.scale.z = this.getNewScaleValue(this.closet.getInitialClosetFaces()[i][2], this.closet.getClosetFaces()[i][2], closet_face.scale.z);
      closet_face.position.x = this.closet.getClosetFaces()[i][3];
      closet_face.position.y = this.closet.getClosetFaces()[i][4];
      closet_face.position.z = this.closet.getClosetFaces()[i][5];
    }

    for (let i = 0; i < this.closet_slots_faces_ids.length; i++) {
      let closet_face = this.group.getObjectById(this.closet_slots_faces_ids[i]);
      closet_face.scale.x = this.getNewScaleValue(this.closet.getInitialClosetSlotFaces()[i][0], this.closet.getClosetSlotFaces()[i][0], closet_face.scale.x);
      closet_face.scale.y = this.getNewScaleValue(this.closet.getInitialClosetSlotFaces()[i][1], this.closet.getClosetSlotFaces()[i][1], closet_face.scale.y);
      closet_face.scale.z = this.getNewScaleValue(this.closet.getInitialClosetSlotFaces()[i][2], this.closet.getClosetSlotFaces()[i][2], closet_face.scale.z);
      closet_face.position.x = this.closet.getClosetSlotFaces()[i][3];
      closet_face.position.y = this.closet.getClosetSlotFaces()[i][4];
      closet_face.position.z = this.closet.getClosetSlotFaces()[i][5];
    }
  }

  /**
   * Adds components to the current closet
   * @param {*} component Component to add
   */
  addComponent(component) {
    if (!component) return;
    var designation = component.model.split(".")[0];
    if (designation == "shelf") this.generateShelf(component);
    if (designation == "pole") this.generatePole(component);
    if (designation == "drawer") this.checkAddDrawerTriggers(component);
    if (designation == "hinged-door") this.checkAddHingedDoorTriggers(component);
    if (designation == "sliding-door") this.checkAddSlidingDoorTriggers(component);
    store.dispatch(SET_CUSTOMIZED_PRODUCT_COMPONENTS, {
      model: component.model,
      slot: component.slot
    });
  }

  /**
  * Removes a components from the current closet
  * @param {*} component Component to remove
  */
  removeComponent(component) {
    if (!component) return;
    var designation = component.model.split(".")[0];
    if (designation == "shelf") this.removeShelf(component.slot);
    if (designation == "pole") this.removePole(component.slot);
    if (designation == "drawer") this.removeDrawer(component.slot);
    if (designation == "hinged-door") this.removeHingedDoor(component.slot);
    if (designation == "sliding-door") this.removeSlidingDoor();
    store.dispatch(REMOVE_CUSTOMIZED_PRODUCT_COMPONENT, {
      model: component.model,
      slot: component.slot
    });
    this.selected_component = null;
    this.controls.enabled = true;
  }

  /**
   * Removes a shelf in the given slot from the current closet
   * @param {*} slot 
   */
  removeShelf(slot) {
    for (let i = 0; i < this.closet.shelves.length; i++) {
      if (this.closet.shelves[i].slotId == slot) {
        this.closet.shelves.splice(i, 1);
        var closet_shelf_face_id = this.closet_shelves_ids.splice(i, 1);
        this.group.remove(this.group.getObjectById(closet_shelf_face_id[0]));
        this.updateClosetGV();
        return;
      }
    }
  }

  /**
 * Removes a pole in the given slot from the current closet
 * @param {*} slot 
 */
  removePole(slot) {
    for (let i = 0; i < this.closet.poles.length; i++) {
      if (this.closet.poles[i].slotId == slot) {
        this.closet.poles.splice(i, 1);
        var closet_pole_id = this.closet_poles_ids.splice(i, 1);
        this.group.remove(this.group.getObjectById(closet_pole_id[0]));
        this.updateClosetGV();
        return;
      }
    }
  }

  /**
   * Removes a drawer in the given slot from the current closet
   * @param {*} slot 
   */
  removeDrawer(slot) {
    for (let i = 0; i < this.closet.drawers.length; i++) {
      if (this.closet.drawers[i].slotId == slot) {
        var closet_drawer_face_id = this.closet_drawers_ids.splice(i * 5, i * 5 + 5);
        var closet_module_face_id = this.closet_modules_ids.splice(i * 4, i * 4 + 4);

        for (let j = 0; j < closet_drawer_face_id.length; j++) {
          this.group.remove(this.group.getObjectById(closet_drawer_face_id[j]));
          this.group.remove(this.group.getObjectById(closet_module_face_id[j]));
        }
        this.updateClosetGV();
        return;
      }
    }
  }

  /**
* Removes a hinged door in the given slot from the current closet
* @param {*} slot 
*/
  removeHingedDoor(slot) {
    for (let i = 0; i < this.closet.hingedDoors.length; i++) {
      if (this.closet.hingedDoors[i].slotId == slot) {
        this.closet.hingedDoors.splice(i, 1);
        var closet_door_face_id = this.closet_hinged_doors_ids.splice(i, 1);
        this.group.remove(this.group.getObjectById(closet_door_face_id[0]));
        this.updateClosetGV();
        return;
      }
    }
  }

  /**
   * Removes the sliding door from the current closet
   */
  removeSlidingDoor() {
    this.closet.removeSlidingDoor();
    var closet_sliding_door_face_id = this.closet_sliding_doors_ids.pop();
    this.group.remove(this.group.getObjectById(closet_sliding_door_face_id));
  }

  /**
   * Generates a cylinder with given properties on a certain position relative to axis x,y and z
   */
  generatePole(component) {
    var slot = component.slot;
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

    var pole = new Pole(radiusTop, radiusBottom, height, radialSegments, heightSegments, openEnded, thetaStart, thetaLength, slot);

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
    poleMesh.userData = { model: component.model, slot: component.slot }

    //Enable shadows for pole's mesh
    poleMesh.castShadow = true;
    poleMesh.receiveShadow = true;

    this.closet.addPole(pole);
    this.group.add(poleMesh);
    this.closet_poles_ids.push(poleMesh.id);
  }


  renderDroppedComponent(event, canvas) {
    var splitted = event.dataTransfer.getData("text").split("/");
    var componentImageFileName = splitted[splitted.length - 1]

    var x = event.clientX;
    var y = event.clientY;
    var rect = canvas.getBoundingClientRect();

    this.mouse.x = (x - rect.left) / (canvas.clientWidth / 2.0) - 1.0;
    this.mouse.y = -((y - rect.bottom) / (canvas.clientHeight / 2.0) + 1.0);

    this.raycaster.setFromCamera(this.mouse, this.camera);
    var intersects = this.raycaster.intersectObjects(this.scene.children[0].children);

    if (intersects.length > 0) {
      //Snapping
      if (this.closet_slots_faces_ids.length == 0) {
        this.addComponent({ model: componentImageFileName, slot: 0 });
      } else {
        var facesXPositionIntervals = [];
        var raycasterPointX = intersects[0].point.x;

        facesXPositionIntervals.push(this.group.getObjectById(this.closet_faces_ids[2]).position.x);
        for (let i = 0; i < this.closet_slots_faces_ids.length; i++) {
          facesXPositionIntervals.push(this.group.getObjectById(this.closet_slots_faces_ids[i]).position.x);
        }
        facesXPositionIntervals.push(this.group.getObjectById(this.closet_faces_ids[3]).position.x);

        for (let i = 1; i < facesXPositionIntervals.length; i++) {
          if (raycasterPointX >= facesXPositionIntervals[i - 1] && raycasterPointX < facesXPositionIntervals[i]) {
            this.addComponent({ model: componentImageFileName, slot: i });
          }
        }
      }
    }
  }

  generateShelf(component) {
    var slot = component.slot;
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
    this.closet.addShelf(new Shelf([width, 3, this.closet.getClosetDepth(), x, y, z], slot));
    this.closet_shelves_ids.push(this.generateParellepiped(width, 3, this.closet.getClosetDepth(), x, y, z, this.material, this.group, component));
  }


  generateDrawer(component) {
    var slot = component.slot;
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

    var module = new Module([width, depthDrawer, depthCloset, x, y - (spaceDrawerModule / 4), z], //Base
      [width, depthDrawer, depthCloset, x, y + heightDrawer + (spaceDrawerModule / 4), z], //Top
      [depthDrawer, heightDrawer + (spaceDrawerModule / 4), depthCloset, x - (width / 2), y + (heightDrawer / 2), z], //Left
      [depthDrawer, heightDrawer + (spaceDrawerModule / 4), depthCloset, x + (width / 2), y + (heightDrawer / 2), z], slot); //Right

    var borders_module = module.module_faces;
    for (let i = 0; i < borders_module.length; i++) {
      this.closet_modules_ids.push(this.generateParellepiped(borders_module[i][0],
        borders_module[i][1], borders_module[i][2], borders_module[i][3],
        borders_module[i][4], borders_module[i][5], this.material, this.group, component));
    }

    var drawer = new Drawer([width - spaceDrawerModule, depthDrawer, depthCloset, x, y + (depthDrawer / 2), z], //Base
      [width - spaceDrawerModule, heightDrawer, depthDrawer, x, y + (heightDrawer / 2), z + (depthCloset / 2) - (depthDrawer / 2)], //Front
      [depthDrawer, heightDrawer, depthCloset - (depthDrawer / 2), x - (width / 2) + (spaceDrawerModule / 2), y + (heightDrawer / 2), z], //Left
      [depthDrawer, heightDrawer, depthCloset - (depthDrawer / 2), x + (width / 2) - (spaceDrawerModule / 2), y + (heightDrawer / 2), z], //Right
      [width - spaceDrawerModule, heightDrawer, depthDrawer, x, y + (heightDrawer / 2), z - (depthCloset / 2) + (depthDrawer / 2)], slot); //Back

    var borders_drawer = drawer.drawer_faces;
    for (let i = 0; i < borders_drawer.length; i++) {
      this.closet_drawers_ids.push(this.generateParellepiped(borders_drawer[i][0],
        borders_drawer[i][1], borders_drawer[i][2], borders_drawer[i][3],
        borders_drawer[i][4], borders_drawer[i][5], this.material, this.group, component));
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
      var slotFace = this.closet.addSlot(slotsToAdd[i]);
      this.closet_slots_faces_ids.push(this.generateParellepiped(slotFace[0], slotFace[1], slotFace[2], slotFace[3], slotFace[4], slotFace[5], this.material, this.group, ""));
    }
    this.updateClosetGV();
  }

  /*New methods*/
  removeAllSlots() {
    var size = this.closet_slots_faces_ids.length;
    for (let i = 0; i < size; i++) {
      this.removeSlot();
    }
  }

  removeAllComponents() {
    var size = this.closet_drawers_ids.length;
    for (let i = 0; i < size; i++) {
      this.closet.removeDrawer();
      var closet_drawer_face_id = this.closet_drawers_ids.pop();
      this.group.remove(this.group.getObjectById(closet_drawer_face_id));
    }

    size = this.closet_modules_ids.length;
    for (let i = 0; i < size; i++) {
      this.closet.removeModule();
      var closet_module_face_id = this.closet_modules_ids.pop();
      this.group.remove(this.group.getObjectById(closet_module_face_id));
    }

    size = this.closet_hinged_doors_ids.length;
    for (let i = 0; i < size; i++) {
      this.closet.removeHingedDoor();
      var closet_hinged_door_face_id = this.closet_hinged_doors_ids.pop();
      this.group.remove(this.group.getObjectById(closet_hinged_door_face_id));
    }

    size = this.closet_sliding_doors_ids.length;
    for (let i = 0; i < size; i++) {
      this.removeSlidingDoor();
    }

    size = this.closet_shelves_ids.length;
    for (let i = 0; i < size; i++) {
      this.closet.removeShelf();
      var closet_shelf_face_id = this.closet_shelves_ids.pop();
      this.group.remove(this.group.getObjectById(closet_shelf_face_id));
    }

    size = this.closet_poles_ids.length;
    for (let i = 0; i < size; i++) {
      this.closet.removePole();
      var closet_poles_id = this.closet_poles_ids.pop();
      this.group.remove(this.group.getObjectById(closet_poles_id));
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
  /*End new methods*/



  /* TODO: Transfer this methods to new Product Renderer */
  /** 
   * Populate vector website dimensions
  */
  populateWebsiteDimensions(websiteDimensions) {
    /* alert(websiteDimensions.width);
    alert(websiteDimensions.height);
    alert(websiteDimensions.depth); */

    if (websiteDimensions.width != undefined || websiteDimensions.height != undefined || websiteDimensions.depth != undefined) {

      this.websiteDimensions = [websiteDimensions.width, websiteDimensions.height, websiteDimensions.depth];
      this.resizeFactor();

    }
  }
  /**  END   */

  /**
   * Changes the dimensions of the closet
   * @param {number} width Number with the closet width
   * @param {number} height Number with the closet height
   * @param {number} depth Number with the closet depth
   */
  changeClosetDimensions(width, height, depth) {
    this.closet.changeClosetWidth(this.resizeVec[this.WIDTH] * width);
    this.closet.changeClosetHeight(this.resizeVec[this.HEIGHT] * height);
    this.closet.changeClosetDepth((this.resizeVec[this.DEPTH] * depth) - 250.8);

    this.updateClosetGV();
  }

  /**
   * Method that populates the vector responsible to resize
   */
  resizeFactor() {
    var i;
    for (i = 0; i < this.NUMBER_DIMENSIONS; i++) {
      this.resizeVec[i] = this.initialDimensions[i] / this.websiteDimensions[i];

    }/* 
    alert(this.resizeVec[this.WIDTH]);
    alert(this.resizeVec[this.HEIGHT]);
    alert(this.resizeVec[this.DEPTH]); */

    store.dispatch(SET_RESIZE_VECTOR_GLOBAL, {
      width: this.resizeVec[this.WIDTH],
      height: this.resizeVec[this.HEIGHT],
      depth: this.resizeVec[this.DEPTH],
    });
  }
  /**
   * Applies the texture to the closet.
   */
  applyTexture(texture) {
    this.material.map = THREE.ImageUtils.loadTexture(texture);
  }

  /**
   * Changes the closet's material's finish.
   * @param {*} shininess The new shininess value
   */
  applyFinish(shininess) {
    this.material.shininess = shininess;
  }

  /**
   * Changes the closet's material's color.
   * @param {*} color The new color in RGB format
   */
  applyColor(color) {
    var values = color.split("-");
    var red = values[0];
    var green = values[1];
    var blue = values[2];
    var alpha = values[3];
    if (alpha == 0) this.material.color.setHex(0xffffff);
    else this.material.color.setRGB(red, green, blue);
  }

  /**
   * Changes the current closet slots
   * @param {number} slots Number with the new closet slots
   */
  changeClosetSlots(slots, slotWidths) {
    var newSlots = this.closet.computeNewClosetSlots(slots);
    if (newSlots > 0) {
      for (let i = 0; i < newSlots; i++) {
        this.addSlot();
      }
    } else {
      newSlots = -newSlots;
      if (newSlots > 0) {
        for (let i = 0; i < newSlots; i++) {
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
   * @param {*} component Component info
   */
  generateParellepiped(width, height, depth, x, y, z, material, group, component) {
    var parellepipedGeometry = new THREE.CubeGeometry(width, height, depth);
    var parellepiped = new THREE.Mesh(parellepipedGeometry, material);
    parellepiped.position.x = x;
    parellepiped.position.y = y;
    parellepiped.position.z = z;

    //enable shadows for parallelepiped mesh
    parellepiped.castShadow = true;
    parellepiped.receiveShadow = true;

    if (component != "") parellepiped.userData = { model: component.model, slot: component.slot }
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
   * Computes the new scale value based on the initial scale value, new scale value and the current scale value
   * @param {number} initialScaleValue Number with the initial scale value
   * @param {number} newScaleValue Number with the new scale value
   * @param {number} currentScaleValue Number with the current scale value
   */
  getNewScaleValue(initialScaleValue, newScaleValue, currentScaleValue) {

    //*Scaling objects to 0 will result in 'Matrix3.getInverse(): can't invert matrix, determinant is 0' warnings
    //See: https://github.com/aframevr/aframe-inspector/issues/524
    //And: https://stackoverflow.com/questions/19150120/scaling-an-object-in-three-js

    if (initialScaleValue == 0) return 0.00001;
    return (newScaleValue * 1) / initialScaleValue;
  }

  /**
   * Represents the action that occurs when any keyboard key is pressed (key down),
   * which is blocking its action (disabling it).
   * @param {*} event 
   */
  onKeyDown(event) {
    event.preventDefault();
    //alert("entro no three");
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
      var intersected_object = intersects[0].object;

      //Checks if the selected closet face is a slot 
      for (var i = 0; i < this.closet_slots_faces_ids.length; i++) {
        var closet_face = this.group.getObjectById(this.closet_slots_faces_ids[i]);
        if (closet_face == intersected_object) {
          //Disables rotation while moving the slot
          this.controls.enabled = false;
          //Sets the selection to the current slot
          this.selected_slot = intersected_object;
          if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
            this.offset = this.intersection.x - this.selected_slot.position.x;
          }
        }
      }

      //Checks if the selected object is a shelf
      for (let j = 0; j < this.closet_shelves_ids.length; j++) {
        let shelf = this.group.getObjectById(this.closet_shelves_ids[j]);
        if (shelf == intersected_object) {
          this.controls.enabled = false;
          this.selected_component = intersected_object;
          if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
            this.offset = this.intersection.y - this.selected_component.position.y;
          }
        }
      }

      //Checks if the selected object is a pole
      for (let j = 0; j < this.closet_poles_ids.length; j++) {
        let pole = this.group.getObjectById(this.closet_poles_ids[j]);
        if (pole == intersected_object) {
          this.controls.enabled = false;
          this.selected_component = intersected_object;
          if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
            this.offset = this.intersection.y - this.selected_component.position.y;
          }
        }
      }

      //Checks if the selected closet face is a face
      if (this.group.getObjectById(this.closet_faces_ids[3]) == intersected_object ||
        this.group.getObjectById(this.closet_faces_ids[2]) == intersected_object) {
        //Disables rotation while moving the face
        this.controls.enabled = false;
        //Sets the selection to the current face
        this.selected_face = intersected_object;
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
          if (this.slidingDoor == intersected_object) {
            this.controls.enabled = false;
            if (this.slidingDoor.position.x < 0) {
              flagClose = true; //"Closing" ==> slide door to the right
            } else {
              flagOpen = true; //"Opening" ==> slide door to the left
            }
          }
          j++;
        }

        let aux = function (context, triggerDoorAnimationsFunction) { return function () { triggerDoorAnimationsFunction(context); } }

        if (flagOpen) {
          requestAnimationFrame(function () {
            context.slideDoorToLeft(aux);
          });
        } else if (flagClose) {
          requestAnimationFrame(function () {
            context.slideDoorToRight(aux);
          });
        }

        flagOpen = false;
        flagClose = false;
        j = 0;

        while (!flagOpen && !flagClose && j < this.closet_drawers_ids.length) {
          //Always get the front face of any drawer at index 5*j+1
          var drawer_front_face = this.group.getObjectById(this.closet_drawers_ids[5 * j + 1]);
          //Check if the selected object is a drawer's front face
          if (drawer_front_face == intersected_object) {
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
          if (this.hingedDoor == intersected_object) {
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
            context.openHingedDoor(context);
          });
        } else if (flagClose) {
          requestAnimationFrame(function () {
            context.closeHingedDoor();
          });
        }
      }
    }
  }

  slideDoorToLeft(aux) {
    if (this.doesClosetHaveOpenDrawers()) {
      this.waitingDoors.push(aux(this, this.slideDoorToLeftAnimation));
      this.closeAllOpenDrawers();
    } else {
      this.slideDoorToLeftAnimation(this);
    }
  }

  slideDoorToRight(aux) {
    if (this.doesClosetHaveOpenDrawers()) {
      this.waitingDoors.push(aux(this, this.slideDoorToRightAnimation));
      this.closeAllOpenDrawers();
    } else {
      this.slideDoorToRightAnimation(this);
    }
  }

  slideDoorToLeftAnimation(context) {
    let closet_left = context.group.getObjectById(context.closet_faces_ids[2]);
    let distanceFromDoorToLeftFace = Math.abs(context.slidingDoor.position.x - closet_left.position.x);
    let position = (Math.abs(closet_left.position.x - closet_left.geometry.parameters.width) / 2) - 2;
    if (position < distanceFromDoorToLeftFace) {
      context.slidingDoor.translateX(-1);
      requestAnimationFrame(function () {
        let aux = function (context, closeFunction) { return function () { closeFunction(context); } }
        context.slideDoorToLeft(aux);
      });
      context.render();
      context.controls.update();
    }
  }

  slideDoorToRightAnimation(context) {
    let closet_right = context.group.getObjectById(context.closet_faces_ids[3]);
    let distanceFromDoorToRightFace = Math.abs(context.slidingDoor.position.x - closet_right.position.x);
    let position = (Math.abs(closet_right.position.x + closet_right.geometry.parameters.width) / 2) - 2;
    if (position < distanceFromDoorToRightFace) {
      context.slidingDoor.translateX(1);
      requestAnimationFrame(function () {
        let aux = function (context, closeFunction) { return function () { closeFunction(context); } }
        context.slideDoorToRight(aux);
      });
      context.render();
      context.controls.update();
    }
  }

  openHingedDoor(context) {
    if (context.hingedDoor.rotation.y > (-Math.PI / 2)) {
      var rotationX = (context.hingedDoor.geometry.parameters.width / 2);
      context.hingedDoor.translateX(-rotationX);
      context.hingedDoor.rotation.y -= Math.PI / 100;
      context.hingedDoor.translateX(rotationX);
      requestAnimationFrame(function () {
        context.openHingedDoor(context);
      });
      context.render();
      context.controls.update();
    }
  }

  closeHingedDoor() {
    var hingedDoorSlot = this.getHingedDoorSlot(this.hingedDoor);
    if (this.doesSlotHaveOpenDrawers(hingedDoorSlot)) {
      let aux = function (context, closeFunction) { return function () { closeFunction(context); } }
      this.waitingDoors.push(aux(this, this.closeHingedDoorAnimation));
      this.closeSlotOpenDrawers(hingedDoorSlot);
    } else {
      this.closeHingedDoorAnimation(this);
    }
  }

  checkAddDrawerTriggers(component) {
    var slot = component.slot;
    this.generateDrawer(component);
    let aux = function (context, triggerDoorAnimationsFunction) { return function () { triggerDoorAnimationsFunction(context); } }
    if (this.doesSlotHaveHingedDoor(slot)) {
      if (!this.isHingedDoorClosed) {
        this.hingedDoor = this.group.getObjectById(this.closet_hinged_doors_ids[slot - 1]);
        requestAnimationFrame(aux(this, this.openHingedDoor));
      }
    }
    if (this.doesClosetHaveSlidingDoors()) {
      //Front face of the last added drawer is always at index length - 4
      var addedDrawer = this.group.getObjectById(this.closet_drawers_ids[this.closet_drawers_ids.length - 4]);
      if (addedDrawer.position.x < 0) {
        for(let i = 0; i < this.closet_sliding_doors_ids.length; i++){
          let door = this.group.getObjectById(this.closet_sliding_doors_ids[i]);
          if(door.position.x < 0){
            this.slidingDoor = door;
            this.slideDoorToRight(aux);
          }
        }
      } else {
        for(let i = 0; i < this.closet_sliding_doors_ids.length; i++){
          let door = this.group.getObjectById(this.closet_sliding_doors_ids[i]);
          if(door.position.x > 0){
            this.slidingDoor = door;
            this.slideDoorToLeft(aux);
          }
        }
      }
    }
  }

  checkAddSlidingDoorTriggers(component) {
    if (this.doesClosetHaveHingedDoors()) {
      store.dispatch(SET_DOORS_FLAG, { flag: "CLOSET_HAS_HINGED_DOORS" });
    } else if (this.doesClosetHaveSlidingDoors()) {
      store.dispatch(SET_DOORS_FLAG, { flag: "CLOSET_HAS_SLIDING_DOORS" });
    } else {
      if (this.doesClosetHaveOpenDrawers()) {
        if (this.openDrawers.length > 0) {
          var context = this;
          this.waitingDoors.push(function () {
            context.generateSlidingDoor(component);
          });
          this.closeAllOpenDrawers();
        } else {
          this.generateSlidingDoor(component);
        }
      } else {
        this.generateSlidingDoor(component);
      }
    }
  }

  checkAddHingedDoorTriggers(component) {
    var slot = component.slot;
    if (this.doesSlotHaveHingedDoor(slot)) {
      store.dispatch(SET_DOORS_FLAG, { flag: "SLOT_HAS_DOOR" });
    } else if (this.doesClosetHaveSlidingDoors()) {
      store.dispatch(SET_DOORS_FLAG, { flag: "CLOSET_HAS_SLIDING_DOORS" });
    } else {
      if (this.doesSlotHaveOpenDrawers(slot)) {
        if (this.openDrawers.length > 0) {
          var context = this;
          this.waitingDoors.push(function () {
            context.addHingedDoor(component);
          });
          this.closeSlotOpenDrawers(slot);
        } else {
          this.addHingedDoor(component);
        }
      } else {
        this.addHingedDoor(component);
      }
    }
  }

  addHingedDoor(component) {
    if (this.openDrawers.length == 0) {
      this.generateHingedDoor(component);
    } else {
      this.generateHingedDoor(component);
    }
  }

  generateHingedDoor(component) {
    var slot = component.slot;
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

    var meshID = this.generateParellepiped(width, height, depth, x, y, z + (depth_closet / 2), this.material, this.group, component);
    var hingedDoor = new HingedDoor([width, height, depth, x, y, z + (depth_closet / 2)], slot, meshID);
    this.closet.addHingedDoor(hingedDoor);
    this.closet_hinged_doors_ids.push(meshID);
  }

  generateSlidingDoor(component) {
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
      [thickness, height, 5, rightFace.position.x, rightFace.position.y, z + 7], 0);

    var back_door = new SlidingDoor([width / 2, (height - thickness), 5, rightFace.position.x / 2, rightFace.position.y, z + 2]);

    var back_frame = new Module([width, thickness, 5, bottomFace.position.x, bottomFace.position.y, z + 2],
      [width, thickness, 5, topFace.position.x, topFace.position.y, z + 2],
      [thickness, height, 5, leftFace.position.x, leftFace.position.y, z + 2],
      [thickness, height, 5, rightFace.position.x, rightFace.position.y, z + 2], 0);

    //Adds front frame
    var frontFrameBorders = front_frame.module_faces;
    for (var i = 0; i < frontFrameBorders.length; i++) {
      this.generateParellepiped(frontFrameBorders[i][0],
        frontFrameBorders[i][1], frontFrameBorders[i][2], frontFrameBorders[i][3],
        frontFrameBorders[i][4], frontFrameBorders[i][5], this.material, this.group, component);
    }

    //Adds front door
    var front_door_mesh_id = this.generateParellepiped(
      front_door.sliding_door_axes[0],
      front_door.sliding_door_axes[1],
      front_door.sliding_door_axes[2],
      front_door.sliding_door_axes[3],
      front_door.sliding_door_axes[4],
      front_door.sliding_door_axes[5],
      this.material, this.group, component);

    //Adds back door
    var back_door_mesh_id = this.generateParellepiped(
      back_door.sliding_door_axes[0],
      back_door.sliding_door_axes[1],
      back_door.sliding_door_axes[2],
      back_door.sliding_door_axes[3],
      back_door.sliding_door_axes[4],
      back_door.sliding_door_axes[5],
      this.material, this.group, component);

    this.closet.addSlidingDoor(front_door);
    this.closet.addSlidingDoor(back_door);

    this.closet_sliding_doors_ids.push(front_door_mesh_id);
    this.closet_sliding_doors_ids.push(back_door_mesh_id);

    //Adds back frame
    var backFrameBorders = back_frame.module_faces;
    for (let i = 0; i < backFrameBorders.length; i++) {
      this.generateParellepiped(backFrameBorders[i][0],
        backFrameBorders[i][1], backFrameBorders[i][2], backFrameBorders[i][3],
        backFrameBorders[i][4], backFrameBorders[i][5], this.material, this.group, component);
    }
  }

  closeSlotOpenDrawers(slot) {
    var i = 0;
    var index = 0;
    // var closet_front = Math.abs(this.group.getObjectById(this.closet_faces_ids[4]).position.z);
    while (i < this.closet_drawers_ids.length) {
      if (this.closet.drawers[index].slotId == slot) {
        var drawer_front_face = this.group.getObjectById(this.closet_drawers_ids[5 * index + 1]);
        if (drawer_front_face.position.z > -50) {
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
    while (i < this.closet_drawers_ids.length) {
      var drawer_front_face = this.group.getObjectById(this.closet_drawers_ids[5 * index + 1]);
      if (drawer_front_face.position.z > -50) {
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
    var numberOfDrawers = this.closet_drawers_ids.length / 5;
    for (let i = 0; i < numberOfDrawers; i++) {
      if (this.closet.drawers[i].slotId == slot) {
        return this.group.getObjectById(this.closet_drawers_ids[5 * i + 1]).position.z >= -50;
      }
    }
    return false;
  }

  doesClosetHaveOpenDrawers() {
    var flag = false;
    var numberOfDrawers = this.closet_drawers_ids.length / 5;
    for (let i = 0; i < numberOfDrawers; i++) {
      if (this.group.getObjectById(this.closet_drawers_ids[5 * i + 1]).position.z >= -50) flag = true;
    }
    return flag;
  }

  doesClosetHaveHingedDoors() {
    return this.closet.hingedDoors.length != 0;
  }

  doesClosetHaveSlidingDoors() {
    return this.closet.slidingDoors.length != 0;
  }

  closeHingedDoorAnimation(context) {
    if (context.hingedDoor.rotation.y <= 0) {
      var rotationX = context.hingedDoor.geometry.parameters.width / 2;
      context.hingedDoor.translateX(-rotationX);
      context.hingedDoor.rotation.y += Math.PI / 100;
      context.hingedDoor.translateX(rotationX);
      requestAnimationFrame(function () {
        context.closeHingedDoor();
      });
      context.render();
      context.controls.update();
    } else {
      context.isHingedDoorClosed = true;
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
      let oldLength = this.openDrawers.length;
      this.openDrawers.pop();
      if (this.openDrawers.length < oldLength) {
        while (this.waitingDoors.length != 0) {
          this.waitingDoors.pop()();
        }
      }
    }
  }

  /**
   * Resizes the renderer to the provided canvas dimensions.
   * @param {number} canvasWidth - Canvas's width.
   * @param {number} canvasHeight - Canvas's height.
   */
  resizeRenderer(canvasWidth, canvasHeight) {

    //*Please note that while the renderer instance has access to the canvas, 
    //*the dimensions don't seem to be updated correctly when accessing the canvas's properties, hence the parameters

    this.camera.aspect = canvasWidth / canvasHeight;
    this.camera.updateProjectionMatrix();

    this.renderer.setSize(canvasWidth, canvasHeight);
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
   * Moves the slot across the defined plan that intersects the closet, without overlapping the closet's faces
   */
  moveSlot() {

    
  /* moveSlot() {
    if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
      var newPosition = this.intersection.x - this.offset; //Subtracts the offset to the x coordinate of the intersection point
      var valueCloset = this.group.getObjectById(this.closet_faces_ids[2]).position.x;
      if (Math.abs(newPosition) < Math.abs(valueCloset)) { //Doesn't allow the slot to overlap the faces of the closet
        this.selected_slot.position.x = newPosition;
      }
    }
  } */
    /*  var information = { idSlot : 0, 
     newValue: 0}; */
     if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
       var newPosition = this.intersection.x - this.offset; //Subtracts the offset to the x coordinate of the intersection point
       var valueCloset = this.group.getObjectById(this.closet_faces_ids[2]).position.x;
       if (Math.abs(newPosition) < Math.abs(valueCloset)) { //Doesn't allow the slot to overlap the faces of the closet
         this.selected_slot.position.x = newPosition;
         for (let i = 0; i < this.closet_slots_faces_ids.length; i++) {
           if (this.group.getObjectById(this.closet_slots_faces_ids[i]) == this.selected_slot) {
             this.group.getObjectById(this.closet_slots_faces_ids[i]).position.x = newPosition;
            /*  var v1 = this.group.getObjectById(this.closet_faces_ids[3]).position.x;
             var v2 = this.closet.getClosetWidth() * 2;
             var v3 = Math.abs(this.group.getObjectById(this.closet_faces_ids[3]).position.x);
             var v4 = Math.abs(this.group.getObjectById(this.closet_faces_ids[2]).position.x);
             var conversion = ((newPosition + v1) * v2) / (v3 + v4); */
            /*  information.idSlot = i + 1;
             information.newValue = conversion; */
           }
         }
       }
     }
     /* return information; */
   }
       
   /**
    * Move slot with slider
    */
   moveSlotSlider(index, newWidth) {
     /* alert("antes" + this.group.getObjectById(this.closet_slots_faces_ids[index]).position.x); */
     var left_closet_face_x_value = this.group.getObjectById(this.closet_faces_ids[2]).position.x;
     this.selected_slot = this.group.getObjectById(this.closet_slots_faces_ids[index]);
     if (index == 0) {
       let newPosition = left_closet_face_x_value + newWidth;
       this.group.getObjectById(this.closet_slots_faces_ids[index]).position.x = newPosition;
     } else {
       this.group.getObjectById(this.closet_slots_faces_ids[index]).position.x = this.group.getObjectById(this.closet_slots_faces_ids[index - 1]).position.x + (newWidth);
     }
     this.updateClosetGV()
     /* alert("depois" + this.group.getObjectById(this.closet_slots_faces_ids[index]).position.x); */
   }


  /**
   * Moves a component across the yy axis without overlapping the slots planes or the closets planes
   */
  moveComponent() {
    if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
      var computedYPosition = this.intersection.y - this.offset; //The component's new computed position on the yy axis
      var computedXPosition = this.intersection.x - this.offset; //The component's new computed position on the xx axis

      if (computedYPosition < this.group.getObjectById(this.closet_faces_ids[1]).position.y &&
        computedYPosition >= this.group.getObjectById(this.closet_faces_ids[0]).position.y &&
        computedXPosition < this.group.getObjectById(this.closet_faces_ids[3]).position.x &&
        computedXPosition >= this.group.getObjectById(this.closet_faces_ids[2]).position.x) {
        this.selected_component.position.y = computedYPosition; //Sets the new position as long as the component stays within the closet boundaries
      } else {
        store.dispatch(SET_COMPONENT_TO_REMOVE, {
          model: this.selected_component.userData.model,
          slot: this.selected_component.userData.slot
        });
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