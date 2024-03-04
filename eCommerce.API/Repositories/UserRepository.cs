using System.Collections.ObjectModel;
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

    public void Delete(int id)
    {
        try
        {
            SqlCommand command = new("DELETE FROM Users WHERE Id = @Id", (SqlConnection)_connection);
            command.Parameters.AddWithValue("@Id", id);

            _connection.Open();

            command.ExecuteNonQuery();
        }
        finally
        {
            _connection.Close();
        }
    }

    public HashSet<User> Get()
    {
        var users = new HashSet<User>();
    
        try
        {
            const string cmdText = @"SELECT
	                                    u.Id,
	                                    u.Name,
                                        u.Email,
	                                    u.Gender,
	                                    u.RG,
	                                    u.CPF,
	                                    u.MothersName,
	                                    u.Status,
	                                    u.RegistrationDate,
	                                    c.Id AS ContactId,
	                                    c.Telephone,
	                                    c.CellPhone,
	                                    da.Id AS DeliveryAddressId,
	                                    da.NameAddress,
	                                    da.ZipCode,
	                                    da.State,
	                                    da.City,
	                                    da.District,
	                                    da.Address,
	                                    da.Number,
	                                    da.Adjunct,
                                        ud.DepartmentId,
                                        d.Name AS DepartmentName
                                    FROM
	                                    Users u
	                                    LEFT JOIN Contacts c ON c.UserId = u.Id
	                                    LEFT JOIN DeliveryAddresses da ON da.UserId = u.Id
                                        LEFT JOIN UsersDepartments ud ON ud.UserId = u.Id
	                                    LEFT JOIN Departments d ON d.Id = ud.DepartmentId";

            SqlCommand command = new(cmdText, (SqlConnection)_connection);

            _connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            User user = null;

            while (reader.Read())
            {
                SetUser(reader, ref user);

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
        User user = null;

        try
        {
            const string cmdText = @"SELECT
	                                    u.Id,
	                                    u.Name,
                                        u.Email,
	                                    u.Gender,
	                                    u.RG,
	                                    u.CPF,
	                                    u.MothersName,
	                                    u.Status,
	                                    u.RegistrationDate,
	                                    c.Id AS ContactId,
	                                    c.Telephone,
	                                    c.CellPhone,
	                                    da.Id AS DeliveryAddressId,
	                                    da.NameAddress,
	                                    da.ZipCode,
	                                    da.State,
	                                    da.City,
	                                    da.District,
	                                    da.Address,
	                                    da.Number,
	                                    da.Adjunct,
                                        ud.DepartmentId,
                                        d.Name AS DepartmentName
                                    FROM
	                                    Users u
	                                    LEFT JOIN Contacts c ON c.UserId = u.Id
	                                    LEFT JOIN DeliveryAddresses da ON da.UserId = u.Id
                                        LEFT JOIN UsersDepartments ud ON ud.UserId = u.Id
	                                    LEFT JOIN Departments d ON d.Id = ud.DepartmentId
                                    WHERE
	                                    u.Id = @Id";
                                    

            SqlCommand command = new(cmdText, (SqlConnection)_connection);
            command.Parameters.AddWithValue("@Id", id);

            _connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
                SetUser(reader, ref user);

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

    private static void SetUser(SqlDataReader reader, ref User user)
    {
        int userId = reader.GetInt32("Id");

        if (user is not null && userId != user.Id)
            user = null;

        user ??= new User
        {
            Id = userId,
            Name = reader.GetString("Name"),
            Email = reader.GetString("Email"),
            Gender = Convert.ToChar(reader.GetString("Gender")),
            Rg = reader.GetString("RG"),
            Cpf = reader.GetString("CPF"),
            MothersName = reader.GetString("MothersName"),
            Status = Convert.ToChar(reader.GetString("Status")),
            RegistrationDate = reader.GetDateTimeOffset(8),
            Contact = new Contact
            {
                Id = reader.GetInt32("ContactId"),
                UserId = userId,
                Telephone = reader.GetString("Telephone"),
                CellPhone = reader.GetString("CellPhone")
            },
            DeliveryAddresses = new HashSet<DeliveryAddress>(),
            Departments = new HashSet<Department>()
        };

        int deliveryAddressId = reader.GetInt32("DeliveryAddressId");

        if (user.DeliveryAddresses.All(deliveryAddress => deliveryAddress.Id != deliveryAddressId))
            user.DeliveryAddresses.Add(new DeliveryAddress
            {
                Id = deliveryAddressId,
                UserId = reader.GetInt32("Id"),
                NameAddress = reader.GetString("NameAddress"),
                ZipCode = reader.GetString("ZipCode"),
                State = reader.GetString("State"),
                City = reader.GetString("City"),
                District = reader.GetString("District"),
                Address = reader.GetString("Address"),
                Number = reader.GetString("Number"),
                Adjunct = reader.GetString("Adjunct")
            });

        int departmentId = reader.GetInt32("DepartmentId");

        if (user.Departments.All(department => department.Id != departmentId))
            user.Departments.Add(new Department
            {
                Id = departmentId,
                Name = reader.GetString("DepartmentName")
            });
    }
}
