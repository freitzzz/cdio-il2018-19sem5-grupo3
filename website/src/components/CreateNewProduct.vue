<template>
    <b-modal :active.sync="active" has-modal-card>
        <form action="">
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
                            <b-input
                                type="String"
                                :value.sync="name"
                                placeholder="Select"
                                icon="brush"
                                required>
                            </b-input>
                        </b-field>
                        <b-checkbox>Components</b-checkbox>
                    </section>
                    <footer class="modal-card-foot">
                        <button class="button is-primary" @click="create($parent)">Create</button>
                    </footer>
                </div>
            </form>
    </b-modal>
</template>

<script>
export default {
    props:{
        active:{
            type: Boolean,
            default: false
        }
    },
    methods:{
    }
}
</script>
