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
                                <option  v-for="color in availableColors" 
                                    :key="color.id" 
                                    :value="color">
                                    {{color.name}}</option>
                            </b-select>
                        </b-field>
                        <button class="button is-primary" @click="createColor()">+</button>
                        <button class="button is-primary" @click="deleteColor()">-</button>
                        <b-field label="Finish">
                            <b-select placeholder="Finishes" icon="tag" v-model="selectedFinish">
                                 <option v-for="finish in availableFinishes" 
                                    :key="finish.id" :value="finish">{{finish.description}} </option>
                            </b-select>
                        </b-field>
                        <button class="button is-primary" @click="createFinish()">+</button>
                        <button class="button is-primary" @click="deleteFinish()">-</button>

                              <div class="example-btn , image">
                                <file-upload
                                  class="button is-primary"
                                  post-action="/files/"
                                  :maximum="1"
                                  :drop="true"
                                  :drop-directory="true"
                                  v-model="file"
                                  @input-file="inputFile"
                                  ref="upload">
                                  Select Image
                                </file-upload>
                                <b-input
                                class="image"
                                v-model="nameImage"
                                type="String"
                                placeholder="Name the Image"
                                disabled="true"
                                required>
                            </b-input>
                            </div>
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-primary" @click="postMaterial()">Create</button>
                    </footer>
                </div>
                <div v-if= "createFinishPanelEnabled" class="modal-card" style="width: auto">
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
                      <button class="button is-primary" @click="addFinish()">+</button>
                      <button class="button is-primary" @click="disableFinishWindow()">Back</button>
                    </footer>
                </div>
                <div v-if="createColorPanelEnabled" class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Create Color</p>
                    </header>
                    <section class="modal-card-body">
                    <b-field label="Name">
                            <b-input
                                v-model="inputColorName"
                                type="String"
                                placeholder="Insert name"
                                required>
                            </b-input>
                        </b-field>
                    <swatches v-model="inputColorValues" colors="text-advanced"></swatches>
                    <br> <br> <br> <br> <br><br> <br>
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
import FileUpload from 'vue-upload-component';
import vueSlider from "vue-slider-component";
export default {
  name: "CreateMaterial",
  data() {
    return {
      referenceMaterial: "",
      referenceFinish: "",
      designationMaterial: "",
      nameImage:"",
      panelCreateMaterial: true,
      createColorPanelEnabled: false,
      createFinishPanelEnabled: false,
      defineNewFinish: false,
      createNewFinish: false,
      inputColorName: "", //this value needs to be null so that the placeholder can be rendered
      inputColorValues: "#000000",
      inputFinishDesignation: "",
      inputFinishShininess: 0, //this value needs to be null so that the placeholder can be rendered
      availableColors: [],
      availableFinishes: [],
      active: true,
      file: [],
      selectedFinish:{},
      selectedColor:{}

    };
  },
  components: {
    FileUpload,
    Swatches,
    vueSlider
    
  }, // window.VueSwatches.default - from CDN

  methods: {
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
          name: element.name,
          red: element.red,
          green: element.green,
          blue: element.blue,
          alpha: "0",
        });
      })
      
      Axios.post("http://localhost:5000/mycm/api/materials", {
        reference: this.referenceMaterial,
        designation: this.designationMaterial,
        image: this.nameImage,
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
    disableFinishWindow() {
      this.panelCreateMaterial = true;
      this.createFinishPanelEnabled = false;
    },
    disableColorWindow() {
      this.panelCreateMaterial = true;
      this.createColorPanelEnabled = false;
    },
    addColor() {
      if (
        this.inputColorName != null &&
        this.inputColorName.trim() != "" &&
        this.availableColors.indexOf(this.inputColorName.trim()) < 0
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
        }
        this.availableColors.push(color)
      }
      this.inputColorName = "";
    },
    addFinish() {
      if (
        this.inputFinishDesignation != null &&
        this.inputFinishDesignation.trim() != "" &&
        this.availableFinishes.indexOf(
          this.inputFinishDesignation.trim()
        ) < 0
      ) {
        var finish = {
          description: this.inputFinishDesignation,
          shininess: this.inputFinishShininess
        }
        this.availableFinishes.push(finish)
      }
      this.inputFinishDesignation = "";
    },
    inputFile(newFile, oldFile) {
      this.nameImage=this.file[0].name
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
  }
};
</script>
<style>
.slidersSection {
  margin-bottom: 13%;
  width: 7px 30px;
  margin-top: 7%;
}
.image{
  margin-top: 2%;
}
</style>