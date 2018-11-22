<template>
    <section>
        <b-field :label="slotName"/>
        <b-field>
            <b-field label="Width">
                <b-input
                    type="Number"
                    :value.sync="minSlotSize"
                    placeholder="200"
                    icon="pound"
                    required>
                </b-input>
            </b-field>
            <b-field label="Height">
                <b-input
                    type="Number"
                    :value.sync="minSlotSize"
                    placeholder="200"
                    icon="pound"
                    required>
                </b-input>
            </b-field>
            <b-field label="Depth">
                <b-input
                    type="Number"
                    :value.sync="minSlotSize"
                    placeholder="200"
                    icon="pound"
                    maxlength="1"
                    required>
                </b-input>
            </b-field>
        </b-field>
    </section>
</template>

<script>
export default {
    props:{
        slotName:{
            type:String,
            required:true
        }
    }
};
</script>
