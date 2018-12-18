<template>
    <b-modal :active.sync="activeFlag" has-modal-card scroll="keep">
            <div v-if = panelEditMaterial class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Edit Material</p>
                    </header>
                    <section class="modal-card-body">
                          <b-field label="Reference" >
                            <b-input 
                                v-model="material.reference"
                                type="String"
                                icon="pound">
                            </b-input>
                        </b-field> 
                        <b-field label="Designation" >
                            <b-input 
                                v-model="material.designation"
                                type="String"
                                icon="pound">
                            </b-input>
                        </b-field> 
                        <b-field label="Edit Colors">
                            <b-select icon="tag" v-model="selectedColor">
                                  <option v-for="color in material.colors" 
                                    :key="color.id" :value="color">{{color.name}} </option>
                            </b-select>
                        </b-field>
                              <button class="btn-primary" @click="createColor()">+</button>
                              <button class="btn-primary" @click="deleteColor()">-</button>
                        <b-field label="Edit Finishes">
                            <b-select icon="tag" v-model="selectedFinish">
                                  <option v-for="finish in material.finishes" 
                                    :key="finish.id" :value="finish">{{finish.description}} </option>
                            </b-select>
                        </b-field> 
                            <button class="btn-primary" @click="createFinish()">+</button>
                            <button class="btn-primary" @click="deleteFinish()">-</button>
                            <div class="example-btn , image">
                                <file-upload
                                  class="btn-primary"
                                  post-action="/files/"
                                  :maximum="1"
                                  :drop="true"
                                  :drop-directory="true"
                                  v-model="file"
                                  @input-file="inputFile"
                                  ref="upload">
                                  Edit Image
                                </file-upload>
                                <b-input
                                class="image"
                                v-model="material.image"
                                type="String"
                                disabled="true"
                                required>
                            </b-input>
                            </div>
                    </section>
                    <footer class="modal-card-foot">
                        <button class="btn-primary" @click="updateBasicInformation()">Edit</button>                    
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
                          <b-field label="Shininess:">
                            <vue-slider
                            class="slidersSection"
                            :min="0"
                            :max="100"
                            v-model="inputFinishShininess"
                            :interval="0.01"
                          ></vue-slider>
                         </b-field>
                    </section>
                    <footer class="modal-card-foot">
                      <small-padding-div>
                        <button class="btn-primary" @click="addFinish()">+</button>
                        <button class="btn-primary" @click="disableFinishWindow()">Back</button>
                      </small-padding-div>
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
                      <small-padding-div>
                        <button class="btn-primary" @click="addColor()">+</button>
                        <button class="btn-primary" @click="disableColorWindow()">Back</button>
                      </small-padding-div>
                    </footer>
            </div>
        </b-modal>
</template> 
<script>
import Axios from "axios";
import Swatches from "vue-swatches";
import "vue-swatches/dist/vue-swatches.min.css";
import { Dialog } from "buefy/dist/components/dialog";
import FileUpload from 'vue-upload-component';
import vueSlider from "vue-slider-component";
export default {
  name: "EditMaterial",
  data() {
    return {
      selectedColor: {},
      selectedFinish: {},
      inputColorName: null, //this value needs to be null so that the placeholder can be rendered
      inputColorValues: "#000000",
      inputFinishDesignation: null,
      inputFinishShininess: 0, //this value needs to be null so that the placeholder can be rendered
      availableMaterials: [],
      panelEditMaterial: true,
      createFinishPanelEnabled: false,
      createColorPanelEnabled: false,
      activeFlag: true,
      file: [],
    };
  },
  components: {
    Swatches,
    FileUpload,
    vueSlider
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
          this.material.id
        }/finishes/${this.selectedFinish.id}`
      )
        .then(
          this.material.finishes.splice(this.selectedFinish, 1),
          this.$toast.open("Delete finish with success!")
        )
        .catch(function(error) {});
    },
    deleteColor() {
      Axios.delete(
        `http://localhost:5000/mycm/api/materials/${
          this.material.id
        }/colors/${this.selectedColor.id}`
      )
        .then(response => {
          let selectedColorIndex = this.material.colors.indexOf(
            this.selectedColor
          );
          this.material.colors.splice(selectedColorIndex, 1);
          this.selectedColor = null;
        }, this.$toast.open("Delete color with success!"))
        .catch(function(error) {});
    },
    updateBasicInformation() {
      Axios.put(
        `http://localhost:5000/mycm/api/materials/${this.material.id}`,
        {
          reference: this.material.reference,
          designation: this.material.designation,
          image: this.material.image
        }
      )
        .then(this.$toast.open("Update te basic information with success!"))
        .catch(function(error) {});
    },
    addColor() {
      if (
        this.inputColorName != null &&
        this.inputColorName.trim() != "" &&
        this.material.colors.indexOf(this.inputColorName) < 0
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
            this.material.id
          }/colors`,
          color
        )
          .then(
            this.material.colors.push(color),
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
        this.material.finishes.indexOf(
          this.inputFinishDesignation.trim()
        ) < 0
      ) {
        var finish = {
          description: this.inputFinishDesignation,
          shininess: this.inputFinishShininess
        };

        Axios.post(
          `http://localhost:5000/mycm/api/materials/${
            this.material.id
          }/finishes`,
          finish
        )
          .then(
            this.material.finishes.push(finish),
            this.$toast.open("Create finish with success!")
          )
          .catch(function(error) {});
      }
      this.inputFinishDesignation = "";
      this.inputFinishShininess = 0;
    },
  inputFile(newFile, oldFile) {
    this.material.image=this.file[0].name
      if (newFile && !oldFile) {
        // add
        console.log('add', newFile)
      }
      if (newFile && oldFile) {
        // update
        console.log('update', newFile)
      }
      if (!newFile && oldFile) {
        // remove
        console.log('remove', oldFile)
      }
    }
  },
  created() {
    Axios.get("http://localhost:5000/mycm/api/materials")
      .then(response => this.availableMaterials.push(...response.data)) //push all elements onto the array
      .catch(error => {
        this.$toast.open(error.response.status + "Not found materials");
      });
  },
  props:{
        /**
         * Current Material details
         */
        material:{
            type:Object,
            required:true
        }
    },
};
</script>
<style>
.slidersSection {
  margin-bottom: 13%;
  width: 7px 30px;
  margin-top: 15%;
}
.image{
  margin-top: 5%;
}
</style>
