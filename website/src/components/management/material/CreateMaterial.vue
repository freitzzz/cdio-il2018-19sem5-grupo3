<template>
    <b-modal :active.sync="active" as-modal-card>
            <div  class="modal-card" style="width: auto">
                <div v-if="panelCreateMaterial">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Create Material</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Reference">
                            <b-input
                                v-model="referenceMaterial"
                                type="String"
                                placeholder="Insert reference"
                                icon="pound"
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Designation">
                            <b-input
                                v-model="designation"
                                type="String"
                                placeholder="Insert designation"
                                icon="pound"
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Colors"> 
                            <b-select placeholder="Colors" icon="tag">
                                <option  v-for="color in availableColors" 
                                    :key="color" 
                                    :value="color">
                                    {{color}}</option>
                            </b-select>
                        </b-field>
                         <button class="button is-primary" @click="createColor()">+</button>
                        <b-field label="Finish">
                            <b-select placeholder="Finishes" icon="tag">
                                 <option v-for="reference in availableFinishes" 
                                    :key="reference" 
                                    :value="reference" @click="createEditFinish()">
                                    {{reference}} </option>
                            </b-select>
                        </b-field>
                        <button class="button is-primary" @click="createFinish()">+</button>
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-primary" @click="postMaterial()">Create</button>
                    </footer>
                </div>
                <div v-if="createFinishPanelEnabled">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Create Finish</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Designation: ">
                            <b-input
                                v-model="referenceFinish" 
                                type="String"
                                placeholder="Insert reference"
                                icon="pound"
                                required>
                            </b-input>
                        </b-field>
                    </section>
                    <footer class="modal-card-foot">
                      <button class="button is-primary" @click="newFinish()">+</button>
                      <button class="button is-primary" @click="desabelFinish()">Back</button>
                    </footer>
                </div>
                <div v-if="createColorPanelEnabled" >
                    <header class="modal-card-head">
                        <p class="modal-card-title">Create Color</p>
                    </header>
                    <section class="modal-card-body">
                    <b-field label="Designation: ">
                            <swatches v-model="color" colors="text-advanced"></swatches>
                    </b-field>
                    <br> <br> <br> <br> <br><br> <br> <br>
                    </section>
                    <footer class="modal-card-foot">
                      <button class="button is-primary" @click="newColor()">+</button>
                      <button class="button is-primary" @click="desabelColor()">Back</button>
                    </footer>
                </div> 
            </div>
    </b-modal>
</template>
<script>
import Swatches from "vue-swatches";

import "vue-swatches/dist/vue-swatches.min.css";

export default {
  name: "CreateMaterial",
  data() {
    return {
      referenceMaterial: "",
      referenceFinish: "",
      designation: "",
      panelCreateMaterial: true,
      createColorPanelEnabled: false,
      createFinishPanelEnabled: false,
      defineNewFinish: false,
      createNewFinish: false,
      availableFinishes: [],
      availableColors: [],
      color: "#000000"
    };
  },
  components: {
    Swatches
  }, // window.VueSwatches.default - from CDN
  props: {
    active: {
      type: Boolean,
      default: true
    }
  },

  /*  */
  methods: {
    postMaterial() {
     alert(this.referenceMaterial);
     alert(this.designation);

      Axios.post("http://localhost:5000/mycm/api/materials", {
        reference: this.referenceMaterial,
        designation: this.designation,
        colors: [
          this.availableColors.forEach(element => {
            element = element.replace("#", "");
            r = parseInt(hex.substring(0, 2), 16);
            g = parseInt(hex.substring(2, 4), 16);
            b = parseInt(hex.substring(4, 6), 16);

            name: element;
            red: r;
            green: g;
            blue: b;
            alpha: 0;
          })
        ],
        finishes: [
          this.availableFinishes.forEach(element => {
            description: element;
          })
        ]
      })
        .then(response => {})
        .catch(error => {});
    },
    createColor() {
      this.panelCreateMaterial = false;
      this.createColorPanelEnabled = true;
    },
    createFinish() {
      this.panelCreateMaterial = false;
      this.createFinishPanelEnabled = true;
    },
    desabelFinish() {
      this.panelCreateMaterial = true;
      this.createFinishPanelEnabled = false;
      this.referenceFinish = "";
    },
    desabelColor() {
      this.panelCreateMaterial = true;
      this.createColorPanelEnabled = false;
    },
    createEditFinish() {},
    newFinish() {
      if (
        this.referenceFinish != null &&
        this.referenceFinish.trim() != "" &&
        this.availableFinishes.indexOf(this.referenceFinish.trim()) < 0
      ) {
        this.availableFinishes.push(this.referenceFinish);
        alert("The reference was successfully inserted!");
      } else {
        alert("The inserted reference is invalid!");
      }
      this.referenceFinish = "";
    },
    newColor() {
      this.availableColors.push(this.color);
      alert("The color was successfully inserted!");
    }
  }
};
</script>