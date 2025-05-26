<<<<<<< HEAD
# Modular-Monolith-Architecture
This project follows a modular monolith design, with distinct modules for billing, identity, and order management.
=======
## Table des matières

1. [Prérequis](#prérequis)
2. [Installation](#installation)
3. [Explication des étapes](#explication-des-étapes)
4. [Git Workflow](#git-workflow)
    1. [Branche principale : `main`](#branche-principale-main)
    2. [Création de branches de fonctionnalités](#création-de-branches-de-fonctionnalités)
    3. [Faire des commits](#faire-des-commits)
    4. [Pull Requests (PR)](#pull-requests-pr)
    5. [Mise à jour de votre branche](#mise-à-jour-de-votre-branche)
    6. [Revue de code et fusion](#revue-de-code-et-fusion)
    7. [Suppression des branches](#suppression-des-branches)


---



## Prérequis

Avant de pouvoir installer et exécuter ce projet, vous devez avoir les éléments suivants :

- **.NET SDK 8.0** ou une version supérieure. 
- **Visual Studio** (recommandé) ou **VS Code** avec les extensions nécessaires pour le développement .NET.
- Une connexion Internet pour restaurer les packages NuGet lors de l'installation.

## Installation

### Installation repo

Suivez ces étapes pour configurer l'environnement de développement et lancer l'application :

1. Clonez ce dépôt sur votre machine locale :

   ```bash
   git clone https://github.com/NexaCorpTech/NexaShopify-backend.git

2. Accédez au dossier du projet :

    ```bash
   cd NexaShopify-backend

3. Restaurer les dépendances NuGet :

    ```bash
    dotnet restore
    ```

   - **Visual Studio** : La restauration des packages NuGet se fait automatiquement lorsque tu ouvres le projet ou que tu compiles l'application. Si cela ne se produit pas, tu peux restaurer manuellement les dépendances via :
      - **Tools > NuGet Package Manager > Package Manager Console** et exécuter la commande :

        ```bash
        dotnet restore
        ```

   - **VS Code** : Utilisez le terminal intégré et tapez :

        ```bash
        dotnet restore
        ```
   
4. Compiler le projet :

    ```bash
    dotnet build
    

5. Exécuter l'application :

   ```bash
    dotnet run

### Installation DB: Microsoft Sql Server


#### Une fois la base installé, éxécuter le script de généraion de la base qui se situe ici: `à remplir`


## Explication des étapes :

1. **Clonez le dépôt** : Cette étape permet de récupérer le code source du projet depuis GitHub.

4. **Accédez au dossier du projet** : Tu te places dans le dossier contenant le code du projet.

5. **Restaurer les dépendances** : Cette étape est cruciale pour que le projet puisse télécharger toutes les bibliothèques nécessaires.

6. **Compiler le projet** : Cela garantit que le projet se compile sans erreur.
   
7. **Installation DB** : installer la base de données.

8. **Exécuter l'application** : Lance le serveur pour tester localement l'application.

---

## Git Workflow

Voici comment nous allons travailler avec Git sur ce projet. Suivez ces étapes pour garantir un développement fluide et une gestion optimale des versions.

### 1. **Branche principale : `main`**

- La branche `main` (ou `master`) contient la version stable et prête à la production de l'application.
- **Ne travaillez pas directement sur la branche `main`**. Créez toujours une nouvelle branche pour vos modifications.

### 2. **Branche d'intégration des devs : `develop`**

- La branche `develop` contient la version ou seront réaliser les PR issue des devs.
- **Ne travaillez pas directement sur la branche `develop`**. Créez toujours une nouvelle branche pour vos modifications.

### 3. **Création de branches de fonctionnalités**

- Créez une nouvelle branche pour chaque fonctionnalité ou bug que vous développez.
- Utilisez le format suivant pour nommer les branches :
  - `feature/<nom-de-la-fonctionnalité>`
  - `bugfix/<nom-du-bug>`
  
   Exemple :
   - `feature/login-form`
   - `bugfix/correction-authentication-error`

Pour créer une branche à partir de `main` :

```bash
git checkout main
git pull origin main
git checkout -b feature/nom-de-la-fonctionnalité
```

### 3. **Faire des commits**

Faites des commits réguliers avec des messages clairs et descriptifs. Suivez cette convention pour vos messages de commit :

- **Feature commits**: Commit Message for Feature `<feat(main-File): description>`&nbsp;&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;&nbsp;`<feat: description>`
- **Bugfix commits**: Commit Message forFixing `<fix(main-File): description>>`&nbsp;&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;&nbsp;`<fix: description>>` 
- **Refactor commits**: Commit Message for Refactoring `<ref(main-File): description>`&nbsp;&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;&nbsp;`<ref: description>`
- **Merge commits**: Commit Message for Merging branch `<merge mybranch -in- develop>`


Exemple de message de commit :

 ```bash
  git commit -m "feat(ComponentLog): Adding the login form"
```
ou bien 

 ```bash
  git commit -m "feat: Adding the login form"
```

### 4. **Pull Requests (PR)**

- Une fois que vous avez terminé de travailler sur votre branche, créez une Pull Request (PR) vers la branche develop.
- Avant de soumettre une PR, assurez-vous d'avoir bien testé votre code et qu'il n'y a pas de conflits de fusion.
- Lors de la création de la PR, indiquez les détails des modifications dans la description pour faciliter la revue de code.

### 5. **Mise à jour de votre branche**

Avant de soumettre une PR, assurez-vous de toujours mettre à jour votre branche avec la dernière version de `main` pour éviter les conflits (Attention: Si c'est fait lors de la création de la branche ce n'est pas la peine pour éviter des conflits) :

```bash
git checkout main
git pull origin main
git checkout feature/nom-de-la-fonctionnalité
git merge main
```
Si des conflits se produisent, résolvez-les et refaites un commit pour finaliser l'intégration.

### 6. **Revue de code et fusion**

Les membres de l'équipe doivent faire une revue de code sur chaque PR avant qu'elle ne soit fusionnée dans `develop`. Une fois que la revue est terminée et que la PR est approuvée, la branche peut être fusionnée dans `develop`.

#### Attention: La fusion dans `main` est une tache que réalisera le chef d'équipe ne jamais fusionnez dans main

### 7. **Suppression des branches**

Après avoir fusionné une PR sur `develop` (jamais sur main), supprimez la branche utilisée pour éviter l'encombrement du dépôt :

```bash
git branch -d feature/nom-de-la-fonctionnalité
git push origin --delete feature/nom-de-la-fonctionnalité
>>>>>>> d8466bf (First Version)
