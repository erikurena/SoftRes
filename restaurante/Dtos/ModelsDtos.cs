using restaurante.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurante.Dtos
{
    public class ModelsDtos
    {
    }
    public partial class ProductoDto
    {
        public int IdProducto { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int? IdCategoria { get; set; }
        public decimal? Precio { get; set; }
        public string? FotoProducto { get; set; }      
        public IFormFile? FotoProductoFile { get; set; }

        public virtual ICollection<Detalleorden>? Detalleordens { get; set; } = new List<Detalleorden>();

        public virtual CategoriumDto? IdCategoriaNavigation { get; set; }
    }

    public partial class CategoriumDto
    {
        public int IdCategoria { get; set; }
        public string? TipoCategoria { get; set; }      
    }

}
