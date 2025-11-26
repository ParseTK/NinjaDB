using System.Collections.Generic;
using NinjaDB.Models;

namespace NinjaDB.Interfaces
{
    public interface IProductService
    {
        Products? GetById(int id);
        IEnumerable<Products> GetAll();
        void Create(Products product);
        void Update(Products product);
        void Delete(int id);
    }
}
