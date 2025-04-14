using System;
using System.Collections.Generic;
using System.Data;
using log4net;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Repository;

namespace mpp_proiect_csharp_DianaGliga11.Repository
{
    public class EventDBRepository : I_EventDBRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EventDBRepository));
        private readonly IDictionary<string, string> Props;
        
        public EventDBRepository(IDictionary<string, string> props) 
        {
            log.Info($"{nameof(EventDBRepository)} constructed.");
            Props = props;
        }

        public void Add(Event entity)
        {
            log.Info($"Adding Event: {entity}");
            var connection = DBUtils.GetConnection(Props);

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO Events (style, distance) VALUES (@style, @distance)";

                var styleParam = command.CreateParameter();
                styleParam.ParameterName = "@style";
                styleParam.Value = entity.Style;
                command.Parameters.Add(styleParam);

                var distanceParam = command.CreateParameter();
                distanceParam.ParameterName = "@distance";
                distanceParam.Value = entity.Distance;
                command.Parameters.Add(distanceParam);

                var result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error("Error while adding new Event ", ex);
                throw new EntityRepoException(ex);
            }

            log.Info($"Added Event: {entity}");
        }

        public void Remove(long id)
        {
            log.Info($"Removing Event: {id}");
            var connection = DBUtils.GetConnection(Props);
            
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM Events WHERE id = @id";
                var idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = id;
                command.Parameters.Add(idParam);
                var result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error("Error while removing Event ", ex);
                throw new EntityRepoException(ex);
            }
            
            log.Info($"Removed Event: {id}");
        }

        public void Update(long id, Event entity)
        {
            log.Info($"Updating Event: {entity}");
            var connection = DBUtils.GetConnection(Props);
            
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "UPDATE Events SET style = @style, distance = @distance WHERE id = @id";

                var styleParam = command.CreateParameter();
                styleParam.ParameterName = "@style";
                styleParam.Value = entity.Style;
                command.Parameters.Add(styleParam);

                var distanceParam = command.CreateParameter();
                distanceParam.ParameterName = "@distance";
                distanceParam.Value = entity.Distance;
                command.Parameters.Add(distanceParam);

                var idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = id;
                command.Parameters.Add(idParam);

                var result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error("Error while updating Event ", ex);
                throw new EntityRepoException(ex);
            }
            
            log.Info($"Updated Event: {entity}");
        }
        
        public Event findById(long id)
        {
            log.Info($"Finding Event: {id}");
            var connection = DBUtils.GetConnection(Props);
            
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Events WHERE id = @id";
                    var paramID = command.CreateParameter();
                    paramID.ParameterName = "@id";
                    paramID.Value = id;
                    command.Parameters.Add(paramID);

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            Event evt = Extract(dataReader);
                            log.Info($"Found Event: {evt}");
                            return evt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while finding Event ", ex);
                throw new EntityRepoException("Event was not found");
            }

            return null;
        }

        public IEnumerable<Event> getAll()
        {
            log.Info("Getting All Events");
            var connection = DBUtils.GetConnection(Props);
            IList<Event> events = new List<Event>();
            
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Events";
                
                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Event evt = Extract(dataReader);
                        events.Add(evt);
                    }
                }
            }
            
            log.Info($"Retrieved {events.Count} events");
            return events;
        }
        

        private Event Extract(IDataReader dataReader)
        {
            long id = dataReader.GetInt64(0);
            string style = dataReader.GetString(1);
            int distance = dataReader.GetInt32(2);
            Event evt = new Event(style, distance)
            {
                Id = id
            };

            return evt;
        }
    }
}