<template>
    <b-modal :active.sync="active" has-modal-card scroll="keep">
                <div class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">New Product</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Reference">
                            <b-input
                                type="String"
                                v-model="referenceItem.value"
                                :placeholder="placeholders.reference"
                                icon="pound"
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Designation">
                            <b-input
                                type="String"
                                v-model="designationItem.value"
                                :placeholder="placeholders.designation"
                                icon="pencil"
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Category">
                            <b-select 
                                v-model="categoryItem.selected"
                                :placeholder="placeholders.category"
                                expanded="true" 
                                icon="tag"
                                @input="changeCurrentCategory">
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
                            :available-items="availableMaterials"
                            :customized-label="materials.customizedLabel"
                            :icon="materials.icon"
                            :place-holder="materials.placeholder"
                            @getAddedItems="changeCurrentMaterials(materials)"
                        />
                        <b-checkbox @input="enableComponents()">Components</b-checkbox>
                        <div v-if="components">
                            <customized-selected-items
                            :available-items="availableComponents"
                            :customized-label="componentsItems.customizedLabel"
                            :icon="componentsItems.icon"
                            :place-holder="componentsItems.placeholder"
                            @getAddedItems="changeCurrentComponents(components)"
                        />
                        </div>
                        <b-checkbox @input="enableDimensions()">Dimensions</b-checkbox>
                        <div v-if="dimensions">
                            <b-field label="Dimensions"/>
                            <b-field>
                                <b-select
                                    v-model="dimensionsItems.selected"
                                    expanded
                                    icon="wrench"
                                >
                                    <option 
                                        v-for="(dimension,index) in dimensionsItems.values" 
                                        :key="index"
                                        :value="dimension"
                                    >
                                        {{dimension}}
                                    </option>
                                </b-select>
                                <button class="button is-danger" @click="addDimensions()">
                                    <b-icon icon="plus"/>
                                </button>
                                <button class="button is-danger" @click="removeDimensions()">
                                    <b-icon icon="minus"/>
                                </button>
                            </b-field>
                            <product-dimensions dimension-label="Width" @getDimension="changeCurrentWidthDimension"/>
                            <product-dimensions dimension-label="Height" @getDimension="changeCurrentHeightDimension"/>
                            <product-dimensions dimension-label="Depth" @getDimension="changeCurrentDepthDimension"/>
                        </div>
                        <b-checkbox @input="enableSlots()">Slots</b-checkbox>
                        <div v-if="slots">
                            <slots-size :slotName="minSlotName"/>
                            <slots-size :slotName="recommendedSlotName" />
                            <slots-size :slotName="maxSlotName" />
                        </div>
                    </section>
                    <footer class="modal-card-foot">
                        <div class="has-text-centered">
                            <button class="button is-primary" @click="emitProduct($parent)">Create</button>
                        </div>
                    </footer>
                </div>
    </b-modal>
</template>

<script>

/**
 * Requires SlotsSize component
 */
import SlotsSize from './SlotsSize.vue'
/**
 * Requires CustomizedSelectedItems component
 */
import CustomizedSelectedItems from './UIComponents/CustomizedSelectedItems.vue'
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
        active:{
            type: Boolean,
            default: false
        }
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
                width:this.addDimensionItems.width,
                height:this.addDimensionItems.height,
                depth:this.addDimensionItems.depth,
            });
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
            this.componentsItem.value=components;
        },
        /**
         * Changes the current materials item
         */
        changeCurrentMaterials(materials){
            this.materialsItem.value=materials;
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
         * Emits the product to the father component
         */
        emitProduct(modal){
            let productDetails={
                reference:this.referenceItem.value,
                designaton:this.designationItem.value,
                category:this.categoryItem.value,
                materials:this.materialsItem.value,
                dimensions:this.dimensionsItem.value,
                components:this.componentsItem.value,
                slots:this.slotsItem.value
            };
            console.log(productDetails);
            //modal.close();
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
        }
    }
}
</script>
