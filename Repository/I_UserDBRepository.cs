using mpp_proiect_csharp_DianaGliga11.Model;

namespace mpp_proiect_csharp_DianaGliga11.Repository
{
public interface I_UserDBRepository : I_Repository<User>
{
    bool checkUserPassword(User user);
}
}