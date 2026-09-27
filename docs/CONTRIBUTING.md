# Mantenimiento del repositorio

Este es un repositorio individual. Los cambios deben mantenerse pequeños, verificables y documentados cuando modifiquen el flujo del daemon.

## Flujo local

Desde `wallpaper-daemon-cs/`:

```bash
dotnet restore
dotnet build
dotnet test tests/wallpaper-daemon-cs.Tests.csproj
```

Antes de probar el cambio real, confirma que existe una sesión GNOME y que `gsettings` está disponible:

```bash
command -v gsettings
dotnet run
```

## Convención de commits

Usa commits pequeños con el formato definido por el proyecto, por ejemplo:

```text
feat: :sparkles: add wallpaper rotation configuration
fix: :bug: handle missing wallpaper directory
docs: :memo: document gsettings integration
test: :white_check_mark: cover URI parsing
```

No incluyas `bin/`, `obj/`, resultados de pruebas, archivos de IDE ni secretos. Estos archivos están excluidos por `.gitignore`.

## Criterios de cambio

- Añade o actualiza pruebas para cada comportamiento nuevo.
- Mantén la integración con `gsettings` aislada en `GnomeWallpaperService`.
- Evita ampliar el alcance del MVP sin actualizar la documentación de arquitectura.
