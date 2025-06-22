using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Required = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace restaurante.Models
{
    public class Reporte
    {
        [Required(ErrorMessage ="Ingrese la Fecha de Inicio")]
        [DisplayName("Fecha de Inicio:")]
        public DateOnly? FechaInicio { get; set; }
        [Required(ErrorMessage = "Ingrese la Fecha de Fin")]
        [DisplayName("Fecha de Finalización")]
        public DateOnly? FechaFin { get; set; }

    }
}
