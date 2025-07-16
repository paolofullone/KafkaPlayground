namespace Domain.Models;
public record MessagePackSampleMessage
{
    public Guid Id { get; set; }
    public DateTime MessageDate { get; set; }
    public decimal RandomPrice { get; set; }
    public int RandomQuantity { get; set; }
}
