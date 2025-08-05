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


--Lab 10--

-> In brunch-ul lab10
- Aici am facut doar un HttpClient care sa se conecteze la server-ul rest (spring) din java (in alt repository).
- Am utilizat Task-uri si practic am rescris metodele pentru POST, PUT, GET, DELETE.
