using VendingMachine.Interfaces;

namespace VendingMachine
{
    public class VendingMachineProcessor : IVendingMachine
    {
        #region Private Members
        private readonly IProductService _productService;
        private readonly ICoinService _coinService;
        private decimal _cost; 
        #endregion
        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="coinService"></param>
        /// <param name="productService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public VendingMachineProcessor(ICoinService coinService, IProductService productService)
        {
            if (coinService == null) throw new ArgumentNullException("coinService parameter is null");
            if (productService == null) throw new ArgumentNullException("productService parameter is null");

            _coinService = coinService;
            _productService = productService;
        }
        /// <summary>
        /// Accept Coin
        /// </summary>
        /// <param name="coin"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public VendingResponse AcceptCoin(InputCoin coin)
        {
            VendingResponse response = new VendingResponse();

            if (coin == null)
                throw new ArgumentNullException("Coin parameter null!");

            //check if the values correspond to an accepted coin            
            var currentCoin = _coinService.GetCoin(coin.Weight, coin.Diameter, coin.Thickness);

            //not a valid coin
            if (currentCoin == null)
            {
                response.Message = "Insert Coin";
                response.IsRejectedCoin = true;
                response.RejectedCoin = coin; //return rejected coin
                return response;
            }

            //valid coin
            _cost = currentCoin.Value * 5;
            response.Message = _cost.ToString();
            response.IsSuccess = true;
            response.IsRejectedCoin = false;
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public VendingResponse SelectProduct(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException("Code parameter empty!");

            var response = new VendingResponse();

            //check if the code is valid            
            //if no, return error object with details
            var product = _productService.GetProduct(code);

            //invalid code entered
            if (product == null)
            {
                response.Message = "Invalid Product Selected. Please try again";
                response.IsSuccess = false;
                return response;
            }

            //no coins entered, but selection pressed
            if (_cost == 0)
            {
                //if exact change item, message = "exact change only"
                response.Message = "Insert Coin";
                response.IsSuccess = false;
                return response;
            }

            //entered coins less than cost
            if (_cost < product.Price)
            {
                response.Message = string.Format("Entered coins value is less than the actual Price : {0}", product.Price);
                response.IsSuccess = false;
                return response;
            }

            //all good, valid code and valid amount entered
            var quantity = _productService.GetProductQuantity(code);
            if (quantity > 0)
            {
                response.Message = "Thank You";
                response.IsSuccess = true;
                _productService.UpdateProductQuantity(code);
                response.Change = MakeChange(Convert.ToDouble(_cost - product.Price));
                _cost = 0.00m;
                return response;
            }

            response.Message = "SOLD OUT";
            response.IsSuccess = false;
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemChange> ReturnCoins()
        {
            return MakeChange(Convert.ToDouble(_cost));
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private IEnumerable<ItemChange> MakeChange(double input)
        {
            List<ItemChange> itemchange = new List<ItemChange>();

            var coins = GetCoinValuesDictionary();

            var change = input;
            if (change == 0) return itemchange;

            foreach (var value in coins.Keys)
            {
                var result = (int)(change / coins[value]);
                if (result > 0)
                {
                    itemchange.Add(new ItemChange
                    {
                        Type = value,
                        Number = result
                    });

                    change = Math.Round(change - (result * coins[value]), 3);
                    var remainingAmount = change;
                    if (remainingAmount == 0)
                        return itemchange;
                }
            }
            return itemchange;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Dictionary<CoinType, double> GetCoinValuesDictionary()
        {
            return new Dictionary<CoinType, double>
            {
                {CoinType.FiveCent, 0.05},
                {CoinType.TenCent, 0.10},
                {CoinType.TwentyFiveCent, 0.25}
            };
        } 
        #endregion
    }
}
