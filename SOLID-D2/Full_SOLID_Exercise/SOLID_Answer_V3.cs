using SOLID_D2.Full_SOLID_Exercise_SOLID_Problem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//1- SRP SIngle Responsibility Pronciple
//2- OCP Open/Closed Principle
//3- LSP Liskov Substitution Principle //not needed here
//4- ISP Interface Segregation Principle 
//5- DIP Dependency Inversion Pronciple
// IoC 
//Dependency Injection

namespace SOLID_D2.Full_SOLID_Exercise_SOLID_Answer_V3
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
        public IPaymentMethod PaymentMethod { get; private set; }

        public Order(string customerName, List<Product> products, decimal totalCost, IPaymentMethod paymentMethod)
        {
            this.CustomerName = customerName;
            this.Products = products;
            this.TotalCost = totalCost;
            this.PaymentMethod = paymentMethod;
        }
    }
    public class OrderService
    {
        private List<Order> orders = new List<Order>();
        private IOrderPaymentProcessor paymentProcessor ;
        private IOrderPriceCalculator orderPriceCalculator ;
        private IOrderConfirmationSender orderConfirmationSender;
        private IProductAvailabilityChecker ProductAvailabilityChecker;

        public OrderService(
            IProductAvailabilityChecker ProductAvailabilityChecker,
            IOrderPriceCalculator orderPriceCalculator,
            IOrderConfirmationSender orderConfirmationSender,
            IOrderPaymentProcessor paymentProcessor)
        {
            this.ProductAvailabilityChecker = ProductAvailabilityChecker;
            this.orderPriceCalculator = orderPriceCalculator;
            this.orderConfirmationSender = orderConfirmationSender;
            this.paymentProcessor = paymentProcessor;
        }
        public void CreateOrder(string customerName, string productName, int quantity, IPaymentMethod paymentMethod)
        {
            decimal totalPrice = orderPriceCalculator.CalculateTotalPrice(productName, quantity);
            List<Product> orderedProducts = ProductAvailabilityChecker.check(productName,quantity);
            if (orderedProducts.Count == 0)
                throw new ArgumentException("Insufficient product quantity");

            Order order = new Order(customerName, orderedProducts, totalPrice, paymentMethod);
            orders.Add(order);
            paymentProcessor.ProcessPayment(totalPrice, paymentMethod);
            //ProcessPayment(totalPrice, paymentMethod);
            orderConfirmationSender.SendOrderConfirmation(order);
        }

    }
    // SRP Classes //////////////////////////////////////
    public class OrderPaymentProcessor:IOrderPaymentProcessor
    {
        public void ProcessPayment(decimal amount, IPaymentMethod paymentMethod)
        {
            paymentMethod.pay(amount);
        }
    }


    public class OrderPriceCalculator:IOrderPriceCalculator
    {
        public IProductRepository productRepo { get; }
        public OrderPriceCalculator(IProductRepository productRepo)
        {
            this.productRepo = productRepo;
        }
        public decimal CalculateTotalPrice(string productName, int quantity)
        {
            decimal productPrice = 0;
            foreach (Product product in productRepo.GetAll())
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

    public class OrderConfirmationSender:IOrderConfirmationSender
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

    public class ProductAvailabilityChecker:IProductAvailabilityChecker
    {
        List<Product> products;
        private IProductRepository productRepository;
        public ProductAvailabilityChecker(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
            products = productRepository.GetAll();

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

    // interfaces & classes of O/CP
    public interface IPaymentMethod
    {
        public void pay(decimal amount);
    }

    public class CreditCard : IPaymentMethod
    {
        public void pay(decimal amount)
        {
            Console.WriteLine("Processing credit card payment of amount " + amount);
        }
    }
    public class PayPal : IPaymentMethod
    {
        public void pay(decimal amount)
        {
            Console.WriteLine("Processing PayPal card payment of amount " + amount);
        }
    }

    public class ProductRepository:IProductRepository
    {
        private DBcontext dbcontext;
        public ProductRepository(DBcontext dbcontext)
        {
            this.dbcontext = dbcontext;   
        }
        public List<Product> GetAll()
        {
            return dbcontext.products;
        }
        public List<int> GetProductsByName(string productName)
        {
            List<int> productIds = new List<int>();
            for (int i = 0; i < dbcontext.products.Count; i++)
            {
                if (dbcontext.products[i].Name.Equals(productName))
                {
                    productIds.Add(i);
                }
            }
            return productIds;
        }
    }
    public class DBcontext
    {
        public List<Product> products = new List<Product>();
    }
    // DIP interfaces 
    public interface IProductRepository
    {
        public List<Product> GetAll();
        public List<int> GetProductsByName(string productName);
    }
    public interface IOrderPaymentProcessor
    {
        public void ProcessPayment(decimal amount, IPaymentMethod paymentMethod);

    }
    public interface IOrderPriceCalculator
    {
        public decimal CalculateTotalPrice(string productName, int quantity);
    }
    public interface IOrderConfirmationSender
    {
        public void SendOrderConfirmation(Order order);

    }
    public interface IProductAvailabilityChecker
    {
        public List<Product> check(string productName, int quantity);
    }

}
