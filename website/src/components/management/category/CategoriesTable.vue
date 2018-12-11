<template>
    <vuetable :api-mode="false" :data="this.data" :fields="columns">
        <template slot="actions" slot-scope="props">
                <div class="custom-actions">
                     <button
                        class="button is-danger"
                        @click="openCategoryDetails(props.rowData.id)">
                        <b-icon icon="magnify"/>
                    </button>
                    <button
                        class="button is-danger"
                        @click="editCategoryDetails(props.rowData.id)">
                        <b-icon icon="pencil"/>
                    </button>
                    <button
                        class="button is-danger"
                        @click="deleteCategory(props.rowData.id)">
                        <b-icon icon="minus"/>
                    </button>
                </div>
                <div v-if="showCategoryDetails">
                    <b-modal :active.sync="showCategoryDetails" has-modal-card scroll="keep">
                        <category-details
                            :category="currentSelectedCategory"
                        />
                    </b-modal>
                </div>
                <div v-if="showEditCategoryDetails">
                    <b-modal :active.sync="showEditCategoryDetails" has-modal-card scroll="keep">
                        <edit-category
                            @emitCategory="updateCategory"
                            :category="currentSelectedCategory2"
                        />
                    </b-modal>
                </div>
</template>
    </vuetable>
</template>

<script>
    /**
     * Require Axios for MYCM Categories API requests
     */
    import Axios from 'axios';
    
    /**
     * Requires CategoryDetails modal for category details
     */
    import CategoryDetails from './CategoryDetails.vue';
    
    /**
     * Requires EditCategory modal for category edition
     */
    import EditCategory from './EditCategory.vue';
    
    /**
     * Requires App Configuration for accessing MYCM API URL
     */
    import Config, {MYCM_API_URL} from '../../../config.js';
    
    export default {
        /**
         * Components exported components
         */
        components: {
            EditCategory,
            CategoryDetails
        },
        /**
         * Category data
         */
        data() {
            return {
                currentSelectedCategory: null,
                currentSelectedCategory2: null,
                columns: [{
                        name: "id",
                        title: "ID"
                    },
                    {
                        name: "name",
                        title: "Name"
                    },
                    {
                        name: "__slot:actions", // <----
                        title: "Actions",
                        titleClass: "center aligned",
                        dataClass: "center aligned"
                    }
                ],
                showEditCategoryDetails: false,
                showCategoryDetails: false
            }
        },
        methods: {
            /**
             * Transforms a boolean value into a icon
             */
            booleansAsIcons(value) {
                return value ?
                    '<span class="ui teal label"><i class="material-icons">check</i></span>' :
                    '<span class="ui teal label"><i class="material-icons">close</i></span>';
            },
            /**
             * Deletes a given category
             */
            deleteCategory(categoryId) {
                Axios
                    .delete(MYCM_API_URL + '/categories/' + categoryId)
                    .then(() => {
                        this.$emit('refreshData');
                        this.$toast.open({
                            message: "Category was deleted with success!"
                        });
                    })
                    .catch((error_message) => {
                        this.$toast.open({
                            message: error_message.data.message
                        });
                    });
            },
            /**
             * Edit details of a category
             */
            editCategoryDetails(categoryId) {
                this.getCategoryDetails(categoryId);
            },
            /**
             * Fetches the details of a certain category 
             */
            getCategoryDetails(categoriesId) {
                return new Promise((accept, reject) => {
                    Axios.get(MYCM_API_URL + '/categories/' + categoriesId)
                        .then((category) => {
                            this.currentSelectedCategory = category.data;
                            this.currentSelectedCategory2 = Object.assign({}, this.currentSelectedCategory);
                        })
                        .catch((error_message) => {
                            this.$toast.open({
                                message: error_message
                            });
                            reject();
                        });
                });
            },
            /**
             * Opens a modal with the category details
             */
            openCategoryDetails(categoryId) {
                this
                    .getCategoryDetails(categoryId)
                    .then((category) => {
                        this.showCategoryDetails = true;
                    });
            },
            /**
             * Updates a given category
             */
            updateCategory(categoryDetails) {
                this.updateCategoryProperties(categoryDetails)
                    .catch((error_message) => {
                        this.$toast.open({
                            message: error_message
                        });
                    });
            },
            /**
             * UPdates given category (PUT)
             */
            updateCategoryProperties(categoryDetails) {
                let categoryPropertiesToUpdate = {};
                if (catgoryDetails.name != null) {
                    categoryPropertiesToUpdate.name = categoryDetails.name;
                }
    
                return new Promise((accept, reject) => {
                    Axios
                        .put(MYCM_API_URL + '/categories/' + categoryDetails.id, categoryPropertiesToUpdate)
                        .then((category) => {
                            accept(category);
                        })
                        .catch((error_message) => {
                            reject(error_message.data.message);
                        })
                })
            }
        },
         
        props: {
            data: []
        }
     
    }
</script>
