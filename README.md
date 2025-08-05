 --Lab 11--

  -> In branch-ul java-lab11
  1. Am utilizat flutter pentru a crea Clientul Web:
  - Am definit entitatea pe care vreau sa o folosesc in flutter: Event.
  - In api_services.dart mi-am definit metodele CRUD si le-am preschimbat in format json.
  - In api_util.dart, am realizat conexiunea la server si am tratat operatiile de POST, PUT, GET, DELETE.
  - In homepage_dart ma folosesc de statusuri pentru a trata actiuniile clientului (paginii) de incarcare, de actualizare. De asemenea, am tratat si posibilele erori care ar putea surveni din cauza datelor introduse gresit. Tot aici am gestionat ce tine de aspectul paginii.
  - In main.dart doar am oferit titlu si tema pentru aplicatie, urmand ca acesta sa apeleze pagina principala ce contine toate state-urile.
  2. Modificari in RestServices:
  - A trebuit sa activez CORS pentru a putea folosi URI-urile de pe server si clientul sa se poatea conecta fara probleme la acesta.

Interfata principala:
<img width="2558" height="1508" alt="Captură de ecran 2025-08-05 084913" src="https://github.com/user-attachments/assets/085dd08f-b1d4-4398-a482-ee2878f85b8d" />
Functionalitatile de Adaugare, Actualizare si Stergere eveniment:
<img width="448" height="1318" alt="Captură de ecran 2025-08-05 085035" src="https://github.com/user-attachments/assets/ac2b9d4e-86d9-46f2-967e-2b9f3276ecf7" />
<img width="515" height="1351" alt="Captură de ecran 2025-08-05 085017" src="https://github.com/user-attachments/assets/e9d7d918-c12e-4d14-9896-9142bfb4e41c" />
<img width="2559" height="826" alt="Captură de ecran 2025-08-05 084935" src="https://github.com/user-attachments/assets/ec14eb53-1241-4765-a52c-5577727c40cb" />
