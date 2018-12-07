<template>
    <div class="modal-card" style="width: auto">
        <header class="modal-card-head">
            <p class="modal-card-title">Signup</p>
        </header>
        <section class="model-card-body">
            <b-field label="Username">
                <b-input
                    type="String"
                    v-model="username"
                    :placeholder="placeholder.username"
                    icon="account"
                    required>
                </b-input>
            </b-field>
            <b-field label="Password">
                <b-input
                    type="password"
                    v-model="password"
                    :placeholder="placeholder.password"
                    icon="key"
                    password-reveal
                    required>
                </b-input>
            </b-field>
        </section>
        <footer class="modal-card-foot">
            <div class="has-text-centered">
                <button class="button is-primary" @click="emitSignup()">Signup</button>
            </div>
        </footer>
    </div>
</template>

<script>
export default {
    /**
     * Component Data
     */
    data(){
        return {
            placeholder:{
                username:"superusername",
                password:"superpassword"
            },
            username:"",
            password:""
        }
    },
    /**
     * Component methods
     */
    methods:{
        /**
         * Emits the signup action
         */
        emitSignup(){
            let signupDetails={
                username:this.username,
                password:this.password
            };
            this.$emit("emitSignup",signupDetails);
        }
    }
}
</script>