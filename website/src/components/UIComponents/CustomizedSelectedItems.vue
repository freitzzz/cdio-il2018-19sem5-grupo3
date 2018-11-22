<template>
    <b-field :label="customizedLabel">
        <b-field>
            <b-select v-model="addedItems" :icon="icon">
                <option :placeholder="placeHolder" v-for="(item,index) in addedItems" :value="item" :key="index">
                    {{item}}
                </option>
            </b-select>
            <b-select v-model="availableItems" :placeholder="placeHolder" :icon="icon" v-on="currentSelectedItem">
                <option :placeholder="placeHolder" v-for="(item,index) in addedItems" :value="item" :key="index">
                    {{item}}
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
                this.addedItems.push(this.currentSelectedItem);
            }
        },
        /**
         * Removes the current selected item from the added items list
         */
        removeSelectedItem(){
            console.log(this.addedItems);
            let newAddedItems=[];
            let removedWithSucess=this.addedItems.length==0 ? true : false;
            while(!removedWithSucess){
                let removedItem=this.addedItems.pop();
                removedWithSucess&=removedItem===this.currentSelectedItem;
                if(!removedWithSucess)newAddedItems.push(removedItem);
            }
            this.addedItems.push(...newAddedItems);
        }
    },
    data(){
        return {
            currentSelectedItem:null,
            addedItems:[],
            currentSelectedAddedItem:null
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
