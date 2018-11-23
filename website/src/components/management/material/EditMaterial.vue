<template>
    <b-modal :active.sync="active" as-modal-card>
            <div class="modal-card" style="width: auto">
                <div v-if="panelCreateMaterial">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Edit Material</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Materials">
                            <b-select placeholder="Select a material" icon="tag" v-model="materialData">
                                  <option v-for="material in availableMaterials" 
                                    :key="material.id" :value="material">{{material.designation}} </option>
                            </b-select>
                        </b-field>
                        <b-field label="Reference" >
                            <b-input 
                                v-model="materialData.reference"
                                type="String"
                                placeholder="Insert reference"
                                icon="pound">
                            </b-input>
                        </b-field> 
                        <b-field label="Designation" >
                            <b-input 
                                v-model="materialData.designation"
                                type="String"
                                placeholder= "Insert designation"
                                icon="pound">
                            </b-input>
                        </b-field> 
                         <b-field label="Colors">
                            <b-select placeholder="Edit a color" icon="tag" v-model="materialDataColors">
                                  <option v-for="color in materialData.colors" 
                                    :key="color.id" :value="color">{{color.name}} </option>
                            </b-select>
                             </b-field> 
                             <button class="button is-primary" @click="createColor()">+</button>
                            <button class="button is-primary" @click="deleteColor()">-</button>
                             <b-field label="Finishes">
                            <b-select placeholder="Edit a finish" icon="tag" v-model="materialDataFinishes">
                                  <option v-for="finish in materialData.finishes" 
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
                      <button class="button is-primary" @click="updateFinish()">+</button>
                      <button class="button is-primary" @click="desabelFinish()">Back</button>
                    </footer>
                </div>
                <div v-if="createColorPanelEnabled" >
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
                    <br> <br> <br> <br> <br><br> <br> <br>
                    </section>
                    <footer class="modal-card-foot">
                      <button class="button is-primary" @click="updateColor()">+</button>
                      <button class="button is-primary" @click="desabelColor()">Back</button>
                    </footer>
                </div>
            </div>
    </b-modal>
</template> 
<script>
import Axios from "axios";
import Swatches from "vue-swatches";
import "vue-swatches/dist/vue-swatches.min.css";
export default {
  name: "EditMaterial",
  data() {
    return {
      materialData: "",
      nameColor: "",
      referenceFinish: "",
      materialDataColors: "",
      materialDataFinishes: "",
      availableMaterials: [],
      panelCreateMaterial: true,
      createFinishPanelEnabled: false,
      createColorPanelEnabled: false,
      color: "#000000"
    };
  },
  components: {
    Swatches
  },
  props: {
    active: {
      type: Boolean,
      default: false
    }
  },
  methods: {
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
    },
    desabelColor() {
      this.panelCreateMaterial = true;
      this.createColorPanelEnabled = false;
    },
    deleteFinish() {
      Axios.delete(
        `http://localhost:5000/mycm/api/materials/${
          this.materialData.id
        }/finishes/${this.materialDataFinishes.id}`
      )
        .then(this.materialData.finishes.pop(this.materialDataFinishes.id))
        .catch(function(error) {});
    },
    deleteColor() {
      Axios.delete(
        `http://localhost:5000/mycm/api/materials/${
          this.materialData.id
        }/colors/${this.materialDataColors.id}`
      )
        .then(this.materialData.colors.pop(this.materialDataColors.id))
        .catch(function(error) {});
    },
    updateBasicInformation() {
      Axios.put(
        `http://localhost:5000/mycm/api/materials/${this.materialData.id}`,
        {
          reference: this.materialData.reference,
          designation: this.materialData.designation
        }
      )
        .then()
        .catch(function(error) {});
    },
    updateColor() {  
      if (
        this.nameColor != null &&
        this.nameColor.trim() != "" &&
        this.materialData.colors.indexOf(this.nameColor) < 0 
      ) {  
      Axios.post(
        `http://localhost:5000/mycm/api/materials/${this.materialData.id}/colors`,
        {
          name: this.nameColor,
          red: parseInt(this.color.replace("#", "").substring(0, 2), 16),
          green: parseInt(this.color.replace("#", "").substring(2, 4), 16),
          blue: parseInt(this.color.replace("#", "").substring(4, 6), 16),
          alpha: "0"
        }
      )
        .then(this.materialData.colors.push(this.nameColor))
        .catch(function(error) {});    
      } else {
        alert("The inserted color is invalid!");
      }
      nameColor: ""
    },
    updateFinish() {
      if (
        this.referenceFinish != null &&
        this.referenceFinish.trim() != "" &&
        this.materialData.finishes.indexOf(this.referenceFinish.trim()) < 0
      ) {
        Axios.post(
        `http://localhost:5000/mycm/api/materials/${this.materialData.id}/finishes`,
        {
          description: this.referenceFinish
        })
        .then(this.materialData.finishes.push(this.referenceFinish))
        .catch(function(error) {}),
        alert("The reference was successfully inserted!");
        
      } else {
        alert("The inserted reference is invalid!");
      }
      
      this.referenceFinish = "";
    },
  },
  created() {
    Axios.get("http://localhost:5000/mycm/api/materials")
      .then(response => this.availableMaterials.push(...response.data)) //push all elements onto the array
      .catch(function(error) {});
  }
};
</script>
