using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService
{
    public class ProductRepositoryMock : IRepository<Product>
    {
        public static readonly Guid ProductOneId = Guid.NewGuid();
        public static readonly Guid ProductTwoId = Guid.NewGuid();

        public Product Get(Guid id)
        {
            return DataStoreMock.Products.SingleOrDefault(x => x.ProductId == id);
        }

        public void Save(Product product)
        {
            var index = DataStoreMock.Products.IndexOf(product);

            if (index == -1)
            {
                DataStoreMock.Products.Add(product);
            }
            else
            {
                DataStoreMock.Products[index] = product;
            }
        }

        public void Dispose()
        {
        }

        private static class DataStoreMock
        {
            public static readonly List<Product> Products = new List<Product>()
            {
                new Product()
                {
                    ProductId = ProductOneId,
                    ProductName = "Product1",
                    QuantityOnHand = 100,
                    QuantityOnOrder = 0,
                    QuantityReserved = 0
                },
                new Product()
                {
                    ProductId = ProductTwoId,
                    ProductName = "Product2",
                    QuantityOnHand = 0,
                    QuantityOnOrder = 100,
                    QuantityReserved = 0
                }
            };
        }
    }
}
