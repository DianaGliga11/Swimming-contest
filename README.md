--Lab 8--

-> In branch-ul java-lab8
1. Am instalat compiler-ul necesar pentru a utiliza Protocol Buffers (tehnologia de la Google). De asemenea, am configurat build.gradle pentru a folosi acest protocol.
2. Fisierul de configurare:
- SwimingContest.proto - contine identificarea tuturor claselor si actiunilor de Request/Response, dar si a atributelor care rezulta din aceste Request-uri si Response-uri.
3. Networking:
- Directorul protocolBuffers contine patru clase, dintre care una generata de compiler-ul pentru protocol buffers.
- In ProtoBuilderUtils apelez metodele generate cu gRpc. Pentru partea de server si client am refacut ServizesProxy si Worker pentru a folosi aceasta tehnologie.
4. Modificari pentru Service si ClientFX:
- Am modificat ServicesProxy-ul si Worker-ul clasic cu cel pentru Protocol Buffers.
