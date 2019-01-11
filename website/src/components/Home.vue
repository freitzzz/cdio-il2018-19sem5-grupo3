<template>
  <div>
    <div v-if="showLogInModal">
      <b-modal :active.sync="showLogInModal">
      <Login @closeLogin="closeLogin" @signUp="signUp()"/>
      </b-modal>
    </div>
    <loading-dialog
      :active="loading.active"
      :message="loading.message"
    />
    <component v-if="!userAuthorizations.client" 
      :is="currentComp" @switch-to-sign-in-form="logIn()"
      @switch-to-sign-up-form="signUp()">
    </component>
  </div>
</template>

<script>
import Intro from "./Intro.vue";
import Login from "./authentication/Login.vue";
import AccountDetails from './UIComponents/AccountDetails.vue';

/**
 * Requires Global Store
 */
import Store from '../store/index';

/**
 * Requires Global Store mutations types
 */
import {SET_USER_NAME,SET_USER_ROLES} from '../store/mutation-types';

/**
 * Requires ManagementTopBar
 */
import ManagementTopBar from './ManagementTopBar.vue';

/**
 * Requires user services
 */
import {getUserDetails} from '../UsersService'; 

/**
 * Requires RolesTopBar for the diverse roles actions
 */
import RolesTopBar from './RolesTopBar.vue';

/**
 * Requires LoadingDialog for keeping the user with the current actions
 */
import LoadingDialog from './UIComponents/LoadingDialog';

export default {
  created(){
    getUserDetails()
    .then((userDetails)=>{
      this.loading.message="Loading website";
      this.loading.active=true;
      this.initNavigationBar(userDetails);
      this.loading.active=false;
    })
    .catch(()=>{
      this.currentComp=Intro;
    })
  },
  name: "home",
  data() {
    return {
      currentComp: null,
      loading:{
        active:false,
        message:""
      },
      showLogInModal: false,
      showSignUpModal: false,
      userAuthorizations:{
        administrator:false,
        client:false,
        contentManager:false,
        logisticManager:false
      }
    };
  },
  methods: {
    logIn() {
      this.showLogInModal = true;
    },
    /**
     * Event that is triggered when login component is closed
     */
    closeLogin(userDetails){
      this.showLogInModal = false;
      this.initNavigationBar(userDetails);
    },
    signUp(){
      this.showLogInModal = false;
      this.showSignUpModal = true;
    },
    /**
     * Updates the current user authorizations based on given user authorization details
     */
    updateUserAuthorizations(authorizationDetails){
        this.userAuthorizations.administrator=authorizationDetails.administrator;
        this.userAuthorizations.client=authorizationDetails.client;
        this.userAuthorizations.contentManager=authorizationDetails.contentManager;
        this.userAuthorizations.logisticManager=authorizationDetails.logisticManager;
    },
    /**
     * Inits MYCS navigation bar
     */
    initNavigationBar(userDetails){
      Store.commit(SET_USER_NAME,userDetails.name);
      Store.commit(SET_USER_ROLES,userDetails.roles);
      if(userDetails.roles.isAdministrator || userDetails.roles.isContentManager || userDetails.roles.isLogisticManager){
        this.currentComp=RolesTopBar;
        this.$router.replace({name:"management"});
      }else if(userDetails.roles.isClient){
        //CURRENT_COMP=USER_TOP_BAR
      }else{
        this.currentComp=Intro;
      }
    }
  },
  /**
   * Component used components
   */
  components: {
    AccountDetails,
    Intro,
    LoadingDialog,
    Login,
    ManagementTopBar,
    RolesTopBar
  }
};
</script>

