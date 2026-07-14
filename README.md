# Custom Binding in Blazor DataGrid Component

A comprehensive example demonstrating how to implement custom data binding with the [Blazor DataGrid](https://www.syncfusion.com/blazor-components/blazor-datagrid) component. This sample shows how to bind custom data sources while leveraging advanced features like paging, sorting, filtering, grouping, aggregates, and inline editing.

## Overview

This project demonstrates the implementation of a custom `DataAdaptor` to manage data operations in the Blazor DataGrid. Instead of relying on built-in data sources, custom binding allows you to define exactly how the grid handles data manipulation, enabling you to:

- Implement your own data retrieval logic
- Apply server-side paging, sorting, and filtering
- Handle custom business rules and validations
- Integrate with external APIs or complex backend systems
- Optimize performance with selective data loading

The sample includes a working example with order data that showcases all these capabilities in action.

## Features

- **Custom Data Binding** - Implement `DataAdaptor` to control data operations
- **Paging** - Server-side pagination with configurable page sizes
- **Sorting** - Multi-column sorting support
- **Filtering** - Excel-style filtering with custom filter operations
- **Grouping** - Group data by any column with aggregates
- **Aggregates** - Sum, average, and custom aggregate calculations
- **Inline Editing** - Add, edit, and delete records with edit toolbar
- **DataManager Integration** - Fully integrated with Blazor DataManager

## Prerequisites

* [.NET SDK 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) or later
* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or later
* [Visual Studio Code](https://code.visualstudio.com/)

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/SyncfusionExamples/Custom-Binding-in-Blazor-DataGrid-Component.git
cd Custom-Binding-in-Blazor-DataGrid-Component
```

### Run with Visual Studio

1. Open the solution file using Visual Studio 2022 or later.
2. Restore the NuGet packages by rebuilding the solution.
3. Build the project to ensure there are no compilation errors.
4. Run the project.

### Run with .NET CLI

```bash
# Restore dependencies
dotnet restore

# Run the project
dotnet run
```

## References

**Documentation**: https://blazor.syncfusion.com/documentation/datagrid/connecting-to-adaptors/custom-adaptor

**Online example**: https://blazor.syncfusion.com/demos/datagrid/custom-binding?theme=fluent2
