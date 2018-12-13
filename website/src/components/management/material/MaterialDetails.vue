<template>
    <div class="modal-card" style="width: auto">
        <header class="modal-card-head">
            <p class="modal-card-title">Material Details</p>
        </header>
        <section class="modal-card-body">
            <b-field label="Reference">
                <b-input
                    type="String"
                    :value="material.reference"
                    disabled="true"
                    icon="pound"
                    required>
                </b-input>
            </b-field>
            <b-field label="Designation">
                <b-input
                    type="String"
                    :value="material.designation"
                    disabled="true"
                    icon="pencil"
                    required>
                </b-input>
            </b-field>
            <b-field label="Image">
                <b-input
                    type="String"
                    :value="material.image"
                    disabled="true"
                    icon="pencil"
                    required>
                </b-input>
            </b-field>
             <b-checkbox @input="enableColors()">Colors</b-checkbox>
            <div v-if="showColors">
                <b-field label="Colors">
                    <simple-table
                        :columns="simpleTablesColumns.colors"
                        :data="material.colors"
                    />
                </b-field>
            </div>
             <b-checkbox @input="enableFinishes()">Finishes</b-checkbox>
            <div v-if="showFinishes">
                <b-field label="Finishes">
                    <simple-table
                        :columns="simpleTablesColumns.finishes"
                        :data="material.finishes"
                    />
                </b-field>
            </div>
        </section>
    </div>
</template>
<script>

/**
 * Requires SimpleTable component
 */
import SimpleTable from './../../UIComponents/SimpleTable';

import Swatches from "vue-swatches";
import "vue-swatches/dist/vue-swatches.min.css";

export default {
    /**
     * Exported used components
     */
    components:{
        SimpleTable,
        Swatches
    },
    /**
     * Received properties from father component
     */
    props:{
         availableColors:{
            type:Array,
            required:true
        },
         availableFinishes:{
            type:Array,
            required:true
        },
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
        simpleTablesColumns:{
                colors:[
                    {
                        name: "id",
                        title: "ID"
                    },
                    {
                        name: "name",
                        title: "Name"
                    },
                    {
                        name: "colors",
                        title: "Color",
                        callback: this.booleansAsColor
                    },
                ],
                finishes:[
                    {
                        name: "id",
                        title: "ID"
                    },
                    {
                        name: "description",
                        title: "Description"
                    },
                    {
                        name: "shininess",
                        title: "Shininess"
                    },
                ],
    },
            showColors:false,
            showFinishes:false,
        }
    },
    /**
     * Component methods
     */
    methods:{
        /**
         * Enables colors section
         */
        enableColors(){
            this.showColors=!this.showColors;
        },
        /**
         * Enables finishes section
         */
        enableFinishes(){
            this.showFinishes=!this.showFinishes;
        },
        /* createSwatcheColor(red,green,blue){
            <swatches v-model="#redgreenblue"></swatches>
        } */
         booleansAsColor(value){
            return `<div class="vue-swatches__swatch vue-swatches__swatch--border" 
            style="display: inline-block; width: 42px; height: 42px; 
            margin-bottom: 11px; margin-right: 11px; 
            border-radius: 11px; 
            background-color: rgb(`+value.red+`,`+value.green+`,`+value.blue+`); 
            cursor: pointer;"><!----> 
            <div class="vue-swatches__check__wrapper vue-swatches--has-children-centered" 
            style="display: none;">
            <div class="vue-swatches__check__circle vue-swatches--has-children-centered">
            <svg version="1.1" role="presentation" 
            width="12" 
            height="12" viewBox="0 0 1792 1792" class="check">
            <path d="M1671 566q0 40-28 68l-724 724-136 136q-28 28-68 28t-68-28l-136-136-362-362q-28-28-28-68t28-68l136-136q28-28 68-28t68 28l294 295 656-657q28-28 68-28t68 28l136 136q28 28 28 68z" class="vue-swatches__check__path"></path></svg></div></div></div>`
            ;
        },
       
    }
}
</script>
