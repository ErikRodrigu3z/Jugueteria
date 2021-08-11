using System.ComponentModel.DataAnnotations;

namespace Jugueteria.Models
{
    public class Toys 
    {       
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int RestriccionEdad { get; set; }
        public string Compañía { get; set; }
        public decimal Precio { get; set; }
        public string Img { get; set; } 


    }
}
