namespace CarBuilderAPI.Models.DTOs;
public class OrderDTO
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int WheelId { get; set; }
    public int TechnologyId { get; set; }
    public int PaintId { get; set; }
    public int InteriorId { get; set; }
     public WheelsDTO Wheel { get; set; }
    public TechnologyDTO Technology { get; set; }
    public PaintColorDTO PaintColor { get; set; }
    public InteriorDTO Interior { get; set; }
     public bool Fullfilled { get; set; }
    public decimal TotalCost
    {
        get
        {
            decimal totalCost = 0;

            if (Wheel != null && Technology != null && PaintColor != null && Interior != null)
        {
            totalCost = Wheel.Price + Technology.Price + PaintColor.Price + Interior.Price;
        }
            return totalCost;
        }
    }
}