--Lab 8--

-> In branch-ul lab8
1. Am facut fisierul de configurare in Protoconfig, pentru a putea genera cu gRpc fisierul necesar.
2. In Networking:
- Mi-am redefinit petodele in ProtocolBuilderUtils pentru a folosi ceea ce s-a genetrat cu gRpc.
- Am rescris ServicesProxy si Worker pentru a folosi ceea ce am scris in ProtocolBuilderUtils.
3. In Server am facut si ProtocolBuffersServer pentru a utiliza Worker-ul cu protocol buffers, insa nu am folosit-o. In schimb am folosit in client ProtocolBuffersServicesProxy, drept IContestServices.
