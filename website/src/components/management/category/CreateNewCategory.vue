<template>
    <b-modal :active.sync="active" has-modal-card>
        <form action="">
                <div class="modal-card" style="width: auto">
                    <header class="modal-card-head">
                        <p class="modal-card-title">New Category</p>
                    </header>
                    <section class="modal-card-body">
                        <b-field label="Name">
                            <b-input
                                type="String"
                                :value.sync="reference"
                                placeholder="#666"
                                icon="pound"
                                required>
                            </b-input>
                        </b-field>
    
                        <b-field label="Parent Category">
                            <b-select placeholder="Select a category" icon="tag">
                                <optgroup label="Wardrobes">
                                    <option value="flint">Classy</option>
                                    <option value="flint">Devil</option>
                                    <option value="flint">Professional</option>
                                </optgroup>
                            </b-select>
                        </b-field>
                     
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
    name:"CreateNewCategory",
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
