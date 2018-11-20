//@ts-check
import * as THREE from 'three'
import 'three/examples/js/controls/OrbitControls'
import Closet from './Closet'

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
        this.selected_slot = null;
        this.selected_face = null;
        this.hovered_object = null;
        this.plane = null;
        this.mouse = new THREE.Vector2();
        this.intersection = new THREE.Vector3(0, 0, 0);
        this.raycaster = new THREE.Raycaster();
        this.canvasWebGL = htmlCanvasElement;
        this.renderer = new THREE.WebGLRenderer({ canvas: this.canvasWebGL, antialias: true });
        this.initCamera();
        this.initControls();
        this.initCloset();
        this.initLighting();

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

        //this.registerEvents();
        this.animate();
    }
    /**
     * Initiates the closet
     */
    initCloset() {
        this.scene = new THREE.Scene();
        this.group = new THREE.Group();
        this.closet = new Closet([204.5, 4.20, 100, 0, 0, 0]
            , [204.5, 4.20, 100, 0, 100, 0]
            , [4.20, 100, 100, -100, 50, 0]
            , [4.20, 100, 100, 100, 50, 0]
            , [200, 100, 0, 0, 50, -50]);
        var faces = this.closet.closet_faces;

        this.textureLoader = new THREE.TextureLoader();
        //A MeshPhongMaterial allows for shiny surfaces
        //A soft white light is being as specular light
        //The shininess value is the same as the matte finishing's value
        this.material = new THREE.MeshPhongMaterial({ specular: 0x404040, shininess: 20 });

        for (var i = 0; i < faces.length; i++) {
            this.closet_faces_ids.push(this.generateParellepiped(faces[i][0], faces[i][1], faces[i][2]
                , faces[i][3], faces[i][4], faces[i][5]));
        }
        this.scene.add(this.group);
        this.renderer.setClearColor(0xFFFFFF, 1);
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
        this.addSlotNumbered(1);
    }

    /**
     * Adds a specified number of slots to the current closet
     * @param{number} slotsToAdd - number of slots being added
     */
    addSlotNumbered(slotsToAdd) {
        for (var i = 0; i < slotsToAdd; i++) {
            var slotFace = this.closet.addSlot();
            this.closet_slots_faces_ids.push(this.generateParellepiped(slotFace[0], slotFace[1], slotFace[2]
                , slotFace[3], slotFace[4], slotFace[5]));
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
     */
    generateParellepiped(width, height, depth, x, y, z) {
        var parellepipedGeometry = new THREE.CubeGeometry(width, height, depth);
        var parellepiped = new THREE.Mesh(parellepipedGeometry, this.material);
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
     * Register the events that can be communicated through the document
     */
    /*     registerEvents() {
    
            document.addEventListener("changeDimensions", (changeDimensionsEvent) => {
                this.changeClosetDimensions(changeDimensionsEvent.detail.width, changeDimensionsEvent.detail.height, changeDimensionsEvent.detail.depth);
            });
    
            document.addEventListener("changeSlots", (changeSlotsEvent) => {
                this.changeClosetSlots(changeSlotsEvent.detail.slots, changeSlotsEvent.detail.slotWidths);
            });
            document.addEventListener("changeMaterial", (changeMaterialEvent) => {
                this.applyTexture(changeMaterialEvent.detail.material);
            });
            document.addEventListener("changeShininess", (changeShininessEvent) => {
                this.changeShininess(changeShininessEvent.detail.shininess);
            });
            document.addEventListener("changeColor", (changeColorEvent) => {
                this.changeColor(changeColorEvent.detail.color);
            });
        } */

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

                if(closet_face == face) {
                    //Disables rotation while moving the slot
                    this.controls.enabled = false;
                    //Sets the selection to the current slot
                    this.selected_slot = face;
                    if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
                        this.offset = this.intersection.x - this.selected_slot.position.x;
                    }
                }
            }

            //Checks if the selected closet face isn't a slot
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
        this.mouse.x = (x - rect.left) / (this.canvasWebGL.clientWidth / 2.0) - 1.0; //Get mouse x position

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

            var rightFacePosition = this.intersection.x - this.offset + this.selected_face.position.x; //Position of the right closet face
            var leftFacePosition = - this.intersection.x - this.offset - this.selected_face.position.x; //Position of the left closet face

            if (this.selected_face == this.group.getObjectById(this.closet_faces_ids[3])) {
                if (this.closet_slots_faces_ids.length == 0) {
                    this.selected_face.position.x = rightFacePosition;
                    this.changeClosetDimensions(rightFacePosition, this.closet.getClosetHeight(), this.closet.getClosetDepth());
                } else {
                    var rightSlotPosition = this.group.getObjectById(this.closet_slots_faces_ids[this.closet_slots_faces_ids.length - 1]).position.x; //Position of the last (more to the right) slot 

                    if (rightFacePosition - rightSlotPosition > rightSlotPosition) {
                        this.selected_face.position.x = rightFacePosition;
                        this.changeClosetDimensions(rightFacePosition, this.closet.getClosetHeight(), this.closet.getClosetDepth());
                    }
                }
            } else if (this.selected_face == this.group.getObjectById(this.closet_faces_ids[2])) {
                if (this.closet_slots_faces_ids.length == 0) {
                    this.selected_face.position.x = leftFacePosition;
                    this.changeClosetDimensions(leftFacePosition, this.closet.getClosetHeight(), this.closet.getClosetDepth());
                } else {
                    var leftSlotPosition = - this.group.getObjectById(this.closet_slots_faces_ids[0]).position.x; //Position of the first (more to the left) slot

                    if (leftFacePosition - leftSlotPosition > leftSlotPosition) {
                        this.selected_face.position.x = leftFacePosition;
                        this.changeClosetDimensions(leftFacePosition, this.closet.getClosetHeight(), this.closet.getClosetDepth());
                    }
                }
            }
        }
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