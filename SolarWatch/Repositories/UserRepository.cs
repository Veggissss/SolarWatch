using SolarWatch.Models;

namespace SolarWatch.Repositories;

public class UserRepository(SolarWatchDbContext context) : IUserRepository
{
    public void Create(User entity)
    {
        context.Add(entity);
        context.SaveChanges();
    }

    public IEnumerable<User> ReadAll()
    {
        return context.Users;
    }

    public User? Read(int id)
    {
        return context.Users.Find(id);
    }

    public void Update(User entity)
    {
        context.Update(entity);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = context.Users.Find(id);
        if (entity != null)
        {
            context.Remove(entity);
        }
    }
}