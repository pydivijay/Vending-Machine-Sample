using VendingMachine.Interfaces;

namespace VendingMachine
{
    public class ValidCoin : ICoinSpecification
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal Diameter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Thickness { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CoinType Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Value { get; set; }
    }
}
