<template>
  <div>
    <div v-if="showLogInModal">
      <b-modal :active.sync="showLogInModal">
      <Login @closeLogin="closeLogin" @signUp="signUp()"/>
      </b-modal>
    </div>
    <management-top-bar
      v-if="userAuthorizations.contentManager"
    />
    <component v-if="!userAuthorizations.client" :is="currentComp" @switch-to-sign-in-form="logIn()"
    @switch-to-sign-up-form="signUp()"></component>
  </div>
</template>

<script>
import Intro from "./Intro.vue";
import Login from "./authentication/Login.vue";
import AccountDetails from './UIComponents/AccountDetails.vue';

/**
 * Requires ManagementTopBar
 */
import ManagementTopBar from './ManagementTopBar.vue';

/**
 * Requires authorization services
 */
import {getUserAuthorizations} from '../AuthorizationService'; 

export default {
  created(){
    getUserAuthorizations()
      .then((authorizationDetails)=>{
        this.updateUserAuthorizations(authorizationDetails);
      }).catch((authorizationDetails)=>{
        this.updateUserAuthorizations(authorizationDetails);
      });
  },
  name: "home",
  data() {
    return {
      currentComp: Intro,
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
    closeLogIn(authorizationDetails){
      this.showLogInModal = false;
      this.updateUserAuthorizations(authorizationDetails);
      if(this.userAuthorizations.contentManager)
        this.currentComp=ManagementTopBar;
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
    }
  },
  /**
   * Component used components
   */
  components: {
    AccountDetails,
    Intro,
    Login,
    ManagementTopBar
  }
};
</script>

