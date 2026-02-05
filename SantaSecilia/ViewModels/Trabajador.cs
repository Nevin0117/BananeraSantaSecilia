using System;
using System.Collections.Generic;
using System.Text;

namespace SantaSecilia.ViewModels
{
    public class Trabajador {
        public Guid Id { get; set; }

        public required string Nombre { get; set; }

        public required string Cedula { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; }

        public Trabajador() {
            Id = Guid.NewGuid();
            Activo = true;
            FechaRegistro = DateTime.Now;
        }
    }
}