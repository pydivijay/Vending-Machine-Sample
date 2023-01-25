using Moq;
using VendingMachine.Interfaces;
using VendingMachine.UnitTests.Helpers;
using Xunit;

namespace VendingMachine.UnitTests
{
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepository;
        private Mock<IProductInventoryRepository> _productInventoryRepository;

        public ProductServiceTests()
        {
            _productRepository = new Mock<IProductRepository>();
            _productInventoryRepository = new Mock<IProductInventoryRepository>();
        }

        [Fact]
        public void ProductService_ProductRepository_IsNull_ExceptionThrown()
        {
            // Arrange + Act + Assert
            AssertException.Throws<ArgumentNullException>(() => new ProductService(null, _productInventoryRepository.Object), "Value cannot be null. (Parameter 'productRepository parameter is null')");
        }

        [Fact]
        public void ProductService_ProductInventoryRepository_IsNull_ExceptionThrown()
        {
            // Arrange + Act + Assert
            AssertException.Throws<ArgumentNullException>(() => new ProductService(_productRepository.Object, null), "Value cannot be null. (Parameter 'productInventoryRepository parameter is null')");
        }

        [Fact]
        public void ProductService_ProductRepository_GetAllProductsIsCalled_VerifyGetProductListIsCalledOnce()
        {
            // Arrange
            _productRepository.Setup(mock => mock.GetProductList()).Returns(new List<Product>());

            // Act
            var productService = new ProductService(_productRepository.Object, _productInventoryRepository.Object);

            var result = productService.GetAllProducts();

            // Assert
            _productRepository.Verify(mock => mock.GetProductList(), Times.Once);
        }

        [Fact]
        public void ProductService_GetProductIsCallled_VerifyGetProductListIsCalledOnce()
        {
            // Arrange
            _productRepository.Setup(mock => mock.GetProductList()).Returns(new List<Product>());

            // Act
            var productService = new ProductService(_productRepository.Object, _productInventoryRepository.Object);

            var result = productService.GetProduct(It.IsAny<string>());

            // Assert
            _productRepository.Verify(mock => mock.GetProductList(), Times.Once);
        }

        [Fact]
        public void ProductService_GetProduct_IsValidProductReturned()
        {
            // Arrange
            _productRepository.Setup(mock => mock.GetProductList()).Returns(CreateProducts());

            // Act
            var productService = new ProductService(_productRepository.Object, _productInventoryRepository.Object);

            var result = productService.GetProduct("COLA1");

            // Assert
            Assert.Equal(result != null, true);
            Assert.Equal(result.Code, "COLA1");
            Assert.Equal(result.Name, "cola");
            Assert.Equal(result.Price, 1.00m);
            Assert.Equal(result.Type, ProductItemType.cola);
        }

        [Fact]
        public void ProductService_GetProductQuantity_IsValidProductQuantityReturned()
        {
            // Arrange
            _productInventoryRepository.Setup(mock => mock.GetInventory()).Returns(CreateProductInventory());

            // Act
            var productService = new ProductService(_productRepository.Object, _productInventoryRepository.Object);

            var result = productService.GetProductQuantity("COLA1");

            // Assert
            Assert.Equal(result, 2);
        }

        [Fact]
        public void ProductService_UpdateProductQuantity_VerifyUpdateInventoryIsCalledOnce()
        {
            // Arrange
            _productInventoryRepository.Setup(mock => mock.UpdateInventory(It.IsAny<string>()));

            // Act
            var productService = new ProductService(_productRepository.Object, _productInventoryRepository.Object);

            productService.UpdateProductQuantity("COLA1");

            // Assert
            _productInventoryRepository.Verify(mock => mock.UpdateInventory(It.IsAny<string>()), Times.Once);
        }

        private List<Product> CreateProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Code = "COLA1",
                    Name = "cola",
                    Price = 1.00m,
                    Type = ProductItemType.cola
                }
            };
        }

        private Dictionary<string, int> CreateProductInventory()
        {
            return new Dictionary<string, int> { { "COLA1", 2 }, { "CHIPS1", 0 }, { "CANDY1", 10 } };
        }
    }
}