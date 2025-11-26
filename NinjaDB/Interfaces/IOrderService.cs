using System.Collections.Generic;
using NinjaDB.Models;

namespace NinjaDB.Interfaces
{
    public interface IOrderService
    {
        Orders? GetById(int id);
        IEnumerable<Orders> GetAll();
        void Create(Orders order);
        void Update(Orders order);
        void Delete(int id);
    }
}
