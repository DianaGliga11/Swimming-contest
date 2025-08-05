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

Logare client:
<img width="615" height="808" alt="Captură de ecran 2025-08-05 090944" src="https://github.com/user-attachments/assets/7520da4b-8cbd-43c6-8157-a8213f3662bf" />

Interfata principala:
<img width="495" height="430" alt="Captură de ecran 2025-08-05 091011" src="https://github.com/user-attachments/assets/9599e53e-f67f-4115-a675-5acbcc2b650d" />
<img width="1280" height="1020" alt="Captură de ecran 2025-08-05 091000" src="https://github.com/user-attachments/assets/b433a5e5-eec1-4dc1-84f1-5ca298b77dff" />

Cautare participanti dupa un eveniment:
<img width="491" height="430" alt="Captură de ecran 2025-08-05 091011" src="https://github.com/user-attachments/assets/1d444563-9492-4423-b6a3-e367a78d787f" />
<img width="1264" height="587" alt="Captură de ecran 2025-08-05 091048" src="https://github.com/user-attachments/assets/425023f8-30d8-4fe4-bd91-2052b715d3d6" />

Adaugare participant:
<img width="523" height="416" alt="Captură de ecran 2025-08-05 091110" src="https://github.com/user-attachments/assets/6ab1e55e-770b-4fa9-a2f2-df3be00fc6ac" />
<img width="581" height="291" alt="Captură de ecran 2025-08-05 091117" src="https://github.com/user-attachments/assets/1a78b08a-869d-46b4-a11c-428d25df041f" />

Inscriere participant la evenimente:
<img width="1371" height="125" alt="Captură de ecran 2025-08-05 091239" src="https://github.com/user-attachments/assets/15ba3bbf-f4f3-4b46-949a-d601e0316cf6" />
<img width="749" height="791" alt="Captură de ecran 2025-08-05 091132" src="https://github.com/user-attachments/assets/6027b1d0-c16a-4f4e-b77e-211cb00f7f36" />
