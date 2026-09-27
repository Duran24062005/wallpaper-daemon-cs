# Migración de `wallpaper-rs` a `wallpaper-daemon-cs`

## Resumen

Migrar el MVP actual de Rust a C#/.NET en `wallpaper-daemon-cs`, manteniendo el comportamiento en Linux con GNOME:

- Escanear `assets` buscando `.jpg`, `.jpeg`, `.png` y `.webp`.
- Leer el fondo actual mediante `gsettings`.
- Elegir aleatoriamente una imagen distinta.
- Aplicarla mediante `gsettings`.
- Repetir el ciclo cada 10 segundos.
- Continuar ejecutándose aunque falle un ciclo individual.

Se tomará como origen `wallpaper-rs`; `wallpaper-rs-2` no forma parte de esta migración.

## Cambios de implementación

- Completar el proyecto `.NET 10` existente.
- Mantener la separación actual por servicios:
  - `WallpaperScanner`: enumeración no recursiva y filtrado por extensión.
  - `WallpaperSelector`: selección aleatoria excluyendo el fondo actual.
  - `GnomeWallpaperService`: ejecución de `/usr/bin/gsettings`, lectura de `picture-uri-dark`, validación de URI `file://` y actualización del fondo.
  - `Scheduler`: espera configurable mediante `Task.Delay` o equivalente.
- Añadir modelos/excepciones para representar configuración y errores de escaneo, URI, proceso externo y ausencia de alternativas.
- Implementar `Program.cs` como orquestador del ciclo continuo, usando configuración por defecto:
  - Directorio: `assets`
  - Intervalo: 10 segundos
- Conservar el tratamiento de rutas relativas y absolutas de Rust, canonicalizando la imagen antes de enviarla a GNOME.
- Actualizar `README.md` con requisitos, ejecución, estructura, limitaciones y comandos de prueba.
- No implementar en esta fase el roadmap de Rust: CLI `start/stop/next/status`, configuración persistente, D-Bus, multi-monitor ni soporte adicional de escritorios.

## Pruebas y aceptación

Crear pruebas automatizadas para:

- Detectar únicamente extensiones de imagen válidas, ignorando mayúsculas/minúsculas.
- Ignorar directorios y archivos no soportados.
- Reportar correctamente un directorio inexistente.
- Seleccionar una imagen distinta al fondo actual.
- Permitir seleccionar la única imagen disponible cuando no existe alternativa.
- Parsear correctamente URI como `'file:///tmp/example.jpg'`.
- Rechazar valores que no comiencen por `file://`.
- Verificar que los errores de `gsettings` se convierten en errores de dominio.
- Compilar y ejecutar el proyecto con `dotnet build` y ejecutar las pruebas con `dotnet test`.

La aceptación final será que el daemon pueda iniciarse desde la raíz de `wallpaper-daemon-cs`, cambie el fondo cada 10 segundos usando la carpeta `assets` y registre los errores sin terminar por un fallo aislado de un ciclo.

## Supuestos

- El sistema objetivo es Linux con GNOME y `gsettings` disponible.
- Se conserva el uso exclusivo de `picture-uri-dark`, igual que la implementación fuente.
- El proyecto C# existente está vacío funcionalmente y sus servicios actuales son esqueletos destinados a completarse.
- Las imágenes de prueba se copiarán o mantendrán en `wallpaper-daemon-cs/assets`.
- No se modificarán los proyectos Rust originales.
