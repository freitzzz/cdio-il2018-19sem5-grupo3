<template>
    <div class="modal-card" style="width: auto">
        <header class="modal-card-head">
            <p class="modal-card-title">Price History</p>
        </header>
        <section class="modal-card-body">
        </section>
    </div>
</template>
<script>

export default {
    
    /**
     * Received properties from father component
     */
    props:{
        /**
         * Current Material details
         */
        material:{
            type:Object,
            required:true
        }
    },
    /**
     * Component Created State call
     */
    created(){
    },
    /**
     * Component data
     */
    data(){
        return{
        }
       
    },
}
</script>
