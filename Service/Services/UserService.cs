using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Repository;

namespace Service;

public class UserService : I_UserService
{
    private I_UserDBRepository userRepository;

    public UserService(I_UserDBRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public User findByID(long id)
    {
        return userRepository.findById(id);
    }

    public IEnumerable<User> getAll()
    {
        return userRepository.getAll();
    }

    public void add(User entity)
    {
        userRepository.Add(entity);
    }

    public void delete(long id)
    {
        userRepository.Remove(id);
    }

    public void update(long id, User entity)
    {
        userRepository.Update(id, entity);
    }

    public User getLogin(string username, string password)
    {
        User user = userRepository.GetUserByCredentials(username, password);
        if (user != null)
        {
            return user;
        }
        return null;
    }
}