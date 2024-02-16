using eCommerce.API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eCommerce.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _connection;

    public UserRepository()
    {
        _connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=eCommerce-ADO.NET;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }

    private static IList<User> _db = new List<User>()
    {
        new() { Id = 1, Name = "Juan Fernandez", Email = "juan.fernandez@email.com" },
        new() { Id = 2, Name = "Maria Lupita", Email = "Maria.lupita@email.com" },
        new() { Id = 3, Name = "Ramirez Gonzalez", Email = "ramirez.Gonzalez@email.com" },
    };

    public void Delete(int id)
    {
        _db.Remove(Get(id));
    }

    public IList<User> Get()
    {
        IList<User> users = new List<User>();
    
        try
        {
            SqlCommand command = new("SELECT * FROM Users", (SqlConnection)_connection);

            _connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                User user = new()
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Email = reader.GetString("Email"),
                    Gender = Convert.ToChar(reader.GetString("Gender")),
                    RG = reader.GetString("RG"),
                    CPF = reader.GetString("CPF"),
                    MothersName = reader.GetString("MothersName"),
                    Status = Convert.ToChar(reader.GetString("Status")),
                    RegistrationDate = reader.GetDateTimeOffset(8)
                };

                users.Add(user);
            }
            
        }
        finally
        {
            _connection.Close();
        }

        return users;
    }

    public User Get(int id)
    {
        return _db.FirstOrDefault(user => user.Id == id);
    }

    public void Insert(User user)
    {
        User lastUser = _db.LastOrDefault();

        //user.Id = lastUser is null ? 1 : (lastUser.Id + 1);

        _db.Add(user);
    }

    public void Update(User user)
    {
        Delete(user.Id);
        _db.Add(user);
    }
}
