using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.DbContexts
{
    public class BookStoreDbContext :DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CustomerDetails> CustomerDetails { get; set; }
        public DbSet<Format> Formats { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderedProduct> OrderedProducts { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationBuilder builder = new();

            builder.AddJsonFile("appsettings.json");

            IConfigurationRoot config = builder.Build();

            var connectionString = config.GetConnectionString("BookStore");

            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.ISBN).IsRequired();
                entity.HasIndex(e => e.ISBN).IsUnique();
                entity.Property(e => e.Author).IsRequired();
                entity.Property(e => e.PageCount).IsRequired();
                entity.Property(e => e.Weight).IsRequired();
                entity.Property(e => e.Publisher).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.ImageUrl).IsRequired();
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasOne(d => d.Language).WithMany(p => p.Books).HasForeignKey(d => d.LanguageId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Category).WithMany(p => p.Books).HasForeignKey(d => d.CategoryId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Format).WithMany(p => p.Books).HasForeignKey(d => d.FormatId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Surname).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.UserType).IsRequired();
            }

            );

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Quantity).IsRequired();
                entity.HasOne(d => d.ShoppingCart).WithMany(p => p.CartItems).HasForeignKey(d => d.ShoppingCartId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Book).WithMany(p => p.CartItems).HasForeignKey(d => d.BookId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            }


            );
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
            }

            );

            modelBuilder.Entity<CustomerDetails>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Address).IsRequired();
                entity.Property(e => e.City).IsRequired();
                entity.Property(e => e.PostalCode).IsRequired();
                entity.Property(e => e.Country).IsRequired();
                entity.Property(e => e.Phone).IsRequired();
                entity.HasOne<User>(d => d.User).WithOne(d=>d.CustomerDetails).HasForeignKey<CustomerDetails>(d => d.UserId).OnDelete(DeleteBehavior.Cascade);

            }

            );
            modelBuilder.Entity<Format>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
            }

            );
            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
            }

            );
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.TotalPrice).IsRequired();
                entity.Property(e => e.dateTime).IsRequired();
                entity.Property(e => e.IsDelivered).IsRequired();
                entity.HasOne(d => d.User).WithMany(p => p.Orders).HasForeignKey(d => d.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.PaymentDetails).WithOne(p => p.Order).HasForeignKey<Order>(d => d.PaymentDetailsId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            }

            );
            modelBuilder.Entity<OrderedProduct>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Quantity).IsRequired();
                entity.HasOne(d => d.Order).WithMany(p => p.OrderedProducts).HasForeignKey(d => d.OrderId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Book).WithMany(p => p.OrderedProducts).HasForeignKey(d => d.BookId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            }

            );
            modelBuilder.Entity<PaymentDetails>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Number).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Surname).IsRequired();
                entity.Property(e => e.Month).IsRequired();
                entity.Property(e => e.Year).IsRequired();
                entity.Property(e => e.CVV).IsRequired();
            }

            );
            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasOne(d => d.User).WithOne(p => p.ShoppingCart).HasForeignKey<ShoppingCart>(d => d.UserId).OnDelete(DeleteBehavior.Cascade);

            }

            );
            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasOne(d => d.Book).WithOne(p => p.Stock).HasForeignKey<Stock>(d => d.BookId).OnDelete(DeleteBehavior.Cascade);

            }

);
        }
    }
}


