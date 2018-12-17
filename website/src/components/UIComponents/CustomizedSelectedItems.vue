<template>
    <b-field :label="customizedLabel">
        <b-field>
            <b-field label="Items" expanded>
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
            </b-field>
            <b-field label="Available Items">
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
            </b-field>
            <b-field label="Actions">
                <b-field>
                    <button class="button is-info" @click="addSelectedItem()">
                        <b-icon icon="plus"/>
                    </button>
                    <button class="button is-info" @click="removeSelectedItem()">
                        <b-icon icon="minus"/>
                    </button>
                </b-field>
            </b-field>
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
                this.getAddedItems();
                this.currentSelectedAddedItem = item.id;
            }
        },
        /**
         * Emits all added items
         */
        getAddedItems(){
            let realAddedItems=[];
            this.addedItems.forEach((item)=>{realAddedItems.push(item.id)});
            this.$emit('emitItems',realAddedItems.slice());
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
            this.getAddedItems();
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
            currentSelectedAddedItem:0
        }
    },
    props:{
        addedItems:{
            type:Array
        },
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
