using eCommerce.API.Models;

namespace eCommerce.API.Repositories;

public interface IUserRepository
{
    public HashSet<User> Get();
    public User Get(int id);
    public User Insert(User user);
    public void Update(User user);
    public void Delete(int id);
}
