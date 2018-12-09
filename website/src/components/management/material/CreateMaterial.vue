<template>
    <b-modal :active.sync="active" as-modal-card>
                <div v-if="panelCreateMaterial" class="modal-card" style="width: auto">
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
                                v-model="designationMaterial"
                                type="String"
                                placeholder="Insert designation"
                                icon="pound"
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Colors"> 
                            <b-select placeholder="Colors" icon="tag" v-model="selectedColor">
                                <option  v-for="color in nameColors" 
                                    :key="color" 
                                    :value="color">
                                    {{color}}</option>
                            </b-select>
                        </b-field>
                        <button class="button is-primary" @click="createColor()">+</button>
                        <button class="button is-primary" @click="deleteColor()">-</button>
                        <b-field label="Finish">
                            <b-select placeholder="Finishes" icon="tag" v-model="selectedFinish">
                                 <option v-for="finish in availableFinishes" 
                                    :key="finish.id" :value="finish">{{finish.referenceFinish}} </option>
                            </b-select>
                        </b-field>
                        <button class="button is-primary" @click="createFinish()">+</button>
                        <button class="button is-primary" @click="deleteFinish()">-</button>
                         <b-field class="modal-card-body">
                        <button class="button is-primary" >Select Image</button>
                            <b-input
                                v-model="pathImage"
                                type="String"
                                disabled
                                required>
                            </b-input>
                        </b-field>
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-primary" @click="postMaterial()">Create</button>
                    </footer>
                </div>
                <div v-if="createFinishPanelEnabled" class="modal-card" style="width: auto">
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
                <div v-if="createColorPanelEnabled" class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Create Color</p>
                    </header>
                    <section class="modal-card-body">
                    <b-field label="Name">
                            <b-input
                                v-model="nameColor"
                                type="String"
                                placeholder="Insert name"
                                icon="pound"
                                required>
                            </b-input>
                        </b-field>
                    <swatches v-model="color" colors="text-advanced"></swatches>
                    <br> <br> <br> <br> <br><br> <br>
                    </section>
                    <footer class="modal-card-foot">
                      <button class="button is-primary" @click="newColor()">+</button>
                      <button class="button is-primary" @click="desabelColor()">Back</button>
                    </footer>
                </div> 
    </b-modal>
</template>
<script>
import Swatches from "vue-swatches";
import Axios from "axios";
import "vue-swatches/dist/vue-swatches.min.css";
export default {
  name: "CreateMaterial",
  data() {
    return {
      referenceMaterial: "",
      referenceFinish: "",
      designationMaterial: "",
      pathImage:"",
      panelCreateMaterial: true,
      createColorPanelEnabled: false,
      createFinishPanelEnabled: false,
      defineNewFinish: false,
      createNewFinish: false,
      availableFinishes: {},
      availableColors: {},
      nameColors: [],
      color: "#000000",
      nameColor: "",
      active: true
    };
  },
  components: {
    Swatches
  }, // window.VueSwatches.default - from CDN

  methods: {
    /* popup() {
      let route = this.$router.resolve({path: 'https://stackoverflow.com/questions/40015037/can-vue-router-open-a-link-in-a-new-tab'});
      // let route = this.$router.resolve('/link/to/page'); // This also works.
      window.open(route.href, '_blank');
    }, */
    postMaterial() {
      let finishesToAdd = [];
      this.availableFinishes.forEach(element => {
        finishesToAdd.push({
          description: element.description
        });
      });
      let colorsToAdd = [];
      this.availableColors.forEach(element => {
        colorsToAdd.push({
          name: element,
          red: parseInt(element.replace("#", "").substring(0, 2), 16),
          green: parseInt(element.replace("#", "").substring(2, 4), 16),
          blue: parseInt(element.replace("#", "").substring(4, 6), 16),
          alpha: "0"
        });
      });
      Axios.post("http://localhost:5000/mycm/api/materials", {
        reference: this.referenceMaterial,
        designation: this.designationMaterial,
        colors: colorsToAdd,
        finishes: finishesToAdd
      })
        .then(response => {
          this.$toast.open("Material Created");
        })
        .catch(error => {});
    },
    deleteFinish() {
      if (selectedFinish != null) {
        this.availableFinishes.splice(this.selectedFinish, 1),
          this.$toast.open("Delete finish with success!");
          this.selectedFinish = null;
      }
    },
    deleteColor() {
      if (selectedColor != null) {
        let selectedColorIndex = this.availableColors.indexOf(
          this.selectedColor
        );
        this.availableColors.splice(selectedColorIndex, 1);
        this.selectedColor = null;
        this.$toast.open("Delete color with success!");
      }
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
    newFinish() {
      if (
        this.referenceFinish != null &&
        this.referenceFinish.trim() != "" &&
        this.availableFinishes.indexOf(this.referenceFinish.trim()) < 0
      ) {
        this.availableFinishes.push(this.referenceFinish);
      }
      this.referenceFinish = "";
    },
    newColor() {
      if (
        this.nameColor != null &&
        this.nameColor.trim() != "" &&
        this.nameColors.indexOf(this.nameColor) < 0 &&
        this.availableColors.indexOf(this.color) < 0
      ) {
        this.nameColors.push(this.nameColor);
        this.availableColors.push(this.color);
      }
      this.nameColor = "";
    }
  }
};
</script>