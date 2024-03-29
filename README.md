
# Backend - ChallengeTechFullStackN5 C#

Este es el desarrollo del backend del challenge N5, desarrollado por John Sandoval


## API Reference

#### GetPermissions

```http
  GET http://{host}:8080/api/permiso/getpermissions
```

| Parametetro | Tipo     | Descripción                     |
| :---------- | :------- | :------------------------- |
| `api_key` | `string` | **No solicitada*** |

#### Función:

Retorna todos los registros de la tabla permisos sin paginación.

#### GetPermission

```http
  GET http://{host}:8080/api/permiso/getpermission/${id}
```

| Parametetro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `id`      | `integer` | **Required**. Id requerido |

#### Función:

Busca un registro por id en la tabla permisos y retorna los datos del registro encontrado, se usa para cargar y mostrar en el frontend los datos que se van a modificar o eliminar, antes de hacerlo.
```json
{
	"id": {1},
	"nombreEmpleado": "Jon",
	"apellidoEmpleado": "Doe",
	"tipoPermiso": 1,
	"fechaPermiso": "2024-02-20T00:00:00"
}
```
#### RequestPermissions

```http
  POST http://{host}:8080/api/permiso/requestpermission
```

| Parametetro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `body`      | `JSON` | **Ej:** {"nombreEmpleado": "Jon", "apellidoEmpleado": "Doe", "tipoPermiso": 3, "fechaPermiso": "2024-02-19T00:00:00"}|



#### Función:
Le envia datos al backend, para que los ingrese en la tabla permisos, requiere un esquema de datos de tipo JSON, en el body de la petición.
```json
{			
	"nombreEmpleado": "John",
	"apellidoEmpleado": "Doe",
	"tipoPermiso": 1,
	"fechaPermiso": "2024-02-19T00:00:00"
}
```
#### ModifyPermissions

```http
  PUT http://{host}:8080/api/permiso/modifypermission/{id}
```

| Parametetro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `id`      | `integer` | **Required**. Id requerido |
| `body`      | `JSON` | **Ej:** {"nombreEmpleado": "Jon", "apellidoEmpleado": "Doe Smith", "tipoPermiso": 3, "fechaPermiso": "2024-02-19T00:00:00"}|



#### Función:
Le envia datos al backend, para actualizarlos en la tabla permisos, requiere un esquema de datos de tipo JSON, en el body de la petición.
```json
{			
    "id": {id},
	"nombreEmpleado": "Jon",
	"apellidoEmpleado": "Doe Smith",
	"tipoPermiso": 3,
	"fechaPermiso": "2024-02-19T00:00:00"
}
```
#### RemovePermissions

```http
  PUT http://{host}:8080/api/permiso/removepermission/{id}
```

| Parametetro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `id`      | `integer` | **Required**. Id requerido |
| `body`      | `JSON` | **Ej:** {"nombreEmpleado": "Jon", "apellidoEmpleado": "Doe Smith", "tipoPermiso": 3, "fechaPermiso": "2024-02-19T00:00:00"}|



#### Función:
Le envia datos al backend, para actualizarlos en la tabla permisos, requiere un esquema de datos de tipo JSON, en el body de la petición.
```json
{			
    "id": {id},
	"nombreEmpleado": "Jon",
	"apellidoEmpleado": "Doe Smith",
	"tipoPermiso": 3,
	"fechaPermiso": "2024-02-19T00:00:00"
}
```
## Dependencias

- [Confluent.Kafka](https://www.nuget.org/packages/Confluent.Kafka): 2.3.0
- [Elasticsearch.Net](https://www.nuget.org/packages/Elasticsearch.Net): 7.17.5
- [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer): 8.0.0
- [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools): 8.0.0
  - PrivateAssets: all
  - IncludeAssets: runtime; build; native; contentfiles; analyzers; buildtransitive
- [Nest](https://www.nuget.org/packages/Nest): 7.17.5
- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json): 13.0.3


## Autor
John Sandoval
- [@MrJohnSandoval](https://www.github.com/MrJohnSandoval)


## Reconocimientos

 - [Construcción profesional de documentación tipo README](https://awesomeopensource.com/project/elangosundar/awesome-README-templates)
 - [READMEs asombrosos](https://github.com/matiassingers/awesome-readme)
 - [Como escribir un buen README](https://bulldogjob.com/news/449-how-to-write-a-good-readme-for-your-github-project)


