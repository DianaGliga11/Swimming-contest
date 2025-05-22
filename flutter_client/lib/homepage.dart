import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import 'api_services/api_service.dart';
import 'models/event.dart';

class HomePage extends StatefulWidget{
  const HomePage({super.key});
  
  @override 
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage>{
  final List<Event> _events  = <Event>[];
  final TextEditingController _searchController = TextEditingController();
  final TextEditingController _styleContrtoller = TextEditingController();
  final TextEditingController _distanceController = TextEditingController();
  bool _isLoading = false;
  bool _updating = false;
  String _searchQuery = "";

  @override
  void initState(){
    super.initState();
    _setEvents();
  }

  Future<void> _setEvents() async{
    setState(() {
      _isLoading = true;
      _events.clear();
    });

    _events.addAll(await ApiService.getEvents());
    setState(() {
      _isLoading = false;
    });
  }

  Future<void> _setSearchedEvent({required String id})async{
    setState(() {
      _isLoading = true;
      _events.clear();
    });

    final Event? found = await ApiService.getEventById(id: id);
    if(found == null){
      setState(() {
        _isLoading = false;
      });
    }else{
      setState(() {
        _events.add(found);
        _isLoading = false;
      });
    }
  }

  Future<void> _createEvent({required String style, required String distance})async{
    setState(() {
      _isLoading = true;
    });

    final int responseCode = await ApiService.createEvent(style: style, distance: distance);
    if(responseCode == 200){
      setState(() {
        _isLoading = false;
        _setEvents();
      });
    }
  }

  Future<void> _updateEvent({required String id, required String style, required String distance})async{
    setState(() {
      _isLoading = true;
      _updating = true;
    });

    final int responseCode = await ApiService.updateEvent(id: id, style: style, distance: distance);
    if(responseCode == 200){
      setState(() {
        _isLoading = false;
        _updating = false;
        _setEvents();
      });
    }
  }

  Future<void> _deleteEvent({required String id})async{
    setState(() {
      _isLoading = true;
      _updating = true;
    });

    final int responseCode = await ApiService.deleteEvent(id: id);
    if(responseCode == 200){
      setState(() {
        _isLoading = false;
        _updating = false;
        _setEvents();
      });
    }
  }

  bool _validateData() {
    if (_styleContrtoller.text.isEmpty || _distanceController.text.isEmpty) {
      setState(() {
        showDialog(
            context: context,
            builder: (BuildContext context) {
              return AlertDialog(
                title: const Text('Input errors detected!'),
                content: const Column(
                  children: <Text>[
                    Text('Race name cannot be empty'),
                    Text('Engine capacity must be a strictly positive integer')
                  ],
                ),
                actions: <Widget>[
                  TextButton(
                      onPressed: () {
                        Navigator.of(context).pop();
                      },
                      child: const Text('OK')
                  )
                ],
              );
            });
      });
      return false;
    }
    return true;
  }

  void _showUpdateMenu({required int id, required String style, required int distance}) {
    setState(() {
      _updating = true;
      showDialog(
          context: context,
          builder: (BuildContext context) {
            _styleContrtoller.text = style;
            _distanceController.text = distance.toString();
            return AlertDialog(
              title: const Text('Update race'),
              content: Column(
                children: <Widget>[
                  Text('id #$id'),
                  TextField(
                    controller: _styleContrtoller,
                    decoration: const InputDecoration(
                        hintText: 'Race name...'
                    ),
                  ),
                  TextField(
                    controller: _distanceController,
                    decoration: const InputDecoration(
                        hintText: 'Engine capacity...'
                    ),
                    keyboardType: TextInputType.number,
                  )
                ],
              ),
              actions: <Widget>[
                TextButton(
                    onPressed: () {
                      _updating = false;
                      Navigator.of(context).pop();
                      if (_validateData()) {
                        final String style = _styleContrtoller
                            .text;
                        final String distance = _distanceController
                            .text;
                        _updateEvent(
                            id: id.toString(), style: style, distance: distance);
                      }
                    },
                    child: const Text('Submit')
                ),
                TextButton(
                    onPressed: () {
                      _updating = false;
                      Navigator.of(context).pop();
                      _deleteEvent(id: id.toString());
                    },
                    child: const Text('Delete')
                ),
                TextButton(
                    onPressed: () {
                      _updating = false;
                      Navigator.of(context).pop();
                    },
                    child: const Text('Cancel')
                )
              ],
            );
          });
    });
  }

  void _showCreateMenu() {
    if (_updating) {
      return;
    }
    setState(() {
      _updating = true;
      showDialog(
          context: context,
          builder: (BuildContext context) {
            return AlertDialog(
              title: const Text('Create race'),
              content: Column(
                children: <TextField>[
                  TextField(
                    controller: _styleContrtoller,
                    decoration: const InputDecoration(
                        hintText: 'Race name...'
                    ),
                  ),
                  TextField(
                    controller: _distanceController,
                    decoration: const InputDecoration(
                        hintText: 'Engine capacity...'
                    ),
                    keyboardType: TextInputType.number,
                  )
                ],
              ),
              actions: <Widget>[
                TextButton(
                    onPressed: () {
                      _updating = false;
                      Navigator.of(context).pop();
                      if (_validateData()) {
                        final String style = _styleContrtoller
                            .text;
                        final String distance = _distanceController
                            .text;
                        _createEvent(
                            style: style, distance: distance);
                      }
                    },
                    child: const Text('Submit')
                ),
                TextButton(
                    onPressed: () {
                      _updating = false;
                      Navigator.of(context).pop();
                    },
                    child: const Text('Cancel')
                )
              ],
            );
          });
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text('Swimming Contest'),
          actions: <Widget>[
            if (_isLoading)
              const Center(
                child: FittedBox(
                  child: Padding(
                    padding: EdgeInsets.all(16),
                    child: CircularProgressIndicator(),
                  ),
                ),
              )
          ],
        ),
        body: _isLoading
            ? const Center(
          child: CircularProgressIndicator(),
        )
            : Column(
          children: <Widget>[
            Row(
              children: <Widget>[
                Expanded(
                    child: TextField(
                      controller: _searchController,
                      decoration: const InputDecoration(
                          label: Text('Search by id...'),
                          prefixIcon: Icon(Icons.search),
                          prefixIconColor: Colors.lightBlue),
                    )),
                TextButton(
                    onPressed: () {
                      _searchQuery = _searchController.text;
                      if (_searchQuery.isEmpty) {
                        _setEvents();
                      } else {
                        _setSearchedEvent(id: _searchQuery);
                      }
                    },
                    style: TextButton.styleFrom(backgroundColor: Colors.lightBlue, foregroundColor: Colors.white),
                    child: const Text('Search')),
                TextButton(
                    onPressed: () {
                      _showCreateMenu();
                    }, child: const Text('Create'))
              ],
            ),
            const SizedBox(
              height: 10,
            ),
            Expanded(
              child: _events.isNotEmpty
                  ? GridView.builder(
                  itemCount: _events.length,
                  gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(crossAxisCount: 4),
                  itemBuilder: (BuildContext context, int index) {
                    final Event event = _events[index];
                    return Stack(fit: StackFit.expand, children: <Widget>[
                      Align(
                          alignment: AlignmentDirectional.bottomEnd,
                          child: Container(
                            decoration: const BoxDecoration(
                                gradient: LinearGradient(
                                    begin: AlignmentDirectional.bottomCenter,
                                    end: AlignmentDirectional.topCenter,
                                    colors: <Color>[Colors.black, Colors.transparent])),
                            child: ListTile(
                              title: Text('${event.style} (${event.distance}cc)'),
                              subtitle: Text('id #${event.id}'),
                              onTap: () {
                                _showUpdateMenu(id: event.id, style: event.style, distance: event.distance);
                              },
                            ),
                          ))
                    ]);
                  })
                  : const Center(
                child: CircularProgressIndicator(semanticsLabel: 'Loading races...'),
              ),
            )
          ],
        ));
  }
}