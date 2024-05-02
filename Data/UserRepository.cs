using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public class UserRepository : IUserRepository
    {
         DataContextEF _entityFramework;

        public UserRepository(IConfiguration configuration)
        {
            // Access EF and pass the config to access the connection string into that class
            _entityFramework = new DataContextEF(configuration);

        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if(entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }
        public void RemoveEntity<T>(T entityToRemove)
        {
            if(entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
            }
        }
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _entityFramework.Users.ToList<User>();
            return users;
        }
        public User GetSingleUser(int userId)
        {
            User? user = _entityFramework.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
            if(user != null)
            {
                return user;
            }
            throw new Exception("Failed to Get User");
        }

    }
}