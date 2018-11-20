<template>
    <div>
        <div class="header">
            <slot name="header">
                <h4 class="title">{{header}}</h4>
            </slot>
        </div>
        <b-select :placeholder="defaultItem">
            <option
                v-for="option in items"
                :value="option.id"
                :key="option.id">
                {{ option }}
            </option>
        </b-select>
    </div>
</template>

<script>
export default {
    props:{
        header:String,
        defaultItem:String,
        items:Array
    },
}
</script>