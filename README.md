# servicliente

Aplicacion .net webapi de ejemplo. Con las siguientes caractetisticas

- 3 Capas:
  - Presentacion --> Controllers
  - Aplicacion --> Services
  - Infraestructura --> Repository
- Utiliza las siguientes dependencias:
  - Entity Framework
  - FluentValidation
  - AutoMapper
- La base de datos es un SqlServer
- Utiliza DTOs en capa de presentacion, los que solo llegan hasta la capa de aplicacion
- Las validaciones se encuentran sobre los DTOs
- Los controladores solo reciben y enrutan los pedidos HTTP
- Se aplican las practicas mas recomendadas sobre parametros de entrada/salida
  - De entrada, los parametros llegan como DTOs al Body de los mensajes
  - De salida, Put, Get y Delete devuelven 204 cuando procesan y 404 cuando no encuentran el recurso
  - De salida, Create devuelve el recurso creado
  - De entrada, GetALL aplica numero y tamaño de pagina
- La logica de negocio se encuentra enteramente en la capa de aplicacion, incluyendo validaciones (validators) mapeos (profiles) y DTOs
- En la infraestructura se usa repository con una interfaz generica. Luego hubo que extenderla para agregar comportamiento de la entidad cliente
- Tiene una sola entidad (por ahora) llamada Cliente
- Para tratamiento de errores se usa patron Result desde repository hacia arriba.
- La paginacion se maneja desde appsetings.json y se carga en una objeto de configuracion que se carga en contenedor de dependencias

