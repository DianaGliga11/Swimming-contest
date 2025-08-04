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

--Lab 6--

-> In branch-ul lab6
1. Modificari asupra Model:
- toate clasele din model au fost facute Serializabile
2. Adaugari Service:
- partea de Observer (tine evidenta inregistrarilor in aplicatie)
- partea de ContestService care face apel la functionalitatile din serviciile efective. Metodele suprascrise din aceasta clasa trebuie sa fie synchronized pentru actualiza datele in timp real
3. Implementarea modulului Server:
- foloseste Socket pentru a crea unn mediu unde sa se conecteze clientii, pe un anumit port
- creaza thread-uri separate pentru fiecare client nou conectat
4. Implementarea modulului Networking:
- folosesc protocolul RPC
- pachetul request contine toate clasele pentru fiecare actiune posibila facuta de client
- pachetul response contine toate clasele responsabile de a da raspunsuri pentru cererile facute de catre client
- ClientWorker gestioneaza cererile facute de catre client, acestea fiid transmise in forma serializata prin socket-ul pornit
- ServicesProxy interactioneaza cu partea de servicii pentru a actualiza schimbarile efectuate de catre client. Tot aici se realizeaza si logarea si delogarea clientului, folosind Observer si un BlockingDeque
5. Implementarea modulului Client:
- partea de controller ramane aproximativ la fel, exceptie facand faptul ca trebuie sa folosim runLater pentru a nu bloca interfata si sa sincronizam tot ce apare nou in interfata (ex. Initializerea tabelelor)
- in pornirea clientului verific portul si serverul, ca in caz ca nu este disponibil sa foloseasca un port si server default
- HomeController trebuie sa implementeze interfata pentru IMainObserver pentru a putea actualiza modificarile efectuate in interfata, dar si logarea/delogarea clientilor.


--Lab 8--

-> In branch-ul lab8
1. Am instalat compiler-ul necesar pentru a utiliza Protocol Buffers (tehnologia de la Google). De asemenea, am configurat build.gradle pentru a folosi acest protocol.
2. Fisierul de configurare:
- SwimingContest.proto - contine identificarea tuturor claselor si actiunilor de Request/Response, dar si a atributelor care rezulta din aceste Request-uri si Response-uri.
3. Networking:
- Directorul protocolBuffers contine patru clase, dintre care una generata de compiler-ul pentru protocol buffers.
- In ProtoBuilderUtils apelez metodele generate cu gRpc. Pentru partea de server si client am refacut ServizesProxy si Worker pentru a folosi aceasta tehnologie.
4. Modificari pentru Servicem si ClientFX:
- Am modificat ServicesProxy-ul si Worker-ul clasic cu cel pentru Protocol Buffers.


  --Lab 9--

  -> In branch-ul lab9
  1. Modificari asupra Model pentru a folosi ORM (Hibernate). Event si Participant vor utiliza hibernate pentru a mapa datele, pe care l-am inclus in dependentele gradle, alaturi de jakarta.
  2. Repository:
  - Folosesc un sessionFactory pentru a putea defini clasele cu hibernate, pe care le-am inclus prin intermediul unui fisier de configurare hibernate.cfg.xml.
  - Am facut repository-uri care se folosesc de clasele hibernate. Utilizez transactions pentru a defini operatiile CRUD.
  3. Modificari la nivel de server pentru a ma utiliza de repository-urile care folosesc ORM-uri.


  --Lab 10--

  -> In branch-ul lab10
  1. Directorul RestServices:
  - Am adaugat dependentele in build.gradle pentru a folosi framework-ul spring.
  - Am realizat un client rest care suprascrie metodele CRUD.
  - In Controller, am realizat actiunile de POST, GET, DELETE, PUT si am injectat EventDBRepository pentru a-l putea folosi in acest context.
  - Serverul atre acces la package-urile utilizate, pentru a putea porni aplicatia spring, dar si asupra proprietatilor legate de baza de date.
  - Pentru partea de client Http, se poate observa in partea de C# din celalat repository.
 

  --Lab 11--

  -> In branch-ul lab11
  1. Am utilizat flutter pentru a crea Clientul Web:
  - Am definit entitatea pe care vreau sa o folosesc in flutter: Event.
  - In api_services.dart mi-am definit metodele CRUD si le-am preschimbat in format json.
  - In api_util.dart, am realizat conexiunea la server si am tratat operatiile de POST, PUT, GET, DELETE.
  - In homepage_dart ma folosesc de statusuri pentru a trata actiuniile clientului (paginii) de incarcare, de actualizare. De asemenea, am tratat si posibilele erori care ar putea surveni din cauza datelor introduse gresit. Tot aici am gestionat ce tine de aspectul paginii.
  - In main.dart doar am oferit titlu si tema pentru aplicatie, urmand ca acesta sa apeleze pagina principala ce contine toate state-urile.
  2. Modificari in RestServices:
  - A trebuit sa activez CORS pentru a putea folosi URI-urile de pe server si clientul sa se poatea conecta fara probleme la acesta.


--Lab 12--

-> In branch-ul lab12

1. In flutter_client:
- Am adaugat partea de login (connect) la server prin WebSocket pentru a putea face modificari la nivelul backend-ului.
- In homepage.dart, in momentul in care am initializat state-ul, am realizat si conexiunea intre clienti prin WebSocket. Astfel se vor realiza modificarile si la nivelul interfetei.
2. In RestServices:
- A fost necesar sa configurez WebSocket pentru a permite utilizarea unui broker si a crea endpoint-uri pentru aplicatie.
- Am modificat putin si URI-ul pentru CORS, pentru a  putea gestiona mai bine pagina.
- Pentru Controller, a fost necesar sa folosesc un SimpleMessageSendingOperations pentru a notifica clientii in momentul in care se realizeaza o modificare in aplicatie, de catre alt client.
