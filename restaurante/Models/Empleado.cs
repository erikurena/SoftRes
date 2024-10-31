using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurante.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }
    [Required(ErrorMessage = "El campo es Obligatorio")]
    public string? Nombre { get; set; }
    [DisplayName("Apellido Paterno")]
    [Required(ErrorMessage = "El campo es Obligatorio")]
    public string? ApellidoPaterno { get; set; }
    [DisplayName("Apellido Materno")]    
    public string? ApellidoMaterno { get; set; }
    [Required(ErrorMessage = "El campo es Obligatorio")]
    public string? FechaContratacion { get; set; }
    [Required(ErrorMessage = "El campo es Obligatorio")]
    [Range(0,99999999,ErrorMessage ="El numero no es correcto")]
    public string? Telefono { get; set; }

    public int IdCargo { get; set; }
    [Required(ErrorMessage = "El campo es Obligatorio")]
    public string? Pass { get; set; }

    public string? FotoEmpleado { get; set; }
    [NotMapped]
    public IFormFile? FotoEmpleadoFile { get; set; }

    public virtual Cargo? IdCargoNavigation { get; set; } 

    public virtual ICollection<Orden>? Ordens { get; set; } = new List<Orden>();
}
