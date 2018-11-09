using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommercialCatalogue",
                columns: table => new
                {
                    activated = table.Column<bool>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    reference = table.Column<string>(nullable: true),
                    designation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommercialCatalogue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomizedProductCollection",
                columns: table => new
                {
                    activated = table.Column<bool>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizedProductCollection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dimension",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Discriminator = table.Column<string>(nullable: false),
                    minValue = table.Column<double>(nullable: true),
                    maxValue = table.Column<double>(nullable: true),
                    increment = table.Column<double>(nullable: true),
                    value = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dimension", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    activated = table.Column<bool>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    reference = table.Column<string>(nullable: true),
                    designation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    activated = table.Column<bool>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true),
                    parentId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategory_ProductCategory_parentId",
                        column: x => x.parentId,
                        principalTable: "ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimePeriod",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    startingDate = table.Column<DateTime>(nullable: false),
                    endingDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimePeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogueCollection",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    customizedProductCollectionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogueCollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogueCollection_CustomizedProductCollection_customizedProductCollectionId",
                        column: x => x.customizedProductCollectionId,
                        principalTable: "CustomizedProductCollection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoubleValue",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    value = table.Column<double>(nullable: false),
                    DiscreteDimensionIntervalId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoubleValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoubleValue_Dimension_DiscreteDimensionIntervalId",
                        column: x => x.DiscreteDimensionIntervalId,
                        principalTable: "Dimension",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Measurement",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    heightId = table.Column<long>(nullable: true),
                    widthId = table.Column<long>(nullable: true),
                    depthId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Measurement_Dimension_depthId",
                        column: x => x.depthId,
                        principalTable: "Dimension",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Measurement_Dimension_heightId",
                        column: x => x.heightId,
                        principalTable: "Dimension",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Measurement_Dimension_widthId",
                        column: x => x.widthId,
                        principalTable: "Dimension",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Red = table.Column<byte>(nullable: false),
                    Green = table.Column<byte>(nullable: false),
                    Blue = table.Column<byte>(nullable: false),
                    Alpha = table.Column<byte>(nullable: false),
                    MaterialId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Color_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Finish",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    description = table.Column<string>(nullable: true),
                    MaterialId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finish", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Finish_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    activated = table.Column<bool>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    reference = table.Column<string>(nullable: true),
                    designation = table.Column<string>(nullable: true),
                    productCategoryId = table.Column<long>(nullable: true),
                    maxSlotSize_height = table.Column<double>(nullable: false),
                    maxSlotSize_width = table.Column<double>(nullable: false),
                    maxSlotSize_depth = table.Column<double>(nullable: false),
                    minSlotSize_height = table.Column<double>(nullable: false),
                    minSlotSize_width = table.Column<double>(nullable: false),
                    minSlotSize_depth = table.Column<double>(nullable: false),
                    recommendedSlotSize_height = table.Column<double>(nullable: false),
                    recommendedSlotSize_width = table.Column<double>(nullable: false),
                    recommendedSlotSize_depth = table.Column<double>(nullable: false),
                    supportsSlots = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_ProductCategory_productCategoryId",
                        column: x => x.productCategoryId,
                        principalTable: "ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialPriceTable",
                columns: table => new
                {
                    activated = table.Column<bool>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    price_value = table.Column<double>(nullable: false),
                    timePeriodId = table.Column<long>(nullable: true),
                    eId = table.Column<string>(nullable: true),
                    entityId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialPriceTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialPriceTable_Material_entityId",
                        column: x => x.entityId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialPriceTable_TimePeriod_timePeriodId",
                        column: x => x.timePeriodId,
                        principalTable: "TimePeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommercialCatalogueCatalogueCollection",
                columns: table => new
                {
                    commercialCatalogueId = table.Column<long>(nullable: false),
                    catalogueCollectionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommercialCatalogueCatalogueCollection", x => new { x.commercialCatalogueId, x.catalogueCollectionId });
                    table.ForeignKey(
                        name: "FK_CommercialCatalogueCatalogueCollection_CatalogueCollection_catalogueCollectionId",
                        column: x => x.catalogueCollectionId,
                        principalTable: "CatalogueCollection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommercialCatalogueCatalogueCollection_CommercialCatalogue_commercialCatalogueId",
                        column: x => x.commercialCatalogueId,
                        principalTable: "CommercialCatalogue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomizedMaterial",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    materialId = table.Column<long>(nullable: true),
                    colorId = table.Column<long>(nullable: true),
                    finishId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizedMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomizedMaterial_Color_colorId",
                        column: x => x.colorId,
                        principalTable: "Color",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomizedMaterial_Finish_finishId",
                        column: x => x.finishId,
                        principalTable: "Finish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomizedMaterial_Material_materialId",
                        column: x => x.materialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinishPriceTable",
                columns: table => new
                {
                    activated = table.Column<bool>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    price_value = table.Column<double>(nullable: false),
                    timePeriodId = table.Column<long>(nullable: true),
                    eId = table.Column<string>(nullable: true),
                    entityId = table.Column<long>(nullable: true),
                    materialEID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinishPriceTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinishPriceTable_Finish_entityId",
                        column: x => x.entityId,
                        principalTable: "Finish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinishPriceTable_TimePeriod_timePeriodId",
                        column: x => x.timePeriodId,
                        principalTable: "TimePeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    fatherProductId = table.Column<long>(nullable: false),
                    mandatory = table.Column<bool>(nullable: false),
                    complementedProductId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => new { x.fatherProductId, x.complementedProductId });
                    table.ForeignKey(
                        name: "FK_Component_Product_complementedProductId",
                        column: x => x.complementedProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Component_Product_fatherProductId",
                        column: x => x.fatherProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductMaterial",
                columns: table => new
                {
                    materialId = table.Column<long>(nullable: false),
                    productId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMaterial", x => new { x.productId, x.materialId });
                    table.ForeignKey(
                        name: "FK_ProductMaterial_Material_materialId",
                        column: x => x.materialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductMaterial_Product_productId",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductMeasurement",
                columns: table => new
                {
                    productId = table.Column<long>(nullable: false),
                    measurementId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMeasurement", x => new { x.productId, x.measurementId });
                    table.ForeignKey(
                        name: "FK_ProductMeasurement_Measurement_measurementId",
                        column: x => x.measurementId,
                        principalTable: "Measurement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductMeasurement_Product_productId",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Restriction",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    description = table.Column<string>(nullable: true),
                    algorithm = table.Column<int>(nullable: false),
                    ComponentcomplementedProductId = table.Column<long>(nullable: true),
                    ComponentfatherProductId = table.Column<long>(nullable: true),
                    MeasurementId = table.Column<long>(nullable: true),
                    ProductMaterialmaterialId = table.Column<long>(nullable: true),
                    ProductMaterialproductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restriction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restriction_Measurement_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Restriction_Component_ComponentfatherProductId_ComponentcomplementedProductId",
                        columns: x => new { x.ComponentfatherProductId, x.ComponentcomplementedProductId },
                        principalTable: "Component",
                        principalColumns: new[] { "fatherProductId", "complementedProductId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Restriction_ProductMaterial_ProductMaterialproductId_ProductMaterialmaterialId",
                        columns: x => new { x.ProductMaterialproductId, x.ProductMaterialmaterialId },
                        principalTable: "ProductMaterial",
                        principalColumns: new[] { "productId", "materialId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Input",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true),
                    value = table.Column<string>(nullable: true),
                    RestrictionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Input", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Input_Restriction_RestrictionId",
                        column: x => x.RestrictionId,
                        principalTable: "Restriction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CatalogueCollectionProduct",
                columns: table => new
                {
                    catalogueCollectionId = table.Column<long>(nullable: false),
                    customizedProductId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogueCollectionProduct", x => new { x.catalogueCollectionId, x.customizedProductId });
                    table.ForeignKey(
                        name: "FK_CatalogueCollectionProduct_CatalogueCollection_catalogueCollectionId",
                        column: x => x.catalogueCollectionId,
                        principalTable: "CatalogueCollection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomizedProduct",
                columns: table => new
                {
                    activated = table.Column<bool>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    reference = table.Column<string>(nullable: true),
                    designation = table.Column<string>(nullable: true),
                    customizedMaterialId = table.Column<long>(nullable: true),
                    customizedDimensions_height = table.Column<double>(nullable: false),
                    customizedDimensions_width = table.Column<double>(nullable: false),
                    customizedDimensions_depth = table.Column<double>(nullable: false),
                    productId = table.Column<long>(nullable: true),
                    insertedInSlotId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizedProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomizedProduct_CustomizedMaterial_customizedMaterialId",
                        column: x => x.customizedMaterialId,
                        principalTable: "CustomizedMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomizedProduct_Product_productId",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CollectionProduct",
                columns: table => new
                {
                    customizedProductId = table.Column<long>(nullable: false),
                    customizedProductCollectionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionProduct", x => new { x.customizedProductId, x.customizedProductCollectionId });
                    table.ForeignKey(
                        name: "FK_CollectionProduct_CustomizedProductCollection_customizedProductCollectionId",
                        column: x => x.customizedProductCollectionId,
                        principalTable: "CustomizedProductCollection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionProduct_CustomizedProduct_customizedProductId",
                        column: x => x.customizedProductId,
                        principalTable: "CustomizedProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Slot",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    slotDimensions_height = table.Column<double>(nullable: false),
                    slotDimensions_width = table.Column<double>(nullable: false),
                    slotDimensions_depth = table.Column<double>(nullable: false),
                    CustomizedProductId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slot_CustomizedProduct_CustomizedProductId",
                        column: x => x.CustomizedProductId,
                        principalTable: "CustomizedProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogueCollection_customizedProductCollectionId",
                table: "CatalogueCollection",
                column: "customizedProductCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogueCollectionProduct_customizedProductId",
                table: "CatalogueCollectionProduct",
                column: "customizedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionProduct_customizedProductCollectionId",
                table: "CollectionProduct",
                column: "customizedProductCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Color_MaterialId",
                table: "Color",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CommercialCatalogueCatalogueCollection_catalogueCollectionId",
                table: "CommercialCatalogueCatalogueCollection",
                column: "catalogueCollectionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Component_complementedProductId",
                table: "Component",
                column: "complementedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomizedMaterial_colorId",
                table: "CustomizedMaterial",
                column: "colorId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomizedMaterial_finishId",
                table: "CustomizedMaterial",
                column: "finishId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomizedMaterial_materialId",
                table: "CustomizedMaterial",
                column: "materialId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomizedProduct_customizedMaterialId",
                table: "CustomizedProduct",
                column: "customizedMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomizedProduct_insertedInSlotId",
                table: "CustomizedProduct",
                column: "insertedInSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomizedProduct_productId",
                table: "CustomizedProduct",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_DoubleValue_DiscreteDimensionIntervalId",
                table: "DoubleValue",
                column: "DiscreteDimensionIntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_Finish_MaterialId",
                table: "Finish",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_FinishPriceTable_entityId",
                table: "FinishPriceTable",
                column: "entityId");

            migrationBuilder.CreateIndex(
                name: "IX_FinishPriceTable_timePeriodId",
                table: "FinishPriceTable",
                column: "timePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Input_RestrictionId",
                table: "Input",
                column: "RestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialPriceTable_entityId",
                table: "MaterialPriceTable",
                column: "entityId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialPriceTable_timePeriodId",
                table: "MaterialPriceTable",
                column: "timePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_depthId",
                table: "Measurement",
                column: "depthId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_heightId",
                table: "Measurement",
                column: "heightId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_widthId",
                table: "Measurement",
                column: "widthId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_productCategoryId",
                table: "Product",
                column: "productCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_parentId",
                table: "ProductCategory",
                column: "parentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMaterial_materialId",
                table: "ProductMaterial",
                column: "materialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMeasurement_measurementId",
                table: "ProductMeasurement",
                column: "measurementId");

            migrationBuilder.CreateIndex(
                name: "IX_Restriction_MeasurementId",
                table: "Restriction",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_Restriction_ComponentfatherProductId_ComponentcomplementedProductId",
                table: "Restriction",
                columns: new[] { "ComponentfatherProductId", "ComponentcomplementedProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_Restriction_ProductMaterialproductId_ProductMaterialmaterialId",
                table: "Restriction",
                columns: new[] { "ProductMaterialproductId", "ProductMaterialmaterialId" });

            migrationBuilder.CreateIndex(
                name: "IX_Slot_CustomizedProductId",
                table: "Slot",
                column: "CustomizedProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogueCollectionProduct_CustomizedProduct_customizedProductId",
                table: "CatalogueCollectionProduct",
                column: "customizedProductId",
                principalTable: "CustomizedProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomizedProduct_Slot_insertedInSlotId",
                table: "CustomizedProduct",
                column: "insertedInSlotId",
                principalTable: "Slot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slot_CustomizedProduct_CustomizedProductId",
                table: "Slot");

            migrationBuilder.DropTable(
                name: "CatalogueCollectionProduct");

            migrationBuilder.DropTable(
                name: "CollectionProduct");

            migrationBuilder.DropTable(
                name: "CommercialCatalogueCatalogueCollection");

            migrationBuilder.DropTable(
                name: "DoubleValue");

            migrationBuilder.DropTable(
                name: "FinishPriceTable");

            migrationBuilder.DropTable(
                name: "Input");

            migrationBuilder.DropTable(
                name: "MaterialPriceTable");

            migrationBuilder.DropTable(
                name: "ProductMeasurement");

            migrationBuilder.DropTable(
                name: "CatalogueCollection");

            migrationBuilder.DropTable(
                name: "CommercialCatalogue");

            migrationBuilder.DropTable(
                name: "Restriction");

            migrationBuilder.DropTable(
                name: "TimePeriod");

            migrationBuilder.DropTable(
                name: "CustomizedProductCollection");

            migrationBuilder.DropTable(
                name: "Measurement");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "ProductMaterial");

            migrationBuilder.DropTable(
                name: "Dimension");

            migrationBuilder.DropTable(
                name: "CustomizedProduct");

            migrationBuilder.DropTable(
                name: "CustomizedMaterial");

            migrationBuilder.DropTable(
                name: "Slot");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "Finish");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "Material");
        }
    }
}
