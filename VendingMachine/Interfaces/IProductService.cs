﻿namespace VendingMachine.Interfaces
{
    public interface IProductService
    {
        int GetProductQuantity(string code);
        Product GetProduct(string code);
        IEnumerable<Product> GetAllProducts();
        void UpdateProductQuantity(string code);
    }
}