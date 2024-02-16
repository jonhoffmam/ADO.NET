namespace eCommerce.API.Models;

public class Contact
{
    public int Id { get; init; }
    public int UserId { get; set; }
    public string Telephone { get; set; }
    public string CellPhone { get; set; }
    public User User { get; set; }
}
