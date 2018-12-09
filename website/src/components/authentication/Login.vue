<template>
    <b-modal :active="active">
        <login-form @emitLogin="login"/>
    </b-modal>
</template>

<script>

/**
 * Requires LoginForm component
 */
import LoginForm from '../UIComponents/LoginForm';
import Axios from 'axios';

export default {
    /**
     * Component imported components
     */
    components:{
        LoginForm
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
         * Logins into MYC API's
         */
        login(details){
            let authenticationRequestData={
                type:"credentials",
                username:details.username,
                password:details.password,
            };
            let authenticationRequestHeaders={
                Secrete:"Secrete"
            };
            Axios.post("http://localhost:2000/myca/api/auth",authenticationRequestData,{
                headers:authenticationRequestHeaders,
            })
            .then((authenticationData)=>{
                console.log(authenticationData.status);
                let sessionCookie=authenticationData.headers.cookie;
                this.$cookies.set("asdasd",sessionCookie);
                this.$toast.open({message:"Succesful Login!\nWe have stored a session cookie in your browser :)"});
                this.active=false;
                emitCloseLogin();
            })
            .catch((_error_message)=>{
                let message=_error_message.response.data.message;
                this.$toast.open({message:message});
            });
        },
        /**
         * Emits close login action
         */
        emitCloseLogin(){
            this.$emit("closeLogin");
        }
    }
}
</script>
