namespace VendingMachine
{
    public class InputCoin : ICoinSpecification
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
    }

    public class InputCoinType
    {
        public decimal value { get; set; }

        public int NumberOfCoins { get; set; }
    }
}
