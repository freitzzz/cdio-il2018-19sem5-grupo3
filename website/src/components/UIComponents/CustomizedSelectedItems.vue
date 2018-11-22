<template>
    <b-field :label="customizedLabel">
        <b-field>
            <b-select 
                expanded=true 
                v-model="currentSelectedAddedItem" 
                :icon="icon">
                <option
                    v-for="(item,index) in addedItems" 
                    :value="item.id" 
                    :key="index">
                        {{item.value}}
                </option>
            </b-select>
            <b-select 
                :placeholder="placeHolder" 
                :icon="icon" 
                v-model="currentSelectedItem">
                <option 
                    v-for="(item,index) in availableItems" 
                    :value="item.id" 
                    :key="index">
                        {{item.value}}
                </option>
            </b-select>
            <button class="button is-danger" @click="addSelectedItem()">
                <b-icon icon="plus"/>
            </button>
            <button class="button is-danger" @click="removeSelectedItem()">
                <b-icon icon="minus"/>
            </button>
        </b-field>
    </b-field>
</template>

<script>
export default {
    methods:{
        activateAddedItems(){
            if(!this.addedItems)this.addedItems=new Array();
        },
        /**
         * Adds the current selected item to the added items list
         */
        addSelectedItem(){
            if(this.currentSelectedItem!=null){
                this.activateAddedItems();
                this.addedItems.push(this.availableItems[this.currentSelectedItem-1]);
            }
        },
        /**
         * Emits all added items
         */
        getAddedItems(){
            this.$emit('getAddedItems',this.addedItems);
        },
        /**
         * Removes the current selected item from the added items list
         */
        removeSelectedItem(){
            let newAddedItems=[];
            this.addedItems.forEach((item)=>{
                if(item.id!=this.currentSelectedAddedItem)
                    newAddedItems.push(item);
            });
            this.addedItems=newAddedItems;
        }
    },
    data(){
        return {
            currentSelectedItem:0,
            addedItems:[],
            currentSelectedAddedItem:0
        }
    },
    props:{
        availableItems:{
            type:Array,
            required:true
        },
        customizedLabel:{
            type:String,
            required:false
        },
        icon:{
            type:String,
            required:false
        },
        placeHolder:{
            type:String,
            required:false
        }
    }
}
</script>
