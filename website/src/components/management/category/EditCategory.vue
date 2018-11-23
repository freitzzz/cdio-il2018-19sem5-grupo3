<template>
    <b-modal :active.sync="activeFlag" has-modal-card>
      
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
       
    </b-modal>
</template> 

<script>
import Axios from "axios";
import { Dialog } from "buefy/dist/components/dialog";
export default {
  name: "EditCategory",
  data() {
    return {
      categoryData: "",
      nameCategory: "",
      availableCategories: [],
      activeFlag: true
    };
  },

  /*  */
  methods: {
    editCategory() {
      var nameCat, cat;
      cat = this.categoryData;
      nameCat = this.nameCategory;

      Axios.put(
        `http://localhost:5000/mycm/api/categories/${this.categoryData}`,
        {
          name: nameCat
        }
      )
        .then(response => {this.$toast.open('Category Edited');})
        .catch(error => {this.$toast.open(error.response.status + 'An error occurred');});

    
    }
  },
  created() {
    Axios.get("http://localhost:5000/mycm/api/categories")
      .then(response => this.availableCategories.push(...response.data)) //push all elements onto the array
      .catch(error => {this.$toast.open(error.response.status + 'An error occurred');
        availableCategories: [];
      });
  }
};
</script>
