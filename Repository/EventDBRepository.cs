using System;
using System.Collections.Generic;
using System.Data;
using log4net;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace mpp_proiect_csharp_DianaGliga11.Repository;

public class EventDBRepository: DatabaseRepoUtils<int, Event>, I_EventDBRepository
{
    private static readonly ILog log = LogManager.GetLogger(typeof(EventDBRepository));
    public EventDBRepository(IDictionary<string, string> props) : base(props)
    {
        log.Info($"{nameof(EventDBRepository)} constructed.");
    }

    protected override Event DecodeReader(IDataReader reader)
    {
        log.Info($"Decoding events from {reader}");
        var id = Convert.ToInt32(reader["id"]);
        var style = reader["style"] as string;
        var distance = Convert.ToInt32(reader["distance"]);
        var eventEvent = new Event(style, distance);
        return eventEvent;
    }

    public void Add(Event entity)
    {
        log.Info($"Adding Event: {entity}");
        int result = ExecuteNonQuery("insert into \"Events\" (\"style\", \"distance\") " +
                                     "values (@style, @distance)", new Dictionary<string, object>
        {
            { "@style", entity.Style },
            { "@distance", entity.Distance }
            
        });
        if (result == 0)
        {
            log.Error($"Event was not added: {entity}");
            throw new EntityRepoException("Event was not added");
        }
        log.Info($"Added successful");    
    }


    public void Remove(Event entity)
    {
        log.Info($"Removing Event: {entity}");
        var r = ExecuteNonQuery("delete from \"Events\" where \"Id\"=@id", new Dictionary<string, object>
        {
            { "@id", entity.Id },
        });
        if(r>0)
            log.Info($"Event was removed: {entity}");
        else
        {
            log.Error($"Event was not removed: {entity}");
            throw new EntityRepoException("Event was not removed");
        }
    }

    public void Update(int id, Event entity)
    {
        log.Info($"Updating Event: {entity}");
        int result = ExecuteNonQuery(
            "update \"Events\" set \"style\"=@style, \"distance\"=@distance where \"Id\"=@id",
            new Dictionary<string, object>
            {
                { "@style", entity.Style },
                { "@distance", entity.Distance },
                { "@id", id }
            });
        if( result ==0){
            log.Error($"Event was not updated: {entity}");
            throw new EntityRepoException("Event was not updated");
        }
        log.Info($"Updated successful");        }

    public Event findById(int id)
    {
        try
        {
            log.Info($"Finding Event: {id}");
            return SelectFirst("select * from \"Events\" where \"Id\"=@id", new Dictionary<string, object>
            {
                { "@id", id },
            });
        }
        catch (EntityRepoException e)
        {
            log.Error($"Event was not found: {e.Message}");
            throw new EntityRepoException("Event was not found");
        }
    }

    public IEnumerable<Event> getAll()
    {
        log.Info($"Getting All Events");
        return Select("select * from \"Events\"");    }
}