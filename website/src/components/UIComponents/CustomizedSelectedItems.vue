<template>
    <b-field :label="customizedLabel">
        <b-field>
            <b-select 
                expanded 
                v-model="currentSelectedAddedItem" 
                :icon="icon"
                @input="getAddedItems"
                >
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
                let item=this.containsItem(this.addedItems,this.currentSelectedItem);
                if(item!=null)return;
                item=this.containsItem(this.availableItems,this.currentSelectedItem);
                this.addedItems.push(item);
            }
        },
        /**
         * Emits all added items
         */
        getAddedItems(){
            let realAddedItems=[];
            this.addedItems.forEach((item)=>{realAddedItems.push(item.id)});
            this.$emit('getAddedItems',realAddedItems.slice());
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
        },
        containsItem(collection,itemID){
            for(let i=0;i<collection.length;i++){
                if(collection[i]!=null&&collection[i].id==itemID)
                    return collection[i];
            }
            return null;
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
