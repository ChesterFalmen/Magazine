using Magazine.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace Magazine.EF
{
    public class AppDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleInOrder> Article_In_Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Producer> Producers { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            var connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Article article1 = new Article { Id = 1, CategoryId = 1, ProducerId = 1, Name = "Iphone 13 128 Gb Space Gray", Price = 45000 };
            Article article2 = new Article { Id = 2, CategoryId = 1, ProducerId = 1, Name = "Iphone 14 256 Gb Purple", Price = 56000 };

            ArticleInOrder articleInOrder1 = new ArticleInOrder { Id = 1, ArticleId = 1, OrderId = 1, Price = 45000 };

            Category category1 = new Category { Id = 1, Name = "Phones", Count = 2 };

            Customer customer1 = new Customer { Id = 1, FirstName = "Vasyl", LastName = "Yurchenko" };
            Customer customer2 = new Customer { Id = 2, FirstName = "Taras", LastName = "Andryshchak" };

            Order order1 = new Order { Id = 1, CustomerId = 1, UsingPromoId = 1, Count = 1, Sum = 45000 };
            Order order2 = new Order { Id = 2, CustomerId = 2, UsingPromoId = 2, Count = 1, Sum = 56000 };

            Producer producer1 = new Producer { Id = 1, Name = "Apple" };

            Promo promo1 = new Promo { Id = 1, UsingPromoId = 1, CustomerId = 1, Name = "NewYear2022", Sum = 10 };
            Promo promo2 = new Promo { Id = 2, UsingPromoId = 2, CustomerId = 2, Name = "B-day", Sum = 15 };

            UsingPromo usingPromo1 = new UsingPromo { Id = 1, PromoId = 1, OrderId = 1 };
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

            // TPH - Table per Hierarchy
            // Маємо Одну спільну таблицю, в якій зберігаються дані з усіх нащадків
            // Колонка Discriminator визначає тип нащадка
            modelBuilder.Entity<Person>()
                .ToTable("Persons")
                .HasDiscriminator<PersonType>("PersonType")
                .HasValue<Customer>(PersonType.Customer);
            

            // TPC - Table per Concrete Type
            // Для кожного окремого типу, буде створена окрема таблиця
            modelBuilder.Entity<Person>().ToTable("Persons");
            modelBuilder.Entity<Customer>().ToTable("Customers");
            
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
                .ToTable("AllArticleInOrder");

            modelBuilder
               .Entity<Article>()
               .Property(x => x.Price)
               .HasDefaultValue(0);

            modelBuilder
               .Entity<Customer>()
               .Property(x => x.FirstName)
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
                .HasForeignKey("CustomerId")
                .IsRequired();

            modelBuilder
                .Entity<Order>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey("CustomerId")
                .IsRequired();
        }
    }

}
