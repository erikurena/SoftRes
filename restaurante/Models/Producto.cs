using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurante.Models;

public partial class Producto
{
    public int IdProducto { get; set; }
    [Required(ErrorMessage = "El campo es Obligatorio")]
    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? IdCategoria { get; set; }
    [Required(ErrorMessage = "El campo es Obligatorio")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números.")]
   
    public decimal? Precio { get; set; }

    public string? FotoProducto { get; set; }
    [NotMapped]
    public IFormFile? FotoProductoFile { get; set; }

    public virtual ICollection<Detalleorden>? Detalleordens { get; set; } = new List<Detalleorden>();

    public virtual Categorium? IdCategoriaNavigation { get; set; } 
}
