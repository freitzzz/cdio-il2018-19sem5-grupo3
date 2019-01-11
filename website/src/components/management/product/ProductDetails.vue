<template>
    <div class="modal-card" style="width: auto">
        <header class="modal-card-head">
            <p class="modal-card-title">Product Details</p>
        </header>
        <section class="modal-card-body">
            <b-field label="Reference">
                <b-input
                    type="String"
                    :value="product.reference"
                    disabled="true"
                    icon="pound"
                    required>
                </b-input>
            </b-field>
            <b-field label="Designation">
                <b-input
                    type="String"
                    :value="product.designation"
                    disabled="true"
                    icon="pencil"
                    required>
                </b-input>
            </b-field>
            <b-field label="3D Model">
                <b-input
                    type="String"
                    :value="product.model"
                    disabled="true"
                    icon="video-3d"
                    required>
                </b-input>
            </b-field>
            <b-field label="Category">
                <b-input
                    type="String"
                    :value="product.category.name"
                    disabled="true"
                    icon="tag"
                />
            </b-field>
            <b-checkbox type="is-info" @input="enableMaterials()">Materials</b-checkbox>
            <div v-if="showMaterials">
                <b-field label="Materials">
                    <b-field>
                        <simple-table
                            :columns="simpleTablesColumns.materials"
                            :data="product.materials"
                            :allowActions="true"
                            @emitShowDetails="showMaterialDetails"
                        />
                        <b-modal :active.sync="detailsModals.materials">
                            <material-details
                                :material="detailsData.material"
                            />
                        </b-modal>
                    </b-field>
                </b-field>
            </div>
            <b-checkbox type="is-info" v-if="product.components!=null" @input="enableComponents()">Components</b-checkbox>
            <div v-if="showComponents">
                <b-field label="Components">
                    <b-field>
                        <simple-table
                            :columns="simpleTablesColumns.components"
                            :data="product.components"
                            :allowActions="true"
                            @emitShowDetails="showComponentDetails"
                        />
                        <b-modal :active.sync="detailsModals.components">
                            <product-details
                                :product="detailsData.component"
                            />
                        </b-modal>
                    </b-field>
                </b-field>
            </div>
            <b-checkbox type="is-info" v-if="product.dimensions!=null" @input="enableDimensions()">Dimensions</b-checkbox>
                <div v-if="showDimensions">
                    <b-field label="Width Dimensions">
                        <simple-table
                            :columns="simpleTablesColumns.dimensions"
                            :data="productDimensions.width"
                        />
                    </b-field>
                    <b-field label="Height Dimensions">
                        <simple-table
                            :columns="simpleTablesColumns.dimensions"
                            :data="productDimensions.height"
                        />
                    </b-field>
                    <b-field label="Depth Dimensions">
                        <simple-table
                            :columns="simpleTablesColumns.dimensions"
                            :data="productDimensions.depth"
                        />
                    </b-field>
                </div>
            <b-checkbox type="is-info" v-if="product.slotWidths!=null" @input="enableSlots()">Slots</b-checkbox>
            <div v-if="showSlots">
                <b-field label="Slots"/>
                <b-field>
                    <b-field label="Minimum Width">
                        <b-input
                            type="String"
                            :value="product.slotWidths.minWidth"
                            disabled="true"
                            icon="wrench"
                            required
                        />
                    </b-field>
                    <b-field label="Recommended Width">
                        <b-input
                            type="String"
                            :value="product.slotWidths.recommendedWidth"
                            disabled="true"
                            icon="wrench"
                            required
                        />
                    </b-field>
                    <b-field label="Maximum Width">
                        <b-input
                            type="String"
                            :value="product.slotWidths.maxWidth"
                            disabled="true"
                            icon="wrench"
                            required
                        />
                    </b-field>
                    <b-field label="Unit">
                        <b-input
                            type="String"
                            :value="product.slotWidths.unit"
                            disabled="true"
                            icon="ruler"
                            required
                        />
                    </b-field>
                </b-field>
            </div>
        </section>
    </div>
</template>

<script>

/**
 * Requires Axios for HTTP requests
 */
import Axios from 'axios';

/**
 * Requires MYCM API URL
 */
import Config,{MYCM_API_URL} from '../../../config.js';

/**
 * Requires MaterialDetails component
 */
import MaterialDetails from '../material/MaterialDetails';

/**
 * Requires own recursive component for product components details
 */
import ProductDetails from './ProductDetails';

/**
 * Requires SimpleTable component
 */
import SimpleTable from './../../UIComponents/SimpleTable';

