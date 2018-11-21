 <template>
<nav class="navbar" role="navigation" aria-label="main navigation">
  <div class="navbar-brand">
    
    <a role="button" class="navbar-burger burger" aria-label="menu" aria-expanded="false" data-target="navbarBasicExample">
      <span aria-hidden="true"></span>
      <span aria-hidden="true"></span>
      <span aria-hidden="true"></span>
    </a>
  </div>

  <div id="navBarManagement" class="navbar-menu">
    <div class="navbar-start">
        <div class="navbar-item has-dropdown is-hoverable">
        <a class="navbar-link">
          Category
        </a>

        <div class="navbar-dropdown">
          <a class="navbar-item">
            Create Category
          </a>
          <a class="navbar-item">
            Edit Category
          </a>
          <a class="navbar-item">
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
        <a class="navbar-item">
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
</nav>

</template> 

<style scoped>
.tag {
  cursor: pointer;
}
</style>



<script>
export default {
  data() {
    return {
      activeTab: 0,
      showBooks: false
    };
  }
};
</script>
