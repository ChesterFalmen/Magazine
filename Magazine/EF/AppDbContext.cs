using Magazine.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Magazine.EF
{
    public class AppDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleInOrder> Article_In_Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Promo> Promos { get; set; }
        public DbSet<UsingPromo> Using_Promos { get; set; }

        public AppDbContext()
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Article article1 = new Article { Id = 1, Name = "Iphone 13 128 Gb Space Gray", CategoryId = 1, Price = 45000, ProducerId = 1 };
            Article article2 = new Article { Id = 2, Name = "Iphone 14 256 Gb Purple", CategoryId = 1, Price = 56000, ProducerId = 1 };

            ArticleInOrder articleInOrder1 = new ArticleInOrder { Id = 1, ArticleId = 1, Price = 45000};

            Category category1 = new Category { Id = 1, Name = "Phones", Count = 2 };

            Customer customer1 = new Customer { Id = 1, Name = "Vasyl", Surname = "Yurchenko" };
            Customer customer2 = new Customer { Id = 2, Name = "Taras", Surname = "Andryshchak" };

            Order order1 = new Order { Id = 1, Count = 1, CustomerId = 1, Sum = 45000 };
            Order order2 = new Order { Id = 2, Count = 1, CustomerId = 2, Sum = 56000 };

            Producer producer1 = new Producer { Id = 1, Name = "Apple" };

            Promo promo1 = new Promo { Id = 1, UsingPromoId = 1, Name = "NewYear2022", Sum = 10, CustomerId = 1 };
            Promo promo2 = new Promo { Id = 1, UsingPromoId = 2, Name = "B-day", Sum = 15, CustomerId = 2 };

            UsingPromo usingPromo1 = new UsingPromo { Id = 1, PromoId = 1, OrderId = 1};
            UsingPromo usingPromo2 = new UsingPromo { Id = 2, PromoId = 2, OrderId = 2 };

            modelBuilder.Entity<Article>().HasData(article1, article2);
            modelBuilder.Entity<ArticleInOrder>().HasData(articleInOrder1);
            modelBuilder.Entity<Category>().HasData(category1);
            modelBuilder.Entity<Customer>().HasData(customer1, customer2);
            modelBuilder.Entity<Order>().HasData(order1, order2);
            modelBuilder.Entity<Producer>().HasData(producer1);
            modelBuilder.Entity<Promo>().HasData(promo1, promo2);
            modelBuilder.Entity<UsingPromo>().HasData(usingPromo1, usingPromo2);

            modelBuilder
              .Entity<Article>()
              .HasKey(x => x.Id)
              .HasName("PK_Article");

            modelBuilder
                .Entity<Promo>()
                .HasOne(x => x.UsingPromo)
                .WithOne(x => x.Promo)
                .HasForeignKey<UsingPromo>(t => t.PromoId);

            modelBuilder
                .Entity<Order>()
                .HasOne(x => x.UsingPromo)
                .WithOne(x => x.Order)
                .HasForeignKey<UsingPromo>(t => t.PromoId);

            modelBuilder
               .Entity<Order>()
               .HasCheckConstraint("Sum", "Sum > 0 AND Sum < 99999999");

            modelBuilder
                .Entity<ArticleInOrder>()
                .ToTable("AllInvoices");

            modelBuilder
               .Entity<Article>()
               .Property(x => x.Price)
               .HasDefaultValue(0);

            modelBuilder
               .Entity<Customer>()
               .Property(x => x.Name)
               .HasColumnName("Name")
               .HasMaxLength(255);

            modelBuilder
                .Entity<ArticleInOrder>()
                .HasOne(x => x.Article)
                .WithMany(x => x.ArticleInOrders)
                .HasForeignKey("ArticleId")
                .IsRequired();

            modelBuilder
                .Entity<ArticleInOrder>()
                .HasOne(x => x.Order)
                .WithMany(x => x.ArticleInOrders)
                .HasForeignKey("ArticleId")
                .IsRequired();

            modelBuilder
                .Entity<Article>()
                .HasOne(x => x.Producer)
                .WithMany(x => x.Articles)
                .HasForeignKey("ProducerId")
                .IsRequired();

            modelBuilder
                .Entity<Article>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Articles)
                .HasForeignKey("CategoryId")
                .IsRequired();

            modelBuilder
                .Entity<Promo>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.Promos)
                .HasForeignKey("Id")
                .IsRequired();

            modelBuilder
                .Entity<Customer>()
                .HasOne(x => x.Order)
                .WithMany(x => x.Customers)
                .HasForeignKey("CustomerId")
                .IsRequired();

        }
    }

}
