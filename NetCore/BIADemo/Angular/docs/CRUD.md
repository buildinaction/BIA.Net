# CRUD
This document explains how to quickly create a CRUD.   
<u>For this example, we imagine that we want to create a new feature with the name: <span style="background-color:yellow">aircrafts</span>.   </u>

## Prerequisite
The back-end is ready, i.e. the <span style="background-color:yellow">Aircraft</span> controller exists as well as permissions such as `Aircraft_List_Access`.

## Create a new feature
First, create a new <span style="background-color:yellow">aircrafts</span> folder under the **src\app\features** folder of your project.   
Then copy, paste and unzip into this feature <span style="background-color:yellow">aircrafts</span> folder the contents of :
* **Angular\docs\planes-popup.zip** if you want a CRUD with popup
* **Angular\docs\planes-page.zip** if you want a CRUD with page

Then, inside the folder of your new feature, execute the file **new-crud.ps1**   
For **old crud name? (singular)**, type plane   
For **old crud name? (plural)**, type planes   
For **new crud name? (singular)**, type <span style="background-color:yellow">aircraft</span>   
For **new crud name? (plural)**, type <span style="background-color:yellow">aircrafts</span>   
When finished, you can delete **new-crud.ps1**   

## Update navigation
Open the file **src\app\shared\permission.ts** and in the **Permission** enum, add
```typescript
  Aircraft_Create = 'Aircraft_Create',
  Aircraft_Delete = 'Aircraft_Delete',
  Aircraft_List_Access = 'Aircraft_List_Access',
  Aircraft_Read = 'Aircraft_Read',
  Aircraft_Save = 'Aircraft_Save',
  Aircraft_Update = 'Aircraft_Update',
```

## Update navigation
Open the file **src\app\shared\navigation.ts** and in the array **NAVIGATION**, add 
```typescript
{
  labelKey: 'app.aircrafts',
  permissions: [Permission.Aircraft_List_Access],
  path: ['/aircrafts']
}
```
## Update routing
Open the file **src\app\app-routing.module.ts** and in the array **routes**, add 
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
Use the back-end with swagger to retrieve a json from the new entity.   
Use this site to convert the json to interface TypeScript:   
http://json2ts.com/   
And then, copy the generated code in **src\app\features\aircrafts\model\aircraft.ts**

## Update translations
Open the file **src\assets\i18n\app\en.json** and   
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

Open the file **src\assets\i18n\app\fr.json** and
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

Open the file **src\assets\i18n\app\es.json** and
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
**src\app\features\aircrafts\components\aircraft-form**

## Table
Open the following file **src\app\features\aircrafts\views\aircrafts-index\aircrafts-index.component.ts**   
Change columns field of the `tableConfiguration` variable according to the columns in the table you want to display.   
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

## Enable Views

To enable views on this feature, set the **aircrafts-index.component.ts** file.  
Add this import:
```typescript
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';
```
in `ngOnInit`, add:
```typescript
ngOnInit() {
  ...
  this.store.dispatch(loadAllView());
}
```
add this property:
```typescript
tableStateKey = 'aircraftsGrid';
```
Set the **aircrafts-index.component.html** file.  
Add the input parameter `tableStateKey` for the `bia-table-controller` and `bia-table` like this:

```html
<bia-table-controller
...
[tableStateKey]="tableStateKey"
></bia-table-controller>
```

```html
<bia-table
...
[tableStateKey]="tableStateKey"
></bia-table>
```

