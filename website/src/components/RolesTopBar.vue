<template>
    <div>
        <nav class="navbar" role="navigation" aria-label="main navigation">
            <!-- Administrator & Content Manager & Logistic Manager Navigation Bar -->
            <div id="adminNavBar" class="navbar-menu">
                <!-- Administrator Actions -->
                <router-link v-if="isAdministrator" class="navbar-item" tag="li" to="/administration/orders">Orders</router-link>
                <router-link v-if="isAdministrator" class="navbar-item" tag="li" to="/administration/prices">Prices</router-link>
                <!-- Content Manager Actions -->
                <router-link v-if="isContentManager" class="navbar-item" tag="li" to="/management/categories">Categories</router-link>
                <router-link v-if="isContentManager" class="navbar-item" tag="li" to="/management/materials">Materials</router-link>
                <router-link v-if="isContentManager" class="navbar-item" tag="li" to="/management/products">Products</router-link>
                <router-link v-if="isContentManager" class="navbar-item" tag="li" to="/management/customization">Create Customized Product</router-link>
                <router-link v-if="isContentManager" class="navbar-item" tag="li" to="/management/collections">Customized Product Collections</router-link>
                <router-link v-if="isContentManager" class="navbar-item" tag="li" to="/management/catalogues">Commercial Catalogues</router-link>
            </div>
            <div class="navbar-end">
                <b-dropdown>
                    <a class="navbar-text" slot="trigger">Welcome, {{this.name}}</a>
                    <a class="navbar-icon" slot="trigger">
                        <i class="far fa-user-circle" style="font-size:30px"/>
                    </a>
                    <b-dropdown-item @click="logout">Logout</b-dropdown-item>
                </b-dropdown>
            </div>
        </nav>
        <router-view/>
    </div>
</template>

<script>

/**
 * Requires Global Store
 */
import Store from '../store/index';

/**
 * Requires Global Store mutations types
 */
import {LOGOUT_USER} from '../store/mutation-types';

export default {
    
    /**
     * Component call when component is created
     */
    created(){
        let userDetails=Store.getters.userDetails;
        this.name=userDetails.name;
        this.isAdministrator=userDetails.roles.isAdministrator;
        this.isContentManager=userDetails.roles.isContentManager;
        this.isLogisticManager=userDetails.roles.isLogisticManager;
    },
    /**
     * Component data
     */
    data:{
        /**
         * Boolean true if the user is an administrator
         */
        isAdministrator:Boolean,
        /**
         * Boolean true if the user is a content manager
         */
        isContentManager:Boolean,
        /**
         * Boolean true if the user is a logistic manager
         */
        isLogisticManager:Boolean,
        /**
         * String with the user name
         */
        name:String
    },
    /**
     * Component methods
     */
    methods:{
        /**
         * Logouts the current user
         */
        logout(){
            Store.commit(LOGOUT_USER);
            this.$router.replace({name:"home"});
        }
    },
    /**
     * Component name
     */
    name:"RolesTopBar"
}
</script>

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
