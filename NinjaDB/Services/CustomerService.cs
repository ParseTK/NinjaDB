using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NinjaDB.Data;
using NinjaDB.Interfaces;
using NinjaDB.Models;

namespace NinjaDB.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly NinjaLedgerDbContext _context;

        public CustomerService(NinjaLedgerDbContext context) => _context = context;

        public Customers? GetById(int id) =>
            _context.Customers.AsNoTracking().FirstOrDefault(c => c.CustomerId == id);

        public IEnumerable<Customers> GetAll() =>
            _context.Customers.AsNoTracking().OrderBy(c => c.CustomerId).ToList();

        public void Create(Customers customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void Update(Customers customer)
        {
            var existing = _context.Customers.Find(customer.CustomerId);
            if (existing == null)
                throw new InvalidOperationException("Customer not found.");

            existing.FirstName = customer.FirstName;
            existing.LastName = customer.LastName;
            existing.Email = customer.Email;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.Customers.Find(id);
            if (existing == null) return;

            _context.Customers.Remove(existing);
            _context.SaveChanges();
        }
    }
}
