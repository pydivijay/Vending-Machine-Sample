using Moq;
using VendingMachine.Interfaces;
using VendingMachine.UnitTests.Helpers;
using Xunit;

namespace VendingMachine.UnitTests
{
    public class VendingMachineProcessorTests
    {
        private Mock<IProductService> _productService;
        private Mock<ICoinService> _coinService;

        public VendingMachineProcessorTests()
        {
            _productService = new Mock<IProductService>();
            _coinService = new Mock<ICoinService>();
        }

        [Fact]
        public void VendingMachine_CoinService_IsNull_ExceptionThrown()
        {
            // Arrange + Act + Assert
            AssertException.Throws<ArgumentNullException>(() => new VendingMachineProcessor(null, _productService.Object), "Value cannot be null. (Parameter 'coinService parameter is null')");
        }

        [Fact]
        public void VendingMachine_ProductService_IsNull_ExceptionThrown()
        {
            // Arrange + Act + Assert
            AssertException.Throws<ArgumentNullException>(() => new VendingMachineProcessor(_coinService.Object, null), "Value cannot be null. (Parameter 'productService parameter is null')");
        }

        [Fact]
        public void VendingMachine_AcceptCoin_NullCoinEntered()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);

            // Assert
            AssertException.Throws<ArgumentNullException>(() => vendingMachine.AcceptCoin(null), "Value cannot be null. (Parameter 'Coin parameter null!')");
        }

        [Fact]
        public void VendingMachine_AcceptCoin_InvalidCoinEntered()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.AcceptCoin(CreateInvalidCoin());

            // Assert
            Assert.Equal(result != null, true);
            Assert.Equal(result.IsRejectedCoin, true);
        }

        [Fact]
        public void VendingMachine_AcceptCoin_ValidCoinEntered()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedFiveCentCoin);

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.AcceptCoin(CreateValidFiveCentCoin());

            // Assert
            Assert.Equal(result != null, true);
            Assert.Equal(result.IsRejectedCoin, false);
            Assert.Equal(result.IsSuccess, true);
            Assert.Equal(result.Message, "0.25");

        }

        [Fact]
        public void VendingMachine_AcceptCoin_VerifyCoinServiceIsCalledOnce()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()));

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.AcceptCoin(CreateInvalidCoin());

            // Assert
            _coinService.Verify(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once);
        }

        [Fact]
        public void VendingMachine_SelectProduct_InvalidCode_ExceptionThrown()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);

            // Assert
            AssertException.Throws<ArgumentNullException>(() => vendingMachine.SelectProduct(""), "Value cannot be null. (Parameter 'Code parameter empty!')");
        }

        [Fact]
        public void VendingMachine_SelectProduct_InvalidCodeEntered()
        {
            // Arrange
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.SelectProduct("INVALIDCODE");

            // Assert
            Assert.Equal(result != null, true);
            Assert.Equal(result.IsSuccess, false);
            Assert.Equal(result.Message, "Invalid Product Selected. Please try again");
        }

        [Fact]
        public void VendingMachine_SelectProduct_InvalidCodeEntered_VerifyGetProductCalledOnce()
        {
            // Arrange
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.SelectProduct("INVALIDCODE");

            // Assert
            _productService.Verify(mock => mock.GetProduct(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void VendingMachine_SelectProduct_NoCoinsEntered()
        {
            // Arrange
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.SelectProduct("COKE1");

            // Assert
            Assert.Equal(result != null, true);
            Assert.Equal(result.IsSuccess, false);
            Assert.Equal(result.Message, "Insert Coin");
        }

        [Fact]
        public void VendingMachine_SelectProduct_NoCoinsEntered_VerifyGetProductCalledOnce()
        {
            // Arrange
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.SelectProduct("COKE1");

            // Assert
            _productService.Verify(mock => mock.GetProduct(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void VendingMachine_SelectProduct_CoinsEntered_LessThanProductPrice()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedFiveCentCoin);
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidFiveCentCoin());

            VendingResponse result = vendingMachine.SelectProduct("COLA1");

            // Assert
            Assert.Equal(result != null, true);
            Assert.Equal(result.IsSuccess, false);
            Assert.Equal(result.Message, string.Format("Entered coins value is less than the actual Price : {0}", 1.00m));
        }

        [Fact]
        public void VendingMachine_SelectProduct_CoinsEntered_IsValid()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedTenCentCoin);
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            _productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            _productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidTenCentCoin());

            VendingResponse result = vendingMachine.SelectProduct("COLA1");

            // Assert
            Assert.Equal(result != null, true);
            Assert.Equal(result.IsSuccess, true);
            Assert.Equal(result.Message, "Thank You");
        }

        [Fact]
        public void VendingMachine_SelectProduct_CoinsEnteredAndReturnCoins_IsValid()
        {
            // Arrange            
            _coinService.Setup(mock => mock.GetCoin(2.268m, 17.91m, 1.35m)).Returns(CreateValidAcceptedTenCentCoin);
            _coinService.Setup(mock => mock.GetCoin(5.00m, 20.5m, 1.95m)).Returns(CreateValidAcceptedTwentyFiveCentCoin);
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            _productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            _productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidTenCentCoin());
            vendingMachine.AcceptCoin(CreateValidTwentyFiveCentCoin());

            VendingResponse result = vendingMachine.SelectProduct("CANDY1");

            // Assert
            Assert.Equal(result != null, true);
            Assert.Equal(result.IsSuccess, true);
            Assert.Equal(result.Message, "Thank You");
            Assert.Equal(result.Change != null, true);
            Assert.Equal(result.Change.SingleOrDefault(item => item.Type == CoinType.FiveCent).Number, 80);
            Assert.Equal(result.Change.SingleOrDefault(item => item.Type == CoinType.TenCent) == null, true);
            Assert.Equal(result.Change.SingleOrDefault(item => item.Type == CoinType.TwentyFiveCent) == null, true);
        }

        [Fact]
        public void VendingMachine_SelectProduct_CoinsEnteredAndReturnMoreThanOneCoin_IsValid()
        {
            // Arrange            
            _coinService.Setup(mock => mock.GetCoin(2.268m, 17.91m, 1.35m)).Returns(CreateValidAcceptedTenCentCoin);
            _coinService.Setup(mock => mock.GetCoin(5.00m, 20.5m, 1.95m)).Returns(CreateValidAcceptedFiveCentCoin);
            _coinService.Setup(mock => mock.GetCoin(5.67m, 24.257m, 1.956m)).Returns(CreateValidAcceptedTwentyFiveCentCoin);
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            _productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            _productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidTenCentCoin());
            vendingMachine.AcceptCoin(CreateValidFiveCentCoin());
            vendingMachine.AcceptCoin(CreateValidTwentyFiveCentCoin());

            VendingResponse result = vendingMachine.SelectProduct("COLA1");

            // Assert
            Assert.Equal(result != null, true);
            Assert.Equal(result.IsSuccess, true);
            Assert.Equal(result.Message, "Thank You");
            Assert.Equal(result.Change != null, true);
            Assert.Equal(result.Change.SingleOrDefault(item => item.Type == CoinType.TwentyFiveCent) == null, true);
        }

        [Fact]
        public void VendingMachine_SelectProduct_CoinsEnteredAndReturned_VerifyGetProductQuantityCalledOnce()
        {
            // Arrange            
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedTenCentCoin);
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            _productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            _productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidTenCentCoin());

            VendingResponse result = vendingMachine.SelectProduct("COLA1");

            // Assert
            _productService.Verify(mock => mock.GetProductQuantity(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void VendingMachine_SelectProduct_CoinsEnteredAndReturned_VerifyUpdateProductQuantityCalledOnce()
        {
            // Arrange            
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedTenCentCoin);
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            _productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            _productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidTenCentCoin());

            VendingResponse result = vendingMachine.SelectProduct("COLA1");

            // Assert           
            _productService.Verify(mock => mock.UpdateProductQuantity(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void VendingMachine_ReturnCoins_CoinsEnteredAndReturned_IsValid()
        {
            // Arrange            
            _coinService.Setup(mock => mock.GetCoin(2.268m, 17.91m, 1.35m)).Returns(CreateValidAcceptedTenCentCoin);
            _coinService.Setup(mock => mock.GetCoin(5.00m, 20.5m, 1.95m)).Returns(CreateValidAcceptedFiveCentCoin);
            _coinService.Setup(mock => mock.GetCoin(5.67m, 24.257m, 1.956m)).Returns(CreateValidAcceptedTwentyFiveCentCoin);

            // Act
            var vendingMachine = new VendingMachineProcessor(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidTenCentCoin());
            vendingMachine.AcceptCoin(CreateValidFiveCentCoin());
            vendingMachine.AcceptCoin(CreateValidTwentyFiveCentCoin());

            var result = vendingMachine.ReturnCoins();

            // Assert
            Assert.Equal(result != null, true);
            Assert.Equal(result.SingleOrDefault(item => item.Type == CoinType.FiveCent).Number, 20);
        }

        private InputCoin CreateValidFiveCentCoin()
        {
            return new InputCoin()
            {
                Diameter = 20.5m,
                Thickness = 1.95m,
                Weight = 5.00m
            };
        }

        private InputCoin CreateValidTenCentCoin()
        {
            return new InputCoin()
            {
                Diameter = 17.91m,
                Thickness = 1.35m,
                Weight = 2.268m
            };
        }
        private InputCoin CreateValidTwentyFiveCentCoin()
        {
            return new InputCoin()
            {
                Diameter = 24.257m,
                Thickness = 1.956m,
                Weight = 5.67m
            };
        }
        private InputCoin CreateInvalidCoin()
        {
            return new InputCoin()
            {
                Diameter = 999m,
                Thickness = 999m,
                Weight = 999m
            };
        }

        private ValidCoin CreateValidAcceptedFiveCentCoin()
        {
            return new ValidCoin()
            {
                Diameter = 20.5m,
                Thickness = 1.95m,
                Weight = 5.00m,
                Type = CoinType.FiveCent,
                Value = 0.05m
            };
        }

        private ValidCoin CreateValidAcceptedTwentyFiveCentCoin()
        {
            return new ValidCoin()
            {
                Diameter = 24.257m,
                Thickness = 1.956m,
                Weight = 5.67m,
                Type = CoinType.TwentyFiveCent,
                Value = 0.20m
            };
        }

        private ValidCoin CreateValidAcceptedTenCentCoin()
        {
            return new ValidCoin()
            {
                Diameter = 17.91m,
                Thickness = 1.35m,
                Weight = 2.268m,
                Type = CoinType.TenCent,
                Value = 1.00m
            };
        }

        private Product CreateProduct()
        {
            return new Product
            {
                Name = "COLA",
                Code = "COLA1",
                Price = 1.00m,
                Type = ProductItemType.cola
            };
        }
    }
}
