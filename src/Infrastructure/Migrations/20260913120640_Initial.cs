using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "public");

        migrationBuilder.CreateTable(
            name: "Categories",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_categories", x => x.id);
                table.ForeignKey(
                    name: "fk_categories_categories_parent_id",
                    column: x => x.parent_id,
                    principalSchema: "public",
                    principalTable: "Categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "users",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                email = table.Column<string>(type: "text", nullable: false),
                first_name = table.Column<string>(type: "text", nullable: false),
                last_name = table.Column<string>(type: "text", nullable: false),
                password_hash = table.Column<string>(type: "text", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_users", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "Inventories",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                category_id = table.Column<Guid>(type: "uuid", nullable: true),
                parent_inventory_id = table.Column<Guid>(type: "uuid", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_inventories", x => x.id);
                table.ForeignKey(
                    name: "fk_inventories_categories_category_id",
                    column: x => x.category_id,
                    principalSchema: "public",
                    principalTable: "Categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_inventories_inventories_parent_inventory_id",
                    column: x => x.parent_inventory_id,
                    principalSchema: "public",
                    principalTable: "Inventories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "ItemDefinitions",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                manufacturer = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                model = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                category_id = table.Column<Guid>(type: "uuid", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_item_definitions", x => x.id);
                table.ForeignKey(
                    name: "fk_item_definitions_categories_category_id",
                    column: x => x.category_id,
                    principalSchema: "public",
                    principalTable: "Categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "PackDefinitions",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                barcode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                category_id = table.Column<Guid>(type: "uuid", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_pack_definitions", x => x.id);
                table.ForeignKey(
                    name: "fk_pack_definitions_categories_category_id",
                    column: x => x.category_id,
                    principalSchema: "public",
                    principalTable: "Categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "refresh_tokens",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                token = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                expires_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_refresh_tokens", x => x.id);
                table.ForeignKey(
                    name: "fk_refresh_tokens_users_user_id",
                    column: x => x.user_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "todo_items",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                description = table.Column<string>(type: "text", nullable: false),
                due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                labels = table.Column<List<string>>(type: "text[]", nullable: false),
                is_completed = table.Column<bool>(type: "boolean", nullable: false),
                completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                priority = table.Column<int>(type: "integer", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_todo_items", x => x.id);
                table.ForeignKey(
                    name: "fk_todo_items_users_user_id",
                    column: x => x.user_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ItemVariants",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                item_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                barcode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                category_id = table.Column<Guid>(type: "uuid", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_item_variants", x => x.id);
                table.ForeignKey(
                    name: "fk_item_variants_categories_category_id",
                    column: x => x.category_id,
                    principalSchema: "public",
                    principalTable: "Categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_item_variants_item_definitions_item_definition_id",
                    column: x => x.item_definition_id,
                    principalSchema: "public",
                    principalTable: "ItemDefinitions",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "InventoryItems",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                item_variant_id = table.Column<Guid>(type: "uuid", nullable: true),
                pack_definition_id = table.Column<Guid>(type: "uuid", nullable: true),
                quantity = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false),
                notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                serial_number = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                asset_number = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                category_id = table.Column<Guid>(type: "uuid", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_inventory_items", x => x.id);
                table.ForeignKey(
                    name: "fk_inventory_items_categories_category_id",
                    column: x => x.category_id,
                    principalSchema: "public",
                    principalTable: "Categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_inventory_items_inventories_inventory_id",
                    column: x => x.inventory_id,
                    principalSchema: "public",
                    principalTable: "Inventories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_inventory_items_item_variants_item_variant_id",
                    column: x => x.item_variant_id,
                    principalSchema: "public",
                    principalTable: "ItemVariants",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_inventory_items_pack_definitions_pack_definition_id",
                    column: x => x.pack_definition_id,
                    principalSchema: "public",
                    principalTable: "PackDefinitions",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "PackDefinitionItems",
            schema: "public",
            columns: table => new
            {
                pack_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                item_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                quantity = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_pack_definition_items", x => new { x.pack_definition_id, x.item_variant_id });
                table.ForeignKey(
                    name: "fk_pack_definition_items_item_variants_item_variant_id",
                    column: x => x.item_variant_id,
                    principalSchema: "public",
                    principalTable: "ItemVariants",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_pack_definition_items_pack_definitions_pack_definition_id",
                    column: x => x.pack_definition_id,
                    principalSchema: "public",
                    principalTable: "PackDefinitions",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Tags",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                inventory_id = table.Column<Guid>(type: "uuid", nullable: true),
                inventory_item_id = table.Column<Guid>(type: "uuid", nullable: true),
                item_definition_id = table.Column<Guid>(type: "uuid", nullable: true),
                item_variant_id = table.Column<Guid>(type: "uuid", nullable: true),
                pack_definition_id = table.Column<Guid>(type: "uuid", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_tags", x => x.id);
                table.ForeignKey(
                    name: "fk_tags_inventory_inventory_id",
                    column: x => x.inventory_id,
                    principalSchema: "public",
                    principalTable: "Inventories",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_tags_inventory_items_inventory_item_id",
                    column: x => x.inventory_item_id,
                    principalSchema: "public",
                    principalTable: "InventoryItems",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_tags_item_definitions_item_definition_id",
                    column: x => x.item_definition_id,
                    principalSchema: "public",
                    principalTable: "ItemDefinitions",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_tags_item_variants_item_variant_id",
                    column: x => x.item_variant_id,
                    principalSchema: "public",
                    principalTable: "ItemVariants",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_tags_pack_definitions_pack_definition_id",
                    column: x => x.pack_definition_id,
                    principalSchema: "public",
                    principalTable: "PackDefinitions",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "TagAssignments",
            schema: "public",
            columns: table => new
            {
                tag_id = table.Column<Guid>(type: "uuid", nullable: false),
                entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                entity_type = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_tag_assignments", x => new { x.tag_id, x.entity_id, x.entity_type });
                table.ForeignKey(
                    name: "fk_tag_assignments_tags_tag_id",
                    column: x => x.tag_id,
                    principalSchema: "public",
                    principalTable: "Tags",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_categories_parent_id",
            schema: "public",
            table: "Categories",
            column: "parent_id");

        migrationBuilder.CreateIndex(
            name: "ix_categories_slug",
            schema: "public",
            table: "Categories",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_inventories_category_id",
            schema: "public",
            table: "Inventories",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventories_parent_inventory_id",
            schema: "public",
            table: "Inventories",
            column: "parent_inventory_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventory_items_category_id",
            schema: "public",
            table: "InventoryItems",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventory_items_inventory_id",
            schema: "public",
            table: "InventoryItems",
            column: "inventory_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventory_items_item_variant_id",
            schema: "public",
            table: "InventoryItems",
            column: "item_variant_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventory_items_pack_definition_id",
            schema: "public",
            table: "InventoryItems",
            column: "pack_definition_id");

        migrationBuilder.CreateIndex(
            name: "ix_item_definitions_category_id",
            schema: "public",
            table: "ItemDefinitions",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_item_definitions_sku",
            schema: "public",
            table: "ItemDefinitions",
            column: "sku",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_item_variants_barcode",
            schema: "public",
            table: "ItemVariants",
            column: "barcode");

        migrationBuilder.CreateIndex(
            name: "ix_item_variants_category_id",
            schema: "public",
            table: "ItemVariants",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_item_variants_item_definition_id",
            schema: "public",
            table: "ItemVariants",
            column: "item_definition_id");

        migrationBuilder.CreateIndex(
            name: "ix_item_variants_sku",
            schema: "public",
            table: "ItemVariants",
            column: "sku",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_pack_definition_items_item_variant_id",
            schema: "public",
            table: "PackDefinitionItems",
            column: "item_variant_id");

        migrationBuilder.CreateIndex(
            name: "ix_pack_definitions_barcode",
            schema: "public",
            table: "PackDefinitions",
            column: "barcode");

        migrationBuilder.CreateIndex(
            name: "ix_pack_definitions_category_id",
            schema: "public",
            table: "PackDefinitions",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_pack_definitions_sku",
            schema: "public",
            table: "PackDefinitions",
            column: "sku",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_refresh_tokens_token",
            schema: "public",
            table: "refresh_tokens",
            column: "token",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_refresh_tokens_user_id",
            schema: "public",
            table: "refresh_tokens",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_tags_inventory_id",
            schema: "public",
            table: "Tags",
            column: "inventory_id");

        migrationBuilder.CreateIndex(
            name: "ix_tags_inventory_item_id",
            schema: "public",
            table: "Tags",
            column: "inventory_item_id");

        migrationBuilder.CreateIndex(
            name: "ix_tags_item_definition_id",
            schema: "public",
            table: "Tags",
            column: "item_definition_id");

        migrationBuilder.CreateIndex(
            name: "ix_tags_item_variant_id",
            schema: "public",
            table: "Tags",
            column: "item_variant_id");

        migrationBuilder.CreateIndex(
            name: "ix_tags_pack_definition_id",
            schema: "public",
            table: "Tags",
            column: "pack_definition_id");

        migrationBuilder.CreateIndex(
            name: "ix_tags_slug",
            schema: "public",
            table: "Tags",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_todo_items_user_id",
            schema: "public",
            table: "todo_items",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_users_email",
            schema: "public",
            table: "users",
            column: "email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PackDefinitionItems",
            schema: "public");

        migrationBuilder.DropTable(
            name: "refresh_tokens",
            schema: "public");

        migrationBuilder.DropTable(
            name: "TagAssignments",
            schema: "public");

        migrationBuilder.DropTable(
            name: "todo_items",
            schema: "public");

        migrationBuilder.DropTable(
            name: "Tags",
            schema: "public");

        migrationBuilder.DropTable(
            name: "users",
            schema: "public");

        migrationBuilder.DropTable(
            name: "InventoryItems",
            schema: "public");

        migrationBuilder.DropTable(
            name: "Inventories",
            schema: "public");

        migrationBuilder.DropTable(
            name: "ItemVariants",
            schema: "public");

        migrationBuilder.DropTable(
            name: "PackDefinitions",
            schema: "public");

        migrationBuilder.DropTable(
            name: "ItemDefinitions",
            schema: "public");

        migrationBuilder.DropTable(
            name: "Categories",
            schema: "public");
    }
}
