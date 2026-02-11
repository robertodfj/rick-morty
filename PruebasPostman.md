<h1 align="center">Pruebas de la api 🐦‍🔥</h1>

<p align="center">
  <img src="https://raw.githubusercontent.com/robertodfj/rick-morty/refs/heads/main/Postman_(software).png" alt="Logo de post man">
</p>

<p align="center">
Pruebas de la API REST realizadas completamente en POSTMAN.
</p>
<p align="center">
En este archivo se encuentran de manera organizada las pruebas realizadas antes de empaquetar y hacer el BOT de Telegram.
</p>

### 🔹 Tecnologías utilizadas
*	Autenticación: JWT (JSON Web Tokens)
*	Pruebas: Postman
*	Manejo de errores: Middleware personalizado (ExceptionMiddleware)

### 🔹 Estructura del proyecto

| Pruebas | Descripción|
| ------------- |:-------------:|
| Auth| Pruebas de login y registro.
| User | Obtener informacion de users y trabajar.
| Character |Compra, venta e informacion de characters.
| Episode| Compra, venta e informacion de episodes.


`Base URL: http://localhost:5235`

--------------------------------------------------

## 1️⃣ Autenticación de usuarios
### 1.1 POST /auth/register ✅

POST http://localhost:5235/auth/register

Body (JSON):
```
{
  "username": "testuser",
  "email": "testuser@gmail.com",
  "password": "1234567890",
  "confirmPassword": "1234567890"
}
```

Respuesta esperada (200 OK):
```
{
  "message": "User registered successfully"
}
```
Pruebas realizadas de validaciones:
+ Falta de informacion en el JSON:
```
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
        "$": [
            "JSON deserialization for type 'RickYMorty.dto.auth.RegisterDTO' was missing required properties including: 'email'; 'confirmPassword'."
        ],
        "registerDTO": [
            "The registerDTO field is required."
        ]
    },
    "traceId": "00-ceeebc3f69dd6d40d2214bb4efa1e5df-f81a9321f142fbd2-00"

}
```
+ Validaciones del DTO incorrectas:
```
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
        "Password": [
            "The field Password must be a string with a minimum length of 10 and a maximum length of 100."
        ],
        "ConfirmPassword": [
            "The field ConfirmPassword must be a string with a minimum length of 10 and a maximum length of 100."
        ]
    },
    "traceId": "00-cf781f88fffed9bf7542591d1791b6df-c1cd7ae6e807d11c-00"
}
```

### 1.2 POST /auth/login ✅

POST http://localhost:5235/auth/login

Body (JSON):
```
{
  "username": "testuser",
  "password": "123456"
}
```
Respuesta esperada (200 OK):
```
{
    "token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidGVzdHVzZXIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0dXNlckBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiZXhwIjoxNzcwODEzODg4fQ._KqbBUxfIml6vk7PTCikCFPv0uIXvX1uMOIJrj48LHY"
}
```

⚠️ IMPORTANTE:
+ Copiar el token y usarlo en las siguientes peticiones protegidas.

### 1.3 POST /auth/register-admin ✅

⚠️ Requiere rol Admin

POST http://localhost:5235/auth/register-admin

Body (JSON):
```
{
  "username": "admin2",
  "password": "123456"
}
```

Respuesta esperada (200 OK):
```
{
  "message": "Admin registered successfully"
}
```

Prueba de validacion:
401 Unauthorized
```
{
    "message": "Authentication token is missing or invalid"
}
```

--------------------------------------------------
## 2️⃣ Pruebas de User
### 2.1 GET /user/my-info ✅

GET http://localhost:5235/user/my-info

Respuesta esperada (200 OK):
```
{
    "username": "testuser",
    "characters": [
        {
            "id": 1,
            "name": "Rick Sanchez",
            "status": "Alive",
            "species": "Human",
            "gender": "Male",
            "forSale": false,
            "price": 0
        },
        {
            "id": 2,
            "name": "Morty Smith",
            "status": "Alive",
            "species": "Human",
            "gender": "Male",
            "forSale": false,
            "price": 0
        }
    ],
    "episodes": [
        {
            "id": 2,
            "name": "Lawnmower Dog",
            "airDate": "2026-02-11",
            "episode": "S01E02",
            "characters": [
                "https://rickandmortyapi.com/api/character/1",
                "https://rickandmortyapi.com/api/character/2",
                "https://rickandmortyapi.com/api/character/38",
                "https://rickandmortyapi.com/api/character/46",
                "https://rickandmortyapi.com/api/character/63",
                "https://rickandmortyapi.com/api/character/80",
                "https://rickandmortyapi.com/api/character/175",
                "https://rickandmortyapi.com/api/character/221",
                "https://rickandmortyapi.com/api/character/239",
                "https://rickandmortyapi.com/api/character/246",
                "https://rickandmortyapi.com/api/character/304",
                "https://rickandmortyapi.com/api/character/305",
                "https://rickandmortyapi.com/api/character/306",
                "https://rickandmortyapi.com/api/character/329",
                "https://rickandmortyapi.com/api/character/338",
                "https://rickandmortyapi.com/api/character/396",
                "https://rickandmortyapi.com/api/character/397",
                "https://rickandmortyapi.com/api/character/398",
                "https://rickandmortyapi.com/api/character/405"
            ],
            "url": "https://rickandmortyapi.com/api/episode/2",
            "created": "2017-11-10T12:56:33.916Z",
            "forSale": false,
            "price": 0
        }
    ],
    "money": 500,
    "lastWorked": null
}
```
Pruebas de validación:
 + 401 Unauthorized si no se incluye el token:
