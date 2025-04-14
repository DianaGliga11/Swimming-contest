using mpp_proiect_csharp_DianaGliga11.Model;

namespace Networking.Request
{
    public class LogoutRequest : IRequest
    {
        public User user{get;set;}

        public LogoutRequest()
        {
        }

        public LogoutRequest(User user)
        {
            this.user = user;
        }
        
    }
}