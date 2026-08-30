# Inventory

A flexible inventory management application for tracking items, variants, packs, and nested storage locations.

The goal is to model real-world possessions and supplies without assuming that everything is a simple "product + quantity".

## Overview

The application is built around a few core concepts:

* **Item Definitions** — define what an item is.
* **Item Variants** — define specific configurations of an item.
* **Packs** — define collections of multiple items.
* **Inventories** — represent places or containers where things can be stored.
* **Inventory Items** — represent what is actually stored in an inventory.
* **Categories & Tags** — provide flexible organization and future search capabilities.

The model is intentionally generic. An "item" can be anything from a consumable to an electronic device or a physical container.

## Domain Model

### Item Definition

An `ItemDefinition` represents the general type of an item.

Examples:

```text
Water Bottle
Phone
Screw
Backpack
Laptop
Chair
```

An item definition can have multiple variants.

### Item Variant

An `ItemVariant` represents a specific configuration of an item.

For example:

```text
Water Bottle
├── 500ml
├── 1L
└── 2L
```

Or:

```text
Phone
├── 256GB / Black
├── 256GB / Blue
└── 512GB / Black
```

Not every item needs multiple variants. A simple item can have a single configuration.

### Packs

A `PackDefinition` represents a collection of items.

A pack isn't restricted to containing multiple copies of the same item.

For example:

```text
Pack: 6 × Water Bottle 2L

Contents:
└── 6 × Water Bottle 2L
```

But it can also contain different items:

```text
Pack: Water + Cups

Contents:
├── 3 × Water Bottle 2L
└── 2 × Cup 300ml
```

This is modeled using `PackDefinitionItem`.

Conceptually:

```text
PackDefinition
└── PackDefinitionItem
    ├── ItemVariant
    └── Quantity
```

### Inventories

An `Inventory` represents a place or container capable of containing other things.

Examples:

```text
House
Backpack
Box
Drawer
Car
Warehouse
```

Inventories can contain other inventories:

```text
House
├── Kitchen
├── Bedroom
└── Box
    └── Backpack
```

This is represented using a self-referencing relationship:

```text
Inventory
├── ParentInventory
└── ChildInventories
```

This allows arbitrary inventory hierarchies.

### Inventory Items

An `InventoryItem` represents something actually present in an inventory.

For example:

```text
House
└── Pantry
    └── 2 × Pack of 6 Water Bottle 2L
```

The inventory entry stores the quantity:

```text
Quantity = 2
PackDefinition = Pack of 6 Water Bottle 2L
```

An inventory item can represent either:

* an `ItemVariant`
* a `PackDefinition`

For example:

```text
Inventory
├── 5 × Water Bottle 2L
├── 1 × iPhone 17 256GB Black
└── 2 × Pack of 6 Water Bottle 2L
```

## Categories

Categories provide hierarchical classification.

For example:

```text
Electronics
├── Computers
│   ├── Laptops
│   └── Desktops
└── Phones
    └── Smartphones
```

Categories can have parent and child categories.

An entity can optionally belong to a category.

## Tags

Tags provide flexible, non-hierarchical metadata.

Examples:

```text
personal
fragile
expensive
travel
work
spare
```

Tags are intended to complement categories rather than replace them.

For example:

```text
Category:
Electronics > Phones > Smartphones

Tags:
personal
expensive
usb-c
```

This will allow future searches such as:

```text
Category = Phones
Tag = personal
```

or:

```text
Tag = fragile
```

## Entity Metadata

Domain entities contain common lifecycle information such as:

```text
Id
CreatedAt
UpdatedAt
IsDeleted
DeletedAt
```

The application uses **soft deletion** rather than immediately removing entities from the database.

This allows deleted records to be retained and potentially restored or audited later.

## Technology

The application is currently built around:

* C#
* .NET
* Entity Framework Core
* Relational database
* EF Core migrations

The domain model is designed to keep the domain concepts independent from the persistence implementation as much as practical.

## Database

Entity Framework Core is used for persistence.

Migrations can be created using the Visual Studio Package Manager Console:

```powershell
Add-Migration MigrationName
```

Apply migrations with:

```powershell
Update-Database
```

For example:

```powershell
Add-Migration InitialCreate
Update-Database
```

## Design Principles

### Generic domain model

The system should not assume that inventory means only food or consumables.

The same model should support:

```text
2 × Water Bottle 2L
1 × iPhone
50 × M4 Screws
1 × Backpack
1 × Laptop
```

### Composition over inheritance

Packs, variants, inventories, and inventory entries are modeled as separate concepts rather than creating a large inheritance hierarchy.

For example:

```text
ItemDefinition
└── ItemVariant

PackDefinition
└── PackDefinitionItem
        └── ItemVariant
```

### Nested inventories

Anything can potentially act as a container.

This makes structures such as the following possible:

```text
House
└── Storage Room
    └── Box
        └── Backpack
            └── Laptop
```

The application can therefore represent both physical locations and physical containers using the same inventory concept.

## Current Domain Structure

At a high level:

```text
                         ┌── Category
                         │
ItemDefinition ──────────┤
    │                    └── Tags
    │
    └── ItemVariant
           │
           ├───────────────┐
           │               │
           ▼               ▼
     PackDefinition    InventoryItem
           │               │
           │               ├── ItemVariant
           │               │
           │               └── PackDefinition
           │
           └── PackDefinitionItem
                    │
                    └── ItemVariant


Inventory
    │
    ├── ParentInventory
    │
    ├── ChildInventories
    │
    └── InventoryItems
```

## Future Direction

The model is intended to support future functionality such as:

* Searching by category
* Searching by tag
* Browsing nested inventories
* Tracking quantities
* Tracking individual physical assets
* Serial numbers and asset numbers
* More sophisticated units of measurement
* Pack composition and potentially nested packs
* Inventory movement between containers
* Inventory history and auditing
* Reporting and statistics
