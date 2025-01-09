# MyDoctor Fullstack Application - ASP.NET Core & Angular

## Περιγραφή
Στην εφαρμογή, οι χρήστες εγγράφονται είτε ως ασθενείς είτε ως ιατροί. Οι ασθενείς, αφού κάνουν login, μπορούν να
αναζητήσουν ιατρούς βάσει κριτηρίων, να δουν στοιχεία γι 'αυτούς και να τους προσθέσουν στους ιατρούς τους. 
Μπορούν να προβάλουν τη λίστα με τους ιατρούς τους και να διαγράψουν όποιον ιατρό επιθυμούν. Επίσης, έχουν τη δυνατότητα
να τροποποιήσουν τα προσωπικά τους στοιχεία.
Οι ιατροί, αφού κάνουν login μπορούν να προβάλουν τους ασθενείς που έχουν εγγραφεί σε αυτούς κι αν επιθυμούν
να διαγράψουν κάποιον ασθενή. Επίσης, μπορούν να τροποποιήσουν τα προσωπικά τους στοιχεία.  


## Τεχνολογίες
- **Backend**:
  - ASP.NET Core
  - Entity Framework Core (Model First)
  - Microsoft SQL Server Express
- **Frontend**:
  - Angular

---

## Προϋποθέσεις

Για να μπορέσετε να εκτελέσετε την εφαρμογή τοπικά, βεβαιωθείτε ότι έχετε εγκαταστήσει:

1. **Backend Requirements**:
   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - [Microsoft SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

2. **Frontend Requirements**:
   - [Node.js](https://nodejs.org/) (έκδοση 20 ή νεότερη)
   - [Angular CLI](https://angular.io/cli)

3. **Git**: Για την κλωνοποίηση του repository από το GitHub.

---

## Οδηγίες για Build και Deploy

### 1. Κλωνοποίηση του Repository

Πρώτα, κλωνοποιήστε το αποθετήριο στον υπολογιστή σας:

```bash
git clone https://github.com/JohnMatthiou/mydoctor-fullstack-app.git
cd mydoctor-fullstack-app
```

---

### 2. Build και Deploy του Backend

1. **Μεταβείτε στον φάκελο του backend**:

   ```bash
   cd MyDoctorApp
   ```

2. **Εγκαταστήστε τα NuGet packages**:

   Εκτελέστε την παρακάτω εντολή για να κατεβάσετε τις απαραίτητες εξαρτήσεις:

   ```bash
   dotnet restore
   ```

3. **Δημιουργία ή Ενημέρωση της Βάσης Δεδομένων**:

   Το backend ακολουθεί το **Model First schema**, επομένως θα χρειαστεί να δημιουργήσετε ή να ενημερώσετε τη βάση δεδομένων στον SQL Server Express.

   - Βεβαιωθείτε ότι ο SQL Server Express εκτελείται.
   - Ελέγξτε το connection string στο αρχείο `appsettings.json`. Η προεπιλεγμένη σύνδεση είναι:

     ```json
     "ConnectionStrings": {
         "DefaultConnection": "Server=localhost\\sqlexpress;Database=MyDoctorDB;User=MyDoctorAdmin;Password=123456;MultipleActiveResultSets=True;TrustServerCertificate=True"
     }
     ```

     Ενημερώστε το αν χρειάζεται.

   - Δημιουργήστε τη βάση δεδομένων με την εντολή:

     ```bash
     dotnet ef database update
     ```

4. **Εκτελέστε την εφαρμογή**:

   Εκτελέστε την εφαρμογή backend:

   ```bash
   dotnet run
   ```

   Το backend θα εκκινήσει και θα είναι διαθέσιμο στη διεύθυνση:

   ```
   https://localhost:5001
   ```

---

### 3. Build και Deploy του Frontend

1. **Μεταβείτε στον φάκελο του frontend**:

   ```bash
   cd ../MyDoctorApp-frontend
   ```

2. **Εγκαταστήστε τα npm packages**:

   Εκτελέστε την παρακάτω εντολή για να κατεβάσετε τις απαραίτητες εξαρτήσεις:

   ```bash
   npm install
   ```

3. **Εκτελέστε την εφαρμογή**:

   Εκτελέστε την Angular εφαρμογή τοπικά:

   ```bash
   ng serve
   ```

   Το frontend θα εκκινήσει και θα είναι διαθέσιμο στη διεύθυνση:

   ```
   http://localhost:4200
   ```

---

### 4. Σύνδεση Backend και Frontend

- Βεβαιωθείτε ότι το frontend επικοινωνεί σωστά με το backend.
- Ελέγξτε τη ρύθμιση του **API URL** στο Angular έργο (στο αρχείο `environment.ts`):

  ```typescript
  export const environment = {
    production: false,
    apiUrl: 'https://localhost:5001'
  };
  ```

---

## 5. Έλεγχος του API με Swagger

Η εφαρμογή υποστηρίζει το **Swagger** για την εύκολη πλοήγηση και έλεγχο των API endpoints. Για να το χρησιμοποιήσετε:

1. Βεβαιωθείτε ότι το backend είναι εκκινημένο και τρέχει στη διεύθυνση `https://localhost:5001`.

2. Μεταβείτε στο Swagger UI χρησιμοποιώντας την εξής διεύθυνση:

   ```http
   https://localhost:5001/swagger
   ```	

3. Στην παραπάνω διεύθυνση, θα εμφανιστεί το Swagger UI όπου μπορείτε να δείτε όλα τα διαθέσιμα API endpoints και να τα δοκιμάσετε απευθείας από το interface.

---

## Χρήση της Εφαρμογής

1. Ανοίξτε τον browser σας και μεταβείτε στο:

   ```
   http://localhost:4200
   ```

2. Χρησιμοποιήστε το frontend για να αλληλεπιδράσετε με τα δεδομένα μέσω του backend.

---

## Troubleshooting

1. **Backend δεν ξεκινάει**:
   - Βεβαιωθείτε ότι ο SQL Server Express εκτελείται.
   - Ελέγξτε το connection string στο `appsettings.json`.

2. **Frontend δεν ξεκινάει**:
   - Βεβαιωθείτε ότι τα npm packages είναι εγκατεστημένα.
   - Ελέγξτε το αρχείο `environment.ts` για τη σωστή διεύθυνση του backend.

---

## Συγγραφέας
Το έργο αναπτύχθηκε από: Ioannis Matthioudakis

---

## Άδεια Χρήσης
Το έργο διανέμεται υπό την άδεια χρήσης [MIT License](LICENSE).

