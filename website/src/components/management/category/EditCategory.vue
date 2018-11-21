<template>
    <b-modal :active.sync="active" has-modal-card>
        <form action="">
                <div class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Edit Category</p>
                    </header>
                    <section class="modal-card-body">
                                            
                        <b-field label="Category">
                            <b-select placeholder="Select a category" icon="tag" v-model="nameCategory">
                                  <option :value="null"></option>
                                  <option v-for="category in availableCategories" 
                                    :key="category.id" :value="category.name">{{category.name}}</option>
                            </b-select>
                        </b-field>
                        <b-field label="Name">
                            <b-input 
                                v-bind:value = "nameCategory"
                                type="String"
                                placeholder="Category"
                                icon="pound"
                                >
                                
                            </b-input>
                            
                        </b-field> 
                     
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-primary" @click="postCategory">Create</button>
                        
                    </footer>
                </div>
            </form>
    </b-modal>
</template> 

<script>
import Axios from "axios";
export default {
  name: "CreateNewCategory",
  data() {
    return {
      nameCategory: "",
      //categoryId: null, //this value needs to be null for the placeholder to work
      availableCategories: []
    };
  },
  props: {
    active: {
      type: Boolean,
      default: false
    }
  },

  /*  */
  methods: {
    postCategory() {
      if (this.categoryId === null) {
        /* Post with just a name */
        Axios.post("http://localhost:5000/mycm/api/categories", {
          name: this.nameCategory
        })
          .then(response => {})
          .catch(error => {});
      } else {
        /* Post with just a name */
        Axios.post(
          `http://localhost:5000/mycm/api/categories/${
            this.categoryId
          }/subcategories`,
          {
            name: this.nameCategory
          }
        )
          .then(response => {})
          .catch(error => {});
      }
    },
    getName(){
        return this.nameCategory;
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
