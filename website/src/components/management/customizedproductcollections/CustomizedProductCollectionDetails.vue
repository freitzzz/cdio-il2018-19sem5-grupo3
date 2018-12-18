<template>
    <div class = "modal-card" style="width: auto">
        <header class = "modal-card-head">
            <p class = "modal-card-title">Collection Details</p>
        </header>
        <section class = "modal-card-body">
            <b-field label = "Name">
                <b-input
                    type="String"
                    :value="collection.name"
                    disabled="true"
                    icon= "pound"
                    required>
                </b-input>
            </b-field>
            <b-checkbox type="is-info" v-if="collection.customizedProducts!=null" @input="enableCustomizedProducts()">
                Customized Products
            </b-checkbox>
            <div v-if="showCustomizedProducts">
                <b-field label = "Customized Products">
                    <simple-table
                        :columns="simpleTableColumns.customizedProducts"
                        :data="collection.customizedProducts"
                    />
                </b-field>
            </div>
        </section>
    </div>
</template>

<script>

import SimpleTable from './../../UIComponents/SimpleTable';

export default {
    components:{
        SimpleTable
    },
    props:{
        collection:{
            type:Object,
            required:true
        }
    },
    data(){
        return{
            simpleTableColumns:{
                customizedProducts:[
                    {
                        name:"id",
                        title:"ID"
                    },
                    {
                        name:"productId",
                        title:"Product ID"
                    },
                    {
                        name:"serialNumber",
                        title:"Serial Number"
                    }
                ]
            },
            showCustomizedProducts:false
        }
    },
    methods:{
        enableCustomizedProducts(){
            this.showCustomizedProducts=!this.showCustomizedProducts;
        }
    }
}

</script>
