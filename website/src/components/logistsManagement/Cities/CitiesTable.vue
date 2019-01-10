<template>
    <section>
        <vuetable :api-mode="false" :data="data" :fields="columns">
            <!-- <template slot="actions" slot-scope="props">
                                    
          <div class ="custom-actions">
              <button
              class ="btn-primary"
              @click ="deleteCity(props.rowData.id)">
              <b-icon icon="minus"></b-icon>
              </button>
          </div>
</template>-->
        </vuetable>
        <b-modal>

        </b-modal>
    </section>
</template>

<script>
    export default {
        name: "CitiesTable",
        data() {
            return {
                columns: [{
                        name: "id",
                        title: "City ID"
                    },
                    {
                        name: "name",
                        title: "Name"
                    },
                    {
                        name: "longitude",
                        title: "Longitude"
                    },
                    {
                        name: "latitude",
                        title: "Latitude"
                    }
                ],
            }
        },
        /**
         * Component properties
         */
        props: {
            data: Array
        }
    }
</script>

<style>
    
</style>