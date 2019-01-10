<template>
    <div>
        <div>
            <button class="btn-primary" @click="createNewCity()">
                            <b-icon icon="plus"/>
                        </button>
            <div v-if="createNewCityModal">
                <b-modal :active.sync="createNewCityModal" has-modal-card scroll="keep">
                </b-modal>
            </div>
            <button class="btn-primary" @click="fetchRequests()">
                            <b-icon 
                                icon="refresh"
                                custom-class="fa-spin"/>
            </button>
        </div>
        <cities-table :data.sync="data" @refreshData="refreshCities"></cities-table>
    </div>
</template>

<script>
    import CitiesTable from "./CitiesTable.vue";
    import CityRequests from "../../../services/myco_api/requests/cities.js"
    export default {
        name: "ListCities",
        data() {
            return {
                createNewCityModal: false,
                columns: [],
                data: Array,
                total: Number
            }
        },
        components: {
            CitiesTable
        },
        methods: {
            fetchRequests(){
                this.refreshCities();
            },
            /* Triggers creation of new City */
            createNewCity() {
                this.createNewCityModal = true;
            },
            fetchCities() {
                CityRequests.getCities()
                    .then(response => {
                        this.data = response.data;
                        console.log(this.data)
                    })
                    .catch(error => {
                        this.$toast.open("There was an error trying to get all the cities.");
                    });
            },
            refreshCities() {
                this.columns = this.generateCitiesTableColumns();
                this.data = [];
                this.total = this.data.length;
                CityRequests.getCities()
                    .then(response => {
                        this.data = this.generateCityTableData(response.data);
                        this.columns = this.generateCitiesTableColumns();
                        this.total = this.data.length
                    })
                    .catch(error => {
                        this.$toast.open("There was an error trying to get all the cities.");
                    });
            },
            /**
             * Generates the data of a cities table by a given list of cities
             */
            generateCityTableData(cities) {
                let citiesTableData = [];
                cities.forEach((city) => {
                    citiesTableData.push({
                        id:city.id,
                        name: city.name,
                        latitude: city.location.latitude,
                        longitude: city.location.longitude,
                    })
                })
                return citiesTableData;
            },
            /* 
             * Generates the needed columns for the cities table
             */
            generateCitiesTableColumns() {
                return [{
                        field: "id",
                        label: "City ID",
                        centered: true
                    },
                    {
                        field: "name",
                        label: "Name",
                        centered: true
                    },
                    {
                        field: "latitude",
                        label: "Latitude",
                        centered: true
                    },
                    {
                        field: "longitude",
                        label: "Longitude",
                        centered: true
                    },
                ];
            }
        },
        created() {
            this.fetchRequests();
        }
    }
</script>

<style>
    
</style>