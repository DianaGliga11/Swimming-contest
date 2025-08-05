 --Lab 9--

  -> In branch-ul java-lab9
  1. Modificari asupra Model pentru a folosi ORM (Hibernate). Event si Participant vor utiliza hibernate pentru a mapa datele, pe care l-am inclus in dependentele gradle, alaturi de jakarta.
  2. Repository:
  - Folosesc un sessionFactory pentru a putea defini clasele cu hibernate, pe care le-am inclus prin intermediul unui fisier de configurare hibernate.cfg.xml.
  - Am facut repository-uri care se folosesc de clasele hibernate. Utilizez transactions pentru a defini operatiile CRUD.
  3. Modificari la nivel de server pentru a ma utiliza de repository-urile care folosesc ORM-uri.
