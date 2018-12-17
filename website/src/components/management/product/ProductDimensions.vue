<template>
    <b-section>
        <b-field :label="dimensionLabel"/>
        <b-section>
                <b-section v-if="dimension.single.available">
                    <b-field>
                        <b-field label="Dimension Value">
                            <b-input
                                type="Number"
                                v-model="dimension.single.value"
                                placeholder="200"
                                icon="wrench"
                                required
                                @input="emitDimension"
                                expanded
                            >
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
                        <b-field label="Unit">
                            <b-select v-model="unit.selected" @input="changeDimensionUnit" >
                                <option 
                                        v-for="(dimensionUnit,index) in availableUnits" 
                                        :key="index"
                                        :value="dimensionUnit.id"
                                >
                                    {{dimensionUnit.unit}}
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
                                v-model="dimension.discrete.value"
                                placeholder="200"
                                icon="wrench"
                                required
                                @input="emitDimension"
                                expanded    
                            >
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
                        <b-field label="Unit">
                            <b-select v-model="unit.selected">
                                <option 
                                        v-for="(dimensionUnit,index) in availableUnits" 
                                        :key="index"
                                        :value="dimensionUnit.id"
                                >
                                    {{dimensionUnit.unit}}
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
                                v-model="dimension.continuous.minValue"
                                placeholder="200"
                                icon="wrench"
                                expanded=true
                                required
                                @input="emitDimension">
                            </b-input>
                        </b-field>
                        <b-field label="Max Value">
                            <b-input
                                type="Number"
                                v-model="dimension.continuous.maxValue"
                                placeholder="200"
                                icon="wrench"
                                expanded=true
                                required
                                @input="emitDimension">
                            </b-input>
                        </b-field>
                        <b-field label="Increment">
                            <b-input
                                type="Number"
                                v-model="dimension.continuous.increment"
                                placeholder="200"
                                icon="wrench"
                                expanded=true
                                required
                                @input="emitDimension">
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
                        <b-field label="Unit">
                            <b-select v-model="unit.selected">
                                <option 
                                        v-for="(dimensionUnit,index) in availableUnits" 
                                        :key="index"
                                        :value="dimensionUnit.id"
                                >
                                    {{dimensionUnit.unit}}
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
     * Component Created State call
     */
    created(){
        for(let i=0;i<this.availableUnits.length;i++)
            this.availableUnits[i].id=i+1;
        let currentDimension=this.currentDimension;
        if(currentDimension!=null){
            if(currentDimension.value!=null){
                this.dimension.selected=1;
                this.dimension.single.value=currentDimension.value;
                this.changeDimensionType();
            }else if(currentDimension.minValue!=null){
                this.dimension.selected=2;
                this.dimension.continuous.minValue=currentDimension.minValue;
                this.dimension.continuous.maxValue=currentDimension.maxValue;
                this.dimension.continuous.increment=currentDimension.increment;
                this.changeDimensionType();
            }else{
                this.dimension.selected=3;
                this.dimension.discrete.selected=currentDimension.values[0];
                this.dimension.discrete.value=currentDimension.values[0];
                this.dimension.discrete.values=currentDimension.values;
                this.changeDimensionType();
            }
        }
    },
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
                    value:Number,
                    values:[]
                },
                availableDimensionTypes:availableDimensionTypes
            },
            unit:{
                selected:1,
                value:String
            },
            availableDimensionTypes
        }
    },
    methods:{
        /**
         * Adds a new discrete value to the discrete values list
         */
        addDiscreteValue(){
            this.dimension.discrete.values.push(this.dimension.discrete.value);
        },
        /**
         * Removes a discrete value from the discrete values list
         */
        removeDiscreteValue(){
            let newDiscreteValues=[];
            this.dimension.discrete.values.forEach((value)=>{
                if(value!=this.dimension.discrete.selected)
                    newDiscreteValues.push(value);
            });
            this.dimension.discrete.values=newDiscreteValues.slice();
        },
        /**
         * Returns the current dimension values
         */
        getCurrentDimension(){
            this.changeDimensionUnit();
            if(this.dimension.single.available){
                return {
                    type:SINGLE,
                    value:this.dimension.single.value,
                    unit:this.unit.value
                };
            }else if(this.dimension.continuous.available){
                return {
                    type:CONTINUOUS,
                    minValue:this.dimension.continuous.minValue,
                    maxValue:this.dimension.continuous.maxValue,
                    increment:this.dimension.continuous.increment,
                    unit:this.unit.value
                };
            }else{
                return {
                    type:DISCRETE,
                    values:this.dimension.discrete.values,
                    unit:this.unit.value
                };
            }
        },
        emitDimension(){
            this.$emit('getDimension',this.getCurrentDimension());
        },
        /**
         * Changes the current dimension type
         */
        changeDimensionType(){
            switch(this.dimension.availableDimensionTypes[this.dimension.selected-1].name){
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
         * Changes the current dimension unit
         */
        changeDimensionUnit(){
            this.unit.value=this.availableUnits[this.unit.selected-1].unit;
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
        availableUnits:Array,
        dimensionLabel:String,
        currentDimension:Object
    }
}
</script>
