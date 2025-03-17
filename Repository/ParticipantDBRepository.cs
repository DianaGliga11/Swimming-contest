using System;
using System.Collections.Generic;
using System.Data;
using log4net;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace mpp_proiect_csharp_DianaGliga11.Repository;

public class ParticipantDBRepository : DatabaseRepoUtils<int, Participant>, I_ParticipantDBRepository
{
    
    private static readonly ILog log = LogManager.GetLogger(typeof(ParticipantDBRepository));
    
    public ParticipantDBRepository(IDictionary<string, string> props) : base(props)
    {
        log.Info($"{nameof(ParticipantDBRepository)} constructed.");
    }

    protected override Participant DecodeReader(IDataReader reader)
    {
        log.Info($"Decoding Participant from DB: {reader}");
        var id = Convert.ToInt32(reader["id"]);
        var name = Convert.ToString(reader["name"]);
        var age = Convert.ToInt32(reader["age"]);
        var participant = new Participant(name, age);
        participant.Id = id;
        return participant;
    }

    public void Add(Participant entity)
    {
        log.Info($"Adding Participant: {entity}");
        int result = ExecuteNonQuery("insert into \"Participants\" (\"name\", \"age\") " +
                                     "values (@name, @age)", new Dictionary<string, object>
        {
            { "@name", entity.Name },
            { "@age", entity.Age }
            
        });
        if (result == 0)
        {
            log.Error($"Participant was not added: {entity}");
            throw new EntityRepoException("Participant was not added");
        }
        log.Info($"Added successful");    
    }
    

    public void Remove(Participant entity)
    {
        log.Info($"Removing Participant: {entity}");
        var r = ExecuteNonQuery("delete from \"Participants\" where \"Id\"=@id", new Dictionary<string, object>
        {
            { "@id", entity.Id },
        });
        if(r>0)
            log.Info($"Participant was removed: {entity}");
        else
        {
            log.Error($"Participant was not removed");
            throw new EntityRepoException("Participant was not removed");
        }
    }

    public void Update(int id, Participant entity)
    {
        log.Info($"Updating Participant: {entity}");
        int result = ExecuteNonQuery(
            "update \"Participants\" set \"name\"=@name, \"age\"=@age where \"Id\"=@id",
            new Dictionary<string, object>
            {
                { "@name", entity.Name },
                { "@age", entity.Age },
                { "@id", id }
            });
        if( result ==0){
            log.Error($"Participant was not updated: {entity}");
            throw new EntityRepoException("Participant was not updated");
        }
        log.Info($"Updated successful");       }

    public Participant findById(int id)
    {
        log.Info($"Finding Participant: {id}");
        return SelectFirst("select * from \"Participants\" where \"Id\"=@id", new Dictionary<string, object>{
            {"@id", id},
        });    }

    public IEnumerable<Participant> getAll()
    {
        log.Info($"Getting All Participants");
        return Select("select * from \"Participants\"");    }
}