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
