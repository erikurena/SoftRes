using System;
using System.Collections.Generic;

namespace restaurante.Models;

public partial class Categorium
{
    public int IdCategoria { get; set; }

    public string? TipoCategoria { get; set; }
    public virtual ICollection<Producto>? Productos { get; set; } = new List<Producto>();
}
