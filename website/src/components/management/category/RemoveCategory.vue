<template>
    <b-modal :active.sync="active" has-modal-card>

                <div class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Remove Category</p>
                    </header>
                    <section class="modal-card-body">
                                            
                        <b-field label="Category">
                            <b-select placeholder="Select a category" icon="tag" v-model="categoryData">
                                  <option :value="null"></option>
                                  <option v-for="category in availableCategories" 
                                    :key="category.id" :value="category.id">{{category.name}} </option>
                            </b-select>                          
                        </b-field>
                    
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-primary" @click="removeCategory">Edit</button>                    
                    </footer>

                    <b-message title="Message" :active.sync="activeMessage">
                        Removed Succesfully
                    </b-message>
                </div>
        
    </b-modal>
</template> 

<script>
import Axios from "axios";
export default {
  name: "RemoveCategory",
  data() {
    return {
      activeMessage: false,
      categoryData: "",
      availableCategories: [],
      active:true
    };
  },

  methods: {
    
    removeCategory() {
      Axios.delete(
        `http://localhost:5000/mycm/api/categories/${this.categoryData}`
      )
        .then(response => {
          activeMessage = true;
        })
        .catch(error => {
          console.log(error);
        });
        availableCategories: [];
      Axios.get("http://localhost:5000/mycm/api/categories")
        .then(response => this.availableCategories.push(...response.data)) //push all elements onto the array
        .catch(function(error) {
          //TODO: inform an error occured while fetching categories
        });
    }
  },
  created() {
    Axios.get("http://localhost:5000/mycm/api/categories")
      .then(response => this.availableCategories.push(...response.data)) //push all elements onto the array
      .catch(function(error) {
        //TODO: inform an error occured while fetching categories
      });
  }
};
</script>
