<template>
    <b-modal :active.sync="activeFlag" has-modal-card scroll="keep">
            <div v-if = "panelEditMaterial" class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Edit Material</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Select a material">
                            <b-select icon="tag" v-model="selectedMaterial">
                                  <option v-for="material in availableMaterials" 
                                    :key="material.id" :value="material">{{material.designation}}</option>
                            </b-select>
                        </b-field>
                        <b-field label="Reference" >
                            <b-input 
                                v-model="selectedMaterial.reference"
                                type="String"
                                placeholder="Insert reference"
                                icon="pound">
                            </b-input>
                        </b-field> 
                        <b-field label="Designation" >
                            <b-input 
                                v-model="selectedMaterial.designation"
                                type="String"
                                placeholder= "Insert designation"
                                icon="pound">
                            </b-input>
                        </b-field> 
                        <b-field label="Edit Colors">
                            <b-select icon="tag" v-model="selectedColor">
                                  <option v-for="color in selectedMaterial.colors" 
                                    :key="color.id" :value="color">{{color.name}} </option>
                            </b-select>
                        </b-field> 
                            <button class="button is-primary" @click="createColor()">+</button>
                            <button class="button is-primary" @click="deleteColor()">-</button>
                        <b-field label="Edit Finishes">
                            <b-select icon="tag" v-model="selectedFinish">
                                  <option v-for="finish in selectedMaterial.finishes" 
                                    :key="finish.id" :value="finish">{{finish.description}} </option>
                            </b-select>
                        </b-field> 
                            <button class="button is-primary" @click="createFinish()">+</button>
                            <button class="button is-primary" @click="deleteFinish()">-</button>
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-primary" @click="updateBasicInformation()">Edit</button>                    
                    </footer>
            </div>
            <div v-if = "createFinishPanelEnabled" class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Create Finish</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Designation: ">
                            <b-input
                                v-model="inputFinishDesignation" 
                                type="String"
                                placeholder="Insert reference"
                                icon="pound"
                                required>
                            </b-input>
                        </b-field>
                    </section>
                    <footer class="modal-card-foot">
                      <button class="button is-primary" @click="addFinish()">+</button>
                      <button class="button is-primary" @click="disableFinishWindow()">Back</button>
                    </footer>    
                 </div>
            <div v-if= "createColorPanelEnabled" class="modal-card" style="width: auto" >
                    <header class="modal-card-head">
                        <p class="modal-card-title">Create Color</p>
                    </header>
                    <section class="modal-card-body">
                      <b-field label="Name">
                            <b-input
                                v-model="inputColorName"
                                type="String"
                                placeholder="Insert name"
                                icon="pound"
                                required>
                            </b-input>
                        </b-field>
                      <swatches v-model="inputColorValues" colors="text-advanced"></swatches>
                      <br> <br> <br> <br> <br><br> <br> <br>
                    </section>
                    <footer class="modal-card-foot">
                      <button class="button is-primary" @click="addColor()">+</button>
                      <button class="button is-primary" @click="disableColorWindow()">Back</button>
                    </footer>
            </div>
        </b-modal>
</template> 
<script>
import Axios from "axios";
import Swatches from "vue-swatches";
import "vue-swatches/dist/vue-swatches.min.css";
import { Dialog } from "buefy/dist/components/dialog";
export default {
  name: "EditMaterial",
  data() {
    return {
      selectedMaterial: {},
      selectedColor: {},
      selectedFinish: {},
      inputColorName: null, //this value needs to be null so that the placeholder can be rendered
      inputColorValues: "#000000",
      inputFinishDesignation: null, //this value needs to be null so that the placeholder can be rendered
      availableMaterials: [],
      panelEditMaterial: true,
      createFinishPanelEnabled: false,
      createColorPanelEnabled: false,
      activeFlag: true
    };
  },
  components: {
    Swatches
  },
  methods: {
    createColor() {
      this.panelEditMaterial = false;
      this.createColorPanelEnabled = true;
    },
    createFinish() {
      this.panelEditMaterial = false;
      this.createFinishPanelEnabled = true;
    },
    disableFinishWindow() {
      this.panelEditMaterial = true;
      this.createFinishPanelEnabled = false;
    },
    disableColorWindow() {
      this.panelEditMaterial = true;
      this.createColorPanelEnabled = false;
    },
    deleteFinish() {
      Axios.delete(
        `http://localhost:5000/mycm/api/materials/${
          this.selectedMaterial.id
        }/finishes/${this.selectedFinish.id}`
      )
        .then(
          this.selectedMaterial.finishes.splice(this.selectedFinish, 1),
          this.$toast.open("Delete finish with success!")
        )
        .catch(function(error) {});
    },
    deleteColor() {
      Axios.delete(
        `http://localhost:5000/mycm/api/materials/${
          this.selectedMaterial.id
        }/colors/${this.selectedColor.id}`
      )
        .then(response => {
          let selectedColorIndex = this.selectedMaterial.colors.indexOf(
            this.selectedColor
          );
          this.selectedMaterial.colors.splice(selectedColorIndex, 1);
          this.selectedColor = null;
        }, this.$toast.open("Delete color with success!"))
        .catch(function(error) {});
    },
    updateBasicInformation() {
      Axios.put(
        `http://localhost:5000/mycm/api/materials/${this.selectedMaterial.id}`,
        {
          reference: this.selectedMaterial.reference,
          designation: this.selectedMaterial.designation
        }
      )
        .then(this.$toast.open("Update te basic information with success!"))
        .catch(function(error) {});
    },
    addColor() {
      if (
        this.inputColorName != null &&
        this.inputColorName.trim() != "" &&
        this.selectedMaterial.colors.indexOf(this.inputColorName) < 0
      ) {
        let color = {
          name: this.inputColorName,
          red: parseInt(
            this.inputColorValues.replace("#", "").substring(0, 2),
            16
          ),
          green: parseInt(
            this.inputColorValues.replace("#", "").substring(2, 4),
            16
          ),
          blue: parseInt(
            this.inputColorValues.replace("#", "").substring(4, 6),
            16
          ),
          alpha: "0"
        };

        Axios.post(
          `http://localhost:5000/mycm/api/materials/${
            this.selectedMaterial.id
          }/colors`,
          color
        )
          .then(
            this.selectedMaterial.colors.push(color),
            this.$toast.open("Create color with success!")
          )
          .catch(function(error) {});
      }
      this.inputColorName = "";
    },
    addFinish() {
      if (
        this.inputFinishDesignation != null &&
        this.inputFinishDesignation.trim() != "" &&
        this.selectedMaterial.finishes.indexOf(
          this.inputFinishDesignation.trim()
        ) < 0
      ) {
        var finish = {
          description: this.inputFinishDesignation
        };

        Axios.post(
          `http://localhost:5000/mycm/api/materials/${
            this.selectedMaterial.id
          }/finishes`,
          finish
        )
          .then(
            this.selectedMaterial.finishes.push(finish),
            this.$toast.open("Create finish with success!")
          )
          .catch(function(error) {});
      }
      this.inputFinishDesignation = "";
    }
  },
  created() {
    Axios.get("http://localhost:5000/mycm/api/materials")
      .then(response => this.availableMaterials.push(...response.data)) //push all elements onto the array
      .catch(error => {
        this.$toast.open(error.response.status + "Not found materials");
      });
  }
};
</script>
