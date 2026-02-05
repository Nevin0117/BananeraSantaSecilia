using System;
using System.Collections.Generic;
using System.Text;

namespace SantaSecilia.ViewModels
{
    public class trabajadorService{
        private readonly List<Trabajador> _trabajadores;

        public trabajadorService()
        {
            _trabajadores = new List<Trabajador>();
        }

        public List<Trabajador> ObtenerTrabajadores()
        {
            return _trabajadores;
        }

        public void RegistrarTrabajador(Trabajador trabajador)
        {
            _trabajadores.Add(trabajador);
        }

        public void EditaTrabaj (Trabajador utrabajor){
            var existente = _trabajadores.FirstOrDefault(t => t.Id == utrabajor.Id);

            if (existente != null){
                existente.Nombre = utrabajor.Nombre;
                existente.Cedula = utrabajor.Cedula;
                existente.Activo = utrabajor.Activo;
            }
        }

    }
}
