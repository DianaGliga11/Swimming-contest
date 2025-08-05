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
