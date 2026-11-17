# API de Habita

La API usa JSON y versiona sus rutas en la URL. La versión actual es `v1`.

```text
/api/v1/{recurso}
```

En desarrollo, la especificación OpenAPI se publica en `/openapi/v1.json` y la interfaz Scalar en `/docs`.

## Autenticación y permisos

Envía el token JWT como `Authorization: Bearer <token>`. El inicio de sesión es público. Las demás rutas requieren la política de acceso a la API; las operaciones de escritura de catálogos y los cambios de estado de agentes requieren rol `Admin`.

## Formato de respuesta

Las respuestas JSON con contenido usan un sobre estable:

```json
{
  "success": true,
  "data": {},
  "error": null
}
```

Los errores usan `success: false`, `data: null` y un objeto `error` con `code`, `message` y, cuando aplica, `details`. Los errores de validación ponen los mensajes por campo en `details`. Los errores inesperados devuelven un mensaje genérico; los detalles técnicos se registran en el servidor.

Una lista sin resultados puede responder `204 No Content`, por lo que no incluye cuerpo.

## Endpoints

| Método | Ruta | Acceso | Uso |
| --- | --- | --- | --- |
| POST | `/api/v1/account/login` | Público | Autenticar y obtener el token. |
| POST | `/api/v1/account/register/developer` | Admin | Registrar una cuenta Developer. |
| POST | `/api/v1/account/register/admin` | Admin | Registrar una cuenta Admin. |
| GET | `/api/v1/properties` | JWT | Listar propiedades con filtros y paginación de `GetPropertyListRequest`. |
| GET | `/api/v1/properties/{id}` | JWT | Obtener una propiedad por identificador. |
| GET | `/api/v1/properties/code/{code}` | JWT | Buscar una propiedad por código. |
| GET | `/api/v1/agents` | JWT | Listar agentes con filtros/paginación. |
| GET | `/api/v1/agents/{id}` | JWT | Consultar un agente. |
| GET | `/api/v1/agents/{id}/properties` | JWT | Listar las propiedades de un agente; admite `page` y `pageSize`. |
| PATCH | `/api/v1/agents/{id}/toggle` | Admin | Alternar el estado del agente. |
| PATCH | `/api/v1/agents/{id}/status` | Admin | Establecer el estado usando `AgentStatusRequest`. |
| GET | `/api/v1/improvement` | JWT | Listar mejoras disponibles. |
| GET | `/api/v1/improvement/{id}` | JWT | Consultar una mejora. |
| POST | `/api/v1/improvement` | Admin | Crear una mejora. |
| PUT | `/api/v1/improvement/{id}` | Admin | Actualizar una mejora. |
| DELETE | `/api/v1/improvement/{id}` | Admin | Eliminar una mejora. |
| GET | `/api/v1/propertytype` | JWT | Listar tipos de propiedad. |
| GET | `/api/v1/propertytype/{id}` | JWT | Consultar un tipo de propiedad. |
| POST | `/api/v1/propertytype` | Admin | Crear un tipo de propiedad. |
| PUT | `/api/v1/propertytype/{id}` | Admin | Actualizar un tipo de propiedad. |
| DELETE | `/api/v1/propertytype/{id}` | Admin | Eliminar un tipo de propiedad. |
| GET | `/api/v1/saletype` | JWT | Listar modalidades de venta. |
| GET | `/api/v1/saletype/{id}` | JWT | Consultar una modalidad de venta. |
| POST | `/api/v1/saletype` | Admin | Crear una modalidad de venta. |
| PUT | `/api/v1/saletype/{id}` | Admin | Actualizar una modalidad de venta. |
| DELETE | `/api/v1/saletype/{id}` | Admin | Eliminar una modalidad de venta. |
| GET | `/health` | Público | Comprobar disponibilidad del servicio. |

Los esquemas de solicitud y respuesta se definen en los DTO de `Habita.Api` y `Habita.Application`; OpenAPI ofrece el esquema detallado para la versión que se esté ejecutando.
