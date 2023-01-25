using VendingMachine.Interfaces;
namespace VendingMachine
{
    public class ProductRepository : IProductRepository
    {
        private static List<Product>? _products;
        /// <summary>
        /// Get Products List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GetProductList()
        {
            return _products ?? (_products = new List<Product>
            {
                new Product() {Code = "COLA1", Type = ProductItemType.cola, Name = "cola", Price = 1.00m},
                new Product() {Code = "CHIPS1", Type = ProductItemType.chips, Name = "chips", Price = 0.50m},
                new Product() {Code = "CANDY1", Type = ProductItemType.candy, Name = "candy", Price = 0.65m},
            });
        }
    }
}