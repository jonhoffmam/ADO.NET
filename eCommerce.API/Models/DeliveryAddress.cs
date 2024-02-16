namespace eCommerce.API.Models;

public class DeliveryAddress
{
    public int Id { get; init; }
    public int UserId { get; set; }
    public string NameAddress { get; init; }
    public string ZipCode { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string Address { get; init; }
    public string Number { get; set; }
    public string Adjunct { get; set; }
    public User User { get; set; }
}
