using System;
using System.Collections.Generic;

namespace restaurante.Models;

public partial class Categoriacomplemento
{
    public int IdCategoriaComplemento { get; set; }

    public string? TipoCategoriaComplemento { get; set; }

    public virtual ICollection<Complemento>? Complementos { get; set; } = new List<Complemento>();

}
