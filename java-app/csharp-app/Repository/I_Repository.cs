using System.Collections.Generic;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace mpp_proiect_csharp_DianaGliga11.Repository
{
public interface I_Repository<E> where E : Entity<long>
{
    void Add(E entity);
    void Remove(long id);
    void Update(long id, E entity);
    E findById(long id);
    IEnumerable<E> getAll();
}
}