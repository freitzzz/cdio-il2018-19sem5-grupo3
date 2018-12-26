<template>
    <section>
        <b-modal :active.sync="active" :width="200">
            <div class="modal-card" style="height:150px;width:auto">
                <b-field>
                    <b-loading :active.sync="active" :is-full-page="false" :canCancel="true"/>
                </b-field>
                <b-field :label="message" class="has-text-centered"/>
            </div>
        </b-modal>
    </section>
</template>

<script>

export default {
    /**
     * Component data
     */
    data(){
        return {
            active:true,
            isLoading:true,
            isFullPage:true
        }
    },
    name:"LoadingDialog",
    /**
     * Component props
     */
    props:{
        message:String
    }
}
</script>
