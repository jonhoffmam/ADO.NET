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
                users.Add(SetUser(reader));
            
        }
        finally
        {
            _connection.Close();
        }

        return users;
    }

    public User Get(int id)
    {
        User user = null;

        try
        {
            SqlCommand command = new("SELECT * FROM Users WHERE Id = @Id", (SqlConnection)_connection);
            command.Parameters.AddWithValue("@Id", id);

            _connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
                user = SetUser(reader);

        }
        finally
        {
            _connection.Close();
        }

        return user;
    }

    public User Insert(User user)
    {
        try
        {
            const string cmdText = @"INSERT INTO Users
                                        (Name, Email, Gender, RG, CPF, MothersName, Status, RegistrationDate)
                                       VALUES
                                        (@Name, @Email, @Gender, @RG, @CPF, @MothersName, @Status, @RegistrationDate);
                                     SELECT CAST(scope_identity() AS int)";


            SqlCommand command = new(cmdText, (SqlConnection)_connection);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Gender", user.Gender);
            command.Parameters.AddWithValue("@RG", user.Rg);
            command.Parameters.AddWithValue("@CPF", user.Cpf);
            command.Parameters.AddWithValue("@MothersName", user.MothersName);
            command.Parameters.AddWithValue("@Status", user.Status);
            command.Parameters.AddWithValue("@RegistrationDate", user.RegistrationDate);

            _connection.Open();

            user.Id = (int)command.ExecuteScalar()!;

            return user;
        }
        finally
        {
            _connection.Close();
        }
    }

    public void Update(User user)
    {
        try
        {
            const string cmdText = @"UPDATE Users SET
                                        Name = @Name,
                                        Email = @Email,
                                        Gender = @Gender,
                                        RG = @RG,
                                        CPF = @CPF,
                                        MothersName = @MothersName,
                                        Status = @Status,
                                        RegistrationDate = @RegistrationDate
                                    WHERE Id = @Id";

            SqlCommand command = new(cmdText, (SqlConnection)_connection);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Gender", user.Gender);
            command.Parameters.AddWithValue("@RG", user.Rg);
            command.Parameters.AddWithValue("@CPF", user.Cpf);
            command.Parameters.AddWithValue("@MothersName", user.MothersName);
            command.Parameters.AddWithValue("@Status", user.Status);
            command.Parameters.AddWithValue("@RegistrationDate", user.RegistrationDate);
            command.Parameters.AddWithValue("@Id", user.Id);

            _connection.Open();

            command.ExecuteNonQuery();
        }
        finally
        {
            _connection.Close();
        }
        
    }

    private static User SetUser(SqlDataReader reader)
    {
        return new User()
        {
            Id = reader.GetInt32("Id"),
            Name = reader.GetString("Name"),
            Email = reader.GetString("Email"),
            Gender = Convert.ToChar(reader.GetString("Gender")),
            Rg = reader.GetString("RG"),
            Cpf = reader.GetString("CPF"),
            MothersName = reader.GetString("MothersName"),
            Status = Convert.ToChar(reader.GetString("Status")),
            RegistrationDate = reader.GetDateTimeOffset(8)
        };
    }
}
