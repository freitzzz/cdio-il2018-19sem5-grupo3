<template>
    <b-field>
        <b-select v-model="addedMaterials" icon="brush">
            <option v-for="(material,index) in addedMaterials" :value="material" :key="index">
                {{material}}
            </option>
        </b-select>
        <b-select placeholder="MDF" icon="brush" v-model="currentSelectedMaterial">
            <option value="MDF">MDF</option>
            <option value="Cherry">Cherry</option>
            <option value="Orange">Orange</option>
        </b-select>
        <button class="button is-danger" @click="addSelectedMaterial()">
            <b-icon icon="plus"/>
        </button>
        <button class="button is-danger" @click="removeSelectedMaterial()">
            <b-icon icon="minus"/>
        </button>
    </b-field>
</template>

<script>
export default {
    methods:{
        activateAddedMaterials(){
            if(!this.addedMaterials)this.addedMaterials=new Array();
        },
        /**
         * Adds the current selected material to the added materials list
         */
        addSelectedMaterial(){
            if(this.currentSelectedMaterial!=null){
                this.activateAddedMaterials();
                this.addedMaterials.push(this.currentSelectedMaterial);
            }
        },
        /**
         * Removes the current selected material from the added materials list
         */
        removeSelectedMaterial(){
            console.log(this.addedMaterials);
            let newAddedMaterials=[];
            let removedWithSucess=this.addedMaterials.length==0 ? true : false;
            while(!removedWithSucess){
                let removedItem=this.addedMaterials.pop();
                removedWithSucess&=removedItem===this.currentSelectedMaterial;
                if(!removedWithSucess)newAddedMaterials.push(removedItem);
            }
            this.addedMaterials.push(...newAddedMaterials);
        }
    },
    data(){
        return {
            currentSelectedMaterial:null,
            addedMaterials:[],
            currentSelectedAddedMaterial:null
        }
    },
    props:{
        availableMaterials:{
            type:Array
        }
    }
}
</script>
