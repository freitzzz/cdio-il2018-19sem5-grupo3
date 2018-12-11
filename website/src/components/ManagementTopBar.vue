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

        <a class="navbar-item" @click="enableListMaterials">
          Material
        </a>

       
        <div class="navbar-start">
        <div class="navbar-item has-dropdown is-hoverable">
        <a class="navbar-link">
          Product
        </a>

        <div class="navbar-dropdown">
          <a class="navbar-item" @click="enableListProducts">
            List Products
          </a>
        </div>

      </div>
        <a class="navbar-item" >
            Create Customized Product
        </a>
      </div>

      <div class = "navbar-start">
        <a class = "navbar-item" @click="enableListCustomizedProductCollections">
          Customized Product Collections
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
 <!--  <b-message title="Information" v-if="WIPFlag">
    Feature is not implemented yet.
  </b-message> -->
  <create-category v-if="CustomCreateCategory"></create-category>
  <edit-category v-if="CustomEditCategory"></edit-category>
  <remove-category v-if="CustomRemoveCategory"></remove-category>

</div>



<section v-if="CustomListProducts" style="width:100%">
    <list-products />
  </section>
  <section v-if="CustomListMaterials" style="width:100%">
    <list-materials />
  </section>
  <section v-if="CustomListCustomizedProductsCollection" style="width:100%">
    <list-customized-product-collections/>
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
import ListMaterials from './management/material/ListMaterials.vue';
import ListProducts from './management/product/ListProducts.vue';
import ListCustomizedProductCollections from './management/customizedproductcollections/ListCustomizedProductCollections.vue';

export default {
  data() {
    return {
      activeTab: 0,
      showBooks: false,
      CustomCreateCategory: false,
      CustomEditCategory: false,
      CustomRemoveCategory: false,
      /*   CountSamePage: 0, //Counts the amount of times*/
      CustomListMaterials: false,
      CustomListProducts:false,
      CustomListCustomizedProductsCollection: false
    };
  },
  methods: {
    enableCreateCategory() {
      if (this.CustomCreateCategory === true) {
        this.CustomCreateCategory = false;
      }
      this.CustomCreateCategory = true;
      this.CustomEditCategory = false;
      this.CustomRemoveCategory = false;
      disableMaterial();
    },
    enableEditCategory() {
      this.CustomEditCategory = true;
      this.CustomCreateCategory = false;
      this.CustomRemoveCategory = false;
      disableMaterial();

    },
    enableRemoveCategory() {
      this.CustomRemoveCategory = true;
      this.CustomCreateCategory = false;
      this.CustomEditCategory = false;
      disableMaterial();

    },
    enableListMaterials(){
      this.CustomRemoveCategory = false;
      this.CustomCreateCategory = false;
      this.CustomEditCategory = false;
      this.CustomListMaterials=true;
    },
    enableListCustomizedProductCollections(){
      this.CustomRemoveCategory = false;
      this.CustomCreateCategory = false;
      this.CustomEditCategory = false;
      this.CustomListMaterials = false;
      this.CustomListProducts = false;
      this.CustomListCustomizedProductsCollection = true;
    },
    enableListProducts(){
      this.CustomListMaterials=false;
      this.CustomListProducts=true;
    },
    disableCategory(){
      /* Category: */
      this.CustomEditCategory = false;
      this.CustomCreateCategory = false;
      this.CustomRemoveCategory = false;
    },
  },
  components: {
    CreateCategory,
    EditCategory,
    RemoveCategory,
    ListMaterials,
    ListProducts,
    ListCustomizedProductCollections
  }
};
</script>
