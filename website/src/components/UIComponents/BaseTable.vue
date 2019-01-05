<template>
    <vuetable
        :api-mode="false"
        :data="tableData"
        :fields="tableColumns"
    >
        <template slot="actions" slot-scope="props">
            <div class="custom-actions">
                <button
                    class="btn-primary"
                    @click="emitShowDetails(props.rowData.id)"
                >
                    <b-icon icon="pencil"/>
                </button>
            </div>
        </template>
    </vuetable>
</template>

<script>

export default {
    /**
     * Call before component creation
     */
    created(){
        this.tableColumns.push(...this.columns);
        this.tableData.push(...this.data);
        this.generateTableActions();
    },
    /**
     * Component data
     */
    data(){
        return {
            tableColumns:[],
            tableData:[]
        }
    },
    /**
     * Component methods
     */
    methods:{
        /**
         * Generates the table actions
         */
        generateTableActions(){
            if(this.allowActions)
                this.tableColumns.push({
                    name:"__slot:actions",
                    title:"Details"
                });
        },
        /**
         * Emits the show details action
         * @param {Number} tableDataId Number with the data identifier
         */
        emitShowDetails(tableDataId){
            this.$emit('emitShowDetails',tableDataId);
        }
    },
    /**
     * Component properties
     */
    props:{
        columns:[],
        data:[],
        allowActions:Boolean
    }
}
</script>
