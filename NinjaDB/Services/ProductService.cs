using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NinjaDB.Data;
using NinjaDB.Interfaces;
using NinjaDB.Models;

namespace NinjaDB.Services
{
    public class ProductService : IProductService
    {
        private readonly NinjaLedgerDbContext _context;

        public ProductService(NinjaLedgerDbContext context) => _context = context;

        public Products? GetById(int id) =>
            _context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id);

        public IEnumerable<Products> GetAll() =>
            _context.Products.AsNoTracking().OrderBy(p => p.ProductId).ToList();

        public void Create(Products product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Products product)
        {
            var existing = _context.Products.Find(product.ProductId);
            if (existing == null)
                throw new InvalidOperationException("Product not found.");

            existing.ProductName = product.ProductName;
            existing.UnitPrice = product.UnitPrice;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.Products.Find(id);
            if (existing == null) return;

            _context.Products.Remove(existing);
            _context.SaveChanges();
        }
    }
}
