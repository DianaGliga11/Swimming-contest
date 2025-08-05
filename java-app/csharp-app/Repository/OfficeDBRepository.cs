using System;
using System.Collections.Generic;
using System.Data;
using log4net;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Repository;

namespace mpp_proiect_csharp_DianaGliga11.Repository
{
    public class OfficeDBRepository : I_OfficeDBRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OfficeDBRepository));
        private readonly IDictionary<string, string> Props;
        private readonly I_ParticipantDBRepository participantRepository;
        private readonly I_EventDBRepository eventRepository;
        
        public OfficeDBRepository(IDictionary<string, string> props, 
                                I_ParticipantDBRepository participantRepo,
                                I_EventDBRepository eventRepo) 
        {
            log.Info($"{nameof(OfficeDBRepository)} constructed.");
            Props = props;
            participantRepository = participantRepo;
            eventRepository = eventRepo;
        }

        public IEnumerable<Participant> findParticipantsByEvent(long eventId)
        {
            log.Info($"Finding participants for event: {eventId}");
            var connection = DBUtils.GetConnection(Props);
            List<Participant> participants = new List<Participant>();
            
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT idParticipant, name, age FROM Offices WHERE idEvent = @eventId";
                    var param = command.CreateParameter();
                    param.ParameterName = "@eventId";
                    param.Value = eventId;
                    command.Parameters.Add(param);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = reader.GetInt64(0);
                            string name = reader.GetString(1);
                            int age = reader.GetInt32(2);
                            Participant participant = new Participant(name, age) { Id = id };
                            participants.Add(participant);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error finding participants by event", ex);
                throw new EntityRepoException(ex);
            }
            
            return participants;
        }

        public void deleteByIDs(long participantID, long eventID)
        {
            log.Info($"Deleting task for participantID={participantID}, eventID={eventID}");
            var connection = DBUtils.GetConnection(Props);

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Offices WHERE idParticipant = @idParticipant AND idEvent = @idEvent";

                var param1 = command.CreateParameter();
                param1.ParameterName = "@idParticipant";
                param1.Value = participantID;
                command.Parameters.Add(param1);

                var param2 = command.CreateParameter();
                param2.ParameterName = "@idEvent";
                param2.Value = eventID;
                command.Parameters.Add(param2);

                command.ExecuteNonQuery();
                log.Info($"Deleted task for participantID={participantID}");
            }
            catch (Exception ex)
            {
                log.Error($"Error deleting task for participantID={participantID}", ex);
                throw new EntityRepoException("Error deleting task");
            }
        }


        public int countEventsForParticipant(long participantId)
        {
            log.Info($"Counting events for participant: {participantId}");
            var connection = DBUtils.GetConnection(Props);
            
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM Offices WHERE idParticipant = @participantId";
                    var param = command.CreateParameter();
                    param.ParameterName = "@participantId";
                    param.Value = participantId;
                    command.Parameters.Add(param);

                    var result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error counting events for participant", ex);
                throw new EntityRepoException(ex);
            }
        }
        
        public void Add(Office entity)
        {
            log.Info($"Adding office: {entity}");
            var connection = DBUtils.GetConnection(Props);
            
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO Offices(idEvent, style, distance, idParticipant, name, age) 
                                          VALUES (@idEvent, @style, @distance, @idParticipant, @name, @age)";
                    
                    var idEventParam = command.CreateParameter();
                    idEventParam.ParameterName = "@idEvent";
                    idEventParam.Value = entity.Event.Id;
                    command.Parameters.Add(idEventParam);
                    
                    var styleParam = command.CreateParameter();
                    styleParam.ParameterName = "@style";
                    styleParam.Value = entity.Event.Style;
                    command.Parameters.Add(styleParam);
                    
                    var distanceParam = command.CreateParameter();
                    distanceParam.ParameterName = "@distance";
                    distanceParam.Value = entity.Event.Distance;
                    command.Parameters.Add(distanceParam);
                    
                    var idParticipantParam = command.CreateParameter();
                    idParticipantParam.ParameterName = "@idParticipant";
                    idParticipantParam.Value = entity.Participant.Id;
                    command.Parameters.Add(idParticipantParam);
                    
                    var nameParam = command.CreateParameter();
                    nameParam.ParameterName = "@name";
                    nameParam.Value = entity.Participant.Name;
                    command.Parameters.Add(nameParam);
                    
                    var ageParam = command.CreateParameter();
                    ageParam.ParameterName = "@age";
                    ageParam.Value = entity.Participant.Age;
                    command.Parameters.Add(ageParam);
                    
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error adding office", ex);
                throw new EntityRepoException(ex);
            }
        }

        public void Remove(long id)
        {
            log.Info($"Removing office: {id}");
            var connection = DBUtils.GetConnection(Props);
            
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Offices WHERE id = @id";
                    var param = command.CreateParameter();
                    param.ParameterName = "@id";
                    param.Value = id;
                    command.Parameters.Add(param);
                    
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error removing office", ex);
                throw new EntityRepoException(ex);
            }
        }

        public void Update(long id, Office entity)
        {
            log.Info($"Updating office: {id}");
            var connection = DBUtils.GetConnection(Props);
            
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE Offices SET 
                                          idEvent = @idEvent, 
                                          style = @style, 
                                          distance = @distance, 
                                          idParticipant = @idParticipant, 
                                          name = @name, 
                                          age = @age 
                                          WHERE id = @id";
                    
                    var idEventParam = command.CreateParameter();
                    idEventParam.ParameterName = "@idEvent";
                    idEventParam.Value = entity.Event.Id;
                    command.Parameters.Add(idEventParam);
                    
                    var styleParam = command.CreateParameter();
                    styleParam.ParameterName = "@style";
                    styleParam.Value = entity.Event.Style;
                    command.Parameters.Add(styleParam);
                    
                    var distanceParam = command.CreateParameter();
                    distanceParam.ParameterName = "@distance";
                    styleParam.Value = entity.Event.Distance;
                    command.Parameters.Add(distanceParam);
                    
                    var idParticipantParam = command.CreateParameter();
                    idParticipantParam.ParameterName = "@idParticipant";
                    idParticipantParam.Value = entity.Participant.Id;
                    command.Parameters.Add(idParticipantParam);
                    
                    var nameParam = command.CreateParameter();
                    nameParam.ParameterName = "@name";
                    nameParam.Value = entity.Participant.Name;
                    command.Parameters.Add(nameParam);
                    
                    var ageParam = command.CreateParameter();
                    ageParam.ParameterName = "@age";
                    ageParam.Value = entity.Participant.Age;
                    command.Parameters.Add(ageParam);
                    
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error updating office", ex);
                throw new EntityRepoException(ex);
            }
        }

        public Office findById(long id)
        {
            log.Info($"Finding office by ID: {id}");
            var connection = DBUtils.GetConnection(Props);
            
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Offices WHERE id = @id";
                    var param = command.CreateParameter();
                    param.ParameterName = "@id";
                    param.Value = id;
                    command.Parameters.Add(param);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return Extract(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error finding office by ID", ex);
                throw new EntityRepoException(ex);
            }

            return null;
        }

        public IEnumerable<Office> getAll()
        {
            log.Info("Getting all offices");
            var connection = DBUtils.GetConnection(Props);
            List<Office> offices = new List<Office>();
            
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Offices";
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            offices.Add(Extract(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error getting all offices", ex);
                throw new EntityRepoException(ex);
            }
            
            return offices;
        }

        public IEnumerable<Office> getEntriesByEvent(long eventId)
        {
            log.Info($"Getting offices for event: {eventId}");
            var connection = DBUtils.GetConnection(Props);
            List<Office> offices = new List<Office>();
            
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Offices WHERE idEvent = @eventId";
                    var param = command.CreateParameter();
                    param.ParameterName = "@eventId";
                    param.Value = eventId;
                    command.Parameters.Add(param);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            offices.Add(Extract(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error getting offices by event", ex);
                throw new EntityRepoException(ex);
            }
            
            return offices;
        }
        

        private Office Extract(IDataReader reader)
        {
            long eventId = reader.GetInt64(reader.GetOrdinal("idEvent"));
            string style = reader.GetString(reader.GetOrdinal("style"));
            int distance = reader.GetInt32(reader.GetOrdinal("distance"));
            Event evt = new Event(style, distance) { Id = eventId };

            long participantId = reader.GetInt64(reader.GetOrdinal("idParticipant"));
            string name = reader.GetString(reader.GetOrdinal("name"));
            int age = reader.GetInt32(reader.GetOrdinal("age"));
            Participant participant = new Participant(name, age) { Id = participantId };

            return new Office(participant, evt);
        }
    }
}