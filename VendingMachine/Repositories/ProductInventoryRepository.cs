namespace VendingMachine
{
    public class ProductInventoryRepository : IProductInventoryRepository
    {
        private static Dictionary<string, int>? _productQuantities;
        #region Public Methods
        /// <summary>
        /// Get Inventory
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetInventory()
        {
            return _productQuantities ?? (_productQuantities = new Dictionary<string, int> { { "COLA1", 2 }, { "CHIPS1", 0 }, { "CANDY1", 10 } });
        }
        /// <summary>
        /// Update Inventory
        /// </summary>
        /// <param name="code"></param>
        public void UpdateInventory(string code)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var currentCount = _productQuantities[code];
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (currentCount > 0)
                _productQuantities[code]--;
        } 
        #endregion
    }
}