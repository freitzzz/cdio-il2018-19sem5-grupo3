<template>
    <div class="modal-card" style="width: auto">
        <header class="modal-card-head">
            <p class="modal-card-title">Edit Product</p>
        </header>
        <section class="modal-card-body">
            <b-field label="Reference">
                <b-input
                    type="String"
                    v-model="product.reference"
                    icon="pound"
                    required>
                </b-input>
            </b-field>
            <b-field label="Designation">
                <b-input
                    type="String"
                    v-model="product.designation"
                    icon="pencil"
                    required>
                </b-input>
            </b-field>
            <b-field label="Category">
                <b-select 
                    :placeholder="product.category.name"
                    v-model="categoryItem.selected"
                    icon="tag"
                    @input="changeCurrentCategory"
                    expanded
                    >
                    <option 
                        v-for="(category,index) in availableCategories" 
                        :key="index"
                        :value="category.id"
                    >
                        {{category.name}}
                    </option>
                </b-select>
            </b-field>
            <customized-selected-items
                :added-items="toCustomizedSelectedMaterials(product.materials)"
                :available-items="toCustomizedSelectedMaterials(availableMaterials)"
                :customized-label="materials.customizedLabel"
                :icon="materials.icon"
                :place-holder="materials.placeholder"
                @emitItems="changeCurrentMaterials"
            />
            <b-checkbox @input="enableComponents()">Components</b-checkbox>
            <div v-if="components">
                <customized-selected-items
                    :added-items="toCustomizedSelectedComponents(product.components ? product.components : [])"
                    :available-items="toCustomizedSelectedComponents(availableComponents)"
                    :customized-label="componentsItems.customizedLabel"
                    :icon="componentsItems.icon"
                    :place-holder="componentsItems.placeholder"
                    :allowRequire="true"
                    @emitItems="changeCurrentComponents"
                />
            </div>
            <b-checkbox @input="enableDimensions()">Dimensions</b-checkbox>
            <div v-if="dimensions">
                <b-field label="Dimensions"/>
                <b-field>
                    <b-select
                        v-model="this.dimensionsItems.selected"
                        icon="wrench"
                        expanded
                    >
                        <option 
                            v-for="(dimension,index) in dimensionsItems.values" 
                            :key="index"
                            :value="dimension"
                        >
                            {{dimension}}
                        </option>
                    </b-select>
                    <button class="btn-primary" @click="addDimensions()">
                        <b-icon icon="plus"/>
                    </button>
                    <button class="btn-primary" @click="removeDimensions()">
                        <b-icon icon="minus"/>
                    </button>
                </b-field>
                <product-dimensions dimension-label="Width" :available-units="availableUnits" :current-dimension="product.dimensions[0].width" @getDimension="changeCurrentWidthDimension"/>
                <product-dimensions dimension-label="Height" :available-units="availableUnits" :current-dimension="product.dimensions[0].height" @getDimension="changeCurrentHeightDimension"/>
                <product-dimensions dimension-label="Depth" :available-units="availableUnits" :current-dimension="product.dimensions[0].depth" @getDimension="changeCurrentDepthDimension"/>
            </div>
        </section>
        <footer class="modal-card-foot">
            <div class="has-text-centered">
                <button class="btn-primary" @click="emitProduct($parent)">Save</button>
            </div>
        </footer>
    </div>
</template>

<script>

/**
 * Requires SlotsSize component
 */
import SlotsSize from '../../UIComponents/SlotsSize.vue'
/**
 * Requires CustomizedSelectedItems component
 */
import CustomizedSelectedItems from '../../UIComponents/CustomizedSelectedItems.vue'
/**
 * Requires ProductDimensions component
 */
import ProductDimensions from './ProductDimensions.vue';

