--Lab 2--

-> branch-ul main
Am realizat doua proiecte .csproj in cadrul .sln (root):
1. Model
- pentru clasele utiolizate ulterior in Swiming Competition
- contine o clasa Entity abstracta care va da ID-ul fiecarei clase derivate
2. Repository
- pentru a gestiona obiectele din Model
- o interfata generala, pentru fiecare interfata ulterioara cu metodele generale
- interfete pentru fiecare obiect, pentru a introduce noi metode ce vor fi ulterior implementate
Tot proiectul este legat folosind gradle: in settings.gradle se introduc modulele create pentru a putea naviga intre ele obiecte/interfete etc.
Ambele module pot fi rulate, insa nu acest lucru se doreste,  urmand sa introduc un nou modul care imi permite rularea intregului proiect.

--Lab 3--

-> branch-ul lab3
Am separat proiectul .sln in Model, Repository si TestApplication:
1. Model - creat in Lab 2
2. Repository:
- contine interfetele, partea de conexiune la baza de date si implementarea interfetelor prin utilizarea bazei de date
- tot aici, am adaugat o clasa DatabaseRepoUtils in care generalizez metodele CRUD pentru baza de date
4. TestApplication:
- contine o clasa Config care preia din app.config connectionString-ul si connectionType-ul necesar pentru a se conecta la baza de date.
- rulez aplicatia aici
- fisierul log4net.config unde creez si gestionez partea de Logger a aplicatiei si redirectionez toate mesajele de tip TRACE intr-ul file (app.log).
