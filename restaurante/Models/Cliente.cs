using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace restaurante.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Telefono { get; set; }    
    public string? Cedula { get; set; }
    public int? FidePuntos { get; set; }
    public virtual ICollection<Orden>? Ordens { get; set; } = new List<Orden>();
}
