using mpp_proiect_csharp_DianaGliga11.Model;

namespace Service;

public interface I_UserService : I_Service<long, User>
{
    User getLogin(string username, string password);
}