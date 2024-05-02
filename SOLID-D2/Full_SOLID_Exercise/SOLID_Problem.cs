using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_D2.Full_SOLID_Exercise_SOLID_Problem
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class Customer
    {
        public string CustomerName { get; set; }
        public List<Product> Products { get; set; }
        public decimal TotalCost { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class OrderService
    {
        private List<Product> products = new List<Product>();
        private List<Order> orders = new List<Order>();

        public void CreateOrder(string customerName, string productName, int quantity, string paymentMethod)
        {
            decimal totalPrice = CalculateTotalPrice(productName, quantity);
            List<Product> orderedProducts = new List<Product>();

            foreach (int productId in GetProductsByName(productName))
            {
                if (products[productId].Quantity >= quantity)
                {
                    products[productId].Quantity -= quantity;
                    orderedProducts.Add(products[productId]);
                    break;
                }
            }

            if (orderedProducts.Count == 0)
            {
                throw new ArgumentException("Insufficient product quantity");
            }

            Order order = new Order(customerName, orderedProducts, totalPrice, paymentMethod);
            orders.Add(order);

            ProcessPayment(totalPrice, paymentMethod);
            SendOrderConfirmation(order);
        }

        private decimal CalculateTotalPrice(string productName, int quantity)
        {
            decimal productPrice = 0;
            foreach (Product product in products)
            {
                if (product.Name.Equals(productName))
                {
                    productPrice = product.Price;
                    break;
                }
            }
            return productPrice * quantity;
        }

        private List<int> GetProductsByName(string productName)
        {
            List<int> productIds = new List<int>();
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].Name.Equals(productName))
                {
                    productIds.Add(i);
                }
            }
            return productIds;
        }

        private void ProcessPayment(decimal amount, string paymentMethod)
        {
            if (paymentMethod.Equals("CreditCard"))
            {
                // Simulate credit card payment
                Console.WriteLine("Processing credit card payment of amount " + amount);
            }
            else if (paymentMethod.Equals("PayPal"))
            {
                // Simulate PayPal payment
                Console.WriteLine("Processing PayPal payment of amount " + amount);
            }
            else
            {
                throw new ArgumentException("Invalid payment method");
            }
        }

        private void SendOrderConfirmation(Order order)
        {
            string message = "Order confirmation for customer " + order.CustomerName + "\n";
            message += "Total cost: " + order.TotalCost + "\n";
            message += "Products:\n";
            foreach (Product product in order.Products)
            {
                message += product.Name + " (" + product.Price + ")\n";
            }
            Console.WriteLine(message);
        }
    }

    public class Order
    {
        public string CustomerName { get; private set; }
        public List<Product> Products { get; private set; }
        public decimal TotalCost { get; private set; }
        public string PaymentMethod { get; private set; }

        public Order(string customerName, List<Product> products, decimal totalCost, string paymentMethod)
        {
            this.CustomerName = customerName;
            this.Products = products;
            this.TotalCost = totalCost;
            this.PaymentMethod = paymentMethod;
        }
    }

}
