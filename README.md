# MiniSiniestros

Aplicación desarrollada como resolución de un challenge técnico para la gestión de siniestros de una ART.

## Tecnologías

- .NET 8
- ASP.NET Core Web API
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- AutoMapper
- JWT Bearer Authentication
- Serilog
- xUnit

## Arquitectura

La solución está organizada en los siguientes proyectos:

- MiniSiniestros.Api
- MiniSiniestros.Web
- MiniSiniestros.Services
- MiniSiniestros.Data
- MiniSiniestros.Data.Migrations
- MiniSiniestros.Entities
- MiniSiniestros.Dto
- MiniSiniestros.ViewModels
- MiniSiniestros.Tests

## Funcionalidades

- CRUD de Siniestros
- CRUD de Empleadores
- CRUD de Trabajadores
- CRUD de Prestadores Médicos
- Autenticación mediante JWT
- Manejo global de excepciones
- Pruebas unitarias

## Base de datos

Al iniciar la aplicación por primera vez:

- Se aplican automáticamente las migraciones.
- Se ejecuta un DatabaseSeeder que carga datos de ejemplo.
- Si la base ya contiene información, no se insertan registros nuevamente.

## Cómo ejecutar

1. Configurar la cadena de conexión en `appsettings.json`.
2. Ejecutar la API.
3. Ejecutar la aplicación MVC.

La base de datos se crea y carga automáticamente la primera vez.

## Usuario de prueba

**Usuario:** operador

**Contraseña:** operador123

## Autor

Matías Kreps