<template>
  <nav class="navbar" role="navigation" aria-label="main navigation">
  
    <div id="navBarManagement" class="navbar-menu">
      <div class="navbar-start">
  
        <a class="navbar-item " @click="enableListCategories">
                              Category
                            </a>
  
        <a class="navbar-item" @click="enableListMaterials">
                              Material
                            </a>
        <a class="navbar-item " @click="enableListProducts">
                              Product
                            </a>
        <a class="navbar-item">
                                Create Customized Product
                            </a>
        <a class = "navbar-item" @click="enableListCollections">
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
  
    <div id="management">
<!--       <b-message title="Information">
        Feature is not implemented yet.
      </b-message>
   -->
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
  </nav>
</template>


<style scoped>
  .tag {
    cursor: pointer;
  }
</style>

<script>
  import ListMaterials from './management/material/ListMaterials.vue'
  import ListCategories from './management/category/ListCategories.vue'
  import ListProducts from './management/product/ListProducts.vue';
  import ListCustomizedProductCollections from './management/customizedproductcollections/ListCustomizedProductCollections.vue';
  export default {
    data() {
      return {
        activeTab: 0,
        showBooks: false,
        CustomListCategories: false,
        CustomListMaterials: false,
        CustomListProducts: false,
        CustomListCollections: false
      };
    },
    methods: {
      enableListCategories() {
        this.CustomListMaterials = false;
        this.CustomListProducts = false;
        this.CustomListCollections = false;
        this.CustomListCategories = true;
      },
  
      enableListProducts() {
        this.CustomListCategories = false;
        this.CustomListMaterials = false;
        this.CustomListCollections = false;
        this.CustomListProducts = true;
      },
      enableListMaterials() {
        this.CustomListProducts = false;
        this.CustomListCategories = false;
        this.CustomListCollections = false;
        this.CustomListMaterials = true;
      },
      enableListCollections(){
        this.CustomListProducts = false;
        this.CustomListCategories = false;
        this.CustomListMaterials = false;
      }
    },
    components: {
      ListCategories,
      ListMaterials,
      ListProducts,
      ListCustomizedProductCollections
    }
  };
</script>
