using System;
using System.Collections.Generic;
using System.Data;
using log4net;
using mpp_proiect_csharp_DianaGliga11.Model;
using mpp_proiect_csharp_DianaGliga11.Repository;

namespace mpp_proiect_csharp_DianaGliga11.Repository
{
    public class UserDBRepository : I_UserDBRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserDBRepository));
        private readonly IDictionary<string, string> Props;
        
        public UserDBRepository(IDictionary<string, string> props) 
        {
            log.Info($"{nameof(UserDBRepository)} constructed.");
            Props = props;
        }

        public void Add(User entity)
        {
            log.Info($"Adding User: {entity}");
            var connection = DBUtils.GetConnection(Props);

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO Users (username, password) VALUES (@username, @password)";

                var usernameParam = command.CreateParameter();
                usernameParam.ParameterName = "@username";
                usernameParam.Value = entity.UserName;
                command.Parameters.Add(usernameParam);

                var passwordParam = command.CreateParameter();
                passwordParam.ParameterName = "@password";
                passwordParam.Value = entity.Password;
                command.Parameters.Add(passwordParam);

                var result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error("Error while adding new User ", ex);
                throw new EntityRepoException(ex);
            }

            log.Info($"Added User: {entity}");
        }

        public void Remove(long id)
        {
            log.Info($"Removing User: {id}");
            var connection = DBUtils.GetConnection(Props);
            
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM Users WHERE id = @id";
                var idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = id;
                command.Parameters.Add(idParam);
                var result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error("Error while removing User ", ex);
                throw new EntityRepoException(ex);
            }
            
            log.Info($"Removed User: {id}");
        }

        public void Update(long id, User entity)
        {
            log.Info($"Updating User: {entity}");
            var connection = DBUtils.GetConnection(Props);
            
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "UPDATE Users SET username = @username, password = @password WHERE id = @id";

                var usernameParam = command.CreateParameter();
                usernameParam.ParameterName = "@username";
                usernameParam.Value = entity.UserName;
                command.Parameters.Add(usernameParam);

                var passwordParam = command.CreateParameter();
                passwordParam.ParameterName = "@password";
                passwordParam.Value = entity.Password;
                command.Parameters.Add(passwordParam);

                var idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = id;
                command.Parameters.Add(idParam);

                var result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error("Error while updating User ", ex);
                throw new EntityRepoException(ex);
            }
            
            log.Info($"Updated User: {entity}");
        }

        public User findById(long id)
        {
            log.Info($"Finding User: {id}");
            var connection = DBUtils.GetConnection(Props);
            
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Users WHERE id = @id";
                    var paramID = command.CreateParameter();
                    paramID.ParameterName = "@id";
                    paramID.Value = id;
                    command.Parameters.Add(paramID);

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            User user = Extract(dataReader);
                            log.Info($"Found User: {user}");
                            return user;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while finding User ", ex);
                throw new EntityRepoException("User was not found");
            }

            return null;
        }

        public IEnumerable<User> getAll()
        {
            log.Info("Getting All Users");
            var connection = DBUtils.GetConnection(Props);
            IList<User> users = new List<User>();
            
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Users";
                
                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        User user = Extract(dataReader);
                        users.Add(user);
                    }
                }
            }
            
            log.Info($"Retrieved {users.Count} users");
            return users;
        }

        public User GetUserByCredentials(string username, string password)
        {
            log.Info($"Searching for User: Username={username}");
            var connection = DBUtils.GetConnection(Props);
            
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Users WHERE username = @username AND password = @password";
                    
                    var usernameParam = command.CreateParameter();
                    usernameParam.ParameterName = "@username";
                    usernameParam.Value = username;
                    command.Parameters.Add(usernameParam);

                    var passwordParam = command.CreateParameter();
                    passwordParam.ParameterName = "@password";
                    passwordParam.Value = password;
                    command.Parameters.Add(passwordParam);

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            User user = Extract(dataReader);
                            log.Info($"Found User: {user}");
                            return user;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while finding User by credentials", ex);
                throw new EntityRepoException("User was not found");
            }

            return null;
        }

        private User Extract(IDataReader dataReader)
        {
            long id = dataReader.GetInt64(0);
            string username = dataReader.GetString(1);
            string password = dataReader.GetString(2);

            User user = new User(username, password)
            {
                Id = id
            };

            return user;
        }
    }
}