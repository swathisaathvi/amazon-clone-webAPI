using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using amazonCloneWebAPI.Models;

namespace TestAmazonCloneWebAPI.MockData
{
    public class ProductsMockData
    {
        public static List<Product> ProductCollection()
        {
            return new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    ProductName = "Pen",
                    Price = 20,
                    Category = "Stationary"
                },
                 new Product
                {
                    ProductId = 2,
                    ProductName = "Bottle",
                    Price = 20,
                    Category = "Kitchen"
                },
                 new Product
                {
                    ProductId = 3,
                    ProductName = "Hair Band",
                    Price = 20,
                    Category = "Accessories"
                },

            };
        }

        public static Product SingleProduct()
        {
            return new Product()
            {
                ProductId = 4,
                ProductName = "Vicks",
                Price = 12,
                Category = "Meducine"
            };
        }
    }
}
