<template>
    <div>
        <!-- CUD BUTTONS -->
        <b-field grouped>
                <b-field>
                    <button class="btn-primary" @click="createMaterial()">
                        <b-icon icon="plus"/>
                    </button>
                </b-field>
                <b-field>
                    <button class="btn-primary" @click="fetchRequests()">
                    <b-icon 
                        icon="refresh"/>
                    </button>
                </b-field>
            <div v-if="createMaterialModal">
                <b-modal :active.sync="createMaterialModal" has-modal-card scroll="keep">
                    <create-price-material 
                        :active="createMaterialModal" 
                        @createMaterialPriceTableEntry="createMaterialPriceTableEntry"
                    />
                </b-modal>
            </div>
            <b-field>
                <button class="btn-primary" @click="showPlotTimeSeriesChart">
                    <b-icon icon="chart-line-stacked"></b-icon>
                </button>
            </b-field>
            <div v-if="showPlotTimeSeriesChartModal">
                <b-modal :active.sync="showPlotTimeSeriesChartModal" has-modal-card scroll="keep">
                    <template>
                        <div class="modal-card" style="width:auto">
                            <header class="modal-card-head">
                                <p class="modal-card-title">Comparison between all material's price histories</p>
                            </header>
                            <section class="modal-card-body">
                                <div style="width:100%" ref="timeSeriesChart"></div>
                            </section>
                        </div>
                    </template>
                </b-modal>
            </div>
            <b-field>
                <b-field>
                    <b-field> 
                        <b-select icon="coin" placeholder="Currency" v-model="selectedCurrency" @input="convertValuesToCurrency">
                            <option v-for="currency in this.currencies" 
                            :key="currency.currency" 
                            :value="currency">
                            {{currency.currency}}</option>
                        </b-select>
                    </b-field>
                    <b-field> 
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
            @refreshData="fetchRequests"
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

export default {

    name:"ListPriceMaterials",

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
                this.$toast.open(error.response.data.message);
            });
        CurrenciesPerAreaRequests.getAreas()
            .then((response)=>{
                this.areas = response.data;
            })
            .catch((error)=>{
                this.$toast.open(error.response.data.message);
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
            failedToFetchMaterialsNotification:false,
            showPlotTimeSeriesChartModal:false
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
        async createMaterialPriceTableEntry(entries){
            let errorOccurred = false;
            for(let i=0; i < entries.length; i++){
                try{
                    await PriceTableRequests.postMaterialPriceTableEntry(entries[i].materialId, entries[i].tableEntry);
                }catch(error){
                    errorOccurred = true;
                    this.$toast.open(error.response.data.message);
                    break;
                }
            }
            if(!errorOccurred){
                this.$toast.open({
                message: "Prices created succesfully!"
                });
                this.createMaterialModal=false;
                this.refreshTable();
            }
        },
        fetchRequests(){
            this.refreshTable();
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
                    const {data} = await PriceTableRequests.getCurrentMaterialPrice(materials[i].id, "", "");
                    this.data.push({
                        id: materials[i].id,
                        tableEntryId: data.tableEntryId,
                        reference: materials[i].reference,
                        designation: materials[i].designation,
                        price: data.currentPrice.value + " " + data.currentPrice.currency + "/" + data.currentPrice.area,
                        startingDate: data.timePeriod.startingDate,
                        endingDate: data.timePeriod.endingDate
                    });
                }catch(error){
                    this.$toast.open(error.response.data);
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
                    this.$toast.open(error.response.data.message);
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
                    this.$toast.open(error.response.data.message);
                }
            }
        },

        showPlotTimeSeriesChart(){
            this.showPlotTimeSeriesChartModal=true;
            /*let xAxisArray = [];
            let yAxisArray = [];

            for (let i = 0; i < this.data.length; i++) {
                xAxisArray.push(this.data[i].startingDateTime);
                xAxisArray.push(this.data[i].endingDateTime);
                yAxisArray.push(this.data[i].value.split(" ")[0]);
                yAxisArray.push(this.data[i].value.split(" ")[0]);
            }

            var trace = {
                type: "scatter",
                mode: "lines",
                name: "Material " + this.materialId,
                x: xAxisArray,
                y: yAxisArray,
                line: { color: "#17BECF" }
            };

            var data = [trace];

            let minValue = 0;
            let maxValue = Math.max(yAxisArray);

            var layout = {
                title: "Price Evolution Time Series",
                width: 750,
                height: 500,
                xaxis: {range: [minValue, maxValue]},
                yaxis: {range: [minValue, maxValue]},
            };

            Plotly.newPlot(this.$refs.timeSeriesChart, data, layout); */
        }
    }
}
</script>
