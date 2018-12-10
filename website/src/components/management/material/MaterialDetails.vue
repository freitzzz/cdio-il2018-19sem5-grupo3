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

export default {
    /**
     * Exported used components
     */
    components:{
        SimpleTable
    },
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
       
    }
}
</script>
