namespace VetApi.Core;

public class TreatmentDto
{
    public required string Name { get; set; }
    public AnimalTypeEnum Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Price { get; set; }
}