```
{
  "message": "Username claim not found."
}
```
### 2.2 GET /user/info/{username} ✅

GET http://localhost:5235/user/info/testuser

```
Respuesta esperada (200 OK):

{
    "username": "testuser",
    "characters": [
        {
            "id": 1,
            "name": "Rick Sanchez",
            "status": "Alive",
            "species": "Human",
            "gender": "Male",
            "forSale": false,
            "price": 0
        },
        {
            "id": 2,
            "name": "Morty Smith",
            "status": "Alive",
            "species": "Human",
            "gender": "Male",
            "forSale": false,
            "price": 0
        }
    ],
    "episodes": [
        {
            "id": 2,
            "name": "Lawnmower Dog",
            "airDate": "2026-02-11",
            "episode": "S01E02",
            "characters": [
                "https://rickandmortyapi.com/api/character/1",
                "https://rickandmortyapi.com/api/character/2",
                "https://rickandmortyapi.com/api/character/38",
                "https://rickandmortyapi.com/api/character/46",
                "https://rickandmortyapi.com/api/character/63",
                "https://rickandmortyapi.com/api/character/80",
                "https://rickandmortyapi.com/api/character/175",
                "https://rickandmortyapi.com/api/character/221",
                "https://rickandmortyapi.com/api/character/239",
                "https://rickandmortyapi.com/api/character/246",
                "https://rickandmortyapi.com/api/character/304",
                "https://rickandmortyapi.com/api/character/305",
                "https://rickandmortyapi.com/api/character/306",
                "https://rickandmortyapi.com/api/character/329",
                "https://rickandmortyapi.com/api/character/338",
                "https://rickandmortyapi.com/api/character/396",
                "https://rickandmortyapi.com/api/character/397",
                "https://rickandmortyapi.com/api/character/398",
                "https://rickandmortyapi.com/api/character/405"
            ],
            "url": "https://rickandmortyapi.com/api/episode/2",
            "created": "2017-11-10T12:56:33.916Z",
            "forSale": false,
            "price": 0
        }
    ],
    "money": 500,
    "lastWorked": null
}
```
Pruebas de validación:
+ 404 Not Found si el usuario no existe:
```
{
    "message": "User not found"
}
```

### 2.3 GET /user/work ✅

GET http://localhost:5235/user/work

Respuesta esperada (200 OK):
```
{
    "earnedMoney": "User testuser has worked and earned 100 money. Total money: 600"
}
```
Pruebas de validación:
+ 409 Conflict si trabajaste hace poco
```
{
    "message": "You have already worked recently. Please wait 15 more minutes before working again."
}
```
+ 401 Unauthorized si no se incluye el token:
```
{
  "message": "User ID claim not found."
}
```
-----------------------------------------------
## 3️⃣ Pruebas de Character
### 3.1 POST /characters/capture ✅

POST http://localhost:5235/characters/capture


Body (JSON):
```
{
  "id": 1
}
```

Respuesta esperada (200 OK):
```
{
  "id": 1,
  "name": "Rick Sanchez",
  "status": "Alive",
  "species": "Human",
  "gender": "Male",
  "ownedByUserId": 1,
  "forSale": false,
  "price": 0
}
```

Pruebas de validación:
+ 401 Unauthorized si no se incluye el token:
```
{
  "message": "Authentication token is missing or invalid"
}
```
+ 409 Conflict si el % de captura no devuelve true en el service
```
{
    "message": "Capture failed. Keep working to increase your chances!"
}
```
+ 409 Conflict si el personaje ya fue capturado
```

{
    "message": "Character already owned by user 1"
}
```
### 3.2 GET /characters/my-characters ✅

GET http://localhost:5235/characters/my-characters

