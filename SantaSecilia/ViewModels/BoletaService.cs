using System;
using System.Collections.Generic;
using System.Text;

namespace SantaSecilia.ViewModels
{
    public class BoletaService{
        public BoletaSemanal GenerarBoleta(
            Guid trabajadorId,
            int totalHoras,
            decimal tarifaPorHora,
            decimal deducciones)
        {
            var pagoBruto = totalHoras * tarifaPorHora;
            var pagoNeto = pagoBruto - deducciones;

            return new BoletaSemanal
            {
                TrabajadorId = trabajadorId,
                TotalHoras = totalHoras,
                PagoBruto = pagoBruto,
                Deducciones = deducciones,
                PagoNeto = pagoNeto
            };
        }
    }
}
