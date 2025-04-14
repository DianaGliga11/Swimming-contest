 --LAB 2--

 -> branch-ul main
Am realizat doua module:
1. Model
- pentru clasele utiolizate ulterior in Swiming Competition
- contine o clasa Entity abstracta care va da ID-ul fiecarei clase derivate
2. Repository
- pentru a gestiona obiectele din Model
- o interfata generala, pentru fiecare interfata ulterioara cu metodele generale
- interfete pentru fiecare obiect, pentru a introduce noi metode ce vor fi ulterior implementate
Tot proiectul este legat folosind gradle: in settings.gradle se introduc modulele create pentru a putea naviga intre ele obiecte/interfete etc.
Ambele module pot fi rulate, insa nu acest lucru se doreste,  urmand sa introduc un nou modul care imi permite rularea intregului proiect.

--LAB 3--
 
 -> In branch-ul lab3
1. Implementarea Repository-lor care utilizeaza baze de date SQLite si fisiere de configurare 'db.config':
- folosesc Sqlite
- in db.config se ofera calea spre baza de date pentru a se crea conexiunea cu aceasta
3. Partea de jurnalizare:
- folosind un fisier de tip xml (log4net.xml)
- tot ce reiese din logger va fi salvat in fisierul app.log (logger de tip TRACE - INFO si ERROR in cazul acesta)
4. Mici modificari la nivel de Model (Entity) :
-pentru a folosi incrementarea automata a ID-ului si a pastra unicitatatea acestuia in tabelele baze de date (primary key).

--Lab 4--

-> In branch-ul lab4
1. Modificari asupra Model si Repository:
- am inclus partea de DTO care ma va ajuta ulterior la GUI
- in Repository pentru Office am sters partea de serializare/deserializare a obiectelor, folosindu-ma acum de DTO-urile create
2. Implementarea Service:
- contine interfata principala si interfete pentru clasele Event, User si Participant
- in service-ul pentru Event am inclus si partea ce tine de Office (functiile getEventsWithParticipantsCount, getParticipantsForEventWithCount, getEntriesByEvent)
3. Implementare Controller:
- contine 6 Controllere pentru fiecare fereastra (pentru logare, fereastra principala, adaugare participant, inscriere participant la evenimente)
- am realizat ulterior ca OfficeController nu este necesar in acest caz, deoarece am implementat EventEntriesController cu aceleasi functionalitati
- fiecare are fisiere fxml pentru partea de frontend a aplicatiei
- aplicatia va porni din HelloApplication care deschide initial fereastra de logare, care va face trecerea spre fereastra principala.
