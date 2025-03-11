namespace mpp_proiect_csharp_DianaGliga11.Model
{
    public interface IHasID<T>
    {
        T GetID();
        void SetID(T Id);
    }
}
