<template>
    <b-modal :active.sync="active" has-modal-card>
        <form action="">
                <div class="modal-card" style="width: auto">
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
                                v-model="this.materialData.reference"
                                type="String"
                                placeholder="Insert reference"
                                icon="pound">
                            </b-input>
                        </b-field> 
                        <b-field label="Desigantion" >
                            <b-input 
                                v-model="this.materialData.designation"
                                type="String"
                                placeholder= "Insert designation"
                                icon="pound">
                            </b-input>
                        </b-field> 
                         <b-field label="Colors">
                            <b-select placeholder="Edit a color" icon="tag" v-model="materialData.colors">
                                  <option v-for="color in availableMaterials" 
                                    :key="color.name" :value="color">{{color.name}} </option>
                            </b-select>
                             </b-field> 
                             
                            <button class="button is-primary" @click="deleteColor()">-</button>
                             <button class="button is-primary" @click="newFinish()">Edit</button>
                             <b-field label="Finish">
                            <b-select placeholder="Edit a finish" icon="tag" v-model="materialData.finishes">
                                  <option v-for="finish in availableMaterials" 
                                    :key="finish.designation" :value="finish">{{finish.designation}} </option>
                            </b-select>
                            
                            
                             </b-field> 
                             
                            <button class="button is-primary" @click="deleteFinish()">-</button>
                             <button class="button is-primary" @click="newFinish()">Edit</button>
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-primary" @click="deleteMaterial(), postMaterial()" >Edit</button>                    
                    </footer>
                </div>
            </form>
    </b-modal>
</template> 
<script>
import Axios from "axios";
export default {
  name: "EditMaterial",
  data() {
    return {
      materialData: "",
      colorsData: "",
      referenceMaterial:"",
      designationMaterial:"",
      availableMaterials: []
    };
  },
  props: {
    active: {
      type: Boolean,
      default: false
    }
  },
  methods: {
      deleteFinish() {
    Axios.delete(`http://localhost:5000/mycm/api/materials/${this.materialData}/colors/${color.id}`)
      .then() //push all elements onto the array
      .catch(function(error) {
        //TODO: inform an error occured while fetching categories
      });
  }
    
  },
  created() {
    Axios.get("http://localhost:5000/mycm/api/materials")
      .then(response => this.availableMaterials.push(...response.data)) //push all elements onto the array
      .catch(function(error) {
        //TODO: inform an error occured while fetching categories
      });
  }
};
</script>
