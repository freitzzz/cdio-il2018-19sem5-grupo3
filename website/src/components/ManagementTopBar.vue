 <template>
<nav class="navbar" role="navigation" aria-label="main navigation">

  <div id="navBarManagement" class="navbar-menu">
        <div class="navbar-start">
        <div class="navbar-item has-dropdown is-hoverable">
        <a class="navbar-link" @click="enableListCategories">
          Category
        </a>

        <a class="navbar-link">
          Material
        </a>

        <div class="navbar-dropdown">
          <a class="navbar-item" @click="enableCreateMaterial">
            Add Material
          </a>
          <a class="navbar-item" @click="enableEditMaterial">
            Edit Material
          </a>
          <a class="navbar-item"  @click="enableRemoveMaterial">
            Remove Material
          </a>
         
        </div>
      </div>

       
        <div class="navbar-start">
        <div class="navbar-item has-dropdown is-hoverable">
        <a class="navbar-link" @click="enableListProducts">
          Product
        </a>    
      </div>
        <a class="navbar-item"  @click="enableWIP">
            Create Customized Product
        </a>
         
    
      </div>
    </div>

    <div class="navbar-end">
      <div class="navbar-item">
        <a class="navbar-item">
            Welcome, Content Manager
        </a>
        <i class="far fa-user-circle" style="font-size:30px"></i>
      </div>
    </div>
  </div>
<div id="management">
  <b-message title="Information" v-if="WIPFlag">
    Feature is not implemented yet.
  </b-message>
  <create-category v-if="CustomCreateCategory"></create-category>
  <edit-category v-if="CustomEditCategory"></edit-category>
  <remove-category v-if="CustomRemoveCategory"></remove-category>

  <create-material v-if="CustomCreateMaterial"></create-material>
   <edit-material v-if="CustomEditMaterial"></edit-material>
  <remove-material v-if="CustomRemoveMaterial"></remove-material>
</div>



<section v-if="CustomListProducts" style="width:100%">
    <list-products />
  </section>
</nav>



</template> 

<style scoped>
.tag {
  cursor: pointer;
}
</style>



<script>
import CreateCategory from "./management/category/CreateNewCategory.vue";
import EditCategory from "./management/category/EditCategory.vue";
import RemoveCategory from "./management/category/RemoveCategory.vue";
import CreateMaterial from "./management/material/CreateMaterial.vue";
import EditMaterial from "./management/material/EditMaterial.vue";
import RemoveMaterial from "./management/material/RemoveMaterial.vue";
import ListProducts from './management/product/ListProducts.vue';
export default {
  data() {
    return {
      activeTab: 0,
      showBooks: false,
      CustomListCategories:false,
      WIPFlag: false,
      /*   CountSamePage: 0, //Counts the amount of times*/
      CustomCreateMaterial: false,
      CustomEditMaterial: false,
      CustomRemoveMaterial: false,

      CustomListProducts:false
    };
  },
  methods: {
    enableListCategory() {
      if (this.CustomCreateCategory === true) {
        this.CustomCreateCategory = false;
      }
      this.CustomCreateCategory = true;
      this.CustomEditCategory = false;
      this.CustomRemoveCategory = false;
      this.WIPFlag = false;
      disableMaterial();
    },
 
    enableCreateMaterial() {
      this.CustomCreateMaterial = true;
      this.CustomEditMaterial = false;
      this.CustomRemoveMaterial = false;
      disableCategory();
    },
    enableListProducts(){
      this.CustomCreateMaterial = false;
      this.CustomEditMaterial = false;
      this.CustomRemoveMaterial = false;
      this.CustomListProducts=true;
    },
    disableCategory(){
      /* Category: */
      this.CustomEditCategory = false;
      this.CustomCreateCategory = false;
      this.CustomRemoveCategory = false;
      this.WIPFlag = false;
    },
    disableMaterial(){
      this.CustomEditMaterial = false;
      this.CustomCreateMaterial = false;
      this.CustomRemoveMaterial = false;
    },

     enableEditMaterial() {
      this.CustomEditMaterial = true;
      this.CustomCreateMaterial = false;
      this.CustomRemoveMaterial = false;
      disableCategory();
    },
    enableRemoveMaterial() {
      this.CustomRemoveMaterial = true;
      this.CustomCreateMaterial = false;
      this.CustomEditMaterial = false;
      disableCategory();
    },
    enableWIP() {
      this.WIPFlag = true;
    }
  },
  components: {
    CreateCategory,
    EditCategory,
    RemoveCategory,
    CreateMaterial,
    EditMaterial,
    RemoveMaterial,
    ListProducts
  }
};
</script>
