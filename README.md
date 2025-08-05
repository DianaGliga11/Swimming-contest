--Lab 4--

-> In branch-ul java-lab4
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
