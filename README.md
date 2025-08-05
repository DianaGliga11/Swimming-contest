--LAB 3--
 
 -> In branch-ul java-lab3
1. Implementarea Repository-lor care utilizeaza baze de date SQLite si fisiere de configurare 'db.config':
- folosesc Sqlite
- in db.config se ofera calea spre baza de date pentru a se crea conexiunea cu aceasta
3. Partea de jurnalizare:
- folosind un fisier de tip xml (log4net.xml)
- tot ce reiese din logger va fi salvat in fisierul app.log (logger de tip TRACE - INFO si ERROR in cazul acesta)
4. Mici modificari la nivel de Model (Entity) :
-pentru a folosi incrementarea automata a ID-ului si a pastra unicitatatea acestuia in tabelele baze de date (primary key).
