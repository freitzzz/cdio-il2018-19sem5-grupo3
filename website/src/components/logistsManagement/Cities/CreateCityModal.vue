<template>
    <div class="modal-card" style="width:auto">
        <header class="modal-card-head">
            <p class="modal-card-title">New City</p>
        </header>
        <section class="modal-card-body">
            <!-- Name -->
            <b-field label="Name">
                <b-input
                type="String"
                v-model="nameItem.value"
                :placeholder="placeholders.name"
                icon="format-quote-open"
                required>
                </b-input>
            </b-field>
            <!-- Latitude -->
            <b-field>
                <b-input
                type="number"
                v-model="latitudeItem.value"
                :placeholder="placeholders.latitude"
                icon="crosshairs"
                required>
                </b-input>
            </b-field>
            <!-- Longitude -->
            <b-field>
                <b-input
                type="number"
                v-model="longitudeItem.value"
                :placeholder="placeholders.longitude"
                icon="crosshairs-gps"
                required>
                </b-input>
            </b-field>
        </section>
        <footer class="moda-card-foot">
            <div class="has-text-centered">
                <button class="btn-primary" @click="emitCity($parent)">Create</button>
            </div>
        </footer>
    </div>
</template>

<script>
    export default {
        /**
         * Component data
         */
        data() {
            return {
                nameItem:{
                    value:null
                },
                latitudeItem:{
                    value:null
                },
                longitudeItem:{
                    value:null
                },
                placeholders:{
                    name:"Lisbon",
                    latitude:"123",
                    longitude:"12356"
                }
                
            }
        },
         /**
         * Emits the product to the father component
         */
        emitCity(modal){
            let cityDetails ={
                name: this.nameItem.value,
                latitude:this.latitude.value,
                longitude:this.longitude.value
            };
            this.$emit('emitCity',cityDetails);
        }
    }
</script>

<style>
    
</style>