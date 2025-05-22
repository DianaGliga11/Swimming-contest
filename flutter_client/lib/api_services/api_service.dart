import 'dart:convert';
import 'package:http/http.dart';
import '../api_utils.dart';
import '../models/event.dart';

class ApiService {
  static Future<List<Event>> getEvents() async {
    final Response response = await ApiUtils.get('/contest/events');
    final List<Event> results = <Event>[];

    if (response.statusCode == 200) {
      final List<dynamic> data = jsonDecode(response.body) as List<dynamic>;

      for (int i = 0; i < data.length; i++) {
        final Map<String, dynamic> race = data[i] as Map<String, dynamic>;
        final Event item = Event(
            id: race['id'] as int,
            style: race['style'] as String,
            distance: race['distance'] as int);
        results.add(item);
      }
    }
    return results;
  }

  static Future<Event?> getEventById({required String id}) async {
    final Response response = await ApiUtils.get('/contest/events/$id');

    if (response.statusCode == 404) {
      return null;
    }

    if (response.statusCode == 200) {
      final Map<String, dynamic> race = jsonDecode(response.body) as Map<
          String,
          dynamic>;

      final Event item = Event(
          id: race['id'] as int,
          style: race['style'] as String,
          distance: race['distance'] as int);
      return item;
    }
    return null;
  }

  static Future<int> createEvent({required String style, required String distance}) async {
    final Response response = await ApiUtils.post(
        path: '/contest/events',
        jsonData: <String, String>{
          'style': style,
          'distance': distance
        }
    );

    return response.statusCode;
  }

  static Future<int> updateEvent({required String id, required String style, required String distance}) async {
    final Response response = await ApiUtils.put(
        path: '/contest/events/$id',
        jsonData: <String, String>{
          'id': id,
          'style': style,
          'distance': distance
        });

    return response.statusCode;
  }

  static Future<int> deleteEvent({required String id}) async {
    final Response response = await ApiUtils.delete('/contest/events/$id');

    return response.statusCode;
  }
}
