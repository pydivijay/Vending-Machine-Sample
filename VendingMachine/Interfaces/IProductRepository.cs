namespace VendingMachine.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProductList();
    }
}