<template>
    <b-modal :active.sync="active" has-modal-card>
        <form action="">
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
                        <b-field label="Category">
                            <b-select v-model="parentCategory" placeholder="Select a category" icon="tag" >
                                <optgroup label="Parent Categories">
                                <!--  <ul id = "example-1"> <li v-for = "item in items">     {{ item.message }} </li > </ul > -->

                                    <option value="flint">---</option>
                                    <option value="flint">Category 1</option>
                                    <option value="flint">Category 2</option>
                                    <option value="flint">Category 3</option>
                                </optgroup>
                            </b-select>
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
      parentCategory: ""
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
      if (new String(parentCategory).equals("---")) {
        /* Post with just a name */
        Axios.post("http://localhost:5000/mycm/api/categories", {
          name: nameCategory
        })
          .then(function(response) {
            alert("Vinho");
            console.log(response);
          })
          .catch(function(error) {
            alert("Vinho Branco");
            console.log(error);
          });
      } else {
        /* Post with just a name */
        Axios.post(
          "http://localhost:5000/mycm/api//categories/parentCategory/subcategories",
          {
            name: nameCategory
          }
        )
          .then(function(response) {
            console.log(response);
          })
          .catch(function(error) {
            console.log(error);
          });
      }
    }
  }
  /*  created() {
    this.getCategories();
  } */
};
</script>
