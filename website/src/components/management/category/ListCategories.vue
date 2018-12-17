<template>
    <div>
        <div>
            <button class="btn-primary" @click="createNewCategory()">
                            <b-icon icon="plus"/>
                            </button>
            <div v-if="createNewCategoryModal">
                <b-modal :active.sync="createNewCategoryModal" has-modal-card scroll="keep">
                    <create-new-category @emitCategory="postCategory" />
                </b-modal>
            </div>
            <button class="btn-primary" @click="fetchRequests()">
                                <b-icon icon="refresh" custom-class="fa-spin"/>
                    </button>
        </div>
        <categories-table :data="data" @refreshData="refreshCategories"></categories-table>
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
             * Fetches all parent categories data by their ids
             */
            getParentCategories(parentCategoriesIds) {
                return new Promise((accept, reject) => {
                    let parentCategories = [];
                    let parents = 0;
                    for (let i = 0; i < parentCategoriesIds.length; i++)
                        if (parentCategoriesIds[i]) parents++;
                    for (let i = 0; i < parentCategoriesIds.length; i++) {
                        if (parentCategoriesIds[i]) {
                            this
                                .getParentCategory(parentCategoriesIds[i])
                                .then((parentCategory) => {
                                    parentCategories.push(parentCategory);
                                    parents--;
                                })
                                .catch((error_message) => {
                                    parents--;
                                    rejectIteration(error_message);
                                });
                        }
                    };
                    alert(parents);
                    if (parents == 0) {
                        alert("!!!")
                        accept(parentCategories);
                    }
                });
            },
            /**
             * Fetches the parent category data by his id
             */
            getParentCategory(parentCategoryId) {
                return new Promise((accept, reject) => {
                    Axios
                        .get(MYCM_API_URL + "/categories/" + parentCategoryId)
                        .then((parentcategory) => {
                            accept(parentcategory.data);
                        })
                        .then((error_message) => {
                            reject(error_message.response.data.message);
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
                        let allCategories = _response.data;
                        this.data = this.generateCategoriesTableData(allCategories);
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
                        field: "parentName",
                        label: "Parent Category",
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
                        name: category.name,
                        parentName: category.parentName
                    });
                });
                return categoriesTableData;
            }
    
        }
    }
</script>
