using Jugueteria.Models;
using Jugueteria.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jugueteria.Service.Repositories.ToysRepository
{
    public class ToysRepository : RepositoryBase<Toys> , IToysRepository
    {
        public ToysRepository(ApplicationDbContext db) : base(db)
        {

        }

        public async Task<int> AddToy(Toys toys)
        {
            try
            {
                _db.Add(toys);
                await _db.SaveChangesAsync();
                return toys.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteToy(int id)
        {
            try
            {
                var toy = await _db.Toys.FirstOrDefaultAsync(x => x.Id == id);
                if (toy != null)
                {
                    _db.Toys.Remove(toy);
                    _db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region Seed Data

        public void SeedData()
        {
            try
            {
                Toys toy1 = new Toys
                {
                    Nombre = "Pegaso",
                    Compañía = "Bandai",
                    Descripcion = "Muñeco de acciòn",
                    Precio = 1000,
                    RestriccionEdad = 12,
                    Img = ""
                };

                _db.Add(toy1);
                _db.SaveChanges();

                Toys toy2 = new Toys
                {
                    Nombre = "Barbie doctora",
                    Compañía = "Mattel",
                    Descripcion = "Muñeca para niñas",
                    Precio = 800,
                    RestriccionEdad = 7,
                    Img = ""
                };

                _db.Add(toy2);
                _db.SaveChanges();

                Toys toy3 = new Toys
                {
                    Nombre = "Raptor",
                    Compañía = "Hot Weels",
                    Descripcion = "Carrito a escala",
                    Precio = 500,
                    RestriccionEdad = 6,
                    Img = ""
                };

                _db.Add(toy3);
                _db.SaveChanges();

                Toys toy4 = new Toys
                {
                    Nombre = "Scooter",
                    Compañía = "Apache",
                    Descripcion = "Scooter infantil",
                    Precio = 900,
                    RestriccionEdad = 12,
                    Img = ""
                };

                _db.Add(toy4);
                _db.SaveChanges();


            }
            catch (Exception)
            {               
                throw;
            }
        }



        #endregion

    }
}
