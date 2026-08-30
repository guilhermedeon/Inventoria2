using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class Inventoria_Start : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "public");

        migrationBuilder.CreateTable(
            name: "category",
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
                table.PrimaryKey("pk_category", x => x.id);
                table.ForeignKey(
                    name: "fk_category_category_parent_id",
                    column: x => x.parent_id,
                    principalSchema: "public",
                    principalTable: "category",
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
            name: "inventory",
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
                table.PrimaryKey("pk_inventory", x => x.id);
                table.ForeignKey(
                    name: "fk_inventory_category_category_id",
                    column: x => x.category_id,
                    principalSchema: "public",
                    principalTable: "category",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_inventory_inventory_parent_inventory_id",
                    column: x => x.parent_inventory_id,
                    principalSchema: "public",
                    principalTable: "inventory",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "item_definition",
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
                table.PrimaryKey("pk_item_definition", x => x.id);
                table.ForeignKey(
                    name: "fk_item_definition_category_category_id",
                    column: x => x.category_id,
                    principalSchema: "public",
                    principalTable: "category",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "pack_definition",
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
                table.PrimaryKey("pk_pack_definition", x => x.id);
                table.ForeignKey(
                    name: "fk_pack_definition_category_category_id",
                    column: x => x.category_id,
                    principalSchema: "public",
                    principalTable: "category",
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
            name: "item_variant",
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
                table.PrimaryKey("pk_item_variant", x => x.id);
                table.ForeignKey(
                    name: "fk_item_variant_category_category_id",
                    column: x => x.category_id,
                    principalSchema: "public",
                    principalTable: "category",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_item_variant_item_definition_item_definition_id",
                    column: x => x.item_definition_id,
                    principalSchema: "public",
                    principalTable: "item_definition",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "inventory_item",
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
                table.PrimaryKey("pk_inventory_item", x => x.id);
                table.ForeignKey(
                    name: "fk_inventory_item_category_category_id",
                    column: x => x.category_id,
                    principalSchema: "public",
                    principalTable: "category",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_inventory_item_inventory_inventory_id",
                    column: x => x.inventory_id,
                    principalSchema: "public",
                    principalTable: "inventory",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_inventory_item_item_variant_item_variant_id",
                    column: x => x.item_variant_id,
                    principalSchema: "public",
                    principalTable: "item_variant",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_inventory_item_pack_definition_pack_definition_id",
                    column: x => x.pack_definition_id,
                    principalSchema: "public",
                    principalTable: "pack_definition",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "pack_definition_item",
            schema: "public",
            columns: table => new
            {
                pack_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                item_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                quantity = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_pack_definition_item", x => new { x.pack_definition_id, x.item_variant_id });
                table.ForeignKey(
                    name: "fk_pack_definition_item_item_variant_item_variant_id",
                    column: x => x.item_variant_id,
                    principalSchema: "public",
                    principalTable: "item_variant",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_pack_definition_item_pack_definition_pack_definition_id",
                    column: x => x.pack_definition_id,
                    principalSchema: "public",
                    principalTable: "pack_definition",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "tag",
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
                table.PrimaryKey("pk_tag", x => x.id);
                table.ForeignKey(
                    name: "fk_tag_inventory_inventory_id",
                    column: x => x.inventory_id,
                    principalSchema: "public",
                    principalTable: "inventory",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_tag_inventory_item_inventory_item_id",
                    column: x => x.inventory_item_id,
                    principalSchema: "public",
                    principalTable: "inventory_item",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_tag_item_definition_item_definition_id",
                    column: x => x.item_definition_id,
                    principalSchema: "public",
                    principalTable: "item_definition",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_tag_item_variant_item_variant_id",
                    column: x => x.item_variant_id,
                    principalSchema: "public",
                    principalTable: "item_variant",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_tag_pack_definition_pack_definition_id",
                    column: x => x.pack_definition_id,
                    principalSchema: "public",
                    principalTable: "pack_definition",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "tag_assignment",
            schema: "public",
            columns: table => new
            {
                tag_id = table.Column<Guid>(type: "uuid", nullable: false),
                entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                entity_type = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_tag_assignment", x => new { x.tag_id, x.entity_id, x.entity_type });
                table.ForeignKey(
                    name: "fk_tag_assignment_tag_tag_id",
                    column: x => x.tag_id,
                    principalSchema: "public",
                    principalTable: "tag",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_category_parent_id",
            schema: "public",
            table: "category",
            column: "parent_id");

        migrationBuilder.CreateIndex(
            name: "ix_category_slug",
            schema: "public",
            table: "category",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_inventory_category_id",
            schema: "public",
            table: "inventory",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventory_parent_inventory_id",
            schema: "public",
            table: "inventory",
            column: "parent_inventory_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventory_item_category_id",
            schema: "public",
            table: "inventory_item",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventory_item_inventory_id",
            schema: "public",
            table: "inventory_item",
            column: "inventory_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventory_item_item_variant_id",
            schema: "public",
            table: "inventory_item",
            column: "item_variant_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventory_item_pack_definition_id",
            schema: "public",
            table: "inventory_item",
            column: "pack_definition_id");

        migrationBuilder.CreateIndex(
            name: "ix_item_definition_category_id",
            schema: "public",
            table: "item_definition",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_item_definition_sku",
            schema: "public",
            table: "item_definition",
            column: "sku",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_item_variant_barcode",
            schema: "public",
            table: "item_variant",
            column: "barcode");

        migrationBuilder.CreateIndex(
            name: "ix_item_variant_category_id",
            schema: "public",
            table: "item_variant",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_item_variant_item_definition_id",
            schema: "public",
            table: "item_variant",
            column: "item_definition_id");

        migrationBuilder.CreateIndex(
            name: "ix_item_variant_sku",
            schema: "public",
            table: "item_variant",
            column: "sku",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_pack_definition_barcode",
            schema: "public",
            table: "pack_definition",
            column: "barcode");

        migrationBuilder.CreateIndex(
            name: "ix_pack_definition_category_id",
            schema: "public",
            table: "pack_definition",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_pack_definition_sku",
            schema: "public",
            table: "pack_definition",
            column: "sku",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_pack_definition_item_item_variant_id",
            schema: "public",
            table: "pack_definition_item",
            column: "item_variant_id");

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
            name: "ix_tag_inventory_id",
            schema: "public",
            table: "tag",
            column: "inventory_id");

        migrationBuilder.CreateIndex(
            name: "ix_tag_inventory_item_id",
            schema: "public",
            table: "tag",
            column: "inventory_item_id");

        migrationBuilder.CreateIndex(
            name: "ix_tag_item_definition_id",
            schema: "public",
            table: "tag",
            column: "item_definition_id");

        migrationBuilder.CreateIndex(
            name: "ix_tag_item_variant_id",
            schema: "public",
            table: "tag",
            column: "item_variant_id");

        migrationBuilder.CreateIndex(
            name: "ix_tag_pack_definition_id",
            schema: "public",
            table: "tag",
            column: "pack_definition_id");

        migrationBuilder.CreateIndex(
            name: "ix_tag_slug",
            schema: "public",
            table: "tag",
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
            name: "pack_definition_item",
            schema: "public");

        migrationBuilder.DropTable(
            name: "refresh_tokens",
            schema: "public");

        migrationBuilder.DropTable(
            name: "tag_assignment",
            schema: "public");

        migrationBuilder.DropTable(
            name: "todo_items",
            schema: "public");

        migrationBuilder.DropTable(
            name: "tag",
            schema: "public");

        migrationBuilder.DropTable(
            name: "users",
            schema: "public");

        migrationBuilder.DropTable(
            name: "inventory_item",
            schema: "public");

        migrationBuilder.DropTable(
            name: "inventory",
            schema: "public");

        migrationBuilder.DropTable(
            name: "item_variant",
            schema: "public");

        migrationBuilder.DropTable(
            name: "pack_definition",
            schema: "public");

        migrationBuilder.DropTable(
            name: "item_definition",
            schema: "public");

        migrationBuilder.DropTable(
            name: "category",
            schema: "public");
    }
}
