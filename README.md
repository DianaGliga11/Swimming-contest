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

--Lab 5--

-> branch-ul lab5
Am adaugat partea de Service si Controller a aplicatiei.
1. Modificari la nivelul Model si Repository pentru a putea folosi si DTO.
2. Service:
- fiecare componenta din Model are partea sa de Service, care provine dintr-unul abstract.
- in implementarea EventService, am adaugat si ce tine de Office pentru a putea face legatura cu acesta, fara a specifica efectiv toate datele legate de functionalitatea de inscriere a unui participant la un eveniment.
3. Controller:
- am folosit fisiere de configurare pentru locatia bazei de date si pentru logs.
- folosesc Form de la dotnet pentru a crea ferestrele.
- Am 5 controlleri pentru ferestrele de login, principala, adaugarea unui participant, inscrierea unui participant si lista de evenimente.

--Lab 7--

-> barnch-ul lab7
Am creat partea de Client, Server si Networking a aplicatiei.
1. Service:
- in Service am adaugat partea de observer care notifica clientii si actualizarile produse la nivelul datelor.
- folosesc design pattern-ul Observer .
2. Server:
- aici se creaza port-ul, host-ul si thread-urile pentru clienti.
- server-ul ia aceste date necesare la creerea conexiunii dintr-un fisier de configurare.
3. Client:
- clientul se conecteaza la host-ul si port-ul respectiv server-ului prin intermediul clasei ServicesProxy.
- fereastra principala HomeController implementeaza IMainObserver pentru a putea sa actualizeze datele interfetei corect.
- folosesc BeginInvoke pentru a evita blocarea interfetei si gestionarea eficenta a thread-urilor.
- am modificat si constructorii pentru NewParticipantController si EventEntriesController pentru a avea acces mai usor la observer si datelele acestora. De asemenea folsoesc sincronizarea cu await pentru a nu bloca interfata.
4. Networking:
- folosesc protocolul Json, care imi va scrie datele sub forma Type, ce trebuie transmis.
- folosesc clase de tip enum pentru a gestiona Request-urile si Response-urile.
- de asemenea, imi definesc parametrii din aceste Request-uri si Response-uri pentru a le accesa corect atunci cand trebuiesc serializate si deserializate.
- partea de server (ClientWorker) foloseste serializarea Json si implementeaza metodele lui IMainObserver pentru a notifica toti clientii de pe server.
- partea de client (ServicesProxy) primeste si transmite datele tot prin serializarea Json si implementeaza IContestServices pentru a actualiza datele in interfata/persistenta.


--Lab 8--

-> In branch-ul lab8
1. Am facut fisierul de configurare in Protoconfig, pentru a putea genera cu gRpc fisierul necesar.
2. In Networking:
- Mi-am redefinit petodele in ProtocolBuilderUtils pentru a folosi ceea ce s-a genetrat cu gRpc.
- Am rescris ServicesProxy si Worker pentru a folosi ceea ce am scris in ProtocolBuilderUtils.
3. In Server am facut si ProtocolBuffersServer pentru a utiliza Worker-ul cu protocol buffers, insa nu am folosit-o. In schimb am folosit in client ProtocolBuffersServicesProxy, drept IContestServices.


--Lab 10--

-> In brunch-ul lab10
- Aici am facut doar un HttpClient care sa se conecteze la server-ul rest (spring) din java (in alt repository).
- Am utilizat Task-uri si practic am rescris metodele pentru POST, PUT, GET, DELETE.
