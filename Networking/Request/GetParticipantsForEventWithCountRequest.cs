namespace Networking.Request
{

    public class GetParticipantsForEventWithCountRequest: IRequest
    {
        public long EventId { get; set; }

        public GetParticipantsForEventWithCountRequest()
        {

        }

        public GetParticipantsForEventWithCountRequest(long eventId)
        {
            EventId = eventId;
        }
}
}