export default {
    /**
     * Exported used components
     */
    components:{
        MaterialDetails,
        ProductDetails,
        SimpleTable
    },
    /**
     * Received properties from father component
     */
    props:{

        /**
         * Current Product details
         */
        product:{
            type:Object,
            required:true
        }
    },
    /**
     * Component Created State call
     */
    created(){
        this.extractProductDimensions();
    },
    /**
     * Component data
     */
    data(){
        return{
            productDimensions:{
                width:[],
                height:[],
                depth:[]
            },
            simpleTablesColumns:{
                components:[
                    {
                        name: "id",
                        title: "ID"
                    },
                    {
                        name: "reference",
                        title: "Reference"
                    },
                    {
                        name: "designation",
                        title: "Designation"
                    },
                    {
                        name: "mandatory",
                        title: "Mandatory",
                        callback: this.booleansAsIcons
                    },
                    {
                        name: "supportsSlots",
                        title: "Supports Slots",
                        dataClass: "centered aligned",
                        callback: this.booleansAsIcons
                    },
                    {
                        name: "hasComponents",
                        title: "Has Components",
                        callback: this.booleansAsIcons
                    }
                ],
                dimensions:[
                    {
                        name: "id",
                        title: "ID"
                    },
                    {
                        name: "value",
                        title: "Value"
                    },
                    {
                        name: "values",
                        title: "Values"
                    },
                    {
                        name: "minValue",
                        title: "Minimum Value"
                    },
                    {
                        name: "maxValue",
                        title: "Maximum Value"
                    },
                    {
                        name: "increment",
                        title: "Increment"
                    },
                    {
                        name: "unit",
                        title: "Unit"
                    },
                ],
                materials:[
                    {
                        name:"id",
                        title:"ID"
                    },
                    {
                        name:"reference",
                        title:"Reference"
                    },
                    {
                        name:"designation",
                        title:"Designation"
                    },
                    {
                        name:"image",
                        title:"Image"
                    }
                ]
            },
            showComponents:false,
            showDimensions:false,
            showMaterials:false,
            showSlots:false,
            detailsModals:{
                components:false,
                materials:false
            },
            detailsData:{
                component:null,
                material:null
            }
        }
    },
    /**
     * Component methods
     */
    methods:{
        /**
         * Deprecated ?
         */
        addDimensions(){
            this.dimensionsItems.values.push({
                width:this.addDimensionItems.width,
                height:this.addDimensionItems.height,
                depth:this.addDimensionItems.depth,
            });
        },
        /**
         * Transforms a boolean value into a icon
         */
        booleansAsIcons(value){
            return value 
                ? '<span class="ui teal label"><i class="material-icons">check</i></span>'
                : '<span class="ui teal label"><i class="material-icons">close</i></span>'
            ;
        },
        /**
         * Enables components section
         */
        enableComponents(){
            this.showComponents=!this.showComponents;
        },
        /**
         * Enables dimensions section
         */
        enableDimensions(){
            this.showDimensions=!this.showDimensions;
        },
        /**
         * Enables materials section
         */
        enableMaterials(){
            this.showMaterials=!this.showMaterials;
        },
        /**
         * Enables slots section
         */
        enableSlots(){
            this.showSlots=!this.showSlots;
        },
        /**
         * Extracts the current product dimensions
         */
        extractProductDimensions(){
            let widthDimensions=[];
            let heightDimensions=[];
            let depthDimensions=[];
            let product=this.product;
            for(let i=0;i<product.dimensions.length;i++){
                widthDimensions.push(product.dimensions[i].width);
                heightDimensions.push(product.dimensions[i].height);
                depthDimensions.push(product.dimensions[i].depth);
            }
            this.productDimensions.width=widthDimensions;
            this.productDimensions.height=heightDimensions;
            this.productDimensions.depth=depthDimensions;
        },
        /**
         * Shows the details of a material
         */
        showMaterialDetails(materialId){
            let promise=new Promise((accept,reject)=>{
                Axios
                    .get(MYCM_API_URL+'/materials/'+materialId)
                    .then((material)=>{
                        accept(material.data);
                    })
                    .catch((error_message)=>{
                        reject(error_message.response.data.message);
                    });
            });
            promise
                .then((material)=>{
                    for(let i =0;i<material.colors.length;i++)
                        material.colors[i].colors={
                            red: material.colors[i].red,
                            green: material.colors[i].green,
                            blue: material.colors[i].blue,
                        };
                    this.detailsData.material=material;
                    this.detailsModals.materials=true;
                })
                .catch((error_message)=>{
                    this.$toast.open({message:error_message});
                });
        },
        /**
         * Shows the details of a component
         */
        showComponentDetails(componentId){
            let promise=new Promise((accept,reject)=>{
                Axios
                    .get(MYCM_API_URL+'/products/'+this.product.id+'/components/'+componentId)
                    .then((component)=>{
                        accept(component.data);
                    })
                    .catch((error_message)=>{
                        reject(error_message.response.data.message);
                    });
            });
            promise
                .then((component)=>{
                    this.detailsData.component=component;
                    this.detailsModals.components=true;
                })
                .catch((error_message)=>{
                    this.$toast.open({message:error_message});
                });
        }
    },
    /**
     * Component name
     */
    name:"ProductDetails"
}
</script>
