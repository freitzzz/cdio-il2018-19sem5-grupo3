<template>
    <b-modal :active.sync="active" as-modal-card>
                <div  v-if="panelCreateMaterial" class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Create Material</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Reference">
                            <b-input
                                type="String"
                                :value.sync="referenceMaterial"
                                placeholder="Insert reference"
                                icon="pound"
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Designation">
                            <b-input
                                type="String"
                                :value.sync="designation"
                                placeholder="Insert designation"
                                icon="pound"
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Colors">
                            <b-select placeholder="Colors" icon="tag" >
                                
                            </b-select>
                        </b-field>
                         <button class="button is-primary" @click="createColor()">+</button>
                        <b-field label="Finish">
                            <b-select placeholder="Finishes" icon="tag">
                            </b-select>
                        </b-field>
                        <button class="button is-primary" @click="createFinish()">+</button>
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-primary" @click="postMaterial()">Create</button>
                    </footer>
                </div>
                <div  v-if="createFinishPanelEnabled" class="modal-card" style="width: auto">
                <div class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Create Finish</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Designation: ">
                        <div v-if="createNewDesignation">
                            <b-input
                                type="String"
                                :value.sync="referenceFnish"
                                placeholder="Insert designation"
                                icon="pound"
                                required>
                            </b-input>
                             </div>
                        </b-field>
                        <button class="button is-primary">-</button>
                    </section>
                    <footer class="modal-card-foot">
                      <button class="button is-primary" @click="newFinish()">+</button>
                      <button class="button is-primary" @click="desabelFinis()">Back</button>
                    </footer>
                </div>
                 </div>
    </b-modal>
</template>
<script>
import CreateFinish from './CreateFinish.vue'
export default {
  
    name:"CreateMaterial",
    data() {
    return {
      referenceMaterial: "",
      referenceFnish: "",
      designation:"",
      panelCreateMaterial: true,
      createFinishPanelEnabled: false,
      defineNewFinish: false,
      createNewDesignation: true,
    }
    
  },
  props: {
     active: {
      type: Boolean,
      default: true
      }
  },

  /*  */
  methods: {
    postMaterial() {
     
        Axios.post("http://localhost:5000/mycm/api/materials", {
          reference: this.referenceMaterial,
          designation: this.designation
        })
          .then(response => {})
          .catch(error => {});
      },
      createColor(){
        alert("Cor");
      },
      createFinish(){
        this.panelCreateMaterial = false;
        this.createFinishPanelEnabled = true;
      },
      desabelFinis(){
        this.panelCreateMaterial = true;
        this.createFinishPanelEnabled = false;
      },
      newFinish(){
        this.defineNewFinish = true;
        
        
      }

  }
};
</script>