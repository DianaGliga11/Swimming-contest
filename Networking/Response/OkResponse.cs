using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Response
{
    public class OkResponse : IResponse
    {
        public User user{get;set;}

        public OkResponse()
        {
        }

        public OkResponse(User user)
        {
            this.user = user;
        }

    }
}