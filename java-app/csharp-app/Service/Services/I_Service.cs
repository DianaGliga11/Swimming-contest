using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Repository;

namespace Service;

public interface I_Service<ID, E> where E : Entity<long>
{
    E findByID(long id);

    IEnumerable<E> getAll();

    void add(E entity);
    
    void delete(long id);
    
    void update(long id, E entity);
}