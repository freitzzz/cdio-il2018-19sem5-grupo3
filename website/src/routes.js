import Home from "./components/Home.vue";
import Customizer from "./components/Customizer.vue";
import ManagementTopBar from "./components/ManagementTopBar.vue";
import ListCategories from "./components/management/category/ListCategories.vue";
import ListMaterials from "./components/management/material/ListMaterials.vue";
import ListProducts from "./components/management/product/ListProducts.vue";
import ListCustomizedProductCollections from "./components/management/customizedproductcollections/ListCustomizedProductCollections.vue";
import ListCommercialCatalogues from "./components/management/commercialcatalogue/ListCommercialCatalogues.vue";

export const routes = [
    { path: "/", redirect: "/home" },
    { path: "/home", component: Home },
    { path: "/customization", component: Customizer },
    {
        path: "/management", component: ManagementTopBar, children: [
            { path: "categories", component: ListCategories },
            { path: "materials", component: ListMaterials },
            { path: "products", component: ListProducts },
            { path: "collections", component: ListCustomizedProductCollections },
            { path: "catalogues", component: ListCommercialCatalogues },
            { path: "customization", component: Customizer }
        ]
    },

];