<template>
  <nav class="navbar" role="navigation" aria-label="main navigation">
  
    <div id="navBarManagement" class="navbar-menu">
      <div class="navbar-start">
        <a class="navbar-item" @click="enableListCategories">Category</a>
        <a class="navbar-item" @click="enableListMaterials"> Material</a>
        <a class="navbar-item" @click="enableListProducts">Product</a>
        <a class="navbar-item" @click="enableListCollections">Customized Product Collections</a>
        <a class="navbar-item" @click="enableCustomizedProduct">Create Customized Product</a>
      </div>
    </div>
  
    <div class="navbar-end">
        <a class="navbar-text">
          Welcome, Content Manager
        </a>
        <a class="navbar-icon">
          <i class="far fa-user-circle" style="font-size:30px"/>
        </a>
    </div>
  
    <div id="management">
    </div>
    <section v-if="CustomListCategories" style="width:100%">
      <list-categories/>
    </section>
    <section v-if="CustomListProducts" style="width:100%">
      <list-products />
    </section>
    <section v-if="CustomListMaterials" style="width:100%">
      <list-materials />
    </section>
    <section v-if="CustomListCollections" style="width:100%">
      <list-customized-product-collections/>
    </section>
    <customizer v-if="CustomCustomizedProduct"></customizer>
  </nav>
</template>


<style scoped>
  .tag {
    cursor: pointer;
  }

  .navbar-item:hover{
    color: #0ba2db !important;
    background-color: #0ba4db47;
    border-radius: 10px;
  }

  .navbar-text{
    color: #000;
    text-align: center !important;
  }

  .navbar-icon{
    color: #0ba2db !important;
    margin-left: 10px;
  }
</style>

<script>
  import ListCustomizedProductCollections from './management/customizedproductcollections/ListCustomizedProductCollections.vue';
  import ListMaterials from './management/material/ListMaterials.vue'
  import ListCategories from './management/category/ListCategories.vue'
  import ListProducts from './management/product/ListProducts.vue';
  import Customizer from './Customizer.vue';
  export default {
    name: 'ManagementTopBar',
    data() {
      return {
        activeTab: 0,
        showBooks: false,
        CustomListCategories: false,
        CustomListMaterials: false,
        CustomListProducts: false,
        CustomListCollections: false,
        CustomCustomizedProduct: false,
      };
    },
    methods: {
      enableCustomizedProduct(){
        this.CustomListMaterials = false;
        this.CustomListProducts = false;
        this.CustomListCollections = false;
        this.CustomListCategories = false;
        this.CustomCustomizedProduct = true;
      },
      enableListCategories() {
        this.CustomListMaterials = false;
        this.CustomListProducts = false;
        this.CustomListCollections = false;
        this.CustomCustomizedProduct = false;
        this.CustomListCategories = true;
      },
  
      enableListProducts() {
        this.CustomListCategories = false;
        this.CustomListMaterials = false;
        this.CustomListCollections = false;
        this.CustomCustomizedProduct = false;
        this.CustomListProducts = true;
      },
      enableListMaterials() {
        this.CustomListProducts = false;
        this.CustomListCategories = false;
        this.CustomListCollections = false;
        this.CustomCustomizedProduct = false;
        this.CustomListMaterials = true;
      },
      enableListCollections() {
        this.CustomListProducts = false;
        this.CustomListCategories = false;
        this.CustomListMaterials = false;
        this.CustomCustomizedProduct = false;
        this.CustomListCollections = true;
      },
    },
    components: {
      ListCategories,
      ListMaterials,
      ListProducts,
      ListCustomizedProductCollections,
      Customizer
    }
  };
</script>
