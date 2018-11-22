<template>
    <section>
        <b-field :label="slotName"/>
        <b-field>
            <b-field label="Width">
                <b-input
                    type="Number"
                    :value.sync="width"
                    placeholder="200"
                    icon="wrench"
                    required>
                </b-input>
            </b-field>
            <b-field label="Height">
                <b-input
                    type="Number"
                    :value.sync="height"
                    placeholder="200"
                    icon="wrench"
                    required>
                </b-input>
            </b-field>
            <b-field label="Depth">
                <b-input
                    type="Number"
                    :value.sync="depth"
                    placeholder="200"
                    icon="wrench"
                    maxlength="1"
                    required>
                </b-input>
            </b-field>
        </b-field>
    </section>
</template>

<script>
export default {
    data(){
        return {
            width:{
                type:Number
            },
            height:{
                type:Number
            },
            depth:{
                type:Number
            },
        }
    },
    methods:{
        /**
         * Emits the current slot size values
         */
        getSlotValues(){
            let slotValues={
                width:this.width,
                height:this.height,
                depth:this.depth
            };
            this.$emit('getSlotValues', slotValues);
            
        }
    },
    props:{
        slotName:{
            type:String,
            required:true
        }
    }
};
</script>
