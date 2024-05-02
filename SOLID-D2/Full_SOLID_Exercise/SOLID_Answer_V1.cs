using SOLID_D2.Full_SOLID_Exercise_SOLID_Problem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//1- SRP SIngle Responsibility Pronciple

namespace SOLID_D2.Full_SOLID_Exercise_SOLID_Answer_V1
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
    public class OrderService
    {
        private List<Product> products = new List<Product>();
        private List<Order> orders = new List<Order>();
        private ProductRepository productRepository { get; set; } = new ProductRepository();
        private OrderPaymentProcessor paymentProcessor = new OrderPaymentProcessor();
        private OrderPriceCalculator orderPriceCalculator = new OrderPriceCalculator();
        private OrderConfirmationSender orderConfirmationSender = new OrderConfirmationSender();
        private ProductAvailabilityChecker ProductAvailabilityChecker { get; set; } = new ProductAvailabilityChecker();

        public void CreateOrder(string customerName, string productName, int quantity, string paymentMethod)
        {
            decimal totalPrice = orderPriceCalculator.CalculateTotalPrice(productName, quantity);

            //List<Product> orderedProducts = new List<Product>();

            //foreach (int productId in productRepository.GetProductsByName(productName))
            //{
            //    if (products[productId].Quantity >= quantity)
            //    {
            //        products[productId].Quantity -= quantity;
            //        orderedProducts.Add(products[productId]);
            //        break;
            //    }

            //}
            List<Product> orderedProducts = ProductAvailabilityChecker.check(productName,quantity);
            if (orderedProducts.Count == 0)
            {
                throw new ArgumentException("Insufficient product quantity");
            }

            Order order = new Order(customerName, orderedProducts, totalPrice, paymentMethod);
            orders.Add(order);
            paymentProcessor.ProcessPayment(totalPrice, paymentMethod);
            //ProcessPayment(totalPrice, paymentMethod);
            orderConfirmationSender.SendOrderConfirmation(order);
        }

        //private decimal CalculateTotalPrice(string productName, int quantity)
        //{
        //    decimal productPrice = 0;
        //    foreach (Product product in products)
        //    {
        //        if (product.Name.Equals(productName))
        //        {
        //            productPrice = product.Price;
        //            break;
        //        }
        //    }
        //    return productPrice * quantity;
        //}

        //private List<int> GetProductsByName(string productName)
        //{
        //    List<int> productIds = new List<int>();
        //    for (int i = 0; i < products.Count; i++)
        //    {
        //        if (products[i].Name.Equals(productName))
        //        {
        //            productIds.Add(i);
        //        }
        //    }
        //    return productIds;
        //}

        //private void ProcessPayment(decimal amount, string paymentMethod)
        //{
        //    if (paymentMethod.Equals("CreditCard"))
        //    {
        //        // Simulate credit card payment
        //        Console.WriteLine("Processing credit card payment of amount " + amount);
        //    }
        //    else if (paymentMethod.Equals("PayPal"))
        //    {
        //        // Simulate PayPal payment
        //        Console.WriteLine("Processing PayPal payment of amount " + amount);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Invalid payment method");
        //    }
        //}

        //private void SendOrderConfirmation(Order order)
        //{
        //    string message = "Order confirmation for customer " + order.CustomerName + "\n";
        //    message += "Total cost: " + order.TotalCost + "\n";
        //    message += "Products:\n";
        //    foreach (Product product in order.Products)
        //    {
        //        message += product.Name + " (" + product.Price + ")\n";
        //    }
        //    Console.WriteLine(message);
        //}
    }
    // SRP Classes //////////////////////////////////////
    public class OrderPaymentProcessor
    {
        public void ProcessPayment(decimal amount, string paymentMethod)
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
    }

    public class ProductRepository
    {
        public List<Product> products = new List<Product>();
        public List<int> GetProductsByName(string productName)
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
    }
    public class OrderPriceCalculator
    {
        public ProductRepository productRepo { get; set; } = new ProductRepository();
        public OrderPriceCalculator()
        {
            
        }
        public decimal CalculateTotalPrice(string productName, int quantity)
        {
            decimal productPrice = 0;
            foreach (Product product in productRepo.products)
            {
                if (product.Name.Equals(productName))
                {
                    productPrice = product.Price;
                    break;
                }
            }
            return productPrice * quantity;
        }
    }

    public class OrderConfirmationSender
    {
        public void SendOrderConfirmation(Order order)
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

    public class ProductAvailabilityChecker
    {
        List<Product> products = new List<Product>();
        private ProductRepository productRepository;
        public ProductAvailabilityChecker()
        {
            this.productRepository = new ProductRepository();

        }
        public List<Product> check(string productName,int quantity)
        {
            List<Product> orderedProducts = new List<Product>();
            foreach (int productId in productRepository.GetProductsByName(productName))
            {
                if (products[productId].Quantity >= quantity)
                {
                    products[productId].Quantity -= quantity;
                    orderedProducts.Add(products[productId]);
                    break;
                }
            }
            return orderedProducts;
        }

         

    }



}
