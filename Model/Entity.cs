namespace mpp_proiect_csharp_DianaGliga11.Model
{
    [Serializable]
    public abstract class Entity<ID>
    {
        protected ID Id { get; set; }

        public override String ToString()
        {
            return $"ID={Id}";
        }

    }

}