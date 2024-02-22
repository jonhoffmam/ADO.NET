namespace eCommerce.API.Models;

public class User
{
    public int Id { get; set;}
    public string Name { get; set; }
    public string Email { get; set; }
    public char Gender { get; set; }
    public string Rg { get; set; }
    public string Cpf { get; set; }
    public string MothersName { get; set; }
    public char Status { get; set; }
    public DateTimeOffset RegistrationDate { get; set; }
    public Contact Contact { get; set; }
    public ICollection<DeliveryAddress> DeliveryAddresses { get; set; }
    public ICollection<Department> Departments { get; set; }
}
