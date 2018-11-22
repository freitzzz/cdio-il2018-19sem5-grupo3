<template>
    <b-modal :active.sync="active" has-modal-card>
                <div class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">New Category</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Name">
                            <b-input 
                                v-model="nameCategory"
                                type="String"
                                placeholder="#Category"
                                icon="pound"
                                required>
                            </b-input>
                        </b-field>                     
                        <b-field label="Parent Category">
                            <b-select placeholder="Select a category" icon="tag" v-model="parentCategoryId">
                                  <option :value="null"></option>
                                  <option v-for="category in availableCategories" 
                                    :key="category.id" :value="category.id">{{category.name}}</option>
                            </b-select>
                        </b-field>
                     
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-primary" @click="postCategory">Create</button>
             
                    </footer>
                </div>
       
    </b-modal>
</template> 

<script>
import Axios from "axios";
export default {
  name: "CreateNewCategory",
  data() {
    return {
      nameCategory: "",
      parentCategoryId: null, //this value needs to be null for the placeholder to work
      availableCategories: [],
      active: false
    };
  },

  /*  */
  methods: {
    postCategory() {
      if (this.parentCategoryId === null) {
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
            this.parentCategoryId
          }/subcategories`,
          {
            name: this.nameCategory
          }
        )
          .then(response => {})
          .catch(error => {});
        this.$toast.open("Category Created");
       
      }
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
