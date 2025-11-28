# 🎬 MovieApp API

API ASP.NET Core (.NET 9) pour rechercher des films via TMDB, gérer une watchlist et des playlists de films, le tout stocké en SQLite.
---

## ✅ Prérequis

- [.NET SDK 9](https://dotnet.microsoft.com/)
- SQLite (fichier `.db` local)
- Un compte TMDB + un **API Read Access Token (v4)**

Optionnel (pour les migrations EF Core) :

```bash
dotnet tool install --global dotnet-ef
```

## ⚙️ Configuration

### 1. Variables d’environnement (.env)

À la racine du projet (MovieAppApi/), créer un fichier :

```.env
TMDB_API_KEY=TON_TOKEN_V4_TMDB_ICI
```

Le chargement est fait au démarrage via DotNetEnv.Env.Load().

### 2. Connection string SQLite

Dans appsettings.json (ou appsettings.Development.json), ajouter :

```php
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=movieapp.db"
  }
}
```

L’API utilise cette connection string :

```php
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
```

La base SQLite (movieapp.db) sera créée automatiquement si elle n’existe pas.

## 🛠️ Packages utilisés

Les principaux packages NuGet (déjà référencés dans MovieAppApi.csproj) :
- DotNetEnv – chargement du .env
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Sqlite
- Microsoft.EntityFrameworkCore.Design
- Swashbuckle.AspNetCore – Swagger/OpenAPI
- Microsoft.AspNetCore.OpenApi

## 🚀 Lancer le projet

Dans le dossier du projet (MovieAppApi/) :

### 1. Restaurer les dépendances
```bash
dotnet restore
```
### 2. Appliquer les migrations EF (création de la base)
```bash
dotnet ef database update
```
### 3. Démarrer l’API
```bash
dotnet run
```

Par défaut (profil http), l’API écoute sur :

    http://localhost:5073

## 📚 Swagger / Documentation

Une fois l’API lancée en environnement Development, la doc Swagger est disponible ici :

    http://localhost:5073/swagger/index.html

## 🌐 Routes principales de l’API

Base URL : http://localhost:5073

### 🔍 Health

    GET /api/health
Retourne un petit objet JSON indiquant que l’API est vivante.

Exemple de réponse :
```json
{
  "status": "OK",
  "message": "MovieApp API is running!",
  "timestamp": "2025-11-28T09:15:00Z"
}
```
### 🎞️ Films (TMDB)
#### 1. Rechercher des films

    GET /api/movies?search_term=...&language=...

Query params :
- search_term (string, requis) : texte recherché
- language (string, requis, "en" ou "fr")

Exemple :

GET /api/movies?search_term=inception&language=fr


Réponse (exemple) :
```json
{
  "page": 1,
  "results": [
    {
      "id": 27205,
      "title": "Inception",
      "overview": "Dom Cobb est un voleur expérimenté...",
      "posterUrl": "https://image.tmdb.org/t/p/w500/abc123.jpg",
      "originalLanguage": "en",
      "releaseDate": "2010-07-15",
      "voteAverage": 8.4
    }
  ]
}
```
#### 2. Récupérer un film par ID TMDB

    GET /api/movies/{movieId}?language=...

Params :
    - movieId (int, requis) : ID du film TMDB
    - language (query, "en" ou "fr")

Exemple :

    GET /api/movies/550?language=en


200 OK → MovieDto
404 Not Found → "Movie id {movieId} not found" (si TMDB renvoie 404)

### 📺 Watchlist (liste perso de films)
#### 1. Récupérer toute la watchlist

    GET /api/watchlist

Réponse (exemple) :
```php
[
  {
    "movieId": 550,
    "title": "Fight Club",
    "posterUrl": "https://image.tmdb.org/t/p/w500/xyz.jpg",
    "addedAt": "2025-11-28T09:30:00Z",
    "isWatched": false
  }
]
```
#### 2. Ajouter un film à la watchlist

    POST /api/watchlist

Body :
```json
{
  "movieId": 550,
  "title": "Fight Club",
  "posterPath": "/xyz.jpg"
}
```

posterPath = chemin renvoyé par TMDB (/t/p/...), l’API reconstruit une URL complète.

Réponse : l’item ajouté.

#### 3. Supprimer un film de la watchlist

    DELETE /api/watchlist/{movieId}

204 No Content si supprimé
404 Not Found si non trouvé

#### 4. Marquer comme vu / non vu

        PATCH /api/watchlist/{movieId}/watched?isWatched=true|false

Exemple :

    PATCH /api/watchlist/550/watched?isWatched=true

### 🎶 Playlists de films

Les playlists contiennent des IDs TMDB de films.

#### 1. Créer une playlist

    POST /api/playlists

Body :
```json
{
  "name": "Mes films du week-end",
  "description": "Sélection chill",
  "movie_ids": [550, 27205]
}
```

Réponse : 201 Created + PlaylistDto
```json
{
  "id": 1,
  "name": "Mes films du week-end",
  "description": "Sélection chill",
  "movie_ids": [550, 27205]
}
```
#### 2. Lister toutes les playlists

    GET /api/playlists

Réponse :
```json
[
  {
    "id": 1,
    "name": "Mes films du week-end",
    "description": "Sélection chill",
    "movie_ids": [550, 27205]
  }
]
```
#### 3. Récupérer une playlist par ID

    GET /api/playlists/{playlistId}

200 OK → PlaylistDto
404 Not Found → "Playlist id {id} not found"

#### 4. Mettre à jour une playlist

    PUT /api/playlists/{playlistId}

Body :
```json
{
  "id": 1,
  "name": "Playlist mise à jour",
  "description": "Nouvelle sélection",
  "movie_ids": [550]
}
```

400 Bad Request si playlistId de la route ≠ id dans le body
200 OK + playlist mise à jour
404 Not Found si playlist inexistante

#### 5. Supprimer une playlist

    DELETE /api/playlists/{playlistId}

204 No Content si supprimée
404 Not Found si non trouvée