using VendingMachine.Interfaces;

namespace VendingMachine
{
    public class ItemChange
    {
        /// <summary>
        /// 
        /// </summary>
        public CoinType Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Number { get; set; }
    }
}