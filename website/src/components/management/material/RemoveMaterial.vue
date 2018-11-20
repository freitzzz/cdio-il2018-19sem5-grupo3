<template>
    <b-modal :active.sync="active" has-modal-card>
        <form action="">
                <div class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Remove Material</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Choose Material">
                            <b-select placeholder="Select a material" icon="tag" v-model="materialchoose">
                                  <option :value="null"></option>
                                  <option v-for="material in availableMaterials" 
                                    :key="material.id" :value="material.id">{{material.name}}</option>
                            </b-select>
                        </b-field>
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-primary" @click="deleteMaterial">Submit</button>
                    </footer>
                </div>
            </form>
    </b-modal>
</template>

<script>
import Axios from "axios";
export default {
  name: "RemoveMaterial",
  data() {
    return {
      materialchoose: null, //this value needs to be null for the placeholder to work
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
    deleteMaterial() {
      if (this.materialchoose != null) {
          id: this.materialchoose.id,
        Axios.post("http://localhost:5000/mycm/api/materials/id")
          .then(response => {})
          .catch(error => {});
      } 
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

