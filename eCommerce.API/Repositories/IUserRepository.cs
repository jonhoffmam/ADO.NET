using eCommerce.API.Models;

namespace eCommerce.API.Repositories;

public interface IUserRepository
{
    public IList<User> Get();
    public User Get(int id);
    public void Insert(User user);
    public void Update(User user);
    public void Delete(int id);
}
