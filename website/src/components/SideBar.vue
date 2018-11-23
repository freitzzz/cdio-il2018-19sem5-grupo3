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
          <a class="navbar-item">
            Add Material
          </a>
          <a class="navbar-item">
            Edit Material
          </a>
          <a class="navbar-item">
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
  <b-message title="Information" v-if="WIPFlag">
    Feature is not implemented yet.
  </b-message>
  <create-category v-if="CustomCreateCategory"></create-category>
  <edit-category v-if="CustomEditCategory"></edit-category>
  <remove-category v-if="CustomRemoveCategory"></remove-category>
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
export default {
  data() {
    return {
      activeTab: 0,
      showBooks: false,
      CustomCreateCategory: false,
      CustomEditCategory: false,
      CustomRemoveCategory: false,
      WIPFlag: false,
      CountSamePage: 0 //Counts the amount of times
    };
  },
  methods: {
    enableCreateCategory() {
      if (this.CustomCreateCategory===true) {
        this.CustomCreateCategory = false;
      }
      this.CustomCreateCategory = true;
      this.CustomEditCategory = false;
      this.CustomRemoveCategory = false;
      this.WIPFlag = false;
    },
    enableEditCategory() {
      this.CustomEditCategory = true;
      this.CustomCreateCategory = false;
      this.CustomRemoveCategory = false;
      this.WIPFlag = false;
    },
    enableRemoveCategory() {
      this.CustomRemoveCategory = true;
      this.CustomCreateCategory = false;
      this.CustomEditCategory = false;
      this.WIPFlag = false;
    },
    enableWIP() {
      this.WIPFlag = true;
    }
  },
  components: {
    CreateCategory,
    EditCategory,
    RemoveCategory
  }
};
</script>
