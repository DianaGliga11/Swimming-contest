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

Login:
<img width="775" height="480" alt="Captură de ecran 2025-08-05 101408" src="https://github.com/user-attachments/assets/eccd9e39-612c-4ccb-a712-254f1fe66626" />

Interfata principala:
<img width="979" height="679" alt="Captură de ecran 2025-08-05 102058" src="https://github.com/user-attachments/assets/4edb4429-209d-4f55-ba0c-abcf000ca754" />

Adaugare participant:
<img width="466" height="212" alt="Captură de ecran 2025-08-05 102128" src="https://github.com/user-attachments/assets/98e4a5fd-1ef8-4cd9-a823-33585e8e7499" />
<img width="283" height="186" alt="Captură de ecran 2025-08-05 102119" src="https://github.com/user-attachments/assets/461bc014-7b1e-47e4-a4b3-ccc7feffc3c3" />

Inscriere participant:
<img width="378" height="490" alt="Captură de ecran 2025-08-05 102307" src="https://github.com/user-attachments/assets/7ecc74b3-65a3-4b00-b87b-b851ff0355fe" />

Tabel cu participantii unui eveniment:
<img width="219" height="473" alt="Captură de ecran 2025-08-05 102139" src="https://github.com/user-attachments/assets/cd144672-af05-4c7f-a298-a03fcf11fbe6" />
<img width="490" height="236" alt="Captură de ecran 2025-08-05 102247" src="https://github.com/user-attachments/assets/57a49637-38b2-4281-9631-838245723c86" />
