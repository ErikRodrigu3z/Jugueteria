using Jugueteria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jugueteria.Service.Repositories.ToysRepository
{
    public interface IToysRepository : IRepositoryBase<Toys> 
    {
        void SeedData();


    }
}
