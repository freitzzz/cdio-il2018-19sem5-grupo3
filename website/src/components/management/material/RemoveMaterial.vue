<template>
    <b-modal :active.sync="active" has-modal-card>
        
                <div class="modal-card" style="width: auto; heigth: auto" >
                    <header class="modal-card-head">
                        <p class="modal-card-title">Remove Material</p>
                    </header>
                    <section class="modal-card-body">  
                        <b-field label="Choose Material">
                            <b-select placeholder="Select a material" icon="tag" v-model="materialData">
                                  <option v-for="material in availableMaterials" 
                                    :key="material.id" :value="material.id">{{material.designation}} </option>
                            </b-select>
                        </b-field>
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-primary" @click="deleteMaterial()">Remove</button>                    
                    </footer>
                </div>
        
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
      availableMaterials: [],
      active:true
    };
  },
  methods: {
    deleteMaterial() {
    Axios.delete(`http://localhost:5000/mycm/api/materials/${this.materialData}`)
      .then(this.availableMaterials.pop(this.materialData), this.materialData=null ) //push all elements onto the array
      .catch(function(error) {
        //TODO: inform an error occured while fetching materials
      });
  }
  },
  created() {
    Axios.get("http://localhost:5000/mycm/api/materials")
      .then(response => this.availableMaterials.push(...response.data)) //push all elements onto the array
      .catch(function(error) {
        //TODO: inform an error occured while fetching materials
      });
  }
};
</script>