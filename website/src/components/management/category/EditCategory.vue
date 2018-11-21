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
      nameCategory:"",
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
 /*    getCategory() {
      alert("entrou");
      Axios.get(
        `http://localhost:5000/mycm/api/categories/${this.categoryData}`
      )
        .then(response => nameCategory.value) //push all elements onto the array
        .catch(function(error) {
          //TODO: inform an error occured while fetching categories
        });
    }, */
    editCategory() {
      alert(this.categoryData);
      Axios.delete(
        `http://localhost:5000/mycm/api/categories/?name=${this.categoryData}`
      )
        .then(response => {})
        .catch(error => {
          console.log("NAO DEU");
        });
      /*Post with just a name */
      /* Axios.put(
        `http://localhost:5000/mycm/api/categories/${this.categoryData}`,
         {
           name: this.categoryData.value
         }
       )
         .then(response => {})
         .catch(error => {}); */
    }
    /*  putCategory: function() {
      var formData = new FormData();
      formData.append("name", this.categoryData.value);
      this.$http.put(
        `http://localhost:5000/mycm/api/categories/$this.categoryData.text`,
        formData
      );
      success(function() {
        this.modalName.showMod = false;
        location.reload();
      }).error(function(data) {
        this.errors.title = data.title;
        this.errors.body = data.body;
        this.modalName.showMod = true;
      });
    } */
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
