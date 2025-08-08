# Directory Site

Sitio web ASP.NET desarrollado para visualización, edición y administración del directorio de Fiscalía Digital.

---

## 🚀 Tecnologías utilizadas

- .NET 8+
- ASP.NET Core Web API
---

## Instalación y configuración
### Requisitos

- .NET SDK 8
- Visual Studio 2022
- SQL Server (local o remoto)
- Git

### Instalacion
Clona el repositorio:
git clone [https://github.com/usuario/nombre-del-repo.git](https://github.com/JuanRangel-FGJTam/DirectorySite.git)

Restaura los paquetes NuGet:

```bash
dotnet restore
```

### Configuracion

Este sitio requiere la comunicación con la API de Fiscalía Digital para su funcionamiento. Toda la lógica del sistema se gestiona 
a través de dicha API, por lo que es necesario especificar su URL correctamente, así como proporcionar las credenciales utilizadas
para la firma de JWT. Estas credenciales deben coincidir exactamente con las registradas en la API para garantizar una integración exitosa.

```jsonc
// appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "DirectoryAPI":"https://api-usersfd.fgjtam.gob.mx/",
  "JwtSettings":{
    "Issuer": "http://usersfd.fgjtam.gob.mx/",
    "Audience":"*.fgjtam.gob.mx",
    "Key":"<my-secret-key>"
  }
}
```

### Ejecucion
Par ejecutar el programa y comprobar su funcionamiento se debe abrir una terminal, acceder a la ruta del proyecto y ejecutar el comando:

```bash
dotnet run
```


---
## Hospedar

### Ubuntu
Para mas informacion de como Hospedar una aplicacion .NET en Linux, consultar [official documentation](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-8.0&tabs=linux-ubuntu).

- Crear el archivo del Servicio

`sudo vim /etc/systemd/system/api-usersfd.service`

```bash
[Unit]
Description=API Fiscalia Digital

[Service]
WorkingDirectory=/var/www/DirectoryApi
ExecStart=/usr/bin/dotnet /var/www/DirectoryApi/bin/Release/net8.0/AuthApi.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-example
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_NOLOGO=true

[Install]
WantedBy=multi-user.target
```

- Save the file and enable the service.

`sudo systemctl enable api-usersfd.service`

- Show the logs

`sudo journalctl -fu api-usersfd.service`

---
## Publish

### Ubuntu
1. Ensure .NET runtime is installed:
2. Navigate to the directory containing the *.csproj file.
3. Run the following command to publish the application:

> `dotnet publish --configuration Release`

This command generates the publish folder in the directory, e.g., `bin/Release/net8.0/publish`.

For apply changes run previous command and reload service

`sudo systemctl daemon-reload`

`sudo systemctl restart api-usersfd.service`


---
## Contributors

- [JuanRangel-FGJTam](https://github.com/JuanRangel-FGJTam)
