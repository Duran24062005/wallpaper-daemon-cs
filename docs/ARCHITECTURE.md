# Arquitectura

## Flujo principal

`Program` ejecuta un ciclo cada 10 segundos:

1. `WallpaperScanner` obtiene archivos de imagen de `assets/` sin entrar en subdirectorios.
2. `GnomeWallpaperService` consulta el fondo actual mediante `gsettings`.
3. `WallpaperSelector` elige una imagen al azar y evita la actual.
4. `GnomeWallpaperService` aplica la imagen mediante `picture-uri-dark`.
5. `Scheduler` espera hasta el siguiente ciclo.

Los errores de dominio se registran y el daemon continúa con el siguiente ciclo.

## Componentes

- `Models/WallpaperConfiguration.cs`: valores predeterminados del MVP.
- `Services/WallpaperScanner.cs`: descubrimiento de imágenes.
- `Services/WallpaperSelector.cs`: selección aleatoria.
- `Services/GnomeWallpaperService.cs`: integración con GNOME y proceso externo.
- `Services/WallpaperExceptions.cs`: errores esperados del dominio.
- `Services/Scheduler.cs`: espera asíncrona.
- `tests/`: pruebas unitarias sin depender de una sesión GNOME real.

## Decisiones y límites actuales

- El backend es GNOME mediante `/usr/bin/gsettings`.
- Solo se escribe `org.gnome.desktop.background/picture-uri-dark`.
- La carpeta y el intervalo están definidos en código: `assets` y 10 segundos.
- No se incluyen todavía CLI, configuración persistente, D-Bus, multi-monitor ni otros escritorios.
