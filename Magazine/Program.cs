using Microsoft.EntityFrameworkCore;
using Magazine.EF;
using Magazine.Models;
using System;
using System.Linq;
using System.Threading;

namespace Magazine
{
    internal class Program
    {
        ///union, except, intersect, join, distinct, group by, агрегатних функцій.
        static AutoResetEvent waitHandler = new AutoResetEvent(true);

        static void Main(string[] args)
        {
            
            //приклади використання union, except, intersect, join, distinct, group by, агрегатних функцій.
            UnionExample();
            ExceptExample();
            IntersectExample();
            JoinExample();
            DistinctExample();
            GroupByExample();
            
            //Навести приклади різних стратегій завантаження зв'язаних даних (Eager, Explicit, Lazy)
            EeagerLoading();
            ExplicitLoading();
            LazyLoading();
            
            // Навести приклад завантаження даних що не відслідковуються, їх зміни та збереження
            using (var db = new AppDbContext())
            {
                // відключення відслідковування для всіх запитів
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; 
                Console.WriteLine(db.ChangeTracker.Entries().Count()); // 0
                var client = db.Persons.FirstOrDefault(x => x.Id==4);
                Console.WriteLine($"{client.FirstName} {client.LastName}");
                client.FirstName = "Vasya";
                client.LastName = "Ivanov";
                db.SaveChanges();
            }
            using (var db = new AppDbContext())
            {
                // відключення відслідковування для конкретного запиту
                var client = db.Persons.Where(x=> x.Id==4).AsNoTracking().FirstOrDefault();
                Console.WriteLine($"{client.FirstName} {client.LastName}");
                db.SaveChanges();
            }
           
            // Навести приклади виклику збережених процедур та функцій за допомогою Entity Framework
            using (var db = new AppDbContext())
            {
                var blogs = db.Persons.FromSqlRaw("select * from select_my_table(2);").ToList();
                blogs.ForEach(x => Console.WriteLine($"ID:{x.Id}  {x.FirstName} {x.LastName}"));
            }
        }

        static void UnionExample()
        {
            // Union, для таблиць Article та Category, виберуємо поле Name з Article
            // та вибираємо поле Name з таблиці Category
            using var db = new AppDbContext();
            var result = db.Articles
                 .Select(x => x.Name)
                 .Union(db.Categories.Select(x => x.Name)).ToList();
             // вивід результату на консоль
            result.ForEach(x=> Console.WriteLine(x));        
        }
        static void ExceptExample()
        {
            // //except
            // // виключаємо з таблиці Article всі імена,
            // // які є в таблиці Category
            using var db = new AppDbContext();
            var result = db.Articles
                .Select(x => x.Name)
                .Except(db.Categories.Select(x => x.Name)).ToList();
            
            // вивід результату на консоль
            result.ForEach(x=> Console.WriteLine(x));   
        }
        static void IntersectExample()
        {
            // //intersect
            // // вибираємо з таблиці Article всі імена,
            // // які є в таблиці Category
            using var db = new AppDbContext();
            var result = db.Articles
                .Select(x => x.Name)
                .Intersect(db.Categories.Select(x => x.Name)).ToList();   
            result.ForEach(Console.WriteLine);
        }
        static void JoinExample()
        {
            //join для таблиць Article та Category
            //вибираємо з таблиці Article всі поля
            //та вибираємо поле Name з таблиці Category
            using var db = new AppDbContext();
            var result = db.Articles
                .Join(db.Categories, article => article.CategoryId, category => category.Id,
                    (article, category) => new
                    {
                        Name = article.Name,
                        CategoryName = category.Name
                    }).ToList();

            // вивід результату на консоль
            result.ForEach(x=> Console.WriteLine(x));   
        }
        static void DistinctExample()
        {
            using var db = new AppDbContext();  
            //distinct uniq values
             //вибираємо всі унікальні імена з таблиці Article
             var result = db.Articles
                 .Select(x => x.Name)
                 .Distinct().ToList();
            
             // вивід результату на консоль
             result.ForEach(x=> Console.WriteLine(x));
        }
        static void GroupByExample()
        {
            //group by та Sum
            // вибираємо всі категорії з таблиці Article
            // та вибираємо суму цін з таблиці Article
            using var db = new AppDbContext();
            var result = db.Articles
                .GroupBy(x => x.CategoryId)
                .Select(x => new
                {
                    CategoryId = x.Key,
                    Sum = x.Sum(y => y.Price)
                }).ToList();
            
            // вивід результату на консоль
            result.ForEach(Console.WriteLine);
        }
        static void EeagerLoading()
        {
            //Eager, у одному запиті запиті (Include), завантажуємо всі дані з таблиць Article та Producers
            using var db = new AppDbContext();
            var result = db.Producers.Include(x => x.Articles).ToList();
            foreach (var producer in result)
            {
                Console.WriteLine($"{producer.Name}: {producer.Articles.Count}: {producer.Id}");
                Console.WriteLine($"Articles: ");
                foreach (var article in producer.Articles)
                {
                    Console.WriteLine($"Name: {article.Name}");
                }
                Console.WriteLine("**********************************************");
            }
        }
        static void ExplicitLoading()
        {
            //Explicit явне завантаження, використовуємо метод Load()
            using (var db = new AppDbContext())
            {
                // явно шукаю по id
                var result = db.Producers.Find(4);
                // явно завантажую дані з таблиці Article
                db.Entry(result).Collection(x => x.Articles).Load();
                // вивід результату на консоль
                Console.WriteLine($"{result.Name}: {result.Articles.Count}: {result.Id}");
                Console.WriteLine($"Articles: ");
                foreach (var article in result.Articles)
                {
                    Console.WriteLine($"Name: {article.Name}");
                }

                Console.WriteLine("**********************************************");
            }
        }

        static void LazyLoading()
        {
            // Lazy loading, потрібно включити в конфігурації та додати virtual у навігаційні властивості 
            using var db = new AppDbContext();
            {
                var result = db.Producers.ToList();
                foreach (var producer in result)
                {
                    Console.WriteLine($"{producer.Name}: {producer.Articles.Count}: {producer.Id}");
                    Console.WriteLine($"Articles: ");
                    foreach (var article in producer.Articles)
                    {
                        Console.WriteLine($"Name: {article.Name}");
                    }
            
                    Console.WriteLine("**********************************************");
                }
            }
        }
    }
}