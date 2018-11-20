<template>
    <div>
        <!-- CUD BUTTONS -->
        <div>
            <button class="button is-danger" @click="createNewProduct()">
                <b-icon icon="plus"/>
            </button>
        </div>
        
        <PaginatedTable 
        :total.sync="total" 
        :columns.sync="columns"
        :data.sync="data"
        :title.sync="title"
        />
    </div>
</template>

<script>

import PaginatedTable from './UIComponents/PaginatedTable.vue'
import Axios from 'axios';

export default {
    components:{
        PaginatedTable
    },
    /**
     * Function that is called when the component is created
     */
    created(){
        this.updateFetchedProducts();
    },
    data(){
        return{
            availableProducts:Array,
            columns:[],
            data:Array,
            total:Number
        }
    },
    methods:{
        /**
         * Fetches all available products
         */
        updateFetchedProducts(){
            console.log("->>>>>>>>>>> "+this.reference);
            Axios.get('http://localhost:5000/mycm/api/products')
            .then((_response)=>{
                this.data=this.generateProductsTableData(_response.data);
                this.columns=this.generateProductsTableColumns();
                this.total=this.data.length;
            })
            .then((_error)=>{
                console.log(_error);
            });
        },
        /**
         * Generates the needed columns for the products table
         */
        generateProductsTableColumns(){
            return [
                {
                    field:"id",
                    label:"ID",
                    centered:true   
                },
                {
                    field:"reference",
                    label:"Reference",
                    centered:true   
                },
                {
                    field:"designation",
                    label:"Designation",
                    centered:true   
                },
                {
                    field:"edit",
                    label:"Edit",
                    centered:true
                }
            ];
        },
        generateProductsTableData(products){
            let productsTableData=[];
            products.forEach((product)=>{
                productsTableData.push({
                    id:product.id,
                    reference:product.reference,
                    designation:product.designation
                });
            });
            return productsTableData;
        }
    }
}
</script>
