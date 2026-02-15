# Rick & Morty Telegram BOT 🤖🧪

<p align="center">
    <img src="https://raw.githubusercontent.com/robertodfj/rick-morty/refs/heads/main/gitReadMe.gif" alt="Rick & Morty GIF">
</p>

## 📖 Descripción

Proyecto **Rick & Morty Telegram BOT**: API REST y Bot en Telegram desarrollados en C# con .NET 7.  
Permite **gestionar usuarios y items**, incluyendo registro, login, captura, listado y comercio de personajes y episodios.  
Autenticación segura mediante **JWT**, con manejo de errores mediante middleware personalizado.  

El bot ofrece interacción directa con la API, mostrando información de usuarios, personajes y episodios de manera clara y formateada.

---

## 🔹 Tecnologías utilizadas

- **Lenguaje:** C#  
- **Framework:** .NET 7 (ASP.NET Core)  
- **Base de datos:** SQL Server  
- **Autenticación:** JWT (JSON Web Tokens)  
- **Pruebas:** Postman  
- **HTTP Cliente:** HttpClient para consumo de APIs externas  
- **Manejo de errores:** Middleware personalizado (ExceptionMiddleware)  
- **Bot Telegram:** Telegram.Bot library  

---

## 🔹 Estructura del proyecto

### API

| Carpeta / Archivo | Descripción |
|------------------|------------|
| `Program.cs` | Configuración principal de la aplicación, DI, JWT, middleware y rutas. |
| `data/` | Contexto de base de datos (`AppDBContext`), migraciones y `SeedData` para usuarios iniciales. |
| `dto/` | Data Transfer Objects para validar y enviar datos de usuarios y Pokémon. |
| `middleware/` | Middleware de manejo de errores y excepciones personalizadas. |
| `service/` | Lógica de negocio: `AuthService` y `PokemonService`. |
| `token/` | Generación y validación de tokens JWT (`GenerateToken`). |
| `model/` | Modelos de base de datos (`User`, `Pokemon`). |

### BOT

| Carpeta / Archivo | Descripción |
|------------------|------------|
| `commands/` | Clases con comandos ejecutables por el bot (registro, login, captura, venta, compra, etc.). |
| `handler/` | `BotUpdateHandler`: recibe y procesa las actualizaciones de Telegram. |
| `service/` | Servicios que interactúan con la API (`UserService`, etc.). |
| `token/` | Extracción y almacenamiento de tokens para autenticación. |
| `model/` | Modelos específicos del bot (`Character`, `Episode`, `UserInfo`). |

---

## 🔹 Funcionalidades del Bot

### Comandos principales:

- `/start` - Inicia el bot y muestra la bienvenida.  
- `/help` - Lista todos los comandos disponibles.  
- `/register` - Registra un nuevo usuario automáticamente.  
- `/login` - Inicia sesión y obtiene token JWT.  
- `/myInfo` - Muestra la información del usuario autenticado.  
- `/userInfo <username>` - Muestra información de otro usuario.  
- `/myCharacters` - Lista los personajes capturados por el usuario.  
- `/myEpisodes` - Lista los episodios capturados por el usuario.  
- `/charactersUser <username>` - Lista los personajes de otro usuario.  
- `/episodesUser <username>` - Lista los episodios de otro usuario.  
- `/captureCharacter` - Captura un personaje aleatorio.  
- `/captureEpisode` - Captura un episodio aleatorio.  
- `/sellCharacter <itemId> <price>` - Pone un personaje a la venta.  
- `/sellEpisode <itemId> <price>` - Pone un episodio a la venta.  
- `/cancelCharacterSell <itemId>` - Cancela la venta de un personaje.  
- `/cancelEpisodeSell <itemId>` - Cancela la venta de un episodio.  
- `/buyCharacter <itemId>` - Compra un personaje del mercado.  
- `/buyEpisode <itemId>` - Compra un episodio del mercado.  
- `/viewMarket` - Visualiza todos los items disponibles en el mercado.  
- `/work` - Trabaja para ganar dinero y aumentar probabilidades de captura.  
- `/editUsername <newUsername>` - Cambia el nombre de usuario (en desarrollo).  

### Funciones destacadas

- Formateo de datos para Telegram con **Markdown**, mostrando personajes y episodios de manera clara.  
- Manejo de errores y mensajes de respuesta amigables para el usuario.  
- Token JWT gestionado automáticamente y guardado por usuario.  
- Funciones de comercio (`sell`, `buy`, `cancel`) completamente integradas con la API.  

---

## 🔹 Pruebas realizadas

Todas las pruebas fueron manuales usando **Postman**, incluyendo:

[Archivo de pruebas - POSTMAN 🐦‍🔥](https://github.com/robertodfj/rick-morty/blob/main/PruebasPostman.md)

---

## ⚡ Observaciones finales

- Código modular y limpio: DTOs, servicios y middleware separados.  
- JWT + roles correctamente implementados.  
- Uso de `HttpClient` para consumir APIs externas.  
- Validaciones robustas y adecuadas para producción.  
- Separación de responsabilidades en el BOT (`commands`, `handler`, `service`, `token`).  
- Respuestas mapeadas para una interacción precisa con el usuario.  
- Validación de datos y manejo de errores en el BOT.  

---

## 👤 Creador y licencia

Creado por **Roberto de Frutos Jiménez**.  
Proyecto con fines educativos, **sin licencia específica**.