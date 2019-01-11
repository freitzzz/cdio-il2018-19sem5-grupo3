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
            <div v-if="allowRequire">
                <b-field label="Required">
                    <b-field>
                        <button class="button" @click="changeRequiredItem()">
                            <b-checkbox v-model="required.value"/>
                        </button>
                    </b-field>
                </b-field>
            </div>
            <b-field label="Actions">
                <b-field>
                     <small-padding-div>
                    <button class="btn-primary" @click="addSelectedItem()">
                        <b-icon icon="plus"/>
                    </button>
                    <button class="btn-primary" @click="removeSelectedItem()">
                        <b-icon icon="minus"/>
                    </button>
                     </small-padding-div>
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
                if(!this.allowRequire){
                    this.addedItems.push(item);
                }else{
                    item.required=this.required.value;
                    this.addedItems.push(item);
                }
                this.getAddedItems();
                this.currentSelectedAddedItem = item.id;
            }
        },
        /**
         * Changes the current item requireness
         */
        changeRequiredItem(){
            this.required.value ? this.required.icon="close" : this.required.icon="check";
            this.required.value=!this.required.value;
        },
        /**
         * Emits all added items
         */
        getAddedItems(){
            let realAddedItems=[];
            if(!this.allowRequire){
                this.addedItems.forEach((item)=>{realAddedItems.push(item.id)});
            }else{
                this.addedItems.forEach((item)=>{realAddedItems.push({
                    id:item.id,
                    required:item.required
                })});
                let realCurrentSelectedAddedItem=this.containsItem(this.addedItems,this.currentSelectedAddedItem);
                if(realCurrentSelectedAddedItem)this.required.value=realCurrentSelectedAddedItem.required;
            }
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
        /**
         * Checks if a collection contains an item
         */
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
            currentSelectedAddedItem:0,
            required:{
                icon:"close",
                value:false
            }
        }
    },
    props:{
        addedItems:{
            type:Array
        },
        allowRequire:Boolean,
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
