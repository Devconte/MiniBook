# 🚀 MiniBook - Application Réseau Social

MiniBook est une application web moderne de réseau social développée avec **ASP.NET Core 9** et **Angular 17**. Elle permet aux utilisateurs de créer des comptes, publier des posts, commenter, et gérer leurs relations amicales.

## 📋 Table des Matières

- [🎯 Vue d'ensemble](#-vue-densemble)
- [🏗️ Architecture](#️-architecture)
- [📁 Structure du Projet](#-structure-du-projet)
- [🚀 Démarrage Rapide](#-démarrage-rapide)
- [🐳 Docker](#-docker)
- [🧪 Tests](#-tests)
- [🔄 CI/CD Pipeline](#-cicd-pipeline)
- [📚 API Documentation](#-api-documentation)
- [🔧 Configuration](#-configuration)
- [📊 Monitoring](#-monitoring)

---

## 🎯 Vue d'ensemble

### Fonctionnalités Principales

- ✅ **Authentification** : Inscription, connexion, gestion des sessions
- ✅ **Posts** : Publication, lecture, gestion des messages
- ✅ **Commentaires** : Commenter les posts des autres utilisateurs
- ✅ **Amis** : Système de demande d'amitié et gestion des relations
- ✅ **Administration** : Rôle admin pour la gestion des utilisateurs
- ✅ **Responsive** : Interface adaptée mobile et desktop

### Technologies Utilisées

| **Backend**           | **Frontend** | **Base de Données** | **DevOps**     |
| --------------------- | ------------ | ------------------- | -------------- |
| ASP.NET Core 9        | Angular 17   | SQL Server 2022     | Docker         |
| Entity Framework Core | TypeScript   | Migrations          | GitHub Actions |
| Cookie Authentication | RxJS         |                     | Docker Compose |
| CORS                  | HTTP Client  |                     |                |

---

## 🏗️ Architecture

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Frontend      │    │   Backend       │    │   Database      │
│   Angular 17    │◄──►│   ASP.NET Core  │◄──►│   SQL Server    │
│   Port: 4200    │    │   Port: 5001    │    │   Port: 1433    │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

### Flux de Données

1. **Frontend** (Angular) → Requêtes HTTP → **Backend** (ASP.NET Core)
2. **Backend** → Entity Framework → **Base de Données** (SQL Server)
3. **Authentification** via cookies sécurisés
4. **CORS** configuré pour permettre les requêtes cross-origin

---

## 📁 Structure du Projet

```
MiniBook/
├── 📁 MiniBook/                    # Backend ASP.NET Core
│   ├── 📁 MiniBook/               # Projet principal
│   │   ├── 📁 Controllers/        # API Controllers
│   │   ├── 📁 Models/             # Entités de données
│   │   ├── 📁 Data/               # DbContext & Migrations
│   │   ├── 📁 Views/              # Vues MVC (optionnelles)
│   │   ├── 📄 Program.cs          # Configuration principale
│   │   ├── 📄 Dockerfile          # Image Docker backend
│   │   └── 📄 docker-compose.yml  # Orchestration Docker
│   ├── 📁 MiniBook.Tests/         # Tests unitaires MSTest
│   └── 📁 .github/workflows/      # Pipeline CI/CD
├── 📁 minibookfront/              # Frontend Angular
│   ├── 📁 src/app/               # Application Angular
│   │   ├── 📁 auth/              # Composants d'authentification
│   │   ├── 📁 posts/             # Composants des posts
│   │   ├── 📁 services/          # Services HTTP
│   │   └── 📁 models/            # Interfaces TypeScript
│   ├── 📄 package.json           # Dépendances Node.js
│   └── 📄 angular.json           # Configuration Angular
├── 📁 diagrammes/                 # Diagrammes UML (PlantUML)
├── 📄 PLAN_DE_TEST.md            # Plan de tests
└── 📄 TABLEAU_TESTS.md           # Résultats des tests
```

---

## 🚀 Démarrage Rapide

### Prérequis

- **.NET 9.0 SDK**
- **Node.js 18+** et **npm**
- **Docker** et **Docker Compose**
- **SQL Server** (via Docker)

### 1. Cloner le Projet

```bash
git clone <repository-url>
cd MiniBook
```

### 2. Backend (ASP.NET Core)

```bash
# Naviguer vers le backend
cd MiniBook/MiniBook

# Restaurer les packages
dotnet restore

# Lancer l'application
dotnet run
```

**🌐 Backend disponible sur :** `http://localhost:5001`

### 3. Frontend (Angular)

```bash
# Naviguer vers le frontend
cd minibookfront

# Installer les dépendances
npm install

# Lancer le serveur de développement
npm start
```

**🌐 Frontend disponible sur :** `http://localhost:4200`

### 4. Base de Données

La base de données SQL Server sera automatiquement créée et peuplée avec des données de test au premier lancement.

---

## 🐳 Docker

### Démarrage avec Docker Compose

```bash
# Naviguer vers le dossier backend
cd MiniBook/MiniBook

# Lancer tous les services
docker-compose up -d

# Vérifier le statut
docker-compose ps
```

### Services Docker

- **🐳 minibook-api** : Application ASP.NET Core (Port 5001)
- **🗄️ sql** : SQL Server 2022 (Port 1433)

### Commandes Utiles

```bash
# Voir les logs
docker-compose logs -f

# Arrêter les services
docker-compose down

# Reconstruire les images
docker-compose up --build
```

---

## 🧪 Tests

### Tests Unitaires (MSTest)

```bash
# Exécuter tous les tests
cd MiniBook/MiniBook.Tests
dotnet test

# Tests avec détails
dotnet test --verbosity normal

# Tests avec rapport
dotnet test --logger trx --results-directory TestResults/
```

### Tests Disponibles

- ✅ **UserTests** : Tests du modèle User (3 tests)
- ✅ **PostTests** : Tests du modèle Post (3 tests)
- ✅ **Total** : 6 tests unitaires

### Résultats des Tests

```bash
Test summary: total: 6, failed: 0, succeeded: 6, skipped: 0
```

---

## 🔄 CI/CD Pipeline

### GitHub Actions Workflow

Le pipeline automatique s'exécute sur :

- **Push** vers `main` ou `develop`
- **Pull Requests** vers `main`

### Étapes du Pipeline

1. **🏗️ Build & Test**

   - Checkout du code
   - Setup .NET 9.0
   - Restauration des packages
   - Compilation en mode Release
   - Exécution des tests unitaires
   - Upload des résultats de tests

2. **🐳 Docker Build**
   - Build de l'image Docker
   - Cache optimisé
   - Tags de version

### Configuration

```yaml
# Fichier : .github/workflows/ci-cd.yml
name: 🚀 MiniBook CI/CD Pipeline
on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]
```

---

## 📚 API Documentation

### Endpoints Principaux

#### 🔐 Authentification

```http
POST /api/account/login
POST /api/account/register
GET  /api/account/current
```

#### 📝 Posts

```http
GET    /api/posts
POST   /api/posts
GET    /api/posts/{id}
PUT    /api/posts/{id}
DELETE /api/posts/{id}
```

#### 🔧 Test

```http
GET /api/posts/test
```

### Exemple d'Utilisation

```typescript
// Login
const response = await fetch("http://localhost:5001/api/account/login", {
  method: "POST",
  headers: { "Content-Type": "application/json" },
  credentials: "include",
  body: JSON.stringify({
    email: "user@example.com",
    password: "password123",
  }),
});
```

---

## 🔧 Configuration

### Backend Configuration

```json
// appsettings.json
{
  "ConnectionStrings": {
    "Default": "Server=localhost,1433;Database=MiniBook;User Id=sa;Password=YourStrong@Passw0rd!;TrustServerCertificate=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Frontend Configuration

```typescript
// src/app/services/post.service.ts
private apiUrl = 'http://localhost:5001/api/posts';
```

### CORS Configuration

```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:5001")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
```

---

## 📊 Monitoring

### Logs

- **Backend** : Logs ASP.NET Core dans la console
- **Docker** : `docker-compose logs -f`
- **Tests** : Fichiers `.trx` dans `TestResults/`

### Health Checks

- **Backend** : `http://localhost:5001/health`
- **Database** : Vérification automatique au démarrage

### Métriques

- **Tests** : 6/6 passent (100% de réussite)
- **Build Time** : ~2-3 secondes
- **Docker Build** : ~30-60 secondes

---

## 🛠️ Développement

### Ajouter une Nouvelle Fonctionnalité

1. **Backend** : Créer le modèle, controller, migration
2. **Frontend** : Créer le service, composant, interface
3. **Tests** : Ajouter les tests unitaires
4. **Commit** : Le pipeline CI/CD s'exécutera automatiquement

### Commandes Utiles

```bash
# Backend
dotnet ef migrations add <NomMigration>
dotnet ef database update

# Frontend
ng generate component <nom-composant>
ng generate service <nom-service>

# Tests
dotnet test --filter "UserTests"
```

---

## 📞 Support

### Problèmes Courants

1. **Port déjà utilisé** : Changer le port dans `launchSettings.json`
2. **CORS** : Vérifier la configuration dans `Program.cs`
3. **Base de données** : Redémarrer Docker avec `docker-compose restart`

### Documentation Supplémentaire

- 📄 `ANALYSE_FONCTIONNELLE.md` : Besoins fonctionnels
- 📄 `CONCEPTION_UML.md` : Diagrammes UML
- 📄 `PLAN_DE_TEST.md` : Stratégie de tests
- 📄 `TABLEAU_TESTS.md` : Résultats détaillés

---

## 📄 Licence

Ce projet est développé dans le cadre d'un exercice pédagogique.

---

**🎯 MiniBook** - Application Réseau Social moderne avec ASP.NET Core et Angular
