using System;
using System.Collections.Generic;
using System.Data;
using log4net;
using mpp_proiect_csharp_DianaGliga11.Model;

namespace mpp_proiect_csharp_DianaGliga11.Repository;

public class OfficeDBRepository : DatabaseRepoUtils<int, Office>, I_OfficeDBRepository
{
    private static readonly ILog log = LogManager.GetLogger(typeof(OfficeDBRepository));

    public OfficeDBRepository(IDictionary<string, string> props) : base(props)
    {
        log.Info($"{nameof(OfficeDBRepository)} constructed.");
    }
        private string SerializeParticipants(List<Participant> participants)
        {
            return string.Join(";", participants.ConvertAll(p => $"{p.Id}-{p.Name}-{p.Age}"));
        }

        private string SerializeEvents(List<Event> events)
        {
            return string.Join(";", events.ConvertAll(e => $"{e.Id}-{e.Style}-{e.Distance}"));
        }

        private List<Participant> DeserializeParticipants(string data)
        {
            List<Participant> participants = new();
            if (string.IsNullOrEmpty(data)) return participants;

            foreach (var pair in data.Split(";"))
            {
                if (!string.IsNullOrEmpty(pair))
                {
                    var parts = pair.Split("-");
                    int id = int.Parse(parts[0]);
                    string name = parts[1];
                    int age = int.Parse(parts[2]);

                    var participant = new Participant(name, age) { Id = id };
                    participants.Add(participant);
                }
            }
            return participants;
        }

        private List<Event> DeserializeEvents(string data)
        {
            List<Event> events = new();
            if (string.IsNullOrEmpty(data)) return events;

            foreach (var pair in data.Split(";"))
            {
                if (!string.IsNullOrEmpty(pair))
                {
                    var parts = pair.Split("-");
                    int id = int.Parse(parts[0]);
                    string style = parts[1];
                    int distance = int.Parse(parts[2]);

                    var eventObj = new Event(style, distance) { Id = id };
                    events.Add(eventObj);
                }
            }
            return events;
        }

        protected override Office DecodeReader(IDataReader reader)
        {
            log.Info("Decoding Office from DB");
            int id = Convert.ToInt32(reader["id"]);
            string participantsData = reader["participants"] as string;
            string eventsData = reader["events"] as string;

            List<Participant> participants = DeserializeParticipants(participantsData);
            List<Event> events = DeserializeEvents(eventsData);

            var office = new Office(participants, events) { Id = id };
            return office;
        }

        public void Add(Office entity)
        {
            log.Info($"Adding Office: {entity}");
            int result = ExecuteNonQuery(
                "INSERT INTO \"Offices\" (\"participants\", \"events\") VALUES (@participants, @events)",
                new Dictionary<string, object>
                {
                    { "@participants", SerializeParticipants(entity.Participants) },
                    { "@events", SerializeEvents(entity.Events) }
                });

            if (result == 0)
            {
                log.Error($"Office was not added: {entity}");
                throw new EntityRepoException("Office was not added");
            }
            log.Info("Added successfully");
        }

        public void Remove(Office entity)
        {
            log.Info($"Removing Office: {entity}");
            int result = ExecuteNonQuery(
                "DELETE FROM \"Offices\" WHERE \"Id\"=@id",
                new Dictionary<string, object> { { "@id", entity.Id } });

            if (result > 0)
            {
                log.Info($"Office was removed: {entity}");
            }
            else
            {
                log.Error("Office was not removed");
                throw new EntityRepoException("Office was not removed");
            }
        }

        public void Update(int id, Office entity)
        {
            log.Info($"Updating Office: {entity}");
            int result = ExecuteNonQuery(
                "UPDATE \"Offices\" SET \"participants\"=@participants, \"events\"=@events WHERE \"Id\"=@id",
                new Dictionary<string, object>
                {
                    { "@participants", SerializeParticipants(entity.Participants) },
                    { "@events", SerializeEvents(entity.Events) },
                    { "@id", id }
                });

            if (result == 0)
            {
                log.Error($"Office was not updated: {entity}");
                throw new EntityRepoException("Office was not updated");
            }
            log.Info("Updated successfully");
        }

        public Office findById(int id)
        {
            log.Info($"Finding Office by ID: {id}");
            return SelectFirst(
                "SELECT * FROM \"Offices\" WHERE \"Id\"=@id",
                new Dictionary<string, object> { { "@id", id } });
        }

        public IEnumerable<Office> getAll()
        {
            log.Info("Getting all Offices");
            return Select("SELECT * FROM \"Offices\"");
        }
    }
