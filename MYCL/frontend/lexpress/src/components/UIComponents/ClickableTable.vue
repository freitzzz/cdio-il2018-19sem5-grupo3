<template>
    <div>
        <div class="header">
            <slot name="header">
                <h4 class="title">{{title}}</h4>
            </slot>
        </div>
        <div class="content table-responsive table-full-width">
            <b-table
                :columns="columns"
                :data="data"
                >
            </b-table>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        columns: Array,
        data: Array,
        title: String
    }
}
</script>
