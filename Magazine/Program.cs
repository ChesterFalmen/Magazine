using Microsoft.EntityFrameworkCore;
using Magazine.EF;
using Magazine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Magazine
{
    internal class Program
    {
        static AutoResetEvent waitHandler = new AutoResetEvent(true);
        static void Main(string[] args)
        {
            DefaultDatabase();
            Console.WriteLine("\nRead: ");
            Read_LINQ_Query_Syntax();
            Create();
            Console.WriteLine("\nCreate: ");
            Read_LINQ_Query_Syntax();
            Update();
            Console.WriteLine("\nUpdate: ");
            Read_LINQ_Query_Syntax();
            Delete();
            Console.WriteLine("\nDelete: ");
            Read_LINQ_Query_Syntax();
        }

        public static void DefaultDatabase()
        {
            AppDbContext context = new AppDbContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public static void Read_LINQ_Query_Syntax()
        {
            AppDbContext context = new AppDbContext();

            var query = from customer in context.Customers
                        join order in context.Orders
                        on customer.Id equals order.CustomerId
                        join articleInOrder in context.Article_In_Orders
                        on order.Id equals articleInOrder.OrderId
                        join article in context.Articles
                        on articleInOrder.ArticleId equals article.Id
                        join producer in context.Producers
                        on article.ProducerId equals producer.Id
                        join category in context.Categories
                        on article.CategoryId equals category.Id
                        select new
                        {
                            Customer = customer,
                            Order = order,
                            ArticleInOrder = articleInOrder,
                            Article = article,
                            Producer = producer,
                            Category = category
                        };

            foreach (var item in query)
            {
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine($"Customer: {item.Customer.Name} {item.Customer.Surname}");
                Console.WriteLine($"Order: [{item.Order.Id}] {item.Order.Sum}$");
                Console.WriteLine($"Article: {item.Article.Name} - {item.Article.Price}$");
                Console.WriteLine($"Article in order: [{item.ArticleInOrder.Id}] {item.ArticleInOrder.Price}$");
                Console.WriteLine($"Producer: {item.Producer.Name}");
                Console.WriteLine($"Category: {item.Category.Name}");
            }

        }

        public static void Delete()
        {
            AppDbContext context = new AppDbContext();
            var temp = context.Articles.Where(x => x.Name == "Iphone 14 256 Gb Purple").Single();
            context.Articles.Remove(temp);
            context.SaveChanges();
        }
        public static void Create()
        {
            AppDbContext context = new AppDbContext();

            Article article = new Article { Name = "Iphone 13 128 Gb Space Gray", Price = 45000 };
            ArticleInOrder articleInOrder = new ArticleInOrder { Price = 45000 };
            Category category = new Category { Name = "Phone", Count = 1 };
            Customer customer = new Customer { Name = "Petro", Surname = "Vasylenko" };
            Order order = new Order { Count = 1, Sum = 55000 };
            Producer producer = new Producer { Name = "Apples" };
            Promo promo = new Promo { Name = "HB", Sum = 1000 };
            UsingPromo usingPromo = new UsingPromo { };

            articleInOrder.Article = article;
            articleInOrder.Order = order;

            article.Producer = producer;
            article.Category = category;

            order.Customer = customer;
            order.UsingPromo = usingPromo;

            promo.UsingPromo = usingPromo;
            promo.Customer = customer;
            
            usingPromo.Promo = promo;
            usingPromo.Order = order;

            context.Add(articleInOrder);
            context.Add(article);
            context.Add(order);
            context.Add(promo);
            context.Add(usingPromo);

            context.SaveChanges();
        }

        public static void Update()
        {
            AppDbContext context = new AppDbContext();

            var temp = context.Orders.Include(x => x.Customer)
                .Where(x => x.CustomerId == 1).First();

            temp.Customer.Surname = "Petrov";

            context.SaveChanges();
        }


    }
}
