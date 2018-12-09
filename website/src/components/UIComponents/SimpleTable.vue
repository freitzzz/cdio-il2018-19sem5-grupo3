<template>
    <vuetable
        :api-mode="false"
        :data="data"
        :fields="columns"
    >
    </vuetable>
</template>

<script>

export default {
    /**
     * Component properties
     */
    props:{
        columns:[],
        data:[]
    }
}
</script>
