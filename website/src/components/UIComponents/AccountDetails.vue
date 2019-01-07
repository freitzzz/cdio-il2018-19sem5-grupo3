<template>
    <b-modal
        :active.sync="active"
        :onCancel="close"
    >
        <header class="modal-card-head">
            <p class="modal-card-title">
                {{verifyValidProperty(customTitle) ? customTitle : "Account Details"}}
            </p>
        </header>
        <div class="modal-card" style="width: auto">
            <section class="modal-card-body">
                <b-field v-if="verifyValidProperty(customMessage)">
                    {{customMessage}}
                </b-field>
                <!-- User API Token -->
                <a v-if="details.apiToken" @click="clipboard(details.apiToken)">
                    <b-field label="API Token">
                        <b-input
                            :id="details.apiToken"
                            v-model="details.apiToken"
                            icon="key-variant"
                            disabled
                        />
                    </b-field>
                </a>
                <!-- User Activation Code -->
                <b-field v-if="verifyValidProperty(details.activationCode)"
                    label="Activation Code">
                    <b-input
                        v-model="details.activationCode"
                        icon="cellphone-key"
                        disabled
                    />
                </b-field>
                <!-- User Email -->
                <b-field v-if="verifyValidProperty(details.email)"
                    label="Email">
                    <b-input
                        v-model="details.email"
                        icon="email"
                        disabled
                    />
                </b-field>
                <!-- User Name -->
                <b-field v-if="verifyValidProperty(details.name)"
                    label="Name">
                    <b-input
                        v-model="details.name"
                        icon="account"
                        disabled
                    />
                </b-field>
            </section>
        </div>
    </b-modal>
</template>

<script>
export default {

    /**
     * Component data
     */
    data(){
        return{
            /**
             * Boolean to keep track of the component modal activeness
             */
            active:true
        }
    },
    /**
     * Component methods
     */
    methods:{
        /**
         * Copies a text into clipboard
         */
        clipboard(text){
            this.$copyText(text);
        },
        /**
         * Emits an event warning the component close
         */
        close(){
            this.$emit('onClose');
        },
        /**
         * Verifies if a propert is valid
         */
        verifyValidProperty(property){
            return property && property.length!=0;
        }
    },
    /**
     * Component name
     */
    name:"AccountDetails",
    /**
     * Component properties
     */
    props:{
        /**
         * String with the custom message to be displayed above the account details
         */
        customMessage:String,
        /**
         * String with the custom title to be displayed as the modal title
         */
        customTitle:String,
        /**
         * Account details to be displayed
         */
        details:{
            /**
             * String with the user activation code
             */
            activationCode:String,
            /**
             * String with the user api token
             */
            apiToken:String,
            /**
             * String with the user email
             */
            email:String,
            /**
             * String with the user name
             */
            name:String
        }
    }
}
</script>
