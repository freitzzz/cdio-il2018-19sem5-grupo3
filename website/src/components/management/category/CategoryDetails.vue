<template>
    <div class="modal-card" style="width: auto">
        <header class="modal-card-head">
            <p class="modal-card-title">Product Details</p>
        </header>
         <section class="modal-card-body">
                <b-field label="Name">
                <b-input
                    type="String"
                    :value="product.name"
                    disabled="true"
                    icon="pound"
                    required>
                </b-input>
            </b-field>
         </section>
    </div>
</template>
<script>
/**
 * Requires SimpleTable component
 */
import SimpleTable from './../../UIComponents/SimpleTable';

export default {
    components:{
        SimpleTable
    },
    data(){
        return{
            simpleTablesColumns:{
                components:[
                    {
                        name: "id",
                        title: "ID"
                    },
                    {
                        name: "name",
                        title: "Name"
                    },
                    {
                        name: "parentcategory",
                        title: "Parent Category"
                    }
                ]
            }
        }
    },
     props:{

        /**
         * Current Product details
         */
        category:{
            type:Object,
            required:true
        }
    },
    methods:{
        booleansAsIcons(value){
            return value 
                ? '<span class="ui teal label"><i class="material-icons">check</i></span>'
                : '<span class="ui teal label"><i class="material-icons">close</i></span>'
            ;
        },
    }
}
</script>
