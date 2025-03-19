--Lab 2--

-> branch-ul main
Am realizat trei proiecte .csproj in cadrul .sln (root):
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
Am separat modulele in Model, Repository si TestApplication:
1. Model - contine toate clasele proiectului
2. Repository - contine interfetele, partea de conexiune la baza de date si implementarea interfetelor prin utilizarea bazei de date. Tot aici, am adaugat lo4net pentru a putea folosi Logger.
3. TestApplication - rulez aplicatia aici si preiau informatiile din db.config (despre baza de date).
