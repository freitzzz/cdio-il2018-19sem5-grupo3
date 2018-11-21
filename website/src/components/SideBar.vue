 <template>

 <nav class="navbar" role="navigation" aria-label="main navigation">
  <div class="navbar-brand">
    <a class="navbar-item" >
      <img src="https://www.flaticon.com/free-icon/closet_171728#" width="112" height="28">
    </a>

    <a role="button" class="navbar-burger burger" aria-label="menu" aria-expanded="false" data-target="navbarBasicExample">
      <span aria-hidden="true"></span>
      <span aria-hidden="true"></span>
      <span aria-hidden="true"></span>
    </a>
  </div>

  <div id="navbarBasicExample" class="navbar-menu">
    <div class="navbar-start">
      <a class="navbar-item">
        Home
      </a>

      <a class="navbar-item">
        Documentation
      </a>

      <div class="navbar-item has-dropdown is-hoverable">
        <a class="navbar-link">
          More
        </a>

        <div class="navbar-dropdown">
          <a class="navbar-item">
            About
          </a>
          <a class="navbar-item">
            Jobs
          </a>
          <a class="navbar-item">
            Contact
          </a>
          <hr class="navbar-divider">
          <a class="navbar-item">
            Report an issue
          </a>
        </div>
      </div>
    </div>

    <div class="navbar-end">
      <div class="navbar-item">
        <div class="buttons">
          <a class="button is-primary">
            <strong>Sign up</strong>
          </a>
          <a class="button is-light">
            Log in
          </a>
        </div>
      </div>
    </div>
  </div>
</nav>

   <!--  <section>
        <b-dropdown hoverable>
            <button class="button is-info" slot="trigger">
                <span>Category</span>
                <b-icon icon="menu-down"></b-icon>
            </button>

            <b-dropdown-item>Add Category</b-dropdown-item>
            <b-dropdown-item>Edit Category</b-dropdown-item>
            <b-dropdown-item>Remove Category</b-dropdown-item>
        </b-dropdown>
        <b-dropdown hoverable>
            <button class="button is-info" slot="trigger">
                <span>Material</span>
                <b-icon icon="menu-down"></b-icon>
            </button>

            <b-dropdown-item>Add Material</b-dropdown-item>
            <b-dropdown-item>Edit Material</b-dropdown-item>
            <b-dropdown-item>Remove Material</b-dropdown-item>
        </b-dropdown>
        <b-dropdown hoverable>
            <button class="button is-info" slot="trigger">
                <span>Product</span>
                <b-icon icon="menu-down"></b-icon>
            </button>

            <b-dropdown-item>Add Product</b-dropdown-item>
            <b-dropdown-item>Edit Product</b-dropdown-item>
            <b-dropdown-item>Remove Product</b-dropdown-item>
        </b-dropdown>

    
    </section> -->
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
