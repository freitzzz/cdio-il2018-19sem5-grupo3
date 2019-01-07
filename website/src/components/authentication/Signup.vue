<template>
    <section>
        <b-modal :active.sync="active">
            <div class="modal-card" style="width: auto">
                <signup-form @emitSignup="signup" />
            </div>
        </b-modal>
        <account-details
            v-if="successfulSignup.show"
            :custom-message="successfulSignup.customMessage"
            :custom-title="successfulSignup.customTitle"
            :details="successfulSignup.details"
            @onClose="emitCloseSignup"
        />
    </section>
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

    /**
     * Requires AccountDetails component
     */
    import AccountDetails from '../UIComponents/AccountDetails';

    export default {
    
        /**
         * Component imported components
         */
        components: {
            AccountDetails,
            SignupForm
        },
        /**
         * Component data
         */
        data(){
            return{
                successfulSignup:{
                    customMessage:"Thank you for signing up on MYC!\nPlease save the following details as they will be required in the future",
                    customTitle:"Successful Signup",
                    details:{
                        activationCode:String
                    },
                    show:false
                }
            }
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
                            let signupData=authenticationData.data;
                            this.successfulSignup.details.activationCode=signupData.activationCode;
                            this.successfulSignup.show=true;
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
                this.active ? this.active=false : this.$emit("closeSignup");
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