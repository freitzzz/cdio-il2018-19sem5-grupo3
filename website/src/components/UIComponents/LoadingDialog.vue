<template>
    <section>
        <!-- v-model on active property is required due to syncing active with isActive -->
        <b-modal v-model="active" :active.sync="isActive" :width="200" :canCancel="false">
            <div class="modal-card" style="height:150px;width:auto">
                <b-field>
                    <b-loading :active.sync="isActive" :is-full-page="false" :canCancel="false"/>
                </b-field>
                <b-field :label.sync="message" class="has-text-centered"/>
            </div>
        </b-modal>
    </section>
</template>

<script>

export default {
    /**
     * Component call when component is created
     */
    created(){
        this.timeoutLoadingDialog();
    },
    /**
     * Component data
     */
    data(){
        return {
            /**
             * Indicates if the current dialog & loading components are active
             */
            isActive:false
        }
    },
    /**
     * Component methods
     */
    methods:{
        /**
         * Times the opening of the loading dialog
         */
        timeoutLoadingDialog(){
            if(this.$props.active){
                new Promise((accept)=>{
                    setTimeout(accept,this.$props.timeout);
                }).then(()=>{
                    this.isActive=this.$props.active;
                });
            }else{
                this.isActive=false;
            }
        }
    },
    /**
     * Component Name
     */
    name:"LoadingDialog",
    /**
     * Component props
     */
    props:{
        /**
         * Synced property which indicates if the loading dialog is active
         */
        active:Boolean,
        /**
         * Synced property which represents the loading dialog message
         */
        message:String,
        /**
         * Property which indicates if the loading dialog should timeout X time before turning active
         * (Value is in milliseconds, default = 0 => no timeout)
         */
        timeout:{
            type:Number,
            default:0
        }
    },
    /**
     * Component call when component is updated
     */
    updated(){
        this.timeoutLoadingDialog();
    }
}
</script>
