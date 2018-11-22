<template>
    <b-section>
        <b-field label="Width"/>
        <b-section>
                <b-section v-if="widthDimension.single">
                    <b-field>
                        <b-field label="Dimension Value">
                            <b-input
                                type="Number"
                                :value.sync="width"
                                placeholder="200"
                                icon="wrench"
                                expanded=true
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Dimension Type">
                            <b-select v-model="widthDimension.selected" @input="changeWidthDimensionType">
                                <option 
                                        v-for="(dimensionType,index) in widthDimension.availableDimensionTypes" 
                                        :key="index"
                                        :value="dimensionType.id"
                                >
                                    {{dimensionType.name}}
                                </option>
                            </b-select>
                        </b-field>
                    </b-field>
                </b-section>
                <b-section v-if="widthDimension.continuous">
                    <b-field>
                        <b-field label="Dimension Value" >
                            <b-input
                                type="Number"
                                :value.sync="width"
                                placeholder="200"
                                icon="wrench"
                                expanded=true
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Values">
                            <b-select/>
                        </b-field>
                            <b-field label="Add">
                                <button class="button is-danger" @click="addContinuousWidthValue()">
                                    <b-icon icon="plus"/>
                                </button>
                            </b-field>
                            <b-field label="Remove">
                                <button class="button is-danger" @click="removeContinuousWidthValue()">
                                    <b-icon icon="minus"/>
                                </button>
                            </b-field>
                        <b-field label="Dimension Type">
                            <b-select v-model="widthDimension.selected" @input="changeWidthDimensionType">
                                <option 
                                        v-for="(dimensionType,index) in widthDimension.availableDimensionTypes" 
                                        :key="index"
                                        :value="dimensionType.id"
                                >
                                    {{dimensionType.name}}
                                </option>
                            </b-select>
                        </b-field>
                    </b-field>
                </b-section>
                <b-section v-if="widthDimension.discrete">
                    <b-field>
                        <b-field label="Min Value">
                            <b-input
                                type="Number"
                                :value.sync="width"
                                placeholder="200"
                                icon="wrench"
                                expanded=true
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Max Value">
                            <b-input
                                type="Number"
                                :value.sync="width"
                                placeholder="200"
                                icon="wrench"
                                expanded=true
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Increment">
                            <b-input
                                type="Number"
                                :value.sync="width"
                                placeholder="200"
                                icon="wrench"
                                expanded=true
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Dimension Type">
                            <b-select v-model="widthDimension.selected" @input="changeWidthDimensionType">
                                <option 
                                        v-for="(dimensionType,index) in widthDimension.availableDimensionTypes" 
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
        <b-field label="Height"/>
        <b-field label="Depth"/>
    </b-section>
</template>

<script>

/**
 * Represents all available dimension types
 */
const availableDimensionTypes=[
    {
        id:1,
        name:"Single"
    },
    {
        id:2,
        name:"Continuous"
    },
    {
        id:3,
        name:"Discrete"
    }
];

export default {
    data(){
        return {
            widthDimension:{
                selected:1,
                single:true,
                continuous:false,
                discrete:false,
                availableDimensionTypes:availableDimensionTypes
            },
            availableDimensionTypes
        }
    },
    methods:{
        changeWidthDimensionType(){
            switch(this.widthDimension.selected){
                case 1:
                    this.toggleWidthDimensionSingle();
                    break;
                case 2:
                    this.toggleWidthDimensionContinuous();
                    break;
                case 3:
                    this.toggleWidthDimensionDiscrete();
                    break;
            }
        },
        toggleWidthDimensionSingle(){
            this.widthDimension.single=true;
            this.widthDimension.continuous=false;
            this.widthDimension.discrete=false;
        },
        toggleWidthDimensionContinuous(){
            this.widthDimension.single=false;
            this.widthDimension.continuous=true;
            this.widthDimension.discrete=false;
        },
        toggleWidthDimensionDiscrete(){
            this.widthDimension.single=false;
            this.widthDimension.continuous=false;
            this.widthDimension.discrete=true;
        },
    }
}
</script>
