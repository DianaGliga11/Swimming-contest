using mpp_proiect_csharp_DianaGliga11.Model;

namespace mpp_proiect_csharp_DianaGliga11.Repository
{
public interface I_Repository<E> where E : Entity<int>
{
    void Add(E entity);
    void Remove(E entity);
    E findById(int id);
    IEnumerable<E> getAll();
}
}