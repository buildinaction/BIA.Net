# CRUD
This document explains how to quickly create a CRUD.<br/>
<u>For this example, we imagine that we want to create a new feature with the name: <span style="background-color:yellow">aircrafts</span>.<br/></u>

## Prerequisite
The back-end is ready, i.e. the <span style="background-color:yellow">Aircraft</span> controller exists as well as permissions such as `Aircraft_List_Access`.

## Create a new feature
First, create a new <span style="background-color:yellow">aircrafts</span> folder under the <b>src\app\features</b> folder of your project.<br/>
Then copy and paste the contents of the folder
<ul>
<i><b>BIADemo\Angular\src\app\features\planes</b> if you want a CRUD with popup</i>
<i><b>BIADemo\Angular\src\app\features\planes-page-mode</b> if you want a CRUD with page</i>
</ul> into the feature <span style="background-color:yellow">aircrafts</span>  folder.

Then, inside the folder of your new feature, execute the file <b>new-crud.ps1</b><br/>
For <b>old crud name? (singular)</b>, type plane<br/>
For <b>old crud name? (plural)</b>, type planes<br/>
For <b>new crud name? (singular)</b>, type <span style="background-color:yellow">aircraft</span><br/>
For <b>new crud name? (plural)</b>, type <span style="background-color:yellow">aircrafts</span><br/>
When finished, you can delete <b>new-crud.ps1</b><br/>

## Update navigation
Open the file <b>src\app\shared\constants.ts</b> and in the <b>Permission</b> enum, add
```typescript
  Aircraft_Create = 'Aircraft_Create',
  Aircraft_Delete = 'Aircraft_Delete',
  Aircraft_List_Access = 'Aircraft_List_Access',
  Aircraft_Read = 'Aircraft_Read',
  Aircraft_Save = 'Aircraft_Save',
  Aircraft_Update = 'Aircraft_Update',
```

## Update navigation
Open the file <b>src\app\shared\constants.ts</b> and in the array <b>NAVIGATION</b>, add 
```typescript
{
  labelKey: 'app.aircrafts',
  permissions: [Permission.Plane_List_Access],
  path: ['/aircrafts']
}
```
## Update routing
Open the file <b>src\app\app-routing.module.ts</b> and in the array <b>routes</b>, add 
```typescript
{
  path: 'aircrafts',
  data: {
    breadcrumb: 'app.aircrafts',
    canNavigate: true
    },
    loadChildren: () => import('./features/aircrafts/aircraft.module').then((m) => m.AircraftModule)
}
```
## Create the model
Use the back-end with swagger to retrieve a json from the new entity.<br/>
Use this site to convert the json to interface TypeScript:<br/>
http://json2ts.com/<br/>
And then, copy the generated code in <b>src\app\features\aircrafts\model\aircraft.ts</b>

## Update translations
Open the file <b>src\assets\i18n\app\en.json</b> and<br/>
add in `"app"`
```json
"app": {
    ...
    "aircrafts": "Aircrafts"
  }
```
add
```json
"aircraft": {
  "add": "Add Aircraft",
  "edit": "Edit Aircraft",
  "listOf": "List of aircrafts"
  }
```
and add translations of interface properties.

Open the file <b>src\assets\i18n\app\fr.json</b> and
add in `"app"`
```json
"app": {
    ...
    "aircrafts": "Aéronefs"
  }
```
add
```json
"aircraft": {
    "add": "Ajouter Aéronef",
    "edit": "Modifier Aéronef",
    "listOf": "Liste des aéronefs"
  }
```
and add translations of interface properties.

Open the file <b>src\assets\i18n\app\es.json</b> and
add in `"app"`
```json
"app": {
    ...
    "aircrafts": "Aeronaves"
  }
```
add
```json
"aircraft": {
   "add": "Añadir Aeronave",
    "edit": "Editar Aeronave",
    "listOf": "Lista de aeronaves"
  }
```
and add translations of interface properties.

When you have finished adding translations, use this site to sort your json:
https://novicelab.org/jsonabc/

## Form
Change the following form component to match the business requirements:
<b>src\app\features\aircrafts\components\aircraft-form</b>

## Table
Open the following file <b>src\app\features\aircrafts\views\aircrafts-index\aircrafts-index.component.ts</b><br/>
Change columns field of the `tableConfiguration` variable according to the columns in the table you want to display.<br/>
If it is a simple string type column with filter and possible sorting, then use this column line to define your column.
```typescript
new PrimeTableColumn('msn', 'aircraft.msn'),
```
If the column type is not a string, or if the column is not sortable, or not sortable, you must define the column as follows: 
```typescript
Object.assign(new PrimeTableColumn('msn', 'aircraft.msn'), {
        isSearchable: false,
        isSortable: false,
        type: TypeTS.Number
      })
```




