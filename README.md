
# Scoreboard API - Basketball Game Management

## Descripción
Scoreboard API es un proyecto de pruebas en C# (.NET 8+) para la gestión de partidos de baloncesto. Permite:
- Crear y administrar equipos y jugadores.
- Registrar partidos y controlar marcador, faltas, periodos y tiempo.
- Simular un partido completo desde un cliente REST (Postman, Frontend, etc.).

Utiliza Entity Framework Core con Code First y SQL Server.

## Estructura del Proyecto
- **Controllers**: Define los endpoints de Teams, Players y Games.
- **Data**: DbContext y configuración de la base de datos.
- **Models**: Modelos de datos (Team, Player, Game, etc.).
- **Repositories**: Repositorios genéricos y específicos para acceso a datos.
- **Services**: Lógica de negocio (GameService).
- **Program.cs**: Configuración principal del API, servicios, DbContext y Swagger.

## Modelos Principales

### Team
- `teamId` INT PK
- `name` NVARCHAR(100)
- `city` NVARCHAR(100)
- `logoUrl` NVARCHAR(100)

### Player
- `playerId` INT PK
- `teamId` INT FK Team
- `jerseyNumber` INT
- `fullName` NVARCHAR(100)
- `position` NVARCHAR(50)

### Game
- `gameId` INT PK
- `gameDate` DATETIME
- `homeTeamId` INT FK Team
- `awayTeamId` INT FK Team
- `homeScore` INT
- `awayScore` INT
- `currentPeriod` INT
- `remainingTime` INT (segundos)
- `periodStartTime` DATETIME
- `gameStatus` NVARCHAR(20) (NOT_STARTED, RUNNING, PAUSED, FINISHED)

### TeamFoul / PlayerFoul
- Registrar faltas de equipos y jugadores por periodo y partido.

## Endpoints Principales

### TeamsController
- `GET /api/teams`
- `GET /api/teams/{id}`
- `POST /api/teams`
- `PUT /api/teams/{id}`
- `DELETE /api/teams/{id}`

### PlayersController
- `GET /api/players`
- `GET /api/players/{id}`
- `POST /api/players`
- `PUT /api/players/{id}`
- `DELETE /api/players/{id}`

### GamesController
- `POST /api/games` Crear partido
- `GET /api/games/{id}` Consultar partido
- `POST /api/games/{id}/score/home` Añadir puntos al equipo local
- `POST /api/games/{id}/score/visitor` Añadir puntos al visitante
- `POST /api/games/{id}/score/home/decrement` Restar puntos al local
- `POST /api/games/{id}/score/visitor/decrement` Restar puntos al visitante
- Faltas equipo y jugador (`inc` / `dec`)
- Control de tiempo (`start`, `pause`, `resume`, `reset-period`)
- Avanzar o regresar periodos (`next-period`, `previous-period`)
- Reiniciar o suspender juego (`reset-game`, `suspend`)
- Guardar juego (`save`)

## Configuración de Program.cs

```csharp
using GameDataService.Data;
using GameDataService.Models;
using GameDataService.Repositories;
using GameDataService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext SQL Server
builder.Services.AddDbContext<ScoreboardDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection")
             ?? throw new InvalidOperationException("Connection string not found");
    options.UseSqlServer(cs);
});

// Repositories & Services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Crear/actualizar DB al arrancar (Code First)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ScoreboardDbContext>();
    db.Database.Migrate();

    if (!db.Teams.Any())
    {
        db.Teams.AddRange(
            new Team { Name = "Home", City = "Azul" },
            new Team { Name = "Visitor", City = "Rojo" }
        );
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
```

## Uso

1. Configura la cadena de conexión en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ScoreboardDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

2. Ejecuta el proyecto con:

```bash
dotnet run
```

3. Accede a Swagger UI: `https://localhost:5001/swagger/index.html`

4. Usa Postman o cualquier cliente REST para probar los endpoints.

5. Si borras la base de datos y vuelves a correr `dotnet run`, la base se creará automáticamente.

## Notas

- Para simplificar, se usa `ReferenceHandler.Preserve` en la serialización JSON para evitar ciclos.
- Todos los cambios se guardan automáticamente en cada acción del `GameService`.
- La colección de Postman incluye la simulación de un partido completo, con control de puntos, faltas y periodos.
