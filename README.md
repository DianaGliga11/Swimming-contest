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

Sincronizarea pentru mai multi clienti:
<img width="1939" height="671" alt="Captură de ecran 2025-08-05 102917" src="https://github.com/user-attachments/assets/070717d3-71e0-46d9-ac64-0b9115cc7da0" />
<img width="1948" height="241" alt="Captură de ecran 2025-08-05 102948" src="https://github.com/user-attachments/assets/0dbd1aa1-87fa-4a1b-8aed-6bca4c102daa" />
