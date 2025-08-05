--Lab 6--

-> In branch-ul java-lab6
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
