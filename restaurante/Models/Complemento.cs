using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace restaurante.Models;

public partial class Complemento
{
    public int IdComplemento { get; set; }
    [Required(ErrorMessage = "El campo es Obligatorio")]
    public string? NombreIngrediente { get; set; }

    public int? IdCategoriaComplemento { get; set; }

    public virtual ICollection<Detallecomplemento>? Detallecomplementos { get; set; } = new List<Detallecomplemento>();

    public virtual Categoriacomplemento? IdCategoriaComplementoNavigation { get; set; } 
}
