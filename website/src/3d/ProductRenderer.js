//@ts-check
import * as THREE from 'three'
import 'three/examples/js/controls/OrbitControls'
import Closet from './Closet'
import Pole from './Pole'

export default class ProductRenderer {
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
     * @type{Closet}
     */
    closet;

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

    /**
     * 
     * @param {HTMLCanvasElement} htmlCanvasElement 
     */
    constructor(htmlCanvasElement) {

        this.closet_faces_ids = [];
        this.closet_slots_faces_ids = [];
        this.closet_poles_ids = [];
        this.closet_shelves_ids = [];
        this.selected_slot = null;
        this.selected_face = null;
        this.selected_component = null;
        this.hovered_object = null;
        this.plane = null;
        this.mouse = new THREE.Vector2();
        this.intersection = new THREE.Vector3(0, 0, 0);
        this.raycaster = new THREE.Raycaster();
        this.canvasWebGL = htmlCanvasElement;
        this.renderer = new THREE.WebGLRenderer({ canvas: this.canvasWebGL, antialias: true });
        this.initCamera();
        this.initControls();
        this.scene = new THREE.Scene();
        this.group = new THREE.Group();
        this.initCloset();
        this.initLighting();

        var geometry = new THREE.SphereBufferGeometry(500, 60, 40);
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

        //this.registerEvents();
        this.animate();
    }

    /**
     * Initiates the closet
     */
    initCloset() {
        var thickness = 4.20;

        this.closet = new Closet([404.5, thickness, 100, 0, -210, -295], //Bottom
            [404.5, thickness, 100, 0, 90, -295], //Top
            [thickness, 300, 100, -200, -60, -295], //Left
            [thickness, 300, 100, 200, -60, -295], //Right
            [400, 300, 0, 0, -60, -345.8]); //Back

        var faces = this.closet.closet_faces;

        this.textureLoader = new THREE.TextureLoader();
        //A MeshPhongMaterial allows for shiny surfaces
        //A soft white light is being as specular light
        //The shininess value is the same as the matte finishing's value
        this.material = new THREE.MeshPhongMaterial({ specular: 0x404040, shininess: 20 });

        for (var i = 0; i < faces.length; i++) {
            this.closet_faces_ids.push(this.generateParellepiped(faces[i][0], faces[i][1], faces[i][2]
                , faces[i][3], faces[i][4], faces[i][5], this.material, this.group));
        }
        this.scene.add(this.group);
        this.group.visible = false;
        this.renderer.setClearColor(0xFFFFFF, 1);
    }

    /**
     * Shows the closet
     */
    showCloset() { this.group.visible = true; }

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
        for (var i = 0; i < this.closet_faces_ids.length; i++) {
            var closet_face = this.group.getObjectById(this.closet_faces_ids[i]);
            closet_face.scale.x = this.getNewScaleValue(this.closet.getInitialClosetFaces()[i][0], this.closet.getClosetFaces()[i][0], closet_face.scale.x);
            closet_face.scale.y = this.getNewScaleValue(this.closet.getInitialClosetFaces()[i][1], this.closet.getClosetFaces()[i][1], closet_face.scale.y);
            closet_face.scale.z = this.getNewScaleValue(this.closet.getInitialClosetFaces()[i][2], this.closet.getClosetFaces()[i][2], closet_face.scale.z);
            closet_face.position.x = this.closet.getClosetFaces()[i][3];
            closet_face.position.y = this.closet.getClosetFaces()[i][4];
            closet_face.position.z = this.closet.getClosetFaces()[i][5];
        }

        for (var i = 0; i < this.closet_slots_faces_ids.length; i++) {
            var closet_face = this.group.getObjectById(this.closet_slots_faces_ids[i]);
            closet_face.scale.x = this.getNewScaleValue(this.closet.getInitialClosetSlotFaces()[i][0], this.closet.getClosetSlotFaces()[i][0], closet_face.scale.x);
            closet_face.scale.y = this.getNewScaleValue(this.closet.getInitialClosetSlotFaces()[i][1], this.closet.getClosetSlotFaces()[i][1], closet_face.scale.y);
            closet_face.scale.z = this.getNewScaleValue(this.closet.getInitialClosetSlotFaces()[i][2], this.closet.getClosetSlotFaces()[i][2], closet_face.scale.z);
            closet_face.position.x = this.closet.getClosetSlotFaces()[i][3];
            closet_face.position.y = this.closet.getClosetSlotFaces()[i][4];
            closet_face.position.z = this.closet.getClosetSlotFaces()[i][5];
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
            for (let j = 0; components[i].length; j++) {
                alert(components[i][j].designation);

                if (components[i][j].slot == 0) { //add to closet structure
                } else if (components[i][j].slot > 0) { //add to closet slot
                    //if (components[i][j].designation == "Shelf") this.generateShelf(components[i][j].slot);
                    //if (components[i][j].designation == "Pole") this.generatePole(components[i][j].slot);
                    if (components[i][j].designation == "Produto Componente") this.generatePole(components[i][j].slot);
                }
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

        if (this.closet_slots_faces_ids.length == 0) { //If there are no slots, the width of the shelf is the same as the closet's structure
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
             alert(slotsToAdd.length);
             alert(slotsToAdd[i].width);
            var slotFace = this.closet.addSlot(slotsToAdd[i]);
            this.closet_slots_faces_ids.push(this.generateParellepiped(slotFace[0], slotFace[1], slotFace[2]
                , slotFace[3], slotFace[4], slotFace[5], this.material, this.group));
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
        this.closet.changeClosetWidth(width);
        this.closet.changeClosetHeight(height);
        this.closet.changeClosetDepth(depth);
        this.updateClosetGV();
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
        this.mouse.y = - ((y - rect.bottom) / (this.canvasWebGL.clientHeight / 2.0) + 1.0); //Get mouse y position
        this.raycaster.setFromCamera(this.mouse, this.camera); //Set raycast position

        //If the selected object is a slot
        if (this.selected_slot) {
            this.moveSlot();
            return;
        }

        //If the selected object is a closet face
        if (this.selected_face) {
            this.moveFace();
            return;
        }

        //If the selected object is a closet pole or shelf
        if (this.selected_component) {
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

                var leftFacePosition = - this.intersection.x - this.offset - this.selected_face.position.x; //Position of the left closet face

                if (this.closet_slots_faces_ids.length == 0) { //If there are no slots
                    this.selected_face.position.x = leftFacePosition;
                    this.closet.changeClosetWidth(leftFacePosition);
                    this.updateClosetGV();
                } else {
                    var leftSlotPosition = - this.group.getObjectById(this.closet_slots_faces_ids[0]).position.x; //Position of the first (more to the left) slot

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

            if (computedPosition < this.group.getObjectById(this.closet_faces_ids[1]).position.y
                && computedPosition >= this.group.getObjectById(this.closet_faces_ids[0]).position.y) {
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