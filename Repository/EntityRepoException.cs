using System;

namespace mpp_proiect_csharp_DianaGliga11.Repository;

public class EntityRepoException : Exception
{
    public EntityRepoException(string message) : base(message)
    {
        
    }

    public EntityRepoException(Exception innerException) : base(innerException.Message, innerException)
    {
        
    }
}