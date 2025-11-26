using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NinjaDB.Data;
using NinjaDB.Interfaces;
using NinjaDB.Models;

namespace NinjaDB.Services
{
    public class OrderService : IOrderService
    {
        private readonly NinjaLedgerDbContext _context;

        public OrderService(NinjaLedgerDbContext context) => _context = context;

        public Orders? GetById(int id) =>
            _context.Orders.AsNoTracking().FirstOrDefault(o => o.OrderId == id);

        public IEnumerable<Orders> GetAll() =>
            _context.Orders.AsNoTracking().OrderBy(o => o.OrderId).ToList();

        public void Create(Orders order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Update(Orders order)
        {
            var existing = _context.Orders.Find(order.OrderId);
            if (existing == null)
                throw new InvalidOperationException("Order not found.");

            existing.Quantity = order.Quantity;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.Orders.Find(id);
            if (existing == null) return;

            _context.Orders.Remove(existing);
            _context.SaveChanges();
        }
    }
}