Respuesta esperada (200 OK):
```
[
  {
    "id": 1,
    "name": "Rick Sanchez",
    "status": "Alive",
    "species": "Human",
    "gender": "Male",
    "ownedByUserId": 1,
    "forSale": false,
    "price": 0
  },
  {
    "id": 2,
    "name": "Morty Smith",
    "status": "Alive",
    "species": "Human",
    "gender": "Male",
    "ownedByUserId": 1,
    "forSale": false,
    "price": 0
  }
]
```

Pruebas de validación:
+ 401 Unauthorized si no se incluye token.

### 3.3 GET /characters/{username} ✅

GET http://localhost:5235/characters/testuser

Respuesta esperada (200 OK):
```
[
  {
    "id": 1,
    "name": "Rick Sanchez",
    "status": "Alive",
    "species": "Human",
    "gender": "Male",
    "ownedByUserId": 1,
    "forSale": false,
    "price": 0
  }
]
```
Pruebas de validación:
+ 404 Not Found si el usuario no existe.
```
{
    "message": "User not found"
}
```
### 3.4 GET /characters/for-sale ✅

GET http://localhost:5235/characters/for-sale


Respuesta esperada (200 OK):
```
[
  {
    "id": 3,
    "name": "Birdperson",
    "status": "Dead",
    "species": "Alien",
    "gender": "Male",
    "ownedByUserId": 2,
    "forSale": true,
    "price": 500
  }
]
```

### 3.5 POST /characters/put-for-sale ✅

POST http://localhost:5235/characters/put-for-sale

Body (JSON):
```
{
  "itemId": 1,
  "price": 100
}
```

Respuesta esperada (200 OK):
```
Character with ID 2 is now for sale at price 100.
```
Pruebas de validación:
+ 401 Unauthorized si no se incluye token.
+ 400 Bad Request si el personaje no pertenece al usuario.
```
{
    "message": "Character not found or you do not own this character."
}
```
+ 409 Conflict si el Character ya esta a la venta
```
{
    "message": "Character is already for sale."
}
```

### 3.6 POST /characters/buy ✅

POST http://localhost:5235/characters/buy

Body (JSON):
```
1
```
Respuesta esperada (200 OK):
```
{
  "message": "Character purchased successfully",
  "characterId": 1,
  "buyerId": 1,
  "price": 100
}
```
Pruebas de validación:
+ 401 Unauthorized si no se incluye token.
+ 400 Bad Request si no hay suficiente dinero o el personaje no está a la venta.
```
{
  "message": "Character not found or not for sale."
}
```
+ 409 No puedes comprar tu propio character
```
{
  "message": "You cannot buy your own character."
}
```
--------
## 4️⃣ Pruebas de Episode
### 4.1 POST /episodes/capture ✅

POST http://localhost:5235/episodes/capture

Body (JSON):
```
{
  "id": 1
}
```

Respuesta esperada (200 OK):
```
{
    "id": 2,
    "name": "Lawnmower Dog",
    "airDate": "2026-02-11",
    "episode": "S01E02",
    "characters": [
        "https://rickandmortyapi.com/api/character/1",
        "https://rickandmortyapi.com/api/character/2",
        "https://rickandmortyapi.com/api/character/38",
        "https://rickandmortyapi.com/api/character/46",
        "https://rickandmortyapi.com/api/character/63",
        "https://rickandmortyapi.com/api/character/80",
        "https://rickandmortyapi.com/api/character/175",
        "https://rickandmortyapi.com/api/character/221",
        "https://rickandmortyapi.com/api/character/239",
        "https://rickandmortyapi.com/api/character/246",
        "https://rickandmortyapi.com/api/character/304",
        "https://rickandmortyapi.com/api/character/305",
        "https://rickandmortyapi.com/api/character/306",
        "https://rickandmortyapi.com/api/character/329",
        "https://rickandmortyapi.com/api/character/338",
        "https://rickandmortyapi.com/api/character/396",
        "https://rickandmortyapi.com/api/character/397",
        "https://rickandmortyapi.com/api/character/398",
        "https://rickandmortyapi.com/api/character/405"
    ],
    "url": "https://rickandmortyapi.com/api/episode/2",
    "created": "2017-11-10T12:56:33.916Z",
    "forSale": false,
    "price": 0
}
```
Pruebas de validación:
+ 401 Unauthorized si no se incluye token
+ 409 Conflict si no se puede capturar
```
{
    "message": "Capture failed. Keep working to increase your chances!"
}
```
⸻

### 4.2 GET /episodes/my-episodes ✅

GET http://localhost:5235/episodes/my-episodes

