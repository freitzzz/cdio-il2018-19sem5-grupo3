<template>
    <div>
        <login-form  v-if="!activateManager"  @emitLogin="login" @signUp="activateSignupComponent" />
        <management-top-bar v-if="activateManager"  :active.sync="activateManager"/>
        <signup
            :active="activateSignup"
            @closeSignup="closeSignupComponent"
        />
        <b-modal :active.sync="activateAccount">
            <activate-account
                :userInfo="userInfo"
                @closeActivationModal="closeActivateAccountComponent"
            >
            </activate-account>
        </b-modal>
    </div>
</template>

<script>
    /**
     * Requires LoginForm component
     */
    import LoginForm from '../UIComponents/LoginForm';
    import Axios from 'axios';
    import Vue from 'vue';
    import ManagementTopBar from '../ManagementTopBar.vue';
    import ActivateAccount from './ActivateAccount.vue';

    /**
     * Requires Signup component
     */
    import Signup from './Signup';
    
    /**
     * Requires MYCA API URL
     */
    import { MYCA_API_URL } from '../../config';
    
    /**
     * Requires APIGrantsService for granting that MYCA API is available
     */
    import APIGrantsService from '../../APIGrantsService';

    /**
     * Requires authorization services
     */
    import {getUserAuthorizations} from '../../AuthorizationService';

    /**
     * Requires users services
     */
    import {getUserDetails} from '../../UsersService';

    /**
     * Requires authentication services
     */
    import {authenticateUser} from '../../AuthenticationService';

    /*    Vue.use(Router); */
    
    export default {
        /**
         * Component data
         */
        data(){
            return{
                activateManager:false,
                activateSignup:false,
                activateAccount:false,
                userDetails:{
                    name:String,
                    roles:{
                        isAdministrator:Boolean,
                        isContentManager:Boolean,
                        isLogisticManager:Boolean
                    }
                },
                userInfo:null
            }
        },
        /* routes: {
            
                path: '/managementtopbar/:name',
                name: 'ManagementTopBar',
                component: ManagementTopBar
            
            }, */
        /**
         * Component imported components
         */
        components: {
            LoginForm,
            ManagementTopBar,
            Signup,
            ActivateAccount
        },
        /**
         * Component methods
         */
        methods: {
            /**
             * Logins into MYC API's
             */
            login(details) {
                let authenticationRequestData = {
                    type: "credentials",
                    email: details.email,
                    password: details.password,
                };
                let authenticationRequestHeaders = {
                    Secrete: "Secrete"
                };
                APIGrantsService
                    .grantAuthenticationAPIIsAvailable()
                    .then(()=>{
                        authenticateUser(authenticationRequestData)
                        .then((authenticationData)=>{
                            this.$toast.open({
                                message: "Succesful Login!\nWe have stored a session cookie in your browser :)"
                            });
                            getUserDetails()
                            .then((userDetails)=>{
                                this.userDetails=Object.assign({},userDetails);
                                this.emitCloseLogin(this.userDetails);
                            })
                            .catch((error_message)=>{
                                this.$toast.open({message:error_message});
                            });
                        })
                        .catch((error_message)=>{
                            let message = error_message.message;
                            let accountActivationRequired = error_message.requiresActivation;
                            if(accountActivationRequired){
                                this.userInfo = {...authenticationRequestData};
                                this.activateAccount = true;
                            }else{
                                this.$toast.open({
                                    message: message
                                });
                            }
                        })
                    })
                    .catch(()=>{
                        this.$toast.open({message:'Our autentication service is currently down! Please hold on :('});
                    });
            },
            /**
             * Emits close login action
             */
            emitCloseLogin(userDetails) {
                this.$emit("closeLogin",userDetails);
            },
            /**
             * Activates signup component
             */
            activateSignupComponent() {
                this.activateSignup=true;
            },
            /**
             * Closes signup action
             */
            closeSignupComponent(){
                this.activateSignup=false;
            },
            /**
             * Closes activate account component
             */
            closeActivateAccountComponent(){
                this.activateAccount=false;
                this.login(this.userInfo);
            }
        }
    }
</script>
