<template>
    <div>
        <login-form  v-if="!activateManager"  @emitLogin="login" @signUp="activateSignupComponent" />
        <management-top-bar v-if="activateManager"  :active.sync="activateManager"/>
        <signup
            :active="activateSignup"
            @closeSignup="closeSignupComponent"
        />
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

    /*    Vue.use(Router); */
    
    export default {
        /**
         * Component data
         */
        data(){
            return{
                activateManager:false,
                activateSignup:false
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
            Signup
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
                document.cookie="myca=cookie!";
                APIGrantsService
                    .grantAuthenticationAPIIsAvailable()
                    .then(()=>{
                        Axios.post(MYCA_API_URL+"/auth", authenticationRequestData, {
                            headers: authenticationRequestHeaders,
                            maxRedirects:0,
                            withCredentials:true
                        })
                        .then((authenticationData) => {
                            let sessionCookie = authenticationData.headers.cookie;
                            this.$toast.open({
                                message: "Succesful Login!\nWe have stored a session cookie in your browser :)"
                            });
                            getUserAuthorizations()
                                .then((userAuthorizations)=>{this.emitCloseLogin(userAuthorizations)})
                                .catch((userAuthorizations)=>{this.emitCloseLogin(userAuthorizations)});
                        })
                        .catch((_error_message) => {
                            let message = _error_message.response.data.message;
                            this.$toast.open({
                                message: message
                            });
                        });
                    })
                    .catch(()=>{
                        this.$toast.open({message:'Our autentication service is currently down! Please hold on :('});
                    });
            },
            /**
             * Emits close login action
             */
            emitCloseLogin(userAuthorizations) {
                this.$emit("closeLogin",userAuthorizations);
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
            }
        }
    }
</script>
