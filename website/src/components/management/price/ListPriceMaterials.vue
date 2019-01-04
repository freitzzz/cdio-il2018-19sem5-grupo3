<template>
    <div>
        <!-- CUD BUTTONS -->
        <b-field grouped>
            <div>
                <b-field>
                <button class="btn-primary" @click="createMaterial()">
                <b-icon icon="plus"/>
                </button>
            <div v-if="createMaterialModal">
                <b-modal :active.sync="createMaterialModal" has-modal-card scroll="keep">
                    <create-price-material 
                        :active="createMaterialModal" 
                        @emitMaterial="postMaterialPriceTableEntry"
                    />
                </b-modal>
            </div> 
            <button class="btn-primary" @click="fetchRequests()">
                <b-icon 
                    icon="refresh"/>
            </button>
            </b-field>
            </div>
            <b-field>
                <b-field>
                    <b-field label="Currency"> 
                        <b-select icon="coin" placeholder="Currency" v-model="selectedCurrency" @input="convertValuesToCurrency">
                            <option v-for="currency in this.currencies" 
                            :key="currency.currency" 
                            :value="currency">
                            {{currency.currency}}</option>
                        </b-select>
                    </b-field>
                    <b-field label="Area"> 
                        <b-select icon="move-resize-variant" placeholder="Area" v-model="selectedArea" @input="convertValuesToArea">
                            <option  v-for="area in this.areas" 
                            :key="area.area" 
                            :value="area">
                            {{area.area}}</option>
                        </b-select>
                    </b-field>
                </b-field>
            </b-field>
        </b-field>
        
        <price-materials-table
            :data="data"
        />
    </div>
</template>

<script>
import CreatePriceMaterial from './CreatePriceMaterial.vue';
import PriceMaterialsTable from './PriceMaterialsTable.vue';
import Axios from 'axios';
import Config,{ MYCM_API_URL } from '../../../config.js';
import PriceTableRequests from './../../../services/mycm_api/requests/pricetables.js';
import MaterialRequests from './../../../services/mycm_api/requests/materials.js';
import CurrenciesPerAreaRequests from './../../../services/mycm_api/requests/currenciesperarea.js';

let colors=[];
let finishes=[];

export default {
    components:{
        PriceMaterialsTable,
        CreatePriceMaterial,
    },
    /**
     * Function that is called when the component is created
     */
    created(){
        this.fetchRequests();
        CurrenciesPerAreaRequests.getCurrencies()
            .then((response)=>{
                this.currencies = response.data;
            })
            .catch((error)=>{
                //throw error?
            });
        CurrenciesPerAreaRequests.getAreas()
            .then((response)=>{
                this.areas = response.data;
            })
            .catch((error)=>{
                //throw error?
            });
    },
    data(){
        return{
            createMaterialModal:false,
            updateMaterialModal:false,
            material:null,
            materialClone:null,
            currentSelectedMaterial:0,
            availableMaterials:Array,
            selectedCurrency:null,
            selectedArea:null,
            currencies:Array,
            areas:Array,
            columns:[],
            data:[],
            dataMaterial:null,
            total:Number,
            failedToFetchMaterialsNotification:false
        }
    },
    methods:{
        /**
         * Changes the current selected material
         */
        changeSelectedMaterial(tableRow){
            this.currentSelectedMaterial=tableRow.id;
        },
        /**
         * Triggers the creation of a new material
         */
        createMaterial(){
            this.createMaterialModal=true;
        },
        /**
         * Posts a new material price table entry
         */
        postMaterialPriceTableEntry(entryDetails){
        },
        fetchRequests(){
            this.refreshTable();
            /* this.fetchAvailableColors();
            this.fetchAvailableFinishes(); */
        },
        /**
         * Fetches all available materials
         */
        refreshTable(){
            this.data = [];
            MaterialRequests.getMaterials()
                .then((response)=>{
                    this.generateMaterialsTableData(response.data);
                    this.columns=this.generateMaterialsTableColumns();
                    this.total=this.data.length;
                })
                .catch((error_message)=>{
                    //Throw error?
                });
        },   
        /**
         * Generates the needed columns for the materials table
         */
        generateMaterialsTableColumns(){
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
                    field:"price",
                    label:"Current Price",
                    centered:true   
                },
                {
                    field:"finishes",
                    label:"Finishes",
                    centered:true   
                },
                {
                    field:"actions",
                    label:"Actions",
                    centered:true
                }
            ];
        },
        /**
         * Generates the data of a materials table by a given list of materials
         */
        async generateMaterialsTableData(materials){
            for(let i=0; i < materials.length; i++){    
                try{
                    const {data : {currentPrice}} = await PriceTableRequests.getCurrentMaterialPrice(materials[i].id, "", "");
                    this.data.push({
                        id: materials[i].id,
                        reference: materials[i].reference,
                        designation: materials[i].designation,
                        price: currentPrice.value + " " + currentPrice.currency + "/" + currentPrice.area
                    });
                }catch(error){
                    //Throw error?
                }
            }
        },
        async convertValuesToCurrency(){
            for(let i=0; i<this.data.length; i++){
                try{
                    let auxArray = this.data[i].price.split(' ');
                    let value = auxArray[0];
                    auxArray = auxArray[1].split('/');
                    let fromCurrency = auxArray[0];
                    let fromArea = auxArray[1];
                    let toCurrency = this.selectedCurrency.currency;
                    const {data: convertedPrice} = await CurrenciesPerAreaRequests.convertValue(fromCurrency,toCurrency,fromArea,fromArea,value)
                    this.data[i].price = convertedPrice.value + " " + convertedPrice.currency + "/" + convertedPrice.area;
                }catch(error){
                    //Throw error?
                }
            }
        },
        async convertValuesToArea(){
            for(let i=0; i<this.data.length; i++){
                try{
                    let auxArray = this.data[i].price.split(' ');
                    let value = auxArray[0];
                    auxArray = auxArray[1].split('/');
                    let fromCurrency = auxArray[0];
                    let fromArea = auxArray[1];
                    let toArea = this.selectedArea.area;
                    const {data: convertedPrice} = await CurrenciesPerAreaRequests.convertValue(fromCurrency,fromCurrency,fromArea,toArea,value);
                    this.data[i].price = convertedPrice.value + " " + convertedPrice.currency + "/" + convertedPrice.area;
                }catch(error){
                    //Throw error?
                }
            }
        }
    }
}
</script>
