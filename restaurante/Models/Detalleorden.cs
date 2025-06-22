using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace restaurante.Models;

public partial class Detalleorden
{
    public int IdDetalleOrden { get; set; }

    public int IdOrden { get; set; }
    public int? Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? SubTotal { get; set; }

    public int? IdProducto { get; set; }

    public virtual ICollection<Detallecomplemento>? Detallecomplementos { get; set; } = new List<Detallecomplemento>();

    public virtual Orden? IdOrdenNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
