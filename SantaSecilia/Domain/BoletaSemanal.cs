using System;
using System.Collections.Generic;
using System.Text;

namespace SantaSecilia.Domain
{
    public class BoletaSemanal{
        public Guid Id { get; set; }
        public Guid TrabajadorId { get; set; }

        public DateTime SemanaInicio { get; set; }
        public DateTime SemanaFin { get; set; }

        public int TotalHoras { get; set; }
        public decimal PagoBruto { get; set; }
        public decimal Deducciones { get; set; }
        public decimal PagoNeto { get; set; }

        public DateTime FechaGeneracion { get; set; }

        public BoletaSemanal()
        {
            Id = Guid.NewGuid();
            FechaGeneracion = DateTime.Now;
        }
    }
}
