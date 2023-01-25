namespace VendingMachine
{
    public interface ICoinSpecification
    {
        decimal Diameter { get; set; }
        decimal Weight { get; set; }
        decimal Thickness { get; set; }
    }
}
