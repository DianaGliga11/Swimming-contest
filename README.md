 --LAB 2--
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
1. Implementarea Repository-lor care utilizeaza baze de date SQLite si fisiere de configurare 'db.config':
- folosesc Sqlite
- in db.config se ofera calea spre baza de date pentru a se crea conexiunea cu aceasta
3. Partea de jurnalizare:
- folosind un fisier de tip xml (log4net.xml)
- tot ce reiese din logger va fi salvat in fisierul app.log (logger de tip TRACE - INFO si ERROR in cazul acesta)
4. Mici modificari la nivel de Model (Entity) :
-pentru a folosi incrementarea automata a ID-ului si a pastra unicitatatea acestuia in tabelele baze de date (primary key).
