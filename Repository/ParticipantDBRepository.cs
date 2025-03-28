using System;
using System.Collections.Generic;
using System.Data;
using log4net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using mpp_proiect_csharp_DianaGliga11.Model;
using NLog.Fluent;

namespace mpp_proiect_csharp_DianaGliga11.Repository;

public class ParticipantDBRepository : I_ParticipantDBRepository
{
    
    private static readonly ILog log = LogManager.GetLogger(typeof(ParticipantDBRepository));
    private readonly IDictionary<string, string?> Props;
    public ParticipantDBRepository(IDictionary<string, string?> props) 
    {
        log.Info($"{nameof(ParticipantDBRepository)} constructed.");
        Props = props;
    }

    public void Add(Participant entity)
    {
        log.Info($"Adding Participant: {entity}");
        IDbConnection connection = DbConnectionUtils.GetConnection(Props);

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Participants (name, age) VALUES (@name, @age)";

            var nameParam = command.CreateParameter();
            nameParam.ParameterName = "@name";
            nameParam.Value = entity.Name;
            command.Parameters.Add(nameParam);

            var ageParam = command.CreateParameter();
            ageParam.ParameterName = "@age";
            ageParam.Value = entity.Age;
            command.Parameters.Add(ageParam);

            var result = command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            log.Error("Error while adding new Participant ", ex);
            throw new EntityRepoException(ex);
        }

        Log.Info($"Added Participant: {entity}");
    }
    

    public void Remove(long id)
    {
        log.Info($"Removing Participant: {id}");
       IDbConnection connection = DbConnectionUtils.GetConnection(Props);
       try
       {
           using var command = connection.CreateCommand();
           command.CommandText = @"DELETE FROM Participants WHERE id = @id";
           var idParam = command.CreateParameter();
           idParam.ParameterName = "@id";
           idParam.Value =id;
           command.Parameters.Add(idParam);
           var result = command.ExecuteNonQuery();
       }
       catch (Exception ex)
       {
           log.Error("Error while removing Participant ", ex);
           throw new EntityRepoException(ex);
       }
       Log.Info($"Removed Participant: {id}");
    }

    public void Update(long id, Participant entity)
    {
        log.Info($"Updating Participant: {entity}");
        IDbConnection connection = DbConnectionUtils.GetConnection(Props);
        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE Participants SET name = @name, age = @age WHERE id = @id";

            var nameParam = command.CreateParameter();
            nameParam.ParameterName = "@name";
            nameParam.Value = entity.Name;
            command.Parameters.Add(nameParam);
            
            var ageParam = command.CreateParameter();
            ageParam.ParameterName = "@age";
            ageParam.Value = entity.Age;
            command.Parameters.Add(ageParam);
            var result = command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            log.Error("Error while updating Participant ", ex);
            throw new EntityRepoException(ex);
        }
        Log.Info($"Updated Participant: {entity}");
    }

    public Participant findById(long id)
    {
        log.Info($"Finding Participant: {id}");
        IDbConnection connection = DbConnectionUtils.GetConnection(Props);
        try
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "select * from \"Participants\" where \"Id\"=@id";
                var paramID = command.CreateParameter();
                paramID.ParameterName = "@id";
                paramID.Value = id;
                command.Parameters.Add(paramID);
                using (var dataReader = command.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        Participant participant = Extract(dataReader);
                        Log.Info("Exiting Finding Participant: {id}");
                        return participant;
                    }
                }
            }
        }
        catch (Exception e)
        {
            log.Error("Participant was not found ", e);
            throw new EntityRepoException("Participant was not found");
        }

        return null;
    }

    public IEnumerable<Participant> getAll()
    {
        log.Info($"Getting All Participants");
        IDbConnection connection = DbConnectionUtils.GetConnection(Props);
        IList<Participant> participants = new List<Participant>();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "select * from \"Participants\"";
            using (var dataReader = command.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    Participant participant = Extract(dataReader);
                    participants.Add(participant);
                }
            }
        }
        log.Info("Exiting Get All Participants");
        return participants;
    }

    public Participant GetParticipantsByData(Participant participant)
    {
        log.Info($"Searching for Participant: {participant}");
        IDbConnection connection = DbConnectionUtils.GetConnection(Props);
        try
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "select * from \"Participants\" where \"Id\"=@id and name name=@name and age age=@age";
                var nameParam = command.CreateParameter();
                nameParam.ParameterName = "@name";
                nameParam.Value = participant.Name;
                command.Parameters.Add(nameParam);

                var ageParam = command.CreateParameter();
                ageParam.ParameterName = "@age";
                ageParam.Value = participant.Age;
                command.Parameters.Add(ageParam);
                using (var dataReader = command.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        Participant extracted = Extract(dataReader);
                        Log.Info("Exiting Finding Participant: {id}");
                        return extracted;
                    }
                }
            }
        }
        catch (Exception e)
        {
            log.Error("Participant was not found ", e);
            throw new EntityRepoException("Participant was not found");
        }
        return null;
    }

    private Participant Extract(IDataReader dataReader)
    {
        var id = dataReader.GetInt64(0);
        var name = dataReader.GetString(1);
        var age = dataReader.GetInt32(2);
        var participant = new Participant(name, age);
        participant.Id = id;
        return participant;
    }
}