using System;
using System.Collections.Generic;

namespace restaurante.Models;

public partial class Orden
{
    public int IdOrden { get; set; }

    public int IdCliente { get; set; }

    public int IdEmpleado { get; set; }

    public DateOnly? FechaOrden { get; set; }

    public decimal? Total { get; set; }

    public TimeOnly? TiempoOrden { get; set; }

    public decimal? Descuento { get; set; }

    public virtual ICollection<Detalleorden>? Detalleordens { get; set; } = new List<Detalleorden>();

    public virtual Cliente? IdClienteNavigation { get; set; } 

    public virtual Empleado? IdEmpleadoNavigation { get; set; }
}
