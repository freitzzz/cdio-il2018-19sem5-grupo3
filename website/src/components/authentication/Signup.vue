<template>
    <b-modal :active="active">
        <signup-form @emitSignup="signup"/>
    </b-modal>
</template>

<script>

/**
 * Requires SignupForm component
 */
import SignupForm from '../UIComponents/SignupForm';
import Axios from 'axios';

export default {
    /**
     * Component imported components
     */
    components:{
        SignupForm
    },
    /**
     * Component data
     */
    data(){
        return{
            active:true
        }
    },
    /**
     * Component methods
     */
    methods:{
        /**
         * Signups into MYC API's
         */
        signup(details){
            let authenticationRequestData={
                type:"credentials",
                username:details.username,
                password:details.password,
            };
            let authenticationRequestHeaders={
                Secrete:"Secrete"
            };
            Axios.post("http://localhost:2000/myca/api/users",authenticationRequestData,{
                headers:authenticationRequestHeaders
            })
            .then((authenticationData)=>{
                let apiToken=authenticationData.data.token;
                this.$toast.open({message:"Here's your API token\nDon't lose it!\n"+apiToken});
                this.active=false;
                emitCloseSignup();
            })
            .catch((_error_message)=>{
                let message=_error_message.response.data.message;
                this.$toast.open({message:message});
            });
        },
        /**
         * Emits close signup action
         */
        emitCloseSignup(){
            this.$emit("closeSignup");
        }
    }
}
</script>