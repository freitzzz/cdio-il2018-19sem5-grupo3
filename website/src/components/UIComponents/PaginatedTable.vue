<template>
    <section>
    <b-field grouped group-multiline>
            <b-field label="Total">
                <b-input type="number" v-model="total"></b-input>
            </b-field>
            <b-field label="Items per page">
                <b-input type="number" v-model="perPage"></b-input>
            </b-field>
        </b-field>
        <div class="block">
            <b-switch v-if="showSimpleSlider" v-model="isSimple">Simple</b-switch>
            <b-switch v-if="showRoundedSlider" v-model="isRounded">Rounded</b-switch>
        </div>
        <hr>
        <ClickableTable :title="title" :columns="columns" :data="data">

    </ClickableTable>
    <b-pagination
        :total="total"
        :current.sync="current"
        :order="order"
        :size="size"
        :simple="isSimple"
        :rounded="isRounded"
        :per-page="perPage">
    </b-pagination>
    </section>
</template>

<script>

import ClickableTable from './ClickableTable.vue'

export default {
    components:{
        ClickableTable
    },
    props: {
        total: {
            type:Number,
            required:true
        },
        current:{
            type:Number,
            default:1
        },
        perPage:{
            type:Number,
            default:20
        },
        order:{
            type:String,
            default:"is-centered"
        },
        size:{
            type:String,
            default:""
        },
        isSimple:{
            type:Boolean,
            default:false
        },
        isRounded:{
            type:Boolean,
            default:false
        },
        showSimpleSlider:{
            type:Boolean,
            default:false
        },
        showRoundedSlider:{
            type:Boolean,
            default:false
        },
        columns:{
            type:Array
        },
        data:{
            type:Array
        }
    }
};
</script>