<template>
    <vuetable
        :api-mode="false"
        :data="tableData"
        :fields="tableColumns"
    >
        <template slot="actions" slot-scope="props">
            <div class="custom-actions">
                <button
                    v-for="(actionsButton,index) in tableActionsButtons"
                    v-bind:key="index"
                    :class="actionsButton.class"
                    @click="emitButtonClick(props.rowData.id,actionsButton.id)"
                >
                    <b-icon :icon="actionsButton.icon"/>
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
        this.tableActionsButtons.push(...this.actionsButtons);
        this.tableColumns.push(...this.columns);
        this.tableData.push(...this.data);
        this.prepareTableActions();
    },
    /**
     * Component data
     */
    data(){
        return {
            tableActionsButtons:[],
            tableColumns:[],
            tableData:[]
        }
    },
    /**
     * Component methods
     */
    methods:{
        /**
         * Prepares the table actions
         */
        prepareTableActions(){
            let clonedTableActionsButtons=[];
            for(let i=this.tableActionsButtons.length;i>0;i--){
                let actionsButton=Object.assign({},this.tableActionsButtons.pop());
                if(!actionsButton.class)actionsButton.class="btn-primary"
                if(!actionsButton.id)actionsButton.id=i-1;
                clonedTableActionsButtons.unshift(actionsButton);
            }
            this.tableActionsButtons.push(...clonedTableActionsButtons);
            this.tableColumns.push({
                name:"__slot:actions",
                title:"Actions"
            });
        },
        /**
         * Emits the button click action
         * @param {Number} tableDataId Number with the data identifier
         * @param {Number} actionId Number with the action id
         */
        emitButtonClick(tableDataId,actionId){
            alert(actionId)
            this.$emit('emitButtonClick',tableDataId);
        }
    },
    /**
     * Component properties
     */
    props:{
        actionsButtons:[],
        columns:[],
        data:[],
    }
}
</script>
