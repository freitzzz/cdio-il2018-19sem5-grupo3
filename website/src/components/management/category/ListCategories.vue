<template>
    <div>
        <div>
            <button class="button is-danger" @click="createNewCategory()">
                <b-icon icon="plus"/>
                </button>
            <div v-if="createNewCategoryModal">
                <b-modal has-modal-card scroll="keep">
                    <create-new-category
                    @emitCategory="postCategory"
                    />
    
                </b-modal>
            </div>
            <button class="button is-danger" @click="fetchRequests()">
                    <b-icon icon="refresh" custom-class="fa-spin"/>
            </button>
        </div>
        <categories-table :data="data"></categories-table>
    </div>
</template>

<script>
    import CreateNewCategory from './CreateNewCategory.vue';
    import CategoriesTable from './CategoriesTable.vue';
    import Axios from 'axios';
    import Config, {
        MYCM_API_URL
    } from '../../../config.js'
    import CategoriesTableVue from './CategoriesTable.vue';
    
    
    export default {
        components: {
            CreateNewCategory,
            CategoriesTable
        },
        created() {
            this.fetchRequests();
        },
        data() {
            return {
                createNewCategoryModal: false,
                updateCategoryModal: false,
                category: null,
                categoryClone: null,
                currentSelectedCategory: 0,
                availableCategories: Array,
                columns: [],
                data: Array,
                total: Number,
                failedToFetchCategoryNotification: false
            }
        },
        methods: {
            /**
             * Changes the current selected Category
             */
            changeSelectedCategory(tableRow) {
                this.currentSelectedCategory = tableRow.id;
            },
            /**
             * Function that is called whe the component is created
             */
            createNewCategory() {
                this.createNewCategoryModal = true;
            },
            /**
             * Post new Category
             * TODO:POST OF SUBCATEGORY
             */
            postCategory(categoryDetails) {
                let newCategory = {};
                if (categoryDetails.name != null) {
                    newCategory.name = categoryDetails.name;
                }
                Axios
                    .post(MYCM_API_URL + '/categories', newCategory)
                    .then((response) => {
                        this.$toast.open({
                            message: "The category was created with success!"
                        });
                        this.createNewCategoryModal = false;
                        this.fetchRequests();
                    })
                    .catch((error_message) => {
                        this.$toast.open({
                            message: error_message.response.data.message
                        });
                    });
    
            },
            /**
             * fetches all possible requests
             */
            fetchRequests() {
                this.refreshCategories();
            },
            /**
             * Fetches all available categories
             */
            refreshCategories() {
                Axios.get(MYCM_API_URL + '/categories')
                    .then((_response) => {
                        this.data = this.generateCategoriesTableData(_response.data);
                        this.columns = this.generateCategoriesTableColumns();
                        this.total = this.data.length;
                    })
                    .catch((error_message) => {
                        this.$toast.open({
                            message: error_message.response.data.message
                        });
                    });
            },
            /**
             * Generates the needed columns for the Categories table
             */
            generateCategoriesTableColumns() {
                return [{
                        field: "id",
                        label: "ID",
                        centered: true
                    },
                    {
                        field: "name",
                        label: "Name",
                        centered: true
                    },
                    {
                        field: "actions",
                        label: "Action",
                        centered: true
                    }
                ];
            },
            /**
             * Generates the data of a category table by a given list of categories
             */
            generateCategoriesTableData(categories) {
                let categoriesTableData = [];
                categories.forEach((category) => {
                    categoriesTableData.push({
                        id: category.id,
                        name: category.name
                    });
                });
                return categoriesTableData;
            }
    
        }
    }
</script>