export default {
    /**
     * Exported used components
     */
    components:{
        SlotsSize,
        CustomizedSelectedItems,
        ProductDimensions
    },
    /**
     * Component Created State call
     */
    created(){
        this.dimensionsItems.values=this.product.dimensions.slice();
        this.dimensionsItems.selected=this.dimensionsItems.values[0];
    },
    /**
     * Received properties from father component
     */
    props:{
        availableMaterials:{
            type:Array,
            required:true
        },
        availableComponents:{
            type:Array,
            required:true
        },
        availableCategories:{
            type:Array,
            required:true
        },
        availableUnits:{
            type:Array,
            required:true
        },
        active:{
            type: Boolean,
            default: false
        },
        product:{
            type:Object,
            required:true
        },
    },
    /**
     * Component data
     */
    data(){
        return{
            referenceItem:{
                value:null
            },
            designationItem:{
                value:null
            },
            categoryItem:{
                selected:0,
                value:null
            },
            materialsItem:{
                value:null
            },
            componentsItem:{
                value:null
            },
            dimensionsItem:{
                value:null
            },
            slotsItem:{
                value:null
            },
            required:{
                value:false
            },
            placeholders:{
                reference:"#666",
                designation:"Devil Wardrobe",
                category:"Select a category"
            },
            components:false,
            slots:false,
            dimensions:false,
            minSlotName:"Minimum Slot Size",
            recommendedSlotName:"Recommended Slot Size",
            maxSlotName:"Maximum Slot Size",
            componentsItems:{
                availableItems:['Drawer','Shelf'],
                customizedLabel:"Components",
                icon:"buffer",
                placeHolder:"Select a component"
            },
            addDimensionItems:{
                width:null,
                height:null,
                depth:null
            },
            dimensionsItems:{
                selected:0,
                values:[]
            },
            slotDimensionsItem:{
                min:null,
                recommended:null,
                max:null
            },
            materials:{
                availableItems:['MDF','Cherry','Orange'],
                customizedLabel:"Materials",
                icon:"brush",
                placeHolder:"Select a material"
            }
        }
    },
    /**
     * Component methods
     */
    methods:{
        addDimensions(){
            this.dimensionsItems.values.push({
                id:0,
                width:this.addDimensionItems.width,
                height:this.addDimensionItems.height,
                depth:this.addDimensionItems.depth,
            });
        },
        removeDimensions(){
            let newDimensions=[];
            this.dimensionsItems.values.forEach((dimension)=>{
                if(dimension.id!=this.dimensionsItems.selected.id){
                    newDimensions.push(dimension);
                }
            });
            this.dimensionsItems.values=newDimensions.slice();
            this.dimensionsItems.selected=newDimensions.length!=0 ? newDimensions[0] : 0;
        },
        /**
         * Changes the current category item
         */
        changeCurrentCategory(){
            this.categoryItem.value=this.categoryItem.selected;
        },
        /**
         * Changes the current components item
         */
        changeCurrentComponents(components){
            console.log(components)
            this.componentsItem.value=components;
        },
        /**
         * Changes the current materials item
         */
        changeCurrentMaterials(materials){
            let addedMaterials=[];
            materials.forEach((material)=>{addedMaterials.push({id:material});});
            this.materialsItem.value=addedMaterials.slice();
        },
        /**
         * Changes the current width dimension item
         */
        changeCurrentWidthDimension(dimension){
            this.addDimensionItems.width=dimension;
        },
        /**
         * Changes the current height dimension item
         */
        changeCurrentHeightDimension(dimension){
            this.addDimensionItems.height=dimension;
        },
        /**
         * Changes the current depth dimension item
         */
        changeCurrentDepthDimension(dimension){
            this.addDimensionItems.depth=dimension;
        },
        /**
         * Changes the current minimum slot dimensions
         */
        changeCurrentMinSlotDimensions(slotDimension){
            this.slotDimensionsItem.min=slotDimension;
        },
        /**
         * Changes the current recommended slot dimensions
         */
        changeCurrentRecommendedSlotDimensions(slotDimension){
            this.slotDimensionsItem.recommended=slotDimension;
        },
        /**
         * Changes the current maximum slot dimensions
         */
        changeCurrentMaxSlotDimensions(slotDimension){
            this.slotDimensionsItem.max=slotDimension;
        },
        /**
         * Changes the current slot dimensions sizes
         */
        changeCurrentSlotDimensionsSizes(){
            this.slotsItem.value={
                minSize:this.slotDimensionsItem.min,
                recommendedSize:this.slotDimensionsItem.recommended,
                maxSize:this.slotDimensionsItem.max
            };
        },
        /**
         * Emits the product to the father component
         */
        emitProduct(modal){
            this.changeCurrentSlotDimensionsSizes();
            let productDetails={
                id:this.product.id,
                reference:this.product.reference,
                designation:this.product.designation,
                category:this.categoryItem.value,
                components:this.componentsItem.value,
                dimensions:this.dimensionsItems.values,
                materials:this.materialsItem.value
            };
            this.$emit('emitProduct',productDetails);
        },
        /**
         * Enables components section
         */
        enableComponents(){
            this.components=!this.components;
        },
        /**
         * Enables slots section
         */
        enableSlots(){
            this.slots=!this.slots;
        },
        /**
         * Enables dimensions section
         */
        enableDimensions(){
            this.dimensions=!this.dimensions;
        },
        /**
         * Transforms a list of components to a list of customized selected components
         */
        toCustomizedSelectedComponents(components){
            let customizedSelectedComponents=[];
            for(let i=0;i<components.length;i++){
                customizedSelectedComponents.push({id:components[i].id,value:components[i].reference,required:components[i].mandatory});
            }
            return customizedSelectedComponents;
        },
        /**
         * Transforms a list of materials to a list of customized selected materials
         */
        toCustomizedSelectedMaterials(materials){
            let customizedSelectedMaterials=[];
            for(let i=0;i<materials.length;i++){
                customizedSelectedMaterials.push({id:materials[i].id,value:materials[i].designation});
            }
            return customizedSelectedMaterials;
        }
    }
}
</script>
