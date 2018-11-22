<template>
    <b-section>
        <b-field :label="dimensionLabel"/>
        <b-section>
                <b-section v-if="dimension.single.available">
                    <b-field>
                        <b-field label="Dimension Value">
                            <b-input
                                type="Number"
                                :value.sync="dimension.single.value"
                                placeholder="200"
                                icon="wrench"
                                expanded=true
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Dimension Type">
                            <b-select v-model="dimension.selected" @input="changeDimensionType">
                                <option 
                                        v-for="(dimensionType,index) in dimension.availableDimensionTypes" 
                                        :key="index"
                                        :value="dimensionType.id"
                                >
                                    {{dimensionType.name}}
                                </option>
                            </b-select>
                        </b-field>
                    </b-field>
                </b-section>
                <b-section v-if="dimension.discrete.available">
                    <b-field>
                        <b-field label="Dimension Value" >
                            <b-input
                                type="Number"
                                placeholder="200"
                                icon="wrench"
                                expanded=true
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Values">
                            <b-select 
                                v-model="dimension.discrete.selected"
                                icon="wrench"
                                >
                                    <option 
                                        v-for="(discreteValue,index) in dimension.discrete.values" 
                                        :key="index"
                                        :value="discreteValue"
                                    >
                                        {{discreteValue}}
                                    </option>
                            </b-select>
                        </b-field>
                            <b-field label="Add">
                                <button class="button is-danger" @click="addDiscreteValue()">
                                    <b-icon icon="plus"/>
                                </button>
                            </b-field>
                            <b-field label="Remove">
                                <button class="button is-danger" @click="removeDiscreteValue()">
                                    <b-icon icon="minus"/>
                                </button>
                            </b-field>
                        <b-field label="Dimension Type">
                            <b-select v-model="dimension.selected" @input="changeDimensionType">
                                <option 
                                        v-for="(dimensionType,index) in dimension.availableDimensionTypes" 
                                        :key="index"
                                        :value="dimensionType.id"
                                >
                                    {{dimensionType.name}}
                                </option>
                            </b-select>
                        </b-field>
                    </b-field>
                </b-section>
                <b-section v-if="dimension.continuous.available">
                    <b-field>
                        <b-field label="Min Value">
                            <b-input
                                type="Number"
                                :value.sync="dimension.continuous.minValue"
                                placeholder="200"
                                icon="wrench"
                                expanded=true
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Max Value">
                            <b-input
                                type="Number"
                                :value.sync="dimension.continuous.maxValue"
                                placeholder="200"
                                icon="wrench"
                                expanded=true
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Increment">
                            <b-input
                                type="Number"
                                :value.sync="dimension.continuous.increment"
                                placeholder="200"
                                icon="wrench"
                                expanded=true
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Dimension Type">
                            <b-select v-model="dimension.selected" @input="changeDimensionType">
                                <option 
                                        v-for="(dimensionType,index) in dimension.availableDimensionTypes" 
                                        :key="index"
                                        :value="dimensionType.id"
                                >
                                    {{dimensionType.name}}
                                </option>
                            </b-select>
                        </b-field>
                    </b-field>
                </b-section>
        </b-section>
    </b-section>
</template>

<script>

/**
 * Represents the single value dimension type
 */
const SINGLE="Single";

/**
 * Represents the continuous value dimension type
 */
const CONTINUOUS="Continuous";

/**
 * Represents the discrete value dimension type
 */
const DISCRETE="Discrete";

/**
 * Represents all available dimension types
 */
const availableDimensionTypes=[
    {
        id:1,
        name:SINGLE
    },
    {
        id:2,
        name:CONTINUOUS
    },
    {
        id:3,
        name:DISCRETE
    }
];

export default {
    /**
     * Internal component data
     */
    data(){
        return {
            dimension:{
                selected:1,
                single:{
                    available:true,
                    value:Number
                },
                continuous:{
                    available:false,
                    minValue:Number,
                    maxValue:Number,
                    increment:Number
                },
                discrete:{
                    available:false,
                    selected:0,
                    values:[]
                },
                availableDimensionTypes:availableDimensionTypes
            },
            availableDimensionTypes
        }
    },
    methods:{
        /**
         * Changes the current dimension type
         */
        changeDimensionType(){
            switch(this.availableDimensionTypes[this.dimension.selected]){
                case SINGLE:
                    this.toggleDimensionSingle();
                    break;
                case CONTINUOUS:
                    this.toggleDimensionContinuous();
                    break;
                case DISCRETE:
                    this.toggleDimensionDiscrete();
                    break;
            }
        },
        /**
         * Toggles the single dimension type
         */
        toggleDimensionSingle(){
            this.dimension.single.available=true;
            this.dimension.continuous.available=false;
            this.dimension.discrete.available=false;
        },
        /**
         * Toggles the continuous dimension type
         */
        toggleDimensionContinuous(){
            this.dimension.single.available=false;
            this.dimension.continuous.available=true;
            this.dimension.discrete.available=false;
        },
        /**
         * Toggles the discrete dimension type
         */
        toggleDimensionDiscrete(){
            this.dimension.single.available=false;
            this.dimension.continuous.available=false;
            this.dimension.discrete.available=true;
        }
    },
    /**
     * Received values from father component
     */
    props:{
        dimensionLabel:String
    }
}
</script>
