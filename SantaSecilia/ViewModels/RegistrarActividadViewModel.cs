using System;
using System.Collections.Generic;
using System.Text;

namespace SantaSecilia.ViewModels
{
    public class RegistrarActividadViewModel
    {
        public required string Actividad { get; set; }
        public required decimal Tarifa { get; set; }
        public bool Activo { get; set; }
        public List<string> Estado => new() { "Activo", "Inactivo" };
    }
}
