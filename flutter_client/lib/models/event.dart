// class Event {
//   final int id;
//   final String style;
//   final int distance;
//
//   Event({required this.id, required this.style, required this.distance});
//
//   factory Event.fromJson(Map<String, dynamic> json) {
//     return Event(
//       id: json['id'],
//       style: json['style'],
//       distance: json['distance'],
//     );
//   }
//
//   Map<String, dynamic> toJson() {
//     return {
//       'id': id,
//       'style': style,
//       'distance': distance,
//     };
//   }
// }

class Event{
  Event({
    required this.id,
    required this.style,
    required this.distance
});

  final int id;
  final String style;
  final int distance;
}