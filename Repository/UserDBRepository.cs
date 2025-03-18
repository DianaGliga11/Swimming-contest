using System;
using System.Collections.Generic;
using System.Data;
using log4net;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace mpp_proiect_csharp_DianaGliga11.Repository;

public class UserDBRepository :DatabaseRepoUtils<int, User>, I_UserDBRepository
{
    
    
    public UserDBRepository(IDictionary<string, string> props) : base(props)
    {
        log.Info($"{nameof(UserDBRepository)} constructed.");
    }

    public void Add(User entity)
    {
        log.Info($"Adding User: {entity}");
        int result = ExecuteNonQuery("insert into \"Users\" (\"username\", \"password\") " +
                                     "values (@username, @password)", new Dictionary<string, object>
        {
            { "@username", entity.UserName },
            { "@password", entity.Password }
            
        });
        if (result == 0)
        {
            log.Error($"User was not added: {entity}");
            throw new EntityRepoException("User was not added");
        }
        log.Info($"Added successful");    }

    public void Remove(User entity)
    {
        log.Info($"Removing User: {entity}");
        var r = ExecuteNonQuery("delete from \"Users\" where \"Id\"=@id", new Dictionary<string, object>
        {
            { "@id", entity.Id },
        });
        if(r>0)
            log.Info($"User was removed: {entity}");
        else
        {
            log.Error($"User was not removed: {entity}");
            throw new EntityRepoException("User was not removed");
        }
    }

    public void Update(int id,User entity)
    {
        log.Info($"Updating User: {entity}");
        int result = ExecuteNonQuery(
            "update \"Users\" set \"username\"=@username, \"password\"=@password where \"Id\"=@id",
            new Dictionary<string, object>
            {
                { "@username", entity.UserName },
                { "@password", entity.Password },
                { "@id", id }
            });
        if( result ==0){
            log.Error($"User was not updated: {entity}");
            throw new EntityRepoException("User was not updated");
        }
        log.Info($"Updated successful");    
    }

    public User findById(int id)
    {
        try
        {
            log.Info($"Finding User: {id}");
            return SelectFirst("select * from \"Users\" where \"Id\"=@id", new Dictionary<string, object>
            {
                { "@id", id },
            });
        }
        catch (EntityRepoException e)
        {
            log.Error($"User was not found: {e.Message}");
            throw new EntityRepoException("User was not found");
        }
    }

    public IEnumerable<User> getAll()
    {
        log.Info($"Getting All Users");
        return Select("select * from \"Users\"");
    }

    public bool checkUserPassword(User user)
    {
        log.Info($"Checking User Password");
        var result = SelectFirst("select * from \"Users\" where \"username\"=@username and \"password\"=@password",
            new Dictionary<string, object>
            {
                { "@username", user.UserName },
                { "@password", user.Password }
            });
        if (result != null)
        {
            log.Info($"User password is valid: {user.UserName}");
            return true;
        }
        else
        {
            log.Error($"User password is not valid: {user.UserName}");
            return false;
        }
    }

    protected override User DecodeReader(IDataReader reader)
    {
        log.Info($"Deconding User for DB: {reader}");
        var id = Convert.ToInt32(reader["id"]);
        var username = reader["username"] as string;
        var password = reader["password"] as string;
        var user = new User(username, password);
        user.Id = id;
        return user;
    }
}