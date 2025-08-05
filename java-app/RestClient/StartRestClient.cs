using System.Net.Http.Json;
using System.Text.Json;
using mpp_proiect_csharp_DianaGliga11.Model;


namespace RestClient
{

    public class StartRestClient
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string URL = "http://localhost:8080/contest/events";

        static async Task Main(string[] args)
        {
            // 1. POST – creare
            var newEvent = new Event { Style = "accelerat", Distance = 800 };
            var responsePost = await client.PostAsJsonAsync(URL, newEvent);
            responsePost.EnsureSuccessStatusCode();
            var createdEvent = await responsePost.Content.ReadFromJsonAsync<Event>();
            Console.WriteLine("Created: " + JsonSerializer.Serialize(createdEvent));

            // 2. GET – toate
            var allEvents = await client.GetFromJsonAsync<Event[]>(URL);
            Console.WriteLine("\nAll Events:");
            foreach (var ev in allEvents)
                Console.WriteLine($"{ev.Id}: {ev.Style} - {ev.Distance}m");

            // 3. GET – după ID
            var getByIdUrl = $"{URL}/{createdEvent.Id}";
            var eventById = await client.GetFromJsonAsync<Event>(getByIdUrl);
            Console.WriteLine("\nFetched by ID: " + JsonSerializer.Serialize(eventById));

            // 4. PUT – actualizare
            eventById.Distance = 150;
            var responsePut = await client.PutAsJsonAsync($"{URL}/{eventById.Id}", eventById);
            responsePut.EnsureSuccessStatusCode();
            Console.WriteLine("\nUpdated Event.");

            // 5. DELETE – ștergere
            var responseDelete = await client.DeleteAsync($"{URL}/{eventById.Id}");
            responseDelete.EnsureSuccessStatusCode();
            Console.WriteLine("\nDeleted Event with ID " + eventById.Id);
        }
    }
}