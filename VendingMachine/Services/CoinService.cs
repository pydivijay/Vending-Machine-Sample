using VendingMachine.Interfaces;

namespace VendingMachine
{
    public class CoinService : ICoinService
    {
        private static readonly IEnumerable<ValidCoin> AcceptedCoins = new List<ValidCoin>() {
                new ValidCoin() {Diameter = 20.5m, Thickness = 1.95m, Type = CoinType.FiveCent, Weight = 5.00m, Value = 0.05m},
                new ValidCoin() {Diameter = 17.91m, Thickness = 1.35m, Type = CoinType.TenCent, Weight = 2.268m, Value = 0.10m},
                new ValidCoin() {Diameter = 24.257m, Thickness = 1.956m, Type = CoinType.TwentyFiveCent, Weight = 5.67m, Value = 0.25m},
                new ValidCoin(){Type=CoinType.FiftyCent,Value=0.50m},
                };
        /// <summary>
        /// Get Coin
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="diameter"></param>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public ValidCoin GetCoin(decimal weight, decimal diameter, decimal thickness)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return AcceptedCoins.FirstOrDefault(x => x.Weight == weight && x.Thickness == thickness && x.Diameter == diameter);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public ValidCoin GetCoinType(decimal value)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return AcceptedCoins.FirstOrDefault(x => x.Value == value);
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
