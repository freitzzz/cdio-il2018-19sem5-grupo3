<template>
    <div class="modal-card" style="width:auto">
        <header class="modal-card-head">
            <p class="modal-card-title">New City</p>
        </header>
        <section class="modal-card-body">
            <!-- Name -->
            <b-field label="Name">
                <b-input type="String" v-model="nameItem.value" :placeholder="placeholders.name" icon="format-quote-open" required>
                </b-input>
            </b-field>
            <!-- Latitude -->
            <b-field>
                <b-input step="any" type="text" pattern="[0-9]+" v-model="latitudeItem.value" :placeholder="placeholders.latitude" icon="crosshairs" required>
                </b-input>
            </b-field>
            <!-- Longitude -->
            <b-field>
                <b-input step="any" type="text" pattern="[0-9]+" v-model="longitudeItem.value" :placeholder="placeholders.longitude" icon="crosshairs-gps" required>
                </b-input>
            </b-field>
        </section>
        <footer class="modal-card-foot">
            <div class="has-text-centered">
                <button class="btn-primary" @click="this.postCity">Create</button>
            </div>
        </footer>
    </div>
</template>

<script>
    import CityRequests from "../../../services/myco_api/requests/cities.js"
    
    export default {
        /**
         * Component data
         */
        data() {
            return {
                nameItem: {
                    value: null
                },
                latitudeItem: {
                    value: null
                },
                longitudeItem: {
                    value: null
                },
                placeholders: {
                    name: "Lisbon",
                    latitude: "123",
                    longitude: "12356"
                }
    
            }
        },
        methods: {
            /**
             * Emits the product to the father component
             */
            postCity() {
            
                var cityDetails = {
                    name: this.nameItem.value,
                    latitude: this.latitudeItem.value,
                    longitude: this.longitudeItem.value
                };
                CityRequests.createCity(cityDetails)
                    .then((response) => {
                        alert("teste");
                        this.$toast.open("The product was created with success!");
                    })
                    .catch((error_message) => {
                        this.$toast.open("There was an error creating the city.");
                    });
    
            }
        }
    }
</script>

<style>
    
</style>