<template>
    <b-modal :active.sync="active">
        <div class="modal-card" style="width: auto">
            <signup-form @emitSignup="signup" />
        </div>
    </b-modal>
</template>

<script>

    /**
     * Requires SignupForm component
     */
    import SignupForm from '../UIComponents/SignupForm';

    /**
     * Requires Axios for HTTP requests
     */
    import Axios from 'axios';
    
    /**
     * Requires MYCA_API_URL
     */
    import Config,{MYCA_API_URL} from '../../config';

    /**
     * Requires MYC APIs grants service
     */
    import APIGrantsService from '../../APIGrantsService.js';

    export default {
    
        /**
         * Component imported components
         */
        components: {
            SignupForm
        },
        /**
         * Component Props
         */
        props:{
            active:Boolean
        },
        /**
         * Component methods
         */
        methods: {
            /**
             * Signups into MYC API's
             */
            signup(details) {
                let authenticationRequestData = Object.assign({},details);
                authenticationRequestData.type="credentials";
                APIGrantsService
                    .grantAuthenticationAPIIsAvailable()
                    .then(()=>{
                        Axios.post(MYCA_API_URL+"/users", authenticationRequestData)
                        .then((authenticationData) => {
                            let apiToken = authenticationData.data.token;
                            this.$toast.open({
                                message: "Here's your API token\nDon't lose it!\n" + apiToken
                            });
                            this.active = false;
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
             * Emits close signup action
             */
            emitCloseSignup() {
                this.$emit("closeSignup");
            }
        },
        /**
         * Component watched values
         */
        watch:{
            /**
             * Watches the active value 
             */
            active(){
                if(!this.active)
                    this.emitCloseSignup();
            }
        }
    }
</script>