<template>
    <b-modal :active.sync="active" has-modal-card>
        <form action="">
                <div class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">Edit Category</p>
                    </header>
                    <section class="modal-card-body">
                                            
                        <b-field label="Category">
                            <b-select placeholder="Select a category" icon="tag" v-model="categoryData">
                                  <option :value="null"></option>
                                  <option v-for="category in availableCategories" 
                                    :key="category.id" :value="category.id">{{category.name}} </option>
                            </b-select>
                            
                        </b-field>
                        <b-field label="Name" >
                            <b-input 
                                v-model="nameCategory"
                                type="String"
                                placeholder="Category"
                                icon="pound"
                                >
                            </b-input>
                            
                        </b-field> 
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-primary" @click="editCategory">Edit</button>                    
                    </footer>
                </div>
            </form>
    </b-modal>
</template> 

<script>
import Axios from "axios";
export default {
  name: "EditCategory",
  data() {
    return {
      categoryData: "",
      nameCategory: "",
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
    editCategory() {
      var nameCat,cat;
      cat = this.categoryData;
      nameCat = this.nameCategory;
      
      Axios.delete(
        `http://localhost:5000/mycm/api/categories/${this.categoryData}`
      )
        .then(response => {})
        .catch(error => {
          console.log("NAO DEU");
        });
      /* Clear buffer of all available categories */
      availableCategories: [];
      Axios.get("http://localhost:5000/mycm/api/categories")
        .then(response => this.availableCategories.push(...response.data)) //push all elements onto the array
        .catch(function(error) {
          //TODO: inform an error occured while fetching categories
          console.log("O get nao funcionou");
        });

     
        /* Post with just a name */
        Axios.post(
          `http://localhost:5000/mycm/api/categories/`,
          {
            name: nameCat
          }
        )
          .then(response => {})
          .catch(error => {});
      
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
