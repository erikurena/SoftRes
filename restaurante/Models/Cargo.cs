using System;
using System.Collections.Generic;

namespace restaurante.Models;

public partial class Cargo
{
    public int IdCargo { get; set; }

    public string? TipoCargo { get; set; }

    public virtual List<Empleado>? Empleados { get; set; } = new List<Empleado>();
}
