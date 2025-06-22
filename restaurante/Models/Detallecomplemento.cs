using System;
using System.Collections.Generic;

namespace restaurante.Models;

public partial class Detallecomplemento
{
    public int IdDetalleProducto { get; set; }
    public int? IdDetalleOrden { get; set; }

    public int? IdComplemento { get; set; }

    public virtual Complemento? IdComplementoNavigation { get; set; }

    public virtual Detalleorden? IdDetalleOrdenNavigation { get; set; }
}
