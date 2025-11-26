using System.Collections.Generic;
using NinjaDB.Models;

namespace NinjaDB.Interfaces
{
    public interface ICustomerService
    {
        Customers? GetById(int id);
        IEnumerable<Customers> GetAll();
        void Create(Customers customer);
        void Update(Customers customer);
        void Delete(int id);
    }
}
