<template>
  <div>
    <nav class="navbar" role="navigation" aria-label="main navigation">
      <div id="navBarManagement" class="navbar-menu">
        <div class="navbar-start">
          <router-link class="navbar-item" tag="li" to="/management/categories">Categories</router-link>
          <router-link class="navbar-item" tag="li" to="/management/materials">Materials</router-link>
          <router-link class="navbar-item" tag="li" to="/management/products">Products</router-link>
          <router-link class="navbar-item" tag="li" to="/management/prices">Price Materials</router-link>
          <router-link
            class="navbar-item"
            tag="li"
            to="/management/customization"
          >Create Customized Product</router-link>
          <router-link
            class="navbar-item"
            tag="li"
            to="/management/collections"
          >Customized Product Collections</router-link>
          <router-link
            class="navbar-item"
            tag="li"
            to="/management/catalogues"
          >Commercial Catalogues</router-link>
        </div>
      </div>

      <div class="navbar-end">
        <a class="navbar-text">Welcome, Content Manager</a>
        <a class="navbar-icon">
          <i class="far fa-user-circle" style="font-size:30px"/>
        </a>
      </div>

      <div id="management"></div>
    </nav>

    <router-view/>
  </div>
</template>


<style scoped>
.tag {
  cursor: pointer;
}

.navbar-item:hover {
  color: #0ba2db !important;
  background-color: #0ba4db47;
  border-radius: 10px;
  cursor: pointer;
}

.navbar-text {
  color: #000;
  text-align: center !important;
}

.navbar-icon {
  color: #0ba2db !important;
  margin-left: 10px;
}
</style>

<script>
export default {
  name: "ManagementTopBar"
};
</script>
