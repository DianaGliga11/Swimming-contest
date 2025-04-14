namespace Networking.Response
{
    public class ErrorResponse : IResponse
    {
        public string message{get;set;}

        public ErrorResponse()
        {
        }

        public ErrorResponse(string message)
        {
            this.message = message;
        }

    }
}