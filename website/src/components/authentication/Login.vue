<template>
    <div>
        <login-form  v-if="!activateManager"  @emitLogin="login" @signUp="signUp" />
        <management-top-bar v-if="activateManager"  :active.sync="activateManager"></management-top-bar>
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
    /*    Vue.use(Router); */
    
    export default {
        data(){
           activateManager:false;
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
            ManagementTopBar
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
                Axios.post("http://localhost:2000/myca/api/auth", authenticationRequestData, {
                        headers: authenticationRequestHeaders,
                    })
                    .then((authenticationData) => {
                        console.log(authenticationData.status);
                        let sessionCookie = authenticationData.headers.cookie;
                        this.$cookies.set("asdasd", sessionCookie);
                        this.$toast.open({
                            message: "Succesful Login!\nWe have stored a session cookie in your browser :)"
                        });
                        this.isManagerCredentials(sessionCookie);
                        emitCloseLogin();
                    })
                    .catch((_error_message) => {
                        let message = _error_message.response.data.message;
                        this.$toast.open({
                            message: message
                        });
                    });
            },
            isManagerCredentials(token) {
                return new Promise((accept, reject) => {
                    Axios.get("http://localhost:2000/myca/api/autho?contentmanager=true")
                        .then((tokenManager) => {
                            if (tokenManager == token) {
                                this.activateManager = true;
                                accept();
                            }
                        })
                        .catch((error_message) => {
    
                            this.$toast.open({
                                message: error_message
                            });
                            reject();
                        });
                });
            },
            /**
             * Emits close login action
             */
            emitCloseLogin() {
                this.$emit("closeLogin");
            },
            signUp() {
                this.$emit("signUp");
            }
        }
    }
</script>
