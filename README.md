# wallpaper-daemon-cs

Daemon de fondos de pantalla para GNOME en Linux, migrado desde `wallpaper-rs` a .NET 10.

## Requisitos

- Linux con sesión GNOME.
- `/usr/bin/gsettings` disponible.
- .NET SDK 10.

## Uso

Desde la raíz de este proyecto, con imágenes en `assets/`:

```bash
dotnet run
```

El programa escanea de forma no recursiva las extensiones `.jpg`, `.jpeg`, `.png` y `.webp`, lee `picture-uri-dark`, selecciona una imagen distinta y la aplica. El ciclo se repite cada 10 segundos. Los errores de un ciclo se registran y no detienen el daemon. Se detiene con `Ctrl+C`.

## Pruebas

```bash
dotnet build wallpaper-daemon-cs.csproj
dotnet test tests/wallpaper-daemon-cs.Tests.csproj
```

Las pruebas cubren escaneo, selección sin repetición, parsing de URI y errores del comando `gsettings` sin requerir una sesión GNOME real.

## Limitaciones

La configuración permanece fija en `assets` y 10 segundos. Solo se actualiza `picture-uri-dark`; no se incluyen CLI de control, configuración persistente, D-Bus, multi-monitor ni otros escritorios.

## Estructura

```text
Models/       configuración
Services/     escaneo, selección, GNOME y scheduler
tests/        pruebas unitarias
docs/         arquitectura y mantenimiento
assets/       imágenes usadas por el daemon
```

Para conocer el flujo interno consulta [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md). Para el flujo de mantenimiento consulta [docs/CONTRIBUTING.md](docs/CONTRIBUTING.md).



```cmd
├── assets
│   ├── 1.jpeg
│   ├── 2.jpeg
│   ├── 3.jpeg
│   ├── 4.jpeg
│   ├── 5.jpeg
│   ├── 6.jpeg
│   └── 7.jpeg
├── docs
│   ├── app.md
│   ├── ARCHITECTURE.md
│   └── CONTRIBUTING.md
├── Models
│   └── WallpaperConfiguration.cs
├── Program.cs
├── Properties
│   └── AssemblyInfo.cs
├── README.md
├── Services
│   ├── GnomeWallpaperService.cs
│   ├── Scheduler.cs
│   ├── WallpaperExceptions.cs
│   ├── WallpaperScanner.cs
│   └── WallpaperSelector.cs
├── tests
│   ├── UnitTest1.cs
│   └── wallpaper-daemon-cs.Tests.csproj
└── wallpaper-daemon-cs.csproj

7 directories, 22 files
