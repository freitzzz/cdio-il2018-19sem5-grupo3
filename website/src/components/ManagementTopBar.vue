<template>
  <nav class="navbar" role="navigation" aria-label="main navigation">
  
    <div id="navBarManagement" class="navbar-menu">
      <div class="navbar-start">

          <a class="navbar-item " @click="enableListCategories">
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
            <a class="navbar-item" @click="enableRemoveMaterial">
              Remove Material
            </a>
  
          </div>
        </div>
  
  
        <div class="navbar-start">
            <a class="navbar-item " @click="enableListProducts">
            Product
          </a>
          <a class="navbar-item" @click="enableWIP">
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


      <create-material v-if="CustomCreateMaterial"></create-material>
      <edit-material v-if="CustomEditMaterial"></edit-material>
      <remove-material v-if="CustomRemoveMaterial"></remove-material>
    </div>
  
  

    <section v-if="CustomListCategories" style="width:100%">
      <list-categories/>
    </section>
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
  import ListCategories from "./management/category/ListCategories.vue;"
  import ListProducts from './management/product/ListProducts.vue';
  export default {
    data() {
      return {
        activeTab: 0,
        showBooks: false,
        CustomListCategories: false,
        WIPFlag: false,
        /*   CountSamePage: 0, //Counts the amount of times*/
        CustomCreateMaterial: false,
        CustomEditMaterial: false,
        CustomRemoveMaterial: false,
  
        CustomListProducts: false
      };
    },
    methods: {
      enableListCategory() {
        this.CustomListProducts = false;
        this.CustomListCategories = true;
  
      },

      enableListProducts() {
        this.CustomCreateMaterial = false;
        this.CustomEditMaterial = false;
        this.CustomRemoveMaterial = false;
        this.CustomListProducts = true;
      },
      enableWIP() {
        this.WIPFlag = true;
      }
    },
    components: {
      ListCategories,
      ListProducts
    }
  };
</script>
