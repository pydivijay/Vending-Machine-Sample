namespace VendingMachine
{
    public class VendingResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsRejectedCoin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public InputCoin? RejectedCoin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ItemChange>? Change { get; set; }
    }
}