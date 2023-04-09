using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Data.DbContexts;
using eCommerceApp.Model;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Services.Classes
{
    public static class PaymentService
    {
        public static double TotalPrice { get; set; } = 0;
        public static string? Message { get; set; }
        public static void Payment(PaymentDetails paymentDetails, User user)
        {
            using (var db = new BookStoreDbContext())
            {
                ShoppingCart? cart = db.ShoppingCarts.Where(s => s.UserId == user.Id).FirstOrDefault();

                foreach (var item in db.CartItems.Where(u => u.ShoppingCartId == cart!.Id))
                {
                    TotalPrice += item.Quantity * item.Book.Price;

                }
                db.PaymentDetails.Add(paymentDetails);
                PaymentDetails details = db.PaymentDetails.Last();
                db.Orders.Add(new Order() { TotalPrice = TotalPrice, dateTime = DateTime.Now, User = user, IsDelivered = false, PaymentDetailsId = details.Id });
                Order order1 = db.Orders.Last();
                foreach (var item in db.CartItems.Where(u => u.ShoppingCartId == cart!.Id))
                {
                    db.OrderedProducts.Add(new OrderedProduct() { BookId = item.BookId, Quantity = item.Quantity, OrderId = order1.Id, Order = order1, Book = item.Book });
                    db.CartItems.Remove(item);

                }
                Message = $"<html><body>Your order has been placed\nYour order id: {order1.Id} \nTotal price:{TotalPrice}</body></html>";
                EmailSenderService.SendEmail(user.Email, Message);
                db.SaveChanges();
            }
        }
    }
}
