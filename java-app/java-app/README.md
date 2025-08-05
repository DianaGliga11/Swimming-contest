--Lab 12--

-> In branch-ul java-lab12

1. In flutter_client:
- Am adaugat partea de login (connect) la server prin WebSocket pentru a putea face modificari la nivelul backend-ului.
- In homepage.dart, in momentul in care am initializat state-ul, am realizat si conexiunea intre clienti prin WebSocket. Astfel se vor realiza modificarile si la nivelul interfetei.
2. In RestServices:
- A fost necesar sa configurez WebSocket pentru a permite utilizarea unui broker si a crea endpoint-uri pentru aplicatie.
- Am modificat putin si URI-ul pentru CORS, pentru a  putea gestiona mai bine pagina.
- Pentru Controller, a fost necesar sa folosesc un SimpleMessageSendingOperations pentru a notifica clientii in momentul in care se realizeaza o modificare in aplicatie, de catre alt client.
<img width="2545" height="1521" alt="Captură de ecran 2025-08-05 090447" src="https://github.com/user-attachments/assets/1279d3cf-aba3-4fe7-b49f-affa3c0e295b" />
<img width="1725" height="319" alt="Captură de ecran 2025-08-05 090520" src="https://github.com/user-attachments/assets/c5531a0f-a69f-4d19-8e29-3a712c6e2f28" />
