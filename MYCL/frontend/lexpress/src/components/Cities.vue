<template>
    <div class="row">
        <div class="col-md-12">
            <!-- CITIES CARD -->
            <div class="card" style="width:100%;height:100%">
                <ClickableTable :title="table.title" :data="table.data" :columns="table.columns">
                </ClickableTable>
            </div>
        </div>
    </div>
</template>

<script>
    import ClickableTable from './UIComponents/ClickableTable.vue'
    const tableData = [
    {
        name: "Porto",
        latitude: 41.14961,
        longitude: -8.61099
    },
    {
        name: "Braga",
        latitude: 41.55032,
        longitude: -8.42005
    },
    {
        name: "Lisboa",
        latitude: 38.71667,
        longitude: -9.13333
    }
    ];
    const tableColumns=[
        {
            field: 'name',
            label: 'Name',
            width: 40
        },
        {
            field: 'latitude',
            label: 'Latitude',
            numeric: true,
        },
        {
            field: 'longitude',
            label: 'Longitude',
            numeric: true,
        },
    ];
export default {
    name: 'Cities',
    components: {
        ClickableTable
    },
  data() {
    return {
      table: {
        title: "Cities",
        columns: [...tableColumns],
        data: [...tableData]
      }
    };
  } 
}
</script>
