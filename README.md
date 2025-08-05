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
