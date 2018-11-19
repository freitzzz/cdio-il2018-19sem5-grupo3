<template>
    <b-modal :active.sync="active" has-modal-card>
                <div class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">New Product</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Reference">
                            <b-input
                                type="String"
                                :value.sync="reference"
                                placeholder="#666"
                                icon="pound"
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Designation">
                            <b-input
                                type="String"
                                :value.sync="designation"
                                placeholder="Devil Wardrobe"
                                icon="pencil"
                                required>
                            </b-input>
                        </b-field>
                        <b-field label="Category">
                            <b-select placeholder="Select a category" icon="tag">
                                <optgroup label="Wardrobes">
                                    <option value="flint">Classy</option>
                                    <option value="flint">Devil</option>
                                    <option value="flint">Professional</option>
                                </optgroup>
                                <optgroup label="Shelves">
                                    <option value="flint">For the Kids</option>
                                    <option value="flint">KKB</option>
                                </optgroup>
                            </b-select>
                        </b-field>
                        <b-field label="Materials">
                            <b-select placeholder="MDF" icon="brush">
                                <option value="flint">MDF</option>
                                <option value="flint">Cherry</option>
                                <option value="flint">Orange</option>
                            </b-select>
                            <div class="has-text-centered">
                                <button class="button is-danger" @click="showMaterials">
                                    <b-icon icon="magnify"/>
                                </button>
                            </div>
                        </b-field>
                        <b-checkbox @input="enableComponents()">Components</b-checkbox>
                        <div v-if="components">
                            <b-field label="Components">
                                <b-select placeholder="Kelf" icon="archive">
                                    <option value="flint">MDF</option>
                                </b-select>
                                <div class="has-text-centered">
                                    <button class="button is-danger" @click="showMaterials">
                                        <b-icon icon="magnify"/>
                                    </button>
                                </div>
                            </b-field>
                        </div>
                    </section>
                    <footer class="modal-card-foot">
                        <div class="has-text-centered">
                            <button class="button is-primary" @click="emitProduct($parent)">Create</button>
                        </div>
                    </footer>
                </div>
   
    </b-modal>
</template>

<script>
export default {
    props:{
        active:{
            type: Boolean,
            default: false
        },
        reference:String,
        designation:String,
        category:String,
    },
    data(){
        return{
            components:false
        }
    },
    methods:{
        emitProduct(modal){
            //modal.close();
            this.$emit('emitProduct',{});
        },
        enableComponents(){
            this.components=!this.components;
        },
        showMaterials(){
            //TODO: Open modal with all available materials
        },
        showComponents(){
            //TODO: Open modal with all available components
        }
    }
}
</script>
