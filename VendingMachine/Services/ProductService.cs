using VendingMachine.Interfaces;

namespace VendingMachine
{
    public class ProductService : IProductService
    {
        #region Private Members
        private readonly IProductRepository _productRepository;
        private readonly IProductInventoryRepository _productInventoryRepository; 
        #endregion
        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productRepository"></param>
        /// <param name="productInventoryRepository"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProductService(IProductRepository productRepository, IProductInventoryRepository productInventoryRepository)
        {
            if (productRepository == null) throw new ArgumentNullException("productRepository parameter is null");
            if (productInventoryRepository == null) throw new ArgumentNullException("productInventoryRepository parameter is null");

            _productRepository = productRepository;
            _productInventoryRepository = productInventoryRepository;
        }
        /// <summary>
        /// Get Product Quantity
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int GetProductQuantity(string code)
        {
            var quantities = _productInventoryRepository.GetInventory();
            return quantities[code];
        }
        /// <summary>
        /// Get Product based on code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Product GetProduct(string code)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return GetAllProducts().FirstOrDefault(x => x.Code == code);
#pragma warning restore CS8603 // Possible null reference return.
        }
        /// <summary>
        /// Get All Products
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetProductList();
        }
        /// <summary>
        /// Update Product Quantity
        /// </summary>
        /// <param name="code"></param>
        public void UpdateProductQuantity(string code)
        {
            _productInventoryRepository.UpdateInventory(code);
        } 
        #endregion
    }
}