```
Respuesta esperada (200 OK):
[
    {
        "id": 2,
        "name": "Lawnmower Dog",
        "airDate": "2026-02-11",
        "episode": "S01E02",
        "characters": [
            "https://rickandmortyapi.com/api/character/1",
            "https://rickandmortyapi.com/api/character/2",
            "https://rickandmortyapi.com/api/character/38",
            "https://rickandmortyapi.com/api/character/46",
            "https://rickandmortyapi.com/api/character/63",
            "https://rickandmortyapi.com/api/character/80",
            "https://rickandmortyapi.com/api/character/175",
            "https://rickandmortyapi.com/api/character/221",
            "https://rickandmortyapi.com/api/character/239",
            "https://rickandmortyapi.com/api/character/246",
            "https://rickandmortyapi.com/api/character/304",
            "https://rickandmortyapi.com/api/character/305",
            "https://rickandmortyapi.com/api/character/306",
            "https://rickandmortyapi.com/api/character/329",
            "https://rickandmortyapi.com/api/character/338",
            "https://rickandmortyapi.com/api/character/396",
            "https://rickandmortyapi.com/api/character/397",
            "https://rickandmortyapi.com/api/character/398",
            "https://rickandmortyapi.com/api/character/405"
        ],
        "url": "https://rickandmortyapi.com/api/episode/2",
        "created": "2017-11-10T12:56:33.916Z",
        "forSale": false,
        "price": 0
    }
]
```
Pruebas de validación:
+ 401 Unauthorized si no se incluye token

### 4.3 GET /episodes/{username} ✅

GET http://localhost:5235/episodes/testuser
```
Respuesta esperada (200 OK):
[
    {
        "id": 2,
        "name": "Lawnmower Dog",
        "airDate": "2026-02-11",
        "episode": "S01E02",
        "characters": [
            "https://rickandmortyapi.com/api/character/1",
            "https://rickandmortyapi.com/api/character/2",
            "https://rickandmortyapi.com/api/character/38",
            "https://rickandmortyapi.com/api/character/46",
            "https://rickandmortyapi.com/api/character/63",
            "https://rickandmortyapi.com/api/character/80",
            "https://rickandmortyapi.com/api/character/175",
            "https://rickandmortyapi.com/api/character/221",
            "https://rickandmortyapi.com/api/character/239",
            "https://rickandmortyapi.com/api/character/246",
            "https://rickandmortyapi.com/api/character/304",
            "https://rickandmortyapi.com/api/character/305",
            "https://rickandmortyapi.com/api/character/306",
            "https://rickandmortyapi.com/api/character/329",
            "https://rickandmortyapi.com/api/character/338",
            "https://rickandmortyapi.com/api/character/396",
            "https://rickandmortyapi.com/api/character/397",
            "https://rickandmortyapi.com/api/character/398",
            "https://rickandmortyapi.com/api/character/405"
        ],
        "url": "https://rickandmortyapi.com/api/episode/2",
        "created": "2017-11-10T12:56:33.916Z",
        "forSale": false,
        "price": 0
    }
]
```
Pruebas de validación:
+ 404 Not Found si el usuario no existe
```
{
    "message": "User not found"
}
```
### 4.4 GET /episodes/for-sale ✅

GET http://localhost:5235/episodes/for-sale

Respuesta esperada (200 OK):
```
[
  {
    "id": 2,
    "name": "Lawnmower Dog",
    "episodeCode": "S01E02",
    "airDate": "2013-12-09",
    "ownedByUserId": 2,
    "forSale": true,
    "price": 250
  }
]
```

### 4.5 POST /episodes/put-for-sale ✅

POST http://localhost:5235/episodes/put-for-sale


Body (JSON):
```
{
  "itemId": 1,
  "price": 100
}
```

Respuesta esperada (200 OK):
```
Episode with ID 1 is now for sale at price 100.
```
Pruebas de validación:
+ 401 Unauthorized si no se incluye token
+ 400 Bad Request si el episodio no pertenece al usuario
```
{
    "message": "Character not found or you do not own this character."
}
```
409 Conflict
```
{
    "message": "Episode is already for sale."
}
```
### 4.6 POST /episodes/buy ✅

POST http://localhost:5235/episodes/buy

Body (JSON):
```
1
```
Respuesta esperada (200 OK):
```
{
  "message": "Episode purchased successfully",
  "episodeId": 1,
  "buyerId": 1,
  "price": 100
}
```

Pruebas de validación:
+ 401 Unauthorized si no se incluye token
+ 400 Bad Request si no hay suficiente dinero o el episodio no está a la venta
+ 409 No puedes comprar tu propio episodio
```
{
    "message": "Episode not found or not for sale."
}
```
---

## ⚡ Observaciones finales
+ Código modular y limpio: Funciona perfectamente
+ JWT + roles correctamente implementados
+ Middleware personalizado muy util para obtener informacion más rapidamente
+ Validaciones correctas y robustas para producción 

+ El proyecto está `listo para` ser extendido a frontend en `Telegram BOT`

## Creador y licencia

Creado por `Roberto de Frutos Jiménez` sin licencia, con fines educativos.
