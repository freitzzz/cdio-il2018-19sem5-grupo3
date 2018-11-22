 <template>
<nav class="navbar" role="navigation" aria-label="main navigation">

  <div id="navBarManagement" class="navbar-menu">
    <div class="navbar-start">
        <div class="navbar-item has-dropdown is-hoverable">
        <a class="navbar-link">
          Category
        </a>

        <div class="navbar-dropdown">
          <a class="navbar-item" @click="enableCreateCategory">
            Create Category
          </a>
          <a class="navbar-item" @click="enableEditCategory">
            Edit Category
          </a>
          <a class="navbar-item" @click="enableRemoveCategory">
            Remove Category
          </a>
        </div>
      </div>

    <div class="navbar-item has-dropdown is-hoverable">
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
        <a class="navbar-link">
          Product
        </a>

        <div class="navbar-dropdown">
          <a class="navbar-item">
            Create Product
          </a>
          <a class="navbar-item">
            Edit Product
          </a>
          <a class="navbar-item">
            Remove Product
          </a>
        </div>

     
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
  <b-message title="Default" v-if="WIPFlag">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce id fermentum quam. Proin sagittis, nibh id hendrerit imperdiet, elit sapien laoreet elit
  </b-message>
  <create-category v-if="CustomCreateCategory"></create-category>
  <edit-category v-if="CustomEditCategory"></edit-category>
  <remove-category v-if="CustomRemoveCategory"></remove-category>
  <create-material v-if="CustomCreateMaterial"></create-material>
  <edit-material v-if="CustomEditMaterial"></edit-material>
  <remove-material v-if="CustomRemoveMaterial"></remove-material>
</div>
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
export default {
  data() {
    return {
      activeTab: 0,
      showBooks: false,
      CustomCreateCategory: false,
      CustomEditCategory: false,
      CustomRemoveCategory: false,
      CustomCreateMaterial: false,
      CustomEditMaterial: false,
      CustomRemoveMaterial: false,
      WIPFlag: false
    };
  },
  methods: {
    enableCreateCategory() {
        this.CustomCreateCategory = true;
        this.CustomEditCategory = false;
        this.CustomRemoveCategory = false;
    },
    enableEditCategory() {
      this.CustomEditCategory = true;
      this.CustomCreateCategory = false;
      this.CustomRemoveCategory = false;
    },
    enableRemoveCategory() {
      this.CustomRemoveCategory = true;
      this.CustomCreateCategory = false;
      this.CustomEditCategory = false;
    },
    enableCreateMaterial() {
        this.CustomCreateMaterial = true;
        this.CustomEditMaterial = false;
        this.CustomRemoveMaterial = false;
    },
    enableEditMaterial() {
      this.CustomEditMaterial = true;
      this.CustomCreateMaterial = false;
      this.CustomRemoveMaterial = false;
    },
    enableRemoveMaterial() {
      this.CustomRemoveMaterial = true;
      this.CustomCreateMaterial = false;
      this.CustomEditMaterial = false;
    },
    enableWIP() {
      WIPFlag = true;
    }
  },
  components: {
    CreateCategory,
    EditCategory,
    RemoveCategory,
    CreateMaterial,
    EditMaterial,
    RemoveMaterial
  }
};
</script>
