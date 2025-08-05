  --Lab 10--

  -> In branch-ul java-lab10
  1. Directorul RestServices:
  - Am adaugat dependentele in build.gradle pentru a folosi framework-ul spring.
  - Am realizat un client rest care suprascrie metodele CRUD.
  - In Controller, am realizat actiunile de POST, GET, DELETE, PUT si am injectat EventDBRepository pentru a-l putea folosi in acest context.
  - Serverul atre acces la package-urile utilizate, pentru a putea porni aplicatia spring, dar si asupra proprietatilor legate de baza de date.
  - Pentru partea de client Http, se poate observa in partea de C# din celalat repository.

Le testez cu CURL din terminal momentan:
<img width="2018" height="413" alt="Captură de ecran 2025-08-05 085422" src="https://github.com/user-attachments/assets/e773319e-1ff8-4b77-8162-da732ec7a603" />
